Imports System.Drawing.Text

''' <summary>
''' Design tokens ของระบบ (สี / ฟอนต์ / ระยะ) — ปรับโทนทั้งโปรแกรมได้จากไฟล์นี้ไฟล์เดียว
''' </summary>
Public NotInheritable Class Theme

    Private Sub New()
    End Sub

    '──────────────── Brand / Surface ────────────────
    Public Shared ReadOnly Primary As Color = Color.FromArgb(13, 148, 136)          ' teal-600
    Public Shared ReadOnly PrimaryHover As Color = Color.FromArgb(15, 118, 110)     ' teal-700
    Public Shared ReadOnly PrimaryPressed As Color = Color.FromArgb(17, 94, 89)     ' teal-800
    Public Shared ReadOnly PrimarySoft As Color = Color.FromArgb(228, 246, 243)     ' teal-50
    Public Shared ReadOnly PrimaryText As Color = Color.FromArgb(17, 94, 89)

    Public Shared ReadOnly AppBackground As Color = Color.FromArgb(244, 246, 250)
    Public Shared ReadOnly Surface As Color = Color.White
    Public Shared ReadOnly SurfaceAlt As Color = Color.FromArgb(248, 250, 252)
    Public Shared ReadOnly SurfaceSunken As Color = Color.FromArgb(241, 245, 249)
    Public Shared ReadOnly Border As Color = Color.FromArgb(226, 232, 240)
    Public Shared ReadOnly BorderStrong As Color = Color.FromArgb(203, 213, 225)

    Public Shared ReadOnly TextPrimary As Color = Color.FromArgb(15, 23, 42)
    Public Shared ReadOnly TextSecondary As Color = Color.FromArgb(71, 85, 105)
    Public Shared ReadOnly TextMuted As Color = Color.FromArgb(148, 163, 184)
    Public Shared ReadOnly TextOnPrimary As Color = Color.White

    '──────────────── Sidebar ────────────────
    Public Shared ReadOnly Sidebar As Color = Color.FromArgb(15, 23, 42)            ' slate-900
    Public Shared ReadOnly SidebarHover As Color = Color.FromArgb(30, 41, 59)       ' slate-800
    Public Shared ReadOnly SidebarSelected As Color = Color.FromArgb(22, 55, 66)
    Public Shared ReadOnly SidebarText As Color = Color.FromArgb(203, 213, 225)
    Public Shared ReadOnly SidebarMuted As Color = Color.FromArgb(100, 116, 139)
    Public Shared ReadOnly SidebarAccent As Color = Color.FromArgb(45, 212, 191)    ' teal-400

    '──────────────── Status ────────────────
    Public Shared ReadOnly Success As Color = Color.FromArgb(22, 163, 74)
    Public Shared ReadOnly SuccessSoft As Color = Color.FromArgb(220, 252, 231)
    Public Shared ReadOnly SuccessText As Color = Color.FromArgb(22, 101, 52)

    Public Shared ReadOnly Warning As Color = Color.FromArgb(217, 119, 6)
    Public Shared ReadOnly WarningSoft As Color = Color.FromArgb(254, 243, 199)
    Public Shared ReadOnly WarningText As Color = Color.FromArgb(146, 64, 14)

    Public Shared ReadOnly Danger As Color = Color.FromArgb(220, 38, 38)
    Public Shared ReadOnly DangerSoft As Color = Color.FromArgb(254, 226, 226)
    Public Shared ReadOnly DangerText As Color = Color.FromArgb(153, 27, 27)

    Public Shared ReadOnly Info As Color = Color.FromArgb(37, 99, 235)
    Public Shared ReadOnly InfoSoft As Color = Color.FromArgb(219, 234, 254)
    Public Shared ReadOnly InfoText As Color = Color.FromArgb(30, 64, 175)

    Public Shared ReadOnly NeutralSoft As Color = Color.FromArgb(241, 245, 249)
    Public Shared ReadOnly NeutralText As Color = Color.FromArgb(51, 65, 85)

    '──────────────── CVD risk (Thai CV Risk score) ────────────────
    Public Shared ReadOnly RiskColors As Color() = {
        Color.FromArgb(148, 163, 184),   ' 0 = ไม่มีข้อมูล
        Color.FromArgb(34, 197, 94),     ' 1 = ต่ำ (<10%)
        Color.FromArgb(234, 179, 8),     ' 2 = ปานกลาง (10-20%)
        Color.FromArgb(249, 115, 22),    ' 3 = สูง (20-30%)
        Color.FromArgb(239, 68, 68),     ' 4 = สูงมาก (30-40%)
        Color.FromArgb(127, 29, 29)      ' 5 = สูงอันตราย (>=40%)
    }

    '──────────────── Metrics ────────────────
    Public Const CornerRadius As Integer = 12
    Public Const SmallRadius As Integer = 8

    '──────────────── Fonts ────────────────
    Private Shared _fontName As String
    Private Shared ReadOnly _fontCache As New Dictionary(Of String, Font)
    Private Shared ReadOnly _fontLock As New Object()

    ''' <summary>ฟอนต์หลัก: Leelawadee UI (ฟอนต์ไทยของ Windows 10/11 ที่เข้าชุดกับ Segoe UI)</summary>
    Public Shared ReadOnly Property FontName As String
        Get
            If _fontName Is Nothing Then
                _fontName = ResolveFontName("Leelawadee UI", "Segoe UI", "Tahoma")
            End If
            Return _fontName
        End Get
    End Property

    Public Shared Function UiFont(size As Single, Optional style As FontStyle = FontStyle.Regular) As Font
        Dim key = $"{size:0.00}|{CInt(style)}"
        SyncLock _fontLock
            Dim f As Font = Nothing
            If Not _fontCache.TryGetValue(key, f) Then
                f = New Font(FontName, size, style, GraphicsUnit.Point)
                _fontCache(key) = f
            End If
            Return f
        End SyncLock
    End Function

    Private Shared Function ResolveFontName(ParamArray candidates As String()) As String
        Try
            Using fonts As New InstalledFontCollection()
                Dim installed = New HashSet(Of String)(fonts.Families.Select(Function(ff) ff.Name), StringComparer.OrdinalIgnoreCase)
                For Each c In candidates
                    If installed.Contains(c) Then Return c
                Next
            End Using
        Catch
        End Try
        Return SystemFonts.MessageBoxFont.Name
    End Function

    '──────────────── Helpers ────────────────

    ''' <summary>แปลงค่า pixel ที่ออกแบบไว้ที่ 96 DPI ให้เหมาะกับ DPI ของจอ</summary>
    Public Shared Function Scale(ctrl As Control, value As Integer) As Integer
        Return CInt(Math.Round(value * Compat.DpiOf(ctrl) / 96.0))
    End Function

    Public Shared Function ScaleF(ctrl As Control, value As Single) As Single
        Return CSng(value * Compat.DpiOf(ctrl) / 96.0)
    End Function

    Public Shared Function Blend(baseColor As Color, overlay As Color, amount As Double) As Color
        amount = Math.Max(0, Math.Min(1, amount))
        ' แปลงเป็น Integer ก่อน (Byte - Byte ใน VB จะ overflow เมื่อผลติดลบ)
        Dim r1 = CInt(baseColor.R), g1 = CInt(baseColor.G), b1 = CInt(baseColor.B)
        Return Color.FromArgb(
            CInt(r1 + (CInt(overlay.R) - r1) * amount),
            CInt(g1 + (CInt(overlay.G) - g1) * amount),
            CInt(b1 + (CInt(overlay.B) - b1) * amount))
    End Function

    Public Shared Function WithAlpha(c As Color, alpha As Integer) As Color
        Return Color.FromArgb(Math.Max(0, Math.Min(255, alpha)), c.R, c.G, c.B)
    End Function

    Public Shared Sub GetLevelColors(level As AlertLevel, ByRef back As Color, ByRef fore As Color, ByRef accent As Color)
        Select Case level
            Case AlertLevel.Success
                back = SuccessSoft : fore = SuccessText : accent = Success
            Case AlertLevel.Warning
                back = WarningSoft : fore = WarningText : accent = Warning
            Case AlertLevel.Danger
                back = DangerSoft : fore = DangerText : accent = Danger
            Case AlertLevel.Info
                back = InfoSoft : fore = InfoText : accent = Info
            Case AlertLevel.Primary
                back = PrimarySoft : fore = PrimaryText : accent = Primary
            Case Else
                back = NeutralSoft : fore = NeutralText : accent = TextSecondary
        End Select
    End Sub

    Public Shared Function RiskColor(level As Integer) As Color
        If level < 0 OrElse level >= RiskColors.Length Then Return RiskColors(0)
        Return RiskColors(level)
    End Function

End Class

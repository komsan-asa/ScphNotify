Imports System.ComponentModel

''' <summary>การ์ดพื้นขาว มุมโค้ง มีหัวข้อ + ไอคอน (container ใส่คอนโทรลอื่นได้)</summary>
Public Class CardPanel
    Inherits Panel

    Private _title As String = ""
    Private _subtitle As String = ""
    Private _icon As IconKind = IconKind.None
    Private _level As AlertLevel = AlertLevel.Primary
    Private _radius As Integer = Theme.CornerRadius
    Private _headerRight As String = ""

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        BackColor = Theme.Surface
        ForeColor = Theme.TextPrimary
        Padding = New Padding(16)
    End Sub

    <Category("Card"), DefaultValue("")>
    Public Property Title As String
        Get
            Return _title
        End Get
        Set(value As String)
            _title = If(value, "")
            PerformLayout()
            Invalidate()
        End Set
    End Property

    <Category("Card"), DefaultValue("")>
    Public Property Subtitle As String
        Get
            Return _subtitle
        End Get
        Set(value As String)
            _subtitle = If(value, "")
            PerformLayout()
            Invalidate()
        End Set
    End Property

    ''' <summary>ข้อความเล็กมุมขวาบนของการ์ด</summary>
    <Category("Card"), DefaultValue("")>
    Public Property HeaderRightText As String
        Get
            Return _headerRight
        End Get
        Set(value As String)
            _headerRight = If(value, "")
            Invalidate()
        End Set
    End Property

    <Category("Card"), DefaultValue(GetType(IconKind), "None")>
    Public Property IconKind As IconKind
        Get
            Return _icon
        End Get
        Set(value As IconKind)
            _icon = value
            Invalidate()
        End Set
    End Property

    ''' <summary>โทนสีของไอคอนหัวการ์ด</summary>
    <Category("Card"), DefaultValue(GetType(AlertLevel), "Primary")>
    Public Property IconLevel As AlertLevel
        Get
            Return _level
        End Get
        Set(value As AlertLevel)
            _level = value
            Invalidate()
        End Set
    End Property

    <Category("Card"), DefaultValue(Theme.CornerRadius)>
    Public Property CornerRadius As Integer
        Get
            Return _radius
        End Get
        Set(value As Integer)
            _radius = Math.Max(0, value)
            Invalidate()
        End Set
    End Property

    Private ReadOnly Property HeaderHeight As Integer
        Get
            If _title = "" Then Return 0
            Return Theme.Scale(Me, If(_subtitle = "", 40, 54))
        End Get
    End Property

    Public Overrides ReadOnly Property DisplayRectangle As Rectangle
        Get
            Dim r = MyBase.DisplayRectangle
            Dim h = HeaderHeight
            Return New Rectangle(r.X, r.Y + h, r.Width, Math.Max(0, r.Height - h))
        End Get
    End Property

    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        ' วาดใน OnPaint ทั้งหมด
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        Gfx.PaintParentBackground(Me, g)
        Gfx.HighQuality(g)

        Dim radius = Theme.ScaleF(Me, _radius)
        Dim r As New RectangleF(0.5F, 0.5F, Width - 1.5F, Height - 1.5F)
        Gfx.FillRounded(g, r, radius, BackColor)
        Gfx.DrawRounded(g, r, radius, Theme.Border)

        If _title = "" Then Return

        Dim pad = Padding
        Dim x = pad.Left
        Dim top = pad.Top
        Dim lineH = Theme.Scale(Me, 28)

        If _icon <> IconKind.None Then
            Dim back As Color, fore As Color, accent As Color
            Theme.GetLevelColors(_level, back, fore, accent)
            Dim chip As New RectangleF(x, top, lineH, lineH)
            Gfx.FillRounded(g, chip, Theme.ScaleF(Me, 8), back)
            Dim inset = Theme.ScaleF(Me, 6)
            IconPainter.Draw(g, _icon, RectangleF.Inflate(chip, -inset, -inset), accent, 2.0F)
            x += lineH + Theme.Scale(Me, 10)
        End If

        Dim rightW = 0
        If _headerRight <> "" Then
            Dim f2 = Theme.UiFont(8.5F)
            rightW = Gfx.Measure(_headerRight, f2).Width + 4
            Gfx.Text(g, _headerRight, f2, New Rectangle(Width - pad.Right - rightW, top, rightW, lineH), Theme.TextMuted,
                     TextFormatFlags.Right Or TextFormatFlags.VerticalCenter)
        End If

        Dim titleRect As New Rectangle(x, top, Width - x - pad.Right - rightW - 8, lineH)
        If _subtitle = "" Then
            Gfx.Text(g, _title, Theme.UiFont(11.0F, FontStyle.Bold), titleRect, Theme.TextPrimary)
        Else
            Dim half = lineH \ 2
            Gfx.Text(g, _title, Theme.UiFont(10.5F, FontStyle.Bold), New Rectangle(titleRect.X, top - 4, titleRect.Width, half + 6), Theme.TextPrimary,
                     TextFormatFlags.Left Or TextFormatFlags.Bottom)
            Gfx.Text(g, _subtitle, Theme.UiFont(8.5F), New Rectangle(titleRect.X, top + half + 2, titleRect.Width, half + 6), Theme.TextSecondary,
                     TextFormatFlags.Left Or TextFormatFlags.Top)
        End If
    End Sub

    Protected Overrides Sub OnResize(eventargs As EventArgs)
        MyBase.OnResize(eventargs)
        Invalidate()
    End Sub

End Class

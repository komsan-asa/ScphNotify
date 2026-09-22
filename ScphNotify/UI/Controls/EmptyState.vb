Imports System.ComponentModel

''' <summary>ข้อความ "ยังไม่มีข้อมูล" กลางพื้นที่ พร้อมไอคอน</summary>
Public Class EmptyState
    Inherits Control

    Private _icon As IconKind = IconKind.Info
    Private _title As String = "ยังไม่มีข้อมูล"
    Private _description As String = ""
    Private _level As AlertLevel = AlertLevel.Primary

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.Selectable, False)
        Size = New Size(360, 220)
    End Sub

    <Category("Appearance"), DefaultValue(GetType(IconKind), "Info")>
    Public Property IconKind As IconKind
        Get
            Return _icon
        End Get
        Set(value As IconKind)
            _icon = value
            Invalidate()
        End Set
    End Property

    <Category("Appearance"), DefaultValue("ยังไม่มีข้อมูล")>
    Public Property Title As String
        Get
            Return _title
        End Get
        Set(value As String)
            _title = If(value, "")
            Invalidate()
        End Set
    End Property

    <Category("Appearance"), DefaultValue("")>
    Public Property Description As String
        Get
            Return _description
        End Get
        Set(value As String)
            _description = If(value, "")
            Invalidate()
        End Set
    End Property

    <Category("Appearance"), DefaultValue(GetType(AlertLevel), "Primary")>
    Public Property Level As AlertLevel
        Get
            Return _level
        End Get
        Set(value As AlertLevel)
            _level = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        Using b As New SolidBrush(BackColor)
            g.FillRectangle(b, ClientRectangle)
        End Using
        Gfx.HighQuality(g)

        Dim back As Color, fore As Color, accent As Color
        Theme.GetLevelColors(_level, back, fore, accent)

        Dim circle = Theme.Scale(Me, 72)
        Dim titleF = Theme.UiFont(13.0F, FontStyle.Bold)
        Dim descF = Theme.UiFont(9.75F)
        Dim maxTextW = Math.Min(Width - 40, Theme.Scale(Me, 460))
        Dim descH = If(_description = "", 0,
                       TextRenderer.MeasureText(_description, descF, New Size(maxTextW, 1000), TextFormatFlags.WordBreak Or TextFormatFlags.HorizontalCenter).Height)
        Dim totalH = circle + Theme.Scale(Me, 18) + Gfx.Measure(_title, titleF).Height + If(descH > 0, descH + Theme.Scale(Me, 6), 0)
        Dim y = Math.Max(8, (Height - totalH) \ 2)

        Dim cr As New RectangleF((Width - circle) / 2.0F, y, circle, circle)
        Using br As New SolidBrush(back)
            g.FillEllipse(br, cr)
        End Using
        Dim inset = circle * 0.28F
        IconPainter.Draw(g, _icon, RectangleF.Inflate(cr, -inset, -inset), accent, 1.8F)

        y += circle + Theme.Scale(Me, 18)
        Dim th = Gfx.Measure(_title, titleF).Height
        Gfx.Text(g, _title, titleF, New Rectangle(0, y, Width, th + 2), Theme.TextPrimary, TextFormatFlags.HorizontalCenter Or TextFormatFlags.Top)
        y += th + Theme.Scale(Me, 6)
        If descH > 0 Then
            TextRenderer.DrawText(g, _description, descF, New Rectangle((Width - maxTextW) \ 2, y, maxTextW, descH + 4), Theme.TextSecondary,
                                  TextFormatFlags.WordBreak Or TextFormatFlags.HorizontalCenter Or TextFormatFlags.NoPrefix)
        End If
    End Sub

End Class

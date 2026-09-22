Imports System.ComponentModel

''' <summary>ปุ่มเมนูด้านซ้าย (sidebar) — ไอคอน + ข้อความ + แถบสีเมื่อถูกเลือก</summary>
<DefaultEvent("Click")>
Public Class NavButton
    Inherits Control

    Private _icon As IconKind = IconKind.None
    Private _selected As Boolean
    Private _badge As String = ""
    Private _hover As Boolean
    Private _pressed As Boolean

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        Cursor = Cursors.Hand
        BackColor = Theme.Sidebar
        ForeColor = Theme.SidebarText
        Size = New Size(208, 44)
        TabStop = True
    End Sub

    <Category("Appearance"), DefaultValue(GetType(IconKind), "None")>
    Public Property IconKind As IconKind
        Get
            Return _icon
        End Get
        Set(value As IconKind)
            _icon = value
            Invalidate()
        End Set
    End Property

    <Category("Appearance"), DefaultValue(False)>
    Public Property Selected As Boolean
        Get
            Return _selected
        End Get
        Set(value As Boolean)
            _selected = value
            Invalidate()
        End Set
    End Property

    ''' <summary>ป้ายเล็กด้านขวา เช่น "เร็วๆ นี้"</summary>
    <Category("Appearance"), DefaultValue("")>
    Public Property BadgeText As String
        Get
            Return _badge
        End Get
        Set(value As String)
            _badge = If(value, "")
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        _hover = True
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        _hover = False
        _pressed = False
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = MouseButtons.Left Then _pressed = True : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        _pressed = False
        Invalidate()
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        If e.KeyCode = Keys.Enter OrElse e.KeyCode = Keys.Space Then OnClick(EventArgs.Empty)
    End Sub

    Protected Overrides Sub OnGotFocus(e As EventArgs)
        MyBase.OnGotFocus(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnLostFocus(e As EventArgs)
        MyBase.OnLostFocus(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnEnabledChanged(e As EventArgs)
        MyBase.OnEnabledChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        Gfx.HighQuality(g)
        g.Clear(If(Parent IsNot Nothing, Parent.BackColor, Theme.Sidebar))

        Dim pad = Theme.Scale(Me, 4)
        Dim r As New RectangleF(pad, pad * 0.5F, Width - pad * 2, Height - pad)
        Dim radius = Theme.ScaleF(Me, 10)

        Dim bg = Color.Empty
        If _selected Then
            bg = Theme.SidebarSelected
        ElseIf Enabled AndAlso (_hover OrElse Focused) Then
            bg = If(_pressed, Theme.Blend(Theme.SidebarHover, Color.Black, 0.15), Theme.SidebarHover)
        End If
        If bg <> Color.Empty Then Gfx.FillRounded(g, r, radius, bg)

        If _selected Then
            Dim barW = Theme.ScaleF(Me, 3.5F)
            Dim barH = r.Height * 0.52F
            Gfx.FillRounded(g, New RectangleF(r.X, r.Y + (r.Height - barH) / 2, barW, barH), barW / 2, Theme.SidebarAccent)
        End If

        Dim fg As Color
        If Not Enabled Then
            fg = Theme.SidebarMuted
        ElseIf _selected Then
            fg = Color.White
        Else
            fg = ForeColor
        End If

        Dim iconSize = Theme.ScaleF(Me, 20)
        Dim iconX = r.X + Theme.ScaleF(Me, 14)
        IconPainter.Draw(g, _icon, New RectangleF(iconX, (Height - iconSize) / 2, iconSize, iconSize),
                         If(_selected, Theme.SidebarAccent, fg), 1.8F)

        Dim textX = CInt(iconX + iconSize + Theme.Scale(Me, 12))
        Dim badgeW = 0
        If _badge <> "" Then
            Dim bf = Theme.UiFont(7.5F)
            Dim sz = Gfx.Measure(_badge, bf)
            badgeW = sz.Width + Theme.Scale(Me, 12)
            Dim bh = sz.Height + Theme.Scale(Me, 4)
            Dim br As New RectangleF(r.Right - badgeW - Theme.Scale(Me, 10), (Height - bh) / 2.0F, badgeW, bh)
            Gfx.FillRounded(g, br, bh / 2.0F, Theme.WithAlpha(Theme.SidebarAccent, 38))
            Gfx.Text(g, _badge, bf, Rectangle.Round(br), Theme.SidebarAccent, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
        End If

        Dim textRect As New Rectangle(textX, 0, CInt(r.Right) - textX - badgeW - Theme.Scale(Me, 14), Height)
        Dim f = If(_selected, Theme.UiFont(Font.SizeInPoints, FontStyle.Bold), Font)
        Gfx.Text(g, Text, f, textRect, fg)
    End Sub

End Class

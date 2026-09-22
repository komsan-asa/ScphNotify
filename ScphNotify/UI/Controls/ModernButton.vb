Imports System.ComponentModel

Public Enum ButtonVariant
    Primary
    Secondary
    Ghost
    Danger
    Subtle
    SidebarGhost
End Enum

''' <summary>ปุ่มมุมโค้งแบบ flat (สืบทอด Button — ใช้กับ AcceptButton / CancelButton / คีย์บอร์ดได้ตามปกติ)</summary>
Public Class ModernButton
    Inherits Button

    Private _variant As ButtonVariant = ButtonVariant.Primary
    Private _icon As IconKind = IconKind.None
    Private _circular As Boolean
    Private _hover As Boolean
    Private _pressed As Boolean

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        FlatStyle = FlatStyle.Flat
        FlatAppearance.BorderSize = 0
        Cursor = Cursors.Hand
        Size = New Size(120, 38)
        AutoSizeMode = AutoSizeMode.GrowAndShrink
        UseVisualStyleBackColor = False
    End Sub

    ''' <summary>
    ''' ขนาดที่พอดีกับไอคอน + ข้อความ (ให้ปุ่มที่ตั้ง AutoSize = True กว้างตามข้อความทุกระดับ DPI
    ''' — แก้ปัญหาข้อความไทยยาวถูกตัดเป็น "บัน...")
    ''' </summary>
    Public Overrides Function GetPreferredSize(proposedSize As Size) As Size
        If _circular Then Return Size
        Dim hasText = Not String.IsNullOrEmpty(Text)
        Dim iconW = If(_icon <> IconKind.None, Theme.Scale(Me, 18), 0)
        Dim gap = If(_icon <> IconKind.None AndAlso hasText, Theme.Scale(Me, 8), 0)
        Dim textSize = If(hasText, Gfx.Measure(Text, Font), Size.Empty)
        Dim padX = Theme.Scale(Me, If(hasText, 20, 12))
        Dim w = iconW + gap + textSize.Width + padX * 2
        Dim h = Math.Max(Theme.Scale(Me, 38), textSize.Height + Theme.Scale(Me, 16))
        Return New Size(Math.Max(w, MinimumSize.Width), Math.Max(h, MinimumSize.Height))
    End Function

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        If AutoSize Then PerformLayout()
        Invalidate()
    End Sub

    Protected Overrides Sub OnFontChanged(e As EventArgs)
        MyBase.OnFontChanged(e)
        If AutoSize Then PerformLayout()
        Invalidate()
    End Sub

    <Category("Appearance"), DefaultValue(GetType(ButtonVariant), "Primary")>
    Public Property Kind As ButtonVariant
        Get
            Return _variant
        End Get
        Set(value As ButtonVariant)
            _variant = value
            Invalidate()
        End Set
    End Property

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

    ''' <summary>ปุ่มวงกลม (ใช้กับปุ่มไอคอนอย่างเดียว)</summary>
    <Category("Appearance"), DefaultValue(False)>
    Public Property Circular As Boolean
        Get
            Return _circular
        End Get
        Set(value As Boolean)
            _circular = value
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

    Protected Overrides Sub OnMouseDown(mevent As MouseEventArgs)
        MyBase.OnMouseDown(mevent)
        If mevent.Button = MouseButtons.Left Then _pressed = True : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(mevent As MouseEventArgs)
        MyBase.OnMouseUp(mevent)
        _pressed = False
        Invalidate()
    End Sub

    Protected Overrides Sub OnEnabledChanged(e As EventArgs)
        MyBase.OnEnabledChanged(e)
        Invalidate()
    End Sub

    Private Sub GetColors(ByRef back As Color, ByRef fore As Color, ByRef border As Color)
        border = Color.Empty
        Select Case _variant
            Case ButtonVariant.Primary
                back = If(_pressed, Theme.PrimaryPressed, If(_hover, Theme.PrimaryHover, Theme.Primary))
                fore = Color.White
            Case ButtonVariant.Danger
                back = If(_pressed, Theme.Blend(Theme.Danger, Color.Black, 0.25), If(_hover, Theme.Blend(Theme.Danger, Color.Black, 0.12), Theme.Danger))
                fore = Color.White
            Case ButtonVariant.Secondary
                back = If(_pressed, Theme.SurfaceSunken, If(_hover, Theme.SurfaceAlt, Theme.Surface))
                fore = Theme.TextPrimary
                border = If(_hover, Theme.BorderStrong, Theme.Border)
            Case ButtonVariant.Subtle
                back = If(_pressed, Theme.Blend(Theme.PrimarySoft, Theme.Primary, 0.18), If(_hover, Theme.Blend(Theme.PrimarySoft, Theme.Primary, 0.08), Theme.PrimarySoft))
                fore = Theme.PrimaryText
            Case ButtonVariant.SidebarGhost
                back = If(_pressed, Theme.Blend(Theme.SidebarHover, Color.Black, 0.2), If(_hover, Theme.SidebarHover, Color.Empty))
                fore = Theme.SidebarText
            Case Else ' Ghost
                back = If(_pressed, Theme.Border, If(_hover, Theme.SurfaceSunken, Color.Empty))
                fore = Theme.TextSecondary
        End Select
        If Not Enabled Then
            If back <> Color.Empty Then back = Theme.Blend(back, Theme.SurfaceSunken, 0.6)
            fore = Theme.TextMuted
        End If
    End Sub

    Protected Overrides Sub OnPaint(pevent As PaintEventArgs)
        Dim g = pevent.Graphics
        Gfx.PaintParentBackground(Me, g)
        Gfx.HighQuality(g)

        Dim back As Color, fore As Color, border As Color
        GetColors(back, fore, border)

        Dim r As New RectangleF(0.5F, 0.5F, Width - 1.5F, Height - 1.5F)
        Dim radius = If(_circular, Math.Min(r.Width, r.Height) / 2, Theme.ScaleF(Me, 8))
        If back <> Color.Empty Then Gfx.FillRounded(g, r, radius, back)
        If border <> Color.Empty Then Gfx.DrawRounded(g, r, radius, border)

        If Focused AndAlso ShowFocusCues Then
            Dim fr = RectangleF.Inflate(r, -2, -2)
            Gfx.DrawRounded(g, fr, Math.Max(1, radius - 2), Theme.WithAlpha(If(_variant = ButtonVariant.Primary, Color.White, Theme.Primary), 140), 1.2F)
        End If

        Dim hasText = Not String.IsNullOrEmpty(Text)
        Dim iconSize = If(hasText, Theme.ScaleF(Me, 18), Math.Min(Width, Height) * 0.5F)
        Dim textSize = If(hasText, Gfx.Measure(Text, Font), Size.Empty)
        Dim gap = If(_icon <> IconKind.None AndAlso hasText, Theme.ScaleF(Me, 8), 0)
        Dim contentW = If(_icon <> IconKind.None, iconSize, 0) + gap + textSize.Width
        ' ถ้าเนื้อหากว้างกว่าปุ่ม ให้ชิดซ้ายแทนการล้นออกทั้งสองข้าง
        Dim x = Math.Max(Theme.ScaleF(Me, 6), (Width - contentW) / 2.0F)

        If _icon <> IconKind.None Then
            IconPainter.Draw(g, _icon, New RectangleF(x, (Height - iconSize) / 2.0F, iconSize, iconSize), fore, 2.0F)
            x += iconSize + gap
        End If
        If hasText Then
            ' วาดด้วย NoPadding เหมือนตอนวัดขนาด (ไม่งั้น DrawText จะกินที่มากกว่าที่วัดไว้ แล้วตัดเป็น "...")
            Dim tx = CInt(Math.Floor(x))
            Dim tw = Math.Max(textSize.Width + Theme.Scale(Me, 4), Width - tx)
            Gfx.Text(g, Text, Font, New Rectangle(tx, 0, tw, Height), fore,
                     TextFormatFlags.Left Or TextFormatFlags.VerticalCenter Or TextFormatFlags.NoPadding)
        End If
    End Sub

End Class

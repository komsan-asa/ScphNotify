Imports System.ComponentModel

''' <summary>กล่องข้อความแจ้งเตือน/ผลลัพธ์ (สีตามระดับ) ข้อความตัดบรรทัดอัตโนมัติ</summary>
Public Class InfoBanner
    Inherits Control

    Private _level As AlertLevel = AlertLevel.Info
    Private _icon As IconKind = IconKind.Info

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.Selectable, False)
        Size = New Size(400, 56)
    End Sub

    <Category("Appearance"), DefaultValue(GetType(AlertLevel), "Info")>
    Public Property Level As AlertLevel
        Get
            Return _level
        End Get
        Set(value As AlertLevel)
            _level = value
            Invalidate()
        End Set
    End Property

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

    Public Sub ShowMessage(text As String, level As AlertLevel, Optional icon As IconKind = IconKind.None)
        _level = level
        _icon = If(icon = IconKind.None, DefaultIconFor(level), icon)
        Me.Text = text
        Visible = True
        Invalidate()
    End Sub

    Private Shared Function DefaultIconFor(level As AlertLevel) As IconKind
        Select Case level
            Case AlertLevel.Success : Return IconKind.Check
            Case AlertLevel.Warning, AlertLevel.Danger : Return IconKind.Warning
            Case Else : Return IconKind.Info
        End Select
    End Function

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        Gfx.PaintParentBackground(Me, g)
        Gfx.HighQuality(g)
        Dim back As Color, fore As Color, accent As Color
        Theme.GetLevelColors(_level, back, fore, accent)
        Dim r As New RectangleF(0.5F, 0.5F, Width - 1.5F, Height - 1.5F)
        Dim radius = Theme.ScaleF(Me, 10)
        Gfx.FillRounded(g, r, radius, back)
        Gfx.DrawRounded(g, r, radius, Theme.WithAlpha(accent, 60))

        Dim s = Theme.ScaleF(Me, 20)
        Dim pad = Theme.Scale(Me, 14)
        IconPainter.Draw(g, _icon, New RectangleF(pad, (Height - s) / 2, s, s), accent, 2.0F)
        Dim tx = CInt(pad + s + Theme.Scale(Me, 10))
        TextRenderer.DrawText(g, Text, Font, New Rectangle(tx, 4, Width - tx - pad, Height - 8), fore,
                              TextFormatFlags.WordBreak Or TextFormatFlags.VerticalCenter Or TextFormatFlags.Left Or TextFormatFlags.NoPrefix)
    End Sub

End Class

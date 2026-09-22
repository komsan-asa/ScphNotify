Imports System.ComponentModel

''' <summary>ไอคอนในกรอบมุมโค้งสีอ่อน (ใช้ที่หัวหน้าต่าง dialog)</summary>
Public Class IconTile
    Inherits Control

    Private _icon As IconKind = IconKind.Info
    Private _level As AlertLevel = AlertLevel.Primary

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.Selectable, False)
        Size = New Size(48, 48)
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
        Gfx.PaintParentBackground(Me, g)
        Gfx.HighQuality(g)
        Dim back As Color, fore As Color, accent As Color
        Theme.GetLevelColors(_level, back, fore, accent)
        Dim s = Math.Min(Width, Height) - 1
        Dim r As New RectangleF((Width - s) / 2.0F, (Height - s) / 2.0F, s, s)
        Gfx.FillRounded(g, r, s * 0.28F, back)
        Dim inset = s * 0.24F
        IconPainter.Draw(g, _icon, RectangleF.Inflate(r, -inset, -inset), accent, 2.0F)
    End Sub

End Class

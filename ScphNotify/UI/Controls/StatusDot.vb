Imports System.ComponentModel

''' <summary>จุดสถานะการเชื่อมต่อ (มีวงแหวนจาง ๆ รอบจุด)</summary>
Public Class StatusDot
    Inherits Control

    Private _level As AlertLevel = AlertLevel.Neutral

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.Selectable, False)
        Size = New Size(14, 14)
    End Sub

    <DefaultValue(GetType(AlertLevel), "Neutral")>
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
        Dim s = Math.Min(Width, Height) - 1.0F
        Using halo As New SolidBrush(Theme.WithAlpha(accent, 55))
            g.FillEllipse(halo, (Width - s) / 2, (Height - s) / 2, s, s)
        End Using
        Dim inner = s * 0.56F
        Using dot As New SolidBrush(accent)
            g.FillEllipse(dot, (Width - inner) / 2, (Height - inner) / 2, inner, inner)
        End Using
    End Sub

End Class

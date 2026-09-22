Imports System.ComponentModel
Imports System.Drawing.Drawing2D

''' <summary>รูปวงกลมแทนผู้ป่วย (ไอคอนคนบนพื้นไล่สี)</summary>
Public Class AvatarCircle
    Inherits Control

    Private _active As Boolean = True

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.Selectable, False)
        Size = New Size(48, 48)
    End Sub

    ''' <summary>False = ไม่มีผู้ป่วย (สีเทา)</summary>
    <DefaultValue(True)>
    Public Property Active As Boolean
        Get
            Return _active
        End Get
        Set(value As Boolean)
            _active = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        Gfx.PaintParentBackground(Me, g)
        Gfx.HighQuality(g)
        Dim s = Math.Min(Width, Height) - 1
        Dim r As New RectangleF((Width - s) / 2.0F, (Height - s) / 2.0F, s, s)
        Dim c1 = If(_active, Color.FromArgb(20, 184, 166), Theme.BorderStrong)
        Dim c2 = If(_active, Color.FromArgb(13, 116, 144), Theme.TextMuted)
        Using br As New LinearGradientBrush(r, c1, c2, 45.0F)
            g.FillEllipse(br, r)
        End Using
        Dim inset = s * 0.26F
        IconPainter.Draw(g, IconKind.User, RectangleF.Inflate(r, -inset, -inset), Color.White, 2.0F)
    End Sub

End Class

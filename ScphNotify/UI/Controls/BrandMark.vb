Imports System.Drawing.Drawing2D

''' <summary>โลโก้ของโปรแกรม (กระดิ่งในกรอบมุมโค้งไล่สี)</summary>
Public Class BrandMark
    Inherits Control

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.Selectable, False)
        Size = New Size(40, 40)
    End Sub

    Public Shared Sub DrawMark(g As Graphics, r As RectangleF)
        Gfx.HighQuality(g)
        Using path = Gfx.RoundedRect(r, r.Width * 0.28F),
              br As New LinearGradientBrush(r, Color.FromArgb(45, 212, 191), Color.FromArgb(13, 116, 144), 45.0F)
            g.FillPath(br, path)
        End Using
        Dim inset = r.Width * 0.22F
        IconPainter.Draw(g, IconKind.Bell, RectangleF.Inflate(r, -inset, -inset), Color.White, 2.2F)
        Dim dot = r.Width * 0.2F
        Using b As New SolidBrush(Color.FromArgb(251, 113, 133)), p As New Pen(Color.White, Math.Max(1.5F, r.Width / 22))
            Dim dr As New RectangleF(r.Right - dot * 1.55F, r.Y + dot * 0.55F, dot, dot)
            g.FillEllipse(b, dr)
            g.DrawEllipse(p, dr)
        End Using
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Gfx.PaintParentBackground(Me, e.Graphics)
        Dim s = Math.Min(Width, Height) - 1
        DrawMark(e.Graphics, New RectangleF((Width - s) / 2.0F, (Height - s) / 2.0F, s, s))
    End Sub

End Class

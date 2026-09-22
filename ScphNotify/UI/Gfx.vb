Imports System.Drawing.Drawing2D
Imports System.Drawing.Text

''' <summary>ตัวช่วยวาดรูปทรงแบบมุมโค้ง / ข้อความ สำหรับคอนโทรลที่วาดเอง</summary>
Public NotInheritable Class Gfx

    Private Sub New()
    End Sub

    Public Shared Function RoundedRect(r As RectangleF, radius As Single) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim d = Math.Min(radius * 2.0F, Math.Min(r.Width, r.Height))
        If d <= 0.5F Then
            path.AddRectangle(r)
            Return path
        End If
        path.AddArc(r.X, r.Y, d, d, 180, 90)
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90)
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90)
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90)
        path.CloseFigure()
        Return path
    End Function

    Public Shared Function RoundedRect(r As Rectangle, radius As Single) As GraphicsPath
        Return RoundedRect(New RectangleF(r.X, r.Y, r.Width, r.Height), radius)
    End Function

    Public Shared Sub FillRounded(g As Graphics, r As RectangleF, radius As Single, c As Color)
        Using p = RoundedRect(r, radius), b As New SolidBrush(c)
            g.FillPath(b, p)
        End Using
    End Sub

    Public Shared Sub DrawRounded(g As Graphics, r As RectangleF, radius As Single, c As Color, Optional width As Single = 1.0F)
        Using p = RoundedRect(r, radius), pen As New Pen(c, width)
            g.DrawPath(pen, p)
        End Using
    End Sub

    ''' <summary>เงาจาง ๆ ใต้การ์ด (วาดหลายชั้นโปร่งใส)</summary>
    Public Shared Sub DrawSoftShadow(g As Graphics, r As RectangleF, radius As Single, depth As Integer, Optional baseAlpha As Integer = 14)
        For i = depth To 1 Step -1
            Dim rr As New RectangleF(r.X - i * 0.5F, r.Y + i * 0.6F, r.Width + i, r.Height + i * 0.6F)
            Using p = RoundedRect(rr, radius + i), b As New SolidBrush(Color.FromArgb(Math.Max(1, baseAlpha - i * 2), 15, 23, 42))
                g.FillPath(b, p)
            End Using
        Next
    End Sub

    Public Shared Sub HighQuality(g As Graphics)
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.PixelOffsetMode = PixelOffsetMode.HighQuality
        g.InterpolationMode = InterpolationMode.HighQualityBicubic
        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit
    End Sub

    ''' <summary>วาดข้อความด้วย TextRenderer (คมชัดแบบ GDI) ภายในกรอบ</summary>
    Public Shared Sub Text(g As Graphics, s As String, f As Font, r As Rectangle, c As Color,
                           Optional flags As TextFormatFlags = TextFormatFlags.Left Or TextFormatFlags.VerticalCenter)
        If String.IsNullOrEmpty(s) Then Return
        TextRenderer.DrawText(g, s, f, r, c, flags Or TextFormatFlags.NoPrefix Or TextFormatFlags.EndEllipsis)
    End Sub

    Public Shared Function Measure(s As String, f As Font) As Size
        If String.IsNullOrEmpty(s) Then Return Size.Empty
        Return TextRenderer.MeasureText(s, f, New Size(Integer.MaxValue, Integer.MaxValue), TextFormatFlags.NoPadding Or TextFormatFlags.NoPrefix)
    End Function

    ''' <summary>เติมพื้นหลังด้วยสีของ parent (ใช้ตรงมุมโค้งของคอนโทรล)</summary>
    Public Shared Sub PaintParentBackground(ctrl As Control, g As Graphics)
        Dim c = If(ctrl.Parent IsNot Nothing, ctrl.Parent.BackColor, Theme.AppBackground)
        If c.A < 255 OrElse c = Color.Transparent Then c = Theme.AppBackground
        g.Clear(c)
    End Sub

End Class

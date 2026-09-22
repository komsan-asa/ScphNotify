Imports System.Drawing.Drawing2D

''' <summary>ไอคอนที่ใช้ในโปรแกรม (วาดแบบเวกเตอร์ คมชัดทุก DPI ไม่ต้องใช้ไฟล์รูป)</summary>
Public Enum IconKind
    None = 0
    ClipboardPlus
    ClipboardCheck
    HeartPulse
    Flask
    Table
    Pill
    Gear
    Refresh
    Database
    Bell
    User
    Close
    Minimize
    Search
    Printer
    LineChart
    Check
    Warning
    Calendar
    Info
    External
    MoreVertical
    ChevronRight
    Plus
    ArrowRight
    Clock
    Activity
    Droplet
    Gauge
    Filter
    Power
    Eye
    EyeOff
    Trash
    Briefcase
    Server
    Lock
    Download
    Kidney
End Enum

Public NotInheritable Class IconPainter

    Private Sub New()
    End Sub

    ''' <summary>วาดไอคอนลงในกรอบ (ออกแบบบน grid 24x24 แบบ stroke)</summary>
    Public Shared Sub Draw(g As Graphics, kind As IconKind, bounds As RectangleF, color As Color, Optional strokeWidth As Single = 2.0F)
        If kind = IconKind.None OrElse bounds.Width <= 0 OrElse bounds.Height <= 0 Then Return
        Dim state = g.Save()
        Try
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.PixelOffsetMode = PixelOffsetMode.HighQuality
            Dim s = Math.Min(bounds.Width, bounds.Height) / 24.0F
            g.TranslateTransform(bounds.X + (bounds.Width - 24 * s) / 2, bounds.Y + (bounds.Height - 24 * s) / 2)
            g.ScaleTransform(s, s)
            Using pen As New Pen(color, strokeWidth), brush As New SolidBrush(color)
                pen.StartCap = LineCap.Round
                pen.EndCap = LineCap.Round
                pen.LineJoin = LineJoin.Round
                DrawCore(g, kind, pen, brush)
            End Using
        Finally
            g.Restore(state)
        End Try
    End Sub

    Public Shared Function ToBitmap(kind As IconKind, size As Integer, color As Color, Optional strokeWidth As Single = 2.0F) As Bitmap
        Dim bmp As New Bitmap(size, size)
        Using g = Graphics.FromImage(bmp)
            g.Clear(Color.Transparent)
            Draw(g, kind, New RectangleF(0, 0, size, size), color, strokeWidth)
        End Using
        Return bmp
    End Function

    Private Shared Sub DrawCore(g As Graphics, kind As IconKind, p As Pen, b As Brush)
        Select Case kind
            Case IconKind.ClipboardPlus, IconKind.ClipboardCheck
                Using path = Gfx.RoundedRect(New RectangleF(5, 4, 14, 18), 2)
                    g.DrawPath(p, path)
                End Using
                Using path = Gfx.RoundedRect(New RectangleF(8.5F, 2, 7, 4), 1)
                    g.DrawPath(p, path)
                End Using
                If kind = IconKind.ClipboardPlus Then
                    g.DrawLine(p, 12, 10, 12, 17)
                    g.DrawLine(p, 8.5F, 13.5F, 15.5F, 13.5F)
                Else
                    g.DrawLines(p, {New PointF(8.5F, 13.5F), New PointF(11, 16), New PointF(15.5F, 11)})
                End If

            Case IconKind.HeartPulse
                Using path As New GraphicsPath()
                    path.AddBezier(12, 20.5F, 7, 16.5F, 2.5F, 13, 2.5F, 8.7F)
                    path.AddBezier(2.5F, 8.7F, 2.5F, 5.6F, 4.9F, 3.5F, 7.5F, 3.5F)
                    path.AddBezier(7.5F, 3.5F, 9.4F, 3.5F, 10.9F, 4.5F, 12, 6.2F)
                    path.AddBezier(12, 6.2F, 13.1F, 4.5F, 14.6F, 3.5F, 16.5F, 3.5F)
                    path.AddBezier(16.5F, 3.5F, 19.1F, 3.5F, 21.5F, 5.6F, 21.5F, 8.7F)
                    path.AddBezier(21.5F, 8.7F, 21.5F, 13, 17, 16.5F, 12, 20.5F)
                    path.CloseFigure()
                    g.DrawPath(p, path)
                End Using
                g.DrawLines(p, {New PointF(5, 12), New PointF(9, 12), New PointF(10.5F, 9.5F), New PointF(12.5F, 14.5F), New PointF(14, 12), New PointF(19, 12)})

            Case IconKind.Flask
                g.DrawLines(p, {New PointF(10, 2.5F), New PointF(10, 9.5F), New PointF(4.8F, 19.6F), New PointF(5.6F, 21.5F),
                                New PointF(18.4F, 21.5F), New PointF(19.2F, 19.6F), New PointF(14, 9.5F), New PointF(14, 2.5F)})
                g.DrawLine(p, 8.5F, 2.5F, 15.5F, 2.5F)
                g.DrawLine(p, 7.2F, 15.5F, 16.8F, 15.5F)

            Case IconKind.Table
                Using path = Gfx.RoundedRect(New RectangleF(3, 3, 18, 18), 2.5F)
                    g.DrawPath(p, path)
                End Using
                g.DrawLine(p, 3, 9, 21, 9)
                g.DrawLine(p, 3, 15, 21, 15)
                g.DrawLine(p, 9, 3, 9, 21)

            Case IconKind.Pill
                Dim st = g.Save()
                g.TranslateTransform(12, 12)
                g.RotateTransform(-45)
                Using path = Gfx.RoundedRect(New RectangleF(-9.5F, -4.6F, 19, 9.2F), 4.6F)
                    g.DrawPath(p, path)
                End Using
                g.DrawLine(p, 0, -4.6F, 0, 4.6F)
                g.Restore(st)

            Case IconKind.Gear
                Dim pts As New List(Of PointF)
                For k = 0 To 7
                    Dim a = k * 45.0
                    For Each item In {(-15.5, 7.2), (-8.5, 10.0), (8.5, 10.0), (15.5, 7.2)}
                        Dim ang = (a + item.Item1) * Math.PI / 180
                        pts.Add(New PointF(CSng(12 + item.Item2 * Math.Cos(ang)), CSng(12 + item.Item2 * Math.Sin(ang))))
                    Next
                Next
                g.DrawPolygon(p, pts.ToArray())
                g.DrawEllipse(p, 9, 9, 6, 6)

            Case IconKind.Refresh
                g.DrawArc(p, 4, 4, 16, 16, -50, 300)
                g.DrawLines(p, {New PointF(20.5F, 3.5F), New PointF(20.5F, 8.5F), New PointF(15.5F, 8.5F)})

            Case IconKind.Database
                g.DrawEllipse(p, 4, 2.5F, 16, 6)
                g.DrawLine(p, 4, 5.5F, 4, 18.5F)
                g.DrawLine(p, 20, 5.5F, 20, 18.5F)
                g.DrawArc(p, 4, 15.5F, 16, 6, 0, 180)
                g.DrawArc(p, 4, 9, 16, 6, 0, 180)

            Case IconKind.Lock
                Using path = Gfx.RoundedRect(New RectangleF(4, 10.5F, 16, 11), 2.5F)
                    g.DrawPath(p, path)
                End Using
                Using path As New GraphicsPath()
                    path.AddArc(7.5F, 4.0F, 9, 9, 180, 180)
                    g.DrawPath(p, path)
                End Using
                g.DrawLine(p, 7.5F, 8.5F, 7.5F, 10.5F)
                g.DrawLine(p, 16.5F, 8.5F, 16.5F, 10.5F)
                g.FillEllipse(b, 10.9F, 14.4F, 2.2F, 2.2F)

            Case IconKind.Download
                g.DrawLine(p, 12, 3, 12, 14.5F)
                g.DrawLines(p, New PointF() {New PointF(7.5F, 10.5F), New PointF(12, 15), New PointF(16.5F, 10.5F)})
                g.DrawLines(p, New PointF() {New PointF(4, 17), New PointF(4, 20.5F), New PointF(20, 20.5F), New PointF(20, 17)})

            Case IconKind.Server
                Using path = Gfx.RoundedRect(New RectangleF(3, 3, 18, 7.5F), 2)
                    g.DrawPath(p, path)
                End Using
                Using path = Gfx.RoundedRect(New RectangleF(3, 13.5F, 18, 7.5F), 2)
                    g.DrawPath(p, path)
                End Using
                g.FillEllipse(b, 6, 5.8F, 2, 2)
                g.FillEllipse(b, 6, 16.3F, 2, 2)

            Case IconKind.Bell
                Using path As New GraphicsPath()
                    path.AddArc(6, 2.5F, 12, 11, 180, 180)
                    path.AddBezier(18, 8, 18, 14.5F, 20.5F, 16.5F, 20.5F, 16.5F)
                    path.AddLine(20.5F, 16.5F, 3.5F, 16.5F)
                    path.AddBezier(3.5F, 16.5F, 3.5F, 16.5F, 6, 14.5F, 6, 8)
                    path.CloseFigure()
                    g.DrawPath(p, path)
                End Using
                g.DrawArc(p, 9.5F, 17.5F, 5, 4, 20, 140)

            Case IconKind.User
                g.DrawEllipse(p, 8, 3, 8, 8)
                Using path As New GraphicsPath()
                    path.AddLine(5, 21, 5, 19)
                    path.AddArc(5, 14, 8, 8, 180, 90)
                    path.AddLine(9, 14, 15, 14)
                    path.AddArc(11, 14, 8, 8, 270, 90)
                    path.AddLine(19, 19, 19, 21)
                    g.DrawPath(p, path)
                End Using

            Case IconKind.Close
                g.DrawLine(p, 6, 6, 18, 18)
                g.DrawLine(p, 18, 6, 6, 18)

            Case IconKind.Minimize
                g.DrawLine(p, 5, 12, 19, 12)

            Case IconKind.Search
                g.DrawEllipse(p, 4, 4, 13, 13)
                g.DrawLine(p, 15.5F, 15.5F, 20.5F, 20.5F)

            Case IconKind.Printer
                g.DrawLines(p, {New PointF(6.5F, 9), New PointF(6.5F, 3), New PointF(17.5F, 3), New PointF(17.5F, 9)})
                Using path As New GraphicsPath()
                    path.AddLine(6.5F, 17.5F, 4, 17.5F)
                    path.AddArc(2, 13.5F, 4, 4, 90, 90)
                    path.AddArc(2, 9, 4, 4, 180, 90)
                    path.AddArc(18, 9, 4, 4, 270, 90)
                    path.AddArc(18, 13.5F, 4, 4, 0, 90)
                    path.AddLine(20, 17.5F, 17.5F, 17.5F)
                    g.DrawPath(p, path)
                End Using
                g.DrawRectangle(p, 6.5F, 14, 11, 7.5F)

            Case IconKind.LineChart
                g.DrawLines(p, {New PointF(3, 3), New PointF(3, 21), New PointF(21, 21)})
                g.DrawLines(p, {New PointF(7, 16), New PointF(11, 11), New PointF(14, 14), New PointF(20, 7)})

            Case IconKind.Check
                g.DrawLines(p, {New PointF(5, 12.5F), New PointF(10, 17.5F), New PointF(19, 7)})

            Case IconKind.Warning
                Using path As New GraphicsPath()
                    path.AddLines({New PointF(12, 3.5F), New PointF(21.5F, 20), New PointF(2.5F, 20)})
                    path.CloseFigure()
                    g.DrawPath(p, path)
                End Using
                g.DrawLine(p, 12, 9.5F, 12, 13.5F)
                g.FillEllipse(b, 10.9F, 15.8F, 2.2F, 2.2F)

            Case IconKind.Calendar
                Using path = Gfx.RoundedRect(New RectangleF(3, 4.5F, 18, 17), 2.5F)
                    g.DrawPath(p, path)
                End Using
                g.DrawLine(p, 8, 2.5F, 8, 6.5F)
                g.DrawLine(p, 16, 2.5F, 16, 6.5F)
                g.DrawLine(p, 3, 10, 21, 10)

            Case IconKind.Info
                g.DrawEllipse(p, 2.5F, 2.5F, 19, 19)
                g.DrawLine(p, 12, 11, 12, 16.5F)
                g.FillEllipse(b, 10.9F, 6.6F, 2.2F, 2.2F)

            Case IconKind.External
                g.DrawLines(p, {New PointF(18, 13.5F), New PointF(18, 20.5F), New PointF(3.5F, 20.5F), New PointF(3.5F, 6), New PointF(10.5F, 6)})
                g.DrawLines(p, {New PointF(15, 3.5F), New PointF(20.5F, 3.5F), New PointF(20.5F, 9)})
                g.DrawLine(p, 10.5F, 13.5F, 20.5F, 3.5F)

            Case IconKind.MoreVertical
                g.FillEllipse(b, 10.3F, 3.3F, 3.4F, 3.4F)
                g.FillEllipse(b, 10.3F, 10.3F, 3.4F, 3.4F)
                g.FillEllipse(b, 10.3F, 17.3F, 3.4F, 3.4F)

            Case IconKind.ChevronRight
                g.DrawLines(p, {New PointF(9, 6), New PointF(15, 12), New PointF(9, 18)})

            Case IconKind.Plus
                g.DrawLine(p, 12, 5, 12, 19)
                g.DrawLine(p, 5, 12, 19, 12)

            Case IconKind.ArrowRight
                g.DrawLine(p, 5, 12, 19, 12)
                g.DrawLines(p, {New PointF(12.5F, 5.5F), New PointF(19, 12), New PointF(12.5F, 18.5F)})

            Case IconKind.Clock
                g.DrawEllipse(p, 2.5F, 2.5F, 19, 19)
                g.DrawLines(p, {New PointF(12, 6.5F), New PointF(12, 12), New PointF(15.5F, 14)})

            Case IconKind.Activity
                g.DrawLines(p, {New PointF(21.5F, 12), New PointF(17.5F, 12), New PointF(14.5F, 20.5F), New PointF(9.5F, 3.5F), New PointF(6.5F, 12), New PointF(2.5F, 12)})

            Case IconKind.Droplet
                Using path As New GraphicsPath()
                    path.AddBezier(12, 2.5F, 11, 7, 5.5F, 10, 5.5F, 15)
                    path.AddBezier(5.5F, 15, 5.5F, 19, 8.5F, 21.5F, 12, 21.5F)
                    path.AddBezier(12, 21.5F, 15.5F, 21.5F, 18.5F, 19, 18.5F, 15)
                    path.AddBezier(18.5F, 15, 18.5F, 10, 13, 7, 12, 2.5F)
                    path.CloseFigure()
                    g.DrawPath(p, path)
                End Using

            Case IconKind.Kidney
                ' รูปไต — เมล็ดถั่วที่มีรอยเว้า (hilum) ทางซ้าย
                Using path As New GraphicsPath()
                    path.AddBezier(13.5F, 3, 18.5F, 3, 21, 6.8F, 21, 12)
                    path.AddBezier(21, 12, 21, 17.2F, 18.5F, 21, 13.5F, 21)
                    path.AddBezier(13.5F, 21, 8.5F, 21, 5.5F, 18, 5.5F, 14.6F)
                    path.AddBezier(5.5F, 14.6F, 5.5F, 12.6F, 10.4F, 13.4F, 10.4F, 12)
                    path.AddBezier(10.4F, 12, 10.4F, 10.6F, 5.5F, 11.4F, 5.5F, 9.4F)
                    path.AddBezier(5.5F, 9.4F, 5.5F, 6, 8.5F, 3, 13.5F, 3)
                    path.CloseFigure()
                    g.DrawPath(p, path)
                End Using

            Case IconKind.Gauge
                g.DrawArc(p, 3, 5, 18, 18, 180, 180)
                g.DrawLine(p, 3, 14, 21, 14)
                g.DrawLine(p, 12, 14, 16.5F, 8.5F)
                g.FillEllipse(b, 10.6F, 12.6F, 2.8F, 2.8F)

            Case IconKind.Filter
                Using path As New GraphicsPath()
                    path.AddLines({New PointF(3, 4), New PointF(21, 4), New PointF(14, 12.5F), New PointF(14, 20), New PointF(10, 18), New PointF(10, 12.5F)})
                    path.CloseFigure()
                    g.DrawPath(p, path)
                End Using

            Case IconKind.Power
                g.DrawArc(p, 4, 4.5F, 16, 16, -60, 300)
                g.DrawLine(p, 12, 2.5F, 12, 11)

            Case IconKind.Eye, IconKind.EyeOff
                Using path As New GraphicsPath()
                    path.AddBezier(2.5F, 12, 5, 7, 8.5F, 5, 12, 5)
                    path.AddBezier(12, 5, 15.5F, 5, 19, 7, 21.5F, 12)
                    path.AddBezier(21.5F, 12, 19, 17, 15.5F, 19, 12, 19)
                    path.AddBezier(12, 19, 8.5F, 19, 5, 17, 2.5F, 12)
                    path.CloseFigure()
                    g.DrawPath(p, path)
                End Using
                g.DrawEllipse(p, 9, 9, 6, 6)
                If kind = IconKind.EyeOff Then g.DrawLine(p, 3.5F, 3.5F, 20.5F, 20.5F)

            Case IconKind.Trash
                g.DrawLine(p, 3.5F, 6, 20.5F, 6)
                g.DrawLines(p, {New PointF(5.5F, 6), New PointF(6.5F, 21), New PointF(17.5F, 21), New PointF(18.5F, 6)})
                g.DrawLines(p, {New PointF(9, 6), New PointF(9, 3), New PointF(15, 3), New PointF(15, 6)})

            Case IconKind.Briefcase
                Using path = Gfx.RoundedRect(New RectangleF(2.5F, 7, 19, 14), 2.5F)
                    g.DrawPath(p, path)
                End Using
                g.DrawLines(p, {New PointF(8.5F, 7), New PointF(8.5F, 4), New PointF(15.5F, 4), New PointF(15.5F, 7)})
                g.DrawLine(p, 2.5F, 13, 21.5F, 13)
        End Select
    End Sub

End Class

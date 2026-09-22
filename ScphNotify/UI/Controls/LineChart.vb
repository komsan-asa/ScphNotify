Imports System.ComponentModel
Imports System.Drawing.Drawing2D

''' <summary>
''' กราฟเส้นแบบเบา (วาดเอง ไม่ต้องใช้ MSChart ซึ่งไม่มีใน .NET 10)
''' รองรับ hover แสดงค่า, เส้นอ้างอิง, และ render เป็นรูปสำหรับพิมพ์
''' </summary>
Public Class LineChart
    Inherits Control

    Public Class ReferenceLine
        Public Property Value As Double
        Public Property Label As String
        Public Property Color As Color
    End Class

    Private ReadOnly _points As New List(Of ChartPoint)
    Private ReadOnly _refs As New List(Of ReferenceLine)
    Private _hover As Integer = -1
    Private _lineColor As Color = Theme.Primary
    Private _emptyText As String = "ไม่มีข้อมูลสำหรับแสดงกราฟ"
    Private _format As String = "0.##"
    Private _showArea As Boolean = True

    ''' <summary>กำหนดสีของจุดแต่ละจุด (เช่นสีตามระดับความเสี่ยง) — Nothing = ใช้ LineColor</summary>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property PointColorizer As Func(Of ChartPoint, Color)

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        BackColor = Theme.Surface
        Size = New Size(400, 240)
    End Sub

    <Category("Chart")>
    Public Property LineColor As Color
        Get
            Return _lineColor
        End Get
        Set(value As Color)
            _lineColor = value
            Invalidate()
        End Set
    End Property

    Private Function ShouldSerializeLineColor() As Boolean
        Return _lineColor <> Theme.Primary
    End Function

    <Category("Chart"), DefaultValue("ไม่มีข้อมูลสำหรับแสดงกราฟ")>
    Public Property EmptyText As String
        Get
            Return _emptyText
        End Get
        Set(value As String)
            _emptyText = If(value, "")
            Invalidate()
        End Set
    End Property

    <Category("Chart"), DefaultValue("0.##")>
    Public Property ValueFormat As String
        Get
            Return _format
        End Get
        Set(value As String)
            _format = If(String.IsNullOrEmpty(value), "0.##", value)
            Invalidate()
        End Set
    End Property

    <Category("Chart"), DefaultValue(True)>
    Public Property ShowArea As Boolean
        Get
            Return _showArea
        End Get
        Set(value As Boolean)
            _showArea = value
            Invalidate()
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property Points As IReadOnlyList(Of ChartPoint)
        Get
            Return _points
        End Get
    End Property

    Public Sub SetData(points As IEnumerable(Of ChartPoint))
        _points.Clear()
        If points IsNot Nothing Then _points.AddRange(points)
        _hover = -1
        Invalidate()
    End Sub

    Public Sub ClearReferenceLines()
        _refs.Clear()
        Invalidate()
    End Sub

    Public Sub AddReferenceLine(value As Double, label As String, color As Color)
        _refs.Add(New ReferenceLine With {.Value = value, .Label = label, .Color = color})
        Invalidate()
    End Sub

    '──────────────── Layout ────────────────

    Private Structure Frame
        Public Plot As RectangleF
        Public Min As Double
        Public Max As Double
        Public TickStep As Double
    End Structure

    Private Shared Function NiceStep(range As Double, targetTicks As Integer) As Double
        If range <= 0 Then Return 1
        Dim rough = range / Math.Max(1, targetTicks)
        Dim mag = Math.Pow(10, Math.Floor(Math.Log10(rough)))
        Dim norm = rough / mag
        Dim nice As Double
        If norm < 1.5 Then
            nice = 1
        ElseIf norm < 3 Then
            nice = 2
        ElseIf norm < 7 Then
            nice = 5
        Else
            nice = 10
        End If
        Return nice * mag
    End Function

    Private Function ComputeFrame(bounds As Rectangle, g As Graphics, scale As Single) As Frame
        Dim values = _points.Select(Function(p) p.Value).Concat(_refs.Select(Function(r) r.Value)).ToList()
        Dim mn = values.Min()
        Dim mx = values.Max()
        If mx - mn < 0.000001 Then
            Dim pad = If(Math.Abs(mx) < 1, 1.0, Math.Abs(mx) * 0.1)
            mn -= pad : mx += pad
        Else
            Dim pad = (mx - mn) * 0.12
            mn -= pad : mx += pad
        End If
        If mn < 0 AndAlso values.Min() >= 0 Then mn = 0
        Dim stp = NiceStep(mx - mn, 4)
        mn = Math.Floor(mn / stp) * stp
        mx = Math.Ceiling(mx / stp) * stp

        Dim f = ChartFont(8.5F)
        Dim labelW = 0
        Dim v = mn
        While v <= mx + stp / 2
            labelW = Math.Max(labelW, Gfx.Measure(v.ToString(_format), f).Width)
            v += stp
        End While

        Dim left = bounds.X + labelW + 14 * scale
        Dim top = bounds.Y + 14 * scale
        Dim right = bounds.Right - 18 * scale
        Dim bottom = bounds.Bottom - 30 * scale
        Return New Frame With {.Plot = New RectangleF(left, top, Math.Max(10, right - left), Math.Max(10, bottom - top)), .Min = mn, .Max = mx, .TickStep = stp}
    End Function

    Private Shared Function PointAt(fr As Frame, index As Integer, count As Integer, value As Double) As PointF
        Dim x = If(count <= 1, fr.Plot.X + fr.Plot.Width / 2, fr.Plot.X + CSng(index * fr.Plot.Width / (count - 1)))
        Dim y = fr.Plot.Bottom - CSng((value - fr.Min) / (fr.Max - fr.Min) * fr.Plot.Height)
        Return New PointF(x, y)
    End Function

    '──────────────── Render ────────────────

    Private _fontScale As Single = 1.0F

    Private Function ChartFont(size As Single, Optional style As FontStyle = FontStyle.Regular) As Font
        Return Theme.UiFont(size * _fontScale, style)
    End Function

    ''' <param name="scale">ตัวคูณขนาดเส้น/ระยะ</param>
    ''' <param name="fontScale">ตัวคูณขนาดตัวอักษร (บนจอใช้ 1 เพราะ Windows ขยายตาม DPI ให้แล้ว)</param>
    Public Sub Render(g As Graphics, bounds As Rectangle, hoverIndex As Integer, Optional scale As Single = 1.0F, Optional fontScale As Single = 1.0F)
        _fontScale = fontScale
        Gfx.HighQuality(g)
        Using bg As New SolidBrush(BackColor)
            g.FillRectangle(bg, bounds)
        End Using

        If _points.Count = 0 Then
            Dim iconSize = 36 * scale
            IconPainter.Draw(g, IconKind.LineChart, New RectangleF(bounds.X + (bounds.Width - iconSize) / 2, bounds.Y + bounds.Height / 2.0F - iconSize - 4, iconSize, iconSize), Theme.TextMuted, 1.6F)
            Gfx.Text(g, _emptyText, ChartFont(9.5F), New Rectangle(bounds.X, bounds.Y + bounds.Height \ 2 + 4, bounds.Width, CInt(24 * scale)), Theme.TextMuted, TextFormatFlags.HorizontalCenter Or TextFormatFlags.Top)
            Return
        End If

        Dim fr = ComputeFrame(bounds, g, scale)
        Dim axisFont = ChartFont(8.5F)

        ' gridlines + y labels
        Using gridPen As New Pen(Theme.Border, 1)
            gridPen.DashStyle = DashStyle.Dash
            Dim v = fr.Min
            While v <= fr.Max + fr.TickStep / 2
                Dim y = fr.Plot.Bottom - CSng((v - fr.Min) / (fr.Max - fr.Min) * fr.Plot.Height)
                g.DrawLine(gridPen, fr.Plot.X, y, fr.Plot.Right, y)
                Gfx.Text(g, v.ToString(_format), axisFont, New Rectangle(bounds.X, CInt(y - 10), CInt(fr.Plot.X - bounds.X - 8 * scale), 20), Theme.TextMuted,
                         TextFormatFlags.Right Or TextFormatFlags.VerticalCenter)
                v += fr.TickStep
            End While
        End Using

        ' reference lines
        For Each rl In _refs
            If rl.Value < fr.Min OrElse rl.Value > fr.Max Then Continue For
            Dim y = fr.Plot.Bottom - CSng((rl.Value - fr.Min) / (fr.Max - fr.Min) * fr.Plot.Height)
            Using p As New Pen(Theme.WithAlpha(rl.Color, 170), 1.4F * scale)
                p.DashStyle = DashStyle.Dot
                g.DrawLine(p, fr.Plot.X, y, fr.Plot.Right, y)
            End Using
            If Not String.IsNullOrEmpty(rl.Label) Then
                Dim sz = Gfx.Measure(rl.Label, axisFont)
                Gfx.Text(g, rl.Label, axisFont, New Rectangle(CInt(fr.Plot.Right - sz.Width - 4), CInt(y - sz.Height - 1), sz.Width + 4, sz.Height), rl.Color)
            End If
        Next

        ' points
        Dim n = _points.Count
        Dim pts = Enumerable.Range(0, n).Select(Function(i) PointAt(fr, i, n, _points(i).Value)).ToArray()

        If _showArea AndAlso n > 1 Then
            Using path As New GraphicsPath()
                path.AddLines(pts)
                path.AddLine(pts(n - 1).X, pts(n - 1).Y, pts(n - 1).X, fr.Plot.Bottom)
                path.AddLine(pts(n - 1).X, fr.Plot.Bottom, pts(0).X, fr.Plot.Bottom)
                path.CloseFigure()
                Using br As New LinearGradientBrush(New RectangleF(fr.Plot.X, fr.Plot.Y - 1, fr.Plot.Width, fr.Plot.Height + 2),
                                                     Theme.WithAlpha(_lineColor, 70), Theme.WithAlpha(_lineColor, 4), LinearGradientMode.Vertical)
                    g.FillPath(br, path)
                End Using
            End Using
        End If

        If n > 1 Then
            Using linePen As New Pen(_lineColor, 2.4F * scale)
                linePen.LineJoin = LineJoin.Round
                linePen.StartCap = LineCap.Round
                linePen.EndCap = LineCap.Round
                g.DrawLines(linePen, pts)
            End Using
        End If

        ' x labels
        Dim maxLabelW = _points.Max(Function(p) Gfx.Measure(p.Label, axisFont).Width) + 10
        Dim stepN = Math.Max(1, CInt(Math.Ceiling(maxLabelW * n / Math.Max(1.0F, fr.Plot.Width + maxLabelW))))
        Dim shown As New List(Of Integer)
        For i = 0 To n - 1 Step stepN
            shown.Add(i)
        Next
        If shown(shown.Count - 1) <> n - 1 Then
            If shown.Count > 1 AndAlso (n - 1) - shown(shown.Count - 1) < stepN Then shown.RemoveAt(shown.Count - 1)
            shown.Add(n - 1)
        End If
        For Each i In shown
            Dim w = maxLabelW
            Dim lx = CInt(pts(i).X - w / 2)
            lx = Math.Max(bounds.X, Math.Min(bounds.Right - w, lx))   ' ไม่ให้ป้ายแกน X ล้นขอบ
            Gfx.Text(g, _points(i).Label, axisFont, New Rectangle(lx, CInt(fr.Plot.Bottom + 8 * scale), w, CInt(18 * scale)),
                     Theme.TextSecondary, TextFormatFlags.HorizontalCenter Or TextFormatFlags.Top)
        Next

        ' hover guide
        If hoverIndex >= 0 AndAlso hoverIndex < n Then
            Using guide As New Pen(Theme.BorderStrong, 1)
                g.DrawLine(guide, pts(hoverIndex).X, fr.Plot.Y, pts(hoverIndex).X, fr.Plot.Bottom)
            End Using
        End If

        ' markers
        For i = 0 To n - 1
            Dim c = If(PointColorizer IsNot Nothing, PointColorizer(_points(i)), _lineColor)
            Dim rad = If(i = hoverIndex, 6.0F, 4.0F) * scale
            Using fill As New SolidBrush(Color.White), stroke As New Pen(c, 2.2F * scale)
                g.FillEllipse(fill, pts(i).X - rad, pts(i).Y - rad, rad * 2, rad * 2)
                g.DrawEllipse(stroke, pts(i).X - rad, pts(i).Y - rad, rad * 2, rad * 2)
            End Using
        Next

        ' tooltip
        If hoverIndex >= 0 AndAlso hoverIndex < n Then
            DrawTooltip(g, _points(hoverIndex), pts(hoverIndex), bounds, scale)
        End If
    End Sub

    Private Sub DrawTooltip(g As Graphics, p As ChartPoint, anchor As PointF, bounds As Rectangle, scale As Single)
        Dim f1 = ChartFont(8.5F)
        Dim f2 = ChartFont(11.0F, FontStyle.Bold)
        Dim valueText = p.Value.ToString(_format)
        Dim lines As New List(Of Tuple(Of String, Font, Color)) From {
            Tuple.Create(p.Label, f1, Color.FromArgb(203, 213, 225)),
            Tuple.Create(valueText, f2, Color.White)}
        If Not String.IsNullOrEmpty(p.Detail) Then lines.Add(Tuple.Create(p.Detail, f1, Color.FromArgb(203, 213, 225)))

        Dim padX = 10 * scale, padY = 7 * scale
        Dim w = lines.Max(Function(l) Gfx.Measure(l.Item1, l.Item2).Width) + padX * 2
        Dim h = lines.Sum(Function(l) Gfx.Measure(l.Item1, l.Item2).Height) + padY * 2 + (lines.Count - 1) * 2
        Dim x = anchor.X + 12 * scale
        If x + w > bounds.Right - 4 Then x = anchor.X - w - 12 * scale
        Dim y = anchor.Y - h - 10 * scale
        If y < bounds.Y + 4 Then y = anchor.Y + 12 * scale
        Dim r As New RectangleF(x, y, w, h)
        Gfx.FillRounded(g, New RectangleF(r.X, r.Y + 2, r.Width, r.Height), 8 * scale, Color.FromArgb(40, 0, 0, 0))
        Gfx.FillRounded(g, r, 8 * scale, Theme.Sidebar)
        Dim ty = y + padY
        For Each l In lines
            Dim sz = Gfx.Measure(l.Item1, l.Item2)
            Gfx.Text(g, l.Item1, l.Item2, New Rectangle(CInt(x + padX), CInt(ty), sz.Width + 4, sz.Height), l.Item3)
            ty += sz.Height + 2
        Next
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Render(e.Graphics, ClientRectangle, _hover, Compat.DpiOf(Me) / 96.0F)
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If _points.Count = 0 Then Return
        Dim scale = Compat.DpiOf(Me) / 96.0F
        Using g = CreateGraphics()
            Dim fr = ComputeFrame(ClientRectangle, g, scale)
            Dim best = -1
            Dim bestDist = Single.MaxValue
            For i = 0 To _points.Count - 1
                Dim pt = PointAt(fr, i, _points.Count, _points(i).Value)
                Dim d = Math.Abs(pt.X - e.X)
                If d < bestDist Then bestDist = d : best = i
            Next
            If bestDist > 40 * scale Then best = -1
            If best <> _hover Then
                _hover = best
                Invalidate()
            End If
        End Using
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        If _hover <> -1 Then
            _hover = -1
            Invalidate()
        End If
    End Sub

    ''' <summary>สร้างรูปกราฟ (ใช้ตอนพิมพ์)</summary>
    Public Function RenderBitmap(width As Integer, height As Integer, Optional scale As Single = 2.0F) As Bitmap
        Dim bmp As New Bitmap(CInt(width * scale), CInt(height * scale))
        Using g = Graphics.FromImage(bmp)
            Render(g, New Rectangle(0, 0, bmp.Width, bmp.Height), -1, scale, scale)
        End Using
        _fontScale = 1.0F
        Return bmp
    End Function

End Class

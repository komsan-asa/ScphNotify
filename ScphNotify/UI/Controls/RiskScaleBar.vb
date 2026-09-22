''' <summary>แถบระดับความเสี่ยง Thai CV Risk 5 ระดับ พร้อมตัวชี้ค่าล่าสุด</summary>
Public Class RiskScaleBar
    Inherits Control

    Private _score As Double? = Nothing

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.Selectable, False)
        Size = New Size(560, 96)
    End Sub

    Public Sub SetScore(score As Double?)
        _score = score
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        Gfx.PaintParentBackground(Me, g)
        Gfx.HighQuality(g)

        Dim markerSpace = Theme.Scale(Me, 30)
        Dim barH = Theme.ScaleF(Me, 12)
        Dim gap = Theme.ScaleF(Me, 4)
        Dim segW = (Width - 2 - gap * 4) / 5.0F
        Dim barY = CSng(markerSpace)

        For i = 1 To 5
            Dim x = 1 + (i - 1) * (segW + gap)
            Dim r As New RectangleF(x, barY, segW, barH)
            Gfx.FillRounded(g, r, barH / 2, Theme.RiskColor(i))
            Dim level = CvdRiskLevel.FromScore(_score)
            Dim isCur = _score.HasValue AndAlso level = i
            Dim f1 = Theme.UiFont(9.0F, FontStyle.Bold)
            Dim f2 = Theme.UiFont(8.5F)
            Dim ty = CInt(barY + barH + Theme.Scale(Me, 6))
            Gfx.Text(g, CvdRiskLevel.Ranges(i), f1, New Rectangle(CInt(x), ty, CInt(segW), Theme.Scale(Me, 18)),
                     If(isCur, Theme.TextPrimary, Theme.TextSecondary), TextFormatFlags.HorizontalCenter Or TextFormatFlags.Top)
            Gfx.Text(g, CvdRiskLevel.Names(i), f2, New Rectangle(CInt(x), ty + Theme.Scale(Me, 18), CInt(segW), Theme.Scale(Me, 18)),
                     If(isCur, Theme.RiskColor(i), Theme.TextMuted), TextFormatFlags.HorizontalCenter Or TextFormatFlags.Top)
        Next

        If _score.HasValue Then
            Dim s = Math.Max(0, Math.Min(49.9, _score.Value))
            Dim idx = Math.Min(4, CInt(Math.Floor(s / 10)))
            Dim frac = CSng((s - idx * 10) / 10)
            Dim mx = 1 + idx * (segW + gap) + frac * segW
            Dim label = $"{_score.Value:0.#}%"
            Dim lf = Theme.UiFont(8.5F, FontStyle.Bold)
            Dim sz = Gfx.Measure(label, lf)
            Dim bw = sz.Width + Theme.Scale(Me, 14)
            Dim bh = sz.Height + Theme.Scale(Me, 6)
            Dim bx = Math.Max(0.0F, Math.Min(Width - bw - 1, mx - bw / 2.0F))
            Dim br As New RectangleF(bx, 0, bw, bh)
            Gfx.FillRounded(g, br, bh / 2.0F, Theme.Sidebar)
            Gfx.Text(g, label, lf, Rectangle.Round(br), Color.White, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
            Dim tipY = bh + 1
            Using b As New SolidBrush(Theme.Sidebar)
                g.FillPolygon(b, {New PointF(mx - 5, tipY), New PointF(mx + 5, tipY), New PointF(mx, barY - 2)})
            End Using
            Using ring As New Pen(Color.White, 2.5F), fill As New SolidBrush(Theme.RiskColor(idx + 1))
                Dim d = barH + 6
                g.FillEllipse(fill, mx - d / 2, barY + barH / 2 - d / 2, d, d)
                g.DrawEllipse(ring, mx - d / 2, barY + barH / 2 - d / 2, d, d)
            End Using
        End If
    End Sub

End Class

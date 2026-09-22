''' <summary>หน้า CVD Risk (แทน dialogHart เดิม)</summary>
Public Class CvdRiskPage
    Implements IPatientPage

    Private _boundKey As String
    Private _rows As New List(Of CvdRiskEntry)

    Public Sub New()
        InitializeComponent()
        GridStyler.Apply(dgvCvd)
        GridStyler.AddTextColumn(dgvCvd, "colDate", "วันที่", 15, minWidth:=78)
        GridStyler.AddTextColumn(dgvCvd, "colRisk", "ความเสี่ยง", 26, minWidth:=130)
        GridStyler.AddTextColumn(dgvCvd, "colBp", "SBP", 9, DataGridViewContentAlignment.MiddleRight, 48)
        GridStyler.AddTextColumn(dgvCvd, "colLab", "Chol / LDL / HDL", 24, DataGridViewContentAlignment.MiddleCenter, 110)
        GridStyler.AddTextColumn(dgvCvd, "colDm", "DM", 7, DataGridViewContentAlignment.MiddleCenter, 40)
        GridStyler.AddTextColumn(dgvCvd, "colSmoke", "บุหรี่", 8, DataGridViewContentAlignment.MiddleCenter, 46)
        GridStyler.AddTextColumn(dgvCvd, "colAge", "อายุ", 8, DataGridViewContentAlignment.MiddleRight, 42)

        chartCvd.ValueFormat = "0.#"
        chartCvd.EmptyText = "ไม่มีข้อมูลสำหรับแสดงแนวโน้ม"
        chartCvd.PointColorizer = Function(p) Theme.RiskColor(CvdRiskLevel.FromScore(p.Value))
        For Each t In {10, 20, 30, 40}
            chartCvd.AddReferenceLine(t, $"{t}%", Theme.RiskColor(t \ 10 + 1))
        Next
    End Sub

    Public Sub ResetBinding() Implements IPatientPage.ResetBinding
        _boundKey = Nothing
    End Sub

    Public Sub BindPatient(snap As PatientSnapshot) Implements IPatientPage.BindPatient
        Dim key = If(snap Is Nothing, "", $"{snap.Vn}|{snap.LoadedAt.Ticks}")
        If key = _boundKey Then Return
        _boundKey = key

        _rows = If(snap?.CvdRisk, New List(Of CvdRiskEntry)).OrderByDescending(Function(r) r.VisitDate.GetValueOrDefault()).ToList()
        Dim hasData = _rows.Count > 0
        tlpMain.Visible = hasData
        emptyCvd.Visible = Not hasData
        If Not hasData Then Return

        ' ล่าสุด
        Dim latest = snap.LatestCvd
        If latest IsNot Nothing Then
            lblScore.Text = latest.ScoreText
            lblScore.ForeColor = Theme.RiskColor(latest.Level)
            lblLevel.Text = $"ความเสี่ยง{CvdRiskLevel.Names(latest.Level)}  ({CvdRiskLevel.Ranges(latest.Level)})"
            lblLevel.ForeColor = Theme.Blend(Theme.RiskColor(latest.Level), Color.Black, 0.25)
            lblLatestDate.Text = $"ประเมินเมื่อ {ThaiDate.MediumDate(latest.VisitDate)}"
            riskBar.SetScore(latest.Score)
        Else
            lblScore.Text = "-"
            lblLevel.Text = "ไม่มีคะแนน"
            lblLatestDate.Text = ""
            riskBar.SetScore(Nothing)
        End If

        ' ตาราง
        dgvCvd.Rows.Clear()
        For Each r In _rows
            dgvCvd.Rows.Add(If(r.VisitDate.HasValue, ThaiDate.ShortDate(r.VisitDate.Value), "-"), r.ScoreText,
                            If(r.Bps.HasValue, r.Bps.Value.ToString("0"), "-"),
                            CompactLab(r.LabText), If(r.IsDm, "Y", "N"), If(r.IsSmoker, "Y", "N"),
                            If(r.Age.HasValue, r.Age.Value.ToString(), "-"))
        Next
        dgvCvd.ClearSelection()

        ' กราฟ (เรียงจากเก่าไปใหม่)
        chartCvd.SetData(_rows.Where(Function(r) r.Score.HasValue).
                               OrderBy(Function(r) r.VisitDate.GetValueOrDefault()).
                               Select(Function(r) New ChartPoint(If(r.VisitDate.HasValue, ThaiDate.ShortDate(r.VisitDate.Value), "-"),
                                                                 r.Score.Value, $"ความเสี่ยง{CvdRiskLevel.Names(r.Level)}")))
    End Sub

    ''' <summary>"Chol:216.00  LDL:142.00 HDL: 44.00" → "216 / 142 / 44"</summary>
    Private Shared Function CompactLab(labText As String) As String
        Dim nums = System.Text.RegularExpressions.Regex.Matches(If(labText, ""), "(?<=:)\s*([0-9.]+|-)")
        If nums.Count <> 3 Then Return labText
        Dim parts = nums.Cast(Of System.Text.RegularExpressions.Match)().Select(Function(m)
                                                                               Dim d As Double
                                                                               Dim v = m.Groups(1).Value
                                                                               If Double.TryParse(v, Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, d) Then Return d.ToString("0.#")
                                                                               Return v
                                                                           End Function)
        Return String.Join(" / ", parts)
    End Function

    ''' <summary>วาดคอลัมน์ "ความเสี่ยง" เป็นป้ายสีตามระดับ (เดิมใช้สีพื้นแถว และเงื่อนไขไม่เคยตรง)</summary>
    Private Sub dgvCvd_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles dgvCvd.CellPainting
        If e.RowIndex < 0 OrElse e.ColumnIndex <> dgvCvd.Columns("colRisk").Index Then Return
        If e.RowIndex >= _rows.Count Then Return
        e.PaintBackground(e.CellBounds, True)
        Dim g = e.Graphics
        Gfx.HighQuality(g)
        Dim entry = _rows(e.RowIndex)
        Dim c = Theme.RiskColor(entry.Level)
        Dim text = $"{entry.ScoreText}  {CvdRiskLevel.Names(entry.Level)}"
        Dim f = Theme.UiFont(9.0F, FontStyle.Bold)
        Dim sz = Gfx.Measure(text, f)
        Dim h = sz.Height + 8
        Dim r As New RectangleF(e.CellBounds.X + 8, e.CellBounds.Y + (e.CellBounds.Height - h) / 2.0F, sz.Width + 22, h)
        Gfx.FillRounded(g, r, h / 2.0F, Theme.Blend(Color.White, c, 0.16))
        Using b As New SolidBrush(c)
            g.FillEllipse(b, r.X + 8, r.Y + (h - 7) / 2.0F, 7, 7)
        End Using
        Gfx.Text(g, text, f, New Rectangle(CInt(r.X) + 19, CInt(r.Y), sz.Width + 4, CInt(h)), Theme.Blend(c, Color.Black, 0.35))
        e.Handled = True
    End Sub

End Class

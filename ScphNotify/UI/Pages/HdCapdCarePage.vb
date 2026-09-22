''' <summary>
''' HD/CAPD Care — ภาพรวมผู้ป่วยบำบัดทดแทนไต
'''
''' แบ่งเป็น 2 ส่วนตามที่มาของข้อมูล
'''   กลุ่ม A (ใช้งานได้จริง) : ดึงจากตารางมาตรฐานของ HOSxP ที่มีข้อมูลอยู่แล้ว
'''       - ชนิด/ความถี่การล้างไต : opitemrece + nondrugitems (รายการค่าบริการ)
'''       - น้ำหนัก / ความดัน      : ovst + opdscreen
'''       - ผล LAB สำคัญ          : lab_head + lab_order
'''       - ยาที่ได้รับ            : opitemrece + drugitems
'''   กลุ่ม B (ตัวอย่างหน้าจอ)  : HOSxP ไม่ได้เก็บค่าเหล่านี้ (Dry weight, UF, BFR, Inflow/Outflow,
'''       Exit site, อาการเฝ้าระวัง) ต้องสร้างตารางเพิ่มและมีหน้าบันทึกก่อน จึงแสดงเป็นข้อมูลสมมติ
'''       เพื่อให้ตกลงหน้าตาก่อนลงมือทำจริง — ทุกการ์ดมีป้าย "ตัวอย่าง" กำกับ
''' </summary>
Public Class HdCapdCarePage
    Implements IPatientPage

    Private _boundKey As String
    Private _snap As PatientSnapshot
    Private _busy As Boolean
    Private _mockBuilt As Boolean

    Public Sub New()
        InitializeComponent()
        GridStyler.Apply(dgvLabs)
        GridStyler.Apply(dgvDrugs)
        GridStyler.Apply(dgvSessionLog)
        chartSessions.LineColor = Theme.Primary
        chartVitals.LineColor = Theme.Info
        bnMock.ShowMessage(
            "ส่วนด้านล่างนี้เป็นตัวอย่างหน้าจอ ยังไม่ใช่ข้อมูลจริง — HOSxP ไม่ได้เก็บ Dry weight, UF, " &
            "Blood Flow Rate, Inflow/Outflow, สภาพ Exit site และอาการเฝ้าระวัง " &
            "ถ้าตกลงหน้าตาแล้วจะสร้างตารางเก็บข้อมูลและหน้าบันทึกให้ต่อ",
            AlertLevel.Warning, IconKind.Info)
    End Sub

    Public Sub ResetBinding() Implements IPatientPage.ResetBinding
        _boundKey = Nothing
    End Sub

    Public Sub BindPatient(snap As PatientSnapshot) Implements IPatientPage.BindPatient
        Dim key = If(snap Is Nothing, "", $"{snap.Hn}|{snap.LoadedAt.Ticks}")
        If key = _boundKey Then Return
        _boundKey = key
        _snap = snap
        BuildMockup()
        If snap Is Nothing Then Return
        LoadAsync()
    End Sub

    '════════════════════ กลุ่ม A — ข้อมูลจริงจาก HOSxP ════════════════════

    Private Async Sub LoadAsync()
        If _busy OrElse _snap Is Nothing Then Return
        Dim hn = _snap.Hn
        Dim cfg = AppConfig.Current.Dialysis
        Dim fromDate = Date.Today.AddMonths(-cfg.Months)

        _busy = True
        ShowLoading()
        Try
            Dim repo = AppSession.Repository
            Dim sessions = Await repo.GetDialysisSessionsAsync(hn, fromDate, cfg.HdList(), cfg.PdList())
            If Not StillSame(hn) Then Return
            Dim vitals = Await repo.GetVitalHistoryAsync(hn, fromDate, 60)
            If Not StillSame(hn) Then Return
            Dim labs = Await repo.GetLabHistoryAsync(hn, cfg.LabList())
            If Not StillSame(hn) Then Return
            Dim drugs = Await repo.GetDialysisDrugsAsync(hn, fromDate, cfg.DrugList())
            If Not StillSame(hn) Then Return

            FillModality(sessions)
            FillSessionChart(sessions, cfg.Months)
            FillVitals(vitals)
            FillLabs(cfg.LabList(), labs)
            FillDrugs(drugs)
        Catch ex As Exception
            flpModality.Controls.Clear()
            flpModality.Controls.Add(Note("โหลดข้อมูลไม่สำเร็จ — " & ex.Message, Theme.Danger))
            ErrorDialog.ShowError(FindForm(), "โหลดข้อมูล HD/CAPD Care ไม่สำเร็จ", ex)
        Finally
            _busy = False
        End Try
    End Sub

    Private Function StillSame(hn As String) As Boolean
        Return _snap IsNot Nothing AndAlso _snap.Hn = hn
    End Function

    Private Sub ShowLoading()
        flpModality.Controls.Clear()
        flpModality.Controls.Add(Note("กำลังโหลด...", Theme.TextMuted))
        lblVitalNow.Text = "กำลังโหลด..."
    End Sub

    '──────────────── การ์ด "การบำบัดทดแทนไต" ────────────────

    Private Sub FillModality(sessions As List(Of DialysisSession))
        flpModality.SuspendLayout()
        flpModality.Controls.Clear()
        Try
            Dim hd = sessions.Where(Function(s) s.Modality = DialysisModality.Hd).Count()
            Dim pd = sessions.Where(Function(s) s.Modality = DialysisModality.Pd).Count()

            ' ชนิดการล้างไต — ใช้ชนิดที่พบมากกว่าเป็นหลัก และบอกไว้ถ้าพบทั้งสองแบบ
            Dim chip As StatusChip
            If hd = 0 AndAlso pd = 0 Then
                ' ยังยึดค่าจากหน้าหลัก (getCaseHd เดิม) ไว้เป็นทางเลือกสำรอง
                Dim legacy = If(_snap?.CaseHd, "").Trim()
                chip = If(legacy = "",
                          New StatusChip("ไม่พบรายการล้างไต", AlertLevel.Neutral, IconKind.Info),
                          New StatusChip(legacy, AlertLevel.Primary, IconKind.Kidney))
            ElseIf pd >= hd Then
                chip = New StatusChip("ล้างไตทางช่องท้อง (PD/CAPD)", AlertLevel.Primary, IconKind.Kidney)
            Else
                chip = New StatusChip("ฟอกเลือด (HD)", AlertLevel.Primary, IconKind.Kidney)
            End If
            chip.Font = Theme.UiFont(10.5F, FontStyle.Bold)
            chip.Margin = New Padding(0, 0, 0, 10)
            flpModality.Controls.Add(chip)

            If hd > 0 AndAlso pd > 0 Then
                flpModality.Controls.Add(Note($"พบทั้งรายการ HD ({hd}) และ PD ({pd}) — ตรวจสอบรายการค่าบริการ", Theme.WarningText))
            End If

            If sessions.Count = 0 Then
                flpModality.Controls.Add(Note("ไม่พบรายการล้างไตย้อนหลัง " & AppConfig.Current.Dialysis.Months & " เดือน", Theme.TextMuted))
            Else
                Dim last = sessions.Max(Function(s) s.SessionDate)
                Dim in30 = sessions.Where(Function(s) s.SessionDate >= Date.Today.AddDays(-30)).Count()
                Dim in90 = sessions.Where(Function(s) s.SessionDate >= Date.Today.AddDays(-90)).Count()
                Dim perWeek = in30 / 30.0 * 7.0
                flpModality.Controls.Add(Stat("ครั้งล่าสุด", ThaiDate.MediumDate(last) & DaysAgo(last)))
                flpModality.Controls.Add(Stat("ความถี่", $"{in30} ครั้ง/30 วัน  ·  เฉลี่ย {perWeek:0.0} ครั้ง/สัปดาห์"))
                flpModality.Controls.Add(Stat("90 วันที่ผ่านมา", $"{in90} ครั้ง"))
            End If

            ' โรคร่วมที่มีผลต่อการดูแล
            Dim dx As New FlowLayoutPanel() With {
                .AutoSize = True, .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                .Margin = New Padding(0, 8, 0, 0), .WrapContents = True}
            If _snap IsNot Nothing Then
                If _snap.HasDmDiagnosis Then dx.Controls.Add(SmallChip("เบาหวาน", AlertLevel.Warning))
                If _snap.HasHtDiagnosis Then dx.Controls.Add(SmallChip("ความดันโลหิตสูง", AlertLevel.Warning))
                If _snap.EgfrResults.Count > 0 Then dx.Controls.Add(SmallChip("eGFR < 60", AlertLevel.Danger))
            End If
            If dx.Controls.Count > 0 Then flpModality.Controls.Add(dx)
        Finally
            flpModality.ResumeLayout(True)
        End Try
    End Sub

    Private Shared Function DaysAgo(d As Date) As String
        Dim n = CInt((Date.Today - d.Date).TotalDays)
        If n <= 0 Then Return "  (วันนี้)"
        Return $"  ({n} วันก่อน)"
    End Function

    '──────────────── กราฟความถี่ / น้ำหนัก ────────────────

    Private Sub FillSessionChart(sessions As List(Of DialysisSession), months As Integer)
        Dim points As New List(Of ChartPoint)
        For i = months - 1 To 0 Step -1
            Dim m = New Date(Date.Today.Year, Date.Today.Month, 1).AddMonths(-i)
            Dim n = sessions.Where(Function(s) s.SessionDate.Year = m.Year AndAlso s.SessionDate.Month = m.Month).Count()
            points.Add(New ChartPoint(ThaiDate.MediumDate(m).Substring(ThaiDate.MediumDate(m).IndexOf(" "c) + 1),
                                      n, $"{n} ครั้ง"))
        Next
        chartSessions.SetData(points)
    End Sub

    Private Sub FillVitals(vitals As List(Of VitalPoint))
        Dim weights = vitals.Where(Function(v) v.BodyWeight.HasValue).
                             OrderBy(Function(v) v.VisitDate).
                             Select(Function(v) New ChartPoint(ThaiDate.ShortDate(v.VisitDate), v.BodyWeight.Value,
                                                               $"{v.BodyWeight.Value:0.0} kg")).ToList()
        chartVitals.SetData(weights)

        Dim latest = vitals.Where(Function(v) v.Systolic.HasValue OrElse v.BodyWeight.HasValue).
                            OrderByDescending(Function(v) v.VisitDate).FirstOrDefault()
        If latest Is Nothing Then
            lblVitalNow.Text = "ไม่พบข้อมูลที่จุดคัดกรอง"
            lblVitalNow.ForeColor = Theme.TextMuted
            Return
        End If

        Dim parts As New List(Of String)
        If latest.BodyWeight.HasValue Then parts.Add($"{latest.BodyWeight.Value:0.0} kg")
        If latest.Systolic.HasValue Then parts.Add($"BP {latest.BpText}")
        If latest.Pulse.HasValue Then parts.Add($"ชีพจร {latest.Pulse.Value:0}")
        lblVitalNow.Text = $"ล่าสุด {ThaiDate.MediumDate(latest.VisitDate)}  ·  " & String.Join("  ·  ", parts)
        lblVitalNow.ForeColor = Theme.TextSecondary
    End Sub

    '──────────────── ตารางผล LAB สำคัญ ────────────────

    Private Sub FillLabs(items As List(Of String), points As List(Of LabPoint))
        dgvLabs.SuspendLayout()
        Try
            dgvLabs.Rows.Clear()
            dgvLabs.Columns.Clear()
            dgvLabs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            GridStyler.AddTextColumn(dgvLabs, "colName", "รายการ", 170, DataGridViewContentAlignment.MiddleLeft, 108)
            GridStyler.AddTextColumn(dgvLabs, "colValue", "ค่าล่าสุด", 85, DataGridViewContentAlignment.MiddleRight, 62)
            GridStyler.AddTextColumn(dgvLabs, "colTrend", "", 30, DataGridViewContentAlignment.MiddleCenter, 24)
            GridStyler.AddTextColumn(dgvLabs, "colDate", "วันที่", 90, DataGridViewContentAlignment.MiddleRight, 68)
            GridStyler.AddTextColumn(dgvLabs, "colPrev", "ครั้งก่อน", 80, DataGridViewContentAlignment.MiddleRight, 60)
            GridStyler.AddTextColumn(dgvLabs, "colTarget", "เป้าหมาย", 95, DataGridViewContentAlignment.MiddleRight, 72)

            Dim byName = points.Where(Function(p) p.ReportDate.HasValue).
                                GroupBy(Function(p) If(p.LabName, "").Trim(), StringComparer.OrdinalIgnoreCase).
                                ToDictionary(Function(g) g.Key,
                                             Function(g) g.OrderByDescending(Function(p) p.ReportDate.Value).ToList(),
                                             StringComparer.OrdinalIgnoreCase)

            For Each item In items
                Dim series As List(Of LabPoint) = Nothing
                If Not byName.TryGetValue(item, series) OrElse series.Count = 0 Then Continue For
                Dim row = BuildKeyLab(item, series)
                Dim idx = dgvLabs.Rows.Add(ShortLabName(item), row.Latest, row.TrendText,
                                           If(row.LatestDate.HasValue, ThaiDate.ShortDate(row.LatestDate.Value), "-"),
                                           row.Previous, row.Target)
                dgvLabs.Rows(idx).Tag = row
            Next
            dgvLabs.ClearSelection()
        Finally
            dgvLabs.ResumeLayout()
        End Try
    End Sub

    ''' <summary>ค่าล่าสุด + ค่าก่อนหน้า + ช่วงเป้าหมายของผู้ป่วยล้างไต</summary>
    Private Shared Function BuildKeyLab(item As String, series As List(Of LabPoint)) As KeyLabRow
        Dim row As New KeyLabRow With {
            .LabName = item,
            .Latest = If(series(0).ResultText, "").Trim(),
            .LatestDate = series(0).ReportDate}
        If series.Count > 1 Then
            row.Previous = If(series(1).ResultText, "").Trim()
            row.PreviousDate = series(1).ReportDate
            Dim a = series(0).NumericValue, b = series(1).NumericValue
            If a.HasValue AndAlso b.HasValue Then
                If a.Value > b.Value Then
                    row.Trend = 1
                ElseIf a.Value < b.Value Then
                    row.Trend = -1
                End If
            End If
        End If
        LabTarget(item, series(0).NumericValue, row)
        Return row
    End Function

    ''' <summary>
    ''' ช่วงเป้าหมายอ้างอิงแนวทางการดูแลผู้ป่วยล้างไตที่ใช้กันทั่วไป
    ''' ใช้เป็นสีเตือนบนหน้าจอเท่านั้น การตัดสินใจทางคลินิกยังเป็นของแพทย์
    ''' </summary>
    Private Shared Sub LabTarget(item As String, value As Double?, row As KeyLabRow)
        Dim name = item.ToUpperInvariant()
        Dim low As Double?, high As Double?
        If name.Contains("POTASSIUM") OrElse name = "K" Then
            low = 3.5 : high = 5.5 : row.Target = "3.5 – 5.5"
        ElseIf name.Contains("HGB") OrElse name.Contains("HEMOGLOBIN") Then
            low = 10 : high = 12 : row.Target = "10 – 12"
        ElseIf name.Contains("HCT") Then
            low = 30 : high = 36 : row.Target = "30 – 36"
        ElseIf name.Contains("ALBUMIN") Then
            low = 3.5 : high = Nothing : row.Target = "≥ 3.5"
        ElseIf name.Contains("PHOSPHORUS") OrElse name.Contains("PO4") Then
            low = 3.5 : high = 5.5 : row.Target = "3.5 – 5.5"
        ElseIf name.Contains("CALCIUM") Then
            low = 8.4 : high = 10.2 : row.Target = "8.4 – 10.2"
        ElseIf name.Contains("PARATHYROID") OrElse name.Contains("PTH") Then
            low = 150 : high = 600 : row.Target = "150 – 600"
        ElseIf name.Contains("CARBONDIOXIDE") OrElse name.Contains("ECO2") Then
            low = 22 : high = 29 : row.Target = "22 – 29"
        ElseIf name.Contains("SODIUM") Then
            low = 135 : high = 145 : row.Target = "135 – 145"
        Else
            row.Target = "-"
            Return
        End If

        If Not value.HasValue Then Return
        Dim v = value.Value
        If (low.HasValue AndAlso v < low.Value) OrElse (high.HasValue AndAlso v > high.Value) Then
            row.Level = AlertLevel.Danger
        Else
            row.Level = AlertLevel.Success
        End If
    End Sub

    ''' <summary>ชื่อรายการใน HOSxP ยาวเกินช่อง — ตัดส่วนหน้า "Na:" ออก</summary>
    Private Shared Function ShortLabName(item As String) As String
        Dim s = If(item, "").TrimStart("*"c)
        Dim colon = s.IndexOf(":"c)
        If colon > 0 AndAlso colon <= 5 Then s = s.Substring(colon + 1)
        Return s.Trim()
    End Function

    Private Sub dgvLabs_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvLabs.CellFormatting
        If e.RowIndex < 0 OrElse e.RowIndex >= dgvLabs.Rows.Count Then Return
        Dim row = TryCast(dgvLabs.Rows(e.RowIndex).Tag, KeyLabRow)
        If row Is Nothing Then Return
        Select Case e.ColumnIndex
            Case 0
                e.CellStyle.Font = Theme.UiFont(9.75F, FontStyle.Bold)
            Case 1
                e.CellStyle.Font = Theme.UiFont(10.5F, FontStyle.Bold)
                Select Case row.Level
                    Case AlertLevel.Danger : e.CellStyle.ForeColor = Theme.Danger
                    Case AlertLevel.Success : e.CellStyle.ForeColor = Theme.SuccessText
                    Case Else : e.CellStyle.ForeColor = Theme.TextPrimary
                End Select
            Case 2
                e.CellStyle.ForeColor = If(row.Trend > 0, Theme.Danger, Theme.Info)
            Case 3, 4
                e.CellStyle.ForeColor = Theme.TextMuted
            Case 5
                e.CellStyle.ForeColor = Theme.TextSecondary
        End Select
    End Sub

    '──────────────── ตารางยา ────────────────

    Private Sub FillDrugs(drugs As List(Of DrugUsage))
        dgvDrugs.SuspendLayout()
        Try
            dgvDrugs.Rows.Clear()
            dgvDrugs.Columns.Clear()
            dgvDrugs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            GridStyler.AddTextColumn(dgvDrugs, "colDrug", "ชื่อยา", 200, DataGridViewContentAlignment.MiddleLeft, 150)
            GridStyler.AddTextColumn(dgvDrugs, "colLast", "สั่งล่าสุด", 95, DataGridViewContentAlignment.MiddleRight, 82)
            GridStyler.AddTextColumn(dgvDrugs, "colTimes", "ครั้ง", 55, DataGridViewContentAlignment.MiddleRight, 48)
            For Each d In drugs
                dgvDrugs.Rows.Add(d.DrugName, ThaiDate.MediumDate(d.LastDate), d.Times)
            Next
            dgvDrugs.ClearSelection()
        Finally
            dgvDrugs.ResumeLayout()
        End Try
    End Sub

    '════════════════════ กลุ่ม B — ตัวอย่างหน้าจอ (ยังไม่มีข้อมูลจริง) ════════════════════

    Private Sub BuildMockup()
        If _mockBuilt Then Return
        _mockBuilt = True
        BuildFluidMock()
        BuildSessionLogMock()
        BuildAccessMock()
        BuildSymptomMock()
    End Sub

    Private Sub BuildFluidMock()
        flpFluid.SuspendLayout()
        Try
            flpFluid.Controls.Add(BigStat("น้ำหนักแห้ง (Dry weight)", "52.0", "kg", Theme.TextPrimary))
            flpFluid.Controls.Add(BigStat("น้ำหนักปัจจุบัน", "52.3", "kg", Theme.TextPrimary))
            flpFluid.Controls.Add(BigStat("ส่วนต่างจากน้ำหนักแห้ง", "+0.3", "kg", Theme.Warning))
            flpFluid.Controls.Add(Stat("ปัสสาวะ 24 ชม.", "150 ml/วัน  ·  UF รอบล่าสุด 1,800 ml"))
            Dim chip As New StatusChip("อยู่ในเกณฑ์ (< 1.0 kg)", AlertLevel.Success, IconKind.Check) With {
                .Margin = New Padding(0, 6, 0, 0)}
            flpFluid.Controls.Add(chip)
        Finally
            flpFluid.ResumeLayout(True)
        End Try
    End Sub

    ''' <summary>ตารางรอบล้างไต — คอลัมน์ของ HD (ถ้าเป็น PD จะเปลี่ยนเป็น Inflow/Outflow/Dwell/สีน้ำยา)</summary>
    Private Sub BuildSessionLogMock()
        dgvSessionLog.SuspendLayout()
        Try
            dgvSessionLog.Columns.Clear()
            dgvSessionLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            GridStyler.AddTextColumn(dgvSessionLog, "c0", "วันที่", 92, DataGridViewContentAlignment.MiddleLeft, 80)
            GridStyler.AddTextColumn(dgvSessionLog, "c1", "นน.ก่อน", 76, DataGridViewContentAlignment.MiddleRight, 64)
            GridStyler.AddTextColumn(dgvSessionLog, "c2", "นน.หลัง", 76, DataGridViewContentAlignment.MiddleRight, 64)
            GridStyler.AddTextColumn(dgvSessionLog, "c3", "UF (ml)", 70, DataGridViewContentAlignment.MiddleRight, 58)
            GridStyler.AddTextColumn(dgvSessionLog, "c4", "BFR", 62, DataGridViewContentAlignment.MiddleRight, 52)
            GridStyler.AddTextColumn(dgvSessionLog, "c5", "BP ก่อน/หลัง", 118, DataGridViewContentAlignment.MiddleRight, 104)
            GridStyler.AddTextColumn(dgvSessionLog, "c6", "เหตุการณ์ระหว่างฟอก", 160, DataGridViewContentAlignment.MiddleLeft, 132)

            Dim rows = New String()() {
                New String() {"53.8", "52.1", "1,700", "280", "148/88→118/72", "-"},
                New String() {"54.2", "52.3", "1,900", "280", "152/90→104/62", "ความดันตก ให้ NSS"},
                New String() {"53.5", "52.0", "1,500", "300", "140/86→120/74", "-"},
                New String() {"54.0", "52.2", "1,800", "300", "146/88→116/70", "ตะคริวปลายรอบ"},
                New String() {"53.9", "52.1", "1,800", "280", "150/92→122/76", "-"}}
            Dim d = Date.Today
            For i = 0 To rows.Length - 1
                While d.DayOfWeek <> DayOfWeek.Monday AndAlso d.DayOfWeek <> DayOfWeek.Wednesday AndAlso d.DayOfWeek <> DayOfWeek.Friday
                    d = d.AddDays(-1)
                End While
                Dim cells As New List(Of Object) From {ThaiDate.MediumDate(d)}
                cells.AddRange(rows(i))
                dgvSessionLog.Rows.Add(cells.ToArray())
                d = d.AddDays(-1)
            Next
            dgvSessionLog.ClearSelection()
        Finally
            dgvSessionLog.ResumeLayout()
        End Try
    End Sub

    Private Sub dgvSessionLog_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvSessionLog.CellFormatting
        ' ทั้งตารางเป็นตัวอย่าง จึงทำให้จางลงเพื่อไม่ให้สับสนกับข้อมูลจริง
        e.CellStyle.ForeColor = Theme.TextSecondary
        If e.ColumnIndex = 6 AndAlso Convert.ToString(e.Value) <> "-" Then
            e.CellStyle.ForeColor = Theme.WarningText
        End If
    End Sub

    Private Sub BuildAccessMock()
        flpAccess.SuspendLayout()
        Try
            flpAccess.Controls.Add(Stat("ชนิดทางเข้าออก", "AVF แขนซ้าย (เปิดใช้ 12 มี.ค. 67)"))
            flpAccess.Controls.Add(Stat("Thrill / Bruit", "คลำได้ชัด / ฟังได้ชัด"))
            flpAccess.Controls.Add(Stat("รอบแผล", "ไม่บวม ไม่แดง ไม่มี discharge"))
            flpAccess.Controls.Add(Stat("ประเมินล่าสุดโดย", "พว.สมศรี  ·  วันนี้ 08:15 น."))
            Dim chips As New FlowLayoutPanel() With {
                .AutoSize = True, .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                .Margin = New Padding(0, 8, 0, 0), .WrapContents = True}
            chips.Controls.Add(SmallChip("ใช้งานได้ปกติ", AlertLevel.Success))
            chips.Controls.Add(SmallChip("ไม่พบสัญญาณติดเชื้อ", AlertLevel.Success))
            chips.Controls.Add(SmallChip("ผู้ป่วย PD → Exit site + สีน้ำยาทิ้ง", AlertLevel.Info))
            flpAccess.Controls.Add(chips)
        Finally
            flpAccess.ResumeLayout(True)
        End Try
    End Sub

    Private Sub BuildSymptomMock()
        flpSymptom.SuspendLayout()
        Try
            Dim items = New Object() {
                New Object() {"บวม / น้ำเกิน", AlertLevel.Warning},
                New Object() {"เหนื่อยหอบ", AlertLevel.Success},
                New Object() {"ปวดท้อง", AlertLevel.Success},
                New Object() {"ไข้", AlertLevel.Success},
                New Object() {"น้ำยาทิ้งขุ่น", AlertLevel.Success},
                New Object() {"ตะคริว", AlertLevel.Warning},
                New Object() {"เวียนศีรษะ", AlertLevel.Success},
                New Object() {"คันตามตัว", AlertLevel.Success}}
            For Each o In items
                Dim pair = DirectCast(o, Object())
                Dim level = DirectCast(pair(1), AlertLevel)
                Dim text = Convert.ToString(pair(0)) & If(level = AlertLevel.Warning, " : มี", " : ไม่มี")
                Dim chip As New StatusChip(text, level,
                                           If(level = AlertLevel.Warning, IconKind.Warning, IconKind.Check)) With {
                    .Margin = New Padding(0, 0, 8, 8)}
                flpSymptom.Controls.Add(chip)
            Next
            flpSymptom.Controls.Add(Note("บันทึกทุกรอบ พร้อมส่งเข้าแถบแจ้งเตือนเมื่อพบอาการที่ต้องเฝ้าระวัง", Theme.TextMuted))
        Finally
            flpSymptom.ResumeLayout(True)
        End Try
    End Sub

    '──────────────── ชิ้นส่วนเล็ก ๆ ที่ใช้ซ้ำ ────────────────

    ''' <summary>คำอธิบายตัวเล็กบรรทัดบน + ค่าตัวหนาบรรทัดล่าง</summary>
    Private Function Stat(caption As String, value As String) As Control
        Dim p As New FlowLayoutPanel() With {
            .AutoSize = True, .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .FlowDirection = FlowDirection.TopDown, .WrapContents = False,
            .Margin = New Padding(0, 0, 0, 8), .Padding = New Padding(0)}
        p.Controls.Add(New Label() With {
            .AutoSize = True, .Text = caption, .Font = Theme.UiFont(8.5F),
            .ForeColor = Theme.TextMuted, .Margin = New Padding(0)})
        p.Controls.Add(New Label() With {
            .AutoSize = True, .Text = value, .Font = Theme.UiFont(10.0F, FontStyle.Bold),
            .ForeColor = Theme.TextPrimary, .Margin = New Padding(0, 1, 0, 0)})
        Return p
    End Function

    ''' <summary>ตัวเลขเด่น + หน่วยตัวเล็กต่อท้าย</summary>
    Private Function BigStat(caption As String, value As String, unit As String, color As Color) As Control
        Dim p As New FlowLayoutPanel() With {
            .AutoSize = True, .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .FlowDirection = FlowDirection.TopDown, .WrapContents = False,
            .Margin = New Padding(0, 0, 0, 6), .Padding = New Padding(0)}
        p.Controls.Add(New Label() With {
            .AutoSize = True, .Text = caption, .Font = Theme.UiFont(8.5F),
            .ForeColor = Theme.TextMuted, .Margin = New Padding(0)})

        Dim row As New FlowLayoutPanel() With {
            .AutoSize = True, .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .WrapContents = False, .Margin = New Padding(0), .Padding = New Padding(0)}
        row.Controls.Add(New Label() With {
            .AutoSize = True, .Text = value, .Font = Theme.UiFont(15.0F, FontStyle.Bold),
            .ForeColor = color, .Margin = New Padding(0)})
        row.Controls.Add(New Label() With {
            .AutoSize = True, .Text = unit, .Font = Theme.UiFont(8.5F),
            .ForeColor = Theme.TextMuted, .Margin = New Padding(3, 10, 0, 0)})
        p.Controls.Add(row)
        Return p
    End Function

    Private Function Note(text As String, color As Color) As Control
        Return New Label() With {
            .AutoSize = True, .MaximumSize = New Size(Theme.Scale(Me, 250), 0),
            .Text = text, .Font = Theme.UiFont(8.5F), .ForeColor = color,
            .Margin = New Padding(0, 4, 0, 4)}
    End Function

    Private Shared Function SmallChip(text As String, level As AlertLevel) As StatusChip
        Return New StatusChip(text, level) With {.Compact = True, .Margin = New Padding(0, 0, 6, 6)}
    End Function

End Class

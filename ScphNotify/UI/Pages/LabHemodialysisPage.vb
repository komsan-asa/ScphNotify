Imports System.IO
Imports System.Text

''' <summary>
''' LAB Template Hemodialysis — ชุดผล LAB มาตรฐานของผู้ป่วยฟอกไต
''' ให้ผลเหมือนหน้า QHis2 emr_html/fcontent_lab_hd.php
'''   - แถว = รายการ LAB ตามเทมเพลตใน appsettings.json (LabHemodialysis.Items)
'''   - คอลัมน์ = วันที่รายงานผล ล่าสุดไม่เกิน LabHemodialysis.MaxDates วัน (ค่าเริ่มต้น 10)
'''   - ผลหลายค่าในวันเดียวกัน (เช่น BUN ก่อน/หลังฟอก) แสดงคั่นด้วย " | " เหมือนของเดิม
''' </summary>
Public Class LabHemodialysisPage
    Implements IPatientPage

    Private _boundKey As String
    Private _snap As PatientSnapshot
    Private _dates As New List(Of Date)
    Private _rows As New List(Of LabTemplateRow)
    Private _busy As Boolean

    Public Sub New()
        InitializeComponent()
        GridStyler.Apply(dgvLab)
        tip.SetToolTip(btnExport, "บันทึกเป็นไฟล์ .csv ที่เปิดด้วย Excel ได้")
        tip.SetToolTip(btnReset, "ล้างช่วงวันที่ แล้วกลับไปแสดงผลตรวจล่าสุด")
        tip.SetToolTip(btnPrint, "ตัวอย่างก่อนพิมพ์ใบ LAB Hemodialysis (รูปแบบเดียวกับ PDF ของ QHis2)")
    End Sub

    Public Sub ResetBinding() Implements IPatientPage.ResetBinding
        _boundKey = Nothing
    End Sub

    Public Sub BindPatient(snap As PatientSnapshot) Implements IPatientPage.BindPatient
        Dim key = If(snap Is Nothing, "", $"{snap.Hn}|{snap.LoadedAt.Ticks}")
        If key = _boundKey Then Return
        Dim patientChanged = _snap Is Nothing OrElse snap Is Nothing OrElse _snap.Hn <> snap.Hn
        _boundKey = key
        _snap = snap
        If snap Is Nothing Then Return
        If patientChanged Then ClearDateRange()
        LoadAsync()
    End Sub

    '──────────────── ตัวกรองช่วงวันที่ ────────────────

    Private Sub ClearDateRange()
        dtpFrom.Checked = False
        dtpTo.Checked = False
        dtpFrom.Value = Date.Today.AddYears(-1)
        dtpTo.Value = Date.Today
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        ClearDateRange()
        LoadAsync()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        LoadAsync()
    End Sub

    '──────────────── โหลดข้อมูล ────────────────

    Private Async Sub LoadAsync()
        If _busy OrElse _snap Is Nothing Then Return
        Dim cfg = AppConfig.Current.LabHemodialysis
        Dim items = cfg.ItemList()
        If items.Count = 0 Then
            ShowEmpty("ยังไม่ได้ตั้งค่าเทมเพลต", "กำหนดรายการ LAB ได้ที่ appsettings.json ส่วน LabHemodialysis.Items")
            Return
        End If

        Dim hn = _snap.Hn
        Dim fromDate As Date? = If(dtpFrom.Checked, CType(dtpFrom.Value.Date, Date?), Nothing)
        Dim toDate As Date? = If(dtpTo.Checked, CType(dtpTo.Value.Date, Date?), Nothing)
        If fromDate.HasValue AndAlso toDate.HasValue AndAlso fromDate.Value > toDate.Value Then
            Dim t = fromDate : fromDate = toDate : toDate = t
        End If

        _busy = True
        btnShow.Enabled = False
        lblInfo.Text = "กำลังโหลดผล LAB..."
        lblInfo.ForeColor = Theme.TextSecondary
        Try
            _dates = Await AppSession.Repository.GetLabTemplateDatesAsync(hn, items, fromDate, toDate, cfg.MaxDates)
            If _snap Is Nothing OrElse _snap.Hn <> hn Then Return

            If _dates.Count = 0 Then
                _rows.Clear()
                ShowEmpty("ไม่พบผล LAB", "ไม่พบผลตรวจของรายการในเทมเพลตฟอกไตในช่วงที่เลือก")
                lblInfo.Text = "ไม่พบข้อมูล"
                Return
            End If

            Dim results = Await AppSession.Repository.GetLabTemplateResultsAsync(hn, items, _dates.First(), _dates.Last())
            If _snap Is Nothing OrElse _snap.Hn <> hn Then Return

            BuildRows(items, results)
            FillGrid()
            lblInfo.Text = $"{ThaiDate.MediumDate(_dates.First())} – {ThaiDate.MediumDate(_dates.Last())}  ·  {_dates.Count} ครั้ง  ·  {_rows.Count} รายการที่มีผล"
        Catch ex As Exception
            ShowEmpty("โหลดผล LAB ไม่สำเร็จ", ex.Message)
            lblInfo.Text = "โหลดข้อมูลไม่สำเร็จ"
            lblInfo.ForeColor = Theme.Danger
            ErrorDialog.ShowError(FindForm(), "โหลด LAB Template Hemodialysis ไม่สำเร็จ", ex)
        Finally
            _busy = False
            btnShow.Enabled = True
        End Try
    End Sub

    ''' <summary>จัดผลลงตาราง: แถวตามลำดับเทมเพลต คอลัมน์ตามวันที่ ค่าซ้ำวันเดียวกันต่อด้วย " | "</summary>
    Private Sub BuildRows(items As List(Of String), results As List(Of LabPoint))
        Dim dateIndex As New Dictionary(Of Date, Integer)
        For i = 0 To _dates.Count - 1
            dateIndex(_dates(i)) = i
        Next

        Dim byName As New Dictionary(Of String, LabTemplateRow)(StringComparer.OrdinalIgnoreCase)
        _rows = New List(Of LabTemplateRow)
        For i = 0 To items.Count - 1
            Dim row As New LabTemplateRow With {.Index = i + 1, .LabName = items(i), .Values = New String(_dates.Count - 1) {}}
            If Not byName.ContainsKey(items(i)) Then byName(items(i)) = row
            _rows.Add(row)
        Next

        For Each p In results
            If Not p.ReportDate.HasValue Then Continue For
            Dim col As Integer
            If Not dateIndex.TryGetValue(p.ReportDate.Value.Date, col) Then Continue For
            Dim row As LabTemplateRow = Nothing
            If Not byName.TryGetValue(If(p.LabName, "").Trim(), row) Then Continue For
            Dim v = If(p.ResultText, "").Trim()
            If v = "" Then Continue For
            row.Values(col) = If(String.IsNullOrWhiteSpace(row.Values(col)), v, row.Values(col) & " | " & v)
        Next

        ' ซ่อนรายการที่ไม่มีผลเลย เหมือนหน้าเว็บเดิม
        _rows = _rows.Where(Function(r) r.HasAny).ToList()
    End Sub

    Private Sub FillGrid()
        dgvLab.SuspendLayout()
        Try
            dgvLab.Rows.Clear()
            dgvLab.Columns.Clear()
            dgvLab.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            Dim colIdx = GridStyler.AddTextColumn(dgvLab, "colIdx", "#", 100, DataGridViewContentAlignment.MiddleCenter, 44)
            colIdx.Frozen = True
            colIdx.Width = Theme.Scale(dgvLab, 52)
            Dim colName = GridStyler.AddTextColumn(dgvLab, "colLab", "รายการ LAB", 100, DataGridViewContentAlignment.MiddleLeft, 170)
            colName.Frozen = True
            colName.Width = Theme.Scale(dgvLab, 210)
            For i = 0 To _dates.Count - 1
                Dim c = GridStyler.AddTextColumn(dgvLab, "colD" & i.ToString(Globalization.CultureInfo.InvariantCulture),
                                                 ThaiDate.MediumDate(_dates(i)), 100, DataGridViewContentAlignment.MiddleRight, 96)
                c.Width = Theme.Scale(dgvLab, 112)
            Next

            For Each r In _rows
                Dim cells As New List(Of Object) From {r.Index, r.LabName}
                For i = 0 To _dates.Count - 1
                    cells.Add(If(r.Values(i), ""))
                Next
                dgvLab.Rows.Add(cells.ToArray())
            Next
            dgvLab.ClearSelection()
        Finally
            dgvLab.ResumeLayout()
        End Try
        dgvLab.Visible = True
        emptyLab.Visible = False
    End Sub

    Private Sub ShowEmpty(title As String, description As String)
        dgvLab.Visible = False
        emptyLab.Visible = True
        emptyLab.Title = title
        emptyLab.Description = description
    End Sub

    Private Sub dgvLab_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvLab.CellFormatting
        If e.ColumnIndex = 0 Then
            e.CellStyle.ForeColor = Theme.TextMuted
        ElseIf e.ColumnIndex = 1 Then
            e.CellStyle.Font = Theme.UiFont(9.75F, FontStyle.Bold)
            e.CellStyle.ForeColor = Theme.TextPrimary
        ElseIf String.IsNullOrWhiteSpace(Convert.ToString(e.Value)) Then
            e.CellStyle.BackColor = Theme.SurfaceAlt
        End If
    End Sub

    '──────────────── พิมพ์ ────────────────

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If _rows.Count = 0 OrElse _dates.Count = 0 OrElse _snap Is Nothing Then
            lblInfo.Text = "ยังไม่มีข้อมูลให้พิมพ์ — กด ""ค้นหาข้อมูล"" ก่อน"
            lblInfo.ForeColor = Theme.Danger
            Return
        End If
        LabHemodialysisPrinter.Preview(FindForm(), _snap, _dates, _rows)
    End Sub

    '──────────────── ส่งออก ────────────────

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If _rows.Count = 0 OrElse _snap Is Nothing Then
            lblInfo.Text = "ยังไม่มีข้อมูลให้ส่งออก"
            lblInfo.ForeColor = Theme.Danger
            Return
        End If
        Using dlg As New SaveFileDialog()
            dlg.Title = "ส่งออก LAB Template Hemodialysis"
            dlg.Filter = "ไฟล์ CSV เปิดด้วย Excel (*.csv)|*.csv"
            dlg.FileName = $"LAB_Hemodialysis_{_snap.Hn}_{Date.Today:yyyyMMdd}.csv"
            If dlg.ShowDialog(FindForm()) <> DialogResult.OK Then Return
            Try
                File.WriteAllText(dlg.FileName, BuildCsv(), New UTF8Encoding(True))   ' BOM เพื่อให้ Excel อ่านภาษาไทยถูก
                lblInfo.Text = $"ส่งออกแล้ว → {dlg.FileName}"
                lblInfo.ForeColor = Theme.TextSecondary
            Catch ex As Exception
                ErrorDialog.ShowError(FindForm(), "ส่งออกไฟล์ไม่สำเร็จ", ex)
            End Try
        End Using
    End Sub

    Private Function BuildCsv() As String
        Dim sb As New System.Text.StringBuilder()
        sb.AppendLine($"LAB Hemodialysis HN : {_snap.Hn}  ชื่อ : {_snap.PatientName}")
        sb.AppendLine()
        Dim header As New List(Of String) From {"#", "รายการ LAB"}
        header.AddRange(_dates.Select(Function(d) d.ToString("yyyy-MM-dd", Globalization.CultureInfo.InvariantCulture)))
        sb.AppendLine(String.Join(",", header.Select(AddressOf Csv)))
        For Each r In _rows
            Dim line As New List(Of String) From {r.Index.ToString(Globalization.CultureInfo.InvariantCulture), r.LabName}
            For i = 0 To _dates.Count - 1
                line.Add(If(r.Values(i), ""))
            Next
            sb.AppendLine(String.Join(",", line.Select(AddressOf Csv)))
        Next
        Return sb.ToString()
    End Function

    Private Shared Function Csv(value As String) As String
        Dim s = If(value, "")
        If s.IndexOfAny({","c, """"c, Convert.ToChar(13), Convert.ToChar(10)}) >= 0 Then
            Return """" & s.Replace("""", """""") & """"
        End If
        Return s
    End Function

End Class

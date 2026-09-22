Imports System.IO
Imports System.Text

''' <summary>
''' ทะเบียนคัดกรองโรคจากการทำงาน — ใหม่ในเวอร์ชัน 2026
''' โปรแกรมเดิมบันทึกลงตาราง opdscreen_occupational อย่างเดียว ไม่มีหน้าดูย้อนหลัง
''' </summary>
Public Class OccupationalRegistryPage

    Private Const MaxRows As Integer = 5000

    Private _rows As New List(Of OccupationalRecord)
    Private _busy As Boolean
    Private _loaded As Boolean
    Private _suspendRange As Boolean

    Public Sub New()
        InitializeComponent()
        GridStyler.Apply(dgvRegistry)
        GridStyler.AddTextColumn(dgvRegistry, "colDate", "วันที่-เวลา", 16, DataGridViewContentAlignment.MiddleLeft, 120)
        GridStyler.AddTextColumn(dgvRegistry, "colHn", "HN", 11, DataGridViewContentAlignment.MiddleLeft, 80)
        GridStyler.AddTextColumn(dgvRegistry, "colVn", "VN", 13, DataGridViewContentAlignment.MiddleLeft, 95)
        GridStyler.AddTextColumn(dgvRegistry, "colName", "ชื่อ-สกุล", 22, DataGridViewContentAlignment.MiddleLeft, 140)
        GridStyler.AddTextColumn(dgvRegistry, "colAge", "อายุ", 7, DataGridViewContentAlignment.MiddleRight, 52)
        GridStyler.AddTextColumn(dgvRegistry, "colQ1", "เกิดจากงาน", 9, DataGridViewContentAlignment.MiddleCenter, 72)
        GridStyler.AddTextColumn(dgvRegistry, "colQ2", "เป็นมากขึ้น", 9, DataGridViewContentAlignment.MiddleCenter, 72)
        GridStyler.AddTextColumn(dgvRegistry, "colQ3", "PM2.5", 8, DataGridViewContentAlignment.MiddleCenter, 66)
        GridStyler.AddTextColumn(dgvRegistry, "colResult", "ผลสรุป", 12, DataGridViewContentAlignment.MiddleLeft, 96)
        GridStyler.AddTextColumn(dgvRegistry, "colStaff", "ผู้บันทึก", 11, DataGridViewContentAlignment.MiddleLeft, 80)

        tip.SetToolTip(btnExport, "บันทึกเป็นไฟล์ .csv ที่เปิดด้วย Excel ได้")
        ApplyQuickRange(1)
    End Sub

    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
        MyBase.OnVisibleChanged(e)
        ' โหลดอัตโนมัติครั้งแรกที่เปิดหน้านี้
        If Visible AndAlso Not _loaded AndAlso Not DesignMode Then
            _loaded = True
            LoadRegistryAsync()
        End If
    End Sub

    '──────────────── ตัวกรอง ────────────────

    Private Sub ApplyQuickRange(index As Integer)
        Dim today = Date.Today
        Dim from As Date
        Select Case index
            Case 0 : from = today                      ' วันนี้
            Case 2 : from = today.AddDays(-29)         ' 30 วัน
            Case Else : from = today.AddDays(-6)       ' 7 วัน
        End Select
        _suspendRange = True
        Try
            dtpFrom.Value = from
            dtpTo.Value = today
        Finally
            _suspendRange = False
        End Try
    End Sub

    Private Sub segRange_SelectedIndexChanged(sender As Object, e As EventArgs) Handles segRange.SelectedIndexChanged
        ApplyQuickRange(segRange.SelectedIndex)
        LoadRegistryAsync()
    End Sub

    Private Sub dtpFrom_ValueChanged(sender As Object, e As EventArgs) Handles dtpFrom.ValueChanged, dtpTo.ValueChanged
        If _suspendRange Then Return
        segRange.SelectedIndex = -1
    End Sub

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            LoadRegistryAsync()
        End If
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        LoadRegistryAsync()
    End Sub

    '──────────────── โหลดข้อมูล ────────────────

    Private Async Sub LoadRegistryAsync()
        If _busy Then Return
        Dim d1 = dtpFrom.Value.Date
        Dim d2 = dtpTo.Value.Date
        If d1 > d2 Then
            Dim t = d1 : d1 = d2 : d2 = t
        End If

        _busy = True
        btnShow.Enabled = False
        ShowInfo("กำลังโหลดทะเบียน...", False)
        Try
            _rows = Await AppSession.Repository.GetOccupationalRegistryAsync(d1, d2, txtSearch.Text, MaxRows)
            FillGrid()
            FillSummary()
            Dim rangeText = $"{ThaiDate.MediumDate(d1)} – {ThaiDate.MediumDate(d2)}"
            If _rows.Count >= MaxRows Then
                ShowInfo($"{rangeText}  ·  แสดง {MaxRows:N0} รายการแรก (ข้อมูลมากกว่านี้ กรุณาย่อช่วงวันที่)", True)
            Else
                ShowInfo($"{rangeText}  ·  พบ {_rows.Count:N0} รายการ", False)
            End If
        Catch ex As Exception
            ShowInfo("โหลดทะเบียนไม่สำเร็จ", True)
            ErrorDialog.ShowError(FindForm(), "โหลดทะเบียนคัดกรองไม่สำเร็จ", ex)
        Finally
            _busy = False
            btnShow.Enabled = True
            UpdateView()
        End Try
    End Sub

    Private Sub FillGrid()
        dgvRegistry.Rows.Clear()
        For Each r In _rows
            dgvRegistry.Rows.Add(
                If(r.ScreenedAt.HasValue, $"{ThaiDate.MediumDate(r.ScreenedAt.Value)}  {r.ScreenedAt.Value:HH:mm}", "-"),
                r.Hn, r.Vn, r.PatientName, r.AgeText,
                OccupationalRecord.YesNo(r.Injury),
                OccupationalRecord.YesNo(r.MoreSymptoms),
                OccupationalRecord.YesNo(r.DustPm),
                If(r.NeedsReferral, $"ส่งต่อ ({r.PositiveCount})", "ปกติ"),
                r.Staff)
        Next
        dgvRegistry.ClearSelection()
    End Sub

    Private Sub FillSummary()
        Dim s = OccupationalSummary.FromRecords(_rows)
        lblTotal.Text = $"{s.Total:N0}"
        lblReferral.Text = $"{s.Referral:N0}"
        lblInjury.Text = $"{s.Injury:N0}"
        lblDust.Text = $"{s.DustPm:N0}"
        cardTotal.HeaderRightText = ""
        cardReferral.HeaderRightText = If(s.Total > 0, $"{s.Referral * 100.0 / s.Total:0.#}%", "")
        cardInjury.HeaderRightText = If(s.Total > 0, $"{s.Injury * 100.0 / s.Total:0.#}%", "")
        cardDust.HeaderRightText = If(s.Total > 0, $"{s.DustPm * 100.0 / s.Total:0.#}%", "")
    End Sub

    Private Sub ShowInfo(text As String, warn As Boolean)
        lblInfo.Text = text
        lblInfo.ForeColor = If(warn, Theme.Danger, Theme.TextSecondary)
    End Sub

    Private Sub UpdateView()
        Dim hasRows = _rows.Count > 0
        dgvRegistry.Visible = hasRows
        emptyRegistry.Visible = Not hasRows
        If Not hasRows Then
            emptyRegistry.Title = "ไม่พบผลคัดกรองในช่วงที่เลือก"
            emptyRegistry.Description = "ลองขยายช่วงวันที่ หรือล้างคำค้นแล้วกด ""แสดงทะเบียน"" อีกครั้ง"
        End If
    End Sub

    Private Sub dgvRegistry_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvRegistry.CellFormatting
        If e.RowIndex < 0 OrElse e.RowIndex >= _rows.Count Then Return
        Dim r = _rows(e.RowIndex)
        Dim name = dgvRegistry.Columns(e.ColumnIndex).Name
        Select Case name
            Case "colQ1", "colQ2", "colQ3"
                Dim yes = (name = "colQ1" AndAlso r.Injury) OrElse
                          (name = "colQ2" AndAlso r.MoreSymptoms) OrElse
                          (name = "colQ3" AndAlso r.DustPm)
                e.CellStyle.ForeColor = If(yes, Theme.Danger, Theme.TextMuted)
                If yes Then e.CellStyle.Font = Theme.UiFont(9.75F, FontStyle.Bold)
            Case "colResult"
                If r.NeedsReferral Then
                    e.CellStyle.ForeColor = Theme.DangerText
                    e.CellStyle.BackColor = Theme.DangerSoft
                    e.CellStyle.Font = Theme.UiFont(9.75F, FontStyle.Bold)
                Else
                    e.CellStyle.ForeColor = Theme.TextSecondary
                End If
        End Select
    End Sub

    '──────────────── ส่งออก ────────────────

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If _rows.Count = 0 Then
            ShowInfo("ยังไม่มีข้อมูลให้ส่งออก — กด ""แสดงทะเบียน"" ก่อน", True)
            Return
        End If
        Using dlg As New SaveFileDialog()
            dlg.Title = "ส่งออกทะเบียนคัดกรองโรคจากการทำงาน"
            dlg.Filter = "ไฟล์ CSV เปิดด้วย Excel (*.csv)|*.csv"
            dlg.FileName = $"ทะเบียนคัดกรองอาชีวอนามัย_{dtpFrom.Value:yyyyMMdd}-{dtpTo.Value:yyyyMMdd}.csv"
            If dlg.ShowDialog(FindForm()) <> DialogResult.OK Then Return
            Try
                File.WriteAllText(dlg.FileName, BuildCsv(), New UTF8Encoding(True))   ' BOM เพื่อให้ Excel อ่านภาษาไทยถูก
                ShowInfo($"ส่งออกแล้ว {_rows.Count:N0} รายการ → {dlg.FileName}", False)
            Catch ex As Exception
                ErrorDialog.ShowError(FindForm(), "ส่งออกไฟล์ไม่สำเร็จ", ex)
            End Try
        End Using
    End Sub

    Private Function BuildCsv() As String
        Dim sb As New System.Text.StringBuilder()
        sb.AppendLine(String.Join(",", {"วันที่", "เวลา", "HN", "VN", "ชื่อ-สกุล", "อายุ",
                                        "เกิดจากการทำงาน", "อาการเป็นมากขึ้น", "ฝุ่น PM2.5", "ผลสรุป", "ผู้บันทึก"}))
        For Each r In _rows
            sb.AppendLine(String.Join(",", {
                Csv(If(r.ScreenedAt.HasValue, ThaiDate.MediumDate(r.ScreenedAt.Value), "")),
                Csv(If(r.ScreenedAt.HasValue, r.ScreenedAt.Value.ToString("HH:mm"), "")),
                Csv(r.Hn), Csv(r.Vn), Csv(r.PatientName),
                Csv(If(r.AgeYears.HasValue, r.AgeYears.Value.ToString(), "")),
                Csv(OccupationalRecord.YesNo(r.Injury)),
                Csv(OccupationalRecord.YesNo(r.MoreSymptoms)),
                Csv(OccupationalRecord.YesNo(r.DustPm)),
                Csv(If(r.NeedsReferral, "ส่งต่อคลินิกโรคจากการทำงาน", "ปกติ")),
                Csv(r.Staff)}))
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

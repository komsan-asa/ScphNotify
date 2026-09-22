''' <summary>หน้าผล LAB (แทน dialogLab เดิม): ค้นหา, ตาราง, กราฟ, พิมพ์</summary>
Public Class LabPage
    Implements IPatientPage

    Private Const PlaceholderItem As String = "— เลือกรายการ LAB —"

    Private _boundKey As String
    Private _snap As PatientSnapshot
    Private _rows As New List(Of LabResultRow)
    Private _points As New List(Of LabPoint)
    Private _shownLab As String = ""
    Private _shownYears As String = ""
    Private _busy As Boolean

    Public Sub New()
        InitializeComponent()
        GridStyler.Apply(dgvLab)
        GridStyler.AddTextColumn(dgvLab, "colOrder", "เลขที่สั่ง", 13)
        GridStyler.AddTextColumn(dgvLab, "colDate", "วันที่รายงาน", 15)
        GridStyler.AddTextColumn(dgvLab, "colItem", "รายการ", 20)
        GridStyler.AddTextColumn(dgvLab, "colResult", "ผล", 11, DataGridViewContentAlignment.MiddleRight)
        GridStyler.AddTextColumn(dgvLab, "colNormal", "ค่าปกติ", 16)
        GridStyler.AddTextColumn(dgvLab, "colFlag", "Flag", 8, DataGridViewContentAlignment.MiddleCenter, 50)
        GridStyler.AddTextColumn(dgvLab, "colGroup", "กลุ่ม", 14)
        chartLab.EmptyText = "ไม่มีผลที่เป็นตัวเลขสำหรับแสดงกราฟ"
    End Sub

    Public Sub ResetBinding() Implements IPatientPage.ResetBinding
        _boundKey = Nothing
    End Sub

    Public Sub BindPatient(snap As PatientSnapshot) Implements IPatientPage.BindPatient
        Dim key = If(snap Is Nothing, "", $"{snap.Vn}|{snap.LoadedAt.Ticks}")
        If key = _boundKey Then Return
        Dim patientChanged = _snap Is Nothing OrElse snap Is Nothing OrElse _snap.Vn <> snap.Vn
        _boundKey = key
        _snap = snap

        ' Ferritin (ตามการตั้งค่า cbFerritin)
        Dim showFerritin = AppSession.Display.ShowFerritin
        cardFerritin.Visible = showFerritin
        tlpTop.ColumnStyles(0).Width = If(showFerritin, 72.0F, 100.0F)
        tlpTop.ColumnStyles(1).Width = If(showFerritin, 28.0F, 0.0F)
        cardFilter.Margin = New Padding(0, 0, If(showFerritin, 14, 0), 12)
        lstFerritin.Items.Clear()
        For Each f In If(snap?.FerritinResults, New List(Of String))
            lstFerritin.Items.Add(New ListEntry(f.Replace("(", "·  (")))
        Next
        lstFerritin.Visible = lstFerritin.Items.Count > 0
        lblNoFerritin.Visible = Not lstFerritin.Visible

        If snap Is Nothing Then Return
        If patientChanged Then
            ResetResults()
            txtSearch.Text = ""
            FillYears()
            LoadLabListAsync()
        End If
    End Sub

    Private Sub FillYears()
        Dim years = AppSession.LabYears
        cmbYearFrom.Items.Clear()
        cmbYearTo.Items.Clear()
        For Each y In years
            cmbYearFrom.Items.Add(y)
            cmbYearTo.Items.Add(y)
        Next
        If years.Count > 0 Then
            cmbYearFrom.SelectedIndex = 0
            cmbYearTo.SelectedIndex = years.Count - 1
        End If
    End Sub

    Private Sub ResetResults()
        _rows.Clear()
        _points.Clear()
        _shownLab = ""
        dgvLab.Rows.Clear()
        chartLab.SetData(Nothing)
        lblResultInfo.Text = ""
        lblResultInfo.ForeColor = Theme.TextSecondary
        UpdateView()
    End Sub

    Private Async Sub LoadLabListAsync()
        If _snap Is Nothing Then Return
        Dim hn = _snap.Hn
        Try
            cmbLab.Enabled = False
            Dim items = Await AppSession.Repository.GetLabItemNamesAsync(txtSearch.Text, hn)
            If _snap Is Nothing OrElse _snap.Hn <> hn Then Return
            cmbLab.BeginUpdate()
            cmbLab.Items.Clear()
            cmbLab.Items.Add(PlaceholderItem)
            For Each n In items
                cmbLab.Items.Add(n)
            Next
            cmbLab.SelectedIndex = If(items.Count = 1, 1, 0)
            cmbLab.EndUpdate()
            If txtSearch.Text.Trim() <> "" Then
                ShowInfo($"พบ {items.Count} รายการที่ตรงกับ ""{txtSearch.Text.Trim()}""", False)
            End If
        Catch ex As Exception
            ShowInfo("โหลดรายการ LAB ไม่สำเร็จ: " & ex.Message, True)
        Finally
            cmbLab.Enabled = True
        End Try
    End Sub

    Private Sub ShowInfo(text As String, isError As Boolean)
        lblResultInfo.Text = text
        lblResultInfo.ForeColor = If(isError, Theme.Danger, Theme.TextSecondary)
    End Sub

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            LoadLabListAsync()
        End If
    End Sub

    Private Sub cmbLab_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbLab.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            btnShow.PerformClick()
        End If
    End Sub

    Private Async Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        If _busy OrElse _snap Is Nothing Then Return
        Dim lab = If(cmbLab.SelectedIndex > 0, Convert.ToString(cmbLab.SelectedItem), cmbLab.Text.Trim())
        If lab = "" OrElse lab = PlaceholderItem Then
            ShowInfo("กรุณาเลือกรายการ LAB ก่อน", True)
            cmbLab.Focus()
            Return
        End If
        If cmbYearFrom.SelectedItem Is Nothing OrElse cmbYearTo.SelectedItem Is Nothing Then Return
        Dim y1 = CInt(cmbYearFrom.SelectedItem)
        Dim y2 = CInt(cmbYearTo.SelectedItem)
        If y1 > y2 Then
            Dim t = y1 : y1 = y2 : y2 = t
        End If

        _busy = True
        btnShow.Enabled = False
        ShowInfo("กำลังโหลด...", False)
        Try
            Dim hn = _snap.Hn
            Dim repo = AppSession.Repository
            _rows = Await repo.GetLabResultsAsync(hn, y1, y2, lab)
            _points = Await repo.GetLabChartAsync(hn, y1, y2, lab)
            _shownLab = lab
            _shownYears = $"{y1}–{y2}"
            FillGrid()
            FillChart()
            ShowInfo($"{lab}  ·  พบ {_rows.Count} รายการ  ·  ปี พ.ศ. {_shownYears}", False)
        Catch ex As Exception
            ShowInfo("โหลดผล LAB ไม่สำเร็จ", True)
            ErrorDialog.ShowError(FindForm(), "โหลดผล LAB ไม่สำเร็จ", ex)
        Finally
            _busy = False
            btnShow.Enabled = True
            UpdateView()
        End Try
    End Sub

    Private Sub FillGrid()
        dgvLab.Rows.Clear()
        For Each r In _rows
            dgvLab.Rows.Add(r.OrderNumber, ThaiDate.MediumDate(r.ReportDate), r.ItemName, r.Result, r.NormalValue, r.Flag, r.SubGroup)
        Next
        dgvLab.ClearSelection()
    End Sub

    Private Sub FillChart()
        chartLab.SetData(_points.Where(Function(p) p.NumericValue.HasValue).
                                 OrderBy(Function(p) p.ReportDate.GetValueOrDefault()).
                                 Select(Function(p) New ChartPoint(If(p.ReportDate.HasValue, ThaiDate.ShortDate(p.ReportDate.Value), "-"),
                                                                   p.NumericValue.Value, p.LabName)))
    End Sub

    Private Sub UpdateView()
        Dim hasData = _shownLab <> ""
        emptyLab.Visible = Not hasData
        dgvLab.Visible = hasData AndAlso segView.SelectedIndex = 0
        chartLab.Visible = hasData AndAlso segView.SelectedIndex = 1
        If hasData AndAlso _rows.Count = 0 AndAlso segView.SelectedIndex = 0 Then
            dgvLab.Visible = False
            emptyLab.Visible = True
            emptyLab.Title = "ไม่พบผล LAB"
            emptyLab.Description = $"ไม่พบผล {_shownLab} ในช่วงปี {_shownYears}"
        Else
            emptyLab.Title = "ยังไม่ได้เลือกรายการ LAB"
            emptyLab.Description = "เลือกรายการ LAB และช่วงปี แล้วกด ""แสดงผล"""
        End If
    End Sub

    Private Sub segView_SelectedIndexChanged(sender As Object, e As EventArgs) Handles segView.SelectedIndexChanged
        UpdateView()
    End Sub

    Private Sub dgvLab_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvLab.CellFormatting
        If e.RowIndex < 0 OrElse e.RowIndex >= _rows.Count Then Return
        Dim flag = _rows(e.RowIndex).Flag.Trim().ToUpperInvariant()
        Dim name = dgvLab.Columns(e.ColumnIndex).Name
        If (name = "colFlag" OrElse name = "colResult") AndAlso flag <> "" Then
            If flag.StartsWith("H") OrElse flag.Contains("CRIT") OrElse flag.StartsWith("A") Then
                e.CellStyle.ForeColor = Theme.Danger
                e.CellStyle.Font = Theme.UiFont(9.75F, FontStyle.Bold)
            ElseIf flag.StartsWith("L") Then
                e.CellStyle.ForeColor = Theme.Info
                e.CellStyle.Font = Theme.UiFont(9.75F, FontStyle.Bold)
            End If
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If _shownLab = "" OrElse (_rows.Count = 0 AndAlso _points.Count = 0) Then
            ShowInfo("กรุณากด ""แสดงผล"" ก่อนพิมพ์รายงาน", True)
            Return
        End If
        LabReportPrinter.Preview(FindForm(), _snap, _shownLab, _shownYears, _rows, chartLab)
    End Sub

End Class

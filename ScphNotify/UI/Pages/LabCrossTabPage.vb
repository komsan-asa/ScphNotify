''' <summary>
''' หน้า LAB Crosstab (แทน dialogLabCrossTab เดิม ซึ่งยังทำไม่เสร็จ — เลือกรายการได้แต่ยังไม่แสดงผล)
''' เวอร์ชันนี้สร้างตารางเทียบผลตามวันที่ให้ครบ
''' </summary>
Public Class LabCrossTabPage
    Implements IPatientPage

    Private _boundVn As String
    Private _snap As PatientSnapshot
    Private _allItems As New List(Of String)
    Private ReadOnly _checked As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    Private _suppressCheck As Boolean
    Private WithEvents _searchDelay As New System.Windows.Forms.Timer() With {.Interval = 250}

    Public Sub New()
        InitializeComponent()
        GridStyler.Apply(dgvCross)
        dgvCross.AllowUserToResizeColumns = True
    End Sub

    Public Sub ResetBinding() Implements IPatientPage.ResetBinding
        ' รายการที่เลือกไว้ไม่ต้องล้าง ยกเว้นเปลี่ยนผู้ป่วย
    End Sub

    Public Async Sub BindPatient(snap As PatientSnapshot) Implements IPatientPage.BindPatient
        Dim vn = If(snap?.Vn, "")
        If vn = _boundVn Then Return
        _boundVn = vn
        _snap = snap
        _checked.Clear()
        _allItems = New List(Of String)
        txtSearch.Text = ""
        ResetTable()
        FillList()
        If snap Is Nothing Then Return

        Try
            clbItems.Enabled = False
            Dim items = Await AppSession.Repository.GetLabItemNamesAsync("", snap.Hn)
            If _boundVn <> vn Then Return
            _allItems = items
            FillList()
        Catch ex As Exception
            cardItems.HeaderRightText = "โหลดไม่สำเร็จ"
        Finally
            clbItems.Enabled = True
        End Try
    End Sub

    Private Sub FillList()
        Dim q = txtSearch.Text.Trim()
        _suppressCheck = True
        clbItems.BeginUpdate()
        clbItems.Items.Clear()
        For Each n In _allItems
            If q = "" OrElse n.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0 Then
                clbItems.Items.Add(n, _checked.Contains(n))
            End If
        Next
        clbItems.EndUpdate()
        _suppressCheck = False
        cardItems.HeaderRightText = $"{_allItems.Count} รายการ"
        UpdateSelectedInfo()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        _searchDelay.Stop()
        _searchDelay.Start()
    End Sub

    Private Sub OnSearchDelay(sender As Object, e As EventArgs) Handles _searchDelay.Tick
        _searchDelay.Stop()
        FillList()
    End Sub

    Private Sub clbItems_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles clbItems.ItemCheck
        If _suppressCheck Then Return
        Dim name = Convert.ToString(clbItems.Items(e.Index))
        If e.NewValue = CheckState.Checked Then _checked.Add(name) Else _checked.Remove(name)
        UpdateSelectedInfo()
    End Sub

    Private Sub UpdateSelectedInfo()
        lblSelectedCount.Text = $"เลือก {_checked.Count} รายการ"
        btnBuild.Enabled = _checked.Count > 0
        flpSelected.SuspendLayout()
        For Each c As Control In flpSelected.Controls.Cast(Of Control)().ToList()
            c.Dispose()
        Next
        flpSelected.Controls.Clear()
        For Each n In _checked.OrderBy(Function(x) x)
            flpSelected.Controls.Add(New StatusChip(n, AlertLevel.Primary, IconKind.Flask) With {.Margin = New Padding(0, 0, 6, 6)})
        Next
        flpSelected.ResumeLayout()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        _checked.Clear()
        FillList()
        ResetTable()
    End Sub

    Private Sub ResetTable()
        dgvCross.Columns.Clear()
        dgvCross.Visible = False
        emptyCross.Visible = True
        emptyCross.Title = "ยังไม่ได้เลือกรายการ"
        emptyCross.Description = "ติ๊กเลือกรายการ LAB ทางซ้าย แล้วกด ""สร้างตาราง"""
        cardTable.HeaderRightText = ""
    End Sub

    Private Async Sub btnBuild_Click(sender As Object, e As EventArgs) Handles btnBuild.Click
        If _snap Is Nothing OrElse _checked.Count = 0 Then Return
        Dim labs = _checked.OrderBy(Function(x) x).ToList()
        btnBuild.Enabled = False
        Try
            Dim points = Await AppSession.Repository.GetLabHistoryAsync(_snap.Hn, labs)
            BuildTable(labs, points)
        Catch ex As Exception
            ErrorDialog.ShowError(FindForm(), "สร้างตาราง LAB ไม่สำเร็จ", ex)
        Finally
            btnBuild.Enabled = _checked.Count > 0
        End Try
    End Sub

    ''' <summary>pivot: แถว = วันที่รายงาน, คอลัมน์ = รายการ LAB</summary>
    Private Sub BuildTable(labs As List(Of String), points As List(Of LabPoint))
        dgvCross.SuspendLayout()
        dgvCross.Columns.Clear()
        GridStyler.AddTextColumn(dgvCross, "colDate", "วันที่รายงาน", 18, minWidth:=120)
        For Each lab In labs
            GridStyler.AddTextColumn(dgvCross, "lab_" & lab, lab, 14, DataGridViewContentAlignment.MiddleRight, 96)
        Next

        Dim byDate = points.GroupBy(Function(p) p.ReportDate.GetValueOrDefault().Date).
                            OrderByDescending(Function(g) g.Key)
        For Each grp In byDate
            Dim cells As New List(Of Object) From {If(grp.Key = Date.MinValue, "-", ThaiDate.MediumDate(grp.Key))}
            For Each lab In labs
                Dim vals = grp.Where(Function(p) String.Equals(p.LabName, lab, StringComparison.OrdinalIgnoreCase)).
                               Select(Function(p) p.ResultText.Trim()).Where(Function(v) v <> "").ToList()
                cells.Add(If(vals.Count = 0, "", String.Join(" / ", vals)))
            Next
            dgvCross.Rows.Add(cells.ToArray())
        Next
        dgvCross.ResumeLayout()
        dgvCross.ClearSelection()

        Dim hasRows = dgvCross.Rows.Count > 0
        dgvCross.Visible = hasRows
        emptyCross.Visible = Not hasRows
        If Not hasRows Then
            emptyCross.Title = "ไม่พบผล LAB"
            emptyCross.Description = "ไม่พบผลของรายการที่เลือกในประวัติผู้ป่วย"
        End If
        cardTable.HeaderRightText = $"{dgvCross.Rows.Count} วันที่ · {labs.Count} รายการ"
    End Sub

End Class

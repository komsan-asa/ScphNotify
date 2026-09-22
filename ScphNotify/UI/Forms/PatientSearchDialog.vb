''' <summary>
''' ค้นหาผู้ป่วยสำหรับโหมด Manual — ค้นด้วยชื่อ / ชื่อ-สกุล / HN / เลขบัตรประชาชน
''' ไม่จำกัดว่าผู้ป่วยต้องมารับบริการในวันนี้ (แต่จะบอกไว้ในคอลัมน์ "มารับบริการล่าสุด")
''' </summary>
Public Class PatientSearchDialog

    Private Const MaxRows As Integer = 200

    Private _rows As New List(Of PatientSearchResult)
    Private _busy As Boolean

    ''' <summary>HN ที่ผู้ใช้เลือก (ว่าง = ยกเลิก)</summary>
    Public ReadOnly Property SelectedHn As String = ""

    ''' <summary>เปิดหน้าค้นหา คืนค่า HN ที่เลือก หรือ "" เมื่อยกเลิก</summary>
    Public Shared Function Pick(owner As IWin32Window) As String
        Using dlg As New PatientSearchDialog()
            If dlg.ShowDialog(owner) = DialogResult.OK Then Return dlg.SelectedHn
        End Using
        Return ""
    End Function

    Public Sub New()
        InitializeComponent()
        GridStyler.Apply(dgvResult)
        GridStyler.AddTextColumn(dgvResult, "colHn", "HN", 14, DataGridViewContentAlignment.MiddleLeft, 90)
        GridStyler.AddTextColumn(dgvResult, "colName", "ชื่อ-สกุล", 36, DataGridViewContentAlignment.MiddleLeft, 170)
        GridStyler.AddTextColumn(dgvResult, "colAge", "อายุ", 10, DataGridViewContentAlignment.MiddleRight, 60)
        GridStyler.AddTextColumn(dgvResult, "colCid", "เลขบัตรประชาชน", 20, DataGridViewContentAlignment.MiddleLeft, 130)
        GridStyler.AddTextColumn(dgvResult, "colVisit", "มารับบริการล่าสุด", 20, DataGridViewContentAlignment.MiddleLeft, 130)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        Icon = AppSession.AppIcon
        txtSearch.Focus()
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        NativeMethods.TrySetCaption(Me, Theme.Surface, Theme.TextPrimary, Theme.Border)
    End Sub

    '──────────────── ค้นหา ────────────────

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SearchAsync()
        End If
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        SearchAsync()
    End Sub

    Private Async Sub SearchAsync()
        If _busy Then Return
        Dim q = txtSearch.Text.Trim()
        If q.Length < 2 Then
            ShowEmpty("พิมพ์อย่างน้อย 2 ตัวอักษร", "HN พิมพ์ไม่ครบหลักได้ (เติม 0 นำหน้าให้เอง) · เลขบัตรประชาชนต้องครบ 13 หลัก")
            Return
        End If

        _busy = True
        btnSearch.Enabled = False
        lblInfo.Text = "กำลังค้นหา..."
        Try
            _rows = Await AppSession.Repository.SearchPatientsAsync(q, MaxRows)
            FillGrid()
            If _rows.Count = 0 Then
                ShowEmpty("ไม่พบผู้ป่วย", NotFoundHint(q))
                lblInfo.Text = "ไม่พบข้อมูล"
            Else
                lblInfo.Text = If(_rows.Count >= MaxRows,
                                  $"แสดง {MaxRows} รายการแรก — พิมพ์ให้เจาะจงขึ้นเพื่อผลที่แม่นกว่า",
                                  $"พบ {_rows.Count} รายการ · ดับเบิลคลิกเพื่อเลือก")
            End If
        Catch ex As Exception
            ShowEmpty("ค้นหาไม่สำเร็จ", ex.Message)
            lblInfo.Text = "ค้นหาไม่สำเร็จ"
            ErrorDialog.ShowError(Me, "ค้นหาผู้ป่วยไม่สำเร็จ", ex)
        Finally
            _busy = False
            btnSearch.Enabled = True
        End Try
    End Sub

    ''' <summary>คำแนะนำเมื่อค้นไม่เจอ — บอกเงื่อนไขของ HN / เลขบัตรประชาชนให้ชัด</summary>
    Private Shared Function NotFoundHint(q As String) As String
        Dim digits = New String(q.Where(AddressOf Char.IsDigit).ToArray())
        If digits.Length > 0 AndAlso digits.Length = q.Length Then
            If digits.Length = 13 Then
                Return "ไม่พบผู้ป่วยที่มี HN หรือเลขบัตรประชาชนตรงกับ " & q
            End If
            Return "ไม่พบผู้ป่วย HN ที่ตรงกับ " & q & " (ถ้าค้นด้วยเลขบัตรประชาชน ต้องพิมพ์ให้ครบ 13 หลัก)"
        End If
        Return "ไม่พบผู้ป่วยที่ตรงกับ " & q & " — ลองพิมพ์เฉพาะชื่อ หรือเฉพาะนามสกุล"
    End Function

    Private Sub FillGrid()
        dgvResult.Rows.Clear()
        For Each p In _rows
            dgvResult.Rows.Add(p.Hn, p.Name, p.AgeText, p.MaskedCid,
                               If(p.LastVisitDate.HasValue,
                                  If(p.VisitedToday, "วันนี้", ThaiDate.MediumDate(p.LastVisitDate.Value)),
                                  "ไม่เคยมารับบริการ"))
        Next
        dgvResult.ClearSelection()
        dgvResult.Visible = _rows.Count > 0
        emptyResult.Visible = _rows.Count = 0
        UpdateSelectButton()
    End Sub

    Private Sub ShowEmpty(title As String, description As String)
        _rows.Clear()
        dgvResult.Rows.Clear()
        dgvResult.Visible = False
        emptyResult.Visible = True
        emptyResult.Title = title
        emptyResult.Description = description
        UpdateSelectButton()
    End Sub

    Private Sub dgvResult_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvResult.CellFormatting
        If e.RowIndex < 0 OrElse e.RowIndex >= _rows.Count Then Return
        If dgvResult.Columns(e.ColumnIndex).Name <> "colVisit" Then Return
        Dim p = _rows(e.RowIndex)
        If p.VisitedToday Then
            e.CellStyle.ForeColor = Theme.Success
            e.CellStyle.Font = Theme.UiFont(9.75F, FontStyle.Bold)
        ElseIf Not p.LastVisitDate.HasValue Then
            e.CellStyle.ForeColor = Theme.TextMuted
        End If
    End Sub

    '──────────────── เลือก ────────────────

    Private ReadOnly Property Selected As PatientSearchResult
        Get
            Dim i = If(dgvResult.CurrentRow IsNot Nothing, dgvResult.CurrentRow.Index, -1)
            If i < 0 OrElse i >= _rows.Count Then Return Nothing
            Return _rows(i)
        End Get
    End Property

    Private Sub UpdateSelectButton()
        btnSelect.Enabled = Selected IsNot Nothing
    End Sub

    Private Sub dgvResult_SelectionChanged(sender As Object, e As EventArgs) Handles dgvResult.SelectionChanged
        UpdateSelectButton()
    End Sub

    Private Sub dgvResult_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvResult.CellDoubleClick
        If e.RowIndex >= 0 Then Accept()
    End Sub

    Private Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click
        Accept()
    End Sub

    Private Sub Accept()
        Dim p = Selected
        If p Is Nothing Then Return
        _SelectedHn = p.Hn
        DialogResult = DialogResult.OK
        Close()
    End Sub

End Class

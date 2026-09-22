<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LabPage
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        pnlPageHeader = New Panel()
        lblSubtitle = New Label()
        lblTitle = New Label()
        tlpMain = New TableLayoutPanel()
        tlpTop = New TableLayoutPanel()
        cardFilter = New CardPanel()
        tlpFilter = New TableLayoutPanel()
        lblSearch = New Label()
        lblLab = New Label()
        lblYearFrom = New Label()
        lblYearTo = New Label()
        txtSearch = New ModernTextBox()
        cmbLab = New ComboBox()
        cmbYearFrom = New ComboBox()
        cmbYearTo = New ComboBox()
        flpActions = New FlowLayoutPanel()
        btnShow = New ModernButton()
        btnPrint = New ModernButton()
        cardFerritin = New CardPanel()
        lstFerritin = New ModernListBox()
        lblNoFerritin = New Label()
        pnlViewBar = New Panel()
        lblResultInfo = New Label()
        segView = New SegmentedControl()
        cardResult = New CardPanel()
        dgvLab = New DataGridView()
        chartLab = New LineChart()
        emptyLab = New EmptyState()
        pnlPageHeader.SuspendLayout()
        tlpMain.SuspendLayout()
        tlpTop.SuspendLayout()
        cardFilter.SuspendLayout()
        tlpFilter.SuspendLayout()
        flpActions.SuspendLayout()
        cardFerritin.SuspendLayout()
        pnlViewBar.SuspendLayout()
        cardResult.SuspendLayout()
        CType(dgvLab, System.ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        '
        'pnlPageHeader
        '
        pnlPageHeader.Controls.Add(lblSubtitle)
        pnlPageHeader.Controls.Add(lblTitle)
        pnlPageHeader.Dock = DockStyle.Top
        pnlPageHeader.Location = New Point(0, 0)
        pnlPageHeader.Name = "pnlPageHeader"
        pnlPageHeader.Size = New Size(900, 66)
        pnlPageHeader.TabIndex = 0
        '
        'lblSubtitle
        '
        lblSubtitle.AutoSize = True
        lblSubtitle.Font = New Font("Leelawadee UI", 9.5F)
        lblSubtitle.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblSubtitle.Location = New Point(1, 33)
        lblSubtitle.Name = "lblSubtitle"
        lblSubtitle.Size = New Size(460, 17)
        lblSubtitle.TabIndex = 1
        lblSubtitle.Text = "LAB Result · ค้นหาผล LAB ย้อนหลัง ดูเป็นตารางหรือกราฟ และพิมพ์รายงาน"
        '
        'lblTitle
        '
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Leelawadee UI", 16.0F, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblTitle.Location = New Point(-2, 0)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(290, 30)
        lblTitle.TabIndex = 0
        lblTitle.Text = "ผลตรวจทางห้องปฏิบัติการ"
        '
        'tlpMain
        '
        tlpMain.ColumnCount = 1
        tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        tlpMain.Controls.Add(tlpTop, 0, 0)
        tlpMain.Controls.Add(pnlViewBar, 0, 1)
        tlpMain.Controls.Add(cardResult, 0, 2)
        tlpMain.Dock = DockStyle.Fill
        tlpMain.Location = New Point(0, 66)
        tlpMain.Name = "tlpMain"
        tlpMain.RowCount = 3
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 166.0F))
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 54.0F))
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpMain.Size = New Size(900, 534)
        tlpMain.TabIndex = 1
        '
        'tlpTop
        '
        tlpTop.ColumnCount = 2
        tlpTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 72.0F))
        tlpTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 28.0F))
        tlpTop.Controls.Add(cardFilter, 0, 0)
        tlpTop.Controls.Add(cardFerritin, 1, 0)
        tlpTop.Dock = DockStyle.Fill
        tlpTop.Location = New Point(0, 0)
        tlpTop.Margin = New Padding(0)
        tlpTop.Name = "tlpTop"
        tlpTop.RowCount = 1
        tlpTop.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpTop.Size = New Size(900, 166)
        tlpTop.TabIndex = 0
        '
        'cardFilter
        '
        cardFilter.Controls.Add(tlpFilter)
        cardFilter.Dock = DockStyle.Fill
        cardFilter.IconKind = IconKind.Search
        cardFilter.Location = New Point(0, 0)
        cardFilter.Margin = New Padding(0, 0, 14, 12)
        cardFilter.Name = "cardFilter"
        cardFilter.Size = New Size(634, 154)
        cardFilter.TabIndex = 0
        cardFilter.Title = "ประวัติ LAB"
        '
        'tlpFilter
        '
        tlpFilter.ColumnCount = 5
        tlpFilter.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 42.0F))
        tlpFilter.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 58.0F))
        tlpFilter.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 92.0F))
        tlpFilter.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 92.0F))
        tlpFilter.ColumnStyles.Add(New ColumnStyle())
        tlpFilter.Controls.Add(lblSearch, 0, 0)
        tlpFilter.Controls.Add(lblLab, 1, 0)
        tlpFilter.Controls.Add(lblYearFrom, 2, 0)
        tlpFilter.Controls.Add(lblYearTo, 3, 0)
        tlpFilter.Controls.Add(txtSearch, 0, 1)
        tlpFilter.Controls.Add(cmbLab, 1, 1)
        tlpFilter.Controls.Add(cmbYearFrom, 2, 1)
        tlpFilter.Controls.Add(cmbYearTo, 3, 1)
        tlpFilter.Controls.Add(flpActions, 4, 1)
        tlpFilter.Dock = DockStyle.Fill
        tlpFilter.Location = New Point(16, 56)
        tlpFilter.Name = "tlpFilter"
        tlpFilter.RowCount = 2
        tlpFilter.RowStyles.Add(New RowStyle(SizeType.Absolute, 24.0F))
        tlpFilter.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpFilter.Size = New Size(602, 82)
        tlpFilter.TabIndex = 0
        '
        'lblSearch
        '
        lblSearch.AutoSize = True
        lblSearch.Font = New Font("Leelawadee UI", 9.0F)
        lblSearch.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblSearch.Location = New Point(3, 0)
        lblSearch.Name = "lblSearch"
        lblSearch.Size = New Size(60, 16)
        lblSearch.TabIndex = 0
        lblSearch.Text = "ช่วยกรอง"
        '
        'lblLab
        '
        lblLab.AutoSize = True
        lblLab.Font = New Font("Leelawadee UI", 9.0F)
        lblLab.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblLab.Location = New Point(150, 0)
        lblLab.Name = "lblLab"
        lblLab.Size = New Size(66, 16)
        lblLab.TabIndex = 1
        lblLab.Text = "รายการ LAB"
        '
        'lblYearFrom
        '
        lblYearFrom.AutoSize = True
        lblYearFrom.Font = New Font("Leelawadee UI", 9.0F)
        lblYearFrom.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblYearFrom.Location = New Point(360, 0)
        lblYearFrom.Name = "lblYearFrom"
        lblYearFrom.Size = New Size(52, 16)
        lblYearFrom.TabIndex = 2
        lblYearFrom.Text = "ปีเริ่มต้น"
        '
        'lblYearTo
        '
        lblYearTo.AutoSize = True
        lblYearTo.Font = New Font("Leelawadee UI", 9.0F)
        lblYearTo.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblYearTo.Location = New Point(452, 0)
        lblYearTo.Name = "lblYearTo"
        lblYearTo.Size = New Size(48, 16)
        lblYearTo.TabIndex = 3
        lblYearTo.Text = "ปีสิ้นสุด"
        '
        'txtSearch
        '
        txtSearch.Dock = DockStyle.Top
        txtSearch.IconKind = IconKind.Search
        txtSearch.Location = New Point(3, 27)
        txtSearch.Name = "txtSearch"
        txtSearch.PlaceholderText = "พิมพ์แล้วกด Enter"
        txtSearch.Size = New Size(141, 38)
        txtSearch.TabIndex = 4
        '
        'cmbLab
        '
        cmbLab.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbLab.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbLab.Dock = DockStyle.Top
        cmbLab.DropDownHeight = 320
        cmbLab.Font = New Font("Leelawadee UI", 11.0F)
        cmbLab.IntegralHeight = False
        cmbLab.Location = New Point(150, 30)
        cmbLab.Margin = New Padding(3, 6, 3, 3)
        cmbLab.Name = "cmbLab"
        cmbLab.Size = New Size(204, 28)
        cmbLab.TabIndex = 5
        '
        'cmbYearFrom
        '
        cmbYearFrom.Dock = DockStyle.Top
        cmbYearFrom.DropDownStyle = ComboBoxStyle.DropDownList
        cmbYearFrom.Font = New Font("Leelawadee UI", 11.0F)
        cmbYearFrom.Location = New Point(360, 30)
        cmbYearFrom.Margin = New Padding(3, 6, 3, 3)
        cmbYearFrom.Name = "cmbYearFrom"
        cmbYearFrom.Size = New Size(86, 28)
        cmbYearFrom.TabIndex = 6
        '
        'cmbYearTo
        '
        cmbYearTo.Dock = DockStyle.Top
        cmbYearTo.DropDownStyle = ComboBoxStyle.DropDownList
        cmbYearTo.Font = New Font("Leelawadee UI", 11.0F)
        cmbYearTo.Location = New Point(452, 30)
        cmbYearTo.Margin = New Padding(3, 6, 3, 3)
        cmbYearTo.Name = "cmbYearTo"
        cmbYearTo.Size = New Size(86, 28)
        cmbYearTo.TabIndex = 7
        '
        'flpActions
        '
        flpActions.AutoSize = True
        flpActions.AutoSizeMode = AutoSizeMode.GrowAndShrink
        flpActions.Controls.Add(btnShow)
        flpActions.Controls.Add(btnPrint)
        flpActions.Location = New Point(544, 24)
        flpActions.Margin = New Padding(6, 0, 0, 0)
        flpActions.Name = "flpActions"
        flpActions.Size = New Size(222, 44)
        flpActions.TabIndex = 8
        flpActions.WrapContents = False
        '
        'btnShow
        '
        btnShow.IconKind = IconKind.Search
        btnShow.Location = New Point(3, 3)
        btnShow.Name = "btnShow"
        btnShow.Size = New Size(112, 38)
        btnShow.TabIndex = 0
        btnShow.Text = "แสดงผล"
        '
        'btnPrint
        '
        btnPrint.IconKind = IconKind.Printer
        btnPrint.Kind = ButtonVariant.Secondary
        btnPrint.Location = New Point(121, 3)
        btnPrint.Name = "btnPrint"
        btnPrint.Size = New Size(98, 38)
        btnPrint.TabIndex = 1
        btnPrint.Text = "พิมพ์"
        '
        'cardFerritin
        '
        cardFerritin.Controls.Add(lstFerritin)
        cardFerritin.Controls.Add(lblNoFerritin)
        cardFerritin.Dock = DockStyle.Fill
        cardFerritin.IconKind = IconKind.Droplet
        cardFerritin.IconLevel = AlertLevel.Danger
        cardFerritin.Location = New Point(648, 0)
        cardFerritin.Margin = New Padding(0, 0, 0, 12)
        cardFerritin.Name = "cardFerritin"
        cardFerritin.Padding = New Padding(16, 16, 16, 8)
        cardFerritin.Size = New Size(252, 154)
        cardFerritin.TabIndex = 1
        cardFerritin.Title = "Ferritin ล่าสุด"
        '
        'lstFerritin
        '
        lstFerritin.Dock = DockStyle.Fill
        lstFerritin.Location = New Point(16, 56)
        lstFerritin.Name = "lstFerritin"
        lstFerritin.RowHeight = 30
        lstFerritin.Size = New Size(220, 90)
        lstFerritin.TabIndex = 0
        '
        'lblNoFerritin
        '
        lblNoFerritin.Dock = DockStyle.Fill
        lblNoFerritin.ForeColor = Color.FromArgb(CByte(148), CByte(163), CByte(184))
        lblNoFerritin.Location = New Point(16, 56)
        lblNoFerritin.Name = "lblNoFerritin"
        lblNoFerritin.Size = New Size(220, 90)
        lblNoFerritin.TabIndex = 1
        lblNoFerritin.Text = "ไม่มีผล Ferritin"
        lblNoFerritin.TextAlign = ContentAlignment.MiddleCenter
        lblNoFerritin.Visible = False
        '
        'pnlViewBar
        '
        pnlViewBar.Controls.Add(lblResultInfo)
        pnlViewBar.Controls.Add(segView)
        pnlViewBar.Dock = DockStyle.Fill
        pnlViewBar.Location = New Point(0, 166)
        pnlViewBar.Margin = New Padding(0)
        pnlViewBar.Name = "pnlViewBar"
        pnlViewBar.Size = New Size(900, 54)
        pnlViewBar.TabIndex = 1
        '
        'lblResultInfo
        '
        lblResultInfo.AutoSize = True
        lblResultInfo.Font = New Font("Leelawadee UI", 9.5F)
        lblResultInfo.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblResultInfo.Location = New Point(236, 14)
        lblResultInfo.Name = "lblResultInfo"
        lblResultInfo.Size = New Size(10, 17)
        lblResultInfo.TabIndex = 1
        '
        'segView
        '
        segView.Items = New String() {"ตาราง", "กราฟ"}
        segView.Location = New Point(0, 4)
        segView.Name = "segView"
        segView.Size = New Size(220, 38)
        segView.TabIndex = 0
        '
        'cardResult
        '
        cardResult.Controls.Add(dgvLab)
        cardResult.Controls.Add(chartLab)
        cardResult.Controls.Add(emptyLab)
        cardResult.Dock = DockStyle.Fill
        cardResult.Location = New Point(0, 220)
        cardResult.Margin = New Padding(0)
        cardResult.Name = "cardResult"
        cardResult.Padding = New Padding(10)
        cardResult.Size = New Size(900, 314)
        cardResult.TabIndex = 2
        '
        'dgvLab
        '
        dgvLab.Dock = DockStyle.Fill
        dgvLab.Location = New Point(10, 10)
        dgvLab.Name = "dgvLab"
        dgvLab.Size = New Size(880, 294)
        dgvLab.TabIndex = 0
        dgvLab.Visible = False
        '
        'chartLab
        '
        chartLab.Dock = DockStyle.Fill
        chartLab.Location = New Point(10, 10)
        chartLab.Name = "chartLab"
        chartLab.Size = New Size(880, 294)
        chartLab.TabIndex = 1
        chartLab.Visible = False
        '
        'emptyLab
        '
        emptyLab.BackColor = Color.White
        emptyLab.Description = "เลือกรายการ LAB และช่วงปี แล้วกด ""แสดงผล"""
        emptyLab.Dock = DockStyle.Fill
        emptyLab.IconKind = IconKind.Flask
        emptyLab.Location = New Point(10, 10)
        emptyLab.Name = "emptyLab"
        emptyLab.Size = New Size(880, 294)
        emptyLab.TabIndex = 2
        emptyLab.Title = "ยังไม่ได้เลือกรายการ LAB"
        '
        'LabPage
        '
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        AutoScroll = True
        AutoScrollMinSize = New Size(820, 500)
        BackColor = Color.FromArgb(CByte(244), CByte(246), CByte(250))
        Controls.Add(tlpMain)
        Controls.Add(pnlPageHeader)
        Font = New Font("Leelawadee UI", 10.0F)
        Name = "LabPage"
        Size = New Size(900, 600)
        pnlPageHeader.ResumeLayout(False)
        pnlPageHeader.PerformLayout()
        tlpMain.ResumeLayout(False)
        tlpTop.ResumeLayout(False)
        cardFilter.ResumeLayout(False)
        tlpFilter.ResumeLayout(False)
        tlpFilter.PerformLayout()
        flpActions.ResumeLayout(False)
        cardFerritin.ResumeLayout(False)
        pnlViewBar.ResumeLayout(False)
        pnlViewBar.PerformLayout()
        cardResult.ResumeLayout(False)
        CType(dgvLab, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlPageHeader As Panel
    Friend WithEvents lblSubtitle As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents tlpTop As TableLayoutPanel
    Friend WithEvents cardFilter As CardPanel
    Friend WithEvents tlpFilter As TableLayoutPanel
    Friend WithEvents lblSearch As Label
    Friend WithEvents lblLab As Label
    Friend WithEvents lblYearFrom As Label
    Friend WithEvents lblYearTo As Label
    Friend WithEvents txtSearch As ModernTextBox
    Friend WithEvents cmbLab As ComboBox
    Friend WithEvents cmbYearFrom As ComboBox
    Friend WithEvents cmbYearTo As ComboBox
    Friend WithEvents flpActions As FlowLayoutPanel
    Friend WithEvents btnShow As ModernButton
    Friend WithEvents btnPrint As ModernButton
    Friend WithEvents cardFerritin As CardPanel
    Friend WithEvents lstFerritin As ModernListBox
    Friend WithEvents lblNoFerritin As Label
    Friend WithEvents pnlViewBar As Panel
    Friend WithEvents lblResultInfo As Label
    Friend WithEvents segView As SegmentedControl
    Friend WithEvents cardResult As CardPanel
    Friend WithEvents dgvLab As DataGridView
    Friend WithEvents chartLab As LineChart
    Friend WithEvents emptyLab As EmptyState
End Class

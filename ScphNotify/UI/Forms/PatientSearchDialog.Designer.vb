<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PatientSearchDialog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        components = New System.ComponentModel.Container()
        pnlHeader = New Panel()
        lblDesc = New Label()
        lblTitle = New Label()
        tileIcon = New IconTile()
        pnlSearch = New Panel()
        txtSearch = New ModernTextBox()
        btnSearch = New ModernButton()
        pnlResult = New Panel()
        dgvResult = New DataGridView()
        emptyResult = New EmptyState()
        pnlFooter = New Panel()
        lblInfo = New Label()
        flpFooter = New FlowLayoutPanel()
        btnSelect = New ModernButton()
        btnCancel = New ModernButton()
        tip = New ToolTip(components)
        pnlHeader.SuspendLayout()
        pnlSearch.SuspendLayout()
        pnlResult.SuspendLayout()
        pnlFooter.SuspendLayout()
        flpFooter.SuspendLayout()
        CType(dgvResult, System.ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        '
        'pnlHeader
        '
        pnlHeader.Controls.Add(lblDesc)
        pnlHeader.Controls.Add(lblTitle)
        pnlHeader.Controls.Add(tileIcon)
        pnlHeader.Dock = DockStyle.Top
        pnlHeader.Location = New Point(0, 0)
        pnlHeader.Name = "pnlHeader"
        pnlHeader.Size = New Size(820, 96)
        pnlHeader.TabIndex = 0
        '
        'lblDesc
        '
        lblDesc.AutoSize = True
        lblDesc.Font = New Font("Leelawadee UI", 9.5F)
        lblDesc.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblDesc.Location = New Point(89, 56)
        lblDesc.Name = "lblDesc"
        lblDesc.Size = New Size(430, 17)
        lblDesc.TabIndex = 2
        lblDesc.Text = "ค้นด้วยชื่อ · ชื่อ-สกุล · HN (ไม่ต้องใส่ 0 นำหน้า) · เลขบัตรประชาชน 13 หลัก"
        '
        'lblTitle
        '
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Leelawadee UI", 14.0F, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblTitle.Location = New Point(87, 26)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(260, 26)
        lblTitle.TabIndex = 1
        lblTitle.Text = "ค้นหาผู้ป่วย"
        '
        'tileIcon
        '
        tileIcon.IconKind = IconKind.Search
        tileIcon.Level = AlertLevel.Primary
        tileIcon.Location = New Point(28, 26)
        tileIcon.Name = "tileIcon"
        tileIcon.Size = New Size(48, 48)
        tileIcon.TabIndex = 0
        '
        'pnlSearch
        '
        pnlSearch.Controls.Add(txtSearch)
        pnlSearch.Controls.Add(btnSearch)
        pnlSearch.Dock = DockStyle.Top
        pnlSearch.Location = New Point(0, 96)
        pnlSearch.Name = "pnlSearch"
        pnlSearch.Padding = New Padding(28, 0, 28, 12)
        pnlSearch.Size = New Size(820, 56)
        pnlSearch.TabIndex = 1
        '
        'txtSearch
        '
        txtSearch.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtSearch.IconKind = IconKind.Search
        txtSearch.Location = New Point(28, 2)
        txtSearch.Name = "txtSearch"
        txtSearch.PlaceholderText = "เช่น สมชาย · 100407 · 3100500123456 แล้วกด Enter"
        txtSearch.Size = New Size(630, 42)
        txtSearch.TabIndex = 0
        '
        'btnSearch
        '
        btnSearch.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnSearch.AutoSize = True
        btnSearch.AutoSizeMode = AutoSizeMode.GrowAndShrink
        btnSearch.IconKind = IconKind.Search
        btnSearch.Location = New Point(668, 2)
        btnSearch.Name = "btnSearch"
        btnSearch.Size = New Size(124, 42)
        btnSearch.TabIndex = 1
        btnSearch.Text = "ค้นหา"
        '
        'pnlResult
        '
        pnlResult.BackColor = Color.White
        pnlResult.Controls.Add(dgvResult)
        pnlResult.Controls.Add(emptyResult)
        pnlResult.Dock = DockStyle.Fill
        pnlResult.Location = New Point(0, 152)
        pnlResult.Name = "pnlResult"
        pnlResult.Padding = New Padding(28, 0, 28, 8)
        pnlResult.Size = New Size(820, 340)
        pnlResult.TabIndex = 2
        '
        'dgvResult
        '
        dgvResult.Dock = DockStyle.Fill
        dgvResult.Location = New Point(28, 0)
        dgvResult.Name = "dgvResult"
        dgvResult.Size = New Size(764, 332)
        dgvResult.TabIndex = 0
        dgvResult.Visible = False
        '
        'emptyResult
        '
        emptyResult.BackColor = Color.White
        emptyResult.Description = "HN พิมพ์ไม่ครบหลักได้ (เติม 0 นำหน้าให้เอง) · เลขบัตรประชาชนต้องครบ 13 หลัก"
        emptyResult.Dock = DockStyle.Fill
        emptyResult.IconKind = IconKind.Search
        emptyResult.Location = New Point(28, 0)
        emptyResult.Name = "emptyResult"
        emptyResult.Size = New Size(764, 332)
        emptyResult.TabIndex = 1
        emptyResult.Title = "ยังไม่ได้ค้นหา"
        '
        'pnlFooter
        '
        pnlFooter.BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        pnlFooter.Controls.Add(flpFooter)
        pnlFooter.Controls.Add(lblInfo)
        pnlFooter.Dock = DockStyle.Bottom
        pnlFooter.Location = New Point(0, 492)
        pnlFooter.Name = "pnlFooter"
        pnlFooter.Size = New Size(820, 72)
        pnlFooter.TabIndex = 3
        '
        'lblInfo
        '
        lblInfo.AutoSize = True
        lblInfo.Font = New Font("Leelawadee UI", 9.0F)
        lblInfo.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        lblInfo.Location = New Point(28, 28)
        lblInfo.Name = "lblInfo"
        lblInfo.Size = New Size(300, 17)
        lblInfo.TabIndex = 0
        lblInfo.Text = "ดับเบิลคลิกที่รายชื่อเพื่อเลือกผู้ป่วยได้เลย"
        '
        'flpFooter
        '
        flpFooter.AutoSize = True
        flpFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink
        flpFooter.Controls.Add(btnSelect)
        flpFooter.Controls.Add(btnCancel)
        flpFooter.Dock = DockStyle.Right
        flpFooter.FlowDirection = FlowDirection.RightToLeft
        flpFooter.Location = New Point(560, 0)
        flpFooter.Name = "flpFooter"
        flpFooter.Padding = New Padding(0, 16, 22, 16)
        flpFooter.Size = New Size(260, 72)
        flpFooter.TabIndex = 1
        flpFooter.WrapContents = False
        '
        'btnSelect
        '
        btnSelect.AutoSize = True
        btnSelect.AutoSizeMode = AutoSizeMode.GrowAndShrink
        btnSelect.Enabled = False
        btnSelect.IconKind = IconKind.Check
        btnSelect.Location = New Point(112, 19)
        btnSelect.Name = "btnSelect"
        btnSelect.Size = New Size(140, 40)
        btnSelect.TabIndex = 0
        btnSelect.Text = "เลือกผู้ป่วยรายนี้"
        '
        'btnCancel
        '
        btnCancel.AutoSize = True
        btnCancel.AutoSizeMode = AutoSizeMode.GrowAndShrink
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.Kind = ButtonVariant.Ghost
        btnCancel.Location = New Point(12, 19)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(90, 40)
        btnCancel.TabIndex = 1
        btnCancel.Text = "ยกเลิก"
        '
        'PatientSearchDialog
        '
        AcceptButton = btnSearch
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        BackColor = Color.White
        CancelButton = btnCancel
        ClientSize = New Size(820, 564)
        Controls.Add(pnlResult)
        Controls.Add(pnlSearch)
        Controls.Add(pnlFooter)
        Controls.Add(pnlHeader)
        Font = New Font("Leelawadee UI", 10.0F)
        MinimizeBox = False
        MinimumSize = New Size(720, 520)
        Name = "PatientSearchDialog"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "ค้นหาผู้ป่วย"
        pnlHeader.ResumeLayout(False)
        pnlHeader.PerformLayout()
        pnlSearch.ResumeLayout(False)
        pnlSearch.PerformLayout()
        pnlResult.ResumeLayout(False)
        pnlFooter.ResumeLayout(False)
        pnlFooter.PerformLayout()
        flpFooter.ResumeLayout(False)
        flpFooter.PerformLayout()
        CType(dgvResult, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlHeader As Panel
    Friend WithEvents lblDesc As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents tileIcon As IconTile
    Friend WithEvents pnlSearch As Panel
    Friend WithEvents txtSearch As ModernTextBox
    Friend WithEvents btnSearch As ModernButton
    Friend WithEvents pnlResult As Panel
    Friend WithEvents dgvResult As DataGridView
    Friend WithEvents emptyResult As EmptyState
    Friend WithEvents pnlFooter As Panel
    Friend WithEvents lblInfo As Label
    Friend WithEvents flpFooter As FlowLayoutPanel
    Friend WithEvents btnSelect As ModernButton
    Friend WithEvents btnCancel As ModernButton
    Friend WithEvents tip As ToolTip
End Class

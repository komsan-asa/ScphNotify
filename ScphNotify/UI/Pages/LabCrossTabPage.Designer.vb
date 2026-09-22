<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LabCrossTabPage
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
        cardItems = New CardPanel()
        clbItems = New CheckedListBox()
        pnlItemsFooter = New Panel()
        btnBuild = New ModernButton()
        btnClear = New ModernButton()
        lblSelectedCount = New Label()
        pnlSearchGap = New Panel()
        txtSearch = New ModernTextBox()
        cardTable = New CardPanel()
        dgvCross = New DataGridView()
        emptyCross = New EmptyState()
        flpSelected = New FlowLayoutPanel()
        pnlPageHeader.SuspendLayout()
        tlpMain.SuspendLayout()
        cardItems.SuspendLayout()
        pnlItemsFooter.SuspendLayout()
        cardTable.SuspendLayout()
        CType(dgvCross, System.ComponentModel.ISupportInitialize).BeginInit()
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
        lblSubtitle.Size = New Size(440, 17)
        lblSubtitle.TabIndex = 1
        lblSubtitle.Text = "เลือกได้หลายรายการ แล้วดูผลเทียบกันตามวันที่รายงานผล"
        '
        'lblTitle
        '
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Leelawadee UI", 16.0F, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblTitle.Location = New Point(-2, 0)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(320, 30)
        lblTitle.TabIndex = 0
        lblTitle.Text = "เปรียบเทียบผล LAB (Crosstab)"
        '
        'tlpMain
        '
        tlpMain.ColumnCount = 2
        tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 316.0F))
        tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        tlpMain.Controls.Add(cardItems, 0, 0)
        tlpMain.Controls.Add(cardTable, 1, 0)
        tlpMain.Dock = DockStyle.Fill
        tlpMain.Location = New Point(0, 66)
        tlpMain.Name = "tlpMain"
        tlpMain.RowCount = 1
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpMain.Size = New Size(900, 534)
        tlpMain.TabIndex = 1
        '
        'cardItems
        '
        cardItems.Controls.Add(clbItems)
        cardItems.Controls.Add(pnlItemsFooter)
        cardItems.Controls.Add(pnlSearchGap)
        cardItems.Controls.Add(txtSearch)
        cardItems.Dock = DockStyle.Fill
        cardItems.IconKind = IconKind.Flask
        cardItems.Location = New Point(0, 0)
        cardItems.Margin = New Padding(0, 0, 16, 0)
        cardItems.Name = "cardItems"
        cardItems.Size = New Size(300, 534)
        cardItems.TabIndex = 0
        cardItems.Title = "รายการ LAB ของผู้ป่วย"
        '
        'clbItems
        '
        clbItems.BorderStyle = BorderStyle.None
        clbItems.CheckOnClick = True
        clbItems.Dock = DockStyle.Fill
        clbItems.Font = New Font("Leelawadee UI", 10.5F)
        clbItems.FormattingEnabled = True
        clbItems.IntegralHeight = False
        clbItems.Location = New Point(16, 104)
        clbItems.Name = "clbItems"
        clbItems.Size = New Size(268, 358)
        clbItems.TabIndex = 2
        '
        'pnlItemsFooter
        '
        pnlItemsFooter.Controls.Add(btnBuild)
        pnlItemsFooter.Controls.Add(btnClear)
        pnlItemsFooter.Controls.Add(lblSelectedCount)
        pnlItemsFooter.Dock = DockStyle.Bottom
        pnlItemsFooter.Location = New Point(16, 462)
        pnlItemsFooter.Name = "pnlItemsFooter"
        pnlItemsFooter.Size = New Size(268, 56)
        pnlItemsFooter.TabIndex = 3
        '
        'btnBuild
        '
        btnBuild.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnBuild.IconKind = IconKind.ArrowRight
        btnBuild.Location = New Point(146, 14)
        btnBuild.Name = "btnBuild"
        btnBuild.Size = New Size(122, 40)
        btnBuild.TabIndex = 1
        btnBuild.Text = "สร้างตาราง"
        '
        'btnClear
        '
        btnClear.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnClear.Kind = ButtonVariant.Ghost
        btnClear.Location = New Point(82, 14)
        btnClear.Name = "btnClear"
        btnClear.Size = New Size(60, 40)
        btnClear.TabIndex = 0
        btnClear.Text = "ล้าง"
        '
        'lblSelectedCount
        '
        lblSelectedCount.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblSelectedCount.AutoSize = True
        lblSelectedCount.Font = New Font("Leelawadee UI", 9.0F)
        lblSelectedCount.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblSelectedCount.Location = New Point(0, 26)
        lblSelectedCount.Name = "lblSelectedCount"
        lblSelectedCount.Size = New Size(78, 16)
        lblSelectedCount.TabIndex = 2
        lblSelectedCount.Text = "เลือก 0 รายการ"
        '
        'pnlSearchGap
        '
        pnlSearchGap.Dock = DockStyle.Top
        pnlSearchGap.Location = New Point(16, 94)
        pnlSearchGap.Name = "pnlSearchGap"
        pnlSearchGap.Size = New Size(268, 10)
        pnlSearchGap.TabIndex = 1
        '
        'txtSearch
        '
        txtSearch.Dock = DockStyle.Top
        txtSearch.IconKind = IconKind.Search
        txtSearch.Location = New Point(16, 56)
        txtSearch.Name = "txtSearch"
        txtSearch.PlaceholderText = "ค้นหาชื่อ LAB..."
        txtSearch.Size = New Size(268, 38)
        txtSearch.TabIndex = 0
        '
        'cardTable
        '
        cardTable.Controls.Add(dgvCross)
        cardTable.Controls.Add(emptyCross)
        cardTable.Controls.Add(flpSelected)
        cardTable.Dock = DockStyle.Fill
        cardTable.IconKind = IconKind.Table
        cardTable.IconLevel = AlertLevel.Info
        cardTable.Location = New Point(316, 0)
        cardTable.Margin = New Padding(0)
        cardTable.Name = "cardTable"
        cardTable.Padding = New Padding(12, 16, 12, 12)
        cardTable.Size = New Size(584, 534)
        cardTable.TabIndex = 1
        cardTable.Title = "ตารางผล LAB"
        '
        'dgvCross
        '
        dgvCross.Dock = DockStyle.Fill
        dgvCross.Location = New Point(12, 96)
        dgvCross.Name = "dgvCross"
        dgvCross.Size = New Size(560, 426)
        dgvCross.TabIndex = 1
        dgvCross.Visible = False
        '
        'emptyCross
        '
        emptyCross.BackColor = Color.White
        emptyCross.Description = "ติ๊กเลือกรายการ LAB ทางซ้าย แล้วกด ""สร้างตาราง"""
        emptyCross.Dock = DockStyle.Fill
        emptyCross.IconKind = IconKind.Table
        emptyCross.Location = New Point(12, 96)
        emptyCross.Name = "emptyCross"
        emptyCross.Size = New Size(560, 426)
        emptyCross.TabIndex = 2
        emptyCross.Title = "ยังไม่ได้เลือกรายการ"
        '
        'flpSelected
        '
        flpSelected.AutoSize = True
        flpSelected.AutoSizeMode = AutoSizeMode.GrowAndShrink
        flpSelected.Dock = DockStyle.Top
        flpSelected.Location = New Point(12, 56)
        flpSelected.Name = "flpSelected"
        flpSelected.Padding = New Padding(0, 0, 0, 8)
        flpSelected.Size = New Size(560, 8)
        flpSelected.TabIndex = 0
        '
        'LabCrossTabPage
        '
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        AutoScroll = True
        AutoScrollMinSize = New Size(760, 460)
        BackColor = Color.FromArgb(CByte(244), CByte(246), CByte(250))
        Controls.Add(tlpMain)
        Controls.Add(pnlPageHeader)
        Font = New Font("Leelawadee UI", 10.0F)
        Name = "LabCrossTabPage"
        Size = New Size(900, 600)
        pnlPageHeader.ResumeLayout(False)
        pnlPageHeader.PerformLayout()
        tlpMain.ResumeLayout(False)
        cardItems.ResumeLayout(False)
        pnlItemsFooter.ResumeLayout(False)
        pnlItemsFooter.PerformLayout()
        cardTable.ResumeLayout(False)
        cardTable.PerformLayout()
        CType(dgvCross, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlPageHeader As Panel
    Friend WithEvents lblSubtitle As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents cardItems As CardPanel
    Friend WithEvents clbItems As CheckedListBox
    Friend WithEvents pnlItemsFooter As Panel
    Friend WithEvents btnBuild As ModernButton
    Friend WithEvents btnClear As ModernButton
    Friend WithEvents lblSelectedCount As Label
    Friend WithEvents pnlSearchGap As Panel
    Friend WithEvents txtSearch As ModernTextBox
    Friend WithEvents cardTable As CardPanel
    Friend WithEvents dgvCross As DataGridView
    Friend WithEvents emptyCross As EmptyState
    Friend WithEvents flpSelected As FlowLayoutPanel
End Class

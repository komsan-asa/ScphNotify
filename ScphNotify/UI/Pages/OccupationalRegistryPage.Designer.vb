<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class OccupationalRegistryPage
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
        components = New System.ComponentModel.Container()
        pnlPageHeader = New Panel()
        lblSubtitle = New Label()
        lblTitle = New Label()
        tlpMain = New TableLayoutPanel()
        cardFilter = New CardPanel()
        tlpFilter = New TableLayoutPanel()
        lblRange = New Label()
        lblFrom = New Label()
        lblTo = New Label()
        lblSearch = New Label()
        segRange = New SegmentedControl()
        dtpFrom = New DateTimePicker()
        dtpTo = New DateTimePicker()
        txtSearch = New ModernTextBox()
        flpActions = New FlowLayoutPanel()
        btnShow = New ModernButton()
        btnExport = New ModernButton()
        tlpStats = New TableLayoutPanel()
        cardTotal = New CardPanel()
        lblTotal = New Label()
        cardReferral = New CardPanel()
        lblReferral = New Label()
        cardInjury = New CardPanel()
        lblInjury = New Label()
        cardDust = New CardPanel()
        lblDust = New Label()
        cardResult = New CardPanel()
        dgvRegistry = New DataGridView()
        emptyRegistry = New EmptyState()
        pnlInfo = New Panel()
        lblInfo = New Label()
        tip = New ToolTip(components)
        pnlPageHeader.SuspendLayout()
        tlpMain.SuspendLayout()
        cardFilter.SuspendLayout()
        tlpFilter.SuspendLayout()
        flpActions.SuspendLayout()
        tlpStats.SuspendLayout()
        cardTotal.SuspendLayout()
        cardReferral.SuspendLayout()
        cardInjury.SuspendLayout()
        cardDust.SuspendLayout()
        cardResult.SuspendLayout()
        pnlInfo.SuspendLayout()
        CType(dgvRegistry, System.ComponentModel.ISupportInitialize).BeginInit()
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
        lblSubtitle.Size = New Size(520, 17)
        lblSubtitle.TabIndex = 1
        lblSubtitle.Text = "ผลคัดกรองที่บันทึกไว้ทั้งหมด · ค้นหาย้อนหลังตามช่วงวันที่ HN VN ชื่อ หรือผู้บันทึก"
        '
        'lblTitle
        '
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Leelawadee UI", 16.0F, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblTitle.Location = New Point(-2, 0)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(360, 30)
        lblTitle.TabIndex = 0
        lblTitle.Text = "ทะเบียนโรคจากการทำงานและ PM2.5"
        '
        'tlpMain
        '
        tlpMain.ColumnCount = 1
        tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        tlpMain.Controls.Add(cardFilter, 0, 0)
        tlpMain.Controls.Add(tlpStats, 0, 1)
        tlpMain.Controls.Add(pnlInfo, 0, 2)
        tlpMain.Controls.Add(cardResult, 0, 3)
        tlpMain.Dock = DockStyle.Fill
        tlpMain.Location = New Point(0, 66)
        tlpMain.Name = "tlpMain"
        tlpMain.RowCount = 4
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 152.0F))
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 120.0F))
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 34.0F))
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpMain.Size = New Size(900, 534)
        tlpMain.TabIndex = 1
        '
        'cardFilter
        '
        cardFilter.Controls.Add(tlpFilter)
        cardFilter.Dock = DockStyle.Fill
        cardFilter.IconKind = IconKind.Filter
        cardFilter.Location = New Point(0, 0)
        cardFilter.Margin = New Padding(0, 0, 0, 12)
        cardFilter.Name = "cardFilter"
        cardFilter.Size = New Size(900, 140)
        cardFilter.TabIndex = 0
        cardFilter.Title = "ช่วงข้อมูลที่ต้องการดู"
        '
        'tlpFilter
        '
        tlpFilter.ColumnCount = 5
        tlpFilter.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 214.0F))
        tlpFilter.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 134.0F))
        tlpFilter.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 134.0F))
        tlpFilter.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        tlpFilter.ColumnStyles.Add(New ColumnStyle())
        tlpFilter.Controls.Add(lblRange, 0, 0)
        tlpFilter.Controls.Add(lblFrom, 1, 0)
        tlpFilter.Controls.Add(lblTo, 2, 0)
        tlpFilter.Controls.Add(lblSearch, 3, 0)
        tlpFilter.Controls.Add(segRange, 0, 1)
        tlpFilter.Controls.Add(dtpFrom, 1, 1)
        tlpFilter.Controls.Add(dtpTo, 2, 1)
        tlpFilter.Controls.Add(txtSearch, 3, 1)
        tlpFilter.Controls.Add(flpActions, 4, 1)
        tlpFilter.Dock = DockStyle.Fill
        tlpFilter.Location = New Point(16, 56)
        tlpFilter.Name = "tlpFilter"
        tlpFilter.RowCount = 2
        tlpFilter.RowStyles.Add(New RowStyle(SizeType.Absolute, 24.0F))
        tlpFilter.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpFilter.Size = New Size(868, 68)
        tlpFilter.TabIndex = 0
        '
        'lblRange
        '
        lblRange.AutoSize = True
        lblRange.Font = New Font("Leelawadee UI", 9.0F)
        lblRange.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblRange.Location = New Point(3, 0)
        lblRange.Name = "lblRange"
        lblRange.Size = New Size(70, 16)
        lblRange.TabIndex = 0
        lblRange.Text = "ช่วงเวลา"
        '
        'lblFrom
        '
        lblFrom.AutoSize = True
        lblFrom.Font = New Font("Leelawadee UI", 9.0F)
        lblFrom.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblFrom.Location = New Point(303, 0)
        lblFrom.Name = "lblFrom"
        lblFrom.Size = New Size(60, 16)
        lblFrom.TabIndex = 1
        lblFrom.Text = "ตั้งแต่วันที่"
        '
        'lblTo
        '
        lblTo.AutoSize = True
        lblTo.Font = New Font("Leelawadee UI", 9.0F)
        lblTo.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblTo.Location = New Point(449, 0)
        lblTo.Name = "lblTo"
        lblTo.Size = New Size(48, 16)
        lblTo.TabIndex = 2
        lblTo.Text = "ถึงวันที่"
        '
        'lblSearch
        '
        lblSearch.AutoSize = True
        lblSearch.Font = New Font("Leelawadee UI", 9.0F)
        lblSearch.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblSearch.Location = New Point(595, 0)
        lblSearch.Name = "lblSearch"
        lblSearch.Size = New Size(140, 16)
        lblSearch.TabIndex = 3
        lblSearch.Text = "ค้นหา HN / VN / ชื่อ / ผู้บันทึก"
        '
        'segRange
        '
        segRange.Dock = DockStyle.Top
        segRange.Items = New String() {"วันนี้", "7 วัน", "30 วัน"}
        segRange.Location = New Point(3, 27)
        segRange.Margin = New Padding(3, 3, 12, 3)
        segRange.Name = "segRange"
        segRange.SelectedIndex = 1
        segRange.Size = New Size(199, 38)
        segRange.TabIndex = 4
        '
        'dtpFrom
        '
        dtpFrom.Dock = DockStyle.Top
        dtpFrom.Font = New Font("Leelawadee UI", 10.5F)
        dtpFrom.Format = DateTimePickerFormat.Short
        dtpFrom.Location = New Point(303, 30)
        dtpFrom.Margin = New Padding(3, 6, 6, 3)
        dtpFrom.Name = "dtpFrom"
        dtpFrom.Size = New Size(137, 28)
        dtpFrom.TabIndex = 5
        '
        'dtpTo
        '
        dtpTo.Dock = DockStyle.Top
        dtpTo.Font = New Font("Leelawadee UI", 10.5F)
        dtpTo.Format = DateTimePickerFormat.Short
        dtpTo.Location = New Point(449, 30)
        dtpTo.Margin = New Padding(3, 6, 6, 3)
        dtpTo.Name = "dtpTo"
        dtpTo.Size = New Size(137, 28)
        dtpTo.TabIndex = 6
        '
        'txtSearch
        '
        txtSearch.Dock = DockStyle.Top
        txtSearch.IconKind = IconKind.Search
        txtSearch.Location = New Point(595, 27)
        txtSearch.Margin = New Padding(3, 3, 6, 3)
        txtSearch.Name = "txtSearch"
        txtSearch.PlaceholderText = "พิมพ์แล้วกด Enter"
        txtSearch.Size = New Size(24, 38)
        txtSearch.TabIndex = 7
        '
        'flpActions
        '
        flpActions.AutoSize = True
        flpActions.AutoSizeMode = AutoSizeMode.GrowAndShrink
        flpActions.Controls.Add(btnShow)
        flpActions.Controls.Add(btnExport)
        flpActions.Location = New Point(628, 24)
        flpActions.Margin = New Padding(6, 0, 0, 0)
        flpActions.Name = "flpActions"
        flpActions.Size = New Size(240, 44)
        flpActions.TabIndex = 8
        flpActions.WrapContents = False
        '
        'btnShow
        '
        btnShow.AutoSize = True
        btnShow.AutoSizeMode = AutoSizeMode.GrowAndShrink
        btnShow.IconKind = IconKind.Search
        btnShow.Location = New Point(3, 3)
        btnShow.Name = "btnShow"
        btnShow.Size = New Size(112, 38)
        btnShow.TabIndex = 0
        btnShow.Text = "แสดงทะเบียน"
        '
        'btnExport
        '
        btnExport.AutoSize = True
        btnExport.AutoSizeMode = AutoSizeMode.GrowAndShrink
        btnExport.IconKind = IconKind.Download
        btnExport.Kind = ButtonVariant.Secondary
        btnExport.Location = New Point(121, 3)
        btnExport.Name = "btnExport"
        btnExport.Size = New Size(112, 38)
        btnExport.TabIndex = 1
        btnExport.Text = "ส่งออก Excel"
        '
        'tlpStats
        '
        tlpStats.ColumnCount = 4
        tlpStats.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        tlpStats.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        tlpStats.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        tlpStats.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        tlpStats.Controls.Add(cardTotal, 0, 0)
        tlpStats.Controls.Add(cardReferral, 1, 0)
        tlpStats.Controls.Add(cardInjury, 2, 0)
        tlpStats.Controls.Add(cardDust, 3, 0)
        tlpStats.Dock = DockStyle.Fill
        tlpStats.Location = New Point(0, 152)
        tlpStats.Margin = New Padding(0)
        tlpStats.Name = "tlpStats"
        tlpStats.RowCount = 1
        tlpStats.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpStats.Size = New Size(900, 120)
        tlpStats.TabIndex = 1
        '
        'cardTotal
        '
        cardTotal.Controls.Add(lblTotal)
        cardTotal.Dock = DockStyle.Fill
        cardTotal.IconKind = IconKind.ClipboardCheck
        cardTotal.IconLevel = AlertLevel.Primary
        cardTotal.Location = New Point(0, 0)
        cardTotal.Margin = New Padding(0, 0, 12, 12)
        cardTotal.Name = "cardTotal"
        cardTotal.Padding = New Padding(16, 12, 16, 10)
        cardTotal.Size = New Size(213, 108)
        cardTotal.TabIndex = 0
        cardTotal.Title = "คัดกรองทั้งหมด"
        '
        'lblTotal
        '
        lblTotal.Dock = DockStyle.Fill
        lblTotal.Font = New Font("Leelawadee UI", 18.0F, FontStyle.Bold)
        lblTotal.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblTotal.Location = New Point(16, 52)
        lblTotal.Name = "lblTotal"
        lblTotal.Size = New Size(181, 46)
        lblTotal.TextAlign = ContentAlignment.MiddleLeft
        lblTotal.TabIndex = 0
        lblTotal.Text = "-"
        '
        'cardReferral
        '
        cardReferral.Controls.Add(lblReferral)
        cardReferral.Dock = DockStyle.Fill
        cardReferral.IconKind = IconKind.Warning
        cardReferral.IconLevel = AlertLevel.Danger
        cardReferral.Location = New Point(225, 0)
        cardReferral.Margin = New Padding(0, 0, 12, 12)
        cardReferral.Name = "cardReferral"
        cardReferral.Padding = New Padding(16, 12, 16, 10)
        cardReferral.Size = New Size(213, 108)
        cardReferral.TabIndex = 1
        cardReferral.Title = "ต้องส่งต่อคลินิก"
        '
        'lblReferral
        '
        lblReferral.Dock = DockStyle.Fill
        lblReferral.Font = New Font("Leelawadee UI", 18.0F, FontStyle.Bold)
        lblReferral.ForeColor = Color.FromArgb(CByte(190), CByte(18), CByte(60))
        lblReferral.Location = New Point(16, 52)
        lblReferral.Name = "lblReferral"
        lblReferral.Size = New Size(181, 46)
        lblReferral.TextAlign = ContentAlignment.MiddleLeft
        lblReferral.TabIndex = 0
        lblReferral.Text = "-"
        '
        'cardInjury
        '
        cardInjury.Controls.Add(lblInjury)
        cardInjury.Dock = DockStyle.Fill
        cardInjury.IconKind = IconKind.Briefcase
        cardInjury.IconLevel = AlertLevel.Warning
        cardInjury.Location = New Point(450, 0)
        cardInjury.Margin = New Padding(0, 0, 12, 12)
        cardInjury.Name = "cardInjury"
        cardInjury.Padding = New Padding(16, 12, 16, 10)
        cardInjury.Size = New Size(213, 108)
        cardInjury.TabIndex = 2
        cardInjury.Title = "เจ็บป่วยจากงาน"
        '
        'lblInjury
        '
        lblInjury.Dock = DockStyle.Fill
        lblInjury.Font = New Font("Leelawadee UI", 18.0F, FontStyle.Bold)
        lblInjury.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblInjury.Location = New Point(16, 52)
        lblInjury.Name = "lblInjury"
        lblInjury.Size = New Size(181, 46)
        lblInjury.TextAlign = ContentAlignment.MiddleLeft
        lblInjury.TabIndex = 0
        lblInjury.Text = "-"
        '
        'cardDust
        '
        cardDust.Controls.Add(lblDust)
        cardDust.Dock = DockStyle.Fill
        cardDust.IconKind = IconKind.Droplet
        cardDust.IconLevel = AlertLevel.Info
        cardDust.Location = New Point(675, 0)
        cardDust.Margin = New Padding(0, 0, 0, 12)
        cardDust.Name = "cardDust"
        cardDust.Padding = New Padding(16, 12, 16, 10)
        cardDust.Size = New Size(225, 108)
        cardDust.TabIndex = 3
        cardDust.Title = "ฝุ่น PM2.5"
        '
        'lblDust
        '
        lblDust.Dock = DockStyle.Fill
        lblDust.Font = New Font("Leelawadee UI", 18.0F, FontStyle.Bold)
        lblDust.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblDust.Location = New Point(16, 52)
        lblDust.Name = "lblDust"
        lblDust.Size = New Size(193, 46)
        lblDust.TextAlign = ContentAlignment.MiddleLeft
        lblDust.TabIndex = 0
        lblDust.Text = "-"
        '
        'pnlInfo
        '
        pnlInfo.Controls.Add(lblInfo)
        pnlInfo.Dock = DockStyle.Fill
        pnlInfo.Location = New Point(0, 256)
        pnlInfo.Margin = New Padding(0)
        pnlInfo.Name = "pnlInfo"
        pnlInfo.Size = New Size(900, 34)
        pnlInfo.TabIndex = 2
        '
        'lblInfo
        '
        lblInfo.AutoSize = True
        lblInfo.Font = New Font("Leelawadee UI", 9.5F)
        lblInfo.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblInfo.Location = New Point(1, 8)
        lblInfo.Name = "lblInfo"
        lblInfo.Size = New Size(10, 17)
        lblInfo.TabIndex = 0
        '
        'cardResult
        '
        cardResult.Controls.Add(dgvRegistry)
        cardResult.Controls.Add(emptyRegistry)
        cardResult.Dock = DockStyle.Fill
        cardResult.Location = New Point(0, 290)
        cardResult.Margin = New Padding(0)
        cardResult.Name = "cardResult"
        cardResult.Padding = New Padding(10)
        cardResult.Size = New Size(900, 244)
        cardResult.TabIndex = 3
        '
        'dgvRegistry
        '
        dgvRegistry.Dock = DockStyle.Fill
        dgvRegistry.Location = New Point(10, 10)
        dgvRegistry.Name = "dgvRegistry"
        dgvRegistry.Size = New Size(880, 224)
        dgvRegistry.TabIndex = 0
        dgvRegistry.Visible = False
        '
        'emptyRegistry
        '
        emptyRegistry.BackColor = Color.White
        emptyRegistry.Description = "เลือกช่วงวันที่ แล้วกด ""แสดงทะเบียน"""
        emptyRegistry.Dock = DockStyle.Fill
        emptyRegistry.IconKind = IconKind.Briefcase
        emptyRegistry.Location = New Point(10, 10)
        emptyRegistry.Name = "emptyRegistry"
        emptyRegistry.Size = New Size(880, 224)
        emptyRegistry.TabIndex = 1
        emptyRegistry.Title = "ยังไม่ได้แสดงทะเบียน"
        '
        'OccupationalRegistryPage
        '
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        AutoScroll = True
        AutoScrollMinSize = New Size(880, 520)
        BackColor = Color.FromArgb(CByte(244), CByte(246), CByte(250))
        Controls.Add(tlpMain)
        Controls.Add(pnlPageHeader)
        Font = New Font("Leelawadee UI", 10.0F)
        Name = "OccupationalRegistryPage"
        Size = New Size(900, 600)
        pnlPageHeader.ResumeLayout(False)
        pnlPageHeader.PerformLayout()
        tlpMain.ResumeLayout(False)
        cardFilter.ResumeLayout(False)
        tlpFilter.ResumeLayout(False)
        tlpFilter.PerformLayout()
        flpActions.ResumeLayout(False)
        flpActions.PerformLayout()
        tlpStats.ResumeLayout(False)
        cardTotal.ResumeLayout(False)
        cardReferral.ResumeLayout(False)
        cardInjury.ResumeLayout(False)
        cardDust.ResumeLayout(False)
        pnlInfo.ResumeLayout(False)
        pnlInfo.PerformLayout()
        cardResult.ResumeLayout(False)
        CType(dgvRegistry, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlPageHeader As Panel
    Friend WithEvents lblSubtitle As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents cardFilter As CardPanel
    Friend WithEvents tlpFilter As TableLayoutPanel
    Friend WithEvents lblRange As Label
    Friend WithEvents lblFrom As Label
    Friend WithEvents lblTo As Label
    Friend WithEvents lblSearch As Label
    Friend WithEvents segRange As SegmentedControl
    Friend WithEvents dtpFrom As DateTimePicker
    Friend WithEvents dtpTo As DateTimePicker
    Friend WithEvents txtSearch As ModernTextBox
    Friend WithEvents flpActions As FlowLayoutPanel
    Friend WithEvents btnShow As ModernButton
    Friend WithEvents btnExport As ModernButton
    Friend WithEvents tlpStats As TableLayoutPanel
    Friend WithEvents cardTotal As CardPanel
    Friend WithEvents lblTotal As Label
    Friend WithEvents cardReferral As CardPanel
    Friend WithEvents lblReferral As Label
    Friend WithEvents cardInjury As CardPanel
    Friend WithEvents lblInjury As Label
    Friend WithEvents cardDust As CardPanel
    Friend WithEvents lblDust As Label
    Friend WithEvents cardResult As CardPanel
    Friend WithEvents dgvRegistry As DataGridView
    Friend WithEvents emptyRegistry As EmptyState
    Friend WithEvents pnlInfo As Panel
    Friend WithEvents lblInfo As Label
    Friend WithEvents tip As ToolTip
End Class

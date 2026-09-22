<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LabHemodialysisPage
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
        lblFrom = New Label()
        lblTo = New Label()
        lblNote = New Label()
        dtpFrom = New DateTimePicker()
        dtpTo = New DateTimePicker()
        flpActions = New FlowLayoutPanel()
        btnShow = New ModernButton()
        btnReset = New ModernButton()
        btnExport = New ModernButton()
        btnPrint = New ModernButton()
        pnlInfo = New Panel()
        lblInfo = New Label()
        cardResult = New CardPanel()
        dgvLab = New DataGridView()
        emptyLab = New EmptyState()
        tip = New ToolTip(components)
        pnlPageHeader.SuspendLayout()
        tlpMain.SuspendLayout()
        cardFilter.SuspendLayout()
        tlpFilter.SuspendLayout()
        flpActions.SuspendLayout()
        pnlInfo.SuspendLayout()
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
        lblSubtitle.Size = New Size(560, 17)
        lblSubtitle.TabIndex = 1
        lblSubtitle.Text = "ชุดผล LAB มาตรฐานของผู้ป่วยฟอกไต · แถวคือรายการตรวจ คอลัมน์คือวันที่รายงานผล"
        '
        'lblTitle
        '
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Leelawadee UI", 16.0F, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblTitle.Location = New Point(-2, 0)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(400, 30)
        lblTitle.TabIndex = 0
        lblTitle.Text = "LAB Template Hemodialysis"
        '
        'tlpMain
        '
        tlpMain.ColumnCount = 1
        tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        tlpMain.Controls.Add(cardFilter, 0, 0)
        tlpMain.Controls.Add(pnlInfo, 0, 1)
        tlpMain.Controls.Add(cardResult, 0, 2)
        tlpMain.Dock = DockStyle.Fill
        tlpMain.Location = New Point(0, 66)
        tlpMain.Name = "tlpMain"
        tlpMain.RowCount = 3
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 152.0F))
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 34.0F))
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpMain.Size = New Size(900, 534)
        tlpMain.TabIndex = 1
        '
        'cardFilter
        '
        cardFilter.Controls.Add(tlpFilter)
        cardFilter.Dock = DockStyle.Fill
        cardFilter.IconKind = IconKind.Calendar
        cardFilter.Location = New Point(0, 0)
        cardFilter.Margin = New Padding(0, 0, 0, 12)
        cardFilter.Name = "cardFilter"
        cardFilter.Size = New Size(900, 140)
        cardFilter.TabIndex = 0
        cardFilter.Title = "ช่วงวันที่"
        '
        'tlpFilter
        '
        tlpFilter.ColumnCount = 4
        tlpFilter.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 170.0F))
        tlpFilter.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 170.0F))
        tlpFilter.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        tlpFilter.ColumnStyles.Add(New ColumnStyle())
        tlpFilter.Controls.Add(lblFrom, 0, 0)
        tlpFilter.Controls.Add(lblTo, 1, 0)
        tlpFilter.Controls.Add(dtpFrom, 0, 1)
        tlpFilter.Controls.Add(dtpTo, 1, 1)
        tlpFilter.Controls.Add(lblNote, 2, 1)
        tlpFilter.Controls.Add(flpActions, 3, 1)
        tlpFilter.Dock = DockStyle.Fill
        tlpFilter.Location = New Point(16, 56)
        tlpFilter.Name = "tlpFilter"
        tlpFilter.RowCount = 2
        tlpFilter.RowStyles.Add(New RowStyle(SizeType.Absolute, 24.0F))
        tlpFilter.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpFilter.Size = New Size(868, 68)
        tlpFilter.TabIndex = 0
        '
        'lblFrom
        '
        lblFrom.AutoSize = True
        lblFrom.Font = New Font("Leelawadee UI", 9.0F)
        lblFrom.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblFrom.Location = New Point(3, 0)
        lblFrom.Name = "lblFrom"
        lblFrom.Size = New Size(120, 16)
        lblFrom.TabIndex = 0
        lblFrom.Text = "วันที่เริ่มต้น (ติ๊กเพื่อใช้)"
        '
        'lblTo
        '
        lblTo.AutoSize = True
        lblTo.Font = New Font("Leelawadee UI", 9.0F)
        lblTo.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblTo.Location = New Point(173, 0)
        lblTo.Name = "lblTo"
        lblTo.Size = New Size(110, 16)
        lblTo.TabIndex = 1
        lblTo.Text = "ถึงวันที่ (ติ๊กเพื่อใช้)"
        '
        'dtpFrom
        '
        dtpFrom.Dock = DockStyle.Top
        dtpFrom.Font = New Font("Leelawadee UI", 10.5F)
        dtpFrom.Format = DateTimePickerFormat.Short
        dtpFrom.Location = New Point(3, 30)
        dtpFrom.Margin = New Padding(3, 6, 12, 3)
        dtpFrom.Name = "dtpFrom"
        dtpFrom.ShowCheckBox = True
        dtpFrom.Size = New Size(155, 28)
        dtpFrom.TabIndex = 2
        '
        'dtpTo
        '
        dtpTo.Dock = DockStyle.Top
        dtpTo.Font = New Font("Leelawadee UI", 10.5F)
        dtpTo.Format = DateTimePickerFormat.Short
        dtpTo.Location = New Point(173, 30)
        dtpTo.Margin = New Padding(3, 6, 12, 3)
        dtpTo.Name = "dtpTo"
        dtpTo.ShowCheckBox = True
        dtpTo.Size = New Size(155, 28)
        dtpTo.TabIndex = 3
        '
        'lblNote
        '
        lblNote.Dock = DockStyle.Fill
        lblNote.Font = New Font("Leelawadee UI", 8.5F)
        lblNote.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        lblNote.Location = New Point(343, 24)
        lblNote.Name = "lblNote"
        lblNote.Padding = New Padding(8, 0, 8, 0)
        lblNote.Size = New Size(230, 44)
        lblNote.TabIndex = 4
        lblNote.Text = "ไม่ติ๊กวันที่ = แสดงผลตรวจล่าสุด 10 ครั้ง" & Global.Microsoft.VisualBasic.ChrW(10) & "ติ๊กช่วงวันที่ = ล่าสุด 10 ครั้งในช่วงนั้น"
        lblNote.TextAlign = ContentAlignment.MiddleLeft
        '
        'flpActions
        '
        flpActions.AutoSize = True
        flpActions.AutoSizeMode = AutoSizeMode.GrowAndShrink
        flpActions.Controls.Add(btnShow)
        flpActions.Controls.Add(btnReset)
        flpActions.Controls.Add(btnPrint)
        flpActions.Controls.Add(btnExport)
        flpActions.Location = New Point(579, 24)
        flpActions.Margin = New Padding(6, 0, 0, 0)
        flpActions.Name = "flpActions"
        flpActions.Size = New Size(407, 44)
        flpActions.TabIndex = 5
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
        btnShow.Text = "ค้นหาข้อมูล"
        '
        'btnReset
        '
        btnReset.AutoSize = True
        btnReset.AutoSizeMode = AutoSizeMode.GrowAndShrink
        btnReset.IconKind = IconKind.Refresh
        btnReset.Kind = ButtonVariant.Ghost
        btnReset.Location = New Point(121, 3)
        btnReset.Name = "btnReset"
        btnReset.Size = New Size(100, 38)
        btnReset.TabIndex = 1
        btnReset.Text = "ล้างช่วงวันที่"
        '
        'btnPrint
        '
        btnPrint.AutoSize = True
        btnPrint.AutoSizeMode = AutoSizeMode.GrowAndShrink
        btnPrint.IconKind = IconKind.Printer
        btnPrint.Kind = ButtonVariant.Secondary
        btnPrint.Location = New Point(227, 3)
        btnPrint.Name = "btnPrint"
        btnPrint.Size = New Size(112, 38)
        btnPrint.TabIndex = 2
        btnPrint.Text = "พิมพ์ใบ LAB"
        '
        'btnExport
        '
        btnExport.AutoSize = True
        btnExport.AutoSizeMode = AutoSizeMode.GrowAndShrink
        btnExport.IconKind = IconKind.Download
        btnExport.Kind = ButtonVariant.Secondary
        btnExport.Location = New Point(345, 3)
        btnExport.Name = "btnExport"
        btnExport.Size = New Size(112, 38)
        btnExport.TabIndex = 3
        btnExport.Text = "ส่งออก Excel"
        '
        'pnlInfo
        '
        pnlInfo.Controls.Add(lblInfo)
        pnlInfo.Dock = DockStyle.Fill
        pnlInfo.Location = New Point(0, 152)
        pnlInfo.Margin = New Padding(0)
        pnlInfo.Name = "pnlInfo"
        pnlInfo.Size = New Size(900, 34)
        pnlInfo.TabIndex = 1
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
        cardResult.Controls.Add(dgvLab)
        cardResult.Controls.Add(emptyLab)
        cardResult.Dock = DockStyle.Fill
        cardResult.Location = New Point(0, 186)
        cardResult.Margin = New Padding(0)
        cardResult.Name = "cardResult"
        cardResult.Padding = New Padding(10)
        cardResult.Size = New Size(900, 348)
        cardResult.TabIndex = 2
        '
        'dgvLab
        '
        dgvLab.Dock = DockStyle.Fill
        dgvLab.Location = New Point(10, 10)
        dgvLab.Name = "dgvLab"
        dgvLab.Size = New Size(880, 328)
        dgvLab.TabIndex = 0
        dgvLab.Visible = False
        '
        'emptyLab
        '
        emptyLab.BackColor = Color.White
        emptyLab.Description = "กำลังโหลดชุดผล LAB ของผู้ป่วยฟอกไต..."
        emptyLab.Dock = DockStyle.Fill
        emptyLab.IconKind = IconKind.Droplet
        emptyLab.Location = New Point(10, 10)
        emptyLab.Name = "emptyLab"
        emptyLab.Size = New Size(880, 328)
        emptyLab.TabIndex = 1
        emptyLab.Title = "กำลังโหลด"
        '
        'LabHemodialysisPage
        '
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        AutoScroll = True
        AutoScrollMinSize = New Size(880, 520)
        BackColor = Color.FromArgb(CByte(244), CByte(246), CByte(250))
        Controls.Add(tlpMain)
        Controls.Add(pnlPageHeader)
        Font = New Font("Leelawadee UI", 10.0F)
        Name = "LabHemodialysisPage"
        Size = New Size(900, 600)
        pnlPageHeader.ResumeLayout(False)
        pnlPageHeader.PerformLayout()
        tlpMain.ResumeLayout(False)
        cardFilter.ResumeLayout(False)
        tlpFilter.ResumeLayout(False)
        tlpFilter.PerformLayout()
        flpActions.ResumeLayout(False)
        flpActions.PerformLayout()
        pnlInfo.ResumeLayout(False)
        pnlInfo.PerformLayout()
        cardResult.ResumeLayout(False)
        CType(dgvLab, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlPageHeader As Panel
    Friend WithEvents lblSubtitle As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents cardFilter As CardPanel
    Friend WithEvents tlpFilter As TableLayoutPanel
    Friend WithEvents lblFrom As Label
    Friend WithEvents lblTo As Label
    Friend WithEvents lblNote As Label
    Friend WithEvents dtpFrom As DateTimePicker
    Friend WithEvents dtpTo As DateTimePicker
    Friend WithEvents flpActions As FlowLayoutPanel
    Friend WithEvents btnShow As ModernButton
    Friend WithEvents btnReset As ModernButton
    Friend WithEvents btnPrint As ModernButton
    Friend WithEvents btnExport As ModernButton
    Friend WithEvents pnlInfo As Panel
    Friend WithEvents lblInfo As Label
    Friend WithEvents cardResult As CardPanel
    Friend WithEvents dgvLab As DataGridView
    Friend WithEvents emptyLab As EmptyState
    Friend WithEvents tip As ToolTip
End Class

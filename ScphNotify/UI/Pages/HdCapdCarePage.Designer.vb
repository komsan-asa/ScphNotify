<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class HdCapdCarePage
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
        tlpTop = New TableLayoutPanel()
        cardModality = New CardPanel()
        flpModality = New FlowLayoutPanel()
        cardSessions = New CardPanel()
        chartSessions = New LineChart()
        cardVitals = New CardPanel()
        chartVitals = New LineChart()
        lblVitalNow = New Label()
        tlpMid = New TableLayoutPanel()
        cardLabs = New CardPanel()
        dgvLabs = New DataGridView()
        cardDrugs = New CardPanel()
        dgvDrugs = New DataGridView()
        bnMock = New InfoBanner()
        tlpMockA = New TableLayoutPanel()
        cardFluid = New CardPanel()
        flpFluid = New FlowLayoutPanel()
        cardSessionLog = New CardPanel()
        dgvSessionLog = New DataGridView()
        tlpMockB = New TableLayoutPanel()
        cardAccess = New CardPanel()
        flpAccess = New FlowLayoutPanel()
        cardSymptom = New CardPanel()
        flpSymptom = New FlowLayoutPanel()
        tip = New ToolTip(components)
        pnlPageHeader.SuspendLayout()
        tlpMain.SuspendLayout()
        tlpTop.SuspendLayout()
        cardModality.SuspendLayout()
        cardSessions.SuspendLayout()
        cardVitals.SuspendLayout()
        tlpMid.SuspendLayout()
        cardLabs.SuspendLayout()
        cardDrugs.SuspendLayout()
        tlpMockA.SuspendLayout()
        cardFluid.SuspendLayout()
        cardSessionLog.SuspendLayout()
        tlpMockB.SuspendLayout()
        cardAccess.SuspendLayout()
        cardSymptom.SuspendLayout()
        CType(dgvLabs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvDrugs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvSessionLog, System.ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        '
        'pnlPageHeader
        '
        pnlPageHeader.Controls.Add(lblSubtitle)
        pnlPageHeader.Controls.Add(lblTitle)
        pnlPageHeader.Dock = DockStyle.Top
        pnlPageHeader.Location = New Point(0, 0)
        pnlPageHeader.Name = "pnlPageHeader"
        pnlPageHeader.Size = New Size(980, 66)
        pnlPageHeader.TabIndex = 0
        '
        'lblSubtitle
        '
        lblSubtitle.AutoSize = True
        lblSubtitle.Font = New Font("Leelawadee UI", 9.5F)
        lblSubtitle.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblSubtitle.Location = New Point(1, 33)
        lblSubtitle.Name = "lblSubtitle"
        lblSubtitle.Size = New Size(600, 17)
        lblSubtitle.TabIndex = 1
        lblSubtitle.Text = "ภาพรวมผู้ป่วยบำบัดทดแทนไต — ฟอกเลือด (HD) และล้างไตทางช่องท้อง (PD/CAPD)"
        '
        'lblTitle
        '
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Leelawadee UI", 16.0F, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblTitle.Location = New Point(-2, 0)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(260, 30)
        lblTitle.TabIndex = 0
        lblTitle.Text = "HD/CAPD Care"
        '
        'tlpMain
        '
        tlpMain.ColumnCount = 1
        tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        tlpMain.Controls.Add(tlpTop, 0, 0)
        tlpMain.Controls.Add(tlpMid, 0, 1)
        tlpMain.Controls.Add(bnMock, 0, 2)
        tlpMain.Controls.Add(tlpMockA, 0, 3)
        tlpMain.Controls.Add(tlpMockB, 0, 4)
        tlpMain.Dock = DockStyle.Fill
        tlpMain.Location = New Point(0, 66)
        tlpMain.Name = "tlpMain"
        tlpMain.RowCount = 5
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 320.0F))
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 306.0F))
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 74.0F))
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 356.0F))
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 320.0F))
        tlpMain.Size = New Size(980, 1324)
        tlpMain.TabIndex = 1
        '
        'tlpTop
        '
        tlpTop.ColumnCount = 3
        tlpTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 32.0F))
        tlpTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 34.0F))
        tlpTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 34.0F))
        tlpTop.Controls.Add(cardModality, 0, 0)
        tlpTop.Controls.Add(cardSessions, 1, 0)
        tlpTop.Controls.Add(cardVitals, 2, 0)
        tlpTop.Dock = DockStyle.Fill
        tlpTop.Location = New Point(0, 0)
        tlpTop.Margin = New Padding(0, 0, 0, 12)
        tlpTop.Name = "tlpTop"
        tlpTop.RowCount = 1
        tlpTop.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpTop.Size = New Size(980, 256)
        tlpTop.TabIndex = 0
        '
        'cardModality
        '
        cardModality.Controls.Add(flpModality)
        cardModality.Dock = DockStyle.Fill
        cardModality.IconKind = IconKind.Kidney
        cardModality.Location = New Point(0, 0)
        cardModality.Margin = New Padding(0, 0, 12, 0)
        cardModality.Name = "cardModality"
        cardModality.Size = New Size(301, 256)
        cardModality.Subtitle = "จากรายการค่าบริการใน HOSxP"
        cardModality.TabIndex = 0
        cardModality.Title = "การบำบัดทดแทนไต"
        '
        'flpModality
        '
        flpModality.AutoScroll = True
        flpModality.Dock = DockStyle.Fill
        flpModality.FlowDirection = FlowDirection.TopDown
        flpModality.Location = New Point(16, 70)
        flpModality.Name = "flpModality"
        flpModality.Size = New Size(269, 170)
        flpModality.TabIndex = 0
        flpModality.WrapContents = False
        '
        'cardSessions
        '
        cardSessions.Controls.Add(chartSessions)
        cardSessions.Dock = DockStyle.Fill
        cardSessions.IconKind = IconKind.Calendar
        cardSessions.Location = New Point(313, 0)
        cardSessions.Margin = New Padding(0, 0, 12, 0)
        cardSessions.Name = "cardSessions"
        cardSessions.Size = New Size(321, 256)
        cardSessions.Subtitle = "จำนวนครั้งต่อเดือน"
        cardSessions.TabIndex = 1
        cardSessions.Title = "ความถี่การล้างไต"
        '
        'chartSessions
        '
        chartSessions.Dock = DockStyle.Fill
        chartSessions.EmptyText = "ไม่พบรายการล้างไตในช่วงที่เลือก"
        chartSessions.Location = New Point(16, 70)
        chartSessions.Name = "chartSessions"
        chartSessions.Size = New Size(289, 170)
        chartSessions.TabIndex = 0
        chartSessions.ValueFormat = "0"
        '
        'cardVitals
        '
        cardVitals.Controls.Add(chartVitals)
        cardVitals.Controls.Add(lblVitalNow)
        cardVitals.Dock = DockStyle.Fill
        cardVitals.IconKind = IconKind.Gauge
        cardVitals.Location = New Point(646, 0)
        cardVitals.Margin = New Padding(0)
        cardVitals.Name = "cardVitals"
        cardVitals.Size = New Size(334, 256)
        cardVitals.Subtitle = "จากจุดคัดกรอง (opdscreen)"
        cardVitals.TabIndex = 2
        cardVitals.Title = "น้ำหนัก / ความดัน"
        '
        'chartVitals
        '
        chartVitals.Dock = DockStyle.Fill
        chartVitals.EmptyText = "ไม่พบน้ำหนักที่จุดคัดกรอง"
        chartVitals.Location = New Point(16, 70)
        chartVitals.Name = "chartVitals"
        chartVitals.Size = New Size(302, 146)
        chartVitals.TabIndex = 0
        chartVitals.ValueFormat = "0.0"
        '
        'lblVitalNow
        '
        lblVitalNow.Dock = DockStyle.Bottom
        lblVitalNow.Font = New Font("Leelawadee UI", 9.5F)
        lblVitalNow.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblVitalNow.Location = New Point(16, 216)
        lblVitalNow.Name = "lblVitalNow"
        lblVitalNow.Size = New Size(302, 24)
        lblVitalNow.TabIndex = 1
        lblVitalNow.TextAlign = ContentAlignment.MiddleLeft
        '
        'tlpMid
        '
        tlpMid.ColumnCount = 2
        tlpMid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 54.0F))
        tlpMid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 46.0F))
        tlpMid.Controls.Add(cardLabs, 0, 0)
        tlpMid.Controls.Add(cardDrugs, 1, 0)
        tlpMid.Dock = DockStyle.Fill
        tlpMid.Location = New Point(0, 268)
        tlpMid.Margin = New Padding(0, 0, 0, 12)
        tlpMid.Name = "tlpMid"
        tlpMid.RowCount = 1
        tlpMid.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpMid.Size = New Size(980, 294)
        tlpMid.TabIndex = 1
        '
        'cardLabs
        '
        cardLabs.Controls.Add(dgvLabs)
        cardLabs.Dock = DockStyle.Fill
        cardLabs.IconKind = IconKind.Flask
        cardLabs.Location = New Point(0, 0)
        cardLabs.Margin = New Padding(0, 0, 12, 0)
        cardLabs.Name = "cardLabs"
        cardLabs.Padding = New Padding(10)
        cardLabs.Size = New Size(517, 294)
        cardLabs.Subtitle = "ค่าล่าสุดเทียบครั้งก่อน"
        cardLabs.TabIndex = 0
        cardLabs.Title = "ผล LAB สำคัญ"
        '
        'dgvLabs
        '
        dgvLabs.Dock = DockStyle.Fill
        dgvLabs.Location = New Point(10, 64)
        dgvLabs.Name = "dgvLabs"
        dgvLabs.Size = New Size(497, 220)
        dgvLabs.TabIndex = 0
        '
        'cardDrugs
        '
        cardDrugs.Controls.Add(dgvDrugs)
        cardDrugs.Dock = DockStyle.Fill
        cardDrugs.IconKind = IconKind.Pill
        cardDrugs.Location = New Point(529, 0)
        cardDrugs.Margin = New Padding(0)
        cardDrugs.Name = "cardDrugs"
        cardDrugs.Padding = New Padding(10)
        cardDrugs.Size = New Size(451, 294)
        cardDrugs.Subtitle = "ที่เกี่ยวข้องกับผู้ป่วยล้างไต"
        cardDrugs.TabIndex = 1
        cardDrugs.Title = "ยาที่ได้รับ"
        '
        'dgvDrugs
        '
        dgvDrugs.Dock = DockStyle.Fill
        dgvDrugs.Location = New Point(10, 64)
        dgvDrugs.Name = "dgvDrugs"
        dgvDrugs.Size = New Size(431, 220)
        dgvDrugs.TabIndex = 0
        '
        'bnMock
        '
        bnMock.Dock = DockStyle.Fill
        bnMock.IconKind = IconKind.Info
        bnMock.Level = AlertLevel.Warning
        bnMock.Location = New Point(0, 574)
        bnMock.Margin = New Padding(0, 0, 0, 12)
        bnMock.Name = "bnMock"
        bnMock.Size = New Size(980, 62)
        bnMock.TabIndex = 2
        '
        'tlpMockA
        '
        tlpMockA.ColumnCount = 2
        tlpMockA.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 34.0F))
        tlpMockA.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 66.0F))
        tlpMockA.Controls.Add(cardFluid, 0, 0)
        tlpMockA.Controls.Add(cardSessionLog, 1, 0)
        tlpMockA.Dock = DockStyle.Fill
        tlpMockA.Location = New Point(0, 648)
        tlpMockA.Margin = New Padding(0, 0, 0, 12)
        tlpMockA.Name = "tlpMockA"
        tlpMockA.RowCount = 1
        tlpMockA.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpMockA.Size = New Size(980, 344)
        tlpMockA.TabIndex = 3
        '
        'cardFluid
        '
        cardFluid.Controls.Add(flpFluid)
        cardFluid.Dock = DockStyle.Fill
        cardFluid.HeaderRightText = "ตัวอย่าง"
        cardFluid.IconKind = IconKind.Droplet
        cardFluid.IconLevel = AlertLevel.Neutral
        cardFluid.Location = New Point(0, 0)
        cardFluid.Margin = New Padding(0, 0, 12, 0)
        cardFluid.Name = "cardFluid"
        cardFluid.Size = New Size(321, 344)
        cardFluid.Subtitle = "Dry weight · UF · ปัสสาวะ"
        cardFluid.TabIndex = 0
        cardFluid.Title = "สมดุลน้ำ"
        '
        'flpFluid
        '
        flpFluid.Dock = DockStyle.Fill
        flpFluid.FlowDirection = FlowDirection.TopDown
        flpFluid.Location = New Point(16, 70)
        flpFluid.Name = "flpFluid"
        flpFluid.Size = New Size(289, 258)
        flpFluid.TabIndex = 0
        flpFluid.WrapContents = False
        '
        'cardSessionLog
        '
        cardSessionLog.Controls.Add(dgvSessionLog)
        cardSessionLog.Dock = DockStyle.Fill
        cardSessionLog.HeaderRightText = "ตัวอย่าง"
        cardSessionLog.IconKind = IconKind.ClipboardCheck
        cardSessionLog.IconLevel = AlertLevel.Neutral
        cardSessionLog.Location = New Point(333, 0)
        cardSessionLog.Margin = New Padding(0)
        cardSessionLog.Name = "cardSessionLog"
        cardSessionLog.Padding = New Padding(10)
        cardSessionLog.Size = New Size(647, 344)
        cardSessionLog.Subtitle = "ค่าที่ต้องบันทึกทุกรอบ"
        cardSessionLog.TabIndex = 1
        cardSessionLog.Title = "บันทึกรอบล้างไต"
        '
        'dgvSessionLog
        '
        dgvSessionLog.Dock = DockStyle.Fill
        dgvSessionLog.Location = New Point(10, 64)
        dgvSessionLog.Name = "dgvSessionLog"
        dgvSessionLog.Size = New Size(627, 270)
        dgvSessionLog.TabIndex = 0
        '
        'tlpMockB
        '
        tlpMockB.ColumnCount = 2
        tlpMockB.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tlpMockB.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tlpMockB.Controls.Add(cardAccess, 0, 0)
        tlpMockB.Controls.Add(cardSymptom, 1, 0)
        tlpMockB.Dock = DockStyle.Fill
        tlpMockB.Location = New Point(0, 1004)
        tlpMockB.Margin = New Padding(0)
        tlpMockB.Name = "tlpMockB"
        tlpMockB.RowCount = 1
        tlpMockB.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpMockB.Size = New Size(980, 320)
        tlpMockB.TabIndex = 4
        '
        'cardAccess
        '
        cardAccess.Controls.Add(flpAccess)
        cardAccess.Dock = DockStyle.Fill
        cardAccess.HeaderRightText = "ตัวอย่าง"
        cardAccess.IconKind = IconKind.Activity
        cardAccess.IconLevel = AlertLevel.Neutral
        cardAccess.Location = New Point(0, 0)
        cardAccess.Margin = New Padding(0, 0, 12, 0)
        cardAccess.Name = "cardAccess"
        cardAccess.Size = New Size(478, 320)
        cardAccess.Subtitle = "AVF / AVG / Catheter / Exit site"
        cardAccess.TabIndex = 0
        cardAccess.Title = "ทางเข้าออกหลอดเลือด"
        '
        'flpAccess
        '
        flpAccess.AutoScroll = True
        flpAccess.Dock = DockStyle.Fill
        flpAccess.FlowDirection = FlowDirection.TopDown
        flpAccess.Location = New Point(16, 70)
        flpAccess.Name = "flpAccess"
        flpAccess.Size = New Size(446, 234)
        flpAccess.TabIndex = 0
        flpAccess.WrapContents = False
        '
        'cardSymptom
        '
        cardSymptom.Controls.Add(flpSymptom)
        cardSymptom.Dock = DockStyle.Fill
        cardSymptom.HeaderRightText = "ตัวอย่าง"
        cardSymptom.IconKind = IconKind.Warning
        cardSymptom.IconLevel = AlertLevel.Neutral
        cardSymptom.Location = New Point(490, 0)
        cardSymptom.Margin = New Padding(0)
        cardSymptom.Name = "cardSymptom"
        cardSymptom.Size = New Size(490, 320)
        cardSymptom.Subtitle = "บันทึกโดยพยาบาลทุกรอบ"
        cardSymptom.TabIndex = 1
        cardSymptom.Title = "อาการเฝ้าระวัง"
        '
        'flpSymptom
        '
        flpSymptom.AutoScroll = True
        flpSymptom.Dock = DockStyle.Fill
        flpSymptom.Location = New Point(16, 70)
        flpSymptom.Name = "flpSymptom"
        flpSymptom.Size = New Size(458, 234)
        flpSymptom.TabIndex = 0
        '
        'HdCapdCarePage
        '
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        AutoScroll = True
        AutoScrollMinSize = New Size(940, 1400)
        BackColor = Color.FromArgb(CByte(244), CByte(246), CByte(250))
        Controls.Add(tlpMain)
        Controls.Add(pnlPageHeader)
        Font = New Font("Leelawadee UI", 10.0F)
        Name = "HdCapdCarePage"
        Size = New Size(980, 700)
        pnlPageHeader.ResumeLayout(False)
        pnlPageHeader.PerformLayout()
        tlpMain.ResumeLayout(False)
        tlpTop.ResumeLayout(False)
        cardModality.ResumeLayout(False)
        cardSessions.ResumeLayout(False)
        cardVitals.ResumeLayout(False)
        tlpMid.ResumeLayout(False)
        cardLabs.ResumeLayout(False)
        cardDrugs.ResumeLayout(False)
        tlpMockA.ResumeLayout(False)
        cardFluid.ResumeLayout(False)
        cardSessionLog.ResumeLayout(False)
        tlpMockB.ResumeLayout(False)
        cardAccess.ResumeLayout(False)
        cardSymptom.ResumeLayout(False)
        CType(dgvLabs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(dgvDrugs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(dgvSessionLog, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlPageHeader As Panel
    Friend WithEvents lblSubtitle As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents tlpTop As TableLayoutPanel
    Friend WithEvents cardModality As CardPanel
    Friend WithEvents flpModality As FlowLayoutPanel
    Friend WithEvents cardSessions As CardPanel
    Friend WithEvents chartSessions As LineChart
    Friend WithEvents cardVitals As CardPanel
    Friend WithEvents chartVitals As LineChart
    Friend WithEvents lblVitalNow As Label
    Friend WithEvents tlpMid As TableLayoutPanel
    Friend WithEvents cardLabs As CardPanel
    Friend WithEvents dgvLabs As DataGridView
    Friend WithEvents cardDrugs As CardPanel
    Friend WithEvents dgvDrugs As DataGridView
    Friend WithEvents bnMock As InfoBanner
    Friend WithEvents tlpMockA As TableLayoutPanel
    Friend WithEvents cardFluid As CardPanel
    Friend WithEvents flpFluid As FlowLayoutPanel
    Friend WithEvents cardSessionLog As CardPanel
    Friend WithEvents dgvSessionLog As DataGridView
    Friend WithEvents tlpMockB As TableLayoutPanel
    Friend WithEvents cardAccess As CardPanel
    Friend WithEvents flpAccess As FlowLayoutPanel
    Friend WithEvents cardSymptom As CardPanel
    Friend WithEvents flpSymptom As FlowLayoutPanel
    Friend WithEvents tip As ToolTip
End Class

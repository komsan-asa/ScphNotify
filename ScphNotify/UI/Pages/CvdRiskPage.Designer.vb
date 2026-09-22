<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CvdRiskPage
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
        cardLatest = New CardPanel()
        lblLatestDate = New Label()
        lblLevel = New Label()
        lblScore = New Label()
        cardScale = New CardPanel()
        riskBar = New RiskScaleBar()
        tlpBottom = New TableLayoutPanel()
        cardHistory = New CardPanel()
        dgvCvd = New DataGridView()
        cardTrend = New CardPanel()
        chartCvd = New LineChart()
        emptyCvd = New EmptyState()
        pnlPageHeader.SuspendLayout()
        tlpMain.SuspendLayout()
        tlpTop.SuspendLayout()
        cardLatest.SuspendLayout()
        cardScale.SuspendLayout()
        tlpBottom.SuspendLayout()
        cardHistory.SuspendLayout()
        CType(dgvCvd, System.ComponentModel.ISupportInitialize).BeginInit()
        cardTrend.SuspendLayout()
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
        lblSubtitle.Size = New Size(480, 17)
        lblSubtitle.TabIndex = 1
        lblSubtitle.Text = "Thai CV Risk Score · ความเสี่ยงในการเกิดโรคหัวใจและโรคหลอดเลือดสมองในอนาคต"
        '
        'lblTitle
        '
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Leelawadee UI", 16.0F, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblTitle.Location = New Point(-2, 0)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(330, 30)
        lblTitle.TabIndex = 0
        lblTitle.Text = "ความเสี่ยงโรคหัวใจและหลอดเลือด"
        '
        'tlpMain
        '
        tlpMain.ColumnCount = 1
        tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        tlpMain.Controls.Add(tlpTop, 0, 0)
        tlpMain.Controls.Add(tlpBottom, 0, 1)
        tlpMain.Dock = DockStyle.Fill
        tlpMain.Location = New Point(0, 66)
        tlpMain.Name = "tlpMain"
        tlpMain.RowCount = 2
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 188.0F))
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpMain.Size = New Size(900, 534)
        tlpMain.TabIndex = 1
        '
        'tlpTop
        '
        tlpTop.ColumnCount = 2
        tlpTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 32.0F))
        tlpTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 68.0F))
        tlpTop.Controls.Add(cardLatest, 0, 0)
        tlpTop.Controls.Add(cardScale, 1, 0)
        tlpTop.Dock = DockStyle.Fill
        tlpTop.Location = New Point(0, 0)
        tlpTop.Margin = New Padding(0)
        tlpTop.Name = "tlpTop"
        tlpTop.RowCount = 1
        tlpTop.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpTop.Size = New Size(900, 188)
        tlpTop.TabIndex = 0
        '
        'cardLatest
        '
        cardLatest.Controls.Add(lblLatestDate)
        cardLatest.Controls.Add(lblLevel)
        cardLatest.Controls.Add(lblScore)
        cardLatest.Dock = DockStyle.Fill
        cardLatest.IconKind = IconKind.HeartPulse
        cardLatest.IconLevel = AlertLevel.Danger
        cardLatest.Location = New Point(0, 0)
        cardLatest.Margin = New Padding(0, 0, 14, 16)
        cardLatest.Name = "cardLatest"
        cardLatest.Size = New Size(274, 172)
        cardLatest.TabIndex = 0
        cardLatest.Title = "ผลประเมินล่าสุด"
        '
        'lblLatestDate
        '
        lblLatestDate.Dock = DockStyle.Fill
        lblLatestDate.Font = New Font("Leelawadee UI", 9.0F)
        lblLatestDate.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblLatestDate.Location = New Point(16, 134)
        lblLatestDate.Name = "lblLatestDate"
        lblLatestDate.Size = New Size(242, 22)
        lblLatestDate.TabIndex = 2
        lblLatestDate.Text = "-"
        '
        'lblLevel
        '
        lblLevel.Dock = DockStyle.Top
        lblLevel.Font = New Font("Leelawadee UI", 10.5F, FontStyle.Bold)
        lblLevel.Location = New Point(16, 108)
        lblLevel.Name = "lblLevel"
        lblLevel.Size = New Size(242, 26)
        lblLevel.TabIndex = 1
        lblLevel.Text = "-"
        '
        'lblScore
        '
        lblScore.Dock = DockStyle.Top
        lblScore.Font = New Font("Leelawadee UI", 22.0F, FontStyle.Bold)
        lblScore.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblScore.Location = New Point(16, 56)
        lblScore.Name = "lblScore"
        lblScore.Size = New Size(242, 52)
        lblScore.TabIndex = 0
        lblScore.Text = "-"
        '
        'cardScale
        '
        cardScale.Controls.Add(riskBar)
        cardScale.Dock = DockStyle.Fill
        cardScale.IconKind = IconKind.Gauge
        cardScale.Location = New Point(288, 0)
        cardScale.Margin = New Padding(0, 0, 0, 16)
        cardScale.Name = "cardScale"
        cardScale.Padding = New Padding(20, 16, 20, 12)
        cardScale.Size = New Size(612, 172)
        cardScale.TabIndex = 1
        cardScale.Title = "ระดับความเสี่ยง 5 ระดับ"
        '
        'riskBar
        '
        riskBar.Dock = DockStyle.Fill
        riskBar.Location = New Point(20, 56)
        riskBar.Name = "riskBar"
        riskBar.Size = New Size(572, 88)
        riskBar.TabIndex = 0
        '
        'tlpBottom
        '
        tlpBottom.ColumnCount = 2
        tlpBottom.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 62.0F))
        tlpBottom.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 38.0F))
        tlpBottom.Controls.Add(cardHistory, 0, 0)
        tlpBottom.Controls.Add(cardTrend, 1, 0)
        tlpBottom.Dock = DockStyle.Fill
        tlpBottom.Location = New Point(0, 188)
        tlpBottom.Margin = New Padding(0)
        tlpBottom.Name = "tlpBottom"
        tlpBottom.RowCount = 1
        tlpBottom.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpBottom.Size = New Size(900, 362)
        tlpBottom.TabIndex = 1
        '
        'cardHistory
        '
        cardHistory.Controls.Add(dgvCvd)
        cardHistory.Dock = DockStyle.Fill
        cardHistory.IconKind = IconKind.Table
        cardHistory.IconLevel = AlertLevel.Info
        cardHistory.Location = New Point(0, 0)
        cardHistory.Margin = New Padding(0, 0, 14, 0)
        cardHistory.Name = "cardHistory"
        cardHistory.Padding = New Padding(12, 16, 12, 12)
        cardHistory.Size = New Size(508, 362)
        cardHistory.TabIndex = 0
        cardHistory.Title = "ประวัติการประเมิน (10 ครั้งล่าสุด)"
        '
        'dgvCvd
        '
        dgvCvd.Dock = DockStyle.Fill
        dgvCvd.Location = New Point(12, 56)
        dgvCvd.Name = "dgvCvd"
        dgvCvd.Size = New Size(484, 294)
        dgvCvd.TabIndex = 0
        '
        'cardTrend
        '
        cardTrend.Controls.Add(chartCvd)
        cardTrend.Dock = DockStyle.Fill
        cardTrend.IconKind = IconKind.LineChart
        cardTrend.Location = New Point(522, 0)
        cardTrend.Margin = New Padding(0)
        cardTrend.Name = "cardTrend"
        cardTrend.Size = New Size(378, 362)
        cardTrend.TabIndex = 1
        cardTrend.Title = "แนวโน้มคะแนนความเสี่ยง (%)"
        '
        'chartCvd
        '
        chartCvd.Dock = DockStyle.Fill
        chartCvd.Location = New Point(16, 56)
        chartCvd.Name = "chartCvd"
        chartCvd.Size = New Size(346, 290)
        chartCvd.TabIndex = 0
        '
        'emptyCvd
        '
        emptyCvd.Description = "ยังไม่มีข้อมูลที่ใช้คำนวณ Thai CV Risk (ต้องมีผล Cholesterol / LDL / HDL จากการคัดกรอง)"
        emptyCvd.Dock = DockStyle.Fill
        emptyCvd.IconKind = IconKind.HeartPulse
        emptyCvd.Location = New Point(0, 66)
        emptyCvd.Name = "emptyCvd"
        emptyCvd.Size = New Size(900, 534)
        emptyCvd.TabIndex = 2
        emptyCvd.Title = "ไม่มีข้อมูล CVD Risk"
        emptyCvd.Visible = False
        '
        'CvdRiskPage
        '
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        AutoScroll = True
        AutoScrollMinSize = New Size(760, 500)
        BackColor = Color.FromArgb(CByte(244), CByte(246), CByte(250))
        Controls.Add(emptyCvd)
        Controls.Add(tlpMain)
        Controls.Add(pnlPageHeader)
        Font = New Font("Leelawadee UI", 10.0F)
        Name = "CvdRiskPage"
        Size = New Size(900, 600)
        pnlPageHeader.ResumeLayout(False)
        pnlPageHeader.PerformLayout()
        tlpMain.ResumeLayout(False)
        tlpTop.ResumeLayout(False)
        cardLatest.ResumeLayout(False)
        cardScale.ResumeLayout(False)
        tlpBottom.ResumeLayout(False)
        cardHistory.ResumeLayout(False)
        CType(dgvCvd, System.ComponentModel.ISupportInitialize).EndInit()
        cardTrend.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlPageHeader As Panel
    Friend WithEvents lblSubtitle As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents tlpTop As TableLayoutPanel
    Friend WithEvents cardLatest As CardPanel
    Friend WithEvents lblLatestDate As Label
    Friend WithEvents lblLevel As Label
    Friend WithEvents lblScore As Label
    Friend WithEvents cardScale As CardPanel
    Friend WithEvents riskBar As RiskScaleBar
    Friend WithEvents tlpBottom As TableLayoutPanel
    Friend WithEvents cardHistory As CardPanel
    Friend WithEvents dgvCvd As DataGridView
    Friend WithEvents cardTrend As CardPanel
    Friend WithEvents chartCvd As LineChart
    Friend WithEvents emptyCvd As EmptyState
End Class

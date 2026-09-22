<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class OccupationalScreeningDialog
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
        pnlHeader = New Panel()
        lblPatient = New Label()
        lblTitle = New Label()
        tileIcon = New IconTile()
        pnlFooter = New Panel()
        lblVersion = New Label()
        flpFooter = New FlowLayoutPanel()
        btnSave = New ModernButton()
        btnLater = New ModernButton()
        flpBody = New FlowLayoutPanel()
        cardQ1 = New CardPanel()
        lblQ1 = New Label()
        segQ1 = New SegmentedControl()
        cardQ2 = New CardPanel()
        lblQ2 = New Label()
        segQ2 = New SegmentedControl()
        cardQ3 = New CardPanel()
        lblQ3 = New Label()
        segQ3 = New SegmentedControl()
        banner = New InfoBanner()
        pnlHeader.SuspendLayout()
        pnlFooter.SuspendLayout()
        flpFooter.SuspendLayout()
        flpBody.SuspendLayout()
        cardQ1.SuspendLayout()
        cardQ2.SuspendLayout()
        cardQ3.SuspendLayout()
        SuspendLayout()
        '
        'pnlHeader
        '
        pnlHeader.Controls.Add(lblPatient)
        pnlHeader.Controls.Add(lblTitle)
        pnlHeader.Controls.Add(tileIcon)
        pnlHeader.Dock = DockStyle.Top
        pnlHeader.Location = New Point(0, 0)
        pnlHeader.Name = "pnlHeader"
        pnlHeader.Size = New Size(620, 100)
        pnlHeader.TabIndex = 0
        '
        'lblPatient
        '
        lblPatient.AutoEllipsis = True
        lblPatient.Font = New Font("Leelawadee UI", 10.0F)
        lblPatient.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblPatient.Location = New Point(89, 56)
        lblPatient.Name = "lblPatient"
        lblPatient.Size = New Size(500, 22)
        lblPatient.TabIndex = 2
        lblPatient.Text = "-"
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
        lblTitle.Text = "การคัดกรองโรคจากการทำงาน"
        '
        'tileIcon
        '
        tileIcon.IconKind = IconKind.Briefcase
        tileIcon.Level = AlertLevel.Warning
        tileIcon.Location = New Point(28, 26)
        tileIcon.Name = "tileIcon"
        tileIcon.Size = New Size(48, 48)
        tileIcon.TabIndex = 0
        '
        'pnlFooter
        '
        pnlFooter.BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        pnlFooter.Controls.Add(flpFooter)
        pnlFooter.Controls.Add(lblVersion)
        pnlFooter.Dock = DockStyle.Bottom
        pnlFooter.Location = New Point(0, 452)
        pnlFooter.Name = "pnlFooter"
        pnlFooter.Size = New Size(620, 72)
        pnlFooter.TabIndex = 2
        '
        'lblVersion
        '
        lblVersion.AutoSize = True
        lblVersion.Font = New Font("Leelawadee UI", 8.5F)
        lblVersion.ForeColor = Color.FromArgb(CByte(148), CByte(163), CByte(184))
        lblVersion.Location = New Point(28, 28)
        lblVersion.Name = "lblVersion"
        lblVersion.Size = New Size(120, 16)
        lblVersion.TabIndex = 2
        lblVersion.Text = "แบบคัดกรอง v.67.08.08"
        '
        'flpFooter
        '
        flpFooter.AutoSize = True
        flpFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink
        flpFooter.Controls.Add(btnSave)
        flpFooter.Controls.Add(btnLater)
        flpFooter.Dock = DockStyle.Right
        flpFooter.FlowDirection = FlowDirection.RightToLeft
        flpFooter.Location = New Point(380, 0)
        flpFooter.Name = "flpFooter"
        flpFooter.Padding = New Padding(0, 16, 22, 16)
        flpFooter.Size = New Size(240, 72)
        flpFooter.TabIndex = 0
        flpFooter.WrapContents = False
        '
        'btnSave
        '
        btnSave.AutoSize = True
        btnSave.AutoSizeMode = AutoSizeMode.GrowAndShrink
        btnSave.IconKind = IconKind.Check
        btnSave.Location = New Point(112, 19)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(112, 40)
        btnSave.TabIndex = 0
        btnSave.Text = "บันทึกผลคัดกรอง"
        '
        'btnLater
        '
        btnLater.AutoSize = True
        btnLater.AutoSizeMode = AutoSizeMode.GrowAndShrink
        btnLater.DialogResult = DialogResult.Cancel
        btnLater.Kind = ButtonVariant.Ghost
        btnLater.Location = New Point(12, 19)
        btnLater.Name = "btnLater"
        btnLater.Size = New Size(98, 40)
        btnLater.TabIndex = 1
        btnLater.Text = "ภายหลัง"
        '
        'flpBody
        '
        flpBody.Controls.Add(cardQ1)
        flpBody.Controls.Add(cardQ2)
        flpBody.Controls.Add(cardQ3)
        flpBody.Controls.Add(banner)
        flpBody.Dock = DockStyle.Fill
        flpBody.FlowDirection = FlowDirection.TopDown
        flpBody.Location = New Point(0, 100)
        flpBody.Name = "flpBody"
        flpBody.Padding = New Padding(28, 4, 28, 8)
        flpBody.Size = New Size(620, 352)
        flpBody.TabIndex = 1
        flpBody.WrapContents = False
        '
        'cardQ1
        '
        cardQ1.Controls.Add(lblQ1)
        cardQ1.Controls.Add(segQ1)
        cardQ1.Location = New Point(28, 4)
        cardQ1.Margin = New Padding(0, 0, 0, 10)
        cardQ1.Name = "cardQ1"
        cardQ1.Padding = New Padding(18, 14, 14, 14)
        cardQ1.Size = New Size(564, 74)
        cardQ1.TabIndex = 0
        '
        'lblQ1
        '
        lblQ1.Dock = DockStyle.Fill
        lblQ1.Font = New Font("Leelawadee UI", 10.5F)
        lblQ1.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblQ1.Location = New Point(18, 14)
        lblQ1.Name = "lblQ1"
        lblQ1.Size = New Size(352, 46)
        lblQ1.TabIndex = 1
        lblQ1.Text = "1. ท่านคิดว่าการเจ็บป่วยครั้งนี้เกิดจากการทำงานหรือไม่"
        lblQ1.TextAlign = ContentAlignment.MiddleLeft
        '
        'segQ1
        '
        segQ1.Dock = DockStyle.Right
        segQ1.FilledSelection = True
        segQ1.Items = New String() {"ใช่", "ไม่ใช่"}
        segQ1.Location = New Point(380, 14)
        segQ1.Name = "segQ1"
        segQ1.SelectedIndex = 1
        segQ1.SelectedLevel = AlertLevel.Success
        segQ1.Size = New Size(170, 46)
        segQ1.TabIndex = 0
        '
        'cardQ2
        '
        cardQ2.Controls.Add(lblQ2)
        cardQ2.Controls.Add(segQ2)
        cardQ2.Location = New Point(28, 88)
        cardQ2.Margin = New Padding(0, 0, 0, 10)
        cardQ2.Name = "cardQ2"
        cardQ2.Padding = New Padding(18, 14, 14, 14)
        cardQ2.Size = New Size(564, 74)
        cardQ2.TabIndex = 1
        '
        'lblQ2
        '
        lblQ2.Dock = DockStyle.Fill
        lblQ2.Font = New Font("Leelawadee UI", 10.5F)
        lblQ2.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblQ2.Location = New Point(18, 14)
        lblQ2.Name = "lblQ2"
        lblQ2.Size = New Size(352, 46)
        lblQ2.TabIndex = 1
        lblQ2.Text = "2. อาการของท่านเป็นมากขึ้นเวลามาทำงานหรือไม่"
        lblQ2.TextAlign = ContentAlignment.MiddleLeft
        '
        'segQ2
        '
        segQ2.Dock = DockStyle.Right
        segQ2.FilledSelection = True
        segQ2.Items = New String() {"ใช่", "ไม่ใช่"}
        segQ2.Location = New Point(380, 14)
        segQ2.Name = "segQ2"
        segQ2.SelectedIndex = 1
        segQ2.SelectedLevel = AlertLevel.Success
        segQ2.Size = New Size(170, 46)
        segQ2.TabIndex = 0
        '
        'cardQ3
        '
        cardQ3.Controls.Add(lblQ3)
        cardQ3.Controls.Add(segQ3)
        cardQ3.Location = New Point(28, 172)
        cardQ3.Margin = New Padding(0, 0, 0, 10)
        cardQ3.Name = "cardQ3"
        cardQ3.Padding = New Padding(18, 14, 14, 14)
        cardQ3.Size = New Size(564, 74)
        cardQ3.TabIndex = 2
        '
        'lblQ3
        '
        lblQ3.Dock = DockStyle.Fill
        lblQ3.Font = New Font("Leelawadee UI", 10.5F)
        lblQ3.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblQ3.Location = New Point(18, 14)
        lblQ3.Name = "lblQ3"
        lblQ3.Size = New Size(352, 46)
        lblQ3.TabIndex = 1
        lblQ3.Text = "3. ท่านคิดว่าการเจ็บป่วยครั้งนี้เกิดจากฝุ่น PM2.5 หรือไม่"
        lblQ3.TextAlign = ContentAlignment.MiddleLeft
        '
        'segQ3
        '
        segQ3.Dock = DockStyle.Right
        segQ3.FilledSelection = True
        segQ3.Items = New String() {"ใช่", "ไม่ใช่"}
        segQ3.Location = New Point(380, 14)
        segQ3.Name = "segQ3"
        segQ3.SelectedIndex = 1
        segQ3.SelectedLevel = AlertLevel.Success
        segQ3.Size = New Size(170, 46)
        segQ3.TabIndex = 0
        '
        'banner
        '
        banner.Location = New Point(28, 256)
        banner.Margin = New Padding(0, 4, 0, 0)
        banner.Name = "banner"
        banner.Size = New Size(564, 62)
        banner.TabIndex = 3
        banner.Text = "ตอบว่า ""ใช่"" ข้อใดข้อหนึ่ง ให้ส่งต่อคลินิกโรคจากการทำงาน โทร 366, 106"
        '
        'OccupationalScreeningDialog
        '
        AcceptButton = btnSave
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        BackColor = Color.White
        CancelButton = btnLater
        ClientSize = New Size(620, 524)
        Controls.Add(flpBody)
        Controls.Add(pnlFooter)
        Controls.Add(pnlHeader)
        Font = New Font("Leelawadee UI", 10.0F)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "OccupationalScreeningDialog"
        StartPosition = FormStartPosition.CenterScreen
        Text = "การคัดกรองโรคจากการทำงาน"
        TopMost = True
        pnlHeader.ResumeLayout(False)
        pnlHeader.PerformLayout()
        pnlFooter.ResumeLayout(False)
        pnlFooter.PerformLayout()
        flpFooter.ResumeLayout(False)
        flpFooter.PerformLayout()
        flpBody.ResumeLayout(False)
        cardQ1.ResumeLayout(False)
        cardQ2.ResumeLayout(False)
        cardQ3.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlHeader As Panel
    Friend WithEvents lblPatient As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents tileIcon As IconTile
    Friend WithEvents pnlFooter As Panel
    Friend WithEvents lblVersion As Label
    Friend WithEvents flpFooter As FlowLayoutPanel
    Friend WithEvents btnSave As ModernButton
    Friend WithEvents btnLater As ModernButton
    Friend WithEvents flpBody As FlowLayoutPanel
    Friend WithEvents cardQ1 As CardPanel
    Friend WithEvents lblQ1 As Label
    Friend WithEvents segQ1 As SegmentedControl
    Friend WithEvents cardQ2 As CardPanel
    Friend WithEvents lblQ2 As Label
    Friend WithEvents segQ2 As SegmentedControl
    Friend WithEvents cardQ3 As CardPanel
    Friend WithEvents lblQ3 As Label
    Friend WithEvents segQ3 As SegmentedControl
    Friend WithEvents banner As InfoBanner
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainShell
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
        pnlSidebar = New Panel()
        pnlSidebarBottom = New Panel()
        lblRefreshInfo = New Label()
        ringRefresh = New CountdownRing()
        lblConnDetail = New Label()
        lblConnStatus = New Label()
        dotConn = New StatusDot()
        pnlSidebarLine = New Panel()
        navSettings = New NavButton()
        navDrug = New NavButton()
        navCrossTab = New NavButton()
        navLab = New NavButton()
        navCvd = New NavButton()
        navMedical = New NavButton()
        pnlModeBar = New Panel()
        flpMode = New FlowLayoutPanel()
        segMode = New SegmentedControl()
        btnSearchPatient = New ModernButton()
        btnClearManual = New ModernButton()
        lblModeHint = New Label()
        navLabHd = New NavButton()
        navHdCapd = New NavButton()
        lblMenuCaption = New Label()
        pnlBrand = New Panel()
        lblBrandSub = New Label()
        lblBrandName = New Label()
        brandMark = New BrandMark()
        pnlHeader = New Panel()
        lblUpdated = New Label()
        btnRefresh = New ModernButton()
        lblPatientMeta = New Label()
        lblPatientName = New Label()
        avatar = New AvatarCircle()
        pnlAlertStrip = New Panel()
        flpAlerts = New FlowLayoutPanel()
        pnlContent = New Panel()
        toolTip = New ToolTip(components)
        pnlSidebar.SuspendLayout()
        pnlSidebarBottom.SuspendLayout()
        pnlBrand.SuspendLayout()
        pnlHeader.SuspendLayout()
        pnlModeBar.SuspendLayout()
        flpMode.SuspendLayout()
        pnlAlertStrip.SuspendLayout()
        SuspendLayout()
        '
        'pnlSidebar
        '
        pnlSidebar.BackColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        pnlSidebar.Controls.Add(pnlSidebarBottom)
        pnlSidebar.Controls.Add(navDrug)
        pnlSidebar.Controls.Add(navHdCapd)
        pnlSidebar.Controls.Add(navLabHd)
        pnlSidebar.Controls.Add(navCrossTab)
        pnlSidebar.Controls.Add(navLab)
        pnlSidebar.Controls.Add(navCvd)
        pnlSidebar.Controls.Add(navMedical)
        pnlSidebar.Controls.Add(lblMenuCaption)
        pnlSidebar.Controls.Add(pnlBrand)
        pnlSidebar.Dock = DockStyle.Left
        pnlSidebar.Location = New Point(0, 0)
        pnlSidebar.Name = "pnlSidebar"
        pnlSidebar.Padding = New Padding(12, 0, 12, 12)
        pnlSidebar.Size = New Size(240, 741)
        pnlSidebar.TabIndex = 0
        '
        'pnlSidebarBottom
        '
        pnlSidebarBottom.Controls.Add(lblRefreshInfo)
        pnlSidebarBottom.Controls.Add(ringRefresh)
        pnlSidebarBottom.Controls.Add(lblConnDetail)
        pnlSidebarBottom.Controls.Add(lblConnStatus)
        pnlSidebarBottom.Controls.Add(dotConn)
        pnlSidebarBottom.Controls.Add(pnlSidebarLine)
        pnlSidebarBottom.Controls.Add(navSettings)
        pnlSidebarBottom.Dock = DockStyle.Bottom
        pnlSidebarBottom.Location = New Point(12, 583)
        pnlSidebarBottom.Name = "pnlSidebarBottom"
        pnlSidebarBottom.Size = New Size(216, 146)
        pnlSidebarBottom.TabIndex = 7
        '
        'lblRefreshInfo
        '
        lblRefreshInfo.AutoSize = True
        lblRefreshInfo.Font = New Font("Leelawadee UI", 8.5F)
        lblRefreshInfo.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        lblRefreshInfo.Location = New Point(46, 118)
        lblRefreshInfo.Name = "lblRefreshInfo"
        lblRefreshInfo.Size = New Size(110, 16)
        lblRefreshInfo.TabIndex = 6
        lblRefreshInfo.Text = "ตรวจสอบทุก 5 วินาที"
        '
        'ringRefresh
        '
        ringRefresh.DarkBackground = True
        ringRefresh.Location = New Point(10, 110)
        ringRefresh.Name = "ringRefresh"
        ringRefresh.Size = New Size(30, 30)
        ringRefresh.TabIndex = 5
        '
        'lblConnDetail
        '
        lblConnDetail.AutoEllipsis = True
        lblConnDetail.Font = New Font("Leelawadee UI", 8.5F)
        lblConnDetail.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        lblConnDetail.Location = New Point(30, 84)
        lblConnDetail.Name = "lblConnDetail"
        lblConnDetail.Size = New Size(184, 18)
        lblConnDetail.TabIndex = 4
        lblConnDetail.Text = "192.168.2.1 / hos"
        '
        'lblConnStatus
        '
        lblConnStatus.AutoEllipsis = True
        lblConnStatus.Font = New Font("Leelawadee UI", 9.5F, FontStyle.Bold)
        lblConnStatus.ForeColor = Color.White
        lblConnStatus.Location = New Point(30, 62)
        lblConnStatus.Name = "lblConnStatus"
        lblConnStatus.Size = New Size(184, 20)
        lblConnStatus.TabIndex = 3
        lblConnStatus.Text = "กำลังเชื่อมต่อ..."
        '
        'dotConn
        '
        dotConn.Location = New Point(10, 65)
        dotConn.Name = "dotConn"
        dotConn.Size = New Size(14, 14)
        dotConn.TabIndex = 2
        '
        'pnlSidebarLine
        '
        pnlSidebarLine.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        pnlSidebarLine.BackColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        pnlSidebarLine.Location = New Point(4, 52)
        pnlSidebarLine.Name = "pnlSidebarLine"
        pnlSidebarLine.Size = New Size(208, 1)
        pnlSidebarLine.TabIndex = 1
        '
        'navSettings
        '
        navSettings.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        navSettings.IconKind = IconKind.Gear
        navSettings.Location = New Point(0, 0)
        navSettings.Name = "navSettings"
        navSettings.Size = New Size(216, 46)
        navSettings.TabIndex = 0
        navSettings.Text = "ตั้งค่า"
        '
        'navLabHd
        '
        navLabHd.Dock = DockStyle.Top
        navLabHd.IconKind = IconKind.Droplet
        navLabHd.Location = New Point(12, 346)
        navLabHd.Name = "navLabHd"
        navLabHd.Size = New Size(216, 46)
        navLabHd.TabIndex = 6
        navLabHd.Text = "LAB Template Hemodialysis"
        '
        'navHdCapd
        '
        navHdCapd.Dock = DockStyle.Top
        navHdCapd.IconKind = IconKind.Kidney
        navHdCapd.Location = New Point(12, 392)
        navHdCapd.Name = "navHdCapd"
        navHdCapd.Size = New Size(216, 46)
        navHdCapd.TabIndex = 7
        navHdCapd.Text = "HD/CAPD Care"
        '
        'navDrug
        '
        navDrug.BadgeText = "เร็วๆ นี้"
        navDrug.Dock = DockStyle.Top
        navDrug.Enabled = False
        navDrug.IconKind = IconKind.Pill
        navDrug.Location = New Point(12, 346)
        navDrug.Name = "navDrug"
        navDrug.Size = New Size(216, 46)
        navDrug.TabIndex = 6
        navDrug.Text = "Drug Monitor"
        '
        'navCrossTab
        '
        navCrossTab.Dock = DockStyle.Top
        navCrossTab.IconKind = IconKind.Table
        navCrossTab.Location = New Point(12, 300)
        navCrossTab.Name = "navCrossTab"
        navCrossTab.Size = New Size(216, 46)
        navCrossTab.TabIndex = 5
        navCrossTab.Text = "LAB Crosstab"
        '
        'navLab
        '
        navLab.Dock = DockStyle.Top
        navLab.IconKind = IconKind.Flask
        navLab.Location = New Point(12, 254)
        navLab.Name = "navLab"
        navLab.Size = New Size(216, 46)
        navLab.TabIndex = 4
        navLab.Text = "ผล LAB"
        '
        'navCvd
        '
        navCvd.Dock = DockStyle.Top
        navCvd.IconKind = IconKind.HeartPulse
        navCvd.Location = New Point(12, 208)
        navCvd.Name = "navCvd"
        navCvd.Size = New Size(216, 46)
        navCvd.TabIndex = 3
        navCvd.Text = "CVD Risk"
        '
        'navMedical
        '
        navMedical.Dock = DockStyle.Top
        navMedical.IconKind = IconKind.ClipboardPlus
        navMedical.Location = New Point(12, 162)
        navMedical.Name = "navMedical"
        navMedical.Selected = True
        navMedical.Size = New Size(216, 46)
        navMedical.TabIndex = 2
        navMedical.Text = "Medical Care"
        '
        'lblMenuCaption
        '
        lblMenuCaption.Dock = DockStyle.Top
        lblMenuCaption.Font = New Font("Leelawadee UI", 8.5F, FontStyle.Bold)
        lblMenuCaption.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        lblMenuCaption.Location = New Point(12, 84)
        lblMenuCaption.Name = "lblMenuCaption"
        lblMenuCaption.Padding = New Padding(10, 0, 0, 6)
        lblMenuCaption.Size = New Size(216, 40)
        lblMenuCaption.TabIndex = 1
        lblMenuCaption.Text = "ข้อมูลผู้ป่วย"
        lblMenuCaption.TextAlign = ContentAlignment.BottomLeft
        '
        'pnlBrand
        '
        pnlBrand.Controls.Add(lblBrandSub)
        pnlBrand.Controls.Add(lblBrandName)
        pnlBrand.Controls.Add(brandMark)
        pnlBrand.Dock = DockStyle.Top
        pnlBrand.Location = New Point(12, 0)
        pnlBrand.Name = "pnlBrand"
        pnlBrand.Size = New Size(216, 84)
        pnlBrand.TabIndex = 0
        '
        'lblBrandSub
        '
        lblBrandSub.AutoSize = True
        lblBrandSub.Font = New Font("Leelawadee UI", 8.5F)
        lblBrandSub.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        lblBrandSub.Location = New Point(62, 47)
        lblBrandSub.Name = "lblBrandSub"
        lblBrandSub.Size = New Size(116, 16)
        lblBrandSub.TabIndex = 2
        lblBrandSub.Text = "Patient Alert · 2026"
        '
        'lblBrandName
        '
        lblBrandName.AutoSize = True
        lblBrandName.Font = New Font("Leelawadee UI", 13.0F, FontStyle.Bold)
        lblBrandName.ForeColor = Color.White
        lblBrandName.Location = New Point(60, 21)
        lblBrandName.Name = "lblBrandName"
        lblBrandName.Size = New Size(118, 24)
        lblBrandName.TabIndex = 1
        lblBrandName.Text = "SCPH Notify"
        '
        'brandMark
        '
        brandMark.Location = New Point(10, 22)
        brandMark.Name = "brandMark"
        brandMark.Size = New Size(42, 42)
        brandMark.TabIndex = 0
        '
        'pnlHeader
        '
        pnlHeader.BackColor = Color.White
        pnlHeader.Controls.Add(lblUpdated)
        pnlHeader.Controls.Add(btnRefresh)
        pnlHeader.Controls.Add(lblPatientMeta)
        pnlHeader.Controls.Add(lblPatientName)
        pnlHeader.Controls.Add(avatar)
        pnlHeader.Dock = DockStyle.Top
        pnlHeader.Location = New Point(240, 0)
        pnlHeader.Name = "pnlHeader"
        pnlHeader.Size = New Size(944, 92)
        pnlHeader.TabIndex = 1
        '
        'lblUpdated
        '
        lblUpdated.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblUpdated.Font = New Font("Leelawadee UI", 8.5F)
        lblUpdated.ForeColor = Color.FromArgb(CByte(148), CByte(163), CByte(184))
        lblUpdated.Location = New Point(598, 36)
        lblUpdated.Name = "lblUpdated"
        lblUpdated.Size = New Size(190, 20)
        lblUpdated.TabIndex = 3
        lblUpdated.Text = "-"
        lblUpdated.TextAlign = ContentAlignment.MiddleRight
        '
        'btnRefresh
        '
        btnRefresh.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnRefresh.IconKind = IconKind.Refresh
        btnRefresh.Kind = ButtonVariant.Secondary
        btnRefresh.Location = New Point(798, 26)
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New Size(118, 40)
        btnRefresh.TabIndex = 4
        btnRefresh.Text = "รีเฟรช"
        '
        'lblPatientMeta
        '
        lblPatientMeta.AutoEllipsis = True
        lblPatientMeta.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblPatientMeta.Font = New Font("Leelawadee UI", 9.5F)
        lblPatientMeta.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblPatientMeta.Location = New Point(96, 52)
        lblPatientMeta.Name = "lblPatientMeta"
        lblPatientMeta.Size = New Size(490, 22)
        lblPatientMeta.TabIndex = 2
        lblPatientMeta.Text = "ยังไม่มีผู้ป่วยถูกเรียกที่เครื่องนี้"
        '
        'lblPatientName
        '
        lblPatientName.AutoEllipsis = True
        lblPatientName.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblPatientName.Font = New Font("Leelawadee UI", 15.0F, FontStyle.Bold)
        lblPatientName.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblPatientName.Location = New Point(94, 18)
        lblPatientName.Name = "lblPatientName"
        lblPatientName.Size = New Size(492, 34)
        lblPatientName.TabIndex = 1
        lblPatientName.Text = "ระบบรอเรียกผู้ป่วย"
        '
        'avatar
        '
        avatar.Active = False
        avatar.Location = New Point(28, 20)
        avatar.Name = "avatar"
        avatar.Size = New Size(52, 52)
        avatar.TabIndex = 0
        '
        'pnlModeBar
        '
        pnlModeBar.BackColor = Color.White
        pnlModeBar.Controls.Add(lblModeHint)
        pnlModeBar.Controls.Add(flpMode)
        pnlModeBar.Dock = DockStyle.Top
        pnlModeBar.Location = New Point(240, 92)
        pnlModeBar.Name = "pnlModeBar"
        pnlModeBar.Size = New Size(944, 58)
        pnlModeBar.TabIndex = 2
        '
        'flpMode
        '
        flpMode.AutoSize = True
        flpMode.AutoSizeMode = AutoSizeMode.GrowAndShrink
        flpMode.Controls.Add(segMode)
        flpMode.Controls.Add(btnSearchPatient)
        flpMode.Controls.Add(btnClearManual)
        flpMode.Dock = DockStyle.Left
        flpMode.Location = New Point(0, 0)
        flpMode.Name = "flpMode"
        flpMode.Padding = New Padding(28, 8, 0, 10)
        flpMode.Size = New Size(640, 58)
        flpMode.TabIndex = 0
        flpMode.WrapContents = False
        '
        'segMode
        '
        segMode.Items = New String() {"รอเรียกผู้ป่วยอัตโนมัติ", "ค้นหาผู้ป่วยเอง"}
        segMode.Location = New Point(28, 8)
        segMode.Margin = New Padding(0, 0, 12, 0)
        segMode.Name = "segMode"
        segMode.SelectedIndex = 0
        segMode.Size = New Size(320, 40)
        segMode.TabIndex = 0
        '
        'btnSearchPatient
        '
        btnSearchPatient.AutoSize = True
        btnSearchPatient.AutoSizeMode = AutoSizeMode.GrowAndShrink
        btnSearchPatient.IconKind = IconKind.Search
        btnSearchPatient.Location = New Point(360, 8)
        btnSearchPatient.Margin = New Padding(0, 0, 6, 0)
        btnSearchPatient.Name = "btnSearchPatient"
        btnSearchPatient.Size = New Size(150, 40)
        btnSearchPatient.TabIndex = 1
        btnSearchPatient.Text = "ค้นหาผู้ป่วย"
        btnSearchPatient.Visible = False
        '
        'btnClearManual
        '
        btnClearManual.AutoSize = True
        btnClearManual.AutoSizeMode = AutoSizeMode.GrowAndShrink
        btnClearManual.IconKind = IconKind.Close
        btnClearManual.Kind = ButtonVariant.Ghost
        btnClearManual.Location = New Point(516, 8)
        btnClearManual.Margin = New Padding(0)
        btnClearManual.Name = "btnClearManual"
        btnClearManual.Size = New Size(120, 40)
        btnClearManual.TabIndex = 2
        btnClearManual.Text = "ล้างผู้ป่วย"
        btnClearManual.Visible = False
        '
        'lblModeHint
        '
        lblModeHint.AutoEllipsis = True
        lblModeHint.Dock = DockStyle.Fill
        lblModeHint.Font = New Font("Leelawadee UI", 9.0F)
        lblModeHint.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        lblModeHint.Location = New Point(640, 0)
        lblModeHint.Name = "lblModeHint"
        lblModeHint.Padding = New Padding(16, 0, 28, 0)
        lblModeHint.Size = New Size(304, 58)
        lblModeHint.TabIndex = 1
        lblModeHint.Text = "ข้อมูลจะแสดงอัตโนมัติเมื่อมีการเรียกผู้ป่วยที่เครื่องนี้"
        lblModeHint.TextAlign = ContentAlignment.MiddleRight
        '
        'pnlAlertStrip
        '
        pnlAlertStrip.BackColor = Color.White
        pnlAlertStrip.Controls.Add(flpAlerts)
        pnlAlertStrip.Dock = DockStyle.Top
        pnlAlertStrip.Location = New Point(240, 150)
        pnlAlertStrip.Name = "pnlAlertStrip"
        pnlAlertStrip.Padding = New Padding(94, 0, 28, 12)
        pnlAlertStrip.Size = New Size(944, 46)
        pnlAlertStrip.TabIndex = 3
        pnlAlertStrip.Visible = False
        '
        'flpAlerts
        '
        flpAlerts.Dock = DockStyle.Fill
        flpAlerts.Location = New Point(94, 0)
        flpAlerts.Name = "flpAlerts"
        flpAlerts.Size = New Size(822, 34)
        flpAlerts.TabIndex = 0
        flpAlerts.WrapContents = False
        '
        'pnlContent
        '
        pnlContent.BackColor = Color.FromArgb(CByte(244), CByte(246), CByte(250))
        pnlContent.Dock = DockStyle.Fill
        pnlContent.Location = New Point(240, 196)
        pnlContent.Name = "pnlContent"
        pnlContent.Padding = New Padding(28, 22, 28, 24)
        pnlContent.Size = New Size(944, 603)
        pnlContent.TabIndex = 4
        '
        'MainShell
        '
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        BackColor = Color.FromArgb(CByte(244), CByte(246), CByte(250))
        ClientSize = New Size(1184, 741)
        Controls.Add(pnlContent)
        Controls.Add(pnlAlertStrip)
        Controls.Add(pnlModeBar)
        Controls.Add(pnlHeader)
        Controls.Add(pnlSidebar)
        Font = New Font("Leelawadee UI", 10.0F)
        KeyPreview = True
        MinimumSize = New Size(1080, 700)
        Name = "MainShell"
        StartPosition = FormStartPosition.CenterScreen
        Text = "SCPH Notify"
        pnlSidebar.ResumeLayout(False)
        pnlSidebarBottom.ResumeLayout(False)
        pnlSidebarBottom.PerformLayout()
        pnlBrand.ResumeLayout(False)
        pnlBrand.PerformLayout()
        pnlHeader.ResumeLayout(False)
        flpMode.ResumeLayout(False)
        flpMode.PerformLayout()
        pnlModeBar.ResumeLayout(False)
        pnlModeBar.PerformLayout()
        pnlAlertStrip.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlSidebar As Panel
    Friend WithEvents pnlSidebarBottom As Panel
    Friend WithEvents lblRefreshInfo As Label
    Friend WithEvents ringRefresh As CountdownRing
    Friend WithEvents lblConnDetail As Label
    Friend WithEvents lblConnStatus As Label
    Friend WithEvents dotConn As StatusDot
    Friend WithEvents pnlSidebarLine As Panel
    Friend WithEvents navSettings As NavButton
    Friend WithEvents navDrug As NavButton
    Friend WithEvents navCrossTab As NavButton
    Friend WithEvents navLab As NavButton
    Friend WithEvents navCvd As NavButton
    Friend WithEvents navMedical As NavButton
    Friend WithEvents pnlModeBar As Panel
    Friend WithEvents flpMode As FlowLayoutPanel
    Friend WithEvents segMode As SegmentedControl
    Friend WithEvents btnSearchPatient As ModernButton
    Friend WithEvents btnClearManual As ModernButton
    Friend WithEvents lblModeHint As Label
    Friend WithEvents navLabHd As NavButton
    Friend WithEvents navHdCapd As NavButton
    Friend WithEvents lblMenuCaption As Label
    Friend WithEvents pnlBrand As Panel
    Friend WithEvents lblBrandSub As Label
    Friend WithEvents lblBrandName As Label
    Friend WithEvents brandMark As BrandMark
    Friend WithEvents pnlHeader As Panel
    Friend WithEvents lblUpdated As Label
    Friend WithEvents btnRefresh As ModernButton
    Friend WithEvents lblPatientMeta As Label
    Friend WithEvents lblPatientName As Label
    Friend WithEvents avatar As AvatarCircle
    Friend WithEvents pnlAlertStrip As Panel
    Friend WithEvents flpAlerts As FlowLayoutPanel
    Friend WithEvents pnlContent As Panel
    Friend WithEvents toolTip As ToolTip
End Class

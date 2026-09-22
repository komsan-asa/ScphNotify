<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class NotifierWidget
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
        dotStatus = New StatusDot()
        avatar = New AvatarCircle()
        lblName = New Label()
        lblMeta = New Label()
        flpChips = New FlowLayoutPanel()
        ring = New CountdownRing()
        btnOpen = New ModernButton()
        btnMenu = New ModernButton()
        trayIcon = New NotifyIcon(components)
        menuMain = New ContextMenuStrip(components)
        miOpenMain = New ToolStripMenuItem()
        miOccupationalRegistry = New ToolStripMenuItem()
        miShowWidget = New ToolStripMenuItem()
        miHideWidget = New ToolStripMenuItem()
        sep1 = New ToolStripSeparator()
        miInterval = New ToolStripMenuItem()
        miTopMost = New ToolStripMenuItem()
        sep2 = New ToolStripSeparator()
        miDbConfig = New ToolStripMenuItem()
        sep3 = New ToolStripSeparator()
        miExit = New ToolStripMenuItem()
        tip = New ToolTip(components)
        flashTimer = New Timer(components)
        menuMain.SuspendLayout()
        SuspendLayout()
        '
        'dotStatus
        '
        dotStatus.Location = New Point(10, 21)
        dotStatus.Name = "dotStatus"
        dotStatus.Size = New Size(10, 10)
        dotStatus.TabIndex = 0
        '
        'avatar
        '
        avatar.Active = False
        avatar.Location = New Point(27, 10)
        avatar.Name = "avatar"
        avatar.Size = New Size(32, 32)
        avatar.TabIndex = 1
        '
        'lblName
        '
        lblName.AutoSize = True
        lblName.BackColor = Color.White
        lblName.Font = New Font("Leelawadee UI", 10.5F, FontStyle.Bold)
        lblName.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblName.Location = New Point(67, 7)
        lblName.Name = "lblName"
        lblName.Size = New Size(142, 20)
        lblName.TabIndex = 2
        lblName.Text = "ระบบรอเรียกผู้ป่วย"
        '
        'lblMeta
        '
        lblMeta.AutoSize = True
        lblMeta.BackColor = Color.White
        lblMeta.Font = New Font("Leelawadee UI", 8.5F)
        lblMeta.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblMeta.Location = New Point(68, 27)
        lblMeta.Name = "lblMeta"
        lblMeta.Size = New Size(120, 16)
        lblMeta.TabIndex = 3
        lblMeta.Text = "กำลังเชื่อมต่อฐานข้อมูล..."
        '
        'flpChips
        '
        flpChips.BackColor = Color.White
        flpChips.Location = New Point(220, 16)
        flpChips.Name = "flpChips"
        flpChips.Size = New Size(0, 0)
        flpChips.TabIndex = 4
        flpChips.WrapContents = False
        '
        'ring
        '
        ring.Location = New Point(232, 13)
        ring.Name = "ring"
        ring.Size = New Size(26, 26)
        ring.TabIndex = 5
        '
        'btnOpen
        '
        btnOpen.Circular = True
        btnOpen.IconKind = IconKind.External
        btnOpen.Kind = ButtonVariant.Ghost
        btnOpen.Location = New Point(262, 12)
        btnOpen.Name = "btnOpen"
        btnOpen.Size = New Size(28, 28)
        btnOpen.TabIndex = 6
        btnOpen.TabStop = False
        '
        'btnMenu
        '
        btnMenu.Circular = True
        btnMenu.IconKind = IconKind.MoreVertical
        btnMenu.Kind = ButtonVariant.Ghost
        btnMenu.Location = New Point(291, 12)
        btnMenu.Name = "btnMenu"
        btnMenu.Size = New Size(28, 28)
        btnMenu.TabIndex = 7
        btnMenu.TabStop = False
        '
        'trayIcon
        '
        trayIcon.ContextMenuStrip = menuMain
        trayIcon.Text = "SCPH Notify"
        trayIcon.Visible = True
        '
        'menuMain
        '
        menuMain.Font = New Font("Leelawadee UI", 10.0F)
        menuMain.Items.AddRange(New ToolStripItem() {miOpenMain, miOccupationalRegistry, miShowWidget, miHideWidget, sep1, miInterval, miTopMost, sep2, miDbConfig, sep3, miExit})
        menuMain.Name = "menuMain"
        menuMain.Size = New Size(300, 214)
        '
        'miOpenMain
        '
        miOpenMain.Font = New Font("Leelawadee UI", 10.0F, FontStyle.Bold)
        miOpenMain.Name = "miOpenMain"
        miOpenMain.Size = New Size(280, 24)
        miOpenMain.Text = "เปิดหน้าต่างหลัก"
        '
        'miOccupationalRegistry
        '
        miOccupationalRegistry.Name = "miOccupationalRegistry"
        miOccupationalRegistry.Size = New Size(280, 24)
        miOccupationalRegistry.Text = "ทะเบียนโรคจากการทำงานและ PM2.5"
        '
        'miShowWidget
        '
        miShowWidget.Name = "miShowWidget"
        miShowWidget.Size = New Size(280, 24)
        miShowWidget.Text = "แสดงแถบแจ้งเตือน"
        '
        'miHideWidget
        '
        miHideWidget.Name = "miHideWidget"
        miHideWidget.Size = New Size(280, 24)
        miHideWidget.Text = "ซ่อนไปที่ถาดระบบ"
        '
        'sep1
        '
        sep1.Name = "sep1"
        sep1.Size = New Size(277, 6)
        '
        'miInterval
        '
        miInterval.Name = "miInterval"
        miInterval.Size = New Size(280, 24)
        miInterval.Text = "ตรวจสอบผู้ป่วยทุก"
        '
        'miTopMost
        '
        miTopMost.CheckOnClick = True
        miTopMost.Name = "miTopMost"
        miTopMost.Size = New Size(280, 24)
        miTopMost.Text = "อยู่บนสุดเสมอ"
        '
        'sep2
        '
        sep2.Name = "sep2"
        sep2.Size = New Size(277, 6)
        '
        'miDbConfig
        '
        miDbConfig.Name = "miDbConfig"
        miDbConfig.ShortcutKeyDisplayString = "Ctrl+F6"
        miDbConfig.Size = New Size(280, 24)
        miDbConfig.Text = "ตั้งค่าการเชื่อมต่อฐานข้อมูล..."
        '
        'sep3
        '
        sep3.Name = "sep3"
        sep3.Size = New Size(277, 6)
        '
        'miExit
        '
        miExit.Name = "miExit"
        miExit.Size = New Size(280, 24)
        miExit.Text = "ออกจากโปรแกรม"
        '
        'flashTimer
        '
        flashTimer.Interval = 60
        '
        'NotifierWidget
        '
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        BackColor = Color.White
        ClientSize = New Size(327, 52)
        ContextMenuStrip = menuMain
        Controls.Add(btnMenu)
        Controls.Add(btnOpen)
        Controls.Add(ring)
        Controls.Add(flpChips)
        Controls.Add(lblMeta)
        Controls.Add(lblName)
        Controls.Add(avatar)
        Controls.Add(dotStatus)
        Font = New Font("Leelawadee UI", 10.0F)
        FormBorderStyle = FormBorderStyle.None
        KeyPreview = True
        MaximizeBox = False
        MinimizeBox = False
        Name = "NotifierWidget"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.Manual
        Text = "SCPH Notify"
        TopMost = True
        menuMain.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents dotStatus As StatusDot
    Friend WithEvents avatar As AvatarCircle
    Friend WithEvents lblName As Label
    Friend WithEvents lblMeta As Label
    Friend WithEvents flpChips As FlowLayoutPanel
    Friend WithEvents ring As CountdownRing
    Friend WithEvents btnOpen As ModernButton
    Friend WithEvents btnMenu As ModernButton
    Friend WithEvents trayIcon As NotifyIcon
    Friend WithEvents menuMain As ContextMenuStrip
    Friend WithEvents miOpenMain As ToolStripMenuItem
    Friend WithEvents miOccupationalRegistry As ToolStripMenuItem
    Friend WithEvents miShowWidget As ToolStripMenuItem
    Friend WithEvents miHideWidget As ToolStripMenuItem
    Friend WithEvents sep1 As ToolStripSeparator
    Friend WithEvents miInterval As ToolStripMenuItem
    Friend WithEvents miTopMost As ToolStripMenuItem
    Friend WithEvents sep2 As ToolStripSeparator
    Friend WithEvents miDbConfig As ToolStripMenuItem
    Friend WithEvents sep3 As ToolStripSeparator
    Friend WithEvents miExit As ToolStripMenuItem
    Friend WithEvents tip As ToolTip
    Friend WithEvents flashTimer As Timer
End Class

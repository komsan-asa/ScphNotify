<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DbConfigDialog
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
        lblDesc = New Label()
        lblTitle = New Label()
        tileIcon = New IconTile()
        pnlFooter = New Panel()
        flpFooter = New FlowLayoutPanel()
        btnSave = New ModernButton()
        btnCancel = New ModernButton()
        btnTest = New ModernButton()
        tlpBody = New TableLayoutPanel()
        lblServer = New Label()
        lblPort = New Label()
        txtServer = New ModernTextBox()
        txtPort = New ModernTextBox()
        lblDatabase = New Label()
        txtDatabase = New ModernTextBox()
        lblUser = New Label()
        txtUser = New ModernTextBox()
        lblPassword = New Label()
        txtPassword = New ModernTextBox()
        banner = New InfoBanner()
        pnlHeader.SuspendLayout()
        pnlFooter.SuspendLayout()
        flpFooter.SuspendLayout()
        tlpBody.SuspendLayout()
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
        pnlHeader.Size = New Size(520, 96)
        pnlHeader.TabIndex = 0
        '
        'lblDesc
        '
        lblDesc.AutoSize = True
        lblDesc.Font = New Font("Leelawadee UI", 9.5F)
        lblDesc.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblDesc.Location = New Point(89, 56)
        lblDesc.Name = "lblDesc"
        lblDesc.Size = New Size(300, 17)
        lblDesc.TabIndex = 2
        lblDesc.Text = "ฐานข้อมูล HOSxP (MySQL / MariaDB) · เปิดหน้านี้ได้ด้วย Ctrl+F6"
        '
        'lblTitle
        '
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Leelawadee UI", 14.0F, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblTitle.Location = New Point(87, 26)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(290, 26)
        lblTitle.TabIndex = 1
        lblTitle.Text = "ตั้งค่าการเชื่อมต่อฐานข้อมูล"
        '
        'tileIcon
        '
        tileIcon.IconKind = IconKind.Database
        tileIcon.Level = AlertLevel.Info
        tileIcon.Location = New Point(28, 26)
        tileIcon.Name = "tileIcon"
        tileIcon.Size = New Size(48, 48)
        tileIcon.TabIndex = 0
        '
        'pnlFooter
        '
        pnlFooter.BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        pnlFooter.Controls.Add(flpFooter)
        pnlFooter.Controls.Add(btnTest)
        pnlFooter.Dock = DockStyle.Bottom
        pnlFooter.Location = New Point(0, 488)
        pnlFooter.Name = "pnlFooter"
        pnlFooter.Size = New Size(520, 72)
        pnlFooter.TabIndex = 2
        '
        'flpFooter
        '
        flpFooter.AutoSize = True
        flpFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink
        flpFooter.Controls.Add(btnSave)
        flpFooter.Controls.Add(btnCancel)
        flpFooter.Dock = DockStyle.Right
        flpFooter.FlowDirection = FlowDirection.RightToLeft
        flpFooter.Location = New Point(280, 0)
        flpFooter.Name = "flpFooter"
        flpFooter.Padding = New Padding(0, 16, 22, 16)
        flpFooter.Size = New Size(240, 72)
        flpFooter.TabIndex = 1
        flpFooter.WrapContents = False
        '
        'btnSave
        '
        btnSave.AutoSize = True
        btnSave.AutoSizeMode = AutoSizeMode.GrowAndShrink
        btnSave.IconKind = IconKind.Check
        btnSave.Location = New Point(112, 19)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(102, 40)
        btnSave.TabIndex = 0
        btnSave.Text = "บันทึก"
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
        'btnTest
        '
        btnTest.AutoSize = True
        btnTest.AutoSizeMode = AutoSizeMode.GrowAndShrink
        btnTest.IconKind = IconKind.Refresh
        btnTest.Kind = ButtonVariant.Secondary
        btnTest.Location = New Point(28, 16)
        btnTest.Name = "btnTest"
        btnTest.Size = New Size(186, 40)
        btnTest.TabIndex = 0
        btnTest.Text = "ทดสอบการเชื่อมต่อ"
        '
        'tlpBody
        '
        tlpBody.ColumnCount = 2
        tlpBody.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 72.0F))
        tlpBody.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 28.0F))
        tlpBody.Controls.Add(lblServer, 0, 0)
        tlpBody.Controls.Add(lblPort, 1, 0)
        tlpBody.Controls.Add(txtServer, 0, 1)
        tlpBody.Controls.Add(txtPort, 1, 1)
        tlpBody.Controls.Add(lblDatabase, 0, 2)
        tlpBody.Controls.Add(txtDatabase, 0, 3)
        tlpBody.Controls.Add(lblUser, 0, 4)
        tlpBody.Controls.Add(txtUser, 0, 5)
        tlpBody.Controls.Add(lblPassword, 0, 6)
        tlpBody.Controls.Add(txtPassword, 0, 7)
        tlpBody.Controls.Add(banner, 0, 8)
        tlpBody.Dock = DockStyle.Fill
        tlpBody.Location = New Point(0, 96)
        tlpBody.Name = "tlpBody"
        tlpBody.Padding = New Padding(25, 0, 25, 8)
        tlpBody.RowCount = 9
        tlpBody.RowStyles.Add(New RowStyle(SizeType.Absolute, 24.0F))
        tlpBody.RowStyles.Add(New RowStyle(SizeType.Absolute, 48.0F))
        tlpBody.RowStyles.Add(New RowStyle(SizeType.Absolute, 24.0F))
        tlpBody.RowStyles.Add(New RowStyle(SizeType.Absolute, 48.0F))
        tlpBody.RowStyles.Add(New RowStyle(SizeType.Absolute, 24.0F))
        tlpBody.RowStyles.Add(New RowStyle(SizeType.Absolute, 48.0F))
        tlpBody.RowStyles.Add(New RowStyle(SizeType.Absolute, 24.0F))
        tlpBody.RowStyles.Add(New RowStyle(SizeType.Absolute, 48.0F))
        tlpBody.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpBody.SetColumnSpan(lblDatabase, 2)
        tlpBody.SetColumnSpan(txtDatabase, 2)
        tlpBody.SetColumnSpan(lblUser, 2)
        tlpBody.SetColumnSpan(txtUser, 2)
        tlpBody.SetColumnSpan(lblPassword, 2)
        tlpBody.SetColumnSpan(txtPassword, 2)
        tlpBody.SetColumnSpan(banner, 2)
        tlpBody.Size = New Size(520, 392)
        tlpBody.TabIndex = 1
        '
        'lblServer
        '
        lblServer.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblServer.AutoSize = True
        lblServer.Font = New Font("Leelawadee UI", 9.0F, FontStyle.Bold)
        lblServer.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblServer.Location = New Point(28, 5)
        lblServer.Name = "lblServer"
        lblServer.Size = New Size(130, 16)
        lblServer.TabIndex = 0
        lblServer.Text = "เซิร์ฟเวอร์ (Host / IP)"
        '
        'lblPort
        '
        lblPort.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblPort.AutoSize = True
        lblPort.Font = New Font("Leelawadee UI", 9.0F, FontStyle.Bold)
        lblPort.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblPort.Location = New Point(363, 5)
        lblPort.Name = "lblPort"
        lblPort.Size = New Size(36, 16)
        lblPort.TabIndex = 1
        lblPort.Text = "พอร์ต"
        '
        'txtServer
        '
        txtServer.Dock = DockStyle.Fill
        txtServer.IconKind = IconKind.Server
        txtServer.Location = New Point(28, 27)
        txtServer.Margin = New Padding(3, 3, 8, 7)
        txtServer.Name = "txtServer"
        txtServer.PlaceholderText = "เช่น 192.168.2.1"
        txtServer.Size = New Size(324, 38)
        txtServer.TabIndex = 2
        '
        'txtPort
        '
        txtPort.Dock = DockStyle.Fill
        txtPort.Location = New Point(363, 27)
        txtPort.Margin = New Padding(3, 3, 3, 7)
        txtPort.Name = "txtPort"
        txtPort.PlaceholderText = "3306"
        txtPort.Size = New Size(129, 38)
        txtPort.TabIndex = 3
        '
        'lblDatabase
        '
        lblDatabase.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblDatabase.AutoSize = True
        lblDatabase.Font = New Font("Leelawadee UI", 9.0F, FontStyle.Bold)
        lblDatabase.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblDatabase.Location = New Point(28, 77)
        lblDatabase.Name = "lblDatabase"
        lblDatabase.Size = New Size(64, 16)
        lblDatabase.TabIndex = 4
        lblDatabase.Text = "ฐานข้อมูล"
        '
        'txtDatabase
        '
        txtDatabase.Dock = DockStyle.Fill
        txtDatabase.IconKind = IconKind.Database
        txtDatabase.Location = New Point(28, 99)
        txtDatabase.Margin = New Padding(3, 3, 3, 7)
        txtDatabase.Name = "txtDatabase"
        txtDatabase.PlaceholderText = "เช่น hos"
        txtDatabase.Size = New Size(464, 38)
        txtDatabase.TabIndex = 5
        '
        'lblUser
        '
        lblUser.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblUser.AutoSize = True
        lblUser.Font = New Font("Leelawadee UI", 9.0F, FontStyle.Bold)
        lblUser.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblUser.Location = New Point(28, 149)
        lblUser.Name = "lblUser"
        lblUser.Size = New Size(60, 16)
        lblUser.TabIndex = 6
        lblUser.Text = "ชื่อผู้ใช้"
        '
        'txtUser
        '
        txtUser.Dock = DockStyle.Fill
        txtUser.IconKind = IconKind.User
        txtUser.Location = New Point(28, 171)
        txtUser.Margin = New Padding(3, 3, 3, 7)
        txtUser.Name = "txtUser"
        txtUser.Size = New Size(464, 38)
        txtUser.TabIndex = 7
        '
        'lblPassword
        '
        lblPassword.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblPassword.AutoSize = True
        lblPassword.Font = New Font("Leelawadee UI", 9.0F, FontStyle.Bold)
        lblPassword.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblPassword.Location = New Point(28, 221)
        lblPassword.Name = "lblPassword"
        lblPassword.Size = New Size(56, 16)
        lblPassword.TabIndex = 8
        lblPassword.Text = "รหัสผ่าน"
        '
        'txtPassword
        '
        txtPassword.Dock = DockStyle.Fill
        txtPassword.Location = New Point(28, 243)
        txtPassword.Margin = New Padding(3, 3, 3, 7)
        txtPassword.Name = "txtPassword"
        txtPassword.PasswordMode = True
        txtPassword.Size = New Size(464, 38)
        txtPassword.TabIndex = 9
        '
        'banner
        '
        banner.Dock = DockStyle.Top
        banner.Location = New Point(28, 299)
        banner.Margin = New Padding(3, 10, 3, 3)
        banner.Name = "banner"
        banner.Size = New Size(464, 62)
        banner.TabIndex = 10
        banner.Visible = False
        '
        'DbConfigDialog
        '
        AcceptButton = btnSave
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        BackColor = Color.White
        CancelButton = btnCancel
        ClientSize = New Size(520, 560)
        Controls.Add(tlpBody)
        Controls.Add(pnlFooter)
        Controls.Add(pnlHeader)
        Font = New Font("Leelawadee UI", 10.0F)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "DbConfigDialog"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "ตั้งค่าการเชื่อมต่อฐานข้อมูล"
        pnlHeader.ResumeLayout(False)
        pnlHeader.PerformLayout()
        pnlFooter.ResumeLayout(False)
        pnlFooter.PerformLayout()
        flpFooter.ResumeLayout(False)
        flpFooter.PerformLayout()
        tlpBody.ResumeLayout(False)
        tlpBody.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlHeader As Panel
    Friend WithEvents lblDesc As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents tileIcon As IconTile
    Friend WithEvents pnlFooter As Panel
    Friend WithEvents flpFooter As FlowLayoutPanel
    Friend WithEvents btnSave As ModernButton
    Friend WithEvents btnCancel As ModernButton
    Friend WithEvents btnTest As ModernButton
    Friend WithEvents tlpBody As TableLayoutPanel
    Friend WithEvents lblServer As Label
    Friend WithEvents lblPort As Label
    Friend WithEvents txtServer As ModernTextBox
    Friend WithEvents txtPort As ModernTextBox
    Friend WithEvents lblDatabase As Label
    Friend WithEvents txtDatabase As ModernTextBox
    Friend WithEvents lblUser As Label
    Friend WithEvents txtUser As ModernTextBox
    Friend WithEvents lblPassword As Label
    Friend WithEvents txtPassword As ModernTextBox
    Friend WithEvents banner As InfoBanner
End Class

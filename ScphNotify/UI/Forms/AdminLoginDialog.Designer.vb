<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AdminLoginDialog
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
        tlpBody = New TableLayoutPanel()
        lblUser = New Label()
        txtUser = New ModernTextBox()
        lblPassword = New Label()
        txtPassword = New ModernTextBox()
        btnHosxp = New ModernButton()
        banner = New InfoBanner()
        pnlFooter = New Panel()
        lblVersion = New Label()
        flpFooter = New FlowLayoutPanel()
        btnSignIn = New ModernButton()
        btnCancel = New ModernButton()
        tip = New ToolTip(components)
        pnlHeader.SuspendLayout()
        tlpBody.SuspendLayout()
        pnlFooter.SuspendLayout()
        flpFooter.SuspendLayout()
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
        pnlHeader.Size = New Size(480, 96)
        pnlHeader.TabIndex = 0
        '
        'lblDesc
        '
        lblDesc.AutoSize = True
        lblDesc.Font = New Font("Leelawadee UI", 9.5F)
        lblDesc.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblDesc.Location = New Point(89, 56)
        lblDesc.Name = "lblDesc"
        lblDesc.Size = New Size(330, 17)
        lblDesc.TabIndex = 2
        lblDesc.Text = "ใช้บัญชีผู้ใช้ HOSxP ที่มีสิทธิ์ผู้ดูแลระบบ"
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
        lblTitle.Text = "เข้าสู่ระบบผู้ดูแล"
        '
        'tileIcon
        '
        tileIcon.IconKind = IconKind.Lock
        tileIcon.Level = AlertLevel.Primary
        tileIcon.Location = New Point(28, 26)
        tileIcon.Name = "tileIcon"
        tileIcon.Size = New Size(48, 48)
        tileIcon.TabIndex = 0
        '
        'tlpBody
        '
        tlpBody.ColumnCount = 1
        tlpBody.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        tlpBody.Controls.Add(lblUser, 0, 0)
        tlpBody.Controls.Add(txtUser, 0, 1)
        tlpBody.Controls.Add(lblPassword, 0, 2)
        tlpBody.Controls.Add(txtPassword, 0, 3)
        tlpBody.Controls.Add(btnHosxp, 0, 4)
        tlpBody.Controls.Add(banner, 0, 5)
        tlpBody.Dock = DockStyle.Fill
        tlpBody.Location = New Point(0, 96)
        tlpBody.Name = "tlpBody"
        tlpBody.Padding = New Padding(25, 0, 25, 8)
        tlpBody.RowCount = 6
        tlpBody.RowStyles.Add(New RowStyle(SizeType.Absolute, 24.0F))
        tlpBody.RowStyles.Add(New RowStyle(SizeType.Absolute, 48.0F))
        tlpBody.RowStyles.Add(New RowStyle(SizeType.Absolute, 28.0F))
        tlpBody.RowStyles.Add(New RowStyle(SizeType.Absolute, 48.0F))
        tlpBody.RowStyles.Add(New RowStyle(SizeType.Absolute, 50.0F))
        tlpBody.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpBody.Size = New Size(480, 280)
        tlpBody.TabIndex = 1
        '
        'lblUser
        '
        lblUser.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblUser.AutoSize = True
        lblUser.Font = New Font("Leelawadee UI", 9.0F, FontStyle.Bold)
        lblUser.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblUser.Location = New Point(28, 5)
        lblUser.Name = "lblUser"
        lblUser.Size = New Size(110, 16)
        lblUser.TabIndex = 0
        lblUser.Text = "ชื่อผู้ใช้ (HOSxP)"
        '
        'txtUser
        '
        txtUser.Dock = DockStyle.Fill
        txtUser.IconKind = IconKind.User
        txtUser.Location = New Point(28, 27)
        txtUser.Margin = New Padding(3, 3, 3, 7)
        txtUser.Name = "txtUser"
        txtUser.PlaceholderText = "loginname"
        txtUser.Size = New Size(424, 38)
        txtUser.TabIndex = 1
        '
        'lblPassword
        '
        lblPassword.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblPassword.AutoSize = True
        lblPassword.Font = New Font("Leelawadee UI", 9.0F, FontStyle.Bold)
        lblPassword.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblPassword.Location = New Point(28, 81)
        lblPassword.Name = "lblPassword"
        lblPassword.Size = New Size(56, 16)
        lblPassword.TabIndex = 2
        lblPassword.Text = "รหัสผ่าน"
        '
        'txtPassword
        '
        txtPassword.Dock = DockStyle.Fill
        txtPassword.IconKind = IconKind.Lock
        txtPassword.Location = New Point(28, 103)
        txtPassword.Margin = New Padding(3, 3, 3, 7)
        txtPassword.Name = "txtPassword"
        txtPassword.PasswordMode = True
        txtPassword.Size = New Size(424, 38)
        txtPassword.TabIndex = 3
        '
        'btnHosxp
        '
        btnHosxp.Dock = DockStyle.Top
        btnHosxp.IconKind = IconKind.User
        btnHosxp.Kind = ButtonVariant.Secondary
        btnHosxp.Location = New Point(28, 159)
        btnHosxp.Margin = New Padding(3, 8, 3, 0)
        btnHosxp.Name = "btnHosxp"
        btnHosxp.Size = New Size(424, 42)
        btnHosxp.TabIndex = 4
        btnHosxp.Text = "ใช้บัญชีที่เปิด HOSxP อยู่บนเครื่องนี้"
        btnHosxp.Visible = False
        '
        'banner
        '
        banner.Dock = DockStyle.Top
        banner.Location = New Point(28, 213)
        banner.Margin = New Padding(3, 10, 3, 3)
        banner.Name = "banner"
        banner.Size = New Size(424, 44)
        banner.TabIndex = 5
        banner.Visible = False
        '
        'pnlFooter
        '
        pnlFooter.BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        pnlFooter.Controls.Add(flpFooter)
        pnlFooter.Controls.Add(lblVersion)
        pnlFooter.Dock = DockStyle.Bottom
        pnlFooter.Location = New Point(0, 376)
        pnlFooter.Name = "pnlFooter"
        pnlFooter.Size = New Size(480, 72)
        pnlFooter.TabIndex = 2
        '
        'lblVersion
        '
        lblVersion.AutoSize = True
        lblVersion.Font = New Font("Leelawadee UI", 8.5F)
        lblVersion.ForeColor = Color.FromArgb(CByte(148), CByte(163), CByte(184))
        lblVersion.Location = New Point(28, 28)
        lblVersion.Name = "lblVersion"
        lblVersion.Size = New Size(200, 16)
        lblVersion.TabIndex = 0
        lblVersion.Text = "v2.0.0  ·  Developed by Komsan Asa"
        '
        'flpFooter
        '
        flpFooter.AutoSize = True
        flpFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink
        flpFooter.Controls.Add(btnSignIn)
        flpFooter.Controls.Add(btnCancel)
        flpFooter.Dock = DockStyle.Right
        flpFooter.FlowDirection = FlowDirection.RightToLeft
        flpFooter.Location = New Point(260, 0)
        flpFooter.Name = "flpFooter"
        flpFooter.Padding = New Padding(0, 16, 22, 16)
        flpFooter.Size = New Size(220, 72)
        flpFooter.TabIndex = 1
        flpFooter.WrapContents = False
        '
        'btnSignIn
        '
        btnSignIn.AutoSize = True
        btnSignIn.AutoSizeMode = AutoSizeMode.GrowAndShrink
        btnSignIn.IconKind = IconKind.Lock
        btnSignIn.Location = New Point(112, 19)
        btnSignIn.Name = "btnSignIn"
        btnSignIn.Size = New Size(102, 40)
        btnSignIn.TabIndex = 0
        btnSignIn.Text = "เข้าสู่ระบบ"
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
        'AdminLoginDialog
        '
        AcceptButton = btnSignIn
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        BackColor = Color.White
        CancelButton = btnCancel
        ClientSize = New Size(480, 448)
        Controls.Add(tlpBody)
        Controls.Add(pnlFooter)
        Controls.Add(pnlHeader)
        Font = New Font("Leelawadee UI", 10.0F)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "AdminLoginDialog"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "เข้าสู่ระบบผู้ดูแล"
        pnlHeader.ResumeLayout(False)
        pnlHeader.PerformLayout()
        tlpBody.ResumeLayout(False)
        tlpBody.PerformLayout()
        pnlFooter.ResumeLayout(False)
        pnlFooter.PerformLayout()
        flpFooter.ResumeLayout(False)
        flpFooter.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlHeader As Panel
    Friend WithEvents lblDesc As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents tileIcon As IconTile
    Friend WithEvents tlpBody As TableLayoutPanel
    Friend WithEvents lblUser As Label
    Friend WithEvents txtUser As ModernTextBox
    Friend WithEvents lblPassword As Label
    Friend WithEvents txtPassword As ModernTextBox
    Friend WithEvents btnHosxp As ModernButton
    Friend WithEvents banner As InfoBanner
    Friend WithEvents pnlFooter As Panel
    Friend WithEvents lblVersion As Label
    Friend WithEvents flpFooter As FlowLayoutPanel
    Friend WithEvents btnSignIn As ModernButton
    Friend WithEvents btnCancel As ModernButton
    Friend WithEvents tip As ToolTip
End Class

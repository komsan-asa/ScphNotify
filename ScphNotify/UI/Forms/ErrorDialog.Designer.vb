<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ErrorDialog
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
        lblMessage = New Label()
        lblTitle = New Label()
        tileIcon = New IconTile()
        pnlFooter = New Panel()
        lnkDetails = New LinkLabel()
        btnCopy = New ModernButton()
        btnClose = New ModernButton()
        pnlDetails = New Panel()
        txtDetails = New TextBox()
        pnlHeader.SuspendLayout()
        pnlFooter.SuspendLayout()
        pnlDetails.SuspendLayout()
        SuspendLayout()
        '
        'pnlHeader
        '
        pnlHeader.Controls.Add(lblMessage)
        pnlHeader.Controls.Add(lblTitle)
        pnlHeader.Controls.Add(tileIcon)
        pnlHeader.Dock = DockStyle.Top
        pnlHeader.Location = New Point(0, 0)
        pnlHeader.Name = "pnlHeader"
        pnlHeader.Size = New Size(560, 128)
        pnlHeader.TabIndex = 0
        '
        'lblMessage
        '
        lblMessage.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblMessage.Font = New Font("Leelawadee UI", 10.0F)
        lblMessage.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblMessage.Location = New Point(88, 54)
        lblMessage.Name = "lblMessage"
        lblMessage.Size = New Size(444, 66)
        lblMessage.TabIndex = 2
        lblMessage.Text = "-"
        '
        'lblTitle
        '
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Leelawadee UI", 13.0F, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblTitle.Location = New Point(86, 26)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(100, 24)
        lblTitle.TabIndex = 1
        lblTitle.Text = "เกิดข้อผิดพลาด"
        '
        'tileIcon
        '
        tileIcon.IconKind = IconKind.Warning
        tileIcon.Level = AlertLevel.Danger
        tileIcon.Location = New Point(28, 26)
        tileIcon.Name = "tileIcon"
        tileIcon.Size = New Size(46, 46)
        tileIcon.TabIndex = 0
        '
        'pnlFooter
        '
        pnlFooter.BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        pnlFooter.Controls.Add(lnkDetails)
        pnlFooter.Controls.Add(btnCopy)
        pnlFooter.Controls.Add(btnClose)
        pnlFooter.Dock = DockStyle.Bottom
        pnlFooter.Location = New Point(0, 136)
        pnlFooter.Name = "pnlFooter"
        pnlFooter.Size = New Size(560, 68)
        pnlFooter.TabIndex = 2
        '
        'lnkDetails
        '
        lnkDetails.ActiveLinkColor = Color.FromArgb(CByte(15), CByte(118), CByte(110))
        lnkDetails.AutoSize = True
        lnkDetails.LinkBehavior = LinkBehavior.HoverUnderline
        lnkDetails.LinkColor = Color.FromArgb(CByte(13), CByte(148), CByte(136))
        lnkDetails.Location = New Point(28, 25)
        lnkDetails.Name = "lnkDetails"
        lnkDetails.Size = New Size(110, 19)
        lnkDetails.TabIndex = 2
        lnkDetails.TabStop = True
        lnkDetails.Text = "แสดงรายละเอียด"
        '
        'btnCopy
        '
        btnCopy.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnCopy.Kind = ButtonVariant.Ghost
        btnCopy.Location = New Point(328, 14)
        btnCopy.Name = "btnCopy"
        btnCopy.Size = New Size(100, 40)
        btnCopy.TabIndex = 1
        btnCopy.Text = "คัดลอก"
        '
        'btnClose
        '
        btnClose.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnClose.DialogResult = DialogResult.OK
        btnClose.Location = New Point(434, 14)
        btnClose.Name = "btnClose"
        btnClose.Size = New Size(98, 40)
        btnClose.TabIndex = 0
        btnClose.Text = "ปิด"
        '
        'pnlDetails
        '
        pnlDetails.Controls.Add(txtDetails)
        pnlDetails.Dock = DockStyle.Fill
        pnlDetails.Location = New Point(0, 128)
        pnlDetails.Name = "pnlDetails"
        pnlDetails.Padding = New Padding(28, 0, 28, 12)
        pnlDetails.Size = New Size(560, 8)
        pnlDetails.TabIndex = 1
        pnlDetails.Visible = False
        '
        'txtDetails
        '
        txtDetails.BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        txtDetails.BorderStyle = BorderStyle.FixedSingle
        txtDetails.Dock = DockStyle.Fill
        txtDetails.Font = New Font("Consolas", 9.0F)
        txtDetails.Location = New Point(28, 0)
        txtDetails.Multiline = True
        txtDetails.Name = "txtDetails"
        txtDetails.ReadOnly = True
        txtDetails.ScrollBars = ScrollBars.Both
        txtDetails.Size = New Size(504, 0)
        txtDetails.TabIndex = 0
        txtDetails.WordWrap = False
        '
        'ErrorDialog
        '
        AcceptButton = btnClose
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        BackColor = Color.White
        CancelButton = btnClose
        ClientSize = New Size(560, 204)
        Controls.Add(pnlDetails)
        Controls.Add(pnlFooter)
        Controls.Add(pnlHeader)
        Font = New Font("Leelawadee UI", 10.0F)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "ErrorDialog"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "SCPH Notify"
        pnlHeader.ResumeLayout(False)
        pnlHeader.PerformLayout()
        pnlFooter.ResumeLayout(False)
        pnlFooter.PerformLayout()
        pnlDetails.ResumeLayout(False)
        pnlDetails.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlHeader As Panel
    Friend WithEvents lblMessage As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents tileIcon As IconTile
    Friend WithEvents pnlFooter As Panel
    Friend WithEvents lnkDetails As LinkLabel
    Friend WithEvents btnCopy As ModernButton
    Friend WithEvents btnClose As ModernButton
    Friend WithEvents pnlDetails As Panel
    Friend WithEvents txtDetails As TextBox
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MedicalCarePage
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
        tlpStatus = New TableLayoutPanel()
        cardDm = New CardPanel()
        flpDm = New FlowLayoutPanel()
        lblDmStatus = New Label()
        cardHt = New CardPanel()
        flpHt = New FlowLayoutPanel()
        lblHtStatus = New Label()
        cardHd = New CardPanel()
        lblHdNote = New Label()
        lblHdValue = New Label()
        cardEgfr = New CardPanel()
        lblEgfrNote = New Label()
        lblEgfrValue = New Label()
        tlpDetail = New TableLayoutPanel()
        cardScreening = New CardPanel()
        lblScreening = New Label()
        cardAppointments = New CardPanel()
        lstAppointments = New ModernListBox()
        lblNoAppointment = New Label()
        pnlPageHeader.SuspendLayout()
        tlpMain.SuspendLayout()
        tlpStatus.SuspendLayout()
        cardDm.SuspendLayout()
        cardHt.SuspendLayout()
        cardHd.SuspendLayout()
        cardEgfr.SuspendLayout()
        tlpDetail.SuspendLayout()
        cardScreening.SuspendLayout()
        cardAppointments.SuspendLayout()
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
        lblSubtitle.Size = New Size(420, 17)
        lblSubtitle.TabIndex = 1
        lblSubtitle.Text = "Medical Care · สถานะโรคเรื้อรัง การตรวจคัดกรอง และนัดหมายครั้งถัดไป"
        '
        'lblTitle
        '
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Leelawadee UI", 16.0F, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblTitle.Location = New Point(-2, 0)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(230, 30)
        lblTitle.TabIndex = 0
        lblTitle.Text = "ภาพรวมการดูแลผู้ป่วย"
        '
        'tlpMain
        '
        tlpMain.ColumnCount = 1
        tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        tlpMain.Controls.Add(tlpStatus, 0, 0)
        tlpMain.Controls.Add(tlpDetail, 0, 1)
        tlpMain.Dock = DockStyle.Fill
        tlpMain.Location = New Point(0, 66)
        tlpMain.Name = "tlpMain"
        tlpMain.RowCount = 2
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 172.0F))
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpMain.Size = New Size(900, 534)
        tlpMain.TabIndex = 1
        '
        'tlpStatus
        '
        tlpStatus.ColumnCount = 4
        tlpStatus.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        tlpStatus.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        tlpStatus.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        tlpStatus.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        tlpStatus.Controls.Add(cardDm, 0, 0)
        tlpStatus.Controls.Add(cardHt, 1, 0)
        tlpStatus.Controls.Add(cardHd, 2, 0)
        tlpStatus.Controls.Add(cardEgfr, 3, 0)
        tlpStatus.Dock = DockStyle.Fill
        tlpStatus.Location = New Point(0, 0)
        tlpStatus.Margin = New Padding(0)
        tlpStatus.Name = "tlpStatus"
        tlpStatus.RowCount = 1
        tlpStatus.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpStatus.Size = New Size(900, 172)
        tlpStatus.TabIndex = 0
        '
        'cardDm
        '
        cardDm.Controls.Add(flpDm)
        cardDm.Controls.Add(lblDmStatus)
        cardDm.Dock = DockStyle.Fill
        cardDm.IconKind = IconKind.Droplet
        cardDm.IconLevel = AlertLevel.Info
        cardDm.Location = New Point(0, 0)
        cardDm.Margin = New Padding(0, 0, 14, 16)
        cardDm.Name = "cardDm"
        cardDm.Size = New Size(211, 156)
        cardDm.TabIndex = 0
        cardDm.Title = "เบาหวาน (DM)"
        '
        'flpDm
        '
        flpDm.Dock = DockStyle.Fill
        flpDm.Location = New Point(16, 88)
        flpDm.Name = "flpDm"
        flpDm.Size = New Size(179, 52)
        flpDm.TabIndex = 1
        '
        'lblDmStatus
        '
        lblDmStatus.AutoEllipsis = True
        lblDmStatus.Dock = DockStyle.Top
        lblDmStatus.Font = New Font("Leelawadee UI", 11.0F, FontStyle.Bold)
        lblDmStatus.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblDmStatus.Location = New Point(16, 56)
        lblDmStatus.Name = "lblDmStatus"
        lblDmStatus.Size = New Size(179, 32)
        lblDmStatus.TabIndex = 0
        lblDmStatus.Text = "-"
        '
        'cardHt
        '
        cardHt.Controls.Add(flpHt)
        cardHt.Controls.Add(lblHtStatus)
        cardHt.Dock = DockStyle.Fill
        cardHt.IconKind = IconKind.Gauge
        cardHt.Location = New Point(225, 0)
        cardHt.Margin = New Padding(0, 0, 14, 16)
        cardHt.Name = "cardHt"
        cardHt.Size = New Size(211, 156)
        cardHt.TabIndex = 1
        cardHt.Title = "ความดัน (HT)"
        '
        'flpHt
        '
        flpHt.Dock = DockStyle.Fill
        flpHt.Location = New Point(16, 88)
        flpHt.Name = "flpHt"
        flpHt.Size = New Size(179, 52)
        flpHt.TabIndex = 1
        '
        'lblHtStatus
        '
        lblHtStatus.AutoEllipsis = True
        lblHtStatus.Dock = DockStyle.Top
        lblHtStatus.Font = New Font("Leelawadee UI", 11.0F, FontStyle.Bold)
        lblHtStatus.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblHtStatus.Location = New Point(16, 56)
        lblHtStatus.Name = "lblHtStatus"
        lblHtStatus.Size = New Size(179, 32)
        lblHtStatus.TabIndex = 0
        lblHtStatus.Text = "-"
        '
        'cardHd
        '
        cardHd.Controls.Add(lblHdNote)
        cardHd.Controls.Add(lblHdValue)
        cardHd.Dock = DockStyle.Fill
        cardHd.IconKind = IconKind.Filter
        cardHd.IconLevel = AlertLevel.Danger
        cardHd.Location = New Point(450, 0)
        cardHd.Margin = New Padding(0, 0, 14, 16)
        cardHd.Name = "cardHd"
        cardHd.Size = New Size(211, 156)
        cardHd.TabIndex = 2
        cardHd.Title = "ฟอกไต (HD / CAPD)"
        '
        'lblHdNote
        '
        lblHdNote.AutoEllipsis = True
        lblHdNote.Dock = DockStyle.Fill
        lblHdNote.Font = New Font("Leelawadee UI", 9.0F)
        lblHdNote.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblHdNote.Location = New Point(16, 96)
        lblHdNote.Name = "lblHdNote"
        lblHdNote.Size = New Size(179, 44)
        lblHdNote.TabIndex = 1
        lblHdNote.Text = "-"
        '
        'lblHdValue
        '
        lblHdValue.AutoEllipsis = True
        lblHdValue.Dock = DockStyle.Top
        lblHdValue.Font = New Font("Leelawadee UI", 18.0F, FontStyle.Bold)
        lblHdValue.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblHdValue.Location = New Point(16, 56)
        lblHdValue.Name = "lblHdValue"
        lblHdValue.Size = New Size(179, 40)
        lblHdValue.TabIndex = 0
        lblHdValue.Text = "-"
        '
        'cardEgfr
        '
        cardEgfr.Controls.Add(lblEgfrNote)
        cardEgfr.Controls.Add(lblEgfrValue)
        cardEgfr.Dock = DockStyle.Fill
        cardEgfr.IconKind = IconKind.Activity
        cardEgfr.IconLevel = AlertLevel.Warning
        cardEgfr.Location = New Point(675, 0)
        cardEgfr.Margin = New Padding(0, 0, 0, 16)
        cardEgfr.Name = "cardEgfr"
        cardEgfr.Size = New Size(225, 156)
        cardEgfr.TabIndex = 3
        cardEgfr.Title = "eGFR < 60 (1 ปี)"
        '
        'lblEgfrNote
        '
        lblEgfrNote.AutoEllipsis = True
        lblEgfrNote.Dock = DockStyle.Fill
        lblEgfrNote.Font = New Font("Leelawadee UI", 9.0F)
        lblEgfrNote.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblEgfrNote.Location = New Point(16, 96)
        lblEgfrNote.Name = "lblEgfrNote"
        lblEgfrNote.Size = New Size(193, 44)
        lblEgfrNote.TabIndex = 1
        lblEgfrNote.Text = "-"
        '
        'lblEgfrValue
        '
        lblEgfrValue.AutoEllipsis = True
        lblEgfrValue.Dock = DockStyle.Top
        lblEgfrValue.Font = New Font("Leelawadee UI", 18.0F, FontStyle.Bold)
        lblEgfrValue.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblEgfrValue.Location = New Point(16, 56)
        lblEgfrValue.Name = "lblEgfrValue"
        lblEgfrValue.Size = New Size(193, 40)
        lblEgfrValue.TabIndex = 0
        lblEgfrValue.Text = "-"
        '
        'tlpDetail
        '
        tlpDetail.ColumnCount = 2
        tlpDetail.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tlpDetail.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tlpDetail.Controls.Add(cardScreening, 0, 0)
        tlpDetail.Controls.Add(cardAppointments, 1, 0)
        tlpDetail.Dock = DockStyle.Fill
        tlpDetail.Location = New Point(0, 172)
        tlpDetail.Margin = New Padding(0)
        tlpDetail.Name = "tlpDetail"
        tlpDetail.RowCount = 1
        tlpDetail.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpDetail.Size = New Size(900, 362)
        tlpDetail.TabIndex = 1
        '
        'cardScreening
        '
        cardScreening.Controls.Add(lblScreening)
        cardScreening.Dock = DockStyle.Fill
        cardScreening.IconKind = IconKind.ClipboardCheck
        cardScreening.Location = New Point(0, 0)
        cardScreening.Margin = New Padding(0, 0, 14, 0)
        cardScreening.Name = "cardScreening"
        cardScreening.Size = New Size(436, 362)
        cardScreening.Subtitle = "สรุปจากฟังก์ชัน HtDmCheckLabComplete ของ HOSxP"
        cardScreening.TabIndex = 0
        cardScreening.Title = "ตรวจคัดกรอง ตา ไต เท้า"
        '
        'lblScreening
        '
        lblScreening.Dock = DockStyle.Fill
        lblScreening.Font = New Font("Leelawadee UI", 11.0F)
        lblScreening.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblScreening.Location = New Point(16, 70)
        lblScreening.Name = "lblScreening"
        lblScreening.Size = New Size(404, 276)
        lblScreening.TabIndex = 0
        lblScreening.Text = "-"
        '
        'cardAppointments
        '
        cardAppointments.Controls.Add(lstAppointments)
        cardAppointments.Controls.Add(lblNoAppointment)
        cardAppointments.Dock = DockStyle.Fill
        cardAppointments.IconKind = IconKind.Calendar
        cardAppointments.IconLevel = AlertLevel.Info
        cardAppointments.Location = New Point(450, 0)
        cardAppointments.Margin = New Padding(0)
        cardAppointments.Name = "cardAppointments"
        cardAppointments.Size = New Size(450, 362)
        cardAppointments.Subtitle = "นัดหมายที่ยังไม่ถึงกำหนด"
        cardAppointments.TabIndex = 1
        cardAppointments.Title = "นัดครั้งถัดไป"
        '
        'lstAppointments
        '
        lstAppointments.DefaultIcon = IconKind.Calendar
        lstAppointments.DefaultLevel = AlertLevel.Info
        lstAppointments.Dock = DockStyle.Fill
        lstAppointments.Location = New Point(16, 70)
        lstAppointments.Name = "lstAppointments"
        lstAppointments.Size = New Size(418, 276)
        lstAppointments.TabIndex = 0
        '
        'lblNoAppointment
        '
        lblNoAppointment.Dock = DockStyle.Fill
        lblNoAppointment.ForeColor = Color.FromArgb(CByte(148), CByte(163), CByte(184))
        lblNoAppointment.Location = New Point(16, 70)
        lblNoAppointment.Name = "lblNoAppointment"
        lblNoAppointment.Size = New Size(418, 276)
        lblNoAppointment.TabIndex = 1
        lblNoAppointment.Text = "ไม่มีนัดหมายครั้งถัดไป"
        lblNoAppointment.TextAlign = ContentAlignment.MiddleCenter
        lblNoAppointment.Visible = False
        '
        'MedicalCarePage
        '
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        AutoScroll = True
        AutoScrollMinSize = New Size(760, 500)
        BackColor = Color.FromArgb(CByte(244), CByte(246), CByte(250))
        Controls.Add(tlpMain)
        Controls.Add(pnlPageHeader)
        Font = New Font("Leelawadee UI", 10.0F)
        Name = "MedicalCarePage"
        Size = New Size(900, 600)
        pnlPageHeader.ResumeLayout(False)
        pnlPageHeader.PerformLayout()
        tlpMain.ResumeLayout(False)
        tlpStatus.ResumeLayout(False)
        cardDm.ResumeLayout(False)
        cardHt.ResumeLayout(False)
        cardHd.ResumeLayout(False)
        cardEgfr.ResumeLayout(False)
        tlpDetail.ResumeLayout(False)
        cardScreening.ResumeLayout(False)
        cardAppointments.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlPageHeader As Panel
    Friend WithEvents lblSubtitle As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents tlpStatus As TableLayoutPanel
    Friend WithEvents cardDm As CardPanel
    Friend WithEvents flpDm As FlowLayoutPanel
    Friend WithEvents lblDmStatus As Label
    Friend WithEvents cardHt As CardPanel
    Friend WithEvents flpHt As FlowLayoutPanel
    Friend WithEvents lblHtStatus As Label
    Friend WithEvents cardHd As CardPanel
    Friend WithEvents lblHdNote As Label
    Friend WithEvents lblHdValue As Label
    Friend WithEvents cardEgfr As CardPanel
    Friend WithEvents lblEgfrNote As Label
    Friend WithEvents lblEgfrValue As Label
    Friend WithEvents tlpDetail As TableLayoutPanel
    Friend WithEvents cardScreening As CardPanel
    Friend WithEvents lblScreening As Label
    Friend WithEvents cardAppointments As CardPanel
    Friend WithEvents lstAppointments As ModernListBox
    Friend WithEvents lblNoAppointment As Label
End Class

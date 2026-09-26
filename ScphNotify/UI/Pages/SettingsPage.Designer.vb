<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SettingsPage
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
        pnlFooter = New Panel()
        lblSaveStatus = New Label()
        btnSave = New ModernButton()
        tlpMain = New TableLayoutPanel()
        cardDisplay = New CardPanel()
        flpDisplay = New FlowLayoutPanel()
        lblGroupMedical = New Label()
        tgDm = New ToggleSwitch()
        tgHt = New ToggleSwitch()
        tgHd = New ToggleSwitch()
        tgEgfr = New ToggleSwitch()
        tgScreen = New ToggleSwitch()
        tgAppt = New ToggleSwitch()
        lblGroupLab = New Label()
        tgFerritin = New ToggleSwitch()
        lblGroupOccupational = New Label()
        tgOccupational = New ToggleSwitch()
        tgCkd = New ToggleSwitch()
        tlpRight = New TableLayoutPanel()
        cardApp = New CardPanel()
        flpApp = New FlowLayoutPanel()
        flpInterval = New FlowLayoutPanel()
        lblInterval = New Label()
        nudInterval = New NumericUpDown()
        lblIntervalUnit = New Label()
        tgTopMost = New ToggleSwitch()
        cardDb = New CardPanel()
        flpDb = New FlowLayoutPanel()
        lblDbServer = New Label()
        lblDbName = New Label()
        lblComputer = New Label()
        btnDbConfig = New ModernButton()
        btnLogout = New ModernButton()
        flpIdle = New FlowLayoutPanel()
        lblIdle = New Label()
        nudIdle = New NumericUpDown()
        lblIdleUnit = New Label()
        lblIdleNote = New Label()
        lblShortcut = New Label()
        pnlPageHeader.SuspendLayout()
        pnlFooter.SuspendLayout()
        tlpMain.SuspendLayout()
        cardDisplay.SuspendLayout()
        flpDisplay.SuspendLayout()
        tlpRight.SuspendLayout()
        cardApp.SuspendLayout()
        flpApp.SuspendLayout()
        flpInterval.SuspendLayout()
        CType(nudInterval, System.ComponentModel.ISupportInitialize).BeginInit()
        flpIdle.SuspendLayout()
        CType(nudIdle, System.ComponentModel.ISupportInitialize).BeginInit()
        cardDb.SuspendLayout()
        flpDb.SuspendLayout()
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
        lblSubtitle.Size = New Size(400, 17)
        lblSubtitle.TabIndex = 1
        lblSubtitle.Text = "Setting Notify · การแสดงผลของเครื่องนี้ และการเชื่อมต่อฐานข้อมูล"
        '
        'lblTitle
        '
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Leelawadee UI", 16.0F, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblTitle.Location = New Point(-2, 0)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(80, 30)
        lblTitle.TabIndex = 0
        lblTitle.Text = "ตั้งค่า"
        '
        'pnlFooter
        '
        pnlFooter.Controls.Add(lblSaveStatus)
        pnlFooter.Controls.Add(btnSave)
        pnlFooter.Dock = DockStyle.Bottom
        pnlFooter.Location = New Point(0, 540)
        pnlFooter.Name = "pnlFooter"
        pnlFooter.Size = New Size(900, 60)
        pnlFooter.TabIndex = 2
        '
        'lblSaveStatus
        '
        lblSaveStatus.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblSaveStatus.AutoEllipsis = True
        lblSaveStatus.Font = New Font("Leelawadee UI", 9.5F)
        lblSaveStatus.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblSaveStatus.Location = New Point(0, 26)
        lblSaveStatus.Name = "lblSaveStatus"
        lblSaveStatus.Size = New Size(740, 22)
        lblSaveStatus.TabIndex = 1
        lblSaveStatus.Text = "การตั้งค่าการแสดงผลจะมีผลกับเครื่องนี้เท่านั้น"
        lblSaveStatus.TextAlign = ContentAlignment.MiddleLeft
        '
        'btnSave
        '
        btnSave.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnSave.IconKind = IconKind.Check
        btnSave.Location = New Point(752, 16)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(148, 42)
        btnSave.TabIndex = 0
        btnSave.Text = "บันทึกการตั้งค่า"
        '
        'tlpMain
        '
        tlpMain.ColumnCount = 2
        tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tlpMain.Controls.Add(cardDisplay, 0, 0)
        tlpMain.Controls.Add(tlpRight, 1, 0)
        tlpMain.Dock = DockStyle.Fill
        tlpMain.Location = New Point(0, 66)
        tlpMain.Name = "tlpMain"
        tlpMain.RowCount = 1
        tlpMain.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpMain.Size = New Size(900, 474)
        tlpMain.TabIndex = 1
        '
        'cardDisplay
        '
        cardDisplay.Controls.Add(flpDisplay)
        cardDisplay.Dock = DockStyle.Fill
        cardDisplay.IconKind = IconKind.Eye
        cardDisplay.Location = New Point(0, 0)
        cardDisplay.Margin = New Padding(0, 0, 16, 0)
        cardDisplay.Name = "cardDisplay"
        cardDisplay.Size = New Size(434, 474)
        cardDisplay.Subtitle = "บันทึกแยกตามเครื่อง ในตาราง app_storage ของ HOSxP"
        cardDisplay.TabIndex = 0
        cardDisplay.Title = "การแสดงผลของเครื่องนี้"
        '
        'flpDisplay
        '
        flpDisplay.AutoScroll = True
        flpDisplay.Controls.Add(lblGroupOccupational)
        flpDisplay.Controls.Add(tgOccupational)
        flpDisplay.Controls.Add(lblGroupMedical)
        flpDisplay.Controls.Add(tgDm)
        flpDisplay.Controls.Add(tgHt)
        flpDisplay.Controls.Add(tgHd)
        flpDisplay.Controls.Add(tgEgfr)
        flpDisplay.Controls.Add(tgScreen)
        flpDisplay.Controls.Add(tgAppt)
        flpDisplay.Controls.Add(lblGroupLab)
        flpDisplay.Controls.Add(tgFerritin)
        flpDisplay.Controls.Add(tgCkd)
        flpDisplay.Dock = DockStyle.Fill
        flpDisplay.FlowDirection = FlowDirection.TopDown
        flpDisplay.Location = New Point(16, 70)
        flpDisplay.Name = "flpDisplay"
        flpDisplay.Size = New Size(402, 388)
        flpDisplay.TabIndex = 0
        flpDisplay.WrapContents = False
        '
        'lblGroupMedical
        '
        lblGroupMedical.AutoSize = True
        lblGroupMedical.Font = New Font("Leelawadee UI", 8.5F, FontStyle.Bold)
        lblGroupMedical.ForeColor = Color.FromArgb(CByte(148), CByte(163), CByte(184))
        lblGroupMedical.Location = New Point(0, 4)
        lblGroupMedical.Margin = New Padding(0, 4, 0, 4)
        lblGroupMedical.Name = "lblGroupMedical"
        lblGroupMedical.Size = New Size(90, 16)
        lblGroupMedical.TabIndex = 0
        lblGroupMedical.Text = "MEDICAL CARE"
        '
        'tgDm
        '
        tgDm.Description = "การวินิจฉัย / การลงทะเบียนคลินิก / DM Green"
        tgDm.Location = New Point(0, 24)
        tgDm.Margin = New Padding(0, 0, 0, 4)
        tgDm.Name = "tgDm"
        tgDm.Size = New Size(380, 46)
        tgDm.TabIndex = 1
        tgDm.Text = "สถานะเบาหวาน (DM)"
        '
        'tgHt
        '
        tgHt.Description = "การวินิจฉัย / การลงทะเบียนคลินิก / HT Green"
        tgHt.Location = New Point(0, 74)
        tgHt.Margin = New Padding(0, 0, 0, 4)
        tgHt.Name = "tgHt"
        tgHt.Size = New Size(380, 46)
        tgHt.TabIndex = 2
        tgHt.Text = "สถานะความดันโลหิตสูง (HT)"
        '
        'tgHd
        '
        tgHd.Description = "ผู้ป่วยฟอกไตทางเลือด / ทางช่องท้อง"
        tgHd.Location = New Point(0, 124)
        tgHd.Margin = New Padding(0, 0, 0, 4)
        tgHd.Name = "tgHd"
        tgHd.Size = New Size(380, 46)
        tgHd.TabIndex = 3
        tgHd.Text = "Case HD / CAPD"
        '
        'tgEgfr
        '
        tgEgfr.Description = "ค่า eGFR ต่ำกว่า 60 ภายใน 1 ปี"
        tgEgfr.Location = New Point(0, 174)
        tgEgfr.Margin = New Padding(0, 0, 0, 4)
        tgEgfr.Name = "tgEgfr"
        tgEgfr.Size = New Size(380, 46)
        tgEgfr.TabIndex = 4
        tgEgfr.Text = "eGFR < 60"
        '
        'tgScreen
        '
        tgScreen.Description = "สรุปการตรวจตา ไต เท้า ของผู้ป่วย DM / HT"
        tgScreen.Location = New Point(0, 224)
        tgScreen.Margin = New Padding(0, 0, 0, 4)
        tgScreen.Name = "tgScreen"
        tgScreen.Size = New Size(380, 46)
        tgScreen.TabIndex = 5
        tgScreen.Text = "ตรวจคัดกรอง ตา ไต เท้า"
        '
        'tgAppt
        '
        tgAppt.Description = "นัดหมายที่ยังไม่ถึงกำหนด"
        tgAppt.Location = New Point(0, 274)
        tgAppt.Margin = New Padding(0, 0, 0, 4)
        tgAppt.Name = "tgAppt"
        tgAppt.Size = New Size(380, 46)
        tgAppt.TabIndex = 6
        tgAppt.Text = "นัดครั้งถัดไป"
        '
        'lblGroupLab
        '
        lblGroupLab.AutoSize = True
        lblGroupLab.Font = New Font("Leelawadee UI", 8.5F, FontStyle.Bold)
        lblGroupLab.ForeColor = Color.FromArgb(CByte(148), CByte(163), CByte(184))
        lblGroupLab.Location = New Point(0, 336)
        lblGroupLab.Margin = New Padding(0, 12, 0, 4)
        lblGroupLab.Name = "lblGroupLab"
        lblGroupLab.Size = New Size(130, 16)
        lblGroupLab.TabIndex = 7
        lblGroupLab.Text = "LAB / DRUG MONITOR"
        '
        'tgFerritin
        '
        tgFerritin.Description = "ผล Ferritin ล่าสุด 3 ครั้งในหน้า LAB"
        tgFerritin.Location = New Point(0, 356)
        tgFerritin.Margin = New Padding(0, 0, 0, 4)
        tgFerritin.Name = "tgFerritin"
        tgFerritin.Size = New Size(380, 46)
        tgFerritin.TabIndex = 8
        tgFerritin.Text = "Ferritin"
        '
        'tgCkd
        '
        tgCkd.Description = "Drug Monitor — เร็วๆ นี้"
        tgCkd.Enabled = False
        tgCkd.Location = New Point(0, 406)
        tgCkd.Margin = New Padding(0, 0, 0, 4)
        tgCkd.Name = "tgCkd"
        tgCkd.Size = New Size(380, 46)
        tgCkd.TabIndex = 9
        tgCkd.Text = "ผู้ป่วยไตเรื้อรัง ระยะที่..."
        '
        'lblGroupOccupational
        '
        lblGroupOccupational.AutoSize = True
        lblGroupOccupational.Font = New Font("Leelawadee UI", 8.5F, FontStyle.Bold)
        lblGroupOccupational.ForeColor = Color.FromArgb(CByte(148), CByte(163), CByte(184))
        lblGroupOccupational.Location = New Point(0, 466)
        lblGroupOccupational.Margin = New Padding(0, 4, 0, 4)
        lblGroupOccupational.Name = "lblGroupOccupational"
        lblGroupOccupational.Size = New Size(150, 16)
        lblGroupOccupational.TabIndex = 10
        lblGroupOccupational.Text = "อาชีวอนามัย / โรคจากการทำงาน"
        '
        'tgOccupational
        '
        tgOccupational.Description = "หน้าต่างคัดกรองอัตโนมัติ + เมนูทะเบียนคัดกรอง"
        tgOccupational.Location = New Point(0, 486)
        tgOccupational.Margin = New Padding(0, 0, 0, 4)
        tgOccupational.Name = "tgOccupational"
        tgOccupational.Size = New Size(380, 46)
        tgOccupational.TabIndex = 11
        tgOccupational.Text = "คัดกรองโรคจากการทำงาน"
        '
        'tlpRight
        '
        tlpRight.ColumnCount = 1
        tlpRight.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        tlpRight.Controls.Add(cardApp, 0, 0)
        tlpRight.Controls.Add(cardDb, 0, 1)
        tlpRight.Dock = DockStyle.Fill
        tlpRight.Location = New Point(450, 0)
        tlpRight.Margin = New Padding(0)
        tlpRight.Name = "tlpRight"
        tlpRight.RowCount = 2
        tlpRight.RowStyles.Add(New RowStyle(SizeType.Absolute, 252.0F))
        tlpRight.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tlpRight.Size = New Size(450, 474)
        tlpRight.TabIndex = 1
        '
        'cardApp
        '
        cardApp.Controls.Add(flpApp)
        cardApp.Dock = DockStyle.Fill
        cardApp.IconKind = IconKind.Clock
        cardApp.Location = New Point(0, 0)
        cardApp.Margin = New Padding(0, 0, 0, 16)
        cardApp.Name = "cardApp"
        cardApp.Size = New Size(450, 236)
        cardApp.Subtitle = "บันทึกไว้ในเครื่องนี้ (%LOCALAPPDATA%\ScphNotify)"
        cardApp.TabIndex = 0
        cardApp.Title = "การทำงานของโปรแกรม"
        '
        'flpApp
        '
        flpApp.Controls.Add(flpInterval)
        flpApp.Controls.Add(flpIdle)
        flpApp.Controls.Add(lblIdleNote)
        flpApp.Controls.Add(tgTopMost)
        flpApp.Dock = DockStyle.Fill
        flpApp.FlowDirection = FlowDirection.TopDown
        flpApp.Location = New Point(16, 70)
        flpApp.Name = "flpApp"
        flpApp.Size = New Size(418, 150)
        flpApp.TabIndex = 0
        flpApp.WrapContents = False
        '
        'flpInterval
        '
        flpInterval.AutoSize = True
        flpInterval.Controls.Add(lblInterval)
        flpInterval.Controls.Add(nudInterval)
        flpInterval.Controls.Add(lblIntervalUnit)
        flpInterval.Location = New Point(0, 0)
        flpInterval.Margin = New Padding(0, 0, 0, 6)
        flpInterval.Name = "flpInterval"
        flpInterval.Size = New Size(260, 34)
        flpInterval.TabIndex = 0
        flpInterval.WrapContents = False
        '
        'lblInterval
        '
        lblInterval.AutoSize = True
        lblInterval.Location = New Point(0, 8)
        lblInterval.Margin = New Padding(0, 8, 8, 0)
        lblInterval.Name = "lblInterval"
        lblInterval.Size = New Size(120, 19)
        lblInterval.TabIndex = 0
        lblInterval.Text = "ตรวจสอบผู้ป่วยทุก"
        '
        'nudInterval
        '
        nudInterval.Font = New Font("Leelawadee UI", 11.0F)
        nudInterval.Location = New Point(128, 3)
        nudInterval.Maximum = New Decimal(New Integer() {120, 0, 0, 0})
        nudInterval.Minimum = New Decimal(New Integer() {3, 0, 0, 0})
        nudInterval.Name = "nudInterval"
        nudInterval.Size = New Size(72, 27)
        nudInterval.TabIndex = 1
        nudInterval.TextAlign = HorizontalAlignment.Center
        nudInterval.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'lblIntervalUnit
        '
        lblIntervalUnit.AutoSize = True
        lblIntervalUnit.Location = New Point(211, 8)
        lblIntervalUnit.Margin = New Padding(8, 8, 0, 0)
        lblIntervalUnit.Name = "lblIntervalUnit"
        lblIntervalUnit.Size = New Size(45, 19)
        lblIntervalUnit.TabIndex = 2
        lblIntervalUnit.Text = "วินาที"
        '
        'tgTopMost
        '
        tgTopMost.Description = "แสดงทับหน้าต่าง HOSxP ตลอดเวลา"
        tgTopMost.Location = New Point(0, 40)
        tgTopMost.Margin = New Padding(0)
        tgTopMost.Name = "tgTopMost"
        tgTopMost.Size = New Size(380, 46)
        tgTopMost.TabIndex = 1
        tgTopMost.Text = "แถบแจ้งเตือนอยู่บนสุดเสมอ"
        '
        'cardDb
        '
        cardDb.Controls.Add(flpDb)
        cardDb.Dock = DockStyle.Fill
        cardDb.IconKind = IconKind.Database
        cardDb.IconLevel = AlertLevel.Info
        cardDb.Location = New Point(0, 252)
        cardDb.Margin = New Padding(0)
        cardDb.Name = "cardDb"
        cardDb.Size = New Size(450, 222)
        cardDb.TabIndex = 1
        cardDb.Title = "ฐานข้อมูล HOSxP"
        '
        'flpDb
        '
        flpDb.Controls.Add(lblDbServer)
        flpDb.Controls.Add(lblDbName)
        flpDb.Controls.Add(lblComputer)
        flpDb.Controls.Add(btnDbConfig)
        flpDb.Controls.Add(btnLogout)
        flpDb.Controls.Add(lblShortcut)
        flpDb.Dock = DockStyle.Fill
        flpDb.FlowDirection = FlowDirection.TopDown
        flpDb.Location = New Point(16, 56)
        flpDb.Name = "flpDb"
        flpDb.Size = New Size(418, 150)
        flpDb.TabIndex = 0
        flpDb.WrapContents = False
        '
        'lblDbServer
        '
        lblDbServer.AutoSize = True
        lblDbServer.Font = New Font("Leelawadee UI", 13.0F, FontStyle.Bold)
        lblDbServer.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblDbServer.Location = New Point(0, 0)
        lblDbServer.Margin = New Padding(0)
        lblDbServer.Name = "lblDbServer"
        lblDbServer.Size = New Size(160, 24)
        lblDbServer.TabIndex = 0
        lblDbServer.Text = "192.168.2.1 : 3306"
        '
        'lblDbName
        '
        lblDbName.AutoSize = True
        lblDbName.Font = New Font("Leelawadee UI", 9.5F)
        lblDbName.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblDbName.Location = New Point(0, 26)
        lblDbName.Margin = New Padding(0, 2, 0, 0)
        lblDbName.Name = "lblDbName"
        lblDbName.Size = New Size(160, 17)
        lblDbName.TabIndex = 1
        lblDbName.Text = "ฐานข้อมูล hos · ผู้ใช้ sa"
        '
        'lblComputer
        '
        lblComputer.AutoSize = True
        lblComputer.Font = New Font("Leelawadee UI", 9.5F)
        lblComputer.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblComputer.Location = New Point(0, 45)
        lblComputer.Margin = New Padding(0, 2, 0, 14)
        lblComputer.Name = "lblComputer"
        lblComputer.Size = New Size(160, 17)
        lblComputer.TabIndex = 2
        lblComputer.Text = "ชื่อเครื่องนี้: -"
        '
        'btnDbConfig
        '
        btnDbConfig.IconKind = IconKind.Server
        btnDbConfig.Kind = ButtonVariant.Secondary
        btnDbConfig.Location = New Point(0, 76)
        btnDbConfig.Margin = New Padding(0)
        btnDbConfig.Name = "btnDbConfig"
        btnDbConfig.Size = New Size(230, 42)
        btnDbConfig.TabIndex = 3
        btnDbConfig.Text = "ตั้งค่าการเชื่อมต่อ..."
        '
        'btnLogout
        '
        btnLogout.IconKind = IconKind.Power
        btnLogout.Kind = ButtonVariant.Danger
        btnLogout.Location = New Point(0, 126)
        btnLogout.Margin = New Padding(0, 8, 0, 0)
        btnLogout.Name = "btnLogout"
        btnLogout.Size = New Size(230, 42)
        btnLogout.TabIndex = 4
        btnLogout.Text = "ออกจากระบบผู้ดูแล"
        btnLogout.Visible = False
        '
        'flpIdle
        '
        flpIdle.AutoSize = True
        flpIdle.Controls.Add(lblIdle)
        flpIdle.Controls.Add(nudIdle)
        flpIdle.Controls.Add(lblIdleUnit)
        flpIdle.Location = New Point(0, 40)
        flpIdle.Margin = New Padding(0, 0, 0, 2)
        flpIdle.Name = "flpIdle"
        flpIdle.Size = New Size(400, 34)
        flpIdle.TabIndex = 5
        flpIdle.WrapContents = False
        '
        'lblIdle
        '
        lblIdle.AutoSize = True
        lblIdle.Location = New Point(0, 8)
        lblIdle.Margin = New Padding(0, 8, 8, 0)
        lblIdle.Name = "lblIdle"
        lblIdle.Size = New Size(196, 19)
        lblIdle.TabIndex = 0
        lblIdle.Text = "ออกจากระบบอัตโนมัติเมื่อไม่ใช้งาน"
        '
        'nudIdle
        '
        nudIdle.Font = New Font("Leelawadee UI", 11.0F)
        nudIdle.Location = New Point(204, 3)
        nudIdle.Maximum = New Decimal(New Integer() {240, 0, 0, 0})
        nudIdle.Minimum = New Decimal(New Integer() {0, 0, 0, 0})
        nudIdle.Name = "nudIdle"
        nudIdle.Size = New Size(72, 27)
        nudIdle.TabIndex = 1
        nudIdle.TextAlign = HorizontalAlignment.Center
        nudIdle.Value = New Decimal(New Integer() {30, 0, 0, 0})
        '
        'lblIdleUnit
        '
        lblIdleUnit.AutoSize = True
        lblIdleUnit.Location = New Point(287, 8)
        lblIdleUnit.Margin = New Padding(8, 8, 0, 0)
        lblIdleUnit.Name = "lblIdleUnit"
        lblIdleUnit.Size = New Size(35, 19)
        lblIdleUnit.TabIndex = 2
        lblIdleUnit.Text = "นาที"
        '
        'lblIdleNote
        '
        lblIdleNote.AutoSize = True
        lblIdleNote.Font = New Font("Leelawadee UI", 8.5F)
        lblIdleNote.ForeColor = Color.FromArgb(CByte(148), CByte(163), CByte(184))
        lblIdleNote.Location = New Point(0, 76)
        lblIdleNote.Margin = New Padding(0, 4, 0, 0)
        lblIdleNote.Name = "lblIdleNote"
        lblIdleNote.Size = New Size(330, 16)
        lblIdleNote.TabIndex = 6
        lblIdleNote.Text = "นับเฉพาะการใช้งานโปรแกรมนี้  ·  ใส่ 0 = ไม่ออกอัตโนมัติ"
        '
        'lblShortcut
        '
        lblShortcut.AutoSize = True
        lblShortcut.Font = New Font("Leelawadee UI", 8.5F)
        lblShortcut.ForeColor = Color.FromArgb(CByte(148), CByte(163), CByte(184))
        lblShortcut.Location = New Point(0, 126)
        lblShortcut.Margin = New Padding(0, 8, 0, 0)
        lblShortcut.Name = "lblShortcut"
        lblShortcut.Size = New Size(200, 16)
        lblShortcut.TabIndex = 7
        lblShortcut.Text = "หรือกด Ctrl+F6 จากหน้าต่างใดก็ได้"
        '
        'SettingsPage
        '
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        AutoScroll = True
        AutoScrollMinSize = New Size(820, 680)
        BackColor = Color.FromArgb(CByte(244), CByte(246), CByte(250))
        Controls.Add(tlpMain)
        Controls.Add(pnlFooter)
        Controls.Add(pnlPageHeader)
        Font = New Font("Leelawadee UI", 10.0F)
        Name = "SettingsPage"
        Size = New Size(900, 600)
        pnlPageHeader.ResumeLayout(False)
        pnlPageHeader.PerformLayout()
        pnlFooter.ResumeLayout(False)
        tlpMain.ResumeLayout(False)
        cardDisplay.ResumeLayout(False)
        flpDisplay.ResumeLayout(False)
        flpDisplay.PerformLayout()
        tlpRight.ResumeLayout(False)
        cardApp.ResumeLayout(False)
        flpApp.ResumeLayout(False)
        flpApp.PerformLayout()
        flpInterval.ResumeLayout(False)
        flpInterval.PerformLayout()
        CType(nudInterval, System.ComponentModel.ISupportInitialize).EndInit()
        flpIdle.ResumeLayout(False)
        flpIdle.PerformLayout()
        CType(nudIdle, System.ComponentModel.ISupportInitialize).EndInit()
        cardDb.ResumeLayout(False)
        flpDb.ResumeLayout(False)
        flpDb.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlPageHeader As Panel
    Friend WithEvents lblSubtitle As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents pnlFooter As Panel
    Friend WithEvents lblSaveStatus As Label
    Friend WithEvents btnSave As ModernButton
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents cardDisplay As CardPanel
    Friend WithEvents flpDisplay As FlowLayoutPanel
    Friend WithEvents lblGroupMedical As Label
    Friend WithEvents tgDm As ToggleSwitch
    Friend WithEvents tgHt As ToggleSwitch
    Friend WithEvents tgHd As ToggleSwitch
    Friend WithEvents tgEgfr As ToggleSwitch
    Friend WithEvents tgScreen As ToggleSwitch
    Friend WithEvents tgAppt As ToggleSwitch
    Friend WithEvents lblGroupLab As Label
    Friend WithEvents tgFerritin As ToggleSwitch
    Friend WithEvents lblGroupOccupational As Label
    Friend WithEvents tgOccupational As ToggleSwitch
    Friend WithEvents tgCkd As ToggleSwitch
    Friend WithEvents tlpRight As TableLayoutPanel
    Friend WithEvents cardApp As CardPanel
    Friend WithEvents flpApp As FlowLayoutPanel
    Friend WithEvents flpInterval As FlowLayoutPanel
    Friend WithEvents lblInterval As Label
    Friend WithEvents nudInterval As NumericUpDown
    Friend WithEvents lblIntervalUnit As Label
    Friend WithEvents tgTopMost As ToggleSwitch
    Friend WithEvents cardDb As CardPanel
    Friend WithEvents flpDb As FlowLayoutPanel
    Friend WithEvents lblDbServer As Label
    Friend WithEvents lblDbName As Label
    Friend WithEvents lblComputer As Label
    Friend WithEvents btnDbConfig As ModernButton
    Friend WithEvents lblShortcut As Label
    Friend WithEvents btnLogout As ModernButton
    Friend WithEvents flpIdle As FlowLayoutPanel
    Friend WithEvents lblIdle As Label
    Friend WithEvents nudIdle As NumericUpDown
    Friend WithEvents lblIdleUnit As Label
    Friend WithEvents lblIdleNote As Label
End Class

''' <summary>หน้าต่างหลัก (แทน frmMain เดิม) — เมนูด้านซ้าย + ข้อมูลผู้ป่วยด้านบน + หน้าเนื้อหา</summary>
Public Class MainShell

    Private ReadOnly _pages As New Dictionary(Of String, Control)
    Private _currentKey As String = ""
    Private _lastVn As String = ""
    Private _noPatient As EmptyState
    Private _navs As Dictionary(Of NavButton, String)

    Private ReadOnly Property Monitor As PatientMonitor
        Get
            Return AppSession.Monitor
        End Get
    End Property

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        Icon = AppSession.AppIcon

        _navs = New Dictionary(Of NavButton, String) From {
            {navMedical, "medical"}, {navCvd, "cvd"}, {navLab, "lab"},
            {navCrossTab, "crosstab"}, {navLabHd, "labhd"}, {navHdCapd, "hdcapd"},
            {navDrug, "drug"}, {navSettings, "settings"}}
        For Each nav In _navs.Keys
            AddHandler nav.Click, AddressOf OnNavClick
        Next

        toolTip.SetToolTip(btnRefresh, "ตรวจสอบและโหลดข้อมูลผู้ป่วยใหม่ (F5)")

        _noPatient = New EmptyState() With {
            .Dock = DockStyle.Fill,
            .BackColor = Theme.AppBackground,
            .IconKind = IconKind.Bell,
            .Title = "ระบบรอเรียกผู้ป่วย",
            .Description = $"ข้อมูลจะแสดงอัตโนมัติเมื่อมีการเรียกผู้ป่วย (ล็อก VN) ที่เครื่อง {AppSession.ComputerName}",
            .Visible = False}
        pnlContent.Controls.Add(_noPatient)

        AddHandler Monitor.PatientChanged, AddressOf OnPatientChanged
        AddHandler Monitor.StateChanged, AddressOf OnMonitorStateChanged
        AddHandler Monitor.Countdown, AddressOf OnCountdown
        AddHandler Monitor.ModeChanged, AddressOf OnModeChanged
        AddHandler AppSession.SettingsChanged, AddressOf OnSettingsChanged

        toolTip.SetToolTip(segMode, "รอเรียกผู้ป่วยอัตโนมัติ = ตามการเรียกผู้ป่วย (ล็อก VN) ที่เครื่องนี้" & Environment.NewLine &
                                    "ค้นหาผู้ป่วยเอง = เลือกผู้ป่วยคนไหนก็ได้ ไม่ต้องมารับบริการวันนี้")

        UpdateTitle()
        UpdateNavVisibility()
        UpdateModeBar()
        ShowPatient(Monitor.Current)
        OnMonitorStateChanged(Monitor, EventArgs.Empty)
        OnCountdown(Monitor, EventArgs.Empty)
        _lastVn = If(Monitor.Current?.Vn, "")
        Navigate("medical")
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        NativeMethods.TrySetCaption(Me, Theme.Surface, Theme.TextPrimary, Theme.Border)
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        RemoveHandler Monitor.PatientChanged, AddressOf OnPatientChanged
        RemoveHandler Monitor.StateChanged, AddressOf OnMonitorStateChanged
        RemoveHandler Monitor.Countdown, AddressOf OnCountdown
        RemoveHandler Monitor.ModeChanged, AddressOf OnModeChanged
        RemoveHandler AppSession.SettingsChanged, AddressOf OnSettingsChanged
        ' ปิดหน้าต่างนี้เมื่อไหร่ ให้กลับสู่โหมดรอเรียกผู้ป่วยอัตโนมัติเสมอ
        Monitor.SwitchToAuto()
        MyBase.OnFormClosed(e)
    End Sub

    Private Sub UpdateTitle()
        ' เวอร์ชัน + ผู้พัฒนาแสดงเสมอ · ชื่อเซิร์ฟเวอร์/ฐานข้อมูลแสดงเฉพาะหลังล็อกอินผู้ดูแลระบบ
        Dim conn = AppSession.ConnectionTitle
        Text = $"SCPH Notify 2026  {AppSession.VersionLine}" & If(conn = "", "", "  " & conn)
        lblConnDetail.Text = AppSession.ConnectionDetail
        toolTip.SetToolTip(lblConnDetail, AppSession.ConnectionDetail)
    End Sub

    '──────────────── โหมดทำงาน (อัตโนมัติ / ค้นหาเอง) ────────────────

    Private _suspendMode As Boolean

    Private Sub UpdateModeBar()
        Dim manual = Monitor.Mode = MonitorMode.Manual
        _suspendMode = True
        Try
            segMode.SelectedIndex = If(manual, 1, 0)
        Finally
            _suspendMode = False
        End Try
        btnSearchPatient.Visible = manual
        btnClearManual.Visible = manual AndAlso Monitor.Current IsNot Nothing
        ringRefresh.Visible = Not manual
        lblRefreshInfo.Visible = Not manual

        If manual Then
            lblModeHint.Text = If(Monitor.Current Is Nothing,
                                  "กด ""ค้นหาผู้ป่วย"" เพื่อเลือกผู้ป่วยที่ต้องการดู",
                                  "ไม่ตรวจสอบการเรียกผู้ป่วยอัตโนมัติขณะอยู่ในโหมดนี้")
        Else
            lblModeHint.Text = $"ข้อมูลจะแสดงอัตโนมัติเมื่อมีการเรียกผู้ป่วยที่เครื่อง {AppSession.ComputerName}"
        End If
    End Sub

    Private Sub OnModeChanged(sender As Object, e As EventArgs)
        UpdateModeBar()
        UpdateEmptyState()
        Navigate(If(_currentKey = "", "medical", _currentKey))
    End Sub

    Private Async Sub segMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles segMode.SelectedIndexChanged
        If _suspendMode Then Return
        If segMode.SelectedIndex = 1 Then
            ' เข้าโหมดค้นหาเอง — เปิดหน้าค้นหาให้เลย
            If Monitor.Mode <> MonitorMode.Manual Then Await PickPatientAsync(enterManualIfCancelled:=True)
        Else
            Monitor.SwitchToAuto()
            Await Monitor.RefreshAsync(True)
        End If
    End Sub

    Private Async Sub btnSearchPatient_Click(sender As Object, e As EventArgs) Handles btnSearchPatient.Click
        Await PickPatientAsync(enterManualIfCancelled:=True)
    End Sub

    Private Sub btnClearManual_Click(sender As Object, e As EventArgs) Handles btnClearManual.Click
        Monitor.ClearManualPatient()
        UpdateModeBar()
    End Sub

    ''' <summary>เปิดหน้าค้นหาผู้ป่วย แล้วแสดงผู้ป่วยที่เลือกในโหมด Manual</summary>
    Private Async Function PickPatientAsync(enterManualIfCancelled As Boolean) As Task
        Dim hn = PatientSearchDialog.Pick(Me)
        If hn = "" Then
            ' ยกเลิก — ถ้ายังไม่เคยเข้าโหมดค้นหาเอง ให้กลับไปโหมดอัตโนมัติ
            If Not enterManualIfCancelled OrElse Monitor.Mode <> MonitorMode.Manual Then
                _suspendMode = True
                Try
                    segMode.SelectedIndex = If(Monitor.Mode = MonitorMode.Manual, 1, 0)
                Finally
                    _suspendMode = False
                End Try
            End If
            UpdateModeBar()
            Return
        End If
        Try
            Await Monitor.ShowManualPatientAsync(hn)
        Catch ex As Exception
            ErrorDialog.ShowError(Me, "โหลดข้อมูลผู้ป่วยไม่สำเร็จ", ex)
        End Try
        UpdateModeBar()
    End Function

    '──────────────── Navigation ────────────────

    Private Sub OnNavClick(sender As Object, e As EventArgs)
        Dim nav = DirectCast(sender, NavButton)
        Navigate(_navs(nav))
    End Sub

    ''' <summary>หน้าที่ต้องมีผู้ป่วยถูกเรียกอยู่จึงจะแสดงข้อมูลได้</summary>
    Private Shared Function RequiresPatient(key As String) As Boolean
        Select Case key
            Case "settings", "drug", "occupational" : Return False
            Case Else : Return True
        End Select
    End Function

    ''' <summary>
    ''' ทะเบียนโรคจากการทำงานและ PM2.5 ย้ายไปอยู่ในเมนูของแถบแจ้งเตือน (คลิกขวา / ปุ่ม ⋮)
    ''' ที่นี่เหลือแค่พาออกจากหน้านั้นเมื่อผู้ใช้ปิดระบบคัดกรองในหน้าตั้งค่า
    ''' </summary>
    Private Sub UpdateNavVisibility()
        If Not AppSession.Display.EnableOccupational AndAlso _currentKey = "occupational" Then Navigate("medical")
    End Sub

    Private Function GetPage(key As String) As Control
        Dim page As Control = Nothing
        If _pages.TryGetValue(key, page) Then Return page
        Select Case key
            Case "medical" : page = New MedicalCarePage()
            Case "cvd" : page = New CvdRiskPage()
            Case "lab" : page = New LabPage()
            Case "crosstab" : page = New LabCrossTabPage()
            Case "labhd" : page = New LabHemodialysisPage()
            Case "hdcapd" : page = New HdCapdCarePage()
            Case "occupational" : page = New OccupationalRegistryPage()
            Case "drug" : page = New DrugPage()
            Case "settings" : page = New SettingsPage()
            Case Else : Throw New ArgumentException(key)
        End Select
        page.Dock = DockStyle.Fill
        page.Visible = False
        pnlContent.Controls.Add(page)
        _pages(key) = page
        Return page
    End Function

    Public Sub Navigate(key As String)
        _currentKey = key
        For Each kv In _navs
            kv.Key.Selected = (kv.Value = key)
        Next

        SuspendLayout()
        Try
            Dim snap = Monitor.Current
            Dim showEmpty = RequiresPatient(key) AndAlso snap Is Nothing
            Dim target As Control = Nothing
            If Not showEmpty Then target = GetPage(key)

            For Each p In _pages.Values
                If p IsNot target Then p.Visible = False
            Next
            _noPatient.Visible = showEmpty
            If showEmpty Then
                _noPatient.BringToFront()
            Else
                Dim pp = TryCast(target, IPatientPage)
                If pp IsNot Nothing Then pp.BindPatient(snap)
                target.Visible = True
                target.BringToFront()
            End If
        Finally
            ResumeLayout(True)
        End Try
    End Sub

    '──────────────── Monitor events ────────────────

    Private Sub OnPatientChanged(sender As Object, e As PatientChangedEventArgs)
        ShowPatient(e.Snapshot)
        UpdateModeBar()
        Dim vn = If(e.Snapshot?.Vn, "")
        If vn <> _lastVn Then
            _lastVn = vn
            ' เหมือนระบบเดิม: เมื่อเปลี่ยนผู้ป่วยให้กลับไปหน้า Medical Care
            ' หน้าที่ไม่ผูกกับผู้ป่วย (ตั้งค่า / ทะเบียนคัดกรอง / Drug) ให้อยู่หน้าเดิม
            If RequiresPatient(_currentKey) Then Navigate("medical") Else Navigate(_currentKey)
        Else
            Navigate(_currentKey)
        End If
    End Sub

    Private Sub ShowPatient(snap As PatientSnapshot)
        flpAlerts.SuspendLayout()
        For Each c As Control In flpAlerts.Controls.Cast(Of Control)().ToList()
            c.Dispose()
        Next
        flpAlerts.Controls.Clear()

        If snap Is Nothing Then
            avatar.Active = False
            If Monitor.Mode = MonitorMode.Manual Then
                lblPatientName.Text = "ยังไม่ได้เลือกผู้ป่วย"
                lblPatientMeta.Text = "กดปุ่ม ""ค้นหาผู้ป่วย"" เพื่อค้นด้วยชื่อ ชื่อ-สกุล HN หรือเลขบัตรประชาชน"
            Else
                lblPatientName.Text = "ระบบรอเรียกผู้ป่วย"
                lblPatientMeta.Text = $"ยังไม่มีผู้ป่วยถูกเรียกที่เครื่อง {AppSession.ComputerName}"
            End If
            pnlAlertStrip.Visible = False
        Else
            avatar.Active = True
            lblPatientName.Text = snap.PatientName
            Dim meta As New List(Of String) From {$"HN {snap.Hn}"}
            If snap.Vn <> "" Then meta.Add($"VN {snap.Vn}")
            If snap.AgeYears.HasValue Then meta.Add(snap.AgeText)
            If snap.IsManual Then meta.Add("โหมดค้นหาเอง")
            lblPatientMeta.Text = String.Join("   ·   ", meta)

            ' โหมดค้นหาเอง: เตือนเมื่อผู้ป่วยไม่ได้มารับบริการวันนี้
            If snap.IsManual AndAlso Not snap.HasVisitToday Then
                Dim last = If(snap.LastVisitDate.HasValue,
                              $"  (มาล่าสุด {ThaiDate.MediumDate(snap.LastVisitDate.Value)})",
                              "  (ไม่เคยมารับบริการ)")
                Dim warnChip As New StatusChip("ผู้ป่วยไม่ได้มารับบริการในวันนี้" & last, AlertLevel.Danger, IconKind.Warning) With {
                    .Margin = New Padding(0, 0, 8, 0)}
                flpAlerts.Controls.Add(warnChip)
            End If

            Dim alerts = snap.GetAlerts(AppSession.Display)
            For Each a In alerts
                Dim chip As New StatusChip(a.Text, a.Level, a.Icon) With {.Margin = New Padding(0, 0, 8, 0)}
                flpAlerts.Controls.Add(chip)
            Next
            If snap.LoadWarnings.Count > 0 Then
                Dim w As New StatusChip($"โหลดข้อมูลไม่ครบ ({snap.LoadWarnings.Count})", AlertLevel.Neutral, IconKind.Info) With {.Margin = New Padding(0, 0, 8, 0)}
                toolTip.SetToolTip(w, String.Join(Environment.NewLine, snap.LoadWarnings))
                flpAlerts.Controls.Add(w)
            End If
            pnlAlertStrip.Visible = flpAlerts.Controls.Count > 0
        End If
        flpAlerts.ResumeLayout()
        lblUpdated.Text = $"อัปเดต {ThaiDate.TimeText(DateTime.Now)}"
        pnlHeader.Invalidate()
    End Sub

    Private Sub OnMonitorStateChanged(sender As Object, e As EventArgs)
        ringRefresh.Busy = Monitor.State = MonitorState.Loading OrElse Monitor.State = MonitorState.Starting
        Select Case Monitor.State
            Case MonitorState.Ready, MonitorState.Waiting
                dotConn.Level = AlertLevel.Success
                lblConnStatus.Text = If(AppSession.IsDemo, "โหมดสาธิต", "เชื่อมต่อแล้ว")
                toolTip.SetToolTip(lblConnStatus, "")
            Case MonitorState.Loading
                dotConn.Level = AlertLevel.Info
                lblConnStatus.Text = "กำลังโหลดข้อมูล..."
            Case MonitorState.Error
                dotConn.Level = AlertLevel.Danger
                lblConnStatus.Text = "เชื่อมต่อฐานข้อมูลไม่ได้"
                toolTip.SetToolTip(lblConnStatus, Monitor.LastError)
            Case Else
                dotConn.Level = AlertLevel.Neutral
                lblConnStatus.Text = "กำลังเชื่อมต่อ..."
        End Select
        btnRefresh.Enabled = Monitor.State <> MonitorState.Loading
        UpdateEmptyState()
    End Sub

    ''' <summary>ข้อความกลางจอเมื่อไม่มีผู้ป่วย / เชื่อมต่อฐานข้อมูลไม่ได้</summary>
    Private Sub UpdateEmptyState()
        If _noPatient Is Nothing Then Return
        If Monitor.State = MonitorState.Error Then
            _noPatient.IconKind = IconKind.Database
            _noPatient.Level = AlertLevel.Danger
            _noPatient.Title = "เชื่อมต่อฐานข้อมูลไม่ได้"
            _noPatient.Description = $"{Monitor.LastError}{Environment.NewLine}{Environment.NewLine}โปรแกรมจะลองเชื่อมต่อใหม่อัตโนมัติ · ตรวจสอบค่าได้ที่เมนู ตั้งค่า หรือกด Ctrl+F6"
        ElseIf Monitor.Mode = MonitorMode.Manual Then
            _noPatient.IconKind = IconKind.Search
            _noPatient.Level = AlertLevel.Primary
            _noPatient.Title = "ยังไม่ได้เลือกผู้ป่วย"
            _noPatient.Description = "กดปุ่ม ""ค้นหาผู้ป่วย"" ด้านบน แล้วค้นด้วยชื่อ ชื่อ-สกุล HN หรือเลขบัตรประชาชน"
        Else
            _noPatient.IconKind = IconKind.Bell
            _noPatient.Level = AlertLevel.Primary
            _noPatient.Title = "ระบบรอเรียกผู้ป่วย"
            _noPatient.Description = $"ข้อมูลจะแสดงอัตโนมัติเมื่อมีการเรียกผู้ป่วย (ล็อก VN) ที่เครื่อง {AppSession.ComputerName}"
        End If
    End Sub

    Private Sub OnCountdown(sender As Object, e As EventArgs)
        ringRefresh.Maximum = Monitor.IntervalSeconds
        ringRefresh.Value = Monitor.SecondsRemaining
        lblRefreshInfo.Text = $"ตรวจสอบทุก {Monitor.IntervalSeconds} วินาที"
    End Sub

    Private Sub OnSettingsChanged(sender As Object, e As EventArgs)
        UpdateTitle()
        UpdateNavVisibility()
        UpdateModeBar()
        ShowPatient(Monitor.Current)
        ' ให้หน้าที่สร้างไว้แล้วแสดงผลตามการตั้งค่าใหม่
        For Each p In _pages.Values.OfType(Of IPatientPage)()
            p.ResetBinding()
        Next
        Navigate(_currentKey)
    End Sub

    '──────────────── Actions ────────────────

    Private Async Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        btnRefresh.Enabled = False
        Try
            For Each p In _pages.Values.OfType(Of IPatientPage)()
                p.ResetBinding()
            Next
            Await Monitor.RefreshAsync(True)
        Finally
            btnRefresh.Enabled = True
        End Try
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = (Keys.Control Or Keys.F6) Then
            DbConfigDialog.ShowConfig(Me)
            Return True
        End If
        If keyData = Keys.F5 Then
            btnRefresh.PerformClick()
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Private Sub pnlHeader_Paint(sender As Object, e As PaintEventArgs) Handles pnlHeader.Paint
        If Not pnlAlertStrip.Visible Then
            Using p As New Pen(Theme.Border)
                e.Graphics.DrawLine(p, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1)
            End Using
        End If
    End Sub

    Private Sub pnlAlertStrip_Paint(sender As Object, e As PaintEventArgs) Handles pnlAlertStrip.Paint
        Using p As New Pen(Theme.Border)
            e.Graphics.DrawLine(p, 0, pnlAlertStrip.Height - 1, pnlAlertStrip.Width, pnlAlertStrip.Height - 1)
        End Using
    End Sub

    Private Sub pnlAlertStrip_VisibleChanged(sender As Object, e As EventArgs) Handles pnlAlertStrip.VisibleChanged
        pnlHeader.Invalidate()
    End Sub

End Class

''' <summary>หน้าที่แสดงข้อมูลของผู้ป่วย</summary>
Public Interface IPatientPage
    Sub BindPatient(snapshot As PatientSnapshot)
    ''' <summary>บังคับให้ BindPatient ครั้งถัดไปโหลดใหม่</summary>
    Sub ResetBinding()
End Interface

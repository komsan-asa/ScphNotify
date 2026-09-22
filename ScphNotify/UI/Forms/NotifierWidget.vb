''' <summary>
''' แถบแจ้งเตือนลอยอยู่บนสุด (แทน dialogMain เดิม)
''' - แสดงผู้ป่วยที่ถูกเรียกที่เครื่องนี้ + ป้ายแจ้งเตือนสำคัญ
''' - คลิก = เปิดหน้าต่างหลัก, ลากเพื่อย้ายตำแหน่ง, คลิกขวา = เมนู, ปิด = ซ่อนไปที่ถาดระบบ
''' </summary>
Public Class NotifierWidget

    Private _allowClose As Boolean
    Private _shell As MainShell
    Private _dragOrigin As Point
    Private _formOrigin As Point
    Private _mouseDown As Boolean
    Private _dragged As Boolean
    Private _flash As Integer
    Private _occupationalOpen As Boolean
    Private _balloonShown As Boolean
    Private _lastVn As String = Nothing

    ''' <summary>รัศมีมุม = ค่าเดียวกับมุมหน้าต่าง Windows 11 (8px) เพื่อให้เส้นขอบที่วาดเองตรงกับมุมที่ DWM ตัด</summary>
    Private Const WidgetRadius As Single = 8.0F
    Private Const CS_DROPSHADOW As Integer = &H20000
    Private Const WS_EX_TOOLWINDOW As Integer = &H80

    Private ReadOnly Property Monitor As PatientMonitor
        Get
            Return AppSession.Monitor
        End Get
    End Property

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp = MyBase.CreateParams
            cp.ClassStyle = cp.ClassStyle Or CS_DROPSHADOW
            cp.ExStyle = cp.ExStyle Or WS_EX_TOOLWINDOW   ' ไม่แสดงใน Alt+Tab
            Return cp
        End Get
    End Property

    '──────────────── Startup ────────────────

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        Icon = AppSession.AppIcon
        trayIcon.Icon = AppSession.AppIcon
        trayIcon.Text = If(AppSession.IsDemo, "SCPH Notify (โหมดสาธิต)", "SCPH Notify")
        menuMain.Renderer = New ModernMenuRenderer()
        TopMost = AppConfig.Current.WidgetTopMost
        miTopMost.Checked = TopMost

        miOpenMain.Image = IconPainter.ToBitmap(IconKind.External, 16, Theme.TextSecondary)
        miOccupationalRegistry.Image = IconPainter.ToBitmap(IconKind.Briefcase, 16, Theme.TextSecondary)
        miDbConfig.Image = IconPainter.ToBitmap(IconKind.Database, 16, Theme.TextSecondary)
        miInterval.Image = IconPainter.ToBitmap(IconKind.Clock, 16, Theme.TextSecondary)
        miExit.Image = IconPainter.ToBitmap(IconKind.Power, 16, Theme.Danger)
        For Each sec In {3, 5, 10, 15, 30, 60}
            Dim item As New ToolStripMenuItem($"{sec} วินาที") With {.Tag = sec}
            AddHandler item.Click, AddressOf OnIntervalItemClick
            miInterval.DropDownItems.Add(item)
        Next

        tip.SetToolTip(btnOpen, "เปิดหน้าต่างหลัก")
        tip.SetToolTip(btnMenu, "เมนู")
        tip.SetToolTip(ring, "วินาทีที่เหลือก่อนตรวจสอบผู้ป่วยรอบถัดไป")

        For Each c In New Control() {Me, avatar, lblName, lblMeta, flpChips, dotStatus, ring}
            AddHandler c.MouseDown, AddressOf OnDragMouseDown
            AddHandler c.MouseMove, AddressOf OnDragMouseMove
            AddHandler c.MouseUp, AddressOf OnDragMouseUp
            AddHandler c.DoubleClick, Sub(s, ev) ShowMainWindow()
            c.Cursor = Cursors.Hand
        Next

        AddHandler Monitor.PatientChanged, AddressOf OnPatientChanged
        AddHandler Monitor.StateChanged, AddressOf OnMonitorStateChanged
        AddHandler Monitor.Countdown, AddressOf OnCountdown
        AddHandler Monitor.OccupationalScreeningRequired, AddressOf OnOccupationalRequired
        AddHandler AppSession.SettingsChanged, AddressOf OnSettingsChanged

        UpdatePatient(Nothing)
        OnMonitorStateChanged(Monitor, EventArgs.Empty)
        LayoutBar()
        PlaceOnScreen()
        Monitor.Start()
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        If Not NativeMethods.TrySetCorners(Me, NativeMethods.CornerPreference.Round) Then UpdateRegion()
    End Sub

    Private _useRegion As Boolean

    Private Sub UpdateRegion()
        _useRegion = True
        Using path = Gfx.RoundedRect(New RectangleF(0, 0, Width, Height), Theme.ScaleF(Me, WidgetRadius))
            Region = New Region(path)
        End Using
    End Sub

    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        If _useRegion Then UpdateRegion()
        Invalidate()
    End Sub

    Private Sub PlaceOnScreen()
        Dim wa = Screen.PrimaryScreen.WorkingArea
        Dim cfg = AppConfig.Current
        Dim p As Point
        If cfg.WidgetLeft.HasValue AndAlso cfg.WidgetTop.HasValue Then
            p = New Point(cfg.WidgetLeft.Value, cfg.WidgetTop.Value)
        Else
            p = New Point(wa.Right - Width - Theme.Scale(Me, 24), wa.Top + Theme.Scale(Me, 24))
        End If
        ' ให้อยู่ในจอเสมอ
        Dim onScreen = Screen.AllScreens.Any(Function(s) s.WorkingArea.IntersectsWith(New Rectangle(p, Size)))
        If Not onScreen Then p = New Point(wa.Right - Width - 24, wa.Top + 24)
        Location = p
    End Sub

    '──────────────── Layout & paint ────────────────

    Private Sub LayoutBar()
        SuspendLayout()
        Dim h = Theme.Scale(Me, 52)
        Dim gap = Theme.Scale(Me, 8)
        Dim x = Theme.Scale(Me, 10)
        dotStatus.Location = New Point(x, (h - dotStatus.Height) \ 2)
        x = dotStatus.Right + Theme.Scale(Me, 7)
        avatar.Location = New Point(x, (h - avatar.Height) \ 2)
        x = avatar.Right + gap

        Dim maxText = Theme.Scale(Me, 300)
        Dim textW = Math.Min(maxText, Math.Max(lblName.PreferredWidth, lblMeta.PreferredWidth))
        lblName.AutoSize = lblName.PreferredWidth <= maxText
        If Not lblName.AutoSize Then lblName.Size = New Size(maxText, lblName.PreferredHeight) : lblName.AutoEllipsis = True
        lblMeta.AutoSize = lblMeta.PreferredWidth <= maxText
        If Not lblMeta.AutoSize Then lblMeta.Size = New Size(maxText, lblMeta.PreferredHeight) : lblMeta.AutoEllipsis = True
        Dim textH = lblName.Height + lblMeta.Height
        lblName.Location = New Point(x, (h - textH) \ 2 - 1)
        lblMeta.Location = New Point(x + 1, lblName.Bottom)
        x += textW + gap

        If flpChips.Controls.Count > 0 Then
            ' คำนวณขนาดเอง (FlowLayoutPanel.AutoSize จะยังไม่อัปเดตระหว่าง SuspendLayout)
            Dim chips = flpChips.Controls.Cast(Of Control)().ToList()
            Dim chipsW = chips.Sum(Function(c) c.GetPreferredSize(Size.Empty).Width + c.Margin.Horizontal)
            Dim chipsH = chips.Max(Function(c) c.GetPreferredSize(Size.Empty).Height)
            flpChips.Visible = True
            flpChips.Size = New Size(chipsW, chipsH)
            flpChips.Location = New Point(x, (h - chipsH) \ 2)
            x = flpChips.Right + gap
        Else
            flpChips.Visible = False
        End If

        ring.Location = New Point(x, (h - ring.Height) \ 2)
        x = ring.Right + Theme.Scale(Me, 4)
        btnOpen.Location = New Point(x, (h - btnOpen.Height) \ 2)
        x = btnOpen.Right + Theme.Scale(Me, 1)
        btnMenu.Location = New Point(x, (h - btnMenu.Height) \ 2)
        ' ระยะขอบขวา = เท่ากับระยะขอบซ้าย เพื่อไม่ให้เหลือพื้นที่ว่างก่อนมุมโค้ง
        x = btnMenu.Right + Theme.Scale(Me, 8)

        ' ให้กรอบหดตามเนื้อหาจริง (ขั้นต่ำแค่พอไม่ให้แถบสั้นจนดูแปลกตอนไม่มีผู้ป่วย)
        Dim newWidth = Math.Max(Theme.Scale(Me, 250), x)
        If newWidth <> Width OrElse h <> Height Then
            ' ถ้าแถบอยู่ครึ่งขวาของจอ ให้ยึดขอบขวาไว้ (ขยายไปทางซ้าย)
            Dim anchorRight = IsHandleCreated AndAlso (Left + Width / 2) > Screen.FromControl(Me).WorkingArea.Left + Screen.FromControl(Me).WorkingArea.Width / 2
            Dim right = Me.Right
            Size = New Size(newWidth, h)
            If anchorRight Then Left = right - newWidth
        End If
        ResumeLayout(False)
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        Gfx.HighQuality(g)
        g.Clear(Theme.Surface)
        Dim radius = Theme.ScaleF(Me, WidgetRadius)
        Dim r As New RectangleF(0.5F, 0.5F, Width - 1.5F, Height - 1.5F)
        If _flash > 0 Then
            Dim t = CSng(Math.Abs(Math.Sin(_flash / 6.0)))
            Gfx.DrawRounded(g, r, radius, Theme.Blend(Theme.Border, Theme.Primary, t), 2.5F)
        Else
            Gfx.DrawRounded(g, r, radius, Theme.BorderStrong)
        End If
        ' แถบสีบาง ๆ ด้านซ้ายตามสถานะ
        Dim accent = StateAccent()
        Using b As New SolidBrush(accent)
            g.FillRectangle(b, 0, Theme.Scale(Me, 12), Theme.Scale(Me, 3), Height - Theme.Scale(Me, 24))
        End Using
    End Sub

    Private Function StateAccent() As Color
        If Monitor Is Nothing Then Return Theme.BorderStrong
        Select Case Monitor.State
            Case MonitorState.Error : Return Theme.Danger
            Case MonitorState.Ready : Return Theme.Primary
            Case MonitorState.Loading : Return Theme.Info
            Case Else : Return Theme.BorderStrong
        End Select
    End Function

    Private Sub flashTimer_Tick(sender As Object, e As EventArgs) Handles flashTimer.Tick
        _flash -= 1
        If _flash <= 0 Then flashTimer.Stop()
        Invalidate()
    End Sub

    '──────────────── Monitor events ────────────────

    Private Sub OnPatientChanged(sender As Object, e As PatientChangedEventArgs)
        UpdatePatient(e.Snapshot)
        Dim vn = If(e.Snapshot?.Vn, "")
        If _lastVn IsNot Nothing AndAlso vn <> "" AndAlso vn <> _lastVn Then
            _flash = 60
            flashTimer.Start()
        End If
        _lastVn = vn
    End Sub

    Private Sub UpdatePatient(snap As PatientSnapshot)
        For Each c As Control In flpChips.Controls.Cast(Of Control)().ToList()
            c.Dispose()
        Next
        flpChips.Controls.Clear()

        If snap Is Nothing Then
            avatar.Active = False
            lblName.Text = If(Monitor.Mode = MonitorMode.Manual, "ยังไม่ได้เลือกผู้ป่วย", "ระบบรอเรียกผู้ป่วย")
            lblMeta.Text = If(Monitor.State = MonitorState.Error, "เชื่อมต่อฐานข้อมูลไม่ได้ — คลิกขวาเพื่อตั้งค่า",
                              If(Monitor.Mode = MonitorMode.Manual, "โหมดค้นหาเอง — คลิกเพื่อเปิดหน้าต่างหลัก",
                                 $"เครื่อง {AppSession.ComputerName}{If(AppSession.IsDemo, " · โหมดสาธิต", "")}"))
            lblMeta.ForeColor = If(Monitor.State = MonitorState.Error, Theme.DangerText, Theme.TextSecondary)
            tip.SetToolTip(lblName, "")
        Else
            avatar.Active = True
            lblName.Text = snap.PatientName
            lblMeta.Text = $"HN {snap.Hn}{If(snap.AgeYears.HasValue, "  ·  " & snap.AgeText, "")}{If(snap.IsManual, "  ·  ค้นหาเอง", "")}"
            lblMeta.ForeColor = Theme.TextSecondary
            tip.SetToolTip(lblName, snap.ClassicTitle)

            ' หมายเหตุ: ป้ายเตือน "ไม่ได้มารับบริการวันนี้" แสดงเฉพาะในหน้าต่างหลัก ไม่แสดงบนแถบลอย
            Dim alerts = snap.GetAlerts(AppSession.Display)
            Const maxChips As Integer = 2
            For Each a In alerts.Take(maxChips)
                flpChips.Controls.Add(New StatusChip(a.Text, a.Level, a.Icon) With {.Compact = True, .Margin = New Padding(0, 0, 6, 0)})
            Next
            If alerts.Count > maxChips Then
                Dim more As New StatusChip($"+{alerts.Count - maxChips}", AlertLevel.Neutral) With {.Compact = True, .Margin = New Padding(0)}
                tip.SetToolTip(more, String.Join(Environment.NewLine, alerts.Skip(maxChips).Select(Function(a) a.Text)))
                flpChips.Controls.Add(more)
            End If
            For Each c As Control In flpChips.Controls
                AddHandler c.Click, Sub(s, ev) ShowMainWindow()
                c.Cursor = Cursors.Hand
            Next
        End If
        LayoutBar()
    End Sub

    Private Sub OnMonitorStateChanged(sender As Object, e As EventArgs)
        ring.Busy = Monitor.State = MonitorState.Loading OrElse Monitor.State = MonitorState.Starting
        Select Case Monitor.State
            Case MonitorState.Ready, MonitorState.Waiting
                dotStatus.Level = AlertLevel.Success
                tip.SetToolTip(dotStatus, If(AppSession.IsDemo, "โหมดสาธิต", "เชื่อมต่อฐานข้อมูลแล้ว: " & AppSession.ConnectionDetail))
            Case MonitorState.Loading
                dotStatus.Level = AlertLevel.Info
                tip.SetToolTip(dotStatus, "กำลังโหลดข้อมูลผู้ป่วย...")
            Case MonitorState.Error
                dotStatus.Level = AlertLevel.Danger
                tip.SetToolTip(dotStatus, "เชื่อมต่อฐานข้อมูลไม่ได้" & Environment.NewLine & Monitor.LastError)
            Case Else
                dotStatus.Level = AlertLevel.Neutral
        End Select
        If Monitor.Current Is Nothing Then UpdatePatient(Nothing)
        Invalidate()
    End Sub

    Private Sub OnCountdown(sender As Object, e As EventArgs)
        ring.Maximum = Monitor.IntervalSeconds
        ring.Value = Monitor.SecondsRemaining
    End Sub

    Private Sub OnOccupationalRequired(sender As Object, e As OccupationalEventArgs)
        If _occupationalOpen Then Return
        _occupationalOpen = True
        Try
            Using dlg As New OccupationalScreeningDialog(e.Target, Monitor.Current)
                dlg.ShowDialog(If(Visible, Me, Nothing))
            End Using
        Finally
            _occupationalOpen = False
        End Try
    End Sub

    '──────────────── Drag / click ────────────────

    Private Sub OnDragMouseDown(sender As Object, e As MouseEventArgs)
        If e.Button <> MouseButtons.Left Then Return
        _mouseDown = True
        _dragged = False
        _dragOrigin = Cursor.Position
        _formOrigin = Location
    End Sub

    Private Sub OnDragMouseMove(sender As Object, e As MouseEventArgs)
        If Not _mouseDown Then Return
        Dim dx = Cursor.Position.X - _dragOrigin.X
        Dim dy = Cursor.Position.Y - _dragOrigin.Y
        If Not _dragged AndAlso Math.Abs(dx) + Math.Abs(dy) < 5 Then Return
        _dragged = True
        Location = New Point(_formOrigin.X + dx, _formOrigin.Y + dy)
    End Sub

    Private Sub OnDragMouseUp(sender As Object, e As MouseEventArgs)
        If Not _mouseDown Then Return
        _mouseDown = False
        If _dragged Then
            AppConfig.Current.WidgetLeft = Left
            AppConfig.Current.WidgetTop = Top
            TrySaveConfig()
        ElseIf e.Button = MouseButtons.Left Then
            ShowMainWindow()   ' เดิม: คลิกที่ชื่อผู้ป่วยเพื่อเปิด frmMain
        End If
    End Sub

    Private Shared Sub TrySaveConfig()
        Try
            AppConfig.Save()
        Catch
        End Try
    End Sub

    '──────────────── Windows ────────────────

    Public Sub ShowMainWindow()
        If _shell Is Nothing OrElse _shell.IsDisposed Then _shell = New MainShell()
        If Not _shell.Visible Then _shell.Show()
        If _shell.WindowState = FormWindowState.Minimized Then _shell.WindowState = FormWindowState.Normal
        _shell.Activate()
        _shell.BringToFront()
    End Sub

    ''' <summary>เรียกจาก thread อื่น เมื่อผู้ใช้เปิดโปรแกรมซ้ำ</summary>
    Public Sub RequestShowMainWindow()
        If IsHandleCreated AndAlso Not IsDisposed Then
            BeginInvoke(New Action(Sub()
                                       ShowWidget()
                                       ShowMainWindow()
                                   End Sub))
        End If
    End Sub

    Private Sub ShowWidget()
        Show()
        WindowState = FormWindowState.Normal
        Activate()
    End Sub

    Private Sub HideToTray()
        Hide()
        trayIcon.Visible = True
        If Not _balloonShown Then
            _balloonShown = True
            trayIcon.BalloonTipIcon = ToolTipIcon.Info
            trayIcon.BalloonTipTitle = "SCPH Notify ยังทำงานอยู่"
            trayIcon.BalloonTipText = "คลิกที่ข้อความนี้ หรือดับเบิลคลิกที่ไอคอนเพื่อเปิดโปรแกรม"
            trayIcon.ShowBalloonTip(5000)
        End If
    End Sub

    Private Sub ExitApplication()
        _allowClose = True
        trayIcon.Visible = False
        Monitor.Stop()
        TrySaveConfig()
        Application.Exit()
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        If Not _allowClose AndAlso e.CloseReason = CloseReason.UserClosing Then
            e.Cancel = True
            HideToTray()
            Return
        End If
        trayIcon.Visible = False
        MyBase.OnFormClosing(e)
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = (Keys.Control Or Keys.F6) Then
            DbConfigDialog.ShowConfig(Me)
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    '──────────────── Menu ────────────────

    Private Sub menuMain_Opening(sender As Object, e As ComponentModel.CancelEventArgs) Handles menuMain.Opening
        miShowWidget.Visible = Not Visible
        miHideWidget.Visible = Visible
        ' ซ่อนเมนูทะเบียนเมื่อปิดระบบคัดกรองโรคจากการทำงานในหน้าตั้งค่า
        miOccupationalRegistry.Visible = AppSession.Display.EnableOccupational
        For Each item In miInterval.DropDownItems.OfType(Of ToolStripMenuItem)()
            item.Checked = CInt(item.Tag) = Monitor.IntervalSeconds
        Next
        miInterval.Text = $"ตรวจสอบผู้ป่วยทุก {Monitor.IntervalSeconds} วินาที"
    End Sub

    Private Sub btnMenu_Click(sender As Object, e As EventArgs) Handles btnMenu.Click
        menuMain.Show(btnMenu, New Point(0, btnMenu.Height + 2))
    End Sub

    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        ShowMainWindow()
    End Sub

    Private Sub miOpenMain_Click(sender As Object, e As EventArgs) Handles miOpenMain.Click
        ShowMainWindow()
    End Sub

    ''' <summary>เปิดหน้าต่างหลักแล้วไปที่หน้าทะเบียนโรคจากการทำงานและ PM2.5</summary>
    Private Sub miOccupationalRegistry_Click(sender As Object, e As EventArgs) Handles miOccupationalRegistry.Click
        ShowMainWindow()
        _shell?.Navigate("occupational")
    End Sub

    Private Sub miShowWidget_Click(sender As Object, e As EventArgs) Handles miShowWidget.Click
        ShowWidget()
    End Sub

    Private Sub miHideWidget_Click(sender As Object, e As EventArgs) Handles miHideWidget.Click
        HideToTray()
    End Sub

    Private Sub miTopMost_CheckedChanged(sender As Object, e As EventArgs) Handles miTopMost.CheckedChanged
        TopMost = miTopMost.Checked
        If AppConfig.Current.WidgetTopMost <> TopMost Then
            AppConfig.Current.WidgetTopMost = TopMost
            TrySaveConfig()
        End If
    End Sub

    Private Sub OnIntervalItemClick(sender As Object, e As EventArgs)
        Dim sec = CInt(DirectCast(sender, ToolStripMenuItem).Tag)
        Monitor.IntervalSeconds = sec
        AppConfig.Current.RefreshSeconds = sec
        TrySaveConfig()
        AppSession.NotifySettingsChanged()
    End Sub

    Private Sub miDbConfig_Click(sender As Object, e As EventArgs) Handles miDbConfig.Click
        DbConfigDialog.ShowConfig(If(Visible, Me, Nothing))
    End Sub

    Private Sub miExit_Click(sender As Object, e As EventArgs) Handles miExit.Click
        ExitApplication()
    End Sub

    Private Sub trayIcon_DoubleClick(sender As Object, e As EventArgs) Handles trayIcon.DoubleClick
        ShowWidget()
        ShowMainWindow()
    End Sub

    Private Sub trayIcon_BalloonTipClicked(sender As Object, e As EventArgs) Handles trayIcon.BalloonTipClicked
        ShowWidget()
    End Sub

    Private Sub OnSettingsChanged(sender As Object, e As EventArgs)
        miTopMost.Checked = AppConfig.Current.WidgetTopMost
        UpdatePatient(Monitor.Current)
    End Sub

End Class

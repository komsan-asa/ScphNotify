''' <summary>
''' หน้าตั้งค่า (แทน dialogSetting เดิม ซึ่งบันทึกได้เพียง cbDmColor และเมนูถูกปิดไว้)
''' ใช้ key เดิมทุกตัวในตาราง app_storage (section = scphNotify)
''' </summary>
Public Class SettingsPage
    Implements IPatientPage

    Private _toggles As Dictionary(Of String, ToggleSwitch)

    Public Sub New()
        InitializeComponent()
        _toggles = New Dictionary(Of String, ToggleSwitch) From {
            {"cbDmColor", tgDm}, {"cbHtColor", tgHt}, {"cbCaseHd", tgHd}, {"cbEgfr", tgEgfr},
            {"cbScreenEyeNepFoot", tgScreen}, {"cbNextOapp", tgAppt}, {"cbFerritin", tgFerritin},
            {"cbOccupational", tgOccupational}}
    End Sub

    Public Sub BindPatient(snapshot As PatientSnapshot) Implements IPatientPage.BindPatient
        LoadValues()
    End Sub

    Public Sub ResetBinding() Implements IPatientPage.ResetBinding
        LoadValues()
    End Sub

    Private Sub LoadValues()
        Dim prefs = AppSession.Display
        For Each kv In _toggles
            kv.Value.Checked = prefs.GetValue(kv.Key)
        Next
        Dim cfg = AppConfig.Current
        nudInterval.Value = Math.Max(nudInterval.Minimum, Math.Min(nudInterval.Maximum, CDec(cfg.RefreshSeconds)))
        nudIdle.Value = Math.Max(nudIdle.Minimum, Math.Min(nudIdle.Maximum, CDec(cfg.Security.AdminIdleLogoutMinutes)))
        tgTopMost.Checked = cfg.WidgetTopMost

        ' ปุ่มออกจากระบบมีความหมายเฉพาะตอนที่ล็อกอินผู้ดูแลอยู่
        btnLogout.Visible = AppSession.IsAdminSignedIn

        Dim db = cfg.Database
        If AppSession.IsDemo Then
            lblDbServer.Text = "โหมดสาธิต"
            lblDbName.Text = "ไม่ได้เชื่อมต่อฐานข้อมูลจริง (เปิดด้วย --demo)"
        ElseIf AppSession.IsAdminSignedIn Then
            lblDbServer.Text = $"{db.Server} : {db.Port}"
            lblDbName.Text = $"ฐานข้อมูล {db.Database}  ·  ผู้ใช้ {db.UserName}"
        Else
            ' ซ่อนข้อมูลเซิร์ฟเวอร์ไว้จนกว่าจะล็อกอินผู้ดูแลระบบ
            lblDbServer.Text = "เชื่อมต่อฐานข้อมูลโรงพยาบาลแล้ว"
            lblDbName.Text = "เข้าสู่ระบบผู้ดูแลเพื่อดูและแก้ไขค่าการเชื่อมต่อ"
        End If

        ' บอกเหตุผลเมื่อถูกให้ออกจากระบบเอง ไม่อย่างนั้นผู้ใช้จะงงว่าทำไมต้องล็อกอินใหม่
        If AppSession.SignedOutByIdle Then
            AppSession.SignedOutByIdle = False
            lblSaveStatus.ForeColor = Theme.WarningText
            lblSaveStatus.Text = $"ออกจากระบบผู้ดูแลอัตโนมัติ เพราะไม่มีการใช้งานเกิน {cfg.Security.AdminIdleLogoutMinutes} นาที"
        End If
        lblComputer.Text = $"ชื่อเครื่องนี้: {AppSession.ComputerName}  (ใช้ค้นหา vn_lock และ app_storage)"
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        btnSave.Enabled = False
        lblSaveStatus.ForeColor = Theme.TextSecondary
        lblSaveStatus.Text = "กำลังบันทึก..."
        Try
            ' ค่าที่เก็บในเครื่อง
            Dim cfg = AppConfig.Current
            cfg.RefreshSeconds = CInt(nudInterval.Value)
            cfg.Security.AdminIdleLogoutMinutes = CInt(nudIdle.Value)
            cfg.WidgetTopMost = tgTopMost.Checked
            AppSession.Monitor.IntervalSeconds = cfg.RefreshSeconds
            AppConfig.Save()

            ' ค่าที่เก็บในฐานข้อมูล (app_storage)
            Dim prefs As New DisplayPreferences()
            For Each kv In _toggles
                prefs.SetValue(kv.Key, kv.Value.Checked)
            Next
            Await AppSession.SaveDisplayPreferencesAsync(prefs)

            lblSaveStatus.ForeColor = Theme.Success
            lblSaveStatus.Text = $"บันทึกแล้ว เวลา {ThaiDate.TimeText(DateTime.Now)}"
        Catch ex As Exception
            lblSaveStatus.ForeColor = Theme.Danger
            lblSaveStatus.Text = "บันทึกการแสดงผลลงฐานข้อมูลไม่สำเร็จ (ค่าที่เก็บในเครื่องบันทึกแล้ว)"
            ErrorDialog.ShowError(FindForm(), "บันทึกการตั้งค่าไม่สำเร็จ", ex)
            AppSession.NotifySettingsChanged()
        Finally
            btnSave.Enabled = True
        End Try
    End Sub

    Private Sub btnDbConfig_Click(sender As Object, e As EventArgs) Handles btnDbConfig.Click
        DbConfigDialog.ShowConfig(FindForm())
        LoadValues()
    End Sub

    ''' <summary>
    ''' ออกจากระบบผู้ดูแลด้วยตนเอง — ข้อมูลเซิร์ฟเวอร์จะถูกซ่อนทันที
    ''' ไม่ต้องถามยืนยัน เพราะกดผิดก็แค่ล็อกอินใหม่ ไม่มีอะไรเสียหาย
    ''' </summary>
    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        AppSession.SignOutAdmin()
        LoadValues()
        lblSaveStatus.ForeColor = Theme.TextSecondary
        lblSaveStatus.Text = $"ออกจากระบบผู้ดูแลแล้ว เวลา {ThaiDate.TimeText(DateTime.Now)}"
    End Sub

End Class

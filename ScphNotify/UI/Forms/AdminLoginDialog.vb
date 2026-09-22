''' <summary>
''' เข้าสู่ระบบผู้ดูแล — ตรวจสอบกับบัญชีผู้ใช้ HOSxP (ตาราง opduser)
''' ใช้วิธีเดียวกับโปรแกรม Appoint2020: passweb = MD5(รหัสผ่าน) และสิทธิ์แอดมินดูจาก groupname
''' เป็นด่านก่อนเข้าหน้าตั้งค่าฐานข้อมูล และเป็นตัวปลดล็อกการแสดงชื่อเซิร์ฟเวอร์บนหัวหน้าต่าง
''' </summary>
Public Class AdminLoginDialog

    Private _busy As Boolean
    Private _hosxpUser As String = ""

    ''' <summary>เปิดหน้าล็อกอิน คืนค่า True เมื่อผ่านสิทธิ์ผู้ดูแลระบบ</summary>
    Public Shared Function SignIn(owner As IWin32Window) As Boolean
        Using dlg As New AdminLoginDialog()
            If owner Is Nothing Then
                dlg.StartPosition = FormStartPosition.CenterScreen
                dlg.TopMost = True
            End If
            dlg.ShowDialog(owner)
        End Using
        Return AppSession.IsAdminSignedIn
    End Function

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        Icon = AppSession.AppIcon
        lblVersion.Text = AppSession.VersionLine
        btnHosxp.Visible = False
        If AppSession.IsDemo Then
            banner.ShowMessage("โหมดสาธิต — ใช้ชื่อผู้ใช้ admin รหัสผ่าน admin", AlertLevel.Info)
        End If
        If AppConfig.Current.Security.AllowHosxpSessionLogin Then LookUpHosxpUserAsync()
        txtUser.Focus()
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        NativeMethods.TrySetCaption(Me, Theme.Surface, Theme.TextPrimary, Theme.Border)
    End Sub

    '──────────────── ใช้บัญชีที่เปิด HOSxP อยู่บนเครื่องนี้ (ต้องกดปุ่มเอง) ────────────────

    ''' <summary>อ่านชื่อผู้ใช้ที่กำลังเปิด HOSxP บนเครื่องนี้ (onlineuser) — เหมือน checkLoginOnComputer เดิม</summary>
    Private Async Sub LookUpHosxpUserAsync()
        Dim account As HosUser = Nothing
        Try
            _hosxpUser = Await AppSession.Repository.GetLoginOnComputerAsync(AppSession.ComputerName)
            If _hosxpUser <> "" Then account = Await AppSession.Repository.GetHosUserAsync(_hosxpUser)
        Catch
            _hosxpUser = ""
        End Try
        If IsDisposed OrElse _hosxpUser = "" Then Return

        ' แสดงเป็นปุ่มให้กดเอง ไม่เข้าสู่ระบบอัตโนมัติ
        Dim shown = If(account IsNot Nothing, account.DisplayName, _hosxpUser)
        btnHosxp.Text = $"เข้าด้วยบัญชี HOSxP บนเครื่องนี้ ({shown})"
        btnHosxp.Visible = True
        tip.SetToolTip(btnHosxp, $"บัญชี {_hosxpUser} กำลังเปิด HOSxP อยู่บนเครื่อง {AppSession.ComputerName}")
    End Sub

    Private Async Sub btnHosxp_Click(sender As Object, e As EventArgs) Handles btnHosxp.Click
        If _busy OrElse _hosxpUser = "" Then Return
        Await SignInAsync(Function() SignInByHosxpSessionAsync(_hosxpUser), _hosxpUser)
    End Sub

    Private Async Function SignInByHosxpSessionAsync(user As String) As Task(Of AuthResult)
        Dim account = Await AppSession.Repository.GetHosUserAsync(user)
        If account Is Nothing Then Return AuthResult.Fail(AuthStatus.UnknownUser)
        If Not account.IsAdmin Then Return AuthResult.Fail(AuthStatus.NotAdmin, account.GroupName, account)
        Return AuthResult.Ok(account)
    End Function

    '──────────────── เข้าสู่ระบบด้วยชื่อผู้ใช้ + รหัสผ่าน ────────────────

    Private Async Sub btnSignIn_Click(sender As Object, e As EventArgs) Handles btnSignIn.Click
        If _busy Then Return
        Dim user = txtUser.Text.Trim()
        Dim pass = txtPassword.Text
        If user = "" Then
            banner.ShowMessage("กรุณากรอกชื่อผู้ใช้", AlertLevel.Warning)
            txtUser.Focus()
            Return
        End If
        If pass = "" Then
            banner.ShowMessage("กรุณากรอกรหัสผ่าน", AlertLevel.Warning)
            txtPassword.Focus()
            Return
        End If
        Await SignInAsync(Function() AppSession.Repository.AuthenticateAsync(user, pass), user)
    End Sub

    Private Async Function SignInAsync(check As Func(Of Task(Of AuthResult)), user As String) As Task
        _busy = True
        btnSignIn.Enabled = False
        btnHosxp.Enabled = False
        UseWaitCursor = True
        banner.ShowMessage("กำลังตรวจสอบสิทธิ์...", AlertLevel.Info, IconKind.Clock)
        Try
            Dim result = Await check()
            If Not result.IsSuccess Then
                ShowFailure(result, user)
                Return
            End If
            AppSession.AdminUser = result.User
            AppSession.NotifySettingsChanged()   ' ปลดล็อกการแสดง [Connect ...] บนหัวหน้าต่าง
            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            banner.ShowMessage("ตรวจสอบสิทธิ์ไม่ได้: " & If(ex.InnerException IsNot Nothing, ex.InnerException.Message, ex.Message), AlertLevel.Danger)
        Finally
            _busy = False
            btnSignIn.Enabled = True
            btnHosxp.Enabled = True
            UseWaitCursor = False
        End Try
    End Function

    ''' <summary>บอกให้ชัดว่าติดขั้นไหน จะได้แก้ถูกจุด</summary>
    Private Sub ShowFailure(result As AuthResult, user As String)
        Dim sec = AppConfig.Current.Security
        Select Case result.Status
            Case AuthStatus.UnknownUser
                banner.ShowMessage($"ไม่พบชื่อผู้ใช้ ""{user}"" ในตาราง {sec.UserTable}", AlertLevel.Danger)
                txtUser.Focus()

            Case AuthStatus.PasswordNotSet
                ' บัญชีมีจริง แต่ HOSxP ยังไม่ได้ตั้งรหัสผ่านสำหรับโปรแกรมภายนอกให้บัญชีนี้
                banner.ShowMessage($"บัญชี {user} ยังไม่ได้ตั้งรหัสผ่านสำหรับโปรแกรมภายนอกใน HOSxP " &
                                   $"(คอลัมน์ {result.Detail} ว่าง) — ให้ตั้งรหัสผ่านใน HOSxP ก่อน หรือใช้ปุ่มเข้าด้วยบัญชี HOSxP บนเครื่องนี้",
                                   AlertLevel.Danger)
                txtPassword.Text = ""

            Case AuthStatus.WrongPassword
                banner.ShowMessage("รหัสผ่านไม่ถูกต้อง", AlertLevel.Danger)
                txtPassword.Text = ""
                txtPassword.Focus()

            Case AuthStatus.NotAdmin
                Dim grp = If(result.Detail = "", "(ไม่มีกลุ่ม)", result.Detail)
                banner.ShowMessage($"บัญชี {user} อยู่กลุ่ม {grp} จึงไม่มีสิทธิ์ผู้ดูแลระบบ", AlertLevel.Danger)
                txtPassword.Text = ""

            Case Else
                banner.ShowMessage("เข้าสู่ระบบไม่สำเร็จ", AlertLevel.Danger)
        End Select
    End Sub

End Class

''' <summary>ตั้งค่าการเชื่อมต่อฐานข้อมูล (แทน frmConfig / dialogConfig เดิม — เปิดด้วย Ctrl+F6)</summary>
Public Class DbConfigDialog

    ''' <summary>เปิดหน้าตั้งค่า คืนค่า True เมื่อบันทึกแล้ว — ต้องผ่านการล็อกอินผู้ดูแลระบบก่อน</summary>
    Public Shared Function ShowConfig(owner As IWin32Window) As Boolean
        If Not AdminGate.Require(owner) Then Return False
        Using dlg As New DbConfigDialog()
            If owner Is Nothing Then
                dlg.StartPosition = FormStartPosition.CenterScreen
                dlg.TopMost = True
            End If
            Return dlg.ShowDialog(owner) = DialogResult.OK
        End Using
    End Function

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        Icon = AppSession.AppIcon
        Dim db = AppConfig.Current.Database
        txtServer.Text = db.Server
        txtPort.Text = db.Port.ToString()
        txtDatabase.Text = db.Database
        txtUser.Text = db.UserName
        txtPassword.Text = db.GetPassword()
        If AppSession.IsDemo Then
            banner.ShowMessage("ขณะนี้เปิดในโหมดสาธิต — ค่าที่บันทึกจะใช้เมื่อเปิดโปรแกรมแบบปกติ", AlertLevel.Info)
        ElseIf AdminGate.RecoveryMode Then
            banner.ShowMessage("โหมดกู้คืน (--dbconfig) — เปิดโดยไม่ตรวจสิทธิ์ผู้ดูแลระบบ", AlertLevel.Warning)
        ElseIf AppSession.AdminUser IsNot Nothing Then
            banner.ShowMessage($"เข้าสู่ระบบโดย {AppSession.AdminUser.DisplayName}", AlertLevel.Success)
        End If
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        NativeMethods.TrySetCaption(Me, Theme.Surface, Theme.TextPrimary, Theme.Border)
    End Sub

    Private Function ReadForm() As DatabaseSettings
        Dim current = AppConfig.Current.Database
        Dim s = current.Clone()
        s.Server = txtServer.Text.Trim()
        Dim port As Integer
        s.Port = If(Integer.TryParse(txtPort.Text.Trim(), port) AndAlso port > 0, port, 3306)
        s.Database = txtDatabase.Text.Trim()
        s.UserName = txtUser.Text.Trim()
        s.Password = txtPassword.Text
        s.PasswordProtected = ""
        Return s
    End Function

    Private Function ValidateInput(s As DatabaseSettings) As Boolean
        If s.Server = "" OrElse s.Database = "" OrElse s.UserName = "" Then
            banner.ShowMessage("กรุณากรอกเซิร์ฟเวอร์ ฐานข้อมูล และชื่อผู้ใช้ให้ครบ", AlertLevel.Warning)
            Return False
        End If
        Return True
    End Function

    Private Async Function TestAsync(s As DatabaseSettings) As Task(Of Boolean)
        banner.ShowMessage("กำลังทดสอบการเชื่อมต่อ...", AlertLevel.Info, IconKind.Clock)
        UseWaitCursor = True
        btnTest.Enabled = False
        btnSave.Enabled = False
        Try
            Await New MySqlHosRepository(s).TestConnectionAsync()
            banner.ShowMessage($"เชื่อมต่อสำเร็จ  ({s.Server}:{s.Port} / {s.Database})", AlertLevel.Success)
            Return True
        Catch ex As Exception
            Dim msg = If(ex.InnerException IsNot Nothing, ex.InnerException.Message, ex.Message)
            banner.ShowMessage("เชื่อมต่อไม่ได้: " & msg, AlertLevel.Danger)
            Return False
        Finally
            UseWaitCursor = False
            btnTest.Enabled = True
            btnSave.Enabled = True
        End Try
    End Function

    Private Async Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        Dim s = ReadForm()
        If Not ValidateInput(s) Then Return
        Await TestAsync(s)
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim s = ReadForm()
        If Not ValidateInput(s) Then Return
        ' เหมือนเดิม: ต้องเชื่อมต่อได้ก่อนจึงบันทึก
        If Not Await TestAsync(s) Then Return
        Try
            s.SetPassword(txtPassword.Text)
            AppConfig.Current.Database = s
            AppConfig.Save()
            AppSession.ReconnectDatabase()
            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            ErrorDialog.ShowError(Me, "บันทึกการตั้งค่าไม่สำเร็จ", ex)
        End Try
    End Sub

End Class

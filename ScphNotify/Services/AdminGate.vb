''' <summary>
''' ด่านตรวจสิทธิ์ผู้ดูแลระบบ — ใช้ก่อนเปิดหน้าตั้งค่าการเชื่อมต่อฐานข้อมูล
''' ตรวจสอบกับบัญชีผู้ใช้ของ HOSxP (ตาราง opduser) ตามค่าใน appsettings.json ส่วน "Security"
''' </summary>
Public NotInheritable Class AdminGate

    Private Sub New()
    End Sub

    ''' <summary>ข้ามการล็อกอินสำหรับรอบนี้ (ใช้กับสวิตช์ --dbconfig ตอนฐานข้อมูลล่ม)</summary>
    Public Shared Property RecoveryMode As Boolean

    Public Shared ReadOnly Property IsRequired As Boolean
        Get
            Return AppConfig.Current.Security.RequireAdminLogin AndAlso Not RecoveryMode
        End Get
    End Property

    ''' <summary>
    ''' คืน True เมื่อผ่านสิทธิ์แล้ว (ล็อกอินไว้ก่อนหน้า หรือเพิ่งล็อกอินสำเร็จ)
    ''' คืน False เมื่อผู้ใช้ยกเลิก หรือไม่มีสิทธิ์
    ''' </summary>
    Public Shared Function Require(owner As IWin32Window) As Boolean
        If Not IsRequired Then Return True
        If AppSession.IsAdminSignedIn Then Return True
        Return AdminLoginDialog.SignIn(owner)
    End Function

End Class

''' <summary>
''' ออกจากระบบผู้ดูแลอัตโนมัติเมื่อไม่มีการใช้งานโปรแกรมนี้
'''
''' หลังล็อกอินผู้ดูแล หน้าตั้งค่าจะเปิดเผยชื่อเซิร์ฟเวอร์ ชื่อฐานข้อมูล และชื่อผู้ใช้
''' ถ้าเจ้าหน้าที่ลุกจากเครื่องโดยไม่กดออกจากระบบ ข้อมูลนั้นจะค้างให้คนถัดไปเห็น
''' จำนวนนาทีตั้งได้ในหน้าตั้งค่า (0 = ไม่ออกอัตโนมัติ)
'''
''' นับเฉพาะการใช้งาน "โปรแกรมนี้" ไม่ได้นับทั้งเครื่อง
''' ถ้าไปอ่านเวลาไม่ใช้งานของ Windows (GetLastInputInfo) สิทธิ์ผู้ดูแลจะไม่หมดอายุเลย
''' เพราะเจ้าหน้าที่พิมพ์งานใน HOSxP อยู่ตลอดวัน ซึ่งไม่ตรงกับเจตนาของการตั้งเวลานี้
''' </summary>
Friend NotInheritable Class AdminIdleTimer
    Implements IMessageFilter

    '── ข้อความของ Windows ที่ถือว่าเป็นการใช้งาน ──
    Private Const WM_KEYDOWN As Integer = &H100
    Private Const WM_SYSKEYDOWN As Integer = &H104
    Private Const WM_MOUSEMOVE As Integer = &H200
    Private Const WM_LBUTTONDOWN As Integer = &H201
    Private Const WM_RBUTTONDOWN As Integer = &H204
    Private Const WM_MBUTTONDOWN As Integer = &H207
    Private Const WM_MOUSEWHEEL As Integer = &H20A

    ''' <summary>ตรวจทุก 15 วินาที — ละเอียดพอสำหรับเวลาหน่วยนาที และไม่กินแรงเครื่อง</summary>
    Private Const TickMs As Integer = 15000

    Private Shared _instance As AdminIdleTimer
    Private Shared _lastActivityUtc As DateTime = DateTime.UtcNow
    Private Shared _lastMouse As Point

    Private ReadOnly _timer As New Timer()

    Private Sub New()
        _timer.Interval = TickMs
        AddHandler _timer.Tick, AddressOf OnTick
        _timer.Start()
    End Sub

    ''' <summary>เริ่มเฝ้าดู — เรียกครั้งเดียวตอนเปิดโปรแกรม ก่อน Application.Run</summary>
    Public Shared Sub Install()
        If _instance IsNot Nothing Then Return
        _instance = New AdminIdleTimer()
        Application.AddMessageFilter(_instance)
    End Sub

    ''' <summary>เริ่มนับเวลาใหม่ — เรียกได้ตลอด แม้ยังไม่ได้ Install</summary>
    Public Shared Sub NoteActivity()
        _lastActivityUtc = DateTime.UtcNow
    End Sub

    ''' <summary>ไม่มีการใช้งานมากี่นาทีแล้ว</summary>
    Public Shared ReadOnly Property IdleMinutes As Double
        Get
            Return (DateTime.UtcNow - _lastActivityUtc).TotalMinutes
        End Get
    End Property

    Public Function PreFilterMessage(ByRef m As Message) As Boolean Implements IMessageFilter.PreFilterMessage
        Select Case m.Msg
            Case WM_KEYDOWN, WM_SYSKEYDOWN, WM_LBUTTONDOWN, WM_RBUTTONDOWN, WM_MBUTTONDOWN, WM_MOUSEWHEEL
                NoteActivity()

            Case WM_MOUSEMOVE
                ' WM_MOUSEMOVE มาถี่มาก และมาเองเมื่อหน้าต่างขยับใต้เมาส์ที่วางนิ่งอยู่
                ' (แถบแจ้งเตือนของโปรแกรมนี้ขยับตามหน้าต่าง HOSxP ด้วย)
                ' จึงนับเป็นการใช้งานเฉพาะเมื่อตำแหน่งเมาส์เปลี่ยนจริง
                Dim p = Control.MousePosition
                If p <> _lastMouse Then
                    _lastMouse = p
                    NoteActivity()
                End If
        End Select

        Return False        ' ดูอย่างเดียว ไม่กินข้อความ
    End Function

    Private Sub OnTick(sender As Object, e As EventArgs)
        ' ยังไม่ได้ล็อกอินก็ไม่มีอะไรให้หมดอายุ — เริ่มนับใหม่ไว้
        ' ไม่อย่างนั้นคนที่เปิดโปรแกรมทิ้งไว้ครึ่งวันแล้วเพิ่งมาล็อกอิน จะถูกเตะออกทันที
        If Not AppSession.IsAdminSignedIn Then
            NoteActivity()
            Return
        End If

        Dim limit = AppConfig.Current.Security.AdminIdleLogoutMinutes
        If limit <= 0 Then Return                      ' 0 = ปิดการออกอัตโนมัติ
        If IdleMinutes < limit Then Return

        AppSession.SignOutAdmin(becauseIdle:=True)
    End Sub

End Class

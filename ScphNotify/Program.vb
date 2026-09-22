Imports System.Threading

''' <summary>
''' จุดเริ่มโปรแกรม
'''   ScphNotify.exe            ใช้งานจริง (เชื่อมต่อ HOSxP ตาม appsettings.json / การตั้งค่าในเครื่อง)
'''   ScphNotify.exe --demo     โหมดสาธิต (ข้อมูลสมมติ ไม่ต่อฐานข้อมูล)
'''   ScphNotify.exe --dbconfig โหมดกู้คืน: เปิดหน้าตั้งค่าฐานข้อมูลโดยไม่ต้องล็อกอิน
'''                             (ใช้เมื่อต่อฐานข้อมูลไม่ได้จนล็อกอินตรวจสิทธิ์ไม่ได้)
''' </summary>
Friend Module Program

    Private Const InstanceName As String = "Local\ScphNotify2026.Instance"
    Private Const SignalName As String = "Local\ScphNotify2026.Show"

    <STAThread()>
    Friend Sub Main(args As String())
        Dim createdNew As Boolean
        Using mutex As New Mutex(True, InstanceName, createdNew)
            If Not createdNew Then
                ' เปิดซ้ำ → ให้โปรแกรมที่เปิดอยู่แสดงหน้าต่างหลักแทน (เดิม: แจ้งว่าเปิดไว้แล้วแล้วปิด)
                SignalRunningInstance()
                Return
            End If

#If Not MONO_PREVIEW Then
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2)
#End If
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException)
            AddHandler Application.ThreadException, AddressOf OnThreadException

            AppConfig.Load()
            Dim demo = args.Any(Function(a) a.Equals("--demo", StringComparison.OrdinalIgnoreCase) OrElse
                                            a.Equals("/demo", StringComparison.OrdinalIgnoreCase))
            AppSession.Initialize(demo)

            ' โหมดกู้คืน: เปิดหน้าตั้งค่าฐานข้อมูลได้โดยไม่ต้องล็อกอิน (ใช้ตอนต่อฐานข้อมูลไม่ได้)
            If args.Any(Function(a) a.Equals("--dbconfig", StringComparison.OrdinalIgnoreCase) OrElse
                                    a.Equals("/dbconfig", StringComparison.OrdinalIgnoreCase)) Then
                AdminGate.RecoveryMode = True
                DbConfigDialog.ShowConfig(Nothing)
                AdminGate.RecoveryMode = False
            End If

            Dim widget As New NotifierWidget()
            Dim signal = ListenForOtherInstances(widget)
            Try
                Application.Run(widget)
            Finally
                signal?.Dispose()
            End Try
        End Using
    End Sub

    Private Sub SignalRunningInstance()
        Try
            Using ev = EventWaitHandle.OpenExisting(SignalName)
                ev.Set()
            End Using
        Catch
        End Try
    End Sub

    Private Function ListenForOtherInstances(widget As NotifierWidget) As EventWaitHandle
        Try
            Dim ev As New EventWaitHandle(False, EventResetMode.AutoReset, SignalName)
            ThreadPool.RegisterWaitForSingleObject(ev, Sub(state, timedOut) widget.RequestShowMainWindow(), Nothing, -1, False)
            Return ev
        Catch
            Return Nothing
        End Try
    End Function

    Private Sub OnThreadException(sender As Object, e As ThreadExceptionEventArgs)
        ErrorDialog.ShowError(Nothing, "เกิดข้อผิดพลาดที่ไม่คาดคิด", e.Exception)
    End Sub

End Module

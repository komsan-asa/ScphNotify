''' <summary>โหมดการทำงานของหน้าจอ</summary>
Public Enum MonitorMode
    ''' <summary>รอเรียกผู้ป่วยอัตโนมัติ — ตามการล็อก VN ที่เครื่องนี้ (ค่าเริ่มต้น)</summary>
    Auto = 0
    ''' <summary>ค้นหาผู้ป่วยเอง — ไม่อ้างอิงว่าผู้ป่วยมารับบริการวันนี้</summary>
    Manual = 1
End Enum

Public Enum MonitorState
    Starting
    Waiting
    Loading
    Ready
    [Error]
End Enum

Public Class PatientChangedEventArgs
    Inherits EventArgs

    Public Sub New(snapshot As PatientSnapshot)
        Me.Snapshot = snapshot
    End Sub

    ''' <summary>Nothing = ไม่มีผู้ป่วยถูกล็อกที่เครื่องนี้ (ระบบรอเรียกผู้ป่วย)</summary>
    Public ReadOnly Property Snapshot As PatientSnapshot
End Class

Public Class OccupationalEventArgs
    Inherits EventArgs

    Public Sub New(target As OccupationalTarget)
        Me.Target = target
    End Sub

    Public ReadOnly Property Target As OccupationalTarget
End Class

''' <summary>
''' ตัวตรวจสอบผู้ป่วยที่ถูกล็อก (vn_lock) ที่เครื่องนี้ทุก ๆ N วินาที
''' รวม TimerRefresh ของ dialogMain และ frmMain เดิมไว้ที่เดียว ทั้งสองหน้าจอรับ event ร่วมกัน
''' </summary>
Public Class PatientMonitor
    Implements IDisposable

    Private WithEvents _timer As New System.Windows.Forms.Timer() With {.Interval = 1000}
    Private _interval As Integer = 5
    Private _remaining As Integer
    Private _busy As Boolean
    Private _lastVn As String = Nothing
    Private _initialized As Boolean
    Private _state As MonitorState = MonitorState.Starting
    Private _current As PatientSnapshot

    Private _mode As MonitorMode = MonitorMode.Auto

    Public Event Countdown As EventHandler
    Public Event ModeChanged As EventHandler
    Public Event StateChanged As EventHandler
    Public Event PatientChanged As EventHandler(Of PatientChangedEventArgs)
    Public Event OccupationalScreeningRequired As EventHandler(Of OccupationalEventArgs)

    Public Sub New(intervalSeconds As Integer)
        IntervalSeconds = intervalSeconds
    End Sub

    Public Property IntervalSeconds As Integer
        Get
            Return _interval
        End Get
        Set(value As Integer)
            _interval = Math.Max(3, Math.Min(120, value))
            If _remaining > _interval Then _remaining = _interval
        End Set
    End Property

    Public ReadOnly Property SecondsRemaining As Integer
        Get
            Return Math.Max(0, _remaining)
        End Get
    End Property

    Public ReadOnly Property State As MonitorState
        Get
            Return _state
        End Get
    End Property

    Public ReadOnly Property LastError As String = ""

    Public ReadOnly Property LastCheckedAt As DateTime?

    Public ReadOnly Property Current As PatientSnapshot
        Get
            Return _current
        End Get
    End Property

    Public ReadOnly Property IsBusy As Boolean
        Get
            Return _busy
        End Get
    End Property

    ''' <summary>โหมดปัจจุบัน — ตั้งผ่าน SwitchToAuto / ShowManualPatientAsync</summary>
    Public ReadOnly Property Mode As MonitorMode
        Get
            Return _mode
        End Get
    End Property

    ''' <summary>
    ''' กลับสู่โหมดรอเรียกผู้ป่วยอัตโนมัติ
    ''' (เรียกทุกครั้งที่ปิดหน้าต่างหลัก เพื่อให้กลับมาเป็นโหมดนี้เสมอ)
    ''' </summary>
    Public Sub SwitchToAuto()
        If _mode = MonitorMode.Auto Then Return
        _mode = MonitorMode.Auto
        _lastVn = Nothing          ' ให้โหลดผู้ป่วยที่ล็อกอยู่จริงในรอบถัดไป
        _current = Nothing
        _remaining = 0
        RaiseEvent ModeChanged(Me, EventArgs.Empty)
        RaiseEvent PatientChanged(Me, New PatientChangedEventArgs(Nothing))
    End Sub

    ''' <summary>เข้าสู่โหมดค้นหาเอง แล้วแสดงผู้ป่วยตาม HN ที่เลือก</summary>
    Public Async Function ShowManualPatientAsync(hn As String) As Task
        If String.IsNullOrWhiteSpace(hn) Then Return
        Dim modeChanged = _mode <> MonitorMode.Manual
        _mode = MonitorMode.Manual
        _lastVn = Nothing
        If modeChanged Then RaiseEvent ModeChanged(Me, EventArgs.Empty)

        SetState(MonitorState.Loading, "")
        Try
            Dim snap = Await SnapshotLoader.LoadByHnAsync(AppSession.Repository, hn)
            _current = snap
            _LastCheckedAt = DateTime.Now
            SetState(If(snap Is Nothing, MonitorState.Waiting, MonitorState.Ready), "")
            RaiseEvent PatientChanged(Me, New PatientChangedEventArgs(snap))
        Catch ex As Exception
            SetState(MonitorState.Error, ex.Message)
            Throw
        End Try
    End Function

    ''' <summary>ออกจากโหมดค้นหาเอง (ล้างผู้ป่วยที่เลือกไว้) โดยไม่เปลี่ยนโหมด</summary>
    Public Sub ClearManualPatient()
        If _mode <> MonitorMode.Manual Then Return
        _current = Nothing
        SetState(MonitorState.Waiting, "")
        RaiseEvent PatientChanged(Me, New PatientChangedEventArgs(Nothing))
    End Sub

    Public Sub Start()
        _remaining = 0
        _timer.Start()
        OnTimerTick(Me, EventArgs.Empty)
    End Sub

    Public Sub [Stop]()
        _timer.Stop()
    End Sub

    ''' <summary>เรียกหลังเปลี่ยนฐานข้อมูล — โหลดทุกอย่างใหม่</summary>
    Public Sub Reset()
        _initialized = False
        _lastVn = Nothing
        _current = Nothing
        SetState(MonitorState.Starting, "")
        _remaining = 0
    End Sub

    ''' <summary>ตรวจสอบทันที (reloadPatient = โหลดข้อมูลผู้ป่วยคนเดิมใหม่ด้วย)</summary>
    Public Async Function RefreshAsync(Optional reloadPatient As Boolean = True) As Task
        _remaining = _interval
        Await CheckAsync(reloadPatient)
    End Function

    Private Async Sub OnTimerTick(sender As Object, e As EventArgs) Handles _timer.Tick
        _remaining -= 1
        If _remaining <= 0 Then
            _remaining = _interval
            RaiseEvent Countdown(Me, EventArgs.Empty)
            Await CheckAsync(False)
        Else
            RaiseEvent Countdown(Me, EventArgs.Empty)
        End If
    End Sub

    Private Sub SetState(state As MonitorState, message As String)
        Dim changed = state <> _state OrElse message <> _LastError
        _state = state
        _LastError = If(message, "")
        If changed Then RaiseEvent StateChanged(Me, EventArgs.Empty)
    End Sub

    Private Async Function CheckAsync(force As Boolean) As Task
        If _busy Then Return
        _busy = True
        Dim repo = AppSession.Repository
        Dim computer = AppSession.ComputerName
        Try
            If Not _initialized Then
                Await AppSession.InitializeLookupsAsync()
                _initialized = True
            End If

            ' โหมดค้นหาเอง: ไม่ตามการล็อก VN และไม่เปลี่ยนผู้ป่วยที่ผู้ใช้เลือกไว้
            If _mode = MonitorMode.Manual Then
                _LastCheckedAt = DateTime.Now
                If force AndAlso _current IsNot Nothing Then
                    Dim reloaded = Await SnapshotLoader.LoadByHnAsync(repo, _current.Hn)
                    _current = reloaded
                    RaiseEvent PatientChanged(Me, New PatientChangedEventArgs(reloaded))
                End If
                SetState(If(_current Is Nothing, MonitorState.Waiting, MonitorState.Ready), "")
                Return
            End If

            Dim vn = If(Await repo.GetLockedVnAsync(computer), "").Trim()
            _LastCheckedAt = DateTime.Now

            If force OrElse _lastVn Is Nothing OrElse vn <> _lastVn Then
                _lastVn = vn
                If vn = "" Then
                    _current = Nothing
                    SetState(MonitorState.Waiting, "")
                    RaiseEvent PatientChanged(Me, New PatientChangedEventArgs(Nothing))
                Else
                    SetState(MonitorState.Loading, "")
                    Dim snap = Await SnapshotLoader.LoadAsync(repo, vn)
                    _current = snap
                    SetState(If(snap Is Nothing, MonitorState.Waiting, MonitorState.Ready), "")
                    RaiseEvent PatientChanged(Me, New PatientChangedEventArgs(snap))
                End If
                Return   ' เหมือนโค้ดเดิม: รอบที่เปลี่ยนผู้ป่วยจะยังไม่ตรวจคัดกรองโรคจากการทำงาน
            End If

            SetState(If(_current Is Nothing, MonitorState.Waiting, MonitorState.Ready), "")
            If vn <> "" Then Await CheckOccupationalAsync(repo, computer, vn)

        Catch ex As Exception
            _lastVn = Nothing   ' ให้โหลดใหม่ในรอบถัดไป
            SetState(MonitorState.Error, ex.Message)
        Finally
            _busy = False
        End Try
    End Function

    ''' <summary>dialogMain.checkShowOccupational + checkOccupationalByVn (เพิ่มเมื่อ 19/07/2567 ในระบบเดิม)</summary>
    Private Async Function CheckOccupationalAsync(repo As IHosRepository, computer As String, vn As String) As Task
        ' ปิดได้จากหน้าตั้งค่า (app_storage key = cbOccupational)
        If Not AppSession.Display.EnableOccupational Then Return
        Dim target As OccupationalTarget
        Try
            target = Await repo.GetOccupationalTargetAsync(computer)
            If target Is Nothing Then Return
            If Await repo.HasOccupationalScreeningAsync(vn) Then Return
            target.Vn = vn
            target.Staff = Await repo.GetLoginOnComputerAsync(computer)
        Catch ex As Exception
            ' ตาราง/ฐานข้อมูลบางแห่งอาจไม่มีตารางคัดกรอง — ไม่ต้องแจ้งเตือนซ้ำทุกรอบ
            Trace.WriteLine("Occupational check failed: " & ex.Message)
            Return
        End Try
        RaiseEvent OccupationalScreeningRequired(Me, New OccupationalEventArgs(target))
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        _timer.Stop()
        _timer.Dispose()
    End Sub

End Class

''' <summary>โหลดข้อมูลทั้งหมดของผู้ป่วย 1 คน (frmMain.checkDataPatient เดิม)</summary>
Public NotInheritable Class SnapshotLoader

    Private Sub New()
    End Sub

    Public Shared Async Function LoadAsync(repo As IHosRepository, vn As String) As Task(Of PatientSnapshot)
        Dim header = Await repo.GetPatientByVnAsync(vn)
        If header Is Nothing OrElse String.IsNullOrWhiteSpace(header.Hn) Then Return Nothing
        Return Await LoadDetailsAsync(repo, New PatientSnapshot With {
            .Vn = vn, .Hn = header.Hn, .PatientName = header.Name, .AgeYears = header.AgeYears, .LoadedAt = DateTime.Now})
    End Function

    ''' <summary>
    ''' โหลดข้อมูลจาก HN โดยไม่อ้างอิง VN (โหมดค้นหาเอง)
    ''' ถ้าผู้ป่วยมารับบริการวันนี้จะผูก VN ของวันนี้ให้ด้วย
    ''' </summary>
    Public Shared Async Function LoadByHnAsync(repo As IHosRepository, hn As String) As Task(Of PatientSnapshot)
        Dim header = Await repo.GetPatientByHnAsync(hn)
        If header Is Nothing OrElse String.IsNullOrWhiteSpace(header.Hn) Then Return Nothing

        Dim s As New PatientSnapshot With {
            .Hn = header.Hn, .PatientName = header.Name, .AgeYears = header.AgeYears,
            .IsManual = True, .LoadedAt = DateTime.Now}
        Try
            s.LastVisitDate = Await repo.GetLastVisitDateAsync(header.Hn)
        Catch
        End Try
        Try
            s.Vn = If(Await repo.GetTodayVnAsync(header.Hn), "")
        Catch
        End Try
        Return Await LoadDetailsAsync(repo, s)
    End Function

    Private Shared Async Function LoadDetailsAsync(repo As IHosRepository, s As PatientSnapshot) As Task(Of PatientSnapshot)
        Dim hn = s.Hn
        Dim warn As New List(Of String)

        s.CaseHd = Await SafeAsync(Function() repo.GetCaseHdAsync(hn), "", "Case HD", warn)
        s.ScreeningText = ClinicalRules.CleanScreeningText(Await SafeAsync(Function() repo.GetScreeningTextAsync(hn), "", "ตรวจคัดกรอง ตา ไต เท้า", warn))
        s.EgfrResults = Await SafeAsync(Function() repo.GetEgfrBelow60Async(hn), New List(Of String), "eGFR", warn)
        s.HasDmDiagnosis = Await SafeAsync(Function() repo.HasDmDiagnosisAsync(hn), False, "DM Dx", warn)
        s.HasHtDiagnosis = Await SafeAsync(Function() repo.HasHtDiagnosisAsync(hn), False, "HT Dx", warn)
        s.NextAppointments = Await SafeAsync(Function() repo.GetNextAppointmentsAsync(hn), New List(Of String), "นัดครั้งถัดไป", warn)
        s.CvdRisk = Await SafeAsync(Function() repo.GetCvdRiskAsync(hn), New List(Of CvdRiskEntry), "CVD Risk", warn)
        s.FerritinResults = Await SafeAsync(Function() repo.GetFerritinAsync(hn), New List(Of String), "Ferritin", warn)

        ' การลงทะเบียนคลินิก (ตรวจเฉพาะผู้ที่มีการวินิจฉัย เหมือนเดิม)
        If s.HasDmDiagnosis Then
            s.DmClinicRegistered = Await SafeAsync(Function() repo.IsClinicMemberAsync(hn, AppSession.DmClinicCode), True, "คลินิก DM", warn)
        End If
        If s.HasHtDiagnosis Then
            s.HtClinicRegistered = Await SafeAsync(Function() repo.IsClinicMemberAsync(hn, AppSession.HtClinicCode), True, "คลินิก HT", warn)
        End If

        ' qualify (DM/HT Green) — คงตรรกะเดิม ดู ClinicalRules
        Dim dq(5) As String
        For i = 1 To 5
            If s.HasDmDiagnosis Then
                Dim idx = i
                dq(i) = Await SafeAsync(Function() repo.GetDmQualifyAsync(hn, idx), " ", $"DM qualify {i}", warn)
            Else
                dq(i) = $"not_in_qualify_0{i}"
            End If
        Next
        Dim hq1 = If(s.HasHtDiagnosis,
                     Await SafeAsync(Function() repo.GetHtQualify01Async(hn), " ", "HT qualify 1", warn),
                     "not_in_qualify_01")
        s.IsDmGreen = ClinicalRules.IsDmGreen(s.HasDmDiagnosis, dq(1), dq(2), dq(3), dq(4), dq(5))
        s.IsHtGreen = ClinicalRules.IsHtGreen(s.HasHtDiagnosis, hq1, "", "", "", "")

        s.LoadWarnings = warn
        Return s
    End Function

    Private Shared Async Function SafeAsync(Of T)(work As Func(Of Task(Of T)), fallback As T, label As String, warnings As List(Of String)) As Task(Of T)
        Try
            Return Await work()
        Catch ex As Exception
            warnings.Add($"{label}: {ex.Message}")
            Return fallback
        End Try
    End Function

End Class

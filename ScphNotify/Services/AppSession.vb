Imports System.Reflection

''' <summary>
''' สถานะกลางของโปรแกรม (แทน Module System_config / MySql_config ของเดิม)
''' </summary>
Public NotInheritable Class AppSession

    Private Sub New()
    End Sub

    Public Shared Property ComputerName As String = GetComputerName()
    Public Shared Property Repository As IHosRepository = New DemoHosRepository()
    Public Shared Property Monitor As PatientMonitor
    Public Shared Property Display As New DisplayPreferences()
    Public Shared Property LabYears As New List(Of Integer)
    Public Shared Property DmClinicCode As String = ""
    Public Shared Property HtClinicCode As String = ""

    ''' <summary>เกิดเมื่อเปลี่ยนฐานข้อมูล หรือบันทึกการตั้งค่าการแสดงผล</summary>
    Public Shared Event SettingsChanged As EventHandler

    Public Shared ReadOnly Property IsDemo As Boolean
        Get
            Return Repository IsNot Nothing AndAlso Repository.IsDemo
        End Get
    End Property

    Public Shared ReadOnly Property VersionText As String
        Get
            Dim v = Assembly.GetExecutingAssembly().GetName().Version
            Return $"v{v.Major}.{v.Minor}.{v.Build}"
        End Get
    End Property

    ''' <summary>ชื่อผู้พัฒนา (แสดงต่อท้ายเวอร์ชันบนหัวหน้าต่างและหน้าตั้งค่า)</summary>
    Public Const DeveloperText As String = "Developed by Komsan Asa"

    Private Shared _adminUser As HosUser

    ''' <summary>ผู้ดูแลระบบที่ล็อกอินอยู่ (Nothing = ยังไม่ได้ล็อกอิน)</summary>
    Public Shared Property AdminUser As HosUser
        Get
            Return _adminUser
        End Get
        Set(value As HosUser)
            _adminUser = value
            If value IsNot Nothing Then
                ' ล็อกอินสำเร็จ — เริ่มนับเวลาไม่ใช้งานใหม่ และลบเหตุผลการออกครั้งก่อน
                SignedOutByIdle = False
                AdminIdleTimer.NoteActivity()
            End If
        End Set
    End Property

    ''' <summary>ครั้งล่าสุดออกจากระบบเพราะหมดเวลาไม่ใช้งาน (ใช้บอกเหตุผลในหน้าตั้งค่า)</summary>
    Public Shared Property SignedOutByIdle As Boolean

    Public Shared ReadOnly Property IsAdminSignedIn As Boolean
        Get
            Return AdminUser IsNot Nothing AndAlso AdminUser.IsAdmin
        End Get
    End Property

    ''' <summary>ออกจากระบบผู้ดูแล (ซ่อนข้อมูลเซิร์ฟเวอร์อีกครั้ง)</summary>
    Public Shared Sub SignOutAdmin()
        SignOutAdmin(False)
    End Sub

    ''' <summary>ออกจากระบบผู้ดูแล พร้อมบอกว่าเป็นเพราะหมดเวลาไม่ใช้งานหรือไม่</summary>
    Public Shared Sub SignOutAdmin(becauseIdle As Boolean)
        If _adminUser Is Nothing Then Return
        _adminUser = Nothing
        SignedOutByIdle = becauseIdle
        RaiseEvent SettingsChanged(Nothing, EventArgs.Empty)
    End Sub

    ''' <summary>ข้อความหัวหน้าต่างแบบเดิม: [Connect server # db : name] — แสดงเฉพาะหลังล็อกอินผู้ดูแลระบบ</summary>
    Public Shared ReadOnly Property ConnectionTitle As String
        Get
            If IsDemo Then Return "[โหมดสาธิต]"
            If Not IsAdminSignedIn Then Return ""
            Dim db = AppConfig.Current.Database
            Return $"[Connect {db.Server} # db : {db.Database}]"
        End Get
    End Property

    ''' <summary>รายละเอียดการเชื่อมต่อสำหรับแถบด้านซ้าย — ซ่อนชื่อเซิร์ฟเวอร์ไว้จนกว่าจะล็อกอิน</summary>
    Public Shared ReadOnly Property ConnectionDetail As String
        Get
            If IsDemo OrElse IsAdminSignedIn Then Return Repository.Description
            Return "เชื่อมต่อฐานข้อมูลโรงพยาบาล"
        End Get
    End Property

    ''' <summary>ข้อความเวอร์ชัน + ผู้พัฒนา</summary>
    Public Shared ReadOnly Property VersionLine As String
        Get
            Return $"{VersionText}  ·  {DeveloperText}"
        End Get
    End Property

    Private Shared _icon As Icon

    Public Shared ReadOnly Property AppIcon As Icon
        Get
            If _icon Is Nothing Then
                Try
                    Using s = Assembly.GetExecutingAssembly().GetManifestResourceStream("ScphNotify.ico")
                        If s IsNot Nothing Then _icon = New Icon(s)
                    End Using
                Catch
                End Try
                If _icon Is Nothing Then _icon = SystemIcons.Application
            End If
            Return _icon
        End Get
    End Property

    Private Shared Function GetComputerName() As String
        Try
            Return Net.Dns.GetHostName()
        Catch
            Return Environment.MachineName
        End Try
    End Function

    Public Shared Sub Initialize(demo As Boolean)
        If demo Then
            Repository = New DemoHosRepository()
        Else
            Repository = New MySqlHosRepository(AppConfig.Current.Database)
        End If
        Monitor = New PatientMonitor(AppConfig.Current.RefreshSeconds)
    End Sub

    ''' <summary>ใช้หลังบันทึกค่าเชื่อมต่อฐานข้อมูลใหม่ (เดิมคือ NewCnnString)</summary>
    Public Shared Sub ReconnectDatabase()
        If Not IsDemo Then Repository = New MySqlHosRepository(AppConfig.Current.Database)
        Monitor?.Reset()
        RaiseEvent SettingsChanged(Nothing, EventArgs.Empty)
    End Sub

    Public Shared Sub NotifySettingsChanged()
        RaiseEvent SettingsChanged(Nothing, EventArgs.Empty)
    End Sub

    ''' <summary>ค่าที่โหลดครั้งเดียวต่อการเชื่อมต่อ: รหัสคลินิก DM/HT, ปีสำหรับ LAB, การตั้งค่าการแสดงผล</summary>
    Public Shared Async Function InitializeLookupsAsync() As Task
        Dim repo = Repository
        DmClinicCode = Await repo.GetClinicCodeAsync("dm_clinic_code")
        HtClinicCode = Await repo.GetClinicCodeAsync("ht_clinic_code")

        Try
            LabYears = Await repo.GetBuddhistYearsAsync()
        Catch
            LabYears = New List(Of Integer)
        End Try
        If LabYears.Count = 0 Then
            Dim y = Date.Today.Year + 543
            LabYears = Enumerable.Range(y - 9, 10).ToList()
        End If

        Await LoadDisplayPreferencesAsync()
    End Function

    Public Shared Async Function LoadDisplayPreferencesAsync() As Task
        Dim prefs As New DisplayPreferences()
        For Each key In DisplayPreferences.Keys
            Try
                Dim v = Await Repository.GetStorageValueAsync(ComputerName, DisplayPreferences.Section, key)
                prefs.SetValue(key, DisplayPreferences.ParseStored(v))
            Catch
                ' ไม่มีตาราง app_storage → ใช้ค่าเริ่มต้น (แสดงทั้งหมด)
            End Try
        Next
        Display = prefs
    End Function

    Public Shared Async Function SaveDisplayPreferencesAsync(prefs As DisplayPreferences) As Task
        For Each key In DisplayPreferences.Keys
            Await Repository.SaveStorageValueAsync(ComputerName, DisplayPreferences.Section, key, If(prefs.GetValue(key), "True", "False"))
        Next
        Display = prefs
        RaiseEvent SettingsChanged(Nothing, EventArgs.Empty)
    End Function

End Class

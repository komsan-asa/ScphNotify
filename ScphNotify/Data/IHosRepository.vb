''' <summary>
''' ช่องทางอ่าน/เขียนข้อมูล HOSxP ทั้งหมดของโปรแกรม
''' - MySqlHosRepository : ใช้งานจริง (SQL ทั้งหมดย้ายมาจาก Class\class*.vb ของโปรแกรมเดิม)
''' - DemoHosRepository  : ข้อมูลตัวอย่าง สำหรับเปิดดูหน้าจอโดยไม่ต่อฐานข้อมูล (ScphNotify.exe --demo)
''' </summary>
Public Interface IHosRepository

    ReadOnly Property Description As String
    ReadOnly Property IsDemo As Boolean

    Function TestConnectionAsync() As Task

    '── ผู้ป่วยที่ล็อกไว้ที่เครื่อง ──
    Function GetLockedVnAsync(computerName As String) As Task(Of String)
    Function GetPatientByVnAsync(vn As String) As Task(Of PatientHeader)

    '── ค้นหาผู้ป่วยเอง (โหมด Manual) ──
    ''' <summary>ค้นด้วยชื่อ / ชื่อ-สกุล / HN / เลขบัตรประชาชน</summary>
    Function SearchPatientsAsync(keyword As String, maxRows As Integer) As Task(Of List(Of PatientSearchResult))
    ''' <summary>ข้อมูลหัวของผู้ป่วยจาก HN (ไม่อ้างอิง VN)</summary>
    Function GetPatientByHnAsync(hn As String) As Task(Of PatientHeader)
    ''' <summary>วันที่มารับบริการล่าสุด (ใช้เตือนว่าไม่ได้มารับบริการวันนี้)</summary>
    Function GetLastVisitDateAsync(hn As String) As Task(Of Date?)
    ''' <summary>VN ของวันนี้ (ถ้ามี) — ใช้ผูกข้อมูลเมื่อผู้ป่วยมาวันนี้จริง</summary>
    Function GetTodayVnAsync(hn As String) As Task(Of String)

    '── Medical care ──
    Function GetClinicCodeAsync(sysName As String) As Task(Of String)
    Function GetScreeningTextAsync(hn As String) As Task(Of String)
    Function GetEgfrBelow60Async(hn As String) As Task(Of List(Of String))
    Function GetFerritinAsync(hn As String) As Task(Of List(Of String))
    Function GetCaseHdAsync(hn As String) As Task(Of String)
    Function HasDmDiagnosisAsync(hn As String) As Task(Of Boolean)
    Function HasHtDiagnosisAsync(hn As String) As Task(Of Boolean)
    Function IsClinicMemberAsync(hn As String, clinic As String) As Task(Of Boolean)
    ''' <summary>คืนค่าข้อความแบบเดียวกับฟังก์ชัน getChkDmQualify0nByHn เดิม (index 1-5)</summary>
    Function GetDmQualifyAsync(hn As String, index As Integer) As Task(Of String)
    ''' <summary>คืนค่าแบบเดียวกับ getChkHtQualify01ByHn เดิม</summary>
    Function GetHtQualify01Async(hn As String) As Task(Of String)
    Function GetNextAppointmentsAsync(hn As String) As Task(Of List(Of String))
    Function GetCvdRiskAsync(hn As String) As Task(Of List(Of CvdRiskEntry))

    '── LAB ──
    Function GetBuddhistYearsAsync() As Task(Of List(Of Integer))
    Function GetLabItemNamesAsync(search As String, hn As String) As Task(Of List(Of String))
    Function GetLabResultsAsync(hn As String, yearFrom As Integer, yearTo As Integer, labName As String) As Task(Of List(Of LabResultRow))
    Function GetLabChartAsync(hn As String, yearFrom As Integer, yearTo As Integer, labName As String) As Task(Of List(Of LabPoint))
    Function GetLabHistoryAsync(hn As String, labNames As IList(Of String)) As Task(Of List(Of LabPoint))

    '── LAB Template Hemodialysis (เทียบเท่า emr_html/fcontent_lab_hd.php) ──
    ''' <summary>วันที่รายงานล่าสุด (มากสุด maxDates วัน) ที่มีผลของรายการในเทมเพลต</summary>
    Function GetLabTemplateDatesAsync(hn As String, labNames As IList(Of String), fromDate As Date?, toDate As Date?, maxDates As Integer) As Task(Of List(Of Date))
    ''' <summary>ผลตรวจของรายการในเทมเพลต ภายในช่วงวันที่ที่ระบุ</summary>
    Function GetLabTemplateResultsAsync(hn As String, labNames As IList(Of String), fromDate As Date, toDate As Date) As Task(Of List(Of LabPoint))

    '── HD/CAPD Care (ข้อมูลกลุ่ม A — ดึงจากตารางที่ HOSxP มีอยู่แล้ว) ──
    ''' <summary>
    ''' รอบการล้างไตย้อนหลัง อนุมานจากรายการค่าบริการใน opitemrece (nondrugitems)
    ''' 1 วัน = 1 รอบ เพราะ HOSxP ไม่ได้เก็บรอบฟอกเป็นรายการของตัวเอง
    ''' </summary>
    Function GetDialysisSessionsAsync(hn As String, fromDate As Date,
                                      hdKeywords As IList(Of String), pdKeywords As IList(Of String)) As Task(Of List(Of DialysisSession))
    ''' <summary>น้ำหนัก / ความดัน / ชีพจร ที่จุดคัดกรอง (ovst + opdscreen)</summary>
    Function GetVitalHistoryAsync(hn As String, fromDate As Date, maxRows As Integer) As Task(Of List(Of VitalPoint))
    ''' <summary>ยาที่เกี่ยวข้องกับผู้ป่วยล้างไต (ค้นชื่อยาแบบ LIKE ตามคำค้นที่ตั้งไว้)</summary>
    Function GetDialysisDrugsAsync(hn As String, fromDate As Date, keywords As IList(Of String)) As Task(Of List(Of DrugUsage))

    '── ค่าตั้งค่าต่อเครื่อง (app_storage) ──
    Function GetStorageValueAsync(computerName As String, section As String, key As String) As Task(Of String)
    Function SaveStorageValueAsync(computerName As String, section As String, key As String, value As String) As Task(Of Boolean)

    '── คัดกรองโรคจากการทำงาน ──
    Function GetOccupationalTargetAsync(computerName As String) As Task(Of OccupationalTarget)
    Function HasOccupationalScreeningAsync(vn As String) As Task(Of Boolean)
    Function GetLoginOnComputerAsync(computerName As String) As Task(Of String)
    Function SaveOccupationalScreeningAsync(vn As String, hn As String, injury As Boolean, moreSymptoms As Boolean, dustPm As Boolean, staff As String) As Task
    ''' <summary>ทะเบียนผลคัดกรองโรคจากการทำงาน (ช่วงวันที่ + คำค้น HN/VN/ชื่อ)</summary>
    Function GetOccupationalRegistryAsync(fromDate As Date, toDate As Date, search As String, maxRows As Integer) As Task(Of List(Of OccupationalRecord))

    '── ล็อกอินผู้ดูแลระบบ (บัญชี HOSxP ตาราง opduser) ──
    ''' <summary>ตรวจสอบชื่อผู้ใช้ + รหัสผ่าน แล้วบอกด้วยว่าติดที่ขั้นไหน</summary>
    Function AuthenticateAsync(loginName As String, password As String) As Task(Of AuthResult)
    ''' <summary>อ่านข้อมูลบัญชีโดยไม่ตรวจรหัสผ่าน (ใช้กับโหมด "ใช้บัญชีที่ล็อกอิน HOSxP อยู่บนเครื่องนี้")</summary>
    Function GetHosUserAsync(loginName As String) As Task(Of HosUser)

End Interface

''' <summary>ข้อผิดพลาดจากการรัน SQL (เก็บคำสั่ง SQL ไว้แสดงในหน้ารายละเอียด — แทน popUpMySqlTrace เดิม)</summary>
Public Class HosQueryException
    Inherits Exception

    Public Sub New(message As String, sql As String, inner As Exception)
        MyBase.New(message, inner)
        Me.Sql = sql
    End Sub

    Public ReadOnly Property Sql As String
End Class

Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.Json
Imports System.Text.Json.Serialization

''' <summary>ค่าการเชื่อมต่อฐานข้อมูล HOSxP (แทน appSettings DB_SERVER / DB_NAME / USERNAME / PASSWORD ใน App.config เดิม)</summary>
Public Class DatabaseSettings
    Public Property Server As String = "localhost"
    Public Property Port As Integer = 3306
    Public Property Database As String = "hos"
    Public Property UserName As String = ""
    ''' <summary>รหัสผ่านแบบข้อความ (ใช้ใน appsettings.json ที่ผู้ดูแลระบบแจกจ่าย)</summary>
    Public Property Password As String = ""
    ''' <summary>รหัสผ่านที่เข้ารหัสด้วย Windows DPAPI (บันทึกจากหน้าตั้งค่าในเครื่องผู้ใช้)</summary>
    Public Property PasswordProtected As String = ""
    ''' <summary>ปิดไว้เป็นค่าเริ่มต้น (เหมือน MySql.Data 6.6 เดิม) — MySQL รุ่นเก่ามักใช้ TLS 1.0 ที่ Windows ปิดไปแล้ว</summary>
    Public Property UseSsl As Boolean = False
    Public Property ConnectTimeoutSeconds As Integer = 15
    Public Property CommandTimeoutSeconds As Integer = 50

    Public Function Clone() As DatabaseSettings
        Return DirectCast(MemberwiseClone(), DatabaseSettings)
    End Function

    Public Function GetPassword() As String
        If Not String.IsNullOrEmpty(PasswordProtected) Then
            Try
                Dim raw = ProtectedData.Unprotect(Convert.FromBase64String(PasswordProtected), AppConfig.Entropy, DataProtectionScope.CurrentUser)
                Return Encoding.UTF8.GetString(raw)
            Catch
                ' ถอดรหัสไม่ได้ (เช่นคัดลอกไฟล์มาจากผู้ใช้อื่น) → ใช้ Password ปกติแทน
            End Try
        End If
        Return If(Password, "")
    End Function

    Public Sub SetPassword(plain As String)
        Try
            Dim enc = ProtectedData.Protect(Encoding.UTF8.GetBytes(If(plain, "")), AppConfig.Entropy, DataProtectionScope.CurrentUser)
            PasswordProtected = Convert.ToBase64String(enc)
            Password = ""
        Catch
            PasswordProtected = ""
            Password = plain
        End Try
    End Sub

    <JsonIgnore>
    Public ReadOnly Property DisplayName As String
        Get
            Return $"{Server} / {Database}"
        End Get
    End Property
End Class

''' <summary>
''' การควบคุมสิทธิ์เข้าหน้าตั้งค่าฐานข้อมูล — ล็อกอินด้วยบัญชี HOSxP (ตาราง opduser)
''' ผู้ดูแลระบบแก้ค่าเหล่านี้ได้ที่ appsettings.json ข้างไฟล์ .exe
''' </summary>
Public Class SecuritySettings
    ''' <summary>True = ต้องล็อกอินก่อนเปิดหน้าตั้งค่าฐานข้อมูล</summary>
    Public Property RequireAdminLogin As Boolean = True

    ''' <summary>ชื่อตารางผู้ใช้ของ HOSxP</summary>
    Public Property UserTable As String = "opduser"
    Public Property LoginColumn As String = "loginname"
    Public Property NameColumn As String = "name"
    ''' <summary>
    ''' คอลัมน์รหัสผ่านที่จะลองตามลำดับ — HOSxP เก็บรหัสผ่านสำหรับโปรแกรมภายนอกไว้ที่ passweb ในรูป MD5
    ''' (ตรงกับ Appoint2020: "loginname = ... and passweb = md5(...)")
    ''' ถ้า passweb ว่าง จะลอง password / passwordx ต่อให้
    ''' </summary>
    Public Property PasswordColumns As String() = {"passweb", "password", "passwordx"}
    ''' <summary>วิธีเทียบรหัสผ่านที่จะลองตามลำดับ: Md5 | Plain | Sha1 | MySqlPassword | EncodeBms | EncodeHos</summary>
    Public Property PasswordModes As String() = {"Md5", "Plain", "Sha1"}

    '── การตัดสินว่าเป็นผู้ดูแลระบบ (แบบเดียวกับ classLogin.checkAccessMenu ของ Appoint2020) ──
    ''' <summary>คอลัมน์กลุ่มผู้ใช้ (opduser.groupname)</summary>
    Public Property GroupColumn As String = "groupname"
    ''' <summary>กลุ่มที่ถือว่าเป็นผู้ดูแลระบบ</summary>
    Public Property AdminGroups As String() = {"admin"}
    ''' <summary>คอลัมน์สิทธิ์ (opduser.accessright) — ใช้ร่วมกับ AccessRightKeyword</summary>
    Public Property AccessRightColumn As String = "accessright"
    ''' <summary>ถ้ากำหนดไว้ บัญชีที่ accessright มีข้อความนี้จะถือว่าเป็นผู้ดูแลระบบด้วย (เว้นว่าง = ไม่ตรวจ)</summary>
    Public Property AccessRightKeyword As String = ""
    ''' <summary>รายชื่อ loginname ที่อนุญาตเพิ่มเติม ไม่ว่ากลุ่มจะเป็นอะไร</summary>
    Public Property AdminLogins As String() = {}

    ''' <summary>
    ''' อนุญาตให้ใช้บัญชีที่กำลังเปิด HOSxP อยู่บนเครื่องนี้โดยไม่ต้องกรอกรหัสผ่าน
    ''' (เหมือนช่อง "Login by pass HOSxP" ของ Appoint2020 — อ่านจากตาราง onlineuser)
    ''' </summary>
    Public Property AllowHosxpSessionLogin As Boolean = True

    ''' <summary>
    ''' ออกจากระบบผู้ดูแลอัตโนมัติเมื่อไม่มีการใช้งานโปรแกรมนี้ครบกี่นาที (0 = ไม่ออกอัตโนมัติ)
    '''
    ''' หลังล็อกอินผู้ดูแล หน้าตั้งค่าจะเปิดเผยชื่อเซิร์ฟเวอร์ ชื่อฐานข้อมูล และชื่อผู้ใช้
    ''' ถ้าเจ้าหน้าที่ลุกจากเครื่องโดยไม่กดออกจากระบบ ข้อมูลนั้นจะค้างให้คนถัดไปเห็น
    ''' ผู้ใช้ปรับค่านี้ได้เองในหน้าตั้งค่า (ดู AdminIdleTimer)
    ''' </summary>
    Public Property AdminIdleLogoutMinutes As Integer = 30

    Public Function IsAdminLogin(loginName As String) As Boolean
        If AdminLogins Is Nothing OrElse AdminLogins.Length = 0 Then Return False
        Return AdminLogins.Any(Function(n) String.Equals(If(n, "").Trim(), If(loginName, "").Trim(), StringComparison.OrdinalIgnoreCase))
    End Function

    Public Function IsAdminGroup(groupName As String) As Boolean
        If AdminGroups Is Nothing OrElse AdminGroups.Length = 0 Then Return False
        Return AdminGroups.Any(Function(g) String.Equals(If(g, "").Trim(), If(groupName, "").Trim(), StringComparison.OrdinalIgnoreCase))
    End Function
End Class

''' <summary>
''' เทมเพลต LAB สำหรับผู้ป่วยฟอกไต — ให้ผลเหมือนหน้า QHis2 emr_html/fcontent_lab_hd.php
''' แก้รายการและลำดับได้ที่ appsettings.json โดยไม่ต้อง build ใหม่
''' </summary>
Public Class LabHemodialysisSettings
    ''' <summary>จำนวนวันที่ (คอลัมน์) ล่าสุดที่แสดง — ของเดิมใช้ 10</summary>
    Public Property MaxDates As Integer = 10

    ''' <summary>
    ''' รายการ LAB ตามลำดับที่ต้องการแสดง (เทียบกับ lab_order.lab_items_name_ref แบบไม่สนตัวพิมพ์)
    ''' แถวที่ไม่มีผลเลยในช่วงที่เลือกจะถูกซ่อนอัตโนมัติ เหมือนหน้าเว็บเดิม
    ''' </summary>
    Public Property Items As String() = {
        "HCT", "HGB", "WBC", "PLT.count",
        "Neutrophil", "Lymphocyte", "Eosinophil", "Basophil", "Monocyte",
        "MCV", "MCH",
        "BUN", "Creatinine", "eGFR",
        "Na:Sodium", "K:Potassium", "Cl:Chloride", "ECO2:Carbondioxide",
        "Ca:Calcium", "Po4:Phosphorus",
        "Albumin", "Uric acid", "HbA1C (Glyco.Hb.)", "Blood sugar", "Glob.",
        "SGOT(AST)", "SGPT(ALT)", "Alk.phosphatase",
        "Cholesterol", "Triglycerides", "HDL-Cholesterol", "LDL-Cholesterol",
        "Iron serum", "TIBC", "Ferritin", "%TSAT", "*Parathyroid Hormone",
        "HBs Ag", "HBs Ab", "HCV Ab", "HIV Ab (CLIA Automation)"}

    Public Function ItemList() As List(Of String)
        If Items Is Nothing Then Return New List(Of String)
        Return Items.Where(Function(s) Not String.IsNullOrWhiteSpace(s)).Select(Function(s) s.Trim()).ToList()
    End Function
End Class

''' <summary>
''' หน้า HD/CAPD Care — คำค้นที่ใช้แยกว่ารายการไหนคือการล้างไต และรายการ LAB/ยาที่จะแสดง
''' แต่ละโรงพยาบาลตั้งชื่อรายการค่าบริการ (nondrugitems) ไม่เหมือนกัน จึงแก้ได้ที่นี่โดยไม่ต้อง build ใหม่
''' </summary>
Public Class DialysisSettings
    ''' <summary>ย้อนหลังกี่เดือน สำหรับรอบล้างไต / น้ำหนัก / ยา</summary>
    Public Property Months As Integer = 6

    ''' <summary>คำค้นในชื่อรายการค่าบริการที่ถือว่าเป็นการฟอกเลือด (HD)</summary>
    Public Property HdKeywords As String() = {"Hemodialysis", "Haemodialysis", "ฟอกเลือด", "ไตเทียม"}

    ''' <summary>คำค้นในชื่อรายการค่าบริการที่ถือว่าเป็นการล้างไตทางช่องท้อง (PD)</summary>
    Public Property PdKeywords As String() = {"CAPD", "APD", "Peritoneal", "ช่องท้อง"}

    ''' <summary>รายการ LAB ที่แสดงในการ์ด "ผล LAB สำคัญ" ตามลำดับ</summary>
    Public Property LabItems As String() = {
        "BUN", "Creatinine", "eGFR",
        "K:Potassium", "Na:Sodium", "ECO2:Carbondioxide",
        "HGB", "HCT", "Albumin",
        "Ca:Calcium", "Po4:Phosphorus", "*Parathyroid Hormone"}

    ''' <summary>คำค้นชื่อยาที่เกี่ยวข้องกับผู้ป่วยล้างไต (ค้นแบบ LIKE ในตาราง drugitems)</summary>
    Public Property DrugKeywords As String() = {
        "EPOETIN", "ERYTHROPOIETIN", "DARBEPOETIN",
        "CALCIUM CARBONATE", "ALUMINIUM HYDROXIDE", "SEVELAMER",
        "CALCITRIOL", "ALFACALCIDOL", "SODAMINT", "SODIUM BICARBONATE",
        "FUROSEMIDE", "FOLIC", "FERROUS", "IRON SUCROSE",
        "AMLODIPINE", "LOSARTAN", "ENALAPRIL", "MANIDIPINE", "METOPROLOL", "DOXAZOSIN"}

    Public Function HdList() As List(Of String)
        Return Clean(HdKeywords)
    End Function

    Public Function PdList() As List(Of String)
        Return Clean(PdKeywords)
    End Function

    Public Function LabList() As List(Of String)
        Return Clean(LabItems)
    End Function

    Public Function DrugList() As List(Of String)
        Return Clean(DrugKeywords)
    End Function

    Private Shared Function Clean(values As String()) As List(Of String)
        If values Is Nothing Then Return New List(Of String)
        Return values.Where(Function(s) Not String.IsNullOrWhiteSpace(s)).Select(Function(s) s.Trim()).ToList()
    End Function
End Class

Public Class AppSettings
    Public Property Database As New DatabaseSettings()
    Public Property Security As New SecuritySettings()
    Public Property LabHemodialysis As New LabHemodialysisSettings()
    Public Property Dialysis As New DialysisSettings()
    ''' <summary>ความถี่ในการตรวจสอบผู้ป่วย (วินาที) — เดิมคือ nudRegister</summary>
    Public Property RefreshSeconds As Integer = 5
    Public Property WidgetTopMost As Boolean = True
    Public Property WidgetLeft As Integer?
    Public Property WidgetTop As Integer?
End Class

''' <summary>
''' โหลด/บันทึกการตั้งค่า
'''   1) appsettings.json ข้างไฟล์ .exe  (ค่าเริ่มต้นที่ผู้ดูแลระบบกำหนด)
'''   2) %LOCALAPPDATA%\ScphNotify\settings.json (ค่าที่ผู้ใช้บันทึกเอง — มีผลเหนือกว่า)
''' </summary>
Public NotInheritable Class AppConfig

    Private Sub New()
    End Sub

    Friend Shared ReadOnly Entropy As Byte() = Encoding.UTF8.GetBytes("ScphNotify.2026")

    Private Shared _current As AppSettings = New AppSettings()

    Public Shared ReadOnly Property Current As AppSettings
        Get
            Return _current
        End Get
    End Property

    Public Shared ReadOnly Property DefaultFilePath As String
        Get
            Return Path.Combine(AppContext.BaseDirectory, "appsettings.json")
        End Get
    End Property

    Public Shared ReadOnly Property UserFilePath As String
        Get
            Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ScphNotify", "settings.json")
        End Get
    End Property

    Private Shared Function JsonOptions() As JsonSerializerOptions
        Dim o As New JsonSerializerOptions()
        o.PropertyNameCaseInsensitive = True
        o.WriteIndented = True
        o.AllowTrailingCommas = True
        o.ReadCommentHandling = JsonCommentHandling.Skip
        Return o
    End Function

    Public Shared Sub Load()
        Dim result As AppSettings = Nothing
        For Each candidate In {UserFilePath, DefaultFilePath}
            Try
                If File.Exists(candidate) Then
                    result = JsonSerializer.Deserialize(Of AppSettings)(File.ReadAllText(candidate, Encoding.UTF8), JsonOptions())
                    If result IsNot Nothing Then Exit For
                End If
            Catch
                result = Nothing
            End Try
        Next
        If result Is Nothing Then result = New AppSettings()
        If result.Database Is Nothing Then result.Database = New DatabaseSettings()
        If result.Security Is Nothing Then result.Security = New SecuritySettings()
        If result.LabHemodialysis Is Nothing Then result.LabHemodialysis = New LabHemodialysisSettings()
        result.LabHemodialysis.MaxDates = Math.Max(1, Math.Min(60, result.LabHemodialysis.MaxDates))
        If result.Dialysis Is Nothing Then result.Dialysis = New DialysisSettings()
        result.Dialysis.Months = Math.Max(1, Math.Min(36, result.Dialysis.Months))
        result.RefreshSeconds = Math.Max(3, Math.Min(120, result.RefreshSeconds))
        result.Security.AdminIdleLogoutMinutes = Math.Max(0, Math.Min(240, result.Security.AdminIdleLogoutMinutes))
        _current = result
    End Sub

    Public Shared Sub Save()
        Dim dir = Path.GetDirectoryName(UserFilePath)
        If Not Directory.Exists(dir) Then Directory.CreateDirectory(dir)
        ' ไม่เก็บรหัสผ่านแบบข้อความลงไฟล์ของผู้ใช้ ถ้าเข้ารหัสได้
        Dim db = _current.Database
        If String.IsNullOrEmpty(db.PasswordProtected) AndAlso Not String.IsNullOrEmpty(db.Password) Then
            db.SetPassword(db.Password)
        End If
        File.WriteAllText(UserFilePath, JsonSerializer.Serialize(_current, JsonOptions()), Encoding.UTF8)
    End Sub

End Class

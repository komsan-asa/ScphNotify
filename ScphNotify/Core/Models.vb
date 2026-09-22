''' <summary>ระดับความสำคัญของการแจ้งเตือน (ใช้กำหนดสีของ chip / badge)</summary>
Public Enum AlertLevel
    Neutral = 0
    Info
    Success
    Warning
    Danger
    Primary
End Enum

''' <summary>ข้อความแจ้งเตือนสั้น ๆ ของผู้ป่วย (แสดงที่แถบแจ้งเตือนและหัวหน้าต่างหลัก)</summary>
Public Class PatientAlert
    Public Sub New(text As String, level As AlertLevel, Optional icon As IconKind = IconKind.None)
        Me.Text = text
        Me.Level = level
        Me.Icon = icon
    End Sub

    Public Property Text As String
    Public Property Level As AlertLevel
    Public Property Icon As IconKind
End Class

''' <summary>ข้อมูลหัวของผู้ป่วยจาก VN (ovst + vn_stat + patient)</summary>
Public Class PatientHeader
    Public Property Hn As String = ""
    Public Property Name As String = ""
    Public Property AgeYears As Integer?
End Class

''' <summary>
''' ข้อมูลทั้งหมดของผู้ป่วยที่ถูกล็อกไว้ที่เครื่องนี้ (โหลดครั้งเดียวเมื่อ VN เปลี่ยน)
''' แทนตัวแปร global cur* ใน System_config.vb ของโปรแกรมเดิม
''' </summary>
Public Class PatientSnapshot
    Public Property Vn As String = ""
    Public Property Hn As String = ""
    Public Property PatientName As String = ""
    Public Property AgeYears As Integer?

    ''' <summary>"Case HD" / "Case CAPD" / ""</summary>
    Public Property CaseHd As String = ""
    ''' <summary>ผลจากฟังก์ชัน HtDmCheckLabComplete(hn) — ตรวจคัดกรอง ตา ไต เท้า</summary>
    Public Property ScreeningText As String = ""
    ''' <summary>eGFR &lt; 60 ภายใน 1 ปี (ล่าสุด 3 ค่า) เช่น "53.59 (31 ส.ค.68)"</summary>
    Public Property EgfrResults As New List(Of String)
    ''' <summary>Ferritin ล่าสุด 3 ค่า</summary>
    Public Property FerritinResults As New List(Of String)

    Public Property HasDmDiagnosis As Boolean
    Public Property HasHtDiagnosis As Boolean
    Public Property DmClinicRegistered As Boolean = True
    Public Property HtClinicRegistered As Boolean = True
    Public Property IsDmGreen As Boolean
    Public Property IsHtGreen As Boolean

    Public Property NextAppointments As New List(Of String)
    Public Property CvdRisk As New List(Of CvdRiskEntry)

    ''' <summary>True = ผู้ป่วยที่ผู้ใช้ค้นหาเอง (โหมด Manual) ไม่ได้มาจากการล็อก VN ที่เครื่องนี้</summary>
    Public Property IsManual As Boolean
    ''' <summary>วันที่มารับบริการล่าสุด (โหมด Manual ใช้บอกว่ามาวันนี้หรือไม่)</summary>
    Public Property LastVisitDate As Date?

    ''' <summary>มารับบริการวันนี้หรือไม่ (โหมดอัตโนมัติถือว่ามาเสมอ เพราะล็อก VN อยู่)</summary>
    Public ReadOnly Property HasVisitToday As Boolean
        Get
            If Not IsManual Then Return True
            Return LastVisitDate.HasValue AndAlso LastVisitDate.Value.Date = Date.Today
        End Get
    End Property

    Public Property LoadedAt As DateTime = DateTime.Now
    ''' <summary>คำสั่งที่โหลดไม่สำเร็จ (แสดงเป็นหมายเหตุ แทนการเด้ง MessageBox แบบเดิม)</summary>
    Public Property LoadWarnings As New List(Of String)

    Public ReadOnly Property AgeText As String
        Get
            Return If(AgeYears.HasValue, $"อายุ {AgeYears.Value} ปี", "")
        End Get
    End Property

    ''' <summary>ข้อความแบบเดิม: "HN: xxx   ชื่อ: yyy  (อายุ n ปี)"</summary>
    Public ReadOnly Property ClassicTitle As String
        Get
            Dim s = $"HN: {Hn}   ชื่อ: {PatientName}"
            If AgeYears.HasValue Then s &= $"  ({AgeText})"
            Return s
        End Get
    End Property

    Public ReadOnly Property LatestCvd As CvdRiskEntry
        Get
            Return CvdRisk.Where(Function(c) c.Score.HasValue).OrderByDescending(Function(c) c.VisitDate.GetValueOrDefault()).FirstOrDefault()
        End Get
    End Property

    ''' <summary>รวมรายการแจ้งเตือนตามการตั้งค่าการแสดงผลของเครื่อง</summary>
    Public Function GetAlerts(prefs As DisplayPreferences) As List(Of PatientAlert)
        Dim list As New List(Of PatientAlert)
        If prefs Is Nothing Then prefs = New DisplayPreferences()

        If prefs.ShowCaseHd AndAlso Not String.IsNullOrWhiteSpace(CaseHd) Then
            list.Add(New PatientAlert(CaseHd.Trim(), AlertLevel.Danger, IconKind.Filter))
        End If
        If prefs.ShowEgfr AndAlso EgfrResults.Count > 0 Then
            list.Add(New PatientAlert("eGFR < 60", AlertLevel.Warning, IconKind.Activity))
        End If
        If prefs.ShowDmColor AndAlso HasDmDiagnosis AndAlso Not DmClinicRegistered Then
            list.Add(New PatientAlert("ยังไม่ลงทะเบียนคลินิก DM", AlertLevel.Warning, IconKind.Droplet))
        End If
        If prefs.ShowHtColor AndAlso HasHtDiagnosis AndAlso Not HtClinicRegistered Then
            list.Add(New PatientAlert("ยังไม่ลงทะเบียนคลินิก HT", AlertLevel.Warning, IconKind.Gauge))
        End If
        If prefs.ShowDmColor AndAlso IsDmGreen Then list.Add(New PatientAlert("DM Green", AlertLevel.Success, IconKind.Droplet))
        If prefs.ShowHtColor AndAlso IsHtGreen Then list.Add(New PatientAlert("HT Green", AlertLevel.Success, IconKind.Gauge))

        Dim cvd = LatestCvd
        If cvd IsNot Nothing AndAlso cvd.Level >= 3 Then
            list.Add(New PatientAlert($"CVD Risk {cvd.ScoreText}", If(cvd.Level >= 4, AlertLevel.Danger, AlertLevel.Warning), IconKind.HeartPulse))
        End If
        Return list
    End Function
End Class

''' <summary>1 แถวของ Thai CV Risk (จาก ThaiCVRiskCal)</summary>
Public Class CvdRiskEntry
    Public Property VisitDate As Date?
    Public Property Score As Double?
    Public Property Bps As Double?
    Public Property LabText As String = ""
    Public Property IsDm As Boolean
    Public Property IsSmoker As Boolean
    Public Property Age As Integer?

    Public ReadOnly Property Level As Integer
        Get
            Return CvdRiskLevel.FromScore(Score)
        End Get
    End Property

    Public ReadOnly Property ScoreText As String
        Get
            Return If(Score.HasValue, $"{Score.Value:0.##}%", "-")
        End Get
    End Property
End Class

Public NotInheritable Class CvdRiskLevel
    Private Sub New()
    End Sub

    Public Shared ReadOnly Names As String() = {"ไม่มีข้อมูล", "ต่ำ", "ปานกลาง", "สูง", "สูงมาก", "สูงอันตราย"}
    Public Shared ReadOnly Ranges As String() = {"-", "< 10%", "10–20%", "20–30%", "30–40%", "≥ 40%"}

    Public Shared Function FromScore(score As Double?) As Integer
        If Not score.HasValue Then Return 0
        Dim s = score.Value
        If s < 10 Then Return 1
        If s < 20 Then Return 2
        If s < 30 Then Return 3
        If s < 40 Then Return 4
        Return 5
    End Function
End Class

''' <summary>ผล LAB 1 แถว (แท็บตาราง)</summary>
Public Class LabResultRow
    Public Property OrderNumber As String = ""
    Public Property ReportDate As Date?
    Public Property ItemName As String = ""
    Public Property Result As String = ""
    Public Property NormalValue As String = ""
    Public Property Flag As String = ""
    Public Property SubGroup As String = ""
End Class

''' <summary>จุดข้อมูลสำหรับกราฟ</summary>
Public Class ChartPoint
    Public Sub New(label As String, value As Double, Optional detail As String = "")
        Me.Label = label
        Me.Value = value
        Me.Detail = detail
    End Sub

    Public Property Label As String
    Public Property Value As Double
    Public Property Detail As String
End Class

''' <summary>ผล LAB สำหรับกราฟ/พิมพ์ (lab_name, report_date, lab_order_result)</summary>
Public Class LabPoint
    Public Property LabName As String = ""
    Public Property ReportDate As Date?
    Public Property ResultText As String = ""

    Public ReadOnly Property NumericValue As Double?
        Get
            Dim v As Double
            Dim cleaned = If(ResultText, "").Trim().TrimStart("<"c, ">"c, "="c).Trim()
            If Double.TryParse(cleaned, Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, v) Then Return v
            Return Nothing
        End Get
    End Property
End Class

''' <summary>
''' 1 แถวของตาราง LAB Template Hemodialysis
''' Index = ลำดับในเทมเพลต (คงเลขเดิมแม้แถวอื่นถูกซ่อน เหมือนคอลัมน์ Index ของหน้าเว็บเดิม)
''' </summary>
Public Class LabTemplateRow
    Public Property Index As Integer
    Public Property LabName As String = ""
    Public Property Values As String()

    Public ReadOnly Property HasAny As Boolean
        Get
            Return Values IsNot Nothing AndAlso Values.Any(Function(v) Not String.IsNullOrWhiteSpace(v))
        End Get
    End Property

    Public Function ValueAt(i As Integer) As String
        If Values Is Nothing OrElse i < 0 OrElse i >= Values.Length Then Return ""
        Return If(Values(i), "")
    End Function
End Class

''' <summary>1 แถวของผลค้นหาผู้ป่วย (โหมด Manual — ค้นด้วยชื่อ / ชื่อ-สกุล / HN / เลขบัตรประชาชน)</summary>
Public Class PatientSearchResult
    Public Property Hn As String = ""
    Public Property Name As String = ""
    Public Property Cid As String = ""
    Public Property AgeYears As Integer?
    Public Property Birthday As Date?
    ''' <summary>วันที่มารับบริการล่าสุด</summary>
    Public Property LastVisitDate As Date?

    Public ReadOnly Property VisitedToday As Boolean
        Get
            Return LastVisitDate.HasValue AndAlso LastVisitDate.Value.Date = Date.Today
        End Get
    End Property

    Public ReadOnly Property AgeText As String
        Get
            Return If(AgeYears.HasValue, $"{AgeYears.Value} ปี", "-")
        End Get
    End Property

    ''' <summary>ปิดบังเลขบัตรประชาชนไว้บางส่วน (แสดงในตารางผลค้นหา)</summary>
    Public ReadOnly Property MaskedCid As String
        Get
            Dim c = If(Cid, "").Trim()
            If c.Length < 13 Then Return c
            Return c.Substring(0, 4) & "-xxxxx-" & c.Substring(9)
        End Get
    End Property
End Class

''' <summary>ผู้ป่วยที่ต้องคัดกรองโรคจากการทำงาน</summary>
Public Class OccupationalTarget
    Public Property Vn As String = ""
    Public Property Hn As String = ""
    Public Property Staff As String = ""
End Class

''' <summary>1 แถวในทะเบียนคัดกรองโรคจากการทำงาน (ตาราง opdscreen_occupational)</summary>
Public Class OccupationalRecord
    Public Property ScreenedAt As Date?
    Public Property Vn As String = ""
    Public Property Hn As String = ""
    Public Property PatientName As String = ""
    Public Property AgeYears As Integer?
    Public Property Injury As Boolean
    Public Property MoreSymptoms As Boolean
    Public Property DustPm As Boolean
    Public Property Staff As String = ""

    ''' <summary>ตอบ "ใช่" อย่างน้อย 1 ข้อ → ต้องส่งต่อคลินิกโรคจากการทำงาน</summary>
    Public ReadOnly Property NeedsReferral As Boolean
        Get
            Return Injury OrElse MoreSymptoms OrElse DustPm
        End Get
    End Property

    Public ReadOnly Property PositiveCount As Integer
        Get
            Return If(Injury, 1, 0) + If(MoreSymptoms, 1, 0) + If(DustPm, 1, 0)
        End Get
    End Property

    Public ReadOnly Property AgeText As String
        Get
            Return If(AgeYears.HasValue, $"{AgeYears.Value} ปี", "-")
        End Get
    End Property

    Public Shared Function YesNo(value As Boolean) As String
        Return If(value, "ใช่", "ไม่ใช่")
    End Function
End Class

''' <summary>บัญชีผู้ใช้ HOSxP (ตาราง opduser)</summary>
Public Class HosUser
    Public Property LoginName As String = ""
    Public Property FullName As String = ""
    ''' <summary>opduser.groupname</summary>
    Public Property GroupName As String = ""
    ''' <summary>opduser.accessright</summary>
    Public Property AccessRight As String = ""
    ''' <summary>มีสิทธิ์ผู้ดูแลระบบ (กลุ่ม admin / อยู่ใน AdminLogins / accessright ตรงกับคำที่กำหนด)</summary>
    Public Property IsAdmin As Boolean

    Public ReadOnly Property DisplayName As String
        Get
            Return If(FullName.Trim() <> "", FullName.Trim(), LoginName)
        End Get
    End Property
End Class

''' <summary>ผลการตรวจสอบบัญชีผู้ใช้</summary>
Public Enum AuthStatus
    Success = 0
    ''' <summary>ไม่พบ loginname นี้ในตารางผู้ใช้</summary>
    UnknownUser
    ''' <summary>มีชื่อผู้ใช้ แต่รหัสผ่านไม่ตรง</summary>
    WrongPassword
    ''' <summary>มีชื่อผู้ใช้ แต่ยังไม่ได้ตั้งรหัสผ่านสำหรับโปรแกรมภายนอกใน HOSxP (คอลัมน์รหัสผ่านว่าง)</summary>
    PasswordNotSet
    ''' <summary>รหัสผ่านถูก แต่บัญชีไม่มีสิทธิ์ผู้ดูแลระบบ</summary>
    NotAdmin
    ''' <summary>ยังไม่ได้เปิด HOSxP บนเครื่องนี้ (โหมดใช้บัญชีที่ล็อกอิน HOSxP อยู่)</summary>
    NoHosxpSession
End Enum

Public Class AuthResult
    Public Property Status As AuthStatus = AuthStatus.UnknownUser
    Public Property User As HosUser
    ''' <summary>ข้อความอธิบายเพิ่ม (เช่น ชื่อกลุ่มของบัญชี) สำหรับแสดงให้ผู้ใช้วินิจฉัยปัญหา</summary>
    Public Property Detail As String = ""

    Public ReadOnly Property IsSuccess As Boolean
        Get
            Return Status = AuthStatus.Success
        End Get
    End Property

    Public Shared Function Fail(status As AuthStatus, Optional detail As String = "", Optional user As HosUser = Nothing) As AuthResult
        Return New AuthResult With {.Status = status, .Detail = detail, .User = user}
    End Function

    Public Shared Function Ok(user As HosUser) As AuthResult
        Return New AuthResult With {.Status = AuthStatus.Success, .User = user}
    End Function
End Class

''' <summary>สรุปยอดของทะเบียนคัดกรอง (ใช้กับการ์ดด้านบนของหน้าทะเบียน)</summary>
Public Class OccupationalSummary
    Public Property Total As Integer
    Public Property Referral As Integer
    Public Property Injury As Integer
    Public Property MoreSymptoms As Integer
    Public Property DustPm As Integer

    Public Shared Function FromRecords(rows As IEnumerable(Of OccupationalRecord)) As OccupationalSummary
        Dim s As New OccupationalSummary()
        For Each r In rows
            s.Total += 1
            If r.NeedsReferral Then s.Referral += 1
            If r.Injury Then s.Injury += 1
            If r.MoreSymptoms Then s.MoreSymptoms += 1
            If r.DustPm Then s.DustPm += 1
        Next
        Return s
    End Function
End Class

''' <summary>
''' การตั้งค่าการแสดงผลต่อเครื่อง (ตาราง app_storage, section = "scphNotify")
''' ชื่อ key เหมือนโปรแกรมเดิมทุกตัว
''' </summary>
Public Class DisplayPreferences
    Public Const Section As String = "scphNotify"

    Public Property ShowDmColor As Boolean = True
    Public Property ShowHtColor As Boolean = True
    Public Property ShowCaseHd As Boolean = True
    Public Property ShowEgfr As Boolean = True
    Public Property ShowScreening As Boolean = True
    Public Property ShowNextAppointment As Boolean = True
    Public Property ShowFerritin As Boolean = True
    ''' <summary>เปิด/ปิดระบบคัดกรองโรคจากการทำงาน (หน้าต่างคัดกรองอัตโนมัติ + เมนูทะเบียน)</summary>
    Public Property EnableOccupational As Boolean = True

    Public Shared ReadOnly Keys As String() = {"cbDmColor", "cbHtColor", "cbCaseHd", "cbEgfr", "cbScreenEyeNepFoot", "cbNextOapp", "cbFerritin", "cbOccupational"}

    Public Function GetValue(key As String) As Boolean
        Select Case key
            Case "cbDmColor" : Return ShowDmColor
            Case "cbHtColor" : Return ShowHtColor
            Case "cbCaseHd" : Return ShowCaseHd
            Case "cbEgfr" : Return ShowEgfr
            Case "cbScreenEyeNepFoot" : Return ShowScreening
            Case "cbNextOapp" : Return ShowNextAppointment
            Case "cbFerritin" : Return ShowFerritin
            Case "cbOccupational" : Return EnableOccupational
        End Select
        Return True
    End Function

    Public Sub SetValue(key As String, value As Boolean)
        Select Case key
            Case "cbDmColor" : ShowDmColor = value
            Case "cbHtColor" : ShowHtColor = value
            Case "cbCaseHd" : ShowCaseHd = value
            Case "cbEgfr" : ShowEgfr = value
            Case "cbScreenEyeNepFoot" : ShowScreening = value
            Case "cbNextOapp" : ShowNextAppointment = value
            Case "cbFerritin" : ShowFerritin = value
            Case "cbOccupational" : EnableOccupational = value
        End Select
    End Sub

    ''' <summary>ค่าที่ไม่มีในฐานข้อมูล (หรือเป็น "0" แบบโปรแกรมเดิม) ถือว่า "แสดง"; ซ่อนเมื่อค่าเป็น "False" เท่านั้น</summary>
    Public Shared Function ParseStored(value As String) As Boolean
        Return Not String.Equals(If(value, "").Trim(), "False", StringComparison.OrdinalIgnoreCase)
    End Function
End Class

'──────────────── HD/CAPD Care ────────────────

''' <summary>ชนิดการบำบัดทดแทนไต</summary>
Public Enum DialysisModality
    Unknown = 0
    ''' <summary>ฟอกเลือดด้วยเครื่องไตเทียม</summary>
    Hd
    ''' <summary>ล้างไตทางช่องท้อง (CAPD / APD)</summary>
    Pd
End Enum

''' <summary>
''' รอบการล้างไต 1 ครั้ง — อนุมานจากรายการค่าบริการใน opitemrece (nondrugitems)
''' HOSxP ไม่ได้เก็บค่าทางคลินิกของรอบฟอก (UF, BFR, น้ำหนักก่อน/หลัง) จึงมีแค่วันที่กับชื่อรายการ
''' </summary>
Public Class DialysisSession
    Public Property SessionDate As Date
    Public Property ItemName As String = ""
    Public Property Modality As DialysisModality
End Class

''' <summary>น้ำหนัก/ความดัน 1 ครั้งที่จุดคัดกรอง (ovst + opdscreen)</summary>
Public Class VitalPoint
    Public Property VisitDate As Date
    Public Property BodyWeight As Double?
    Public Property Systolic As Double?
    Public Property Diastolic As Double?
    Public Property Pulse As Double?

    Public ReadOnly Property BpText As String
        Get
            If Not Systolic.HasValue OrElse Not Diastolic.HasValue Then Return "-"
            Return $"{Systolic.Value:0}/{Diastolic.Value:0}"
        End Get
    End Property
End Class

''' <summary>ยาที่ผู้ป่วยเคยได้รับ (ชื่อยา + วันที่สั่งล่าสุด + จำนวนครั้ง)</summary>
Public Class DrugUsage
    Public Property DrugName As String = ""
    Public Property LastDate As Date?
    Public Property Times As Integer
End Class

''' <summary>1 แถวของตาราง "ผล LAB สำคัญ" — ค่าล่าสุดเทียบกับค่าก่อนหน้า</summary>
Public Class KeyLabRow
    Public Property LabName As String = ""
    Public Property Latest As String = ""
    Public Property LatestDate As Date?
    Public Property Previous As String = ""
    Public Property PreviousDate As Date?
    Public Property Target As String = ""
    Public Property Level As AlertLevel = AlertLevel.Neutral

    ''' <summary>ลูกศรแนวโน้มเทียบครั้งก่อน (ใช้ค่าตัวเลขเท่านั้น)</summary>
    Public Property Trend As Integer

    Public ReadOnly Property TrendText As String
        Get
            Select Case Trend
                Case > 0 : Return ChrW(9650)      ' ▲
                Case < 0 : Return ChrW(9660)      ' ▼
                Case Else : Return ""
            End Select
        End Get
    End Property

    Public ReadOnly Property HasValue As Boolean
        Get
            Return Latest <> ""
        End Get
    End Property
End Class

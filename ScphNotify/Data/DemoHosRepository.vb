''' <summary>
''' ข้อมูลตัวอย่าง (ไม่เชื่อมต่อฐานข้อมูล) — เปิดด้วย ScphNotify.exe --demo
''' ใช้สำหรับดู/ปรับหน้าจอ หรือสาธิตโปรแกรม ข้อมูลทั้งหมดเป็นข้อมูลสมมติ
''' </summary>
Public Class DemoHosRepository
    Implements IHosRepository

    Private ReadOnly _storage As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
    Private ReadOnly _screened As New HashSet(Of String)
    Private ReadOnly _rnd As New Random(26)

    Public Property Occupational As Boolean = False
    ''' <summary>จำลองสถานะ "ยังไม่มีผู้ป่วยถูกเรียก"</summary>
    Public Property SimulateNoPatient As Boolean = False

    Public ReadOnly Property Description As String Implements IHosRepository.Description
        Get
            Return "โหมดสาธิต (ข้อมูลสมมติ)"
        End Get
    End Property

    Public ReadOnly Property IsDemo As Boolean Implements IHosRepository.IsDemo
        Get
            Return True
        End Get
    End Property

    Private Shared Function Delay(Of T)(value As T) As Task(Of T)
        Return Task.Delay(40).ContinueWith(Function(x) value)
    End Function

    Private Shared Function ThaiShort(d As Date) As String
        Return ThaiDate.ShortDate(d)
    End Function

    Public Function TestConnectionAsync() As Task Implements IHosRepository.TestConnectionAsync
        Return Task.Delay(300)
    End Function

    Public Function GetLockedVnAsync(computerName As String) As Task(Of String) Implements IHosRepository.GetLockedVnAsync
        Return Delay(If(SimulateNoPatient, "", "690918083015"))
    End Function

    Public Function GetPatientByVnAsync(vn As String) As Task(Of PatientHeader) Implements IHosRepository.GetPatientByVnAsync
        Return Delay(New PatientHeader With {.Hn = "000482917", .Name = "นายสมชาย ใจดีมาก", .AgeYears = 58})
    End Function

    Private Shared ReadOnly DemoPatients As PatientSearchResult() = {
        New PatientSearchResult With {.Hn = "000482917", .Name = "นายสมชาย ใจดีมาก", .Cid = "3100500123456", .AgeYears = 58, .LastVisitDate = Date.Today},
        New PatientSearchResult With {.Hn = "000264620", .Name = "น.ส.ประภาพร ดอกไม้", .Cid = "3100500223451", .AgeYears = 41, .LastVisitDate = Date.Today.AddDays(-23)},
        New PatientSearchResult With {.Hn = "000264606", .Name = "นางมาลี ศรีสุข", .Cid = "3100500334512", .AgeYears = 63, .LastVisitDate = Date.Today.AddDays(-5)},
        New PatientSearchResult With {.Hn = "000264626", .Name = "นายวิทยา คงทน", .Cid = "3100500445123", .AgeYears = 47, .LastVisitDate = Date.Today.AddMonths(-7)},
        New PatientSearchResult With {.Hn = "000264653", .Name = "นายเอกชัย ปิ่นทอง", .Cid = "3100500551234", .AgeYears = 52, .LastVisitDate = Nothing}}

    Public Function SearchPatientsAsync(keyword As String, maxRows As Integer) As Task(Of List(Of PatientSearchResult)) Implements IHosRepository.SearchPatientsAsync
        Dim q = If(keyword, "").Trim()
        If q = "" Then Return Delay(New List(Of PatientSearchResult))
        Dim digits = New String(q.Where(AddressOf Char.IsDigit).ToArray())
        Dim hnList = MySqlHosRepository.HnCandidates(digits)
        Dim digitsOnly = digits.Length > 0 AndAlso digits.Length = q.Length
        Return Delay(DemoPatients.Where(Function(p) hnList.Contains(p.Hn) OrElse
                                            (digits.Length = 13 AndAlso p.Cid = digits) OrElse
                                            (Not digitsOnly AndAlso p.Name.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)).
                                  Take(Math.Max(1, maxRows)).ToList())
    End Function

    Public Function GetPatientByHnAsync(hn As String) As Task(Of PatientHeader) Implements IHosRepository.GetPatientByHnAsync
        Dim p = DemoPatients.FirstOrDefault(Function(x) x.Hn = hn)
        If p Is Nothing Then Return Delay(Of PatientHeader)(Nothing)
        Return Delay(New PatientHeader With {.Hn = p.Hn, .Name = p.Name, .AgeYears = p.AgeYears})
    End Function

    Public Function GetLastVisitDateAsync(hn As String) As Task(Of Date?) Implements IHosRepository.GetLastVisitDateAsync
        Dim p = DemoPatients.FirstOrDefault(Function(x) x.Hn = hn)
        Return Delay(If(p Is Nothing, Nothing, p.LastVisitDate))
    End Function

    Public Function GetTodayVnAsync(hn As String) As Task(Of String) Implements IHosRepository.GetTodayVnAsync
        Dim p = DemoPatients.FirstOrDefault(Function(x) x.Hn = hn)
        Return Delay(If(p IsNot Nothing AndAlso p.VisitedToday, "690918083015", ""))
    End Function

    Public Function GetClinicCodeAsync(sysName As String) As Task(Of String) Implements IHosRepository.GetClinicCodeAsync
        Return Delay(If(sysName = "dm_clinic_code", "001", "002"))
    End Function

    Public Function GetScreeningTextAsync(hn As String) As Task(Of String) Implements IHosRepository.GetScreeningTextAsync
        Return Delay("ยังไม่ตรวจ: ตา (ครั้งล่าสุด 12 ก.ค.67), เท้า (ครั้งล่าสุด 3 มิ.ย.67), ")
    End Function

    Public Function GetEgfrBelow60Async(hn As String) As Task(Of List(Of String)) Implements IHosRepository.GetEgfrBelow60Async
        Dim today = Date.Today
        Return Delay(New List(Of String) From {
            $"52.87 ({ThaiShort(today.AddDays(-14))})",
            $"55.10 ({ThaiShort(today.AddMonths(-3))})",
            $"58.42 ({ThaiShort(today.AddMonths(-7))})"})
    End Function

    Public Function GetFerritinAsync(hn As String) As Task(Of List(Of String)) Implements IHosRepository.GetFerritinAsync
        Dim today = Date.Today
        Return Delay(New List(Of String) From {
            $"212.4 ({ThaiShort(today.AddMonths(-1))})",
            $"256.0 ({ThaiShort(today.AddMonths(-5))})"})
    End Function

    Public Function GetCaseHdAsync(hn As String) As Task(Of String) Implements IHosRepository.GetCaseHdAsync
        Return Delay("Case HD")
    End Function

    Public Function HasDmDiagnosisAsync(hn As String) As Task(Of Boolean) Implements IHosRepository.HasDmDiagnosisAsync
        Return Delay(True)
    End Function

    Public Function HasHtDiagnosisAsync(hn As String) As Task(Of Boolean) Implements IHosRepository.HasHtDiagnosisAsync
        Return Delay(True)
    End Function

    Public Function IsClinicMemberAsync(hn As String, clinic As String) As Task(Of Boolean) Implements IHosRepository.IsClinicMemberAsync
        Return Delay(clinic <> "001")   ' ยังไม่ลงทะเบียนคลินิก DM
    End Function

    Public Function GetDmQualifyAsync(hn As String, index As Integer) As Task(Of String) Implements IHosRepository.GetDmQualifyAsync
        Return Delay($"dm_qualify_0{index}")
    End Function

    Public Function GetHtQualify01Async(hn As String) As Task(Of String) Implements IHosRepository.GetHtQualify01Async
        Return Delay("not_in_ht_qualify_01")
    End Function

    Public Function GetNextAppointmentsAsync(hn As String) As Task(Of List(Of String)) Implements IHosRepository.GetNextAppointmentsAsync
        Dim list As New List(Of String)
        Dim deps = {"คลินิกโรคไต", "คลินิกเบาหวาน", "ห้องตรวจอายุรกรรม"}
        For i = 0 To 2
            Dim d = Date.Today.AddDays(9 + i * 23)
            list.Add($"{d.Day}/{d.Month}/{d.Year + 543} ({deps(i)})")
        Next
        Return Delay(list)
    End Function

    Public Function GetCvdRiskAsync(hn As String) As Task(Of List(Of CvdRiskEntry)) Implements IHosRepository.GetCvdRiskAsync
        Dim scores = {8.4, 9.6, 12.1, 13.8, 15.2, 18.9, 21.4, 24.7}
        Dim list As New List(Of CvdRiskEntry)
        For i = 0 To scores.Length - 1
            Dim d = Date.Today.AddMonths(-(scores.Length - 1 - i) * 3).AddDays(-2)
            list.Add(New CvdRiskEntry With {
                .VisitDate = d, .Score = scores(i), .Bps = 128 + i * 3 + _rnd.Next(0, 6),
                .LabText = $"Chol:{188 + i * 4}.00  LDL:{112 + i * 3}.00 HDL: {48 - i}.00",
                .IsDm = True, .IsSmoker = i < 5, .Age = 56 + i \ 4})
        Next
        list.Reverse()
        Return Delay(list)
    End Function

    Public Function GetBuddhistYearsAsync() As Task(Of List(Of Integer)) Implements IHosRepository.GetBuddhistYearsAsync
        Dim y = Date.Today.Year + 543
        Return Delay(Enumerable.Range(y - 9, 10).ToList())
    End Function

    Private Shared ReadOnly LabNames As String() = {"BUN", "Cholesterol", "Creatinine", "eGFR", "FBS", "Ferritin", "HbA1C", "HDL", "Hemoglobin", "LDL", "Potassium", "Triglyceride"}

    Public Function GetLabItemNamesAsync(search As String, hn As String) As Task(Of List(Of String)) Implements IHosRepository.GetLabItemNamesAsync
        Dim s = If(search, "").Trim()
        Return Delay(LabNames.Where(Function(n) s = "" OrElse n.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0).ToList())
    End Function

    Private Function Series(labName As String, count As Integer) As List(Of LabPoint)
        Dim baseValue As Double, spread As Double, trend As Double
        Select Case labName.ToUpperInvariant()
            Case "HBA1C" : baseValue = 8.6 : spread = 0.4 : trend = -0.12
            Case "EGFR" : baseValue = 68 : spread = 2.5 : trend = -1.1
            Case "CREATININE" : baseValue = 1.1 : spread = 0.06 : trend = 0.03
            Case "FBS" : baseValue = 158 : spread = 12 : trend = -2.5
            Case "LDL" : baseValue = 142 : spread = 8 : trend = -3
            Case "FERRITIN" : baseValue = 280 : spread = 20 : trend = -4
            Case Else : baseValue = 100 : spread = 8 : trend = 0.5
        End Select
        Dim list As New List(Of LabPoint)
        Dim rnd As New Random(labName.Length * 7)
        For i = 0 To count - 1
            Dim v = baseValue + trend * i + (rnd.NextDouble() - 0.5) * 2 * spread
            list.Add(New LabPoint With {.LabName = labName, .ReportDate = Date.Today.AddDays(-(count - 1 - i) * 45 - 3), .ResultText = v.ToString(If(v < 20, "0.00", "0.0"), Globalization.CultureInfo.InvariantCulture)})
        Next
        list.Reverse()   ' ล่าสุดก่อน (เหมือน ORDER BY DESC)
        Return list
    End Function

    Public Function GetLabResultsAsync(hn As String, yearFrom As Integer, yearTo As Integer, labName As String) As Task(Of List(Of LabResultRow)) Implements IHosRepository.GetLabResultsAsync
        Dim normal = If(labName.ToUpperInvariant() = "HBA1C", "4.0 - 6.4", If(labName.ToUpperInvariant() = "EGFR", "> 90", "-"))
        Dim rows = Series(labName, 14).Select(Function(p, i) New LabResultRow With {
            .OrderNumber = (740215 - i * 37).ToString(),
            .ReportDate = p.ReportDate,
            .ItemName = labName,
            .Result = p.ResultText,
            .NormalValue = normal,
            .Flag = If(i Mod 3 = 0, "H", ""),
            .SubGroup = "Chemistry"}).ToList()
        Return Delay(rows)
    End Function

    Public Function GetLabChartAsync(hn As String, yearFrom As Integer, yearTo As Integer, labName As String) As Task(Of List(Of LabPoint)) Implements IHosRepository.GetLabChartAsync
        Return Delay(Series(labName, 14))
    End Function

    Public Function GetLabHistoryAsync(hn As String, labNames As IList(Of String)) As Task(Of List(Of LabPoint)) Implements IHosRepository.GetLabHistoryAsync
        ' ใช้ชุดเดียวกับเทมเพลตฟอกไต ค่าจะได้สมจริงกับผู้ป่วยล้างไต (หน้า HD/CAPD Care ใช้ฟังก์ชันนี้)
        Dim all As New List(Of LabPoint)
        For Each n In labNames
            all.AddRange(TemplateSeries(n))
        Next
        Return Delay(all)
    End Function

    '── LAB Template Hemodialysis (ข้อมูลสมมติ) ──

    Private Function TemplateSeries(labName As String) As List(Of LabPoint)
        Dim list As New List(Of LabPoint)
        Dim rnd As New Random(labName.Length * 31 + labName.Length)
        Dim baseValue As Double, spread As Double
        Select Case labName.ToUpperInvariant()
            Case "HCT" : baseValue = 25 : spread = 2
            Case "HGB" : baseValue = 8.1 : spread = 0.7
            Case "WBC" : baseValue = 5200 : spread = 400
            Case "PLT.COUNT" : baseValue = 205000 : spread = 20000
            Case "BUN" : baseValue = 78 : spread = 6
            Case "CREATININE" : baseValue = 10.5 : spread = 0.9
            Case "EGFR" : baseValue = 5.0 : spread = 0.5
            Case "FERRITIN" : baseValue = 36.9 : spread = 4
            ' ค่าที่ใช้ในหน้า HD/CAPD Care — ตั้งให้สมจริงกับผู้ป่วยฟอกไต
            Case "K:POTASSIUM" : baseValue = 5.2 : spread = 0.6
            Case "NA:SODIUM" : baseValue = 138 : spread = 3
            Case "CL:CHLORIDE" : baseValue = 101 : spread = 3
            Case "ECO2:CARBONDIOXIDE" : baseValue = 21.5 : spread = 2
            Case "CA:CALCIUM" : baseValue = 8.8 : spread = 0.5
            Case "PO4:PHOSPHORUS" : baseValue = 5.9 : spread = 0.7
            Case "ALBUMIN" : baseValue = 3.4 : spread = 0.3
            Case "*PARATHYROID HORMONE" : baseValue = 420 : spread = 90
            Case Else : baseValue = 30 : spread = 5
        End Select
        ' ไม่ใช่ทุกรายการที่ตรวจทุกครั้ง (เหมือนของจริง ช่องจึงว่างบ้าง)
        Dim every = If(baseValue > 1000 OrElse labName.Length Mod 3 = 0, 1, 3)
        For i = 0 To 9
            If i Mod every <> 0 Then Continue For
            Dim v = baseValue + (rnd.NextDouble() - 0.5) * 2 * spread
            list.Add(New LabPoint With {
                .LabName = labName,
                .ReportDate = Date.Today.AddDays(-(9 - i) * 30),
                .ResultText = v.ToString(If(v < 100, "0.0", "0"), Globalization.CultureInfo.InvariantCulture)})
        Next
        Return list
    End Function

    Public Function GetLabTemplateDatesAsync(hn As String, labNames As IList(Of String), fromDate As Date?, toDate As Date?, maxDates As Integer) As Task(Of List(Of Date)) Implements IHosRepository.GetLabTemplateDatesAsync
        Dim all = If(labNames, New List(Of String)()).SelectMany(AddressOf TemplateSeries).
                     Where(Function(p) p.ReportDate.HasValue).Select(Function(p) p.ReportDate.Value.Date).
                     Where(Function(d) (Not fromDate.HasValue OrElse d >= fromDate.Value.Date) AndAlso
                                       (Not toDate.HasValue OrElse d <= toDate.Value.Date)).
                     Distinct().OrderByDescending(Function(d) d).Take(Math.Max(1, maxDates)).
                     OrderBy(Function(d) d).ToList()
        Return Delay(all)
    End Function

    Public Function GetLabTemplateResultsAsync(hn As String, labNames As IList(Of String), fromDate As Date, toDate As Date) As Task(Of List(Of LabPoint)) Implements IHosRepository.GetLabTemplateResultsAsync
        Dim rows = If(labNames, New List(Of String)()).SelectMany(AddressOf TemplateSeries).
                      Where(Function(p) p.ReportDate.HasValue AndAlso
                                        p.ReportDate.Value.Date >= fromDate.Date AndAlso
                                        p.ReportDate.Value.Date <= toDate.Date).ToList()
        Return Delay(rows)
    End Function

    '── HD/CAPD Care (ข้อมูลสมมติ) ──

    ''' <summary>สาธิตเป็นผู้ป่วย HD สัปดาห์ละ 3 ครั้ง (จันทร์ / พุธ / ศุกร์)</summary>
    Public Function GetDialysisSessionsAsync(hn As String, fromDate As Date,
                                             hdKeywords As IList(Of String), pdKeywords As IList(Of String)) _
                                             As Task(Of List(Of DialysisSession)) Implements IHosRepository.GetDialysisSessionsAsync
        Dim list As New List(Of DialysisSession)
        Dim d = Date.Today
        While d >= fromDate.Date
            If d.DayOfWeek = DayOfWeek.Monday OrElse d.DayOfWeek = DayOfWeek.Wednesday OrElse d.DayOfWeek = DayOfWeek.Friday Then
                list.Add(New DialysisSession With {
                    .SessionDate = d,
                    .ItemName = "ค่าบริการฟอกเลือดด้วยเครื่องไตเทียม (Hemodialysis)",
                    .Modality = DialysisModality.Hd})
            End If
            d = d.AddDays(-1)
        End While
        Return Delay(list)
    End Function

    Public Function GetVitalHistoryAsync(hn As String, fromDate As Date, maxRows As Integer) _
                                         As Task(Of List(Of VitalPoint)) Implements IHosRepository.GetVitalHistoryAsync
        Dim list As New List(Of VitalPoint)
        Dim rnd As New Random(2569)
        Dim d = Date.Today
        While d >= fromDate.Date AndAlso list.Count < maxRows
            list.Add(New VitalPoint With {
                .VisitDate = d,
                .BodyWeight = Math.Round(52.0 + (rnd.NextDouble() - 0.35) * 2.4, 1),
                .Systolic = Math.Round(135 + (rnd.NextDouble() - 0.5) * 26, 0),
                .Diastolic = Math.Round(80 + (rnd.NextDouble() - 0.5) * 16, 0),
                .Pulse = Math.Round(78 + (rnd.NextDouble() - 0.5) * 14, 0)})
            d = d.AddDays(-7)
        End While
        Return Delay(list)
    End Function

    Public Function GetDialysisDrugsAsync(hn As String, fromDate As Date, keywords As IList(Of String)) _
                                          As Task(Of List(Of DrugUsage)) Implements IHosRepository.GetDialysisDrugsAsync
        Dim list As New List(Of DrugUsage) From {
            New DrugUsage With {.DrugName = "EPOETIN ALFA 4000 IU INJ", .LastDate = Date.Today.AddDays(-4), .Times = 22},
            New DrugUsage With {.DrugName = "CALCIUM CARBONATE 1250 MG TAB", .LastDate = Date.Today.AddDays(-11), .Times = 6},
            New DrugUsage With {.DrugName = "FUROSEMIDE 40 MG TAB", .LastDate = Date.Today.AddDays(-11), .Times = 6},
            New DrugUsage With {.DrugName = "FOLIC ACID 5 MG TAB", .LastDate = Date.Today.AddDays(-11), .Times = 6},
            New DrugUsage With {.DrugName = "AMLODIPINE 10 MG TAB", .LastDate = Date.Today.AddDays(-39), .Times = 4},
            New DrugUsage With {.DrugName = "ALFACALCIDOL 0.25 MCG CAP", .LastDate = Date.Today.AddDays(-39), .Times = 3}}
        Return Delay(list)
    End Function

    Public Function GetStorageValueAsync(computerName As String, section As String, key As String) As Task(Of String) Implements IHosRepository.GetStorageValueAsync
        Dim v As String = Nothing
        _storage.TryGetValue($"{section}|{key}", v)
        Return Delay(v)
    End Function

    Public Function SaveStorageValueAsync(computerName As String, section As String, key As String, value As String) As Task(Of Boolean) Implements IHosRepository.SaveStorageValueAsync
        _storage($"{section}|{key}") = value
        Return Delay(True)
    End Function

    Public Function GetOccupationalTargetAsync(computerName As String) As Task(Of OccupationalTarget) Implements IHosRepository.GetOccupationalTargetAsync
        If Not Occupational Then Return Delay(Of OccupationalTarget)(Nothing)
        Return Delay(New OccupationalTarget With {.Hn = "000482917", .Vn = "690918083015", .Staff = "demo"})
    End Function

    Public Function HasOccupationalScreeningAsync(vn As String) As Task(Of Boolean) Implements IHosRepository.HasOccupationalScreeningAsync
        Return Delay(_screened.Contains(vn))
    End Function

    Public Function GetLoginOnComputerAsync(computerName As String) As Task(Of String) Implements IHosRepository.GetLoginOnComputerAsync
        Return Delay("demo")
    End Function

    Public Function SaveOccupationalScreeningAsync(vn As String, hn As String, injury As Boolean, moreSymptoms As Boolean, dustPm As Boolean, staff As String) As Task Implements IHosRepository.SaveOccupationalScreeningAsync
        _screened.Add(vn)
        Return Task.Delay(200)
    End Function

    Private Shared ReadOnly DemoNames As String() = {
        "นายสมชาย ใจดีมาก", "น.ส.ประภาพร ดอกไม้", "นางมาลี ศรีสุข", "นายวิทยา คงทน", "นายอนุชา แสงทอง",
        "น.ส.กนกวรรณ พูนผล", "นางสุดา ทองแท้", "นายเอกชัย ปิ่นทอง", "น.ส.ฐิติมา ชื่นบาน", "นายพิชัย รุ่งเรือง"}
    Private Shared ReadOnly DemoStaff As String() = {"somchai", "pranee", "wipada", "admin"}

    Public Function GetOccupationalRegistryAsync(fromDate As Date, toDate As Date, search As String, maxRows As Integer) As Task(Of List(Of OccupationalRecord)) Implements IHosRepository.GetOccupationalRegistryAsync
        Dim rnd As New Random(2569)
        Dim all As New List(Of OccupationalRecord)
        Dim days = Math.Max(1, CInt((toDate.Date - fromDate.Date).TotalDays) + 1)
        For d = 0 To days - 1
            Dim day = fromDate.Date.AddDays(d)
            If day > Date.Today Then Exit For
            For i = 1 To rnd.Next(0, 5)
                Dim idx = rnd.Next(0, DemoNames.Length)
                Dim inj = rnd.Next(0, 10) < 2
                Dim more = rnd.Next(0, 10) < 2
                Dim dust = rnd.Next(0, 10) < 3
                all.Add(New OccupationalRecord With {
                    .ScreenedAt = day.AddHours(8).AddMinutes(rnd.Next(0, 420)),
                    .Hn = (264600 + idx * 7 + d).ToString("000000000"),
                    .Vn = day.ToString("yyMMdd") & (80000 + rnd.Next(1000, 9999)).ToString(),
                    .PatientName = DemoNames(idx),
                    .AgeYears = 21 + (idx * 4 + d) Mod 38,
                    .Injury = inj, .MoreSymptoms = more, .DustPm = dust,
                    .Staff = DemoStaff(rnd.Next(0, DemoStaff.Length))})
            Next
        Next
        Dim q = If(search, "").Trim()
        Dim rows = all.Where(Function(r) q = "" OrElse
                                 r.Hn.Contains(q) OrElse r.Vn.Contains(q) OrElse
                                 r.PatientName.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                                 r.Staff.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0).
                         OrderByDescending(Function(r) r.ScreenedAt).Take(Math.Max(1, maxRows)).ToList()
        Return Delay(rows)
    End Function

    ''' <summary>โหมดสาธิต: admin/admin = ผู้ดูแลระบบ, demo/demo = ผู้ใช้ทั่วไป (ไม่มีสิทธิ์)</summary>
    Private Shared Function DemoUser(login As String) As HosUser
        Select Case If(login, "").Trim().ToLowerInvariant()
            Case "admin"
                Return New HosUser With {.LoginName = "admin", .FullName = "ผู้ดูแลระบบ (สาธิต)", .GroupName = "admin", .IsAdmin = True}
            Case "demo"
                Return New HosUser With {.LoginName = "demo", .FullName = "ผู้ใช้ทั่วไป (สาธิต)", .GroupName = "user", .IsAdmin = False}
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function GetHosUserAsync(loginName As String) As Task(Of HosUser) Implements IHosRepository.GetHosUserAsync
        Return Delay(DemoUser(loginName))
    End Function

    Public Function AuthenticateAsync(loginName As String, password As String) As Task(Of AuthResult) Implements IHosRepository.AuthenticateAsync
        Dim u = DemoUser(loginName)
        If u Is Nothing Then Return Delay(AuthResult.Fail(AuthStatus.UnknownUser))
        If password <> u.LoginName Then Return Delay(AuthResult.Fail(AuthStatus.WrongPassword, "", u))
        If Not u.IsAdmin Then Return Delay(AuthResult.Fail(AuthStatus.NotAdmin, u.GroupName, u))
        Return Delay(AuthResult.Ok(u))
    End Function

End Class

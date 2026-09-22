Imports MySqlConnector

''' <summary>
''' เข้าถึงฐานข้อมูล HOSxP ผ่าน MySqlConnector
''' SQL ทุกคำสั่งย้ายมาจาก classNcdClinic / classOvst / classPatient / classLab / classLogin / classOccupational
''' ของโปรแกรมเดิม — เปลี่ยนจากการต่อ string เป็น parameter (@hn ...) เพื่อป้องกัน SQL Injection
''' </summary>
Public Class MySqlHosRepository
    Implements IHosRepository

    Private ReadOnly _settings As DatabaseSettings
    Private ReadOnly _cs As String

    Public Sub New(settings As DatabaseSettings)
        _settings = settings.Clone()
        _cs = BuildConnectionString(_settings)
    End Sub

    Public Shared Function BuildConnectionString(s As DatabaseSettings) As String
        Dim b As New MySqlConnectionStringBuilder()
        b.Server = s.Server
        b.Port = CUInt(If(s.Port > 0, s.Port, 3306))
        b.Database = s.Database
        b.UserID = s.UserName
        b.Password = s.GetPassword()
        b.ConnectionTimeout = CUInt(Math.Max(3, s.ConnectTimeoutSeconds))
        b.DefaultCommandTimeout = CUInt(Math.Max(5, s.CommandTimeoutSeconds))
        b.SslMode = If(s.UseSsl, MySqlSslMode.Preferred, MySqlSslMode.None)
        b.ConvertZeroDateTime = True          ' HOSxP มีวันที่ 0000-00-00
        ' หมายเหตุ: MySqlConnector รับ-ส่งข้อความเป็น utf8mb4 เสมอ เซิร์ฟเวอร์จะแปลงคอลัมน์ tis620 ให้อัตโนมัติ
        Return b.ConnectionString
    End Function

    Public ReadOnly Property Description As String Implements IHosRepository.Description
        Get
            Return _settings.DisplayName
        End Get
    End Property

    Public ReadOnly Property IsDemo As Boolean Implements IHosRepository.IsDemo
        Get
            Return False
        End Get
    End Property

    '──────────────────────── Helpers ────────────────────────

    Private Shared Function P(name As String, value As Object) As MySqlParameter
        Return New MySqlParameter(name, If(value, DBNull.Value))
    End Function

    Private Async Function QueryAsync(Of T)(sql As String, map As Func(Of IDataRecord, T), ParamArray ps As MySqlParameter()) As Task(Of List(Of T))
        Dim list As New List(Of T)
        Try
            Using conn As New MySqlConnection(_cs)
                Await conn.OpenAsync().ConfigureAwait(False)
                Using cmd As New MySqlCommand(sql, conn)
                    If ps IsNot Nothing Then
                        For Each prm In ps
                            cmd.Parameters.Add(prm)
                        Next
                    End If
                    Using reader = Await cmd.ExecuteReaderAsync().ConfigureAwait(False)
                        While Await reader.ReadAsync().ConfigureAwait(False)
                            list.Add(map(reader))
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New HosQueryException(ex.Message, sql, ex)
        End Try
        Return list
    End Function

    Private Async Function ExecuteAsync(sql As String, ParamArray ps As MySqlParameter()) As Task(Of Integer)
        Try
            Using conn As New MySqlConnection(_cs)
                Await conn.OpenAsync().ConfigureAwait(False)
                Using cmd As New MySqlCommand(sql, conn)
                    For Each prm In ps
                        cmd.Parameters.Add(prm)
                    Next
                    Return Await cmd.ExecuteNonQueryAsync().ConfigureAwait(False)
                End Using
            End Using
        Catch ex As Exception
            Throw New HosQueryException(ex.Message, sql, ex)
        End Try
    End Function

    Private Async Function FirstStringAsync(sql As String, column As String, ParamArray ps As MySqlParameter()) As Task(Of String)
        Dim rows = Await QueryAsync(sql, Function(r) StrOf(r, column), ps).ConfigureAwait(False)
        Return If(rows.Count > 0, rows(0), "")
    End Function

    '──────────────────────── Connection ────────────────────────

    Public Async Function TestConnectionAsync() As Task Implements IHosRepository.TestConnectionAsync
        Try
            Using conn As New MySqlConnection(_cs)
                Await conn.OpenAsync().ConfigureAwait(False)
            End Using
        Catch ex As Exception
            Throw New HosQueryException(ex.Message, "(open connection)", ex)
        End Try
    End Function

    '──────────────────────── ผู้ป่วยที่ล็อกไว้ ────────────────────────

    ' classOvst.getDataLockVnByComputer
    Public Function GetLockedVnAsync(computerName As String) As Task(Of String) Implements IHosRepository.GetLockedVnAsync
        Return FirstStringAsync(
            "SELECT vn FROM `vn_lock` WHERE lock_computer = @computer LIMIT 1",
            "vn", P("@computer", computerName))
    End Function

    ' classOvst.getDataPateintByVn
    Public Async Function GetPatientByVnAsync(vn As String) As Task(Of PatientHeader) Implements IHosRepository.GetPatientByVnAsync
        Const sql As String =
            "SELECT CONCAT(p.pname, p.fname, ' ', p.lname) AS pt_name, o.hn, v.age_y
               FROM ovst o
               LEFT OUTER JOIN vn_stat v ON v.vn = o.vn
               LEFT OUTER JOIN patient p ON p.hn = o.hn
              WHERE o.vn = @vn
              LIMIT 1"
        Dim rows = Await QueryAsync(sql,
            Function(r) New PatientHeader With {
                .Hn = StrOf(r, "hn"),
                .Name = StrOf(r, "pt_name"),
                .AgeYears = IntOf(r, "age_y")
            }, P("@vn", vn)).ConfigureAwait(False)
        Return rows.FirstOrDefault()
    End Function

    '──────────────────────── ค้นหาผู้ป่วยเอง (โหมด Manual) ────────────────────────

    ''' <summary>
    ''' ค้นด้วยชื่อ / ชื่อ-สกุล / HN / เลขบัตรประชาชน (ไม่จำกัดว่าต้องมารับบริการวันนี้)
    '''  - HN  : พิมพ์ไม่ครบหลักได้ โปรแกรมจะเติม 0 นำหน้าให้เองทุกความยาวที่เป็นไปได้
    '''  - CID : ต้องครบ 13 หลักจึงจะนำมาค้น
    ''' </summary>
    Public Async Function SearchPatientsAsync(keyword As String, maxRows As Integer) As Task(Of List(Of PatientSearchResult)) Implements IHosRepository.SearchPatientsAsync
        Dim q = If(keyword, "").Trim()
        If q = "" Then Return New List(Of PatientSearchResult)

        Dim digits = New String(q.Where(AddressOf Char.IsDigit).ToArray())
        Dim digitsOnly = digits.Length > 0 AndAlso digits.Length = q.Length

        Dim ps As New List(Of MySqlParameter)
        Dim conds As New List(Of String)

        ' HN — เทียบแบบเท่ากับ (ใช้ index ได้) โดยลองทุกความยาวที่เติม 0 นำหน้า
        If digits <> "" Then
            Dim names As New List(Of String)
            Dim candidates = HnCandidates(digits)
            For i = 0 To candidates.Count - 1
                Dim nm = "@hn" & i.ToString(Globalization.CultureInfo.InvariantCulture)
                names.Add(nm)
                ps.Add(P(nm, candidates(i)))
            Next
            conds.Add($"p.hn IN ({String.Join(", ", names)})")
        End If

        ' เลขบัตรประชาชน — ต้องครบ 13 หลัก
        If digits.Length = 13 Then
            conds.Add("p.cid = @cid")
            ps.Add(P("@cid", digits))
        End If

        ' ชื่อ / ชื่อ-สกุล (ข้ามเมื่อผู้ใช้พิมพ์เป็นตัวเลขล้วน จะได้ไม่ต้องสแกนทั้งตาราง)
        If Not digitsOnly Then
            conds.Add("(p.fname LIKE @like OR p.lname LIKE @like" &
                      " OR CONCAT(p.fname, ' ', p.lname) LIKE @like" &
                      " OR CONCAT(p.pname, p.fname, ' ', p.lname) LIKE @like)")
            ps.Add(P("@like", "%" & q & "%"))
        End If

        If conds.Count = 0 Then Return New List(Of PatientSearchResult)

        Dim sql =
            "SELECT p.hn, p.cid, p.birthday,
                    CONCAT(p.pname, p.fname, ' ', p.lname) AS pt_name,
                    TIMESTAMPDIFF(YEAR, p.birthday, CURDATE()) AS age_y,
                    (SELECT MAX(o.vstdate) FROM ovst o WHERE o.hn = p.hn) AS last_visit
               FROM patient p
              WHERE " & String.Join(" OR ", conds) & "
              ORDER BY last_visit DESC, p.fname, p.lname
              LIMIT " & Math.Max(1, Math.Min(500, maxRows)).ToString(Globalization.CultureInfo.InvariantCulture)

        Return Await QueryAsync(sql,
            Function(r) New PatientSearchResult With {
                .Hn = StrOf(r, "hn"),
                .Cid = StrOf(r, "cid"),
                .Name = StrOf(r, "pt_name"),
                .Birthday = DateOf(r, "birthday"),
                .AgeYears = IntOf(r, "age_y"),
                .LastVisitDate = DateOf(r, "last_visit")
            }, ps.ToArray()).ConfigureAwait(False)
    End Function

    ''' <summary>
    ''' HN ที่เป็นไปได้จากตัวเลขที่ผู้ใช้พิมพ์ — เช่น "100407" จะได้ 100407, 0100407, 00100407, 000100407, ...
    ''' (HOSxP แต่ละโรงพยาบาลใช้ความยาว HN ไม่เท่ากัน จึงลองทุกความยาวจนถึง 13 หลัก)
    ''' </summary>
    Friend Shared Function HnCandidates(digits As String) As List(Of String)
        Dim list As New List(Of String)
        If String.IsNullOrEmpty(digits) Then Return list
        Dim core = digits.TrimStart("0"c)
        If core = "" Then core = "0"
        list.Add(digits)
        For length = core.Length To 13
            Dim padded = core.PadLeft(length, "0"c)
            If Not list.Contains(padded) Then list.Add(padded)
        Next
        Return list
    End Function

    Public Async Function GetPatientByHnAsync(hn As String) As Task(Of PatientHeader) Implements IHosRepository.GetPatientByHnAsync
        Const sql As String =
            "SELECT p.hn, CONCAT(p.pname, p.fname, ' ', p.lname) AS pt_name,
                    TIMESTAMPDIFF(YEAR, p.birthday, CURDATE()) AS age_y
               FROM patient p
              WHERE p.hn = @hn
              LIMIT 1"
        Dim rows = Await QueryAsync(sql,
            Function(r) New PatientHeader With {
                .Hn = StrOf(r, "hn"),
                .Name = StrOf(r, "pt_name"),
                .AgeYears = IntOf(r, "age_y")
            }, P("@hn", hn)).ConfigureAwait(False)
        Return rows.FirstOrDefault()
    End Function

    Public Async Function GetLastVisitDateAsync(hn As String) As Task(Of Date?) Implements IHosRepository.GetLastVisitDateAsync
        Dim rows = Await QueryAsync("SELECT MAX(vstdate) AS last_visit FROM ovst WHERE hn = @hn",
                                    Function(r) DateOf(r, "last_visit"), P("@hn", hn)).ConfigureAwait(False)
        Return If(rows.Count > 0, rows(0), Nothing)
    End Function

    Public Function GetTodayVnAsync(hn As String) As Task(Of String) Implements IHosRepository.GetTodayVnAsync
        Return FirstStringAsync("SELECT vn FROM ovst WHERE hn = @hn AND vstdate = CURDATE() ORDER BY vsttime DESC LIMIT 1",
                                "vn", P("@hn", hn))
    End Function

    '──────────────────────── Medical care ────────────────────────

    ' classNcdClinic.getDataHtDmClinicCode
    Public Function GetClinicCodeAsync(sysName As String) As Task(Of String) Implements IHosRepository.GetClinicCodeAsync
        Return FirstStringAsync("SELECT sys_value AS code FROM sys_var WHERE sys_name = @name", "code", P("@name", sysName))
    End Function

    ' classNcdClinic.getDataHtDmCheckLabCompleteByHn
    Public Function GetScreeningTextAsync(hn As String) As Task(Of String) Implements IHosRepository.GetScreeningTextAsync
        Return FirstStringAsync("SELECT HtDmCheckLabComplete(@hn) AS dd", "dd", P("@hn", hn))
    End Function

    ' classNcdClinic.getDataEfgrByHn
    Public Function GetEgfrBelow60Async(hn As String) As Task(Of List(Of String)) Implements IHosRepository.GetEgfrBelow60Async
        Const sql As String =
            "SELECT CONCAT(lo.lab_order_result, ' (', ThaiDateShort(lh.report_date), ')') AS eGFR
               FROM lab_order lo
               LEFT OUTER JOIN lab_head lh ON lh.lab_order_number = lo.lab_order_number
               LEFT OUTER JOIN lab_items li ON li.lab_items_code = lo.lab_items_code
              WHERE li.lab_items_name IN ('eGFR')
                AND li.lab_items_name IS NOT NULL
                AND lh.hn = @hn
                AND lo.lab_order_result < 60
                AND lh.report_date BETWEEN DATE(DATE_SUB(NOW(), INTERVAL 1 YEAR)) AND DATE(NOW())
              ORDER BY report_date DESC
              LIMIT 3"
        Return QueryNonEmptyStringsAsync(sql, "eGFR", P("@hn", hn))
    End Function

    ' classNcdClinic.getDataFerritinByHn
    Public Function GetFerritinAsync(hn As String) As Task(Of List(Of String)) Implements IHosRepository.GetFerritinAsync
        Const sql As String =
            "SELECT CONCAT(lo.lab_order_result, ' (', ThaiDateShort(lh.report_date), ')') AS ferritin
               FROM lab_order lo
               LEFT OUTER JOIN lab_head lh ON lh.lab_order_number = lo.lab_order_number
               LEFT OUTER JOIN lab_items li ON li.lab_items_code = lo.lab_items_code
              WHERE UPPER(li.lab_items_name) LIKE UPPER('%Ferritin%')
                AND li.lab_items_name IS NOT NULL
                AND lh.hn = @hn
              ORDER BY report_date DESC, report_time DESC
              LIMIT 3"
        Return QueryNonEmptyStringsAsync(sql, "ferritin", P("@hn", hn))
    End Function

    Private Async Function QueryNonEmptyStringsAsync(sql As String, column As String, ParamArray ps As MySqlParameter()) As Task(Of List(Of String))
        Dim rows = Await QueryAsync(sql, Function(r) StrOf(r, column).Trim(), ps).ConfigureAwait(False)
        Return rows.Where(Function(s) s <> "").ToList()
    End Function

    ' classNcdClinic.getDataChkHdCaseByHn
    Public Async Function GetCaseHdAsync(hn As String) As Task(Of String) Implements IHosRepository.GetCaseHdAsync
        Const sql As String =
            "SELECT IF((SELECT COUNT(i1.vstdate)
                          FROM opitemrece i1
                          LEFT OUTER JOIN nondrugitems n ON n.icode = i1.icode
                         WHERE n.`name` LIKE '%Hemodialysis%'
                           AND i1.hn = @hn) >= 1, 'Case HD',
                    IF((SELECT COUNT(i2.vstdate)
                          FROM opitemrece i2
                          LEFT OUTER JOIN nondrugitems n ON n.icode = i2.icode
                         WHERE n.`name` LIKE '%CAPD%'
                           AND i2.hn = @hn) >= 1, 'Case CAPD', ' ')) AS case_hd"
        Return (Await FirstStringAsync(sql, "case_hd", P("@hn", hn)).ConfigureAwait(False)).Trim()
    End Function

    ' classNcdClinic.getDataChkDmDxGreenByHn
    Public Async Function HasDmDiagnosisAsync(hn As String) As Task(Of Boolean) Implements IHosRepository.HasDmDiagnosisAsync
        Const sql As String =
            "SELECT 1 AS dm_dx FROM `ovstdiag`
              WHERE hn = @hn AND icd10 REGEXP '^(E1[1234][123456789])'
                AND vstdate BETWEEN DATE(DATE_SUB(NOW(), INTERVAL 1 YEAR)) AND DATE(NOW())
              ORDER BY vstdate DESC
              LIMIT 1"
        Return (Await FirstStringAsync(sql, "dm_dx", P("@hn", hn)).ConfigureAwait(False)) = "1"
    End Function

    ' classNcdClinic.getDataChkHtDxGreenByHn
    Public Async Function HasHtDiagnosisAsync(hn As String) As Task(Of Boolean) Implements IHosRepository.HasHtDiagnosisAsync
        Const sql As String =
            "SELECT 1 AS ht_dx FROM `ovstdiag`
              WHERE hn = @hn AND icd10 LIKE 'I10%'
                AND vstdate BETWEEN DATE(DATE_SUB(NOW(), INTERVAL 1 YEAR)) AND DATE(NOW())
              ORDER BY vstdate DESC
              LIMIT 1"
        Return (Await FirstStringAsync(sql, "ht_dx", P("@hn", hn)).ConfigureAwait(False)) = "1"
    End Function

    ' classNcdClinic.getDataChkRegisterByClinicByHn
    Public Async Function IsClinicMemberAsync(hn As String, clinic As String) As Task(Of Boolean) Implements IHosRepository.IsClinicMemberAsync
        Dim rows = Await QueryAsync("SELECT clinic FROM clinicmember WHERE hn = @hn AND clinic = @clinic LIMIT 1",
                                    Function(r) Not IsNullOf(r, "clinic"), P("@hn", hn), P("@clinic", clinic)).ConfigureAwait(False)
        Return rows.Count > 0 AndAlso rows(0)
    End Function

    ''' <summary>
    ''' พอร์ตจาก frmMain.getChkDmQualify01ByHn … getChkDmQualify05ByHn
    ''' คืนค่าข้อความชุดเดิมทุกตัว (ดูหมายเหตุใน ClinicalRules.vb)
    ''' </summary>
    Public Async Function GetDmQualifyAsync(hn As String, index As Integer) As Task(Of String) Implements IHosRepository.GetDmQualifyAsync
        Dim sql As String
        Dim column = $"qualify_0{index}"
        Dim hitValue = $"dm_qualify_0{index}"
        Dim nullValue As String
        Dim noRowValue As String

        Select Case index
            Case 1  ' classNcdClinic.getDataChkDmQualify01ByHn
                sql = "SELECT 'in_qualify_01' AS qualify_01 FROM lab_head lh
                         LEFT OUTER JOIN `lab_order` lo ON lo.lab_order_number = lh.lab_order_number
                         LEFT OUTER JOIN lab_items li ON li.lab_items_code = lo.lab_items_code
                         LEFT OUTER JOIN patient pt ON pt.hn = lh.hn
                        WHERE 1
                          AND li.lab_items_name LIKE '%HbA1C%'
                          AND lo.lab_order_result < 7
                          AND (lo.lab_order_result < 8 AND TIMESTAMPDIFF(YEAR, pt.birthday, NOW()) >= 75)
                          AND (lo.lab_order_result < 8 AND lh.hn IN (
                                SELECT DISTINCT hn FROM ovstdiag
                                 WHERE hn = lh.hn
                                   AND icd10 REGEXP '^(I2[012][0-9])|(I50)|(I6[0-9])|(N18[45])|(G4[01])'
                                   AND vstdate BETWEEN DATE(DATE_SUB(NOW(), INTERVAL 1 YEAR)) AND DATE(NOW())))
                          AND lh.hn IN (SELECT hn FROM `ovstdiag` WHERE 1
                                           AND hn = @hn
                                           AND icd10 REGEXP '^(E1[1234][123456789])'
                                           AND vstdate BETWEEN DATE(DATE_SUB(NOW(), INTERVAL 1 YEAR)) AND DATE(NOW())
                                         GROUP BY hn)
                        LIMIT 1"
                nullValue = "not_in_dm_qualify_01" : noRowValue = "not_in_qualify_01"
            Case 2  ' classNcdClinic.getDataChkHtDmQualify02ByHn
                sql = "SELECT IF(lo.lab_order_result <= 30.00, 'in_qualify_02', '') AS qualify_02, lo.lab_order_result
                         FROM lab_order lo
                         LEFT OUTER JOIN lab_head lh ON lh.lab_order_number = lo.lab_order_number
                         LEFT OUTER JOIN lab_items li ON li.lab_items_code = lo.lab_items_code
                        WHERE li.lab_items_name IN ('eGFR')
                          AND li.lab_items_name IS NOT NULL
                          AND lh.hn = @hn
                        ORDER BY report_date DESC, report_time DESC
                        LIMIT 1"
                nullValue = "not_in_dm_qualify_02" : noRowValue = "not_in_qualify_02"
            Case 3  ' classNcdClinic.getDataChkHtDmQualify03ByHn
                sql = "SELECT IF(icd10 <> NULL, 'in_qualify_03', '') AS qualify_03, icd10
                         FROM `ovstdiag`
                        WHERE 1
                          AND hn = @hn
                          AND icd10 REGEXP '^(I50[019])|(I48)|(N80[45])'
                          AND vstdate BETWEEN DATE(DATE_SUB(NOW(), INTERVAL 1 YEAR)) AND DATE(NOW())
                        ORDER BY vstdate DESC LIMIT 1"
                nullValue = "not_in_dm_qualify_03" : noRowValue = "not_in_dm_qualify_03"
            Case 4  ' classNcdClinic.getDataChkDmQualify04ByHn
                sql = "SELECT IF(i.icode, 'in_qualify_04', '') AS qualify_04, GROUP_CONCAT(d.name) AS d_name
                         FROM opitemrece i
                         LEFT OUTER JOIN drugitems d ON d.icode = i.icode
                        WHERE 1
                          AND i.hn = @hn
                          AND d.icode IN (SELECT icode FROM drugitems
                                           WHERE UPPER(name) LIKE UPPER('%TELMISARTAN%')
                                              OR UPPER(name) LIKE UPPER('%Manidipine%')
                                              OR UPPER(name) LIKE UPPER('%ATORVASTATIN%')
                                              OR UPPER(name) LIKE UPPER('%CARVEDILOL%')
                                              OR UPPER(name) LIKE UPPER('%BISOPROLOL%')
                                              OR UPPER(name) LIKE UPPER('%Gliclazide%')
                                              OR UPPER(name) LIKE UPPER('%Vildagliptin%')
                                              OR UPPER(name) LIKE UPPER('%HUMULIN-70/30 100%')
                                              OR UPPER(name) LIKE UPPER('%HUMULIN-N 100%')
                                              OR UPPER(name) LIKE UPPER('%INSULIN GARGINE%')
                                              OR UPPER(name) LIKE UPPER('%dapagliflozin%'))
                          AND vstdate BETWEEN DATE(DATE_SUB(NOW(), INTERVAL 1 YEAR)) AND DATE(NOW())"
                nullValue = "not_in_dm_qualify_04" : noRowValue = "not_in_dm_qualify_04"
            Case 5  ' classNcdClinic.getDataChkDmQualify05ByHn
                sql = "SELECT 'not_in_qualify_05' AS qualify_05"
                nullValue = "not_in_qualify_05" : noRowValue = "not_in_dm_qualify_05"
            Case Else
                Throw New ArgumentOutOfRangeException(NameOf(index))
        End Select

        Dim rows = Await QueryAsync(sql, Function(r) Not IsNullOf(r, column), P("@hn", hn)).ConfigureAwait(False)
        If rows.Count = 0 Then Return noRowValue
        Return If(rows(0), hitValue, nullValue)
    End Function

    ' frmMain.getChkHtQualify01ByHn + classNcdClinic.getDataChkHtQualify01ByHn (ตรวจค่า BP 2 ครั้ง: offset 0 และ 2 ตามโค้ดเดิม)
    Public Async Function GetHtQualify01Async(hn As String) As Task(Of String) Implements IHosRepository.GetHtQualify01Async
        Const sql As String =
            "SELECT IF(bps <= 140, IF(bpd <= 90, 1, 0), 0) AS qualify_01 FROM opdscreen
              WHERE 1 AND hn = @hn ORDER BY vn DESC LIMIT @offset, 1"
        Dim pass1 = Await QueryAsync(sql, Function(r) IntOf(r, "qualify_01").GetValueOrDefault(), P("@hn", hn), P("@offset", 0)).ConfigureAwait(False)
        Dim pass2 = Await QueryAsync(sql, Function(r) IntOf(r, "qualify_01").GetValueOrDefault(), P("@hn", hn), P("@offset", 2)).ConfigureAwait(False)
        Dim ok1 = pass1.Count > 0 AndAlso pass1(0) = 1
        Dim ok2 = pass2.Count > 0 AndAlso pass2(0) = 1
        Return If(ok1 AndAlso ok2, "ht_qualify_01", "not_in_ht_qualify_01")
    End Function

    ' classPatient.getDataNextOappDateByHn
    Public Function GetNextAppointmentsAsync(hn As String) As Task(Of List(Of String)) Implements IHosRepository.GetNextAppointmentsAsync
        Const sql As String =
            "SELECT CONCAT(SUBSTR(oa.nextdate, 9, 2) * 1, '/', SUBSTR(oa.nextdate, 6, 2) * 1, '/', LEFT(oa.nextdate, 4) + 543,
                           ' (', IFNULL(d.department, '-'), ')') AS next_date
               FROM oapp oa
               LEFT OUTER JOIN kskdepartment d ON d.depcode = oa.depcode
              WHERE oa.hn = @hn
                AND nextdate > DATE(NOW())
              ORDER BY nextdate"
        Return QueryNonEmptyStringsAsync(sql, "next_date", P("@hn", hn))
    End Function

    ''' <summary>
    ''' classNcdClinic.getDataCvdRiskByHn
    ''' แก้ไข: เงื่อนไข CASE เดิมเขียนแบบ "10 &lt;= score &lt; 20" ซึ่ง MySQL ตีความผิด (ทุกค่าที่ ≥10 กลายเป็น Yellow)
    ''' จึงเปลี่ยนเป็น "score &gt;= 10 AND score &lt; 20" ให้ตรงกับตารางสีบนหน้าจอ
    ''' </summary>
    Public Function GetCvdRiskAsync(hn As String) As Task(Of List(Of CvdRiskEntry)) Implements IHosRepository.GetCvdRiskAsync
        Const sql As String =
            "SELECT CASE
                        WHEN score < 10 THEN '1-Green'
                        WHEN score >= 10 AND score < 20 THEN '2-Yellow'
                        WHEN score >= 20 AND score < 30 THEN '3-Orange'
                        WHEN score >= 30 AND score < 40 THEN '4-Red'
                        WHEN score >= 40 THEN '5-Maroon'
                        ELSE 'Not Color'
                    END AS color,
                    z.*
               FROM (
                    SELECT ThaiCVRiskCal(a.age, a.sex, a.dm, a.smoking, a.bps, a.cholesterol, a.hdl, a.ldl, a.waist, a.height) AS score,
                           a.vstdate, a.bps,
                           CONCAT('Chol:', IFNULL(ROUND(a.cholesterol, 2), '-'), ' ', ' LDL:', IFNULL(ROUND(a.ldl, 2), '-'), ' ', 'HDL: ', IFNULL(ROUND(a.hdl, 2), '-')) AS lab,
                           IF(a.dm = 1, 'Y', 'N') AS ncd,
                           IF(a.smoking = 1, 'Y', 'N') AS smoke,
                           a.age
                      FROM (
                            SELECT v.vstdate,
                                   v.age_y AS age,
                                   IF(v.sex = 1, 1, 0) AS sex,
                                   IF(c.clinic = 'dm+', 1, 0) AS dm,
                                   IF(s.smoking_type_id IN (2, 3), 1, 0) AS smoking,
                                   s.bps,
                                   s.tc AS cholesterol,
                                   s.hdl,
                                   s.ldl,
                                   s.waist,
                                   s.height
                              FROM vn_stat AS v
                             INNER JOIN opdscreen AS s ON (v.vn = s.vn)
                             INNER JOIN clinic_visit AS c ON (v.vn = c.vn)
                             WHERE v.hn = @hn
                             GROUP BY v.vstdate, v.vn
                             ORDER BY v.vstdate DESC
                             LIMIT @limit
                           ) AS a
                    ) AS z
              WHERE lab NOT IN ('Chol:-  LDL:- HDL: -', 'Chol:0.00  LDL:0.00 HDL: 0.00')"
        Return QueryAsync(sql,
            Function(r) New CvdRiskEntry With {
                .VisitDate = DateOf(r, "vstdate"),
                .Score = DblOf(r, "score"),
                .Bps = DblOf(r, "bps"),
                .LabText = StrOf(r, "lab"),
                .IsDm = StrOf(r, "ncd") = "Y",
                .IsSmoker = StrOf(r, "smoke") = "Y",
                .Age = IntOf(r, "age")
            }, P("@hn", hn), P("@limit", 10))
    End Function

    '──────────────────────── LAB ────────────────────────

    ' classLab.getDataLastYear
    Public Function GetBuddhistYearsAsync() As Task(Of List(Of Integer)) Implements IHosRepository.GetBuddhistYearsAsync
        Const sql As String =
            "SELECT YEAR(vstdate) + 543 AS nyear
               FROM ovst
              WHERE vstdate IS NOT NULL
                AND vstdate > DATE_SUB(NOW(), INTERVAL @last - 1 YEAR)
              GROUP BY YEAR(vstdate)
              ORDER BY nyear ASC
              LIMIT @last"
        Return QueryAsync(sql, Function(r) IntOf(r, "nyear").GetValueOrDefault(), P("@last", 10))
    End Function

    ' classLab.getDataLabList
    Public Function GetLabItemNamesAsync(search As String, hn As String) As Task(Of List(Of String)) Implements IHosRepository.GetLabItemNamesAsync
        Dim sql = "SELECT li.lab_items_name
                     FROM `lab_items` li
                     LEFT OUTER JOIN lab_order lo ON lo.lab_items_code = li.lab_items_code
                     LEFT OUTER JOIN lab_head lh ON lh.lab_order_number = lo.lab_order_number
                    WHERE 1
                      AND li.lab_items_group NOT IN ('04')"
        Dim ps As New List(Of MySqlParameter)
        If Not String.IsNullOrWhiteSpace(search) Then
            sql &= " AND UPPER(lab_items_name) LIKE UPPER(@search)"
            ps.Add(P("@search", "%" & search.Trim() & "%"))
        End If
        If Not String.IsNullOrWhiteSpace(hn) Then
            sql &= " AND lh.hn = @hn"
            ps.Add(P("@hn", hn))
        End If
        sql &= " GROUP BY li.lab_items_name ORDER BY li.lab_items_name"
        Return QueryNonEmptyStringsAsync(sql, "lab_items_name", ps.ToArray())
    End Function

    ' classLab.getDataLabByHnByLabName
    Public Function GetLabResultsAsync(hn As String, yearFrom As Integer, yearTo As Integer, labName As String) As Task(Of List(Of LabResultRow)) Implements IHosRepository.GetLabResultsAsync
        Const sql As String =
            "SELECT lh.lab_order_number, lh.report_date, lo.lab_items_name_ref, lo.lab_order_result, lo.lab_items_normal_value_ref,
                    (SELECT s.lab_result_status_code
                       FROM hosxp_gateway.lab_order lo2
                       LEFT OUTER JOIN hosxp_gateway.lab_result_status s ON s.lab_result_status_id = lo2.lab_result_status
                      WHERE 1
                        AND lo2.lab_order_number = lo.lab_order_number
                        AND lo2.lab_items_code = lo.lab_items_code LIMIT 1) AS flag,
                    (SELECT lab_items_sub_group_name FROM `lab_items_sub_group`
                      WHERE lab_items_sub_group_code = lo.lab_items_sub_group_code LIMIT 1) AS sub_group_name
               FROM lab_order lo
               LEFT OUTER JOIN lab_head lh ON lh.lab_order_number = lo.lab_order_number
              WHERE 1
                AND lh.hn = @hn
                AND YEAR(receive_date) + 543 BETWEEN @y1 AND @y2
                AND UPPER(lo.lab_items_name_ref) = UPPER(@lab)
              ORDER BY lo.lab_order_number DESC"
        Return QueryAsync(sql,
            Function(r) New LabResultRow With {
                .OrderNumber = StrOf(r, "lab_order_number"),
                .ReportDate = DateOf(r, "report_date"),
                .ItemName = StrOf(r, "lab_items_name_ref"),
                .Result = StrOf(r, "lab_order_result"),
                .NormalValue = StrOf(r, "lab_items_normal_value_ref"),
                .Flag = StrOf(r, "flag"),
                .SubGroup = StrOf(r, "sub_group_name")
            }, P("@hn", hn), P("@y1", yearFrom), P("@y2", yearTo), P("@lab", labName))
    End Function

    ' classLab.getDataChartLabByHnByLabName
    Public Function GetLabChartAsync(hn As String, yearFrom As Integer, yearTo As Integer, labName As String) As Task(Of List(Of LabPoint)) Implements IHosRepository.GetLabChartAsync
        Const sql As String =
            "SELECT lo.lab_items_name_ref AS lab_name, lh.report_date, lo.lab_order_result
               FROM lab_order lo
               LEFT OUTER JOIN lab_head lh ON lh.lab_order_number = lo.lab_order_number
              WHERE 1
                AND lh.hn = @hn
                AND YEAR(receive_date) + 543 BETWEEN @y1 AND @y2
                AND UPPER(lo.lab_items_name_ref) = UPPER(@lab)
              ORDER BY lo.lab_order_number DESC
              LIMIT 20"
        Return QueryAsync(Of LabPoint)(sql, AddressOf MapLabPoint, P("@hn", hn), P("@y1", yearFrom), P("@y2", yearTo), P("@lab", labName))
    End Function

    ''' <summary>ใหม่: ใช้กับหน้า LAB Crosstab (ของเดิมยังทำไม่เสร็จ)</summary>
    Public Function GetLabHistoryAsync(hn As String, labNames As IList(Of String)) As Task(Of List(Of LabPoint)) Implements IHosRepository.GetLabHistoryAsync
        If labNames Is Nothing OrElse labNames.Count = 0 Then Return Task.FromResult(New List(Of LabPoint))
        Dim ps As New List(Of MySqlParameter) From {P("@hn", hn)}
        Dim names As New List(Of String)
        For i = 0 To labNames.Count - 1
            names.Add($"UPPER(@lab{i})")
            ps.Add(P($"@lab{i}", labNames(i)))
        Next
        Dim sql = "SELECT lo.lab_items_name_ref AS lab_name, lh.report_date, lo.lab_order_result
                     FROM lab_order lo
                     LEFT OUTER JOIN lab_head lh ON lh.lab_order_number = lo.lab_order_number
                    WHERE lh.hn = @hn
                      AND UPPER(lo.lab_items_name_ref) IN (" & String.Join(", ", names) & ")
                    ORDER BY lh.report_date DESC, lo.lab_order_number DESC
                    LIMIT 3000"
        Return QueryAsync(Of LabPoint)(sql, AddressOf MapLabPoint, ps.ToArray())
    End Function

    '──────────────────────── LAB Template Hemodialysis ────────────────────────
    ' เทียบเท่าหน้า QHis2  emr_html/fcontent_lab_hd.php
    '   - ไม่เลือกช่วงวันที่ : แสดงผลตรวจล่าสุด N วันย้อนหลัง (ค่าเริ่มต้น 10)
    '   - เลือกช่วงวันที่    : แสดงผลตรวจล่าสุด N วันย้อนหลังจากวันสุดท้ายที่เลือก

    Private Shared Function LabNameFilter(labNames As IList(Of String), ps As List(Of MySqlParameter)) As String
        Dim names As New List(Of String)
        For i = 0 To labNames.Count - 1
            names.Add($"UPPER(@lab{i.ToString(Globalization.CultureInfo.InvariantCulture)})")
            ps.Add(P($"@lab{i.ToString(Globalization.CultureInfo.InvariantCulture)}", labNames(i)))
        Next
        Return "UPPER(lo.lab_items_name_ref) IN (" & String.Join(", ", names) & ")"
    End Function

    ''' <summary>วันที่รายงานล่าสุด (มากสุด maxDates วัน) ที่มีผลของรายการในเทมเพลต</summary>
    Public Async Function GetLabTemplateDatesAsync(hn As String, labNames As IList(Of String),
                                                   fromDate As Date?, toDate As Date?,
                                                   maxDates As Integer) As Task(Of List(Of Date)) Implements IHosRepository.GetLabTemplateDatesAsync
        If labNames Is Nothing OrElse labNames.Count = 0 Then Return New List(Of Date)
        Dim ps As New List(Of MySqlParameter) From {P("@hn", hn)}
        Dim filter = LabNameFilter(labNames, ps)

        Dim range = ""
        If fromDate.HasValue Then
            range &= " AND lh.report_date >= @from"
            ps.Add(P("@from", fromDate.Value.Date))
        End If
        If toDate.HasValue Then
            range &= " AND lh.report_date <= @to"
            ps.Add(P("@to", toDate.Value.Date))
        End If

        Dim sql = "SELECT DISTINCT lh.report_date AS d
                     FROM lab_order lo
                     LEFT OUTER JOIN lab_head lh ON lh.lab_order_number = lo.lab_order_number
                    WHERE lh.hn = @hn
                      AND lh.report_date IS NOT NULL
                      AND " & filter & range & "
                    ORDER BY d DESC
                    LIMIT " & Math.Max(1, Math.Min(60, maxDates)).ToString(Globalization.CultureInfo.InvariantCulture)

        Dim rows = Await QueryAsync(sql, Function(r) DateOf(r, "d"), ps.ToArray()).ConfigureAwait(False)
        Return rows.Where(Function(d) d.HasValue).Select(Function(d) d.Value.Date).Distinct().OrderBy(Function(d) d).ToList()
    End Function

    ''' <summary>ผลตรวจของรายการในเทมเพลต ภายในช่วงวันที่ที่ระบุ</summary>
    Public Function GetLabTemplateResultsAsync(hn As String, labNames As IList(Of String),
                                               fromDate As Date, toDate As Date) As Task(Of List(Of LabPoint)) Implements IHosRepository.GetLabTemplateResultsAsync
        If labNames Is Nothing OrElse labNames.Count = 0 Then Return Task.FromResult(New List(Of LabPoint))
        Dim ps As New List(Of MySqlParameter) From {P("@hn", hn), P("@from", fromDate.Date), P("@to", toDate.Date)}
        Dim filter = LabNameFilter(labNames, ps)

        Dim sql = "SELECT lo.lab_items_name_ref AS lab_name, lh.report_date, lo.lab_order_result
                     FROM lab_order lo
                     LEFT OUTER JOIN lab_head lh ON lh.lab_order_number = lo.lab_order_number
                    WHERE lh.hn = @hn
                      AND lh.report_date BETWEEN @from AND @to
                      AND " & filter & "
                    ORDER BY lh.report_date, lo.lab_order_number
                    LIMIT 5000"
        Return QueryAsync(Of LabPoint)(sql, AddressOf MapLabPoint, ps.ToArray())
    End Function

    Private Shared Function MapLabPoint(r As IDataRecord) As LabPoint
        Return New LabPoint With {
            .LabName = StrOf(r, "lab_name"),
            .ReportDate = DateOf(r, "report_date"),
            .ResultText = StrOf(r, "lab_order_result")
        }
    End Function

    '──────────────────────── HD/CAPD Care ────────────────────────
    ' ข้อมูลกลุ่ม A ทั้งหมดมาจากตารางมาตรฐานของ HOSxP ไม่ต้องสร้างตารางเพิ่ม
    '   - รอบล้างไต : opitemrece + nondrugitems (รายการค่าบริการ — วิธีเดียวกับ getCaseHd เดิม)
    '   - น้ำหนัก/ความดัน : ovst + opdscreen
    '   - ยา : opitemrece + drugitems

    ''' <summary>เงื่อนไข LIKE ของชื่อรายการ (คืน "" ถ้าไม่มีคำค้น)</summary>
    Private Shared Function LikeAnyFilter(column As String, keywords As IList(Of String),
                                          prefix As String, ps As List(Of MySqlParameter)) As String
        If keywords Is Nothing OrElse keywords.Count = 0 Then Return ""
        Dim parts As New List(Of String)
        For i = 0 To keywords.Count - 1
            Dim name = $"@{prefix}{i.ToString(Globalization.CultureInfo.InvariantCulture)}"
            parts.Add($"{column} LIKE {name}")
            ps.Add(P(name, "%" & keywords(i) & "%"))
        Next
        Return "(" & String.Join(" OR ", parts) & ")"
    End Function

    Public Async Function GetDialysisSessionsAsync(hn As String, fromDate As Date,
                                                   hdKeywords As IList(Of String), pdKeywords As IList(Of String)) _
                                                   As Task(Of List(Of DialysisSession)) Implements IHosRepository.GetDialysisSessionsAsync
        Dim ps As New List(Of MySqlParameter) From {P("@hn", hn), P("@from", fromDate.Date)}
        Dim hd = LikeAnyFilter("n.`name`", hdKeywords, "hd", ps)
        Dim pd = LikeAnyFilter("n.`name`", pdKeywords, "pd", ps)
        Dim both = String.Join(" OR ", {hd, pd}.Where(Function(s) s <> ""))
        If both = "" Then Return New List(Of DialysisSession)

        Dim sql = "SELECT i.vstdate AS d, n.`name` AS item_name
                     FROM opitemrece i
                     INNER JOIN nondrugitems n ON n.icode = i.icode
                    WHERE i.hn = @hn AND i.vstdate >= @from AND (" & both & ")
                    GROUP BY i.vstdate, n.`name`
                    ORDER BY i.vstdate DESC
                    LIMIT 500"

        Dim hdList = If(hdKeywords, CType(New List(Of String)(), IList(Of String)))
        Dim pdList = If(pdKeywords, CType(New List(Of String)(), IList(Of String)))
        Dim rows = Await QueryAsync(Of DialysisSession)(sql,
            Function(r)
                Dim name = StrOf(r, "item_name")
                Return New DialysisSession With {
                    .SessionDate = If(DateOf(r, "d"), Date.MinValue),
                    .ItemName = name,
                    .Modality = ClassifyModality(name, hdList, pdList)}
            End Function, ps.ToArray()).ConfigureAwait(False)
        Return rows.Where(Function(s) s.SessionDate > Date.MinValue).ToList()
    End Function

    ''' <summary>ชื่อรายการนี้เป็น HD หรือ PD (ตรวจ PD ก่อน เพราะบางที่ชื่อรายการ CAPD มีคำว่า dialysis ปนอยู่)</summary>
    Friend Shared Function ClassifyModality(itemName As String, hdKeywords As IList(Of String),
                                            pdKeywords As IList(Of String)) As DialysisModality
        Dim name = If(itemName, "")
        If pdKeywords IsNot Nothing Then
            For Each k In pdKeywords
                If name.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0 Then Return DialysisModality.Pd
            Next
        End If
        If hdKeywords IsNot Nothing Then
            For Each k In hdKeywords
                If name.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0 Then Return DialysisModality.Hd
            Next
        End If
        Return DialysisModality.Unknown
    End Function

    Public Function GetVitalHistoryAsync(hn As String, fromDate As Date, maxRows As Integer) _
                                         As Task(Of List(Of VitalPoint)) Implements IHosRepository.GetVitalHistoryAsync
        Dim limit = Math.Max(1, Math.Min(500, maxRows))
        Dim sql = "SELECT v.vstdate AS d, s.bw, s.bps, s.bpd, s.pulse
                     FROM ovst v
                     INNER JOIN opdscreen s ON s.vn = v.vn
                    WHERE v.hn = @hn AND v.vstdate >= @from
                      AND (s.bw > 0 OR s.bps > 0)
                    ORDER BY v.vstdate DESC
                    LIMIT " & limit.ToString(Globalization.CultureInfo.InvariantCulture)
        Return QueryAsync(Of VitalPoint)(sql,
            Function(r) New VitalPoint With {
                .VisitDate = If(DateOf(r, "d"), Date.MinValue),
                .BodyWeight = Positive(DblOf(r, "bw")),
                .Systolic = Positive(DblOf(r, "bps")),
                .Diastolic = Positive(DblOf(r, "bpd")),
                .Pulse = Positive(DblOf(r, "pulse"))},
            P("@hn", hn), P("@from", fromDate.Date))
    End Function

    ''' <summary>HOSxP เก็บค่าที่ไม่ได้วัดเป็น 0 — ถือว่าไม่มีค่า</summary>
    Private Shared Function Positive(value As Double?) As Double?
        If Not value.HasValue OrElse value.Value <= 0 Then Return Nothing
        Return value
    End Function

    Public Function GetDialysisDrugsAsync(hn As String, fromDate As Date, keywords As IList(Of String)) _
                                          As Task(Of List(Of DrugUsage)) Implements IHosRepository.GetDialysisDrugsAsync
        If keywords Is Nothing OrElse keywords.Count = 0 Then Return Task.FromResult(New List(Of DrugUsage))
        Dim ps As New List(Of MySqlParameter) From {P("@hn", hn), P("@from", fromDate.Date)}
        Dim filter = LikeAnyFilter("d.`name`", keywords, "dg", ps)

        Dim sql = "SELECT d.`name` AS drug_name, MAX(i.vstdate) AS last_date, COUNT(DISTINCT i.vstdate) AS times
                     FROM opitemrece i
                     INNER JOIN drugitems d ON d.icode = i.icode
                    WHERE i.hn = @hn AND i.vstdate >= @from AND " & filter & "
                    GROUP BY d.`name`
                    ORDER BY last_date DESC
                    LIMIT 60"
        Return QueryAsync(Of DrugUsage)(sql,
            Function(r) New DrugUsage With {
                .DrugName = StrOf(r, "drug_name"),
                .LastDate = DateOf(r, "last_date"),
                .Times = If(IntOf(r, "times"), 0)},
            ps.ToArray())
    End Function

    '──────────────────────── app_storage ────────────────────────

    ' classLogin.sqlSelectDefaultSetting — คืนค่า Nothing ถ้าไม่พบ
    Public Async Function GetStorageValueAsync(computerName As String, section As String, key As String) As Task(Of String) Implements IHosRepository.GetStorageValueAsync
        Const sql As String =
            "SELECT storage_value FROM `app_storage`
              WHERE computer_name = UPPER(@computer) AND storage_section = @section AND storage_key = @key
              LIMIT 1"
        Dim rows = Await QueryAsync(sql, Function(r) StrOf(r, "storage_value"), P("@computer", computerName), P("@section", section), P("@key", key)).ConfigureAwait(False)
        Return rows.FirstOrDefault()
    End Function

    ' stdFunction.updateAppStorage (+ classLogin.sqlchk/sqlUpdate/sqlInsertDefaultSetting)
    Public Async Function SaveStorageValueAsync(computerName As String, section As String, key As String, value As String) As Task(Of Boolean) Implements IHosRepository.SaveStorageValueAsync
        Dim ids = Await QueryAsync(
            "SELECT app_storage_id AS id FROM `app_storage`
              WHERE computer_name = @computer AND storage_section = @section AND storage_key = @key",
            Function(r) StrOf(r, "id"), P("@computer", computerName), P("@section", section), P("@key", key)).ConfigureAwait(False)

        Dim affected As Integer
        If ids.Count > 0 AndAlso ids(0) <> "" Then
            affected = Await ExecuteAsync(
                "UPDATE `app_storage`
                    SET computer_name = @computer, storage_section = @section, storage_key = @key, storage_value = @value
                  WHERE app_storage_id = @id",
                P("@computer", computerName), P("@section", section), P("@key", key), P("@value", value), P("@id", ids(0))).ConfigureAwait(False)
        Else
            affected = Await ExecuteAsync(
                "INSERT INTO app_storage (app_storage_id, computer_name, storage_section, storage_key, storage_value)
                 VALUES (get_serialnumber('app_storage_id'), @computer, @section, @key, @value)",
                P("@computer", computerName), P("@section", section), P("@key", key), P("@value", value)).ConfigureAwait(False)
        End If
        Return affected > 0
    End Function

    '──────────────────────── คัดกรองโรคจากการทำงาน ────────────────────────

    ' classOccupational.getDataLockVnShowOccupationalByComputer
    Public Async Function GetOccupationalTargetAsync(computerName As String) As Task(Of OccupationalTarget) Implements IHosRepository.GetOccupationalTargetAsync
        Const sql As String =
            "SELECT dop.show_screen, ou.kskloginname AS staff, vn.hn, vl.vn
               FROM `vn_lock` vl
               LEFT OUTER JOIN onlineuser ou ON ou.servername = vl.lock_computer
               LEFT OUTER JOIN kskdepartment d ON d.department = ou.department
               LEFT OUTER JOIN depcode_occupational dop ON dop.depcode = d.depcode
               LEFT OUTER JOIN vn_stat vn ON vn.vn = vl.vn
              WHERE 1 = 1
                AND dop.show_screen = 'Y'
                AND vn.age_y BETWEEN '15' AND '59'
                AND vl.lock_computer = @computer
              LIMIT 1"
        Dim rows = Await QueryAsync(sql,
            Function(r) New OccupationalTarget With {
                .Hn = StrOf(r, "hn"),
                .Vn = StrOf(r, "vn"),
                .Staff = StrOf(r, "staff")
            }, P("@computer", computerName)).ConfigureAwait(False)
        Return rows.FirstOrDefault()
    End Function

    ' classOccupational.getDataOccupationalByVn
    Public Async Function HasOccupationalScreeningAsync(vn As String) As Task(Of Boolean) Implements IHosRepository.HasOccupationalScreeningAsync
        Dim rows = Await QueryAsync("SELECT vn FROM `opdscreen_occupational` WHERE vn = @vn LIMIT 1",
                                    Function(r) StrOf(r, "vn"), P("@vn", vn)).ConfigureAwait(False)
        Return rows.Count > 0
    End Function

    ' classLogin.checkLoginOnComputer
    Public Function GetLoginOnComputerAsync(computerName As String) As Task(Of String) Implements IHosRepository.GetLoginOnComputerAsync
        Return FirstStringAsync("SELECT kskloginname FROM `onlineuser` WHERE 1 = 1 AND servername = UPPER(@computer) LIMIT 1",
                                "kskloginname", P("@computer", computerName))
    End Function

    ' classOccupational.sqlInsertOpdscreenOccupational
    Public Async Function SaveOccupationalScreeningAsync(vn As String, hn As String, injury As Boolean, moreSymptoms As Boolean, dustPm As Boolean, staff As String) As Task Implements IHosRepository.SaveOccupationalScreeningAsync
        Await ExecuteAsync(
            "INSERT INTO opdscreen_occupational (vn, hn, occupational_injury, have_more_symptoms, dust_pm, staff, date_time)
             VALUES (@vn, @hn, @injury, @more, @dust, @staff, NOW())",
            P("@vn", vn), P("@hn", hn),
            P("@injury", If(injury, "Y", "N")), P("@more", If(moreSymptoms, "Y", "N")), P("@dust", If(dustPm, "Y", "N")),
            P("@staff", staff)).ConfigureAwait(False)
    End Function

    ''' <summary>ทะเบียนผลคัดกรองโรคจากการทำงาน — ใหม่ในเวอร์ชัน 2026 (ของเดิมบันทึกอย่างเดียว ไม่มีหน้าดูย้อนหลัง)</summary>
    Public Async Function GetOccupationalRegistryAsync(fromDate As Date, toDate As Date, search As String, maxRows As Integer) As Task(Of List(Of OccupationalRecord)) Implements IHosRepository.GetOccupationalRegistryAsync
        Dim q = If(search, "").Trim()
        Dim sql =
            "SELECT o.date_time, o.vn, o.hn, o.occupational_injury, o.have_more_symptoms, o.dust_pm, o.staff,
                    CONCAT(p.pname, p.fname, ' ', p.lname) AS pt_name, v.age_y
               FROM opdscreen_occupational o
               LEFT OUTER JOIN patient p ON p.hn = o.hn
               LEFT OUTER JOIN vn_stat v ON v.vn = o.vn
              WHERE o.date_time >= @from AND o.date_time < @to
                AND (@q = ''
                     OR o.hn LIKE @like
                     OR o.vn LIKE @like
                     OR o.staff LIKE @like
                     OR CONCAT(p.pname, p.fname, ' ', p.lname) LIKE @like)
              ORDER BY o.date_time DESC
              LIMIT " & Math.Max(1, Math.Min(20000, maxRows)).ToString(Globalization.CultureInfo.InvariantCulture)

        Return Await QueryAsync(sql,
            Function(r) New OccupationalRecord With {
                .ScreenedAt = DateOf(r, "date_time"),
                .Vn = StrOf(r, "vn"),
                .Hn = StrOf(r, "hn"),
                .PatientName = StrOf(r, "pt_name"),
                .AgeYears = IntOf(r, "age_y"),
                .Injury = IsYes(StrOf(r, "occupational_injury")),
                .MoreSymptoms = IsYes(StrOf(r, "have_more_symptoms")),
                .DustPm = IsYes(StrOf(r, "dust_pm")),
                .Staff = StrOf(r, "staff")
            },
            P("@from", fromDate.Date), P("@to", toDate.Date.AddDays(1)),
            P("@q", q), P("@like", "%" & q & "%")).ConfigureAwait(False)
    End Function

    Private Shared Function IsYes(v As String) As Boolean
        Dim s = If(v, "").Trim()
        Return s.StartsWith("Y", StringComparison.OrdinalIgnoreCase) OrElse s = "1"
    End Function

    '──────────────────────── ล็อกอินผู้ดูแลระบบ (บัญชี HOSxP) ────────────────────────
    ' ใช้วิธีเดียวกับโปรแกรม Appoint2020 (D:\VB Project\Appoint2020)
    '   LoginForm.vb        : loginname = '...' AND passweb = md5('...')   ← ตาราง opduser
    '   classLogin.vb       : สิทธิ์แอดมิน = groupname = 'admin' OR accessright LIKE '%...%'
    '   selectFullNameStaff : SELECT name FROM opduser WHERE loginname = ...

    ''' <summary>อ่านบัญชีผู้ใช้ HOSxP โดยไม่ตรวจรหัสผ่าน</summary>
    Public Async Function GetHosUserAsync(loginName As String) As Task(Of HosUser) Implements IHosRepository.GetHosUserAsync
        Dim sec = AppConfig.Current.Security
        Dim t = SafeIdentifier(sec.UserTable, "opduser")
        Dim cLogin = SafeIdentifier(sec.LoginColumn, "loginname")
        Dim cName = SafeIdentifier(sec.NameColumn, "name")
        Dim cGroup = If(String.IsNullOrWhiteSpace(sec.GroupColumn), "", SafeIdentifier(sec.GroupColumn, ""))
        Dim cRight = If(String.IsNullOrWhiteSpace(sec.AccessRightColumn), "", SafeIdentifier(sec.AccessRightColumn, ""))

        ' HOSxP บางรุ่นอาจไม่มี groupname / accessright — ถ้าคำสั่งแรกพัง ให้ลดคอลัมน์ลงแล้วลองใหม่
        For attempt = 0 To 2
            Dim groupSel = If(attempt = 0 AndAlso cGroup <> "", "`" & cGroup & "`", "''") & " AS group_name"
            Dim rightSel = If(attempt <= 1 AndAlso cRight <> "", "`" & cRight & "`", "''") & " AS access_right"
            Dim sql = $"SELECT `{cLogin}` AS login_name, `{cName}` AS full_name, {groupSel}, {rightSel} FROM `{t}` WHERE `{cLogin}` = @u LIMIT 1"
            Try
                Dim rows = Await QueryAsync(sql,
                    Function(r) New HosUser With {
                        .LoginName = StrOf(r, "login_name"),
                        .FullName = StrOf(r, "full_name"),
                        .GroupName = StrOf(r, "group_name"),
                        .AccessRight = StrOf(r, "access_right")
                    }, P("@u", loginName)).ConfigureAwait(False)
                Dim found = rows.FirstOrDefault()
                If found IsNot Nothing Then found.IsAdmin = IsAdminUser(found)
                Return found
            Catch ex As HosQueryException
                If attempt = 2 Then Throw
            End Try
        Next
        Return Nothing
    End Function

    ''' <summary>ตรวจชื่อผู้ใช้ + รหัสผ่านกับตารางผู้ใช้ของ HOSxP</summary>
    Public Async Function AuthenticateAsync(loginName As String, password As String) As Task(Of AuthResult) Implements IHosRepository.AuthenticateAsync
        Dim sec = AppConfig.Current.Security
        Dim t = SafeIdentifier(sec.UserTable, "opduser")
        Dim cLogin = SafeIdentifier(sec.LoginColumn, "loginname")

        ' 1) มีชื่อผู้ใช้นี้หรือไม่ — แยกข้อความ "ไม่พบชื่อผู้ใช้" ออกจาก "รหัสผ่านไม่ถูกต้อง" เพื่อให้หาสาเหตุง่าย
        Dim user = Await GetHosUserAsync(loginName).ConfigureAwait(False)
        If user Is Nothing Then Return AuthResult.Fail(AuthStatus.UnknownUser)

        ' 2) ลองทุกคอลัมน์ × ทุกวิธีเข้ารหัส (HOSxP แต่ละรุ่น/แต่ละโรงพยาบาลตั้งไม่เหมือนกัน)
        Dim columns = If(sec.PasswordColumns IsNot Nothing AndAlso sec.PasswordColumns.Length > 0,
                         sec.PasswordColumns, New String() {"passweb"})
        Dim modes = If(sec.PasswordModes IsNot Nothing AndAlso sec.PasswordModes.Length > 0,
                       sec.PasswordModes, New String() {"Md5"})
        Dim usableColumns As New List(Of String)     ' คอลัมน์ที่มีจริงและมีค่าอยู่
        Dim emptyColumns As New List(Of String)      ' คอลัมน์ที่มีจริงแต่ค่าว่าง

        For Each rawCol In columns
            Dim c = SafeIdentifier(rawCol, "")
            If c = "" Then Continue For

            ' คอลัมน์นี้มีจริงไหม และของบัญชีนี้ว่างหรือเปล่า
            Dim filled As Integer? = Nothing
            Try
                Dim probe = Await QueryAsync($"SELECT LENGTH(TRIM(COALESCE(`{c}`, ''))) AS n FROM `{t}` WHERE `{cLogin}` = @u LIMIT 1",
                                             Function(r) IntOf(r, "n"), P("@u", loginName)).ConfigureAwait(False)
                filled = If(probe.Count > 0, probe(0), Nothing)
            Catch ex As HosQueryException
                Continue For        ' ไม่มีคอลัมน์นี้ในตาราง
            End Try
            If Not filled.HasValue OrElse filled.Value = 0 Then
                emptyColumns.Add(c)
                Continue For
            End If
            usableColumns.Add(c)

            For Each mode In modes
                Dim expr = PasswordExpression(mode, c)
                If expr = "" Then Continue For
                Try
                    Dim rows = Await QueryAsync($"SELECT `{cLogin}` AS login_name FROM `{t}` WHERE `{cLogin}` = @u AND {expr} LIMIT 1",
                                                Function(r) StrOf(r, "login_name"),
                                                P("@u", loginName), P("@p", password)).ConfigureAwait(False)
                    If rows.Count > 0 Then
                        user.IsAdmin = IsAdminUser(user)
                        If Not user.IsAdmin Then Return AuthResult.Fail(AuthStatus.NotAdmin, user.GroupName, user)
                        Return AuthResult.Ok(user)
                    End If
                Catch ex As HosQueryException
                    ' ฟังก์ชันที่ MySQL รุ่นนั้นไม่รองรับ (เช่น PASSWORD() ใน MySQL 8) → ลองวิธีถัดไป
                End Try
            Next
        Next

        ' 3) ไม่มีคอลัมน์ไหนที่ตั้งรหัสผ่านไว้เลย → บอกให้ชัดว่าต้องไปตั้งใน HOSxP
        If usableColumns.Count = 0 Then
            Return AuthResult.Fail(AuthStatus.PasswordNotSet, String.Join(", ", emptyColumns), user)
        End If
        Return AuthResult.Fail(AuthStatus.WrongPassword, String.Join(", ", usableColumns), user)
    End Function


    ''' <summary>groupname อยู่ในกลุ่มแอดมิน / อยู่ใน AdminLogins / accessright มีคำที่กำหนด</summary>
    Private Shared Function IsAdminUser(u As HosUser) As Boolean
        Dim sec = AppConfig.Current.Security
        If sec.IsAdminLogin(u.LoginName) Then Return True
        If sec.IsAdminGroup(u.GroupName) Then Return True
        Dim keyword = If(sec.AccessRightKeyword, "").Trim()
        If keyword <> "" AndAlso If(u.AccessRight, "").IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 Then Return True
        Return False
    End Function

    ''' <summary>นิพจน์เทียบรหัสผ่าน (@p = รหัสผ่านที่ผู้ใช้กรอก)</summary>
    Private Shared Function PasswordExpression(mode As String, passColumn As String) As String
        Dim c = "`" & passColumn & "`"
        Select Case If(mode, "").Trim().ToUpperInvariant()
            Case "MD5" : Return c & " = MD5(@p)"
            Case "PLAIN" : Return c & " = @p"
            Case "SHA1" : Return c & " = SHA1(@p)"
            Case "MYSQLPASSWORD" : Return c & " = PASSWORD(@p)"
            Case "ENCODEBMS" : Return c & " = ENCODE(@p, 'bms')"
            Case "ENCODEHOS" : Return c & " = ENCODE(@p, 'hos')"
            Case Else : Return ""
        End Select
    End Function

    ''' <summary>อนุญาตเฉพาะชื่อตาราง/คอลัมน์ที่เป็นตัวอักษร ตัวเลข และ _ (ค่ามาจากไฟล์ตั้งค่า ไม่ใช่จากผู้ใช้)</summary>
    Private Shared Function SafeIdentifier(value As String, fallback As String) As String
        Dim s = If(value, "").Trim()
        If s = "" Then Return fallback
        For Each ch In s
            If Not (Char.IsLetterOrDigit(ch) OrElse ch = "_"c) Then Return fallback
        Next
        Return s
    End Function

End Class

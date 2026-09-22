''' <summary>
''' ตรรกะป้าย "DM / HT Green" (ย้ายมาจาก dialogMedicalCare_Load ของโปรแกรมเดิม)
'''
''' ⚠ หมายเหตุจากการตรวจโค้ดเดิม (คงตรรกะเดิมไว้ทุกตัวอักษร เพื่อให้ผลลัพธ์เหมือนระบบเดิม):
'''   - ป้าย DM Green จะแสดงเมื่อ qualify 01-05 ทุกตัวเป็น "not_in_qualify_0n"
'''     แต่ qualify_03 คืนค่า "dm_qualify_03" หรือ "not_in_dm_qualify_03" เท่านั้น (ไม่เคยเป็น "not_in_qualify_03")
'''     qualify_05 ก็คืน "dm_qualify_05" เสมอ → ในระบบเดิมป้าย DM Green จึงไม่เคยแสดง
'''   - ป้าย HT Green ต้องการ curChkHtQualify02-05 ซึ่งโปรแกรมเดิมไม่เคยกำหนดค่า → ไม่เคยแสดงเช่นกัน
'''   หากต้องการแก้ ให้แก้เฉพาะในไฟล์นี้ (และ SQL ใน MySqlHosRepository.GetDmQualifyAsync) ได้ที่เดียว
''' </summary>
Public NotInheritable Class ClinicalRules

    Private Sub New()
    End Sub

    Public Shared Function IsDmGreen(hasDmDx As Boolean, q1 As String, q2 As String, q3 As String, q4 As String, q5 As String) As Boolean
        Return hasDmDx AndAlso
               q1 = "not_in_qualify_01" AndAlso
               q2 = "not_in_qualify_02" AndAlso
               q3 = "not_in_qualify_03" AndAlso
               q4 = "not_in_qualify_04" AndAlso
               q5 = "not_in_qualify_05"
    End Function

    Public Shared Function IsHtGreen(hasHtDx As Boolean, q1 As String, q2 As String, q3 As String, q4 As String, q5 As String) As Boolean
        Return hasHtDx AndAlso
               q1 = "not_in_qualify_01" AndAlso
               q2 = "not_in_qualify_02" AndAlso
               q3 = "not_in_qualify_03" AndAlso
               q4 = "not_in_qualify_04" AndAlso
               q5 = "not_in_qualify_05"
    End Function

    ''' <summary>
    ''' ข้อความตรวจคัดกรอง ตา ไต เท้า — โค้ดเดิมตัด 2 ตัวอักษรท้ายทิ้ง (ตัวคั่น ", ")
    ''' และแก้ค่าตัวแปร global ทุกครั้งที่เปิดหน้า ทำให้ข้อความสั้นลงเรื่อย ๆ — ที่นี่ตัดเฉพาะตัวคั่นท้ายข้อความ
    ''' </summary>
    Public Shared Function CleanScreeningText(raw As String) As String
        If String.IsNullOrWhiteSpace(raw) Then Return ""
        Return raw.Trim().TrimEnd(","c, " "c, "|"c, ";"c, "/"c).Trim()
    End Function

End Class

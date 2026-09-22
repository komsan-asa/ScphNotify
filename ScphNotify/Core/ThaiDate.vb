''' <summary>รูปแบบวันที่ภาษาไทย (พ.ศ.)</summary>
Public NotInheritable Class ThaiDate

    Private Sub New()
    End Sub

    Private Shared ReadOnly ShortMonths As String() = {"", "ม.ค.", "ก.พ.", "มี.ค.", "เม.ย.", "พ.ค.", "มิ.ย.", "ก.ค.", "ส.ค.", "ก.ย.", "ต.ค.", "พ.ย.", "ธ.ค."}
    Private Shared ReadOnly LongMonths As String() = {"", "มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤศจิกายน", "ธันวาคม"}

    ''' <summary>เช่น 18 ก.ย.69 (เหมือนฟังก์ชัน ThaiDateShort ใน HOSxP)</summary>
    Public Shared Function ShortDate(d As Date) As String
        Return $"{d.Day} {ShortMonths(d.Month)}{(d.Year + 543) Mod 100:00}"
    End Function

    ''' <summary>เช่น 18 ก.ย. 2569</summary>
    Public Shared Function MediumDate(d As Date) As String
        Return $"{d.Day} {ShortMonths(d.Month)} {d.Year + 543}"
    End Function

    Public Shared Function MediumDate(d As Date?) As String
        Return If(d.HasValue, MediumDate(d.Value), "-")
    End Function

    ''' <summary>เช่น 18 กันยายน 2569</summary>
    Public Shared Function LongDate(d As Date) As String
        Return $"{d.Day} {LongMonths(d.Month)} {d.Year + 543}"
    End Function

    Public Shared Function TimeText(d As Date) As String
        Return d.ToString("HH:mm:ss", Globalization.CultureInfo.InvariantCulture)
    End Function

End Class

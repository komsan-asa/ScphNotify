Imports System.Globalization
Imports System.Text

''' <summary>อ่านค่าจาก DataReader แบบปลอดภัย (DBNull / byte[] / ชนิดตัวเลขที่ต่างกัน)</summary>
Friend Module DbValue

    Private Function Raw(r As IDataRecord, name As String) As Object
        Dim i = r.GetOrdinal(name)
        If r.IsDBNull(i) Then Return Nothing
        Return r.GetValue(i)
    End Function

    Public Function IsNullOf(r As IDataRecord, name As String) As Boolean
        Return Raw(r, name) Is Nothing
    End Function

    Public Function StrOf(r As IDataRecord, name As String) As String
        Dim v = Raw(r, name)
        If v Is Nothing Then Return ""
        Dim bytes = TryCast(v, Byte())
        If bytes IsNot Nothing Then Return Encoding.UTF8.GetString(bytes)
        If TypeOf v Is DateTime Then Return DirectCast(v, DateTime).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
        Return Convert.ToString(v, CultureInfo.InvariantCulture)
    End Function

    Public Function DblOf(r As IDataRecord, name As String) As Double?
        Dim v = Raw(r, name)
        If v Is Nothing Then Return Nothing
        If TypeOf v Is String OrElse TypeOf v Is Byte() Then
            Dim d As Double
            If Double.TryParse(StrOf(r, name).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, d) Then Return d
            Return Nothing
        End If
        Try
            Return Convert.ToDouble(v, CultureInfo.InvariantCulture)
        Catch
            Return Nothing
        End Try
    End Function

    Public Function IntOf(r As IDataRecord, name As String) As Integer?
        Dim d = DblOf(r, name)
        If Not d.HasValue Then Return Nothing
        Return CInt(Math.Round(d.Value))
    End Function

    Public Function DateOf(r As IDataRecord, name As String) As Date?
        Dim v = Raw(r, name)
        If v Is Nothing Then Return Nothing
        If TypeOf v Is DateTime Then
            Dim dt = DirectCast(v, DateTime)
            If dt.Year < 1900 Then Return Nothing   ' 0000-00-00 ของ HOSxP
            Return dt
        End If
        Dim parsed As DateTime
        If DateTime.TryParse(StrOf(r, name), CultureInfo.InvariantCulture, DateTimeStyles.None, parsed) Then Return parsed
        Return Nothing
    End Function

End Module

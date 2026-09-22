''' <summary>
''' จุดรวมโค้ดที่ขึ้นกับเวอร์ชันของ Windows Forms
''' (ค่าคงที่ MONO_PREVIEW ใช้เฉพาะตอนตรวจ build/preview นอก Windows — ใน Visual Studio ไม่ได้กำหนด)
''' </summary>
Friend Module Compat

    Public Function DpiOf(c As Control) As Integer
#If MONO_PREVIEW Then
        Return 96
#Else
        If c Is Nothing OrElse c.DeviceDpi <= 0 Then Return 96
        Return c.DeviceDpi
#End If
    End Function

    Public Sub SetPlaceholder(tb As TextBox, text As String)
#If Not MONO_PREVIEW Then
        tb.PlaceholderText = text
#End If
    End Sub

End Module

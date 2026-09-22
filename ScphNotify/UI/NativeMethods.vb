Imports System.Runtime.InteropServices

''' <summary>เรียก DWM ของ Windows 11 เพื่อทำมุมหน้าต่างโค้ง / สีแถบหัวเรื่อง (Windows 10 จะข้ามไปเฉย ๆ)</summary>
Friend NotInheritable Class NativeMethods

    Private Sub New()
    End Sub

    Private Const DWMWA_USE_IMMERSIVE_DARK_MODE As Integer = 20
    Private Const DWMWA_WINDOW_CORNER_PREFERENCE As Integer = 33
    Private Const DWMWA_BORDER_COLOR As Integer = 34
    Private Const DWMWA_CAPTION_COLOR As Integer = 35
    Private Const DWMWA_TEXT_COLOR As Integer = 36

    Public Enum CornerPreference
        [Default] = 0
        DoNotRound = 1
        Round = 2
        RoundSmall = 3
    End Enum

    <DllImport("dwmapi.dll", PreserveSig:=True)>
    Private Shared Function DwmSetWindowAttribute(hwnd As IntPtr, attr As Integer, ByRef attrValue As Integer, attrSize As Integer) As Integer
    End Function

    Private Shared Function ToColorRef(c As Color) As Integer
        Return c.R Or (CInt(c.G) << 8) Or (CInt(c.B) << 16)
    End Function

    Private Shared Function SetAttr(hwnd As IntPtr, attr As Integer, value As Integer) As Boolean
        Try
            If hwnd = IntPtr.Zero OrElse Environment.OSVersion.Platform <> PlatformID.Win32NT Then Return False
            Return DwmSetWindowAttribute(hwnd, attr, value, 4) = 0
        Catch
            Return False
        End Try
    End Function

    ''' <summary>มุมโค้งแบบ Windows 11 — คืนค่า False หากระบบไม่รองรับ</summary>
    Public Shared Function TrySetCorners(form As Form, pref As CornerPreference) As Boolean
        Return SetAttr(form.Handle, DWMWA_WINDOW_CORNER_PREFERENCE, CInt(pref))
    End Function

    ''' <summary>ตั้งสีแถบหัวเรื่อง / ข้อความ / เส้นขอบ (Windows 11 build 22000+)</summary>
    Public Shared Sub TrySetCaption(form As Form, caption As Color, text As Color, border As Color)
        SetAttr(form.Handle, DWMWA_CAPTION_COLOR, ToColorRef(caption))
        SetAttr(form.Handle, DWMWA_TEXT_COLOR, ToColorRef(text))
        SetAttr(form.Handle, DWMWA_BORDER_COLOR, ToColorRef(border))
    End Sub

End Class

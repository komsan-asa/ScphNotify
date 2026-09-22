''' <summary>หน้าต่างแจ้งข้อผิดพลาด พร้อมรายละเอียด/คำสั่ง SQL (แทน MessageBox + popUpMySqlTrace เดิม)</summary>
Public Class ErrorDialog

    Private _expanded As Boolean
    Private _collapsedHeight As Integer

    Public Shared Sub ShowError(owner As IWin32Window, title As String, ex As Exception)
        Dim details As New System.Text.StringBuilder()
        Dim q = TryCast(ex, HosQueryException)
        If q IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(q.Sql) Then
            details.AppendLine("SQL:")
            details.AppendLine(q.Sql.Trim())
            details.AppendLine()
        End If
        details.AppendLine(If(ex?.ToString(), ""))
        ShowMessage(owner, title, If(ex?.Message, "ไม่ทราบสาเหตุ"), details.ToString())
    End Sub

    Public Shared Sub ShowMessage(owner As IWin32Window, title As String, message As String, Optional details As String = "")
        Using dlg As New ErrorDialog()
            dlg.lblTitle.Text = title
            dlg.lblMessage.Text = message
            dlg.txtDetails.Text = details
            dlg.lnkDetails.Visible = Not String.IsNullOrWhiteSpace(details)
            dlg.btnCopy.Visible = dlg.lnkDetails.Visible
            If owner Is Nothing Then
                dlg.StartPosition = FormStartPosition.CenterScreen
                dlg.TopMost = True
            End If
            dlg.ShowDialog(owner)
        End Using
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        Icon = AppSession.AppIcon
        _collapsedHeight = ClientSize.Height
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        NativeMethods.TrySetCaption(Me, Theme.Surface, Theme.TextPrimary, Theme.Border)
    End Sub

    Private Sub lnkDetails_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkDetails.LinkClicked
        _expanded = Not _expanded
        pnlDetails.Visible = _expanded
        ClientSize = New Size(ClientSize.Width, If(_expanded, _collapsedHeight + Theme.Scale(Me, 240), _collapsedHeight))
        lnkDetails.Text = If(_expanded, "ซ่อนรายละเอียด", "แสดงรายละเอียด")
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click
        Try
            Clipboard.SetText($"{lblTitle.Text}{Environment.NewLine}{lblMessage.Text}{Environment.NewLine}{Environment.NewLine}{txtDetails.Text}")
            btnCopy.Text = "คัดลอกแล้ว"
        Catch
        End Try
    End Sub

End Class

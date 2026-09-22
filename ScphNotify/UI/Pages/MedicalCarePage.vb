''' <summary>หน้า Medical Care (แทน dialogMedicalCare เดิม)</summary>
Public Class MedicalCarePage
    Implements IPatientPage

    Private _boundKey As String

    Public Sub ResetBinding() Implements IPatientPage.ResetBinding
        _boundKey = Nothing
    End Sub

    Public Sub BindPatient(snap As PatientSnapshot) Implements IPatientPage.BindPatient
        Dim key = If(snap Is Nothing, "", $"{snap.Vn}|{snap.LoadedAt.Ticks}")
        If key = _boundKey Then Return
        _boundKey = key

        ApplyVisibility()
        If snap Is Nothing Then Return

        SuspendLayout()
        Try
            BindDiagnosis(snap.HasDmDiagnosis, snap.DmClinicRegistered, snap.IsDmGreen, lblDmStatus, flpDm)
            BindDiagnosis(snap.HasHtDiagnosis, snap.HtClinicRegistered, snap.IsHtGreen, lblHtStatus, flpHt)
            BindDialysis(snap.CaseHd)
            BindEgfr(snap.EgfrResults)
            BindScreening(snap.ScreeningText)
            BindAppointments(snap.NextAppointments)
        Finally
            ResumeLayout(True)
        End Try
    End Sub

    ''' <summary>ซ่อน/แสดงการ์ดตามการตั้งค่าของเครื่อง (app_storage)</summary>
    Private Sub ApplyVisibility()
        Dim p = AppSession.Display
        Dim status = New (Card As CardPanel, Show As Boolean)() {
            (cardDm, p.ShowDmColor), (cardHt, p.ShowHtColor), (cardHd, p.ShowCaseHd), (cardEgfr, p.ShowEgfr)}
        Dim lastVisible = -1
        For i = 0 To status.Length - 1
            status(i).Card.Visible = status(i).Show
            tlpStatus.ColumnStyles(i).Width = If(status(i).Show, 25.0F, 0.0F)
            If status(i).Show Then lastVisible = i
        Next
        For i = 0 To status.Length - 1
            status(i).Card.Margin = New Padding(0, 0, If(i = lastVisible, 0, 14), 16)
        Next
        tlpMain.RowStyles(0).Height = If(lastVisible >= 0, 172.0F, 0.0F)

        cardScreening.Visible = p.ShowScreening
        cardAppointments.Visible = p.ShowNextAppointment
        tlpDetail.ColumnStyles(0).Width = If(p.ShowScreening, 50.0F, 0.0F)
        tlpDetail.ColumnStyles(1).Width = If(p.ShowNextAppointment, 50.0F, 0.0F)
        cardScreening.Margin = New Padding(0, 0, If(p.ShowNextAppointment, 14, 0), 0)
    End Sub

    Private Shared Sub AddChip(host As FlowLayoutPanel, text As String, level As AlertLevel, icon As IconKind)
        host.Controls.Add(New StatusChip(text, level, icon) With {.Margin = New Padding(0, 0, 6, 6)})
    End Sub

    Private Shared Sub ClearChips(host As FlowLayoutPanel)
        For Each c As Control In host.Controls.Cast(Of Control)().ToList()
            c.Dispose()
        Next
        host.Controls.Clear()
    End Sub

    Private Sub BindDiagnosis(hasDx As Boolean, registered As Boolean, green As Boolean, status As Label, chips As FlowLayoutPanel)
        ClearChips(chips)
        If Not hasDx Then
            status.Text = "ไม่พบการวินิจฉัยใน 1 ปี"
            status.ForeColor = Theme.TextMuted
            status.Font = Theme.UiFont(10.0F)
            Return
        End If
        status.Text = "มีการวินิจฉัยใน 1 ปี"
        status.ForeColor = Theme.TextPrimary
        status.Font = Theme.UiFont(11.0F, FontStyle.Bold)
        If registered Then
            AddChip(chips, "ลงทะเบียนคลินิกแล้ว", AlertLevel.Success, IconKind.Check)
        Else
            AddChip(chips, "ยังไม่ลงทะเบียนคลินิก", AlertLevel.Warning, IconKind.Warning)
        End If
        If green Then AddChip(chips, "Green", AlertLevel.Success, IconKind.Check)
    End Sub

    Private Sub BindDialysis(caseHd As String)
        If String.IsNullOrWhiteSpace(caseHd) Then
            lblHdValue.Text = "ไม่พบ"
            lblHdValue.ForeColor = Theme.TextMuted
            lblHdNote.Text = "ไม่พบรายการ Hemodialysis / CAPD ในประวัติการรับบริการ"
        Else
            lblHdValue.Text = caseHd.Trim()
            lblHdValue.ForeColor = Theme.Danger
            lblHdNote.Text = "พบรายการฟอกไตในประวัติการรับบริการ — ระวังการสั่งยาและการปรับขนาดยา"
        End If
    End Sub

    ''' <summary>แยก "53.59 (31 ส.ค.68)" → ค่า / วันที่</summary>
    Private Shared Sub SplitValueDate(text As String, ByRef value As String, ByRef dateText As String)
        Dim i = text.IndexOf("("c)
        If i > 0 Then
            value = text.Substring(0, i).Trim()
            dateText = text.Substring(i).Trim().Trim("("c, ")"c).Trim()
        Else
            value = text.Trim()
            dateText = ""
        End If
    End Sub

    Private Sub BindEgfr(results As List(Of String))
        If results Is Nothing OrElse results.Count = 0 Then
            lblEgfrValue.Text = "ไม่พบ"
            lblEgfrValue.ForeColor = Theme.TextMuted
            lblEgfrNote.Text = "ไม่พบค่า eGFR ต่ำกว่า 60 ในช่วง 1 ปีที่ผ่านมา"
            Return
        End If
        Dim v As String = "", d As String = ""
        SplitValueDate(results(0), v, d)
        lblEgfrValue.Text = v
        lblEgfrValue.ForeColor = Theme.Warning
        Dim lines As New List(Of String)
        If d <> "" Then lines.Add($"ล่าสุด {d}")
        If results.Count > 1 Then
            Dim previous = results.Skip(1).Select(Function(r)
                                                      Dim pv As String = "", pd As String = ""
                                                      SplitValueDate(r, pv, pd)
                                                      Return pv
                                                  End Function)
            lines.Add("ก่อนหน้า  " & String.Join("  ·  ", previous))
        End If
        lblEgfrNote.Text = String.Join(Environment.NewLine, lines)
    End Sub

    Private Sub BindScreening(text As String)
        If String.IsNullOrWhiteSpace(text) Then
            lblScreening.Text = "ไม่มีข้อมูลการตรวจคัดกรอง"
            lblScreening.ForeColor = Theme.TextMuted
        Else
            lblScreening.Text = text
            lblScreening.ForeColor = Theme.TextPrimary
        End If
    End Sub

    Private Sub BindAppointments(items As List(Of String))
        lstAppointments.BeginUpdate()
        lstAppointments.Items.Clear()
        For Each raw In If(items, New List(Of String))
            Dim title As String = "", dept As String = ""
            SplitValueDate(raw, title, dept)
            lstAppointments.Items.Add(New ListEntry(FormatThaiDate(title), dept, IconKind.Calendar, AlertLevel.Info))
        Next
        lstAppointments.EndUpdate()
        Dim hasItems = lstAppointments.Items.Count > 0
        lstAppointments.Visible = hasItems
        lblNoAppointment.Visible = Not hasItems
        cardAppointments.HeaderRightText = If(hasItems, $"{lstAppointments.Items.Count} รายการ", "")
    End Sub

    ''' <summary>"25/9/2569" → "25 ก.ย. 2569"</summary>
    Private Shared Function FormatThaiDate(s As String) As String
        Dim parts = s.Split("/"c)
        Dim d, m, y As Integer
        If parts.Length = 3 AndAlso Integer.TryParse(parts(0), d) AndAlso Integer.TryParse(parts(1), m) AndAlso Integer.TryParse(parts(2), y) Then
            Try
                Return ThaiDate.MediumDate(New Date(y - 543, m, d))
            Catch
            End Try
        End If
        Return s
    End Function

End Class

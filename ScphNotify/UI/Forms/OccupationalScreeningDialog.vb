''' <summary>
''' แบบคัดกรองโรคจากการทำงาน (แทน dialogOccupational เดิม)
''' แสดงเมื่อแผนกของเครื่องนี้กำหนด show_screen = 'Y' ในตาราง depcode_occupational,
''' ผู้ป่วยอายุ 15–59 ปี และยังไม่มีข้อมูลใน opdscreen_occupational ของ VN นี้
''' </summary>
Public Class OccupationalScreeningDialog

    Private ReadOnly _target As OccupationalTarget

    ' สำหรับ Visual Studio Designer
    Public Sub New()
        Me.New(New OccupationalTarget(), Nothing)
    End Sub

    Public Sub New(target As OccupationalTarget, snap As PatientSnapshot)
        InitializeComponent()
        _target = If(target, New OccupationalTarget())
        Dim name = If(snap IsNot Nothing AndAlso snap.Vn = _target.Vn, snap.PatientName, "")
        lblPatient.Text = $"HN {_target.Hn}   ·   VN {_target.Vn}" & If(name <> "", $"   ·   {name}", "")
        For Each seg In {segQ1, segQ2, segQ3}
            AddHandler seg.SelectedIndexChanged, AddressOf OnAnswerChanged
        Next
        OnAnswerChanged(Nothing, EventArgs.Empty)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        Icon = AppSession.AppIcon
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        NativeMethods.TrySetCaption(Me, Theme.Surface, Theme.TextPrimary, Theme.Border)
    End Sub

    Private Shared Function IsYes(seg As SegmentedControl) As Boolean
        Return seg.SelectedIndex = 0
    End Function

    Private Sub OnAnswerChanged(sender As Object, e As EventArgs)
        Dim anyYes = False
        For Each seg In {segQ1, segQ2, segQ3}
            seg.SelectedLevel = If(IsYes(seg), AlertLevel.Danger, AlertLevel.Success)
            anyYes = anyYes OrElse IsYes(seg)
        Next
        If anyYes Then
            banner.ShowMessage("มีคำตอบ ""ใช่"" — กรุณาส่งต่อคลินิกโรคจากการทำงาน โทร 366, 106", AlertLevel.Danger)
        Else
            banner.ShowMessage("ตอบว่า ""ใช่"" ข้อใดข้อหนึ่ง ให้ส่งต่อคลินิกโรคจากการทำงาน โทร 366, 106", AlertLevel.Info)
        End If
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        btnSave.Enabled = False
        UseWaitCursor = True
        Try
            Await AppSession.Repository.SaveOccupationalScreeningAsync(
                _target.Vn, _target.Hn, IsYes(segQ1), IsYes(segQ2), IsYes(segQ3), _target.Staff)
            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            ErrorDialog.ShowError(Me, "บันทึกแบบคัดกรองไม่สำเร็จ", ex)
        Finally
            UseWaitCursor = False
            If Not IsDisposed Then btnSave.Enabled = True
        End Try
    End Sub

End Class

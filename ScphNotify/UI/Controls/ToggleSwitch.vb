Imports System.ComponentModel

''' <summary>สวิตช์เปิด/ปิด (สืบทอด CheckBox — ใช้ Checked / CheckedChanged ได้ตามปกติ)</summary>
Public Class ToggleSwitch
    Inherits CheckBox

    Private _hover As Boolean
    Private _anim As Single     ' 0 = ปิด, 1 = เปิด
    Private WithEvents _timer As New System.Windows.Forms.Timer() With {.Interval = 15}
    Private _description As String = ""

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        Cursor = Cursors.Hand
        AutoSize = False
        Size = New Size(260, 40)
    End Sub

    ''' <summary>คำอธิบายบรรทัดที่สอง (ตัวเล็ก สีจาง)</summary>
    <Category("Appearance"), DefaultValue("")>
    Public Property Description As String
        Get
            Return _description
        End Get
        Set(value As String)
            _description = If(value, "")
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnCheckedChanged(e As EventArgs)
        MyBase.OnCheckedChanged(e)
        If IsHandleCreated AndAlso Visible Then
            _timer.Start()
        Else
            _anim = If(Checked, 1.0F, 0.0F)
        End If
        Invalidate()
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        _anim = If(Checked, 1.0F, 0.0F)
    End Sub

    Private Sub OnAnimate(sender As Object, e As EventArgs) Handles _timer.Tick
        Dim target = If(Checked, 1.0F, 0.0F)
        Dim stepSize = 0.18F
        If Math.Abs(_anim - target) <= stepSize Then
            _anim = target
            _timer.Stop()
        Else
            _anim += If(target > _anim, stepSize, -stepSize)
        End If
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(eventargs As EventArgs)
        MyBase.OnMouseEnter(eventargs)
        _hover = True
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(eventargs As EventArgs)
        MyBase.OnMouseLeave(eventargs)
        _hover = False
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(pevent As PaintEventArgs)
        Dim g = pevent.Graphics
        Gfx.PaintParentBackground(Me, g)
        Gfx.HighQuality(g)

        Dim trackW = Theme.ScaleF(Me, 40)
        Dim trackH = Theme.ScaleF(Me, 22)
        Dim track As New RectangleF(1, (Height - trackH) / 2, trackW, trackH)

        Dim offColor = If(_hover, Theme.BorderStrong, Theme.Blend(Theme.Border, Theme.BorderStrong, 0.5))
        Dim onColor = If(_hover, Theme.PrimaryHover, Theme.Primary)
        Dim trackColor = Theme.Blend(offColor, onColor, _anim)
        If Not Enabled Then trackColor = Theme.Blend(trackColor, Theme.SurfaceSunken, 0.6)
        Gfx.FillRounded(g, track, trackH / 2, trackColor)

        Dim knob = trackH - Theme.ScaleF(Me, 6)
        Dim kx = track.X + Theme.ScaleF(Me, 3) + (trackW - knob - Theme.ScaleF(Me, 6)) * _anim
        Dim kr As New RectangleF(kx, track.Y + Theme.ScaleF(Me, 3), knob, knob)
        Using shadow As New SolidBrush(Color.FromArgb(40, 0, 0, 0))
            g.FillEllipse(shadow, kr.X, kr.Y + 1, kr.Width, kr.Height)
        End Using
        Using b As New SolidBrush(Color.White)
            g.FillEllipse(b, kr)
        End Using

        If Focused AndAlso ShowFocusCues Then
            Gfx.DrawRounded(g, RectangleF.Inflate(track, 2, 2), trackH / 2 + 2, Theme.WithAlpha(Theme.Primary, 110), 1.5F)
        End If

        Dim textX = CInt(track.Right + Theme.Scale(Me, 12))
        Dim fg = If(Enabled, Theme.TextPrimary, Theme.TextMuted)
        If _description = "" Then
            Gfx.Text(g, Text, Font, New Rectangle(textX, 0, Width - textX, Height), fg)
        Else
            Dim half = Height \ 2
            Gfx.Text(g, Text, Font, New Rectangle(textX, 0, Width - textX, half + 2), fg, TextFormatFlags.Left Or TextFormatFlags.Bottom)
            Gfx.Text(g, _description, Theme.UiFont(8.5F), New Rectangle(textX, half + 1, Width - textX, half), Theme.TextSecondary, TextFormatFlags.Left Or TextFormatFlags.Top)
        End If
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then _timer.Dispose()
        MyBase.Dispose(disposing)
    End Sub

End Class

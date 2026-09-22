Imports System.ComponentModel
Imports System.Drawing.Drawing2D

''' <summary>วงแหวนนับถอยหลัง (แสดงวินาทีที่เหลือก่อนตรวจสอบผู้ป่วยรอบถัดไป)</summary>
Public Class CountdownRing
    Inherits Control

    Private _value As Integer = 5
    Private _maximum As Integer = 5
    Private _busy As Boolean
    Private _dark As Boolean
    Private _spin As Single
    Private WithEvents _spinTimer As New System.Windows.Forms.Timer() With {.Interval = 40}

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.Selectable, False)
        Size = New Size(32, 32)
    End Sub

    <DefaultValue(5)>
    Public Property Value As Integer
        Get
            Return _value
        End Get
        Set(v As Integer)
            _value = Math.Max(0, v)
            Invalidate()
        End Set
    End Property

    <DefaultValue(5)>
    Public Property Maximum As Integer
        Get
            Return _maximum
        End Get
        Set(v As Integer)
            _maximum = Math.Max(1, v)
            Invalidate()
        End Set
    End Property

    ''' <summary>กำลังโหลดข้อมูล (หมุนวน)</summary>
    <DefaultValue(False)>
    Public Property Busy As Boolean
        Get
            Return _busy
        End Get
        Set(v As Boolean)
            If _busy = v Then Return
            _busy = v
            If v Then _spinTimer.Start() Else _spinTimer.Stop()
            Invalidate()
        End Set
    End Property

    ''' <summary>ใช้บนพื้นหลังสีเข้ม (sidebar)</summary>
    <DefaultValue(False)>
    Public Property DarkBackground As Boolean
        Get
            Return _dark
        End Get
        Set(v As Boolean)
            _dark = v
            Invalidate()
        End Set
    End Property

    Private Sub OnSpin(sender As Object, e As EventArgs) Handles _spinTimer.Tick
        _spin = (_spin + 18) Mod 360
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        Gfx.PaintParentBackground(Me, g)
        Gfx.HighQuality(g)

        Dim stroke = Math.Max(2.5F, Theme.ScaleF(Me, 3))
        Dim dia = Math.Min(Width, Height) - stroke - 1
        Dim r As New RectangleF((Width - dia) / 2, (Height - dia) / 2, dia, dia)
        Dim track = If(_dark, Theme.SidebarHover, Theme.Border)
        Dim accent = If(_dark, Theme.SidebarAccent, Theme.Primary)

        Using p As New Pen(track, stroke)
            g.DrawEllipse(p, r)
        End Using
        Using p As New Pen(accent, stroke)
            p.StartCap = LineCap.Round
            p.EndCap = LineCap.Round
            If _busy Then
                g.DrawArc(p, r, _spin - 90, 100)
            Else
                Dim sweep = 360.0F * Math.Min(1.0F, CSng(_value) / _maximum)
                If sweep > 0.5F Then g.DrawArc(p, r, -90, sweep)
            End If
        End Using

        If Not _busy Then
            Dim f = Theme.UiFont(If(Width > 34, 9.0F, 7.5F), FontStyle.Bold)
            Gfx.Text(g, _value.ToString(), f, ClientRectangle, If(_dark, Color.White, Theme.TextPrimary),
                     TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.NoPadding)
        End If
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then _spinTimer.Dispose()
        MyBase.Dispose(disposing)
    End Sub

End Class

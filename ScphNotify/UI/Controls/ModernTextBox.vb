Imports System.ComponentModel

''' <summary>ช่องกรอกข้อความมุมโค้ง มีไอคอนนำหน้า / ปุ่มแสดงรหัสผ่าน / ข้อความตัวอย่าง</summary>
<DefaultEvent("TextChanged")>
Public Class ModernTextBox
    Inherits Control

    Private ReadOnly _tb As TextBox
    Private _icon As IconKind = IconKind.None
    Private _password As Boolean
    Private _reveal As Boolean
    Private _placeholder As String = ""
    Private _focused As Boolean
    Private _hover As Boolean

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        _tb = New TextBox() With {.BorderStyle = BorderStyle.None, .BackColor = Theme.Surface, .ForeColor = Theme.TextPrimary}
        Controls.Add(_tb)
        AddHandler _tb.TextChanged, Sub(s, e) OnTextChanged(e)
        AddHandler _tb.KeyDown, Sub(s, e) OnKeyDown(e)
        AddHandler _tb.KeyPress, Sub(s, e) OnKeyPress(e)
        AddHandler _tb.GotFocus, Sub(s, e)
                                     _focused = True
                                     Invalidate()
                                 End Sub
        AddHandler _tb.LostFocus, Sub(s, e)
                                      _focused = False
                                      Invalidate()
                                  End Sub
        AddHandler _tb.MouseEnter, Sub(s, e) SetHover(True)
        AddHandler _tb.MouseLeave, Sub(s, e) SetHover(ClientRectangle.Contains(PointToClient(Cursor.Position)))
        BackColor = Theme.Surface
        Size = New Size(240, 38)
        Cursor = Cursors.IBeam
    End Sub

    <Browsable(False)>
    Public ReadOnly Property InnerTextBox As TextBox
        Get
            Return _tb
        End Get
    End Property

    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue("")>
    Public Overrides Property Text As String
        Get
            If _tb Is Nothing Then Return ""
            Return _tb.Text
        End Get
        Set(value As String)
            If _tb IsNot Nothing Then _tb.Text = value
        End Set
    End Property

    <Category("Appearance"), DefaultValue("")>
    Public Property PlaceholderText As String
        Get
            Return _placeholder
        End Get
        Set(value As String)
            _placeholder = If(value, "")
            Compat.SetPlaceholder(_tb, _placeholder)
        End Set
    End Property

    <Category("Appearance"), DefaultValue(GetType(IconKind), "None")>
    Public Property IconKind As IconKind
        Get
            Return _icon
        End Get
        Set(value As IconKind)
            _icon = value
            PerformLayout()
            Invalidate()
        End Set
    End Property

    ''' <summary>ช่องรหัสผ่าน (มีปุ่มรูปตาเพื่อแสดง/ซ่อน)</summary>
    <Category("Behavior"), DefaultValue(False)>
    Public Property PasswordMode As Boolean
        Get
            Return _password
        End Get
        Set(value As Boolean)
            _password = value
            _tb.UseSystemPasswordChar = value AndAlso Not _reveal
            PerformLayout()
            Invalidate()
        End Set
    End Property

    <Category("Behavior"), DefaultValue(32767)>
    Public Property MaxLength As Integer
        Get
            Return _tb.MaxLength
        End Get
        Set(value As Integer)
            _tb.MaxLength = If(value <= 0, 32767, value)
        End Set
    End Property

    Public Sub SelectAll()
        _tb.SelectAll()
    End Sub

    Private Sub SetHover(v As Boolean)
        If _hover = v Then Return
        _hover = v
        Invalidate()
    End Sub

    Private ReadOnly Property EyeRect As Rectangle
        Get
            Dim s = Theme.Scale(Me, 28)
            Return New Rectangle(Width - s - Theme.Scale(Me, 6), (Height - s) \ 2, s, s)
        End Get
    End Property

    Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
        MyBase.OnLayout(levent)
        If _tb Is Nothing Then Return
        Dim left = Theme.Scale(Me, 12) + If(_icon <> IconKind.None, Theme.Scale(Me, 26), 0)
        Dim right = Theme.Scale(Me, 12) + If(_password, Theme.Scale(Me, 30), 0)
        _tb.Font = Font
        _tb.Location = New Point(left, (Height - _tb.PreferredHeight) \ 2 + 1)
        _tb.Width = Math.Max(10, Width - left - right)
    End Sub

    Protected Overrides Sub OnFontChanged(e As EventArgs)
        MyBase.OnFontChanged(e)
        PerformLayout()
    End Sub

    Protected Overrides Sub OnEnabledChanged(e As EventArgs)
        MyBase.OnEnabledChanged(e)
        _tb.Enabled = Enabled
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        SetHover(True)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        SetHover(False)
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        Cursor = If(_password AndAlso EyeRect.Contains(e.Location), Cursors.Hand, Cursors.IBeam)
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If _password AndAlso EyeRect.Contains(e.Location) Then
            _reveal = Not _reveal
            _tb.UseSystemPasswordChar = Not _reveal
            Invalidate()
        End If
        _tb.Focus()
    End Sub

    Protected Overrides Sub OnGotFocus(e As EventArgs)
        MyBase.OnGotFocus(e)
        _tb.Focus()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        Gfx.PaintParentBackground(Me, g)
        Gfx.HighQuality(g)
        Dim r As New RectangleF(0.5F, 0.5F, Width - 1.5F, Height - 1.5F)
        Dim radius = Theme.ScaleF(Me, 8)
        Dim back = If(Enabled, Theme.Surface, Theme.SurfaceSunken)
        _tb.BackColor = back
        Gfx.FillRounded(g, r, radius, back)
        If _focused Then
            Gfx.DrawRounded(g, RectangleF.Inflate(r, -0.5F, -0.5F), radius, Theme.Primary, 1.8F)
        Else
            Gfx.DrawRounded(g, r, radius, If(_hover, Theme.BorderStrong, Theme.Border))
        End If
        If _icon <> IconKind.None Then
            Dim s = Theme.ScaleF(Me, 18)
            IconPainter.Draw(g, _icon, New RectangleF(Theme.ScaleF(Me, 12), (Height - s) / 2, s, s), If(_focused, Theme.Primary, Theme.TextMuted), 2.0F)
        End If
        If _password Then
            Dim er = EyeRect
            Dim s = Theme.ScaleF(Me, 18)
            IconPainter.Draw(g, If(_reveal, IconKind.EyeOff, IconKind.Eye), New RectangleF(er.X + (er.Width - s) / 2, er.Y + (er.Height - s) / 2, s, s), Theme.TextMuted, 1.8F)
        End If
    End Sub

End Class

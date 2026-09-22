Imports System.ComponentModel

''' <summary>ตัวเลือกแบบแท็บแคปซูล (แทน TabControl / RadioButton แบบเดิม)</summary>
<DefaultEvent("SelectedIndexChanged")>
Public Class SegmentedControl
    Inherits Control

    Private _items As String() = {"ตัวเลือก 1", "ตัวเลือก 2"}
    Private _selected As Integer
    Private _hoverIndex As Integer = -1
    Private _filled As Boolean
    Private _selectedLevel As AlertLevel = AlertLevel.Primary

    Public Event SelectedIndexChanged As EventHandler

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.Selectable, True)
        Cursor = Cursors.Hand
        Size = New Size(240, 36)
        TabStop = True
    End Sub

    ' DesignerSerializationVisibility ต้องระบุให้ชัด ไม่งั้น analyzer ของ WinForms
    ' ฟ้อง WFO1000 เป็น error ตอน build (property ที่เป็น array/collection)
    <Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property Items As String()
        Get
            Return _items
        End Get
        Set(value As String())
            _items = If(value, New String() {})
            If _selected >= _items.Length Then _selected = Math.Max(0, _items.Length - 1)
            Invalidate()
        End Set
    End Property

    <Category("Behavior"), DefaultValue(0)>
    Public Property SelectedIndex As Integer
        Get
            Return _selected
        End Get
        Set(value As Integer)
            If _items.Length = 0 Then Return
            value = Math.Max(0, Math.Min(_items.Length - 1, value))
            If value = _selected Then Return
            _selected = value
            Invalidate()
            RaiseEvent SelectedIndexChanged(Me, EventArgs.Empty)
        End Set
    End Property

    ''' <summary>True = ช่องที่เลือกเป็นสีทึบ (ใช้กับคำถาม ใช่ / ไม่ใช่)</summary>
    <Category("Appearance"), DefaultValue(False)>
    Public Property FilledSelection As Boolean
        Get
            Return _filled
        End Get
        Set(value As Boolean)
            _filled = value
            Invalidate()
        End Set
    End Property

    ''' <summary>สีของช่องที่เลือกเมื่อ FilledSelection = True</summary>
    <Category("Appearance"), DefaultValue(GetType(AlertLevel), "Primary")>
    Public Property SelectedLevel As AlertLevel
        Get
            Return _selectedLevel
        End Get
        Set(value As AlertLevel)
            _selectedLevel = value
            Invalidate()
        End Set
    End Property

    Private Function SegmentRect(i As Integer) As RectangleF
        Dim pad = Theme.ScaleF(Me, 3)
        Dim w = (Width - pad * 2) / Math.Max(1, _items.Length)
        Return New RectangleF(pad + i * w, pad, w, Height - pad * 2)
    End Function

    Private Function HitTest(p As Point) As Integer
        For i = 0 To _items.Length - 1
            If SegmentRect(i).Contains(p) Then Return i
        Next
        Return -1
    End Function

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        Dim h = HitTest(e.Location)
        If h <> _hoverIndex Then _hoverIndex = h : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        _hoverIndex = -1
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button <> MouseButtons.Left Then Return
        Focus()
        Dim h = HitTest(e.Location)
        If h >= 0 Then SelectedIndex = h
    End Sub

    Protected Overrides Function IsInputKey(keyData As Keys) As Boolean
        If keyData = Keys.Left OrElse keyData = Keys.Right Then Return True
        Return MyBase.IsInputKey(keyData)
    End Function

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        If e.KeyCode = Keys.Left Then SelectedIndex -= 1
        If e.KeyCode = Keys.Right Then SelectedIndex += 1
    End Sub

    Protected Overrides Sub OnGotFocus(e As EventArgs)
        MyBase.OnGotFocus(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnLostFocus(e As EventArgs)
        MyBase.OnLostFocus(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        Gfx.PaintParentBackground(Me, g)
        Gfx.HighQuality(g)

        Dim outer As New RectangleF(0, 0, Width - 1, Height - 1)
        Dim radius = Theme.ScaleF(Me, 9)
        Gfx.FillRounded(g, outer, radius, Theme.SurfaceSunken)
        If Focused Then Gfx.DrawRounded(g, outer, radius, Theme.WithAlpha(Theme.Primary, 120), 1.2F)

        For i = 0 To _items.Length - 1
            Dim r = SegmentRect(i)
            Dim isSel = i = _selected
            Dim fg = Theme.TextSecondary
            If isSel Then
                If _filled Then
                    Dim back As Color, fore As Color, accent As Color
                    Theme.GetLevelColors(_selectedLevel, back, fore, accent)
                    Gfx.FillRounded(g, r, radius - 2, If(Enabled, accent, Theme.BorderStrong))
                    fg = Color.White
                Else
                    Gfx.FillRounded(g, New RectangleF(r.X, r.Y + 1, r.Width, r.Height), radius - 2, Color.FromArgb(18, 15, 23, 42))
                    Gfx.FillRounded(g, r, radius - 2, Theme.Surface)
                    fg = Theme.TextPrimary
                End If
            ElseIf i = _hoverIndex Then
                Gfx.FillRounded(g, r, radius - 2, Theme.Blend(Theme.SurfaceSunken, Theme.Border, 0.6))
            End If
            If Not Enabled Then fg = Theme.TextMuted
            Gfx.Text(g, _items(i), If(isSel, Theme.UiFont(Font.SizeInPoints, FontStyle.Bold), Font), Rectangle.Round(r), fg,
                     TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
        Next
    End Sub

End Class

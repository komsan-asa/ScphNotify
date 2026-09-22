Imports System.ComponentModel

''' <summary>ป้ายสถานะทรงแคปซูล (เช่น "Case HD", "eGFR &lt; 60")</summary>
Public Class StatusChip
    Inherits Control

    Private _level As AlertLevel = AlertLevel.Neutral
    Private _icon As IconKind = IconKind.None
    Private _compact As Boolean

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.Selectable, False)
        TabStop = False
        MyBase.AutoSize = True
        Size = New Size(80, 26)
    End Sub

    Public Sub New(text As String, level As AlertLevel, Optional icon As IconKind = IconKind.None)
        Me.New()
        _level = level
        _icon = icon
        Me.Text = text
    End Sub

    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(True),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Overrides Property AutoSize As Boolean
        Get
            Return MyBase.AutoSize
        End Get
        Set(value As Boolean)
            MyBase.AutoSize = value
            AdjustSize()
        End Set
    End Property

    <Category("Appearance"), DefaultValue(GetType(AlertLevel), "Neutral")>
    Public Property Level As AlertLevel
        Get
            Return _level
        End Get
        Set(value As AlertLevel)
            _level = value
            Invalidate()
        End Set
    End Property

    <Category("Appearance"), DefaultValue(GetType(IconKind), "None")>
    Public Property IconKind As IconKind
        Get
            Return _icon
        End Get
        Set(value As IconKind)
            _icon = value
            AdjustSize()
            Invalidate()
        End Set
    End Property

    ''' <summary>ขนาดเล็ก (ใช้บนแถบแจ้งเตือน)</summary>
    <Category("Appearance"), DefaultValue(False)>
    Public Property Compact As Boolean
        Get
            Return _compact
        End Get
        Set(value As Boolean)
            _compact = value
            AdjustSize()
            Invalidate()
        End Set
    End Property

    Private ReadOnly Property ChipFont As Font
        Get
            Return Theme.UiFont(If(_compact, 8.25F, 9.0F), FontStyle.Bold)
        End Get
    End Property

    Public Overrides Function GetPreferredSize(proposedSize As Size) As Size
        Dim ts = Gfx.Measure(Text, ChipFont)
        Dim padX = Theme.Scale(Me, If(_compact, 8, 10))
        Dim iconW = If(_icon <> IconKind.None, Theme.Scale(Me, If(_compact, 13, 15)) + Theme.Scale(Me, 5), 0)
        Dim h = Math.Max(ts.Height + Theme.Scale(Me, If(_compact, 6, 9)), Theme.Scale(Me, If(_compact, 20, 26)))
        Return New Size(ts.Width + padX * 2 + iconW, h)
    End Function

    Private Sub AdjustSize()
        If AutoSize Then Size = GetPreferredSize(Size.Empty)
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        AdjustSize()
        Invalidate()
    End Sub

    Protected Overrides Sub OnParentChanged(e As EventArgs)
        MyBase.OnParentChanged(e)
        AdjustSize()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        Gfx.PaintParentBackground(Me, g)
        Gfx.HighQuality(g)

        Dim back As Color, fore As Color, accent As Color
        Theme.GetLevelColors(_level, back, fore, accent)
        Dim r As New RectangleF(0, 0, Width - 1, Height - 1)
        Gfx.FillRounded(g, r, r.Height / 2, back)

        Dim padX = Theme.Scale(Me, If(_compact, 8, 10))
        Dim x = padX
        If _icon <> IconKind.None Then
            Dim s = Theme.ScaleF(Me, If(_compact, 13, 15))
            IconPainter.Draw(g, _icon, New RectangleF(x, (Height - s) / 2, s, s), accent, 2.2F)
            x += CInt(s) + Theme.Scale(Me, 5)
        End If
        Gfx.Text(g, Text, ChipFont, New Rectangle(x, 0, Width - x - padX + 4, Height), fore)
    End Sub

End Class

Imports System.ComponentModel

''' <summary>ListBox แบบวาดเอง: ไอคอนในกรอบสี + ข้อความหลัก + ข้อความรอง</summary>
Public Class ModernListBox
    Inherits ListBox

    Private _rowHeight As Integer = 46
    Private _defaultIcon As IconKind = IconKind.None
    Private _defaultLevel As AlertLevel = AlertLevel.Primary

    Public Sub New()
        DrawMode = DrawMode.OwnerDrawFixed
        BorderStyle = BorderStyle.None
        IntegralHeight = False
        BackColor = Theme.Surface
        ItemHeight = _rowHeight
    End Sub

    <Category("Appearance"), DefaultValue(46)>
    Public Property RowHeight As Integer
        Get
            Return _rowHeight
        End Get
        Set(value As Integer)
            _rowHeight = Math.Max(24, value)
            ItemHeight = Theme.Scale(Me, _rowHeight)
            Invalidate()
        End Set
    End Property

    <Category("Appearance"), DefaultValue(GetType(IconKind), "None")>
    Public Property DefaultIcon As IconKind
        Get
            Return _defaultIcon
        End Get
        Set(value As IconKind)
            _defaultIcon = value
            Invalidate()
        End Set
    End Property

    <Category("Appearance"), DefaultValue(GetType(AlertLevel), "Primary")>
    Public Property DefaultLevel As AlertLevel
        Get
            Return _defaultLevel
        End Get
        Set(value As AlertLevel)
            _defaultLevel = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        ItemHeight = Theme.Scale(Me, _rowHeight)
    End Sub

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        If e.Index < 0 OrElse e.Index >= Items.Count Then Return
        Dim g = e.Graphics
        Gfx.HighQuality(g)

        Dim item = Items(e.Index)
        Dim entry = TryCast(item, ListEntry)
        If entry Is Nothing Then entry = New ListEntry(Convert.ToString(item), "", _defaultIcon, _defaultLevel)
        Dim icon = If(entry.Icon = IconKind.None, _defaultIcon, entry.Icon)

        Dim selected = (e.State And DrawItemState.Selected) = DrawItemState.Selected
        Using bg As New SolidBrush(If(selected, Theme.SurfaceAlt, BackColor))
            g.FillRectangle(bg, e.Bounds)
        End Using

        Dim x = e.Bounds.X + Theme.Scale(Me, 2)
        If icon <> IconKind.None Then
            Dim back As Color, fore As Color, accent As Color
            Theme.GetLevelColors(If(entry.Icon = IconKind.None, _defaultLevel, entry.Level), back, fore, accent)
            Dim s = Theme.Scale(Me, 30)
            Dim chip As New RectangleF(x, e.Bounds.Y + (e.Bounds.Height - s) / 2.0F, s, s)
            Gfx.FillRounded(g, chip, Theme.ScaleF(Me, 8), back)
            Dim inset = Theme.ScaleF(Me, 7)
            IconPainter.Draw(g, icon, RectangleF.Inflate(chip, -inset, -inset), accent, 2.0F)
            x += s + Theme.Scale(Me, 12)
        End If

        Dim textW = e.Bounds.Right - x - Theme.Scale(Me, 6)
        If String.IsNullOrEmpty(entry.Subtitle) Then
            Gfx.Text(g, entry.Title, Font, New Rectangle(x, e.Bounds.Y, textW, e.Bounds.Height), Theme.TextPrimary)
        Else
            Dim half = e.Bounds.Height \ 2
            Gfx.Text(g, entry.Title, Font, New Rectangle(x, e.Bounds.Y, textW, half + 2), Theme.TextPrimary, TextFormatFlags.Left Or TextFormatFlags.Bottom)
            Gfx.Text(g, entry.Subtitle, Theme.UiFont(8.5F), New Rectangle(x, e.Bounds.Y + half + 1, textW, half - 1), Theme.TextSecondary, TextFormatFlags.Left Or TextFormatFlags.Top)
        End If

        If e.Index < Items.Count - 1 Then
            Using sep As New Pen(Theme.Border)
                g.DrawLine(sep, x, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1)
            End Using
        End If
    End Sub

End Class

''' <summary>รายการ 1 บรรทัดใน ModernListBox</summary>
Public Class ListEntry
    Public Sub New(title As String, Optional subtitle As String = "", Optional icon As IconKind = IconKind.None, Optional level As AlertLevel = AlertLevel.Neutral)
        Me.Title = title
        Me.Subtitle = subtitle
        Me.Icon = icon
        Me.Level = level
    End Sub

    Public Property Title As String
    Public Property Subtitle As String
    Public Property Icon As IconKind
    Public Property Level As AlertLevel
    Public Property Tag As Object

    Public Overrides Function ToString() As String
        Return Title
    End Function
End Class

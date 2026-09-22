''' <summary>สีเมนูคลิกขวา / เมนูถาดระบบ ให้เข้าชุดกับธีม</summary>
Public Class ModernMenuRenderer
    Inherits ToolStripProfessionalRenderer

    Public Sub New()
        MyBase.New(New MenuColors())
        RoundedEdges = True
    End Sub

    Protected Overrides Sub OnRenderMenuItemBackground(e As ToolStripItemRenderEventArgs)
        If Not e.Item.Selected OrElse Not e.Item.Enabled Then
            MyBase.OnRenderMenuItemBackground(e)
            Return
        End If
        Dim g = e.Graphics
        Gfx.HighQuality(g)
        Dim r As New RectangleF(4, 1, e.Item.Width - 8, e.Item.Height - 2)
        Gfx.FillRounded(g, r, 6, Theme.PrimarySoft)
    End Sub

    Protected Overrides Sub OnRenderItemText(e As ToolStripItemTextRenderEventArgs)
        e.TextColor = If(e.Item.Enabled, Theme.TextPrimary, Theme.TextMuted)
        MyBase.OnRenderItemText(e)
    End Sub

    Protected Overrides Sub OnRenderItemCheck(e As ToolStripItemImageRenderEventArgs)
        Dim g = e.Graphics
        Gfx.HighQuality(g)
        Dim r = e.ImageRectangle
        Gfx.FillRounded(g, New RectangleF(r.X - 2, r.Y - 2, r.Width + 4, r.Height + 4), 5, Theme.PrimarySoft)
        IconPainter.Draw(g, IconKind.Check, New RectangleF(r.X, r.Y, r.Width, r.Height), Theme.Primary, 2.4F)
    End Sub

    Private Class MenuColors
        Inherits ProfessionalColorTable

        Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
            Get
                Return Color.White
            End Get
        End Property
        Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
            Get
                Return Color.White
            End Get
        End Property
        Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
            Get
                Return Color.White
            End Get
        End Property
        Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
            Get
                Return Color.White
            End Get
        End Property
        Public Overrides ReadOnly Property MenuBorder As Color
            Get
                Return Theme.Border
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemBorder As Color
            Get
                Return Color.Transparent
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemSelected As Color
            Get
                Return Theme.PrimarySoft
            End Get
        End Property
        Public Overrides ReadOnly Property SeparatorDark As Color
            Get
                Return Theme.Border
            End Get
        End Property
        Public Overrides ReadOnly Property SeparatorLight As Color
            Get
                Return Color.White
            End Get
        End Property
    End Class

End Class

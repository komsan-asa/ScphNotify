Imports System.Reflection

''' <summary>ตกแต่ง DataGridView ให้เป็นตารางแบบเรียบ ทันสมัย (ไม่มีเส้นตั้ง, หัวตารางสีอ่อน, แถวสูง)</summary>
Public NotInheritable Class GridStyler

    Private Sub New()
    End Sub

    Public Shared Sub Apply(grid As DataGridView)
        With grid
            .BorderStyle = BorderStyle.None
            .BackgroundColor = Theme.Surface
            .GridColor = Theme.Border
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            .EnableHeadersVisualStyles = False
            .RowHeadersVisible = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AllowUserToOrderColumns = False
            .ReadOnly = True
            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            .ColumnHeadersHeight = Theme.Scale(grid, 40)
            .RowTemplate.Height = Theme.Scale(grid, 38)
            .StandardTab = True

            With .ColumnHeadersDefaultCellStyle
                .BackColor = Theme.SurfaceAlt
                .ForeColor = Theme.TextSecondary
                .SelectionBackColor = Theme.SurfaceAlt
                .SelectionForeColor = Theme.TextSecondary
                .Font = Theme.UiFont(9.0F, FontStyle.Bold)
                .Padding = New Padding(8, 0, 8, 0)
                .Alignment = DataGridViewContentAlignment.MiddleLeft
                .WrapMode = DataGridViewTriState.False
            End With

            With .DefaultCellStyle
                .BackColor = Theme.Surface
                .ForeColor = Theme.TextPrimary
                .SelectionBackColor = Theme.PrimarySoft
                .SelectionForeColor = Theme.TextPrimary
                .Font = Theme.UiFont(9.75F)
                .Padding = New Padding(8, 0, 8, 0)
                .WrapMode = DataGridViewTriState.False
            End With

            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 253)
        End With
        EnableDoubleBuffer(grid)
    End Sub

    ''' <summary>ลดการกระพริบตอนเลื่อนตาราง</summary>
    Public Shared Sub EnableDoubleBuffer(c As Control)
        Try
            Dim pi = GetType(Control).GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
            pi?.SetValue(c, True, Nothing)
        Catch
        End Try
    End Sub

    ''' <summary>เพิ่มคอลัมน์ข้อความ</summary>
    Public Shared Function AddTextColumn(grid As DataGridView, name As String, header As String, fillWeight As Single,
                                         Optional align As DataGridViewContentAlignment = DataGridViewContentAlignment.MiddleLeft,
                                         Optional minWidth As Integer = 60) As DataGridViewTextBoxColumn
        Dim col As New DataGridViewTextBoxColumn() With {
            .Name = name,
            .HeaderText = header,
            .FillWeight = fillWeight,
            .MinimumWidth = Theme.Scale(grid, minWidth),
            .SortMode = DataGridViewColumnSortMode.NotSortable
        }
        col.DefaultCellStyle.Alignment = align
        col.HeaderCell.Style.Alignment = align
        grid.Columns.Add(col)
        Return col
    End Function

End Class

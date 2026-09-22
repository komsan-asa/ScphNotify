Imports System.Drawing.Drawing2D
Imports System.Drawing.Printing

''' <summary>
''' พิมพ์ใบ LAB Hemodialysis ให้หน้าตาเหมือนไฟล์ PDF ของหน้า QHis2 (emr_html/fcontent_lab_hd.php)
'''   หัวเรื่อง : LAB Hemodialysis HN : xxx  ชื่อ : xxx
'''   ตาราง    : Index | LAB Name | วันที่รายงานผล ...  หัวตารางพื้นน้ำเงินเข้ม แถวสลับสี
''' ใช้ PrintDocument + PrintPreviewDialog — ไม่ต้องติดตั้ง runtime เพิ่ม
''' </summary>
Public NotInheritable Class LabHemodialysisPrinter

    Private Sub New()
    End Sub

    ' โทนสีให้ตรงกับไฟล์ PDF ตัวอย่าง
    Private Shared ReadOnly HeaderBack As Color = Color.FromArgb(47, 62, 82)
    Private Shared ReadOnly RowAlt As Color = Color.FromArgb(240, 242, 245)
    Private Shared ReadOnly GridLine As Color = Color.FromArgb(203, 213, 225)

    Private Class State
        Public Snap As PatientSnapshot
        Public Dates As List(Of Date)
        Public Rows As List(Of LabTemplateRow)
        Public RowIndex As Integer
        Public Page As Integer
        Public PrintedAt As DateTime
    End Class

    Public Shared Sub Preview(owner As IWin32Window, snap As PatientSnapshot,
                              dates As List(Of Date), rows As List(Of LabTemplateRow))
        If snap Is Nothing OrElse dates Is Nothing OrElse dates.Count = 0 Then Return

        Dim st As New State With {
            .Snap = snap,
            .Dates = dates,
            .Rows = If(rows, New List(Of LabTemplateRow))}

        Using doc As New PrintDocument()
            doc.DocumentName = $"LAB Hemodialysis HN {snap.Hn}"
            doc.DefaultPageSettings.Margins = New Margins(40, 40, 45, 45)
            ' คอลัมน์วันที่เยอะ → พิมพ์แนวนอนเพื่อให้อ่านได้ (ของเดิม 10 คอลัมน์พอดีแนวตั้ง)
            doc.DefaultPageSettings.Landscape = dates.Count > 10

            AddHandler doc.BeginPrint, Sub(s, e)
                                           st.RowIndex = 0
                                           st.Page = 0
                                           st.PrintedAt = DateTime.Now
                                       End Sub
            AddHandler doc.PrintPage, Sub(s, e) PrintPage(e, st)

            Using dlg As New PrintPreviewDialog()
                dlg.Document = doc
                dlg.UseAntiAlias = True
                dlg.Text = $"ตัวอย่างก่อนพิมพ์ — LAB Hemodialysis HN {snap.Hn}"
                dlg.Icon = AppSession.AppIcon
                dlg.StartPosition = FormStartPosition.CenterScreen
                dlg.Size = New Size(1040, 880)
                dlg.ShowDialog(owner)
            End Using
        End Using
    End Sub

    '──────────────── วาดหน้ากระดาษ ────────────────

    Private Shared Sub PrintPage(e As PrintPageEventArgs, st As State)
        Dim g = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
        st.Page += 1

        Dim m = e.MarginBounds
        Dim fontName = Theme.FontName
        Using fTitle As New Font(fontName, 13, FontStyle.Bold),
              fHead As New Font(fontName, 7.5F, FontStyle.Bold),
              fName As New Font(fontName, 8),
              fCell As New Font(fontName, 8),
              fFoot As New Font(fontName, 7.5F),
              brWhite As New SolidBrush(Color.White),
              brText As New SolidBrush(Color.FromArgb(30, 41, 59)),
              brMuted As New SolidBrush(Color.FromArgb(120, 130, 145)),
              brHead As New SolidBrush(HeaderBack),
              brAlt As New SolidBrush(RowAlt),
              penGrid As New Pen(GridLine, 0.6F)

            Dim center As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center, .Trimming = StringTrimming.EllipsisCharacter}
            Dim left As New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center, .Trimming = StringTrimming.EllipsisCharacter}

            Dim y As Single = m.Top

            ' ── หัวเรื่อง (กึ่งกลาง เหมือนไฟล์ PDF) ──
            Dim title = $"LAB Hemodialysis HN : {st.Snap.Hn}   ชื่อ : {st.Snap.PatientName}"
            Dim ts = g.MeasureString(title, fTitle)
            g.DrawString(title, fTitle, brText, m.Left + (m.Width - ts.Width) / 2, y)
            y += ts.Height + 10

            ' ── ความกว้างคอลัมน์ ──
            Dim n = st.Dates.Count
            Dim wIndex As Single = 38
            Dim wName As Single = Math.Min(190, Math.Max(110, m.Width * 0.22F))
            Dim wDate As Single = (m.Width - wIndex - wName) / Math.Max(1, n)
            Dim xs(n + 1) As Single
            xs(0) = m.Left
            xs(1) = xs(0) + wIndex
            xs(2) = xs(1) + wName
            For i = 0 To n - 1
                xs(i + 2) = xs(1) + wName + wDate * i
            Next

            ' ── หัวตาราง ──
            ' ย่อฟอนต์หัวคอลัมน์วันที่ให้พอดีความกว้าง จะได้ไม่ชนกันเมื่อมีหลายคอลัมน์
            Using fDate As Font = FitFont(g, fontName, "2026-12-31", wDate - 5, 7.5F, 5.0F)
                Dim headH As Single = 26
                g.FillRectangle(brHead, m.Left, y, m.Width, headH)
                g.DrawString("Index", fHead, brWhite, New RectangleF(xs(0), y, wIndex, headH), center)
                g.DrawString("LAB Name", fHead, brWhite, New RectangleF(xs(1) + 6, y, wName - 8, headH), left)
                For i = 0 To n - 1
                    g.DrawString(st.Dates(i).ToString("yyyy-MM-dd", Globalization.CultureInfo.InvariantCulture),
                                 fDate, brWhite, New RectangleF(xs(i + 2) + 2, y, wDate - 4, headH), center)
                Next
                y += headH
            End Using

            ' ── แถวข้อมูล ──
            Dim bottom As Single = m.Bottom - 22
            While st.RowIndex < st.Rows.Count
                Dim r = st.Rows(st.RowIndex)
                Dim rowH = MeasureRowHeight(g, r, n, wName - 8, wDate - 6, fName, fCell)
                If y + rowH > bottom Then Exit While

                If st.RowIndex Mod 2 = 0 Then g.FillRectangle(brAlt, m.Left, y, m.Width, rowH)

                g.DrawString(r.Index.ToString(Globalization.CultureInfo.InvariantCulture), fCell, brText,
                             New RectangleF(xs(0), y, wIndex, rowH), center)
                g.DrawString(r.LabName, fName, brText, New RectangleF(xs(1) + 6, y, wName - 8, rowH), left)
                For i = 0 To n - 1
                    Dim v = r.ValueAt(i)
                    If v <> "" Then g.DrawString(v, fCell, brText, New RectangleF(xs(i + 2) + 3, y, wDate - 6, rowH), center)
                Next

                ' เส้นตาราง
                g.DrawLine(penGrid, m.Left, y + rowH, m.Right, y + rowH)
                For i = 0 To n + 1
                    g.DrawLine(penGrid, xs(i), y, xs(i), y + rowH)
                Next
                g.DrawLine(penGrid, m.Right, y, m.Right, y + rowH)

                y += rowH
                st.RowIndex += 1
            End While

            ' ── ท้ายกระดาษ ──
            Dim foot = $"SCPH Notify · พิมพ์เมื่อ {ThaiDate.LongDate(st.PrintedAt)} {st.PrintedAt:HH:mm} น."
            g.DrawString(foot, fFoot, brMuted, m.Left, m.Bottom - 14)
            Dim pageText = $"หน้า {st.Page}"
            Dim pw = g.MeasureString(pageText, fFoot)
            g.DrawString(pageText, fFoot, brMuted, m.Right - pw.Width, m.Bottom - 14)

            e.HasMorePages = st.RowIndex < st.Rows.Count
        End Using
    End Sub

    ''' <summary>หาขนาดฟอนต์ที่ใหญ่ที่สุดที่ข้อความยังกว้างไม่เกินคอลัมน์</summary>
    Private Shared Function FitFont(g As Graphics, fontName As String, sample As String, maxWidth As Single,
                                    maxSize As Single, minSize As Single) As Font
        Dim size = maxSize
        While size > minSize
            Dim f As New Font(fontName, size, FontStyle.Bold)
            If g.MeasureString(sample, f).Width <= maxWidth Then Return f
            f.Dispose()
            size -= 0.25F
        End While
        Return New Font(fontName, minSize, FontStyle.Bold)
    End Function

    ''' <summary>ความสูงของแถว — ยืดตามข้อความที่ต้องตัดบรรทัด เช่น "Positive (334.0)"</summary>
    Private Shared Function MeasureRowHeight(g As Graphics, r As LabTemplateRow, dateCount As Integer,
                                             nameWidth As Single, cellWidth As Single,
                                             fName As Font, fCell As Font) As Single
        Dim h As Single = 20
        h = Math.Max(h, g.MeasureString(r.LabName, fName, CInt(Math.Max(40, nameWidth))).Height + 6)
        For i = 0 To dateCount - 1
            Dim v = r.ValueAt(i)
            If v = "" Then Continue For
            h = Math.Max(h, g.MeasureString(v, fCell, CInt(Math.Max(30, cellWidth))).Height + 6)
        Next
        Return h
    End Function

End Class

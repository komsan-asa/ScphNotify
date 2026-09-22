Imports System.Drawing.Drawing2D
Imports System.Drawing.Printing

''' <summary>
''' พิมพ์รายงานผล LAB (แทน Crystal Reports rptLabReport เดิม ซึ่งไม่รองรับ .NET 10)
''' ใช้ PrintDocument + PrintPreviewDialog ของ Windows Forms — ไม่ต้องติดตั้ง runtime เพิ่ม
''' </summary>
Public NotInheritable Class LabReportPrinter

    Private Sub New()
    End Sub

    Private Class State
        Public Snap As PatientSnapshot
        Public LabName As String
        Public Years As String
        Public Rows As List(Of LabResultRow)
        Public Chart As Bitmap
        Public RowIndex As Integer
        Public Page As Integer
        Public PrintedAt As DateTime
    End Class

    Public Shared Sub Preview(owner As IWin32Window, snap As PatientSnapshot, labName As String, years As String,
                              rows As List(Of LabResultRow), chart As LineChart)
        Dim st As New State With {
            .Snap = snap, .LabName = labName, .Years = years,
            .Rows = If(rows, New List(Of LabResultRow)),
            .Chart = If(chart IsNot Nothing AndAlso chart.Points.Count > 0, chart.RenderBitmap(760, 280, 2.0F), Nothing)}

        Using doc As New PrintDocument()
            doc.DocumentName = $"LAB {labName} HN {snap?.Hn}"
            doc.DefaultPageSettings.Margins = New Margins(50, 50, 50, 50)
            AddHandler doc.BeginPrint, Sub(s, e)
                                           st.RowIndex = 0
                                           st.Page = 0
                                           st.PrintedAt = DateTime.Now
                                       End Sub
            AddHandler doc.PrintPage, Sub(s, e) PrintPage(e, st)

            Using dlg As New PrintPreviewDialog()
                dlg.Document = doc
                dlg.UseAntiAlias = True
                dlg.Text = $"ตัวอย่างก่อนพิมพ์ — {labName}"
                dlg.Icon = AppSession.AppIcon
                dlg.StartPosition = FormStartPosition.CenterScreen
                dlg.Size = New Size(1000, 860)
                dlg.ShowDialog(owner)
            End Using
        End Using
        st.Chart?.Dispose()
    End Sub

    Private Shared Sub PrintPage(e As PrintPageEventArgs, st As State)
        Dim g = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
        st.Page += 1

        Dim m = e.MarginBounds
        Dim fontName = Theme.FontName
        Using fTitle As New Font(fontName, 16, FontStyle.Bold),
              fSub As New Font(fontName, 9),
              fBody As New Font(fontName, 10),
              fBold As New Font(fontName, 10, FontStyle.Bold),
              fHead As New Font(fontName, 9, FontStyle.Bold),
              brText As New SolidBrush(Theme.TextPrimary),
              brMuted As New SolidBrush(Theme.TextSecondary),
              brAccent As New SolidBrush(Theme.Primary),
              penLine As New Pen(Theme.Border, 1)

            Dim y As Single = m.Top

            ' ── หัวรายงาน ──
            g.FillRectangle(brAccent, m.Left, y, 6, 30)
            g.DrawString("รายงานผลตรวจทางห้องปฏิบัติการ", fTitle, brText, m.Left + 14, y)
            Dim printed = $"SCPH Notify · พิมพ์เมื่อ {ThaiDate.LongDate(st.PrintedAt)} {st.PrintedAt:HH:mm} น."
            Dim ps = g.MeasureString(printed, fSub)
            g.DrawString(printed, fSub, brMuted, m.Right - ps.Width, y + 8)
            y += 42

            ' ── กล่องข้อมูลผู้ป่วย ──
            Dim box As New RectangleF(m.Left, y, m.Width, 52)
            Gfx.FillRounded(g, box, 8, Theme.SurfaceAlt)
            Gfx.DrawRounded(g, box, 8, Theme.Border)
            Dim s = st.Snap
            Dim line1 = If(s Is Nothing, "-", $"ผู้ป่วย: {s.PatientName}      HN: {s.Hn}      {s.AgeText}")
            Dim line2 = $"รายการ: {st.LabName}      ช่วงปี พ.ศ. {st.Years}      จำนวน {st.Rows.Count} รายการ"
            g.DrawString(line1, fBold, brText, box.X + 12, box.Y + 7)
            g.DrawString(line2, fBody, brMuted, box.X + 12, box.Y + 28)
            y += 64

            ' ── กราฟ (หน้าแรก) ──
            If st.Page = 1 AndAlso st.Chart IsNot Nothing Then
                Dim h = CSng(m.Width * st.Chart.Height / st.Chart.Width)
                g.DrawImage(st.Chart, New RectangleF(m.Left, y, m.Width, h))
                g.DrawRectangle(penLine, m.Left, y, m.Width, h)
                y += h + 14
            End If

            ' ── ตาราง ──
            Dim cols = {("วันที่รายงาน", 0.18F), ("เลขที่สั่ง", 0.15F), ("ผล", 0.13F), ("ค่าปกติ", 0.22F), ("Flag", 0.08F), ("กลุ่ม", 0.24F)}
            Dim rowH As Single = 24
            Dim headerRect As New RectangleF(m.Left, y, m.Width, rowH)
            Using brHead As New SolidBrush(Theme.SurfaceSunken)
                g.FillRectangle(brHead, headerRect)
            End Using
            Dim x As Single = m.Left
            For Each c In cols
                g.DrawString(c.Item1, fHead, brMuted, x + 6, y + 5)
                x += m.Width * c.Item2
            Next
            y += rowH

            Dim bottom = m.Bottom - 30
            Using brAlt As New SolidBrush(Color.FromArgb(250, 251, 253)), brDanger As New SolidBrush(Theme.Danger)
                While st.RowIndex < st.Rows.Count AndAlso y + rowH <= bottom
                    Dim r = st.Rows(st.RowIndex)
                    If st.RowIndex Mod 2 = 1 Then g.FillRectangle(brAlt, m.Left, y, m.Width, rowH)
                    g.DrawLine(penLine, m.Left, y + rowH, m.Right, y + rowH)
                    Dim values = {ThaiDate.MediumDate(r.ReportDate), r.OrderNumber, r.Result, r.NormalValue, r.Flag, r.SubGroup}
                    x = m.Left
                    Dim flagged = r.Flag.Trim() <> ""
                    For i = 0 To cols.Length - 1
                        Dim w = m.Width * cols(i).Item2
                        Dim cell As New RectangleF(x + 6, y + 4, w - 10, rowH - 6)
                        Dim fnt = If(i = 2, fBold, fBody)
                        Dim br = If(flagged AndAlso (i = 2 OrElse i = 4), CType(brDanger, Brush), brText)
                        Using sf As New StringFormat() With {.Trimming = StringTrimming.EllipsisCharacter, .FormatFlags = StringFormatFlags.NoWrap}
                            g.DrawString(values(i), fnt, br, cell, sf)
                        End Using
                        x += w
                    Next
                    y += rowH
                    st.RowIndex += 1
                End While
            End Using

            If st.Rows.Count = 0 Then
                g.DrawString("ไม่มีข้อมูลผล LAB ในช่วงที่เลือก", fBody, brMuted, m.Left + 6, y + 6)
            End If

            ' ── ท้ายหน้า ──
            g.DrawLine(penLine, m.Left, m.Bottom - 16, m.Right, m.Bottom - 16)
            g.DrawString("เอกสารนี้พิมพ์จากระบบ SCPH Notify (ข้อมูลจาก HOSxP)", fSub, brMuted, m.Left, m.Bottom - 12)
            Dim pageText = $"หน้า {st.Page}"
            g.DrawString(pageText, fSub, brMuted, m.Right - g.MeasureString(pageText, fSub).Width, m.Bottom - 12)
        End Using

        e.HasMorePages = st.RowIndex < st.Rows.Count
    End Sub

End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DrugPage
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        pnlPageHeader = New Panel()
        lblSubtitle = New Label()
        lblTitle = New Label()
        cardPlaceholder = New CardPanel()
        emptyDrug = New EmptyState()
        pnlPageHeader.SuspendLayout()
        cardPlaceholder.SuspendLayout()
        SuspendLayout()
        '
        'pnlPageHeader
        '
        pnlPageHeader.Controls.Add(lblSubtitle)
        pnlPageHeader.Controls.Add(lblTitle)
        pnlPageHeader.Dock = DockStyle.Top
        pnlPageHeader.Location = New Point(0, 0)
        pnlPageHeader.Name = "pnlPageHeader"
        pnlPageHeader.Size = New Size(900, 66)
        pnlPageHeader.TabIndex = 0
        '
        'lblSubtitle
        '
        lblSubtitle.AutoSize = True
        lblSubtitle.Font = New Font("Leelawadee UI", 9.5F)
        lblSubtitle.ForeColor = Color.FromArgb(CByte(71), CByte(85), CByte(105))
        lblSubtitle.Location = New Point(1, 33)
        lblSubtitle.Name = "lblSubtitle"
        lblSubtitle.Size = New Size(300, 17)
        lblSubtitle.TabIndex = 1
        lblSubtitle.Text = "ติดตามการใช้ยาที่ต้องระวังในผู้ป่วยโรคเรื้อรัง"
        '
        'lblTitle
        '
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Leelawadee UI", 16.0F, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        lblTitle.Location = New Point(-2, 0)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(160, 30)
        lblTitle.TabIndex = 0
        lblTitle.Text = "Drug Monitor"
        '
        'cardPlaceholder
        '
        cardPlaceholder.Controls.Add(emptyDrug)
        cardPlaceholder.Dock = DockStyle.Fill
        cardPlaceholder.Location = New Point(0, 66)
        cardPlaceholder.Name = "cardPlaceholder"
        cardPlaceholder.Size = New Size(900, 534)
        cardPlaceholder.TabIndex = 1
        '
        'emptyDrug
        '
        emptyDrug.BackColor = Color.White
        emptyDrug.Description = "โมดูลนี้ยังอยู่ระหว่างพัฒนา (ในระบบเดิมเมนูนี้ถูกปิดไว้เช่นกัน) — เตรียมโครงหน้าไว้สำหรับต่อยอด"
        emptyDrug.Dock = DockStyle.Fill
        emptyDrug.IconKind = IconKind.Pill
        emptyDrug.Location = New Point(16, 16)
        emptyDrug.Name = "emptyDrug"
        emptyDrug.Size = New Size(868, 502)
        emptyDrug.TabIndex = 0
        emptyDrug.Title = "เร็วๆ นี้"
        '
        'DrugPage
        '
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        BackColor = Color.FromArgb(CByte(244), CByte(246), CByte(250))
        Controls.Add(cardPlaceholder)
        Controls.Add(pnlPageHeader)
        Font = New Font("Leelawadee UI", 10.0F)
        Name = "DrugPage"
        Size = New Size(900, 600)
        pnlPageHeader.ResumeLayout(False)
        pnlPageHeader.PerformLayout()
        cardPlaceholder.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlPageHeader As Panel
    Friend WithEvents lblSubtitle As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents cardPlaceholder As CardPanel
    Friend WithEvents emptyDrug As EmptyState
End Class

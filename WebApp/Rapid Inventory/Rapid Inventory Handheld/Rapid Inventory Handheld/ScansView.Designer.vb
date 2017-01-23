<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ScansView
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Private mainMenu1 As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.dataGridScans = New System.Windows.Forms.DataGrid
        Me.btnEditScan = New System.Windows.Forms.Button
        Me.btnDone = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'dataGridScans
        '
        Me.dataGridScans.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.dataGridScans.Location = New System.Drawing.Point(3, 26)
        Me.dataGridScans.Name = "dataGridScans"
        Me.dataGridScans.Size = New System.Drawing.Size(234, 213)
        Me.dataGridScans.TabIndex = 3
        '
        'btnEditScan
        '
        Me.btnEditScan.Location = New System.Drawing.Point(3, 245)
        Me.btnEditScan.Name = "btnEditScan"
        Me.btnEditScan.Size = New System.Drawing.Size(109, 20)
        Me.btnEditScan.TabIndex = 4
        Me.btnEditScan.Text = "Edit Scan"
        '
        'btnDone
        '
        Me.btnDone.Location = New System.Drawing.Point(128, 245)
        Me.btnDone.Name = "btnDone"
        Me.btnDone.Size = New System.Drawing.Size(109, 20)
        Me.btnDone.TabIndex = 5
        Me.btnDone.Text = "Done"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(3, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(234, 20)
        Me.Label1.Text = "Section #1"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ScansView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnDone)
        Me.Controls.Add(Me.btnEditScan)
        Me.Controls.Add(Me.dataGridScans)
        Me.Menu = Me.mainMenu1
        Me.Name = "ScansView"
        Me.Text = "ScansView"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dataGridScans As System.Windows.Forms.DataGrid
    Friend WithEvents btnEditScan As System.Windows.Forms.Button
    Friend WithEvents btnDone As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class

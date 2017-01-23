<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class CountMenu
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
        Me.chkboxPromptForQty = New System.Windows.Forms.CheckBox
        Me.lblSectionNumber = New System.Windows.Forms.Label
        Me.btnDone = New System.Windows.Forms.Button
        Me.lblViewScans = New System.Windows.Forms.LinkLabel
        Me.txtBoxBarcode = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'chkboxPromptForQty
        '
        Me.chkboxPromptForQty.Location = New System.Drawing.Point(3, 3)
        Me.chkboxPromptForQty.Name = "chkboxPromptForQty"
        Me.chkboxPromptForQty.Size = New System.Drawing.Size(110, 20)
        Me.chkboxPromptForQty.TabIndex = 0
        Me.chkboxPromptForQty.Text = "Promt for Qty"
        '
        'lblSectionNumber
        '
        Me.lblSectionNumber.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblSectionNumber.Location = New System.Drawing.Point(119, 3)
        Me.lblSectionNumber.Name = "lblSectionNumber"
        Me.lblSectionNumber.Size = New System.Drawing.Size(118, 20)
        Me.lblSectionNumber.Text = "Section #"
        '
        'btnDone
        '
        Me.btnDone.Location = New System.Drawing.Point(86, 245)
        Me.btnDone.Name = "btnDone"
        Me.btnDone.Size = New System.Drawing.Size(72, 20)
        Me.btnDone.TabIndex = 3
        Me.btnDone.Text = "Done"
        '
        'lblViewScans
        '
        Me.lblViewScans.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Underline)
        Me.lblViewScans.Location = New System.Drawing.Point(53, 187)
        Me.lblViewScans.Name = "lblViewScans"
        Me.lblViewScans.Size = New System.Drawing.Size(133, 29)
        Me.lblViewScans.TabIndex = 5
        Me.lblViewScans.Text = "View Scans"
        Me.lblViewScans.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtBoxBarcode
        '
        Me.txtBoxBarcode.Location = New System.Drawing.Point(27, 95)
        Me.txtBoxBarcode.Name = "txtBoxBarcode"
        Me.txtBoxBarcode.Size = New System.Drawing.Size(188, 21)
        Me.txtBoxBarcode.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(53, 72)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(133, 20)
        Me.Label1.Text = "Scan Next Item"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'CountMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtBoxBarcode)
        Me.Controls.Add(Me.lblViewScans)
        Me.Controls.Add(Me.btnDone)
        Me.Controls.Add(Me.lblSectionNumber)
        Me.Controls.Add(Me.chkboxPromptForQty)
        Me.KeyPreview = True
        Me.Menu = Me.mainMenu1
        Me.Name = "CountMenu"
        Me.Text = "Count"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chkboxPromptForQty As System.Windows.Forms.CheckBox
    Friend WithEvents lblSectionNumber As System.Windows.Forms.Label
    Friend WithEvents btnDone As System.Windows.Forms.Button
    Friend WithEvents lblViewScans As System.Windows.Forms.LinkLabel
    Friend WithEvents txtBoxBarcode As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class

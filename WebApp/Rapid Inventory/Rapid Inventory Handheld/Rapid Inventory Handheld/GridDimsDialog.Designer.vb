<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class GridDimsDialog
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.cmbBoxGridDim3 = New System.Windows.Forms.ComboBox
        Me.cmbBoxGridDim2 = New System.Windows.Forms.ComboBox
        Me.cmbBoxGridDim1 = New System.Windows.Forms.ComboBox
        Me.txtboxGridDim3 = New System.Windows.Forms.TextBox
        Me.txtboxGridDim2 = New System.Windows.Forms.TextBox
        Me.txtboxGridDim1 = New System.Windows.Forms.TextBox
        Me.lblBarcode = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnForce = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cmbBoxGridDim3)
        Me.Panel1.Controls.Add(Me.cmbBoxGridDim2)
        Me.Panel1.Controls.Add(Me.cmbBoxGridDim1)
        Me.Panel1.Controls.Add(Me.txtboxGridDim3)
        Me.Panel1.Controls.Add(Me.txtboxGridDim2)
        Me.Panel1.Controls.Add(Me.txtboxGridDim1)
        Me.Panel1.Controls.Add(Me.lblBarcode)
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.btnForce)
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(234, 262)
        '
        'cmbBoxGridDim3
        '
        Me.cmbBoxGridDim3.Location = New System.Drawing.Point(40, 163)
        Me.cmbBoxGridDim3.Name = "cmbBoxGridDim3"
        Me.cmbBoxGridDim3.Size = New System.Drawing.Size(152, 22)
        Me.cmbBoxGridDim3.TabIndex = 9
        '
        'cmbBoxGridDim2
        '
        Me.cmbBoxGridDim2.Location = New System.Drawing.Point(40, 102)
        Me.cmbBoxGridDim2.Name = "cmbBoxGridDim2"
        Me.cmbBoxGridDim2.Size = New System.Drawing.Size(152, 22)
        Me.cmbBoxGridDim2.TabIndex = 8
        '
        'cmbBoxGridDim1
        '
        Me.cmbBoxGridDim1.Location = New System.Drawing.Point(40, 41)
        Me.cmbBoxGridDim1.Name = "cmbBoxGridDim1"
        Me.cmbBoxGridDim1.Size = New System.Drawing.Size(152, 22)
        Me.cmbBoxGridDim1.TabIndex = 7
        '
        'txtboxGridDim3
        '
        Me.txtboxGridDim3.ForeColor = System.Drawing.Color.Silver
        Me.txtboxGridDim3.Location = New System.Drawing.Point(40, 191)
        Me.txtboxGridDim3.Name = "txtboxGridDim3"
        Me.txtboxGridDim3.Size = New System.Drawing.Size(152, 21)
        Me.txtboxGridDim3.TabIndex = 5
        '
        'txtboxGridDim2
        '
        Me.txtboxGridDim2.ForeColor = System.Drawing.Color.Silver
        Me.txtboxGridDim2.Location = New System.Drawing.Point(40, 130)
        Me.txtboxGridDim2.Name = "txtboxGridDim2"
        Me.txtboxGridDim2.Size = New System.Drawing.Size(152, 21)
        Me.txtboxGridDim2.TabIndex = 4
        '
        'txtboxGridDim1
        '
        Me.txtboxGridDim1.ForeColor = System.Drawing.Color.Silver
        Me.txtboxGridDim1.Location = New System.Drawing.Point(40, 66)
        Me.txtboxGridDim1.Name = "txtboxGridDim1"
        Me.txtboxGridDim1.Size = New System.Drawing.Size(152, 21)
        Me.txtboxGridDim1.TabIndex = 3
        '
        'lblBarcode
        '
        Me.lblBarcode.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblBarcode.Location = New System.Drawing.Point(40, 9)
        Me.lblBarcode.Name = "lblBarcode"
        Me.lblBarcode.Size = New System.Drawing.Size(152, 20)
        Me.lblBarcode.Text = "Barcode"
        Me.lblBarcode.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(120, 221)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(72, 20)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        'btnForce
        '
        Me.btnForce.Location = New System.Drawing.Point(40, 221)
        Me.btnForce.Name = "btnForce"
        Me.btnForce.Size = New System.Drawing.Size(72, 20)
        Me.btnForce.TabIndex = 0
        Me.btnForce.Text = "Force"
        '
        'GridDimsDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Menu = Me.mainMenu1
        Me.Name = "GridDimsDialog"
        Me.Text = "SelectOptionDialog"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnForce As System.Windows.Forms.Button
    Friend WithEvents lblBarcode As System.Windows.Forms.Label
    Friend WithEvents txtboxGridDim1 As System.Windows.Forms.TextBox
    Friend WithEvents txtboxGridDim3 As System.Windows.Forms.TextBox
    Friend WithEvents txtboxGridDim2 As System.Windows.Forms.TextBox
    Friend WithEvents cmbBoxGridDim1 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbBoxGridDim3 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbBoxGridDim2 As System.Windows.Forms.ComboBox
End Class

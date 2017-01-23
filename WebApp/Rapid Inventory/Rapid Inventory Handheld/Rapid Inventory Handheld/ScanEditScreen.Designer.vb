<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ScanEditScreen
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
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.txtBoxGridDim3 = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtBoxGridDim2 = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblBarcode = New System.Windows.Forms.Label
        Me.txtBoxGridDim1 = New System.Windows.Forms.TextBox
        Me.txtBoxScanDescr = New System.Windows.Forms.TextBox
        Me.txtboxScanQty = New System.Windows.Forms.TextBox
        Me.txtboxItemNo = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(3, 239)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(102, 20)
        Me.btnUpdate.TabIndex = 0
        Me.btnUpdate.Text = "Update"
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(129, 239)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(102, 20)
        Me.btnDelete.TabIndex = 1
        Me.btnDelete.Text = "Delete"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txtBoxScanDescr)
        Me.Panel1.Controls.Add(Me.txtboxScanQty)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.txtboxItemNo)
        Me.Panel1.Controls.Add(Me.txtBoxGridDim3)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.txtBoxGridDim2)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.lblBarcode)
        Me.Panel1.Controls.Add(Me.txtBoxGridDim1)
        Me.Panel1.Controls.Add(Me.btnDelete)
        Me.Panel1.Controls.Add(Me.btnUpdate)
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(234, 262)
        '
        'txtBoxGridDim3
        '
        Me.txtBoxGridDim3.Location = New System.Drawing.Point(15, 202)
        Me.txtBoxGridDim3.Name = "txtBoxGridDim3"
        Me.txtBoxGridDim3.Size = New System.Drawing.Size(90, 21)
        Me.txtBoxGridDim3.TabIndex = 11
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.Label5.Location = New System.Drawing.Point(15, 177)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(90, 22)
        Me.Label5.Text = "Dim 3"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.Label1.Location = New System.Drawing.Point(129, 124)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 22)
        Me.Label1.Text = "Dim 2"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtBoxGridDim2
        '
        Me.txtBoxGridDim2.Location = New System.Drawing.Point(129, 149)
        Me.txtBoxGridDim2.Name = "txtBoxGridDim2"
        Me.txtBoxGridDim2.Size = New System.Drawing.Size(90, 21)
        Me.txtBoxGridDim2.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.Label4.Location = New System.Drawing.Point(15, 124)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(90, 22)
        Me.Label4.Text = "Dim 1"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.Label3.Location = New System.Drawing.Point(15, 79)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(205, 22)
        Me.Label3.Text = "Description"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.Label2.Location = New System.Drawing.Point(158, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 22)
        Me.Label2.Text = "Qty"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblBarcode
        '
        Me.lblBarcode.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblBarcode.Location = New System.Drawing.Point(3, 10)
        Me.lblBarcode.Name = "lblBarcode"
        Me.lblBarcode.Size = New System.Drawing.Size(228, 20)
        Me.lblBarcode.Text = "12445826414"
        Me.lblBarcode.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtBoxGridDim1
        '
        Me.txtBoxGridDim1.Location = New System.Drawing.Point(15, 149)
        Me.txtBoxGridDim1.Name = "txtBoxGridDim1"
        Me.txtBoxGridDim1.Size = New System.Drawing.Size(90, 21)
        Me.txtBoxGridDim1.TabIndex = 4
        '
        'txtBoxScanDescr
        '
        Me.txtBoxScanDescr.Location = New System.Drawing.Point(15, 100)
        Me.txtBoxScanDescr.Name = "txtBoxScanDescr"
        Me.txtBoxScanDescr.Size = New System.Drawing.Size(205, 21)
        Me.txtBoxScanDescr.TabIndex = 3
        '
        'txtboxScanQty
        '
        Me.txtboxScanQty.Location = New System.Drawing.Point(158, 55)
        Me.txtboxScanQty.Name = "txtboxScanQty"
        Me.txtboxScanQty.Size = New System.Drawing.Size(44, 21)
        Me.txtboxScanQty.TabIndex = 2
        '
        'txtboxItemNo
        '
        Me.txtboxItemNo.Location = New System.Drawing.Point(15, 55)
        Me.txtboxItemNo.Name = "txtboxItemNo"
        Me.txtboxItemNo.Size = New System.Drawing.Size(90, 21)
        Me.txtboxItemNo.TabIndex = 18
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.Label6.Location = New System.Drawing.Point(15, 30)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(90, 22)
        Me.Label6.Text = "Item No"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ScanEditScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Menu = Me.mainMenu1
        Me.Name = "ScanEditScreen"
        Me.Text = "ScanDialog"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents txtboxScanQty As System.Windows.Forms.TextBox
    Friend WithEvents lblBarcode As System.Windows.Forms.Label
    Friend WithEvents txtBoxGridDim1 As System.Windows.Forms.TextBox
    Friend WithEvents txtBoxScanDescr As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtBoxGridDim2 As System.Windows.Forms.TextBox
    Friend WithEvents txtBoxGridDim3 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtboxItemNo As System.Windows.Forms.TextBox
End Class

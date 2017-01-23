<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ForceDialog
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
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnForce = New System.Windows.Forms.Button
        Me.lblForceNotification = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.btnForce)
        Me.Panel1.Controls.Add(Me.lblForceNotification)
        Me.Panel1.Location = New System.Drawing.Point(16, 75)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(208, 119)
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(108, 80)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(97, 36)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        '
        'btnForce
        '
        Me.btnForce.Location = New System.Drawing.Point(3, 80)
        Me.btnForce.Name = "btnForce"
        Me.btnForce.Size = New System.Drawing.Size(99, 36)
        Me.btnForce.TabIndex = 1
        Me.btnForce.Text = "Force"
        '
        'lblForceNotification
        '
        Me.lblForceNotification.Location = New System.Drawing.Point(3, 11)
        Me.lblForceNotification.Name = "lblForceNotification"
        Me.lblForceNotification.Size = New System.Drawing.Size(202, 66)
        Me.lblForceNotification.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ForceDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.Panel1)
        Me.Menu = Me.mainMenu1
        Me.Name = "ForceDialog"
        Me.Text = "ForceDialog"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnForce As System.Windows.Forms.Button
    Friend WithEvents lblForceNotification As System.Windows.Forms.Label
End Class

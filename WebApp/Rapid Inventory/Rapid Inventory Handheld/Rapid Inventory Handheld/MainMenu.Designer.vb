<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class MainMenu
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
        Me.btnCount = New System.Windows.Forms.Button
        Me.btnUploadCount = New System.Windows.Forms.Button
        Me.btnClearAllData = New System.Windows.Forms.Button
        Me.btnLogOut = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnCount
        '
        Me.btnCount.Location = New System.Drawing.Point(0, 0)
        Me.btnCount.Name = "btnCount"
        Me.btnCount.Size = New System.Drawing.Size(240, 72)
        Me.btnCount.TabIndex = 0
        Me.btnCount.Text = "Count"
        '
        'btnUploadCount
        '
        Me.btnUploadCount.Location = New System.Drawing.Point(0, 68)
        Me.btnUploadCount.Name = "btnUploadCount"
        Me.btnUploadCount.Size = New System.Drawing.Size(240, 72)
        Me.btnUploadCount.TabIndex = 1
        Me.btnUploadCount.Text = "Upload Count"
        '
        'btnClearAllData
        '
        Me.btnClearAllData.Location = New System.Drawing.Point(0, 136)
        Me.btnClearAllData.Name = "btnClearAllData"
        Me.btnClearAllData.Size = New System.Drawing.Size(240, 72)
        Me.btnClearAllData.TabIndex = 2
        Me.btnClearAllData.Text = "Clear All Data"
        '
        'btnLogOut
        '
        Me.btnLogOut.Location = New System.Drawing.Point(0, 205)
        Me.btnLogOut.Name = "btnLogOut"
        Me.btnLogOut.Size = New System.Drawing.Size(240, 72)
        Me.btnLogOut.TabIndex = 3
        Me.btnLogOut.Text = "Log Out"
        '
        'MainMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(253, 268)
        Me.Controls.Add(Me.btnLogOut)
        Me.Controls.Add(Me.btnClearAllData)
        Me.Controls.Add(Me.btnUploadCount)
        Me.Controls.Add(Me.btnCount)
        Me.Menu = Me.mainMenu1
        Me.Name = "MainMenu"
        Me.Text = "Main Menu"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCount As System.Windows.Forms.Button
    Friend WithEvents btnUploadCount As System.Windows.Forms.Button
    Friend WithEvents btnClearAllData As System.Windows.Forms.Button
    Friend WithEvents btnLogOut As System.Windows.Forms.Button
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class LoginScreen
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
        Me.btnLogin = New System.Windows.Forms.Button
        Me.btnRefreshLogins = New System.Windows.Forms.Button
        Me.comboBoxCompLoc = New System.Windows.Forms.ComboBox
        Me.txtBoxPassword = New System.Windows.Forms.TextBox
        Me.txtBoxUserName = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnLogin
        '
        Me.btnLogin.Location = New System.Drawing.Point(37, 223)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(166, 28)
        Me.btnLogin.TabIndex = 4
        Me.btnLogin.Text = "Login"
        '
        'btnRefreshLogins
        '
        Me.btnRefreshLogins.Location = New System.Drawing.Point(37, 189)
        Me.btnRefreshLogins.Name = "btnRefreshLogins"
        Me.btnRefreshLogins.Size = New System.Drawing.Size(166, 28)
        Me.btnRefreshLogins.TabIndex = 3
        Me.btnRefreshLogins.Text = "Refresh Logins"
        '
        'comboBoxCompLoc
        '
        Me.comboBoxCompLoc.Location = New System.Drawing.Point(37, 129)
        Me.comboBoxCompLoc.Name = "comboBoxCompLoc"
        Me.comboBoxCompLoc.Size = New System.Drawing.Size(166, 22)
        Me.comboBoxCompLoc.TabIndex = 2
        '
        'txtBoxPassword
        '
        Me.txtBoxPassword.Location = New System.Drawing.Point(37, 82)
        Me.txtBoxPassword.Name = "txtBoxPassword"
        Me.txtBoxPassword.Size = New System.Drawing.Size(166, 21)
        Me.txtBoxPassword.TabIndex = 1
        '
        'txtBoxUserName
        '
        Me.txtBoxUserName.Location = New System.Drawing.Point(37, 35)
        Me.txtBoxUserName.Name = "txtBoxUserName"
        Me.txtBoxUserName.Size = New System.Drawing.Size(166, 21)
        Me.txtBoxUserName.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(37, 59)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(166, 20)
        Me.Label1.Text = "Username"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(37, 106)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(166, 20)
        Me.Label2.Text = "Password"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(37, 154)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(166, 20)
        Me.Label3.Text = "Company-Location"
        '
        'LoginScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtBoxUserName)
        Me.Controls.Add(Me.txtBoxPassword)
        Me.Controls.Add(Me.comboBoxCompLoc)
        Me.Controls.Add(Me.btnRefreshLogins)
        Me.Controls.Add(Me.btnLogin)
        Me.KeyPreview = True
        Me.Menu = Me.mainMenu1
        Me.Name = "LoginScreen"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents btnRefreshLogins As System.Windows.Forms.Button
    Friend WithEvents comboBoxCompLoc As System.Windows.Forms.ComboBox
    Friend WithEvents txtBoxPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtBoxUserName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label

End Class

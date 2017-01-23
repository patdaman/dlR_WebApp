Imports System.Net
Imports System.Data.SqlServerCe

Public Class LoginScreen

    'Connect to sqlce database
    'Public dbConn As SqlCeConnection = New SqlCeConnection("Data Source = \Application Data\Microsoft\SQL CE 3.5\RAPID_COUNT.sdf")
    Public dbConn As SqlCeConnection = New SqlCeConnection("Data Source = \Storage Card\RAPID_COUNT.sdf")
    Public sqlCmd As SqlCeCommand = Nothing
    Public dataReader As SqlCeDataReader = Nothing

    Public username As String
    Public companyName As String
    Public locationId As String
    Public handheldId As String

    Public isManager As Boolean = False

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        'get handheld MAC address
        getHandheldId()

        'Populate combobox with company name and location data
        fillCompanyLocComboBox()

    End Sub

    'Function to fill company location combo box
    Public Sub fillCompanyLocComboBox()

        Try
            'open connection
            dbConn.Open()

            'Get company name and location to populate combo box
            sqlCmd = New SqlCeCommand("SELECT DISTINCT COMPANY_NAM + '-' + LOC_ID AS COMP_LOC FROM RAPID_USR_ACCT", dbConn)

            'execute data reader to fill combo box
            dataReader = sqlCmd.ExecuteReader()

            'populate combo box
            While dataReader.Read()
                comboBoxCompLoc.Items.Add(dataReader("COMP_LOC").ToString)
            End While

            'clean up
            dataReader.Close()
            sqlCmd.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message())
        Finally
            'close connection
            dbConn.Close()

        End Try

    End Sub

    'Function to verify if device is connected to the internet.
    Public Function verifyConnection()
        Dim request As WebRequest = WebRequest.Create("https://www.google.com")
        Dim response As WebResponse

        Try
            response = request.GetResponse()
            response.Close()
            request = Nothing
            Return True
        Catch ex As Exception
            request = Nothing
            MsgBox("A wireless connection cannot be established. Please move closer to the wireles access point")
            Return False
        End Try
    End Function

    Public Sub refreshLogins()
        'First lets verify that we have internet connection.
        Dim netCon As Boolean = verifyConnection()

        'If there is a network connection then proceed
        If netCon = True Then
            'Populate the RAPID_USR_ACCT table with the latest values from the server

            'Let the user know that the process has been completed
            MsgBox("Refresh Successful")
        End If
    End Sub

    Public Sub login()

        Try
            'Get the input from the username text box
            username = Trim(txtBoxUserName.Text)

            'Get the input from the password text box
            Dim password As String = Trim(txtBoxPassword.Text)

            'Split the input form the Company Name and Location Text Box
            Dim companyLocArray() = Split(comboBoxCompLoc.Text, "-", -1)

            'assign the company name from the array
            companyName = Trim(companyLocArray(0).ToString)

            'assign the locationId from the array
            locationId = Trim(companyLocArray(1).ToString)

            'open connection
            dbConn.Open()

            'verify crudentials 
            sqlCmd = New SqlCeCommand("SELECT 1, IS_MANAGER FROM RAPID_USR_ACCT " & _
                                      "WHERE COMPANY_NAM = '" & companyName & "'" & _
                                      " AND LOC_ID = '" & locationId & "'" & _
                                      " AND USR_NAM = '" & username & "'" & _
                                      " AND PWD = '" & password & "'", dbConn)

            'return the only the first row of the query
            dataReader = sqlCmd.ExecuteReader

            'create variables to store Verification and failed logins
            Dim result As Integer
            Dim failedLogins As Integer
            Dim managerResult As String

            'get result from query
            While dataReader.Read()
                result = dataReader.GetInt32(0)
                managerResult = dataReader(1).ToString
            End While

            'clean up
            dataReader.Close()
            'sqlCmd.Dispose()

            If result = 1 Then

                If managerResult = "Y" Then

                    isManager = True

                End If

                'show main menu
                Dim mainMenu As Form = New MainMenu
                mainMenu.Show()

                'hide login screen
                Me.Hide()

            Else
                MsgBox("Login credentials are incorrect, Please enter correctly")

                'update the failed login count
                Dim sqlCmd = New SqlCeCommand("UPDATE RAPID_USR_ACCT " & _
                                              "SET FLD_LGN_CNT = FLD_LGN_CNT + 1 " & _
                                              "WHERE COMPANY_NAM = '" & companyName & "'" & _
                                              " AND LOC_ID = '" & locationId & "'" & _
                                              " AND USR_NAM = '" & username & "' ", dbConn)

                sqlCmd.ExecuteNonQuery()

                'return the only the first row of the query
                dataReader = sqlCmd.ExecuteReader

                'get result from query
                While dataReader.Read()
                    failedLogins = dataReader.GetInt32(0)
                End While

                'If the number of failed login is greater than 6 then lock out the system and email Rapid.
                If failedLogins >= 6 Then
                    MsgBox(failedLogins)
                    'lock out account

                    'send email to rapid

                End If
            End If

            'clean up
            dataReader.Close()
            sqlCmd.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message())

        Finally
            'close connection
            dbConn.Close()

        End Try
    End Sub

    Public Sub getHandheldId()
        'get mac adress

        'if id doesnt exists insert it

        'set the handheldId 
        handheldId = "1234"

    End Sub

    'function to disable buttons
    Public Sub verifyManager(ByVal button1 As Button, ByVal button2 As Button)

        If isManager = False Then

            If button1 Is Nothing Then
            Else

                button1.Enabled = False

            End If

            If button2 Is Nothing Then
            Else

                button2.Enabled = False

            End If

        End If
    End Sub

    'function called when login button is pressed
    Private Sub btnRefreshLogins_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshLogins.Click
        refreshLogins()
    End Sub

    'function called when login button is clicked
    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        login()
    End Sub
End Class

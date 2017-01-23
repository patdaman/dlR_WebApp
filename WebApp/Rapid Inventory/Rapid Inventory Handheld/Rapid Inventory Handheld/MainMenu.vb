Imports System.Net
Imports System.Data.SqlServerCe
Imports System.Data
Imports System.Windows.Forms
Imports System.Data.SqlServerCe.SqlCeEngine

Public Class MainMenu

    Public dbConn As SqlCeConnection = LoginScreen.dbConn
    Public sqlCmd As SqlCeCommand = Nothing
    Public dataReader As SqlCeDataReader = Nothing

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        LoginScreen.verifyManager(btnClearAllData, btnUploadCount)

    End Sub

    'function to upload count to server
    Private Sub UploadCount()

        'Verify connection
        Dim netConn As Boolean = LoginScreen.verifyConnection()

        'if there is a network cennection then go ahead and send the count data to the server
        If netConn = True Then

            'messagebox for confirmation
            MsgBox("Upload Complete")
        End If

    End Sub

    'function called when count button is pressed
    Private Sub btnCount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCount.Click
        'Show section number dialog
        Dim scan As Form = New SectionNumberScreen
        scan.Show()

        'close Main Menu
        Me.Close()

    End Sub

    'fucntion to clear all data on device
    Private Sub ClealAllData()

        Dim result As Integer = MsgBox("Are you sure you want to delete all count data on this device?", MsgBoxStyle.YesNo)

        If result = DialogResult.Yes Then

            Try

                'open database connection
                dbConn.Open()

                'delete all records from RAPID_IM_BARCOD table
                sqlCmd = New SqlCeCommand("DELETE RAPID_IM_BARCOD", dbConn)

                sqlCmd.ExecuteNonQuery()

                'delete all records from RAPID_IM_GRID_DIM table
                sqlCmd = New SqlCeCommand("DELETE RAPID_IM_GRID_DIMS", dbConn)

                sqlCmd.ExecuteNonQuery()

                'delete all record from RAPID
                sqlCmd = New SqlCeCommand("DELETE RAPID_IM_INV", dbConn)

                sqlCmd.ExecuteNonQuery()

                'delete all item cell records
                sqlCmd = New SqlCeCommand("DELETE RAPID_IM_INV_CELL", dbConn)

                sqlCmd.ExecuteNonQuery()

                'delete all raw scan data
                sqlCmd = New SqlCeCommand("DELETE RAPID_RAW_SCAN_DATA", dbConn)

                sqlCmd.ExecuteNonQuery()

                'reset identity on RAPID_RAW_SCAN_DATA COLUMN
                sqlCmd = New SqlCeCommand("ALTER TABLE RAPID_RAW_SCAN_DATA ALTER COLUMN SCAN_ID IDENTITY(1,1)", dbConn)

                sqlCmd.ExecuteNonQuery()

                'delete all item data
                sqlCmd = New SqlCeCommand("DELETE RAPID_IM_ITEM", dbConn)

                sqlCmd.ExecuteNonQuery()

                'send success notification 
                MsgBox("Refresh Successful", MsgBoxStyle.OkOnly)

                'clean  up
                sqlCmd.Dispose()

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                'close db connection
                dbConn.Close()
            End Try
        End If

    End Sub

    'function called when upload count button is pressed
    Private Sub btnUploadCount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUploadCount.Click
        UploadCount()
    End Sub

    'function called when Clear all data button is pressed
    Private Sub btnClearAllData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearAllData.Click
        ClealAllData()
    End Sub

    'function called when log out button is pressed
    Private Sub btnLogOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogOut.Click
        'Show login form
        LoginScreen.Show()

        'Close this form
        Me.Refresh()
        Me.Close()
    End Sub
End Class
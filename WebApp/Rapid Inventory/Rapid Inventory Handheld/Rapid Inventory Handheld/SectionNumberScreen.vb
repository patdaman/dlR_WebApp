Imports System.Net
Imports System.Data.SqlServerCe

Public Class SectionNumberScreen

    'Connect to sqlce database
    Public dbConn As SqlCeConnection = LoginScreen.dbConn
    Public sqlCmd As SqlCeCommand = Nothing
    Public dataReader As SqlCeDataReader = Nothing

    Public username As String
    Public companyName As String
    Public locationId As String
    Public sectionId As String

    Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Try
            'open connection
            dbConn.Open()

            'Get comapny name and location to pupulate combo box
            sqlCmd = New SqlCeCommand("SELECT SECTION_ID FROM RAPID_RAW_SCAN_DATA", dbConn)

            'get section number for input
            sectionId = txtBoxSectionNumber.Text.ToString

            'if no value returned
            If IsDBNull(sqlCmd.ExecuteScalar) Or sqlCmd.ExecuteScalar Is Nothing Then

                'if there are not records in the scan data table then get the section number from the text box
                sectionId = txtBoxSectionNumber.Text.ToString

                'clean up
                sqlCmd.Dispose()

                'close connection
                dbConn.Close()

                'show count menu
                Dim countMenu As Form = New CountMenu(sectionId)
                countMenu.Show()

                'close current form
                Me.Close()

            Else

                'execute data reader to fill combo box
                Dim sqlSectionId As String = sqlCmd.ExecuteScalar.ToString

                'if the current section id does not match the input
                If sectionId <> sqlSectionId Then
                    MsgBox("You must upload your counts for section " & sqlSectionId & " before you begin scanning this new fixture.")

                    'if ok was pressed then navigate back to the main menu
                    If MsgBoxResult.Ok Then

                        'clean up
                        sqlCmd.Dispose()

                        'close connection
                        dbConn.Close()

                        'go back to the main menu
                        Dim mainMenu = New MainMenu
                        mainMenu.Show()

                        'hide current form
                        Me.Close()

                    End If

                Else
                    'clean up
                    sqlCmd.Dispose()

                    'close connection
                    dbConn.Close()

                    'show count menu
                    Dim countMenu As Form = New CountMenu(sectionId)
                    countMenu.Show()

                    'close current form
                    Me.Close()

                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message())
        Finally

        End Try

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Dim Main As Form = New MainMenu
        Main.Show()

        Me.Close()

    End Sub
End Class
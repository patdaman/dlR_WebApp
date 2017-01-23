Imports System.Net
Imports System.Data.SqlServerCe
Imports System.Data
Imports System.Windows.Forms

Public Class CountMenu

    Public dbConn As SqlCeConnection = LoginScreen.dbConn
    Public sqlCmd As SqlCeCommand = Nothing
    Public dataReader As SqlCeDataReader = Nothing

    'get values from login
    Public username As String = LoginScreen.username
    Public companyName As String = LoginScreen.companyName
    Public locationId As String = LoginScreen.locationId
    Public sectionId As String = SectionNumberScreen.sectionId


    Public Shared barcode As String
    Public Shared forceType As String


    Public Sub New(ByVal sectionId As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.sectionId = sectionId

        lblSectionNumber.Text = "Section #" & sectionId

    End Sub

    Private Function verifyScan()

        Try

            'get the barcode number from the tetbox displayed
            barcode = txtBoxBarcode.Text

            'open connection
            dbConn.Open()

            'get all the barcode data to display
            sqlCmd = New SqlCeCommand("SELECT TOP(1) 1 FROM RAPID_IM_BARCOD " & _
                                      "WHERE BARCOD = '" & barcode & "'" & _
                                      "AND LOC_ID = '" & locationId & "'" & _
                                      "AND COMPANY_NAM = '" & companyName & "'", dbConn)

            'get the result of the query
            Dim barcodExists As Int32 = Convert.ToInt32(sqlCmd.ExecuteScalar)

            'if the barcode exists
            If barcodExists = 1 Then ' --> 1

                'check to see if item is a gridded item
                sqlCmd = New SqlCeCommand("SELECT TOP (1) 1 FROM RAPID_IM_BARCOD B " & _
                                          " INNER JOIN RAPID_IM_ITEM I ON B.ITEM_NO = I.ITEM_NO AND B.COMPANY_NAM = I.COMPANY_NAM AND B.LOC_ID = I.LOC_ID " & _
                                          "WHERE I.TRK_METH = 'G' " & _
                                          " AND BARCOD = '" & barcode & "'", dbConn)

                Dim isGridded As Int32 = Convert.ToInt32(sqlCmd.ExecuteScalar)

                'if item is a gridded item
                If isGridded = 1 Then ' --> 2

                    'see if item is a gridded item and has no cell specific data
                    sqlCmd = New SqlCeCommand("SELECT TOP (1) 1 " & _
                                              "FROM RAPID_IM_BARCOD B " & _
                                              " INNER JOIN RAPID_IM_ITEM I ON B.ITEM_NO = I.ITEM_NO AND B.LOC_ID = I.LOC_ID AND B.COMPANY_NAM = I.COMPANY_NAM " & _
                                              " LEFT OUTER JOIN RAPID_IM_INV_CELL D ON B.ITEM_NO = D.ITEM_NO AND B.LOC_ID = D.LOC_ID AND B.COMPANY_NAM = D.COMPANY_NAM " & _
                                              "WHERE B.BARCOD = '" & barcode & "' " & _
                                              " AND I.TRK_METH = 'G' " & _
                                              " AND B.LOC_ID = '" & locationId & "'" & _
                                              " AND B.COMPANY_NAM = '" & companyName & "'" & _
                                              " AND D.DIM_1_UPR IS NULL", dbConn)

                    Dim noCell = Convert.ToInt32(sqlCmd.ExecuteScalar)

                    'If item is a gridded item and has not cell specific data
                    If noCell = 1 Then '--> 3

                        'close connection
                        dbConn.Close()

                        'show grid dimensions dialog
                        Dim gridDims = New GridDimsDialog(sectionId, barcode)
                        gridDims.Show()

                        Return False

                    End If '--> 3

                End If '--> 2

                'check if item type is serialized
                sqlCmd = New SqlCeCommand("SELECT TOP(1) 1 FROM RAPID_IM_BARCOD B " & _
                                          " INNER JOIN RAPID_IM_SER S ON B.ITEM_NO = S.ITEM_NO AND B.LOC_ID = S.LOC_ID AND B.COMPANY_NAM = S.COMPANY_NAM " & _
                                          "WHERE BARCOD = '" & barcode & "' ", dbConn)

                Dim isSerialized As Int32 = Convert.ToInt32(sqlCmd.ExecuteScalar)

                'if item type is serialized
                If isSerialized = 1 Then '--> 2

                    'show message box
                    MsgBox("Item type is serilized, you cannot scan this item")

                    'clear the text in the box
                    txtBoxBarcode.Text = ""

                    'set the focus to the barcode text box to allow for another scan
                    txtBoxBarcode.Focus()

                    dbConn.Close()

                    Return False

                End If '--> 2

                'check if item is stocked at another location, and not this one
                sqlCmd = New SqlCeCommand("SELECT TOP (1) 1 FROM RAPID_IM_BARCOD " & _
                                          "WHERE BARCOD = '" & barcode & "' " & _
                                          " AND LOC_ID <> '" & locationId & "'" & _
                                          " AND COMPANY_NAM = '" & companyName & "'", dbConn)

                Dim diffLocation As Int32 = Convert.ToInt32(sqlCmd.ExecuteScalar)

                'if item is stocked at another location and not this one. 
                If diffLocation = 1 Then '--> 2

                    dbConn.Close()

                    'set the forceType to NS to idicate where this dialog was called
                    forceType = "NS"

                    'show the force dialog
                    Dim force = New ForceDialog(forceType)
                    force.ShowDialog()

                    Return False

                End If ' --> 2

                'check if item type is non inventory
                sqlCmd = New SqlCeCommand("SELECT TOP (1) 1 FROM RAPID_IM_BARCOD B " & _
                                          " INNER JOIN RAPID_IM_ITEM I ON B.ITEM_NO = I.ITEM_NO AND B.LOC_ID = I.LOC_ID AND B.COMPANY_NAM = I.COMPANY_NAM " & _
                                          "WHERE BARCOD = '" & barcode & "' " & _
                                          " AND I.ITEM_TYP = 'N'" & _
                                          " AND B.LOC_ID = '" & locationId & "'" & _
                                          " AND B.COMPANY_NAM = '" & companyName & "'", dbConn)

                Dim isNonIventory As Int32 = Convert.ToInt32(sqlCmd.ExecuteScalar)

                'if item is type is non-inventory 
                If isNonIventory = 1 Then '--> 2

                    'close database connection
                    dbConn.Close()

                    'set the forceType to NI to indicate where this dialog was called
                    forceType = "NI"

                    'show the force dialog
                    Dim force = New ForceDialog(forceType)
                    force.ShowDialog()

                    Return False

                End If '--> 2

                'close database connection
                dbConn.Close()

                'if barcode does not exists
            Else ' --> 1

                'close database connection
                dbConn.Close()

                'set the forceType to NB to indicate where this dialog was called
                forceType = "NB"

                'show the force dialog with barcode not exists option
                Dim force = New ForceDialog(forceType)
                force.ShowDialog()

                Return False

            End If '--> 1

        Catch ex As Exception
            MsgBox(ex.Message)

            sqlCmd.Dispose()
            dbConn.Close()
        End Try

        Return True

    End Function

    Public Sub insertScan(ByVal companyName As String, ByVal locationId As String, ByVal sectionId As String, ByVal barcode As String, ByVal countQty As String, ByVal handheldId As String)

        Try

            'open connection
            dbConn.Open()

            'insert data from scan
            sqlCmd = New SqlCeCommand("INSERT INTO RAPID_RAW_SCAN_DATA (COMPANY_NAM, LOC_ID, SECTION_ID, BARCOD, CNT_QTY, HANDHELD_ID) " & _
                                      "SELECT '" & companyName & "','" & locationId & "','" & sectionId & "','" & barcode & "','" & countQty & "','" & handheldId & "'", dbConn)

            Dim result As Integer = sqlCmd.ExecuteNonQuery()

            sqlCmd.Dispose()

            If result = 1 Then
                MsgBox("scan successfull")
            Else
                MsgBox("scan failed")
            End If
            dbConn.Close()

        Catch ex As Exception

            MsgBox(ex.Message)

            sqlCmd.Dispose()
            dbConn.Close()

        End Try

    End Sub

    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click

        'go back to main menu
        Dim mainMenu As Form = New MainMenu
        mainMenu.Show()

        'hide current form
        Me.Close()

    End Sub

    'function called when checkbox state is pressed
    Private Sub chkboxPromptForQty_CheckStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkboxPromptForQty.CheckStateChanged

        'set the focus to the barcode textbox
        txtBoxBarcode.Focus()

    End Sub

    'function called when enter is pressed
    Private Sub txtBoxBarcode_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBoxBarcode.KeyPress

        'if the enter key is pressed
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Enter) Then

            'verfiy that the scan is valid
            Dim isValid As Boolean = verifyScan()

            ' if scan if valid 
            If isValid = True Then

                'if the checkbox is checked
                If chkboxPromptForQty.Checked Then

                    'show scan edit screen
                    Dim scanEditScreen As Form = New ScanEditScreen(barcode)
                    scanEditScreen.Show()

                    'close this screen
                    Me.Close()

                Else

                    'insert scan
                    insertScan(companyName, locationId, sectionId, barcode, "1", LoginScreen.handheldId)

                End If

            End If

        End If

    End Sub

    'function called when the view scans label is pressed.
    Private Sub lblViewScans_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblViewScans.Click

        'show scan view
        Dim scansView As Form = New ScansView(sectionId)
        scansView.Show()

        'hide count menu
        Me.Close()

    End Sub
End Class
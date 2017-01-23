Imports System.Net
Imports System.Data.SqlServerCe
Imports System.Data



Public Class ForceDialog

    Public dbConn As SqlCeConnection = LoginScreen.dbConn

    Public sqlCmd As SqlCeCommand = Nothing
    Public dataReader As SqlCeDataReader = Nothing

    Public username As String = LoginScreen.username
    Public companyName As String = LoginScreen.companyName
    Public locationId As String = LoginScreen.locationId
    Public sectionId As String = SectionNumberScreen.sectionId
    Public barcode As String = CountMenu.barcode

    Public isManager As Boolean = LoginScreen.isManager

    Private forceType As String

    Public Sub New(ByVal forceType As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.forceType = forceType

        'load dialog
        createDialog()

        LoginScreen.verifyManager(btnForce, Nothing)

    End Sub

    Public Sub createDialog()

        Select Case forceType

            'if forcetype is Item not stocked then set the label reflecting this
            Case "NS"
                lblForceNotification.Text = "Item not stocked at this location, do you wish to force this item?"

                'if forcetype is Item not found then set the label reflecting this
            Case "NF"
                lblForceNotification.Text = "Item is not found, do you wish to force this item?"

                'if forcetype is Item type non-inventory then set the label reflecting this
            Case "NI"
                lblForceNotification.Text = "Item type is non-inventory, do you wish to force this item?"

                'if forcetype is barcode does not exists then set the label reflecting this
            Case "NB"
                lblForceNotification.Text = "Barcode does not exists, do you wish to force this item?"

        End Select

    End Sub

    'function called when cancel button is pressed
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Dim countMenu As New CountMenu(sectionId)

        'set the text to empty
        countMenu.txtBoxBarcode.Text = ""

        'set the focus to the barcode text box
        countMenu.txtBoxBarcode.Focus()

        countMenu.Show()

        'close this dialog
        Me.Close()

    End Sub

    'function called when force button is pressed
    Private Sub btnForce_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForce.Click

        'insert Scan
        Dim countMenu As New CountMenu(Nothing)
        countMenu.insertScan(companyName, locationId, sectionId, barcode, "1", LoginScreen.handheldId)

        'End If

        Dim scanEditScreen As Form = New ScanEditScreen(barcode)
        scanEditScreen.Show()

        Me.Close()
    End Sub

End Class
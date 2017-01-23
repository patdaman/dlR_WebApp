Public Class MySession
    Public Shared Property companyName() As String
        Get

            ' Gets object from session
            Dim objUserTheme As Object = HttpContext.Current.Session("companyName")
            ' Check if value is null
            If objUserTheme Is Nothing Then
                ' If value is null, return default value
                Return "none"
            Else
                ' If value is not null, return it
                Return objUserTheme.ToString()
            End If
        End Get
        Set(ByVal value As String)
            ' Adds value to session variable
            HttpContext.Current.Session("companyName") = value
        End Set

    End Property

    Public Shared Property locationID() As String
        Get

            ' Gets object from session
            Dim objUserTheme As Object = HttpContext.Current.Session("locationID")
            ' Check if value is null
            If objUserTheme Is Nothing Then
                ' If value is null, return default value
                Return "none"
            Else
                ' If value is not null, return it
                Return objUserTheme.ToString()
            End If
        End Get
        Set(ByVal value As String)
            ' Adds value to session variable
            HttpContext.Current.Session("locationID") = value
        End Set

    End Property

    Public Shared Property isManager() As String
        Get

            ' Gets object from session
            Dim objUserTheme As Object = HttpContext.Current.Session("isManager")
            ' Check if value is null
            If objUserTheme Is Nothing Then
                ' If value is null, return default value
                Return "none"
            Else
                ' If value is not null, return it
                Return objUserTheme.ToString()
            End If
        End Get
        Set(ByVal value As String)
            ' Adds value to session variable
            HttpContext.Current.Session("isManager") = value
        End Set

    End Property

End Class
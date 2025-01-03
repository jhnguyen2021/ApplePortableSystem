Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient


Public Class sessionmanager
    Private _s As Boolean
    Public Sub New(ByVal userid As String)
        If Not userid = "" Then
            GetUserInSession(userid)
        End If

    End Sub

    Sub GetUserInSession(userid As String)
        Dim dt = ReadRecords("Select * from tblUsers where pkUserid='" & userid & "'")
        If dt.Rows.Count > 0 Then
            HttpContext.Current.Session("user") = New PortalUser(dt(0)("pkuserid").ToString(), dt(0)("email").ToString(), dt(0)("firstname").ToString(), dt(0)("lastname").ToString(), dt(0)("fkdepartmentID").ToString(), dt(0)("fkbranchID").ToString(),
                                                                     dt(0)("email").ToString(), dt(0)("fkroleID").ToString(), dt(0)("fkcompanyID").ToString())
        End If


    End Sub


    Private Function ReadRecords(ByVal query As String) As DataTable
        Dim dt As New DataTable
        Try
            Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)

                Dim sqlComm As New SqlCommand(query, strConnStr)
                strConnStr.Open()
                Dim reader As SqlDataReader

                reader = sqlComm.ExecuteReader()

                dt.Load(reader)
                reader.Close()
                strConnStr.Close()
            End Using



        Catch ex As Exception

        End Try
        Return dt

    End Function
End Class


Public Class PortalUser
    Public Sub New(ByVal userID As Integer, ByVal username As String, ByVal firstname As String, ByVal lastname As String, ByVal department As String, ByVal office As String, ByVal email As String, ByVal role As String, ByVal company As String)
        _userID = userID
        _username = username
        _firstname = firstname
        _lastname = lastname
        _department = department
        _office = office
        _email = email
        _role = role
        _company = company
    End Sub

    Private _userID As Integer
    Public Property userID() As Integer
        Get
            Return _userID
        End Get
        Set(ByVal value As Integer)
            _userID = value
        End Set
    End Property

    Private _role As Integer
    Public Property role() As Integer
        Get
            Return _role
        End Get
        Set(ByVal value As Integer)
            _role = value
        End Set
    End Property

    Private _company As Integer
    Public Property company() As Integer
        Get
            Return _company
        End Get
        Set(ByVal value As Integer)
            _company = value
        End Set
    End Property

    Private _username As String
    Public Property username() As String
        Get
            Return _username
        End Get
        Set(ByVal value As String)
            _username = value
        End Set
    End Property

    Private _firstname As String
    Public Property firstname() As String
        Get
            Return _firstname
        End Get
        Set(ByVal value As String)
            _firstname = value
        End Set
    End Property

    Private _lastname As String
    Public Property lastname() As String
        Get
            Return _lastname
        End Get
        Set(ByVal value As String)
            _lastname = value
        End Set
    End Property

    Private _department As String
    Public Property department() As String
        Get
            Return _department
        End Get
        Set(ByVal value As String)
            _department = value
        End Set
    End Property

    Private _office As String
    Public Property office() As String
        Get
            Return _office
        End Get
        Set(ByVal value As String)
            _office = value
        End Set
    End Property
    Private _email As String
    Public Property email() As String
        Get
            Return _email
        End Get
        Set(ByVal value As String)
            _email = value
        End Set
    End Property
End Class




Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports Microsoft.Owin.Security
Imports System.DirectoryServices
Imports System.Data.SqlClient
Imports System.Data

Partial Public Class Account_Login
    Inherits Page
    Public email As String = ""
    Dim puser As PortalUser
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim returnUrl = HttpUtility.UrlEncode(Request.QueryString("ReturnUrl"))
        If Request.QueryString("a") <> "" Then
            failure.Text = "Account successfully created. Please login."
        End If
        If Request.QueryString("s") <> "" Then
            failure.Text = "Password reset successfully."
        End If
    End Sub

    Protected Sub LogIn(sender As Object, e As EventArgs)
        If IsValid Then


            If AuthenticateUser(Replace(txtUsername.Text, "'", "''"), Replace(txtPassword.Text, "'", "''")) Then

                puser = Session("user")
                Response.Cookies.Clear()
                Dim expiryDate As DateTime = DateTime.Now.AddDays(180)
                Dim ticket As FormsAuthenticationTicket = New FormsAuthenticationTicket(2, puser.firstname, DateTime.Now, expiryDate, True, puser.userID.ToString())
                Dim encryptedTicket = FormsAuthentication.Encrypt(ticket)
                Dim authenticationCookie As HttpCookie = New HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                authenticationCookie.Expires = ticket.Expiration
                Response.Cookies.Add(authenticationCookie)
                'End If


                'FormsAuthentication.SetAuthCookie(puser.firstname, True)
                Session("user") = puser
                If puser.role = 1004 Then
                    Response.Redirect("../warehouse.aspx", False)
                Else
                    Response.Redirect("../default.aspx", False)
                End If
            Else
                failure.Text = "Invalid username or password."


            End If

        Else
            failure.Text = "Not Valid."
        End If
    End Sub


    Function AuthenticateUser(u As String, p As String) As Boolean
        Dim wrapper As New Simple3Des(ConfigurationSettings.AppSettings("3dskey"))
        Try
            Dim dt = ReadRecords("Select * from tblUsers where email = '" & u & "' and password = '" & wrapper.EncryptData(p) & "' and inactive = 0")
            If dt.Rows.Count > 0 Then
                HttpContext.Current.Session("user") = New PortalUser(dt(0)("pkuserid").ToString(), dt(0)("email").ToString(), dt(0)("firstname").ToString(), dt(0)("lastname").ToString(), dt(0)("fkdepartmentID").ToString(), dt(0)("fkbranchID").ToString(),
                                                                     dt(0)("email").ToString(), dt(0)("fkroleID").ToString(), dt(0)("fkcompanyID").ToString())
                Return True
            End If

        Catch

            Return False
        End Try
    End Function

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        LogIn(sender, e)
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
            failure.Text = ex.Message
        End Try
        Return dt

    End Function

End Class

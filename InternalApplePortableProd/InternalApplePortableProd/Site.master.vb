Imports System.Collections.Generic
Imports System.Security.Claims
Imports System.Security.Principal
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls


Partial Class MasterPage
    Inherits System.Web.UI.MasterPage

    Dim puser As PortalUser

    Protected Sub Page_Load(sender As Object, e As EventArgs)
        'If Request.Cookies("User") Is Nothing Then
        'FormsAuthentication.SetAuthCookie("", False)
        'FormsAuthentication.SignOut()
        'Response.Cookies(FormsAuthentication.FormsCookieName).Expires = DateTime.Now.AddYears(-1)

        'End If
        If IsNothing(Session("user")) Then
            Dim ticket = Request.Cookies(FormsAuthentication.FormsCookieName).Value
            Dim dycriptedTicket = FormsAuthentication.Decrypt(ticket)
            If dycriptedTicket.UserData = "" Then
                Response.Redirect("Default.aspx")
            End If
            Dim t As New sessionmanager(dycriptedTicket.UserData)
        End If

        puser = Session("user")


        Select Case puser.role
            Case 1001
                Try
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Users"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Warehouse"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Shipping"))
                Catch ex As Exception
                End Try
            Case 1006
                Try
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Users"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Warehouse"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Shipping"))
                Catch ex As Exception
                End Try
            Case 1000
                Try
                    If puser.department = 1004 Then
                        NavigationMenu.Items.Remove(NavigationMenu.FindItem("Exhibits"))
                    End If
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Users"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Warehouse"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Shipping"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Reporting"))

                Catch ex As Exception
                End Try
            Case 1004
                Try

                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Home"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("New Order"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Inventory"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Exhibits"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Schedule"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Users"))
                    'NavigationMenu.Items.Remove(NavigationMenu.FindItem("Admin"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("History"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Reporting"))

                Catch ex As Exception

                End Try
            Case 1005
                Try

                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Home"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("New Order"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Inventory"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Exhibits"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Schedule"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Users"))
                    'NavigationMenu.Items.Remove(NavigationMenu.FindItem("Admin"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("History"))
                    NavigationMenu.Items.Remove(NavigationMenu.FindItem("Reporting"))

                Catch ex As Exception

                End Try
        End Select



    End Sub

    Protected Sub Unnamed_LoggingOut(sender As Object, e As LoginCancelEventArgs)
        'Context.GetOwinContext().Authentication.SignOut()
    End Sub

End Class


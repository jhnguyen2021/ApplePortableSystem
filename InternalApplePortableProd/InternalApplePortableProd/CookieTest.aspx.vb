
Partial Class CookieTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If (User.Identity.IsAuthenticated) Then

            Dim sb = New StringBuilder()
            Dim idu As FormsIdentity = User.Identity
            Dim ticket = idu.Ticket
            sb.Append("Authenticated")
            sb.Append("<br/>CookiePath: " + ticket.CookiePath.ToString)
            sb.Append("<br/>Expiration: " + ticket.Expiration.ToString)
            sb.Append("<br/>Expired: " + ticket.Expired.ToString)
            sb.Append("<br/>IsPersistent: " + ticket.IsPersistent.ToString)
            sb.Append("<br/>IssueDate: " + ticket.IssueDate.ToString)
            sb.Append("<br/>Name: " + ticket.Name.ToString)
            sb.Append("<br/>UserData: " + ticket.UserData.ToString)
            sb.Append("<br/>Version: " + ticket.Version.ToString)
            Label1.Text = sb.ToString()

        Else
            Label1.Text = "Not Authenticated"
        End If


    End Sub


End Class

Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports System.Net
Imports Telerik.Web.Spreadsheet

Partial Class Emailer
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PendingOutBoundItems()

    End Sub

    Sub PendingOutBoundItems()

        Dim sql = "SELECT distinct o.* , oi.fkOrderID from tblOrders o join tblOrderItems oi on oi.fkOrderID = o.pkOrderID join tblInventory i on i.pkInventoryID = oi.fkInventoryID where fkStatusID = 1002 and DateOrder > (GETDATE() - 7) and i.fkDepartmentID = 1004 order by o.DateArrive"
        Dim dt = ReadRecords(sql)
        If dt.Rows.Count > 0 Then
            Dim htmlbody = CreateEmailHtml()
            Try
                Dim wrapper As New Simple3Des(ConfigurationSettings.AppSettings("3dskey"))

                Dim SmtpServer As New SmtpClient("smtp-legacy.office365.com")
                SmtpServer.Port = 587
                SmtpServer.UseDefaultCredentials = False
                SmtpServer.Credentials = New System.Net.NetworkCredential(ConfigurationManager.AppSettings("o365user"), "Boc00263")
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network
                SmtpServer.EnableSsl = True

                Dim mail As New MailMessage()
                'mail.To.Add("tbs@artica.nl")
                mail.To.Add("meghnag@pinnacle-exp.com")
                mail.To.Add("jasonn@pinnacle-exp.com")
                mail.Subject = ConfigurationSettings.AppSettings("ClientName") & " " & "Pending Outbound Orders"
                mail.Body = "<style>" & css.InnerHtml & "</style><table style='width:90%;background-color:#ededed;border: 1px solid #dfdfdf;padding:30px;margin-left:auto;margin-right:auto;margin-top:30px;' ><tr><td></td></tr><tr><td><h1>There are pending outbound orders to be processed.</h1>" & htmlbody & " <br><br>Thank you,<br>Pinnacle</td></tr></table>"
                mail.From = New MailAddress("PinnacleNoReply@pinnacle-exp.com")
                mail.IsBodyHtml = True
                SmtpServer.Send(mail)

            Catch ex As Exception
            End Try
        End If
    End Sub

    Function CreateEmailHtml() As String

        Dim strBeforeRows As String
        Dim strRows As String
        Dim strAfterRows As String
        Dim strAll As String

        ' HTML before rows
        strBeforeRows = "<head><style>table, th, td {border: 1px solid gray; border-collapse:" &
        "collapse;}</style></head><body>" &
        "<table style=""width:60%""><tr>" &
        "<th bgcolor=""#bdf0ff"">Order #</th>" &
        "<th bgcolor=""#bdf0ff"">Event Name</th>" &
        "<th bgcolor=""#bdf0ff"">Event Start Date</th>" &
        "<th bgcolor=""#bdf0ff"">Event End Date</th>" &
        "<th bgcolor=""#bdf0ff"">Arrival Date</th>" &
        "<th bgcolor=""#bdf0ff"">Location</th>"

        Dim sql = "SELECT distinct o.* , oi.fkOrderID from tblOrders o join tblOrderItems oi on oi.fkOrderID = o.pkOrderID join tblInventory i on i.pkInventoryID = oi.fkInventoryID where fkStatusID = 1002 and DateOrder > (GETDATE() - 7) and i.fkDepartmentID = 1004 order by o.DateArrive"
        Dim dt = ReadRecords(sql)
        ' Collection
        strRows = ""
        If dt.Rows.Count > 0 Then
            Dim i As Integer
            i = 0
            For Each r In dt.Rows
                strRows = strRows & "<tr>"
                strRows = strRows & "<td ""col width=10%"">'" & dt(i)("pkOrderID").ToString() & "'</td>"
                strRows = strRows & "<td ""col width=10%"">'" & dt(i)("EventName").ToString() & "'</td>"
                strRows = strRows & "<td ""col width=10%"">'" & CDate(dt(i)("DateStart").ToString()).Date() & "'</td>"
                strRows = strRows & "<td ""col width=10%"">'" & CDate(dt(i)("DatePickup").ToString()).Date() & "'</td>"
                strRows = strRows & "<td ""col width=10%"">'" & CDate(dt(i)("DateArrive").ToString()).Date() & "'</td>"
                strRows = strRows & "<td ""col width=10%"">'" & dt(i)("ShipAddress1").ToString() & "<br>" & dt(i)("ShipCity").ToString() & "<br>" & dt(i)("ShipState").ToString() & "<br>" & dt(i)("ShipZip").ToString() & "<br>" & dt(i)("PickupCountry").ToString() & "'</td>"
                strRows = strRows & "</tr>"
                i = i + 1
            Next
        End If

        ' HTML after rows
        strAfterRows = "</table></body>"

        ' final HTML - concatenate the 3 string variables
        strAll = strBeforeRows & strRows & strAfterRows

        Return strAll

    End Function

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
            ErrorEmail(ex.Message)
        End Try
        Return dt

    End Function


    Sub ErrorEmail(body As String)

        Dim SmtpServer As New SmtpClient("smtp-legacy.office365.com")
        SmtpServer.Port = 587
        SmtpServer.UseDefaultCredentials = False
        SmtpServer.Credentials = New System.Net.NetworkCredential(ConfigurationManager.AppSettings("o365user"), "Boc00263")
        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network
        SmtpServer.EnableSsl = True

        Dim mail As New MailMessage()
        mail.To.Add("Jasonn@pinnacle-exp.com")
        mail.To.Add("mikes@pinnacle-exp.com")
        mail.Subject = ConfigurationSettings.AppSettings("ClientName") & "Email Errors"
        mail.Body = body
        mail.From = New MailAddress("PinnacleNoReply@pinnacle-exp.com")
        mail.IsBodyHtml = True

        SmtpServer.Send(mail)

    End Sub
End Class




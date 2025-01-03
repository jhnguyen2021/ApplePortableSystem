Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Mail

Partial Class Admin
    Inherits System.Web.UI.Page
    Dim puser As PortalUser
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsNothing(Session("user")) Then
            Dim ticket = Request.Cookies(FormsAuthentication.FormsCookieName).Value
            Dim dycriptedTicket = FormsAuthentication.Decrypt(ticket)
            If dycriptedTicket.UserData = "" Then
                Response.Redirect("account/login.aspx")
            End If
            Dim t As New sessionmanager(dycriptedTicket.UserData)
        End If

        puser = Session("user")
        If puser.role = 1000 Then
            'Response.Redirect("Defaults.aspx")
        End If

        If Not IsPostBack Then


        End If

    End Sub

    Protected Sub RadGrid1_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGrid1.NeedDataSource
        Dim uid = puser.userID.ToString()
        If puser.role = 1001 Then
            uid = "%"
        End If

        Dim sql = "Select *,  Concat(Format(Cast(DateStart as date), 'MM/dd/yyyy') , ' - ' , Format(Cast(DateEnd as date), 'MM/dd/yyyy')) AS eventdates from vwOrderAdmin where  fkStatusID > 1000 and DateEnd > GETDATE() order by DateEnd desc"

        Dim dt = ReadRecords(sql)

        RadGrid1.DataSource = dt

    End Sub

    Protected Sub RadGrid2_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGrid2.NeedDataSource
        Dim uid = puser.userID.ToString()
        If puser.department = 1002 Or puser.role = 1001 Then
            uid = "%"
        End If

        Dim sql = "Select Concat(Format(Cast(DateStart as date), 'MM/dd/yyyy') , ' - ' , Format(Cast(DateEnd as date), 'MM/dd/yyyy')) AS eventdates,* from vwOrderAdmin  where fkStatusID > 1001 and DateEnd <= GETDATE() order by DateEnd desc"

        Dim dt = ReadRecords(sql)

        RadGrid2.DataSource = dt

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

    Sub ErrorEmail(body As String)

        Dim SmtpServer As New SmtpClient("smtp-legacy.office365.com")
        SmtpServer.Port = 587
        SmtpServer.UseDefaultCredentials = False
        SmtpServer.Credentials = New System.Net.NetworkCredential(ConfigurationManager.AppSettings("o365user"), "awT@(Yg7JCMD]<")
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

    Function BillingStatus(fkDepartmentID, CurrentBilled)
        Dim status = ""

        If fkDepartmentID.ToString = "1002" Then
            status = "PO"
        End If

        If fkDepartmentID.ToString <> "1002" And CurrentBilled.ToString = "" Then
            status = "Unbilled"
        End If

        If fkDepartmentID.ToString <> "1002" And CurrentBilled.ToString <> "" Then
            status = CurrentBilled.ToString
        End If

        Return status
    End Function

    Protected Sub RadGrid1_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles RadGrid1.ItemCommand
        'InventoryItems.Visible = False
        'RadGrid2.Visible = False

        If e.CommandName = "Delete" Then
            Dim item As Telerik.Web.UI.GridDataItem
            item = e.Item

            Dim OrderID = item.GetDataKeyValue("pkOrderID")
            CancelOrder(OrderID)


            RadGrid1.DataSource = Nothing
            RadGrid1.Rebind()
        End If

        If e.CommandName = "Edit" Then
            Dim item As Telerik.Web.UI.GridDataItem
            item = e.Item

            Dim OrderID = item.GetDataKeyValue("pkOrderID")
            Response.Redirect("Place.aspx?OrderID=" & OrderID)
        End If

        If e.CommandName = "Received Back" Or e.CommandName = "Processed" Then
            'Dim item As Telerik.Web.UI.GridDataItem
            'item = e.Item

            'orderID = item.GetDataKeyValue("OrderID")
            'RadGrid2.Rebind()
        End If

    End Sub


    Sub CancelOrder(pkid)
        UpdateOrderStatus(pkid, 1006)

        'reverse taxes

        'reverse cc charges

        Try
            Dim fr As System.Net.HttpWebRequest
            Dim targetURI As New Uri("http://" & HttpContext.Current.Request.Url.Host & "/Order.aspx?email=1&orderid=" & pkid & "&notes=%20-%20Canceled")

            fr = DirectCast(HttpWebRequest.Create(targetURI), System.Net.HttpWebRequest)
            If (fr.GetResponse().ContentLength > 0) Then
                Dim str As New System.IO.StreamReader(fr.GetResponse().GetResponseStream())
                'Response.Write(str.ReadToEnd())
                str.Close()
            End If
        Catch ex As System.Net.WebException
            ErrorEmail(ex.Message)
        End Try

    End Sub


    Sub UpdateOrderStatus(pkid, status)
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "Update tblOrders set fkstatusID=@fkstatusID where pkorderid=@pkorderid"


            sqlComm.Parameters.Add(New SqlParameter("pkorderid", pkid))
            sqlComm.Parameters.Add(New SqlParameter("fkstatusID", status))
            Try
                pkid = sqlComm.ExecuteScalar()
            Catch ex As Exception

            End Try
        End Using
    End Sub
End Class

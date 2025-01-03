Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.IO

Partial Class Order
    Inherits Telerik.Web.UI.RadAjaxPage
    Dim puser As PortalUser
    Public services As String = "none"
    Public portal As String = "none"
    Dim pkid = ""
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If IsNothing(Session("user")) Then
                Dim ticket = Request.Cookies(FormsAuthentication.FormsCookieName).Value
                Dim dycriptedTicket = FormsAuthentication.Decrypt(ticket)
                If dycriptedTicket.UserData = "" Then
                    Response.Redirect("account/login.aspx")
                End If
                Dim t As New sessionmanager(dycriptedTicket.UserData)
            End If


            If Request.QueryString("print") <> "" Then
                RadGrid1.RegisterWithScriptManager = False
                EmailOrder(Request.QueryString("OrderID"), "adamn@pinnacle-exp.com")
            End If

            Dim str = order.InnerHtml
            If Request.QueryString("OrderID") <> "" Then
                pkid = Replace(Request.QueryString("OrderID"), "'", "''")
                Dim dt = ReadRecords("Select tblorders.*, tblorderstatus.status from tblOrders left outer join tblOrderStatus on tblOrderStatus.pkStatusID=tblOrders.fkStatusID where pkOrderID = " & pkid)
                If dt.Rows.Count > 0 Then
                    str = Regex.Replace(str, "\[ContactName\]", dt(0)("ContactName").ToString)
                    str = Regex.Replace(str, "\[ContactEmail\]", dt(0)("ContactEmail").ToString)
                    str = Regex.Replace(str, "\[ContactPhone\]", dt(0)("ContactPhone").ToString)
                    str = Regex.Replace(str, "\[CostCenter\]", dt(0)("CostCenter").ToString)
                    str = Regex.Replace(str, "\[EventName\]", dt(0)("EventName").ToString)
                    str = Regex.Replace(str, "\[DateStart\]", dt(0)("DateStart").ToString)
                    str = Regex.Replace(str, "\[DateEnd\]", dt(0)("DateEnd").ToString)
                    str = Regex.Replace(str, "\[EventVenue\]", dt(0)("EventVenueName").ToString)
                    str = Regex.Replace(str, "\[Website\]", dt(0)("Website").ToString)
                    str = Regex.Replace(str, "\[DateArrive\]", CDate(dt(0)("DateArrive").ToString).Date())
                    str = Regex.Replace(str, "\[DatePickup\]", dt(0)("DatePickup").ToString)
                    str = Regex.Replace(str, "\[BoothSize\]", dt(0)("BoothSize").ToString)
                    str = Regex.Replace(str, "\[Boothnum\]", dt(0)("Boothnum").ToString)
                    str = Regex.Replace(str, "\[BoothType\]", dt(0)("BoothType").ToString)
                    str = Regex.Replace(str, "\[Electrical\]", dt(0)("Electrical").ToString)
                    str = Regex.Replace(str, "\[Internet\]", dt(0)("Internet").ToString)
                    str = Regex.Replace(str, "\[Carpet\]", dt(0)("Carpet").ToString)
                    str = Regex.Replace(str, "\[LeadRetrieval\]", dt(0)("LeadRetrieval").ToString)
                    str = Regex.Replace(str, "\[RentalFurniture\]", dt(0)("RentalFurniture").ToString)
                    str = Regex.Replace(str, "\[Shipto\]", dt(0)("Shipto").ToString)
                    str = Regex.Replace(str, "\[ShipContactName\]", dt(0)("ShipContactName").ToString)
                    str = Regex.Replace(str, "\[ShipContactPhone\]", dt(0)("ShipContactPhone").ToString)
                    str = Regex.Replace(str, "\[ShipContactEmail\]", dt(0)("ShipContactEmail").ToString)
                    str = Regex.Replace(str, "\[ShipAddress1\]", dt(0)("ShipAddress1").ToString)
                    str = Regex.Replace(str, "\[ShipAddress2\]", dt(0)("ShipAddress2").ToString)
                    str = Regex.Replace(str, "\[ShipCity\]", dt(0)("ShipCity").ToString)
                    str = Regex.Replace(str, "\[ShipState\]", dt(0)("ShipState").ToString)
                    str = Regex.Replace(str, "\[ShipZip\]", dt(0)("ShipZip").ToString)
                    str = Regex.Replace(str, "\[ShipCountry\]", dt(0)("ShipCountry").ToString)
                    str = Regex.Replace(str, "\[PickupFrom\]", dt(0)("PickupFrom").ToString)
                    str = Regex.Replace(str, "\[PickupContactName\]", dt(0)("PickupContactName").ToString)
                    str = Regex.Replace(str, "\[PickupContactPhone\]", dt(0)("PickupContactPhone").ToString)
                    str = Regex.Replace(str, "\[PickupContactEmail\]", dt(0)("PickupContactEmail").ToString)
                    str = Regex.Replace(str, "\[PickupAddress1\]", dt(0)("PickupAddress1").ToString)
                    str = Regex.Replace(str, "\[PickupAddress2\]", dt(0)("PickupAddress2").ToString)
                    str = Regex.Replace(str, "\[PickupCity\]", dt(0)("PickupCity").ToString)
                    str = Regex.Replace(str, "\[PickupState\]", dt(0)("PickupState").ToString)
                    str = Regex.Replace(str, "\[PickupZip\]", dt(0)("PickupZip").ToString)
                    str = Regex.Replace(str, "\[PickupCountry\]", dt(0)("PickupCountry").ToString)
                    str = Regex.Replace(str, "\[Notes\]", dt(0)("Notes").ToString)
                    str = Regex.Replace(str, "\[Status\]", dt(0)("Status").ToString)
                    str = Regex.Replace(str, "\[pkOrderID\]", Replace(Request.QueryString("OrderID"), "'", "''").ToString)
                    str = Regex.Replace(str, "\[Portalpass\]", dt(0)("ExhibitorPass").ToString())
                    str = Regex.Replace(str, "\[Portaluser\]", dt(0)("ExhibitorUser").ToString())
                    str = Regex.Replace(str, "\[Portalsite\]", dt(0)("ExhibitorWebsite").ToString())
                    str = Regex.Replace(str, "\[Labor\]", dt(0)("Labor").ToString())
                    str = Regex.Replace(str, "\[Material\]", dt(0)("Material").ToString())
                    str = Regex.Replace(str, "\[ShippingNotes\]", dt(0)("ShippingNotes").ToString())
                    str = Regex.Replace(str, "\[TrackingNumber\]", dt(0)("TrackingNumber").ToString())
                    str = Regex.Replace(str, "\[ShippingType\]", dt(0)("ShippingType").ToString())
                    str = Regex.Replace(str, "\[TrackingNumber\]", dt(0)("TrackingNumber").ToString())
                    str = Regex.Replace(str, "\[fkFulfillmentLocationId\]", dt(0)("fkFulfillmentLocationId").ToString())


                    If dt(0)("BoothSize").ToString.Contains("10") Or dt(0)("BoothSize").ToString.Contains("20") Then
                        'services = "table-row"

                    End If

                    If dt(0)("ExhibitorWebsite").ToString <> "" Then
                        'portal = "table-row"

                    End If

                    If dt(0)("status").ToString.Contains("Received") Then
                        fulfillment.Visible = True
                    End If


                    Dim uploaded = ""
                    Dim targetFolder As String = Server.MapPath("Documents/" & pkid)
                    Dim di As New IO.DirectoryInfo(targetFolder)
                    Try
                        Dim aryFi As IO.FileInfo() = di.GetFiles("*.*")
                        Dim fi As IO.FileInfo
                        For Each fi In aryFi
                            uploaded = uploaded & "<a href='" & HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/") & "Documents/" & pkid & "/" & fi.Name & "' download>" & fi.Name & "</a><br>"

                        Next
                    Catch ex As Exception

                    End Try
                    str = Regex.Replace(str, "\[Attachments\]", uploaded)

                    order.InnerHtml = str


                End If

                RadGrid1.DataSource = ReadRecords("SELECT tblOrderItems.Quantity, vwinventoryview.pkInventoryID, vwinventoryview.description, vwinventoryview.type, vwinventoryview.PartNum, vwinventoryview.Picture , vwInventoryView.fkDepartmentID, vwInventoryView.location  FROM tblOrders INNER JOIN " &
                                                   "tblOrderItems ON tblOrders.pkOrderID = tblOrderItems.fkOrderID INNER JOIN vwinventoryview ON tblOrderItems.fkInventoryID = vwinventoryview.pkInventoryID where tblOrders.pkOrderID = " & pkid)
                RadGrid1.DataBind()
            Else

            End If

        End If

    End Sub

    Function GetImagePath(ByVal imagePath As String) As String

        Return Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath & "Account/inventory/" & imagePath

    End Function

    Function GetDamagesImagePath(ByVal imagePath) As String
        Dim pics = imagePath.ToString().Split(":")
        Dim imgs = ""

        For Each pic In pics
            If pic <> "" Then
                imgs = imgs & "<div class='imgcontainer'><img src='" & Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath & "inbound/" & pkid & "/" & pic & "' height='80' alt='" & pic & "' class='fancybox'/></div>"
            End If

        Next


        'ConfigurationSettings.AppSettings("website") & "/Inbound/" & imagePath


        Return imgs

    End Function

    Protected Sub RadGrid2_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGrid2.NeedDataSource
        Dim sql = "Select *, Description+' '+Type+' '+Partnum as Name from vwOrderInventoryItems where pkorderid = " & pkid

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

    Sub EmailOrder(pkid, toEmail)

        Try
            Dim sb As New StringBuilder
            Dim sw As New StringWriter(sb)
            Dim htmltw As New Html32TextWriter(sw)
            RadGrid1.RenderControl(htmltw)

            Dim str As String = sb.ToString
            Dim strstart = "<script "
            Dim strend = "</script>"

            Dim t = sb




            While t.ToString.IndexOf(strstart) > 0

                Dim s = t.ToString.IndexOf(strstart)
                Dim e = t.ToString.IndexOf(strend)
                Dim total = (t.ToString.IndexOf(strend) + strend.Length) - t.ToString.IndexOf(strstart)


                t = t.Replace(t.ToString().Substring(t.ToString.IndexOf(strstart), (t.ToString.IndexOf(strend) + strend.Length) - t.ToString.IndexOf(strstart)), "")
            End While

            sb = t




            Dim SmtpServer As New SmtpClient(ConfigurationSettings.AppSettings("SMTP"))
            SmtpServer.EnableSsl = False
            Dim mail As New MailMessage()
            mail.To.Add(toEmail)
            mail.To.Add(ConfigurationSettings.AppSettings("SupportEmail"))
            mail.Subject = ConfigurationSettings.AppSettings("ClientName") & " Portable Order# " & pkid & " Confirmation"
            mail.Body = "<style>" & css.InnerHtml & "</style><table style='width:90%;background-color:#ededed;border: 1px solid #dfdfdf;padding:30px;margin-left:auto;margin-right:auto;margin-top:30px;' ><tr><td></td></tr><tr><td><h1>Order# " & pkid & "</h1>" & order.InnerHtml & sb.ToString() & "</td></tr></table>"
            mail.From = New MailAddress(ConfigurationSettings.AppSettings("SupportEmail"), ConfigurationSettings.AppSettings("ClientName") & " Portables")
            mail.IsBodyHtml = True
            'SmtpServer.Send(mail)

            SendEmail(ConfigurationSettings.AppSettings("ClientName") & " Portable Order# " & pkid & " Confirmation", "<style>" & css.InnerHtml & "</style><table style='width:90%;background-color:#ededed;border: 1px solid #dfdfdf;padding:30px;margin-left:auto;margin-right:auto;margin-top:30px;' ><tr><td></td></tr><tr><td><h1>Order# " & pkid & "</h1>" & order.InnerHtml & sb.ToString() & "</td></tr></table>", "adamnorbut@gmail.com", ConfigurationSettings.AppSettings("SupportEmail"), ConfigurationSettings.AppSettings("ClientName") & " Portables")
            'SendEmail("test", "testbody", "testto", "testfrom", "testname")

        Catch ex As Exception

        End Try

    End Sub

    Sub SendEmail(subject, body, toEmail, fromEmail, fromName)
        Dim data = Encoding.UTF8.GetBytes("{""subject"": """ & HttpUtility.HtmlEncode(subject) & """, ""body"": """ & HttpUtility.HtmlEncode(body) & """, ""toEmail"": """ & toEmail & """, ""fromEmail"": """ & fromEmail & """, ""fromName"": """ & fromName & """}")
        Dim responses As String
        Dim request As System.Net.WebRequest
        request = System.Net.WebRequest.Create(New Uri("https://yourpinnacle.net/clients/Mail.aspx/SendMail"))
        request.ContentLength = data.Length
        request.ContentType = "application/json"
        request.Method = "POST"
        Using requestStream = request.GetRequestStream
            requestStream.Write(data, 0, data.Length)
            requestStream.Close()
            Try
                Using responseStream = request.GetResponse.GetResponseStream
                    Using reader As New StreamReader(responseStream)
                        responses = reader.ReadToEnd
                    End Using
                End Using
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try
        End Using


    End Sub


    'Sub btnUpdateOrder_Click(sender As Object, e As System.Windows.RoutedEventArgs)
    '    Dim validate As Boolean = True
    '    If puser.role = 1001 Then
    '        validate = IsValid
    '    End If

    'End Sub

    'Sub RadButton3_Click(sender As Object, e As EventArgs)

    '    Dim validate As Boolean = True
    '    If puser.role = 1001 Then
    '        validate = IsValid
    '    End If



    '    Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
    '        strConnStr.Open()

    '        Dim sqlComm As New SqlCommand()
    '        sqlComm.Connection = strConnStr
    '        sqlComm.CommandText = "Update tblOrders set [ContactName]=@ContactName,[ContactEmail]=@ContactEmail,[ContactPhone]=@ContactPhone,[EventName]=@EventName,[DateStart]=@DateStart,[DateEnd]=@DateEnd,[DateArrive]=@DateArrive" &
    '                    ",[Notes]=@Notes,[ShipTo]=@ShipTo,[ShipContactName]=@ShipContactName,[ShipContactPhone]=@ShipContactPhone,[ShipContactEmail]=@ShipContactEmail,[ShipAddress1]=@ShipAddress1,[ShipAddress2]=@ShipAddress2,[ShipCity]=@ShipCity,[ShipState]=@ShipState,[ShipZip]=@ShipZip " &
    '                    ",[ShipCountry]=@ShipCountry, [TrackingNumber] = @TrackingNumber where pkOrderID=@pkOrderID"
    '        sqlComm.Parameters.Add(New SqlParameter("pkorderid", Request.QueryString("OrderID")))

    '        'sqlComm.Parameters.Add(New SqlParameter("EventName", tbEventName.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("ContactEmail", tbContactName.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("ContactPhone", tbContactPhone.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("EventName", tbEventName.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("DateStart", tbArrivalDate.SelectedDate))
    '        'sqlComm.Parameters.Add(New SqlParameter("DateEnd", tbPickupDate.SelectedDate))
    '        'sqlComm.Parameters.Add(New SqlParameter("DateArrive", tbArrivalDate.SelectedDate))
    '        'sqlComm.Parameters.Add(New SqlParameter("DatePickup", tbPickupDate.SelectedDate))
    '        'sqlComm.Parameters.Add(New SqlParameter("EventVenueName", tbVenue.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("Website", tbWebsite.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("BoothSize", IIf(tbBoothSize.Text = "", tbBoothSize.SelectedValue, tbBoothSize.Text)))
    '        'sqlComm.Parameters.Add(New SqlParameter("Attachments", ""))
    '        'sqlComm.Parameters.Add(New SqlParameter("BoothNum", tbBoothnum.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("BoothType", cbBoothType.SelectedValue))
    '        'sqlComm.Parameters.Add(New SqlParameter("Electrical", IIf(cbElectrical.Text = "", cbElectrical.SelectedValue, cbElectrical.Text)))
    '        'sqlComm.Parameters.Add(New SqlParameter("Internet", rbInternet.SelectedValue))
    '        'sqlComm.Parameters.Add(New SqlParameter("Carpet", rbShipCarpet.SelectedValue))
    '        'sqlComm.Parameters.Add(New SqlParameter("LeadRetrieval", rbLeadRetrieval.SelectedValue))
    '        'sqlComm.Parameters.Add(New SqlParameter("RentalFurniture", tbShipFurniture.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("Notes", tbNotes.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("DateOrder", Now()))
    '        'sqlComm.Parameters.Add(New SqlParameter("fkStatusID", status))
    '        'sqlComm.Parameters.Add(New SqlParameter("fkUserID", fkUserID.Value))
    '        'sqlComm.Parameters.Add(New SqlParameter("ShipTo", tbShipTo.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("ShipContactName", tbSOnsiteContact.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("ShipContactPhone", tbContactPhone.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("ShipContactEmail", tbContactEmail.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("ShipAddress1", tbSAddress1.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("ShipAddress2", tbSAddress2.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("ShipCity", tbSCity.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("ShipState", cbSState.SelectedValue))
    '        'sqlComm.Parameters.Add(New SqlParameter("ShipZip", tbSZip.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("ShipCountry", cbSCountry.SelectedValue))
    '        'sqlComm.Parameters.Add(New SqlParameter("PickupFrom", tbPickupfrom.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("PickupContactName", tbPOnsiteContact.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("PickupContactPhone", tbPOnsitePhone.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("PickupContactEmail", tbPOnsiteEmail.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("PickupAddress1", tbPAddress1.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("PickupAddress2", tbPAddress2.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("PickupCity", tbPCity.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("PickupState", cbPState.SelectedValue))
    '        'sqlComm.Parameters.Add(New SqlParameter("PickupZip", tbPZip.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("PickupCountry", cbPCountry.SelectedValue))
    '        'sqlComm.Parameters.Add(New SqlParameter("CostCenter", tbCostCenter.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("UDF10", ""))

    '        'sqlComm.Parameters.Add(New SqlParameter("Material", rbMaterial.SelectedValue))
    '        'sqlComm.Parameters.Add(New SqlParameter("Labor", rbLabor.SelectedValue))
    '        'sqlComm.Parameters.Add(New SqlParameter("TierCosts", tierCost))
    '        'sqlComm.Parameters.Add(New SqlParameter("ExhibitorWebsite", tbPortalsite.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("ExhibitorPass", tbPortalpass.Text))
    '        'sqlComm.Parameters.Add(New SqlParameter("ExhibitorUser", tbPortaluser.Text))

    '        Try
    '            pkid = sqlComm.ExecuteScalar()
    '            If Request.QueryString("OrderID") <> "" Then
    '                pkid = Request.QueryString("OrderID")
    '            End If
    '        Catch ex As Exception
    '            pkid = "Error: " & ex.Message
    '            ErrorEmail(ex.Message)
    '        End Try
    '    End Using
    'End Sub

    Sub ErrorEmail(body As String)

        Dim SmtpServer As New SmtpClient(ConfigurationSettings.AppSettings("SMTP"))
        SmtpServer.EnableSsl = False
        'SmtpServer.Credentials = New Net.NetworkCredential("@gmail.com", "xxx")

        Dim mail As New MailMessage("no-reply@yourpinnacle.net", "adamn@pinnacle-exhibits.com", "Error", body)
        mail.From = New MailAddress("no-reply@yourpinnacle.net", "Pinnacle Portables")
        mail.IsBodyHtml = True

        SmtpServer.Send(mail)

    End Sub


    Protected Sub OrdersNote_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles OrdersNote.NeedDataSource

        Dim orderid = Request.QueryString("OrderID")
        Dim sql = "SELECT tblOrderNotes.* , tblOrderNotes.Notes as logsNote, Format(Cast([tblOrderNotes].DateCreated as date), 'MM/dd/yyyy') as dateTime , tblusers.Firstname+' '+tblusers.lastname as [user] from tblOrderNotes join tblUsers on tblOrderNotes.fkUserId = tblUsers.pkUserID WHERE fkOrderId=" & orderid

        OrdersNote.DataSource = ReadRecords(sql)
    End Sub

End Class

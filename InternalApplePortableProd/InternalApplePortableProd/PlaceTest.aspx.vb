Imports Telerik.Web.UI
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail

Partial Class PlaceTest
    Inherits Telerik.Web.UI.RadAjaxPage
    Dim orderID As String = ""
    Dim puser As PortalUser
    Public services As String
    Public portal As String
    Public search As String
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

        If puser.role = 1003 Then
            'RadButton2.CausesValidation = False
            'RadButton3.Visible = True
        Else
            tbStartDate.MinDate = Now.AddDays(5)
            tbPickupDate.MinDate = Now.AddDays(5)

            'tbStartDate.MinDate = Now.AddDays(10)
            'tbEndDate.MinDate = Now.AddDays(10)
        End If

        If Not IsPostBack Then
            search = ""
            fkUserID.Value = puser.userID


            'cbPState.DataSource = ReadRecords("select * from tblstate order by state asc")
            'cbPState.DataTextField = "State"
            'cbPState.DataValueField = "Code"
            'cbPState.DataBind()
            'Dim myItem2 As New RadComboBoxItem
            'myItem2.Value = "Select..."
            'myItem2.Text = "Select..."
            'cbPState.Items.Insert(0, myItem2)


            If Request.QueryString("orderID") <> "" Then
                orderID = Replace(Request.QueryString("orderID"), "'", "''")
                Dim dt = ReadRecords("Select * from tblOrderS where pkorderID=" & orderID)
                If dt.Rows.Count > 0 Then

                    If puser.userID <> CInt(dt(0)("fkuserid").ToString()) AndAlso puser.role <> 1001 Then
                        Response.Redirect("default.aspx")
                    End If


                    fkstatusID.Value = dt(0)("fkStatusID").ToString()

                    tbContactName.Text = dt(0)("ContactName").ToString()
                    tbContactEmail.Text = dt(0)("ContactEmail").ToString()
                    tbContactPhone.Text = dt(0)("ContactPhone").ToString()
                    tbEventName.Text = dt(0)("EventName").ToString()
                    'tbStartDate.SelectedDate = dt(0)("DateStart").ToString()
                    'tbEndDate.SelectedDate = dt(0)("DateEnd").ToString()
                    If dt(0)("DateArrive").ToString() <> "" Then
                        tbStartDate.SelectedDate = dt(0)("DateStart").ToString()
                        tbArrivalDate.SelectedDate = dt(0)("DateArrive").ToString()
                        tbPickupDate.SelectedDate = dt(0)("DatePickup").ToString()
                    End If

                    tbVenue.Text = dt(0)("EventVenueName").ToString()
                    tbWebsite.Text = dt(0)("Website").ToString()
                    If tbBoothSize.SelectedValue.IndexOf(dt(0)("BoothSize").ToString()) > 0 Then
                        tbBoothSize.SelectedValue = dt(0)("BoothSize").ToString()
                    Else
                        Dim myItem As New RadComboBoxItem
                        myItem.Value = dt(0)("BoothSize").ToString()
                        myItem.Text = dt(0)("BoothSize").ToString()
                        tbBoothSize.Items.Insert(0, myItem)
                        tbBoothSize.SelectedIndex = 0
                    End If

                    '=dt(0)("Attachments", ""))
                    tbBoothnum.Text = dt(0)("BoothNum").ToString()
                    cbBoothType.SelectedValue = dt(0)("BoothType").ToString()
                    cbElectrical.SelectedValue = dt(0)("Electrical").ToString()
                    If cbElectrical.SelectedValue.IndexOf(dt(0)("Electrical").ToString()) > 0 Then
                        cbElectrical.SelectedValue = dt(0)("Electrical").ToString()
                    Else
                        Dim myItem As New RadComboBoxItem
                        myItem.Value = dt(0)("Electrical").ToString()
                        myItem.Text = dt(0)("Electrical").ToString()
                        cbElectrical.Items.Insert(0, myItem)
                        cbElectrical.SelectedIndex = 0
                    End If
                    rbInternet.SelectedValue = dt(0)("Internet").ToString()
                    rbShipCarpet.SelectedValue = dt(0)("Carpet").ToString()
                    rbLeadRetrieval.SelectedValue = dt(0)("LeadRetrieval").ToString()
                    tbShipFurniture.Text = dt(0)("RentalFurniture").ToString()
                    tbNotes.Content = dt(0)("Notes").ToString()
                    '=dt(0)("DateOrder", Now()))
                    '=dt(0)("fkStatusID", 1000))
                    '=dt(0)("fkUserID", 1000))
                    tbShipTo.Text = dt(0)("ShipTo").ToString()
                    tbSOnsiteContact.Text = dt(0)("ShipContactName").ToString()
                    tbSOnsitePhone.Text = dt(0)("ShipContactPhone").ToString()
                    tbSOnsiteEmail.Text = dt(0)("ShipContactEmail").ToString()
                    tbSAddress1.Value = dt(0)("ShipAddress1").ToString()
                    tbSAddress2.Value = dt(0)("ShipAddress2").ToString()
                    tbSCity.Value = dt(0)("ShipCity").ToString()
                    cbSState.Value = dt(0)("ShipState").ToString()
                    tbSZip.Value = dt(0)("ShipZip").ToString()
                    cbSCountry.Value = dt(0)("ShipCountry").ToString()
                    tbPickupfrom.Text = dt(0)("PickupFrom").ToString()
                    tbPOnsiteContact.Text = dt(0)("PickupContactName").ToString()
                    tbPOnsitePhone.Text = dt(0)("PickupContactPhone").ToString()
                    tbPOnsiteEmail.Text = dt(0)("PickupContactEmail").ToString()
                    tbPAddress1.Text = dt(0)("PickupAddress1").ToString()
                    tbPAddress2.Text = dt(0)("PickupAddress2").ToString()
                    tbPCity.Text = dt(0)("PickupCity").ToString()
                    cbPState.SelectedValue = dt(0)("PickupState").ToString()
                    tbPZip.Text = dt(0)("PickupZip").ToString()
                    cbPCountry.SelectedValue = dt(0)("PickupCountry").ToString()
                    tbCostCenter.Text = dt(0)("CostCenter").ToString()
                    tbPortalpass.Text = dt(0)("ExhibitorPass").ToString()
                    tbPortaluser.Text = dt(0)("ExhibitorUser").ToString()
                    tbPortalsite.Text = dt(0)("ExhibitorWebsite").ToString()
                    rbMaterial.SelectedValue = dt(0)("Material").ToString()
                    rbLabor.SelectedValue = dt(0)("Labor").ToString()
                    fkUserID.Value = dt(0)("fkUserID").ToString()


                    If tbPortalsite.Text = "" Then
                        rbPortal.SelectedValue = "No"
                    Else
                        rbPortal.SelectedValue = "Yes"
                    End If

                    '=dt(0)(("UDF10", "Pending"))

                    'Get files
                    Dim targetFolder As String = Server.MapPath("Documents/" & orderID)
                    Dim di As New IO.DirectoryInfo(targetFolder)
                    Try
                        Dim aryFi As IO.FileInfo() = di.GetFiles("*.*")
                        Dim fi As IO.FileInfo
                        Dim uploaded = ""
                        For Each fi In aryFi
                            uploaded = fi.Name & " <br>"

                        Next
                    Catch ex As Exception

                    End Try
                End If

                'Dim dtitems = ReadRecords("Select * from tblOrderItems where fkOrderID = " & orderID)
                'If dtitems.Rows.Count > 0 Then

                'End If


            End If
            'CheckDefaults()
        End If

    End Sub


    'Protected Sub CountryPickup(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
    '    If cbPCountry.SelectedItem IsNot Nothing Then
    '        If cbPCountry.SelectedItem.Text = "UNITED STATES" Then

    '            cbPState.Visible = True
    '            tbPRegion.Visible = False
    '        ElseIf cbPCountry.SelectedItem.Text = "CANADA" Then

    '            cbPState.Visible = True
    '            tbPRegion.Visible = False

    '        Else

    '            cbPState.Visible = False
    '            tbPRegion.Visible = True
    '        End If

    '    End If

    'End Sub

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
            'ErrorEmail(ex.Message)
        End Try
        Return dt

    End Function

    Protected Sub RadButton2_Click(sender As Object, e As EventArgs) Handles RadButton2.Click
        Dim startDate = tbStartDate.SelectedDate
        Dim endDate = tbPickupDate.SelectedDate
        Dim today = Now

        If endDate < startDate Or startDate < today Then
            message.Text = "Invalid Start/End Date"
            Exit Sub
        Else
            message.Text = ""
        End If

        Dim validate As Boolean = True
        'If puser.role = 1000 Then
        '    validate = IsValid
        'End If

        If validate Then

            Dim pkid = SaveOrder(1000)
            If pkid.ToString.Contains("Error") Then
                lbError.Text = pkid
                Exit Sub

            End If

            Dim targetFolder As String = Server.MapPath("Documents/" & pkid & "/")
            System.IO.Directory.CreateDirectory(targetFolder)
            For Each file As UploadedFile In AsyncUpload1.UploadedFiles
                file.SaveAs(targetFolder & file.GetName, True)
            Next

            Session("pkid") = pkid
            Response.Redirect("InventorySelect.aspx?OrderID=" & pkid)
        End If
    End Sub

    Function SaveOrder(ByVal status)
        Dim pkid = ""

        Dim address1 = tbSAddress1.Value


        Dim tierCost = 0.0
        If tbBoothSize.SelectedValue.Contains("Softside") Then
            tierCost = 4250
        ElseIf tbBoothSize.SelectedValue.Contains("Hardside") Then
            tierCost = 9000
        ElseIf tbBoothSize.SelectedValue.Contains("Other") Then
            tierCost = 1500
        Else
            tierCost = 875
        End If

        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            If Request.QueryString("OrderID") = "" Then
                sqlComm.CommandText = "Insert into tblOrders ([ContactName],[ContactEmail],[ContactPhone],[EventName],[DateStart],[DateEnd],[DateArrive],[DatePickup],[EventVenueName],[Website],[BoothSize],[Attachments],[BoothNum],[BoothType],[Electrical],[Internet]" &
                                    ",[Carpet],[LeadRetrieval],[RentalFurniture],[Notes],[DateOrder],[fkStatusID],[fkUserID],[ShipTo],[ShipContactName],[ShipContactPhone],[ShipContactEmail],[ShipAddress1],[ShipAddress2],[ShipCity],[ShipState],[ShipZip] " &
                                    ",[ShipCountry],[PickupFrom],[PickupContactName],[PickupContactPhone],[PickupContactEmail],[PickupAddress1],[PickupAddress2],[PickupCity],[PickupState],[PickupZip],[PickupCountry],[CostCenter],[UDF10], [ExhibitorWebsite], [ExhibitorPass], [ExhibitorUser], [Material], [Labor], [TierCosts],[fkDepartmentID], [ShippingType]) values (@ContactName,@ContactEmail,@ContactPhone,@EventName,@DateStart,@DateEnd,@DateArrive,@DatePickup,@EventVenueName,@Website,@BoothSize,@Attachments,@BoothNum,@BoothType,@Electrical,@Internet " &
                                    ",@Carpet,@LeadRetrieval,@RentalFurniture,@Notes,@DateOrder,@fkStatusID,@fkUserID,@ShipTo,@ShipContactName,@ShipContactPhone,@ShipContactEmail,@ShipAddress1,@ShipAddress2,@ShipCity,@ShipState,@ShipZip " &
                                    ",@ShipCountry,@PickupFrom,@PickupContactName,@PickupContactPhone,@PickupContactEmail,@PickupAddress1,@PickupAddress2,@PickupCity,@PickupState,@PickupZip,@PickupCountry,@CostCenter,@UDF10,@ExhibitorWebsite, @ExhibitorPass, @ExhibitorUser, @Material, @Labor, @TierCosts, @fkDepartmentID, @ShippingType); SELECT SCOPE_IDENTITY()"
            Else
                sqlComm.CommandText = "Update tblOrders set [ContactName]=@ContactName,[ContactEmail]=@ContactEmail,[ContactPhone]=@ContactPhone,[EventName]=@EventName,[DateStart]=@DateStart,[DateEnd]=@DateEnd,[DateArrive]=@DateArrive,[DatePickup]=@DatePickup,[EventVenueName]=@EventVenueName,[Website]=@Website,[BoothSize]=@BoothSize,[Attachments]=@Attachments,[BoothNum]=@BoothNum,[BoothType]=@BoothType,[Electrical]=@Electrical,[Internet]=@Internet" &
                                    ",[Carpet]=@Carpet,[LeadRetrieval]=@LeadRetrieval,[RentalFurniture]=@RentalFurniture,[Notes]=@Notes,[DateOrder]=@DateOrder,[fkStatusID]=@fkStatusID,[fkUserID]=@fkUserID,[ShipTo]=@ShipTo,[ShipContactName]=@ShipContactName,[ShipContactPhone]=@ShipContactPhone,[ShipContactEmail]=@ShipContactEmail,[ShipAddress1]=@ShipAddress1,[ShipAddress2]=@ShipAddress2,[ShipCity]=@ShipCity,[ShipState]=@ShipState,[ShipZip]=@ShipZip " &
                                    ",[ShipCountry]=@ShipCountry,[PickupFrom]=@PickupFrom,[PickupContactName]=@PickupContactName,[PickupContactPhone]=@PickupContactPhone,[PickupContactEmail]=@PickupContactEmail,[PickupAddress1]=@PickupAddress1,[PickupAddress2]=@PickupAddress2,[PickupCity]=@PickupCity,[PickupState]=@PickupState,[PickupZip]=@PickupZip,[PickupCountry]=@PickupCountry,[CostCenter]=@CostCenter,[UDF10]=@UDF10, ExhibitorWebsite=@ExhibitorWebsite, ExhibitorPass=@ExhibitorPass, ExhibitorUser=@ExhibitorUser, Material=@Material, Labor=@Labor, TierCosts=@TierCosts, fkDepartmentID = @fkDepartmentID, ShippingType = @ShippingType where pkOrderID=@pkOrderID"
                sqlComm.Parameters.Add(New SqlParameter("pkorderid", Request.QueryString("OrderID")))
            End If

            sqlComm.Parameters.Add(New SqlParameter("ContactName", tbContactName.Text))
            sqlComm.Parameters.Add(New SqlParameter("ContactEmail", tbContactEmail.Text))
            sqlComm.Parameters.Add(New SqlParameter("ContactPhone", tbContactPhone.Text))
            sqlComm.Parameters.Add(New SqlParameter("EventName", tbEventName.Text))
            sqlComm.Parameters.Add(New SqlParameter("DateStart", tbStartDate.SelectedDate))
            sqlComm.Parameters.Add(New SqlParameter("DateEnd", tbPickupDate.SelectedDate))
            sqlComm.Parameters.Add(New SqlParameter("DateArrive", tbArrivalDate.SelectedDate))
            sqlComm.Parameters.Add(New SqlParameter("DatePickup", tbPickupDate.SelectedDate))
            sqlComm.Parameters.Add(New SqlParameter("EventVenueName", tbVenue.Text))
            sqlComm.Parameters.Add(New SqlParameter("Website", tbWebsite.Text))
            sqlComm.Parameters.Add(New SqlParameter("BoothSize", IIf(tbBoothSize.Text = "", tbBoothSize.SelectedValue, tbBoothSize.Text)))
            sqlComm.Parameters.Add(New SqlParameter("Attachments", ""))
            sqlComm.Parameters.Add(New SqlParameter("BoothNum", tbBoothnum.Text))
            sqlComm.Parameters.Add(New SqlParameter("BoothType", cbBoothType.SelectedValue))
            sqlComm.Parameters.Add(New SqlParameter("Electrical", IIf(cbElectrical.Text = "", cbElectrical.SelectedValue, cbElectrical.Text)))
            sqlComm.Parameters.Add(New SqlParameter("Internet", rbInternet.SelectedValue))
            sqlComm.Parameters.Add(New SqlParameter("Carpet", rbShipCarpet.SelectedValue))
            sqlComm.Parameters.Add(New SqlParameter("LeadRetrieval", rbLeadRetrieval.SelectedValue))
            sqlComm.Parameters.Add(New SqlParameter("RentalFurniture", tbShipFurniture.Text))
            sqlComm.Parameters.Add(New SqlParameter("Notes", tbNotes.Text))
            sqlComm.Parameters.Add(New SqlParameter("DateOrder", Now()))
            sqlComm.Parameters.Add(New SqlParameter("fkStatusID", status))
            sqlComm.Parameters.Add(New SqlParameter("fkUserID", puser.userID))
            sqlComm.Parameters.Add(New SqlParameter("ShipTo", tbShipTo.Text))
            sqlComm.Parameters.Add(New SqlParameter("ShipContactName", tbSOnsiteContact.Text))
            sqlComm.Parameters.Add(New SqlParameter("ShipContactPhone", tbContactPhone.Text))
            sqlComm.Parameters.Add(New SqlParameter("ShipContactEmail", tbContactEmail.Text))
            sqlComm.Parameters.Add(New SqlParameter("ShipAddress1", tbSAddress1.Value))
            sqlComm.Parameters.Add(New SqlParameter("ShipAddress2", tbSAddress2.Value))
            sqlComm.Parameters.Add(New SqlParameter("ShipCity", tbSCity.Value))
            sqlComm.Parameters.Add(New SqlParameter("ShipState", cbSState.Value))
            sqlComm.Parameters.Add(New SqlParameter("ShipZip", tbSZip.Value))
            sqlComm.Parameters.Add(New SqlParameter("ShipCountry", cbSCountry.Value))
            sqlComm.Parameters.Add(New SqlParameter("PickupFrom", tbPickupfrom.Text))
            sqlComm.Parameters.Add(New SqlParameter("PickupContactName", tbPOnsiteContact.Text))
            sqlComm.Parameters.Add(New SqlParameter("PickupContactPhone", tbPOnsitePhone.Text))
            sqlComm.Parameters.Add(New SqlParameter("PickupContactEmail", tbPOnsiteEmail.Text))
            sqlComm.Parameters.Add(New SqlParameter("PickupAddress1", tbPAddress1.Text))
            sqlComm.Parameters.Add(New SqlParameter("PickupAddress2", tbPAddress2.Text))
            sqlComm.Parameters.Add(New SqlParameter("PickupCity", tbPCity.Text))
            sqlComm.Parameters.Add(New SqlParameter("PickupState", cbPState.SelectedValue))
            sqlComm.Parameters.Add(New SqlParameter("PickupZip", tbPZip.Text))
            sqlComm.Parameters.Add(New SqlParameter("PickupCountry", cbPCountry.SelectedValue))
            sqlComm.Parameters.Add(New SqlParameter("CostCenter", tbCostCenter.Text))
            sqlComm.Parameters.Add(New SqlParameter("UDF10", ""))
            sqlComm.Parameters.Add(New SqlParameter("Material", rbMaterial.SelectedValue))
            sqlComm.Parameters.Add(New SqlParameter("Labor", rbLabor.SelectedValue))
            sqlComm.Parameters.Add(New SqlParameter("TierCosts", tierCost))
            sqlComm.Parameters.Add(New SqlParameter("fkDepartmentID", puser.department))
            sqlComm.Parameters.Add(New SqlParameter("ExhibitorWebsite", tbPortalsite.Text))
            sqlComm.Parameters.Add(New SqlParameter("ExhibitorPass", tbPortalpass.Text))
            sqlComm.Parameters.Add(New SqlParameter("ExhibitorUser", tbPortaluser.Text))
            sqlComm.Parameters.Add(New SqlParameter("ShippingType", rbShippingType.SelectedValue))


            Try
                pkid = sqlComm.ExecuteScalar()
                If Request.QueryString("OrderID") <> "" Then
                    pkid = Request.QueryString("OrderID")
                End If
            Catch ex As Exception
                pkid = "Error: " & ex.Message
                'ErrorEmail(ex.Message)
            End Try
        End Using
        Return pkid
    End Function

    'Protected Sub RadButton1_Click(sender As Object, e As EventArgs) Handles RadButton1.Click
    '    Grid1.Rebind()
    'End Sub


    Protected Sub datechanged()
        tbArrivalDate.SelectedDate = CDate(tbStartDate.SelectedDate).Date.AddDays(-1)
        'If Not tbArrivalDate.IsEmpty AndAlso Not tbPickupDate.IsEmpty Then
        '    GetInventory()
        'End If
    End Sub

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
End Class

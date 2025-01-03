Imports Telerik.Web.UI
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.IO

Partial Class PlaceTest
    Inherits Telerik.Web.UI.RadAjaxPage
    Dim orderID As String = ""
    Dim puser As PortalUser
    Public services As String
    Public portal As String
    Public search As String
    Public status As Int16
    Public assetArrivalDate As DateTime

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If IsNothing(Session("user")) Then
            Dim ticket = Request.Cookies(FormsAuthentication.FormsCookieName).Value
            Dim dycriptedTicket = FormsAuthentication.Decrypt(ticket)
            If dycriptedTicket.UserData = "" Then
                Dim strFile As String = "C:\inetpub\logs\LogFiles\W3SVC2\ExcLogs" & DateTime.Today.ToString("dd-MMM-yyyy") & ".txt"
                Dim sw As StreamWriter
                Try
                    If (Not File.Exists(strFile)) Then
                        sw = File.CreateText(strFile)
                        sw.WriteLine("Start Error Log")
                    Else
                        sw = File.AppendText(strFile)
                    End If
                    sw.WriteLine("Redirect Occured at-- " & DateTime.Now)
                    sw.Close()
                Catch ex As IOException
                    MsgBox("Error writing to log file.")
                End Try

                Response.Redirect("account/login.aspx")
            End If
            Dim t As New sessionmanager(dycriptedTicket.UserData)
        End If

        puser = Session("user")


        'If puser.role = 1003 Then

        'Else
        '    tbStartDate.MinDate = Now.AddDays(5)
        '    tbPickupDate.MinDate = Now.AddDays(5)
        '    tbArrivalDate.MinDate = Now.AddDays(4)
        'End If



        If Not IsPostBack Then

            'If puser.role = 1003 Or puser.userID = 1047 Or puser.userID = 1002 Or puser.userID = 1014 Or puser.userID = 1016 Then
            '    Dim item1 As New RadComboBoxItem()
            '    item1.Text = "United States"
            '    item1.Value = "United States"
            '    cbsLocation.Items.Add(item1)
            '    Dim item2 As New RadComboBoxItem()
            '    item2.Text = "Europe"
            '    item2.Value = "Europe"
            '    cbsLocation.Items.Add(item2)
            '    Dim item3 As New RadComboBoxItem()
            '    item3.Text = "Japan"
            '    item3.Value = "Japan"
            '    cbsLocation.Items.Add(item3)
            '    Dim item4 As New RadComboBoxItem()
            '    item4.Text = "China"
            '    item4.Value = "China"
            '    cbsLocation.Items.Add(item4)
            '    Dim item5 As New RadComboBoxItem()
            '    item5.Text = "Australia"
            '    item5.Value = "Australia"
            '    cbsLocation.Items.Add(item5)
            'Else

            '    Select Case puser.department
            '        Case 1004
            '            Dim item1 As New RadComboBoxItem()
            '            item1.Text = "Europe"
            '            item1.Value = "Europe"
            '            cbsLocation.Items.Add(item1)
            '    End Select
            '    tbStartDate.MinDate = Now.AddDays(5)
            '    tbPickupDate.MinDate = Now.AddDays(5)
            '    tbArrivalDate.MinDate = Now.AddDays(4)
            'End If


            search = ""
            fkUserID.Value = puser.userID


            'cbSCountry.DataSource = ReadRecords("Select * from tblCountry")
            'cbSCountry.DataTextField = "CountryName"
            'cbSCountry.DataValueField = "CountryCode"
            'cbSCountry.DataBind()


            If Request.QueryString("orderID") <> "" Then
                orderID = Replace(Request.QueryString("orderID"), "'", "''")
                Dim dt = ReadRecords("Select * from tblOrderS where pkorderID=" & orderID)
                If dt.Rows.Count > 0 Then

                    'If puser.userID <> CInt(dt(0)("fkuserid").ToString()) AndAlso puser.role <> 1003 Then
                    '    Response.Redirect("default.aspx")
                    'End If

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
                        arrivalDateHolder.Value = dt(0)("DateArrive")
                        dateEnd.Value = dt(0)("DatePickup")
                        dateStart.Value = dt(0)("DateStart")
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
                    cbSCountry.SelectedValue = dt(0)("ShipCountry").ToString()
                    shipCountry.Value = dt(0)("ShipCountry").ToString()
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
                    tbTracking.Text = dt(0)("TrackingNumber").ToString()
                    cbsLocation.SelectedValue = dt(0)("fkFulfillmentLocationId").ToString()
                    'AURegionalAdmin.Value = dt(0)("AURegionalAdmin").ToString()
                    'CNRegionalAdmin.Value = dt(0)("CNRegionalAdmin").ToString()
                    'JPRegionalAdmin.Value = dt(0)("JPRegionalAdmin").ToString()


                    If tbPortalsite.Text = "" Then
                        rbPortal.SelectedValue = "No"
                    Else
                        rbPortal.SelectedValue = "Yes"
                    End If


                    statusHolder.Value = dt(0)("fkStatusID")


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


                Dim selectedCountry = cbSCountry.Text
                Dim dt3 = ReadRecords("select distinct ShippingCity from tblcarton where ShippingCountry ='" & selectedCountry.ToString() & "'")
                If dt3.Rows.Count > 0 Then
                    nonAPACcity.Visible = False
                    APACcity.Visible = True
                    ddlShipCity.DataSource = dt3
                    ddlShipCity.DataTextField = "ShippingCity"
                    ddlShipCity.DataValueField = "ShippingCity"
                    ddlShipCity.DataBind()
                    ddlShipCity.SelectedValue = tbSCity.Value
                Else
                    nonAPACcity.Visible = True
                    APACcity.Visible = False
                End If

                'Dim dtitems = ReadRecords("Select * from tblOrderItems where fkOrderID = " & orderID)
                'If dtitems.Rows.Count > 0 Then

                'End If

                'trTracking.Visible = True

                If puser.role = 1003 Or puser.role = 1001 Then
                    notes.Visible = True
                    notesContainer.Visible = True
                    'notes.Visible = True
                End If

                Dim dt2 = ReadRecords("Select * from tblOrderItems where fkorderID=" & orderID)

                If dt2.Rows.Count < 1 Then
                    RadButton3.Visible = False
                End If

                SaveOrder(1002, False)


            Else
                trTracking.Visible = False
                tbContactName.Text = puser.firstname + " " + puser.lastname
                tbContactEmail.Text = puser.email
                notes.Visible = False
                notesContainer.Visible = False
                RadButton3.Visible = False
                RadButton4.Visible = False
                APACcity.Visible = False

            End If
            'CheckDefaults()
        End If



        fulfillmentlocationholder.Visible = False
        serviceCountry.Visible = False

        'If Not puser.role = 1003 Then
        '    If puser.role = 1001 Then
        '        If puser.department = 1006 Or puser.department = 1007 Or puser.department = 1008 Then
        '            fulfillmentlocationholder.Visible = True
        '        Else
        '            fulfillmentlocationholder.Visible = False
        '        End If
        '    ElseIf puser.role = 1006 Then
        '        fulfillmentlocationholder.Visible = True
        '    Else
        '        fulfillmentlocationholder.Visible = False
        '    End If

        'End If


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
        Dim pkid
        Dim fullfilmentLocation = cbsLocation.Text

        If endDate < startDate Or startDate < today Then
            message.Text = "Invalid Start/End Date"
            Exit Sub
        Else
            message.Text = ""
        End If

        Dim shipCountryVal = String.Empty
        If shipCountry.Value.ToString() <> "" Then
            shipCountryVal = shipCountry.Value.ToString()
        Else
            shipCountryVal = cbSCountry.Text
        End If

        Dim isServiceCountry = validateShipToCountry(shipCountryVal)

        If Not isServiceCountry Then
            serviceCountry.Visible = True
            Exit Sub
        End If

        Dim validate As Boolean = True
        'If puser.role = 1000 Then
        '    validate = IsValid
        'End If

        If validate Then

            'Dim pkid = SaveOrder(statusHolder.Value)
            If statusHolder.Value = "" Then
                pkid = SaveOrder(1000)
                'If fullfilmentLocation = "United States" Or fullfilmentLocation = "Europe" Then
                '    pkid = SaveOrder(1000)
                'Else
                '    pkid = SaveOrder(1001)
                'End If
            Else
                pkid = SaveOrder(statusHolder.Value, True)
                'If fullfilmentLocation = "United States" Or fullfilmentLocation = "Europe" Then
                '    pkid = SaveOrder(statusHolder.Value, True)
                'Else
                '    pkid = SaveOrder(1001)
                'End If
            End If

            If pkid.ToString.Contains("Error") Then
                lbError.Text = pkid
                Exit Sub

            End If

            If tbChangeLog.Text <> "" Then
                AddOrderChangeLog(pkid)
            End If

            'Dim targetFolder As String = Server.MapPath("Documents/" & pkid & "/")
            'System.IO.Directory.CreateDirectory(targetFolder)
            'For Each file As UploadedFile In RadAsyncUpload1.UploadedFiles
            '    file.SaveAs(targetFolder & file.GetName, True)
            'Next

            Session("pkid") = pkid
            If Request.QueryString("orderID") <> "" Then
                Response.Redirect("InventorySelect.aspx?OrderID=" & pkid & "&editOrder=true")
            Else
                Response.Redirect("InventorySelect.aspx?OrderID=" & pkid)
            End If

        End If
    End Sub

    Function AddOrderChangeLog(pkid)

        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "Insert into tblOrderNotes([fkOrderId], [fkUserId], [DateCreated], [Notes]) values (@fkOrderId,@fkUserId,@DateCreated,@Notes); SELECT SCOPE_IDENTITY()"

            sqlComm.Parameters.Add(New SqlParameter("fkOrderId", pkid))
            sqlComm.Parameters.Add(New SqlParameter("fkUserId", puser.userID))
            sqlComm.Parameters.Add(New SqlParameter("DateCreated", Now()))
            sqlComm.Parameters.Add(New SqlParameter("Notes", tbChangeLog.Text))

            Try
                Dim noteId = sqlComm.ExecuteScalar()
            Catch ex As Exception
                Dim errorM = ex.Message
            End Try
        End Using
    End Function

    Function SaveOrder(ByVal status As String, Optional ByVal isSave As Boolean = False)
        Dim pkid = ""

        Dim address1 = tbSAddress1.Value

        'If arrivalDateHolder.Value <> "" Then
        '    If tbArrivalDate.SelectedDate <> arrivalDateHolder.Value Then
        '        status = 1000
        '    End If
        'End If

        Dim shipCountryVal = String.Empty
        If shipCountry.Value.ToString() <> "" Then
            shipCountryVal = shipCountry.Value.ToString()
        Else
            shipCountryVal = cbPCountry.SelectedValue
        End If

        Dim shipCityVal = String.Empty
        Dim dt = ReadRecords("select distinct ShippingCity from tblcarton where ShippingCountry ='" & shipCountryVal & "'")
        If dt.Rows.Count > 0 Then
            shipCityVal = ddlShipCity.SelectedValue
        Else
            shipCityVal = tbSCity.Value
        End If

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
                                    ",[ShipCountry],[PickupFrom],[PickupContactName],[PickupContactPhone],[PickupContactEmail],[PickupAddress1],[PickupAddress2],[PickupCity],[PickupState],[PickupZip],[PickupCountry],[CostCenter],[UDF10], [ExhibitorWebsite], [ExhibitorPass], [ExhibitorUser], [Material], [Labor], [TierCosts],[fkDepartmentID], [ShippingType], [fkFulfillmentLocationId]) values (@ContactName,@ContactEmail,@ContactPhone,@EventName,@DateStart,@DateEnd,@DateArrive,@DatePickup,@EventVenueName,@Website,@BoothSize,@Attachments,@BoothNum,@BoothType,@Electrical,@Internet " &
                                    ",@Carpet,@LeadRetrieval,@RentalFurniture,@Notes,@DateOrder,@fkStatusID,@fkUserID,@ShipTo,@ShipContactName,@ShipContactPhone,@ShipContactEmail,@ShipAddress1,@ShipAddress2,@ShipCity,@ShipState,@ShipZip " &
                                    ",@ShipCountry,@PickupFrom,@PickupContactName,@PickupContactPhone,@PickupContactEmail,@PickupAddress1,@PickupAddress2,@PickupCity,@PickupState,@PickupZip,@PickupCountry,@CostCenter,@UDF10,@ExhibitorWebsite, @ExhibitorPass, @ExhibitorUser, @Material, @Labor, @TierCosts, @fkDepartmentID, @ShippingType, @fkFulfillmentLocationId); SELECT SCOPE_IDENTITY()"
            Else
                sqlComm.Parameters.Add(New SqlParameter("pkorderid", Request.QueryString("OrderID")))

                If isSave Then
                    sqlComm.CommandText = "Update tmpOrders set [ContactName]=@ContactName,[ContactEmail]=@ContactEmail,[ContactPhone]=@ContactPhone,[EventName]=@EventName,[DateStart]=@DateStart,[DateEnd]=@DateEnd,[DateArrive]=@DateArrive,[DatePickup]=@DatePickup,[EventVenueName]=@EventVenueName,[Website]=@Website,[BoothSize]=@BoothSize,[Attachments]=@Attachments,[BoothNum]=@BoothNum,[BoothType]=@BoothType,[Electrical]=@Electrical,[Internet]=@Internet" &
                                    ",[Carpet]=@Carpet,[LeadRetrieval]=@LeadRetrieval,[RentalFurniture]=@RentalFurniture,[Notes]=@Notes,[DateModified]=@DateModified,[fkStatusID]=@fkStatusID,[ShipTo]=@ShipTo,[ShipContactName]=@ShipContactName,[ShipContactPhone]=@ShipContactPhone,[ShipContactEmail]=@ShipContactEmail,[ShipAddress1]=@ShipAddress1,[ShipAddress2]=@ShipAddress2,[ShipCity]=@ShipCity,[ShipState]=@ShipState,[ShipZip]=@ShipZip " &
                                    ",[ShipCountry]=@ShipCountry,[PickupFrom]=@PickupFrom,[PickupContactName]=@PickupContactName,[PickupContactPhone]=@PickupContactPhone,[PickupContactEmail]=@PickupContactEmail,[PickupAddress1]=@PickupAddress1,[PickupAddress2]=@PickupAddress2,[PickupCity]=@PickupCity,[PickupState]=@PickupState,[PickupZip]=@PickupZip,[PickupCountry]=@PickupCountry,[CostCenter]=@CostCenter,[UDF10]=@UDF10, ExhibitorWebsite=@ExhibitorWebsite, ExhibitorPass=@ExhibitorPass, ExhibitorUser=@ExhibitorUser, Material=@Material, Labor=@Labor, TierCosts=@TierCosts, TrackingNumber = @TrackingNumber,  ShippingType = @ShippingType, fkFulfillmentLocationId = @fkFulfillmentLocationId where pkOrderID=@pkOrderID and sessionid = (SELECT MAX(sessionid) FROM tmpOrders)"
                Else
                    archiveSession()

                    sqlComm.CommandText = "Insert into tmpOrders ([ContactName],[ContactEmail],[ContactPhone],[EventName],[DateStart],[DateEnd],[DateArrive],[DatePickup],[EventVenueName],[Website],[BoothSize],[Attachments],[BoothNum],[BoothType],[Electrical],[Internet]" &
                                    ",[Carpet],[LeadRetrieval],[RentalFurniture],[Notes],[DateOrder],[fkStatusID],[fkUserID],[ShipTo],[ShipContactName],[ShipContactPhone],[ShipContactEmail],[ShipAddress1],[ShipAddress2],[ShipCity],[ShipState],[ShipZip] " &
                                    ",[ShipCountry],[PickupFrom],[PickupContactName],[PickupContactPhone],[PickupContactEmail],[PickupAddress1],[PickupAddress2],[PickupCity],[PickupState],[PickupZip],[PickupCountry],[CostCenter],[UDF10], [ExhibitorWebsite], [ExhibitorPass], [ExhibitorUser], [Material], [Labor], [TierCosts],[fkDepartmentID], [ShippingType], [fkFulfillmentLocationId], [pkOrderId], [isarchived], [dateCreated]) values (@ContactName,@ContactEmail,@ContactPhone,@EventName,@DateStart,@DateEnd,@DateArrive,@DatePickup,@EventVenueName,@Website,@BoothSize,@Attachments,@BoothNum,@BoothType,@Electrical,@Internet " &
                                    ",@Carpet,@LeadRetrieval,@RentalFurniture,@Notes,@DateOrder,@fkStatusID,@fkUserID,@ShipTo,@ShipContactName,@ShipContactPhone,@ShipContactEmail,@ShipAddress1,@ShipAddress2,@ShipCity,@ShipState,@ShipZip " &
                                    ",@ShipCountry,@PickupFrom,@PickupContactName,@PickupContactPhone,@PickupContactEmail,@PickupAddress1,@PickupAddress2,@PickupCity,@PickupState,@PickupZip,@PickupCountry,@CostCenter,@UDF10,@ExhibitorWebsite, @ExhibitorPass, @ExhibitorUser, @Material, @Labor, @TierCosts, @fkDepartmentID, @ShippingType, @fkFulfillmentLocationId,@pkOrderId, @isarchived, @dateCreated); SELECT SCOPE_IDENTITY()"


                End If
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
            sqlComm.Parameters.Add(New SqlParameter("ShipContactPhone", tbSOnsitePhone.Text))
            sqlComm.Parameters.Add(New SqlParameter("ShipContactEmail", tbSOnsiteEmail.Text))
            sqlComm.Parameters.Add(New SqlParameter("ShipAddress1", tbSAddress1.Value))
            sqlComm.Parameters.Add(New SqlParameter("ShipAddress2", tbSAddress2.Value))
            sqlComm.Parameters.Add(New SqlParameter("ShipCity", shipCityVal))

            sqlComm.Parameters.Add(New SqlParameter("ShipState", cbSState.Value))
            sqlComm.Parameters.Add(New SqlParameter("ShipZip", tbSZip.Value))
            sqlComm.Parameters.Add(New SqlParameter("ShipCountry", shipCountryVal))
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
            sqlComm.Parameters.Add(New SqlParameter("TrackingNumber", tbTracking.Text))
            sqlComm.Parameters.Add(New SqlParameter("fkDepartmentID", puser.department))
            sqlComm.Parameters.Add(New SqlParameter("ExhibitorWebsite", tbPortalsite.Text))
            sqlComm.Parameters.Add(New SqlParameter("ExhibitorPass", tbPortalpass.Text))
            sqlComm.Parameters.Add(New SqlParameter("ExhibitorUser", tbPortaluser.Text))
            sqlComm.Parameters.Add(New SqlParameter("ShippingType", rbShippingType.SelectedValue))
            sqlComm.Parameters.Add(New SqlParameter("fkFulfillmentLocationId", cbsLocation.SelectedValue))
            sqlComm.Parameters.Add(New SqlParameter("DateModified", Now()))
            sqlComm.Parameters.Add(New SqlParameter("isarchived", 0))
            sqlComm.Parameters.Add(New SqlParameter("dateCreated", Now()))
            'sqlComm.Parameters.Add(New SqlParameter("AURegionalAdmin", AURegionalAdmin.Value))
            'sqlComm.Parameters.Add(New SqlParameter("CNRegionalAdmin", CNRegionalAdmin.Value))
            'sqlComm.Parameters.Add(New SqlParameter("JPRegionalAdmin", JPRegionalAdmin.Value))

            Try
                pkid = sqlComm.ExecuteScalar()
                If Request.QueryString("OrderID") <> "" Then
                    If isSave = False Then
                        AddTempOrderitems(pkid)
                    End If
                    If pkid = "" Then
                        pkid = Request.QueryString("OrderID")
                    End If
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
        Dim startDte = CDate(tbStartDate.SelectedDate).Date
        If startDte.DayOfWeek = DayOfWeek.Monday Or startDte.DayOfWeek = DayOfWeek.Saturday Or startDte.DayOfWeek = DayOfWeek.Sunday Then
            tbArrivalDate.SelectedDate = AddBusinessDays(CDate(tbStartDate.SelectedDate).Date, -1)
        Else
            tbArrivalDate.SelectedDate = CDate(tbStartDate.SelectedDate).Date.AddDays(-1)
        End If
    End Sub

    Public Shared Function AddBusinessDays(ByVal startDate As DateTime,
                                      ByVal businessDays As Integer) As DateTime
        Dim di As Integer
        Dim calendarDays As Integer

        'di:     shift to Friday. If its Sat Or Sun dont shift
        di = (businessDays - Math.Max(0, (5 - startDate.DayOfWeek)))

        'Start = Friday -> add di/5 weeks -> End = Friday
        'If the Then the remaining(<5 days) Is > 0: add it + 2 days (Sat+Sun)
        'shift back to initial day
        calendarDays = ((((di / 5) * 7) _
                       + IIf(((di Mod 5) <> 0), (2 + (di Mod 5)), 0)) _
                       + (5 - startDate.DayOfWeek))

        Return startDate.AddDays(CDbl(calendarDays))

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

    Protected Sub RadButton4_Click(sender As Object, e As EventArgs) Handles RadButton4.Click
        archiveSession()
        Response.Redirect("Reports.aspx", False)
    End Sub


    Function archiveSession()
        Dim pkid = Request.QueryString("orderID")

        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "update tmpOrders set isarchived = 1 where pkOrderId = @pkid;update tmpOrderItems set isarchived = 1 where fkOrderID = @pkid;"
            sqlComm.Parameters.Add(New SqlParameter("pkid", pkid))

            Try
                sqlComm.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        End Using
    End Function


    Protected Sub RadButton3_Click(sender As Object, e As EventArgs) Handles RadButton3.Click

        Dim startDate = tbStartDate.SelectedDate
        Dim endDate = tbPickupDate.SelectedDate
        Dim today = Now

        If endDate < startDate Or startDate < today Then
            message.Text = "Invalid Start/End Date"
            Exit Sub
        Else
            message.Text = ""
        End If


        Dim pkid = SaveOrder(statusHolder.Value, True)
        If pkid.ToString.Contains("Error") Then
            lbError.Text = pkid
            Exit Sub
        End If

        If tbChangeLog.Text <> "" Then
            AddOrderChangeLog(pkid)
        End If

        Session("pkid") = pkid
        Response.Redirect("Review.aspx?existingOrder=true", False)

    End Sub

    Function AddTempOrderitems(sessionid As String) As Object
        Dim dt2 = ReadRecords("Select * from tblOrderItems where fkorderID=" & orderID)

        If dt2.Rows.Count > 0 Then
            For Each r In dt2.Rows
                Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
                    strConnStr.Open()

                    Dim sqlComm As New SqlCommand()
                    sqlComm.Connection = strConnStr

                    sqlComm.CommandText = "Insert into tmpOrderItems([fkInventoryID], [Quantity], [fkOrderID], [ExtendedCost], [dateCreated], [isarchived], [sessionID]) values (@fkInventoryID,@Quantity,@fkOrderID,@ExtendedCost,@dateCreated, @isarchived, @sessionID);"

                    sqlComm.Parameters.Add(New SqlParameter("fkInventoryID", dt2(0)("fkinventoryID")))
                    sqlComm.Parameters.Add(New SqlParameter("Quantity", dt2(0)("Quantity")))
                    sqlComm.Parameters.Add(New SqlParameter("fkOrderID", dt2(0)("fkOrderID")))
                    sqlComm.Parameters.Add(New SqlParameter("ExtendedCost", ""))
                    sqlComm.Parameters.Add(New SqlParameter("dateCreated", Now()))
                    sqlComm.Parameters.Add(New SqlParameter("isarchived", 0))
                    sqlComm.Parameters.Add(New SqlParameter("sessionID", sessionid))

                    Try
                        sqlComm.ExecuteScalar()
                    Catch ex As Exception

                    End Try
                End Using
            Next
        End If
    End Function

    Protected Sub RadCB_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        shipCountry.Value = cbSCountry.Text

    End Sub

    'Function populateCity(selectedCountry)
    '    Dim dt = ReadRecords("select distinct ShippingCity from tblcarton where ShippingCountry =" & selectedCountry)
    '    If dt.Rows.Count > 0 Then
    '        nonAPACcity.Visible = False
    '        APACcity.Visible = True
    '        ddlShipCity.DataSource = dt
    '        ddlShipCity.DataTextField = "ShippingCity"
    '        ddlShipCity.DataValueField = "ShippingCity"
    '        ddlShipCity.DataBind()
    '    Else
    '        nonAPACcity.Visible = True
    '        APACcity.Visible = False
    '    End If

    'End Function


    Function validateShipToCountry(country) As Boolean

        If puser.department = 1004 Then
            Return True
        Else
            Dim dt2 = ReadRecords("Select * from tblShipCountry where ShipCountryName='" & country.ToString() & "'")
            If dt2.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End If

    End Function


    Protected Sub RadComboBox1_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
        Dim selectedCountry = cbSCountry.Text
        Dim dt = ReadRecords("select distinct ShippingCity from tblcarton where ShippingCountry ='" & selectedCountry.ToString() & "'")
        If dt.Rows.Count > 0 Then
            nonAPACcity.Visible = False
            APACcity.Visible = True
            ddlShipCity.DataSource = dt
            ddlShipCity.DataTextField = "ShippingCity"
            ddlShipCity.DataValueField = "ShippingCity"
            ddlShipCity.DataBind()
        Else
            nonAPACcity.Visible = True
            APACcity.Visible = False
        End If
    End Sub


End Class

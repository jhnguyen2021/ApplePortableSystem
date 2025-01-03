Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Net.Mail
Imports System.IO


Partial Class Review
    Inherits Telerik.Web.UI.RadAjaxPage
    Dim puser As PortalUser
    Public services As String = "none"
    Public portal As String = "none"
    Public fulfillmentLocation As String
    Public assetArrivalDate As Date



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        puser = Session("user")
        cctable.Visible = False
        RadButton2.CausesValidation = False


        If (Request.QueryString.AllKeys.Contains("approveOrder")) Then
            RadButton2.Visible = False
            RadButton3.Visible = False
            RadButton1.Visible = False
            contactInfo.Visible = True

            If IsNothing(Session("pkid")) Then
                Session("pkid") = Request.QueryString("OrderID")
            End If

            contactInfo.Visible = True

        Else
            contactInfo.Visible = False
            RadButton2.Visible = True
            RadButton3.Visible = True
            RadButton1.Visible = True
            tblDecision.Visible = False

        End If


        RadGrid1.RegisterWithScriptManager = False

        If Not IsPostBack Then

            ddlccstate.DataSource = ReadRecords("select * from tblstate order by state asc")
            ddlccstate.DataTextField = "State"
            ddlccstate.DataValueField = "Code"
            ddlccstate.DataBind()
            Dim myItem2 As New RadComboBoxItem
            myItem2.Value = "Select..."
            myItem2.Text = "Select..."
            ddlccstate.Items.Insert(0, myItem2)



            Dim y As Date = Now()
            Dim lc As New ListItemCollection
            Dim ls1 As New ListItem("-")
            lc.Add(ls1)
            For i = 0 To 5
                Dim ls As New ListItem
                ls.Text = y.AddYears(i).Year
                ls.Value = y.AddYears(i).Year
                'ddlYears.Attributes.Add(y.AddYears(i).Year, y.AddYears(i).Year)
                lc.Add(ls)

            Next
            ddlYears.DataSource = lc
            ddlYears.DataBind()


            Dim str = order.InnerHtml
            Dim str2 = adminName.InnerHtml
            Dim str3 = adminEmail.InnerHtml
            Dim str4 = totalNumberItems.InnerHtml
            Dim str5 = totalNumberBoxes.InnerHtml
            Dim str6 = totalShippingCost.InnerHtml


            If Not IsNothing(Session("pkid")) Then
                Dim dt As DataTable
                Dim dt2 As DataTable
                Dim dt3 As DataTable
                Dim dt4 As DataTable
                Dim dt5 As DataTable
                Dim dt6 As DataTable


                If (Request.QueryString.AllKeys.Contains("editOrder")) Then
                    dt = ReadRecords("Select * from tmpOrders where sessionid = (SELECT MAX(sessionid) FROM tmpOrders) and isarchived = 0 and pkorderID= " & Session("pkid"))
                    dt2 = ReadRecords("SELECT distinct tmpOrderItems.Quantity, vwinventoryview.pkInventoryID, vwinventoryview.description, vwinventoryview.Name, vwinventoryview.PartNum, vwinventoryview.Picture ,vwInventoryView.fkDepartmentID,  vwInventoryView.Notes  FROM tmporders INNER JOIN " &
                                                      "tmpOrderItems ON tmporders.pkOrderID = tmpOrderItems.fkOrderID INNER JOIN vwinventoryview ON tmpOrderItems.fkInventoryID = vwinventoryview.pkInventoryID where tmpOrderItems.isarchived = 0 and tmporders.pkOrderID = " & Session("pkid"))

                    dt3 = ReadRecords("select Sum(Quantity) as Total from  tmpOrderItems  where isarchived = 0  and fkOrderID =" & Session("pkid"))

                ElseIf (Request.QueryString.AllKeys.Contains("existingOrder")) Then
                    dt = ReadRecords("Select * from tmpOrders where sessionid = (SELECT MAX(sessionid) FROM tmpOrders) and isarchived = 0 and pkorderID= " & Session("pkid"))
                    dt2 = ReadRecords("SELECT tblOrderItems.Quantity, vwinventoryview.pkInventoryID, vwinventoryview.description,vwinventoryview.Name, vwinventoryview.PartNum, vwinventoryview.Picture ,vwInventoryView.fkDepartmentID ,  vwInventoryView.Notes , vwinventoryview.location FROM tblOrders INNER JOIN " &
                                                     "tblOrderItems ON tblOrders.pkOrderID = tblOrderItems.fkOrderID INNER JOIN vwinventoryview ON tblOrderItems.fkInventoryID = vwinventoryview.pkInventoryID where tblOrders.pkOrderID = " & Session("pkid"))
                    dt3 = ReadRecords("select sum(Quantity) as Total from tblorderitems where fkorderid =" & Session("pkid"))

                Else
                    dt = ReadRecords("Select * from tblOrders where pkOrderID = " & Session("pkid"))
                    dt2 = ReadRecords("SELECT tblOrderItems.Quantity, vwinventoryview.pkInventoryID, vwinventoryview.description,vwinventoryview.Name, vwinventoryview.PartNum, vwinventoryview.Picture ,vwInventoryView.fkDepartmentID , vwInventoryView.Notes , vwinventoryview.location FROM tblOrders INNER JOIN " &
                                                      "tblOrderItems ON tblOrders.pkOrderID = tblOrderItems.fkOrderID INNER JOIN vwinventoryview ON tblOrderItems.fkInventoryID = vwinventoryview.pkInventoryID where tblOrders.pkOrderID = " & Session("pkid"))
                    dt3 = ReadRecords("select sum(Quantity) as Total from tblorderitems where fkorderid =" & Session("pkid"))
                End If




                If dt.Rows.Count > 0 Then

                    str = Regex.Replace(str, "\[ContactName\]", dt(0)("ContactName").ToString())
                    str2 = Regex.Replace(str2, "\[AdminContactName\]", puser.firstname + " " + puser.lastname)
                    str3 = Regex.Replace(str3, "\[AdminContactEmail\]", puser.email)
                    contactEmail.Value = dt(0)("ContactEmail").ToString()
                    str = Regex.Replace(str, "\[ContactEmail\]", dt(0)("ContactEmail").ToString())
                    str = Regex.Replace(str, "\[ContactPhone\]", dt(0)("ContactPhone").ToString())
                    str = Regex.Replace(str, "\[CostCenter\]", dt(0)("CostCenter").ToString())
                    str = Regex.Replace(str, "\[EventName\]", dt(0)("EventName").ToString())
                    str = Regex.Replace(str, "\[DateStart\]", dt(0)("DateStart").ToString())
                    str = Regex.Replace(str, "\[DateEnd\]", dt(0)("DateEnd").ToString())
                    str = Regex.Replace(str, "\[EventVenue\]", dt(0)("EventVenueName").ToString())
                    str = Regex.Replace(str, "\[Website\]", dt(0)("Website").ToString())
                    str = Regex.Replace(str, "\[DateArrive\]", CDate(dt(0)("DateArrive").ToString()).Date())
                    assetArrivalDate = CDate(dt(0)("DateArrive").ToString()).Date()
                    assetArrivalDate.AddDays(-14)
                    assetArrivalDateHolder.Value = assetArrivalDate
                    dateModified.Value = dt(0)("DateModified").ToString()

                    str = Regex.Replace(str, "\[DatePickup\]", dt(0)("DatePickup").ToString())
                    str = Regex.Replace(str, "\[BoothSize\]", dt(0)("BoothSize").ToString())
                    str = Regex.Replace(str, "\[Boothnum\]", dt(0)("Boothnum").ToString())
                    str = Regex.Replace(str, "\[BoothType\]", dt(0)("BoothType").ToString())
                    lbboothsize.Value = dt(0)("BoothSize").ToString()
                    If dt(0)("BoothSize").ToString.Contains("10") Or dt(0)("BoothSize").ToString.Contains("20") Then
                        services = "table-row"

                    End If

                    If dt(0)("ExhibitorWebsite").ToString <> "" Then
                        portal = "table-row"

                    End If

                    str = Regex.Replace(str, "\[Electrical\]", dt(0)("Electrical").ToString())
                    str = Regex.Replace(str, "\[Internet\]", dt(0)("Internet").ToString())
                    str = Regex.Replace(str, "\[Carpet\]", dt(0)("Carpet").ToString())
                    str = Regex.Replace(str, "\[LeadRetrieval\]", dt(0)("LeadRetrieval").ToString())
                    str = Regex.Replace(str, "\[RentalFurniture\]", dt(0)("RentalFurniture").ToString())

                    str = Regex.Replace(str, "\[Shipto\]", dt(0)("Shipto").ToString())
                    str = Regex.Replace(str, "\[ShipContactName\]", dt(0)("ShipContactName").ToString())
                    str = Regex.Replace(str, "\[ShipContactPhone\]", dt(0)("ShipContactPhone").ToString())
                    str = Regex.Replace(str, "\[ShipContactEmail\]", dt(0)("ShipContactEmail").ToString())
                    str = Regex.Replace(str, "\[ShipAddress1\]", dt(0)("ShipAddress1").ToString())
                    str = Regex.Replace(str, "\[ShipAddress2\]", dt(0)("ShipAddress2").ToString())
                    str = Regex.Replace(str, "\[ShipCity\]", dt(0)("ShipCity").ToString())
                    str = Regex.Replace(str, "\[ShipState\]", dt(0)("ShipState").ToString())
                    str = Regex.Replace(str, "\[ShipZip\]", dt(0)("ShipZip").ToString())
                    str = Regex.Replace(str, "\[ShipCountry\]", dt(0)("ShipCountry").ToString())
                    str = Regex.Replace(str, "\[PickupFrom\]", dt(0)("PickupFrom").ToString())
                    str = Regex.Replace(str, "\[PickupContactName\]", dt(0)("PickupContactName").ToString())
                    str = Regex.Replace(str, "\[PickupContactPhone\]", dt(0)("PickupContactPhone").ToString())
                    str = Regex.Replace(str, "\[PickupContactEmail\]", dt(0)("PickupContactEmail").ToString())
                    str = Regex.Replace(str, "\[PickupAddress1\]", dt(0)("PickupAddress1").ToString())
                    str = Regex.Replace(str, "\[PickupAddress2\]", dt(0)("PickupAddress2").ToString())
                    str = Regex.Replace(str, "\[PickupCity\]", dt(0)("PickupCity").ToString())
                    str = Regex.Replace(str, "\[PickupState\]", dt(0)("PickupState").ToString())
                    str = Regex.Replace(str, "\[PickupZip\]", dt(0)("PickupZip").ToString())
                    str = Regex.Replace(str, "\[PickupCountry\]", dt(0)("PickupCountry").ToString())

                    str = Regex.Replace(str, "\[Portalpass\]", dt(0)("ExhibitorPass").ToString().ToString())
                    str = Regex.Replace(str, "\[Portaluser\]", dt(0)("ExhibitorUser").ToString().ToString())
                    str = Regex.Replace(str, "\[Portalsite\]", dt(0)("ExhibitorWebsite").ToString().ToString())
                    str = Regex.Replace(str, "\[Labor\]", dt(0)("Labor").ToString().ToString())
                    str = Regex.Replace(str, "\[Material\]", dt(0)("Material").ToString().ToString())
                    str = Regex.Replace(str, "\[ShippingType\]", dt(0)("ShippingType").ToString())
                    str = Regex.Replace(str, "\[TrackingNumber\]", dt(0)("TrackingNumber").ToString())
                    str = Regex.Replace(str, "\[fkFulfillmentLocationId\]", dt(0)("fkFulfillmentLocationId").ToString())
                    str = Regex.Replace(str, "\[Notes\]", dt(0)("Notes").ToString())
                    Try
                        JPRegionalAdmin.Value = dt(0)("JPRegionalAdmin").ToString()
                        AURegionalAdmin.Value = dt(0)("AURegionalAdmin").ToString()
                        CNRegionalAdmin.Value = dt(0)("CNRegionalAdmin").ToString()
                    Catch ex As Exception

                    End Try


                    fulfillmentLocationHolder.Value = dt(0)("fkFulfillmentLocationId")

                    If fulfillmentLocationHolder.Value = "Japan" Or fulfillmentLocationHolder.Value = "China" Or fulfillmentLocationHolder.Value = "Australia" Or fulfillmentLocationHolder.Value = "Australia" Then
                        shippingEstimate.Visible = True
                        'trackingNumberHolder.Visible = False

                        Dim shipCountry = dt(0)("ShipCountry").ToString()
                        Dim shipCity = String.Empty

                        Dim dt8 = ReadRecords("select distinct ShippingCity from tblcarton where ShippingCountry ='" & shipCountry & "'")
                        If dt8.Rows.Count > 0 Then
                            shipCity = dt(0)("ShipCity").ToString()
                            dt6 = ReadRecords("Select Sum(Cost * 2) as TotalShippingCost from tblAPACShipmentItem join tblCarton on tblAPACShipmentItem.CartonName = tblCarton.CartonName where isarchived = 0 and tblCarton.ShippingCity = '" & shipCity & "' and fkorderid=" & Session("pkid"))
                            str6 = Regex.Replace(str6, "\[totalShippingCost\]", "$" + dt6(0)("TotalShippingCost").ToString())
                            contactInfo.Visible = False
                        Else
                            totalShippingEstimate.Visible = False

                        End If

                        dt4 = ReadRecords("Select Sum(Quantity) as TotalBoxes from tblAPACShipmentItem where isArchived = 0 and fkorderid=" & Session("pkid"))

                        dt6 = ReadRecords("Select Sum(Cost) as TotalShippingCost from tblAPACShipmentItem join tblCarton on tblAPACShipmentItem.CartonName = tblCarton.CartonName where isarchived = 0 and tblCarton.ShippingCity = '" & shipCity & "' and fkorderid=" & Session("pkid"))

                        str4 = Regex.Replace(str4, "\[totalNumberItems\]", dt3(0)("Total").ToString())
                        str5 = Regex.Replace(str5, "\[totalNumberBoxes\]", dt4(0)("TotalBoxes").ToString())


                        dt5 = ReadRecords("Select (CASE WHEN Item2 <> 0  THEN 2 ELSE 1 END) TbTQty ,* from tblAPACShipmentItem join tblCarton on tblAPACShipmentItem.CartonName = tblCarton.CartonName   where isArchived = 0  and tblCarton.ShippingCity = '" & shipCity & "' and fkorderid=" & Session("pkid"))
                        shipmentBoxes.DataSource = dt5
                    ElseIf fulfillmentLocationHolder.Value = "EUROPE" Then
                        shippingEstimate.Visible = True

                        Dim shipCity = String.Empty

                        Dim dt8 = ReadRecords("select distinct ShippingCity from tblcarton where ShippingCountry ='EUROPE'")
                        If dt8.Rows.Count > 0 Then
                            shipCity = dt(0)("ShipCity").ToString()
                            dt6 = ReadRecords("Select Sum(Cost * 2) as TotalShippingCost from tblEMEAShipmentItem join tblCarton on tblEMEAShipmentItem.CartonName = tblCarton.CartonName where isarchived = 0 and tblCarton.ShippingCity = '" & shipCity & "' and fkorderid=" & Session("pkid"))
                            str6 = Regex.Replace(str6, "\[totalShippingCost\]", "$" + dt6(0)("TotalShippingCost").ToString())
                            contactInfo.Visible = False
                        Else
                            totalShippingEstimate.Visible = False

                            dt4 = ReadRecords("Select Sum(Quantity) as TotalBoxes from tblEMEAShipmentItem where isArchived = 0 and fkorderid=" & Session("pkid"))


                            dt6 = ReadRecords("Select Sum(Cost) as TotalShippingCost from tblEMEAShipmentItem join tblCarton on tblEMEAShipmentItem.CartonName = tblCarton.CartonName where isarchived = 0 and tblCarton.ShippingCity = '" & shipCity & "' and fkorderid=" & Session("pkid"))

                            str4 = Regex.Replace(str4, "\[totalNumberItems\]", dt3(0)("Total").ToString())
                            str5 = Regex.Replace(str5, "\[totalNumberBoxes\]", dt4(0)("TotalBoxes").ToString())


                            dt5 = ReadRecords("Select (CASE WHEN Item2 <> 0  THEN 2 ELSE 1 END) TbTQty ,* from tblEMEAShipmentItem join tblCarton on tblEMEAShipmentItem.CartonName = tblCarton.CartonName   where isArchived = 0  and tblCarton.ShippingCity = '" & shipCity & "' and fkorderid=" & Session("pkid"))
                            shipmentBoxes.DataSource = dt5

                        End If


                    Else

                        shippingEstimate.Visible = False
                    End If

                    Select Case puser.userID
                        Case 1093
                            If dt(0)("JPRegionalAdmin").ToString() <> "" Then
                                rbApproveReject.Enabled = False
                            End If
                        Case 1094
                            If dt(0)("AURegionalAdmin").ToString() <> "" Then
                                rbApproveReject.Enabled = False
                            End If
                        Case 1095
                            If dt(0)("CNRegionalAdmin").ToString() <> "" Then
                                rbApproveReject.Enabled = False
                            End If
                    End Select

                    Dim uploaded = ""
                    Dim targetFolder As String = Server.MapPath("Documents/" & Session("pkid"))
                    Dim di As New IO.DirectoryInfo(targetFolder)
                    Try
                        Dim aryFi As IO.FileInfo() = di.GetFiles("*.*")
                        Dim fi As IO.FileInfo
                        For Each fi In aryFi
                            uploaded = uploaded & "<a href='" & HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/") & "Documents/" & Session("pkid") & "/" & fi.Name & "' download>" & fi.Name & "</a><br>"

                        Next
                    Catch ex As Exception

                    End Try
                    str = Regex.Replace(str, "\[Attachments\]", uploaded)

                    order.InnerHtml = str
                    adminName.InnerHtml = str2
                    adminEmail.InnerHtml = str3
                    totalNumberItems.InnerHtml = str4
                    totalNumberBoxes.InnerHtml = str5
                    totalShippingCost.InnerHtml = str6



                End If

                RadGrid1.DataSource = dt2
                RadGrid1.DataBind()
                reasonsforRejection.Visible = False
                rejectionsNote.Visible = False
                txtRejectionReason.Visible = False
            Else
                'Response.Redirect("default.aspx")
            End If

        End If

    End Sub

    Protected Sub RadButton2_Click(sender As Object, e As EventArgs) Handles RadButton2.Click
        

        Dim pkid = Session("pkid")
        Session("pkid") = Nothing

        If (Request.QueryString.AllKeys.Contains("editOrder")) Or (Request.QueryString.AllKeys.Contains("existingOrder")) Then
            SaveOrderHeaders(pkid)
            DeleteOrderItems(pkid)
            GetOrderItems(pkid)
            TruncateTempTbl(pkid)
        End If


        If fulfillmentLocationHolder.Value.ToLower() = "united states" Or fulfillmentLocationHolder.Value.ToLower() = "europe" Then
            UpdateOrderStatus(pkid, 1002)
            EmailOrder(pkid, puser.email)
        Else
            If puser.role = 1006 Then
                UpdateOrderStatus(pkid, 1002)
                EmailOrder(pkid, puser.email)
            Else
                UpdateOrderStatus(pkid, 1001)
                SendNotification(1, puser.email, pkid)
                SendNotification(4, "lyle_andrews@apple.com", pkid)
            End If
        End If

        Response.Redirect("order.aspx?OrderID=" & pkid)
        'End If
    End Sub

    Function SaveOrderHeaders(pkid)
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "Update tblOrders set tblOrders.AURegionalAdmin =  NULL, tblOrders.CNRegionalAdmin = NULL, tblOrders.JPRegionalAdmin = NULL,  tblOrders.ContactName = tmpOrders.ContactName, tblOrders.ContactEmail = tmpOrders.ContactEmail, tblOrders.ContactPhone = tmpOrders.ContactPhone, tblOrders.EventName = tmpOrders.EventName, tblOrders.DateStart = tmpOrders.DateStart, tblOrders.DateEnd = tmpOrders.DateEnd, tblOrders.DateArrive = tmpOrders.DateArrive, tblOrders.DatePickup = tmpOrders.DatePickup," &
  "tblOrders.Notes = tmpOrders.Notes , tblOrders.DateOrder = tmpOrders.DateOrder, tblOrders.fkStatusId = tmpOrders.fkStatusId , tblOrders.fkUserID = tmpOrders.fkUserID, tblOrders.ShipTo = tmpOrders.ShipTo , tblOrders.ShipContactName = tmpOrders.ShipContactName, tblOrders.ShipContactPhone = tmpOrders.ShipContactPhone, tblOrders.ShipContactEmail = tmpOrders.ShipContactEmail," &
  "tblOrders.ShipAddress1 = tmpOrders.ShipAddress1, tblOrders.ShipAddress2 = tmpOrders.ShipAddress2, tblOrders.ShipCity = tmpOrders.ShipCity, tblOrders.ShipState = tmpOrders.ShipState, tblOrders.ShipZip = tmpOrders.ShipZip, tblOrders.ShipCountry = tmpOrders.ShipCountry, tblOrders.TierCosts = tmpOrders.TierCosts, tblOrders.TrackingNumber = tmpOrders.TrackingNumber, tblOrders.fkDepartmentID = tmpOrders.fkDepartmentID," &
  "tblOrders.ShippingType = tmpOrders.ShippingType, tblOrders.DateModified = tmpOrders.DateModified, tblOrders.fkFulfillmentLocationId = tmpOrders.fkFulfillmentLocationId FROM tblOrders  INNER JOIN tmpOrders ON tblOrders.pkorderid = tmporders.pkOrderId  where tblOrders.pkOrderID = @pkorderid and tmporders.sessionid = (SELECT MAX(sessionid) FROM tmpOrders)"
            sqlComm.Parameters.Add(New SqlParameter("pkorderid", pkid))

            Try
                sqlComm.ExecuteScalar()
            Catch ex As Exception
                MsgBox(ex.Message.ToString())
            End Try
        End Using
    End Function

    Sub DeleteOrderItems(pkid)
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "update tmpOrderItems set isarchived = 1 where fkOrderID = @pkid;delete from tblOrderItems where fkOrderID = @pkid"
            sqlComm.Parameters.Add(New SqlParameter("pkid", pkid))

            Try
                sqlComm.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        End Using
    End Sub

    Function GetOrderItems(pkid) As Boolean
        Dim totalCost As Double
        Dim isEmpty As Boolean = False
        Dim addToCart As Integer = 0
        For Each c As Control In Master.FindControl("MainContent").Controls
            If c.ID = "RadAjaxPanel6" Then
                For Each d As Control In c.Controls
                    If TypeOf d Is Telerik.Web.UI.RadGrid Then
                        Dim grid As RadGrid = CType(d, RadGrid)
                        If grid.MasterTableView.Items.Count > 0 Then
                            For Each dataItem As GridDataItem In grid.MasterTableView.Items
                                AddItem(pkid, dataItem.GetDataKeyValue("Quantity"), dataItem.GetDataKeyValue("pkInventoryID"))
                            Next
                        End If
                    End If
                Next
            End If
        Next
    End Function

    Sub TruncateTempTbl(pkid)
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "update tmpOrders set isarchived = 1 where pkOrderId = @pkid;update tmpOrderItems set isarchived = 1 where fkOrderID = @pkid"
            sqlComm.Parameters.Add(New SqlParameter("pkid", pkid))

            Try
                sqlComm.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        End Using
    End Sub

    Sub AddItem(pkid, quantity, fkinventoryID)
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr

            sqlComm.CommandText = "Insert into tblOrderItems ([fkInventoryID],[Quantity],[fkOrderID]) values (@fkInventoryID,@Quantity,@fkOrderID); SELECT SCOPE_IDENTITY()"

            sqlComm.Parameters.Add(New SqlParameter("fkInventoryID", fkinventoryID))
            sqlComm.Parameters.Add(New SqlParameter("Quantity", quantity))
            sqlComm.Parameters.Add(New SqlParameter("fkOrderID", pkid))

            Try
                pkid = sqlComm.ExecuteScalar()
            Catch ex As Exception
                'ErrorEmail(ex.Message)
            End Try
        End Using
    End Sub


    Protected Sub RadButton5_Click(sender As Object, e As EventArgs) Handles RadButton5.Click
        Dim OrderID = Session("pkid")
        Dim rejectionReason = String.Empty
        If rbApproveReject.SelectedValue = "Reject" Then
            UpdateOrderStatus(OrderID, 1003)


            If rbl.SelectedValue = "Other" Then
                If rejectionsNote.Text <> "" Then
                    InsertRejectionNotes(OrderID, rejectionsNote.Text)
                    SendNotification(3, contactEmail.Value, OrderID, rejectionsNote.Text)
                    AddOrderChangeLog(OrderID, 2, "Rejected Order by: " + puser.email + ". Reason: (" + rejectionsNote.Text + "),")
                Else
                    txtRejectionReason.Visible = True
                    Exit Sub
                End If
            Else
                SendNotification(3, contactEmail.Value, OrderID, rbl.SelectedValue)
                InsertRejectionNotes(OrderID, rbl.SelectedValue)
                AddOrderChangeLog(OrderID, 2, "Order rejected by: " + puser.email + ". Reason: (" + rbl.SelectedValue + ")")
            End If


        ElseIf rbApproveReject.SelectedValue = "Approve" Then
            'If puser.userID = 1025 Then
            UpdateOrderStatus(OrderID, 1002)
            ComparefulfillmentLocation(OrderID)
                AddOrderChangeLog(OrderID, 1, "Order Approved by: " + puser.email)
                SendNotification(2, contactEmail.Value, OrderID)
                If dateModified.Value <> "" Then

                Select Case fulfillmentLocationHolder.Value
                    Case "Japan"
                        SendNotification(6, "yasuda@kt-planning.co.jp", OrderID)
                    Case "Australia"
                        SendNotification(6, "cheryl@exposuregroup.com.au", OrderID)
                    Case "China"
                        SendNotification(6, "g.kopelchak@idea-intl.com", OrderID)
                End Select
                SendNotification(6, contactEmail.Value, OrderID)
            Else
                EmailOrder(OrderID, contactEmail.Value)
            End If

        End If

        Response.Redirect("order.aspx?OrderID=" & OrderID)
    End Sub

    Sub InsertApprovalStatus(orderId, status)
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr

            Select Case puser.userID
                Case 1093
                    sqlComm.CommandText = "Update tblOrders set JPRegionalAdmin=@status where pkorderid=@pkorderid"
                    If AURegionalAdmin.Value <> "" And CNRegionalAdmin.Value <> "" Then
                        UpdateOrderStatus(orderId, 1002)
                        SendNotification(2, contactEmail.Value, orderId)
                        If dateModified.Value <> "" Then
                            SendNotification(6, "Jasonn@pinnacle-exp.com", orderId)
                        Else
                            EmailOrder(orderId, "Jasonn@pinnacle-exp.com")
                        End If
                        ComparefulfillmentLocation(orderId)
                        AddOrderChangeLog(orderId, 1, "Order Approved by: " + puser.email)
                    End If
                Case 1094
                    sqlComm.CommandText = "Update tblOrders set AURegionalAdmin=@status where pkorderid=@pkorderid"
                    If JPRegionalAdmin.Value <> "" And CNRegionalAdmin.Value <> "" Then
                        UpdateOrderStatus(orderId, 1002)
                        SendNotification(2, contactEmail.Value, orderId)
                        If dateModified.Value <> "" Then
                            SendNotification(6, "Jasonn@pinnacle-exp.com", orderId)
                        Else
                            EmailOrder(orderId, "Jasonn@pinnacle-exp.com")
                        End If
                        ComparefulfillmentLocation(orderId)
                        AddOrderChangeLog(orderId, 1, "Order Approved by: " + puser.email)

                    End If
                Case 1095
                    sqlComm.CommandText = "Update tblOrders set CNRegionalAdmin=@status where pkorderid=@pkorderid"
                    If JPRegionalAdmin.Value <> "" And AURegionalAdmin.Value <> "" Then
                        UpdateOrderStatus(orderId, 1002)
                        SendNotification(2, contactEmail.Value, orderId)
                        If dateModified.Value <> "" Then
                            SendNotification(6, "Jasonn@pinnacle-exp.com", orderId)
                        Else
                            EmailOrder(orderId, "Jasonn@pinnacle-exp.com")
                        End If
                        ComparefulfillmentLocation(orderId)

                        AddOrderChangeLog(orderId, 1, "Order Approved by: " + puser.email)

                    End If
            End Select

            sqlComm.Parameters.Add(New SqlParameter("pkorderid", orderId))
            sqlComm.Parameters.Add(New SqlParameter("status", status))
            Try
                sqlComm.ExecuteScalar()
            Catch ex As Exception

            End Try
        End Using
    End Sub

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


            Try
                Dim wrapper As New Simple3Des(ConfigurationSettings.AppSettings("3dskey"))

                Dim SmtpServer As New SmtpClient("smtp-legacy.office365.com")
                SmtpServer.Port = 587
                SmtpServer.UseDefaultCredentials = False
                SmtpServer.Credentials = New System.Net.NetworkCredential(ConfigurationManager.AppSettings("o365user"), "Boc00263")
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network
                SmtpServer.EnableSsl = True

                Dim mail As New MailMessage()
                mail.To.Add(toEmail)
                Select Case fulfillmentLocationHolder.Value
                    Case "Japan"
                        mail.To.Add("yasuda@kt-planning.co.jp")
                        mail.To.Add("ueno@kt-planning.co.jp")
                        mail.To.Add("raymond.ho@kt-planning.co.jp")
                        mail.To.Add("ivy.chee@kt-planning.co.jp")
                        mail.To.Add("enist@pinnacle-exp.com")
                    Case "Australia"
                        mail.To.Add("cheryl@exposuregroup.com.au")
                        mail.To.Add("ben@exposuregroup.com.au")
                        mail.To.Add("enist@pinnacle-exp.com")
                    Case "China"
                        mail.To.Add("g.kopelchak@idea-intl.com")

                End Select
                mail.To.Add(ConfigurationSettings.AppSettings("SupportEmail"))

                Dim sql = "SELECT distinct o.* , oi.fkOrderID from tblOrders o join tblOrderItems oi on oi.fkOrderID = o.pkOrderID join tblInventory i on i.pkInventoryID = oi.fkInventoryID where o.pkorderid = '" & pkid & "' and i.fkDepartmentID = 1004 order by o.DateArrive"
                Dim dt = ReadRecords(sql)
                If dt.Rows.Count > 0 Then
                    mail.To.Add(ConfigurationSettings.AppSettings("ArticaWarehouseEmail"))
                ElseIf puser.department = 1004 Then
                    mail.To.Add(ConfigurationSettings.AppSettings("ArticaWarehouseEmail"))
                End If


                mail.Subject = ConfigurationSettings.AppSettings("ClientName") & " Portable Order# " & pkid & " Confirmation"
                mail.Body = "<style>" & css.InnerHtml & "</style><table style='width:90%;background-color:#ededed;border: 1px solid #dfdfdf;padding:30px;margin-left:auto;margin-right:auto;margin-top:30px;' ><tr><td></td></tr><tr><td><h1>Order# " & pkid & "</h1>" & order.InnerHtml & sb.ToString() & "</td></tr></table>"
                mail.From = New MailAddress("PinnacleNoReply@pinnacle-exp.com")
                mail.IsBodyHtml = True

                SmtpServer.Send(mail)

            Catch ex As Exception

            End Try


        Catch ex As Exception

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


    Function SaveOrder(pkid)
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "Update tblOrders set CCFirstName=@CCFirstName, CCLastName=@CCLastName, CCAddress1=@CCAddress1, CCAddress2=@CCAddress2, CCCity=@CCCity, CCState=@CCState, CCZip=@CCZip, CCExp=@CCExp, CCCode=@CCCode, CCNumber=@CCNumber where pkorderid=@pkorderid"

            sqlComm.Parameters.Add(New SqlParameter("pkorderid", pkid))
            sqlComm.Parameters.Add(New SqlParameter("CCFirstName", tbccFname.Text))
            sqlComm.Parameters.Add(New SqlParameter("CCLastName", tbccLname.Text))
            sqlComm.Parameters.Add(New SqlParameter("CCAddress1", tbccAddress1.Text))
            sqlComm.Parameters.Add(New SqlParameter("CCAddress2", tbccAddress2.Text))
            sqlComm.Parameters.Add(New SqlParameter("CCCity", tbccCity.Text))
            sqlComm.Parameters.Add(New SqlParameter("CCState", ddlccstate.SelectedValue))
            sqlComm.Parameters.Add(New SqlParameter("CCZip", tbccZip.Text))
            sqlComm.Parameters.Add(New SqlParameter("CCExp", ddlMonths.SelectedValue & "/" & ddlYears.SelectedValue))
            sqlComm.Parameters.Add(New SqlParameter("CCCode", tbCCCode.Text))
            sqlComm.Parameters.Add(New SqlParameter("CCNumber", tbCC.Text))

            Try
                pkid = sqlComm.ExecuteScalar()
            Catch ex As Exception

            End Try
        End Using
        Return pkid
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

        End Try
        Return dt

    End Function

    Protected Sub OrdersNote_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles OrdersNote.NeedDataSource

        Dim orderid = Session("pkid")
        Dim sql = "SELECT tblOrderNotes.* ,tblOrderNotes.Notes as logsNote, Format(Cast([tblOrderNotes].DateCreated as date), 'MM/dd/yyyy') as dateTime , tblusers.Firstname+' '+tblusers.lastname as [user] from tblOrderNotes join tblUsers on tblOrderNotes.fkUserId = tblUsers.pkUserID WHERE fkOrderId=" & orderid

        OrdersNote.DataSource = ReadRecords(sql)
    End Sub

    Protected Sub RadButton3_Click(sender As Object, e As EventArgs) Handles RadButton3.Click

        Dim pkid = Session("pkid")

        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "update tmpOrders set isarchived = 1 where pkOrderId = @pkid;Delete from tmpOrderItems where fkOrderID = @pkid"
            sqlComm.Parameters.Add(New SqlParameter("pkid", pkid))

            Try
                sqlComm.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        End Using


        Response.Redirect("Reports.aspx", False)

    End Sub

    Sub SendNotification(templateId As Integer, toEmail As String, pkid As Integer, Optional ByVal rejectionReason As String = "")

        Dim approver = puser.firstname + " " + puser.lastname

        Dim html = String.Empty
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
            Dim wrapper As New Simple3Des(ConfigurationSettings.AppSettings("3dskey"))

            Dim SmtpServer As New SmtpClient("smtp-legacy.office365.com")
            SmtpServer.Port = 587
            SmtpServer.UseDefaultCredentials = False
            SmtpServer.Credentials = New System.Net.NetworkCredential(ConfigurationManager.AppSettings("o365user"), "Boc00263")
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network
            SmtpServer.EnableSsl = True

            Dim mail As New MailMessage()
            Select Case templateId
                Case 1
                    'Portable order Pending Approval
                    mail.To.Add(toEmail)
                    mail.Subject = ConfigurationSettings.AppSettings("ClientName") & " Portable Order# " & pkid & " - Order Pending Approval"
                    mail.Body = "<style>" & css.InnerHtml & "</style><table style='width:90%;background-color:#ededed;border: 1px solid #dfdfdf;padding:10px;margin-left:auto;margin-right:auto;margin-top:30px;' ><tr><td></td></tr><tr><td><h1>Order# " & pkid & " - Order Pending Approval </h1></td></tr><tr><td>Thank you for placing your portables order. This is now routing for local Apple approval. Once approved, the order will then be fulfilled as requested. If you have any questions, please reach out to <a href='mailto:recruiting_events@apple.com'>recruiting_events@apple.com</a> </td></tr><tr><td>" & order.InnerHtml & sb.ToString() & "</td></tr></table>"

                    'Portable order Approved
                Case 2
                    mail.To.Add(toEmail)

                    mail.Subject = ConfigurationSettings.AppSettings("ClientName") & " Portable Order# " & pkid & " - Order Approved"
                    mail.Body = "<style>" & css.InnerHtml & "</style><table style='width:90%;background-color:#ededed;border: 1px solid #dfdfdf;padding:10px;margin-left:auto;margin-right:auto;margin-top:30px;' ><tr><td></td></tr><tr><td><h1>Order# " & pkid & " - Order Approved by: " & approver & " </h1></td></tr><tr><td>Order Approved by: " & approver & "</td></tr><tr><td>Your portables order has now been approved and is being fulfilled by our local partners. If you have any questions, please reach out to <a href='mailto:recruiting_events@apple.com'>recruiting_events@apple.com</a></td></tr><tr><td>" & order.InnerHtml & sb.ToString() & "</td></tr></table>"

                    'Portable order Rejected
                Case 3
                    mail.To.Add(toEmail)
                    'mail.To.Add(ConfigurationSettings.AppSettings("SupportEmail"))
                    mail.Subject = ConfigurationSettings.AppSettings("ClientName") & " Portable Order# " & pkid & " - Order Rejected"
                    mail.Body = "<style>" & css.InnerHtml & "</style><table style='width:90%;background-color:#ededed;border: 1px solid #dfdfdf;padding:10px;margin-left:auto;margin-right:auto;margin-top:30px;' ><tr><td></td></tr><tr><td><h1>Order# " & pkid & " - Order Rejected by: " & approver & " </h1></td></tr><tr><td>Your portables order has not been approved. Please reach out directly to the Apple approver that declined your request for more details <a href='" & puser.email & "'>" & puser.email & "</a> </td></tr> <tr><td>If you have any questions, please reach out to: <a href='mailto:recruiting_events@apple.com'>recruiting_events@apple.com</a></td></tr>  <tr><td>Order Rejected by: " & approver & "</td></tr><tr><td>Rejection reason: " & rejectionReason & "</td></tr><tr><td>Please Contact: <a href='" & puser.email & "'>" & puser.email & "</a> </td></tr><tr><td>" & order.InnerHtml & sb.ToString() & "</td></tr></table>"
                    'Portable order Pending Approval
                Case 4
                    mail.To.Add(toEmail)
                    'mail.To.Add(ConfigurationSettings.AppSettings("SupportEmail"))
                    mail.Subject = ConfigurationSettings.AppSettings("ClientName") & " Portable Order# " & pkid & " - Order Is Pending Your Approval"
                    'mail.Body = "<style>" & css.InnerHtml & "</style><table style='width:90%;background-color:#ededed;border: 1px solid #dfdfdf;padding:30px;margin-left:auto;margin-right:auto;margin-top:30px;' ><tr><td></td></tr><tr><td><h1>Order# " & pkid & " -  Order Is Pending Your Approval </h1>" & order.InnerHtml & sb.ToString() & "</td></tr></table>"
                    mail.Body = "<style>" & css.InnerHtml & "</style><table style='width:90%;background-color:#ededed;border: 1px solid #dfdfdf;padding:10px;margin-left:auto;margin-right:auto;margin-top:30px;' ><tr><td></td></tr><tr><td><h1>Order# " & pkid & " - Order Is Pending Your Approval </h1></td></tr><tr><td>You have received a new Portables request which requires your attention please.</td></tr><tr><td>" & order.InnerHtml & sb.ToString() & "</td></tr></table>"

                Case 5
                    mail.To.Add(toEmail)

                    Select Case toEmail
                        Case "cheryl@exposuregroup.com.au"
                            mail.To.Add("ben@exposuregroup.com.au")
                            mail.To.Add("enist@pinnacle-exp.com")
                        Case "yasuda@kt-planning.co.jp"
                            mail.To.Add("ueno@kt-planning.co.jp")
                            mail.To.Add("raymond.ho@kt-planning.co.jp")
                            mail.To.Add("ivy.chee@kt-planning.co.jp")
                            mail.To.Add("enist@pinnacle-exp.com")

                    End Select
                    'mail.To.Add(ConfigurationSettings.AppSettings("SupportEmail"))
                    mail.Subject = ConfigurationSettings.AppSettings("ClientName") & " Portable Order# " & pkid & " - Cancellation"
                    mail.Body = "<style>" & css.InnerHtml & "</style><table style='width:90%;background-color:#ededed;border: 1px solid #dfdfdf;padding:10px;margin-left:auto;margin-right:auto;margin-top:30px;' ><tr><td></td></tr><tr><td><h1>Order# " & pkid & " - Cancellation </h1></td></tr><tr><td>Your order is now cancelled.</td></tr><tr><td>" & order.InnerHtml & sb.ToString() & "</td></tr></table>"

                Case 6
                    mail.To.Add(toEmail)

                    Select Case toEmail
                        Case "cheryl@exposuregroup.com.au"
                            mail.To.Add("ben@exposuregroup.com.au")
                            mail.To.Add("enist@pinnacle-exp.com")
                        Case "yasuda@kt-planning.co.jp"

                            mail.To.Add("ueno@kt-planning.co.jp")
                            mail.To.Add("raymond.ho@kt-planning.co.jp")
                            mail.To.Add("ivy.chee@kt-planning.co.jp")
                            mail.To.Add("enist@pinnacle-exp.com")
                    End Select

                    'mail.To.Add(ConfigurationSettings.AppSettings("SupportEmail"))
                    mail.Subject = ConfigurationSettings.AppSettings("ClientName") & " Portable Order# " & pkid & " - Order Is Updated"
                            mail.Body = "<style>" & css.InnerHtml & "</style><table style='width:90%;background-color:#ededed;border: 1px solid #dfdfdf;padding:30px;margin-left:auto;margin-right:auto;margin-top:30px;' ><tr><td></td></tr><tr><td><h1>Order# " & pkid & " -  Order Is Updated </h1>" & order.InnerHtml & sb.ToString() & "</td></tr></table>"
                    End Select

                    mail.From = New MailAddress("PinnacleNoReply@pinnacle-exp.com")
            mail.IsBodyHtml = True

            Try
                SmtpServer.Send(mail)

            Catch ex As System.Net.Mail.SmtpException
            End Try

        Catch ex As Exception
            'ErrorEmail("Send Error: " & ex.Message)
        End Try

    End Sub


    Protected Sub rbl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbl.SelectedIndexChanged
        If rbl.SelectedValue = "Other" Then
            rejectionsNote.Visible = True
        Else
            rejectionsNote.Visible = False
        End If
    End Sub

    Protected Sub rbApproveReject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbApproveReject.SelectedIndexChanged
        If rbApproveReject.SelectedValue = "Reject" Then
            reasonsforRejection.Visible = True
            If rbl.SelectedValue = "Other" Then
                rejectionsNote.Visible = True
            End If
        Else
            reasonsforRejection.Visible = False
            rejectionsNote.Visible = False
        End If
    End Sub

    Sub InsertRejectionNotes(pkid, notes)
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "Update tblOrders set RejectionReason=@RejectionReason where pkorderid=@pkorderid"

            sqlComm.Parameters.Add(New SqlParameter("pkorderid", pkid))
            sqlComm.Parameters.Add(New SqlParameter("RejectionReason", notes))
            Try
                pkid = sqlComm.ExecuteScalar()
            Catch ex As Exception

            End Try
        End Using
    End Sub

    Protected Sub RadButton1_Click(sender As Object, e As EventArgs) Handles RadButton1.Click
        Dim OrderID = Session("pkid")
        Response.Redirect("Place.aspx?OrderID=" & OrderID)
    End Sub


    Function AddOrderChangeLog(pkid As Integer, templateId As Integer, Notes As String) As String


        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "Insert into tblOrderNotes([fkOrderId], [fkUserId], [DateCreated], [Notes], [FulfillmentLocation]) values (@fkOrderId,@fkUserId,@DateCreated,@Notes, @FulfillmentLocation); SELECT SCOPE_IDENTITY()"

            sqlComm.Parameters.Add(New SqlParameter("fkOrderId", pkid))
            sqlComm.Parameters.Add(New SqlParameter("fkUserId", puser.userID))
            sqlComm.Parameters.Add(New SqlParameter("DateCreated", Now()))
            sqlComm.Parameters.Add(New SqlParameter("Notes", Notes))
            sqlComm.Parameters.Add(New SqlParameter("FulfillmentLocation", fulfillmentLocationHolder.Value))


            Try
                Dim noteId = sqlComm.ExecuteScalar()
            Catch ex As Exception
                Dim errorM = ex.Message
            End Try
        End Using
    End Function


    Sub ComparefulfillmentLocation(orderid)
        Dim dt = ReadRecords("SELECT FulfillmentLocation, * from tblOrderNotes  WHERE FulfillmentLocation IS NOT NULL and  DateCreated IN (SELECT max(DateCreated) FROM tblOrderNotes) and fkOrderId=" & orderid)
        If dt.Rows.Count > 0 Then
            Dim latestApprovedFulfillmentLocation = dt(0)("FulfillmentLocation").ToString()
            If latestApprovedFulfillmentLocation <> fulfillmentLocationHolder.Value Then
                Select Case latestApprovedFulfillmentLocation
                    Case "Japan"
                        SendNotification(5, "yasuda@kt-planning.co.jp", orderid)
                    Case "Australia"
                        SendNotification(5, "cheryl@exposuregroup.com.au", orderid)
                    Case "China"
                        SendNotification(5, "g.kopelchak@idea-intl.com", orderid)
                End Select
            End If
        End If
    End Sub


End Class

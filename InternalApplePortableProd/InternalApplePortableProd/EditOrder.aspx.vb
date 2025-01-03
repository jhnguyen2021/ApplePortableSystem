Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.IO
Imports Telerik.Web.UI

Partial Class EditOrder
    Inherits Telerik.Web.UI.RadAjaxPage
    Dim orderID As String = ""
    Dim puser As PortalUser
    Public services As String
    Public portal As String
    Public search As String
    Public departmentId As Integer
    Public Property DetailsView_AddClass As Object

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

        departmentId = puser.department


        If puser.role = 1001 Then
            RadButton2.CausesValidation = False
            RadButton3.Visible = True
        Else
            'tbStartDate.MinDate = Now.AddDays(10)
            'tbPickupDate.MinDate = Now.AddDays(10)

            'tbStartDate.MinDate = Now.AddDays(10)
            'tbEndDate.MinDate = Now.AddDays(10)
        End If




        If Not IsPostBack Then
            nav1.Visible = False
            search = ""
            fkUserID.Value = puser.userID
            If puser.department = 1002 Then
                trBoothType.Visible = True
            Else
                cbBoothType.SelectedValue = "NA"
                trBoothType.Visible = False


                tbBoothSize.FindItemByText("10 x 20").Remove()
                tbBoothSize.FindItemByText("20 x 20").Remove()
            End If


            tbBoothSize.Enabled = False

            Session("filter") = ""
            Session("cart") = Nothing
            Session("Reset") = True
            'ClientScript.RegisterStartupScript(Me.GetType(), "SessionAlert", "SessionExpireAlert(" & timeout & ");", True)
            'Page.DataBind()



            cbPState.DataSource = ReadRecords("select * from tblstate order by state asc")
            cbPState.DataTextField = "State"
            cbPState.DataValueField = "Code"
            cbPState.DataBind()
            Dim myItem2 As New RadComboBoxItem
            myItem2.Value = "Select..."
            myItem2.Text = "Select..."
            cbPState.Items.Insert(0, myItem2)

            If Request.QueryString("orderID") <> "" Then
                tbBoothSize.Enabled = True
                nav1.Visible = True
                orderID = Replace(Request.QueryString("orderID"), "'", "''")
                Dim dt = ReadRecords("Select * from tblOrderS where pkorderID=" & orderID)
                If dt.Rows.Count > 0 Then

                    fkstatusID.Value = dt(0)("fkStatusID").ToString()

                    tbContactName.Text = dt(0)("ContactName").ToString()
                    tbContactEmail.Text = dt(0)("ContactEmail").ToString()
                    tbContactPhone.Text = dt(0)("ContactPhone").ToString()
                    tbEventName.Text = dt(0)("EventName").ToString()
                    'tbStartDate.SelectedDate = dt(0)("DateStart").ToString()
                    'tbEndDate.SelectedDate = dt(0)("DateEnd").ToString()
                    If dt(0)("DateArrive").ToString() <> "" Then
                        tbArrivalDate.SelectedDate = dt(0)("DateArrive").ToString()
                        tbPickupDate.SelectedDate = dt(0)("DatePickup").ToString()
                        tbStartDate.SelectedDate = dt(0)("DateStart").ToString()
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
                    tbTracking.Text = dt(0)("TrackingNumber").ToString()



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


    Protected Sub RadGrid1_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles Grid1.NeedDataSource

        Dim sql = String.Empty

        Dim company = puser.company
        Dim department = puser.department
        'this variable holds SQL logic to remove exhibit part numbers, the leading space in the string is NECESSARY
        'Dim exhibitremoval = " AND NOT PARTNUM BETWEEN 2000 and 2999 AND NOT PARTNUM >= 9000 ORDER BY type"
        'Adminisrator role of 1003 gets to see everything regardless of companyID or departmentID, logic has been added to limit part number return to EXCLUDE exhibit part numbers
        If puser.role = 1003 Then
            sql = "SELECT * from vwInventoryView WHERE isDeleted = 0 and Available > 0"
        End If

        'Super user role of 1001 gets to see everything within their company, based on the item's departmentID referencing companyID against the user login companyID, logic has been added to limit part number return to EXCLUDE exhibit part numbers
        If puser.role = 1001 Then
            sql = "SELECT * from vwInventoryView LEFT JOIN tblDepartment on vwInventoryView.fkDepartmentID = tblDepartment.pkDepartmentID WHERE isDeleted = 0 and Available > 0 AND tblDepartment.fkCompanyID =" & puser.company
        End If

        'A normal user or warehouse user will only be able to see inventory of the company AND department in which they are enrolled, logic has been added to limit part number return to exclude EXHIBIT part numbers
        If puser.role = 1000 Or puser.role = 1004 Then
            sql = "SELECT * FROM vwInventoryView WHERE isDeleted = 0 and Available > 0 AND fkDepartmentID = " & department
        End If

        Grid1.DataSource = ReadRecords(sql & " order by Partnum")

    End Sub


    Protected Sub CountryPickup(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        If cbPCountry.SelectedItem IsNot Nothing Then
            If cbPCountry.SelectedItem.Text = "UNITED STATES" Then

                cbPState.Visible = True
                tbPRegion.Visible = False
            ElseIf cbPCountry.SelectedItem.Text = "CANADA" Then

                cbPState.Visible = True
                tbPRegion.Visible = False

            Else

                cbPState.Visible = False
                tbPRegion.Visible = True
            End If

        End If

    End Sub

    Protected Sub RadGrid1_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles Grid1.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            GetQuantity(e.Item)
        End If
    End Sub

    Protected Sub orderGrid_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles orderGrid.NeedDataSource
        orderGrid.DataSource = Session("cart")
    End Sub


    Protected Sub orderGrid_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles orderGrid.ItemCommand
        If e.CommandName = "DeleteOrder" Then
            Dim item As Telerik.Web.UI.GridDataItem
            item = e.Item

            Dim pkID = item.GetDataKeyValue("pkInventoryID")
            Dim dt As DataTable = TryCast(Session("cart"), DataTable)
            For Each r As DataRow In dt.Rows
                If r("pkInventoryID") = pkID Then
                    r.Delete()
                End If
            Next
            dt.AcceptChanges()
            Session("cart") = dt
            orderGrid.DataSource = Nothing
            orderGrid.Rebind()

            For Each item In Grid1.MasterTableView.Items
                If (item.GetDataKeyValue("pkInventoryID").ToString = pkID) Then
                    Dim ddl As DropDownList = item.FindControl("myDD")
                    ddl.SelectedValue = 0
                End If
            Next
            'Grid1.Rebind()
        End If
    End Sub

    Protected Sub orderGrid_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs)
        If TypeOf e.Item Is GridDataItem Then
            GetCartQuantity(e.Item)
        End If
    End Sub



    Function GetCartQuantity(ByVal item As GridDataItem)

        Try
            Dim count = 0
            Dim quantity = 0
            Dim pkid = item.GetDataKeyValue("pkInventoryID")
            Dim qty = item.GetDataKeyValue("Quantity")
            Dim sdate As Date
            Dim edate As Date

            If tbArrivalDate.SelectedDate IsNot Nothing And tbPickupDate.SelectedDate IsNot Nothing Then
                tbBoothSize.Enabled = True
                nav1.Visible = True
                sdate = tbArrivalDate.SelectedDate
                sdate = sdate.AddDays(-ConfigurationSettings.AppSettings("bufferDays"))
                edate = tbPickupDate.SelectedDate
                edate = edate.AddDays(ConfigurationSettings.AppSettings("bufferDays"))
                'Dim sql = "SELECT pkOrderID, Quantity, vwInventoryView.Available FROM tblOrders INNER JOIN tblOrderItems ON tblOrders.pkOrderID = tblOrderItems.fkOrderID INNER JOIN tblInventory ON tblOrderItems.fkInventoryID = tblInventory.pkInventoryID join  vwInventoryView on vwInventoryView.pkInventoryID = tblInventory.pkInventoryID WHERE (tblInventory.isDeleted=0  AND (tblOrderItems.fkInventoryID = '" & pkid & "'))"

                Dim sql = "SELECT ISNULL(Sum(tblOrderItems.Quantity),0) as sumQuantity ,  (SELECT SUM(Quantity) AS Expr1 FROM dbo.tblItems WHERE  (tblItems.fkInventoryID = '" & pkid & "') AND (fkItemStatusID IN (1000))) AS Available  FROM tblOrders INNER JOIN tblOrderItems ON tblOrders.pkOrderID = tblOrderItems.fkOrderID INNER JOIN tblInventory ON tblOrderItems.fkInventoryID = tblInventory.pkInventoryID  WHERE (tblInventory.isDeleted=0 AND (tblOrderItems.fkInventoryID = '" & pkid & "')) AND ((tblOrders.DateArrive BETWEEN N'" & sdate & "' AND N'" & edate & "') OR (tblOrders.DatePickup BETWEEN N'" & sdate & "' AND N'" & edate & "'))"

                Dim dt = ReadRecords(sql)
                If dt.Rows.Count > 0 Then
                    For Each r In dt.Rows
                        count = count + CInt(r("Available").ToString())
                    Next
                End If
            End If


            Dim myDD As DropDownList

            myDD = DirectCast(item.FindControl("myQty"), DropDownList)

            Try
                While quantity <= count
                    myDD.Items.Add(quantity)
                    quantity = quantity + 1
                End While
            Catch ex As Exception
            End Try

            myDD.SelectedIndex = qty

        Catch ex As Exception
            ErrorEmail(ex.Message)
        End Try

    End Function

    Function GetQuantity(ByVal item As GridDataItem)
        Try
            Dim count = 0
            Dim quantity = 0
            Dim pkid = item.GetDataKeyValue("pkInventoryID")
            Dim sdate As Date
            Dim edate As Date



            ''Get the quantity of booths available by summing orders during that time and subtracking them from the total quantity.
            If tbArrivalDate.SelectedDate IsNot Nothing And tbPickupDate.SelectedDate IsNot Nothing Then
                tbBoothSize.Enabled = True
                nav1.Visible = True
                sdate = tbArrivalDate.SelectedDate
                sdate = sdate.AddDays(-ConfigurationSettings.AppSettings("bufferDays"))
                edate = tbPickupDate.SelectedDate
                edate = edate.AddDays(ConfigurationSettings.AppSettings("bufferDays"))
                Dim sql = "SELECT * FROM tblOrders INNER JOIN tblOrderItems ON tblOrders.pkOrderID = tblOrderItems.fkOrderID INNER JOIN tblInventory ON tblOrderItems.fkInventoryID = tblInventory.pkInventoryID WHERE (tblInventory.isDeleted=0 and (tblOrders.fkStatusID = 1001 or tblOrders.fkStatusID = 1002) AND (tblOrderItems.fkInventoryID = '" & pkid & "')) AND ((tblOrders.DateArrive BETWEEN N'" & sdate & "' AND N'" & edate & "') OR (tblOrders.DatePickup BETWEEN N'" & sdate & "' AND N'" & edate & "'))"

                If orderID <> "" Then 'if there a draft ignore the order
                    sql = sql & " and pkOrderID<>" & orderID
                End If
                Dim dt = ReadRecords(sql)
                If dt.Rows.Count > 0 Then
                    For Each r In dt.Rows
                        count = count + CInt(r("Quantity").ToString())
                    Next
                End If
                count = CInt(item("Available").Text.ToString()) - count
            End If

            Dim myDD As DropDownList

            myDD = DirectCast(item.FindControl("myDD"), DropDownList)

            Try
                While quantity <= count
                    myDD.Items.Add(quantity)
                    quantity = quantity + 1

                End While
            Catch ex As Exception
                item.Display = False 'hide the item
            End Try


            If myDD.Items.Count <= 0 Then
                myDD.Items.Add(0)
                item.Display = False 'hide the item
            End If

            If Request.QueryString("orderid") <> "" Then
                Dim dt2 = ReadRecords("Select tblOrderItems.quantity, tblInventory.Picture, tblinventory.pkinventoryid, tblInventory.PartNum  from tblOrderItems INNER JOIN tblInventory ON tblOrderItems.fkInventoryID = tblInventory.pkInventoryID where tblOrderItems.fkinventoryID=" & pkid & " and tblOrderItems.fkOrderID=" & Replace(Request.QueryString("orderID"), "'", "''"))
                If dt2.Rows.Count > 0 Then
                    Dim q = dt2(0)("quantity").ToString
                    AddtoCart(q, pkid, dt2(0)("PartNum").ToString, dt2(0)("picture").ToString)
                End If
            End If

        Catch ex As Exception
            ErrorEmail(ex.Message)
        End Try
    End Function


    Sub AddtoCart(qty, pkInventoryID, partNum, picture)

        If IsNothing(Session("cart")) Then
            Dim dt As New DataTable
            dt.Columns.Add("Quantity")
            dt.Columns.Add("pkInventoryID")
            dt.Columns.Add("PartNum")
            dt.Columns.Add("Picture")
            dt.AcceptChanges()
            Dim dr As DataRow = dt.NewRow

            dr(0) = qty
            dr(1) = pkInventoryID
            dr(2) = partNum
            dr(3) = picture
            dt.Rows.Add(dr)
            dt.AcceptChanges()
            dt.AcceptChanges()

            Session("cart") = dt
        Else
            Dim dt As DataTable = TryCast(Session("cart"), DataTable)
            For Each r As DataRow In dt.Rows
                If r("pkInventoryID") = pkInventoryID Then
                    r.Delete()
                End If
            Next
            dt.AcceptChanges()
            Dim dr As DataRow = dt.NewRow
            dr(0) = qty
            dr(1) = pkInventoryID
            dr(2) = partNum
            dr(3) = picture
            If qty > 0 Then
                dt.Rows.Add(dr)
            End If

            dt.AcceptChanges()
            Session("cart") = dt
        End If
        orderGrid.Rebind()

    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim ddl As DropDownList = TryCast(sender, DropDownList)
        Dim item As GridDataItem = TryCast(ddl.NamingContainer, GridDataItem)
        Dim pkinventoryid = item.GetDataKeyValue("pkInventoryID")

        Dim partNum = item.GetDataKeyValue("PartNum")

        AddtoCart(ddl.SelectedValue, pkinventoryid, partNum, item.GetDataKeyValue("Picture").ToString)
    End Sub


    Protected Sub DropDownList2_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim ddl As DropDownList = TryCast(sender, DropDownList)
        Dim item As GridDataItem = TryCast(ddl.NamingContainer, GridDataItem)
        Dim pkinventoryid = item.GetDataKeyValue("pkInventoryID")
        Dim qty = item.GetDataKeyValue("Quantity")
        Dim partNum = item.GetDataKeyValue("PartNum")
        Dim d = ddl.SelectedValue

        AddtoCart(ddl.SelectedValue, pkinventoryid, partNum, item.GetDataKeyValue("Picture").ToString)

    End Sub

    Sub updateOrderItemQty(OrderId, qty, pkid)
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "Update tblOrderItems set Quantity = @Quantity where fkOrderID = @OrderId and fkInventoryID = @pkid"
            sqlComm.Parameters.Add(New SqlParameter("OrderId", OrderId))
            sqlComm.Parameters.Add(New SqlParameter("Quantity", qty))
            sqlComm.Parameters.Add(New SqlParameter("pkid", pkid))

            Try
                sqlComm.ExecuteNonQuery()
            Catch ex As Exception
                ErrorEmail(ex.Message)
            End Try
        End Using
    End Sub

    Sub GetOrderItems(pkid)
        For Each c As Control In Master.FindControl("MainContent").Controls
            If c.ID = "RadAjaxPanel6" Then
                For Each d As Control In c.Controls
                    If TypeOf d Is Telerik.Web.UI.RadGrid Then
                        Dim grid As RadGrid = CType(d, RadGrid)
                        For Each dataItem As GridDataItem In grid.MasterTableView.Items
                            'DeleteOrderItems(pkid)
                            'Dim qty = CType(dataItem.FindControl("myDD"), DropDownList).SelectedValue
                            'If qty = "" Then
                            '    qty = 0
                            'Else
                            '    qty = CInt(qty)
                            'End If

                            'If qty > 0 Then

                            '    AddItem(pkid, qty, dataItem.GetDataKeyValue("pkInventoryID"))
                            'End If
                            AddItem(pkid, dataItem.GetDataKeyValue("Quantity"), dataItem.GetDataKeyValue("pkInventoryID"))

                        Next
                    End If

                Next
            End If
        Next

    End Sub

    Public Function CheckAvailability(ByVal pkid, ByVal qty) As Boolean
        Dim count = 0

        Dim sdate As Date
        Dim edate As Date
        sdate = tbArrivalDate.SelectedDate
        sdate = sdate.AddDays(-ConfigurationSettings.AppSettings("bufferDays"))
        edate = tbPickupDate.SelectedDate
        edate = edate.AddDays(ConfigurationSettings.AppSettings("bufferDays"))
        Dim sql = "SELECT vwInventoryView.Available - SUM(dbo.tblOrderItems.Quantity) AS currentavailable FROM tblOrders INNER JOIN tblOrderItems ON tblOrders.pkOrderID = tblOrderItems.fkOrderID INNER JOIN tblInventory ON tblOrderItems.fkInventoryID = tblInventory.pkInventoryID  INNER JOIN vwInventoryView ON tblInventory.pkInventoryID = vwInventoryView.pkInventoryID " &
                    "WHERE (tblInventory.isDeleted=0 and (tblOrders.fkStatusID = 1001 or tblOrders.fkStatusID = 1002) AND (tblOrderItems.fkInventoryID = '" & pkid & "')) AND ((tblOrders.DateArrive BETWEEN N'" & sdate & "' AND N'" & edate & "') OR (tblOrders.DatePickup BETWEEN N'" & sdate & "' AND N'" & edate & "')) "
        If orderID <> "" Then 'if there a draft ignore the order
            sql = sql & " and pkOrderID<>" & orderID
        End If
        sql = sql & " GROUP BY vwInventoryView.Available "

        Dim dt = ReadRecords(sql)
        If dt.Rows.Count > 0 Then

            count = CInt(dt(0)("currentavailable").ToString())
            If qty > count Then
                Return False

            End If
        End If

        Return True

    End Function




    Sub DeleteOrderItems(pkid)
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "Delete from tblOrderItems where fkOrderID = @pkid"
            sqlComm.Parameters.Add(New SqlParameter("pkid", pkid))

            Try
                sqlComm.ExecuteNonQuery()
            Catch ex As Exception
                ErrorEmail(ex.Message)
            End Try
        End Using
    End Sub

    Sub AddItem(pkid, quantity, fkinventoryID)
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            'If Request.QueryString("OrderID") = "" Then
            sqlComm.CommandText = "Insert into tblOrderItems ([fkInventoryID],[Quantity],[fkOrderID]) values (@fkInventoryID,@Quantity,@fkOrderID); SELECT SCOPE_IDENTITY()"
            'Else
            'sqlComm.CommandText = "update"
            'End If

            sqlComm.Parameters.Add(New SqlParameter("fkInventoryID", fkinventoryID))
            sqlComm.Parameters.Add(New SqlParameter("Quantity", quantity))
            sqlComm.Parameters.Add(New SqlParameter("fkOrderID", pkid))
            Try
                pkid = sqlComm.ExecuteScalar()
            Catch ex As Exception
                ErrorEmail(ex.Message)
            End Try
        End Using
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
            ErrorEmail(ex.Message)
        End Try
        Return dt

    End Function

    Protected Sub RadButton2_Click(sender As Object, e As EventArgs) Handles RadButton2.Click
        Dim validate As Boolean = True
        If puser.role = 1000 Then
            validate = IsValid
        End If

        If validate Then

            Dim pkid = SaveOrder(1000)
            If pkid.ToString.Contains("Error") Then
                lbError.Text = pkid
                Exit Sub

            End If
            DeleteOrderItems(pkid)
            GetOrderItems(pkid)

            Dim targetFolder As String = Server.MapPath("Documents/" & pkid & "/")
            System.IO.Directory.CreateDirectory(targetFolder)
            For Each file As UploadedFile In AsyncUpload1.UploadedFiles
                file.SaveAs(targetFolder & file.GetName, True)
            Next

            Session("pkid") = pkid
            Response.Redirect("Review.aspx", False)
        End If
    End Sub

    Protected Sub RadButton3_Click(sender As Object, e As EventArgs) Handles RadButton3.Click
        Dim pkid = SaveOrder(fkstatusID.Value)
        If pkid.ToString.Contains("Error") Then
            lbError.Text = pkid
            Exit Sub

        End If
        DeleteOrderItems(pkid)
        GetOrderItems(pkid)

        Dim targetFolder As String = Server.MapPath("Documents/" & pkid & "/")
        System.IO.Directory.CreateDirectory(targetFolder)
        For Each file As UploadedFile In AsyncUpload1.UploadedFiles
            file.SaveAs(targetFolder & file.GetName, True)
        Next
        Response.Redirect("Admin.aspx", False)

    End Sub

    Function SaveOrder(ByVal status)
        Dim pkid = ""

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
                                    ",[ShipCountry]=@ShipCountry,[PickupFrom]=@PickupFrom,[PickupContactName]=@PickupContactName,[PickupContactPhone]=@PickupContactPhone,[PickupContactEmail]=@PickupContactEmail,[PickupAddress1]=@PickupAddress1,[PickupAddress2]=@PickupAddress2,[PickupCity]=@PickupCity,[PickupState]=@PickupState,[PickupZip]=@PickupZip,[PickupCountry]=@PickupCountry,[CostCenter]=@CostCenter,[UDF10]=@UDF10, ExhibitorWebsite=@ExhibitorWebsite, ExhibitorPass=@ExhibitorPass, ExhibitorUser=@ExhibitorUser, Material=@Material, Labor=@Labor, TierCosts=@TierCosts, TrackingNumber = @TrackingNumber, fkDepartmentID = @fkDepartmentID, ShippingType = @ShippingType where pkOrderID=@pkOrderID"
                sqlComm.Parameters.Add(New SqlParameter("pkorderid", Request.QueryString("OrderID")))
            End If

            sqlComm.Parameters.Add(New SqlParameter("ContactName", tbContactName.Text))
            sqlComm.Parameters.Add(New SqlParameter("ContactEmail", tbContactEmail.Text))
            sqlComm.Parameters.Add(New SqlParameter("ContactPhone", tbContactPhone.Text))
            sqlComm.Parameters.Add(New SqlParameter("EventName", tbEventName.Text))
            sqlComm.Parameters.Add(New SqlParameter("DateStart", tbArrivalDate.SelectedDate))
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
            sqlComm.Parameters.Add(New SqlParameter("fkUserID", fkUserID.Value))
            sqlComm.Parameters.Add(New SqlParameter("ShipTo", tbShipTo.Text))
            sqlComm.Parameters.Add(New SqlParameter("ShipContactName", tbSOnsiteContact.Text))
            sqlComm.Parameters.Add(New SqlParameter("ShipContactPhone", tbSOnsitePhone.Text))
            sqlComm.Parameters.Add(New SqlParameter("ShipContactEmail", tbSOnsiteEmail.Text))
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
            sqlComm.Parameters.Add(New SqlParameter("TrackingNumber", tbTracking.Text))
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
                ErrorEmail(ex.Message)
            End Try
        End Using
        Return pkid
    End Function


    Protected Sub RadButton1_Click(sender As Object, e As EventArgs) Handles RadButton1.Click
        Grid1.Rebind()
    End Sub


    Protected Sub datechanged()
        tbArrivalDate.SelectedDate = CDate(tbStartDate.SelectedDate).Date.AddDays(-1)
        If Not tbArrivalDate.IsEmpty AndAlso Not tbPickupDate.IsEmpty Then
            Grid1.Rebind()
        End If
    End Sub

    Sub CheckDefaults()
        If tbBoothSize.SelectedValue.ToString.Contains("10") Or tbBoothSize.SelectedValue.ToString.Contains("20") Or tbBoothSize.SelectedValue.ToString.Contains("Other") Then
            trElectrical.Visible = True
            cbElectrical.SelectedValue = "5 Amp"
            trCarpet.Visible = True
            trLeads.Visible = True
            trFurniture.Visible = True
            trInternet.Visible = True
            trMaterial.Visible = True
            trLabor.Visible = True
        Else
            trElectrical.Visible = False
            trCarpet.Visible = False
            trLeads.Visible = False
            trFurniture.Visible = False
            trInternet.Visible = False
            trMaterial.Visible = False
            trLabor.Visible = False

            cbElectrical.SelectedValue = ""
            rbShipCarpet.SelectedValue = "No"
            rbInternet.SelectedValue = "No"
            rbLabor.SelectedValue = "No"
            rbLeadRetrieval.SelectedValue = "No"
            rbMaterial.SelectedValue = "No"
            tbShipFurniture.Text = ""

        End If

        If tbBoothSize.SelectedValue.ToString.Contains("10 x 10 Softside") Then
            rbLabor.SelectedValue = "No"
            rbLabor.Enabled = False
        ElseIf tbBoothSize.SelectedValue.ToString.Contains("10 x 10 Hardside") Or tbBoothSize.SelectedValue.ToString.Contains("Other") Then
            rbLabor.SelectedValue = "Yes"
            rbLabor.Enabled = True
        End If


        If rbPortal.SelectedValue = "Yes" Then
            trportalsite.Visible = True
            trportalname.Visible = True
            trportalpass.Visible = True
            vtbPortalsite.Enabled = True
            vtbPortalpass.Enabled = True
            vtbPortaluser.Enabled = True
        Else
            trportalsite.Visible = False
            trportalname.Visible = False
            trportalpass.Visible = False
            vtbPortalsite.Enabled = False
            vtbPortalpass.Enabled = False
            vtbPortaluser.Enabled = False
        End If

        'if its a pull up add table drap
        If tbBoothSize.SelectedValue.ToString.Contains("Pull Up") Then
            search = "Pull Up"
        End If
        If tbBoothSize.SelectedValue.ToString.Contains("10 x 10") Then
            search = "10x10"
        End If

        Grid1.Rebind()


    End Sub

    Protected Sub rbPortal_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbPortal.SelectedIndexChanged
        CheckDefaults()
    End Sub

    Sub ErrorEmail(body As String)

        Dim SmtpServer As New SmtpClient(ConfigurationSettings.AppSettings("SMTP"))
        SmtpServer.EnableSsl = False
        'SmtpServer.Credentials = New Net.NetworkCredential("@gmail.com", "xxx")

        Dim mail As New MailMessage("no-reply@yourpinnacle.net", "adamn@pinnacle-exhibits.com", "Error", body)
        mail.From = New MailAddress("no-reply@yourpinnacle.net", "Pinnacle Portables")
        mail.IsBodyHtml = True

        SmtpServer.Send(mail)

    End Sub
End Class

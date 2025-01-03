Imports Telerik.Web.UI
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail

Partial Class InventorySelect
    Inherits Telerik.Web.UI.RadAjaxPage
    Dim orderID As String = ""
    Dim puser As PortalUser
    Public services As String
    Public portal As String
    Public search As String
    Public departmentId As Integer
    Public startDate As Date
    Public endDate As Date
    Public fulfillmentLocation As String
    Public outboundBufferDays As Integer
    Public inboundBufferDays As Integer

    Dim atpQtyChanged As Boolean = False
    Dim atpQty0 As Boolean = False
    Dim qtyChangedCount As Integer = 0
    Dim qty0Count As Integer = 0
    Dim clearCart As Boolean = False


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

        If Not IsPostBack Then

            search = ""
            fkUserID.Value = puser.userID

            Session("filter") = ""
            Session("cart") = Nothing
            Session("Reset") = True
            Session("qtyChangedTbl") = Nothing
            Session("qty0Tbl") = Nothing

            qty0.Visible = False
            qtyChanged.Visible = False
            qtyChangedGrid.Visible = False
            qty0Grid.Visible = False



            'ClientScript.RegisterStartupScript(Me.GetType(), "SessionAlert", "SessionExpireAlert(" & timeout & ");", True)
            'Page.DataBind()

        End If


        If Request.QueryString("orderID") <> "" Then
            orderID = Replace(Request.QueryString("orderID"), "'", "''")
            'Dim dt = ReadRecords("Select * from tblOrderS where pkorderID=" & orderID)
            Dim dt As DataTable

            If Request.QueryString("editOrder") <> "" Then
                dt = ReadRecords("Select * from tmpOrders where sessionid = (SELECT MAX(sessionid) FROM tmpOrders) and pkorderID=" & orderID)
            Else
                dt = ReadRecords("Select * from tblOrderS where pkorderID=" & orderID)
            End If

            If dt.Rows.Count > 0 Then

                fkstatusID.Value = dt(0)("fkStatusID").ToString()

                If dt(0)("DateArrive").ToString() <> "" Then
                    endDate = dt(0)("DatePickup")
                    startDate = dt(0)("DateArrive")
                End If

                'If dt(0)("DateArrive").ToString() <> "" Then
                '    fulfillmentLocation = dt(0)("fkFulfillmentLocationId").ToString()
                '    Select Case fulfillmentLocation
                '        Case "Europe"
                '            outboundBufferDays = 7
                '            inboundBufferDays = 10
                '        Case "United States"
                '            outboundBufferDays = 7
                '            inboundBufferDays = 10
                '        Case "Japan"
                '            outboundBufferDays = 5
                '            inboundBufferDays = 8
                '        Case "China"
                '            outboundBufferDays = 6
                '            inboundBufferDays = 9
                '        Case "Australia"
                '            outboundBufferDays = 8
                '            inboundBufferDays = 11
                '    End Select
                'End If

                If Not IsPostBack Then
                    Dim dt2 = ReadRecords("Select * from tblShipCountry where ShipCountryName='" & dt(0)("ShipCountry").ToString() & "'")

                    If dt2.Rows.Count > 0 Then
                        dlfullfillmentLocation.DataSource = ReadRecords("Select * from tblShipCountry where ShipCountryName='" & dt(0)("ShipCountry").ToString() & "'order by Sequence asc")
                        dlfullfillmentLocation.DataTextField = "PrimaryShipFromLocation"
                        dlfullfillmentLocation.DataValueField = "PrimaryShipFromLocation"
                        dlfullfillmentLocation.DataBind()
                        If dt(0)("fkFulfillmentLocationId") <> "" Then
                            For Each item In dlfullfillmentLocation.Items
                                If item.Text.ToString() = dt(0)("fkFulfillmentLocationId").ToString() Then
                                    dlfullfillmentLocation.SelectedValue = dt(0)("fkFulfillmentLocationId")
                                End If
                            Next
                            'dlfullfillmentLocation.SelectedValue = dt(0)("fkFulfillmentLocationId")
                        End If
                    Else
                        Select Case puser.department
                            Case 1004
                                Dim item1 As New RadComboBoxItem()
                                item1.Text = "Europe"
                                item1.Value = "Europe"
                                dlfullfillmentLocation.Items.Add(item1)
                            Case Else
                                Dim item1 As New RadComboBoxItem()
                                item1.Text = "United States"
                                item1.Value = "United States"
                                dlfullfillmentLocation.Items.Add(item1)
                        End Select
                    End If
                End If



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
        End If

    End Sub




    Protected Sub RadGrid1_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles Grid1.NeedDataSource

        Dim sql = String.Empty

        Dim orderId = Replace(Request.QueryString("orderID"), "'", "''")

        Dim company = puser.company
        Dim department = puser.department
        Dim exhibitremoval = "and fkInventoryTypeID != 1012"

        Dim sdate As Date
        Dim edate As Date

        fulfillmentLocation = dlfullfillmentLocation.SelectedValue
        Select Case fulfillmentLocation
            Case "Europe"
                outboundBufferDays = 7
                inboundBufferDays = 10
            Case "United States"
                outboundBufferDays = 7
                inboundBufferDays = 10
            Case "Japan"
                outboundBufferDays = 5
                inboundBufferDays = 8
            Case "China"
                outboundBufferDays = 6
                inboundBufferDays = 9
            Case "Australia"
                outboundBufferDays = 8
                inboundBufferDays = 11
        End Select


        sdate = startDate
        sdate = MinusBusinessDays(sdate, outboundBufferDays)
        edate = endDate
        edate = AddBusinessDays(edate, inboundBufferDays)

        'this variable holds SQL logic to remove exhibit part numbers, the leading space in the string is NECESSARY
        'Dim exhibitremoval = " AND NOT PARTNUM BETWEEN 2000 and 2999 AND NOT PARTNUM >= 9000 ORDER BY type"
        'Adminisrator role of 1003 gets to see everything regardless of companyID or departmentID, logic has been added to limit part number return to EXCLUDE exhibit part numbers
        If puser.role = 1003 Or puser.role = 1006 Or puser.role = 1001 Then
            sql = "SELECT case when fkInventoryTypeID in (1003,1004,1005,1006,1007,1008,1009,1011) then dbo.GetATPConsumableByDateRangeBufferTest(pkInventoryID,N'" & sdate & "', N'" & edate & "','" & inboundBufferDays & "','" & -outboundBufferDays & "','" & orderId & "') else dbo.GetATPByDateRangeBufferTest(pkInventoryID,N'" & sdate & "', N'" & edate & "','" & inboundBufferDays & "','" & -outboundBufferDays & "','" & orderId & "') end as [QuantityAvailableByDate], vwInventoryView.*,ISNULL(Total,0) as TotalInventory, ISNULL(vwInventoryView.Available, ' ') as Available, STRING_AGG( ISNULL(tbldepartment.Description, ' '), '<br>') As deptdesc2 from vwInventoryView join tblDepartmentInventory on vwInventoryView.pkInventoryID = tblDepartmentInventory.fkInventoryID join tblDepartment on tblDepartment.pkDepartmentID = tblDepartmentInventory.fkDepartmentID WHERE vwInventoryView.Location = '" & fulfillmentLocation & "' and isDeleted = 0" & exhibitremoval &
                "Group by vwInventoryView.fkInventoryTypeID, vwInventoryView.Total, vwInventoryView.Available, vwInventoryView.pkInventoryID, vwInventoryView.Name, vwInventoryView.Description , vwInventoryView.DateModified, vwInventoryView.fkInventoryTypeID," &
            "vwInventoryView.Notes, vwInventoryView.UDF01, vwInventoryView.UDF02, vwInventoryView.UDF03, vwInventoryView.isDeleted, vwInventoryView.fkDepartmentID, vwInventoryView.Weight, vwInventoryView.Cost, vwInventoryView.Type,vwInventoryView.SortingSequence, vwInventoryView.deptdesc, vwinventoryview.Available, vwInventoryView.Total, vwInventoryView.PartNum , vwInventoryView.Picture, vwInventoryView.DateCreated, vwInventoryView.events, vwInventoryView.threshold, vwInventoryView.Location"
        End If

        'Super user role of 1001 gets to see everything within their company, based on the item's departmentID referencing companyID against the user login companyID, logic has been added to limit part number return to EXCLUDE exhibit part numbers
        If puser.role = 1001 Then
            'sql = "SELECT * from vwInventoryView LEFT JOIN tblDepartment on vwInventoryView.fkDepartmentID = tblDepartment.pkDepartmentID WHERE isDeleted = 0 AND tblDepartment.fkCompanyID =" & puser.company & exhibitremoval
            'sql = "SELECT case when fkInventoryTypeID in (1003,1004,1005,1006,1007,1008,1009,1011) then dbo.GetATPConsumableByDateRangeBufferTest(pkInventoryID,N'" & sdate & "', N'" & edate & "','" & inboundBufferDays & "','" & -outboundBufferDays & "','" & orderId & "') else dbo.GetATPByDateRangeBufferTest(pkInventoryID,N'" & sdate & "', N'" & edate & "','" & inboundBufferDays & "','" & -outboundBufferDays & "','" & orderId & "') end as [QuantityAvailableByDate], vwInventoryView.*, ISNULL(Total,0) as TotalInventory, ISNULL(vwInventoryView.Available, ' ') as Available, STRING_AGG( ISNULL(tbldepartment.Description, ' '), '<br>') As deptdesc2 from vwInventoryView join tblDepartmentInventory on vwInventoryView.pkInventoryID = tblDepartmentInventory.fkInventoryID join tblDepartment on tblDepartment.pkDepartmentID = tblDepartmentInventory.fkDepartmentID WHERE isDeleted = 0 and vwInventoryView.pkInventoryID in (select distinct fkInventoryID from tblDepartmentInventory tDI inner join tblDepartment tD on tDI.fkDepartmentID = tD.pkDepartmentID where tD.fkCompanyID = " & puser.company & ") " & exhibitremoval &
            '"Group by vwInventoryView.fkInventoryTypeID, vwInventoryView.Total, vwInventoryView.Available, vwInventoryView.pkInventoryID, vwInventoryView.Name, vwInventoryView.Description , vwInventoryView.DateModified, vwInventoryView.fkInventoryTypeID," &
            '"vwInventoryView.Notes, vwInventoryView.UDF01, vwInventoryView.UDF02, vwInventoryView.UDF03, vwInventoryView.isDeleted, vwInventoryView.fkDepartmentID, vwInventoryView.Weight, vwInventoryView.Cost, vwInventoryView.Type,vwInventoryView.SortingSequence, vwInventoryView.deptdesc, vwinventoryview.Available, vwInventoryView.Total, vwInventoryView.PartNum , vwInventoryView.Picture, vwInventoryView.DateCreated, vwInventoryView.events, vwInventoryView.threshold, vwInventoryView.Location"
        End If

        'A normal user or warehouse user will only be able to see inventory of the company AND department in which they are enrolled, logic has been added to limit part number return to exclude EXHIBIT part numbers
        If puser.role = 1000 Or puser.role = 1004 Then
            If puser.department = 1006 Or puser.department = 1007 Or puser.department = 1008 Then
                sql = "SELECT case when fkInventoryTypeID in (1003,1004,1005,1006,1007,1008,1009,1011) then dbo.GetATPConsumableByDateRangeBufferTest(pkInventoryID,N'" & sdate & "', N'" & edate & "','" & inboundBufferDays & "','" & -outboundBufferDays & "','" & orderId & "') else dbo.GetATPByDateRangeBufferTest(pkInventoryID,N'" & sdate & "', N'" & edate & "','" & inboundBufferDays & "','" & -outboundBufferDays & "','" & orderId & "') end as [QuantityAvailableByDate], vwInventoryView.*,ISNULL(Total,0) as TotalInventory, ISNULL(vwInventoryView.Available, ' ') as Available, STRING_AGG( ISNULL(tbldepartment.Description, ' '), '<br>') As deptdesc2 from vwInventoryView join tblDepartmentInventory on vwInventoryView.pkInventoryID = tblDepartmentInventory.fkInventoryID join tblDepartment on tblDepartment.pkDepartmentID = tblDepartmentInventory.fkDepartmentID WHERE vwInventoryView.Location = '" & fulfillmentLocation & "' and isDeleted = 0" & exhibitremoval &
                "Group by vwInventoryView.fkInventoryTypeID, vwInventoryView.Total, vwInventoryView.Available, vwInventoryView.pkInventoryID, vwInventoryView.Name, vwInventoryView.Description , vwInventoryView.DateModified, vwInventoryView.fkInventoryTypeID," &
            "vwInventoryView.Notes, vwInventoryView.UDF01, vwInventoryView.UDF02, vwInventoryView.UDF03, vwInventoryView.isDeleted, vwInventoryView.fkDepartmentID, vwInventoryView.Weight, vwInventoryView.Cost, vwInventoryView.Type,vwInventoryView.SortingSequence, vwInventoryView.deptdesc, vwinventoryview.Available, vwInventoryView.Total, vwInventoryView.PartNum , vwInventoryView.Picture, vwInventoryView.DateCreated, vwInventoryView.events, vwInventoryView.threshold, vwInventoryView.Location"
            Else
                sql = "SELECT case when fkInventoryTypeID in (1003,1004,1005,1006,1007,1008,1009,1011) then dbo.GetATPConsumableByDateRangeBufferTest(pkInventoryID,N'" & sdate & "', N'" & edate & "','" & inboundBufferDays & "','" & -outboundBufferDays & "','" & orderId & "') else dbo.GetATPByDateRangeBufferTest(pkInventoryID,N'" & sdate & "', N'" & edate & "','" & inboundBufferDays & "','" & -outboundBufferDays & "','" & orderId & "') end as [QuantityAvailableByDate], vwInventoryView.*, ISNULL(Total,0) as TotalInventory, ISNULL(vwInventoryView.Available, ' ') as Available, vwInventoryView.PartNum as PartNum, STRING_AGG( ISNULL(tbldepartment.Description, ' '), '<br>') As deptdesc2 from vwInventoryView join tblDepartmentInventory on vwInventoryView.pkInventoryID = tblDepartmentInventory.fkInventoryID join tblDepartment on tblDepartment.pkDepartmentID = tblDepartmentInventory.fkDepartmentID WHERE isDeleted = 0 and vwInventoryView.pkInventoryID in (select fkInventoryID from tblDepartmentInventory tDI inner join tblUsers tU on tDI.fkDepartmentID = tU.fkDepartmentID where tU.pkUserID = " & puser.userID & ")" & exhibitremoval &
            "Group by vwInventoryView.fkInventoryTypeID, vwInventoryView.Total, vwInventoryView.Available,  vwInventoryView.pkInventoryID, vwInventoryView.Name, vwInventoryView.Description , vwInventoryView.DateModified, vwInventoryView.fkInventoryTypeID," &
            "vwInventoryView.Notes, vwInventoryView.UDF01, vwInventoryView.UDF02, vwInventoryView.UDF03, vwInventoryView.isDeleted, vwInventoryView.fkDepartmentID, vwInventoryView.Weight, vwInventoryView.Cost, vwInventoryView.Type,vwInventoryView.SortingSequence, vwInventoryView.deptdesc, vwinventoryview.Available, vwInventoryView.Total, vwInventoryView.PartNum , vwInventoryView.Picture, vwInventoryView.DateCreated, vwInventoryView.events, vwInventoryView.threshold, vwInventoryView.Location"
            End If
        End If
        'sql = "SELECT * FROM vwInventoryView WHERE isDeleted = 0 AND fkDepartmentID = " & department & exhibitremoval


        'If puser.role = 1006 Then
        '    sql = "SELECT vwInventoryView.*,ISNULL(Total,0) as TotalInventory, ISNULL(vwInventoryView.Available, ' ') as QuantityAvailableByDate, STRING_AGG( ISNULL(tbldepartment.Description, ' '), '<br>') As deptdesc2 from vwInventoryView join tblDepartmentInventory on vwInventoryView.pkInventoryID = tblDepartmentInventory.fkInventoryID join tblDepartment on tblDepartment.pkDepartmentID = tblDepartmentInventory.fkDepartmentID WHERE isDeleted = 0 and Location in ('Japan','China','Australia')" & exhibitremoval &
        '       "Group by vwInventoryView.fkInventoryTypeID, vwInventoryView.Total, vwInventoryView.Available, vwInventoryView.pkInventoryID, vwInventoryView.Name, vwInventoryView.Description , vwInventoryView.DateModified, vwInventoryView.fkInventoryTypeID," &
        '   "vwInventoryView.Notes, vwInventoryView.UDF01, vwInventoryView.UDF02, vwInventoryView.UDF03, vwInventoryView.isDeleted, vwInventoryView.fkDepartmentID, vwInventoryView.Weight, vwInventoryView.Cost, vwInventoryView.Type,vwInventoryView.SortingSequence, vwInventoryView.deptdesc, vwinventoryview.Available, vwInventoryView.Total, vwInventoryView.PartNum , vwInventoryView.Picture, vwInventoryView.DateCreated, vwInventoryView.events, vwInventoryView.threshold,vwInventoryView.Location"
        'End If

        Grid1.DataSource = ReadRecords(sql & " order by SortingSequence")

    End Sub

    'Protected Sub qtyChangedGrid_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles qtyChangedGrid.ItemDataBound
    '    If TypeOf e.Item Is GridDataItem Then
    '        If qtyChangedGrid.Items.Count < 0 Then
    '            qtyChanged.Visible = False
    '        End If
    '    End If
    'End Sub

    'Protected Sub qty0Grid_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles qty0Grid.ItemDataBound
    '    If TypeOf e.Item Is GridDataItem Then
    '        If qty0Grid.Items.Count <= 0 Then
    '            qty0.Visible = False
    '        End If
    '    End If
    'End Sub

    Protected Sub Grid1_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles Grid1.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            GetQuantity(e.Item)
        End If
    End Sub



    Protected Sub orderGrid_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles orderGrid.NeedDataSource
        orderGrid.DataSource = Session("cart")
    End Sub

    Protected Sub qtyChangedGrid_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles qtyChangedGrid.NeedDataSource
        qtyChangedGrid.DataSource = Session("qtyChangedTbl")
    End Sub

    Protected Sub qty0Grid_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles qty0Grid.NeedDataSource
        qty0Grid.DataSource = Session("qty0Tbl")
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
                    item.Display = True
                End If
            Next
            'updateOrderStatus(1000)

        End If
    End Sub


    Protected Sub orderGrid_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs)
        If TypeOf e.Item Is GridDataItem Then
            Dim itemStatus = GetCartQuantity(e.Item)
            Dim img As System.Web.UI.WebControls.Image = CType(e.Item.FindControl("Image1"), System.Web.UI.WebControls.Image)
            Select Case itemStatus
                Case "atpQty0"
                    img.ImageUrl = "https://assets.yourpinnacle.net/CL1001/WarningHexagonRed.png"
                Case "atpQtyChanged"
                    img.ImageUrl = "https://assets.yourpinnacle.net/CL1001/WarningTriangleYellow.png"
                Case Else
                    img.Visible = False
            End Select
        End If
    End Sub

    Function GetCartQuantity(ByVal item As GridDataItem) As String
        Dim itemStatus As String = "No changes"

        Try
            Dim count = 0
            Dim quantity = 0
            Dim pkid = item.GetDataKeyValue("pkInventoryID")
            Dim qty = item.GetDataKeyValue("Quantity")
            Dim available = item.GetDataKeyValue("Available")
            Dim partNum = item.GetDataKeyValue("PartNum")
            Dim name = item.GetDataKeyValue("Name")

            Dim orderId = Replace(Request.QueryString("orderID"), "'", "''")

            Dim sdate As Date
            Dim edate As Date

            sdate = startDate
            sdate = MinusBusinessDays(sdate, outboundBufferDays)
            edate = endDate
            edate = AddBusinessDays(edate, inboundBufferDays)

            Dim sql = "SELECT case when fkInventoryTypeID in (1003,1004,1005,1006,1007,1008,1009,1011) then dbo.GetATPConsumableByDateRangeBufferTest(pkInventoryID,N'" & sdate & "', N'" & edate & "','" & inboundBufferDays & "','" & -outboundBufferDays & "','" & orderId & "') else dbo.GetATPByDateRangeBufferTest(pkInventoryID,N'" & sdate & "', N'" & edate & "','" & inboundBufferDays & "','" & -outboundBufferDays & "','" & orderId & "') end as [QuantityAvailableByDate],  * from vwInventoryView where isdeleted = 0 AND pkinventoryId = '" & pkid & "'"

            Dim dt = ReadRecords(sql)
            If dt.Rows.Count > 0 Then
                For Each r In dt.Rows
                    count = count + CInt(r("QuantityAvailableByDate").ToString())
                Next
            End If

            Dim cartDD As DropDownList

            cartDD = DirectCast(item.FindControl("myQty"), DropDownList)

            Try
                While quantity <= count
                    cartDD.Items.Add(quantity)
                    quantity = quantity + 1

                End While
            Catch ex As Exception
            End Try

            If cartDD.Items.Count - 1 < qty Then
                atpQtyChanged = True
                cartDD.ForeColor = Drawing.Color.Red
                itemStatus = "atpQtyChanged"
                If cartDD.Items.Count - 1 > 0 Then
                    qtyChanged.Visible = True
                    qtyChangedGrid.Visible = True
                    addToQtyChanged(partNum, name, qty, count)
                Else

                End If
                cartDD.SelectedIndex = count
            End If

            If count <= 0 Then
                cartDD.Items.Add(0)
                'item.Display = False 'hide the item
                atpQty0 = True
                qty0.Visible = True
                qty0Grid.Visible = True
                addToQty0(partNum, name, qty, count)

                cartDD.SelectedIndex = qty
                itemStatus = "atpQty0"
                'For Each item In Grid1.MasterTableView.Items
                '    If (item.GetDataKeyValue("pkInventoryID").ToString = pkid) Then
                '        item.Display = True
                '    End If
                'Next
            End If



            If count < qty Then
                cartDD.SelectedIndex = count
            Else
                cartDD.SelectedIndex = qty
            End If

        Catch ex As Exception
        End Try

        Return itemStatus

    End Function

    Sub addToQtyChanged(partNum, Name, PreviousQty, NewQty)
        If IsNothing(Session("qtyChangedTbl")) Then
            Dim dt As New DataTable
            dt.Columns.Add("PartNum")
            dt.Columns.Add("Name")
            dt.Columns.Add("PreviousQuantity")
            dt.Columns.Add("NewQuantity")
            dt.AcceptChanges()
            Dim dr As DataRow = dt.NewRow

            dr(0) = partNum
            dr(1) = Name
            dr(2) = PreviousQty
            dr(3) = NewQty


            dt.Rows.Add(dr)
            dt.AcceptChanges()
            Session("qtyChangedTbl") = dt
        Else
            Dim dt As DataTable = TryCast(Session("qtyChangedTbl"), DataTable)
            For Each r As DataRow In dt.Rows
                If r("PartNum") = partNum Then
                    r.Delete()
                End If
            Next
            dt.AcceptChanges()
            Dim dr As DataRow = dt.NewRow
            dr(0) = partNum
            dr(1) = Name
            dr(2) = PreviousQty
            dr(3) = NewQty
            dt.Rows.Add(dr)
            dt.AcceptChanges()
            Session("qtyChangedTbl") = dt
        End If
        qtyChangedGrid.Rebind()

    End Sub

    Sub addToQty0(partNum, Name, PreviousQty, NewQty)
        If IsNothing(Session("qty0Tbl")) Then
            Dim dt As New DataTable
            dt.Columns.Add("PartNum")
            dt.Columns.Add("Name")
            dt.Columns.Add("PreviousQuantity")
            dt.Columns.Add("NewQuantity")
            dt.AcceptChanges()
            Dim dr As DataRow = dt.NewRow

            dr(0) = partNum
            dr(1) = Name
            dr(2) = PreviousQty
            dr(3) = NewQty


            dt.Rows.Add(dr)
            dt.AcceptChanges()
            Session("qty0Tbl") = dt
        Else
            Dim dt As DataTable = TryCast(Session("qty0Tbl"), DataTable)
            For Each r As DataRow In dt.Rows
                If r("PartNum") = partNum Then
                    r.Delete()
                End If
            Next
            dt.AcceptChanges()
            Dim dr As DataRow = dt.NewRow
            dr(0) = partNum
            dr(1) = Name
            dr(2) = PreviousQty
            dr(3) = NewQty
            dt.Rows.Add(dr)
            dt.AcceptChanges()
            Session("qty0Tbl") = dt
        End If
        qty0Grid.Rebind()
    End Sub

    Function GetQuantity(ByVal item As GridDataItem)
        Try
            Dim count = 0
            Dim quantity = 0
            Dim pkid = item.GetDataKeyValue("pkInventoryID")
            Dim qty = item.GetDataKeyValue("Quantity")

            Dim orderId = Replace(Request.QueryString("orderID"), "'", "''")


            Dim sdate As Date
            Dim edate As Date

            sdate = startDate
            sdate = MinusBusinessDays(sdate, outboundBufferDays)
            edate = endDate
            edate = AddBusinessDays(edate, inboundBufferDays)

            Dim sql = "SELECT case when fkInventoryTypeID in (1003,1004,1005,1006,1007,1008,1009,1011) then dbo.GetATPConsumableByDateRangeBufferTest(pkInventoryID,N'" & sdate & "', N'" & edate & "','" & inboundBufferDays & "','" & -outboundBufferDays & "','" & orderId & "') else dbo.GetATPByDateRangeBufferTest(pkInventoryID,N'" & sdate & "', N'" & edate & "','" & inboundBufferDays & "','" & -outboundBufferDays & "','" & orderId & "') end as [QuantityAvailableByDate],  * from vwInventoryView where isdeleted = 0 AND pkinventoryId = '" & pkid & "'"

            Dim dt = ReadRecords(sql)
            If dt.Rows.Count > 0 Then
                For Each r In dt.Rows
                    count = count + CInt(r("QuantityAvailableByDate").ToString())
                Next
            End If

            Dim myDD As DropDownList

            myDD = DirectCast(item.FindControl("myDD"), DropDownList)

            Try
                While quantity <= count
                    myDD.Items.Add(quantity)
                    quantity = quantity + 1

                End While
            Catch ex As Exception
                'item.Display = False 'hide the item
            End Try


            If myDD.Items.Count <= 0 Then
                myDD.Items.Add(0)
                'item.Display = False 'hide the item
            End If


            If Request.QueryString("orderid") <> "" Then
                Dim dt2 = ReadRecords("SELECT case when fkInventoryTypeID in (1003,1004,1005,1006,1007,1008,1009,1011) then dbo.GetATPConsumableByDateRangeBufferTest(pkInventoryID,N'" & sdate & "', N'" & edate & "','" & inboundBufferDays & "','" & -outboundBufferDays & "','" & orderId & "') else dbo.GetATPByDateRangeBufferTest(pkInventoryID,N'" & sdate & "', N'" & edate & "','" & inboundBufferDays & "','" & -outboundBufferDays & "','" & orderId & "') end as [QuantityAvailableByDate],  * from vwInventoryView iv join tblOrderItems oi on iv.pkInventoryID = oi.fkInventoryID where iv.isdeleted = 0 and iv.pkInventoryID =" & pkid & " and oi.fkOrderID =" & Replace(Request.QueryString("orderID"), "'", "''"))
                'Dim dt2 = ReadRecords("Select tblOrderItems.quantity, tblInventory.Picture, tblinventory.pkinventoryid, tblInventory.PartNum, tblInventory.Description  from tblOrderItems INNER JOIN tblInventory ON tblOrderItems.fkInventoryID = tblInventory.pkInventoryID where tblOrderItems.fkinventoryID=" & pkid & " and tblOrderItems.fkOrderID=" & Replace(Request.QueryString("orderID"), "'", "''"))
                If dt2.Rows.Count > 0 Then
                    Dim atpQtyChanged As Boolean = False
                    Dim atpQty0 As Boolean = False
                    Dim q = dt2(0)("quantity").ToString
                    Dim description = dt2(0)("Description").ToString
                    Dim type = dt2(0)("Type").ToString
                    AddtoCart(q, pkid, dt2(0)("PartNum").ToString, dt2(0)("picture").ToString, description, count, item, type)
                    item.Display = False
                End If
            End If


        Catch ex As Exception
            'ErrorEmail(ex.Message)
        End Try
    End Function

    Sub AddtoCart(qty, pkInventoryID, partNum, picture, description, count, item, type, Optional highlight = False)
        If IsNothing(Session("cart")) Then
            Dim dt As New DataTable
            dt.Columns.Add("Quantity")
            dt.Columns.Add("pkInventoryID")
            dt.Columns.Add("PartNum")
            dt.Columns.Add("Picture")
            dt.Columns.Add("Description")
            dt.Columns.Add("Type")
            Dim dr As DataRow = dt.NewRow

            dr(0) = qty
            dr(1) = pkInventoryID
            dr(2) = partNum
            dr(3) = picture
            dr(4) = description
            dr(5) = type
            dt.Rows.Add(dr)
            dt.AcceptChanges()
            Session("cart") = dt

        Else
            Dim dt As DataTable = TryCast(Session("cart"), DataTable)
            For Each r As DataRow In dt.Rows
                If r("pkInventoryID") = pkInventoryID Then
                    r.Delete()
                End If
            Next

            Dim dr As DataRow = dt.NewRow
            dr(0) = qty
            dr(1) = pkInventoryID
            dr(2) = partNum
            dr(3) = picture
            dr(4) = description
            dr(5) = type
            If qty > 0 Then
                dt.Rows.Add(dr)
            End If

            dt.AcceptChanges()
            Session("cart") = dt

        End If
        orderGrid.Rebind()

    End Sub

    'Function GetCartQuantity(pkid, count, qty)

    '    For Each item In orderGrid.MasterTableView.Items
    '        If (item.GetDataKeyValue("pkInventoryID").ToString = pkid) Then
    '            Dim quantity = 0
    '            Dim ddl As DropDownList
    '            ddl = DirectCast(item.FindControl("myQty"), DropDownList)
    '            Try
    '                While quantity <= count
    '                    ddl.Items.Add(quantity)
    '                    quantity = quantity + 1

    '                End While
    '            Catch ex As Exception
    '                item.Display = False 'hide the item
    '            End Try

    '            ddl.SelectedIndex = qty
    '        End If
    '    Next

    'End Function


    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim ddl As DropDownList = TryCast(sender, DropDownList)
        Dim item As GridDataItem = TryCast(ddl.NamingContainer, GridDataItem)
        Dim pkinventoryid = item.GetDataKeyValue("pkInventoryID")

        Dim partNum = item.GetDataKeyValue("PartNum")
        Dim description = item.GetDataKeyValue("Description")
        Dim type = item.GetDataKeyValue("Type")

        Dim count = ddl.Items.Count - 1
        AddtoCart(ddl.SelectedValue, pkinventoryid, partNum, item.GetDataKeyValue("Picture").ToString, description, count, item, type)
        'updateOrderStatus(1000)
        item.Display = False
        ddl.SelectedValue = 0
    End Sub


    Protected Sub DropDownList2_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim ddl As DropDownList = TryCast(sender, DropDownList)
        Dim item As GridDataItem = TryCast(ddl.NamingContainer, GridDataItem)
        Dim pkinventoryid = item.GetDataKeyValue("pkInventoryID")
        Dim qty = item.GetDataKeyValue("Quantity")
        Dim partNum = item.GetDataKeyValue("PartNum")
        Dim description = item.GetDataKeyValue("Description")
        Dim type = item.GetDataKeyValue("Type")

        Dim count = ddl.Items.Count - 1

        If ddl.SelectedValue = 0 Then
            Dim dt As DataTable = TryCast(Session("cart"), DataTable)
            For Each r As DataRow In dt.Rows
                If r("pkInventoryID") = pkinventoryid Then
                    r.Delete()
                End If
            Next
            dt.AcceptChanges()
            Session("cart") = dt
            orderGrid.DataSource = Nothing
            orderGrid.Rebind()

            For Each item In Grid1.MasterTableView.Items
                If (item.GetDataKeyValue("pkInventoryID").ToString = pkinventoryid) Then
                    ddl.SelectedValue = 0
                    item.Display = True
                End If
            Next
        Else
            AddtoCart(ddl.SelectedValue, pkinventoryid, partNum, item.GetDataKeyValue("Picture").ToString, description, count, item, type)
        End If

        'updateOrderStatus(1000)
    End Sub

    'Protected Sub updateOrderStatus(ByVal orderStatus)
    '    Dim orderId = Request.QueryString("orderID")

    '    Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
    '        strConnStr.Open()

    '        Dim sqlComm As New SqlCommand()
    '        sqlComm.Connection = strConnStr

    '        sqlComm.CommandText = "update tblOrders set [fkStatusID]=@fkStatusID where pkOrderID=@pkOrderID"
    '        sqlComm.Parameters.Add(New SqlParameter("pkOrderID", orderId))
    '        sqlComm.Parameters.Add(New SqlParameter("fkStatusID", orderStatus))

    '        Try
    '            orderId = sqlComm.ExecuteScalar()
    '        Catch ex As Exception

    '        End Try
    '    End Using

    'End Sub


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

    Function GetOrderItems(pkid) As Boolean
        Dim isEmpty As Boolean = False
        Dim addToCart As Integer = 0
        Dim frameQty As Integer = 0
        Dim bannerQty As Integer = 0
        Dim tableQty As Integer = 0
        Dim bannerCaseItem1 As Integer = 0
        Dim bannerCaseItem2 As Integer = 0
        Dim dhlItem1 As Integer = 0
        Dim dhlItem2 As Integer = 0


        For Each c As Control In Master.FindControl("MainContent").Controls
            If c.ID = "RadAjaxPanel1" Then
                For Each d As Control In c.Controls
                    If TypeOf d Is Telerik.Web.UI.RadGrid Then
                        Dim grid As RadGrid = CType(d, RadGrid)
                        ' access HeaderItem
                        'Dim grid As RadGrid = DirectCast(d.FindControl("orderGrid"), RadGrid)

                        'Dim grid As RadGrid = CType(d.FindControl("orderGrid"), RadGrid)
                        If grid.MasterTableView.Items.Count > 0 Then
                            For Each dataItem As GridDataItem In grid.MasterTableView.Items
                                Dim qty = CType(dataItem.FindControl("myQty"), DropDownList).SelectedValue
                                If qty = "" Then
                                    qty = 0
                                Else
                                    qty = CInt(qty)
                                End If
                                If qty > 0 Then
                                    AddItem(pkid, dataItem.GetDataKeyValue("Quantity"), dataItem.GetDataKeyValue("pkInventoryID"))
                                    addToCart = +1
                                End If

                                If dlfullfillmentLocation.SelectedValue = "Japan" Or dlfullfillmentLocation.SelectedValue = "China" Or dlfullfillmentLocation.SelectedValue = "Australia" Or dlfullfillmentLocation.SelectedValue = "EUROPE" Then
                                    Dim type = dataItem.GetDataKeyValue("Type")
                                    Select Case type
                                        Case "Self-Standing Frame"
                                            frameQty += qty
                                            bannerCaseItem1 = dataItem.GetDataKeyValue("PartNum")
                                        Case "Black Banner Case"
                                            bannerQty += qty
                                            bannerCaseItem2 = dataItem.GetDataKeyValue("PartNum")
                                        Case "Table Throw"
                                            tableQty += qty
                                            dhlItem1 = dataItem.GetDataKeyValue("PartNum")
                                    End Select
                                End If
                            Next

                            If dlfullfillmentLocation.SelectedValue = "Japan" Or dlfullfillmentLocation.SelectedValue = "China" Or dlfullfillmentLocation.SelectedValue = "Australia" Or dlfullfillmentLocation.SelectedValue = "EUROPE" Then
                                If bannerQty <> frameQty Then
                                    resetCart(pkid)
                                    errorMessage.Text = "&#x2022;Please add more  Banner Cases or Self-standing frame to your cart to proceed"
                                    isEmpty = True
                                    qty0.Visible = False
                                    qtyChanged.Visible = False
                                    qtyChangedGrid.Visible = False
                                    qty0Grid.Visible = False
                                    Return isEmpty
                                    Exit Function
                                End If

                                If Request.QueryString("editOrder") <> "" Then
                                    archiveSession()
                                End If

                                Dim shipmentid As Int16

                                If dlfullfillmentLocation.SelectedValue = "EUROPE" Then
                                    shipmentid = InsertEMEAShipment(pkid)
                                Else
                                    shipmentid = InsertAPACShipment(pkid)
                                End If


                                If bannerQty > 0 Then
                                    Dim item1 As Nullable(Of Integer)
                                    Dim item2 As Nullable(Of Integer)

                                    For index As Integer = 1 To bannerQty
                                        item1 = bannerCaseItem1
                                        For index2 As Integer = 1 To frameQty
                                            item2 = bannerCaseItem2
                                        Next

                                        If dlfullfillmentLocation.SelectedValue = "EUROPE" Then
                                            InsertEMEAShipmentItem(pkid, 1, shipmentid, "Black Banner Case", item1, item2)
                                        Else
                                            InsertAPACShipmentItem(pkid, 1, shipmentid, "Black Banner Case", item1, item2)
                                        End If
                                    Next
                                End If

                                If tableQty > 0 Then
                                    Dim item1 As Nullable(Of Integer)
                                    Dim item2 As Nullable(Of Integer)

                                    If tableQty Mod 2 = 0 Then
                                        For index As Integer = 1 To tableQty Step 2
                                            item1 = dhlItem1
                                            item2 = dhlItem1
                                            If dlfullfillmentLocation.SelectedValue = "EUROPE" Then
                                                InsertEMEAShipmentItem(pkid, 1, shipmentid, "DHL Box Medium", item1, item2)
                                            Else
                                                InsertAPACShipmentItem(pkid, 1, shipmentid, "DHL Box Medium", item1, item2)
                                            End If
                                        Next
                                        Continue For
                                    ElseIf tableQty Mod 2 = 1 Then
                                        For index As Integer = 1 To tableQty - 1 Step 2
                                            item1 = dhlItem1
                                            item2 = dhlItem1

                                            If dlfullfillmentLocation.SelectedValue = "EUROPE" Then
                                                InsertEMEAShipmentItem(pkid, 1, shipmentid, "DHL Box Medium", item1, item2)
                                            Else
                                                InsertAPACShipmentItem(pkid, 1, shipmentid, "DHL Box Medium", item1, item2)
                                            End If
                                        Next
                                        If dlfullfillmentLocation.SelectedValue = "EUROPE" Then
                                            InsertEMEAShipmentItem(pkid, 1, shipmentid, "DHL Box Medium", item1, item2)
                                        Else
                                            InsertAPACShipmentItem(pkid, 1, shipmentid, "DHL Box Medium", item1, item2)
                                        End If
                                    End If

                                End If
                            End If

                        End If
                    End If
                Next
            End If
        Next
        If addToCart < 1 Then
            errorMessage.Text = "&#x2022; Please add items to your cart to proceed"
            isEmpty = True
            qty0.Visible = False
            qtyChanged.Visible = False
            qtyChangedGrid.Visible = False
            qty0Grid.Visible = False

        End If
        Return isEmpty

    End Function



    Public Function CheckAvailability(ByVal pkid, ByVal qty) As Boolean
        Dim count = 0

        Dim sdate As Date
        Dim edate As Date
        sdate = startDate
        sdate = MinusBusinessDays(sdate, 7)
        edate = endDate
        edate = AddBusinessDays(edate, 10)
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
            sqlComm.CommandText = "update tmpOrderItems set isarchived = 1 where fkOrderID = @pkid"
            sqlComm.Parameters.Add(New SqlParameter("pkid", pkid))

            Try
                sqlComm.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        End Using
    End Sub

    Sub AddItem(pkid, quantity, fkinventoryID)
        Dim sessionid As Int16
        Dim dt = ReadRecords("select top 1 sessionid from tmpOrderItems where fkorderid ='" & pkid & "' order by sessionID desc")
        If dt.Rows.Count > 0 Then
            sessionid = dt(0)("sessionid")
        End If

        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            If Request.QueryString("editOrder") <> "" Then
                sqlComm.CommandText = "Insert into tmpOrderItems([fkInventoryID], [Quantity], [fkOrderID], [ExtendedCost], [dateCreated], [isarchived], [sessionID]) values (@fkInventoryID,@Quantity,@fkOrderID,@ExtendedCost,@dateCreated, @isarchived, @sessionID);"
            Else
                sqlComm.CommandText = "Insert into tblOrderItems ([fkInventoryID],[Quantity],[fkOrderID]) values (@fkInventoryID,@Quantity,@fkOrderID); SELECT SCOPE_IDENTITY()"
            End If

            'Else
            'sqlComm.CommandText = "update"
            'End If

            sqlComm.Parameters.Add(New SqlParameter("fkInventoryID", fkinventoryID))
            sqlComm.Parameters.Add(New SqlParameter("Quantity", quantity))
            sqlComm.Parameters.Add(New SqlParameter("fkOrderID", pkid))
            sqlComm.Parameters.Add(New SqlParameter("ExtendedCost", ""))
            sqlComm.Parameters.Add(New SqlParameter("dateCreated", Now()))
            sqlComm.Parameters.Add(New SqlParameter("isarchived", 0))
            sqlComm.Parameters.Add(New SqlParameter("sessionID", sessionid))
            Try
                pkid = sqlComm.ExecuteScalar()
            Catch ex As Exception
                'ErrorEmail(ex.Message)
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
            'ErrorEmail(ex.Message)
        End Try
        Return dt

    End Function


    Protected Sub RadButton1_Click(sender As Object, e As EventArgs) Handles RadButton1.Click
        Grid1.Rebind()
    End Sub


    Protected Sub RadButton2_Click(sender As Object, e As EventArgs) Handles RadButton2.Click

        Dim pkid = Request.QueryString("orderID")
        If pkid.ToString.Contains("Error") Then
            lbError.Text = pkid
            Exit Sub
        End If
        DeleteOrderItems(pkid)
        Dim isEmpty = GetOrderItems(pkid)



        If isEmpty Then
            Return
        End If

        UpdateFulfillmentLocation(pkid)

        Session("pkid") = pkid
        If (Request.QueryString.AllKeys.Contains("editOrder")) Then
            Response.Redirect("Review.aspx?editOrder=true", False)
        Else
            Response.Redirect("Review.aspx", False)
        End If
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

    Function AddBusinessDays(startDate As Date, numberOfDays As Integer) As Date
        Dim newDate As Date = startDate
        While numberOfDays > 0
            newDate = newDate.AddDays(1)

            If newDate.DayOfWeek() > 0 AndAlso newDate.DayOfWeek() < 6 Then '1-5 is Mon-Fri
                numberOfDays -= 1
            End If

        End While
        Return newDate
    End Function

    Function MinusBusinessDays(startDate As Date, numberOfDays As Integer) As Date
        Dim newDate As Date = startDate
        While numberOfDays > 0
            newDate = newDate.AddDays(-1)

            If newDate.DayOfWeek() > 0 AndAlso newDate.DayOfWeek() < 6 Then '1-5 is Mon-Fri
                numberOfDays -= 1
            End If

        End While
        Return newDate
    End Function


    Protected Sub RadButton3_Click(sender As Object, e As EventArgs) Handles RadButton3.Click

        Dim pkid = Request.QueryString("orderID")

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

        Response.Redirect("Reports.aspx", False)

    End Sub


    Protected Sub dlfullfillmentLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Grid1.Rebind()
        clearCart = True
        Try
            Dim dt As DataTable = TryCast(Session("cart"), DataTable)
            For Each r As DataRow In dt.Rows
                r.Delete()
            Next
            dt.AcceptChanges()
            Session("cart") = dt
            orderGrid.DataSource = Nothing
            orderGrid.Rebind()

        Catch ex As Exception

        End Try


        qty0.Visible = False
        qtyChanged.Visible = False
        qtyChangedGrid.Visible = False
        qty0Grid.Visible = False

    End Sub


    Sub UpdateFulfillmentLocation(pkid)
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "update tmpOrders set fkFulfillmentLocationId = @fulfillmentLocationId where sessionid = (SELECT MAX(sessionid) FROM tmpOrders) and pkorderID=@pkid;update tblOrders set fkFulfillmentLocationId = @fulfillmentLocationId where pkorderID=@pkid"
            sqlComm.Parameters.Add(New SqlParameter("fulfillmentLocationId", dlfullfillmentLocation.SelectedValue))
            sqlComm.Parameters.Add(New SqlParameter("pkid", pkid))

            Try
                sqlComm.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        End Using
    End Sub



    Function InsertAPACShipment(pkid) As String
        Dim shipmentid As Int16
        Dim SessionId As Int16
        Dim dt = ReadRecords("select top 1 sessionid from tblAPACShipment where fkorderid ='" & pkid & "' order by sessionID desc")
        If dt.Rows.Count > 0 Then
            SessionId = dt(0)("sessionid") + 1
        Else
            SessionId = 1
        End If

        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "Insert into tblAPACShipment ([fkOrderId], [SessionID], [isArchived]) values (@fkOrderID,@sessionID , @isArchived); SELECT SCOPE_IDENTITY()"


            sqlComm.Parameters.Add(New SqlParameter("fkOrderID", pkid))
            sqlComm.Parameters.Add(New SqlParameter("sessionID", SessionId))
            sqlComm.Parameters.Add(New SqlParameter("isArchived", 0))

            Try
                shipmentid = sqlComm.ExecuteScalar()
            Catch ex As Exception
                'ErrorEmail(ex.Message)
            End Try
        End Using

        Return shipmentid
    End Function


    Function InsertEMEAShipment(pkid) As String
        Dim shipmentid As Int16
        Dim SessionId As Int16
        Dim dt = ReadRecords("select top 1 sessionid from tblEMEAShipment where fkorderid ='" & pkid & "' order by sessionID desc")
        If dt.Rows.Count > 0 Then
            SessionId = dt(0)("sessionid") + 1
        Else
            SessionId = 1
        End If

        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "Insert into tblEMEAShipment ([fkOrderId], [SessionID], [isArchived]) values (@fkOrderID,@sessionID , @isArchived); SELECT SCOPE_IDENTITY()"


            sqlComm.Parameters.Add(New SqlParameter("fkOrderID", pkid))
            sqlComm.Parameters.Add(New SqlParameter("sessionID", SessionId))
            sqlComm.Parameters.Add(New SqlParameter("isArchived", 0))

            Try
                shipmentid = sqlComm.ExecuteScalar()
            Catch ex As Exception
                'ErrorEmail(ex.Message)
            End Try
        End Using

        Return shipmentid
    End Function


    Function InsertAPACShipmentItem(pkid, Quantity, shipmentid, CartonName, Item1, Item2)
        Dim SessionId As Int16
        Dim dt = ReadRecords("select top 1 sessionid from tblAPACShipment where fkorderid ='" & pkid & "' order by sessionID desc")
        If dt.Rows.Count > 0 Then
            SessionId = dt(0)("sessionid")
        End If

        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "Insert into tblAPACShipmentItem ([fkOrderid], [Quantity], [fkShipmentId], [CartonName], [SessionID], [isArchived] , [Item1] , [Item2] ) values (@fkOrderID,@Quantity, @fkShipmentId, @CartonName,@sessionID, @isArchived, @Item1, @Item2); SELECT SCOPE_IDENTITY()"


            sqlComm.Parameters.Add(New SqlParameter("fkOrderID", pkid))
            sqlComm.Parameters.Add(New SqlParameter("Quantity", Quantity))
            sqlComm.Parameters.Add(New SqlParameter("fkShipmentId", shipmentid))
            sqlComm.Parameters.Add(New SqlParameter("CartonName", CartonName))
            sqlComm.Parameters.Add(New SqlParameter("sessionID", SessionId))
            sqlComm.Parameters.Add(New SqlParameter("isArchived", 0))
            sqlComm.Parameters.Add(New SqlParameter("Item1", Item1))
            sqlComm.Parameters.Add(New SqlParameter("Item2", Item2))



            Try
                sqlComm.ExecuteScalar()
            Catch ex As Exception
                'ErrorEmail(ex.Message)
            End Try
        End Using
    End Function


    Function InsertEMEAShipmentItem(pkid, Quantity, shipmentid, CartonName, Item1, Item2)
        Dim SessionId As Int16
        Dim dt = ReadRecords("select top 1 sessionid from tblEMEAShipment where fkorderid ='" & pkid & "' order by sessionID desc")
        If dt.Rows.Count > 0 Then
            SessionId = dt(0)("sessionid")
        End If

        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "Insert into tblEMEAShipmentItem ([fkOrderid], [Quantity], [fkShipmentId], [CartonName], [SessionID], [isArchived] , [Item1] , [Item2] ) values (@fkOrderID,@Quantity, @fkShipmentId, @CartonName,@sessionID, @isArchived, @Item1, @Item2); SELECT SCOPE_IDENTITY()"


            sqlComm.Parameters.Add(New SqlParameter("fkOrderID", pkid))
            sqlComm.Parameters.Add(New SqlParameter("Quantity", Quantity))
            sqlComm.Parameters.Add(New SqlParameter("fkShipmentId", shipmentid))
            sqlComm.Parameters.Add(New SqlParameter("CartonName", CartonName))
            sqlComm.Parameters.Add(New SqlParameter("sessionID", SessionId))
            sqlComm.Parameters.Add(New SqlParameter("isArchived", 0))
            sqlComm.Parameters.Add(New SqlParameter("Item1", Item1))
            sqlComm.Parameters.Add(New SqlParameter("Item2", Item2))



            Try
                sqlComm.ExecuteScalar()
            Catch ex As Exception
                'ErrorEmail(ex.Message)
            End Try
        End Using
    End Function


    Function archiveSession()
        Dim pkid = Request.QueryString("orderID")

        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "Update tblAPACShipment set isArchived = 1 where fkOrderId =@pkid ; Update tblAPACShipmentItem set isArchived = 1 where fkOrderId =@pkid"
            sqlComm.Parameters.Add(New SqlParameter("pkid", pkid))

            Try
                sqlComm.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        End Using
    End Function


    Sub resetCart(pkid)
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr

            If Request.QueryString("editOrder") <> "" Then
                sqlComm.CommandText = "update tmpOrderItems set isarchived = 1 where fkOrderID = @pkid;"
            Else
                sqlComm.CommandText = "delete from tblOrderItems where fkOrderID = @pkid"

            End If

            'sqlComm.CommandText = "update tmpOrderItems set isarchived = 1 where fkOrderID = @pkid;delete from tblOrderItems where fkOrderID = @pkid"
            sqlComm.Parameters.Add(New SqlParameter("pkid", pkid))

            Try
                sqlComm.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        End Using
    End Sub

End Class

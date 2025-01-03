Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.IO
Imports System.Net.Mail

Partial Class Fulfillment
    Inherits Telerik.Web.UI.RadAjaxPage

    Public _pkid, _closeout, _finished, _status, _deleting

    Public _sku As Nullable(Of Integer)

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        RadGrid1.RegisterWithScriptManager = False
        RadGrid2.RegisterWithScriptManager = False
        _pkid = Replace(Request.QueryString("orderid"), "'", "''")
        _status = Replace(Request.QueryString("statusid"), "'", "''")
        If _pkid <> "" Then
            Response.Cookies("ActiveJob").Expires = Now()
            Response.Cookies("ActiveJob").Value = ""

            Response.Cookies("ActiveJob").Expires = Now().AddDays(5)
            Response.Cookies("ActiveJob").Value = _pkid & ":" & _status
        End If

        If Not Request.Cookies("ActiveJob") Is Nothing AndAlso IsNothing(_pkid) Then
            _pkid = Request.Cookies("ActiveJob").Value.Split(":")(0)
            _status = Request.Cookies("ActiveJob").Value.Split(":")(1)
        End If

        If Not Page.IsPostBack Then
            _closeout = False
        End If

        If _status = 1002 Then 'outbound
            LinkButtonOutbound.Visible = True
            RadGrid1.Visible = True
        ElseIf _status = 1005 Then
            LinkButtonInbound.Visible = True
            RadGrid2.Visible = True
        End If

    End Sub

    Protected Sub LinkButton3_Click(sender As Object, e As System.EventArgs) Handles BackToShipping.Click
        Response.Redirect("shipping.aspx")
    End Sub

    'Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    '    ProcessBarcode()

    'End Sub


    Protected Sub ProcessBarcode(sku As Integer)
        Dim sql = ""
        Dim dt As DataTable
        Dim status
        Dim consumable = False

        'Dim inventoryID = TextBox1.Text
        'TextBox1.Text = ""
        lbStatus.Text = ""
        If _status = 1002 Then

            'sql = "Select * from vwOrderInventoryItems where pkOrderID=" & _pkid & " and sku=" & inventoryID
            sql = "SELECT tblinventory.fkInventoryTypeID, tblItems.fkItemStatusID, tblItems.pkItemID, vwOrderInventoryItems.pkOrderID, vwOrderInventoryItems.pkInventoryID, vwOrderInventoryItems.fkItemStatusID AS scanned, vwOrderInventoryItems.sku, vwOrderItems.Quantity, { fn IFNULL(t.qty, 0) }  AS tquantity, tblItems.Quantity AS itemized ,vwOrderInventoryItems.DateIn, vwOrderInventoryItems.DateOut " &
                  "FROM tblItems INNER JOIN tblInventory ON tblItems.fkInventoryID = tblInventory.pkInventoryID INNER JOIN vwOrderItems ON tblInventory.pkInventoryID = vwOrderItems.pkInventoryID LEFT OUTER JOIN " &
                  " vwOrderInventoryItems ON tblItems.pkItemID = vwOrderInventoryItems.sku AND vwOrderItems.pkOrderID = vwOrderInventoryItems.pkOrderID AND vwOrderItems.pkInventoryID = vwOrderInventoryItems.pkInventoryID LEFT OUTER JOIN " &
                 "(SELECT SUM(Quantity) AS qty, pkOrderID, pkInventoryID FROM    vwOrderInventoryItems AS vwOrderInventoryItems_1 GROUP BY pkOrderID,  pkInventoryID) AS t ON t.pkOrderID = vwOrderItems.pkOrderID AND t.pkInventoryID = vwOrderItems.pkInventoryID " &
                  "where tblItems.pkItemid=" & sku & " and vwOrderItems.pkOrderID=" & _pkid

            dt = ReadRecords(sql)

            If dt.Rows.Count > 0 AndAlso dt(0)("fkItemStatusID").ToString = "1003" AndAlso String.IsNullOrEmpty(dt(0)("DateIn").ToString) Then
                lbStatus.Text = "Property never returned. Please scan item back in before oubound"
                Return
            End If

            If dt.Rows.Count > 0 AndAlso (dt(0)("scanned").ToString = "1003" Or dt(0)("fkItemStatusID").ToString = "1003") Then
                lbStatus.Text = "Already scanned"
            ElseIf dt.Rows.Count > 0 AndAlso dt(0)("quantity").ToString = dt(0)("tquantity").ToString Then
                lbStatus.Text = "Quantity Reached"
            ElseIf dt.Rows.Count > 0 AndAlso dt(0)("fkItemStatusID").ToString = "1001" Then
                lbStatus.Text = "This is damaged"
            ElseIf dt.Rows.Count > 0 AndAlso dt(0)("fkItemStatusID").ToString = "1002" Then
                lbStatus.Text = "This is missing"

            ElseIf dt.Rows.Count > 0 AndAlso dt(0)("fkItemStatusID").ToString = "1004" Then
                lbStatus.Text = "This is archived"
            ElseIf dt.Rows.Count > 0 AndAlso dt(0)("sku").ToString = "" Then

                If dt(0)("fkInventoryTypeID") = 1003 Or dt(0)("fkInventoryTypeID") = 1004 Or dt(0)("fkInventoryTypeID") = 1005 Or dt(0)("fkInventoryTypeID") = 1006 Or dt(0)("fkInventoryTypeID") = 1007 Or dt(0)("fkInventoryTypeID") = 1008 Or dt(0)("fkInventoryTypeID") = 1009 Or dt(0)("fkInventoryTypeID") = 1011 Then
                    status = 1005
                    consumable = True
                    'UpdateItemInventory(sku, dt(0)("quantity"))
                Else
                    status = 1003
                    consumable = False
                End If

                If dt(0)("itemized").ToString() > 1 Then
                    UpdateOrderItem(1, sku, dt(0)("quantity").ToString, status, "", consumable)
                Else
                    UpdateOrderItem(1, sku, 1, status, "", consumable)
                End If


            ElseIf dt.Rows.Count = 0 Then
                lbStatus.Text = "Not Part of order"

            End If

            RadGrid1.Rebind()


        Else
            sql = "Select * from vwOrderInventoryItems where pkOrderID=" & _pkid & " and sku=" & sku
            dt = ReadRecords(sql)
            If dt.Rows.Count > 0 Then
                If Not dt(0)("fkItemStatusID").ToString = "1003" Then
                    lbStatus.Text = "Already scanned"
                Else

                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowMessage(" & sku & ");", True)
                    'ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "test(" & dt(0)("pkOrderInventoryItemsID").ToString & ");", True)

                    'AddNewItem(dt(0)("pkShowInvID"), inventoryID)
                End If
                'does the item exist
                'Already check in

                'RadGrid2.Rebind()
            Else
                lbStatus.Text = "Not Part of order"
            End If

        End If

    End Sub

    Sub UpdateOrderItem(insert As Boolean, itemID As Integer, qty As Integer, statusID As Integer, notes As String, consumable As Boolean)

        Dim pkid, sql

        If insert Then
            sql = "INSERT INTO [tblOrderInventoryItems] ([fkOrderID],[fkOrderItemsID],[Quantity],[DateOut],[fkItemStatusID],[notes],[fkItemID]) VALUES(@fkOrderID, @fkOrderItemsID, @Quantity, @DateOut, @fkItemStatusID, @notes, @fkItemID); SELECT SCOPE_IDENTITY()"
        Else
            sql = "UPDATE [tblOrderInventoryItems]  SET [fkOrderID]=@fkOrderID, [fkOrderItemsID]=@fkOrderItemsID,[Quantity]=@Quantity,[DateOut]=@DateOut,[DateIn]=@DateIn,[fkItemStatusID]=@fkItemStatusID,[notes]=@notes,[fkItemID]=@fkItemID WHERE pkOrderInventoryItemsID=@pkOrderInventoryItemsID"
        End If


        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = sql

            sqlComm.Parameters.Add(New SqlParameter("fkOrderID", _pkid))
            sqlComm.Parameters.Add(New SqlParameter("Quantity", qty))
            sqlComm.Parameters.Add(New SqlParameter("DateOut", Now()))
            sqlComm.Parameters.Add(New SqlParameter("fkOrderItemsID", 0))
            sqlComm.Parameters.Add(New SqlParameter("fkItemStatusID", statusID))

            sqlComm.Parameters.Add(New SqlParameter("notes", notes))
            sqlComm.Parameters.Add(New SqlParameter("fkItemID", itemID))


            Try
                pkid = sqlComm.ExecuteScalar()
                If consumable Then
                    UpdateItemInventory(itemID, qty)
                End If
            Catch ex As Exception

            End Try
        End Using


    End Sub


    Sub UpdateItemInventory(itemID As Integer, qty As Integer)
        Dim pkid

        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr

            sqlComm.CommandText = "update tblitems set Quantity = Quantity - @qty,[DateModified]=@DateModified where pkItemID=@pkItemID"
            sqlComm.Parameters.Add(New SqlParameter("pkItemID", itemID))
            sqlComm.Parameters.Add(New SqlParameter("qty", qty))
            sqlComm.Parameters.Add(New SqlParameter("DateModified", Now()))

            Try
                pkid = sqlComm.ExecuteScalar()
            Catch ex As Exception

            End Try
        End Using

    End Sub


    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim ddl As DropDownList = TryCast(sender, DropDownList)
        Dim item As GridDataItem = TryCast(ddl.NamingContainer, GridDataItem)
        Dim pkinventoryid = item.GetDataKeyValue("pkInventoryID")

        Dim sku = CInt(ddl.SelectedValue)
        _sku = sku
    End Sub



    'Private Sub AddToOrder(ByVal InventoryID, ByVal InventoryItemsID)
    '    Dim scanned = CheckIfAlreadyScannedOut(InventoryItemsID)
    '    Dim orderItemsID
    '    Dim orderItems = BusinessLogicLayer.OrderItems.GetOrderItemsListBySQLCommand("Select * from tblOrderItems where fkInventoryID = " & InventoryID & " and fkOrderID = " & pkid)

    '    If scanned = False Then
    '        If orderItems Is Nothing Then
    '            Dim orderItem As New BusinessLogicLayer.OrderItems
    '            orderItem.OrderID = pkid
    '            orderItem.InventoryID = InventoryID
    '            orderItem.Quantity = 1
    '            orderItem.Save()
    '            orderItemsID = orderItem.OrderItemsID

    '        Else
    '            orderItems(0).Quantity = orderItems(0).Quantity + 1
    '            orderItems(0).Save()
    '            orderItemsID = orderItems(0).OrderItemsID
    '        End If

    '        Dim orderinventorytype As New BusinessLogicLayer.OrderInventoryItems
    '        orderinventorytype.OrderID = pkid
    '        orderinventorytype.OrderItemsID = orderItemsID
    '        orderinventorytype.Quantity = 1
    '        orderinventorytype.DateOut = Now()
    '        orderinventorytype.InventoryItemStatusID = 1003
    '        orderinventorytype.InventoryItemsID = InventoryItemsID
    '        orderinventorytype.Save()

    '        Dim inventoryitem As BusinessLogicLayer.InventoryItems
    '        inventoryitem = BusinessLogicLayer.InventoryItems.GetInventoryItemsByInventoryItemsID(InventoryItemsID)
    '        inventoryitem.InventoryItemStatusID = 1003
    '        inventoryitem.Save()

    '    End If
    '    Dim order = BusinessLogicLayer.Order.GetOrderByOrderID(pkid)
    '    order.OrderStatusID = 1002
    '    order.Save()

    '    Response.Redirect("Fulfillment.aspx")

    'End Sub

    'Private Sub AddToOrderItemInventory(ByVal InventoryID, ByVal InventoryItemsID)
    '    Dim fkinventoryID = InventoryID

    '    Dim dt = ReadRecords("SELECT tblOrderItems.Quantity, SUM(tblOrderInventoryItems.Quantity) AS Expr1, tblOrderItems.pkOrderItemsID " & _
    '                         "FROM tblOrderItems LEFT OUTER JOIN " & _
    '                         "tblOrderInventoryItems ON tblOrderItems.pkOrderItemsID = tblOrderInventoryItems.fkOrderItemsID " & _
    '                         "WHERE(tblOrderItems.fkOrderID = " & pkid & ") and tblorderitems.fkinventoryid = " & fkinventoryID & " " & _
    '                         "GROUP BY tblOrderItems.Quantity, tblOrderInventoryItems.Quantity, tblOrderItems.pkOrderItemsID")

    '    Dim scanned = CheckIfAlreadyScannedOut(InventoryItemsID)

    '    If scanned = True Then
    '        Response.Write("<h1><center><font color='red'>Already scanned out</font></center></h1>")
    '    Else
    '        If dt.Rows.Count > 0 AndAlso (IsDBNull(dt(0)(1))) Then


    '            Dim inventoryitem As BusinessLogicLayer.InventoryItems
    '            inventoryitem = BusinessLogicLayer.InventoryItems.GetInventoryItemsByInventoryItemsID(InventoryItemsID)

    '            'if there are more then one inventoryitem in the quanity prompt for quantity
    '            If inventoryitem.Quantity > 1 Then
    '                Dim radwin1 As New RadWindow
    '                radwin1.NavigateUrl = "FulFillmentQty.aspx?pkid=" & pkid & "&OrderItemsID=" & dt(0)(2) & "&InventoryItemsID=" & InventoryItemsID
    '                radwin1.VisibleOnPageLoad = True
    '                radwin1.Width = 200
    '                radwin1.Height = 200
    '                Me.form1.Controls.Add(radwin1)

    '            Else

    '                inventoryitem.InventoryItemStatusID = 1003
    '                inventoryitem.Save()

    '                'Set the inventory item as out
    '                Dim orderinventoryitem As New BusinessLogicLayer.OrderInventoryItems
    '                orderinventoryitem.OrderID = pkid
    '                orderinventoryitem.InventoryItemStatusID = 1003
    '                orderinventoryitem.OrderItemsID = dt(0)(2)
    '                orderinventoryitem.Quantity = 1
    '                orderinventoryitem.DateOut = Now()
    '                orderinventoryitem.InventoryItemsID = InventoryItemsID
    '                orderinventoryitem.Save()

    '            End If

    '        ElseIf dt.Rows.Count > 0 AndAlso (dt(0)(1) < dt(0)(0)) Then
    '            Dim orderinventoryitem As New BusinessLogicLayer.OrderInventoryItems
    '            orderinventoryitem.OrderID = pkid
    '            orderinventoryitem.InventoryItemStatusID = 1003
    '            orderinventoryitem.OrderItemsID = dt(0)(2)
    '            orderinventoryitem.Quantity = 1
    '            orderinventoryitem.DateOut = Now()
    '            orderinventoryitem.InventoryItemsID = InventoryItemsID
    '            orderinventoryitem.Save()

    '            'Set the inventory item as out
    '            Dim inventoryitem As BusinessLogicLayer.InventoryItems
    '            inventoryitem = BusinessLogicLayer.InventoryItems.GetInventoryItemsByInventoryItemsID(InventoryItemsID)
    '            inventoryitem.InventoryItemStatusID = 1003
    '            inventoryitem.Save()
    '        ElseIf dt.Rows.Count = 0 Then
    '            Response.Write("<h1><center><font color='red'>Not part of order</font></center></h1>")
    '        End If
    '    End If

    '    Dim orderitems As BusinessLogicLayer.OrderItemsCollection
    '    orderitems = BusinessLogicLayer.OrderItems.GetOrderItemsListByOrderID(pkid)


    '    Dim orderinventoryitems As BusinessLogicLayer.OrderInventoryItemsCollection
    '    orderinventoryitems = BusinessLogicLayer.OrderInventoryItems.GetOrderInventoryItemsListByOrderID(pkid)



    'End Sub

    'Private Sub RecieveIn(ByVal barcode)

    '    Dim scanned = CheckIfAlreadyScannedIn(barcode)

    '    If scanned = False Then

    '        Dim InventoryItem As BusinessLogicLayer.InventoryItemsCollection
    '        InventoryItem = BusinessLogicLayer.InventoryItems.GetInventoryItemsListBySQLCommand("Select * from tblInventoryItems where barcode = '" & barcode & "'")

    '        'if there are more then one inventoryitem in the quanity prompt for quantity
    '        'If InventoryItem(0).Quantity > 1 Then
    '        'Dim radwin1 As New RadWindow
    '        'radwin1.NavigateUrl = "FulFillmentQty.aspx?In=1&InventoryItemsID=" & InventoryItem(0).InventoryItemsID
    '        'radwin1.VisibleOnPageLoad = True
    '        'radwin1.Width = 200
    '        'radwin1.Height = 200
    '        'Me.form1.Controls.Add(radwin1)

    '        'Else
    '        InventoryItem(0).InventoryItemStatusID = 1000
    '        InventoryItem(0).Save()

    '        Dim orderinventoryitem As BusinessLogicLayer.OrderInventoryItemsCollection
    '        orderinventoryitem = BusinessLogicLayer.OrderInventoryItems.GetOrderInventoryItemsListByInventoryItemsID(InventoryItem(0).InventoryItemsID)

    '        orderinventoryitem(0).InventoryItemStatusID = 1000
    '        orderinventoryitem(0).DateIn = Now()
    '        orderinventoryitem(0).Save()
    '        'End If

    '    Else
    '        Response.Write("<h1><center><font color='red'>Already Scanned In</font></center></h1>")
    '    End If


    '    Dim orderitems As BusinessLogicLayer.OrderItemsCollection
    '    orderitems = BusinessLogicLayer.OrderItems.GetOrderItemsListByOrderID(pkid)


    '    Dim orderinventoryitems As BusinessLogicLayer.OrderInventoryItemsCollection
    '    orderinventoryitems = BusinessLogicLayer.OrderInventoryItems.GetOrderInventoryItemsListByOrderID(pkid)



    'End Sub


    Protected Sub RadGrid1_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGrid1.NeedDataSource
        Dim sql = "Select *, Description+' '+Type+' '+Partnum as Name from vwOrderItems where pkOrderID=" & _pkid


        Dim dt = ReadRecords(sql)
        RadGrid1.DataSource = dt
        If dt.Rows.Count > 0 Then
            lbTitle.Text = dt(0)("pkOrderID").ToString() & " - " & dt(0)("EventName").ToString()
        End If

    End Sub


    Protected Sub RadGrid2_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGrid2.NeedDataSource
        Dim sql = "Select *, Description+' '+Type+' '+Partnum as Name from vwOrderInventoryItems where pkorderid = " & _pkid

        Dim dt = ReadRecords(sql)
        RadGrid2.DataSource = dt
        If dt.Rows.Count > 0 Then
            lbTitle.Text = dt(0)("pkOrderID").ToString() & " - " & dt(0)("EventName").ToString()
        End If
    End Sub

    Protected Sub RadGrid2_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles RadGrid2.ItemDataBound
        If _closeout = True Then
            If TypeOf e.Item Is GridDataItem Then


                Dim dataitem As String = DirectCast(e.Item.DataItem, DataRowView)("Status").ToString()

                If dataitem = "Out" AndAlso _status = 1005 Then
                    e.Item.BackColor = Drawing.Color.Red
                    _finished = False
                End If
            End If
        End If
    End Sub

    Protected Sub UpdateItemsStatus(pkItemID, fkStatusID)
        Dim pkid
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr

            sqlComm.CommandText = "update tblitems set [fkItemStatusID]=@fkItemStatusID,[DateModified]=@DateModified where pkItemID=@pkItemID"
            sqlComm.Parameters.Add(New SqlParameter("pkItemID", pkItemID))
            sqlComm.Parameters.Add(New SqlParameter("fkItemStatusID", fkStatusID))
            sqlComm.Parameters.Add(New SqlParameter("DateModified", Now()))

            Try
                    pkid = sqlComm.ExecuteScalar()
            Catch ex As Exception

            End Try
        End Using

    End Sub

    Protected Sub RadGrid_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles RadGrid1.ItemCommand
        Dim pkid

        If e.CommandName = "Add" Then

            Dim item As Telerik.Web.UI.GridDataItem
            item = e.Item

            Dim skuDDL As DropDownList = e.Item.FindControl("skuDDL")
            Dim sku = CInt(skuDDL.SelectedValue)
            Dim itemToRemove As ListItem = skuDDL.Items.FindByValue(skuDDL.SelectedValue)
            skuDDL.Items.Remove(itemToRemove)
            ProcessBarcode(sku)
            'UpdateItemsStatus(sku, 1003)
            RadGrid1.Rebind()
        End If


        If e.CommandName = "Delete" And e.Item.OwnerTableView.Name = "DetailTable" Then
            Dim a As GridDataItem = e.Item
            Dim pkOrderInventoryItemsID = a.GetDataKeyValue("pkOrderInventoryItemsID")
            Dim sku = a.GetDataKeyValue("sku")
            UpdateItemsStatus(sku, 1000)


            Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
                strConnStr.Open()

                Dim sqlComm As New SqlCommand()
                sqlComm.Connection = strConnStr

                sqlComm.CommandText = "Delete from tblorderinventoryitems where pkOrderInventoryItemsID=@pkOrderInventoryItemsID"
                sqlComm.Parameters.Add(New SqlParameter("pkOrderInventoryItemsID", pkOrderInventoryItemsID))

                Try
                    pkid = sqlComm.ExecuteScalar()
                Catch ex As Exception

                End Try
            End Using

            a.OwnerTableView.ClearChildEditItems()
            a.OwnerTableView.Rebind()

            RadGrid1.DataSource = Nothing
            RadGrid1.Rebind()


        Else
            'DisplayMessage(False, "Customer " + e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("CustomerID").ToString() + " deleted")
        End If

        If e.CommandName = "Delete" And e.Item.OwnerTableView.Name = "MasterTable" Then
            Dim a As GridDataItem = e.Item
            Dim pkInventoryID = a.GetDataKeyValue("pkInventoryID")
            'Update this to delete the OrderItem from the pkID

            Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
                strConnStr.Open()

                Dim sqlComm As New SqlCommand()
                sqlComm.Connection = strConnStr

                sqlComm.CommandText = "Delete from tblorderinventoryitems INNER JOIN tblOrderInventoryItems ON vwOrderInventoryItems.pkOrderInventoryItemsID = tblOrderInventoryItems.pkOrderInventoryItemsID where pkInventoryID=@pkInventoryID"
                sqlComm.Parameters.Add(New SqlParameter("pkInventoryID", pkInventoryID))

                Try
                    pkid = sqlComm.ExecuteScalar()
                Catch ex As Exception

                End Try
            End Using


            Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
                strConnStr.Open()

                Dim sqlComm As New SqlCommand()
                sqlComm.Connection = strConnStr

                sqlComm.CommandText = "Delete from tblorderitems where fkInventoryID=@fkInventoryID and fkOrderID=@fkOrderID"
                sqlComm.Parameters.Add(New SqlParameter("fkInventoryID", pkInventoryID))
                sqlComm.Parameters.Add(New SqlParameter("fkOrderID", _pkid))

                Try
                    pkid = sqlComm.ExecuteScalar()
                Catch ex As Exception

                End Try
            End Using

            _deleting = False
            RadGrid1.Rebind()


        End If

    End Sub

    Private Sub RadGrid1_DetailTableDataBind(ByVal source As Object, ByVal e As GridDetailTableDataBindEventArgs) Handles RadGrid1.DetailTableDataBind
        Dim parentItem As GridDataItem = CType(e.DetailTableView.ParentItem, GridDataItem)
        If parentItem.Edit Then
            Return
        End If
        'If (e.DetailTableView.DataMember = "pkInventoryID") Then
        Dim dt = ReadRecords("SELECT * from vwOrderInventoryItems WHERE (pkInventoryID = " & parentItem.GetDataKeyValue("pkInventoryID") & ") and pkOrderID='" & _pkid & "'")
        e.DetailTableView.DataSource = dt
        If CInt(parentItem("Quantity").Text) = GetQuantity(dt) Then
            parentItem.BackColor = Drawing.Color.Green
            parentItem.Enabled = False
            If Not Page.IsPostBack Then
                e.DetailTableView.ParentItem.OwnerTableView.Items(e.DetailTableView.ParentItem.ItemIndex).Expanded = False
            End If


        Else
            If _closeout = True Then
                parentItem.BackColor = Drawing.Color.Red

                _finished = False
            End If

        End If


        'End If
    End Sub

    Protected Sub RadGrid1_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles RadGrid1.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            GetSkuQuantity(e.Item)
        End If
    End Sub

    Function GetSkuQuantity(ByVal item As GridDataItem)
        Try
            Dim count = 0
            Dim quantity = 0
            Dim pkid = item.GetDataKeyValue("pkInventoryID")
            Dim qty = item.GetDataKeyValue("Quantity")
            Dim sku = item.GetDataKeyValue("sku")

            Dim myDD As DropDownList

            myDD = DirectCast(item.FindControl("skuDDL"), DropDownList)

            ''Get the quantity of booths available by summing orders during that time and subtracking them from the total quantity.

            Dim dt = ReadRecords("Select *  FROM tblItems where fkItemStatusID = 1000 and fkInventoryID =" & pkid)

            If dt.Rows.Count > 0 Then
                For Each r In dt.Rows
                    myDD.Items.Add(r("pkItemID").ToString())
                Next
            End If

        Catch ex As Exception
            'ErrorEmail(ex.Message)
        End Try
    End Function

    Private Function GetQuantity(ByVal dt As DataTable) As Integer
        Dim q = 0

        For Each r As DataRow In dt.Rows
            q = q + CInt(r("Quantity"))
        Next


        Return q

    End Function

    Private Function CheckIfAlreadyScannedOut(ByVal inventoryitemsID As String) As Boolean
        Dim check = True

        Dim dt = ReadRecords("Select * tblitems where pkitemid=" & inventoryitemsID & " and fkItemStatusID=1003")
        If dt.Rows.Count = 0 Then
            check = False
        End If


        Return check
    End Function

    Private Function CheckIfAlreadyScannedIn(ByVal barcode As String) As Boolean
        Dim check = True

        Dim dt = ReadRecords("Select * from tblitems where pkitemid=" & barcode & " and fkitemstatusid=1003")
        If dt.Rows.Count > 0 Then
            check = False
        End If

        Return check
    End Function

    Function GetImagePath(ByVal imagePath As String) As String

        Return Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath & "Account/inventory/" & imagePath

    End Function

    Function GetDamagesImagePath(ByVal imagePath) As String
        Dim pics = imagePath.ToString().Split(":")
        Dim imgs = ""

        For Each pic In pics
            If pic <> "" Then
                imgs = imgs & "<img src='" & Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath & "inbound/" & _pkid & "/" & pic & "' height='80' alt='" & pic & "' />"
            End If

        Next


        'ConfigurationSettings.AppSettings("website") & "/Inbound/" & imagePath


        Return imgs

    End Function

    Protected Sub RadGrid1_PreRender(sender As Object, e As EventArgs)
        'If Not Page.IsPostBack Then
        'RadGrid1.MasterTableView.Items(0).Expanded = True
        'RadGrid1.MasterTableView.Items(0).ChildItem.NestedTableViews(0).Items(0).Expanded = True
        If _finished = True Then
            RadGrid1.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None
        End If

        'End If
        If _deleting = True Then
            RadGrid1.MasterTableView.GetColumn("Delete").Display = True
        End If

        'If _sku.HasValue Then
        '    TextBox1.Text = "Hello"
        'End If

    End Sub

    Protected Sub RadGrid2_PreRender(sender As Object, e As EventArgs) Handles RadGrid2.PreRender
        'If Not Page.IsPostBack Then
        'RadGrid1.MasterTableView.Items(0).Expanded = True
        'RadGrid1.MasterTableView.Items(0).ChildItem.NestedTableViews(0).Items(0).Expanded = True
        If _finished = True Then
            RadGrid2.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None
        End If

        'End If
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) Handles LinkButtonOutbound.Click
        _closeout = True
        _finished = True
        RadGrid1.Rebind()
        If _finished = True Then
            UpdateOrderStatus(_pkid, 1005, "CheckOut")
            UpdateItemsInventory(_pkid)



            RadGrid2.Visible = True
            RadGrid2.MasterTableView.Columns(4).Visible = False
            RadGrid2.MasterTableView.Columns(5).Visible = False
            RadGrid2.MasterTableView.Columns(6).Visible = False
            RadGrid2.MasterTableView.Columns(7).Visible = False
            RadGrid2.MasterTableView.Columns(8).Visible = True
            RadGrid2.MasterTableView.Columns(9).Visible = False

            RadGrid2.Rebind()
            RadGrid2.RegisterWithScriptManager = False
            Dim sb As New StringBuilder
            Dim sw As New StringWriter(sb)
            Dim htmltw As New Html32TextWriter(sw)
            RadGrid2.RenderControl(htmltw)

            Dim str As String = sb.ToString
            Dim strstart = "<script "
            Dim strend = "</script>"

            Dim t = sb




            While t.ToString.IndexOf(strstart) > 0

                
                Dim total = (t.ToString.IndexOf(strend) + strend.Length) - t.ToString.IndexOf(strstart)


                t = t.Replace(t.ToString().Substring(t.ToString.IndexOf(strstart), (t.ToString.IndexOf(strend) + strend.Length) - t.ToString.IndexOf(strstart)), "")
            End While

            sb = t
            'SendEmail(ConfigurationSettings.AppSettings("ClientName") & " Order# " & lbTitle.Text, lbTitle.Text & "<br><br>Processed Outbound<br><br>" & sb.ToString(), "Processed Outbound")
            SendEmail(ConfigurationSettings.AppSettings("ClientName") & " Order# " & lbTitle.Text, lbTitle.Text & "<br><br>Processed Outbound<br><br>" & sb.ToString(), ConfigurationSettings.AppSettings("SupportEmail"), "no-reply@pinnacle-exhibits.com", ConfigurationSettings.AppSettings("ClientName") & " Asset Management")

            Response.Redirect("shipping.aspx")
        End If
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As System.EventArgs) Handles LinkButtonInbound.Click
        _closeout = True
        _finished = True
        RadGrid2.Rebind()

        Dim dt = ReadRecords("Select * from vwOrderInventoryItems wher pkOrderID=" & _pkid)

        If dt.Rows.Count > 0 Then
            For Each r In dt.Rows
                If r("fkItemStatusID").ToString = "1003" And Not CInt(r("Quantity").ToString) > 1 Then
                    _finished = False
                End If
            Next
        End If

        


        If _finished = True Then
            UpdateOrderStatus(_pkid, 1004, "CheckIn")
            UpdateItemsInventory(_pkid)

            RadGrid2.MasterTableView.Columns(5).Visible = True
            RadGrid2.MasterTableView.Columns(6).Visible = False

            RadGrid2.Rebind()
            RadGrid2.RegisterWithScriptManager = False
            Dim sb As New StringBuilder
            Dim sw As New StringWriter(sb)
            Dim htmltw As New Html32TextWriter(sw)
            RadGrid2.RenderControl(htmltw)

            Dim str As String = sb.ToString
            Dim strstart = "<script "
            Dim strend = "</script>"

            Dim t = sb




            While t.ToString.IndexOf(strstart) > 0

              
                Dim total = (t.ToString.IndexOf(strend) + strend.Length) - t.ToString.IndexOf(strstart)


                t = t.Replace(t.ToString().Substring(t.ToString.IndexOf(strstart), (t.ToString.IndexOf(strend) + strend.Length) - t.ToString.IndexOf(strstart)), "")
            End While

            sb = t

            'SendEmail(ConfigurationSettings.AppSettings("ClientName") & " Order# " & lbTitle.Text, lbTitle.Text & "<br><br>Processed Inbound<br><br>" & sb.ToString(), "Processed Inbound")
            SendEmail(ConfigurationSettings.AppSettings("ClientName") & " Order# " & lbTitle.Text, lbTitle.Text & "<br><br>Processed Inbound<br><br>" & sb.ToString(), ConfigurationSettings.AppSettings("SupportEmail"), "no-reply@pinnacle-exhibits.com", ConfigurationSettings.AppSettings("ClientName") & " Asset Management")
            Response.Redirect("shipping.aspx")
        End If
    End Sub

    Protected Sub UpdateItemsInventory(orderID)
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr

            sqlComm.CommandText = "update tblItems set tblItems.fkItemStatusID=tblOrderInventoryItems.fkItemStatusID from tblItems inner join tblOrderInventoryItems on tblitems.pkItemID=tblOrderInventoryItems.fkItemID where tblOrderInventoryItems.fkOrderID = @fkOrderID and tblItems.Quantity=1"
            sqlComm.Parameters.Add(New SqlParameter("fkOrderID", orderID))

            Try
                sqlComm.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        End Using

        'decrement quantity from outbound inventory
        'If _status <> 1005 Then

        '    Dim sql = "Select * from vwOrderInventoryItems where pkOrderID = " & orderID & " and OriginalQty>1"
        '    Dim dt = ReadRecords(sql)
        '    If dt.Rows.Count > 0 Then
        '        For Each r In dt.Rows



        '            Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
        '                strConnStr.Open()

        '                Dim sqlComm As New SqlCommand()
        '                sqlComm.Connection = strConnStr

        '                sqlComm.CommandText = "update tblItems set quantity=@quantity where pkItemID = @pkItemID"
        '                sqlComm.Parameters.Add(New SqlParameter("quantity", CInt(r("OriginalQty").ToString()) - CInt(r("Quantity").ToString())))
        '                sqlComm.Parameters.Add(New SqlParameter("pkItemID", r("sku").ToString()))
        '                Try
        '                    sqlComm.ExecuteNonQuery()
        '                Catch ex As Exception

        '                End Try
        '            End Using



        '        Next
        '    End If

        'End If

    End Sub

    Protected Sub UpdateOrderStatus(orderID, statusID, checkstatus)

        Dim pkid
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr

            sqlComm.CommandText = "update tblorders set fkStatusID=@fkStatusID, " & checkstatus & "=@" & checkstatus & " where pkOrderID=@pkOrderID"
            sqlComm.Parameters.Add(New SqlParameter("fkStatusID", statusID))
            sqlComm.Parameters.Add(New SqlParameter(checkstatus, Now()))
            sqlComm.Parameters.Add(New SqlParameter("pkOrderID", orderID))

            Try
                pkid = sqlComm.ExecuteScalar()
            Catch ex As Exception

            End Try
        End Using

    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)

    End Sub

    'Sub SendEmail(ByVal subject, ByVal body, ByVal status)
    '    'RadGrid1.RegisterWithScriptManager = False


    '    Dim fromemail = "no-reply@pinnacle-exhibits.com"
    '    Dim fromemailname = ConfigurationSettings.AppSettings("ClientName") & " Asset Management"
    '    Try


    '        Dim SmtpServer As New SmtpClient(ConfigurationSettings.AppSettings("SMTP"))
    '        SmtpServer.EnableSsl = False
    '        Dim mail As New MailMessage()
    '        mail.To.Add(ConfigurationSettings.AppSettings("SupportEmail"))
    '        mail.Bcc.Add(ConfigurationSettings.AppSettings("ManagementEmail"))
    '        mail.Subject = subject & " " & status
    '        mail.Body = body
    '        mail.From = New MailAddress(fromemail, fromemailname)
    '        mail.IsBodyHtml = True

    '        SmtpServer.Send(mail)

    '    Catch ex As Exception

    '    End Try



    'End Sub

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
            ' SendEmail("Error reading query", ex.Message)
        End Try
        Return dt

    End Function

End Class

Imports Telerik.Web.UI
Imports System.Data.SqlClient
Imports System.IO
Imports System.Data
Imports System.Web.Mail
Imports System.Net.Mail

Partial Class Reports
    Inherits Telerik.Web.UI.RadAjaxPage
    Dim puser As PortalUser
    Dim pkid = ""
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

        'If puser.role <> 1003 Then
        '    RadGrid1.MasterTableView.GetColumn("Delete").Visible = False
        'End If

        If Request.QueryString("inventoryID") <> "" Then
            If puser.role <> 1003 And puser.role <> 1001 Then
                RadGrid1.MasterTableView.GetColumn("ShowStartDateBuffers").Display = False
                RadGrid1.MasterTableView.GetColumn("EventName").Display = False
                RadGrid1.MasterTableView.GetColumn("User").Display = False
                RadGrid1.MasterTableView.GetColumn("CancelOrder").Display = False
                RadGrid1.MasterTableView.GetColumn("deptdesc").Display = False
                RadGrid1.MasterTableView.GetColumn("ViewOrder").Display = False
                RadGrid1.MasterTableView.GetColumn("EditOrder").Display = False
                RadGrid1.MasterTableView.GetColumn("deptdesc").Display = False
            Else
                RadGrid1.MasterTableView.GetColumn("deptdesc").Display = False
            End If



        End If


        Try
            If Not IsPostBack Then
                If Request.QueryString("inventoryID") <> "" Then

                    GridTable.DataSource = ReadRecords("Select * from tblInventory where pkInventoryID = " & Replace(Request.QueryString("inventoryID"), "'", "''"))
                    GridTable.DataBind()
                Else
                    GridTable.Visible = False
                End If


            End If

        Catch ex As Exception

        End Try


    End Sub



    Protected Sub RadGrid1_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGrid1.NeedDataSource
        Dim uid = puser.userID.ToString()
        Dim sql = ""

        Dim company = puser.company
        Dim department = puser.department

        If puser.role = 1003 Then
            uid = "%"
            sql = "Select DATEADD(d, -7, DateArrive) AS StartDateWithBuffers, DATEADD(d, 10, DateEnd) AS EndDateWithBuffers, tblUsers.fkRoleID as role, tblorders.*, DateDiff(Day, GETDATE(), '2030-04-17 00:00:00.000') As DateDiff, tblorderstatus.status, tblDepartment.Description as deptdesc, tblusers.Firstname+' '+tblusers.lastname as [user] from tblOrders inner join tblusers on tblusers.pkuserid=tblorders.fkuserid inner join tblorderstatus on tblorderstatus.pkstatusid = tblorders.fkstatusid inner join tblDepartment on tblDepartment.pkDepartmentID = tblOrders.fkDepartmentID  where fkuserid like '" & uid & "' and tblOrders.fkStatusID not in (1006) order by tblorders.datestart desc"
        End If

        If puser.role = 1001 Then
            uid = "%"
            sql = "Select DATEADD(d, -7, DateArrive) AS StartDateWithBuffers, DATEADD(d, 10, DateEnd) AS EndDateWithBuffers, tblUsers.fkRoleID as role, tblorders.*, DateDiff(Day, GETDATE(), DateArrive) As DateDiff, tblorderstatus.status, tblDepartment.Description as deptdesc, tblusers.Firstname+' '+tblusers.lastname as [user] from tblOrders inner join tblusers on tblusers.pkuserid=tblorders.fkuserid inner join tblorderstatus on tblorderstatus.pkstatusid = tblorders.fkstatusid inner join tblDepartment on tblDepartment.pkDepartmentID = tblOrders.fkDepartmentID  where  fkuserid like '" & uid & "' and tblOrders.fkStatusID not in (1006) order by tblorders.datestart desc"
        End If

        If puser.role = 1000 Or puser.role = 1004 Then
            sql = "Select DATEADD(d, -7, DateArrive) AS StartDateWithBuffers, DATEADD(d, 10, DateEnd) AS EndDateWithBuffers, tblUsers.fkRoleID as role, tblorders.*, DateDiff(Day, GETDATE(), DateArrive) As DateDiff, tblorderstatus.status, tblDepartment.Description as deptdesc, tblusers.Firstname+' '+tblusers.lastname as [user] from tblOrders inner join tblusers on tblusers.pkuserid=tblorders.fkuserid inner join tblorderstatus on tblorderstatus.pkstatusid = tblorders.fkstatusid inner join tblDepartment on tblDepartment.pkDepartmentID = tblOrders.fkDepartmentID where  tblOrders.fkDepartmentID = '" & puser.department & "' and fkuserid like '" & uid & "' and tblOrders.fkStatusID not in (1006) order by tblorders.datestart desc"
        End If

        If puser.role = 1006 Then
            uid = "%"
            sql = "Select DATEADD(d, -7, DateArrive) AS StartDateWithBuffers, DATEADD(d, 10, DateEnd) AS EndDateWithBuffers, tblUsers.fkRoleID as role, tblorders.*, DateDiff(Day, GETDATE(), DateArrive) As DateDiff, tblorderstatus.status, tblDepartment.Description as deptdesc, tblusers.Firstname+' '+tblusers.lastname as [user] from tblOrders inner join tblusers on tblusers.pkuserid=tblorders.fkuserid inner join tblorderstatus on tblorderstatus.pkstatusid = tblorders.fkstatusid inner join tblDepartment on tblDepartment.pkDepartmentID = tblOrders.fkDepartmentID  where  fkuserid like '" & uid & "' and tblOrders.fkStatusID not in (1006) and fkFulfillmentLocationId in ('Japan','China','Australia') order by tblorders.datestart desc"
        End If


        If Request.QueryString("inventoryID") <> "" Then
            sql = "SELECT *, DATEADD(d, -7, DateArrive) AS StartDateWithBuffers, DATEADD(d, 10, DateEnd) AS EndDateWithBuffers, tblUsers.fkRoleID as role, tblusers.Firstname+' '+tblusers.lastname as [user], DateDiff(Day, GETDATE(), DateArrive) As DateDiff, tblorderstatus.status FROM tblOrderItems INNER JOIN tblOrders ON tblOrderItems.fkOrderID = tblOrders.pkOrderID inner join tblusers on tblusers.pkuserid=tblorders.fkuserid inner join tblorderstatus on tblorderstatus.pkstatusid = tblorders.fkstatusid WHERE tblOrders.fkStatusID not in (1006) and (tblOrderItems.fkInventoryID = " & Replace(Request.QueryString("inventoryID"), "'", "''") & ") "

        End If

        RadGrid1.DataSource = ReadRecords(sql)

    End Sub

    Private Sub RadGrid1_DetailTableDataBind(ByVal source As Object, ByVal e As GridDetailTableDataBindEventArgs) Handles RadGrid1.DetailTableDataBind
        Dim parentItem As GridDataItem = CType(e.DetailTableView.ParentItem, GridDataItem)
        If parentItem.Edit Then
            Return
        End If
        Dim t = parentItem("pkOrderID").Text
        ' If (e.DetailTableView.DataMember = "pkOrderID") Then
        e.DetailTableView.DataSource = ReadRecords("Select * FROM tblOrderItems INNER JOIN vwInventoryview ON tblOrderItems.fkInventoryID = vwInventoryview.pkInventoryID INNER JOIN tblOrders ON tblOrderItems.fkOrderID = tblOrders.pkOrderID where fkOrderID =" & parentItem("pkOrderID").Text)
        ' End If
    End Sub

    Protected Sub RadGrid1_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles RadGrid1.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then


            If puser.role <> 1003 And puser.role <> 1001 Then
                RadGrid1.MasterTableView.GetColumn("ShowStartDateBuffers").Display = False
                'RadGrid1.MasterTableView.GetColumn("EventName").Display = False
                'RadGrid1.MasterTableView.GetColumn("User").Display = False
                'RadGrid1.MasterTableView.GetColumn("CancelOrder").Display = False
                'RadGrid1.MasterTableView.GetColumn("deptdesc").Display = False
                'RadGrid1.MasterTableView.GetColumn("ViewOrder").Display = False
                'RadGrid1.MasterTableView.GetColumn("EditOrder").Display = False

            End If

            'RadGrid1.MasterTableView.GetColumn("deptdesc").Display = False



            'Dim linkbutton As LinkButton = e.Item.FindControl("LBEdit")
            'Dim item As GridDataItem = TryCast(e.Item, GridDataItem)
            'Dim pkid = item.GetDataKeyValue("OrderID")
            'Dim order = BusinessLogicLayer.Order.GetOrderByOrderID(pkid)


            'If item("Name").Text = "1005" Or item("Name").Text = "1004" Then
            'linkbutton.CommandName = item("OrderStatus").Text
            'Else
            'linkbutton.Text = GetShowStatus(item("OrderStatus").Text)
            'linkbutton.Enabled = False
            'End If

        End If
    End Sub

    'Protected Sub RadGrid1_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadGrid1.PreRender
    '    If (Not Page.IsPostBack) Then



    '        Dim sql = "Select * from tblOrder where fkOrderStatusID = '1002'"

    '        If Request.QueryString("inventoryID") <> "" Then
    '            sql = "SELECT * FROM tblOrderItems INNER JOIN tblOrder ON tblOrderItems.fkOrderID = tblOrder.pkOrderID WHERE (tblOrderItems.fkInventoryID = " & Replace(Request.QueryString("inventoryID"), "'", "''") & ") AND (tblOrder.fkOrderStatusID = 1001)"

    '        End If

    '        Select Case userinsession.RoleID
    '            Case 1000
    '                sql = sql & ""
    '            Case 1001
    '                sql = sql & ""
    '            Case Else
    '                sql = sql & " and fkUserID = '" & userinsession.UserID & "'"
    '        End Select

    '        sql = sql & " order by ShowStartDate desc"

    '        Dim orders As BusinessLogicLayer.OrderCollection
    '        orders = BusinessLogicLayer.Order.GetOrderListBySQLCommand(sql)
    '        RadGrid1.DataSource = orders
    '        RadGrid1.DataBind()

    '        'RadGrid1.EditIndexes.Add(1)
    '        RadGrid1.Rebind()
    '        '  RadGrid1.MasterTableView.Items(0).Expanded = True
    '    End If
    'End Sub

    Protected Sub RadGrid1_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles RadGrid1.ItemCommand
        'InventoryItems.Visible = False
        'RadGrid2.Visible = False

        If e.CommandName = "Cancel" Then
            Dim item As Telerik.Web.UI.GridDataItem
            item = e.Item

            Dim OrderID = item.GetDataKeyValue("pkOrderID")

            Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
                strConnStr.Open()

                Dim sqlComm As New SqlCommand()
                sqlComm.Connection = strConnStr
                sqlComm.CommandText = "Update tblOrders set fkStatusID = 1006 where pkOrderID=@pkOrderID;Delete from tblOrderItems where fkOrderID=@pkOrderID"
                sqlComm.Parameters.Add(New SqlParameter("pkorderid", OrderID))

                Try
                    pkid = sqlComm.ExecuteScalar()
                    EmailOrder(OrderID, puser.email)
                Catch ex As Exception
                    pkid = "Error: " & ex.Message
                End Try


            End Using

            Response.Redirect("Reports.aspx")
            RadGrid1.DataSource = Nothing
            RadGrid1.Rebind()
        End If


        If e.CommandName = "Delete" Then
            Dim item As Telerik.Web.UI.GridDataItem
            item = e.Item

            Dim OrderID = item.GetDataKeyValue("pkOrderID")

            Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
                strConnStr.Open()

                Dim sqlComm As New SqlCommand()
                sqlComm.Connection = strConnStr
                sqlComm.CommandText = "Delete from tblOrders where pkOrderID=@pkOrderID;Delete from tblOrderItems where fkOrderID=@pkOrderID"
                sqlComm.Parameters.Add(New SqlParameter("pkorderid", OrderID))

                Try
                    pkid = sqlComm.ExecuteScalar()
                Catch ex As Exception
                    pkid = "Error: " & ex.Message
                End Try


            End Using


            'Dim orderitems As BusinessLogicLayer.OrderItemsCollection
            'orderitems = BusinessLogicLayer.OrderItems.GetOrderItemsListByOrderID(OrderID)
            'If Not orderitems Is Nothing Then
            'For Each myitem As BusinessLogicLayer.OrderItems In orderitems
            ' myitem.Delete()
            'Next
            'End If

            'Dim order As BusinessLogicLayer.Order
            'order = BusinessLogicLayer.Order.GetOrderByOrderID(OrderID)
            'If order.OrderStatusID = 1002 Then
            '    order.OrderStatusID = 1006
            '    order.Save()
            '    SendEmail(order.OrderID, order.StatusText)
            'End If




            'Dim MyOrder As BusinessLogicLayer.Order
            'MyOrder = BusinessLogicLayer.Order.GetOrderByOrderID(OrderID)
            'MyOrder.Delete()
            Response.Redirect("Reports.aspx")
            RadGrid1.DataSource = Nothing
            RadGrid1.Rebind()
        End If

        If e.CommandName = "ViewOrder" Then
            Dim item As Telerik.Web.UI.GridDataItem
            item = e.Item

            Dim OrderID = item.GetDataKeyValue("pkOrderID")
            Response.Redirect("Order.aspx?OrderID=" & OrderID)
        End If

        If e.CommandName = "EditOrder" Then
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


    Sub EmailOrder(pkid, toEmail)

        Try
            Dim SmtpServer As New SmtpClient("smtp-legacy.office365.com")
            SmtpServer.Port = 587
            SmtpServer.UseDefaultCredentials = False
            SmtpServer.Credentials = New System.Net.NetworkCredential(ConfigurationManager.AppSettings("o365user"), "awT@(Yg7JCMD]<")
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network
            SmtpServer.EnableSsl = True

            Dim mail As New Net.Mail.MailMessage()
            mail.To.Add(toEmail)
            mail.To.Add(ConfigurationSettings.AppSettings("SupportEmail"))
            If puser.department = 1004 Then
                mail.To.Add(ConfigurationSettings.AppSettings("ArticaWarehouseEmail"))
            End If
            mail.Subject = ConfigurationSettings.AppSettings("ClientName") & " Portable Order# " & pkid & " " & "Cancellation"
            mail.Body = "Your order is now cancelled.<br><br>Best, Your Portables Team"
            mail.From = New MailAddress("PinnacleNoReply@pinnacle-exp.com")
            mail.IsBodyHtml = True

            SmtpServer.Send(mail)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub




    Function GetImagePath(ByVal ImagePath As String) As String

        Return ConfigurationSettings.AppSettings("website") & "/" & ImagePath

    End Function



    Function GetImageLink(status, image) As String
        Dim link = ""
        If Not IsDBNull(image) Then
            link = "<a href='Account/inventory/" & image & "' class='fancybox'>" & status & "</a>"
        Else
            link = status
        End If
        Return link
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


    Private Sub RadGrid1_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadGrid1.Init
        Dim menu As GridFilterMenu = RadGrid1.FilterMenu
        Dim i As Integer = 0
        While i < menu.Items.Count
            If menu.Items(i).Text = "EqualTo" Then
                i = i + 1
            Else
                menu.Items.RemoveAt(i)
            End If
        End While
    End Sub
    'RadGrid1_Init

    Protected Sub RadSearchBox1_Search(sender As Object, e As SearchBoxEventArgs)

        Dim [select] As String = String.Empty
        Dim uid = puser.userID.ToString()

        Dim company = puser.company
        Dim department = puser.department


        If RadSearchBox1.Text IsNot Nothing Then
            'Dim dataItem = DirectCast(e.DataItem, Dictionary(Of String, Object))
            'Dim pkorderId As Integer = CInt(dataItem("pkOrderID").ToString())

            Dim searchText = CInt(RadSearchBox1.Text)

            If puser.role = 1003 Then
                uid = "%"
                [select] = "Select DATEADD(d, -7, DateArrive) AS StartDateWithBuffers, DATEADD(d, 10, DateEnd) AS EndDateWithBuffers, tblUsers.fkRoleID as role, tblorders.*, DateDiff(Day, GETDATE(), '2030-04-17 00:00:00.000') As DateDiff, tblorderstatus.status, tblDepartment.Description as deptdesc, tblusers.Firstname+' '+tblusers.lastname as [user] from tblOrders inner join tblusers on tblusers.pkuserid=tblorders.fkuserid inner join tblorderstatus on tblorderstatus.pkstatusid = tblorders.fkstatusid inner join tblDepartment on tblDepartment.pkDepartmentID = tblOrders.fkDepartmentID  where tblOrders.fkStatusID not in (1006) and pkOrderID like '" & searchText & "' order by tblorders.datestart desc"
            End If

            If puser.role = 1001 Or puser.role = 1006 Then
                uid = "%"
                [select] = "Select DATEADD(d, -7, DateArrive) AS StartDateWithBuffers, DATEADD(d, 10, DateEnd) AS EndDateWithBuffers, tblUsers.fkRoleID as role, tblorders.*, DateDiff(Day, GETDATE(), DateArrive) As DateDiff, tblorderstatus.status, tblDepartment.Description as deptdesc, tblusers.Firstname+' '+tblusers.lastname as [user] from tblOrders inner join tblusers on tblusers.pkuserid=tblorders.fkuserid inner join tblorderstatus on tblorderstatus.pkstatusid = tblorders.fkstatusid inner join tblDepartment on tblDepartment.pkDepartmentID = tblOrders.fkDepartmentID  where tblOrders.fkStatusID not in (1006) and  pkOrderID like  '" & searchText & "' order by tblorders.datestart desc"
            End If

            If puser.role = 1000 Or puser.role = 1004 Then
                [select] = "Select DATEADD(d, -7, DateArrive) AS StartDateWithBuffers, DATEADD(d, 10, DateEnd) AS EndDateWithBuffers, tblUsers.fkRoleID as role, tblorders.*, DateDiff(Day, GETDATE(), DateArrive) As DateDiff, tblorderstatus.status, tblDepartment.Description as deptdesc, tblusers.Firstname+' '+tblusers.lastname as [user] from tblOrders inner join tblusers on tblusers.pkuserid=tblorders.fkuserid inner join tblorderstatus on tblorderstatus.pkstatusid = tblorders.fkstatusid inner join tblDepartment on tblDepartment.pkDepartmentID = tblOrders.fkDepartmentID where tblOrders.fkStatusID not in (1006) and  tblOrders.fkDepartmentID like '" & puser.department & "' and pkOrderID like  '" & searchText & "' order by tblorders.datestart desc"
            End If

            RadGrid1.DataSource = ReadRecords([select])
            RadGrid1.Rebind()

            'Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ConnectionString)
            '    Dim cmd = New SqlCommand([select], con)
            '    cmd.Connection.Open()

            '    'cmd.Parameters.Add(New SqlParameter("sportText", sportText))
            '    'cmd.Parameters.Add(New SqlParameter("countryText", countryText))


            '    Dim sqlReader = cmd.ExecuteReader()
            '    RadGrid1.DataSource = sqlReader
            '    RadGrid1.Rebind()
            '    sqlReader.Close()
            '    cmd.Connection.Close()
            '    cmd.Dispose()
            'End Using
        End If
    End Sub

End Class

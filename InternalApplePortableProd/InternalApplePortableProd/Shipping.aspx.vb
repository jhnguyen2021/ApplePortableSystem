Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Partial Class Shipping
    Inherits Telerik.Web.UI.RadAjaxPage
    Dim puser As PortalUser
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNothing(Session("user")) Then
            Dim ticket = Request.Cookies(FormsAuthentication.FormsCookieName).Value
            Dim dycriptedTicket = FormsAuthentication.Decrypt(ticket)
            If dycriptedTicket.UserData = "" Then
                Response.Redirect("account/login.aspx")
            End If
            Dim t As New sessionmanager(dycriptedTicket.UserData)
        End If

        puser = Session("user")


        If Not IsPostBack Then
            closeShipDate.Visible = False
            lateShipDate.Visible = False
        End If

        'If puser.role = 1004 Then
        '    RadGrid1.MasterTableView.GetColumn("edit").Visible = False
        'End If

        'If Not puser.role = 1003 Then
        '    If Not puser.role = 1001 Then
        '        Response.Redirect("default.aspx")
        '    End If
        'End If
    End Sub


    Protected Sub RadGrid1_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGrid1.NeedDataSource

        Dim search = ""
        If tbsearch.Text <> "" Then
            search = tbsearch.Text
        End If

        Dim sql = String.Empty

        'Select Case puser.role
        '    Case puser.role = 1001 Or puser.role = 1003
        '        sql = "Select * from tblOrders where fkStatusID=1002 and (eventname like '%" & search & "%' or pkorderid like '%" & search & "%') order by datearrive"
        '    Case puser.role = 1004
        '        sql = "SELECT distinct o.* , oi.fkOrderID from tblOrders o join tblOrderItems oi on oi.fkOrderID = o.pkOrderID join tblInventory i on i.pkInventoryID = oi.fkInventoryID where fkStatusID = 1002 and i.fkDepartmentID = 1004 order by o.DateArrive"
        '    Case Else
        '        sql = "Select * from tblOrders where fkStatusID=1002 and (eventname like '%" & search & "%' or pkorderid like '%" & search & "%') and fkDepartmentID = '" & puser.department & "' order by datearrive"
        'End Select


        If puser.role = 1003 Or puser.role = 1001 Then
            sql = "Select * from tblOrders where fkStatusID=1002 and (eventname like '%" & search & "%' or pkorderid like '%" & search & "%') order by datearrive"
        ElseIf puser.role = 1004 Then
            sql = "SELECT distinct o.* , oi.fkOrderID from tblOrders o join tblOrderItems oi on oi.fkOrderID = o.pkOrderID join tblInventory i on i.pkInventoryID = oi.fkInventoryID join tblDepartmentInventory di on di.fkInventoryID = i.pkInventoryID where fkStatusID = 1002 and di.fkDepartmentID = '" & puser.department & "'  order by o.DateArrive"
        ElseIf puser.role = 1005 Then
            sql = "SELECT distinct o.* , oi.fkOrderID from tblOrders o join tblOrderItems oi on oi.fkOrderID = o.pkOrderID join tblInventory i on i.pkInventoryID = oi.fkInventoryID join tblDepartmentInventory di on di.fkInventoryID = i.pkInventoryID where fkStatusID = 1002 and di.fkDepartmentID in (1006,1007,1008)  order by o.DateArrive"

        Else
            sql = "Select * from tblOrders where fkStatusID=1002 and (eventname like '%" & search & "%' or pkorderid like '%" & search & "%') and fkDepartmentID = '" & puser.department & "' order by datearrive"
        End If

        RadGrid1.DataSource = ReadRecords(sql)
    End Sub

    Protected Sub RadGrid2_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGrid2.NeedDataSource

        Dim search = ""
        If tbsearch.Text <> "" Then
            search = tbsearch.Text
        End If

        Dim sql = String.Empty

        If puser.role = 1003 Or puser.role = 1001 Then
            sql = "Select * from tblOrders where fkStatusID=1005 and (eventname like '%" & search & "%' or pkorderid like '%" & search & "%') order by datepickup"
        ElseIf puser.role = 1004 Then
            sql = "SELECT distinct o.* , oi.fkOrderID from tblOrders o join tblOrderItems oi on oi.fkOrderID = o.pkOrderID join tblInventory i on i.pkInventoryID = oi.fkInventoryID where fkStatusID = 1005 and i.fkDepartmentID = '" & puser.department & "' and (eventname like '%" & search & "%' or pkorderid like '%" & search & "%') order by o.DateArrive"
        ElseIf puser.role = 1005 Then
            sql = "SELECT distinct o.* , oi.fkOrderID from tblOrders o join tblOrderItems oi on oi.fkOrderID = o.pkOrderID join tblInventory i on i.pkInventoryID = oi.fkInventoryID where fkStatusID = 1005 and i.fkDepartmentID in (1006,1007,1008) and (eventname like '%" & search & "%' or pkorderid like '%" & search & "%') order by o.DateArrive"
        Else
            sql = "Select * from tblOrders where fkStatusID=1005 and (eventname like '%" & search & "%' or pkorderid like '%" & search & "%') and fkDepartmentID = '" & puser.department & "' order by datepickup"
        End If


        RadGrid2.DataSource = ReadRecords(sql)
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

    Protected Sub tbsearch_TextChanged(sender As Object, e As EventArgs) Handles tbsearch.TextChanged
        RadGrid1.Rebind()
        RadGrid2.Rebind()
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        tbsearch.Text = ""
        RadGrid1.Rebind()
        RadGrid2.Rebind()
    End Sub


    Protected Sub RadGrid1_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs)
        'Dim pkid As Integer

        If TypeOf e.Item Is GridDataItem Then

            Dim item As Telerik.Web.UI.GridDataItem
            item = e.Item
            getOutboundStatus(item)
            Dim img As System.Web.UI.WebControls.Image = CType(e.Item.FindControl("Image1"), System.Web.UI.WebControls.Image)
        End If

        'If TypeOf e.Item Is GridEditableItem Then
        '    Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)
        '    'upload.ControlObjectsVisibility = ControlObjectsVisibility.None
        '    'RadAjaxPanel1.ResponseScripts.Add(String.Format("window['UploadId'] = '{0}';", upload.ClientID))

        '    Try
        '        Dim dt = ReadRecords("Select fkDepartmentID from tblDepartmentInventory where fkInventoryID=" & editItem.GetDataKeyValue("pkInventoryID"))
        '        If dt.Rows.Count > 0 Then
        '            For Each r In dt.Rows
        '                Dim combo = TryCast(editItem.FindControl("RadComboBox1"), RadComboBox)
        '                For Each item As RadComboBoxItem In combo.Items
        '                    If item.Value = r(0) Then
        '                        item.Checked = True
        '                    End If
        '                Next
        '            Next
        '        End If
        '    Catch ex As Exception

        '    End Try
        'End If

    End Sub


    Function getOutboundStatus(ByVal item As GridDataItem) As String
        Dim itemStatus As String = "No changes"
        Dim img As System.Web.UI.WebControls.Image = CType(item.FindControl("Image1"), System.Web.UI.WebControls.Image)
        Dim pkorderId = item.GetDataKeyValue("pkOrderID")
        Try
            Dim dt = ReadRecords("Select c.LeadTime, DATEDIFF(day, GETDATE(), DATEADD(DAY, -c.LeadTime, o.DateArrive)) AS remainShipDays from tblOrders o join tblShipCountry c on o.ShipCountry = c.ShipCountryName where o.fkFulfillmentLocationId = c.PrimaryShipFromLocation and o.pkOrderID =" & item.GetDataKeyValue("pkOrderID"))
            If dt.Rows.Count > 0 Then
                Dim remainShipDays = dt(0)("remainShipDays")
                If remainShipDays > 0 And remainShipDays < 10 Then
                    img.ImageUrl = "https://assets.yourpinnacle.net/CL1001/WarningTriangleYellow.png"
                    closeShipDate.Visible = True
                    spanText.InnerText = "Based on fulfillment location Lead time you have less than " & remainShipDays & " days to ship items"
                ElseIf remainShipDays <= 0 Then
                    img.ImageUrl = "https://assets.yourpinnacle.net/CL1001/WarningHexagonRed.png"
                    lateShipDate.Visible = True
                End If
            End If

        Catch ex As Exception
        End Try

        Return itemStatus

    End Function
End Class

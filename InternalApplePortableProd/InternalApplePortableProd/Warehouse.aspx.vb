Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.IO

Partial Class Warehouse
    Inherits Telerik.Web.UI.RadAjaxPage
    Private isExport As Boolean = False
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

        'If Not puser.department = 1001 Or Not puser.role = 1003 Then
        '    Response.Redirect("default.aspx")
        'End If


        Try
            If Not IsPostBack Then

                If puser.role <> 1003 Then
                    RadGrid1.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = False
                    RadGrid1.MasterTableView.Columns(0).Visible = False
                    RadGrid1.MasterTableView.Columns(1).Visible = False
                    LinkButton1.Visible = False
                End If

                'Select Case userinsession.RoleID
                '    Case 1000 '

                '    Case 1001 '
                '        RadGrid1.Columns.Item(0).Visible = False
                '        RadGrid1.Columns.Item(6).Visible = False
                '        RadGrid1.Columns.Item(7).Visible = False
                '        RadGrid1.Columns.Item(10).Visible = False
                '        RadGrid1.Columns.Item(11).Visible = False
                '        RadGrid1.Columns.Item(12).Visible = False
                '        RadGrid1.Columns.Item(13).Visible = False
                '        ' RadGrid1.Columns.Item(14).Visible = False
                '        'RadGrid1.Columns.Item(15).Visible = False
                '        RadGrid1.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = False

                '    Case Else   'User
                '        RadGrid1.Columns.Item(0).Visible = False
                '        RadGrid1.Columns.Item(6).Visible = False
                '        RadGrid1.Columns.Item(7).Visible = False
                '        RadGrid1.Columns.Item(10).Visible = False
                '        RadGrid1.Columns.Item(11).Visible = False
                '        RadGrid1.Columns.Item(12).Visible = False
                '        RadGrid1.Columns.Item(13).Visible = False
                '        ' RadGrid1.Columns.Item(14).Visible = False
                '        'RadGrid1.Columns.Item(15).Visible = False
                '        RadGrid1.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = False

                'End Select
            End If

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub RadGrid1_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs)
        If TypeOf e.Item Is GridEditableItem AndAlso e.Item.IsInEditMode Then
            Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)


            Dim quantity As RadNumericTextBox = e.Item.FindControl("tbQuantity")
            Dim validator As RequiredFieldValidator = New RequiredFieldValidator()
            validator.ErrorMessage = "*"
            validator.ForeColor = Drawing.Color.Red
            validator.Display = ValidatorDisplay.Dynamic
            validator.ControlToValidate = "tbQuantity"
            DirectCast(quantity.Parent, TableCell).Controls.Add(validator)

            Dim brand As RadComboBox = e.Item.FindControl("ccInventory")
            brand.DataSource = ReadRecords("SELECT tblInventory.pkInventoryID, tblInventory.PartNum + '-' + tblInventoryType.Description + '-' +tblInventory.Description AS Name, tblInventoryType.pkInventoryTypeID FROM tblInventory INNER JOIN " &
              "tblInventoryType ON tblInventory.fkInventoryTypeID = tblInventoryType.pkInventoryTypeID WHERE tblInventory.isDeleted = 0 ORDER BY Name")
            brand.DataTextField = "Name"
            brand.DataValueField = "pkInventoryID"
            brand.DataBind()

            Dim statusbox As RadComboBox = e.Item.FindControl("ccStatus")
            statusbox.DataSource = ReadRecords("Select * from tblItemStatus")
            statusbox.DataTextField = "Status"
            statusbox.DataValueField = "pkItemStatusID"
            statusbox.DataBind()

            '  If Not IsPostBack Then
            Dim editedItem As GridEditFormItem = CType(e.Item, GridEditFormItem)
            If Not editedItem.ParentItem Is Nothing Then

                brand.SelectedValue = editedItem.ParentItem.GetDataKeyValue("fkInventoryID").ToString()
                statusbox.SelectedValue = editedItem.ParentItem.GetDataKeyValue("fkItemStatusID").ToString()
                quantity.Text = editedItem.ParentItem.GetDataKeyValue("quantity").ToString()
            Else
                If Not IsNothing(Session("catagorySelection")) Then
                    'brand.SelectedValue = Session("catagorySelection")
                End If

            End If

        End If

        'PDF formatting

        If TypeOf e.Item Is GridHeaderItem AndAlso isExport Then

            e.Item.Style("font-size") = "8pt"
            e.Item.Style("color") = "white"
            e.Item.Style("background-color") = "#4b6c9e"
            e.Item.Style("height") = "20px"
            e.Item.Style("vertical-align") = "left"



        End If

        If TypeOf e.Item Is GridItem AndAlso isExport Then
            For Each cell As TableCell In e.Item.Cells
                cell.Style("font-size") = "8pt"
                cell.Style("text-align") = "left"
                cell.Style("font-weight") = "bold"

            Next
        End If

    End Sub

    Protected Sub RadGrid1_InsertCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs)
        InsertUpdate("insert", e)
    End Sub

    Protected Sub RadGrid1_UpdateCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs)
        InsertUpdate("update", e)
    End Sub

    Sub InsertUpdate(cmd As String, ByVal e As GridCommandEventArgs)
        Dim pkid
        Dim editedItem As GridEditableItem = CType(e.Item, GridEditableItem)
        Dim newValues As Hashtable = New Hashtable
        e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem)
        'Dim upload As RadUpload = e.Item.FindControl("RadUpload1")
        'Randomize()
        'Dim r As New Random(System.DateTime.Now.Millisecond)

        'Dim ImageName = Split(Upload.UploadedFiles.Item(0).FileName, "\")
        'Dim FileImage = r.Next(1, 999999) & ImageName(ImageName.Length - 1)
        'Dim targetFolder As String = Server.MapPath("inventory/")
        'For Each validFile As UploadedFile In Upload.UploadedFiles
        'validFile.SaveAs(targetFolder & FileImage, True)

        'Next

        Dim brand As RadComboBox = e.Item.FindControl("ccInventory")
        Dim statusbox As RadComboBox = e.Item.FindControl("ccStatus")
        Dim qty As RadNumericTextBox = e.Item.FindControl("tbQuantity")
        Dim itemized As RadNumericTextBox = e.Item.FindControl("tbQuantity")
        Dim cbitemized As CheckBox = e.Item.FindControl("cbItemized")
        'Dim pkItemId = editedItem.GetDataKeyValue("pkItemID").ToString()

        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr

            If cmd = "insert" Then
                sqlComm.CommandText = "Insert into tblitems ([Quantity],[fkItemStatusID],[DateAdded],[DateModified],[fkCompanyID],[fkInventoryID],[Notes],[UDF01]) VALUES " &
                        "(@Quantity,@fkItemStatusID,@DateAdded,@DateModified,@fkCompanyID,@fkInventoryID,@Notes,@UDF01 ); SELECT SCOPE_IDENTITY()"
            Else
                sqlComm.CommandText = "update tblitems set [Quantity]=@Quantity,[fkItemStatusID]=@fkItemStatusID,[DateModified]=@DateModified, " &
                   "[fkCompanyID]=@fkCompanyID,[fkInventoryID]=@fkInventoryID,[Notes]=@Notes,[UDF01]=@UDF01 where pkItemID=@pkItemID"
                Dim pkItemId = editedItem.GetDataKeyValue("pkItemID").ToString()
                sqlComm.Parameters.Add(New SqlParameter("pkItemID", pkItemId))
                'sqlComm.Parameters.Add(New SqlParameter("quantity", CInt(qty.Value)))
            End If

            sqlComm.Parameters.Add(New SqlParameter("fkItemStatusID", statusbox.SelectedValue))
            sqlComm.Parameters.Add(New SqlParameter("DateAdded", Now()))
            sqlComm.Parameters.Add(New SqlParameter("DateModified", Now()))
            sqlComm.Parameters.Add(New SqlParameter("fkCompanyID", 1000))
            sqlComm.Parameters.Add(New SqlParameter("fkInventoryID", brand.SelectedValue))
            sqlComm.Parameters.Add(New SqlParameter("Notes", ConvertNothing(newValues("Notes"))))
            sqlComm.Parameters.Add(New SqlParameter("UDF01", 0))



            Try
                If cmd = "insert" And cbitemized.Checked Then
                    sqlComm.Parameters.Add(New SqlParameter("quantity", 1))
                    For a = 1 To qty.Value
                        pkid = sqlComm.ExecuteScalar()
                    Next
                ElseIf cmd = "update" And cbitemized.Checked Then
                    sqlComm.CommandText = "delete from dbo.tblItems where pkItemID= @pkItemID"
                    pkid = sqlComm.ExecuteScalar()

                    sqlComm.CommandText = "Insert into tblitems ([Quantity],[fkItemStatusID],[DateAdded],[DateModified],[fkCompanyID],[fkInventoryID],[Notes],[UDF01]) VALUES " &
                        "(@quantityUpdate,@fkItemStatusID,@DateAdded,@DateModified,@fkCompanyID,@fkInventoryID,@Notes,@UDF01 ); SELECT SCOPE_IDENTITY()"
                    sqlComm.Parameters.Add(New SqlParameter("quantityUpdate", 1))
                    For a = 1 To qty.Value
                        pkid = sqlComm.ExecuteScalar()
                    Next
                Else
                    sqlComm.Parameters.Add(New SqlParameter("quantity", CInt(qty.Value)))
                    pkid = sqlComm.ExecuteScalar()
                End If
            Catch ex As Exception
            End Try
        End Using
        e.Item.Edit = False
        e.Item.OwnerTableView.IsItemInserted = False
        e.Canceled = True
        RadGrid1.Rebind()
    End Sub


    Function ConvertNothing(ByRef val As String) As String
        If val Is Nothing Then
            Return ""
        Else
            Return val
        End If

    End Function

    Function ConvertFileImage(ByVal val As String) As String
        val = Replace(val, "'", "")
        val = Replace(val, "(", "")
        val = Replace(val, ")", "")
        Return val


    End Function

    Protected Sub RadGrid1_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles RadGrid1.ItemCommand
        If e.CommandName = Telerik.Web.UI.RadGrid.ExportToExcelCommandName OrElse e.CommandName = Telerik.Web.UI.RadGrid.ExportToWordCommandName OrElse e.CommandName = Telerik.Web.UI.RadGrid.ExportToCsvCommandName OrElse e.CommandName = Telerik.Web.UI.RadGrid.ExportToPdfCommandName Then
            ConfigureExport(e.CommandName)
        End If
    End Sub

    Sub AddValidator(ByVal editItem As GridEditableItem, ByVal ControlName As String)
        Dim validatorName As RequiredFieldValidator = New RequiredFieldValidator()
        validatorName.ErrorMessage = "*"
        validatorName.ForeColor = Drawing.Color.Red
        validatorName.Display = ValidatorDisplay.Dynamic
        Dim cellName As TableCell

        If TypeOf editItem.EditManager.GetColumnEditor(ControlName) Is GridTextBoxColumnEditor Then
            Dim editorName As GridTextBoxColumnEditor = editItem.EditManager.GetColumnEditor(ControlName)
            cellName = editorName.TextBoxControl.Parent
            validatorName.ControlToValidate = editorName.TextBoxControl.ID

        Else
            Dim editorName As GridNumericColumnEditor = editItem.EditManager.GetColumnEditor(ControlName)
            cellName = editorName.NumericTextBox.Parent
            validatorName.ControlToValidate = editorName.NumericTextBox.ID

        End If


        cellName.Controls.Add(validatorName)

    End Sub

    Protected Sub RadGrid1_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGrid1.NeedDataSource

        Dim sql = String.Empty

        If puser.role = 1003 Or puser.role = 1001 Then
            sql = "Select CONVERT(VARCHAR(10), t.DateOut, 101) as dateOut, t.fkorderID, tblItems.*,tblInventory.PartNum + '-' + tblInventoryType.Description + '-' +tblInventory.Description AS Name, tblItemStatus.Status, tblItems.fkInventoryID, tblInventoryType.pkInventoryTypeID, tblInventory.fkDepartmentID, ISNULL(dbo.tblLocation.Location, 'United States') AS Location FROM tblItems INNER JOIN " &
             "tblItemStatus ON tblItemStatus.pkItemStatusID = tblItems.fkItemStatusID INNER JOIN " &
             "tblInventory ON tblItems.fkInventoryID = tblInventory.pkInventoryID INNER JOIN " &
             "tblLocation on tblLocation.fkdepartmentid = tblinventory.fkDepartmentID INNER JOIN " &
             "tblInventoryType ON tblInventory.fkInventoryTypeID = tblInventoryType.pkInventoryTypeID " &
         "left outer join (select fkOrderID, fkItemID, DateOut, rank() over (partition by fkItemID order by DateOut desc) as rank from tblOrderInventoryItems) as t on t.fkItemID=tblItems.pkItemID  and t.rank=1" &
        "where tblInventory.isDeleted = 0 and fkItemStatusID not in (1004) order by Name"
        ElseIf puser.role = 1005 Then
            sql = "Select CONVERT(VARCHAR(10), t.DateOut, 101) as dateOut, t.fkorderID, tblItems.*,tblInventory.PartNum + '-' + tblInventoryType.Description + '-' +tblInventory.Description AS Name, tblItemStatus.Status, tblItems.fkInventoryID, tblInventoryType.pkInventoryTypeID, tblInventory.fkDepartmentID, ISNULL(dbo.tblLocation.Location, 'United States') AS Location FROM tblItems INNER JOIN " &
            "tblItemStatus ON tblItemStatus.pkItemStatusID = tblItems.fkItemStatusID INNER JOIN " &
            "tblInventory ON tblItems.fkInventoryID = tblInventory.pkInventoryID INNER JOIN " &
             "tblDepartmentInventory on tblDepartmentInventory.fkInventoryID = tblInventory.pkInventoryID INNER JOIN " &
             "tblLocation ON tblLocation.fkdepartmentid = tblDepartmentInventory.fkdepartmentid INNER JOIN " &
            "tblInventoryType ON tblInventory.fkInventoryTypeID = tblInventoryType.pkInventoryTypeID " &
       " left outer join (SELECT i.fkOrderID, i.DateOut, i.fkItemID FROM dbo.tblOrderInventoryItems AS i INNER JOIN  (SELECT MAX(DateOut) AS DateOut, fkItemID  FROM dbo.tblOrderInventoryItems GROUP BY fkItemID) AS d ON d.DateOut = i.DateOut " &
       ") as t on t.fkItemID=tblItems.pkItemID where tblDepartmentInventory.fkDepartmentID in (1006,1007,1008)" &
       "and tblInventory.isDeleted = 0 and fkItemStatusID not in (1004) order by Name"
        Else
            sql = "Select CONVERT(VARCHAR(10), t.DateOut, 101) as dateOut, t.fkorderID, tblItems.*,tblInventory.PartNum + '-' + tblInventoryType.Description + '-' +tblInventory.Description AS Name, tblItemStatus.Status, tblItems.fkInventoryID, tblInventoryType.pkInventoryTypeID, tblInventory.fkDepartmentID, ISNULL(dbo.tblLocation.Location, 'United States') AS Location FROM tblItems INNER JOIN " &
             "tblItemStatus ON tblItemStatus.pkItemStatusID = tblItems.fkItemStatusID INNER JOIN " &
             "tblInventory ON tblItems.fkInventoryID = tblInventory.pkInventoryID INNER JOIN " &
              "tblDepartmentInventory on tblDepartmentInventory.fkInventoryID = tblInventory.pkInventoryID INNER JOIN " &
              "tblLocation ON tblLocation.fkdepartmentid = tblDepartmentInventory.fkdepartmentid INNER JOIN " &
             "tblInventoryType ON tblInventory.fkInventoryTypeID = tblInventoryType.pkInventoryTypeID " &
        " left outer join (SELECT i.fkOrderID, i.DateOut, i.fkItemID FROM dbo.tblOrderInventoryItems AS i INNER JOIN  (SELECT MAX(DateOut) AS DateOut, fkItemID  FROM dbo.tblOrderInventoryItems GROUP BY fkItemID) AS d ON d.DateOut = i.DateOut " &
        ") as t on t.fkItemID=tblItems.pkItemID where tblDepartmentInventory.fkDepartmentID = '" & puser.department & "'" &
        "and tblInventory.isDeleted = 0 and fkItemStatusID not in (1004) order by Name"
        End If

        RadGrid1.DataSource = ReadRecords(sql)
    End Sub

    Function GetInventoryName(ByVal id As String) As String
        Dim e = ""
        Dim dt = ReadRecords("SELECT * FROM tblInventory WHERE (pkInventoryID = " & id & " )")
        If dt.Rows.Count > 0 Then
            e = dt(0)("Description")
        End If

        Return e
    End Function

    Function getLastUsed(id) As String
        Dim e = ""
        Try


            Dim dt = ReadRecords("SELECT * FROM tblOrders WHERE pkOrderID= " & id & " ")
           If dt.Rows.Count > 0 Then
                e = "<a href='Order.aspx?OrderID=" & id & "'>" & CDate(dt(0)("DateStart")).Date & "</a>"
            End If
        Catch ex As Exception

        End Try
        Return e
    End Function

    Public Sub ConfigureExport(ByVal command As String)
        '  RadGrid1.ExportSettings.ExportOnlyData = True
        RadGrid1.ExportSettings.IgnorePaging = True
        RadGrid1.ExportSettings.OpenInNewWindow = True
        Select Case command
            Case Telerik.Web.UI.RadGrid.ExportToExcelCommandName
                RadGrid1.MasterTableView.ExportToExcel()
            Case Telerik.Web.UI.RadGrid.ExportToWordCommandName
                RadGrid1.MasterTableView.ExportToWord()
            Case Telerik.Web.UI.RadGrid.ExportToCsvCommandName
                RadGrid1.MasterTableView.ExportToCSV()
            Case Else
                'RadGrid1.ExportSettings.ExportOnlyData = True
                isExport = True

                RadGrid1.Columns.Item(0).Visible = False    'edit button
                '   RadGrid1.Columns.Item(9).Visible = False    '
                RadGrid1.Columns.Item(11).Visible = False   'Deleted
                RadGrid1.Columns.Item(18).Visible = False   'Events
                RadGrid1.MasterTableView.ExportToPdf()
        End Select



        'RadGrid1.MasterTableView.ExportToExcel()
        '  RadGrid1.MasterTableView.ExportToWord()
        '  RadGrid1.MasterTableView.ExportToCSV()
        'RadGrid1.MasterTableView.ExportToPdf()

    End Sub
    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)

        TryCast(TryCast(sender, CheckBox).NamingContainer, GridItem).Selected = TryCast(sender, CheckBox).Checked
        Dim checkHeader As Boolean = True
        For Each dataItem As GridDataItem In RadGrid1.MasterTableView.Items
            If Not TryCast(dataItem.FindControl("CheckBox1"), CheckBox).Checked Then
                checkHeader = False
                Exit For
            End If
        Next
        Dim headerItem As GridHeaderItem = TryCast(RadGrid1.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        TryCast(headerItem.FindControl("headerChkbox"), CheckBox).Checked = checkHeader
    End Sub

    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = TryCast(sender, CheckBox)
        For Each dataItem As GridDataItem In RadGrid1.MasterTableView.Items
            TryCast(dataItem.FindControl("CheckBox1"), CheckBox).Checked = headerCheckBox.Checked
            dataItem.Selected = headerCheckBox.Checked
        Next
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Dim l As New ArrayList

        For Each item As GridDataItem In RadGrid1.SelectedItems
            'Response.Write(item.GetDataKeyValue("partnum"))
            l.Add("'" & item.GetDataKeyValue("pkItemID") & "'")

        Next

        Session("labels") = String.Join(",", l.ToArray())

        ScriptManager.RegisterClientScriptBlock(Me, Page.GetType, "label", "openURL('labels.aspx?cid=" & Replace(Request.QueryString("cid"), "'", "''") & "')", True)
        'Response.Write("<script>window.open ('label.aspx?cid=" & Replace(Request.QueryString("cid"), "'", "''") & "','_blank');</script>")

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


End Class

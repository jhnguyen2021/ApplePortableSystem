
Imports Telerik.Web.UI
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Imaging

Partial Class Exhibits
    Inherits Telerik.Web.UI.RadAjaxPage
    Private isExport As Boolean = False
    Dim puser As PortalUser
    Protected Sub RadScriptManager1_AsyncPostBackError(ByVal sender As Object, ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs) Handles RadScriptManager1.AsyncPostBackError

        RadScriptManager1.AsyncPostBackErrorMessage = e.Exception.Message

    End Sub

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


        Try
            If Not IsPostBack Then

                If puser.role = 1000 Then
                    RadGrid1.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = False
                    RadGrid1.MasterTableView.Columns(0).Visible = False
                    RadGrid1.MasterTableView.Columns(5).Visible = False
                    RadGrid1.MasterTableView.Columns(6).Visible = False
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

    Protected Sub RadGrid1_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGrid1.NeedDataSource
        Dim sql = "SELECT * from vwInventoryView WHERE isDeleted = 0  and deptdesc = 'Retail' ORDER BY type"


        'Select Case userinsession.RoleID
        '    Case 1000 'matrexadmin
        '        'inventory = BusinessLogicLayer.Inventory.GetInventoryListBySQLCommand("Select * from tblInventory where isDeleted = 0 order by PartNumber asc")
        '        inventory = BusinessLogicLayer.InventoryView.GetAllInventoryView()
        '    Case 1001 'BMOAdmin
        '        'inventory = BusinessLogicLayer.Inventory.GetInventoryListBySQLCommand("Select * from tblInventory where isDeleted = 0 order by PartNumber asc")
        '        inventory = BusinessLogicLayer.InventoryView.GetAllInventoryViewNotDeleted()

        '    Case Else   'User
        '        'inventory = BusinessLogicLayer.Inventory.GetInventoryListBySQLCommand("Select * from tblInventory where isDeleted = 0 order by PartNumber asc")

        '        inventory = BusinessLogicLayer.InventoryView.GetAllInventoryViewNotDeleted()


        'End Select

        RadGrid1.DataSource = ReadRecords(sql)
    End Sub

    Protected Sub RadGrid1_PreRender(ByVal source As Object, ByVal e As EventArgs) Handles RadGrid1.PreRender
        For Each grpHdrItem As GridGroupHeaderItem In RadGrid1.MasterTableView.GetItems(GridItemType.GroupHeader)

            If grpHdrItem.DataCell.Text.Contains("Brand") Then
                '     Dim test = grpHdrItem.DataCell.Text.Substring(12)
                grpHdrItem.DataCell.Text = getBrand(grpHdrItem.DataCell.Text.Substring(7))
            ElseIf grpHdrItem.DataCell.Text.Contains("Type") Then
                '     Dim test = grpHdrItem.DataCell.Text.Substring(12)
                grpHdrItem.DataCell.Text = getInventoryType(grpHdrItem.DataCell.Text.Substring(4))

            End If
            If Not Page.IsPostBack Then
                grpHdrItem.Expanded = False
            End If

        Next

        '  For Each grpField As GridGroupByField In GridPremiums.MasterTableView.GetItems(gridgr
        '  Next

    End Sub


    Protected Sub RadGrid1_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs)
        If TypeOf e.Item Is GridEditableItem AndAlso e.Item.IsInEditMode Then
            Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)
            Dim upload As RadAsyncUpload = e.Item.FindControl("RadUpload1")
            'upload.ControlObjectsVisibility = ControlObjectsVisibility.None
            'RadAjaxPanel1.ResponseScripts.Add(String.Format("window['UploadId'] = '{0}';", upload.ClientID))
        End If
    End Sub

    Protected Sub RadGrid1_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs)
        If TypeOf e.Item Is GridEditableItem AndAlso e.Item.IsInEditMode Then
            Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)
            'Dim upload As RadUpload = e.Item.FindControl("RadUpload1")
            'upload.ControlObjectsVisibility = ControlObjectsVisibility.None
            'Dim validator As CustomValidator = New CustomValidator()
            'validator.ErrorMessage = "Please select file to be uploaded"
            'validator.ClientValidationFunction = "validateRadUpload"
            'validator.Display = ValidatorDisplay.Dynamic
            'DirectCast(upload.Parent, TableCell).Controls.Add(validator)


            Dim brand As RadComboBox = e.Item.FindControl("ccInventory")
            brand.DataSource = ReadRecords("SELECT * from tblInventoryType ORDER BY Description")
            brand.DataTextField = "Description"
            brand.DataValueField = "pkInventoryTypeID"
            brand.DataBind()



            '  If Not IsPostBack Then
            Dim editedItem As GridEditFormItem = CType(e.Item, GridEditFormItem)
            If Not editedItem.ParentItem Is Nothing Then

                brand.SelectedValue = editedItem.ParentItem.GetDataKeyValue("fkInventoryTypeID").ToString()
                'Else
                'If Not IsNothing(Session("catagorySelection")) Then
                'brand.SelectedValue = Session("catagorySelection")
                'End If

            End If

            'Dim editName As RadTextBox = e.Item.FindControl("Name")
            'Dim editorName As GridTextBoxColumnEditor = editItem.EditManager.GetColumnEditor("Name")
            'Dim cellName As TableCell = editorName.TextBoxControl.Parent
            'Dim validatorName As RequiredFieldValidator = New RequiredFieldValidator()
            'validatorName.ErrorMessage = "*"
            'validatorName.ForeColor = Drawing.Color.Red
            'validatorName.ControlToValidate = editorName.TextBoxControl.ID
            'validatorName.Display = ValidatorDisplay.Dynamic
            'cellName.Controls.Add(validatorName)

            'AddValidator(editItem, "Name")
            AddValidator(editItem, "Description")
            AddValidator(editItem, "PartNum")
            'AddValidator(editItem, "Quantity")
            'AddValidator(editItem, "Skid")
            'AddValidator(editItem, "ItemNotes")
            'AddValidator(editItem, "Weight")
            'AddValidator(editItem, "Cost")
            'AddValidator(editItem, "Rebate")

            'Dim brand As RadComboBox = e.Item.FindControl("ccBrand")
            'brand.Visible = False
            'Dim department As New BusinessLogicLayer.DepartmentCollection
            'department = BusinessLogicLayer.Department.GetAllDepartment

            'brand.DataSource = department
            'brand.DataTextField = "DepartmentName"
            'brand.DataValueField = "DepartmentID"
            'brand.DataBind()

            'Dim inventoryEdit As RadComboBox = e.Item.FindControl("ccInventory")
            'Dim inventoryType As New BusinessLogicLayer.InventoryTypeCollection
            'inventoryType = BusinessLogicLayer.InventoryType.GetAllInventoryType

            'inventoryEdit.DataSource = inventoryType
            'inventoryEdit.DataTextField = "Description"
            'inventoryEdit.DataValueField = "TypeID"
            'inventoryEdit.DataBind()

            ''  If Not IsPostBack Then
            'Dim editedItem As GridEditFormItem = CType(e.Item, GridEditFormItem)
            'If Not editedItem.ParentItem Is Nothing Then
            '    Dim datakey = editedItem.ParentItem.GetDataKeyValue("InventoryID").ToString()
            '    Dim InventoryItem As BusinessLogicLayer.Inventory
            '    InventoryItem = BusinessLogicLayer.Inventory.GetInventoryByInventoryID(datakey)



            '    'brand.SelectedValue = InventoryItem.DepartmentID
            '    inventoryEdit.SelectedValue = InventoryItem.TypeID


            'End If

            'If Not IsNothing(Session("InventoryTypeID")) Then
            '    inventoryEdit.SelectedValue = Session("InventoryTypeID")
            'End If

            ''End If


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

    Protected Function TrimDescription(ByVal description As String) As String
        If Not String.IsNullOrEmpty(description) AndAlso description.Length > 200 Then
            Return String.Concat(description.Substring(0, 200), "...")
        End If
        Return description
    End Function

    Function getBrand(ByVal id As String) As String
        'Dim Department As BusinessLogicLayer.Department
        'Department = BusinessLogicLayer.Department.GetDepartmentByDepartmentID(id)
        'Return Department.DepartmentName
    End Function



    Function getEvents(ByVal id As String) As String
        Dim e = ""
        'Dim dt = ReadRecords("SELECT * FROM tblOrderItems INNER JOIN tblOrders ON tblOrderItems.fkOrderID = tblOrders.pkOrderID WHERE (tblOrderItems.fkInventoryID = " & id & " )")
        'Dim orderItems As BusinessLogicLayer.OrderItemsCollection
        'orderItems = BusinessLogicLayer.OrderItems.GetOrderItemsListBySQLCommand("SELECT * FROM tblOrderItems INNER JOIN tblOrder ON tblOrderItems.fkOrderID = tblOrder.pkOrderID WHERE (tblOrderItems.fkInventoryID = " & id & ") AND (tblOrder.fkOrderStatusID = 1002 or tblOrder.fkOrderStatusID = 1004 or tblOrder.fkOrderStatusID = 1005)")
        If id <> "" Then
            e = "<a href='Reports.aspx?inventoryID=" & id & "'>Events</a>"
        End If

        Return e
    End Function

    Function getReserved(ByVal quantity As String, ByVal id As String) As String
        'Dim inventory As BusinessLogicLayer.OrderItemsCollection
        'Dim sql = "SELECT * FROM tblOrder INNER JOIN tblOrderItems ON tblOrder.pkOrderID = tblOrderItems.fkOrderID INNER JOIN tblInventory ON tblOrderItems.fkInventoryID = tblInventory.pkInventoryID WHERE (tblOrderItems.fkInventoryID = " & id & ")  AND (tblOrder.fkOrderStatusID = 1002) "
        'inventory = BusinessLogicLayer.OrderItems.GetOrderItemsListBySQLCommand(sql)
        'Dim total = 0
        'If inventory IsNot Nothing Then


        '    For Each i In inventory
        '        total = total + i.Quantity
        '    Next
        'End If

        'Return total
    End Function

    Function getTotal(ByVal quantity As String, ByVal id As String) As String
        'Dim inventory As BusinessLogicLayer.OrderItemsCollection
        'inventory = BusinessLogicLayer.OrderItems.GetOrderItemsListBySQLCommand("SELECT * FROM tblOrder INNER JOIN tblOrderItems ON tblOrder.pkOrderID = tblOrderItems.fkOrderID INNER JOIN tblInventory ON tblOrderItems.fkInventoryID = tblInventory.pkInventoryID WHERE (tblOrderItems.fkInventoryID = " & id & ")  AND (tblOrder.fkOrderStatusID = 1002) ")
        'Dim total = 0
        'If inventory IsNot Nothing Then


        '    For Each i In inventory
        '        total = total + i.Quantity
        '    Next
        'End If

        'Return total + quantity
    End Function

    Function getInventoryType(ByVal id As String) As String
        'Try


        '    Dim InventoryType As BusinessLogicLayer.InventoryType
        '    InventoryType = BusinessLogicLayer.InventoryType.GetInventoryTypeByTypeID(id)
        '    Return InventoryType.Description
        'Catch ex As Exception

        'End Try
    End Function

    Protected Sub RadGrid1_InsertCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs)

        InsertUpdate("insert", e)

    End Sub

    Protected Sub RadGrid1_UpdateCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs)
        InsertUpdate("update", e)


    End Sub

    Function CreateNewType(t As String) As String
        Dim pkid
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)


            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr

            sqlComm.CommandText = "Insert into tblInventoryType ([Description], [isDeleted]) VALUES (@Description, @isDeleted); SELECT SCOPE_IDENTITY()"



            sqlComm.Parameters.Add(New SqlParameter("Description", t))
            sqlComm.Parameters.Add(New SqlParameter("isDeleted", 0))
            Try
                pkid = sqlComm.ExecuteScalar()
            Catch ex As Exception

            End Try

        End Using

        Return pkid
    End Function

    Sub InsertUpdate(cmd As String, ByVal e As GridCommandEventArgs)
        Dim pkid
        Dim editedItem As GridEditableItem = CType(e.Item, GridEditableItem)
        Dim newValues As Hashtable = New Hashtable
        e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem)
        Dim upload As RadAsyncUpload = e.Item.FindControl("RadUpload1")

        Randomize()
        Dim r As New Random(System.DateTime.Now.Millisecond)
        Dim uploadedfile = ""
        Dim FileImage = ""
        Dim targetFolder As String = Server.MapPath("inventory/")
        For Each validFile As UploadedFile In upload.UploadedFiles
            Dim ImageName = Split(upload.UploadedFiles.Item(0).FileName, "\")
            FileImage = r.Next(1, 999999) & ImageName(ImageName.Length - 1)
            validFile.SaveAs(targetFolder & FileImage, True)
            TestRotate(targetFolder & FileImage)


            uploadedfile = ",[Picture]=@Picture"

        Next

        Dim brand As RadComboBox = e.Item.FindControl("ccInventory")
        Dim fkInventoryTypeID = ""
        If brand.SelectedIndex > 0 Then
            fkInventoryTypeID = brand.SelectedValue
        Else
            fkInventoryTypeID = CreateNewType(brand.Text)
        End If



        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            If cmd = "insert" Then
                sqlComm.CommandText = "Insert into tblInventory ([fkCompanyID],[PartNum],[Description],[Picture],[UDF01],[UDF02],[UDF03],[UDF04],[UDF05],[UDF06],[UDF07],[UDF08],[UDF09],[UDF10],[DateCreated],[DateModified],[Owner],[LastModifiedBy], isDeleted, fkInventoryTypeID, weight, notes, name, threshold, cost) VALUES " & _
                "(@fkCompanyID ,@PartNum ,@Description ,@Picture ,@UDF01 ,@UDF02 ,@UDF03 ,@UDF04 ,@UDF05 ,@UDF06 ,@UDF07 ,@UDF08 ,@UDF09 ,@UDF10 ,@DateCreated ,@DateModified ,@Owner ,@LastModifiedBy, @isDeleted, @fkInventoryTypeID, @weight, @notes, @name, @threshold, @cost ); SELECT SCOPE_IDENTITY()"
            Else
                sqlComm.CommandText = "Update tblInventory set [fkCompanyID]=@fkCompanyID,[PartNum]=@PartNum,[Description]=@Description" & uploadedfile & ",[UDF01]=@UDF01,[UDF02]=@UDF02,[UDF03]=@UDF03,[UDF04]=@UDF04,[UDF05]=@UDF05,[UDF06]=@UDF06, " & _
                    "[UDF07]=@UDF07,[UDF08]=@UDF08,[UDF09]=@UDF09,[UDF10]=@UDF10,[DateModified]=@DateModified,[LastModifiedBy]=@LastModifiedBy, isDeleted=@isDeleted, fkInventoryTypeID=@fkInventoryTypeID, weight=@weight, notes=@notes, name=@name, threshold=@threshold, cost=@cost where pkInventoryID=@pkInventoryID "
                sqlComm.Parameters.Add(New SqlParameter("pkInventoryID", editedItem.GetDataKeyValue("pkInventoryID").ToString()))
            End If


            sqlComm.Parameters.Add(New SqlParameter("fkCompanyID", 1000))
            sqlComm.Parameters.Add(New SqlParameter("PartNum", ConvertNothing(newValues("PartNum"))))
            sqlComm.Parameters.Add(New SqlParameter("Description", ConvertNothing(newValues("Description"))))
            sqlComm.Parameters.Add(New SqlParameter("Picture", FileImage))
            sqlComm.Parameters.Add(New SqlParameter("UDF01", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF02", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF03", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF04", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF05", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF06", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF07", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF08", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF09", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF10", ""))
            sqlComm.Parameters.Add(New SqlParameter("DateCreated", Now()))
            sqlComm.Parameters.Add(New SqlParameter("DateModified", Now()))
            sqlComm.Parameters.Add(New SqlParameter("isDeleted", ConvertNothing(newValues("isDeleted"))))
            sqlComm.Parameters.Add(New SqlParameter("Owner", puser.userID))
            sqlComm.Parameters.Add(New SqlParameter("fkInventoryTypeID", fkInventoryTypeID))
            sqlComm.Parameters.Add(New SqlParameter("notes", ConvertNothing(newValues("Notes"))))
            sqlComm.Parameters.Add(New SqlParameter("name", ConvertNothing(newValues("Name"))))
            sqlComm.Parameters.Add(New SqlParameter("weight", ConvertNothing(newValues("Weight"))))
            sqlComm.Parameters.Add(New SqlParameter("cost", ConvertNothing(newValues("Cost"))))
            sqlComm.Parameters.Add(New SqlParameter("threshold", ConvertNothing(newValues("Threshold"))))
            sqlComm.Parameters.Add(New SqlParameter("LastModifiedBy", puser.userID))




            Try
                pkid = sqlComm.ExecuteScalar()
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

    Public Sub ConfigureExport(ByVal command As String)
        '  RadGrid1.ExportSettings.ExportOnlyData = True
        RadGrid1.ExportSettings.IgnorePaging = True
        RadGrid1.ExportSettings.OpenInNewWindow = True

        RadGrid1.Columns.Item(0).Visible = False
        RadGrid1.Columns.Item(1).Visible = False
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


    Public Function TestRotate(sImageFilePath As String) As Boolean
        Dim rft As RotateFlipType = RotateFlipType.RotateNoneFlipNone
        Dim img As Bitmap = Image.FromFile(sImageFilePath)
        Dim properties As PropertyItem() = img.PropertyItems
        Dim bReturn As Boolean = False
        For Each p As PropertyItem In properties
            If p.Id = 274 Then
                Dim orientation As Short = BitConverter.ToInt16(p.Value, 0)
                Select Case orientation
                    Case 1
                        rft = RotateFlipType.RotateNoneFlipNone
                    Case 3
                        rft = RotateFlipType.Rotate180FlipNone
                    Case 6
                        rft = RotateFlipType.Rotate90FlipNone
                    Case 8
                        rft = RotateFlipType.Rotate270FlipNone
                End Select
            End If
        Next
        If rft <> RotateFlipType.RotateNoneFlipNone Then
            img.RotateFlip(rft)
            System.IO.File.Delete(sImageFilePath)
            img.Save(sImageFilePath, System.Drawing.Imaging.ImageFormat.Jpeg)
            bReturn = True
        End If
        Return bReturn

    End Function
End Class

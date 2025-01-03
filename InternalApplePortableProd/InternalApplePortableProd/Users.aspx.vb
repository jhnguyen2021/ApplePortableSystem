Imports Telerik.Web.UI
Imports System.Data
Imports System.Data.SqlClient

Partial Class Users
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

        'If Not puser.department = 1001 Then
        '    Response.Redirect("default.aspx")
        'End If


        If Not Page.IsPostBack Then

        End If
        ' RegisterUser.ContinueDestinationPageUrl = Request.QueryString("ReturnUrl")
        'Dim userinsession As BusinessLogicLayer.User = SessionManager.GetUserInSession()
        'If userinsession IsNot Nothing Then
        '    Select Case userinsession.RoleID
        '        Case 1000 'matrexadmin
        '            RadGrid1.Visible = True
        '        Case 1001 'BMOAdmin
        '            RadGrid1.Visible = True
        '            RadGrid1.Columns.Item(0).Visible = False
        '            RadGrid1.Columns.Item(4).Visible = False
        '            RadGrid1.Columns.Item(6).Visible = False
        '            RadGrid1.Columns.Item(7).Visible = False
        '            RadGrid1.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = False
        '            sql = "Select * from tblUser where isDeleted=0"
        '        Case Else   'User
        '            RadGrid1.Visible = False

        '    End Select
        'End If

        'If Request.QueryString("uid") IsNot Nothing And Request.QueryString("sid") IsNot Nothing Then
        '    Dim user As BusinessLogicLayer.User
        '    user = BusinessLogicLayer.User.GetUserByUserID(Request.QueryString("uid"))
        '    If Page.User.Identity.IsAuthenticated Then
        '        Select Case user.RoleID
        '            Case 1000 'matrexadmin
        '                RadGrid1.Visible = True
        '            Case 1001 'BMOAdmin
        '                RadGrid1.Visible = True
        '                RadGrid1.Columns.Item(0).Visible = False
        '                RadGrid1.Columns.Item(5).Visible = False
        '                RadGrid1.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = False

        '            Case Else   'User
        '                RadGrid1.Visible = False

        '        End Select
        '    End If
        '    user.IsDeleted = Request.QueryString("sid")
        '    user.Save()

        '    Literal1.Text = "<h2>User " & user.Fullname & " has been " & If(Request.QueryString("sid") = 0, "Approved", "Declined") & "</h2>"

        '    If Request.QueryString("sid") = 0 Then
        '        Dim strToaddress As String = user.Email
        '        Dim msgMail As New MailMessage
        '        msgMail.From = ConfigurationSettings.AppSettings("FromEmail")
        '        msgMail.Subject = "Heartland Portables Online Tool"
        '        msgMail.BodyFormat = MailFormat.Html



        '        Dim strBody As String = "<style type='text/css'>.style1{font-size: xx-small; width: 260px; float:left; padding-left: 1px;}.style2{font-size: xx-small; width: 520px; padding-left: 1px;}</style>"

        '        strBody = strBody & "Your registration has been processed and you are now able to log into the Heartland Portables Online Tool.<br><br><a href='" & ConfigurationSettings.AppSettings("website") & "'>Heartland Portables Online Tool</a>"

        '        Try
        '            msgMail.Body = strBody
        '            SmtpMail.SmtpServer = ConfigurationSettings.AppSettings("smptServerAddress")


        '            msgMail.To = strToaddress
        '            SmtpMail.Send(msgMail)

        '        Catch ex As Exception

        '        End Try


        '    End If

        'End If
    End Sub

    Protected Sub RadGrid1_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs)
        If TypeOf e.Item Is GridEditableItem AndAlso e.Item.IsInEditMode Then
            Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)


            Dim RoleEdit As RadComboBox = e.Item.FindControl("ccRole")


            RoleEdit.DataSource = ReadRecords("Select * from tblDepartment where inactive=0")
            RoleEdit.DataTextField = "Description"
            RoleEdit.DataValueField = "pkDepartmentID"
            RoleEdit.DataBind()

            '    '  If Not IsPostBack Then
            Dim editedItem As GridEditFormItem = CType(e.Item, GridEditFormItem)
            If Not editedItem.ParentItem Is Nothing Then
                Dim datakey = editedItem.ParentItem.GetDataKeyValue("fkDepartmentID").ToString()
                Dim phonenum = editedItem.ParentItem.GetDataKeyValue("phone").ToString()
                RoleEdit.SelectedValue = datakey
                Dim tbphone As RadNumericTextBox = e.Item.FindControl("txtPhone")
                tbphone.Text = phonenum
            End If


            '    'End If


        End If
    End Sub

    Protected Sub RadGrid1_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGrid1.NeedDataSource
        Dim sql = "Select tblusers.*, tbldepartment.description as department from tblUsers inner join tblDepartment on tblDepartment.pkdepartmentid=tblusers.fkdepartmentid"
        If puser.role = 1000 Then
            sql = "Select tblusers.*, tbldepartment.description as department from tblUsers inner join tblDepartment on tblDepartment.pkdepartmentid=tblusers.fkdepartmentid where tblusers.email not like '%pinnacle%'"
        End If


        RadGrid1.DataSource = ReadRecords(sql)
    End Sub

    Protected Sub RadGrid1_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles RadGrid1.ItemCommand
        If e.CommandName = Telerik.Web.UI.RadGrid.ExportToExcelCommandName OrElse e.CommandName = Telerik.Web.UI.RadGrid.ExportToWordCommandName OrElse e.CommandName = Telerik.Web.UI.RadGrid.ExportToCsvCommandName OrElse e.CommandName = Telerik.Web.UI.RadGrid.ExportToPdfCommandName Then
            ConfigureExport(e.CommandName)
        End If
    End Sub

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
                RadGrid1.MasterTableView.ExportToPdf()
        End Select



        'RadGrid1.MasterTableView.ExportToExcel()
        '  RadGrid1.MasterTableView.ExportToWord()
        '  RadGrid1.MasterTableView.ExportToCSV()
        'RadGrid1.MasterTableView.ExportToPdf()

    End Sub

    Protected Sub RadGrid1_InsertCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs)
        Dim pkid = ""

        'Dim User As New BusinessLogicLayer.User
        '' User.DateAdded = Now
        '' User.DateModified = Now
        Dim wrapper As New Simple3Des(ConfigurationSettings.AppSettings("3dskey"))
        Dim editedItem As GridEditableItem = CType(e.Item, GridEditableItem)
        Dim newValues As Hashtable = New Hashtable
        e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem)
        Dim role As RadComboBox = e.Item.FindControl("ccRole")
        Dim phone As RadNumericTextBox = e.Item.FindControl("txtphone")
        'User.Fname = newValues("FName")
        'User.Lname = ConvertNothing(newValues("LName"))
        'User.Email = ConvertNothing(newValues("Email"))
        'User.Phone = ConvertNothing(newValues("Phone"))
        'User.Password = ConvertNothing(newValues("Password"))
        'User.IsDeleted = newValues("isDeleted")

        'Dim role As RadComboBox = e.Item.FindControl("ccRole")
        'User.RoleID = role.SelectedValue

        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "INSERT INTO tblUsers ([FirstName],[LastName],[Email],[Password],[fkRoleID],[fkCompanyID],[DateCreated],[DateModified],[Inactive],[Phone],[fkDepartmentID],[fkBranchID]) VALUES" & _
           " (@FirstName,@LastName,@Email,@Password,@fkRoleID,@fkCompanyID,@DateCreated,@DateModified,@Inactive,@Phone,@fkDepartmentID,@fkBranchID)"
            sqlComm.Parameters.Add(New SqlParameter("firstname", ConvertNothing(newValues("FirstName"))))
            sqlComm.Parameters.Add(New SqlParameter("lastname", ConvertNothing(newValues("LastName"))))
            sqlComm.Parameters.Add(New SqlParameter("email", ConvertNothing(newValues("Email"))))
            sqlComm.Parameters.Add(New SqlParameter("inactive", newValues("inactive")))
            sqlComm.Parameters.Add(New SqlParameter("phone", phone.Value.ToString()))
            sqlComm.Parameters.Add(New SqlParameter("password", wrapper.EncryptData("abc123")))
            sqlComm.Parameters.Add(New SqlParameter("fkRoleID", 1000))
            sqlComm.Parameters.Add(New SqlParameter("fkCompanyID", 1000))
            sqlComm.Parameters.Add(New SqlParameter("fkBranchID", 1000))
            sqlComm.Parameters.Add(New SqlParameter("DateCreated", Now()))
            sqlComm.Parameters.Add(New SqlParameter("datemodified", Now()))
            sqlComm.Parameters.Add(New SqlParameter("fkdepartmentID", role.SelectedValue))
            Try
                pkid = sqlComm.ExecuteScalar()
            Catch ex As Exception

            End Try
        End Using
        e.Item.OwnerTableView.IsItemInserted = False
        e.Canceled = True
        RadGrid1.DataSource = Nothing
        RadGrid1.Rebind()


    End Sub

    Protected Sub RadGrid1_UpdateCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs)
        Dim pkid = ""

        Dim editedItem As GridEditFormItem = CType(e.Item, GridEditFormItem)
        Dim datakey As String = editedItem.ParentItem.GetDataKeyValue("pkUserID").ToString()

        Dim newValues As Hashtable = New Hashtable
        e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem)

        Dim role As RadComboBox = e.Item.FindControl("ccRole")
        Dim phone As RadNumericTextBox = e.Item.FindControl("txtphone")



        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "Update tblUsers set firstname=@firstname, lastname=@lastname, email=@email, inactive=@inactive, phone=@phone, fkDepartmentID=@fkDepartmentID, datemodified=@datemodified where pkuserid=@pkuserid"
            sqlComm.Parameters.Add(New SqlParameter("firstname", ConvertNothing(newValues("FirstName"))))
            sqlComm.Parameters.Add(New SqlParameter("lastname", ConvertNothing(newValues("LastName"))))
            sqlComm.Parameters.Add(New SqlParameter("email", ConvertNothing(newValues("Email"))))
            sqlComm.Parameters.Add(New SqlParameter("inactive", newValues("inactive")))
            sqlComm.Parameters.Add(New SqlParameter("phone", phone.Text.ToString()))
            sqlComm.Parameters.Add(New SqlParameter("pkuserid", datakey))
            sqlComm.Parameters.Add(New SqlParameter("datemodified", Now()))
            sqlComm.Parameters.Add(New SqlParameter("fkdepartmentID", role.SelectedValue))
            Try
                pkid = sqlComm.ExecuteScalar()
            Catch ex As Exception

            End Try
        End Using

        e.Item.Edit = False
        RadGrid1.DataSource = Nothing
        RadGrid1.Rebind()


    End Sub

    Function ConvertNothing(ByRef val As String) As String
        If val Is Nothing Then
            Return ""
        Else
            Return val
        End If

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
End Class


Imports Telerik.Web.UI
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Imaging



Partial Class Reporting
    Inherits Telerik.Web.UI.RadAjaxPage
    Private isExport As Boolean = False
    Dim puser As PortalUser
    Private hasDateRange As Boolean = False




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
        RadGrid1.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = False

        Try
            If Not IsPostBack Then

                ddldepartment.DataSource = ReadRecords("select * from tbldepartment where inactive=0")
                ddldepartment.DataTextField = "Description"
                ddldepartment.DataValueField = "pkdepartmentid"
                ddldepartment.DataBind()

                Dim radComboBoxItem1 As New RadComboBoxItem
                radComboBoxItem1.Text = "Select All Departments"
                radComboBoxItem1.Value = "All"
                ddldepartment.Items.Insert(0, radComboBoxItem1)


                ddlUser.DataSource = ReadRecords("SELECT CONCAT(FirstName, ' ', LastName) AS Name, * from tblUsers where email like '%apple.com%'")
                ddlUser.DataTextField = "Name"
                ddlUser.DataValueField = "pkUserID"
                ddlUser.DataBind()
                Dim radComboBoxItem2 As New RadComboBoxItem
                radComboBoxItem2.Text = "Select All Users"
                radComboBoxItem2.Value = "All"
                ddlUser.Items.Insert(0, radComboBoxItem2)


                'Dim sortExpr As New GridSortExpression()
                'sortExpr.FieldName = "Location"
                'sortExpr.SortOrder = GridSortOrder.Ascending
                ''Add sort expression, which will sort against first column
                'RadGrid1.MasterTableView.SortExpressions.AddSortExpression(sortExpr)
            End If
        Catch ex As Exception
        End Try
    End Sub


    Protected Sub RadGrid1_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGrid1.NeedDataSource

        Dim company = puser.company
        Dim department = puser.department
        'this variable holds SQL logic to remove exhibit part numbers, the leading space in the string is NECESSARY

        Dim sql = "SELECT * FROM vwReportingView"

        RadGrid1.DataSource = ReadRecords(sql)

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
            'MsgBox(ex.ToString())
        End Try
        Return dt

    End Function


    Protected Sub runReport_Click(sender As Object, e As EventArgs) Handles runReport.Click
        Dim userid = ddlUser.SelectedValue
        Dim department = ddldepartment.SelectedValue

        Dim a = ddlUser.Text
        Dim b = ddldepartment.Text

        If ddlUser.Text = "Select All Users" Then
            userid = "%"
        End If
        If ddldepartment.Text = "Select All Departments" Then
            department = "%"
        End If

        Try
            Dim startDate As Date = tbStartDate.SelectedDate
            Dim endDate As Date = tbPickupDate.SelectedDate
            Dim today = Now
            hasDateRange = True


            If endDate < startDate Then
                message.Text = "Start date cannot be less than end date"
                messageDiv.Attributes.CssStyle.Add("Color", "#FF0000")
                Exit Sub
            Else
                message.Text = ""



                'startDate = AddBusinessDays(startDate, -7)
                'endDate = AddBusinessDays(endDate, 10)

                Dim sql = String.Empty

                sql = "Select * from vwReportingView where (EventStartDate >= N'" & startDate & "' and EventEndDate < N'" & endDate & "') and pkDepartmentID like '" & department & "'  and UserID like '" & userid & "'"

                RadGrid1.DataSource = ReadRecords(sql)


                Dim dt = ReadRecords(sql)
                RadGrid1.DataSource = dt
                RadGrid1.Rebind()

            End If
        Catch ex As Exception
        End Try

    End Sub


    Protected Sub RadGrid1_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles RadGrid1.ItemCommand
        If e.CommandName = Telerik.Web.UI.RadGrid.ExportToExcelCommandName OrElse e.CommandName = Telerik.Web.UI.RadGrid.ExportToWordCommandName OrElse e.CommandName = Telerik.Web.UI.RadGrid.ExportToCsvCommandName OrElse e.CommandName = Telerik.Web.UI.RadGrid.ExportToPdfCommandName Then
            ConfigureExport(e.CommandName)
        End If
    End Sub

    Public Sub ConfigureExport(ByVal command As String)
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


End Class


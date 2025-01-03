Imports System.Data
Imports System.Data.SqlClient

Partial Class Account_Manager
    Inherits Telerik.Web.UI.RadAjaxPage
    Dim user As PortalUser
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsNothing(Session("user")) Then
            Dim ticket = Request.Cookies(FormsAuthentication.FormsCookieName).Value
            Dim dycriptedTicket = FormsAuthentication.Decrypt(ticket)
            If dycriptedTicket.UserData = "" Then
                Response.Redirect("login.aspx")
            End If
            Dim t As New sessionmanager(dycriptedTicket.UserData)
        End If

        user = Session("user")

        If Not IsPostBack Then
            ddlDivision.DataSource = ReadRecords("select * from tbldepartment where inactive=0 and fkcompanyid=1002")
            ddlDivision.DataTextField = "Description"
            ddlDivision.DataValueField = "pkdepartmentid"
            ddlDivision.DataBind()

            Dim dt = ReadRecords("select * from tblUsers where pkuserid=" & user.userID)
            If dt.Rows.Count > 0 Then
                tbFirstName.Text = dt(0)("FirstName").ToString()
                tbLastName.Text = dt(0)("LastName").ToString()
                tbEmail.Text = dt(0)("Email").ToString()
                
                tbPhone.Text = dt(0)("Phone").ToString()
                ddlDivision.SelectedValue = dt(0)("fkDepartmentID").ToString
                'If dt(0)("fkDepartmentID").ToString = 1000 Then
                'valManager.Enabled = True
                'managerrow.Visible = True
                'Else
                valManager.Enabled = False
                managerrow.Visible = False
                'End If
                'tbManager.Text = dt(0)("UDF01").ToString
            End If
        End If

    End Sub

    Protected Sub RadButton2_Click(sender As Object, e As EventArgs) Handles RadButton2.Click
        Dim pkid = ""
        Dim manager = ""
        If ddlDivision.SelectedItem.Text = "AB Sales" Then
            manager = tbManager.Text
        End If
        Dim wrapper As New Simple3Des(ConfigurationSettings.AppSettings("3dskey"))

        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            'If Request.QueryString("OrderID") = "" Then
            sqlComm.CommandText = "Update tblUsers set [FirstName]=@FirstName,[LastName]=@LastName,[Email]=@Email,[Password]=@Password,[DateModified]=@DateModified,[Phone]=@Phone,[fkDepartmentID]=@fkDepartmentID, UDF01=@UDF01 where pkuserid=@pkuserid; "
            If tbPassword.Text = "" Then
                sqlComm.CommandText = "Update tblUsers set [FirstName]=@FirstName,[LastName]=@LastName,[Email]=@Email,[DateModified]=@DateModified,[Phone]=@Phone,[fkDepartmentID]=@fkDepartmentID, UDF01=@UDF01 where pkuserid=@pkuserid; "
            End If
            'Else
            'sqlComm.CommandText = "update"
            'End If
            sqlComm.Parameters.Add(New SqlParameter("FirstName", tbFirstName.Text))
            sqlComm.Parameters.Add(New SqlParameter("LastName", tbLastName.Text))
            sqlComm.Parameters.Add(New SqlParameter("Email", tbEmail.Text))
            sqlComm.Parameters.Add(New SqlParameter("Password", wrapper.EncryptData(tbPassword.Text)))
            'sqlComm.Parameters.Add(New SqlParameter("fkRoleID ", 1000))
            'sqlComm.Parameters.Add(New SqlParameter("fkCompanyID ", 1000))
            'sqlComm.Parameters.Add(New SqlParameter("DateCreated ", Now.Date()))
            sqlComm.Parameters.Add(New SqlParameter("DateModified", Now.Date()))
            'sqlComm.Parameters.Add(New SqlParameter("Inactive", 0))
            sqlComm.Parameters.Add(New SqlParameter("UDF01", manager))
            'sqlComm.Parameters.Add(New SqlParameter("UDF02", ""))
            'sqlComm.Parameters.Add(New SqlParameter("UDF03 ", ""))
            'sqlComm.Parameters.Add(New SqlParameter("UDF04 ", ""))
            'sqlComm.Parameters.Add(New SqlParameter("UDF05 ", ""))
            'sqlComm.Parameters.Add(New SqlParameter("UDF06 ", ""))
            'sqlComm.Parameters.Add(New SqlParameter("UDF07 ", ""))
            'sqlComm.Parameters.Add(New SqlParameter("UDF08 ", ""))
            'sqlComm.Parameters.Add(New SqlParameter("UDF09 ", ""))
            'sqlComm.Parameters.Add(New SqlParameter("UDF10 ", ""))
            sqlComm.Parameters.Add(New SqlParameter("pkuserid", user.userID))
            sqlComm.Parameters.Add(New SqlParameter("Phone", tbPhone.Value))
            sqlComm.Parameters.Add(New SqlParameter("fkDepartmentID", ddlDivision.SelectedValue))
            'sqlComm.Parameters.Add(New SqlParameter("fkBranchID", ""))
            Try
                sqlComm.ExecuteNonQuery()
                Label1.Text = "Updated"
            Catch ex As Exception
                Label1.Text = "Error: " & ex.Message
                'failure.Text = "Unable to register your email"
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

        End Try
        Return dt

    End Function

    Protected Sub ddlDivision_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDivision.SelectedIndexChanged
        If ddlDivision.SelectedValue = 1000 Then
            valManager.Enabled = True
            managerrow.Visible = True
        Else
            valManager.Enabled = False
            managerrow.Visible = False
        End If
    End Sub
End Class

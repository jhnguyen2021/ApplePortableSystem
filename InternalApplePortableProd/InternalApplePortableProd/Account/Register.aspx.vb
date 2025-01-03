Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports System.Security.Cryptography

Partial Class Account_Register
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then

            ddlDivision.DataSource = ReadRecords("select * from tbldepartment where inactive=0")
            ddlDivision.DataTextField = "Description"
            ddlDivision.DataValueField = "pkdepartmentid"
            ddlDivision.DataBind()
            Dim myItem As New ListItem
            myItem.Value = "Select..."
            myItem.Text = "Select..."
            ddlDivision.Items.Insert(0, myItem)

        End If


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

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If txtUsername.Text.ToLower.Contains("@apple.com") Or txtUsername.Text.ToLower.Contains("@pinnacle-exp.com") Or txtUsername.Text.ToLower.Contains("@pinnacle-exhibits.com") Or txtUsername.Text.ToLower.Contains("kt-planning.co.jp") Or txtUsername.Text.ToLower.Contains("exposuregroup.com.au") Or txtUsername.Text.ToLower.Contains("idea-intl.com") Then
            If ReadRecords("select * from tblUsers where email like '" & txtUsername.Text.ToLower & "'").Rows.Count = 0 Then
                RegisterUser()
            Else
                failure.Text = "Account already exists"
            End If

        Else
            failure.Text = "Unable to register your email"
        End If
    End Sub

    Sub RegisterUser()

        Dim wrapper As New Simple3Des(ConfigurationSettings.AppSettings("3dskey"))



        Dim pkid
        Dim manager = ""
        Dim inactive = 1
        If ddlDivision.SelectedItem.Text = "AB Sales" Or ddlDivision.SelectedItem.Text = "AB Pro Serve" Then
            manager = txtManager.Text
        End If

        If ddlDivision.SelectedItem.Text = "Other Org of Amazon" Then
            EmailManagement()
            inactive = 1
        End If


        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            'If Request.QueryString("OrderID") = "" Then
            sqlComm.CommandText = "INSERT INTO tblUsers ([FirstName],[LastName],[Email],[Password],[fkRoleID],[fkCompanyID],[DateCreated],[DateModified],[Inactive],[UDF01],[UDF02],[UDF03],[UDF04],[UDF05],[UDF06],[UDF07],[UDF08],[UDF09],[UDF10],[Phone],[fkDepartmentID],[fkBranchID]) " & _
            "values (@FirstName ,@LastName ,@Email ,@Password ,@fkRoleID ,@fkCompanyID ,@DateCreated ,@DateModified ,@Inactive ,@UDF01 ,@UDF02 ,@UDF03 ,@UDF04 ,@UDF05 ,@UDF06 ,@UDF07 ,@UDF08 ,@UDF09 ,@UDF10 ,@Phone ,@fkDepartmentID ,@fkBranchID ); SELECT SCOPE_IDENTITY()"
            'Else
            'sqlComm.CommandText = "update"
            'End If
            sqlComm.Parameters.Add(New SqlParameter("FirstName", txtfname.Text))
            sqlComm.Parameters.Add(New SqlParameter("LastName", txtlname.Text))
            sqlComm.Parameters.Add(New SqlParameter("Email", txtUsername.Text.ToLower))
            sqlComm.Parameters.Add(New SqlParameter("Password", wrapper.EncryptData(txtPassword.Text)))
            sqlComm.Parameters.Add(New SqlParameter("fkRoleID", 1000))
            sqlComm.Parameters.Add(New SqlParameter("fkCompanyID", 1002))
            sqlComm.Parameters.Add(New SqlParameter("DateCreated", Now.Date()))
            sqlComm.Parameters.Add(New SqlParameter("DateModified", Now.Date()))
            sqlComm.Parameters.Add(New SqlParameter("Inactive", inactive))
            sqlComm.Parameters.Add(New SqlParameter("UDF01", manager))
            sqlComm.Parameters.Add(New SqlParameter("UDF02", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF03", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF04", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF05", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF06", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF07", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF08", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF09", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF10", ""))
            sqlComm.Parameters.Add(New SqlParameter("Phone", ""))
            sqlComm.Parameters.Add(New SqlParameter("fkDepartmentID", ddlDivision.SelectedValue))
            sqlComm.Parameters.Add(New SqlParameter("fkBranchID", ""))
            Try
                pkid = sqlComm.ExecuteScalar()
            Catch ex As Exception
                failure.Text = "Unable to register your email"
            End Try
        End Using
        Response.Redirect("login.aspx?a=" & Now.Date())


    End Sub

    

    Sub EmailManagement()

        Try
            

            Dim SmtpServer As New SmtpClient(ConfigurationSettings.AppSettings("SMTP"))
            SmtpServer.EnableSsl = False
            Dim mail As New MailMessage()
            mail.To.Add(ConfigurationSettings.AppSettings("ManagementEmail"))

            mail.Subject = ConfigurationSettings.AppSettings("ClientName") & " Portables Access Request"
            mail.Body = "<div style='width:90%;background-color:#ededed;border: 1px solid #dfdfdf;padding:30px;margin-left:auto;margin-right:auto;margin-top:30px;'><Br><br>" & txtfname.Text & " " & txtlname.Text & " (" & txtUsername.Text & ") has requested access. </div>"
            mail.From = New MailAddress(ConfigurationSettings.AppSettings("SupportEmail"), ConfigurationSettings.AppSettings("ClientName") & " Portables")
            mail.IsBodyHtml = True

            SmtpServer.Send(mail)

        Catch ex As Exception

        End Try


    End Sub


End Class



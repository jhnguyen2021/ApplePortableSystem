
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail

Partial Class Account_ResetPassword
    Inherits System.Web.UI.Page
    Dim pass As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim returnUrl = HttpUtility.UrlEncode(Request.QueryString("ReturnUrl"))
        Dim email As String = Request.QueryString("email")
        Dim token As String = Request.QueryString("token")

        If Not IsPostBack Then
            If Request.QueryString("email") <> "" Then
                If VerifyToken(Replace(email, "'", "''"), Replace(token, "'", "''")) Then
                    enterPass.Visible = True
                    rePass.Visible = True
                    Button1.Visible = False
                    Button2.Visible = True
                    txtUsername.Text = email
                Else
                    enterPass.Visible = False
                    rePass.Visible = False
                    Button1.Visible = True
                    Button2.Visible = False
                    failure.Text = "Token expired."
                End If
            Else
                enterPass.Visible = False
                rePass.Visible = False
                Button1.Visible = True
                Button2.Visible = False
            End If
        End If

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        LogIn(sender, e)
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim email As String = Page.Request.Form("txtUsername").ToString()
        Dim password As String = Page.Request.Form("txtPassword").ToString()
        Dim password2 As String = Page.Request.Form("txtConfirmPassword").ToString()

        If password <> password2 Then
            failure.Text = "Password must match"
            Return
        End If

        ResetUserPassword(email, password, password2)
    End Sub

    Protected Sub LogIn(sender As Object, e As EventArgs)
        Dim email As String = Page.Request.Form("txtUsername").ToString()
        Dim wrapper As New Simple3Des(ConfigurationSettings.AppSettings("3dskey"))
        Dim expirationDte = DateAdd("h", 24, Now())
        If AuthenticateUser(Replace(email, "'", "''")) Then

            Dim token = RandString(5)
            token = wrapper.EncryptData(token)

            Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
                strConnStr.Open()

                Dim sqlComm As New SqlCommand()
                sqlComm.Connection = strConnStr

                sqlComm.CommandText = "Insert into ResetTickets ([username], [tokenHash], [expirationDate], [tokenUsed]) values (@username, @tokenHash, @expirationDate, @tokenUsed)"

                sqlComm.Parameters.Add(New SqlParameter("username", email))
                sqlComm.Parameters.Add(New SqlParameter("tokenHash", token))
                sqlComm.Parameters.Add(New SqlParameter("expirationDate", expirationDte))
                sqlComm.Parameters.Add(New SqlParameter("tokenUsed", 0))
                Try
                    sqlComm.ExecuteScalar()
                    SendResetPasswordLink(email, token)
                    failure.Text = "Activation email has been sent"
                Catch ex As Exception
                    failure.Text = ex.Message
                End Try
            End Using
        Else
            failure.Text = "Invalid Email Address."
        End If
    End Sub

    Private Sub SendResetPasswordLink(email As String, token As String)

        Dim href2 = "https://intappleportables.yourpinnacle.net/Account/ResetPassword.aspx"
        Dim href = href2.Replace(".aspx", (".aspx?email=") & email & "&token=" & token)

        Try
            Dim wrapper As New Simple3Des(ConfigurationSettings.AppSettings("3dskey"))

            Dim SmtpServer As New SmtpClient("smtp-legacy.office365.com")
            SmtpServer.Port = 587
            SmtpServer.UseDefaultCredentials = False
            SmtpServer.Credentials = New System.Net.NetworkCredential(ConfigurationManager.AppSettings("o365user"), "Boc00263")
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network
            SmtpServer.EnableSsl = True

            Dim mail As New MailMessage()
            mail.To.Add(email)
            mail.Subject = ConfigurationSettings.AppSettings("ClientName") & " Password Reset Link"
            mail.Body = "Hello " + email + "," + "<br /><br />" + " " +
        "<br /><a href = '" + href + "'>Click here to reset your password.</a>" + " " +
        "<br /><br />Once the password has been reset you will be able to log into the portal." + " " +
        "<br /><br /> Pinnacle Portable Team"
            mail.From = New MailAddress("PinnacleNoReply@pinnacle-exp.com")
            mail.IsBodyHtml = True

            SmtpServer.Send(mail)

        Catch ex As Exception
        End Try
    End Sub

    Function AuthenticateUser(user As String) As Boolean
        Try
            Dim dt = ReadRecords("Select email, password from tblUsers where email = '" & user & "' and inactive = 0")
            If dt.Rows.Count > 0 Then
                pass = dt(0)("password")
                Return True
            End If
        Catch
            Return False
        End Try
    End Function

    Function VerifyToken(email As String, token As String) As Boolean
        Dim expiryDate As DateTime = DateTime.Now()
        Try
            Dim dt = ReadRecords("Select * from ResetTickets where username = '" & email & "' and tokenUsed = 0")
            If dt.Rows.Count > 0 Then
                Return True
            End If
        Catch
            Return False
        End Try
    End Function

    Sub ResetUserPassword(email As String, pass As String, confirmPass As String)

        Dim wrapper As New Simple3Des(ConfigurationSettings.AppSettings("3dskey"))

        'If pass <> confirmPass Then
        '    failure.Text = "Password must match"
        '    Return
        'End If

        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()
            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = " Update ResetTickets set tokenUsed = 1 where userName = @email"
            sqlComm.Parameters.Add(New SqlParameter("@email", email))
            Try
                sqlComm.ExecuteScalar()
            Catch ex As Exception
                failure.Text = "Please contact Admin"
            End Try
        End Using

        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()
            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr

            sqlComm.CommandText = "Update tblUsers set Password = @Password  where Email=@Email"
            sqlComm.Parameters.Add(New SqlParameter("Email", email))
            sqlComm.Parameters.Add(New SqlParameter("Password", wrapper.EncryptData(pass)))

            Try
                sqlComm.ExecuteScalar()
                Response.Redirect("login.aspx?s=" & Now.Date())
            Catch ex As Exception
                failure.Text = "Please contact your adminstrator."
            End Try
        End Using

    End Sub


    Function RandString(n As Long) As String
        'Assumes that Randomize has been invoked by caller
        Dim i As Long, j As Long, m As Long, s As String, pool As String
        pool = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
        m = Len(pool)
        For i = 1 To n
            j = 1 + Int(m * Rnd())
            s = s & Mid(pool, j, 1)
        Next i
        RandString = s
    End Function


    Sub ErrorEmail(body As String)

        Dim SmtpServer As New SmtpClient("smtp-legacy.office365.com")
        SmtpServer.Port = 587
        SmtpServer.UseDefaultCredentials = False
        SmtpServer.Credentials = New System.Net.NetworkCredential(ConfigurationManager.AppSettings("o365user"), "Boc00263")
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



Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail

Partial Class Account_EmailPassword
    Inherits System.Web.UI.Page
    Dim pass As String

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        LogIn(sender, e)
    End Sub

    Protected Sub LogIn(sender As Object, e As EventArgs)

        If (AuthenticateUser(Replace(txtUsername.Text.ToLower, "'", "''"))) Then
            EmailPassword(txtUsername.Text.ToLower, pass)
            failure.Text = "Your password has been emailed to you."
        Else
            failure.Text = "Invalid Email."
        End If
    End Sub

    Function AuthenticateUser(user As String) As Boolean
        Try
            Dim regex As Regex = New Regex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            Dim match As Match = regex.Match(user)

            If match.Success Then
                Dim dt = ReadRecords("Select email, password from tblUsers where email = '" & user & "' and inactive = 0")
                If dt.Rows.Count > 0 Then
                    pass = dt(0)("password")
                    Return True
                End If
            End If

        Catch
            Return False
        End Try
    End Function

    Sub EmailPassword(user As String, pass As String)

        'Dim wrapper As New Simple3Des(ConfigurationSettings.AppSettings("3dskey"))
        'Dim visible = wrapper.DecryptData(pass)

        Try
            Dim wrapper As New Simple3Des(ConfigurationSettings.AppSettings("3dskey"))

            Dim SmtpServer As New SmtpClient("smtp-legacy.office365.com")
            SmtpServer.Port = 587
            SmtpServer.UseDefaultCredentials = False
            SmtpServer.Credentials = New System.Net.NetworkCredential(ConfigurationManager.AppSettings("o365user"), "Boc00263")
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network
            SmtpServer.EnableSsl = True

            Dim mail As New MailMessage()
            mail.To.Add(user)
            mail.Subject = ConfigurationSettings.AppSettings("ClientName") & " Portables Password Request"
            mail.Body = "You recently requested your password for your Portables Asset Manager. Below is your password.<br><br><center>" & wrapper.DecryptData(pass) & "</center><br><br>If you did not " &
                       "request a password, please ignore this email or reply to let us konw. <br><br>Best, Your Portables Team"
            mail.From = New MailAddress("PinnacleNoReply@pinnacle-exp.com")
            mail.IsBodyHtml = True

            SmtpServer.Send(Mail)

        Catch ex As Exception
            MsgBox(ex.Message)
            'ErrorEmail("Send Error: " & ex.Message)
        End Try

    End Sub

    'Sub ErrorEmail(body As String)

    '    Dim SmtpServer As New SmtpClient(ConfigurationSettings.AppSettings("SMTP"), 25)
    '    SmtpServer.EnableSsl = False
    '    'SmtpServer.Credentials = New Net.NetworkCredential("@gmail.com", "xxx")

    '    Dim mail As New MailMessage("no-reply@yourpinnacle.net", "adamn@pinnacle-exhibits.com", "Error", body)
    '    mail.From = New MailAddress("no-reply@yourpinnacle.net", "Pinnacle Leads")
    '    mail.IsBodyHtml = True

    '    SmtpServer.Send(mail)

    'End Sub

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

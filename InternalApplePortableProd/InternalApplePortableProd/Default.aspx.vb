Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports SAPbobsCOM
Partial Class _Default
    Inherits Telerik.Web.UI.RadAjaxPage
    'Public oCompany As New Company()
    Public errorCode As Integer = 0
    Public errorMessage As String = ""
    Dim puser As PortalUser
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'Dim u As ERPUser = Session("user")

        If IsNothing(Session("user")) Then
            Dim ticket = Request.Cookies(FormsAuthentication.FormsCookieName).Value
            Dim dycriptedTicket = FormsAuthentication.Decrypt(ticket)
            If dycriptedTicket.UserData = "" Then
                FormsAuthentication.SignOut()
                Response.Redirect("account/login.aspx")
                SAMLLogIn()
                puser = Session("user")
            End If
            Dim t As New sessionmanager(dycriptedTicket.UserData)
            Try
                ' Dim t As New sessionmanager(puser.userID)
            Catch ex As Exception

            End Try

        End If
        puser = Session("user")

        If puser.role = 1006 Or puser.role = 1003 Then
            pendingApprovals.Visible = True
        Else
            pendingApprovals.Visible = False
        End If
    End Sub

    Protected Sub SAMLLogIn()
        'response.write("Test: " & Request.ServerVariables("mail"))
        Dim SAMLEmail As String

        SAMLEmail = Request.ServerVariables("mail")
        'SAMLEmail = "caleb.stjean@gmail.com"

        If SAMLEmail Is Nothing Then
            SAMLEmail = Request.ServerVariables("HTTP_mail")
        End If

        If Not SAMLEmail Is Nothing Then

            GetSAMLUser(SAMLEmail)

            puser = Session("user")
            Response.Cookies.Clear()
            Dim expiryDate As DateTime = DateTime.Now.AddDays(180)
            Dim ticket As FormsAuthenticationTicket = New FormsAuthenticationTicket(2, puser.firstname, DateTime.Now, expiryDate, True, puser.userID.ToString())
            Dim encryptedTicket = FormsAuthentication.Encrypt(ticket)
            Dim authenticationCookie As HttpCookie = New HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            authenticationCookie.Expires = ticket.Expiration
            Response.Cookies.Add(authenticationCookie)

            Session("user") = puser

            'Response.Redirect("../default.aspx", False)
        End If

    End Sub

    Protected Sub RadGrid3_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGrid3.NeedDataSource
        Dim uid = puser.userID.ToString()
        Dim sql = ""

        Select Case puser.role
            Case 1003
                uid = "%"
                sql = "Select *, CONVERT(varchar, DateStart, 101) + ' - ' + CONVERT(varchar, DateEnd, 101) AS eventdates from tblOrders join tblLocation on tblLocation.Location = tblOrders.fkFulfillmentLocationId where  fkStatusID = 1001 order by DateEnd desc"
            Case 1001
                If puser.userID = 1002 Or puser.userID = 1047 Then
                    uid = "%"
                    sql = "Select *, CONVERT(varchar, DateStart, 101) + ' - ' + CONVERT(varchar, DateEnd, 101) AS eventdates from tblOrders join tblLocation on tblLocation.Location = tblOrders.fkFulfillmentLocationId where  fkStatusID = 1001 order by DateEnd desc"
                End If
            Case 1006
                uid = "%"
                sql = "Select *, CONVERT(varchar, DateStart, 101) + ' - ' + CONVERT(varchar, DateEnd, 101) AS eventdates from tblOrders join tblLocation on tblLocation.Location = tblOrders.fkFulfillmentLocationId where  fkStatusID = 1001 order by DateEnd desc"
        End Select

        Dim dt = ReadRecords(sql)

        RadGrid3.DataSource = dt

    End Sub


    Protected Sub RadGrid1_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGrid1.NeedDataSource
        Dim uid = puser.userID.ToString()
        Dim sql = ""

        If puser.role = 1001 Or puser.role = 1003 Then
            uid = "%"
            sql = "Select *, CONVERT(varchar, DateStart, 101) + ' - ' + CONVERT(varchar, DateEnd, 101) AS eventdates from tblOrders where  fkStatusID = 1000 order by DateEnd desc"
        ElseIf puser.role = 1006 Then
            sql = "Select *, CONVERT(varchar, DateStart, 101) + ' - ' + CONVERT(varchar, DateEnd, 101) AS eventdates from tblOrders where  fkStatusID = 1000 and fkFulfillmentLocationId in ('Japan','China','Australia') order by DateEnd desc"
        Else
            sql = "Select *, CONVERT(varchar, DateStart, 101) + ' - ' + CONVERT(varchar, DateEnd, 101) AS eventdates from tblOrders where  fkStatusID = 1000 and fkDepartmentID = '" & puser.department & "' and fkUserID like '" & puser.userID & "' order by DateEnd desc"
            'Select Case puser.department
            '    Case 1002
            '        sql = "Select *, CONVERT(varchar, DateStart, 101) + ' - ' + CONVERT(varchar, DateEnd, 101) AS eventdates from tblOrders where  fkStatusID = 1000 and fkDepartmentID = 1002 and fkUserID like '" & puser.userID & "' order by datestart asc"
            '    Case 1003
            '        sql = "Select *, CONVERT(varchar, DateStart, 101) + ' - ' + CONVERT(varchar, DateEnd, 101) AS eventdates from tblOrders where  fkStatusID = 1000 and fkDepartmentID = 1003 and fkUserID like '" & puser.userID & "' order by datestart asc"
            '    Case 1004
            '        sql = "Select *, CONVERT(varchar, DateStart, 101) + ' - ' + CONVERT(varchar, DateEnd, 101) AS eventdates from tblOrders where  fkStatusID = 1000 and fkDepartmentID = 1004 and fkUserID like '" & puser.userID & "' order by datestart asc"
            '    Case 1005
            '        sql = "Select *, CONVERT(varchar, DateStart, 101) + ' - ' + CONVERT(varchar, DateEnd, 101) AS eventdates from tblOrders where  fkStatusID = 1000 and fkDepartmentID = 1005 and fkUserID like '" & puser.userID & "' order by datestart asc"
            '    Case Else
            '        sql = "Select *, CONVERT(varchar, DateStart, 101) + ' - ' + CONVERT(varchar, DateEnd, 101) AS eventdates from tblOrders where  fkStatusID = 1000 and fkDepartmentID = 1005 and fkUserID like '" & puser.userID & "' order by datestart asc"
            'End Select
        End If


        'Dim sql = "Select *, CONVERT(varchar, DateStart, 101) + ' - ' + CONVERT(varchar, DateEnd, 101) AS eventdates from tblOrders where  fkStatusID = 1000 and fkUserID like '" & puser.userID & "' order by datestart asc"

        Dim dt = ReadRecords(sql)

        RadGrid1.DataSource = dt

    End Sub

    Protected Sub RadGrid2_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGrid2.NeedDataSource
        Dim uid = puser.userID.ToString()
        Dim sql = ""


        If puser.role = 1001 Or puser.role = 1003 Then
            uid = "%"
            sql = "Select Concat( Format(Cast(DateStart as date), 'MM/dd/yyyy') , ' - ' , Format(Cast(DateEnd as date), 'MM/dd/yyyy')) AS eventdates, tblorders.*, tblorderstatus.status from tblOrders left outer join tblOrderStatus on tblOrderStatus.pkStatusID=tblOrders.fkStatusID where (fkStatusID > 1001 and fkStatusID not in (1006)) and fkuserid like '" & uid & "' order by DateEnd desc"
        ElseIf puser.role = 1006 Then
            uid = "%"
            sql = "Select Concat( Format(Cast(DateStart as date), 'MM/dd/yyyy') , ' - ' , Format(Cast(DateEnd as date), 'MM/dd/yyyy')) AS eventdates, tblorders.*, tblorderstatus.status from tblOrders left outer join tblOrderStatus on tblOrderStatus.pkStatusID=tblOrders.fkStatusID where (fkStatusID > 1001 and fkStatusID not in (1006)) and fkuserid like '" & uid & "' and fkFulfillmentLocationId in ('Japan','China','Australia') order by DateEnd desc"
        Else
            'sql = "Select CONVERT(varchar, DateStart, 101) + ' - ' + CONVERT(varchar, DateEnd, 101) AS eventdates, tblorders.*, tblorderstatus.status from tblOrders left outer join tblOrderStatus on tblOrderStatus.pkStatusID=tblOrders.fkStatusID where fkStatusID > 1001 and tblOrders.fkDepartmentID = '" & puser.department & "' and fkuserid like '" & uid & "' order by datestart desc"
            sql = "Select Concat( Format(Cast(DateStart as date), 'dd/MM/yyyy ') , ' - ' , Format(Cast(DateEnd as date), 'dd/MM/yyyy')) AS eventdates, tblorders.*, tblorderstatus.status from tblOrders left outer join tblOrderStatus on tblOrderStatus.pkStatusID=tblOrders.fkStatusID where (fkStatusID > 1001 and fkStatusID not in (1006)) and tblOrders.fkDepartmentID = '" & puser.department & "' and fkuserid like '" & uid & "' order by DateEnd desc"
            'If Not puser.department = 1003 Then
            '    sql = "Select CONVERT(varchar, DateStart, 101) + ' - ' + CONVERT(varchar, DateEnd, 101) AS eventdates, tblorders.*, tblorderstatus.status from tblOrders left outer join tblOrderStatus on tblOrderStatus.pkStatusID=tblOrders.fkStatusID where fkStatusID > 1001 and tblOrders.fkDepartmentID = 1002 and fkuserid like '" & uid & "' order by datestart desc"
            'End If

            'If puser.department = 1003 Then
            '    sql = "Select CONVERT(varchar, DateStart, 101) + ' - ' + CONVERT(varchar, DateEnd, 101) AS eventdates, tblorders.*, tblorderstatus.status from tblOrders left outer join tblOrderStatus on tblOrderStatus.pkStatusID=tblOrders.fkStatusID where fkStatusID > 1001 and tblOrders.fkDepartmentID = 1003 and fkuserid like '" & uid & "' order by datestart desc"
            'End If

            'If puser.department <> 1003 And puser.department <> 1002 Then
            '    sql = "Select CONVERT(varchar, DateStart, 101) + ' - ' + CONVERT(varchar, DateEnd, 101) AS eventdates, tblorders.*, tblorderstatus.status from tblOrders left outer join tblOrderStatus on tblOrderStatus.pkStatusID=tblOrders.fkStatusID where fkStatusID > 1001 and tblOrders.fkDepartmentID = 1003 and fkuserid like '" & uid & "' order by datestart desc"
            'End If
        End If

        Dim dt = ReadRecords(sql)

        RadGrid2.DataSource = dt

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

    Protected Sub GetSAMLUser(email As String)
        Dim dt = ReadRecords("Select * from tblUsers where email like '" & email.Trim() & "'")
        If dt.Rows.Count > 0 Then
            HttpContext.Current.Session("user") = New PortalUser(dt(0)("pkUserID").ToString(), dt(0)("email").ToString(), dt(0)("FirstName").ToString(), dt(0)("LastName").ToString(), dt(0)("fkDepartmentID").ToString(), "",
                                                                         dt(0)("email").ToString(), dt(0)("fkRoleID").ToString(), dt(0)("fkCompanyID").ToString())
        Else
            HttpContext.Current.Session("user") = New PortalUser(CInt(Math.Ceiling(Rnd() * 10000)) + 1, email, email, email, 1000, "", email, 1000, 1000)
        End If

    End Sub


    Protected Sub RadGrid3_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles RadGrid3.ItemCommand

        If e.CommandName = "ViewOrder" Then
            Dim item As Telerik.Web.UI.GridDataItem
            item = e.Item

            Dim OrderID = item.GetDataKeyValue("pkOrderID")
            Session("pkid") = OrderID
            Response.Redirect("Review.aspx?OrderID=" & OrderID & "&approveOrder=true")
        End If

    End Sub
End Class

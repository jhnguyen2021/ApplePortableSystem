Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Partial Class Schedule
    Inherits Telerik.Web.UI.RadAjaxPage
    Dim puser As PortalUser
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Dim ds As BusinessLogicLayer.OrderCollection
        Dim Calendardate = Now()
        If IsNothing(Session("user")) Then
            Dim ticket = Request.Cookies(FormsAuthentication.FormsCookieName).Value
            Dim dycriptedTicket = FormsAuthentication.Decrypt(ticket)
            If dycriptedTicket.UserData = "" Then
                Response.Redirect("account/login.aspx")
            End If
            Dim t As New sessionmanager(dycriptedTicket.UserData)
        End If

        puser = Session("user")

        Dim uid = puser.userID.ToString()
        If puser.role = 1001 Then
            uid = "%"
        End If

        If Not Page.IsPostBack Then

            Dim sql2 = ""

            If puser.role = 1003 Or puser.role = 1001 Then
                sql2 = "Select tblOrders.*, 'Event '+tblorders.eventname+' : Order# '+cast(tblorders.pkorderID as varchar)+' : '+tblorderstatus.status as status2 from tblOrders left outer join tblOrderStatus on tblOrderStatus.pkStatusID=tblOrders.fkStatusID where tblOrders.fkstatusid = 1002"
            ElseIf puser.role = 1006 Then
                sql2 = "Select tblOrders.*, 'Event '+tblorders.eventname+' : Order# '+cast(tblorders.pkorderID as varchar)+' : '+tblorderstatus.status as status2 from tblOrders left outer join tblOrderStatus on tblOrderStatus.pkStatusID=tblOrders.fkStatusID where tblOrders.fkstatusid = 1002 and fkFulfillmentLocationId in ('Japan','China','Australia')"
            Else
                sql2 = "Select tblOrders.*, 'Event '+tblorders.eventname+' : Order# '+cast(tblorders.pkorderID as varchar)+' : '+tblorderstatus.status as status2 from tblOrders left outer join tblOrderStatus on tblOrderStatus.pkStatusID=tblOrders.fkStatusID where tblOrders.fkstatusid = 1002 and tblOrders.fkDepartmentID = '" & puser.department & "' and tblorders.fkuserid like '" & uid & "'"
            End If

            Dim ds = ReadRecords(sql2)

            RadScheduler1.DataSource = ds
            RadScheduler1.DataKeyField = "pkOrderID"
            RadScheduler1.DataEndField = "DatePickUp"
            RadScheduler1.DataStartField = "DateArrive"
            RadScheduler1.DataSubjectField = "Status2"

            RadScheduler1.SelectedDate = Calendardate


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

    Protected Sub RadScheduler1_AppointmentDataBound(sender As Object, e As SchedulerEventArgs)
        If e.Appointment.Subject.IndexOf("Approved") > 0 Then
            e.Appointment.BackColor = Drawing.Color.LightGreen
        ElseIf e.Appointment.Subject.IndexOf("Pending") > 0 Then
            e.Appointment.BackColor = Drawing.Color.FromArgb(0, 179, 198)
        ElseIf e.Appointment.Subject.IndexOf("Rejected") > 0 Then
            e.Appointment.Visible = False
        End If
        '

    End Sub
End Class

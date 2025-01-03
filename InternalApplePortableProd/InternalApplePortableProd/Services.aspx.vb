Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports Square
Imports Square.Models
Imports Square.Apis
Imports Square.Exceptions


Partial Class Services
    Inherits System.Web.UI.Page
    Public pkid As String = ""
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        pkid = Replace(Request.QueryString("OrderID"), "'", "''")
        If Not IsPostBack Then

            Dim dt = ReadRecords("Select * from tblOrders inner join tblUsers on tblorders.fkUserID=tblusers.pkUserID where pkOrderID=" & pkid)
            If dt.Rows.Count > 0 Then
                lbshowinfo.Text = "<h3>" & dt(0)("EventName").ToString & " (" & CDate(dt(0)("DateStart").ToString).Date & "-" & CDate(dt(0)("DateEnd").ToString).Date & ")</h3>Contact: " & dt(0)("ContactName").ToString

                tbCarpet.Text = dt(0)("CarpetNotes").ToString
                tbElectrical.Text = dt(0)("ElectricalNotes").ToString
                tbExhibitorPortal.Text = dt(0)("OtherNotes").ToString
                tbFurniture.Text = dt(0)("RentalFurnitureNotes").ToString
                tbInternet.Text = dt(0)("InternetNotes").ToString
                tbLabor.Text = dt(0)("LaborNotes").ToString
                tbLeadRerieval.Text = dt(0)("LeadRetrievalNotes").ToString
                tbMaterials.Text = dt(0)("MaterialNotes").ToString
                tbTier.Text = dt(0)("TierNotes").ToString
                tbShipping.Text = dt(0)("ShippingNotes").ToString
                tbInbound.Text = dt(0)("InspectionNotes").ToString
                tbDrayage.Text = dt(0)("DrayageNotes").ToString
                cCarpet.Text = dt(0)("CarpetCosts").ToString
                cElectrical.Text = dt(0)("ElectricalCosts").ToString
                cExhibitorPortal.Text = dt(0)("OtherCosts").ToString
                cFurniture.Text = dt(0)("RentalFurnitureCosts").ToString
                cInternet.Text = dt(0)("InternetCosts").ToString
                cLabor.Text = dt(0)("LaborCosts").ToString
                cLeadRerieval.Text = dt(0)("LeadRetrievalCosts").ToString
                cMaterials.Text = dt(0)("MaterialCosts").ToString
                cTier.Text = dt(0)("TierCosts").ToString
                cShipping.Text = dt(0)("ShippingCosts").ToString
                cInbound.Text = dt(0)("InspectionCosts").ToString
                cDrayage.Text = dt(0)("DrayageCosts").ToString

                CalculateTotal()

                If dt(0)("Electrical").ToString() <> "" Then
                    vElectrical.Enabled = True
                    vElectricalc.Enabled = True
                End If

                If dt(0)("Labor").ToString() = "Yes" Then
                    vLabor.Enabled = True
                    vLaborC.Enabled = True
                End If

                If dt(0)("LeadRetrieval").ToString() = "Yes" Then
                    vLeadRetrieval.Enabled = True
                    vLeadRetrievalC.Enabled = True
                End If

                If dt(0)("Carpet").ToString() = "Yes" Then
                    vCarpet.Enabled = True
                    vCarpetC.Enabled = True
                End If

                If dt(0)("Internet").ToString() = "Yes" Then
                    vInternet.Enabled = True
                    vInternetC.Enabled = True
                End If

                If dt(0)("Material").ToString() = "Yes" Then
                    vMaterials.Enabled = True
                    vMaterialsC.Enabled = True
                End If

                If dt(0)("RentalFurniture").ToString() <> "" Then
                    vFurniture.Enabled = True
                    vFurnitureC.Enabled = True
                End If

                If dt(0)("CurrentBilled").ToString() <> "" Then
                    btnPay.Visible = False
                    verifyPay.Visible = False
                    Paid.InnerHtml = dt(0)("CurrentBilled").ToString() & " Paid"
                End If

                If dt(0)("fkDepartmentID").ToString = "1002" Then
                    btnPay.Visible = False
                    ccprocessing.Visible = False
                    verifyPay.Visible = False
                Else
                    GetSquareInfo(dt(0)("email").ToString())
                    ccprocessing.Visible = True
                End If




                

            End If
        End If


    End Sub


    Sub GetSquareInfo(email)
        Dim client As New SquareClient.Builder()

        Dim environment As Square.Environment = Square.Environment.Sandbox
        client.Environment(environment)
        client.AccessToken("EAAAEO-p5QBsW7X6jIAQHyMWMaoG9fAXwGNfS97b79VulpXyuJGaYfrD2lIO12aU")


        Dim locationApi As ILocationsApi = client.Build.LocationsApi
        Dim customerApi As ICustomersApi = client.Build.CustomersApi
        Dim r As ListCustomersResponse = customerApi.ListCustomers()
        For Each cust In r.Customers

            If cust.EmailAddress.Contains(email) Then
                If cust.Cards.Count > 0 Then
                    custID.Value = cust.Id
                    cardID.Value = cust.Cards(0).Id
                    lbCustomer.Text = cust.GivenName & " " & cust.FamilyName
                    lbCard.Text = cust.Cards(0).Last4
                    Exit For
                End If
            End If
        Next
    End Sub


    Function CDbl0(ByVal num) As Double
        If num = "" Then
            Return 0
        Else
            Return CDbl(num)
        End If

    End Function

    Sub CalculateTotal()
        Dim subtotal = CDbl(CDbl0(cCarpet.Text) _
                + CDbl0(cElectrical.Text) _
                + CDbl0(cExhibitorPortal.Text) _
                + CDbl0(cFurniture.Text) _
                + CDbl0(cInternet.Text) _
                + CDbl0(cLabor.Text) _
                + CDbl0(cLeadRerieval.Text) _
                + CDbl0(cMaterials.Text) _
                + CDbl0(cShipping.Text) _
                + CDbl0(cDrayage.Text))
        Dim nomarkup = CDbl0(cTier.Text) + CDbl0(cInbound.Text)
        servicemanagement.InnerHtml = FormatNumber((subtotal * 0.3), 2)


        ccprocessing.InnerHtml = FormatNumber(subtotal * 0.03, 2)

        Dim t = CDbl(ccprocessing.InnerHtml)


        If ccprocessing.Visible Then
            total.InnerText = FormatNumber((subtotal * 0.3) + (subtotal * 0.03) + subtotal + nomarkup, 2)
        Else
            total.InnerText = FormatNumber((subtotal * 0.3) + subtotal + nomarkup, 2)
        End If

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        CalculateTotal()
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr

            sqlComm.CommandText = "Update tblOrders set [CarpetNotes]=@CarpetNotes,[ElectricalNotes]=@ElectricalNotes,[OtherNotes]=@OtherNotes,[RentalFurnitureNotes]=@RentalFurnitureNotes,[InternetNotes]=@InternetNotes,[LaborNotes]=@LaborNotes,[LeadRetrievalNotes]=@LeadRetrievalNotes,[MaterialNotes]=@MaterialNotes,[TierNotes]=@TierNotes,[ShippingNotes]=@ShippingNotes,[InspectionNotes]=@InspectionNotes,[DrayageNotes]=@DrayageNotes," & _
                                    "[CarpetCosts]=@CarpetCosts,[ElectricalCosts]=@ElectricalCosts,[OtherCosts]=@OtherCosts,[RentalFurnitureCosts]=@RentalFurnitureCosts,[InternetCosts]=@InternetCosts,[LaborCosts]=@LaborCosts,[LeadRetrievalCosts]=@LeadRetrievalCosts,[MaterialCosts]=@MaterialCosts,[TierCosts]=@TierCosts,[ShippingCosts]=@ShippingCosts,[InspectionCosts]=@InspectionCosts, DrayageCosts=@DrayageCosts where pkOrderID=@pkOrderID"
            sqlComm.Parameters.Add(New SqlParameter("pkorderid", pkid))

            sqlComm.Parameters.Add(New SqlParameter("CarpetNotes", tbCarpet.Text))
            sqlComm.Parameters.Add(New SqlParameter("ElectricalNotes", tbElectrical.Text))
            sqlComm.Parameters.Add(New SqlParameter("OtherNotes", tbExhibitorPortal.Text))
            sqlComm.Parameters.Add(New SqlParameter("RentalFurnitureNotes", tbFurniture.Text))
            sqlComm.Parameters.Add(New SqlParameter("InternetNotes", tbInternet.Text))
            sqlComm.Parameters.Add(New SqlParameter("LaborNotes", tbLabor.Text))
            sqlComm.Parameters.Add(New SqlParameter("LeadRetrievalNotes", tbLeadRerieval.Text))
            sqlComm.Parameters.Add(New SqlParameter("MaterialNotes", tbMaterials.Text))
            sqlComm.Parameters.Add(New SqlParameter("TierNotes", tbTier.Text))
            sqlComm.Parameters.Add(New SqlParameter("ShippingNotes", tbShipping.Text))
            sqlComm.Parameters.Add(New SqlParameter("InspectionNotes", tbInbound.Text))
            sqlComm.Parameters.Add(New SqlParameter("DrayageNotes", tbDrayage.Text))
            sqlComm.Parameters.Add(New SqlParameter("CarpetCosts", cCarpet.Text))
            sqlComm.Parameters.Add(New SqlParameter("ElectricalCosts", cElectrical.Text))
            sqlComm.Parameters.Add(New SqlParameter("OtherCosts", cExhibitorPortal.Text))
            sqlComm.Parameters.Add(New SqlParameter("RentalFurnitureCosts", cFurniture.Text))
            sqlComm.Parameters.Add(New SqlParameter("InternetCosts", cInternet.Text))
            sqlComm.Parameters.Add(New SqlParameter("LaborCosts", cLabor.Text))
            sqlComm.Parameters.Add(New SqlParameter("LeadRetrievalCosts", cLeadRerieval.Text))
            sqlComm.Parameters.Add(New SqlParameter("MaterialCosts", cMaterials.Text))
            sqlComm.Parameters.Add(New SqlParameter("TierCosts", cTier.Text))
            sqlComm.Parameters.Add(New SqlParameter("ShippingCosts", cShipping.Text))
            sqlComm.Parameters.Add(New SqlParameter("InspectionCosts", cInbound.Text))
            sqlComm.Parameters.Add(New SqlParameter("DrayageCosts", cDrayage.Text))

            sqlComm.Parameters.Add(New SqlParameter("ServiceManagementCosts", cInbound.Text))
            sqlComm.Parameters.Add(New SqlParameter("CCProcessingCosts", cDrayage.Text))


            Try
                sqlComm.ExecuteNonQuery()

                Label1.Text = " Saved"
            Catch ex As Exception
                ErrorEmail(ex.Message)
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

    Sub ErrorEmail(body As String)

        Dim SmtpServer As New SmtpClient(ConfigurationSettings.AppSettings("SMTP"))
        SmtpServer.EnableSsl = False
        'SmtpServer.Credentials = New Net.NetworkCredential("@gmail.com", "xxx")

        Dim mail As New MailMessage("no-reply@yourpinnacle.net", "adamn@pinnacle-exhibits.com", "Error", body)
        mail.From = New MailAddress("no-reply@yourpinnacle.net", "Pinnacle Portables")
        mail.IsBodyHtml = True

        SmtpServer.Send(mail)

    End Sub

    Protected Sub btnPay_Click(sender As Object, e As EventArgs) Handles btnPay.Click
        CalculateTotal()
        If verifyPay.Checked Then


            Dim client As New SquareClient.Builder()

            Dim environment As Square.Environment = Square.Environment.Sandbox
            client.Environment(environment)
            client.AccessToken("EAAAEO-p5QBsW7X6jIAQHyMWMaoG9fAXwGNfS97b79VulpXyuJGaYfrD2lIO12aU")



            Dim paymentAPi As IPaymentsApi = client.Build.PaymentsApi
            Dim uuid = Guid.NewGuid().ToString()

            Dim amt As New Money.Builder
            amt.Amount(Replace(Replace(total.InnerText, ".", ""), ",", ""))
            amt.Currency("USD")

            Dim createPaymentRequest As New CreatePaymentRequest.Builder(cardID.Value, uuid, amt.Build)
            createPaymentRequest.LocationId("B7NEE7W5QFZW7")
            createPaymentRequest.Note("Order# " & pkid)
            createPaymentRequest.CustomerId(custID.Value)

            Try
                Dim rsp = paymentAPi.CreatePayment(createPaymentRequest.Build)
                UpdateCurrentPaid(pkid, total.InnerHtml)
                Label1.Text = "Credit Card Charged"
                btnPay.Visible = False
                verifyPay.Visible = False

                'Generate Invoice

            Catch ex As Exception
                Dim er = TryCast(ex, Square.Exceptions.ApiException)
                'Response.Redirect("Review.aspx?error=" & er.Errors(0).Detail)
                'Response.Write("Error " & ex.Message)
                Label1.Text = er.Errors(0).Detail
            End Try
        Else
            Label1.Text = "Check the box to verify you are ready to take payment."
        End If


    End Sub

    Sub UpdateCurrentPaid(pkid, billed)
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = "Update tblOrders set CurrentBilled=@CurrentBilled where pkorderid=@pkorderid"


           


                sqlComm.Parameters.Add(New SqlParameter("pkorderid", pkid))
            sqlComm.Parameters.Add(New SqlParameter("CurrentBilled", CDbl(billed)))

                Try
                    pkid = sqlComm.ExecuteScalar()
                Catch ex As Exception

                End Try
        End Using
    End Sub

End Class

Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Partial Class Labels
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Session("labels")) Then
            Dim sql = "SELECT tblInventoryType.Description as InventoryType, LEFT(tblInventory.Description, 40) AS Description, tblInventory.PartNum, tblItems.pkItemID, ('Apple') as Name FROM   tblInventory INNER JOIN " &
             "tblItems ON tblInventory.pkInventoryID = tblItems.fkInventoryID INNER JOIN " &
             "tblInventoryType ON tblInventory.fkInventoryTypeID = tblInventoryType.pkInventoryTypeID " &
             "where pkitemid in (" & Session("labels") & ")"

            Dim dt = ReadRecords(sql)
            Repeater1.DataSource = dt
            Repeater1.DataBind()
        End If
    End Sub

    Protected Sub Repeater1_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles Repeater1.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then


            Dim barcode As RadBarcode = TryCast(e.Item.FindControl("RadBarcode2"), RadBarcode)
            barcode.Text = e.Item.DataItem("pkitemid")

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
End Class

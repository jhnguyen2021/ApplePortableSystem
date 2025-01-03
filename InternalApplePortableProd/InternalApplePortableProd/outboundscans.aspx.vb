Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Net.Mail
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Data
Partial Class outboundscans
    Inherits System.Web.UI.Page

    Public _showID, _pkShowInvID, _inventoryID, _jobnum, _partpkid, _partName, _crateskid, _outboundlocation, _inboundlocation, _outboundnotes, _showInvID
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        'lbID.Text = Replace(Request.QueryString("pid"), "'", "''")
        '_pid = lbID.Text
        If Not Request.Cookies("ActiveJob") Is Nothing AndAlso IsNothing(_showID) Then
            _showID = Request.Cookies("ActiveJob").Value

            _pkShowInvID = Replace(Request.QueryString("pkShowInvID"), "'", "''")
            _inventoryID = Replace(Request.QueryString("inventoryID"), "'", "''")
            Dim sql = "Select opha.U_V33_PESubjob, vwshowinventory.*, ISNULL(tblShowInventoryItems.OutboundSkidCrate, vwShowInventory.SkidCrate) AS crateskid, tblShowInventoryItems.Outboundnotes,  ISNULL(tblShowInventoryItems.OutboundLocation, vwShowInventory.Location) AS oloc, ISNULL(tblShowInventoryItems.InboundLocation, vwShowInventory.Location) as iloc from vwShowInventory " & _
                   "INNER JOIN OPHA ON vwShowInventory.fkShowID = OPHA.AbsEntry LEFT OUTER JOIN tblShowInventoryItems ON vwShowInventory.pkID = tblShowInventoryItems.fkInventoryID " & _
                    "WHERE (vwShowInventory.fkShowID = " & _showID & " and vwShowInventory.pkid=" & _inventoryID & ")"
            Dim dt = ReadRecords(sql)
            If dt.Rows.Count > 0 Then
                _jobnum = dt(0)("U_V33_PESubjob").ToString
                _partpkid = dt(0)("pkid").ToString
                _partName = dt(0)("partnum").ToString & " - " & dt(0)("ItemName").ToString
                _crateskid = dt(0)("CrateSkid").ToString
                _inboundlocation = dt(0)("iloc").ToString
                _outboundlocation = dt(0)("oloc").ToString
                _outboundnotes = dt(0)("outboundnotes").ToString
                _showInvID = dt(0)("pkshowInvID").ToString
            End If
        End If

        lbJobNum.Text = _jobnum
        lbID.Text = _partName

        If Not Page.IsPostBack Then
            Try
                txtcrateskid.Text = _crateskid
                txtinboundlocation.Text = _inboundlocation
                txtoutboundlocation.Text = _outboundlocation
                notes.Text = _outboundnotes
            Catch ex As Exception

            End Try


        End If
        If Page.Request.RequestType = "POST" Then
            'Dim pkid = Request.QueryString("id")

            Dim strDate = DateTime.Now.ToString("yyyyMMddhhmmss")
            If HttpContext.Current.Request.Files.Count > 0 Then
                Try

                    Dim targetFolder As String = ""
                    Dim i = 0
                    targetFolder = Server.MapPath("Outbounds/")
                    CheckFolder(targetFolder & lbJobNum.Text & "/")
                    i = CheckFile(targetFolder & lbJobNum.Text & "/" & _partpkid & "-" & i & ".jpg")

                    Dim file = HttpContext.Current.Request.Files(0)
                    file.SaveAs(targetFolder & lbJobNum.Text & "/" & _partpkid & "-" & i & ".jpg")
                    TestRotate(targetFolder & lbJobNum.Text & "/" & _partpkid & "-" & i & ".jpg")


                Catch ex As Exception
                    Response.Write(ex.Message)

                End Try

            End If
            AddNewItem()
            'UpdateInbound(_pid, HttpContext.Current.Request.Form("notes"), GetPictures(_jobnum, _partpkid), HttpContext.Current.Request.Form("txtcrateskid"))
            Dim script1 As String = "RefreshParentPage();"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "RefreshParentPage", script1, True)
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind('navigateToInserted');", True)
        End If
    End Sub

    Function GetPictures(jobnum, partpkid) As String
        Dim f = ""
        Dim targetFolder = Server.MapPath("Outbounds/" & jobnum & "/")
        System.IO.Directory.CreateDirectory(targetFolder)
        Dim fiArr As FileInfo() = New DirectoryInfo(targetFolder).GetFiles()
        For Each fri In fiArr
            If fri.Name.Contains(partpkid) Then
                f = f & fri.Name & ":"
            End If
        Next
        If f.Length > 1 Then
            f = f.Remove(f.Length - 1, 1)
        End If

        Return f
    End Function

    Public Sub CheckFolder(path As String)
        If Not Directory.Exists(path) Then
            Directory.CreateDirectory(path)
        End If
    End Sub

    Function CheckFile(path As String) As Integer

        Try
            Dim num = 0
            While File.Exists(path)
                Dim fileparts = path.Split(".")
                num = CInt(fileparts(0).Substring(fileparts(0).Length - 1)) + 1
                path = fileparts(0).Substring(0, fileparts(0).Length - 1) & num & "." & fileparts(1)

            End While
            Return num
        Catch ex As Exception
            'lbError.Text = ex.Message
        End Try

        Return 0

    End Function

    Sub ResizeImage(filename As String)
        Dim bm As New Bitmap(filename & ".jpg")
        Dim newbm = ResizeImage(bm)
        newbm.Save(filename & ".png", System.Drawing.Imaging.ImageFormat.Png)



        'Dim newbm As Bitmap = DirectCast(bm.Clone(), Bitmap)
        ' bm.Dispose()
        ' newbm.SetResolution(72, 72)

        'Dim bm_final As New Bitmap(newbm)
        ' bm_final.Save(filename & "test.png", System.Drawing.Imaging.ImageFormat.Png)


    End Sub

    Public Shared Function ResizeImage(ByVal InputImage As Image) As Image
        Return New Bitmap(InputImage, New Size(400, 300))
    End Function

    Protected Sub AddNewItem()
        Dim pkid = ""
        Dim statusID = 1005

        Dim UDF1 = Request.Cookies("UserID").Value
        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Inventory").ToString)

            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr


            sqlComm.CommandText = "INSERT INTO tblShowInventoryItems (fkShowInvID,fkInventoryID,DateIn,DateOut,Quantity,OutboundPicture,fkInventoryStatusID, OutboundNotes, OutboundSkidCrate, OutboundLocation, UDF1,fkShowID) VALUES (@fkShowInvID,@fkInventoryID,@DateIn,@DateOut,@Quantity,@OutboundPicture,@fkInventoryStatusID, @OutboundNotes, @OutboundSkidCrate, @OutboundLocation, @UDF1,@fkShowID); SELECT SCOPE_IDENTITY()"
            'sqlComm.CommandText = "Update tblShowInventoryItems Set fkShowInvID=@fkShowInvID,fkInventoryID=@fkInventoryID,DateIn=@DateIn,DateOut=@DateOut,Quantity=@Quantity,Picture=@Picture,fkInventoryStatusID=@fkInventoryStatusID,Notes=@Notes,UDF1=@UDF1 where pkShowInvItemsID=@pkShowInvItemsID; SELECT SCOPE_IDENTITY()"



            sqlComm.Parameters.Add(New SqlParameter("fkShowInvID", _showInvID))
            sqlComm.Parameters.Add(New SqlParameter("fkInventoryID", _inventoryID))
            sqlComm.Parameters.Add(New SqlParameter("DateIn", Now()))
            sqlComm.Parameters.Add(New SqlParameter("DateOut", Now()))
            sqlComm.Parameters.Add(New SqlParameter("Quantity", 1))
            sqlComm.Parameters.Add(New SqlParameter("OutboundPicture", GetPictures(_jobnum, _partpkid)))
            sqlComm.Parameters.Add(New SqlParameter("fkInventoryStatusID", statusID))
            sqlComm.Parameters.Add(New SqlParameter("OutboundNotes", notes.Text))
            sqlComm.Parameters.Add(New SqlParameter("OutboundSkidCrate", txtcrateskid.Text))
            sqlComm.Parameters.Add(New SqlParameter("OutboundLocation", txtinboundlocation.Text))
            'sqlComm.Parameters.Add(New SqlParameter("InboundLocation", ""))
            sqlComm.Parameters.Add(New SqlParameter("UDF1", UDF1))
            sqlComm.Parameters.Add(New SqlParameter("fkShowID", _showID))
            Try
                pkid = sqlComm.ExecuteScalar()
            Catch ex As Exception

            End Try

        End Using

    End Sub

    Private Function ReadRecords(ByVal query As String) As DataTable
        Dim dt As New DataTable
        Try
            Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Inventory").ToString)

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

    Public Function TestRotate(sImageFilePath As String) As Boolean
        Dim rft As RotateFlipType = RotateFlipType.RotateNoneFlipNone
        Dim img As Bitmap = Image.FromFile(sImageFilePath)
        Dim properties As PropertyItem() = img.PropertyItems
        Dim bReturn As Boolean = False
        For Each p As PropertyItem In properties
            If p.Id = 274 Then
                Dim orientation As Short = BitConverter.ToInt16(p.Value, 0)
                Select Case orientation
                    Case 1
                        rft = RotateFlipType.RotateNoneFlipNone
                    Case 3
                        rft = RotateFlipType.Rotate180FlipNone
                    Case 6
                        rft = RotateFlipType.Rotate90FlipNone
                    Case 8
                        rft = RotateFlipType.Rotate270FlipNone
                End Select
            End If
        Next
        If rft <> RotateFlipType.RotateNoneFlipNone Then
            img.RotateFlip(rft)
            System.IO.File.Delete(sImageFilePath)
            img.Save(sImageFilePath, System.Drawing.Imaging.ImageFormat.Jpeg)
            bReturn = True
        End If
        Return bReturn

    End Function
End Class

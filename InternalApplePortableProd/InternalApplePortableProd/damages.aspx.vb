Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Net.Mail
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Data

Partial Class Damages
    Inherits Telerik.Web.UI.RadAjaxPage
    Public _status, _pkid, _pkItemID
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        'lbID.Text = Replace(Request.QueryString("pid"), "'", "''")
        '_pid = lbID.Text
        If Not Request.Cookies("ActiveJob") Is Nothing AndAlso IsNothing(_pkid) Then
            _pkid = Request.Cookies("ActiveJob").Value.Split(":")(0)

        End If
        _pkItemID = Replace(Request.QueryString("itemid"), "'", "''")

        Dim sql = "SELECT *, Description+' '+Type+' '+Partnum as Name from vwOrderInventoryItems where sku=" & _pkItemID & " and pkOrderID=" & _pkid
        Dim dt = ReadRecords(sql)
            If dt.Rows.Count > 0 Then
            lbID.Text = dt(0)("Name").ToString()
            _status = dt(0)("fkItemStatusID").ToString()
            notes.Text = dt(0)("notes").ToString()
            Label1.Text = ShowPictures(dt(0)("picture").ToString().Split(":"))
            End If


        If Not Page.IsPostBack Then
            Try
                ddlStatus.SelectedValue = _status
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
                    targetFolder = Server.MapPath("Inbound/")
                    CheckFolder(targetFolder & _pkid & "/")
                    i = CheckFile(targetFolder & _pkid & "/" & _pkItemID & "-" & i & ".jpg")

                    Dim file = HttpContext.Current.Request.Files(0)
                    file.SaveAs(targetFolder & _pkid & "/" & _pkItemID & "-" & i & ".jpg")
                    TestRotate(targetFolder & _pkid & "/" & _pkItemID & "-" & i & ".jpg")


                Catch ex As Exception
                    Response.Write(ex.Message)

                End Try

            End If

            UpdateOrderItem(False, _pkItemID, 1, HttpContext.Current.Request.Form("ddlStatus"), HttpContext.Current.Request.Form("notes"), GetPictures(_pkid, _pkItemID))
            Dim script1 As String = "RefreshParentPage();"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "RefreshParentPage", script1, True)
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind('navigateToInserted');", True)
        End If
    End Sub

    Function ShowPictures(pics As String())
        Dim imgs = ""
        For Each pic In pics
            imgs = imgs & "<img class='imgs fancybox' src='Inbound/" & _pkid & "/" & pic & " ' />"
        Next

        Return imgs

    End Function

    Function GetPictures(jobnum, partpkid) As String
        Dim f = ""
        Dim targetFolder = Server.MapPath("Inbound/" & jobnum & "/")
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

  
    Sub UpdateOrderItem(insert As Boolean, itemID As Integer, qty As Integer, statusID As Integer, notes As String, picture As String)

        Dim pkid, sql

        If insert Then
            sql = "INSERT INTO [tblOrderInventoryItems] ([fkOrderID],[fkOrderItemsID],[Quantity],[DateOut],[DateIn],[fkItemStatusID],[picture],[notes],[fkItemID]) VALUES(@fkOrderID, @fkOrderItemsID, @Quantity, @DateOut, @DateIn, @fkItemStatusID, @picture, @notes, @fkItemID); SELECT SCOPE_IDENTITY()"
        Else
            sql = "UPDATE [tblOrderInventoryItems]  SET [fkOrderID]=@fkOrderID, [fkOrderItemsID]=@fkOrderItemsID,[Quantity]=@Quantity,[DateIn]=@DateIn,[fkItemStatusID]=@fkItemStatusID,[picture]=@picture,[notes]=@notes,[fkItemID]=@fkItemID WHERE [fkOrderID]=@fkOrderID and [fkItemID]=@fkItemID"
        End If


        Using strConnStr As New SqlConnection(ConfigurationManager.ConnectionStrings("Portables").ToString)
            strConnStr.Open()

            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = strConnStr
            sqlComm.CommandText = sql

            sqlComm.Parameters.Add(New SqlParameter("fkOrderID", _pkid))
            sqlComm.Parameters.Add(New SqlParameter("Quantity", qty))
            sqlComm.Parameters.Add(New SqlParameter("fkOrderItemsID", 0))
            sqlComm.Parameters.Add(New SqlParameter("DateIn", Now()))
            sqlComm.Parameters.Add(New SqlParameter("fkItemStatusID", statusID))
            sqlComm.Parameters.Add(New SqlParameter("picture", picture))

            sqlComm.Parameters.Add(New SqlParameter("notes", notes))
            sqlComm.Parameters.Add(New SqlParameter("fkItemID", itemID))

            Try
                pkid = sqlComm.ExecuteScalar()
            Catch ex As Exception

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

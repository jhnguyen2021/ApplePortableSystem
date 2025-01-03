<%@ Page Language="VB" AutoEventWireup="false" CodeFile="outboundscans.aspx.vb" Inherits="outboundscans" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.10.2.min.js" ></script>
    <style>
        #submit{
            border-width: 1px;
            border-color: #000000;
            border-style: solid;
            padding:5px;
            margin:5px;
            width:100px;
            text-align:center;
            margin-top:50px;
        }


    </style>
</head>
<body >
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <div>
        <div class="left">Job#</div><div class="right"><asp:Label ID="lbJobNum" runat="server"></asp:Label></div>
        <div class="left">Part#</div><div class="right"><asp:Label ID="lbID" runat="server"></asp:Label></div>
        <div class="left">Crate/Skid#</div><div class="right"><asp:TextBox ID="txtcrateskid" runat="server"></asp:TextBox></div>
        <div class="left">Outbound Location</div><div class="right"><asp:TextBox ID="txtoutboundlocation" runat="server"></asp:TextBox></div>
        <div class="left">Return Location</div><div class="right"><asp:TextBox ID="txtinboundlocation" runat="server"></asp:TextBox></div>
        <br />
        <div class="left">Notes</div><asp:TextBox ID="notes" runat="server" TextMode="MultiLine"></asp:TextBox><br />

        <input type="file" accept="image/*" id="target" capture multiple/>
        <div id="submit">Submit</div>
        <script>
            var file = null
            var inventoryID = <%= _inventoryID%>;
            var pkShowInvID = <%= _showID%>;
            $(document).ready(function () {



                $("#target").change(function () {

                    var output = document.getElementById('output');
                    for (var i = 0; i < this.files.length; i++) {

                        file = this.files[i];
                        //output.src = URL.createObjectURL(file);
                        upload(file)
                    }
                    //var file = this.files[0];
                    //upload(file)
                    //drawOnCanvas(file);
                });

                function upload(file) {
                   
                    var form = new FormData(),
                        xhr = new XMLHttpRequest();
                    form.append('image', file);
                    xhr.open('POST', 'outboundscans?inventoryID='+inventoryID+'&pkShowInvID='+pkShowInvID, true);
                    xhr.send(form);

                }

            });

            $("#submit").click(function () {
                valid = true

                if ($('#ddlStatus').val() == '0') {
                    $(this).css('border-color', 'red')
                    valid = false
                } else {
                    $(this).css('border-color', 'white')
                }
                /*
                $('.containerLeft input').each(function () {
                    if ($(this).val() == '') {
                        $(this).css('border-color', 'red')
                        valid = false
                    } else {
                        $(this).css('border-color', 'white')
                    }
                    if (!document.getElementById('CheckBox1').checked) {
                        $('#CheckBox1').parent('label').css("color", "red")
                        valid = false
                    } else {
                        $('#CheckBox1').parent('label').css("color", "#ffffff")
                    }
                    if (!document.getElementById('CheckBox2').checked) {
                        $('#CheckBox2').parent('label').css("color", "red")
                        valid = false
                    } else {
                        $('#CheckBox2').parent('label').css("color", "#ffffff")
                    }
                    if (file == null) {
                        valid = false
                    }

                })
                */
                if (valid) {
                    var form = $('#form1').serialize()
                    //var form = new FormData()
                    //xhr = new XMLHttpRequest();
                    //form.append('fname', $('#fname').val())
                    //form.append('lname', $('#lname').val())
                    //form.append('email', $('#email').val())
                    //form.append('title', '')
                    //form.append('employer', '')
                    //form.append('bdate', document.getElementById('CheckBox1').checked)
                    //form.append('permission', document.getElementById('CheckBox2').checked)
                    //form.append('image', file);
                    //form.append('ddlstatus', $('#ddlStatus').val())
                    //form.append('notes', $('#notes').val())
                    
                    $.post('outboundscans?inventoryID='+inventoryID+'&pkShowInvID='+pkShowInvID, form, function(){
                        RefreshParentPage()
                        GetRadWindow().Close()
                    })
                    /*
                    $.ajax({
                        url: 'damages?pid='+pid+'&sid='+sid,
                        data: form,
                        cache: false,
                        processData: false,
                        contentType: false,
                        type: 'POST',
                        success: function (dataofconfirm) {
                            // do something with the result
                        }
                    });
                    */

                    //xhr.open('post', 'damages.aspx?pid=' + pid, true);
                    //xhr.onload = function () {
                    // do something to response
                    //    console.log(this.responseText);
                    //    location.reload()
                    //};
                    //xhr.send(form);
                    //alert(xhr.responseText)
                    //alert("Sent " + pid)
                }
            })




    </script>
        <script type="text/javascript">


            
            function CloseAndRebind(args) {
                GetRadWindow().Close();

            }

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz az well)
                return oWindow;
            }

            function RefreshParentPage() {
                // GetRadWindow().BrowserWindow.location.reload();
            }
    </script>  
    </div>
    </form>
</body>
</html>

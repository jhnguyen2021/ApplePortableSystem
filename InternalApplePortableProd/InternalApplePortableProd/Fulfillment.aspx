<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Fulfillment.aspx.vb" Inherits="Fulfillment" EnableEventValidation="false" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title></title>
    <script src="js/jquery-1.10.2.min.js"></script>
    <style>
        html, body {
            padding:0px;
            margin:0px;
            font-family:arial;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
           <AjaxSettings>
               <telerik:AjaxSetting AjaxControlID="RadGrid1">
                   <UpdatedControls>
                       <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" />
                   </UpdatedControls>
               </telerik:AjaxSetting>
           </AjaxSettings>
       </telerik:RadAjaxManager>
   <telerik:RadScriptBlock runat="server" ID="RadScriptBlock1" >
     <script type="text/javascript">
         

$('#skuDDL').change(function() {
        alert( "Handler for .change() called." );
    });         


function ShowMessage(id) {
             var oManager = GetRadWindowManager();
             //Get an existing window DialogWindow using getWindowByName
             var oWnd = oManager.getWindowByName("RadWindow");
             if (oWnd == null)
                 var wnd = window.radopen("damages.aspx?itemid=" + id, "RadWindow");
             else {
                 oWnd.closeAll;
                 var wnd = window.radopen("damages.aspx?itemid=" + id, "RadWindow");
             }
             return false;
         }

         function test(id) {
             alert (id)
         }

         function Outbound(pkShowInvID, inventoryID) {
             var oManager = GetRadWindowManager();
             //Get an existing window DialogWindow using getWindowByName
             var oWnd = oManager.getWindowByName("RadWindow");
             if (oWnd == null) {
                 var wnd = window.radopen("outboundscans.aspx?pkShowInvID=" + pkShowInvID + "&inventoryID=" + inventoryID, "RadWindow");
             }
             else {
                 oWnd.closeAll;
                 var wnd = window.radopen("outboundscans.aspx?pkShowInvID=" + pkShowInvID + "&inventoryID=" + inventoryID, "RadWindow");
             }
         }

         function GetRadWindow() {
             var oWindow = null;
             if (window.radWindow)
                 oWindow = window.radWindow;
             else if (window.frameElement && window.frameElement.radWindow)
                 oWindow = window.frameElement.radWindow;
             return oWindow;
         }
         function CloseModal() {
             // GetRadWindow().close();
             setTimeout(function () {
                 GetRadWindow().close();
             }, 0);
         }

         function OnClientClose(sender, args) {
             try {
                 var masterTable1 = $find("<%= RadGrid1.ClientID%>").get_masterTableView();
                masterTable1.rebind();
            } catch (error) { }
            try {
                var masterTable2 = $find("<%= RadGrid2.ClientID %>").get_masterTableView();
                 masterTable2.rebind();
             } catch (error) {  }
         
         
         }

         

         var mql = window.matchMedia("(orientation: portrait)");

         // If there are matches, we're in portrait
         if (mql.matches) {
             // Portrait orientation

         } else {
             // Landscape orientation

         }

         // Add a media query change listener
         mql.addListener(function (m) {
             if (m.matches) {
                // alert('portrait')
                 //$("#mobileCanvas").css({"height":"480px","width":"640px"})
                 $("#pcCanvas").css({ "height": "640px", "width": "480px" })
             }
             else {
                 $("#pcCanvas").css({ "height": "640px", "width": "480px" })
                // alert('landscape')
             }
         });


         $(document).ready(function () {
             $('#TextBox1').focus()

             $('#TextBox1').on("keyup", function () {
                 if (this.value.length > 3) {
                     $('#Button1').trigger('click')
                     
                 }
             })
         })

         var prm = Sys.WebForms.PageRequestManager.getInstance();

         prm.add_endRequest(function () {
             $('#TextBox1').focus()

             $('#TextBox1').on("keyup", function () {
                 if (this.value.length > 3) {
                     $('#Button1').trigger('click')
                     
                 }
             })


         });

     </script>   
        </telerik:RadScriptBlock>
      
        
       
       
        <div style="width:100%;text-align:center;font-weight:bold;margin-top:40px;"><asp:Label id="lbTitle" runat="server" ></asp:Label></div>
        
        <div>
            <div class="select" style="float:left;display:none;">
                <label for="videoSource"></label><select id="videoSource"></select><div id="go" style="display:inline-block;text-decoration:underline;">Read Barcode</div>
            </div>
        
            <div style="text-align:center;display:inline-block;color:red;width:100%"><strong><asp:Label id="lbStatus" runat="server" ></asp:Label></strong></div>
     
            
       </div>







   <div style="width:640px;height:480px;margin-left:auto;margin-right:auto;overflow:hidden;position:relative;">
    <video muted autoplay id="video" playsinline="true" style="position:absolute;z-index:1;" width="640" height="480" ></video>
    <canvas id="pcCanvas"  style="position:absolute;z-index:0;"></canvas>
    <canvas id="mobileCanvas"  style="position:absolute;z-index:0;"></canvas>
  </div>
    
 
        
        <div style="position:absolute;z-index:2;left:0;top:100px;width:100%;">
            <div style="display:inline;">
<%--            <asp:TextBox ID="TextBox1" runat="server" placeholder="Barcode#" ></asp:TextBox><asp:Button ID="Button1" runat="server" Text="Submit"  /><asp:Label id="lbInventoryLink" runat="server" ></asp:Label>--%>
     </div>
                
                <div style="float:right;margin-top: -75px;">
          <asp:LinkButton ID="BackToShipping" runat="server"  class="button" >Back to Shipping</asp:LinkButton>
          <br/>
                <asp:LinkButton ID="LinkButtonOutbound" runat="server" Visible="false" class="button" >Complete Outbound</asp:LinkButton>
                <asp:LinkButton ID="LinkButtonInbound" runat="server" Visible="false" class="button" >Complete Inbound</asp:LinkButton>
            </div>  
        
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" RestoreOriginalRenderDelegate="false">
<%--     <asp:TextBox ID="TextBox1" runat="server" placeholder="Barcode#" ></asp:TextBox><asp:Button ID="Button1" runat="server" Text="Submit"  /><asp:Label id="lbInventoryLink" runat="server" ></asp:Label>--%>
 
    <telerik:RadGrid ID="RadGrid1" runat="server" 
            AutoGenerateColumns="False" OnPreRender="RadGrid1_PreRender" Width="100%" 
            Visible="False" DataMember="InventoryID" ResolvedRenderMode="Classic">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" FileName="Inventory"></ExportSettings>
            <MasterTableView AllowSorting="false" ClientDataKeyNames="pkInventoryID" DataKeyNames="pkInventoryID" CommandItemDisplay="None"  DataMember="pkInventoryID" HierarchyDefaultExpanded="true" Name="MasterTable" Width="100%">
                <CommandItemSettings ShowAddNewRecordButton='false' ></CommandItemSettings>
                    <RowIndicatorColumn >
                    <HeaderStyle Width="20px"></HeaderStyle>
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn>
                    <HeaderStyle Width="20px"></HeaderStyle>
                    </ExpandCollapseColumn>
                        <DetailTables>
                            <telerik:GridTableView AutoGenerateColumns="false" DataKeyNames="pkOrderInventoryItemsID, sku"  Name="DetailTable" HierarchyDefaultExpanded="true" Width="100%">
                                
                                <Columns>
                                    <telerik:GridBoundColumn HeaderText="Barcode" UniqueName="sku" DataField="sku"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="Quantity" HeaderText="Quantity" DataField="Quantity"></telerik:GridBoundColumn>
                                    <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn"></telerik:GridButtonColumn>
                                </Columns>
                            </telerik:GridTableView>
                        </DetailTables>
                    <Columns>
                    <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="Delete" Display="false"></telerik:GridButtonColumn>
                    
                        
                        
                        <telerik:GridTemplateColumn HeaderText="Image" UniqueName="Image" DataField="Picture">
                        <ItemTemplate>
                            <img src='<%# GetImagePath(Eval("Picture"))%>' height='80' alt='<%# GetImagePath(Eval("Picture"))%>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridBoundColumn HeaderText="Name" UniqueName="Name" DataField="Name">
                          
                    </telerik:GridBoundColumn>
                          <telerik:GridTemplateColumn HeaderText="SKU" UniqueName="pkItemID">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="skuDDL" runat="server" Font-Size="X-Small" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" >
                                            <asp:ListItem Value="">-Select-</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                              
                                </telerik:GridTemplateColumn>

                  <telerik:GridButtonColumn  ButtonType="LinkButton" CommandName="Add" Text="Submit" UniqueName="ButtonColumn"></telerik:GridButtonColumn>

                    <telerik:GridBoundColumn HeaderText="Quantity" UniqueName="Quantity" DataField="Quantity">
                    </telerik:GridBoundColumn>
                    
                    </Columns>
</MasterTableView >
    </telerik:RadGrid>
    <telerik:RadGrid ID="RadGrid2" runat="server" AutoGenerateColumns="False" GridLines="None"  Width="100%" Visible="false">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" FileName="Inventory"></ExportSettings>
            <MasterTableView AllowSorting="false" ClientDataKeyNames="pkOrderInventoryItemsID" DataKeyNames="pkOrderInventoryItemsID" CommandItemDisplay="None" >
                <CommandItemSettings  ShowAddNewRecordButton='false' ></CommandItemSettings>
                    <RowIndicatorColumn >
                    <HeaderStyle Width="20px"></HeaderStyle>
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn>
                    <HeaderStyle Width="20px"></HeaderStyle>
                    </ExpandCollapseColumn>
                    <Columns>
                    
                    <telerik:GridBoundColumn HeaderText="Name" UniqueName="Name" DataField="Name" HeaderStyle-Width="200px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="SKU" UniqueName="SKU" DataField="SKU" HeaderStyle-Width="20px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Quantity" UniqueName="Quantity" DataField="Quantity"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Date Out" Datafield="DateOut" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DateOut" ></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Date In" Datafield="DateIn" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DateIn" ></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Status" UniqueName="Status" DataField="Status" Visible="false"></telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" UniqueName="StatusID">
              <ItemTemplate>
                     <a href="javascript:ShowMessage(<%# Eval("SKU") %>)" ><%# Eval("Status") %></a>
              </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn HeaderText="Image" UniqueName="ImageIn" DataField="Picture">
                        <ItemTemplate>
                            <%# GetDamagesImagePath(Eval("picture")) %>
                            
                            
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Image" UniqueName="ImageOut" DataField="Picture" Visible="false">
                        <ItemTemplate>
                            <img src='<%# GetImagePath(Eval("InvPicture"))%>' height='80' alt='<%# GetImagePath(Eval("InvPicture"))%>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn HeaderText="Notes" UniqueName="Notes" DataField="Notes"></telerik:GridBoundColumn>
                    </Columns>
</MasterTableView >
    </telerik:RadGrid>
</telerik:RadAjaxPanel>
        </div>


     <telerik:RadWindowManager ID="RadWindowManager" Width="600" Height="600" VisibleStatusbar="false" OnClientClose="OnClientClose"
                Behaviors="Close,Move,Resize" runat="server" ></telerik:RadWindowManager> 
    <div id="divInfo1" runat="server" visible="false">Scan an item to add to this order.</div>
    <audio id="beep">
        <source src="beep.mp3" type="audio/mpeg">
  Your browser does not support the audio tag.
        </audio>    
    </form>
    <script async src="js/zxing.js"></script>
    <script src="js/video2.js"></script>
    <script>

        $(document).ready(function () {

            $("#go").click(function () {
                $("#beep")[0].play();
            })
        });

    </script>

</body>
</html>
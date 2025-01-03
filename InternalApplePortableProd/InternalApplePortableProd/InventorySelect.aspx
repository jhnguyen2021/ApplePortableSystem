<%@ Page Title="Select Inventory" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="InventorySelect.aspx.vb" Inherits="InventorySelect" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <script type="text/javascript" src="/fancybox/jquery.fancybox-1.3.4.pack.js"></script>
     <script src="https://polyfill.io/v3/polyfill.min.js?features=default"></script>

    <link rel="stylesheet" type="text/css" href="./style.css" />
    <script type="module" src="./index.js"></script>

<link rel="stylesheet" type="text/css" href="Scripts/jquery.fancybox.css" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
     <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
         <script type="text/javascript">

             var a = $telerik.$;
             var uploadsInProgress = 0;


             function onFileSelected(sender, args) {
                 if (!uploadsInProgress)
                     a("#SaveButton").attr("disabled", "disabled");

                 uploadsInProgress++;
             }
             function onFileUploaded(sender, args) { 
                 decrementUploadsInProgress();
             }

             function onUploadFailed(sender, args) {
                 decrementUploadsInProgress();
             }

             function decrementUploadsInProgress() {
                 uploadsInProgress--;

                 if (!uploadsInProgress)
                     a("#SaveButton").removeAttr("disabled");
             }

             $(window).load(function () {
                 

                 $(".fancybox").click(function () {
                     $.fancybox.open(this.src);
                     
                 });

                 $('.imgcontainer').find('img').each(function () {
                     var imgClass = (this.width / this.height > 1) ? 'wide' : 'tall';
                     $(this).addClass(imgClass);
                 })
                
             });


             //function resizeGrid() {
             //    $("#nav").show();
             //    $(".results").css('height', "38vh");
             //}

             var prm = Sys.WebForms.PageRequestManager.getInstance();

             prm.add_endRequest(function () {
                 
                <%-- document.getElementById("<%= tbSRegion.ClientID%>").style.display = 'none';
                 document.getElementById("<%= cbSState.ClientID%>").style.display = 'inline-block';--%>
                $(".fancybox").click(function () {
                    $.fancybox.open(this.src);
                    
                });
                $('.imgcontainer').find('img').each(function () {
                    var imgClass = (this.width / this.height > 1) ? 'wide' : 'tall';
                    $(this).addClass(imgClass);
                })
            });
                 
             
          function OnPickupClientSelectedIndexChanged(sender, eventArgs) {
                 var item = eventArgs.get_item();
             }


window.onbeforeunload = function(){
        return'You must click "Buy Now" to make payment and finish your order. If you leave now your order will be canceled.';
};

 function Click1(button)
        {
            window.onbeforeunload = null;
        }

 function StandardConfirm(sender, args) {
window.onbeforeunload = null;
    args.set_cancel(!window.confirm('Are you sure you want to cancel all changes?'));
}
           

         </script>
    </telerik:RadScriptBlock>   
    <style runat="server">
        .entry{
            display:inline-block;
            height:auto;
            width:500px;
            margin-left:50px;
            vertical-align:top;
        }
        h2 {
            font-size:25px;
            margin-left:-30px;
            padding-bottom:0px;
            margin-bottom:0px;
        }
        .search {
            height:75px;
            width:200px;
            border-radius:20px;
        }
        .results {
            width:auto;
            background-color:#ededed;
            border: 1px solid #dfdfdf;
            padding:30px;
            margin-left:30px;
            margin-right:175px;
            overflow: auto;
           height: auto;
        }
        .imgcontainer {
            width: 80px;
            height: 80px;
            /*background: #fff;*/
            margin: 0 auto;
        }
        .imgcontainer img.wide {
            max-width: 100%;
            position: relative;
            top: 50%;
            transform: translateY(-50%);
        }
        .imgcontainer img.tall {
            max-height: 100%;
            max-width: 100%;
            width: auto;
            display:block;
            margin:auto;
        }

        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 300px;
            height: 140px;
        }

        .hidden {
            display:none;
        }

        .txtBox {
         width:242px;
         border-color: #8e8e8e #b8b8b8 #b8b8b8 #8e8e8e;
         background: #fff;
        color: #333;
         font: 12px "segoe ui",arial,sans-serif;
        }

      /*  #nav {
            right:5px; top:0px; background-color:White;width:88.46vw; margin-left: 30px; overflow: auto; height: 20vh;
        }*/

        
    </style>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <%--<telerik:AjaxSetting AjaxControlID="tbBoothSize">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="orderGrid" UpdatePanelCssClass="" />

                    <telerik:AjaxUpdatedControl ControlID="nav1" UpdatePanelCssClass="" />

                </UpdatedControls>ship
            </telerik:AjaxSetting>--%>
            <telerik:AjaxSetting AjaxControlID="Grid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="orderGrid" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="orderGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Grid1" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <asp:HiddenField ID="fkstatusID" runat="server" /><asp:HiddenField ID="fkUserID" runat="server" />
        


    
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server"  Width="100%" RestoreOriginalRenderDelegate="false">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ValidationGroup="ValidateFields"/>   
    <br />
    <br />
    
    <table class="entry" style="height:80px;margin-top:50px;display:none;">
        <tr>
            <td><telerik:RadTextBox ID="tbSearch" runat="server" CssClass="search" Height="40px" Width="350px" EmptyMessage="SEARCH" BackColor="#ededed" Font-Size="18px" ></telerik:RadTextBox><telerik:RadButton ID="RadButton1" runat="server" Text="Search" Height="40px" style="margin-left:20px;" RenderMode="Lightweight"></telerik:RadButton></td>
        </tr>
    </table>

        <table runat="server" style="height:80px;font-family: 'Segoe UI',Arial,Helvetica,sans-serif;font-size:12px;padding:5px;line-height:16px;margin-left:30px;">
        <tr>
            <td>Your fulfillment location has been selected based on your event location. To view available inventory from other sites, please select an alternate site.</td>
             
            
        </tr>
            <tr>
                <td><telerik:RadComboBox ID="dlfullfillmentLocation"  runat="server" width="200px" AutoPostBack="true"  AllowCustomText="False" OnSelectedIndexChanged="dlfullfillmentLocation_SelectedIndexChanged">
                       
                    </telerik:RadComboBox></td>
            </tr>
    </table>


         <div style="margin-left:35px;">
           <asp:Label ID="errorMessage" ForeColor="Red" runat="server" />
            </div>
      <div style="margin-left:35px;"><h3>AVAILABLE INVENTORY</h3></div>  
        <div runat="server" id="results" class="results">
        <telerik:RadGrid ID="Grid1" runat="server" AutoGenerateColumns="False" EnableViewState="true" GridLines="None" >
                        <MasterTableView AllowSorting="True" ClientDataKeyNames="pkInventoryID" DataKeyNames="pkInventoryID, Picture, PartNum, Description, Type" >
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                 

                                <telerik:GridTemplateColumn DataField="Picture" HeaderText="Image" 
                                    UniqueName="Picture">
                                    <ItemTemplate>
                                         <div class="imgcontainer"><img src='https://assets.yourpinnacle.net/CL1001/IMG/<%# (Eval("Picture"))%>' height='80' alt='<%# (Eval("Picture"))%>' class="fancybox"/></div>
                                    </ItemTemplate>
                                    <HeaderStyle  HorizontalAlign="Center"  />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="Type" HeaderText="Type" UniqueName="Type" ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle  HorizontalAlign="Center"  />
                                </telerik:GridBoundColumn>
                                 <telerik:GridBoundColumn DataField="Description" HeaderText="Description" 
                                    UniqueName="Description">
                                     <HeaderStyle HorizontalAlign="Center"  />
                                </telerik:GridBoundColumn>
                                  <telerik:GridBoundColumn DataField="deptdesc2" HeaderText="Department" UniqueName="Department" AllowSorting="true" SortExpression="deptdesc2">
                                </telerik:GridBoundColumn>

                                 <telerik:GridTemplateColumn UniqueName="Location" HeaderText="Item Location" DataField="Location" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <%# (Eval("Location"))%>  
            </ItemTemplate>
                                      <HeaderStyle HorizontalAlign="Center"  />
        </telerik:GridTemplateColumn>    
                               
                                <telerik:GridBoundColumn DataField="PartNum" HeaderText="Part#" 
                                    UniqueName="PartNum">
                                </telerik:GridBoundColumn> 
                                 <%--<telerik:GridBoundColumn DataField="Total" HeaderText="Total Inventory Amount"  ItemStyle-HorizontalAlign="Center"
                                    UniqueName="Total" ><HeaderStyle Width="150px" />
                                </telerik:GridBoundColumn>--%>

                                 <telerik:GridTemplateColumn UniqueName="TotalInventory" HeaderText="Total Inventory Amount" ItemStyle-HorizontalAlign="Center" >
            <ItemTemplate>
  <%# If(Eval("TotalInventory") > 0, Eval("TotalInventory"), "Out of Stock") %> 
            </ItemTemplate>
                                     <HeaderStyle Width="150px"  HorizontalAlign="Center"   />
        </telerik:GridTemplateColumn>

                                 <telerik:GridTemplateColumn UniqueName="QuantityAvailableByDate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Available Inventory" >
            <ItemTemplate>
  <%# If(Eval("QuantityAvailableByDate") > 0, Eval("QuantityAvailableByDate"), "Out of Stock") %> 
            </ItemTemplate>
        </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Quantity" UniqueName="Quantity">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="myDD" runat="server" Font-Size="X-Small" AutoPostBack="true"  OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
    </div>
    
<%--             <div style="background-color:#ddd;font-size:14px;text-align:center;margin-left: 30px; margin-right:175px;" id="nav1" runat="server">Inventory Options Below</div>--%>

      <div style="margin-left:35px;"><h3>YOUR ITEMS</h3></div> 
    
      <div  style="margin-left:30px;width:30%">
        <div runat="server" id="qtyChanged" style="font-family: 'Segoe UI',Arial,Helvetica,sans-serif;font-size:12px;padding:5px;line-height:16px;"><img style="height:20px;vertical-align:middle;" src="https://assets.yourpinnacle.net/CL1001/WarningTriangleYellow.png" alt="Italian Trulli"/>&nbsp;&nbsp;<span>Based on availability, certain item quantities have been reduced for the revised dates.</span></div>
                  <telerik:RadGrid ID="qtyChangedGrid" runat="server" AutoGenerateColumns="False"  
                        EnableViewState="false" GridLines="None" Skin="Default">
                      <MasterTableView AllowSorting="True" ClientDataKeyNames="PartNum" DataKeyNames="PartNum,Name,PreviousQuantity, NewQuantity" >
                            <RowIndicatorColumn>
                                <HeaderStyle Width="10px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                             <Columns>
                                 <telerik:GridBoundColumn DataField="PartNum" HeaderText="Part #" UniqueName="PartNum"> 
                                </telerik:GridBoundColumn>
                                 <telerik:GridBoundColumn DataField="PreviousQuantity" HeaderText="Previous Cart Quantity" UniqueName="PreviousQuantity">
                                      <ItemStyle HorizontalAlign="Center"  />
                                     <HeaderStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                 <telerik:GridBoundColumn DataField="NewQuantity" HeaderText="New Cart Quantity" UniqueName="NewQuantity"> 
                                       <ItemStyle HorizontalAlign="Center"  />
                                     <HeaderStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                             </Columns>
                        </MasterTableView>
                      </telerik:RadGrid>
    </div>
    
    <div style="margin-left:30px;width:30%">
                <div runat="server" id="qty0" style="font-family: 'Segoe UI',Arial,Helvetica,sans-serif;font-size:12px;padding:5px;line-height:16px;"><img style="height:20px;vertical-align:middle;" src="https://abportables.yourpinnacle.net/Img/WarningHexagonRed.png" alt="Italian Trulli"/>&nbsp;&nbsp;<span>Based on availability, certain items are no longer available on the revised dates.</span></div>
                   <telerik:RadGrid ID="qty0Grid" runat="server" AutoGenerateColumns="False" 

                        EnableViewState="false" GridLines="None" Skin="Default">
                      <MasterTableView AllowSorting="True" ClientDataKeyNames="PartNum" DataKeyNames="PartNum,Name,PreviousQuantity, NewQuantity" >
                            <RowIndicatorColumn>
                                <HeaderStyle Width="10px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                             <FooterStyle BackColor="#cc6633"></FooterStyle>
                             <Columns>
                                 <telerik:GridBoundColumn DataField="PartNum" HeaderText="Part #" UniqueName="PartNum"> 
                                </telerik:GridBoundColumn>
                                
                                 <telerik:GridBoundColumn DataField="PreviousQuantity" HeaderText="Previous Cart Quantity" UniqueName="PreviousQuantity">
                                      <ItemStyle HorizontalAlign="Center"  />
                                     <HeaderStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                 <%--<telerik:GridBoundColumn DataField="NewQuantity" HeaderText="New Cart Quantity" UniqueName="NewQuantity"> 
                                       <ItemStyle HorizontalAlign="Center"  />
                                     <HeaderStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>--%>

                                  <telerik:GridTemplateColumn UniqueName="NewQuantity" HeaderText="New Cart Quantity" >
            <ItemTemplate>
  <%# If(Eval("NewQuantity") > 0, Eval("NewQuantity"), 0) %> 
            </ItemTemplate>
                                      <ItemStyle HorizontalAlign="Center"  />
                                     <HeaderStyle HorizontalAlign="Center" />
        </telerik:GridTemplateColumn>
                             </Columns>
                        </MasterTableView>
                      </telerik:RadGrid>
        </div>


  <div id="nav" style="top:0px; background-color:White;margin-left: 30px; margin-right:175px; overflow: auto; height: 20vh;margin-top:30px;">
                    
                    <telerik:RadGrid ID="orderGrid" runat="server" AutoGenerateColumns="False" OnItemCreated="orderGrid_ItemCreated"
                        EnableViewState="false" GridLines="None" Skin="Default">
                        <MasterTableView AllowSorting="True" ClientDataKeyNames="pkInventoryID" DataKeyNames="pkInventoryID, Quantity, PartNum, Picture, Description, Type" >
                            <RowIndicatorColumn>
                                <HeaderStyle Width="10px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>

                                   <telerik:GridTemplateColumn HeaderText="Alerts">
                       <ItemTemplate>
                           <asp:Image ID="Image1" runat="server" style="height:20px;"/>
                       </ItemTemplate>
                                    <HeaderStyle Width="50px" />
                                     <ItemStyle HorizontalAlign="Center"  />
                                     <HeaderStyle HorizontalAlign="Center" />
                   </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn  DataField="PartNum" HeaderText="Part #"  UniqueName="PartNum" > <HeaderStyle Width="150px" /></telerik:GridBoundColumn>

                                <telerik:GridTemplateColumn DataField="pkInventoryID" HeaderText="Image" HeaderStyle-HorizontalAlign="Center" UniqueName="Picture">
                                    <ItemTemplate>
                                        <div class="imgcontainer"><img src='https://assets.yourpinnacle.net/CL1001/IMG/<%# (Eval("Picture"))%>' height='80' alt='<%# (Eval("pkInventoryID"))%>' class="fancybox"/></div>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                           <telerik:GridBoundColumn  DataField="Description" HeaderText="Description"  UniqueName="Description" > <HeaderStyle /></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Qty" UniqueName="Qty">
                                     <HeaderStyle  Width="100px" />
                                    <ItemTemplate>
                                        <asp:DropDownList ID="myQty"  DataField="Quantity" runat="server" Font-Size="X-Small" AutoPostBack="true" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                               <telerik:GridBoundColumn Display="false" DataField="Type" HeaderText="Type" UniqueName="Type">
                                </telerik:GridBoundColumn>

                                <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="DeleteOrder" Text="X" UniqueName="ButtonColumn"><HeaderStyle Width="20px" /></telerik:GridButtonColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings> 
                            <Scrolling AllowScroll="True"  UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="500px" ></Scrolling> 
                        </ClientSettings>
                    </telerik:RadGrid>
        
               
     </div>
         </telerik:RadAjaxPanel>
    <%--<h3>Session Idle:&nbsp;<span id="secondsIdle"></span>&nbsp;seconds.</h3>--%>
<asp:LinkButton ID="lnkFake" runat="server" />
<%--<cc1:modalpopupextender ID="mpeTimeout" BehaviorID ="mpeTimeout" runat="server" PopupControlID="pnlPopup" TargetControlID="lnkFake"
    OkControlID="btnYes" CancelControlID="btnNo" BackgroundCssClass="modalBackground" OnOkScript = "ResetSession()">
</cc1:modalpopupextender>
<asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
    <div class="header">
        Session Expiring!
    </div>
    <div class="body">
        Your Session will expire in&nbsp;<span id="seconds"></span>&nbsp;seconds.<br />
        Do you want to extend your session?
    </div>
    <div class="footer" align="right">
        <asp:Button ID="btnYes" runat="server" Text="Yes" CssClass="yes" />
        <asp:Button ID="btnNo" runat="server" Text="No" CssClass="no" />
    </div>
 </asp:Panel>--%>
    <telerik:RadButton ID="RadButton2" runat="server" Text="Review Order" Height="40px" style="margin-left:50px;margin-top:50px;margin-bottom:50px;" RenderMode="Lightweight" OnClientClicked="Click1"  ValidationGroup="ValidateFields"></telerik:RadButton>
    <telerik:RadButton ID="RadButton3" runat="server" Text="Cancel Changes" OnClientClicking="StandardConfirm" Height="40px" style="margin-left:50px;margin-top:50px;margin-bottom:50px;" RenderMode="Lightweight" ></telerik:RadButton>
    <asp:Label ID="lbError" ForeColor="Red" runat="server" />
</asp:Content>


 <%@ Page Title="Review" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Review.aspx.vb" Inherits="Review" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <script type="text/javascript" src="Scripts/jquery-1.7.2.min.js"></script>
<script src="Scripts/jquery.fancybox.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="Scripts/jquery.fancybox.css" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <style runat="server" id="css">
        .entry{
            display:inline-block;
            height:auto;
            width:500px;
            margin-left:50px;
            vertical-align:top;
        }
        h2 {
            font-size:25px;
            
            padding-bottom:0px;
            margin-bottom:0px;
        }
        .search {
            height:75px;
            width:200px;
            border-radius:20px;
        }
        .results {
            width:90%;
            background-color:#ededed;
            border: 1px solid #dfdfdf;
            padding:30px;
            margin-left:auto;
            margin-right:auto;
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
        .orders {
            width:90%;
            /*background-color:#ededed;*/
            border: 1px solid #dfdfdf;
            padding:30px;
            margin-left:40px;
            font-size:0.8vw;
        }

        #RadGrid1 {
            margin-top: 100px;
        }

        .hidden {
            display:none;
        }

        .services {
            display:<%= services %>;
        }
        .portal {
            display:<%= portal %>;
        }

        .log {
            border:1px solid black;
        }

       #rbNotes {
           text-align: center;
       }

       .orderContainer {
           display:inline-flex;width: 100%;
       }

       .shippingAndChangeHistory {
            display:inline-flex;width: 100%;

       }

      #shipmentBoxes {
          font-size: 100px;
      }

       

       @media (max-width: 1029px ) {
            .orderContainer {
           display:block;
           width: 100%;
       }
       }

    </style>
    <script>
        $(document).ready(function () {
            $(".fancybox").click(function () {
                $.fancybox.open(this.src);
            });

            $('.imgcontainer').find('img').each(function () {
                var imgClass = (this.width / this.height > 1) ? 'wide' : 'tall';
                $(this).addClass(imgClass);
            })
        });
        function handleEvent(sender, eventArgs) {
            var value = sender.get_value();
            type = getCreditCardType(value);

            switch (type) {
                case "mastercard":
                    //show MasterCard icon
                    $("#ccVISA").css({ opacity: 0.4 });
                    $("#ccMAST").css({ opacity: 1.0 });
                    $("#ccAMEX").css({ opacity: 0.4 });
                    //$("#ccDISC").css({ opacity: 0.4 });
                    break;

                case "visa":
                    //show Visa icon
                    $("#ccVISA").css({ opacity: 1.0 });
                    $("#ccMAST").css({ opacity: 0.4 });
                    $("#ccAMEX").css({ opacity: 0.4 });
                    //$("#ccDISC").css({ opacity: 0.4 });
                    break;

                case "amex":
                    //show American Express icon
                    $("#ccVISA").css({ opacity: 0.4 });
                    $("#ccMAST").css({ opacity: 0.4 });
                    $("#ccAMEX").css({ opacity: 1.0 });
                    //$("#ccDISC").css({ opacity: 0.4 });
                    break;
                case "disc":
                    //show American Express icon
                    $("#ccVISA").css({ opacity: 0.4 });
                    $("#ccMAST").css({ opacity: 0.4 });
                    $("#ccAMEX").css({ opacity: 0.4 });
                    //$("#ccDISC").css({ opacity: 1.0 });
                    break;

                default:
                    $("#ccVISA").css({ opacity: 0.5 });
                    $("#ccMAST").css({ opacity: 0.5 });
                    $("#ccAMEX").css({ opacity: 0.5 });
                    //$("#ccDISC").css({ opacity: 0.5 });
            }
        }


    function getCreditCardType(accountNumber) {

        //start without knowing the credit card type
        var result = "unknown";

        //first check for MasterCard
        if (/^5[1-5]/.test(accountNumber)) {
            result = "mastercard";
        }

            //then check for Visa
        else if (/^4/.test(accountNumber)) {
            result = "visa";
        }

            //then check for AmEx
        else if (/^3[47]/.test(accountNumber)) {
            result = "amex";
        }

        // else if (/^6/.test(accountNumber)) {
        //     result = "disc";
        // }

        return result;
    }
    
    
    function Validate() {
        var isValid = false;
        isValid = Page_ClientValidate('agree');
        if (isValid) {
            isValid = Page_ClientValidate('cc');
        }
        return isValid;
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

function CancelPrompt() {
 window.onbeforeunload = null;
}



    </script>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

   


    <asp:HiddenField ID="lbboothsize" runat="server" />
        
        <div style="width:90%;height:65px;">
               <telerik:RadButton ID="RadButton1" 
                   runat="server" Text="Edit Order" Height="40px" style="margin-left:60px;margin-top:10px;margin-bottom:50px;float:right;" RenderMode="Lightweight"  ValidationGroup="cc"></telerik:RadButton>
<%--         <telerik:RadButton ID="RadButton1" runat="server" Text="Edit Order" Height="40px" style="margin-right:30px;width:110px;float:right;margin-top:10px;margin-bottom:50px;" RenderMode="Lightweight"  ValidationGroup="cc"></telerik:RadButton>--%>
                    <telerik:RadButton ID="RadButton3" runat="server" Text="Cancel Changes" OnClientClicking="StandardConfirm" Height="40px" style="margin-left:60px;margin-top:10px;margin-bottom:50px;float:right;" RenderMode="Lightweight"  ValidationGroup="cc"></telerik:RadButton>
                                    <telerik:RadButton ID="RadButton2" runat="server" Text="Complete Order" Height="40px" style="width:110px;float:right;margin-top:10px;margin-bottom:50px;" RenderMode="Lightweight" OnClientClicked="Click1"  ValidationGroup="cc"></telerik:RadButton>

            </div>
       

    <div class="orders" >
        <div class="orderContainer" runat="server">
       
        
        <div runat="server" id="order" style="width:75%;">
        <table cellspacing="0" cellpadding="0" border="0" >
            <tr><td style="vertical-align:top;">
     <table style="display:inline-block;height:auto;width:auto;margin-left:10px;vertical-align:top;padding-right:110px;">
        <tr><td colspan="2"><h2>Show Information</h2></td></tr>
        <tr><td>Event Name:</td><td>[EventName]</td></tr>
        <tr><td>Apple Contact Name</td><td>[ContactName]</td></tr>
        <tr><td>Apple Contact Email:</td><td>[ContactEmail]</td></tr>
        <tr><td>Apple Contact Phone #:</td><td>[ContactPhone]</td></tr>
        <tr><td>Event Start Date:</td><td>[DateStart]</td></tr>
        <tr><td style="width:200px;">Event End Date:</td><td>[DatePickup]</td></tr>
    </table>
    </td>
    <td>
    <table style="display:inline-block;height:350px;width:auto;vertical-align:top;">
        <tr>
          <td colspan="2"><h2>Shipping Information</h2></td>
        </tr>
            <tr>
                <td>Ship to:</td>
                <td>[Shipto]</td>
            </tr>
          <tr>
                <td>Booth #:</td>
                <td>[Boothnum]</td>
            </tr>
            <tr>
                <td>Onsite Contact Name:</td>
                <td>[ShipContactName]</td>   
            </tr>
             <tr>
                <td>Onsite Contact Phone #:</td>
                <td>[ShipContactPhone]</td>
            </tr>
            <tr>
                <td>Onsite Contact Email:</td>
                <td>[ShipContactEmail]</td>
            </tr>
            <tr>
                <td>Address Line 1:</td>
                <td>[ShipAddress1]</td>
            </tr>
            <tr>
                <td>Address Line 2:</td>
                <td>[ShipAddress2]</td>
            </tr>
            <tr>
                <td>City:</td>
                <td>[ShipCity]</td>
            </tr>
            <tr>
                <td valign="top">
                    <div style="vertical-align:top;">State/Province/Region:</div>
                </td>
                <td>[ShipState]</td>
            </tr>
            <tr>
                <td>ZIP/Postal Code:</td>
                <td>[ShipZip]</td>
            </tr>
            <tr>
                <td style="width:200px">
                    Country:</td>
                <td>[ShipCountry]</td>
            </tr>
            <tr>
                <td>Asset Arrival Date:</td>
                <td>[DateArrive]</td>
            </tr>
        <tr>
                <td>Shipping Type:</td>
                <td>[ShippingType]</td>
            </tr>
         <tr>
                <td>Tracking#:</td>
                <td style="max-width:150px;overflow-wrap:break-word;">[TrackingNumber]</td>
            </tr>
         <tr>
                <td>FulfillmentLocation:</td>
                <td>[fkFulfillmentLocationId]</td>
            </tr>
      
                  <tr><td style="white-space: nowrap;width:200px;vertical-align:top;">Attach Document(s):<div style="width:200px"></div></td><td>[Attachments]</td></tr>
    </table>
     </td>
    </tr>
              <tr>
          <td>
         </td>
     </tr>
            </table>
    <br />
        
         <table style="display:inline-block;width:60%;">
        <tr>
            <td><h2>Additional Information</h2></td>
        </tr>
        <tr>
            <td>[Notes]</td>
        </tr>
    </table>     
            
            
           

    </div>




            <div runat="server" id="shippingEstimate" style=" float:right;margin-left:30px;">
         <div><h2>Shipping Estimates</h2></div>
        <table  cellspacing="0" cellpadding="0" border="0" runat="server" style="float: left;width:55%;font-family: 'Segoe UI',Arial,Helvetica,sans-serif;font-size:0.8vw;border-spacing: 0 5px;">
        <tr>
                <td>Total number of items:</td>
                <td>
                    <div id="totalNumberItems" runat="server" style="margin-left:10px;">
                         [totalNumberItems]
                     </div>
                    </td>
            </tr>
          <tr>
                <td>Total number of shipment boxes:</td>
                <td>
                    <div id="totalNumberBoxes" runat="server" style="margin-left:10px;">
                         [totalNumberBoxes]
                     </div>
                    </td>
            </tr>
    
        
        
        
        
        </table>



        <telerik:RadGrid ID="shipmentBoxes" style="border: none;width: 75%;border-spacing: 0 5px;" runat="server" AutoGenerateColumns="false">
        <MasterTableView AllowSorting="false" DataKeyNames="Cartonname, Cost" ShowFooter="true" ShowHeader="false" ShowGroupFooter="true" >
                            <Columns>
                               <%-- <telerik:GridBoundColumn DataField="CartonName" HeaderText="" UniqueName="CartonName">
                                    <HeaderStyle Width="300px" />
                                </telerik:GridBoundColumn>--%>

                            
                                   <telerik:GridTemplateColumn UniqueName="CartonName" HeaderText="" DataField="CartonName">
            <ItemTemplate>


                  <%# If(Eval("CartonName") <> "Black Banner Case", Eval("TbTQty") & " " & "Table Throws", "1 Black Banner Case, 1 Self-standing Frame") %> 


<%--                 <%# If(Eval("CartonName") = "Black Banner Case", "1 Black Banner Case, 1 Self-standing Frame", Eval("TbTQty")) & " " & "Table Throws" %> --%>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
                                

                                  <telerik:GridTemplateColumn UniqueName="CartonName" HeaderText="" DataField="">
            <ItemTemplate>
                 <%# Eval("Item1")   %> ,   <%#   Eval("Item2") %> 
            </ItemTemplate>
        </telerik:GridTemplateColumn>



       <%-- <telerik:GridNumericColumn  Aggregate="Sum" HeaderText="Total Shipping Cost" UniqueName="Cost" DataField="Cost"  NumericType="Currency" DataType="System.Decimal"  ReadOnly="true" FooterAggregateFormatString="Total shipping cost: {0:C}">            <FooterStyle HorizontalAlign="Left" />
                    <FooterStyle Width="300px" HorizontalAlign="Left" />
        </telerik:GridNumericColumn>--%>
                            </Columns>
                        </MasterTableView>
    </telerik:RadGrid>


        <table  cellspacing="0" cellpadding="0" border="0" runat="server" style="float: left;width:65%;font-family: 'Segoe UI',Arial,Helvetica,sans-serif;font-size:0.8vw;border-spacing: 0 5px;">
               
             <tr id="totalShippingEstimate" runat="server">
                <td ><b>Total Round Trip Shipping Estimates:</b></td>
                <td>
                    <div id="totalShippingCost" runat="server" style="margin-left:10px;">
                                        <b>[totalShippingCost]</b> 
                    </div>
                    </td>
            </tr>

            <tr id="contactInfo" runat="server">
                <td><b>Please contact <u>recruiting_events@apple.com</u> to get a round trip shipping estimate to the location specified in this order.</b></td>
            </tr>
            
    </table>

<table  cellspacing="0" cellpadding="0" border="0" runat="server" style="float: left;font-family: 'Segoe UI',Arial,Helvetica,sans-serif;font-size:0.8vw;border-spacing: 0 15px;">
                      <tr><td style="color:rgb(128, 128, 128);font-size: 12px;">*Please note that the round trip shipping estimates are approximate and may be subject to variations.</td></tr>         
       
    </table>
    </div>


    </div>

        <div class="shippingAndChangeHistory">

            <div id="changeHistory" style="width:40%;" >
             
                
                <h2>Order Change History</h2>
              <telerik:RadGrid ID="OrdersNote" Style="margin-top:50px;width: 95%; overflow: auto;" runat="server" AutoGenerateColumns="false">
        <MasterTableView AllowSorting="false"  >
                            <Columns>
                                <telerik:GridBoundColumn DataField="dateTime" HeaderText="Date/Time" UniqueName="DateCreated">
                                    <HeaderStyle Width="75px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="User" HeaderText="User" 
                                    UniqueName="User">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="logsNote" HeaderText="Notes" 
                                    UniqueName="logsNote">
                                </telerik:GridBoundColumn> 
                                <telerik:GridBoundColumn DataField="FulfillmentLocation" HeaderText="FulfillmentLocation" 
                                    UniqueName="FulfillmentLocation">
                                </telerik:GridBoundColumn> 
                            </Columns>
                        </MasterTableView>
    </telerik:RadGrid>


            </div>
                    <div runat="server" id="tblDecision" style="font-size:0.8vw;width:100%;">
         <div><h2 style="margin-left:30px;">Approval Process</h2></div>
        <table  cellspacing="0" cellpadding="0" border="0" runat="server" style="float: left;font-family: Arial;color:#353535;  border-spacing: 0 0px;margin-left:35px;">
        <tr runat="server">
           <td style="width:160px;">Actions:</td>
            <td><asp:RadioButtonList ID="rbApproveReject" OnSelectedIndexChanged="rbApproveReject_SelectedIndexChanged" onchange="CancelPrompt()"   runat="server"  style="display:inline-block;padding-right: 15px;" 
                                         AutoPostBack="true"       RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True" Value="Approve">Approve</asp:ListItem>
                                                <asp:ListItem Value="Reject">Reject</asp:ListItem>
            </asp:RadioButtonList> 
            </td>
            </tr>
            <tr id="reasonsforRejection" runat="server">
                <td runat="server" style="display:block;margin-top:3px;margin-left:-5px;width:160px;">*Reason for Rejection:</td>
            <td style="width:400px;">
                <asp:RadioButtonList ID="rbl" runat="server" RepeatDirection="vertical" TextAlign="Right" onchange="CancelPrompt()" OnSelectedIndexChanged="rbl_SelectedIndexChanged" AutoPostBack="true"  >
                     <asp:ListItem Selected="True" Value="More information required. Please reach out directly to the person who rejected the request.">&nbsp;&nbsp;More information required. Please reach out directly <br /> <span style="margin-left:31px;">to the person who rejected the request.</span></asp:ListItem>
                     <asp:ListItem Value="Cost of shipping cannot be justified.">&nbsp;&nbsp;Cost of shipping cannot be justified.</asp:ListItem>
                     <asp:ListItem Value="Equipment requested is not appropriate for the event.">&nbsp;&nbsp;Equipment requested is not appropriate for the event.</asp:ListItem>
                     <asp:ListItem Value="Other">&nbsp;&nbsp;Other </asp:ListItem>
                </asp:RadioButtonList>
            </td>

                </tr>
           
            <tr>
                <td>

                </td>
                <td style="padding-left:30px;padding-bottom:10px;">
                          <telerik:RadTextBox id="rejectionsNote"  runat="server" margin-left="30px;"
  TextMode="MultiLine"
  Rows="3" 
  Wrap="true"
                        width="460">
</telerik:RadTextBox>
                  
                </td>
            </tr>

            <tr id="txtRejectionReason" runat="server">
                <td></td>
              <td style="color:rgb(255,0,0);font-size: 16px;white-space: nowrap;">* Injection Reasons.</td>
            </tr>
            <tr>
                <td style="width:160px;">
                    Action taken by:
                </td>
                 <td>
                     <div id="adminName" runat="server" style="margin-left:10px;">
                         [AdminContactName]
                     </div>
                </td>
            </tr>

            <tr>
                <td style="width:160px;">
                    Email ID:
                </td>
                 <td>
                     <div id="adminEmail" runat="server" style="margin-left:10px;">
                         [AdminContactEmail]
                     </div>
                     </td>
            </tr>

            <tr>
                <td>
            </td>
                <td>
            <telerik:RadButton ID="RadButton5" runat="server" Text="Submit" Height="30px" style="margin-left:20px;width:70px;margin-top:30px;" RenderMode="Lightweight"  ValidationGroup="cc" OnClientClicked="Click1"></telerik:RadButton>
            </td>
        </tr>
    </table>
        
    </div>

            

        </div>

    </div>

    <div class="orders">
     <telerik:RadAjaxPanel ID="RadAjaxPanel6" runat="server" HorizontalAlign="NotSet"  width="100%" RestoreOriginalRenderDelegate="false" >
    <telerik:RadGrid Style="margin-top: 30px" ID="RadGrid1" runat="server" AutoGenerateColumns="false" >
        <MasterTableView AllowSorting="false"  ClientDataKeyNames="pkInventoryID" DataKeyNames="pkInventoryID, Quantity" ShowFooter="true" >
                            <Columns>
                               
                                <telerik:GridTemplateColumn DataField="Picture" HeaderText="Image" UniqueName="Picture">
                                    <ItemTemplate>
                                         <div class="imgcontainer"><img src='https://assets.yourpinnacle.net/CL1001/IMG/<%# (Eval("Picture"))%>' height='80' alt='<%# (Eval("Picture"))%>' class="fancybox"/></div>
                                    </ItemTemplate>
                                      <HeaderStyle  HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="Type" HeaderText="Type" UniqueName="Type">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Description" HeaderText="Description" 
                                    UniqueName="Description">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="location" HeaderText="Item Location" 
                                    UniqueName="location">
                                </telerik:GridBoundColumn>
                                 <%--<telerik:GridTemplateColumn UniqueName="Fulfillment" HeaderText="Item Location" DataField="fkDepartmentID">
            <ItemTemplate>
                 <%# If(Eval("fkDepartmentID") <> 1004, "US", "APAC") %>   
            </ItemTemplate>
        </telerik:GridTemplateColumn>    --%>
                                <telerik:GridBoundColumn DataField="PartNum" HeaderText="Part#" 
                                    UniqueName="Dimensions">
                                </telerik:GridBoundColumn> 
                                <telerik:GridBoundColumn DataField="Quantity" HeaderText="Quantity" 
                                    UniqueName="Quantity" >
                                </telerik:GridBoundColumn>
                                
                            </Columns>
                        </MasterTableView>



    </telerik:RadGrid>


                     <table class="entry" style="display:none;">
        
        <tr>
              <td>
                   <input class="txtBox" id="fulfillmentLocationHolder" name="fulfillmentLocationHolder" runat="server"  Visible="false" />
                </td>
        </tr>
            <tr>
              <td>
                   <input class="txtBox" id="assetArrivalDateHolder" name="assetArrivalDateHolder" runat="server"  Visible="false" />
                </td>
        </tr>
                          <tr>
              <td>
                   <input class="txtBox" id="AURegionalAdmin" name="AURegionalAdmin" runat="server"  Visible="false" />
                </td>
                              <td>
                   <input class="txtBox" id="CNRegionalAdmin" name="CNRegionalAdmin" runat="server"  Visible="false" />
                </td>
                              <td>
                   <input class="txtBox" id="JPRegionalAdmin" name="JPRegionalAdmin" runat="server"  Visible="false" />
                </td>

                 <td>
                   <input class="txtBox" id="contactEmail" name="contactEmail" runat="server"  Visible="false" />
                </td>

                               <td>
                   <input class="txtBox" id="dateModified" name="dateModified" runat="server"  Visible="false" />
                </td>
        </tr>
    </table>

                                </telerik:RadAjaxPanel>

    </div>


    <br />
        <div class="orders"  style="display:none;">
                <table style="width: 800px;" border="0" cellpadding="1" cellspacing="2" runat="server" id="cctable">
                
                <tr>
                <td nowrap="nowrap"><h3>Payment Method</h3></td>
                </tr>
                <tr>
                    <td nowrap="nowrap">&nbsp; Credit Card:</td>
                    <td colspan="3">
                        <div class="txt_result"><img src="img/ccVISA.png" id="ccVISA" /><img src="img/ccMAST.png" id="ccMAST" /><img id="ccAMEX" src="img/ccAMEX.png" /><img id="ccDISC" src="img/ccDISC.png" style="display:none;"/></div>
                    </td>
                </tr>
                <tr>
                    <td nowrap="nowrap">&nbsp; Card Number:</td>
                    <td>
                        <telerik:RadMaskedTextBox ID="tbCC" Runat="server" 
                            Mask="#### #### #### ####" ControlToValidate="tbCC">
                            <ClientEvents OnBlur="handleEvent" OnKeyPress="handleEvent" />
                        </telerik:RadMaskedTextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbCC" Enabled="false"
                            ValidationExpression="^(3[47][0-9]{13}|5[1-5][0-9]{14}|4[0-9]{12}(?:[0-9]{3})?)$" ErrorMessage="Valid Credit Card Number Required" 
                            ForeColor="Red" ValidationGroup="cc">*</asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="rfvCreditCardNumber" runat='server' ControlToValidate="tbCC" 
    ErrorMessage="Credit Card Number Required" ForeColor="Red" ValidationGroup="cc">*</asp:RequiredFieldValidator>
                    </td>
                    <td nowrap="nowrap">&nbsp; Security Code:</td>
                    <td>
                    <div class="txt_result">
                        <telerik:RadMaskedTextBox ID="tbCCCode" Runat="server" Mask="####" Width="50px">
                        </telerik:RadMaskedTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat='server' ControlToValidate="tbCCCode" 
    ErrorMessage="Credit Card Security Code Required" ForeColor="Red" ValidationGroup="cc">*</asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td nowrap="nowrap">&nbsp; Expires:</td>
                    <td>
                    <div class="txt_result"><asp:DropDownList ID="ddlMonths" runat="server">
                        <asp:ListItem Selected="True">-</asp:ListItem>
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                        <asp:ListItem>9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        </asp:DropDownList> / 
                        <asp:DropDownList ID="ddlYears" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlYears"
                            ErrorMessage="Required Expiration Year" InitialValue="-" ForeColor="Red" ValidationGroup="cc">*</asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlMonths"
                            ErrorMessage="Required Expiration Month" InitialValue="-" ForeColor="Red" ValidationGroup="cc">*</asp:RequiredFieldValidator>
                        </div>
                    </td>
                    <td nowrap="nowrap">
                        &nbsp; </td>
                    <td>
                    <div class="txt_result"></div>
                    </td>
                </tr>
                <tr>
                <td><h3>Billing Address</h3></td>
                </tr>
                <tr>
                    <td nowrap="nowrap">
                    &nbsp; First Name:</td>
                    <td><telerik:RadTextBox ID="tbccFname" Runat="server" Width="100%">
                        </telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="First Name" ControlToValidate="tbccFname" ForeColor="Red" ValidationGroup="cc">*</asp:RequiredFieldValidator>
                    </td>
                    <td nowrap="nowrap">
                    &nbsp; Last Name:</td>
                    <td>
                    <div class="txt_result">
                        <telerik:RadTextBox ID="tbccLname" Runat="server" Width="100%">
                        </telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Last Name" ControlToValidate="tbccLname" ForeColor="Red" ValidationGroup="cc">*</asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td valign="top" nowrap="nowrap">
                    &nbsp; Address:</td>
                    <td colspan="3">
                    <div class="txt_result">
                        <telerik:RadTextBox ID="tbccAddress1" Runat="server" Width="100%">
                        </telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Address" ControlToValidate="tbccAddress1" ForeColor="Red" ValidationGroup="cc">*</asp:RequiredFieldValidator>
                        </div>
                    </td>
                    
                </tr>
                <tr>
                    <td nowrap="nowrap">
                        &nbsp; </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="tbccAddress2" Runat="server" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                    
                </tr>
                <tr>
                    <td nowrap="nowrap">
                    &nbsp; City:</td>
                    <td>
                    <div class="txt_result">
                        <telerik:RadTextBox ID="tbccCity" Runat="server" Width="100%">
                        </telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="City" ControlToValidate="tbccCity" ForeColor="Red" ValidationGroup="cc">*</asp:RequiredFieldValidator>
                        </div>
                    </td>
                    <td nowrap="nowrap">
                    &nbsp; State:</td>
                    <td>
                    <div class="txt_result">
                        <telerik:RadComboBox ID="ddlccstate" Runat="server" Width="100%" MarkFirstMatch="True">
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="State" ControlToValidate="ddlccstate" ForeColor="Red" ValidationGroup="cc" InitialValue="Select...">*</asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td nowrap="nowrap">
                    &nbsp; Zip Code:</td>
                    <td>
                    <div class="txt_result">
                        <telerik:RadMaskedTextBox ID="tbccZip" Runat="server" 
                            Mask="#####-####" Width="100%">
                        </telerik:RadMaskedTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Zip" ControlToValidate="tbccZip" ForeColor="Red" ValidationGroup="cc">*</asp:RequiredFieldValidator>
                        </div>
                    </td>
                    <td nowrap="nowrap"></td>
                    <td></td>
                </tr>
                
            </table>
            
            </div>

</asp:Content>


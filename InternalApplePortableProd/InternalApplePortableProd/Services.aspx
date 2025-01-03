<%@ Page Language="VB" MasterPageFile="~/Site.master"  AutoEventWireup="false" CodeFile="Services.aspx.vb" Inherits="Services" Title="Services" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <script type="text/javascript" src="Scripts/jquery-1.7.2.min.js"></script>
<script src="Scripts/jquery.fancybox.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="Scripts/jquery.fancybox.css" />
    <style>
        .entry {
            width:600px;
            height:30px;
        }
        .cost {
            text-align:right;
            width:70px;
        }
        .float {
            float:left;
            margin-left:10px;
            margin-right:10px;
            margin-bottom:10px;
            border: 1px solid #f1f1f1;
            padding: 10px;

        }
        .button {
            margin-left:40px;
            height:30px;
            Width:60px;
        }

        table { 
            border-collapse: collapse; 
        }
        .top_border {
            border-top: 1px solid #666666;
            border-bottom: 2px solid #666666;
            height:36px;
        }
        .top_spacer {
            margin-top:10px;
            padding-top:10px;
        }
        h3{
            margin:0px;
        }
    </style>
    <telerik:RadScriptBlock runat="server">
        <script>
            function update_total(){
                var cElectrical = $find("<%= cElectrical.ClientID%>");
                var cInternet = $find("<%= cInternet.ClientID%>");
                var cCarpet = $find("<%= cCarpet.ClientID%>");
                var cLeadRerieval = $find("<%= cLeadRerieval.ClientID%>");
                var cMaterials = $find("<%= cMaterials.ClientID%>");
                var cLabor = $find("<%= cLabor.ClientID%>");
                var cFurniture = $find("<%= cFurniture.ClientID%>");
                var cExhibitorPortal = $find("<%= cExhibitorPortal.ClientID%>");
                var cTier = $find("<%= cTier.ClientID%>");
                var cInbound = $find("<%= cInbound.ClientID%>");
                var cShipping = $find("<%= cShipping.ClientID%>");
                var cDrayage = $find("<%= cDrayage.ClientID%>");
                var total = (cElectrical.get_value()||0) + (cInternet.get_value()||0) + (cCarpet.get_value()||0) + (cLeadRerieval.get_value()||0) + (cMaterials.get_value()||0) + (cLabor.get_value()||0) + (cFurniture.get_value()||0) + (cExhibitorPortal.get_value()||0) + (cShipping.get_value() || 0) + (cDrayage.get_value() || 0)

                var nomarkup = (cInbound.get_value() || 0) + (cTier.get_value() || 0)

                document.getElementById("<%= servicemanagement.ClientID%>").innerHTML = total * 0.3
                
                if (document.getElementById("<%= ccprocessing.ClientID%>")!=null) {
                    document.getElementById("<%= ccprocessing.ClientID%>").innerHTML = total * 0.03
                    document.getElementById("<%= total.ClientID%>").innerHTML = (total * 0.3) + (total * 0.03) + total + nomarkup
                    document.getElementById("<%= btnPay.ClientID()%>").disabled = true
                } else {
                    document.getElementById("<%= total.ClientID%>").innerHTML = (total * 0.3) + total + nomarkup
                }
                
                

            }

        </script>
    </telerik:RadScriptBlock>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <div style="margin-left:20px;width:1000px;">
        <div><h2>Event Information<asp:Button ID="Button1" runat="server" Text="Save" CssClass="button" CausesValidation="false"/><asp:Label ID="Label1" runat="server" ></asp:Label></h2></div>
        <div><asp:Label runat="server" ID="lbshowinfo" ></asp:Label></div><br />
        
        <table border="0" >
            <tr>
                <td>Electrical</td>
                <td><asp:TextBox ID="tbElectrical" runat="server" TextMode="MultiLine" CssClass="entry"></asp:TextBox><asp:RequiredFieldValidator ID="vElectrical" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="tbElectrical" >*</asp:RequiredFieldValidator></td>
                <td><telerik:RadNumericTextBox ID="cElectrical" runat="server" Culture="en-US" DbValueFactor="1" LabelWidth="64px" Type="Currency" Width="70px" ClientEvents-OnValueChanged="update_total">
<NegativeStyle Resize="None"></NegativeStyle>

<NumberFormat ZeroPattern="$n"></NumberFormat>

<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<EnabledStyle Resize="None"></EnabledStyle>
        </telerik:RadNumericTextBox> <asp:RequiredFieldValidator ID="vElectricalc" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="cElectrical" >*</asp:RequiredFieldValidator></td>
                <td></td>
            </tr>
            <tr>
                <td>Internet</td>
                <td><asp:TextBox ID="tbInternet" runat="server" TextMode="MultiLine" CssClass="entry"></asp:TextBox><asp:RequiredFieldValidator ID="vInternet" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="tbInternet">*</asp:RequiredFieldValidator></td>
                <td><telerik:RadNumericTextBox ID="cInternet" runat="server" Culture="en-US" DbValueFactor="1" LabelWidth="64px" Type="Currency" Width="70px" ClientEvents-OnValueChanged="update_total" >
<NegativeStyle Resize="None"></NegativeStyle>

<NumberFormat ZeroPattern="$n"></NumberFormat>

<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<EnabledStyle Resize="None"></EnabledStyle>
        </telerik:RadNumericTextBox><asp:RequiredFieldValidator ID="vInternetC" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="cInternet" >*</asp:RequiredFieldValidator></td>
                <td></td>
            </tr>
            <tr>
                <td>Carpet</td>
                <td><asp:TextBox ID="tbCarpet" runat="server" TextMode="MultiLine" CssClass="entry"></asp:TextBox><asp:RequiredFieldValidator ID="vCarpet" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="tbCarpet" >*</asp:RequiredFieldValidator></td>
                <td><telerik:RadNumericTextBox ID="cCarpet" runat="server" Culture="en-US" DbValueFactor="1" LabelWidth="64px" Type="Currency" Width="70px" ClientEvents-OnValueChanged="update_total" >
<NegativeStyle Resize="None"></NegativeStyle>

<NumberFormat ZeroPattern="$n"></NumberFormat>

<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<EnabledStyle Resize="None"></EnabledStyle>
        </telerik:RadNumericTextBox><asp:RequiredFieldValidator ID="vCarpetC" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="cCarpet" >*</asp:RequiredFieldValidator></td>
                <td></td>
            </tr>
            <tr>
                <td>Lead Rerieval</td>
                <td><asp:TextBox ID="tbLeadRerieval" runat="server" TextMode="MultiLine" CssClass="entry"></asp:TextBox><asp:RequiredFieldValidator ID="vLeadRetrieval" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="tbLeadRerieval" >*</asp:RequiredFieldValidator></td>
                <td><telerik:RadNumericTextBox ID="cLeadRerieval" runat="server" Culture="en-US" DbValueFactor="1" LabelWidth="64px" Type="Currency" Width="70px" ClientEvents-OnValueChanged="update_total" >
<NegativeStyle Resize="None"></NegativeStyle>

<NumberFormat ZeroPattern="$n"></NumberFormat>

<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<EnabledStyle Resize="None"></EnabledStyle>
        </telerik:RadNumericTextBox><asp:RequiredFieldValidator ID="vLeadRetrievalC" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="cLeadRerieval" >*</asp:RequiredFieldValidator></td>
                <td></td>
            </tr>
            <tr>
                <td>Materials</td>
                <td><asp:TextBox ID="tbMaterials" runat="server" TextMode="MultiLine" CssClass="entry"></asp:TextBox><asp:RequiredFieldValidator ID="vMaterials" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="tbMaterials" >*</asp:RequiredFieldValidator></td>
                <td><telerik:RadNumericTextBox ID="cMaterials" runat="server" Culture="en-US" DbValueFactor="1" LabelWidth="64px" Type="Currency" Width="70px" ClientEvents-OnValueChanged="update_total" >
<NegativeStyle Resize="None"></NegativeStyle>

<NumberFormat ZeroPattern="$n"></NumberFormat>

<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<EnabledStyle Resize="None"></EnabledStyle>
        </telerik:RadNumericTextBox><asp:RequiredFieldValidator ID="vMaterialsC" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="cMaterials" >*</asp:RequiredFieldValidator></td>
                <td></td>
            </tr>
            <tr>
                <td>Labor</td>
                <td><asp:TextBox ID="tbLabor" runat="server" TextMode="MultiLine" CssClass="entry"></asp:TextBox><asp:RequiredFieldValidator ID="vLabor" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="tbLabor" >*</asp:RequiredFieldValidator></td>
                <td><telerik:RadNumericTextBox ID="cLabor" runat="server" Culture="en-US" DbValueFactor="1" LabelWidth="64px" Type="Currency" Width="70px" ClientEvents-OnValueChanged="update_total" >
<NegativeStyle Resize="None"></NegativeStyle>

<NumberFormat ZeroPattern="$n"></NumberFormat>

<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<EnabledStyle Resize="None"></EnabledStyle>
        </telerik:RadNumericTextBox><asp:RequiredFieldValidator ID="vLaborC" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="cLabor" >*</asp:RequiredFieldValidator></td>
                <td></td>
            </tr>
            <tr>
                <td>Rental Furniture</td>
                <td><asp:TextBox ID="tbFurniture" runat="server" TextMode="MultiLine" CssClass="entry"></asp:TextBox><asp:RequiredFieldValidator ID="vFurniture" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="tbFurniture" >*</asp:RequiredFieldValidator></td>
                <td><telerik:RadNumericTextBox ID="cFurniture" runat="server" Culture="en-US" DbValueFactor="1" LabelWidth="64px" Type="Currency" Width="70px" ClientEvents-OnValueChanged="update_total" >
<NegativeStyle Resize="None"></NegativeStyle>

<NumberFormat ZeroPattern="$n"></NumberFormat>

<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<EnabledStyle Resize="None"></EnabledStyle>
        </telerik:RadNumericTextBox><asp:RequiredFieldValidator ID="vFurnitureC" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="cFurniture" >*</asp:RequiredFieldValidator></td>
                <td></td>
            </tr>
             <tr>
                <td>Exhibitor Portal</td>
                <td><asp:TextBox ID="tbExhibitorPortal" runat="server" TextMode="MultiLine" CssClass="entry"></asp:TextBox><asp:RequiredFieldValidator ID="vExhibitorPortal" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="tbExhibitorPortal" >*</asp:RequiredFieldValidator></td>
                <td><telerik:RadNumericTextBox ID="cExhibitorPortal" runat="server" Culture="en-US" DbValueFactor="1" LabelWidth="64px" Type="Currency" Width="70px" ClientEvents-OnValueChanged="update_total" >
<NegativeStyle Resize="None"></NegativeStyle>

<NumberFormat ZeroPattern="$n"></NumberFormat>

<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<EnabledStyle Resize="None"></EnabledStyle>
        </telerik:RadNumericTextBox><asp:RequiredFieldValidator ID="vExhibitorPortalC" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="cExhibitorPortal" >*</asp:RequiredFieldValidator></td>
                <td></td>
            </tr>
             
             
             <tr>
                <td>Shipping</td>
                <td><asp:TextBox ID="tbShipping" runat="server" TextMode="MultiLine" CssClass="entry"></asp:TextBox><asp:RequiredFieldValidator ID="vShipping" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="tbShipping" >*</asp:RequiredFieldValidator></td>
                <td><telerik:RadNumericTextBox ID="cShipping" runat="server" Culture="en-US" DbValueFactor="1" LabelWidth="64px" Type="Currency" Width="70px" ClientEvents-OnValueChanged="update_total"  >
<NegativeStyle Resize="None"></NegativeStyle>

<NumberFormat ZeroPattern="$n"></NumberFormat>

<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<EnabledStyle Resize="None"></EnabledStyle>
        </telerik:RadNumericTextBox><asp:RequiredFieldValidator ID="vShippingC" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="cShipping" >*</asp:RequiredFieldValidator></td>
                <td></td>
            </tr>
            <tr>
                <td>Drayage</td>
                <td><asp:TextBox ID="tbDrayage" runat="server" TextMode="MultiLine" CssClass="entry"></asp:TextBox><asp:RequiredFieldValidator ID="vDrayage" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="tbDrayage" >*</asp:RequiredFieldValidator></td>
                <td><telerik:RadNumericTextBox ID="cDrayage" runat="server" Culture="en-US" DbValueFactor="1" LabelWidth="64px" Type="Currency" Width="70px" ClientEvents-OnValueChanged="update_total"  >
<NegativeStyle Resize="None"></NegativeStyle>

<NumberFormat ZeroPattern="$n"></NumberFormat>

<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<EnabledStyle Resize="None"></EnabledStyle>
        </telerik:RadNumericTextBox><asp:RequiredFieldValidator ID="vDrayageC" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="cDrayage" >*</asp:RequiredFieldValidator></td>
                <td></td>
            </tr>
            <tr>
                <td>Admin</td>
                <td><asp:TextBox ID="tbTier" runat="server" TextMode="MultiLine" CssClass="entry"></asp:TextBox><asp:RequiredFieldValidator ID="vTier" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="tbTier" >*</asp:RequiredFieldValidator></td>
                <td><telerik:RadNumericTextBox ID="cTier" runat="server" Culture="en-US" DbValueFactor="1" LabelWidth="64px" Type="Currency" Width="70px" ClientEvents-OnValueChanged="update_total" >
<NegativeStyle Resize="None"></NegativeStyle>

<NumberFormat ZeroPattern="$n"></NumberFormat>

<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<EnabledStyle Resize="None"></EnabledStyle>
        </telerik:RadNumericTextBox><asp:RequiredFieldValidator ID="vTierC" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="cTier" >*</asp:RequiredFieldValidator></td>
                <td></td>
            </tr>
            <tr>
                <td>Outbound/Inbound/Inspection</td>
                <td><asp:TextBox ID="tbInbound" runat="server" TextMode="MultiLine" CssClass="entry"></asp:TextBox><asp:RequiredFieldValidator ID="vInbound" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="tbInbound" >*</asp:RequiredFieldValidator></td>
                <td><telerik:RadNumericTextBox ID="cInbound" runat="server" Culture="en-US" DbValueFactor="1" LabelWidth="64px" Type="Currency" Width="70px" ClientEvents-OnValueChanged="update_total" >
<NegativeStyle Resize="None"></NegativeStyle>

<NumberFormat ZeroPattern="$n"></NumberFormat>

<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<EnabledStyle Resize="None"></EnabledStyle>
        </telerik:RadNumericTextBox><asp:RequiredFieldValidator ID="vInboundC" runat="server" ErrorMessage="" Enabled="false" ForeColor="Red" ControlToValidate="cInbound" >*</asp:RequiredFieldValidator></td>
                <td></td>
            </tr>
            <tr style="height:36px">
                <td>Service Management</td>
                <td></td>
                <td><div id="servicemanagement" runat="server" class="cost"></div></td>
                <td>&nbsp;</td>
            </tr>
            <tr style="height:36px">
                <td>CC Processing</td>
                <td></td>
                <td><div id="ccprocessing" runat="server" class="cost" visible="false"></div></td>
                <td>&nbsp;</td>
            </tr>
            <tr class="top_border">
                <td colspan="2" ><span>Customer:<asp:Label ID="lbCustomer" runat="server" /></span>
            <span style="margin-left:50px;">Credit Card:<asp:Label ID="lbCard" runat="server"  /></span><asp:HiddenField  runat="server" ID="cardID"></asp:HiddenField><asp:HiddenField runat="server" ID="custID"></asp:HiddenField></td>
                <td><div id="total" runat="server" class="cost"></div>
                </td>
                <td><div id="Paid" runat="server" ></div></td>
            </tr>
            <tr class="top_spacer">
                <td colspan="2" class="top_spacer"><asp:CheckBox ID="verifyPay" runat="server" style="float:right" Text="Are you ready to bill the credit card?"/></td>
                <td class="top_spacer"><asp:button ID="btnPay" runat="server" Text="Charge Card" CausesValidation="true"/></td>
                <td></td>
            </tr>
        </table>
        <div>
            
        </div>

    </div>
</asp:Content>

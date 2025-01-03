<%@ Page Title="Place Order" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Place.aspx.vb" Inherits="PlaceTest" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
     <script src="https://polyfill.io/v3/polyfill.min.js?features=default"></script>
    <link
      href="https://fonts.googleapis.com/css?family=Roboto:400,500"
      rel="stylesheet"
    />

    <!-- playground-hide -->
    <script>
        const process = { env: {} };
        process.env.GOOGLE_MAPS_API_KEY =
            "AIzaSyD7PN-w7LeWkaRXjw_FhP8pgp0PXlf3WKM";
    </script>
    <!-- playground-hide-end -->
    
<%--    <link rel="stylesheet" type="text/css" href="./style.css" />--%>
   <%-- <script type="module" src="./index.js"></script>--%>

     <script
      src="https://maps.googleapis.com/maps/api/js?key=AIzaSyD7PN-w7LeWkaRXjw_FhP8pgp0PXlf3WKM&callback=initAutocomplete&libraries=places&v=weekly"
      defer
    ></script>

<link rel="stylesheet" type="text/css" href="Scripts/jquery.fancybox.css" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
     <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
         <script type="text/javascript">
             
             var autocomplete;
             var address1Field;
             var address2Field;
             var postalField;

 var dateChanged = false;
 var fulfillmentLocationChanged = false
            var sDate;
            var eDate;
            var aDate;


             var a = $telerik.$;
             var uploadsInProgress = 0;

             var limitNum = 1000;
             var message = 'Please limit to' + limitNum + ' characters!';
             /*var counter = $get('counter');*/
             

             function LimitLength(mode) {
                 
                 var rtfEditor = $find("<%=tbNotes.ClientID %>");
                 
                 if (mode == 2) {
                     var oValue = rtfEditor.get_textArea().innerHTML.trim().length;
                 }
                 else
                 {
                     var oValue = rtfEditor.get_html(true).trim();
                 }

                 if (oValue.length >= limitNum) {
                     rtfEditor.set_html(oValue.substring(0, limitNum - 1));
                     alert(message);
                 }

                 var counter = $get("counter");
                 counter.innerHTML = " Characters Remaining: " + (limitNum - oValue.length);

             }
             

             function AttachHandlers(mode) {

                 var rtfEditor = $find("<%=tbNotes.ClientID %>");
               
                 if (mode == 1) {
                     rtfEditor.attachEventHandler("onkeyup", LimitLength);
                     rtfEditor.attachEventHandler("onpaste", LimitLength);
                     rtfEditor.attachEventHandler("onblur", LimitLength);
                 }
                 else {
                     var textarea = rtfEditor.get_textArea();

                     if (window.attachEvent) {
                         textarea.attachEvent("onkeydown", LimitLength);
                         textarea.attachEvent("onpaste", LimitLength);
                         textarea.attachEvent("onblur", LimitLength);
                     }
                     else {
                         textarea.addEventListener("keyup", LimitLength, true);
                         textarea.addEventListener("paste", LimitLength, true);
                         textarea.addEventListener("blur", LimitLength, true);
                     }
                 }
             }

             function OnClientLoad(editor, args) {

                 rtfEditor = editor;
                 AttachHandlers(1);
                 LimitLength(1);

                 editor.add_modeChange(function (sender, args) {
                     var mode = sender.get_mode();
                     if (mode == 1 || mode == 2) {
                         AttachHandlers(mode);
                         LimitLength(mode);
                     }
                 });
             }


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

//$("#ctl00_MainContent_cbSCountry_Input").on("blur input", function () {
//       document.querySelector("#MainContent_shipCountry").value = $("#ctl00_MainContent_cbSCountry_Input").val()
//      });
                

                 $(".fancybox").click(function () {
                     $.fancybox.open(this.src);
                 });

                 $('.imgcontainer').find('img').each(function () {
                     var imgClass = (this.width / this.height > 1) ? 'wide' : 'tall';
                     $(this).addClass(imgClass);
                 })

                sDate = $("#ctl00_MainContent_tbStartDate_dateInput").val()
                  eDate = $("#ctl00_MainContent_tbPickupDate_dateInput").val()
                  aDate = $("#ctl00_MainContent_tbArrivalDate_dateInput").val()
                
             });

             var prm = Sys.WebForms.PageRequestManager.getInstance();

             prm.add_endRequest(function () {
              
                 $(".fancybox").click(function () {
                     $.fancybox.open(this.src);
                 });
                 $('.imgcontainer').find('img').each(function () {
                     var imgClass = (this.width / this.height > 1) ? 'wide' : 'tall';
                     $(this).addClass(imgClass);
                 })
                 window.initAutocomplete();

               <%--  var hdShipTo = document.getElementById("<%=hdShipTo.ClientID %>").value
                 var hdOnsiteEmail = document.getElementById("<%=hdOnsiteEmail.ClientID %>").value
                 var hdOnsiteContact = document.getElementById("<%=hdOnsiteContact.ClientID %>").value
                 var hdOnsitePhone = document.getElementById("<%=hdOnsitePhone.ClientID %>").value

                 var ShipTo = document.getElementById("<%=tbShipTo.ClientID %>").value
                 var OnsiteEmail = document.getElementById("<%=tbSOnsiteEmail.ClientID %>").value
                 var OnsiteContact = document.getElementById("<%=tbSOnsiteContact.ClientID %>").value
                 var OnsitePhone = document.getElementById("<%=tbSOnsitePhone.ClientID %>").value--%>
                 

        <%--  if (hdShipTo != '') {
                     document.getElementById("<%=tbShipTo.ClientID %>").value = hdShipTo
                 }
                   
                 if (hdOnsiteEmail != '') {
                     document.getElementById("<%=tbSOnsiteEmail.ClientID %>").value = hdOnsiteEmail
                 }
                 if (hdOnsiteContact != '' ) {
                     document.getElementById("<%=tbSOnsiteContact.ClientID %>").value = hdOnsiteContact
                 }
                 if (hdOnsitePhone != '') {
                     document.getElementById("<%=tbSOnsitePhone.ClientID %>").value = hdOnsitePhone
                 }--%>

                  <%--   document.getElementById("<%=tbShipTo.ClientID %>").value = hdShipTo
                     document.getElementById("<%=tbSOnsiteEmail.ClientID %>").value = hdOnsiteEmail
                     document.getElementById("<%=tbSOnsiteContact.ClientID %>").value = hdOnsiteContact
                     document.getElementById("<%=tbSOnsitePhone.ClientID %>").value = hdOnsitePhone--%>
             });


             function PreventPostback() {
                 window.initAutocomplete();
             }
            

             function validateOrderQuantity() {
                 document.getElementById("<%=CheckBox1.ClientID %>").checked = true
                 FillBilling()
             }


             function FillBilling() {

                 if (document.getElementById("<%=CheckBox1.ClientID %>").checked == true) {
                    $telerik.$('#<%=tbPickupfrom.ClientID %>').val(document.getElementById("<%=tbShipTo.ClientID %>").value)
                    $telerik.$('#<%=tbPOnsiteContact.ClientID %>').val(document.getElementById("<%=tbSOnsiteContact.ClientID %>").value)
                    $telerik.$('#<%=tbPOnsiteEmail.ClientID %>').val(document.getElementById("<%=tbSOnsiteEmail.ClientID %>").value)
                    $telerik.$('#<%=tbPOnsitePhone.ClientID %>').val(document.getElementById("<%=tbSOnsitePhone.ClientID %>").value)
                    //var phone = $find("<%=tbPOnsitePhone.ClientID %>");
                    //phone.set_value(document.getElementById("<%=tbSOnsitePhone.ClientID %>").value);
                     document.getElementById("<%=tbPOnsitePhone.ClientID %>").value = document.getElementById("<%=tbSOnsitePhone.ClientID %>").value;
                     $("#tbSAddress1").val();
                     $("#tbSAddress2").val();
                     $("#tbSCity").val();
                     $("#tbSZip").val();
                    $t<%--elerik.$('#<%=tbPAddress1.ClientID %>').val(document.getElementById("<%=tbSAddress1.ClientID %>").value)
                    $telerik.$('#<%=tbPAddress2.ClientID %>').val(document.getElementById("<%=tbSAddress2.ClientID %>").value)
                    $telerik.$('#<%=tbPCity.ClientID %>').val(document.getElementById("<%=tbSCity.ClientID %>").value)
                    $telerik.$('#<%=tbPZip.ClientID %>').val(document.getElementById("<%=tbSZip.ClientID %>").value)--%>

           <%-- var country = $find("<%=cbSCountry.ClientID %>");
            var state = $find("<%=cbSState.ClientID %>");--%>

            if (country._text == 'UNITED STATES' ) {
                var combo = $find("<%= cbPCountry.ClientID %>");
                var item = combo.findItemByText(country._text);
                if (item) {
                    item.select();
                }
                var state2 = $find("<%= cbPState.ClientID %>");
                var item2 = state2.findItemByText(state._text);
                if (item2) {
                    item2.select();
                }
             
                var combo = $find("<%= cbPCountry.ClientID %>");
                var item = combo.findItemByText(country._text);
                if (item) {
                    item.select();
                }
              
                $("#tbSRegion").val();
            }
            //  f.billing_last_name.value = f.last_name.value;

            //   f.billing_address_1.value = f.address_1.value;

            //   f.billing_address_2.value = f.address_2.value;

            //   f.billing_city.value = f.city.value;

            //   f.billing_state.value = f.state.value;

            //   f.billing_zipcode.value = f.zipcode.value;

            //$(".pickup").hide()

        }

        if (document.getElementById("<%=CheckBox1.ClientID %>").checked == false) {

            document.getElementById("<%=tbPickupfrom.ClientID %>").value = ' '
            document.getElementById("<%=tbPOnsiteContact.ClientID %>").value = ' '
            document.getElementById("<%=tbPOnsitePhone.ClientID %>").value = ' '
            document.getElementById("<%=tbPOnsiteEmail.ClientID %>").value = ' '
            document.getElementById("<%=tbPAddress1.ClientID %>").value = ' '
            document.getElementById("<%=tbPAddress2.ClientID %>").value = ' '
            document.getElementById("<%=tbPCity.ClientID %>").value = ' '
            document.getElementById("<%=tbPZip.ClientID %>").value = ' '
            //    document.getElementById("<%=cbPCountry.ClientID %>").value = ' '
            //    document.getElementById("<%=cbPState.ClientID %>").value = ''
            //$(".pickup").show()
        }

             }

function OnShippingClientSelectedIndexChanged(sender, eventArgs) {
                 var item = eventArgs.get_item();
             }

          function OnPickupClientSelectedIndexChanged(sender, eventArgs) {
                 var item = eventArgs.get_item();
             }

             window.initAutocomplete = function () {
                 address1Field = document.querySelector("#MainContent_tbSAddress1");
                 address2Field = document.querySelector("#MainContent_tbSAddress2");
                 postalField = document.querySelector("#MainContent_tbSZip");
                 
                 autocomplete = new google.maps.places.Autocomplete(address1Field, {
                     /*componentRestrictions: { country: ["us", "ca"] },*/
                     fields: ["address_components", "geometry"],
                     types: ["address"],
                 });
                 //address1Field.focus();
                 autocomplete.addListener("place_changed", fillInAddress);
             }
             function fillInAddress() {
                 var place = autocomplete.getPlace();
                 var address1 = "";
                 var postcode = "";
                 var shipCountry = document.querySelector("#shipCountry");
                 for (var _i = 0, _a = place.address_components; _i < _a.length; _i++) {
                     var component = _a[_i];
                     var componentType = component.types[0];
                     switch (componentType) {
                         case "street_number": {
                             address1 = "".concat(component.long_name, " ").concat(address1);
                             break;
                         }
                         case "route": {
                             address1 += component.short_name;
                             break;
                         }
                         case "postal_code": {
                             postcode = "".concat(component.long_name).concat(postcode);
                             break;
                         }
                         case "postal_code_suffix": {
                             postcode = "".concat(postcode, "-").concat(component.long_name);
                             break;
                         }
                         //case "locality":
                         //    document.querySelector("#MainContent_tbSCity").value =
                         //        component.long_name;
                         //    break;
                         case "administrative_area_level_1": {
                             document.querySelector("#MainContent_cbSState").value =
                                 component.short_name;
                             break;
                         }
                         //case "country":
                         //    document.querySelector("#ctl00_MainContent_cbSCountry_Input").value = component.long_name;
                         //    document.querySelector("#MainContent_shipCountry").value = component.long_name;
                         //    break;
                     }
                 }
                 address1Field.value = address1;
                 postalField.value = postcode;

             }

            <%-- function tbShipTo_Blur(sender, eventArgs) {
                 var shipTo = sender.get_value();
                 $("#<%= hdShipTo.ClientID %>").val(shipTo);
             }

             function tbSOnsiteEmail_Blur(sender, eventArgs) {
                 var OnsiteEmail = sender.get_value();
                 $("#<%= hdOnsiteEmail.ClientID %>").val(OnsiteEmail);
             }

             function tbSOnsiteContact_Blur(sender, eventArgs) {
                 var OnsiteContact = sender.get_value();
                 $("#<%= hdOnsiteContact.ClientID %>").val(OnsiteContact);
                
             }

             function tbSOnsitePhone_Blur(sender, eventArgs) {
                 var OnsitePhone = sender.get_value();
                 $("#<%= hdOnsitePhone.ClientID %>").val(OnsitePhone);
                
             }--%>


 window.onbeforeunload = function(){
        return'You must click "Buy Now" to make payment and finish your order. If you leave now your order will be canceled.';
};


function Click1(button)
        {
            window.onbeforeunload = null;
        }

  function Click2(button)
        {
            window.onbeforeunload = null;
            if(dateChanged) {
                alert("Please proceed to select your inventory as the event dates have been modified.");
             Return;       
                }
            if (fulfillmentLocationChanged) {
                alert("Please proceed to select your inventory as the fulfillment location has been modified.");
             Return;   
             }    
        }

function OnClientSelectedIndexChanged(sender, args) {
         fulfillmentLocationChanged = true
    }




 function StandardConfirm(sender, args) {
window.onbeforeunload = null;
    args.set_cancel(!window.confirm('Are you sure you want to cancel all changes?'));
}

function dateSelected(sender, eventArgs) {

if (sDate != $("#ctl00_MainContent_tbStartDate_dateInput").val() || eDate != $("#ctl00_MainContent_tbPickupDate_dateInput").val() || aDate != $("#ctl00_MainContent_tbArrivalDate_dateInput").val()) {
     dateChanged = true;
} else {
      dateChanged = false;
    }
}

      


           

         </script>
    </telerik:RadScriptBlock>   
    <style runat="server">
        .entry{
            display:inline-block;
            height:auto;
            width:500px;
            margin-left:150px;
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

        #MainContent_ddlShipCity {
            width:250px;
         border-color: #8e8e8e #b8b8b8 #b8b8b8 #8e8e8e;
         background: #fff;
        color: #333;
         font: 12px "segoe ui",arial,sans-serif;
        }
        

       

        
    </style>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <%--<telerik:AjaxSetting AjaxControlID="tbBoothSize">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="orderGrid" UpdatePanelCssClass="" />

                    <telerik:AjaxUpdatedControl ControlID="nav1" UpdatePanelCssClass="" />

                </UpdatedControls>ship
            </telerik:AjaxSetting>--%>
           <%-- <telerik:AjaxSetting AjaxControlID="Grid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="orderGrid" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="orderGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Grid1" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <asp:HiddenField ID="fkstatusID" runat="server" /><asp:HiddenField ID="fkUserID" runat="server" />
    
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server"  Width="100%" RestoreOriginalRenderDelegate="false">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ValidationGroup="ValidateFields"/>   
        <div runat="server" style="margin-left:20px;">
             <table style="margin-top:20px;">
             <tr runat="server" id="serviceCountry">
            <td></td>
            <td style="color:rgb(255,0,0);font-size: 18px;white-space: nowrap;">*We do not ship to the selected location, please enter a new address or contact support.</td>
             </tr>
         </table>
        </div> 
        <table class="entry">
        <tr><td colspan="2"><h2>Show Information:</h2></td></tr>
        <tr><td>Event Name:</td><td><telerik:RadTextBox ID="tbEventName" runat="server" width="250px"></telerik:RadTextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Event Name" ControlToValidate="tbEventName" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator></td></tr>
        <tr><td>Apple Contact Name:</td><td><telerik:RadTextBox ID="tbContactName" runat="server" width="250px"></telerik:RadTextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Contact Name" ControlToValidate="tbContactName" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator></td></tr>
        <tr><td>Apple Contact Email:</td><td><telerik:RadTextBox ID="tbContactEmail" runat="server" width="250px"></telerik:RadTextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Contact Email" ControlToValidate="tbContactEmail" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="emailValidator" runat="server" Display="Dynamic"
    ErrorMessage="Please, enter valid e-mail address." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$" ForeColor="Red" ControlToValidate="tbContactEmail" ValidationGroup="ValidateFields">*</asp:RegularExpressionValidator></td></tr>
        <tr><td>Apple Contact Phone #:</td><td>
             <telerik:RadMaskedTextBox RenderMode="Lightweight" ID="tbContactPhone" runat="server" Mask="(###)-#######" Width="250px">
                    </telerik:RadMaskedTextBox>
        <asp:RegularExpressionValidator Display="Dynamic" Font-size="7px" ID="MaskedTextBoxRegularExpressionValidator"
                                runat="server" ForeColor="red" ErrorMessage="Invalid Phone#" ControlToValidate="tbContactPhone" 
                                ValidationExpression="\(\d{3}\)-\d{7}"></asp:RegularExpressionValidator>

            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Contact Phone" ControlToValidate="tbContactPhone" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator></td></tr>
        <tr runat="server" visible="false"><td>Cost Center/ PO:</td><td><telerik:RadTextBox ID="tbCostCenter" runat="server" width="250px"></telerik:RadTextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="Cost Center" ControlToValidate="tbCostCenter" ForeColor="Red" ValidationGroup="ValidateFields" Enabled="false">*</asp:RequiredFieldValidator></td></tr>
        <tr><td>Event Start Date:</td><td>
            <telerik:RadDateTimePicker ID="tbStartDate" runat="server" AutoPostBackControl="Calendar" CausesValidation="false" OnSelectedDateChanged="datechanged" Width="250px">
                <TimeView runat="server" CellSpacing="-1">
                </TimeView>
                <TimePopupButton HoverImageUrl="" ImageUrl="" />
                <Calendar runat="server" EnableWeekends="True" FastNavigationNextText="&amp;lt;&amp;lt;" ShowRowHeaders="false" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                </Calendar>
                <DateInput runat="server" AutoPostBack="True" DateFormat="M/d/yyyy" DisplayDateFormat="M/d/yyyy" LabelWidth="40%">
                    <EmptyMessageStyle Resize="None" />
                    <ReadOnlyStyle Resize="None" />
                    <FocusedStyle Resize="None" />
                    <DisabledStyle Resize="None" />
                    <InvalidStyle Resize="None" />
                    <HoveredStyle Resize="None" />
                    <EnabledStyle Resize="None" />
                </DateInput>
                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                <ClientEvents OnDateSelected="dateSelected" />
            </telerik:RadDateTimePicker><asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Start Date/Time" ControlToValidate="tbStartDate" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator>
            
            </td></tr>
        <tr><td>Event End Date:</td><td>
            <telerik:RadDateTimePicker ID="tbPickupDate" runat="server" AutoPostBackControl="Calendar" CausesValidation="false"  OnSelectedDateChanged="datechanged" Width="250px">
                <TimeView runat="server" CellSpacing="-1">
                </TimeView>
                <TimePopupButton HoverImageUrl="" ImageUrl="" />
                <Calendar runat="server" EnableWeekends="True" FastNavigationNextText="&amp;lt;&amp;lt;" ShowRowHeaders="false" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                </Calendar>
                <DateInput runat="server" ID="inputPickUpdate" AutoPostBack="True" DateFormat="M/d/yyyy" DisplayDateFormat="M/d/yyyy" LabelWidth="40%">
                    <EmptyMessageStyle Resize="None" />
                    <ReadOnlyStyle Resize="None" />
                    <FocusedStyle Resize="None" />
                    <DisabledStyle Resize="None" />
                    <InvalidStyle Resize="None" />
                    <HoveredStyle Resize="None" />
                    <EnabledStyle Resize="None" />
                </DateInput>
                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                  <ClientEvents OnDateSelected="dateSelected" />
            </telerik:RadDateTimePicker><asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Pickup Date/Time" ControlToValidate="tbPickupDate" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator>
           
                                    </td></tr>
             <tr><td style="color:rgb(128, 128, 128);font-size: 9px;">*Select Event Start Date to view inventory</td></tr>
                    <tr><td><span style="color:red;" class="message"><asp:Literal id="message" runat="server" /></span></td></tr>

        <tr runat="server" visible="false"><td>Event Venue Name:</td><td><telerik:RadTextBox ID="tbVenue" runat="server" width="250px"></telerik:RadTextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Venue" ControlToValidate="tbVenue" ForeColor="Red" ValidationGroup="ValidateFields" Enabled="false" >*</asp:RequiredFieldValidator>
                                      </td></tr>
        <tr runat="server" visible="false"><td>Website Link:</td><td><telerik:RadTextBox  ID="tbWebsite" runat="server" width="250px"></telerik:RadTextBox></td></tr>
        
            <tr>
            <td colspan="2"><h2>Addtional Information:</h2></td>
             </tr>
            <tr runat="server">
            <td colspan="2"><span id="counter"></span>
            <telerik:RadEditor MaxTextLength="1000"  ID="tbNotes" runat="server"  StripFormattingOptions="ConvertWordLists, MSWordNoMargins"
                        ContentAreaCssFile="~/Styles/RadEditorOverrides.css" EditModes="Design"
                        style="z-index:3; width:auto" Height="90px" >
                        <cssfiles>
                            <telerik:EditorCssFile Value="~/Styles/RadEditorOverrides.css" />
                        </cssfiles>
                        <tools>
                            <telerik:EditorToolGroup Tag="Formatting">
                                <telerik:EditorTool Name="Bold" />
                                <telerik:EditorTool Name="Italic" />
                                <telerik:EditorTool Name="Underline" />
                                <telerik:EditorSeparator Visible="true" />
                                <telerik:EditorTool Name="ForeColor" />
                                <telerik:EditorTool Name="BackColor" />
                                <telerik:EditorSeparator Visible="true" />
                                <telerik:EditorTool Name="FontName" Visible="false"  />
                                <telerik:EditorTool Name="RealFontSize" Visible="false" /> 
                            </telerik:EditorToolGroup>
                        </tools>

                        <content>
                        </content>
                
                    </telerik:RadEditor></td>
        </tr>
            
            
            <tr runat="server" id="notesContainer">
            <td colspan="2"><h2>Order Change Log:</h2></td>
        </tr>
        <tr runat="server" id="notes">
            <td colspan="2"><telerik:RadEditor MaxTextLength="1000"  ID="tbChangeLog" runat="server"  StripFormattingOptions="ConvertWordLists, MSWordNoMargins"
                        ContentAreaCssFile="~/Styles/RadEditorOverrides.css" EditModes="Design"
                        style="z-index:3; width:auto" Height="90px" >
                        <cssfiles>
                            <telerik:EditorCssFile Value="~/Styles/RadEditorOverrides.css" />
                        </cssfiles>
                        <tools>
                            <telerik:EditorToolGroup Tag="Formatting">
                                <telerik:EditorTool Name="Bold" />
                                <telerik:EditorTool Name="Italic" />
                                <telerik:EditorTool Name="Underline" />
                                <telerik:EditorSeparator Visible="true" />
                                <telerik:EditorTool Name="ForeColor" />
                                <telerik:EditorTool Name="BackColor" />
                                <telerik:EditorSeparator Visible="true" />
                                <telerik:EditorTool Name="FontName" Visible="false"  />
                                <telerik:EditorTool Name="RealFontSize" Visible="false" /> 
                            </telerik:EditorToolGroup>
                        </tools>

                        <content>
                        </content>
                
                    </telerik:RadEditor></td>
        </tr>

    </table>
        
     <table class="entry" style="display:none;">
        <tr><td colspan="2"><h2>Booth Information</h2></td></tr>
        
        <tr><td>Booth Size:</td><td><telerik:RadComboBox ID="tbBoothSize" runat="server" AllowCustomText="False" AutoPostBack="true">
                        <Items>
                            <telerik:RadComboBoxItem runat="server" Text="Select..." Value="Select..." Selected="true"/>
                            <telerik:RadComboBoxItem runat="server" Text="Regional Standard Pull Up" Value="Regional Standard Pull Up" />
                            <telerik:RadComboBoxItem runat="server" Text="10 x 10 Softside" Value="10 x 10 Softside" />
                            <telerik:RadComboBoxItem runat="server" Text="10 x 10 Hardside" Value="10 x 10 Hardside" />
                            <telerik:RadComboBoxItem runat="server" Text="10 x 20" Value="10 x 20" />
                            <telerik:RadComboBoxItem runat="server" Text="20 x 20" Value="20 x 20" />
                            <telerik:RadComboBoxItem runat="server" Text="Other" Value="Other" />
                        </Items>
                    </telerik:RadComboBox><asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" 
                        ErrorMessage="Select Booth Size" ControlToValidate="tbBoothSize" InitialValue="Select..." ForeColor="Red" ValidationGroup="ValidateFields"  Enabled="false">*</asp:RequiredFieldValidator></td></tr>
       <%-- <tr><td>Booth #:</td><td><telerik:RadTextBox ID="tbBoothnum" runat="server"></telerik:RadTextBox></td></tr>--%>
        <tr runat="server" id="trBoothType" ><td>Booth Type:</td><td><telerik:RadComboBox ID="cbBoothType" Runat="server" Height="140px" >
                        <Items>
                            <telerik:RadComboBoxItem runat="server" Text="Select..." Value="Select..." />
                            <telerik:RadComboBoxItem runat="server" Text="End-Cap Booth" Value="End-Cap Booth" />
                            <telerik:RadComboBoxItem runat="server" Text="Inline" Value="Inline" />
                            <telerik:RadComboBoxItem runat="server" Text="Island" Value="Island" />
                            <telerik:RadComboBoxItem runat="server" Text="Peninsula" Value="Peninsula" />
                            <telerik:RadComboBoxItem runat="server" Text="Perimeter Booth" Value="Perimeter Booth" />
                            <telerik:RadComboBoxItem runat="server" Text="Split Island" Value="Split Island" />
                            <telerik:RadComboBoxItem runat="server" Text="NA" Value="NA" />
                        </Items>
                    </telerik:RadComboBox></td></tr>
         <asp:Label ID="lbservices" runat="server" >
        
        <tr runat="server" id="trElectrical" ><td>Electrical:</td><td><telerik:RadComboBox ID="cbElectrical" Runat="server" Height="140px"  AllowCustomText="False">
                        <Items>
                            <telerik:RadComboBoxItem runat="server" Text="5 Amp" Value="5 Amp" Selected="true" />
                            <telerik:RadComboBoxItem runat="server" Text="" Value="" />
                        </Items>
                    </telerik:RadComboBox></td></tr>
        <tr runat="server" id="trInternet"><td>Internet:</td><td><asp:RadioButtonList ID="rbInternet" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem>No</asp:ListItem>
                                                <asp:ListItem Selected="True" >Yes</asp:ListItem>
                                            </asp:RadioButtonList></td></tr>
        <tr runat="server" id="trCarpet"><td>Carpet w/Padding:</td><td><asp:RadioButtonList ID="rbShipCarpet" runat="server" 
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem>No</asp:ListItem>
                                                <asp:ListItem Selected="True">Yes</asp:ListItem>
                                            </asp:RadioButtonList></td></tr>
        <tr runat="server" id="trLeads"><td>Lead Retrieval:</td><td><asp:RadioButtonList ID="rbLeadRetrieval" runat="server" 
                                                 RepeatDirection="Horizontal">
                                                <asp:ListItem>No</asp:ListItem>
                                                <asp:ListItem Selected="True">Yes</asp:ListItem>
                                            </asp:RadioButtonList></td></tr>
        <tr runat="server" id="trMaterial"><td>Material:</td><td><asp:RadioButtonList ID="rbMaterial" runat="server" 
                                                 RepeatDirection="Horizontal">
                                                <asp:ListItem>No</asp:ListItem>
                                                <asp:ListItem Selected="True">Yes</asp:ListItem>
                                            </asp:RadioButtonList></td></tr>
        <tr runat="server" id="trLabor"><td>Labor:</td><td><asp:RadioButtonList ID="rbLabor" runat="server" 
                                                 RepeatDirection="Horizontal">
                                                <asp:ListItem>No</asp:ListItem>
                                                <asp:ListItem Selected="True">Yes</asp:ListItem>
                                            </asp:RadioButtonList></td></tr>
        <tr runat="server" id="trFurniture"><td>Rental Furniture:</td><td><telerik:RadTextBox ID="tbShipFurniture" Runat="server" >
                    </telerik:RadTextBox></td></tr>
             </asp:Label>
         <tr><td>Is there an Exhibitor Portal?</td><td><asp:RadioButtonList ID="rbPortal" runat="server"  style="display:inline-block;" AutoPostBack="true"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem>No</asp:ListItem>
                                                <asp:ListItem>Yes</asp:ListItem></asp:RadioButtonList> <asp:RequiredFieldValidator style="display:inline-block;" ID="RequiredFieldValidator32" runat="server"  ErrorMessage="Select if there is an exhibitor portal" ControlToValidate="rbPortal"  ForeColor="Red" ValidationGroup="ValidateFields"  Enabled="false">*</asp:RequiredFieldValidator></td></tr>
         <tr runat="server" id="trportalsite"><td>Exhibitor Portal Website:</td><td><telerik:RadTextBox ID="tbPortalsite" runat="server"></telerik:RadTextBox><asp:RequiredFieldValidator style="display:inline-block;" ID="vtbPortalsite" runat="server"  ErrorMessage="Enter Exhibitor Portal Website" ControlToValidate="tbPortalsite"  ForeColor="Red" ValidationGroup="ValidateFields" Enabled="false">*</asp:RequiredFieldValidator></td></tr>
         <tr runat="server" id="trportalname"><td>Username:</td><td><telerik:RadTextBox ID="tbPortaluser" runat="server"></telerik:RadTextBox><asp:RequiredFieldValidator style="display:inline-block;" ID="vtbPortaluser" runat="server"  ErrorMessage="Enter Username" ControlToValidate="tbPortaluser"  ForeColor="Red" ValidationGroup="ValidateFields"  Enabled="false">*</asp:RequiredFieldValidator></td></tr>
         <tr runat="server" id="trportalpass"><td>Password:</td><td><telerik:RadTextBox ID="tbPortalpass" runat="server"></telerik:RadTextBox><asp:RequiredFieldValidator style="display:inline-block;" ID="vtbPortalpass" runat="server"  ErrorMessage="Enter Password" ControlToValidate="tbPortalpass"  ForeColor="Red" ValidationGroup="ValidateFields"  Enabled="false">*</asp:RequiredFieldValidator></td></tr>

    </table>
        
    <table class="entry" style="display:inline;">
        <tr>
          <td colspan="2"><h2>Shipping Information</h2></td>
        </tr>
            <tr>
                <td>Ship to:</td>
                <td>
                    <telerik:RadTextBox ID="tbShipTo" Runat="server" Name="tbShipTo" width="250px" >
                    </telerik:RadTextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                        ErrorMessage="Ship to" ControlToValidate="tbShipTo" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator>
                </td>
            </tr>
                <tr><td>Booth #:</td><td><telerik:RadTextBox width="250px" ID="tbBoothnum" runat="server"></telerik:RadTextBox></td></tr>

            <tr>
                <td>Onsite Contact Name:</td>
                <td>
                    <telerik:RadTextBox ID="tbSOnsiteContact" Runat="server" width="250px" >
                    </telerik:RadTextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" 
                        ErrorMessage="Ship to Onsite Contact" ControlToValidate="tbSOnsiteContact" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator>
                </td>
            </tr>
        <tr>
                <td>Onsite Contact Email:</td>
                <td>
                    <telerik:RadTextBox ID="tbSOnsiteEmail" Runat="server" width="250px">
                    </telerik:RadTextBox ><asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" 
                        ErrorMessage="Ship to Onsite Contact Email" ControlToValidate="tbSOnsiteEmail" ForeColor="Red"  ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic"
    ErrorMessage="Please, enter valid e-mail address." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$" ForeColor="Red" ControlToValidate="tbSOnsiteEmail" ValidationGroup="ValidateFields">*</asp:RegularExpressionValidator>
                </td>
            </tr>
             <tr>
                <td>Onsite Contact Phone #:</td>
                <td>
                                        <telerik:RadMaskedTextBox RenderMode="Lightweight" ID="tbSOnsitePhone" runat="server" Mask="(###)-#######" Width="250px">
                    </telerik:RadMaskedTextBox>
        <asp:RegularExpressionValidator Display="Dynamic" Font-size="7px" ID="RegularExpressionValidator3"
                                runat="server" ForeColor="red" ErrorMessage="Invalid Phone#" ControlToValidate="tbSOnsitePhone" ValidationGroup="ValidateFields"
                                ValidationExpression="\(\d{3}\)-\d{7}"></asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Contact Phone" ControlToValidate="tbSOnsitePhone" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator></td></tr>

            
            <%--<tr>
                <td>Address Line 1:</td>
                <td
                    <telerik:RadTextBox ID="tbSAddress1" Runat="server" width="250px">
                    </telerik:RadTextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" 
                        ErrorMessage="Ship to Address" ControlToValidate="tbSAddress1" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator>
                </td>
            </tr>--%>

       
        <tr>
                <td>Address Line 1:</td>
                <td>
                <input class="txtBox" 
          id="tbSAddress1"
          autocomplete="off"
          runat="server"
          required="required"
        />
                </td>
            </tr>
            <%--<tr>
                <td>Address Line 2:</td>
                <td>
                    <telerik:RadTextBox ID="tbSAddress2" Runat="server" width="250px">
                    </telerik:RadTextBox>
                </td>
            </tr>--%>

        <tr>
                <td>Address Line 2:</td>
                <td>
                  <input class="txtBox" style="width:242px;" id="tbSAddress2" name="address2"  runat="server" />
                </td>
            </tr>
            <%--<tr>
                <td>City:</td>
                <td>
                    <telerik:RadTextBox ID="tbSCity" Runat="server" width="250px">
                    </telerik:RadTextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" 
                        ErrorMessage="Ship to City" ControlToValidate="tbSCity" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator>
                </td>
            </tr>--%>

       
            <%--<tr>
                <td style="vertical-align: top;">
                    <div style="vertical-align:top;">State/Province/Region:</div>
                </td>
                <td>
                    
                        <div style="vertical-align:top;display:inline;">
                            <telerik:RadComboBox ID="cbSState" Runat="server" Height="140px" MarkFirstMatch="True" Filter="Contains" width="250px">
                            </telerik:RadComboBox>
                            <telerik:RadTextBox ID="tbSRegion" Runat="server" TabIndex="16" width="250px"></telerik:RadTextBox>
                            
                        </div>
                    
                </td>
            </tr>--%>

    <tr>
                <td style="vertical-align: top;">
                    <div style="vertical-align:top;">State/Province/Region:</div>
                </td>
                <td>
                    <input class="txtBox" style="width:242px;" id="cbSState" name="state" runat="server"
          required="required" />
                </td>
            </tr>
          <%--  <tr>
                <td>ZIP/Postal Code:</td>
                <td>
                    <telerik:RadTextBox ID="tbSZip" Runat="server" width="250px">
                    </telerik:RadTextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" 
                        ErrorMessage="Ship to Zip" ControlToValidate="tbSZip" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator>
                </td>
            </tr>--%>

        <tr>
                <td>ZIP/Postal Code:</td>
                <td>
                   <input class="txtBox" id="tbSZip" name="postcode" runat="server"
          required="required" />
                </td>
            </tr>
            <tr>
                <td style="width:250px">
                    Country:</td>
                <td>
                
                    <telerik:RadComboBox ID="cbSCountry" Runat="server" AutoCompleteSeparator="" MarkFirstMatch="true" OnClientSelectedIndexChanged="OnShippingClientSelectedIndexChanged" autopostback="True" 
    onselectedindexchanged="RadComboBox1_SelectedIndexChanged"  OnTextChanged="RadCB_TextChanged" width="250px" >
                        <Items>
   <telerik:RadComboBoxItem Value="Afganistan" Text="Afghanistan" />
   <telerik:RadComboBoxItem Value="Albania" Text="Albania" />
   <telerik:RadComboBoxItem Value="Algeria" Text="Algeria" />
   <telerik:RadComboBoxItem Value="American Samoa" Text="American Samoa" />
   <telerik:RadComboBoxItem Value="Andorra" Text="Andorra" />
   <telerik:RadComboBoxItem Value="Angola" Text="Angola" />
   <telerik:RadComboBoxItem Value="Anguilla" Text="Anguilla" />
   <telerik:RadComboBoxItem Value="Antigua & Barbuda" Text="Antigua & Barbuda" />
   <telerik:RadComboBoxItem Value="Argentina" Text="Argentina" />
   <telerik:RadComboBoxItem Value="Armenia" Text="Armenia" />
   <telerik:RadComboBoxItem Value="Aruba" Text="Aruba" />
   <telerik:RadComboBoxItem Value="Australia" Text="Australia" />
   <telerik:RadComboBoxItem Value="Austria" Text="Austria" />
   <telerik:RadComboBoxItem Value="Azerbaijan" Text="Azerbaijan" />
   <telerik:RadComboBoxItem Value="Bahamas" Text="Bahamas" />
   <telerik:RadComboBoxItem Value="Bahrain" Text="Bahrain" />
   <telerik:RadComboBoxItem Value="Bangladesh" Text="Bangladesh" />
   <telerik:RadComboBoxItem Value="Barbados" Text="Barbados" />
   <telerik:RadComboBoxItem Value="Belarus" Text="Belarus" />
   <telerik:RadComboBoxItem Value="Belgium" Text="Belgium" />
   <telerik:RadComboBoxItem Value="Belize" Text="Belize" />
   <telerik:RadComboBoxItem Value="Benin" Text="Benin" />
   <telerik:RadComboBoxItem Value="Bermuda" Text="Bermuda" />
   <telerik:RadComboBoxItem Value="Bhutan" Text="Bhutan" />
   <telerik:RadComboBoxItem Value="Bolivia" Text="Bolivia" />
   <telerik:RadComboBoxItem Value="Bonaire" Text="Bonaire" />
   <telerik:RadComboBoxItem Value="Bosnia & Herzegovina" Text="Bosnia & Herzegovina" />
   <telerik:RadComboBoxItem Value="Botswana" Text="Botswana" />
   <telerik:RadComboBoxItem Value="Brazil" Text="Brazil" />
   <telerik:RadComboBoxItem Value="British Indian Ocean Ter" Text="British Indian Ocean Ter" />
   <telerik:RadComboBoxItem Value="Brunei" Text="Brunei" />
   <telerik:RadComboBoxItem Value="Bulgaria" Text="Bulgaria" />
   <telerik:RadComboBoxItem Value="Burkina Faso" Text="Burkina Faso" />
   <telerik:RadComboBoxItem Value="Burundi" Text="Burundi" />
   <telerik:RadComboBoxItem Value="Cambodia" Text="Cambodia" />
   <telerik:RadComboBoxItem Value="Cameroon" Text="Cameroon" />
   <telerik:RadComboBoxItem Value="CA" Text="Canada" />
   <telerik:RadComboBoxItem Value="Canary Islands" Text="Canary Islands" />
   <telerik:RadComboBoxItem Value="Cape Verde" Text="Cape Verde" />
   <telerik:RadComboBoxItem Value="Cayman Islands" Text="Cayman Islands" />
   <telerik:RadComboBoxItem Value="Central African Republic" Text="Central African Republic" />
   <telerik:RadComboBoxItem Value="Chad" Text="Chad" />
   <telerik:RadComboBoxItem Value="Channel Islands" Text="Channel Islands" />
   <telerik:RadComboBoxItem Value="Chile" Text="Chile" />
   <telerik:RadComboBoxItem Value="China" Text="China" />
   <telerik:RadComboBoxItem Value="Christmas Island" Text="Christmas Island" />
   <telerik:RadComboBoxItem Value="Cocos Island" Text="Cocos Island" />
   <telerik:RadComboBoxItem Value="Colombia" Text="Colombia" />
   <telerik:RadComboBoxItem Value="Comoros" Text="Comoros" />
   <telerik:RadComboBoxItem Value="Congo" Text="Congo" />
   <telerik:RadComboBoxItem Value="Cook Islands" Text="Cook Islands" />
   <telerik:RadComboBoxItem Value="Costa Rica" Text="Costa Rica" />
   <telerik:RadComboBoxItem Value="Cote DIvoire" Text="Cote DIvoire" />
   <telerik:RadComboBoxItem Value="Croatia" Text="Croatia" />
   <telerik:RadComboBoxItem Value="Cuba" Text="Cuba" />
   <telerik:RadComboBoxItem Value="Curaco" Text="Curacao" />
   <telerik:RadComboBoxItem Value="Cyprus" Text="Cyprus" />
   <telerik:RadComboBoxItem Value="Czech Republic" Text="Czech Republic" />
   <telerik:RadComboBoxItem Value="Denmark" Text="Denmark" />
   <telerik:RadComboBoxItem Value="Djibouti" Text="Djibouti" />
   <telerik:RadComboBoxItem Value="Dominica" Text="Dominica" />
   <telerik:RadComboBoxItem Value="Dominican Republic" Text="Dominican Republic" />
   <telerik:RadComboBoxItem Value="East Timor" Text="East Timor" />
   <telerik:RadComboBoxItem Value="Ecuador" Text="Ecuador" />
   <telerik:RadComboBoxItem Value="Egypt" Text="Egypt" />
   <telerik:RadComboBoxItem Value="El Salvador" Text="El Salvador" />
   <telerik:RadComboBoxItem Value="Equatorial Guinea" Text="Equatorial Guinea" />
   <telerik:RadComboBoxItem Value="Eritrea" Text="Eritrea" />
   <telerik:RadComboBoxItem Value="Estonia" Text="Estonia" />
   <telerik:RadComboBoxItem Value="Ethiopia" Text="Ethiopia" />
   <telerik:RadComboBoxItem Value="Falkland Islands" Text="Falkland Islands" />
   <telerik:RadComboBoxItem Value="Faroe Islands" Text="Faroe Islands" />
   <telerik:RadComboBoxItem Value="Fiji" Text="Fiji" />
   <telerik:RadComboBoxItem Value="Finland" Text="Finland" />
   <telerik:RadComboBoxItem Value="France" Text="France" />
   <telerik:RadComboBoxItem Value="French Guiana" Text="French Guiana" />
   <telerik:RadComboBoxItem Value="French Polynesia" Text="French Polynesia" />
   <telerik:RadComboBoxItem Value="French Southern Ter" Text="French Southern Ter" />
   <telerik:RadComboBoxItem Value="Gabon" Text="Gabon" />
   <telerik:RadComboBoxItem Value="Gambia" Text="Gambia" />
   <telerik:RadComboBoxItem Value="Georgia" Text="Georgia" />
   <telerik:RadComboBoxItem Value="Germany" Text="Germany" />
   <telerik:RadComboBoxItem Value="Ghana" Text="Ghana" />
   <telerik:RadComboBoxItem Value="Gibraltar" Text="Gibraltar" />
   <telerik:RadComboBoxItem Value="Great Britain" Text="Great Britain" />
   <telerik:RadComboBoxItem Value="Greece" Text="Greece" />
   <telerik:RadComboBoxItem Value="Greenland" Text="Greenland" />
   <telerik:RadComboBoxItem Value="Grenada" Text="Grenada" />
   <telerik:RadComboBoxItem Value="Guadeloupe" Text="Guadeloupe" />
   <telerik:RadComboBoxItem Value="Guam" Text="Guam" />
   <telerik:RadComboBoxItem Value="Guatemala" Text="Guatemala" />
   <telerik:RadComboBoxItem Value="Guinea" Text="Guinea" />
   <telerik:RadComboBoxItem Value="Guyana" Text="Guyana" />
   <telerik:RadComboBoxItem Value="Haiti" Text="Haiti" />
   <telerik:RadComboBoxItem Value="Hawaii" Text="Hawaii" />
   <telerik:RadComboBoxItem Value="Honduras" Text="Honduras" />
   <telerik:RadComboBoxItem Value="Hong Kong" Text="Hong Kong" />
   <telerik:RadComboBoxItem Value="Hungary" Text="Hungary" />
   <telerik:RadComboBoxItem Value="Iceland" Text="Iceland" />
   <telerik:RadComboBoxItem Value="Indonesia" Text="Indonesia" />
   <telerik:RadComboBoxItem Value="India" Text="India" />
   <telerik:RadComboBoxItem Value="Iran" Text="Iran" />
   <telerik:RadComboBoxItem Value="Iraq" Text="Iraq" />
   <telerik:RadComboBoxItem Value="Ireland" Text="Ireland" />
   <telerik:RadComboBoxItem Value="Isle of Man" Text="Isle of Man" />
   <telerik:RadComboBoxItem Value="Israel" Text="Israel" />
   <telerik:RadComboBoxItem Value="Italy" Text="Italy" />
   <telerik:RadComboBoxItem Value="Jamaica" Text="Jamaica" />
   <telerik:RadComboBoxItem Value="Japan" Text="Japan" />
   <telerik:RadComboBoxItem Value="Jordan" Text="Jordan" />
   <telerik:RadComboBoxItem Value="Kazakhstan" Text="Kazakhstan" />
   <telerik:RadComboBoxItem Value="Kenya" Text="Kenya" />
   <telerik:RadComboBoxItem Value="Kiribati" Text="Kiribati" />
   <telerik:RadComboBoxItem Value="Korea North" Text="Korea North" />
   <telerik:RadComboBoxItem Value="Korea South" Text="Korea South" />
   <telerik:RadComboBoxItem Value="Kuwait" Text="Kuwait" />
   <telerik:RadComboBoxItem Value="Kyrgyzstan" Text="Kyrgyzstan" />
   <telerik:RadComboBoxItem Value="Laos" Text="Laos" />
   <telerik:RadComboBoxItem Value="Latvia" Text="Latvia" />
   <telerik:RadComboBoxItem Value="Lebanon" Text="Lebanon" />
   <telerik:RadComboBoxItem Value="Lesotho" Text="Lesotho" />
   <telerik:RadComboBoxItem Value="Liberia" Text="Liberia" />
   <telerik:RadComboBoxItem Value="Libya" Text="Libya" />
   <telerik:RadComboBoxItem Value="Liechtenstein" Text="Liechtenstein" />
   <telerik:RadComboBoxItem Value="Lithuania" Text="Lithuania" />
   <telerik:RadComboBoxItem Value="Luxembourg" Text="Luxembourg" />
   <telerik:RadComboBoxItem Value="Macau" Text="Macau" />
   <telerik:RadComboBoxItem Value="Macedonia" Text="Macedonia" />
   <telerik:RadComboBoxItem Value="Madagascar" Text="Madagascar" />
   <telerik:RadComboBoxItem Value="Malaysia" Text="Malaysia" />
   <telerik:RadComboBoxItem Value="Malawi" Text="Malawi" />
   <telerik:RadComboBoxItem Value="Maldives" Text="Maldives" />
   <telerik:RadComboBoxItem Value="Mali" Text="Mali" />
   <telerik:RadComboBoxItem Value="Malta" Text="Malta" />
   <telerik:RadComboBoxItem Value="Marshall Islands" Text="Marshall Islands" />
   <telerik:RadComboBoxItem Value="Martinique" Text="Martinique" />
   <telerik:RadComboBoxItem Value="Mauritania" Text="Mauritania" />
   <telerik:RadComboBoxItem Value="Mauritius" Text="Mauritius" />
   <telerik:RadComboBoxItem Value="Mayotte" Text="Mayotte" />
   <telerik:RadComboBoxItem Value="MX" Text="Mexico" />
   <telerik:RadComboBoxItem Value="Midway Islands" Text="Midway Islands" />
   <telerik:RadComboBoxItem Value="Moldova" Text="Moldova" />
   <telerik:RadComboBoxItem Value="Monaco" Text="Monaco" />
   <telerik:RadComboBoxItem Value="Mongolia" Text="Mongolia" />
   <telerik:RadComboBoxItem Value="Montserrat" Text="Montserrat" />
   <telerik:RadComboBoxItem Value="Morocco" Text="Morocco" />
   <telerik:RadComboBoxItem Value="Mozambique" Text="Mozambique" />
   <telerik:RadComboBoxItem Value="Myanmar" Text="Myanmar" />
   <telerik:RadComboBoxItem Value="Nambia" Text="Nambia" />
   <telerik:RadComboBoxItem Value="Nauru" Text="Nauru" />
   <telerik:RadComboBoxItem Value="Nepal" Text="Nepal" />
   <telerik:RadComboBoxItem Value="Netherland Antilles" Text="Netherland Antilles" />
   <telerik:RadComboBoxItem Value="Netherlands" Text="Netherlands (Holland, Europe)" />
   <telerik:RadComboBoxItem Value="Nevis" Text="Nevis" />
   <telerik:RadComboBoxItem Value="New Caledonia" Text="New Caledonia" />
   <telerik:RadComboBoxItem Value="New Zealand" Text="New Zealand" />
   <telerik:RadComboBoxItem Value="Nicaragua" Text="Nicaragua" />
   <telerik:RadComboBoxItem Value="Niger" Text="Niger" />
   <telerik:RadComboBoxItem Value="Nigeria" Text="Nigeria" />
   <telerik:RadComboBoxItem Value="Niue" Text="Niue" />
   <telerik:RadComboBoxItem Value="Norfolk Island" Text="Norfolk Island" />
   <telerik:RadComboBoxItem Value="Norway" Text="Norway" />
   <telerik:RadComboBoxItem Value="Oman" Text="Oman" />
   <telerik:RadComboBoxItem Value="Pakistan" Text="Pakistan" />
   <telerik:RadComboBoxItem Value="Palau Island" Text="Palau Island" />
   <telerik:RadComboBoxItem Value="Palestine" Text="Palestine" />
   <telerik:RadComboBoxItem Value="Panama" Text="Panama" />
   <telerik:RadComboBoxItem Value="Papua New Guinea" Text="Papua New Guinea" />
   <telerik:RadComboBoxItem Value="Paraguay" Text="Paraguay" />
   <telerik:RadComboBoxItem Value="Peru" Text="Peru" />
   <telerik:RadComboBoxItem Value="Phillipines" Text="Philippines" />
   <telerik:RadComboBoxItem Value="Pitcairn Island" Text="Pitcairn Island" />
   <telerik:RadComboBoxItem Value="Poland" Text="Poland" />
   <telerik:RadComboBoxItem Value="Portugal" Text="Portugal" />
   <telerik:RadComboBoxItem Value="Puerto Rico" Text="Puerto Rico" />
   <telerik:RadComboBoxItem Value="Qatar" Text="Qatar" />
   <telerik:RadComboBoxItem Value="Republic of Montenegro" Text="Republic of Montenegro" />
   <telerik:RadComboBoxItem Value="Republic of Serbia" Text="Republic of Serbia" />
   <telerik:RadComboBoxItem Value="Reunion" Text="Reunion" />
   <telerik:RadComboBoxItem Value="Romania" Text="Romania" />
   <telerik:RadComboBoxItem Value="Russia" Text="Russia" />
   <telerik:RadComboBoxItem Value="Rwanda" Text="Rwanda" />
   <telerik:RadComboBoxItem Value="St Barthelemy" Text="St Barthelemy" />
   <telerik:RadComboBoxItem Value="St Eustatius" Text="St Eustatius" />
   <telerik:RadComboBoxItem Value="St Helena" Text="St Helena" />
   <telerik:RadComboBoxItem Value="St Kitts-Nevis" Text="St Kitts-Nevis" />
   <telerik:RadComboBoxItem Value="St Lucia" Text="St Lucia" />
   <telerik:RadComboBoxItem Value="St Maarten" Text="St Maarten" />
   <telerik:RadComboBoxItem Value="St Pierre & Miquelon" Text="St Pierre & Miquelon" />
   <telerik:RadComboBoxItem Value="St Vincent & Grenadines" Text="St Vincent & Grenadines" />
   <telerik:RadComboBoxItem Value="Saipan" Text="Saipan" />
   <telerik:RadComboBoxItem Value="Samoa" Text="Samoa" />
   <telerik:RadComboBoxItem Value="Samoa American" Text="Samoa American" />
   <telerik:RadComboBoxItem Value="San Marino" Text="San Marino" />
   <telerik:RadComboBoxItem Value="Sao Tome & Principe" Text="Sao Tome & Principe" />
   <telerik:RadComboBoxItem Value="Saudi Arabia" Text="Saudi Arabia" />
   <telerik:RadComboBoxItem Value="Senegal" Text="Senegal" />
   <telerik:RadComboBoxItem Value="Seychelles" Text="Seychelles" />
   <telerik:RadComboBoxItem Value="Sierra Leone" Text="Sierra Leone" />
   <telerik:RadComboBoxItem Value="Singapore" Text="Singapore" />
   <telerik:RadComboBoxItem Value="Slovakia" Text="Slovakia" />
   <telerik:RadComboBoxItem Value="Slovenia" Text="Slovenia" />
   <telerik:RadComboBoxItem Value="Solomon Islands" Text="Solomon Islands" />
   <telerik:RadComboBoxItem Value="Somalia" Text="Somalia" />
   <telerik:RadComboBoxItem Value="South Africa" Text="South Africa" />
   <telerik:RadComboBoxItem Value="Spain" Text="Spain" />
   <telerik:RadComboBoxItem Value="Sri Lanka" Text="Sri Lanka" />
   <telerik:RadComboBoxItem Value="Sudan" Text="Sudan" />
   <telerik:RadComboBoxItem Value="Suriname" Text="Suriname" />
   <telerik:RadComboBoxItem Value="Swaziland" Text="Swaziland" />
   <telerik:RadComboBoxItem Value="Sweden" Text="Sweden" />
   <telerik:RadComboBoxItem Value="Switzerland" Text="Switzerland" />
   <telerik:RadComboBoxItem Value="Syria" Text="Syria" />
   <telerik:RadComboBoxItem Value="Tahiti" Text="Tahiti" />
   <telerik:RadComboBoxItem Value="Taiwan" Text="Taiwan" />
   <telerik:RadComboBoxItem Value="Tajikistan" Text="Tajikistan" />
   <telerik:RadComboBoxItem Value="Tanzania" Text="Tanzania" />
   <telerik:RadComboBoxItem Value="Thailand" Text="Thailand" />
   <telerik:RadComboBoxItem Value="Togo" Text="Togo" />
   <telerik:RadComboBoxItem Value="Tokelau" Text="Tokelau" />
   <telerik:RadComboBoxItem Value="Tonga" Text="Tonga" />
   <telerik:RadComboBoxItem Value="Trinidad & Tobago" Text="Trinidad & Tobago" />
   <telerik:RadComboBoxItem Value="Tunisia" Text="Tunisia" />
   <telerik:RadComboBoxItem Value="Turkey" Text="Turkey" />
   <telerik:RadComboBoxItem Value="Turkmenistan" Text="Turkmenistan" />
   <telerik:RadComboBoxItem Value="Turks & Caicos Is" Text="Turks & Caicos Is" />
   <telerik:RadComboBoxItem Value="Tuvalu" Text="Tuvalu" />
   <telerik:RadComboBoxItem Value="Uganda" Text="Uganda" />
   <telerik:RadComboBoxItem Value="United Kingdom" Text="United Kingdom" />
   <telerik:RadComboBoxItem Value="Ukraine" Text="Ukraine" />
   <telerik:RadComboBoxItem Value="United Arab Erimates" Text="United Arab Emirates" />
   <telerik:RadComboBoxItem Value="United States" Text="United States"/>
   <telerik:RadComboBoxItem Value="Uraguay" Text="Uruguay" />
   <telerik:RadComboBoxItem Value="Uzbekistan" Text="Uzbekistan" />
   <telerik:RadComboBoxItem Value="Vanuatu" Text="Vanuatu" />
   <telerik:RadComboBoxItem Value="Vatican City State" Text="Vatican City State" />
   <telerik:RadComboBoxItem Value="Venezuela" Text="Venezuela" />
   <telerik:RadComboBoxItem Value="Vietnam" Text="Vietnam" />
   <telerik:RadComboBoxItem Value="Virgin Islands (Brit)" Text="Virgin Islands (Brit)" />
   <telerik:RadComboBoxItem Value="Virgin Islands (USA)" Text="Virgin Islands (USA)" />
   <telerik:RadComboBoxItem Value="Wake Island" Text="Wake Island" />
   <telerik:RadComboBoxItem Value="Wallis & Futana Is" Text="Wallis & Futana Is" />
   <telerik:RadComboBoxItem Value="Yemen" Text="Yemen" />
   <telerik:RadComboBoxItem Value="Zaire" Text="Zaire" />
   <telerik:RadComboBoxItem Value="Zambia" Text="Zambia" />
   <telerik:RadComboBoxItem Value="Zimbabwe" Text="Zimbabwe" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>

         <tr runat="server" id="nonAPACcity">
                <td >City:</td>
                <td>
                  <input class="txtBox" style="width:242px;" id="tbSCity" name="locality" runat="server"
          required="required" />
                </td>
            </tr>


         <tr runat="server" id="APACcity">
                <td >City:</td>
                <td>

                    <asp:DropDownList ID="ddlShipCity" runat="server" width="250px"></asp:DropDownList>
                  </td>
            </tr>

        <%--<tr>
                <td style="width:250px">
                    Country:</td>
                <td>
                    <input class="txtBox" id="cbSCountry" name="country" runat="server"
          required="required" />
                </td>
            </tr>--%>
        
        <tr id="fulfillmentlocationholder" runat="server">
                <td style="width:250px">
                    Fulfillment location :</td>
                <td>
                     <telerik:RadComboBox ID="cbsLocation" Runat="server" AutoCompleteSeparator="" MarkFirstMatch="true"  width="250px"  OnClientSelectedIndexChanged="OnClientSelectedIndexChanged"> 
                        <Items>
                            <%-- <telerik:RadComboBoxItem Value="United States" Text="United States" />
                             <telerik:RadComboBoxItem Value="Europe" Text="Europe" />
                             <telerik:RadComboBoxItem Value="Japan" Text="Japan" />
                              <telerik:RadComboBoxItem Value="China" Text="China" />
                              <telerik:RadComboBoxItem Value="Australia" Text="Australia" />--%>
                            </Items>
                         </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    Asset Arrival Date:</td>
                <td><telerik:RadDatePicker ID="tbArrivalDate" runat="server" Calendar-ShowRowHeaders="false" Width="250px">
                      <Calendar runat="server" EnableWeekends="False" FastNavigationNextText="&amp;lt;&amp;lt;" ShowRowHeaders="false" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                </Calendar>
                      <ClientEvents OnDateSelected="dateSelected" />
                    </telerik:RadDatePicker>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Arrival Date" ControlToValidate="tbArrivalDate" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator></td>
            </tr>
            <tr runat="server" id="tr1"><td>Shipping Type:</td><td><asp:RadioButtonList ID="rbShippingType" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem value="Residential">Residential</asp:ListItem>
                                                <asp:ListItem value="Commerical"  Selected="True" >Commerical</asp:ListItem>
                                            </asp:RadioButtonList></td></tr>
        <tr><td style="white-space: nowrap;width:200px;vertical-align:top;">Attach Document(s):<div style="width:200px"></div></td>
            <td>
                <telerik:RadAsyncUpload ID="RadAsyncUpload1" runat="server" 
                        MultipleFileSelection="Automatic" OnClientFileSelected="onFileSelected" OnClientFileUploaded="onFileUploaded" OnClientFileUploadFailed="onUploadFailed">
                    </telerik:RadAsyncUpload>
            </td>
        </tr>
        

        <tr runat="server" id="trTracking">
                <td>Tracking Number:</td>
                <td>
                    <telerik:RadTextBox ID="tbTracking" Runat="server" Name="tbTracking" width="250px" >
                    </telerik:RadTextBox>
                </td>
            </tr>

        <tr>
                <td></td>
                <td><asp:CheckBox ID="CheckBox1" runat="server" text="Copy Shipping" onclick="FillBilling()"  name="billingtoo" AutoPostBack="false" Visible="false" /></td>
            </tr>



    </table>       
    <table class="entry" style="display:none">
         <tr>
          <td colspan="2"><h2>Pickup Information</h2></td>
         </tr>
        <tr>
          <td>Pickup from:</td>
          <td>
                    <telerik:RadTextBox ID="tbPickupfrom" Runat="server" Name="tbPickupfrom" >
                    </telerik:RadTextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" 
                        ErrorMessage="Pickup from" ControlToValidate="tbPickupfrom" ForeColor="Red" ValidationGroup="ValidateFields" Enabled="false">*</asp:RequiredFieldValidator>
                </td> 
         </tr>
        <tr>
          <td>Onsite Contact Name:</td>
          <td>
                    <telerik:RadTextBox ID="tbPOnsiteContact" Runat="server" >
                    </telerik:RadTextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" 
                        ErrorMessage="Pickup Onsite Contact" ControlToValidate="tbPOnsiteContact" ForeColor="Red" ValidationGroup="ValidateFields" Enabled="false">*</asp:RequiredFieldValidator>
                </td> 
         </tr>
        <tr>
          <td>Onsite Contact Phone #:</td>
          <td>
                    <telerik:RadNumericTextBox ID="tbPOnsitePhone" Runat="server" NumberFormat-DecimalDigits="0">
            <NegativeStyle Resize="None" />
            <NumberFormat DecimalDigits="0" GroupSeparator="" ZeroPattern="n" />
            <EmptyMessageStyle Resize="None" />
            <ReadOnlyStyle Resize="None" />
            <FocusedStyle Resize="None" />
            <DisabledStyle Resize="None" />
            <InvalidStyle Resize="None" />
            <HoveredStyle Resize="None" />
            <EnabledStyle Resize="None" />
                    </telerik:RadNumericTextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" 
                        ErrorMessage="Pickup Onsite Contact Phone #" ControlToValidate="tbPOnsitePhone" ForeColor="Red" ValidationGroup="ValidateFields" Enabled="false">*</asp:RequiredFieldValidator>
                </td> 
         </tr>
        <tr>
          <td>Onsite Contact Email:</td>
          <td>
                    <telerik:RadTextBox ID="tbPOnsiteEmail" Runat="server" >
                    </telerik:RadTextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" 
                        ErrorMessage="Pickup Onsite Contact Email" ControlToValidate="tbPOnsiteEmail" ForeColor="Red" ValidationGroup="ValidateFields" Enabled="false">*</asp:RequiredFieldValidator>
              <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic"
    ErrorMessage="Please, enter valid e-mail address." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$" ForeColor="Red" ControlToValidate="tbPOnsiteEmail" ValidationGroup="ValidateFields" Enabled="false">*</asp:RegularExpressionValidator>
                </td> 
         </tr>
        <tr>
          <td>Address Line 1:</td>
          <td>
                    <telerik:RadTextBox ID="tbPAddress1" Runat="server" >
                    </telerik:RadTextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" 
                        ErrorMessage="Pickup Address" ControlToValidate="tbPAddress1" ForeColor="Red" ValidationGroup="ValidateFields" Enabled="false">*</asp:RequiredFieldValidator>
                </td> 
         </tr>
        <tr>
          <td>Address Line 2:</td>
          <td>
                    <telerik:RadTextBox ID="tbPAddress2" Runat="server">
                    </telerik:RadTextBox>
                </td> 
         </tr>
        <tr>
          <td>City:</td>
          <td>
                    <telerik:RadTextBox ID="tbPCity" Runat="server" >
                    </telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" 
                        ErrorMessage="Pickup City" ControlToValidate="tbPCity" ForeColor="Red" ValidationGroup="ValidateFields" Enabled="false">*</asp:RequiredFieldValidator>
                </td> 
         </tr>
        <tr>
          <td valign="top"><div style="vertical-align:top;">State/Province/Region:</div></td>
          <td>
                            <telerik:RadComboBox ID="cbPState" Runat="server" Height="140px"  MarkFirstMatch="True" Filter="Contains">
                            </telerik:RadComboBox>
                            <telerik:RadTextBox ID="tbPRegion" Runat="server" style="display:none;" >
                            </telerik:RadTextBox>
                        </td> 
         </tr>
        <tr>
          <td>ZIP/Postal Code:</td>
          <td>
                    <telerik:RadTextBox ID="tbPZip" Runat="server" >
                    </telerik:RadTextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" 
                        ErrorMessage="Pickup Zip" ControlToValidate="tbPZip" ForeColor="Red" ValidationGroup="ValidateFields" Enabled="false">*</asp:RequiredFieldValidator>
                </td> 
         </tr>
        <tr>
          <td style="width:250px">Country:</td>
          <td>
                    <telerik:RadComboBox ID="cbPCountry" Runat="server" AutoCompleteSeparator="" MarkFirstMatch="true" OnClientSelectedIndexChanged="OnPickupClientSelectedIndexChanged" >
                         <Items>
                       
   <telerik:RadComboBoxItem Value="Afganistan" Text="Afghanistan" />
   <telerik:RadComboBoxItem Value="Albania" Text="Albania" />
   <telerik:RadComboBoxItem Value="Algeria" Text="Algeria" />
   <telerik:RadComboBoxItem Value="American Samoa" Text="American Samoa" />
   <telerik:RadComboBoxItem Value="Andorra" Text="Andorra" />
   <telerik:RadComboBoxItem Value="Angola" Text="Angola" />
   <telerik:RadComboBoxItem Value="Anguilla" Text="Anguilla" />
   <telerik:RadComboBoxItem Value="Antigua & Barbuda" Text="Antigua & Barbuda" />
   <telerik:RadComboBoxItem Value="Argentina" Text="Argentina" />
   <telerik:RadComboBoxItem Value="Armenia" Text="Armenia" />
   <telerik:RadComboBoxItem Value="Aruba" Text="Aruba" />
   <telerik:RadComboBoxItem Value="Australia" Text="Australia" />
   <telerik:RadComboBoxItem Value="Austria" Text="Austria" />
   <telerik:RadComboBoxItem Value="Azerbaijan" Text="Azerbaijan" />
   <telerik:RadComboBoxItem Value="Bahamas" Text="Bahamas" />
   <telerik:RadComboBoxItem Value="Bahrain" Text="Bahrain" />
   <telerik:RadComboBoxItem Value="Bangladesh" Text="Bangladesh" />
   <telerik:RadComboBoxItem Value="Barbados" Text="Barbados" />
   <telerik:RadComboBoxItem Value="Belarus" Text="Belarus" />
   <telerik:RadComboBoxItem Value="Belgium" Text="Belgium" />
   <telerik:RadComboBoxItem Value="Belize" Text="Belize" />
   <telerik:RadComboBoxItem Value="Benin" Text="Benin" />
   <telerik:RadComboBoxItem Value="Bermuda" Text="Bermuda" />
   <telerik:RadComboBoxItem Value="Bhutan" Text="Bhutan" />
   <telerik:RadComboBoxItem Value="Bolivia" Text="Bolivia" />
   <telerik:RadComboBoxItem Value="Bonaire" Text="Bonaire" />
   <telerik:RadComboBoxItem Value="Bosnia & Herzegovina" Text="Bosnia & Herzegovina" />
   <telerik:RadComboBoxItem Value="Botswana" Text="Botswana" />
   <telerik:RadComboBoxItem Value="Brazil" Text="Brazil" />
   <telerik:RadComboBoxItem Value="British Indian Ocean Ter" Text="British Indian Ocean Ter" />
   <telerik:RadComboBoxItem Value="Brunei" Text="Brunei" />
   <telerik:RadComboBoxItem Value="Bulgaria" Text="Bulgaria" />
   <telerik:RadComboBoxItem Value="Burkina Faso" Text="Burkina Faso" />
   <telerik:RadComboBoxItem Value="Burundi" Text="Burundi" />
   <telerik:RadComboBoxItem Value="Cambodia" Text="Cambodia" />
   <telerik:RadComboBoxItem Value="Cameroon" Text="Cameroon" />
   <telerik:RadComboBoxItem Value="CA" Text="Canada" />
   <telerik:RadComboBoxItem Value="Canary Islands" Text="Canary Islands" />
   <telerik:RadComboBoxItem Value="Cape Verde" Text="Cape Verde" />
   <telerik:RadComboBoxItem Value="Cayman Islands" Text="Cayman Islands" />
   <telerik:RadComboBoxItem Value="Central African Republic" Text="Central African Republic" />
   <telerik:RadComboBoxItem Value="Chad" Text="Chad" />
   <telerik:RadComboBoxItem Value="Channel Islands" Text="Channel Islands" />
   <telerik:RadComboBoxItem Value="Chile" Text="Chile" />
   <telerik:RadComboBoxItem Value="China" Text="China" />
   <telerik:RadComboBoxItem Value="Christmas Island" Text="Christmas Island" />
   <telerik:RadComboBoxItem Value="Cocos Island" Text="Cocos Island" />
   <telerik:RadComboBoxItem Value="Colombia" Text="Colombia" />
   <telerik:RadComboBoxItem Value="Comoros" Text="Comoros" />
   <telerik:RadComboBoxItem Value="Congo" Text="Congo" />
   <telerik:RadComboBoxItem Value="Cook Islands" Text="Cook Islands" />
   <telerik:RadComboBoxItem Value="Costa Rica" Text="Costa Rica" />
   <telerik:RadComboBoxItem Value="Cote DIvoire" Text="Cote DIvoire" />
   <telerik:RadComboBoxItem Value="Croatia" Text="Croatia" />
   <telerik:RadComboBoxItem Value="Cuba" Text="Cuba" />
   <telerik:RadComboBoxItem Value="Curaco" Text="Curacao" />
   <telerik:RadComboBoxItem Value="Cyprus" Text="Cyprus" />
   <telerik:RadComboBoxItem Value="Czech Republic" Text="Czech Republic" />
   <telerik:RadComboBoxItem Value="Denmark" Text="Denmark" />
   <telerik:RadComboBoxItem Value="Djibouti" Text="Djibouti" />
   <telerik:RadComboBoxItem Value="Dominica" Text="Dominica" />
   <telerik:RadComboBoxItem Value="Dominican Republic" Text="Dominican Republic" />
   <telerik:RadComboBoxItem Value="East Timor" Text="East Timor" />
   <telerik:RadComboBoxItem Value="Ecuador" Text="Ecuador" />
   <telerik:RadComboBoxItem Value="Egypt" Text="Egypt" />
   <telerik:RadComboBoxItem Value="El Salvador" Text="El Salvador" />
   <telerik:RadComboBoxItem Value="Equatorial Guinea" Text="Equatorial Guinea" />
   <telerik:RadComboBoxItem Value="Eritrea" Text="Eritrea" />
   <telerik:RadComboBoxItem Value="Estonia" Text="Estonia" />
   <telerik:RadComboBoxItem Value="Ethiopia" Text="Ethiopia" />
   <telerik:RadComboBoxItem Value="Falkland Islands" Text="Falkland Islands" />
   <telerik:RadComboBoxItem Value="Faroe Islands" Text="Faroe Islands" />
   <telerik:RadComboBoxItem Value="Fiji" Text="Fiji" />
   <telerik:RadComboBoxItem Value="Finland" Text="Finland" />
   <telerik:RadComboBoxItem Value="France" Text="France" />
   <telerik:RadComboBoxItem Value="French Guiana" Text="French Guiana" />
   <telerik:RadComboBoxItem Value="French Polynesia" Text="French Polynesia" />
   <telerik:RadComboBoxItem Value="French Southern Ter" Text="French Southern Ter" />
   <telerik:RadComboBoxItem Value="Gabon" Text="Gabon" />
   <telerik:RadComboBoxItem Value="Gambia" Text="Gambia" />
   <telerik:RadComboBoxItem Value="Georgia" Text="Georgia" />
   <telerik:RadComboBoxItem Value="Germany" Text="Germany" />
   <telerik:RadComboBoxItem Value="Ghana" Text="Ghana" />
   <telerik:RadComboBoxItem Value="Gibraltar" Text="Gibraltar" />
   <telerik:RadComboBoxItem Value="Great Britain" Text="Great Britain" />
   <telerik:RadComboBoxItem Value="Greece" Text="Greece" />
   <telerik:RadComboBoxItem Value="Greenland" Text="Greenland" />
   <telerik:RadComboBoxItem Value="Grenada" Text="Grenada" />
   <telerik:RadComboBoxItem Value="Guadeloupe" Text="Guadeloupe" />
   <telerik:RadComboBoxItem Value="Guam" Text="Guam" />
   <telerik:RadComboBoxItem Value="Guatemala" Text="Guatemala" />
   <telerik:RadComboBoxItem Value="Guinea" Text="Guinea" />
   <telerik:RadComboBoxItem Value="Guyana" Text="Guyana" />
   <telerik:RadComboBoxItem Value="Haiti" Text="Haiti" />
   <telerik:RadComboBoxItem Value="Hawaii" Text="Hawaii" />
   <telerik:RadComboBoxItem Value="Honduras" Text="Honduras" />
   <telerik:RadComboBoxItem Value="Hong Kong" Text="Hong Kong" />
   <telerik:RadComboBoxItem Value="Hungary" Text="Hungary" />
   <telerik:RadComboBoxItem Value="Iceland" Text="Iceland" />
   <telerik:RadComboBoxItem Value="Indonesia" Text="Indonesia" />
   <telerik:RadComboBoxItem Value="India" Text="India" />
   <telerik:RadComboBoxItem Value="Iran" Text="Iran" />
   <telerik:RadComboBoxItem Value="Iraq" Text="Iraq" />
   <telerik:RadComboBoxItem Value="Ireland" Text="Ireland" />
   <telerik:RadComboBoxItem Value="Isle of Man" Text="Isle of Man" />
   <telerik:RadComboBoxItem Value="Israel" Text="Israel" />
   <telerik:RadComboBoxItem Value="Italy" Text="Italy" />
   <telerik:RadComboBoxItem Value="Jamaica" Text="Jamaica" />
   <telerik:RadComboBoxItem Value="Japan" Text="Japan" />
   <telerik:RadComboBoxItem Value="Jordan" Text="Jordan" />
   <telerik:RadComboBoxItem Value="Kazakhstan" Text="Kazakhstan" />
   <telerik:RadComboBoxItem Value="Kenya" Text="Kenya" />
   <telerik:RadComboBoxItem Value="Kiribati" Text="Kiribati" />
   <telerik:RadComboBoxItem Value="Korea North" Text="Korea North" />
   <telerik:RadComboBoxItem Value="Korea Sout" Text="Korea South" />
   <telerik:RadComboBoxItem Value="Kuwait" Text="Kuwait" />
   <telerik:RadComboBoxItem Value="Kyrgyzstan" Text="Kyrgyzstan" />
   <telerik:RadComboBoxItem Value="Laos" Text="Laos" />
   <telerik:RadComboBoxItem Value="Latvia" Text="Latvia" />
   <telerik:RadComboBoxItem Value="Lebanon" Text="Lebanon" />
   <telerik:RadComboBoxItem Value="Lesotho" Text="Lesotho" />
   <telerik:RadComboBoxItem Value="Liberia" Text="Liberia" />
   <telerik:RadComboBoxItem Value="Libya" Text="Libya" />
   <telerik:RadComboBoxItem Value="Liechtenstein" Text="Liechtenstein" />
   <telerik:RadComboBoxItem Value="Lithuania" Text="Lithuania" />
   <telerik:RadComboBoxItem Value="Luxembourg" Text="Luxembourg" />
   <telerik:RadComboBoxItem Value="Macau" Text="Macau" />
   <telerik:RadComboBoxItem Value="Macedonia" Text="Macedonia" />
   <telerik:RadComboBoxItem Value="Madagascar" Text="Madagascar" />
   <telerik:RadComboBoxItem Value="Malaysia" Text="Malaysia" />
   <telerik:RadComboBoxItem Value="Malawi" Text="Malawi" />
   <telerik:RadComboBoxItem Value="Maldives" Text="Maldives" />
   <telerik:RadComboBoxItem Value="Mali" Text="Mali" />
   <telerik:RadComboBoxItem Value="Malta" Text="Malta" />
   <telerik:RadComboBoxItem Value="Marshall Islands" Text="Marshall Islands" />
   <telerik:RadComboBoxItem Value="Martinique" Text="Martinique" />
   <telerik:RadComboBoxItem Value="Mauritania" Text="Mauritania" />
   <telerik:RadComboBoxItem Value="Mauritius" Text="Mauritius" />
   <telerik:RadComboBoxItem Value="Mayotte" Text="Mayotte" />
   <telerik:RadComboBoxItem Value="MX" Text="Mexico" />
   <telerik:RadComboBoxItem Value="Midway Islands" Text="Midway Islands" />
   <telerik:RadComboBoxItem Value="Moldova" Text="Moldova" />
   <telerik:RadComboBoxItem Value="Monaco" Text="Monaco" />
   <telerik:RadComboBoxItem Value="Mongolia" Text="Mongolia" />
   <telerik:RadComboBoxItem Value="Montserrat" Text="Montserrat" />
   <telerik:RadComboBoxItem Value="Morocco" Text="Morocco" />
   <telerik:RadComboBoxItem Value="Mozambique" Text="Mozambique" />
   <telerik:RadComboBoxItem Value="Myanmar" Text="Myanmar" />
   <telerik:RadComboBoxItem Value="Nambia" Text="Nambia" />
   <telerik:RadComboBoxItem Value="Nauru" Text="Nauru" />
   <telerik:RadComboBoxItem Value="Nepal" Text="Nepal" />
   <telerik:RadComboBoxItem Value="Netherland Antilles" Text="Netherland Antilles" />
   <telerik:RadComboBoxItem Value="Netherlands" Text="Netherlands (Holland, Europe)" />
   <telerik:RadComboBoxItem Value="Nevis" Text="Nevis" />
   <telerik:RadComboBoxItem Value="New Caledonia" Text="New Caledonia" />
   <telerik:RadComboBoxItem Value="New Zealand" Text="New Zealand" />
   <telerik:RadComboBoxItem Value="Nicaragua" Text="Nicaragua" />
   <telerik:RadComboBoxItem Value="Niger" Text="Niger" />
   <telerik:RadComboBoxItem Value="Nigeria" Text="Nigeria" />
   <telerik:RadComboBoxItem Value="Niue" Text="Niue" />
   <telerik:RadComboBoxItem Value="Norfolk Island" Text="Norfolk Island" />
   <telerik:RadComboBoxItem Value="Norway" Text="Norway" />
   <telerik:RadComboBoxItem Value="Oman" Text="Oman" />
   <telerik:RadComboBoxItem Value="Pakistan" Text="Pakistan" />
   <telerik:RadComboBoxItem Value="Palau Island" Text="Palau Island" />
   <telerik:RadComboBoxItem Value="Palestine" Text="Palestine" />
   <telerik:RadComboBoxItem Value="Panama" Text="Panama" />
   <telerik:RadComboBoxItem Value="Papua New Guinea" Text="Papua New Guinea" />
   <telerik:RadComboBoxItem Value="Paraguay" Text="Paraguay" />
   <telerik:RadComboBoxItem Value="Peru" Text="Peru" />
   <telerik:RadComboBoxItem Value="Phillipines" Text="Philippines" />
   <telerik:RadComboBoxItem Value="Pitcairn Island" Text="Pitcairn Island" />
   <telerik:RadComboBoxItem Value="Poland" Text="Poland" />
   <telerik:RadComboBoxItem Value="Portugal" Text="Portugal" />
   <telerik:RadComboBoxItem Value="Puerto Rico" Text="Puerto Rico" />
   <telerik:RadComboBoxItem Value="Qatar" Text="Qatar" />
   <telerik:RadComboBoxItem Value="Republic of Montenegro" Text="Republic of Montenegro" />
   <telerik:RadComboBoxItem Value="Republic of Serbia" Text="Republic of Serbia" />
   <telerik:RadComboBoxItem Value="Reunion" Text="Reunion" />
   <telerik:RadComboBoxItem Value="Romania" Text="Romania" />
   <telerik:RadComboBoxItem Value="Russia" Text="Russia" />
   <telerik:RadComboBoxItem Value="Rwanda" Text="Rwanda" />
   <telerik:RadComboBoxItem Value="St Barthelemy" Text="St Barthelemy" />
   <telerik:RadComboBoxItem Value="St Eustatius" Text="St Eustatius" />
   <telerik:RadComboBoxItem Value="St Helena" Text="St Helena" />
   <telerik:RadComboBoxItem Value="St Kitts-Nevis" Text="St Kitts-Nevis" />
   <telerik:RadComboBoxItem Value="St Lucia" Text="St Lucia" />
   <telerik:RadComboBoxItem Value="St Maarten" Text="St Maarten" />
   <telerik:RadComboBoxItem Value="St Pierre & Miquelon" Text="St Pierre & Miquelon" />
   <telerik:RadComboBoxItem Value="St Vincent & Grenadines" Text="St Vincent & Grenadines" />
   <telerik:RadComboBoxItem Value="Saipan" Text="Saipan" />
   <telerik:RadComboBoxItem Value="Samoa" Text="Samoa" />
   <telerik:RadComboBoxItem Value="Samoa American" Text="Samoa American" />
   <telerik:RadComboBoxItem Value="San Marino" Text="San Marino" />
   <telerik:RadComboBoxItem Value="Sao Tome & Principe" Text="Sao Tome & Principe" />
   <telerik:RadComboBoxItem Value="Saudi Arabia" Text="Saudi Arabia" />
   <telerik:RadComboBoxItem Value="Senegal" Text="Senegal" />
   <telerik:RadComboBoxItem Value="Seychelles" Text="Seychelles" />
   <telerik:RadComboBoxItem Value="Sierra Leone" Text="Sierra Leone" />
   <telerik:RadComboBoxItem Value="Singapore" Text="Singapore" />
   <telerik:RadComboBoxItem Value="Slovakia" Text="Slovakia" />
   <telerik:RadComboBoxItem Value="Slovenia" Text="Slovenia" />
   <telerik:RadComboBoxItem Value="Solomon Islands" Text="Solomon Islands" />
   <telerik:RadComboBoxItem Value="Somalia" Text="Somalia" />
   <telerik:RadComboBoxItem Value="South Africa" Text="South Africa" />
   <telerik:RadComboBoxItem Value="Spain" Text="Spain" />
   <telerik:RadComboBoxItem Value="Sri Lanka" Text="Sri Lanka" />
   <telerik:RadComboBoxItem Value="Sudan" Text="Sudan" />
   <telerik:RadComboBoxItem Value="Suriname" Text="Suriname" />
   <telerik:RadComboBoxItem Value="Swaziland" Text="Swaziland" />
   <telerik:RadComboBoxItem Value="Sweden" Text="Sweden" />
   <telerik:RadComboBoxItem Value="Switzerland" Text="Switzerland" />
   <telerik:RadComboBoxItem Value="Syria" Text="Syria" />
   <telerik:RadComboBoxItem Value="Tahiti" Text="Tahiti" />
   <telerik:RadComboBoxItem Value="Taiwan" Text="Taiwan" />
   <telerik:RadComboBoxItem Value="Tajikistan" Text="Tajikistan" />
   <telerik:RadComboBoxItem Value="Tanzania" Text="Tanzania" />
   <telerik:RadComboBoxItem Value="Thailand" Text="Thailand" />
   <telerik:RadComboBoxItem Value="Togo" Text="Togo" />
   <telerik:RadComboBoxItem Value="Tokelau" Text="Tokelau" />
   <telerik:RadComboBoxItem Value="Tonga" Text="Tonga" />
   <telerik:RadComboBoxItem Value="Trinidad & Tobago" Text="Trinidad & Tobago" />
   <telerik:RadComboBoxItem Value="Tunisia" Text="Tunisia" />
   <telerik:RadComboBoxItem Value="Turkey" Text="Turkey" />
   <telerik:RadComboBoxItem Value="Turkmenistan" Text="Turkmenistan" />
   <telerik:RadComboBoxItem Value="Turks & Caicos Is" Text="Turks & Caicos Is" />
   <telerik:RadComboBoxItem Value="Tuvalu" Text="Tuvalu" />
   <telerik:RadComboBoxItem Value="Uganda" Text="Uganda" />
   <telerik:RadComboBoxItem Value="United Kingdom" Text="United Kingdom" />
   <telerik:RadComboBoxItem Value="Ukraine" Text="Ukraine" />
   <telerik:RadComboBoxItem Value="United Arab Erimates" Text="United Arab Emirates" />
   <telerik:RadComboBoxItem Value="US" Text="United States" Selected="TRUE"/>
   <telerik:RadComboBoxItem Value="Uraguay" Text="Uruguay" />
   <telerik:RadComboBoxItem Value="Uzbekistan" Text="Uzbekistan" />
   <telerik:RadComboBoxItem Value="Vanuatu" Text="Vanuatu" />
   <telerik:RadComboBoxItem Value="Vatican City State" Text="Vatican City State" />
   <telerik:RadComboBoxItem Value="Venezuela" Text="Venezuela" />
   <telerik:RadComboBoxItem Value="Vietnam" Text="Vietnam" />
   <telerik:RadComboBoxItem Value="Virgin Islands (Brit)" Text="Virgin Islands (Brit)" />
   <telerik:RadComboBoxItem Value="Virgin Islands (USA)" Text="Virgin Islands (USA)" />
   <telerik:RadComboBoxItem Value="Wake Island" Text="Wake Island" />
   <telerik:RadComboBoxItem Value="Wallis & Futana Is" Text="Wallis & Futana Is" />
   <telerik:RadComboBoxItem Value="Yemen" Text="Yemen" />
   <telerik:RadComboBoxItem Value="Zaire" Text="Zaire" />
   <telerik:RadComboBoxItem Value="Zambia" Text="Zambia" />
   <telerik:RadComboBoxItem Value="Zimbabwe" Text="Zimbabwe" />
                        </Items>
                    </telerik:RadComboBox>
                </td> 
         </tr>

                 <tr>
                <td></td>
               <td>
                   <input class="txtBox" id="statusHolder" name="statusHolder" runat="server"  Visible="false" />
                </td>
                  <td>
                   <input class="txtBox" id="arrivalDateHolder" name="arrivalDateHolder" runat="server"  Visible="false" />
                </td>
                 <td>
                   <input class="txtBox" id="dateEnd" name="dateEnd" runat="server"  Visible="false" />
                </td>
                 <td>
                   <input class="txtBox" id="dateStart" name="dateStart" runat="server"  Visible="false" />
                </td>
                     <td>
                     <input class="txtBox" id="shipCountry" name="shipCountry" runat="server" Visible="false" /> 
                </td>
                     <%--<td>
                     <input class="txtBox" id="AURegionalAdmin" name="AURegionalAdmin" runat="server" Visible="false" /> 
                </td>
                     <td>
                     <input class="txtBox" id="CNRegionalAdmin" name="CNRegionalAdmin" runat="server" Visible="false" /> 
                </td>
                     <td>
                     <input class="txtBox" id="JPRegionalAdmin" name="JPRegionalAdmin" runat="server" Visible="false" /> 
                </td>--%>
            </tr>

    </table>
    
    <br />
    <br />
    
    <table class="entry" style="height:80px;margin-top:50px;display:none;">
        
        <tr>
            <td><telerik:RadTextBox ID="tbSearch" runat="server" CssClass="search" Height="40px" Width="350px" EmptyMessage="SEARCH" BackColor="#ededed" Font-Size="18px" ></telerik:RadTextBox><telerik:RadButton ID="RadButton1" runat="server" Text="Search" Height="40px" style="margin-left:20px;" RenderMode="Lightweight"></telerik:RadButton></td>
        </tr>
    </table>
    </telerik:RadAjaxPanel>

    <telerik:RadButton ID="RadButton2" runat="server" Text="Select Inventory" Height="40px" style="margin-left:100px;margin-top:50px;margin-bottom:50px;" RenderMode="Lightweight"  ValidationGroup="ValidateFields" OnClientClicked="Click1"  OnClientClick="validateOrderQuantity()"></telerik:RadButton>
    <telerik:RadButton ID="RadButton3" runat="server" Text="Review Order" Height="40px" style="margin-left:50px;margin-top:50px;margin-bottom:50px;"  OnClientClicked="Click2" ValidationGroup="ValidateFields" RenderMode="Lightweight"></telerik:RadButton>
    <telerik:RadButton ID="RadButton4" runat="server" Text="Cancel Changes" Height="40px" style="margin-left:50px;margin-top:50px;margin-bottom:50px;"  OnClientClicking="StandardConfirm"  ValidationGroup="ValidateFields" RenderMode="Lightweight"></telerik:RadButton>
    <asp:Label ID="lbError" ForeColor="Red" runat="server" />
</asp:Content>


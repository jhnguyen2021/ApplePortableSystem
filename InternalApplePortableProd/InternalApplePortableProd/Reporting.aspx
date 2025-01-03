
<%@ Page Title="Reporting" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Reporting.aspx.vb" Inherits="Reporting " ValidateRequest="false"%>
<%@ Register assembly="Telerik.Web.UI" Namespace ="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <script type="text/javascript" src="Scripts/jquery-1.7.2.min.js"></script>
<script src="Scripts/jquery.fancybox.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="Scripts/jquery.fancybox.css" />
<telerik:radscriptblock id="RadScriptBlock1" runat="server">
<script type="text/javascript">


</script>
<style>

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
            background-color:#ededed;
            border: 1px solid #dfdfdf;
            padding:30px;
            margin-left:auto;
            margin-right:auto;
/*            margin-top:30px;
*/            overflow:auto;
              border-top:none;
              padding-top: 0px;
        }

.dateTimeFilter {
     width:90%;
            background-color:#ededed;
            border: 1px solid #dfdfdf;
            padding:30px;
            margin-left:auto;
            margin-right:auto;
/*            margin-top:30px;
*/            overflow:auto;
              border-bottom:none;
}

#header {
    background: #c5c5c5 0 -2099px repeat-x url('WebResource.axd?d=dIbqQ_qfp2B5-I4gZSJx5EcMWymuEAHpyBByJM13E5SbD6GtFj8SL0L6v7wwfbnHWvNat8X0K-ZNY7RA4a2gU06Ka9RMQL2xx67uKm3iEG5m4qd_DA5ITF3OfMkj3sfqyOe42rf36BXzQbgPPlgDsQ2&t=637153648200000000')
}

.rgExpXLS
 {
   float:left;
 margin-right:1500px !important;
}
.RadGrid_Default .rgCommandRow
 {
  color: Transparent !important;
 }

.rgExpCSV
 {
   float:left;
   margin-left: -1490px !important;
 }





</style>
</telerik:radscriptblock>
<telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>

     <div style="margin-left:75px;"><h3>Reporting Dashboard</h3></div>  
<div class="dateTimeFilter" runat="server" style="border-bottom:none;">
    
        <table id="header" style="border: 1px solid #828282; border-bottom:none; width: 100%;">
            <tr>
                <td>
                     <div runat="server" style=" height:33px;border-bottom:none">
                 <telerik:RadButton ID="RadButton2" runat="server" Text="Order Item History" style="height: 25px;background:padding: 0;border: none;background: none;font-size: 16px;line-height: 24px;color: #000;" RenderMode="Lightweight"></telerik:RadButton>
               </div>
                </td>
            </tr>
        </table>
        <table runat="server" style="border:1px solid #828282; background-color:white;border-top:none;width:100%;font-size:14px;padding-left:40px;">
     
       <tr>
            <td style="width: 6%;">
               <telerik:RadButton ID="RadButton1" runat="server" Text="Start Date" style="background:padding: 0;border: none;background: none;font-size: 12px;line-height: 21px;color: #000;" RenderMode="Lightweight"></telerik:RadButton>
           </td>
           <%-- <td style="width:6%;">
                Start Dates:
           </td>--%>
           <td style="width:20%" >
            <telerik:RadDatePicker ID="tbStartDate" runat="server" AutoPostBackControl="Calendar" CausesValidation="false"  Width="180px">
                <Calendar runat="server" EnableWeekends="False" FastNavigationNextText="&amp;lt;&amp;lt;" ShowRowHeaders="false" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
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
            </telerik:RadDatePicker>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"  ControlToValidate="tbStartDate" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator>
            </td>
   
       </tr>
            <tr>
                  <td style="width: 6%;">
               <telerik:RadButton ID="RadButton4" runat="server" Text="End Date" style="background:padding: 0;border: none;background: none;font-size: 12px;line-height: 21px;color: #000;" RenderMode="Lightweight"></telerik:RadButton>
           </td>
                 
          <%--<td style="width:6%;">
                End Dates:
           </td>--%>
                 <td>
            <telerik:RadDatePicker ID="tbPickupDate" runat="server" AutoPostBackControl="Calendar" CausesValidation="false"  Width="180px">
                <Calendar runat="server" EnableWeekends="False" FastNavigationNextText="&amp;lt;&amp;lt;" ShowRowHeaders="false" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
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
                <DatePopupButton runat="server" HoverImageUrl="" ImageUrl="" />
            </telerik:RadDatePicker>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"  ControlToValidate="tbPickupDate" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator>
                                    </td>
            </tr>
           
            <tr>
                  <td style="width: 6%;">
               <telerik:RadButton ID="RadButton3" runat="server" Text="Department:" style="background:padding: 0;border: none;background: none;font-size: 12px;line-height: 21px;color: #000;" RenderMode="Lightweight"></telerik:RadButton>
           </td>
               <%--  <td style="width:6%;">
               Department:
            </td>--%>
                    <td><telerik:RadComboBox ID="ddldepartment" runat="server" AllowCustomText="False" AutoPostBack="true">
                    </telerik:RadComboBox>
                        </td>
            </tr>
            <tr>
                 <td style="width: 6%;">
               <telerik:RadButton ID="RadButton5" runat="server" Text="Users:" style="background:padding: 0;border: none;background: none;font-size: 12px;line-height: 21px;color: #000;" RenderMode="Lightweight"></telerik:RadButton>
           </td>
              <%--  <td style="width:6%;">
               User:
            </td>--%>
                  <td style="width:10%;"
                      ><telerik:RadComboBox ID="ddlUser" runat="server" AllowCustomText="False" AutoPostBack="true">
                       
                    </telerik:RadComboBox>
                        </td>
                <td>
                     <div>
                         <telerik:RadButton ID="runReport" runat="server" Text="Run Report" style="height: 25px;width:100px;margin-left:32px;font-size: 12px;color: #000;" ValidationGroup="ValidateFields" RenderMode="Lightweight"></telerik:RadButton>
                     </div>
           </td>
            </tr>
                 
            
                
    </table>
      <table>
                   <tr><td style="color:#333333 !important;width:500px;font-size:12px;line-height:16px;"><div runat="server"  id="messageDiv" class="message"><asp:Literal id="message" runat="server" /></div></td></tr>
        </table>
    </div>

<div class="orders">
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server"  Width="100%" RestoreOriginalRenderDelegate="false"> 
    <telerik:RadProgressManager ID="RadProgressManager1" runat="server" />
    <telerik:RadProgressArea ID="RadProgressArea1" runat="server" Visible="false" />

    <telerik:RadGrid ID="RadGrid1" runat="server" 
            AutoGenerateColumns="False" GridLines="None" EnableViewState="true" 
            ShowStatusBar="True" ShowGroupPanel="True" AllowSorting="True"
             Width="100%">
                <ExportSettings IgnorePaging="true" OpenInNewWindow="true" FileName='Reporting'>
                 <Pdf PageHeight="297mm" PageWidth="250mm" PageTitle='Asset Manager Inventory' />
                        
                </ExportSettings>

                <MasterTableView AllowSorting="True" ClientDataKeyNames="OrderID" DataKeyNames="OrderID" CommandItemDisplay="Top">

                    <%--<GroupByExpressions>
                    <telerik:GridGroupByExpression>
                        <SelectFields>
                            <telerik:GridGroupByField  FieldAlias="Location" FieldName="Location" />
                        </SelectFields>
                        <GroupByFields> 
                            <telerik:GridGroupByField  FieldAlias="Location" FieldName="Location"  SortOrder="Descending"></telerik:GridGroupByField>
                        </GroupByFields>
                    </telerik:GridGroupByExpression>

                </GroupByExpressions>--%>

                
<CommandItemSettings ShowExportToPdfButton="false" ShowExportToCsvButton="true"  ShowExportToExcelButton="true" ShowExportToWordButton="false" ></CommandItemSettings>

    <Columns>
 <telerik:GridBoundColumn  HeaderText="Order ID" UniqueName="OrderID" DataField="OrderID" > <HeaderStyle Width="100px" />  <HeaderStyle Width="75px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="User ID" UniqueName="UserID" DataField="UserID" > <HeaderStyle Width="75px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="User Email Address" UniqueName="UserEmailAddress" DataField="UserEmailAddress" > <HeaderStyle Width="175px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="User Full Name" UniqueName="UserFullName" DataField="UserFullName" > <HeaderStyle Width="150px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="User Department" UniqueName="UserDepartment" DataField="UserDepartment" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
       <telerik:GridBoundColumn HeaderText="Contact Name" UniqueName="ContactName" DataField="ContactName" > <HeaderStyle Width="150px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Contact Email" UniqueName="ContactEmail" DataField="ContactEmail" > <HeaderStyle Width="175px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Contact Phone" UniqueName="ContactPhone" DataField="ContactPhone" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
         <telerik:GridBoundColumn HeaderText="Event Name" UniqueName="EventName" DataField="EventName" > <HeaderStyle Width="200px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Event Start Date" UniqueName="EventStartDate" DataField="EventStartDate" > <HeaderStyle Width="200px" />  </telerik:GridBoundColumn>

         <telerik:GridBoundColumn HeaderText="Event End Date" UniqueName="EventEndDate" DataField="EventEndDate" > <HeaderStyle Width="200px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Asset Arrival Date" UniqueName="AssetArrivalDate" DataField="AssetArrivalDate" > <HeaderStyle Width="200px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Asset Pickup Date" UniqueName="AssetPickupDate" DataField="AssetPickupDate" > <HeaderStyle Width="200px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Event Venue" UniqueName="EventVenue" DataField="EventVenue" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
        
        <telerik:GridBoundColumn HeaderText="Booth Size" UniqueName="BoothSize" DataField="BoothSize" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
        
        <telerik:GridBoundColumn HeaderText="Electrical Requirements" UniqueName="ElectricalRequirements" DataField="ElectricalRequirements" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
                       <telerik:GridBoundColumn HeaderText="Order Date" UniqueName="DateOrder" DataField="DateOrder" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>

               <telerik:GridBoundColumn HeaderText="Order Status ID" UniqueName="OrderStatusID" DataField="OrderStatusID" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
         <telerik:GridBoundColumn HeaderText="Order Status Description" UniqueName="OrderStatusDescription" DataField="OrderStatusDescription" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Shipping Location" UniqueName="ShippingLocation" DataField="ShippingLocation" > <HeaderStyle Width="200px" />  </telerik:GridBoundColumn>

         <telerik:GridBoundColumn HeaderText="Shipping Contact Name" UniqueName="ShippingContactName" DataField="ShippingContactName" > <HeaderStyle Width="150px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Shipping Contact Phone" UniqueName="ShippingContactPhone" DataField="ShippingContactPhone" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Shipping Contact Email" UniqueName="ShippingContactEmail" DataField="ShippingContactEmail" > <HeaderStyle Width="200px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Shipping Address 1" UniqueName="ShippingAddress1" DataField="ShippingAddress1" > <HeaderStyle Width="200px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Shipping Address 2" UniqueName="ShippingAddress2" DataField="ShippingAddress2" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
       <telerik:GridBoundColumn HeaderText="Shipping City" UniqueName="ShippingCity" DataField="ShippingCity" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Shipping State" UniqueName="ShippingState" DataField="ShippingState" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Shipping Zip Code" UniqueName="ShippingZipCode" DataField="ShippingZipCode" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
         <telerik:GridBoundColumn HeaderText="Shipping Country" UniqueName="ShippingCountry" DataField="ShippingCountry" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>

         <telerik:GridBoundColumn HeaderText="Material Costs" UniqueName="MaterialCosts" DataField="MaterialCosts" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Labor Costs" UniqueName="LaborCosts" DataField="LaborCosts" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Order Checkout" UniqueName="OrderCheckout" DataField="OrderCheckout" > <HeaderStyle Width="200px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Order Checkin" UniqueName="OrderCheckin" DataField="OrderCheckin" > <HeaderStyle Width="200px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Shipping Tracking Number" UniqueName="ShippingTrackingNumber" DataField="ShippingTrackingNumber" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
       <telerik:GridBoundColumn HeaderText="Shipping Method" UniqueName="ShippingMethod" DataField="ShippingMethod" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Item Category" UniqueName="ItemCategory" DataField="ItemCategory" > <HeaderStyle Width="150px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Item Description" UniqueName="ItemDescription" DataField="ItemDescription" > <HeaderStyle Width="500px" />  </telerik:GridBoundColumn>
         <telerik:GridBoundColumn HeaderText="Consumable" UniqueName="Consumable" DataField="Consumable" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Inventory ID" UniqueName="InventoryID" DataField="InventoryID" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
     
            <telerik:GridBoundColumn HeaderText="Warehouse Part Number" UniqueName="WarehousePartNumber" DataField="WarehousePartNumber" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
         <telerik:GridBoundColumn HeaderText="Item Weight" UniqueName="Weight" DataField="Weight" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
               <telerik:GridBoundColumn HeaderText="Order Item Quantity" UniqueName="OrderItemQuantity" DataField="OrderItemQuantity" > <HeaderStyle Width="100px" />  </telerik:GridBoundColumn>
        
    </Columns>



</MasterTableView >
                    <ClientSettings AllowColumnsReorder="True" AllowDragToGroup="True" ReorderColumnsOnClient="True">
                        <Selecting AllowRowSelect="False" />
                        <Resizing AllowColumnResize="True" AllowRowResize="True" EnableRealTimeResize="True"
                            ResizeGridOnColumnResize="False" />
                                                    <Scrolling AllowScroll="True"   SaveScrollPosition="true" ScrollHeight="600px"  ></Scrolling> 
                    </ClientSettings>
                    <GroupingSettings ShowUnGroupButton="true" />
                </telerik:RadGrid>
            </telerik:RadAjaxPanel>
              <script type="text/javascript">
                 function onRequestStart(sender, args) {
                     if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                    args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                    args.get_eventTarget().indexOf("ExportToPdfButton") >= 0 ||
                    args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                         args.set_enableAjax(false);
                     }
                 }
              </script>

    <telerik:RadAjaxManager runat="server" >
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" 
                        LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
</telerik:RadAjaxLoadingPanel>

    </div>
<br />
<br />
<br />
</asp:Content>


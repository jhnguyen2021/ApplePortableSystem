<%@ Page Title="Inventory" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Inventory.aspx.vb" Inherits="Inventory" ValidateRequest="false"%>
<%@ Register assembly="Telerik.Web.UI" Namespace ="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <script type="text/javascript" src="Scripts/jquery-1.7.2.min.js"></script>
<script src="Scripts/jquery.fancybox.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="Scripts/jquery.fancybox.css" />
<telerik:radscriptblock id="RadScriptBlock1" runat="server">
<script type="text/javascript">
    //On insert and update buttons click temporarily disables ajax to perform upload actions
    

    function validateRadUpload(source, e) {
        e.IsValid = false;

        var upload = $find(source.parentNode.getElementsByTagName('div')[0].id);
        var inputs = upload.getFileInputs();
        for (var i = 0; i < inputs.length; i++) {
            //check for empty string or invalid extension
            var a = inputs[i].value;
            if (inputs[i].value != "") { // && upload.isExtensionValid(inputs[i].value)) {
                e.IsValid = true;
                break;
            }
        }
    }

    $(document).ready(function () {
        /*
         *  Simple image gallery. Uses default settings
         */

        $(".fancybox").click(function () {
            $.fancybox.open(this.src);
        });

        $('.imgcontainer').find('img').each(function () {
            var imgClass = (this.width / this.height > 1) ? 'wide' : 'tall';
            $(this).addClass(imgClass);
        })
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
    });


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


</style>
</telerik:radscriptblock>
<telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>


<div class="dateTimeFilter" runat="server" style="border-bottom:none;">
    
        <table  id="header" style="border: 1px solid #828282; border-bottom:none; width: 100%;">
            <tr>
                <td>
                     <div runat="server" style=" height:33px;border-bottom:none">
                 <telerik:RadButton ID="RadButton2" runat="server" Text="Check inventory availablity for your upcoming event." style="height: 25px;background:padding: 0;border: none;background: none;font-size: 14px;line-height: 24px;color: #000;" RenderMode="Lightweight"></telerik:RadButton>
               </div>
                </td>
            </tr>
        </table>
        <table runat="server" style="border:1px solid #828282; background-color:white;border-top:none;width:100%;">
       <%-- <tr>
           <td >
               <div runat="server" style="width: 101.5%; height:33px;background-color: #ededed;">
                 <telerik:RadButton ID="RadButton2" runat="server" Text="Check inventory availablity for your upcoming event." style="height: 25px;background:padding: 0;border: none;background: none;font-size: 12px;line-height: 21px;color: #000;" RenderMode="Lightweight"></telerik:RadButton>
               </div>
           </td> 
        </tr>--%>
       <tr>
            <td style="width: 10%">
               <telerik:RadButton ID="RadButton1" runat="server" Text="Asset Arrival Date:" style="height: 45px;background:padding: 0;border: none;background: none;font-size: 12px;line-height: 21px;color: #000;" RenderMode="Lightweight"></telerik:RadButton>
           </td>
           <td >
               <span runat="server" style="margin-top:5px;">
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
                   </span>
            </td>
          
       </tr>
            <tr>
                <td style="width: 10%">
               <telerik:RadButton ID="RadButton4" runat="server" Text="Event End Date:" style="height: 45px;background:padding: 0;border: none;background: none;font-size: 12px;line-height: 21px;color: #000;" RenderMode="Lightweight"></telerik:RadButton>
           </td>
                <td >
               <span runat="server" style="margin-top:5px;">
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
                   </span>
                                    </td>
            </tr>


            <tr>
                 <td style="width: 10%">
               <telerik:RadButton ID="RadButton5" Visibility="False" runat="server" Text="" style="height: 45px;background:padding: 0;border: none;background: none;font-size: 12px;line-height: 21px;color: #000;" RenderMode="Lightweight"></telerik:RadButton>
           </td>
                <td>
              <telerik:RadButton ID="RadButton3" runat="server" Text="Check Availability" style="height: 25px;width:130px;font-size: 12px;color: #000;" ValidationGroup="ValidateFields" RenderMode="Lightweight"></telerik:RadButton>
           </td>
            </tr>
    </table>
       
        <table>
                   <tr><td style="color:#333333 !important;width:500px;font-size:12px;line-height:16px;"><div runat="server"  id="messageDiv" class="message"><asp:Literal id="message" runat="server" /></div></td></tr>
        </table>
    <table runat="server" id="bufferDatesTbl" style="width:500px;font-family: 'Segoe UI',Arial,Helvetica,sans-serif;font-size:12px;border-spacing: 0;border: 1px solid #dfdfdf;">
        <tr >
            <td style="text-align: center;background-color:#C0C0C0;"><b>Item Location</b></td>
      <td style="text-align: center;background-color:#C0C0C0;"><b>Available From</b></td>
      <td style="text-align: center;background-color:#C0C0C0;"><b>Available Until</b></td>
        </tr>
       <tr>
      <td >USA</td>
      <td style="text-align: center;"><asp:Literal id="usaStart" runat="server" /></td>
      <td style="text-align: center;"><asp:Literal id="usaEnd" runat="server" /></td>
    </tr>
    <tr>
      <td >Europe</td>
      <td style="text-align: center;"><asp:Literal id="EuStart" runat="server" /></td>
     <td style="text-align: center;"><asp:Literal id="EuEnd" runat="server" /></td>
    </tr>
    <tr>
      <td >Japan</td>
      <td style="text-align: center;"><asp:Literal id="JapanStart" runat="server" /></td>
      <td style="text-align: center;"><asp:Literal id="JapanEnd" runat="server" /></td>
    </tr>
         <tr>
      <td >China</td>
      <td style="text-align: center;"><asp:Literal id="ChinaStart" runat="server" /></td>
      <td style="text-align: center;"><asp:Literal id="ChinaEnd" runat="server" /></td>
    </tr>
         <tr>
      <td >Australia</td>
      <td style="text-align: center;"><asp:Literal id="AuStart" runat="server" /></td>
      <td style="text-align: center;"><asp:Literal id="AuEnd" runat="server" /></td>
    </tr>
    </table>

    </div>

<div class="orders" style="table-layout: auto; overflow: auto;">
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server"  Width="100%" RestoreOriginalRenderDelegate="false"> 
    <telerik:RadProgressManager ID="RadProgressManager1" runat="server" />
    <telerik:RadProgressArea ID="RadProgressArea1" runat="server" Visible="false" />

    <telerik:RadGrid ID="RadGrid1" runat="server" 
            AutoGenerateColumns="False" GridLines="None" EnableViewState="true" 
            ShowStatusBar="True" ShowGroupPanel="True" AllowSorting="True"
            OnItemDataBound="RadGrid1_ItemDataBound" 
            OnItemCreated="RadGrid1_ItemCreated" OnInsertCommand="RadGrid1_InsertCommand" 
            OnUpdateCommand="RadGrid1_UpdateCommand" Width="100%"  >
                <ExportSettings IgnorePaging="true" OpenInNewWindow="true" FileName='Inventory'>
                 <Pdf PageHeight="297mm" PageWidth="250mm" PageTitle='Asset Manager Inventory' />
                </ExportSettings>

                <MasterTableView TableLayout="Auto" AllowSorting="True" ClientDataKeyNames="pkInventoryID" DataKeyNames="pkInventoryID, fkInventoryTypeID" CommandItemDisplay="Top">

                    <GroupByExpressions>
                    <telerik:GridGroupByExpression>
                        <SelectFields>
                            <telerik:GridGroupByField  FieldAlias="Location" FieldName="Location" />
                        </SelectFields>
                        <GroupByFields> 
                            <telerik:GridGroupByField  FieldAlias="Location" FieldName="Location"  SortOrder="Descending"></telerik:GridGroupByField>
                        </GroupByFields>
                    </telerik:GridGroupByExpression>
                </GroupByExpressions>

                
<CommandItemSettings ShowExportToPdfButton="false" ShowExportToCsvButton="true" ShowExportToExcelButton="false" ShowExportToWordButton="false" ></CommandItemSettings>

    <Columns>
        <telerik:GridEditCommandColumn ButtonType="ImageButton"><HeaderStyle Width="3%" /></telerik:GridEditCommandColumn>   
        <telerik:GridTemplateColumn HeaderText="Image" UniqueName="ImagePath" DataField="Picture" HeaderStyle-Width="90px">
         <ItemTemplate>
             <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
             <div class="imgcontainer"><img src='https://assets.yourpinnacle.net/CL1001/IMG/<%# (Eval("Picture"))%>' height='80' alt='<%# Eval("Picture")%>' class="fancybox"/><!--<asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Picture") %>' class="fancybox"  />--></div>
        </telerik:RadCodeBlock></ItemTemplate>
        <edititemtemplate>
             <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="RadUpload1"  >
                            </telerik:RadAsyncUpload>
        </edititemtemplate>
             <HeaderStyle  HorizontalAlign="Center"  />
        </telerik:GridTemplateColumn>

        <telerik:GridTemplateColumn UniqueName="Name" HeaderText="Type" DataField="Type" GroupByExpression="Type Group By Type" >
            <ItemTemplate>
                <%# Eval("Type")%>           
            </ItemTemplate>
            <edititemtemplate>
     <telerik:RadComboBox id="ccInventory" runat="server" Width="177px" AllowCustomText="true" MarkFirstMatch="true" />
        </edititemtemplate>
        <HeaderStyle  HorizontalAlign="Center" />
        </telerik:GridTemplateColumn>

      <telerik:GridBoundColumn HeaderText="Description" UniqueName="Description" AllowSorting="true" SortExpression="Description" DataField="Description" FilterControlWidth="300px" HeaderStyle-Width="300px"></telerik:GridBoundColumn>


        <telerik:GridTemplateColumn UniqueName="Department" HeaderText="Department" AllowSorting="true" SortExpression="deptdesc" DataField="deptdesc2" >
            <ItemTemplate>
                <%# Eval("deptdesc2")%>           
            </ItemTemplate> 
           
            <EditItemTemplate>
                <telerik:RadComboBox RenderMode="Lightweight" ID="RadComboBox1" Width="177px" runat="server" CheckBoxes="true"  EnableCheckAllItemsCheckBox="true">
                    <Items>
                    </Items>
                </telerik:RadComboBox>
            </EditItemTemplate>
        <HeaderStyle  />
        </telerik:GridTemplateColumn>

       <%-- <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Date Added" Datafield="Datecreated" FilterControlWidth="80px" HeaderStyle-Width="80px" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DateAdded" ReadOnly="true"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Date Modified" Datafield="DateModified" FilterControlWidth="80px" HeaderStyle-Width="80px" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DateModified" ReadOnly="true"></telerik:GridBoundColumn>--%>
        
        
<telerik:GridBoundColumn HeaderText="Part#" UniqueName="PartNum" DataField="PartNum" >
            </telerik:GridBoundColumn>
         <telerik:GridTemplateColumn UniqueName="Location" HeaderText="Item Location" DataField="Location" >
            <ItemTemplate>
                <%# Eval("Location")%>           
            </ItemTemplate>
            <edititemtemplate>
     <telerik:RadComboBox id="ccLocation" runat="server"  AllowCustomText="true" MarkFirstMatch="true" />
        </edititemtemplate>
        <HeaderStyle HorizontalAlign="Center" />
                          <ItemStyle HorizontalAlign="Center" />
        </telerik:GridTemplateColumn>

     

         <telerik:GridTemplateColumn UniqueName="Total" HeaderText="Total Inventory Amount" >
            <ItemTemplate>
  <%# If(Eval("TotalInventory") <> 0, Eval("TotalInventory"), "Out of Stock") %> 
            </ItemTemplate>
             <ItemStyle HorizontalAlign="Center" />
             <HeaderStyle HorizontalAlign="Center" />
        </telerik:GridTemplateColumn>
        
        <telerik:GridTemplateColumn UniqueName="Available" HeaderText="On Hand Inventory" >
            <ItemTemplate>
 <%-- <%# If(Eval("Available") > 0, Eval("Available"), "Out of Stock") %> --%>
            </ItemTemplate>
                         <ItemStyle HorizontalAlign="Center" />
            <HeaderStyle HorizontalAlign="Center" />
        </telerik:GridTemplateColumn>


         <telerik:GridTemplateColumn UniqueName="QuantityAvailableByDate" HeaderText="Available Inventory" >
            <ItemTemplate>
  <%# If(Eval("QuantityAvailableByDate") > 0, Eval("QuantityAvailableByDate"), "Out of Stock") %> 
            </ItemTemplate>
                         <ItemStyle HorizontalAlign="Center" />
            <HeaderStyle HorizontalAlign="Center" />
        </telerik:GridTemplateColumn>
       
        
<%--        <telerik:GridNumericColumn HeaderText="Total Inventory Amount" UniqueName="Total" DataField="Total" FilterControlWidth="40px" HeaderStyle-Width="40px" ReadOnly="true"></telerik:GridNumericColumn>--%>
        <telerik:GridBoundColumn HeaderText="Notes" UniqueName="Notes"   DataField="Notes"></telerik:GridBoundColumn>
              
        <telerik:GridNumericColumn HeaderText="Weight(lbs)" UniqueName="Weight" DataField="Weight" DataType="System.Decimal" Visible="false" ></telerik:GridNumericColumn>
            <%--    <telerik:GridNumericColumn HeaderText="Length(cm)" UniqueName="Length" DataField="Length" DataType="System.Decimal" Visible="false" ></telerik:GridNumericColumn>
                <telerik:GridNumericColumn HeaderText="Width(cm)" UniqueName="Width" DataField="Width" DataType="System.Decimal" Visible="false" ></telerik:GridNumericColumn>
        <telerik:GridNumericColumn HeaderText="Height(cm)" UniqueName="Height" DataField="Height" DataType="System.Decimal" Visible="false" ></telerik:GridNumericColumn>--%>


        <telerik:GridNumericColumn HeaderText="Unit Cost" UniqueName="Cost" DataField="Cost" DataFormatString="{0:C}" NumericType="Currency" DataType="System.Decimal" Visible="false" ReadOnly="true" ></telerik:GridNumericColumn>
        <telerik:GridNumericColumn HeaderText="Threshold" UniqueName="Threshold" DataField="Threshold"  Visible="false"></telerik:GridNumericColumn>
        <telerik:GridTemplateColumn UniqueName="Events"  HeaderText="Events" >
            <ItemTemplate>
                <%# getEvents(DataBinder.Eval(Container.DataItem, "events").ToString())%>               
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridCheckBoxColumn HeaderText="Deleted" UniqueName="siDeleted" DataField="isDeleted" Visible="false" ></telerik:GridCheckBoxColumn>  
    </Columns>
</MasterTableView>
                    <ClientSettings AllowColumnsReorder="True" AllowDragToGroup="True" ReorderColumnsOnClient="True">
                        <Selecting AllowRowSelect="False" />
                       <Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
      ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />
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


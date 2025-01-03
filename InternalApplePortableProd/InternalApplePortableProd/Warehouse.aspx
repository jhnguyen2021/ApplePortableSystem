
<%@ Page Title="Warehouse" Language="VB" validateRequest="false" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Warehouse.aspx.vb" Inherits="Warehouse" %>
<%@ Register assembly="Telerik.Web.UI" Namespace ="Telerik.Web.UI" tagprefix="telerik" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<script type="text/javascript" src="Scripts/jquery-1.7.2.min.js"></script>
<telerik:radscriptblock id="RadScriptBlock1" runat="server">
<script type="text/javascript">
    //On insert and update buttons click temporarily disables ajax to perform upload actions
    

    

    $(document).ready(function () {
        
    });

    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {

        
    });


</script>

</telerik:radscriptblock>
<style>
.orders {
            width:90%;
            background-color:#ededed;
            border: 1px solid #dfdfdf;
            padding:30px;
            margin-left:auto;
            margin-right:auto;
            margin-top:30px;
        }
</style>

<telerik:radscriptmanager ID="RadScriptManager1" runat="server"></telerik:radscriptmanager>
<div runat="server" class="orders">
<telerik:radajaxpanel ID="RadAjaxPanel1" runat="server" Width="100%" RestoreOriginalRenderDelegate="false"> 
    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="buttonSmallrow" >Print 1 Up</asp:LinkButton>
    <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="False" GridLines="None" EnableViewState="true" AllowAutomaticInserts="true" ShowStatusBar="True" ShowGroupPanel="True" AllowSorting="True" Width="100%"
            AllowAutomaticUpdates="true" AllowAutomaticDeletes="true" AllowMultiRowSelection="True" EnableRowHoverStyle="true" RenderMode="Lightweight"
            OnItemCreated="RadGrid1_ItemCreated" OnInsertCommand="RadGrid1_InsertCommand" 
           OnUpdateCommand="RadGrid1_UpdateCommand">
                <ExportSettings IgnorePaging="true" OpenInNewWindow="true" FileName='Inventory'></ExportSettings>
                <MasterTableView AllowSorting="True" ClientDataKeyNames="pkItemID" DataKeyNames="pkItemID, fkInventoryID, fkItemStatusID, quantity" CommandItemDisplay="Top">
<CommandItemSettings ShowExportToPdfButton="false" ShowExportToCsvButton="true" ShowExportToExcelButton="false" ShowExportToWordButton="false" ></CommandItemSettings>

    <Columns>
        
        <telerik:GridEditCommandColumn ButtonType="ImageButton"><HeaderStyle Width="3%" /></telerik:GridEditCommandColumn>   
        <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" AllowFiltering="false" Visible="true">
                            <HeaderStyle Width="30px"></HeaderStyle>
                            <HeaderTemplate>
                             <asp:CheckBox id="headerChkbox" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True" runat="server"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox id="CheckBox1" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                     </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn UniqueName="Type" HeaderText="Type" SortExpression="Name" DataField="Name" GroupByExpression="Name Group By Name" >
            <ItemTemplate>
                <%# Eval("Name")%>           
            </ItemTemplate>
            <edititemtemplate>
     <telerik:RadComboBox id="ccInventory" runat="server" Width="500px"/>
        </edititemtemplate>
        <HeaderStyle Width="250px" />
        </telerik:GridTemplateColumn>
        <telerik:GridBoundColumn HeaderText="SKU" UniqueName="pkItemID" DataField="pkItemID" HeaderStyle-Width="50px" ReadOnly="true"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Date Added" Datafield="DateAdded" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DateAdded" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Date Modified" Datafield="DateModified" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DateModified" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
        <telerik:GridTemplateColumn UniqueName="Quantity" HeaderText="Quantity" SortExpression="Quantity" DataField="Quantity" GroupByExpression="Quantity Group By Quantity" >
            <ItemTemplate>
                <%# Eval("Quantity")%>           
            </ItemTemplate>
            <edititemtemplate>
     <telerik:RadNumericTextBox ID="tbQuantity" runat="server" width="75px" NumberFormat-DecimalDigits="0">
            <NegativeStyle Resize="None" />
            <NumberFormat DecimalDigits="0" GroupSeparator="" ZeroPattern="n" />
            <EmptyMessageStyle Resize="None" />
            <ReadOnlyStyle Resize="None" />
            <FocusedStyle Resize="None" />
            <DisabledStyle Resize="None" />
            <InvalidStyle Resize="None" />
            <HoveredStyle Resize="None" />
            <EnabledStyle Resize="None" />
            </telerik:RadNumericTextBox><asp:CheckBox ID="cbItemized" runat="server" Text="Itemized" />
        </edititemtemplate>
        <HeaderStyle Width="100px" />
        </telerik:GridTemplateColumn>

        <telerik:GridBoundColumn HeaderText="Item Location" UniqueName="Location" DataField="Location" HeaderStyle-Width="100px" ReadOnly="true"></telerik:GridBoundColumn>

        <telerik:GridBoundColumn HeaderText="Notes" UniqueName="Notes" DataField="Notes" ></telerik:GridBoundColumn>      
        <telerik:GridTemplateColumn UniqueName="Status" HeaderText="Status" SortExpression="InventoryItemStatusID" HeaderStyle-Width="90px">
            <ItemTemplate>
                <%# Eval("Status")%>               
            </ItemTemplate>
            <edititemtemplate>
     <telerik:RadComboBox id="ccStatus" runat="server" />
        </edititemtemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn UniqueName="Last" HeaderText="Last Used" HeaderStyle-Width="100px" ReadOnly="true" >
            <ItemTemplate>
                <a href='Order.aspx?OrderID=<%#Eval("fkOrderID")%>'><%# Eval("DateOut")%></a>               
            </ItemTemplate>
        </telerik:GridTemplateColumn>
    </Columns>
</MasterTableView >
                    <ClientSettings AllowColumnsReorder="True" AllowDragToGroup="True" ReorderColumnsOnClient="True">
                        <Resizing AllowColumnResize="True" AllowRowResize="True" EnableRealTimeResize="True"
                            ResizeGridOnColumnResize="False" />
                    </ClientSettings>
                    <GroupingSettings ShowUnGroupButton="true" />
                </telerik:RadGrid>
            </telerik:radajaxpanel>
             <script type="text/javascript">
                 function onRequestStart(sender, args) {
                     if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                    args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                    args.get_eventTarget().indexOf("ExportToPdfButton") >= 0 ||
                    args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                         args.set_enableAjax(false);
                     }
                 }

                 function openURL(url) {
                     var w = window.open(url, '_blank')
                 }
    </script>

    <telerik:radajaxmanager runat="server" >
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" 
                        LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:radajaxmanager>
<telerik:radajaxloadingpanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
</telerik:radajaxloadingpanel>
    <telerik:RadWindowManager ID="RadWindowManager" Width="800" Height="600" VisibleStatusbar="false"
                Behaviors="Close,Move,Resize" runat="server" ></telerik:RadWindowManager>
    </div>
<br />
<br />
<br />

</asp:Content>


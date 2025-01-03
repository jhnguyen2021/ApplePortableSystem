<%@ Page Title="Users" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Users.aspx.vb" Inherits="Users" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
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
<telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <div class="orders">
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" RestoreOriginalRenderDelegate="false"> 
<telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="False" GridLines="None" EnableViewState="false" AllowAutomaticInserts="true"
            AllowAutomaticUpdates="true" AllowAutomaticDeletes="true" ShowStatusBar="True"
            OnItemCreated="RadGrid1_ItemCreated" OnInsertCommand="RadGrid1_InsertCommand" OnUpdateCommand="RadGrid1_UpdateCommand" Width="100%" >
                <ExportSettings IgnorePaging="true" OpenInNewWindow="true" FileName="Users"></ExportSettings>

                <MasterTableView AllowSorting="True" ClientDataKeyNames="pkUserID" DataKeyNames="pkUserID, fkDepartmentID, phone" CommandItemDisplay="Top">
<CommandItemSettings ShowExportToPdfButton="false" ShowExportToCsvButton="true" ShowExportToExcelButton="false" ShowExportToWordButton="false" ></CommandItemSettings>

<RowIndicatorColumn >
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn>
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>
    <Columns>
        <telerik:GridEditCommandColumn ButtonType="ImageButton">
                        <HeaderStyle Width="3%" />
                    </telerik:GridEditCommandColumn>
        <telerik:GridBoundColumn HeaderText="First Name" UniqueName="FName" DataField="FirstName"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn HeaderText="Last Name" UniqueName="LName" DataField="LastName"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn HeaderText="Email" UniqueName="Email" DataField="Email"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn HeaderText="Password" UniqueName="Password" DataField="Password" Visible="false" ReadOnly="true" ></telerik:GridBoundColumn>
        <telerik:GridTemplateColumn HeaderText="Phone Number" UniqueName="PhoneNumber" DataField="Phone">
            <ItemTemplate>
                <%# Eval("Phone")%>               
            </ItemTemplate>
            <edititemtemplate>
     <telerik:RadNumericTextBox ID="txtPhone" runat="server" width="166px" NumberFormat-DecimalDigits="0">
            <NegativeStyle Resize="None" />
            <NumberFormat DecimalDigits="0" GroupSeparator="" ZeroPattern="n" />
            <EmptyMessageStyle Resize="None" />
            <ReadOnlyStyle Resize="None" />
            <FocusedStyle Resize="None" />
            <DisabledStyle Resize="None" />
            <InvalidStyle Resize="None" />
            <HoveredStyle Resize="None" />
            <EnabledStyle Resize="None" />
            </telerik:RadNumericTextBox>
    </edititemtemplate>
        </telerik:GridTemplateColumn>
         <telerik:GridTemplateColumn UniqueName="Department" HeaderText="Department" SortExpression="Department">
            <ItemTemplate>
                <%# Eval("Department")%>               
            </ItemTemplate>
            <edititemtemplate>
     <telerik:RadComboBox id="ccRole" runat="server" />
    </edititemtemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridCheckBoxColumn HeaderText="Disabled" UniqueName="Disabled" DataField="inactive"></telerik:GridCheckBoxColumn>
    </Columns>

</MasterTableView >

                </telerik:RadGrid>
                </telerik:RadAjaxPanel>
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" RestoreOriginalRenderDelegate="false">
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" 
                        LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager><telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
</telerik:RadAjaxLoadingPanel>
        </div>
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
    
                <br />
                <br />
                <br />
                <br />
</asp:Content>


<%@ Page Title="Reports" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Reports.aspx.vb" Inherits="Reports" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <style>
        .entry{
            display:inline-block;
            height:300px;
            width:500px;
            margin-left:50px;
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
            background-color:#ededed;
            border: 1px solid #dfdfdf;
            padding:30px;
            margin-left:auto;
            margin-right:auto;
            margin-top:30px;
        }
    </style>
    
     <script type="text/javascript" src="Scripts/jquery-1.7.2.min.js"></script>
<script src="Scripts/jquery.fancybox.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="Scripts/jquery.fancybox.css" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">    </telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" 
        DefaultLoadingPanelID="RadAjaxLoadingPanel1">
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" UpdatePanelHeight="" 
                        LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="InventoryItems" 
                        LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" 
                        LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="orders" >
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" RestoreOriginalRenderDelegate="false">

    <div class="demo-container no-bg">
            <telerik:RadSearchBox RenderMode="Lightweight" runat="server" ID="RadSearchBox1"
                CssClass="searchBox"
                Width="50" DropDownSettings-Height="300"
                DataSourceID="SqlDataSource1"
                DataTextField="pkOrderID"
                DataValueField="pkOrderID"
                DataKeyNames="pkOrderID"
                EmptyMessage="Search"
                Filter="StartsWith"
                MaxResultCount="20"
                 OnSearch="RadSearchBox1_Search">
            </telerik:RadSearchBox>
        </div>



     <telerik:RadGrid ID="GridTable" runat="server" AutoGenerateColumns="False" GridLines="None" >
         <ClientSettings  EnableRowHoverStyle="true"></ClientSettings>
                <MasterTableView  ClientDataKeyNames="pkInventoryID" DataKeyNames="pkInventoryID,fkDepartmentID">
                <Columns>   
                    <telerik:GridTemplateColumn HeaderText="Image" UniqueName="Picture" DataField="Picture">
                     <ItemTemplate>
                         <div class="imgcontainer"><img src='Account/inventory/<%# (Eval("Picture"))%>' height='80' alt='<%# (Eval("Picture"))%>' class="fancybox"/></div>
                    </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn HeaderText="Name" UniqueName="Name" DataField="UDF01"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Description" UniqueName="Description" DataField="Description"></telerik:GridBoundColumn>
                   
                    <telerik:GridBoundColumn HeaderText="Part#" UniqueName="PartNum" DataField="PartNum"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Date Added" Datafield="DateCreated" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DateCreated" ReadOnly="true"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Date Modified" Datafield="DateModified" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DateModified" ReadOnly="true"></telerik:GridBoundColumn>
                    <%--<telerik:GridBoundColumn HeaderText="Total" UniqueName="PartNum" DataField="UDF03"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Available" UniqueName="PartNum" DataField="UDF02"></telerik:GridBoundColumn>--%>
                    <telerik:GridBoundColumn HeaderText="Notes" UniqueName="Notes" DataField="Notes"></telerik:GridBoundColumn>
                </Columns>
            </MasterTableView >
    </telerik:RadGrid>

    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False" GridLines="None" >
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" FileName='Report'></ExportSettings>
        <ClientSettings  EnableRowHoverStyle="true"></ClientSettings>
<MasterTableView AllowSorting="True" ClientDataKeyNames="pkOrderID" DataKeyNames="pkOrderID" CommandItemDisplay="Top"  EnableHierarchyExpandAll="true" >
    <CommandItemSettings ShowExportToPdfButton="false" ShowExportToCsvButton="true" ShowExportToExcelButton="false" ShowExportToWordButton="false" ShowAddNewRecordButton="false"></CommandItemSettings>
            <DetailTables>
                <telerik:GridTableView AutoGenerateColumns="false" DataKeyNames="pkOrderID" Width="100%">
                    <ParentTableRelation>
                        <telerik:GridRelationFields DetailKeyField="pkOrderID" MasterKeyField="pkOrderID" />
                    </ParentTableRelation>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Image" UniqueName="Picture" DataField="Picture">
                            <ItemTemplate>
                                <div class="imgcontainer"><img src='Account/inventory/<%# (Eval("Picture"))%>' height='80' alt='<%# (Eval("Picture"))%>' class="fancybox"/></div>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Type" UniqueName="Type" DataField="Type"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Description" UniqueName="Description" DataField="Description"></telerik:GridBoundColumn>
                         <telerik:GridTemplateColumn UniqueName="Fulfillment" HeaderText="Item Location" DataField="fkDepartmentID">
            <ItemTemplate>
                 <%# If(Eval("fkDepartmentID") <> 1004, "US", "Europe") %>   
            </ItemTemplate>
        </telerik:GridTemplateColumn>    
                        <telerik:GridBoundColumn HeaderText="Part#" UniqueName="PartNum" DataField="PartNum"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn UniqueName="Quantity" HeaderText="Quantity" DataField="Quantity"></telerik:GridBoundColumn>
                    </Columns>                                 
                </telerik:GridTableView>
            </DetailTables>

    <Columns>
        <telerik:GridBoundColumn HeaderText="Order #" UniqueName="pkOrderID" DataField="pkOrderID">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn HeaderText="Show Name" UniqueName="EventName" DataField="EventName" AllowFiltering="false">
        </telerik:GridBoundColumn>


        <telerik:GridTemplateColumn HeaderText="Asset Arrival Date" UniqueName="DateArrive" DataField="DateArrive"  AllowFiltering="false" SortExpression="DateArrive">
         <ItemTemplate>
             <%# DataBinder.Eval(Container.DataItem, "DateArrive", "{0:d}")%>
        </ItemTemplate>
        </telerik:GridTemplateColumn>

         <telerik:GridTemplateColumn HeaderText="Show Dates" UniqueName="ShowDates" DataField="ShowStartDate" SortExpression="DateStart" AllowFiltering="false">
         <ItemTemplate>
             <%# DataBinder.Eval(Container.DataItem, "DateStart", "{0:d}")%>-<%# DataBinder.Eval(Container.DataItem, "DateEnd", "{0:d}")%>
        </ItemTemplate>
        </telerik:GridTemplateColumn>

  <telerik:GridTemplateColumn HeaderText="Show Dates with Buffers" UniqueName="ShowStartDateBuffers" DataField="ShowStartDate"  AllowFiltering="false" SortExpression="StartDateWithBuffers">
         <ItemTemplate>
             <%# DataBinder.Eval(Container.DataItem, "StartDateWithBuffers", "{0:d}")%>-<%# DataBinder.Eval(Container.DataItem, "EndDateWithBuffers", "{0:d}")%>
        </ItemTemplate>
        </telerik:GridTemplateColumn>


        <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Order Date" Datafield="DateOrder" AllowFiltering="false"
           DataFormatString="{0:MM/dd/yyyy}" UniqueName="OrderDate">
        </telerik:GridBoundColumn>
        
        <telerik:GridBoundColumn HeaderText="Orderer" UniqueName="User" DataField="User" SortExpression="User" AllowFiltering="false"></telerik:GridBoundColumn>
                 <telerik:GridBoundColumn HeaderText="Dept" UniqueName="deptdesc" DataField="deptdesc" AllowFiltering="false" ></telerik:GridBoundColumn>
        <telerik:GridTemplateColumn HeaderText="Status" UniqueName="Status" DataField="Status" AllowFiltering="false" SortExpression="Status" >
         <ItemTemplate>
           <asp:LinkButton ID="LBEdit" runat="server" CausesValidation="False" CommandName='<%# DataBinder.Eval(Container.DataItem, "Status")%>' Text='<%# DataBinder.Eval(Container.DataItem, "Status")%>'></asp:LinkButton>
         </ItemTemplate>
        </telerik:GridTemplateColumn>
<%--        <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="ViewOrder" Text="View" UniqueName="ButtonColumn"><HeaderStyle Width="50px" /></telerik:GridButtonColumn>--%>
         <telerik:GridTemplateColumn HeaderText="" UniqueName="ViewOrder" SortExpression="View" AllowFiltering="false">
         <ItemTemplate>
           <asp:LinkButton ID="ViewOrder" Text="View" runat="server" CausesValidation="False" CommandName="ViewOrder"
              ></asp:LinkButton>
         </ItemTemplate>
        </telerik:GridTemplateColumn>

        <telerik:GridTemplateColumn HeaderText="" UniqueName="EditOrder" SortExpression="Edit" AllowFiltering="false" >
         <ItemTemplate>
           <asp:LinkButton ID="EditOrder" Text="edit" runat="server" CausesValidation="False" CommandName="EditOrder" ToolTip="Orders within ten days of shipment cannot be edited."  
               Enabled='<%# If(Eval("DateDiff") > 10, "True", "False") %>'></asp:LinkButton>
         </ItemTemplate>
        </telerik:GridTemplateColumn>


          <telerik:GridTemplateColumn HeaderText="" UniqueName="CancelOrder" SortExpression="Cancel" AllowFiltering="false" >
         <ItemTemplate>
           <asp:LinkButton ID="CancelOrder" Text="Cancel" runat="server" CausesValidation="False" CommandName="Cancel" ToolTip="Orders within seven days of shipment cannot be cancelled."  OnClientClick="return confirm('Are you sure you want to cancel this?')"
               Enabled='<%# If(Eval("DateDiff") > 7, "True", "False") %>'></asp:LinkButton>
         </ItemTemplate>
        </telerik:GridTemplateColumn>


        <%--<telerik:GridTemplateColumn HeaderText="" UniqueName="Delete" SortExpression="Delete" AllowFiltering="false" >
         <ItemTemplate>
           <asp:LinkButton ID="DeleteOrder" Text="Delete" runat="server" CausesValidation="False" CommandName="Delete"  OnClientClick="return confirm('Are you sure you want to delete this?')"
              ></asp:LinkButton>
         </ItemTemplate>
        </telerik:GridTemplateColumn>--%>
<%--        <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="EditOrder" Text="Edit" UniqueName="ButtonColumn"><HeaderStyle Width="50px" /></telerik:GridButtonColumn>--%>
<%--        <telerik:GridButtonColumn CommandName="Delete" Text="Cancel" ConfirmText="Are you sure you want to cancel this?" ></telerik:GridButtonColumn>--%>
    </Columns>
</MasterTableView >
    </telerik:RadGrid>
   
        
   
   
    <script type="text/javascript">
        $(document).ready(function () {
            $(".fancybox").click(function () {
                $.fancybox.open(this.src);
            });

            $('.imgcontainer').find('img').each(function () {
                var imgClass = (this.width / this.height > 1) ? 'wide' : 'tall';
                $(this).addClass(imgClass);
            })
            $("#ctl00_MainContent_RadGrid1_ctl00_ctl13_DeleteOrder").click(function () {
                alert("Handler for .click() called.");
            });

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

        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure you want to cancel this?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
                location.reload();
            }
        }

        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                    args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                    args.get_eventTarget().indexOf("ExportToPdfButton") >= 0 ||
                    args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                args.set_enableAjax(false);
            }
        }
    </script>

    </telerik:RadAjaxPanel>


        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        ConnectionString="<%$ ConnectionStrings:Portables %>"
        SelectCommand="Select * from  tblOrders"></asp:SqlDataSource>
</div>
     <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
</telerik:RadAjaxLoadingPanel>

</asp:Content>


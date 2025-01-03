<%@ Page Title="Inventory" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Retail.aspx.vb" Inherits="Exhibits" ValidateRequest="false"%>
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
            margin-top:30px;
        }
</style>
</telerik:radscriptblock>
<telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
<div class="orders">
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server"  Width="100%" RestoreOriginalRenderDelegate="false"> 
    <telerik:RadProgressManager ID="RadProgressManager1" runat="server" />
    <telerik:RadProgressArea ID="RadProgressArea1" runat="server" Visible="false" />

    <telerik:RadGrid ID="RadGrid1" runat="server" 
            AutoGenerateColumns="False" GridLines="None" EnableViewState="true" 
            ShowStatusBar="True" ShowGroupPanel="True" AllowSorting="True"
            OnItemDataBound="RadGrid1_ItemDataBound" 
            OnItemCreated="RadGrid1_ItemCreated" OnInsertCommand="RadGrid1_InsertCommand" 
            OnUpdateCommand="RadGrid1_UpdateCommand" Width="100%">
                <ExportSettings IgnorePaging="true" OpenInNewWindow="true" FileName='Inventory'>
                 <Pdf PageHeight="297mm" PageWidth="250mm" PageTitle='Asset Manager Inventory' />
                </ExportSettings>

                <MasterTableView AllowSorting="True" ClientDataKeyNames="pkInventoryID" DataKeyNames="pkInventoryID, fkInventoryTypeID" CommandItemDisplay="Top">
                
<CommandItemSettings ShowExportToPdfButton="false" ShowExportToCsvButton="true" ShowExportToExcelButton="false" ShowExportToWordButton="false" ></CommandItemSettings>

    <Columns>
        <telerik:GridEditCommandColumn ButtonType="ImageButton"><HeaderStyle Width="3%" /></telerik:GridEditCommandColumn>   
        <telerik:GridTemplateColumn HeaderText="Image" UniqueName="ImagePath" DataField="Picture" HeaderStyle-Width="90px">
         <ItemTemplate>
             <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
             <div class="imgcontainer"><img src='<%= HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/")%>inventory/<%# (Eval("Picture"))%>' height='80' alt='<%# Eval("Picture")%>' class="fancybox"/><!--<asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Picture") %>' class="fancybox"  />--></div>
        </telerik:RadCodeBlock></ItemTemplate>
        <edititemtemplate>
             <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="RadUpload1"  >
                            </telerik:RadAsyncUpload>
        </edititemtemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn UniqueName="Name" HeaderText="Name" SortExpression="Type" DataField="Type" GroupByExpression="Type Group By Type" >
            <ItemTemplate>
                <%# Eval("Type")%>           
            </ItemTemplate>
            <edititemtemplate>
     <telerik:RadComboBox id="ccInventory" runat="server" Width="300px" AllowCustomText="true" MarkFirstMatch="true" />
        </edititemtemplate>
        <HeaderStyle Width="250px" />
        </telerik:GridTemplateColumn>
        
        <telerik:GridBoundColumn HeaderText="Description" UniqueName="Description" DataField="Description" FilterControlWidth="500px" HeaderStyle-Width="500px"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn HeaderText="Part#" UniqueName="PartNum" DataField="PartNum"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Date Added" Datafield="Datecreated" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DateAdded" ReadOnly="true"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Date Modified" Datafield="DateModified" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DateModified" ReadOnly="true"></telerik:GridBoundColumn>
        <telerik:GridNumericColumn HeaderText="Available" UniqueName="Available" DataField="Available" ReadOnly="true"></telerik:GridNumericColumn>
        <telerik:GridNumericColumn HeaderText="Total" UniqueName="Total" DataField="Total" ReadOnly="true"></telerik:GridNumericColumn>
        <telerik:GridBoundColumn HeaderText="Notes" UniqueName="Notes" DataField="Notes"></telerik:GridBoundColumn>
              
        <telerik:GridNumericColumn HeaderText="Weight(lbs)" UniqueName="Weight" DataField="Weight" DataType="System.Decimal" Visible="false" ></telerik:GridNumericColumn>
        <telerik:GridNumericColumn HeaderText="Unit Cost" UniqueName="Cost" DataField="Cost" DataFormatString="{0:C}" NumericType="Currency" DataType="System.Decimal" Visible="false" ReadOnly="true" ></telerik:GridNumericColumn>
        <telerik:GridNumericColumn HeaderText="Threshold" UniqueName="Threshold" DataField="Threshold"  Visible="false"></telerik:GridNumericColumn>
        <telerik:GridTemplateColumn UniqueName="Events" HeaderText="Events" >
            <ItemTemplate>
                <%# getEvents(DataBinder.Eval(Container.DataItem, "events").ToString())%>               
            </ItemTemplate>
            
        </telerik:GridTemplateColumn>
        <telerik:GridCheckBoxColumn HeaderText="Deleted" UniqueName="siDeleted" DataField="isDeleted" Visible="false" ></telerik:GridCheckBoxColumn>  
    </Columns>
</MasterTableView >
                    <ClientSettings AllowColumnsReorder="True" AllowDragToGroup="True" ReorderColumnsOnClient="True">
                        <Selecting AllowRowSelect="False" />
                        <Resizing AllowColumnResize="True" AllowRowResize="True" EnableRealTimeResize="True"
                            ResizeGridOnColumnResize="False" />
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


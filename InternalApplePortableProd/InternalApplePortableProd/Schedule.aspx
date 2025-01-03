<%@ Page Title="Schedule" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Schedule.aspx.vb" Inherits="Schedule" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
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
    h3 {
        margin-left:20px;
    }
</style>
<script type="text/javascript">
    function OnClientAppointmentClick(sender, eventArgs) {
        var apt = eventArgs.get_appointment();
        window.location = "Order.aspx?OrderID=" + apt.get_id()
    }
</script>
<div>
        <div style="display:none"><h3>Click on orders in blue to approve or decline:</h3></div><br />
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        
        <telerik:RadAjaxLoadingPanel runat="Server" ID="RadAjaxLoadingPanel1">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="Configuratorpanel1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadScheduler1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server" RestoreOriginalRenderDelegate="false" >
          <div class="orders">
        <telerik:RadScheduler  runat="server" ID="RadScheduler1" SelectedDate="2015-01-31"
            SelectedView="MonthView" Height="800px" Width="100%" OnAppointmentDataBound="RadScheduler1_AppointmentDataBound" AllowEdit="false" AllowDelete="false" OverflowBehavior="Expand" AdvancedForm-MaximumHeight="800px" OnClientAppointmentClick="OnClientAppointmentClick">
<ExportSettings>
<Pdf PageTopMargin="1in" PageBottomMargin="1in" PageLeftMargin="1in" PageRightMargin="1in"></Pdf>
</ExportSettings>

            <AdvancedForm Modal="true" ></AdvancedForm>
            <WeekView UserSelectable="false" />
            <DayView UserSelectable="false" />
            <MultiDayView UserSelectable="false" />
            <TimelineView UserSelectable="false" />
            <MonthView UserSelectable="False" AdaptiveRowHeight="false" />
            <TimeSlotContextMenuSettings EnableDefault="false"></TimeSlotContextMenuSettings>
            <AppointmentContextMenuSettings EnableDefault="false"></AppointmentContextMenuSettings>
        </telerik:RadScheduler>
              </div>

    </telerik:RadAjaxPanel>
    </div>
</asp:Content>


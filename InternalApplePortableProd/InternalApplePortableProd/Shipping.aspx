    

<%@ Page Title="Shipping" Language="VB" validateRequest="false" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Shipping.aspx.vb" Inherits="Shipping" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<script type="text/javascript" src="Scripts/jquery-1.7.2.min.js"></script>
<telerik:radscriptblock id="RadScriptBlock1" runat="server">
<script type="text/javascript">
    //On insert and update buttons click temporarily disables ajax to perform upload actions

</script>

</telerik:radscriptblock>
<style>
 body, html {
            font-family:Arial;
        }
        .buttons {
            margin-right:10px;
        }
        .search {
            width :350px;
            margin-left:auto;
            margin-right:auto;
            margin-top: 10px;
        }
</style>
      <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel runat="server"  width="100%" RestoreOriginalRenderDelegate="false">
            <div class="search" >
                <telerik:RadTextBox ID="tbsearch" runat="server" ></telerik:RadTextBox>
                <asp:Button ID="Button1" runat="server" Text="Search" CssClass="buttons" />
                <asp:Button ID="Button2" runat="server" Text="Clear"  CssClass="buttons" />
            </div>
     <div><h2>Outbound</h2>

       <div  style="width:30%">
        <div runat="server" id="closeShipDate" style="font-family: 'Segoe UI',Arial,Helvetica,sans-serif;font-size:12px;padding:5px;line-height:16px;"><img style="height:20px;vertical-align:middle;" src="https://assets.yourpinnacle.net/CL1001/WarningTriangleYellow.png" alt="Italian Trulli"/>&nbsp;&nbsp;<span runat="server" id="spanText"></span></div>
    </div>

<div style="width:30%">
                <div runat="server" id="lateShipDate" style="font-family: 'Segoe UI',Arial,Helvetica,sans-serif;font-size:12px;padding:5px;line-height:16px;"><img style="height:20px;vertical-align:middle;" src="https://abportables.yourpinnacle.net/Img/WarningHexagonRed.png" alt="Italian Trulli"/>&nbsp;&nbsp;<span>Based on ship location, items will not arrive at location on time.</span></div>
        </div>


      <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="False" GridLines="None" EnableViewState="true" AllowAutomaticInserts="true" ShowStatusBar="false" ShowGroupPanel="false" AllowSorting="True" Width="100%"
            AllowAutomaticUpdates="true" OnItemDataBound="RadGrid1_ItemDataBound" >
        <MasterTableView AllowSorting="True" ClientDataKeyNames="pkOrderID" DataKeyNames="pkOrderID,DateArrive" CommandItemDisplay="Top">
                
<CommandItemSettings ShowExportToPdfButton="false" ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToWordButton="false" ShowAddNewRecordButton="false"></CommandItemSettings>
    <Columns>
      <telerik:GridTemplateColumn HeaderText="">
                       <ItemTemplate>
                           <asp:Image ID="Image1" runat="server" style="height:13px;"/>
                       </ItemTemplate>
                                    <HeaderStyle Width="50px" />
                                     <ItemStyle HorizontalAlign="Center"  />
                                     <HeaderStyle HorizontalAlign="Center" />
                   </telerik:GridTemplateColumn>

        <telerik:GridBoundColumn HeaderText="Order#" UniqueName="pkOrderID" DataField="pkOrderID" HeaderStyle-Width="50px" ></telerik:GridBoundColumn>
        <telerik:GridBoundColumn HeaderText="Event" UniqueName="EventName" DataField="EventName" ></telerik:GridBoundColumn> 
        <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Arrival Date" Datafield="DateArrive" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DateArrive"  HeaderStyle-Width="70px"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Pickup Date" Datafield="DatePickup" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DatePickup" HeaderStyle-Width="70px"></telerik:GridBoundColumn>      
        <telerik:GridTemplateColumn UniqueName="View" HeaderText="View" HeaderStyle-Width="70px"  >
            <ItemTemplate>
                <a href='Order.aspx?OrderID=<%#Eval("pkorderID")%> & "'>View</a>               
            </ItemTemplate>
        </telerik:GridTemplateColumn>

        <telerik:GridTemplateColumn UniqueName="Edit" HeaderText="Edit" HeaderStyle-Width="70px"  >
            <ItemTemplate>
                <a href='Place.aspx?OrderID=<%#Eval("pkorderID")%>'>Edit</a>               
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn UniqueName="Process" HeaderText="Process" HeaderStyle-Width="70px"  >
            <ItemTemplate>
                <a href='Fulfillment.aspx?OrderID=<%#Eval("pkorderID")%>&statusid=1002'>Process</a>               
            </ItemTemplate>
        </telerik:GridTemplateColumn>
    </Columns>
</MasterTableView >
                    
                </telerik:RadGrid>
    </div>

     <div><h2>Inbound</h2>
        <telerik:RadGrid ID="RadGrid2"  runat="server" AutoGenerateColumns="False" GridLines="None" EnableViewState="true" AllowAutomaticInserts="true" ShowStatusBar="false" ShowGroupPanel="false" AllowSorting="True" Width="100%"
            AllowAutomaticUpdates="true" AllowAutomaticDeletes="true" >
        <MasterTableView AllowSorting="True" ClientDataKeyNames="pkOrderID" DataKeyNames="pkOrderID" CommandItemDisplay="Top">
                
<CommandItemSettings ShowExportToPdfButton="false" ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToWordButton="false" ShowAddNewRecordButton="false"></CommandItemSettings>

    <Columns>
        <telerik:GridBoundColumn HeaderText="Order#" UniqueName="pkOrderID" DataField="pkOrderID" HeaderStyle-Width="50px" ></telerik:GridBoundColumn>
        <telerik:GridBoundColumn HeaderText="Event" UniqueName="EventName" DataField="EventName" ></telerik:GridBoundColumn> 
        <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Arrival Date" Datafield="DateArrive" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DateArrive"  HeaderStyle-Width="70px"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Pickup Date" Datafield="DatePickup" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DatePickup" HeaderStyle-Width="70px"></telerik:GridBoundColumn>      
        <telerik:GridTemplateColumn UniqueName="View" HeaderText="View" HeaderStyle-Width="70px"  >
            <ItemTemplate>
                <a href='Order.aspx?OrderID=<%#Eval("pkorderID")%> & "'>View</a>               
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn UniqueName="Process" HeaderText="Process" HeaderStyle-Width="70px"  >
            <ItemTemplate>
                <a href='Fulfillment.aspx?OrderID=<%#Eval("pkorderID")%>&statusid=1005'>Process</a>               
            </ItemTemplate>
        </telerik:GridTemplateColumn>
    </Columns>
</MasterTableView >
                    
                </telerik:RadGrid>
    </div>

        </telerik:RadAjaxPanel>

</asp:Content>

<%--<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body, html {
            font-family:Arial;
        }
        .buttons {
            margin-right:10px;
        }
        .search {
            width :350px;
            margin-left:auto;
            margin-right:auto;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel runat="server"  width="100%" RestoreOriginalRenderDelegate="false">
            <div class="search" >
                <telerik:RadTextBox ID="tbsearch" runat="server" ></telerik:RadTextBox>
                <asp:Button ID="Button1" runat="server" Text="Search" CssClass="buttons" />
                <asp:Button ID="Button2" runat="server" Text="Clear"  CssClass="buttons" />
            </div>
     <div><h2>Outbound</h2>
      <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="False" GridLines="None" EnableViewState="true" AllowAutomaticInserts="true" ShowStatusBar="false" ShowGroupPanel="false" AllowSorting="True" Width="100%"
            AllowAutomaticUpdates="true" AllowAutomaticDeletes="true" >
        <MasterTableView AllowSorting="True" ClientDataKeyNames="pkOrderID" DataKeyNames="pkOrderID" CommandItemDisplay="Top">
                
<CommandItemSettings ShowExportToPdfButton="false" ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToWordButton="false" ShowAddNewRecordButton="false"></CommandItemSettings>

    <Columns>
        <telerik:GridBoundColumn HeaderText="Order#" UniqueName="pkOrderID" DataField="pkOrderID" HeaderStyle-Width="50px" ></telerik:GridBoundColumn>
        <telerik:GridBoundColumn HeaderText="Event" UniqueName="EventName" DataField="EventName" ></telerik:GridBoundColumn> 
        <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Arrival Date" Datafield="DateArrive" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DateArrive"  HeaderStyle-Width="70px"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Pickup Date" Datafield="DatePickup" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DatePickup" HeaderStyle-Width="70px"></telerik:GridBoundColumn>      
        <telerik:GridTemplateColumn UniqueName="View" HeaderText="View" HeaderStyle-Width="70px"  >
            <ItemTemplate>
                <a href='Order.aspx?OrderID=<%#Eval("pkorderID")%> & "'>View</a>               
            </ItemTemplate>
        </telerik:GridTemplateColumn>

        <telerik:GridTemplateColumn UniqueName="Edit" HeaderText="Edit" HeaderStyle-Width="70px"  >
            <ItemTemplate>
                <a href='Place.aspx?OrderID=<%#Eval("pkorderID")%> & "'>Edit</a>               
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn UniqueName="Process" HeaderText="Process" HeaderStyle-Width="70px"  >
            <ItemTemplate>
                <a href='Fulfillment.aspx?OrderID=<%#Eval("pkorderID")%>&statusid=1002'>Process</a>               
            </ItemTemplate>
        </telerik:GridTemplateColumn>
    </Columns>
</MasterTableView >
                    
                </telerik:RadGrid>
    </div>

     <div><h2>Inbound</h2>
        <telerik:RadGrid ID="RadGrid2"  runat="server" AutoGenerateColumns="False" GridLines="None" EnableViewState="true" AllowAutomaticInserts="true" ShowStatusBar="false" ShowGroupPanel="false" AllowSorting="True" Width="100%"
            AllowAutomaticUpdates="true" AllowAutomaticDeletes="true" >
        <MasterTableView AllowSorting="True" ClientDataKeyNames="pkOrderID" DataKeyNames="pkOrderID" CommandItemDisplay="Top">
                
<CommandItemSettings ShowExportToPdfButton="false" ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToWordButton="false" ShowAddNewRecordButton="false"></CommandItemSettings>

    <Columns>
        <telerik:GridBoundColumn HeaderText="Order#" UniqueName="pkOrderID" DataField="pkOrderID" HeaderStyle-Width="50px" ></telerik:GridBoundColumn>
        <telerik:GridBoundColumn HeaderText="Event" UniqueName="EventName" DataField="EventName" ></telerik:GridBoundColumn> 
        <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Arrival Date" Datafield="DateArrive" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DateArrive"  HeaderStyle-Width="70px"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Pickup Date" Datafield="DatePickup" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DatePickup" HeaderStyle-Width="70px"></telerik:GridBoundColumn>      
        <telerik:GridTemplateColumn UniqueName="View" HeaderText="View" HeaderStyle-Width="70px"  >
            <ItemTemplate>
                <a href='Order.aspx?OrderID=<%#Eval("pkorderID")%> & "'>View</a>               
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn UniqueName="Process" HeaderText="Process" HeaderStyle-Width="70px"  >
            <ItemTemplate>
                <a href='Fulfillment.aspx?OrderID=<%#Eval("pkorderID")%>&statusid=1005'>Process</a>               
            </ItemTemplate>
        </telerik:GridTemplateColumn>
    </Columns>
</MasterTableView >
                    
                </telerik:RadGrid>
    </div>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>--%>

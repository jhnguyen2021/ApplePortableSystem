<%@ Page Language="VB" MasterPageFile="~/Site.master"  AutoEventWireup="false" CodeFile="Admin.aspx.vb" Inherits="Admin" Title="Admin" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <style>
        .Missing {
            color:red !important;
        }
    </style>
         <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <div style="width:100%"><h2>Upcoming Events:</h2><br />
            <div class="orders">
            <telerik:RadGrid ID="RadGrid1" runat="server" Width="100%" AllowFilteringByColumn="True" AllowSorting="True"  AutoGenerateColumns="false" Height="500px" >
                <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="10px"  />
                </ClientSettings>
                <MasterTableView CommandItemDisplay="None" DataKeyNames="pkOrderID" ShowFooter="true">
                 <Columns>
                     
                     <telerik:GridTemplateColumn UniqueName="pkOrderID" HeaderText="Event Name"  SortExpression="pkOrderID" FilterControlAltText="Filter pkOrderID column" DataField="pkOrderID" HeaderStyle-Width="100" FilterControlWidth="60">
                         <ItemTemplate>
                            <a href='Order.aspx?orderID=<%# DataBinder.Eval(Container, "DataItem.pkOrderID")%>' target="_blank"><%# DataBinder.Eval(Container, "DataItem.pkOrderID")%></a>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn UniqueName="eventname" HeaderText="Event Name"  SortExpression="eventname" FilterControlAltText="Filter eventname column" DataField="eventname" >
                         <ItemTemplate>
                            <a href='Place.aspx?orderID=<%# DataBinder.Eval(Container, "DataItem.pkOrderID")%>' target="_blank"><%# DataBinder.Eval(Container, "DataItem.eventname")%></a>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn UniqueName="slabor" HeaderText="Labor"  SortExpression="slabor" FilterControlAltText="Filter slabor column" DataField="slabor" HeaderStyle-Width="100" FilterControlWidth="60">
                         <ItemTemplate>
                            <a href='Services.aspx?orderID=<%# DataBinder.Eval(Container, "DataItem.pkOrderID")%>' class='<%# DataBinder.Eval(Container, "DataItem.slabor")%>'><%# DataBinder.Eval(Container, "DataItem.slabor")%></a>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn UniqueName="selectrical" HeaderText="Electrical"  SortExpression="selectrical" FilterControlAltText="Filter selectrical column" DataField="selectrical" HeaderStyle-Width="100" FilterControlWidth="60">
                         <ItemTemplate>
                            <a href='Services.aspx?orderID=<%# DataBinder.Eval(Container, "DataItem.pkOrderID")%>' class='<%# DataBinder.Eval(Container, "DataItem.selectrical")%>'><%# DataBinder.Eval(Container, "DataItem.selectrical")%></a>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn UniqueName="sLeadRetrieval" HeaderText="Lead Retrieval"  SortExpression="sLeadRetrieval" FilterControlAltText="Filter sLeadRetrieval column" DataField="sLeadRetrieval" HeaderStyle-Width="100" FilterControlWidth="60">
                         <ItemTemplate>
                            <a href='Services.aspx?orderID=<%# DataBinder.Eval(Container, "DataItem.pkOrderID")%>' class='<%# DataBinder.Eval(Container, "DataItem.sLeadRetrieval")%>'><%# DataBinder.Eval(Container, "DataItem.sLeadRetrieval")%></a>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn UniqueName="sCarpet" HeaderText="Carpet"  SortExpression="sCarpet" FilterControlAltText="Filter sCarpet column" DataField="sCarpet" HeaderStyle-Width="100" FilterControlWidth="60">
                         <ItemTemplate>
                            <a href='Services.aspx?orderID=<%# DataBinder.Eval(Container, "DataItem.pkOrderID")%>' class='<%# DataBinder.Eval(Container, "DataItem.sCarpet")%>'><%# DataBinder.Eval(Container, "DataItem.sCarpet")%></a>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn UniqueName="sInternet" HeaderText="Internet"  SortExpression="sInternet" FilterControlAltText="Filter sInternet column" DataField="sInternet" HeaderStyle-Width="100" FilterControlWidth="60">
                         <ItemTemplate>
                            <a href='Services.aspx?orderID=<%# DataBinder.Eval(Container, "DataItem.pkOrderID")%>' class='<%# DataBinder.Eval(Container, "DataItem.sInternet")%>'><%# DataBinder.Eval(Container, "DataItem.sInternet")%></a>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>
                      <telerik:GridBoundColumn SortExpression="DateEnd" DataField="eventdates" HeaderText="Event Dates" UniqueName="eventdates" HeaderStyle-Width="150" FilterControlWidth="100">
                    </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="dateorder" HeaderText="Order Date" UniqueName="orderdate" HeaderStyle-Width="150" FilterControlWidth="100">
                    </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="status" HeaderText="Status" UniqueName="status" HeaderStyle-Width="100" FilterControlWidth="60">
                    </telerik:GridBoundColumn>
                     <telerik:GridTemplateColumn UniqueName="CurrentBilled" HeaderText="Billing"  SortExpression="CurrentBilled" FilterControlAltText="Filter CurrentBilled column" DataField="CurrentBilled" HeaderStyle-Width="100" FilterControlWidth="60">
                         <ItemTemplate>
                            <%# BillingStatus(Eval("fkDepartmentID"),Eval("CurrentBilled"))  %>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
    </telerik:RadGrid></div></div>
    <br /><br />
    <div style="width:100%"><h2>Past Events:</h2><br />
        <div class="orders">
            <telerik:RadGrid ID="RadGrid2" runat="server" AllowFilteringByColumn="True" AllowSorting="True"  AutoGenerateColumns="false" Height="500px" >
             <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="10px"  />
                </ClientSettings>
                <MasterTableView CommandItemDisplay="None" DataKeyNames="pkOrderID" ShowFooter="true">
                 <Columns>
                     <telerik:GridBoundColumn DataField="pkOrderID" HeaderText="Order#" UniqueName="pkOrderID" HeaderStyle-Width="100" FilterControlWidth="60"></telerik:GridBoundColumn>
                     <telerik:GridTemplateColumn UniqueName="eventname" HeaderText="Event Name"  SortExpression="eventname" FilterControlAltText="Filter eventname column" DataField="eventname" >
                         <ItemTemplate>
                            <a href='Order.aspx?orderID=<%# DataBinder.Eval(Container, "DataItem.pkOrderID")%>' ><%# DataBinder.Eval(Container, "DataItem.eventname")%></a>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn UniqueName="slabor" HeaderText="slabor"  SortExpression="slabor" FilterControlAltText="Filter slabor column" DataField="slabor" HeaderStyle-Width="100" FilterControlWidth="60">
                         <ItemTemplate>
                            <a href='Services.aspx?orderID=<%# DataBinder.Eval(Container, "DataItem.pkOrderID")%>' ><%# DataBinder.Eval(Container, "DataItem.slabor")%></a>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn UniqueName="selectrical" HeaderText="Electrical"  SortExpression="selectrical" FilterControlAltText="Filter selectrical column" DataField="selectrical" HeaderStyle-Width="100" FilterControlWidth="60">
                         <ItemTemplate>
                            <a href='Services.aspx?orderID=<%# DataBinder.Eval(Container, "DataItem.pkOrderID")%>' ><%# DataBinder.Eval(Container, "DataItem.selectrical")%></a>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn UniqueName="sLeadRetrieval" HeaderText="Lead Retrieval"  SortExpression="sLeadRetrieval" FilterControlAltText="Filter sLeadRetrieval column" DataField="sLeadRetrieval" HeaderStyle-Width="100" FilterControlWidth="60">
                         <ItemTemplate>
                            <a href='Services.aspx?orderID=<%# DataBinder.Eval(Container, "DataItem.pkOrderID")%>' ><%# DataBinder.Eval(Container, "DataItem.sLeadRetrieval")%></a>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn UniqueName="sCarpet" HeaderText="Carpet"  SortExpression="sCarpet" FilterControlAltText="Filter sCarpet column" DataField="sCarpet" HeaderStyle-Width="100" FilterControlWidth="60">
                         <ItemTemplate>
                            <a href='Services.aspx?orderID=<%# DataBinder.Eval(Container, "DataItem.pkOrderID")%>' ><%# DataBinder.Eval(Container, "DataItem.sCarpet")%></a>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn UniqueName="sInternet" HeaderText="Internet"  SortExpression="sInternet" FilterControlAltText="Filter sInternet column" DataField="sInternet" HeaderStyle-Width="100" FilterControlWidth="60">
                         <ItemTemplate>
                            <a href='Services.aspx?orderID=<%# DataBinder.Eval(Container, "DataItem.pkOrderID")%>' ><%# DataBinder.Eval(Container, "DataItem.sInternet")%></a>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>
                     <telerik:GridBoundColumn SortExpression="DateEnd" DataField="eventdates" HeaderText="Event Dates" UniqueName="eventdates" HeaderStyle-Width="150" FilterControlWidth="100">
                    </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="dateorder" HeaderText="Order Date" UniqueName="orderdate" HeaderStyle-Width="150" FilterControlWidth="100">
                    </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="status" HeaderText="Status" UniqueName="status" HeaderStyle-Width="100" FilterControlWidth="60">
                    </telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
    </telerik:RadGrid>
    </div>
        </div>
</asp:Content>

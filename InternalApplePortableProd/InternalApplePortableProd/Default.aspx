<%@ Page Language="VB"  Title="Home" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>


<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        h2 {
            font-size:25px;
            margin-left:30px;
            padding-bottom:0px;
            margin-bottom:0px;
        }
        .orders {
            width:90%;
            background-color:#ededed;
            border: 1px solid #dfdfdf;
            padding:30px;
            margin-left:auto;
            margin-right:auto;
        }
    </style>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>


    <div runat="server" id="pendingApprovals" style="width:100%"><h2>Pending Approval:</h2><br />
            <div class="orders">
            <telerik:RadGrid ID="RadGrid3" runat="server" Width="100%"  AllowSorting="True"  AutoGenerateColumns="false" Height="200px" >
                <ClientSettings  EnableRowHoverStyle="true">
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="10px"   />
                </ClientSettings>
                <MasterTableView CommandItemDisplay="None" DataKeyNames="pkOrderID" ShowFooter="true">
                 <Columns>
                     <%--<telerik:GridTemplateColumn UniqueName="eventname" HeaderText="Event Name"  SortExpression="eventname" FilterControlAltText="Filter eventname column" DataField="eventname">
                         <ItemTemplate>
                            <a href='Place.aspx?orderID=<%# DataBinder.Eval(Container, "DataItem.pkOrderID")%>' target="_blank"><%# DataBinder.Eval(Container, "DataItem.eventname")%></a>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>--%>

                      <telerik:GridBoundColumn DataField="eventname" HeaderText="Event Name" UniqueName="eventname" SortExpression="eventname">
                    </telerik:GridBoundColumn>

                      <telerik:GridBoundColumn DataField="ContactName" HeaderText="Created By" UniqueName="ContactName" SortExpression="ContactName">
                    </telerik:GridBoundColumn>

                     <telerik:GridBoundColumn DataField="DateOrder" HeaderText="Created On" UniqueName="DateOrder" SortExpression="DateOrder">
                    </telerik:GridBoundColumn>

                     <telerik:GridBoundColumn DataField="fkFulfillmentLocationId" HeaderText="Fullfilment Location" UniqueName="fkFulfillmentLocationId" SortExpression="fkFulfillmentLocationIds">
                    </telerik:GridBoundColumn>

                     <telerik:GridBoundColumn DataField="eventdates" HeaderText="Asset Arrival Date" UniqueName="eventdates" SortExpression="DateEnd">
                    </telerik:GridBoundColumn>


                    <telerik:GridTemplateColumn HeaderText="" UniqueName="ViewOrder" SortExpression="View" AllowFiltering="false">
         <ItemTemplate>
           <asp:LinkButton ID="ViewOrder" Text="View" runat="server" CausesValidation="False" CommandName="ViewOrder"
              ></asp:LinkButton>
         </ItemTemplate>
        </telerik:GridTemplateColumn>

                </Columns>
            </MasterTableView>
    </telerik:RadGrid></div></div>

        <div style="width:100%"><h2>Incomplete orders:</h2><br />
            <div class="orders">
            <telerik:RadGrid ID="RadGrid1" runat="server" Width="100%" AllowFilteringByColumn="True" AllowSorting="True"  AutoGenerateColumns="false" Height="500px" >
                <ClientSettings  EnableRowHoverStyle="true">
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="10px"   />
                </ClientSettings>
                <MasterTableView CommandItemDisplay="None" DataKeyNames="pkOrderID" ShowFooter="true">
                 <Columns>
                     <telerik:GridTemplateColumn UniqueName="eventname" HeaderText="Event Name"  SortExpression="eventname" FilterControlAltText="Filter eventname column" DataField="eventname" HeaderStyle-Width="80%">
                         <ItemTemplate>
                            <a href='Place.aspx?orderID=<%# DataBinder.Eval(Container, "DataItem.pkOrderID")%>' target="_blank"><%# DataBinder.Eval(Container, "DataItem.eventname")%></a>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>
                     <telerik:GridBoundColumn DataField="eventdates" HeaderText="Event Dates" UniqueName="eventdates" SortExpression="DateEnd">
                    </telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
    </telerik:RadGrid></div></div>
    <br /><br />
    <div style="width:100%"><h2>Your recent orders:</h2><br />
        <div class="orders">
            <telerik:RadGrid ID="RadGrid2" runat="server" AllowFilteringByColumn="True" AllowSorting="True"  AutoGenerateColumns="false" Height="500px" >
             <ClientSettings  EnableRowHoverStyle="true">
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="10px"  />
                </ClientSettings>
                <MasterTableView CommandItemDisplay="None" DataKeyNames="pkOrderID" ShowFooter="true">
                 <Columns>
                     <telerik:GridTemplateColumn UniqueName="eventname" HeaderText="Event Name"  SortExpression="eventname" FilterControlAltText="Filter eventname column" DataField="eventname" HeaderStyle-Width="50%">
                         <ItemTemplate>
                            <a href='Order.aspx?orderID=<%# DataBinder.Eval(Container, "DataItem.pkOrderID")%>' target="_blank"><%# DataBinder.Eval(Container, "DataItem.eventname")%></a>
                         </ItemTemplate>
                      </telerik:GridTemplateColumn>
                     
                     <telerik:GridBoundColumn DataField="eventdates" HeaderText="Event Dates" UniqueName="eventdates" SortExpression="DateEnd">
                    </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="dateorder" HeaderText="Order Date" UniqueName="orderdate">
                    </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="status" HeaderText="Status" UniqueName="status">
                    </telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
    </telerik:RadGrid>

        </div>
        
    </div>

</asp:Content>

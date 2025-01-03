<%@ Page Title="Order" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Order.aspx.vb" Inherits="Order" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <script type="text/javascript" src="Scripts/jquery-1.7.2.min.js"></script>
<script src="Scripts/jquery.fancybox.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="Scripts/jquery.fancybox.css" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <style runat="server" id="css">
        .entry {
            display: inline-block;
            height: auto;
            width: 500px;
            margin-left: 50px;
            vertical-align: top;
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
            display:inline-block;
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
            /*background-color:#ededed;*/
            border: 1px solid #dfdfdf;
            padding:30px;
            margin-left:auto;
            margin-right:auto;
            margin-top:30px;
        }
        .hidden {
            display:none;
        }

       /* .services {
            display:<%= services %>;
        }
        .portal {
            display:<%= portal %>;
        }*/
    </style>
    <script>
        $(document).ready(function () {
            $(".fancybox").click(function () {
                $.fancybox.open(this.src);
            });

            $('.imgcontainer').find('img').each(function () {
                var imgClass = (this.width / this.height > 1) ? 'wide' : 'tall';
                $(this).addClass(imgClass);
            })
        });
        </script>
<div class="orders" >
    <div runat="server" id="order">
        <h2 style="margin-left:20px;margin-bottom:20px;"> Order#: [pkOrderID] - Status: [Status]</h2>
        <table cellspacing="0" cellpadding="0" border="0" ><tr><td style="vertical-align:top;">
     <table style="display:inline-block;height:auto;width:auto;margin-left:50px;vertical-align:top;">
        <tr><td colspan="2"><h2>Show Information</h2></td></tr>
        <tr><td>Event Name:</td><td>[EventName]</td></tr>
        <tr><td>Apple Contact Name</td><td>[ContactName]</td></tr>
        <tr><td>Apple Contact Email:</td><td>[ContactEmail]</td></tr>
        <tr><td>Apple Contact Phone #:</td><td>[ContactPhone]</td></tr>
        <tr><td>Event Start Date:</td><td>[DateStart]</td></tr>
        <tr><td style="width:200px;">Event End Date:</td><td>[DatePickup]</td></tr>
        <tr>
            <td colspan="2"><h2>Additional Information:</h2></td>
        </tr>
        <tr>
            <td colspan="2">[Notes]</td>
        </tr>
    </table>
    </td>
    <td style="vertical-align: top;">
    <table style="display:inline-block;width:auto;margin-left:50px;vertical-align:top;">
        <tr>
          <td colspan="2"><h2>Shipping Information</h2></td>
        </tr>
            <tr>
                <td>Ship to:</td>
                <td>[Shipto]</td>
            </tr>
        <tr>
                <td>Booth #:</td>
                <td>[Boothnum]</td>
            </tr>
            <tr>
                <td>Onsite Contact Name:</td>
                <td>[ShipContactName]</td>   
            </tr>
             <tr>
                <td>Onsite Contact Phone #:</td>
                <td>[ShipContactPhone]</td>
            </tr>
            <tr>
                <td>Onsite Contact Email:</td>
                <td>[ShipContactEmail]</td>
            </tr>
            <tr>
                <td>Address Line 1:</td>
                <td>[ShipAddress1]</td>
            </tr>
            <tr>
                <td>Address Line 2:</td>
                <td>[ShipAddress2]</td>
            </tr>
            <tr>
                <td>City:</td>
                <td>[ShipCity]</td>
            </tr>
            <tr>
                <td valign="top">
                    <div style="vertical-align:top;">State/Province/Region:</div>
                </td>
                <td>[ShipState]</td>
            </tr>
            <tr>
                <td>ZIP/Postal Code:</td>
                <td>[ShipZip]</td>
            </tr>
            <tr>
                <td style="width:200px">
                    Country:</td>
                <td>[ShipCountry]</td>
            </tr>
            <tr>
                <td>Asset Arrival Date:</td>
                <td>[DateArrive]</td>
            </tr>
                    <tr><td>Shipping Type:</td><td>[ShippingType]</td></tr>

            <tr>
                <td>Tracking#:</td>
                <td>[TrackingNumber]</td>
            </tr>
        <tr>
                <td>FulfillmentLocation:</td>
                <td>[fkFulfillmentLocationId]</td>
            </tr>
         <tr><td style="white-space: nowrap;width:200px;vertical-align:top;">Attach Document(s):<div style="width:200px"></div></td><td>[Attachments]</td></tr>
    </table>
     </td>
    </tr>
 <tr><td>
   
         </td>
     </tr>
            </table>
    <br />
    </div>
       <h2 style="margin-left: 30px;">Order Change History</h2>
              <telerik:RadGrid ID="OrdersNote" Style="margin-left: 50px;margin-top:10px;width: 40%; overflow: auto; height: 10vh;" runat="server" AutoGenerateColumns="false">
        <MasterTableView AllowSorting="false"  >
                            <Columns>
                                <telerik:GridBoundColumn DataField="dateTime" HeaderText="Date/Time" UniqueName="DateCreated">
                                    <HeaderStyle Width="75px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="User" HeaderText="User" 
                                    UniqueName="User">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="logsNote" HeaderText="Notes" 
                                    UniqueName="logsNote">
                                </telerik:GridBoundColumn> 
                            </Columns>
                        </MasterTableView>
    </telerik:RadGrid>
     <h2 style="margin-left: 30px;">Order Inventory</h2>
    <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="false" Style="margin-top:10px;">
        <MasterTableView AllowSorting="false"  >
                            <Columns>
                               
                                <telerik:GridTemplateColumn DataField="Picture" HeaderText="Image" UniqueName="Picture">
                                    <ItemTemplate>
                                         <div class="imgcontainer"><img src='https://assets.yourpinnacle.net/CL1001/IMG/<%# (Eval("Picture"))%>' height='80' alt='<%# (Eval("Picture"))%>' class="fancybox"/></div>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="Type" HeaderText="Type" UniqueName="Type">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Description" HeaderText="Description" 
                                    UniqueName="Description">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="location" HeaderText="Item Location" 
                                    UniqueName="location">
                                </telerik:GridBoundColumn>

                               <%-- <telerik:GridTemplateColumn UniqueName="Fulfillment" HeaderText="Item Location" DataField="fkDepartmentID">
            <ItemTemplate>
                 <%# If(Eval("fkDepartmentID") <> 1004, "US", "Europe") %>   
            </ItemTemplate>
        </telerik:GridTemplateColumn>    --%>
                                <telerik:GridBoundColumn DataField="PartNum" HeaderText="Part#" 
                                    UniqueName="Dimensions">
                                </telerik:GridBoundColumn> 
                                <telerik:GridBoundColumn DataField="Quantity" HeaderText="Quantity" 
                                    UniqueName="Quantity" >
                                </telerik:GridBoundColumn>
                                
                            </Columns>
                        </MasterTableView>



    </telerik:RadGrid>


    <div id="fulfillment" runat="server" visible="false"><br /><br />
        <h2 style="margin-left:20px;margin-bottom:20px;">Fulfillment:</h2>

    
    <telerik:RadGrid ID="RadGrid2" runat="server" AutoGenerateColumns="False" GridLines="None"  Width="100%" >
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" FileName="Inventory"></ExportSettings>
            <MasterTableView AllowSorting="false" ClientDataKeyNames="pkOrderInventoryItemsID" DataKeyNames="pkOrderInventoryItemsID" CommandItemDisplay="None" >
                <CommandItemSettings  ShowAddNewRecordButton='false' ></CommandItemSettings>
                    <RowIndicatorColumn >
                    <HeaderStyle Width="20px"></HeaderStyle>
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn>
                    <HeaderStyle Width="20px"></HeaderStyle>
                    </ExpandCollapseColumn>
                    <Columns>
                    
                    <telerik:GridBoundColumn HeaderText="Name" UniqueName="Name" DataField="Name" ></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="SKU" UniqueName="SKU" DataField="SKU" HeaderStyle-Width="20px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Quantity" UniqueName="Quantity" DataField="Quantity" HeaderStyle-Width="20px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Date Out" Datafield="DateOut" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DateOut" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataType="System.DateTime" HeaderText="Date In" Datafield="DateIn" DataFormatString="{0:MM/dd/yyyy}" UniqueName="DateIn" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Status" UniqueName="Status" DataField="Status" HeaderStyle-Width="70px" ></telerik:GridBoundColumn>
                        
        <telerik:GridTemplateColumn HeaderText="Image" UniqueName="ImageIn" DataField="Picture">
                        <ItemTemplate>
                            <%# GetDamagesImagePath(Eval("picture")) %>
                            
                            
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn HeaderText="Notes" UniqueName="Notes" DataField="Notes"></telerik:GridBoundColumn>
                    </Columns>
</MasterTableView >
    </telerik:RadGrid>

    </div>
    </div>
</asp:Content>


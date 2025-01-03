<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Emailer.aspx.vb" Inherits="Emailer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <script type="text/javascript" src="Scripts/jquery-1.7.2.min.js"></script>
<script src="Scripts/jquery.fancybox.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="Scripts/jquery.fancybox.css" />
    <title></title>
    <style runat="server" id="css">
        /*.entry{
            display:inline-block;
            height:auto;
            width:500px;
            margin-left:50px;
            vertical-align:top;
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
            height: 80px;*/
            /*background: #fff;*/
            /*margin: 0 auto;
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
        }*/
        .orders {
            width:90%;
            /*background-color:#ededed;*/
            border: 1px solid #dfdfdf;
            padding:30px;
            margin-left:auto;
            margin-right:auto;
            margin-top:30px;
            
        }

        table {
  font-family: arial, sans-serif;
  border-collapse: collapse;
  width: 100%;
}

td, th {
  border: 1px solid #dddddd;
  text-align: left;
  padding: 8px;
}

tr:nth-child(even) {
  background-color: #dddddd;
}

        .hidden {
            display:none;
        }

        
    </style>

</head>
<body>
    <div class="orders" style="display:none;">
    <div runat="server" id="order">
        <table cellspacing="0" cellpadding="0" border="0" >
            <tr>
                <td style="vertical-align:top;">
     <table style="display:inline-block;height:auto;width:auto;margin-left:50px;vertical-align:top;">
         <tr>
    <th>Order #</th>
    <th>Event Name</th>
    <th>Event Start Date</th>
    <th>Event End Date</th>
    <th>Location</th>
  </tr>
  <tr>
    <td>[pkId]</td>
    <td>[EventName]</td>
    <td>[DateStart]</td>
    <td>[DatePickup]</td>
    <td>[Location]</td>
  </tr>
    </table>
    </td>
    <td>
     </td>
    </tr>
            </table>
    <br />
    </div>
    </div>

    <%--<form id="form1" runat="server">

    <div id="OrderNotReceived" runat="server">


    </div>
        <asp:GridView ID="GridView" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical">
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#808080" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
        </asp:GridView>
    </form>--%>
</body>
</html>
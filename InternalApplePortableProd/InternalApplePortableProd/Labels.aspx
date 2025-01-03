<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Labels.aspx.vb" Inherits="Labels" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        @page { margin: 0;size: landscape; padding: 0;}
        @media print {
            body, html {margin: 0;size: landscape; padding: 0;}
            footer {page-break-after: always;}
            footer:last-child{ page-break-after:auto;}

        }

         #Custom_Size__1 {
/*            border: 1px solid;
*/     position: absolute;
            width: 350px;
            height: 115px;
            background-color: rgba(255,255,255,1);
            overflow: hidden;
            /*--web-view-name: Custom Size – 1;
		--web-view-id: Custom_Size__1;
		--web-scale-on-resize: true;
		--web-enable-deep-linking: true;*/
        }

        #div_rightspacer {
            fill: rgba(255,255,255,1);
        }

        .div_rightspacer {
            position: absolute;
            overflow: visible;
            width: 10px;
            height: 84px;
            left: 253px;
            top: 0px;
        }

        #div_leftspacer {
            fill: rgba(255,255,255,1);
        }

        .div_leftspacer {
            position: absolute;
            overflow: visible;
            width: 10px;
            height: 84px;
            left: 0px;
            top: 0px;
        }

        #div_logo {
            fill: rgba(255,255,255,1);
        }

        .div_logo {
            position: absolute;
            overflow: visible;
            width: 57px;
            height: 14px;
            left: 10px;
            top: 0px;
        }

        #div_company {
            fill: rgba(255,255,255,1);
        }

        .div_company {
            position: absolute;
            overflow: visible;
            width: 112px;
            height: 14px;
            left: 67px;
            top: 0px;
        }

        #div_partnum {
            fill: rgba(255,255,255,1);
        }

        .div_partnum {
            position: absolute;
            overflow: visible;
            width: 74px;
            height: 14px;
            left: 179px;
            top: 0px;
        }

        #div_descr {
            fill: rgba(255,255,255,1);
        }

        .div_descr {
            position: absolute;
            overflow: visible;
            width: 243px;
            height: 28px;
            left: 10px;
            top: 14px;
        }

        #div_barcode {
            fill: rgba(255,255,255,1);
        }

        .div_barcode {
            position: absolute;
            overflow: visible;
            width: 186px;
            height: 42px;
            left: 67px;
            top: 42px;
        }

        #div_sku {
            fill: rgba(255,255,255,1);
        }

        .div_sku {
            position: absolute;
            overflow: visible;
            width: 57px;
            height: 42px;
            left: 10px;
            top: 42px;
        }

        #barcode {
            left: 73px;
            top: 61px;
            position: absolute;
            overflow: visible;
            width: 187px;
            height: 41px;
            text-align: center;
            font-family: Segoe UI;
            font-style: normal;
            font-weight: normal;
            font-size: 24px;
            color: rgba(112,112,112,1);
        }

        #SKU_ {
            left: 20px;
            top: 55px;
            position: absolute;
            overflow: visible;
            width: 57px;
            height: 41px;
            text-align: left;
            font-family: Segoe UI;
            font-weight:bold;
            font-size: 16px;
            color: rgba(112,112,112,1);
            letter-spacing: 0.01px;
        }

        #Description {
            left: 21px;
            top: 10px;
            position: relative;
            overflow: visible;
            width: 311px;
            height: 28px;
            text-align: left;
            font-family: Segoe UI;
            font-style: normal;
            font-size: 11px;
            color: rgba(112,112,112,1);
            letter-spacing: 0.01px;
        }

        #Logo {
            left: 14px;
            top: -3px;
            position: absolute;
            overflow: visible;
            width: 58px;
            height: 14px;
            text-align: left;
            font-family: Segoe UI;
            font-style: normal;
            font-weight:bold;
            font-size: 8px;
            color: rgba(112,112,112,1);
            letter-spacing: 0.01px;
        }

        #PartNum {
            left: 250px;
            top: 0px;
            position: absolute;
            overflow: visible;
            width: 75px;
            height: 14px;
            text-align: right;
            font-family: Segoe UI;
            font-weight:bold;
            font-weight: lighter;
            font-size: 8px;
            color: rgba(112,112,112,1);
            letter-spacing: 0.01px;
        }

        #Company_Name {
            left: 152px;
            top: 4px;
            position: absolute;
            overflow: visible;
            width: 113px;
            height: 14px;
            text-align: left;
            font-family: Segoe UI;
            font-weight:bold;
            font-weight: lighter;
            font-size: 8px;
            color: rgba(112,112,112,1);
            letter-spacing: 0.01px;
        }
        
        body, html {margin: 0;size: landscape; padding: 0; font-family:Arial}
    </style>
</head>
<body>
    <%--<form id="form1" runat="server">
    <div>
     <asp:Repeater ID="Repeater1" runat="server">
                <HeaderTemplate>
                
                </HeaderTemplate>
            <ItemTemplate>




                <table style="width:310px;padding:0px;margin:0px;border:0px;">
            <tr>
                <td style="vertical-align:top;padding:0px;margin:0px;">
                    <div style="float:left;height:30px;overflow:hidden; "><img src="img/logo.png" alt="PINNACLE" style="width:20px;"/></div>
                    <div style="float:right;font-weight:bold;font-size:30px;height:30px;overflow:hidden;"><%# Eval("partnum")%></div>          
                </td>
            </tr>
<tr>
    <td>
        <div style="float:left;display:inline-block;font-size:16px;font-weight:bold;height:45px;width:150px;overflow-wrap:break-word;margin-left:2px">
            <%# Eval("InventoryType")%> - <%# Eval("Description")%>...

            <div style="font-size:12px;font-weight:bold;"> sku:<%# Eval("pkitemID")%></div>

        </div>
          <div style="display:inline-block;margin-top:120px;margin-left:50px;"><telerik:RadBarcode ID="RadBarcode2" runat="server" Height="40px" Width="150px" ShowText="False"></telerik:RadBarcode></div>
    </td>
</tr>           
            <tr>
                <td><center><div style="margin-right:55px;"><%# Eval("Name")%></div></center></td>
            </tr>
        </table>
            <footer></footer>
            </ItemTemplate>

        </asp:Repeater>
        
    </div>
        <script>
            window.print()
        </script>
    </form>--%>

      <form id="form1" runat="server">
        <div>
            <asp:Repeater ID="Repeater1" runat="server">
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>

                    <table runat="server" style="padding: 0px; margin: 0px; border: 0px;">
                        <tr>
                            <td>
                                <div id="Custom_Size__1" style="font-size:12px;color:black;font-weight:bold;overflow-wrap:break-word">
                                    <svg class="div_rightspacer">
                                        <rect id="div_rightspacer" rx="0" ry="0" x="0" y="0" width="10" height="84">
                                        </rect>
                                    </svg>
                                    <svg class="div_leftspacer">
                                        <rect id="div_leftspacer" rx="0" ry="0" x="0" y="0" width="10" height="84">
                                        </rect>
                                    </svg>
                                    <svg class="div_logo">
                                        <rect id="div_logo" rx="0" ry="0" x="0" y="0" width="57" height="14">
                                        </rect>
                                    </svg>
                                    <svg class="div_company">
                                        <rect id="div_company" rx="0" ry="0" x="0" y="0" width="112" height="14">
                                        </rect>
                                    </svg>
                                    <svg class="div_partnum">
                                        <rect id="div_partnum" rx="0" ry="0" x="0" y="0" width="74" height="14">
                                        </rect>
                                    </svg>
                                    <svg class="div_descr">
                                        <rect id="div_descr" rx="0" ry="0" x="0" y="0" width="243" height="28">
                                        </rect>
                                    </svg>
                                    <svg class="div_barcode">
                                        <rect id="div_barcode" rx="0" ry="0" x="0" y="0" width="186" height="42">
                                        </rect>
                                    </svg>
                                    <svg class="div_sku">
                                        <rect id="div_sku" rx="0" ry="0" x="0" y="0" width="57" height="42">
                                        </rect>
                                    </svg>
                                    <div id="barcode">
                                        <span>
                                            <telerik:RadBarcode ID="RadBarcode2" runat="server" Height="35px" Width="235px" ShowText="False"></telerik:RadBarcode>
                                        </span>
                                    </div>
                                    <div id="SKU_">
                                        <span>SKU<br />
                                            <%# Eval("pkitemID")%></span>
                                    </div>
                                    </br>
                                    <div id="Description">
                                        <span><%# Eval("InventoryType")%> - <%# Eval("Description")%>...</span>
                                    </div>
                                    <div id="Logo">
                                        <span>
                                            <img src="img/logo.png" style="height:27px;" alt="PINNACLE" />

                                        </span>
                                    </div>
                                    <div style="font-size:14px;font-weight:bold;" id="PartNum">
                                        <span><%# Eval("partnum")%></span>
                                    </div>
                                    <div id="Company_Name">
                                        <span><%# Eval("Name")%></span>
                                    </div>
                                </div>
                            </td>
                        </tr>

                    </table>
                    <footer></footer>
                </ItemTemplate>

            </asp:Repeater>

        </div>
        <script>
            window.print()
        </script>
    </form>


</body>

</html>

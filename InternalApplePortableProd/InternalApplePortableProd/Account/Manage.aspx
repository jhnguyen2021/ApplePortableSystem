<%@ Page Title="Manage" Language="VB" AutoEventWireup="false"  MasterPageFile="~/Site.master"  CodeFile="Manage.aspx.vb" Inherits="Account_Manager" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="Scripts/jquery-1.7.2.min.js"></script>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <style>
        .entry{
            display:inline-block;
            width:500px;
            margin-left:50px;
            margin-top:50px;
            background-color:#ededed;
            border: 1px solid #dfdfdf;
            padding:30px;
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
            width:auto;
            background-color:#ededed;
            border: 1px solid #dfdfdf;
            padding:30px;
            margin-left:30px;
            margin-right:175px;
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

        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 300px;
            height: 140px;
        }

        .hidden {
            display:none;
        }
        h2 {
            padding: 0px;
            margin:0px;
        }
    </style>
    <telerik:RadAjaxPanel ID="panel1" runat="server" RestoreOriginalRenderDelegate="false">
    <table class="entry">
        <tr><td colspan="2" ><h2>User Information</h2></td></tr>
        <tr><td>First Name:</td><td><telerik:RadTextBox ID="tbFirstName" runat="server" width="200px"></telerik:RadTextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="First Name" ControlToValidate="tbFirstName" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator></td></tr>
        <tr><td>Last Name:</td><td><telerik:RadTextBox ID="tbLastName" runat="server" width="200px"></telerik:RadTextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="Last Name" ControlToValidate="tbLastName" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator></td></tr>
        <tr><td>Email:</td><td><telerik:RadTextBox ID="tbEmail" runat="server" width="200px"></telerik:RadTextBox><asp:RegularExpressionValidator ID="emailValidator" runat="server" Display="Dynamic"
    ErrorMessage="Please, enter valid e-mail address." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$" ForeColor="Red" ControlToValidate="tbEmail" ValidationGroup="ValidateFields">*</asp:RegularExpressionValidator>
  <asp:RequiredFieldValidator ID="Requiredfieldvalidator5" runat="server" Display="Dynamic" ControlToValidate="tbEmail" ErrorMessage="Email" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator> 
            </td></tr>
        <tr><td>Phone #:</td><td><telerik:RadNumericTextBox ID="tbPhone" runat="server" width="200px" NumberFormat-DecimalDigits="0">
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
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Phone" ControlToValidate="tbPhone" ForeColor="Red" ValidationGroup="ValidateFields">*</asp:RequiredFieldValidator></td></tr>
        
        <tr><td>Password</td><td><telerik:RadTextBox ID="tbPassword" runat="server" width="200px" TextMode="Password"></telerik:RadTextBox><asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Passwords don't match" ForeColor="Red" ControlToCompare="tbPassword2" ControlToValidate="tbPassword" Operator="Equal">*</asp:CompareValidator></td></tr>
        <tr><td>Re-type Password</td><td><telerik:RadTextBox ID="tbPassword2" runat="server" width="200px" TextMode="Password"></telerik:RadTextBox>
            
            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords don't match" ForeColor="Red" ControlToCompare="tbPassword" ControlToValidate="tbPassword2" Operator="Equal">*</asp:CompareValidator>
            </td></tr>
        <tr><td>Division:</td><td><asp:DropDownList ID="ddlDivision" runat="server" width="200px" AutoPostBack="true"></asp:DropDownList></td></tr>
        <tr runat="server" id="managerrow" visible="false"><td>Mananger's Email:</td><td><telerik:RadTextBox ID="tbManager" runat="server" width="200px"></telerik:RadTextBox>
            <asp:RequiredFieldValidator ID="valManager" runat="server" ErrorMessage="Venue" ControlToValidate="tbManager" ForeColor="Red" ValidationGroup="ValidateFields" Enabled="false">*</asp:RequiredFieldValidator>
                                      </td></tr>
    </table>
       </telerik:RadAjaxPanel> 
        <br />
   
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ValidationGroup="ValidateFields"/> 
    <telerik:RadButton ID="RadButton2" runat="server" Text="Update" Height="40px" style="margin-left:50px;margin-top:20px;margin-bottom:50px;" RenderMode="Lightweight"  ValidationGroup="ValidateFields"  OnClientClick="validateOrderQuantity()"></telerik:RadButton>
    <asp:Label ID="Label1" runat="server" style="margin-left:50px;margin-top:30px;display:inline-block;"></asp:Label>
    
</asp:Content>

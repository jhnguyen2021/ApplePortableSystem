<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ResetPassword.aspx.vb" Inherits="Account_ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Asset Management System</title>
	<meta charset="UTF-8">
	<meta name="viewport" content="width=device-width, initial-scale=1">
<!--===============================================================================================-->	
	<link rel="icon" type="image/png" href="images/icons/favicon.ico"/>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="vendor/bootstrap/css/bootstrap.min.css">
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="fonts/font-awesome-4.7.0/css/font-awesome.min.css">
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="fonts/iconic/css/material-design-iconic-font.min.css">
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="vendor/animate/animate.css">
<!--===============================================================================================-->	
	<link rel="stylesheet" type="text/css" href="vendor/css-hamburgers/hamburgers.min.css">
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="vendor/animsition/css/animsition.min.css">
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="vendor/select2/select2.min.css">
<!--===============================================================================================-->	
	<link rel="stylesheet" type="text/css" href="vendor/daterangepicker/daterangepicker.css">
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="css/util.css">
	<link rel="stylesheet" type="text/css" href="css/main.css">
</head>
<body>
        <div class="limiter">
		<div class="container-login100" >
			<div class="wrap-login100">
				<form class="login100-form validate-form"  id="form1" runat="server">
					<span class="login100-form-logo">
						<img src="../img/logo.png" />
					</span>

					<span class="login100-form-title p-b-34 p-t-27">
						Reset Password
					</span>

					<div class="wrap-input100 validate-input" data-validate = "Enter username">
						
						<asp:TextBox class="input100" type="text" name="username" placeholder="Username" id="txtUsername" runat="server"></asp:TextBox>
						<span class="focus-input100" data-placeholder="&#xf207;"></span>
					</div>

					<div runat="server" id="enterPass" class="wrap-input100 validate-input" data-validate="Enter password">
						<asp:TextBox class="input100" type="password" name="pass" placeholder="Password"  id="txtPassword" runat="server"></asp:TextBox>
						<span class="focus-input100" data-placeholder="&#xf191;"></span>
					</div>

					<div runat="server" id="rePass" class="wrap-input100 validate-input" data-validate="Enter password">
						<asp:TextBox class="input100" type="password" name="pass" placeholder="Confirm Password"  id="txtConfirmPassword" runat="server"></asp:TextBox>
						<span class="focus-input100" data-placeholder="&#xf191;"></span>
					</div>

					<div class="container-login100-form-btn">
						<asp:Button class="login100-form-btn" ID="Button1" runat="server" Text="Validate Email" />
					</div>
					<div class="container-login100-form-btn">
						<asp:Button class="login100-form-btn" ID="Button2" runat="server" Text="Reset Password" />
					</div>
                    <div class="text-center p-t-90">
                        <asp:Label ID="failure" runat="server" Text="" ></asp:Label>
					</div>
					
				</form>
			</div>
		</div>
	</div>
	

	<div id="dropDownSelect1"></div>
	
	<script src="vendor/jquery/jquery-3.2.1.min.js"></script>
	<script src="vendor/animsition/js/animsition.min.js"></script>
	<script src="vendor/bootstrap/js/popper.js"></script>
	<script src="vendor/bootstrap/js/bootstrap.min.js"></script>
	<script src="vendor/select2/select2.min.js"></script>
	<script src="vendor/daterangepicker/moment.min.js"></script>
	<script src="vendor/daterangepicker/daterangepicker.js"></script>
	<script src="vendor/countdowntime/countdowntime.js"></script>
	<script src="js/main.js"></script>

          
</body>
</html>

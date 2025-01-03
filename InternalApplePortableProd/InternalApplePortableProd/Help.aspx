
<%@ Page Title="Help" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Help.aspx.vb" Inherits="Help" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

	    	<style>

		* {
  box-sizing: border-box;
}

/* Create four equal columns that floats next to each other */
.column {
  float: left;
  width: 25%;
  height: 600px; /* Should be removed. Only for demonstration */;
	border: black solid 1px;
	
}

/* Clear floats after the columns */
.row:after {
  content: "";
  display: table;
  clear: both;
}

p {
	padding-left: 10px;
}


	</style>

    <h3>Order Inquires</h3>
	<p>For additional support please contact <a href="mailto:appleportables@pinnacle-exp.com">appleportables@pinnacle-exp.com</a></p>
    <h3>Internal Wiki</h3>
    <div class="desc"><a href='Documents/Help//Editing a Portable Order.pdf'>Edit a Portable Order</a></div><br/>




	<div class="row">
  
		<div class="column" >
	  <div style="text-align:center;border-bottom: black solid 1px;padding:10px;"><b>CHINA-Address</b></div>
    <p>APAC-China</p>
		<p>Shanghai Yahui Trading company</p>
		<p>Shanghai, China</p>
		<p>Xuhui District</p>
		<p>1602 Zhongshan West Road, Building 2 Office: 206-D07</p>
		<p>Attention: Glen Kopelchak</p>
		<p>Phone: 135 1214 1942</p>
		<p></p>
		<p></p>
		<p>Chinese Address</p>
		<p></p>
  <p>上海崖慧国际贸易有限公司</p>
  <p>上海市徐汇区</p>
  <p>中山西路1602号2楼206-D07室</p>
  <p>联系：王哥蓝 Glen Kopelchak </p>
			<p>电话号：135 1214 1942</p>
  </div>
  <div class="column" >
   <div style="text-align:center;border-bottom: black solid 1px;padding:10px;"><b>AUSTRALIA-Address</b></div>
   <p>Apple - APAC - Australia</p>
		<p>Exposure Group Pty Ltd</p>
		<p>23 Foundry Road, Seven Hills</p>
		<p>NSW, 2147 Australia</p>
		<p>Benjamin Feltham / Managing Director</p>
		<p><a href="mailto:ben@exposuregroup.com.au">ben@exposuregroup.com.au</a></p>
		<p>Mobile: +61402106543</p>
		<p>Office: +612 9519 7444</p>
  </div>
  <div class="column" >
    <div style="text-align:center;border-bottom: black solid 1px;padding:10px;"><b>JAPAN-Address</b></div>
    <p>Apple - APAC - Japan</p>
		<p>Shiroishi Transportation, Nagareyama warehouse</p>
		<p>8-1 Minami</p>
		<p>Nagareyama, Chiba</p>
		<p>270-0124</p>
		<p>Japan</p>
		<p>Raymond Ho （レイモンド　ホー）</p>
		<p>Mobile : +81(0)70-1401-8341</p>
	  <p></p>
		<p></p>
		<p>Japanese Details</p>
		<p></p>
  <p>白石通運流山営業所</p>
  <p>千葉県流山市南8-1</p>
  <p>〒270-0124</p>
  <p>Raymond Ho （レイモンド　ホー）</p>
			<p>Mobile : +81(0)70-1401-8341</p>
  </div>
  <div class="column" >
    <div style="text-align:center;border-bottom: black solid 1px;padding:10px;"><b>EMEA-Address</b></div>
    <p>Apple - EMEA</p>
		<p>Oldenzaalsestraat 21</p>
		<p>NL 7631 CT Ootmarsum - The Netherlands</p>
  </div>
</div>

</asp:Content>


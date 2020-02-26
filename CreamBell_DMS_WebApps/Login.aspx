<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CreamBell_DMS_WebApps.Login" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>CBSAM Portal</title>
    <link href="css/acxiom.css" rel="stylesheet" />
    <meta charset="utf-8" />
</head>
<body>
    <form id="form1" runat="server">

        <div id="outercontainer">
            <div class="logoarea">
                <img src="images/CB-logo.jpg" border="0px" alt="CBSM LOGO"
                    title="CBSM LOGO" />
            </div>
            <div class="logingap">
                Login
            </div>
            <div class="loginleftarea">
                <div class="firstrow">
                    <span class="username">
                        <img src="images/username.png" border="0px" alt="CBSM LOGO"
                            title="CBSM LOGO" />
                    </span><span>
                        <asp:TextBox ID="txtUserName" class="input1" runat="server" placeholder="Username "></asp:TextBox>
                    </span>
                </div>
                <div class="firstrow1">
                    <span class="username">
                        <img src="images/password.png" border="0px" alt="CBSM LOGO"
                            title="CBSM LOGO" />
                    </span><span>
                        <asp:TextBox ID="txtPassword" runat="server" class="input1" placeholder="Password" TextMode="Password"></asp:TextBox>
                    </span>
                </div>
                <div class="firstrow2">
                    <span class="chklogin">
                        <asp:CheckBox ID="chkRemember" class="chklogin" runat="server" />
                    </span>
                    <span class="rnblogin">Remember Passwords</span>
                    <span class="sgnin">
                        <a href="#">
                            <asp:ImageButton ID="BtnLogin" class="center-block" src="images/signinbtn.png" runat="server" border="0" OnClick="BtnLogin_Click1" /></a>
                        </a>
                        <br />
                        <asp:Label ID="LblMessage" runat="server" Text="" ForeColor="Red" Font-Italic="true"></asp:Label>
                    </span>
                </div>

            </div>
        </div>
    </form>
</body>
</html>


<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmUserProfile.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmUserProfile" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/style.css" rel="stylesheet" />

    <link href="css/ui-controls.css" rel="stylesheet" />


    <link href="JqueryNotify/jquery.notice.css" rel="stylesheet" />
    <script src="JqueryNotify/jquery.notice.js"></script>
    <script src="JqueryNotify/jquery2.0.2.min.js"></script>

    <script type="text/javascript">

        function test() {
            $(".arrow_box").addClass("arrow_box1")
            // remove a class
            $(".arrow_box").removeClass("arrow_box")
        }
        function test1() {

            $(".arrow_box1").addClass("arrow_box")
            // remove a class
            $(".arrow_box1").removeClass("arrow_box1")
        }
    </script>

    <script>
        $(function () {
            setTimeout(function () { $("#logindiv").fadeOut(1500); }, 5000)
            $('#body').load(function () {
                $('#logindiv').show();
                setTimeout(function () { $("#logindiv").fadeOut(1500); }, 5000)
            })
        })

        $(document).ready(function () {
            var hidField = document.getElementById('<%=SesionLoginID.ClientID %>');

            jQuery.noticeAdd({
                text: 'Welcome, ' + hidField.value,
                stay: false
            });

        });

        function GetValue() {
            var hidField = document.getElementById('<%=SesionLoginID.ClientID %>');
             alert(hidField.value);
         }
    </script>   

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    
    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #0085ca; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px; color: #FFFFFF;">User Profile</span>
    </div>


    <asp:HiddenField ID="SesionLoginID" runat="server" />

    <div style="background-color: #c5d9f1; width: 100%">

        <div style="overflow: auto; height: 450px; margin: 10px 0px 0px 10px;">

            <table align="center">
                <tr>
                    <td style="text-align: right">User Id :
                        
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtUserID" runat="server" CssClass="textbox1" Width="284px" Enabled="false"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">User Name :
                        
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="textbox1" Width="284px" Enabled="false"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">State :
                        
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtUserState" runat="server" CssClass="textbox1" Width="284px" Enabled="false"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">Site Name:
                        
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtSiteName" runat="server" CssClass="textbox1" Width="284px" Enabled="false"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">Mobile Number :
                        
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtMobileNo" runat="server" CssClass="textbox1" Width="284px" Enabled="false"></asp:TextBox>

                    </td>
                </tr>
                 <tr>
                    <td style="text-align: right">Phone Number :
                        
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="textbox1" Width="284px" Enabled="false"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">Address :
                        
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="textbox1" TextMode="MultiLine" Width="284px" Height="67px" Enabled="false"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">VAT/TIN NO :
                   </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtVat" runat="server" CssClass="textbox1" Width="284px" Enabled="false"></asp:TextBox>

                    </td>
                </tr>
                 <tr>
                    <td style="text-align: right">GSTTIN :
                        
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtGSTNo" runat="server" CssClass="textbox1" Width="284px" Enabled="false"></asp:TextBox>

                    </td>
                </tr>
                 <tr>
                    <td style="text-align: right">GST REGISTRATION DATE :
                        
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtRegnDate" runat="server" CssClass="textbox1" Width="284px" Enabled="false"></asp:TextBox>

                    </td>
                </tr>
                 <tr>
                    <td style="text-align: right">COMPOSITION :
                        
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtComposition" runat="server" CssClass="textbox1" Width="284px" Enabled="false"></asp:TextBox>

                    </td>
                </tr>
                 <tr>
                    <td style="text-align: right">PAN NO :
                        
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtPAN" runat="server" CssClass="textbox1" Width="284px" Enabled="false"></asp:TextBox>

                    </td>
                </tr>
            </table>

        </div>
    </div>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="CreamBell_DMS_WebApps.Home" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="css/style.css" rel="stylesheet" />
    <script src="Javascript/custom.js"></script>
    <script src="dashboard/jquery.min.js"></script>
    <link href="dashboard/jquerysctipttop.css" rel="stylesheet" />
    <script src="dashboard/waypoints.min.js"></script>
    <%--<script src="jquery/jquery.counterup.js"></script>
    <script src="jquery/jquery.counterup.min.js"></script>

    <script>
        jQuery(document).ready(function ($) {
            $('.counter').counterUp({
                delay: 30,
                time: 1000
            });
        });
    </script>--%>
    <script src="dashboard/jquery.min.js"></script>
    <link href="dashboard/jquerysctipttop.css" rel="stylesheet" />
    <script src="dashboard/waypoints.min.js"></script>

    <link href="css/ui-css.css" rel="stylesheet" />


    <style type="text/css">
        span {
            font-size: 66px;
            color: #555;
            display: inline-block;
            font-weight: 400;
            text-align: center;
        }

            span > span {
                margin-bottom: 0;
            }

        .textShadowEffect {
            text-shadow: 2px 2px 5px red;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="100%">
                        <div id="UserInfoDashboard" style="margin-left: 50px">

                            <table style="border-spacing: 15px; border-collapse: separate; width: 90%; text-align: center; margin-left: 29px;">
                                <tr style="width: 90%">

                                    <td style="width: 30%; height: 80px; padding: 6px;">
                                        <div id="Currentinfo" style="background-color: AliceBlue; width: 25%; height: 10%; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19); position: absolute; text-wrap: avoid; margin-top: -30px; text-align: center;">
                                            <asp:Label ID="LblLastLoginTime" runat="server" Text="Last Login at  : " Font-Size="12" Font-Bold="true" Style="margin: 15px;"></asp:Label>
                                        </div>
                                    </td>

                                    <td style="width: 30%; height: 80px; padding: 6px;">
                                        <div id="LastLoginTime" style="background-color: LightSalmon; width: 25%; height: 10%; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19); position: absolute; text-wrap: avoid; margin-top: -30px; text-align: center; vertical-align: middle">
                                            <asp:Label ID="LblUserName" runat="server" Text="UserName : " Font-Size="12" Font-Bold="true" Style="margin: 15px;"></asp:Label>
                                        </div>

                                    </td>

                                    <td style="width: 30%; height: 80px; padding: 6px;">
                                        <div id="Div1" style="background-color: Plum; width: 25%; height: 10%; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19); position: absolute; text-wrap: avoid; margin-top: -30px; text-align: center; vertical-align: middle">
                                            <asp:Label ID="LblSite" runat="server" Text=" Site Location :" Font-Size="12" Font-Bold="true" Style="margin: 15px;"></asp:Label>
                                        </div>

                                    </td>

                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td width="100%">
                        <div id="dashboard" style="width: 80%; margin-top: 15px; margin-left: 94px; margin-right: 20px">
                            <div class="polaroid" onclick="location.href='frmPurchaseIndentList.aspx'" style="cursor: pointer; background-color: white">

                                <div class="container" style="font-weight: bold; font-size: large">
                                    Total Purchase Indent
                <p style="font-weight: bold; font-size: large">
                    <span class="counter" style="display: inline-block; width: 32%">
                        <asp:Label ID="LblPurchaseIndent" runat="server" Text=""></asp:Label>
                    </span>

                </p>
                                </div>
                            </div>

                            <div class="polaroid" onclick="location.href='frmPurchReceiptList.aspx'" style="font-weight: bold; font-size: large; cursor: pointer; background-color: Pink">

                                <div class="container" style="font-weight: bold; font-size: large">
                                    Total Purchase Invoice
                <p style="font-weight: bold; font-size: large">
                    <span class="counter" style="display: inline-block; width: 32%">
                        <asp:Label ID="LblPurchaseInvoice" runat="server" Text=""></asp:Label>
                    </span>
                </p>
                                </div>
                            </div>

                            <div class="polaroid" onclick="location.href='frmPurchaseReturnList.aspx'" style="font-weight: bold; font-size: large; cursor: pointer; background-color: Lavender">

                                <div class="container" style="font-weight: bold; font-size: large">
                                    Total Purchase Return
                <p style="font-weight: bold; font-size: large">
                    <span class="counter" style="display: inline-block; width: 32%">
                        <asp:Label ID="LblPurchaseReturn" runat="server" Text=""></asp:Label>
                    </span>
                </p>
                                </div>
                            </div>

                            <div class="polaroid" onclick="location.href='ReportSalesInvoice.aspx'" style="font-weight: bold; font-size: large; cursor: pointer; background-color: LightGreen">

                                <div class="container" style="font-weight: bold; font-size: large">
                                    Total Sale Invoice
                <p style="font-weight: bold; font-size: large">
                    <span class="counter" style="display: inline-block; width: 32%">
                        <asp:Label ID="LblSaleInvoice" runat="server" Text=""></asp:Label>
                    </span>
                </p>
                                </div>
                            </div>

                            <div class="polaroid" onclick="location.href='frmLoadSheetCreation.aspx'" style="font-size: large; cursor: pointer; background-color: LightYellow">

                                <div class="container">
                                    Pending Sale Order
                <p style="font-weight: bold; font-size: large">
                    <span class="counter" style="display: inline-block; width: 32%">
                        <asp:Label ID="LblSaleOrder" runat="server" Text=""></asp:Label>
                    </span>
                </p>
                                </div>
                            </div>

                            <div class="polaroid" onclick="location.href='frmProductMaster.aspx'" style="font-weight: bold; font-size: large; cursor: pointer; background-color: MistyRose">

                                <div class="container">
                                    Total Product
                <p style="font-weight: bold; font-size: large">
                    <span class="counter" style="display: inline-block; width: 32%">
                        <asp:Label ID="LblTotProduct" runat="server" Text=""></asp:Label>
                    </span>
                </p>
                                </div>
                            </div>

                            <div class="polaroid" onclick="location.href='frmCustomerPartyGroup.aspx'" style="font-weight: bold; font-size: large; cursor: pointer; background-color: Tan">

                                <div class="container">
                                    Total Distributor Group
                <p style="font-weight: bold; font-size: large">
                    <span class="counter" style="display: inline-block; width: 32%">
                        <asp:Label ID="LblDistributorGroup" runat="server" Text=""></asp:Label>
                    </span>
                </p>
                                </div>
                            </div>

                            <div class="polaroid" onclick="location.href='frmCustomerPartyMaster.aspx'" style="font-weight: bold; font-size: large; cursor: pointer; background-color: Thistle">

                                <div class="container">
                                    Total Customer
                <p style="font-weight: bold; font-size: large">
                    <span class="counter" style="display: inline-block; width: 32%">
                        <asp:Label ID="LblTotDistributor" runat="server" Text=""></asp:Label>
                    </span>
                </p>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

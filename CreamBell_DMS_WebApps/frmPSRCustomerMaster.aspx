<%@ Page Title="PSR-Customer Master" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPSRCustomerMaster.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPSRCustomerMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />

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

    <style type="text/css">
        .input1 {
            width: 270px;
            height: 10px;
            padding: 10px 5px;
            float: left;
            border: 0;
            background: #eee;
            -moz-border-radius: 3px 0 0 3px;
            -webkit-border-radius: 3px 0 0 3px;
            border-radius: 3px 0 0 3px;
        }

            .input1:focus {
                outline: 0;
                background: #fff;
                -moz-box-shadow: 0 0 2px rgba(0,0,0,.8) inset;
                -webkit-box-shadow: 0 0 2px rgba(0,0,0,.8) inset;
                box-shadow: 0 0 2px rgba(0,0,0,.8) inset;
            }

            .input1::-webkit-input-placeholder {
                color: #999;
                font-weight: normal;
                font-style: italic;
            }

            .input1:-moz-placeholder {
                color: #999;
                font-weight: normal;
                font-style: italic;
            }

            .input1:-ms-input-placeholder {
                color: #999;
                font-weight: normal;
                font-style: italic;
            }
    </style>




    <style type="text/css">
        /*DropDownCss*/
        .ddl {
            background-color: #eeeeee;
            padding: 5px;
            border: 1px solid #7d6754;
            border-radius: 4px;
            padding: 3px;
            -webkit-appearance: none;
            background-image: url('Images/arrow-down-icon-black.png');
            background-position: right;
            background-repeat: no-repeat;
            text-indent: 0.01px; /*In Firefox*/
            text-overflow: ''; /*In Firefox*/
        }

            .ddl:hover {
                background: #add8e6;
                background-image: url('Images/arrow-down-icon-black.png');
                background-position: right;
                background-repeat: no-repeat;
                text-indent: 0.01px; /*In Firefox*/
                text-overflow: ''; /*In Firefox*/
            }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; color: white; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px;">PSR - Retailer Linking Master</span>
    </div>
   
    <table style="width: 100%">
        <tr>
            <td>
                <asp:ImageButton ID="imgBtnExportToExcel" runat="server" ImageUrl="~/Images/excel-24.ico" ToolTip="Click To Generate Excel Report" style="margin:0px 0px 0px 10px" OnClick="imgBtnExportToExcel_Click" />
            </td>
            <td style="width: 80%; text-align: right;">
                <asp:DropDownList ID="DDLSearchFilter" runat="server" CssClass="ddl" data-toggle="dropdown" Width="200" Style="margin-left: 0px" TabIndex="1">
                    <asp:ListItem>Retailer Name</asp:ListItem>
                    <asp:ListItem>Beat Name</asp:ListItem>
                    <asp:ListItem>PSR Name</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 20%">
                <div>
                    <asp:TextBox ID="txtSearch" runat="server" placeholder="Search here..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" TabIndex="2" />
                    <span id="span1" onmouseover="test()" onmouseout="test1()">
                        <asp:Button ID="BtnSearch" runat="server" Style="margin: 0px 0px 0px -2px" Text="Search" OnClick="BtnSearch_Click1" TabIndex="3"></asp:Button>&nbsp;&nbsp;
                    </span>
                </div>

            </td>
        </tr>
    </table>

    <div style="margin-left: 0px; height: 300px; width: 99%; overflow: auto">
 <asp:GridView ID="gvDetails" runat="server" GridLines="Horizontal" AutoGenerateColumns="False" Width="99%" BackColor="White"
            BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" OnPageIndexChanging="gvDetails_PageIndexChanging" TabIndex="4">

            <AlternatingRowStyle BackColor="#CCFFCC" />
            <Columns>
                <asp:BoundField HeaderText="Retailer Code" DataField="CustomerCode">
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                    <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Retailer Name" DataField="CustomerName">
                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                    <ItemStyle Width="150px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="PSR Code" DataField="PSRCode">
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                    <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="PSR Name" DataField="PSRName">
                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                    <ItemStyle Width="150px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Beat Code" DataField="BeatCode">
                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                    <ItemStyle Width="80px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Beat Name" DataField="BeatName">
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                    <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Beat Day" DataField="BeatDayDesc">
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                    <ItemStyle Width="100px" />
                </asp:BoundField>

            </Columns>
            <FooterStyle CssClass="table-condensed" BackColor="#B5C7DE" ForeColor="#4A3C8C" />
            <HeaderStyle BackColor="#05345C" Font-Bold="True" ForeColor="#F7F7F7" />
            <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Left" VerticalAlign="Middle" />
            <RowStyle BackColor="White" ForeColor="#4A3C8C" />
            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
            <SortedAscendingCellStyle BackColor="#F4F4FD" />
            <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
            <SortedDescendingCellStyle BackColor="#D8D8F0" />
            <SortedDescendingHeaderStyle BackColor="#3E3277" />
        </asp:GridView>

    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmDiscountMaster.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmDiscountMaster" %>

<%--<%@ Register TagPrefix="ob" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>--%>
<%--<%@ Register TagPrefix="ob" Namespace="Obout.SuperForm" Assembly="obout_SuperForm" %>--%>
<%@ Register TagPrefix="owd" Namespace="OboutInc.Window" Assembly="obout_Window_NET" %>
<%@ Register TagPrefix="ob" Namespace="Obout.Interface" Assembly="obout_Interface" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <asp:UpdateProgress ID="upprogress" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
        <ProgressTemplate>
            <div class="overlay"></div>
            <div class="overlayContent">
                <center>
                    Please Wait...don't close this window until processing is being done.
                     <br />
                    <img src="../../IMAGES/bar.gif" alt="" />
                </center>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div style="border-bottom-style: solid; border-bottom-width: 5px; border-bottom-color: #000066">
        <table>
            <tr>
                <td style="padding: 10px" class="auto-style1">
                    <asp:Button ID="Button1" runat="server" Text="New" CssClass="button" Height="31px" />
                </td>
                <td style="padding: 0px 0px 0px 300px;" class="auto-style1">
                    <asp:DropDownList ID="drpSearch" runat="server" CssClass="ddl" Width="200" Style="margin-left: 0px">
                        <asp:ListItem>Discount Pattern</asp:ListItem>
                        <asp:ListItem>State</asp:ListItem>
                        <asp:ListItem>Discount Type</asp:ListItem>
                        <asp:ListItem>Customer Group</asp:ListItem>
                        <asp:ListItem>Customer/Party</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="TextBox4" runat="server" CssClass="input1 cf" placeholder="Search here..." />
                        <span id="span1" class="arrow_box" onmouseover="test()" onmouseout="test1()">
                            <asp:Button ID="Button4" runat="server" CssClass="button1 cf" Style="margin: 0px 0px 0px -2px" Text="Search"></asp:Button>
                        </span>
                    </div>

                </td>
            </tr>
        </table>
    </div>
    <div style="width: 98%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px;">Discount Master</span>
    </div>
    <div class="form-style-6">
        <table style="width: 50%; border-spacing: 0px">
            <tr>
                <td>Discount Pattern</td>
                <td>
                    <asp:DropDownList ID="drpDiscountPattern" runat="server" Width="200" Style="margin-left: 0px"></asp:DropDownList></td>
                <td class="tdpadding">&nbsp;</td>
            </tr>
            <tr>
                <td>State</td>
                <td>
                    <asp:DropDownList ID="drpState" runat="server" Width="200" Style="margin-left: 0px"></asp:DropDownList></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Discount Type</td>
                <td>
                    <asp:DropDownList ID="drpDiscountType" runat="server" Width="200" Style="margin-left: 0px"></asp:DropDownList></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Customer Group</td>
                <td>
                    <asp:DropDownList ID="drpCustomerGroup" runat="server" Width="200" Style="margin-left: 0px"></asp:DropDownList></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Customer/Party</td>
                <td>
                    <asp:DropDownList ID="drpCustomer" runat="server" Width="200" Style="margin-left: 0px"></asp:DropDownList></td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="772px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="2" ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="3">
            <Columns>

                <asp:TemplateField HeaderText="Material Group">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Material_Group") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList1" runat="server">
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Discount%" DataField="Discount" />
                <asp:BoundField HeaderText="DiscValue" DataField="DiscValue" />
                <asp:BoundField HeaderText="FromDate" DataField="FromDate" />
                <asp:BoundField HeaderText="ToDate" DataField="ToDate" />

            </Columns>
            <EmptyDataTemplate>
                No Record Found...
            </EmptyDataTemplate>
            <FooterStyle BackColor="#bfbfbf" />
            <HeaderStyle BackColor="#bfbfbf" ForeColor="#000000" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#007DBB" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#00547E" />
        </asp:GridView>
        <br />
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="772px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="2" ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="3">
            <Columns>

                <asp:TemplateField HeaderText="Material Group">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Material_Group") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList2" runat="server">
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Material Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Material_Name") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList2" runat="server">
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Material Group">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Material_Code") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList2" runat="server">
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Discount%" DataField="Discount" />
                <asp:BoundField HeaderText="DiscValue" DataField="DiscValue" />
                <asp:BoundField HeaderText="FromDate" DataField="FromDate" />
                <asp:BoundField HeaderText="ToDate" DataField="ToDate" />

            </Columns>
            <EmptyDataTemplate>
                No Record Found...
            </EmptyDataTemplate>
            <FooterStyle BackColor="#bfbfbf" />
            <HeaderStyle BackColor="#bfbfbf" ForeColor="#000000" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#007DBB" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#00547E" />
        </asp:GridView>
        <br />
        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="True" ShowFooter="True" Width="998px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
            <Columns>
                <asp:BoundField HeaderText="Discount Pattern" DataField="Customer Name" />
                <asp:BoundField HeaderText="Discount Type" DataField="Test1" />
                <asp:BoundField HeaderText="Coustomer Group" DataField="Test1" />
                <asp:BoundField HeaderText="Customer/Party" DataField="Test1" />

            </Columns>
            <EmptyDataTemplate>
                No Record Found...
            </EmptyDataTemplate>
            <FooterStyle BackColor="#bfbfbf" />
            <HeaderStyle BackColor="#bfbfbf" ForeColor="#000000" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#007DBB" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#00547E" />
        </asp:GridView>
    </div>


</asp:Content>

﻿<%@ Page Title="Purchase Indent List" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPurchaseIndentList.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPurchaseIndentList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="css/btnSearch.css" rel="stylesheet" />

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

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gvHeader.ClientID%>').clone(true);
                /*Code to remove first ror which is header row*/
                $(gridHeader).find("tr:gt(0)").remove();
                $('#<%=gvHeader.ClientID%> tr th').each(function (i) {
                 /* Here Set Width of each th from gridview to new table th */
                 $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
             });
             $("#controlHead1").append(gridHeader);
             $('#controlHead1').css('position', 'absolute');
             $('#controlHead1').css('top', '129');

            });

         $(document).ready(function () {
             /*Code to copy the gridview header with style*/
             var gridLine = $('#<%=gvLineDetails.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridLine).find("tr:gt(0)").remove();
            $('#<%=gvLineDetails.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridLine).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead2").append(gridLine);
            $('#controlHead2').css('position', 'absolute');
            $('#controlHead2').css('top', '129');

        });
    </script>

    <style type="text/css">
        .ddl {
            background-image: url('Images/arrow-down-icon-black.png');
        }

            .ddl:hover {
                background-image: url('Images/arrow-down-icon-black.png');
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">



    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #0085ca; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px; color: #FFFFFF;">Purchase Indent List</span>
    </div>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table style="width: 99%">
                <tr>
                    <td style="padding: 10px">&nbsp;</td>
                    <td style="text-align: right;">
                        <%--<asp:Label ID="lblSearch" runat="server" Text="Indent No  :" style="font-size: 16px;" ></asp:Label>--%>
                   
                    </td>
                    <td style="text-align: right">
                        <div>
                            <asp:DropDownList ID="ddlSerch" runat="server" CssClass="ddl" data-toggle="dropdown" Width="200" Style="margin-left: 0px">
                                <asp:ListItem Text="Indent No" Value="Indent_No"></asp:ListItem>
                                <asp:ListItem Text="SO No" Value="SO_No"></asp:ListItem>
                                <asp:ListItem Text="Invoice No" Value="Invoice_No"></asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;&nbsp;
	                    <asp:TextBox ID="txtSearch" runat="server" placeholder="Search here..." CssClass="textboxStyleNew" />
                            <span id="span1" onmouseover="test()" onmouseout="test1()">
                                <asp:Button ID="btn2" runat="server" Style="margin: 0px 0px 0px -2px" Text="Search" OnClick="btn2_Click" CssClass="textboxStyleNew"></asp:Button>
                            </span>
                        </div>

                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>


    <%--<div style="overflow:auto;height:200px;margin: 10px 0px 0px 10px;" > --%>

    <div id="controlHead1" style="margin-top: 5px; margin-left: 5px; padding-right: 10px;"></div>
    <div style="height: 300px; overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px;">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                <asp:GridView runat="server" ID="gvHeader" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True"
                    OnRowDataBound="gvHeader_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Indent No">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtn" runat="server" Text='<%# Bind("Indent_No") %>' OnClick="lnkbtn_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Indent Date">
                            <ItemTemplate>
                                <asp:Label ID="Indent_Date" runat="server" Text='<%# Eval("Indent_Date")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delivery Date">
                            <ItemTemplate>
                                <asp:Label ID="Delivery_Date" runat="server" Text='<%# Eval("Required_Date")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Plant Code">
                            <ItemTemplate>
                                <asp:Label ID="SALEOFFICE_CODE" runat="server" Text='<%# Eval("SALEOFFICE_CODE")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Plant Name">
                            <ItemTemplate>
                                <asp:Label ID="SALEOFFICE_NAME" runat="server" Text='<%# Eval("SALEOFFICE_NAME")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IndentQty(Box)">
                            <ItemTemplate>
                                <asp:Label ID="Box" runat="server" Text='<%# Eval("Box")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IndentQty(Crates)">
                            <ItemTemplate>
                                <asp:Label ID="Crates" runat="server" Text='<%# Eval("Crates")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Indent Qty(Ltr)">
                            <ItemTemplate>
                                <asp:Label ID="Ltr" runat="server" Text='<%# Eval("Ltr")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Indent SO">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnso" runat="server" Text='<%# Bind("SO_No") %>' OnClick="lnkbtnso_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Indent Invoice">
                          <ItemTemplate>
                                <asp:LinkButton ID="lnkbtninv" runat="server" Text='<%# Bind("INVOICE_No") %>' OnClick="lnkbtninv_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Confirm">
                            <ItemTemplate>
                            <%--<asp:Label ID="Confirm" runat="server" Text='<%# Eval("Confirm")%>'></asp:Label>--%>
                            <asp:LinkButton ID="Status" runat="server" Text='<%# Eval("Confirm")%>' OnClick="Status_Click" CommandArgument='<%#Eval ("Indent_No") %>'></asp:LinkButton>
                             </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>


                    </Columns>
                    <EmptyDataTemplate>
                        No Record Found...
                    </EmptyDataTemplate>
                    <%-- <FooterStyle BackColor="#bfbfbf" />
             <HeaderStyle BackColor="#bfbfbf"  ForeColor="#000000" />
             <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
             <RowStyle ForeColor="#000066" />
             <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
             <SortedAscendingCellStyle BackColor="#F1F1F1" />
             <SortedAscendingHeaderStyle BackColor="#007DBB" />
             <SortedDescendingCellStyle BackColor="#CAC9C9" />
             <SortedDescendingHeaderStyle BackColor="#00547E" />--%>
                    <%-- <FooterStyle CssClass="table-condensed" BackColor="#B5C7DE" ForeColor="#4A3C8C" />--%>
                    <HeaderStyle BackColor="#05345C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <RowStyle BackColor="White" ForeColor="#4A3C8C" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <SortedAscendingCellStyle BackColor="#F4F4FD" />
                    <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                    <SortedDescendingCellStyle BackColor="#D8D8F0" />
                    <SortedDescendingHeaderStyle BackColor="#3E3277" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--<div style="overflow:auto;height:250px;margin: 10px 0px 0px 10px;" > --%>

    <div id="controlHead2" style="margin-top: 5px; margin-left: 3px; padding-right: 10px; width: 99%;"></div>
    <div style="height: 300px; overflow: auto; margin-top: 3px; margin-left: 5px; padding-right: 10px; width: 99%;">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <%--<asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" ShowFooter="True"  BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" EnableSortingAndPagingCallbacks="True">--%>
                <asp:GridView runat="server" ID="gvLineDetails" GridLines="Horizontal" AutoGenerateColumns="False" Width="99%" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                    <Columns>
                        <asp:TemplateField HeaderText="Indent No" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="Indent_No" runat="server" Text='<%#  Eval("Indent_No") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Line No" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="Line_No" runat="server" Text='<%#  Eval("Line_No") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Group" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="PRODUCT_GROUP" runat="server" Text='<%# Eval("PRODUCT_GROUP")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Code" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="PRODUCT_CODE" runat="server" Text='<%# Eval("PRODUCT_CODE")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Name" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="Product_Name" runat="server" Text='<%# Eval("Product_Name")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Indent Qty(Box)" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="Box" runat="server" Text='<%# Eval("Box")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Indent Qty(Crates)" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="Crates" runat="server" Text='<%# Eval("Crates")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Indent Qty(Ltr)" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="Ltr" runat="server" Text='<%# Eval("Ltr")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>


                    </Columns>
                    <EmptyDataTemplate>
                        No Record Found...
                    </EmptyDataTemplate>
                    <%-- <FooterStyle BackColor="#bfbfbf" />
             <HeaderStyle BackColor="#bfbfbf"  ForeColor="#000000" />
             <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
             <RowStyle ForeColor="#000066" />
             <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
             <SortedAscendingCellStyle BackColor="#F1F1F1" />
             <SortedAscendingHeaderStyle BackColor="#007DBB" />
             <SortedDescendingCellStyle BackColor="#CAC9C9" />
             <SortedDescendingHeaderStyle BackColor="#00547E" />--%>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


</asp:Content>
<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPurchReceiptList.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPurchReceiptList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        /*DropDownCss*/
        .ddl {
            background-image: url('Images/arrow-down-icon-black.png');
        }

            .ddl:hover {
                background-image: url('Images/arrow-down-icon-black.png');
            }

        .auto-style1 {
            width: 5px;
        }
    </style>


    <script type="text/javascript">
        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=GridHeaderPurchList.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=GridHeaderPurchList.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead").append(gridHeader);
            $('#controlHead').css('position', 'absolute');
            $('#controlHead').css('top', '129');


        });


        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=GridView2.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=GridView2.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead1").append(gridHeader);
            $('#controlHead1').css('position', 'absolute');
            $('#controlHead1').css('top', '129');


        });


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="sasa" runat="server">
        <ContentTemplate>



            <div style="width: 99%; height: 18px; border-radius: 4px; margin: 4px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
                <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; color: white; margin: 6px 0px 0px 10px;">Purchase Invoice Receipt List [POSTED]</span>
            </div>

            <div style="width: 100%">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>

                        <table style="width: 100%; vertical-align: text-top">
                            <tr>
                                <td style="width: 50%"></td>

                                <td style="text-align: right; width: 15%"></td>
                                <td style="text-align: right; width: 10%">
                                    <asp:DropDownList ID="ddlSearch" runat="server" CssClass="textboxStyleNew" data-toggle="dropdown" Width="200px">
                                        <asp:ListItem>Receipt No</asp:ListItem>
                                        <asp:ListItem>Invoice No</asp:ListItem>
                                    </asp:DropDownList>



                                </td>
                                <td style="text-align: right; width: 25%">
                                    <asp:TextBox ID="txtSerch" runat="server" placeholder="Search here..." Height="16px" CssClass="textboxStyleNew" />
                                    <span id="span1" style="text-align: right">
                                        <asp:Button ID="btn2" runat="server" CssClass="ReportSearch" Text="Search" OnClick="btn2_Click"></asp:Button>
                                    </span>

                                </td>
                            </tr>
                        </table>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div id="controlHead" style="margin-top: 5px; margin-left: 5px; padding-right: 10px;"></div>
            <div style="height: 450px; overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px;">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="GridHeaderPurchList" runat="server" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                            BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                            <Columns>

                                <asp:TemplateField HeaderText="RECEIPT NO">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtn" runat="server" Text='<%# Bind("DOCUMENT_NO") %>' OnClick="lnkbtn_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="DOCUMENT_DATE" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="RECEIPT DATE">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:BoundField DataField="MATERIAL_VALUE" DataFormatString="₹ {0:n2}" HeaderText="VALUE">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="INDENT NO" DataField="PURCH_INDENTNO">
                                    <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                    <ItemStyle Width="80px" HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="INDENT DATE" DataField="PURCH_INDENTDATE" DataFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                    <ItemStyle Width="80px" HorizontalAlign="Left" />
                                </asp:BoundField>

                                <%-- <asp:BoundField DataField="DOCUMENT_NO" HeaderText="RECEIPT NO" >
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>--%>

                                <asp:BoundField DataField="SALE_INVOICENO" HeaderText="INVOICE NO">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:BoundField DataField="GSTINNO" HeaderText="GST INVOICE NO">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:BoundField DataField="SALE_INVOICEDATE" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="INVOICE DATE">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:BoundField DataField="CRATES" DataFormatString="{0:n2}" HeaderText="CRATES">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField DataField="BOX" DataFormatString="{0:n2}" HeaderText="BOX">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField DataField="LTR" DataFormatString="{0:n2}" HeaderText="LTR">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField DataField="REFERENCENUMBER" HeaderText="PREDOCUMENT NO">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                </asp:BoundField>

                            </Columns>
                            <EmptyDataTemplate>
                                No Record Found...
                            </EmptyDataTemplate>
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
            <br />

            <div id="controlHead1" style="margin-top: 18px; margin-left: 5px; padding-right: 10px;"></div>
            <div style="height: 250px; overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px;">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" GridLines="Horizontal" Width="100%" BackColor="White"
                            BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">

                            <Columns>
                                <asp:BoundField HeaderText="RECIEPT NO" DataField="PURCH_RECIEPTNO">
                                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                </asp:BoundField>

                                <asp:BoundField DataField="PRODUCT_GROUP" HeaderText="PRODUCTGROUP">
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="PCODE/NAME" DataField="PRODUCTDESC">
                                    <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                    <ItemStyle Width="215px" HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="BOX" DataField="BOX" DataFormatString="{0:n2}">
                                    <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                    <ItemStyle Width="110px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="CRATES" DataField="CRATES" DataFormatString="{0:n2}">
                                    <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                    <ItemStyle Width="110px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="LTR" DataField="LTR" DataFormatString="{0:n2}">
                                    <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                    <ItemStyle Width="110px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="MRP" DataField="PRODUCT_MRP" DataFormatString="{0:n2}">
                                    <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                    <ItemStyle Width="110px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="RATE" DataField="RATE" DataFormatString="{0:n2}">
                                    <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="BASIC-VALUE" DataField="BASICVALUE" DataFormatString="{0:n2}">
                                    <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="TRD DISC%" DataField="TRDDISCPERC" DataFormatString="{0:n2}">
                                    <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="TRD VALUE" DataField="TRDDISCVALUE" DataFormatString="{0:n2}">
                                    <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="PRICE EQUAL." DataField="PRICE_EQUALVALUE" DataFormatString="{0:n2}">
                                    <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="VAT INC%" DataField="VAT_INC_PERC" DataFormatString="{0:n2}">
                                    <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="VAT VALUE" DataField="VAT_INC_PERCVALUE" DataFormatString="{0:n2}">
                                    <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="GROSS" DataField="GROSSRATE" DataFormatString="{0:n2}">
                                    <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="NET VALUE" DataField="AMOUNT" DataFormatString="{0:n2}">
                                    <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>

                            </Columns>
                            <EmptyDataTemplate>
                                No Record Found...
                            </EmptyDataTemplate>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

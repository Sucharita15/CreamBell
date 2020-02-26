<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPurchUnPostList.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPurchUnPostList" %>


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
            width: 924px;
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

    </script>


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <asp:UpdatePanel runat="server">
        <ContentTemplate>


            <div style="width: 99%; height: 18px; border-radius: 4px; margin: 4px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
                <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; color: white; margin: 6px 0px 0px 10px;">Purchase Invoice Receipt List [UNPOSTED] </span>
            </div>
            <div>
                <table style="width: 100%">
                    <tr>

                        <td style="width: 50%"></td>
                        <td style="text-align: right; width: 15%"></td>
                        <td style="text-align: right; width: 10%">

                            <asp:DropDownList ID="ddlSearch" runat="server" CssClass="textboxStyleNew">
                                <asp:ListItem>Receipt No</asp:ListItem>
                                <asp:ListItem>Invoice No</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right; width: 25%">
                            <asp:TextBox ID="txtSerch" runat="server" placeholder="Search here..." Height="16px" CssClass="textboxStyleNew" />
                            <span id="span1" style="text-align: right">
                                <asp:Button ID="btn2" runat="server" CssClass="textboxStyleNew" Style="margin: 0px 0px 0px -2px" Text="Search" OnClick="btn2_Click"></asp:Button>
                            </span>
                        </td>
                    </tr>
                </table>
            </div>



            <div id="controlHead" style="margin-top: 4px; margin-left: 5px; padding-right: 10px;"></div>
            <div style="height: 455px; overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px;">

                <asp:GridView ID="GridHeaderPurchList" runat="server" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True"
                    OnRowDataBound="GridHeaderPurchList_RowDataBound">
                    <Columns>

                        <asp:TemplateField HeaderText="RECEIPT NO">
                            <ItemTemplate>
                                <asp:LinkButton ID="LnkBtnDocumnetNo" runat="server" Text='<%# Bind("DOCUMENT_NO") %>' OnClick="LnkBtnDocumnetNo_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="DOCUMENT_DATE" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="RECEIPT DATE">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="MATERIAL_VALUE" DataFormatString="₹ {0:n2}" HeaderText="VALUE">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="INDENT NO" DataField="PURCH_INDENTNO">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="INDENT DATE" DataField="PURCH_INDENTDATE" DataFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="SALE_INVOICENO" HeaderText="INVOICE NO">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="GSTINNo" HeaderText="GST INVOICE NO">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="SALE_INVOICEDATE" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="INVOICE DATE">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="STATUS" HeaderText="STATUS">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
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
            </div>


            <%--<div id="controlHead" style="margin-top:4px; margin-left:5px;padding-right:10px;"></div>
        <div style="height:115px;overflow: auto;margin-top:5px; margin-left:5px;padding-right:10px;">

        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" ShowFooter="True" 
             BackColor="White" BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
            HeaderStyle-CssClass="FixedHeader" HeaderStyle-BackColor="YellowGreen" 
             ShowHeaderWhenEmpty="True" EnableSortingAndPagingCallbacks="True" Width="1050px" ShowHeader="true" Height="10px">            
            
            <Columns>
                <asp:BoundField HeaderText="RECIEPT NO" DataField="PURCH_RECIEPTNO"  >
                <HeaderStyle HorizontalAlign="Left" Width="150px"/>
                <ItemStyle HorizontalAlign="Left" Width="150px" />
                </asp:BoundField>

                <asp:BoundField DataField="PRODUCT_GROUP" HeaderText="PRODUCTGROUP" >
                <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                <ItemStyle HorizontalAlign="Left" Width="100px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="PCODE/NAME" DataField="PRODUCTDESC" >
                <HeaderStyle Width="200px" HorizontalAlign="Left" />
                <ItemStyle Width="200" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="BOX" DataField="BOX" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField HeaderText="CRATES" DataField="CRATES" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField HeaderText="LTR"  DataField="LTR" DataFormatString="{0:n2}">
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                 <asp:BoundField HeaderText="MRP"  DataField="PRODUCT_MRP" DataFormatString="{0:n2}">
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                 <asp:BoundField HeaderText="RATE"  DataField="RATE" DataFormatString="{0:n2}">
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField HeaderText="BASIC-VALUE"  DataField="BASICVALUE" DataFormatString="{0:n2}">
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField HeaderText="TRD DISC%"  DataField="TRDDISCPERC" DataFormatString="{0:n2}">
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField HeaderText="TRD VALUE"  DataField="TRDDISCVALUE" DataFormatString="{0:n2}">
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField HeaderText="PRICE EQUAL."  DataField="PRICE_EQUALVALUE" DataFormatString="{0:n2}">
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField HeaderText="VAT INC%"  DataField="VAT_INC_PERC" DataFormatString="{0:n2}">
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField HeaderText="VAT VALUE"  DataField="VAT_INC_PERCVALUE" DataFormatString="{0:n2}">
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField HeaderText="GROSS"  DataField="GROSSRATE" DataFormatString="{0:n2}">
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField HeaderText="NET VALUE"  DataField="AMOUNT" DataFormatString="{0:n2}">
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

            </Columns>
            <EmptyDataTemplate>
                 No Record Found...
            </EmptyDataTemplate>
            <FooterStyle CssClass="table-condensed" BackColor="White" ForeColor="#4A3C8C" />
            <HeaderStyle BackColor="#05345C" Font-Bold="True" ForeColor="#F7F7F7" Font-Size="XX-Small" /> 
            <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Left" VerticalAlign="Middle" />
            <RowStyle BackColor="White" ForeColor="#4A3C8C" />
            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
            <SortedAscendingCellStyle BackColor="#F4F4FD" />
            <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
            <SortedDescendingCellStyle BackColor="#D8D8F0" />
            <SortedDescendingHeaderStyle BackColor="#3E3277" />
        </asp:GridView>
        

       </div> --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

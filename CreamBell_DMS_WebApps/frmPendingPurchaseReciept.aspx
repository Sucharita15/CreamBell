<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPendingPurchaseReciept.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPendingPurchaseReciept" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script type="text/javascript">

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gvHeader.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=gvHeader.ClientID%> tr th').each(function (i) {
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
    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px; color: #FFFFFF;">
        &nbsp;&nbsp;&nbsp;
         Pending Purchase Reciept
    </div>
    <div>

        <table style="width: 100%">
            <tr>
                <td style="width: 60%">
                    <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="#FF3300"></asp:Label>
                </td>
                <td style="width: 20%; text-align: right">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="DDLSearchType" runat="server" CssClass="ddl" data-toggle="dropdown" Width="200" Style="margin-left: 0px">
                                <asp:ListItem>Invoice_No</asp:ListItem>
                                <asp:ListItem>Invoice Date</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>

                    </asp:UpdatePanel>
                </td>
                <td style="width: 20%; text-align: right">
                    <asp:UpdatePanel runat="server">
                      
                        <ContentTemplate>

                            <div>

                                <asp:TextBox ID="txtSearch" runat="server" placeholder="Search here..." />

                                <asp:Button ID="btnSearchCustomer" runat="server" Style="margin: 0px 0px 0px -2px" Text="Search" OnClick="btnSearchCustomer_Click"></asp:Button>

                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>

            </tr>
        </table>


    </div>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <table style="width: 100%">
                    <tr>
                        <td>

                            <asp:GridView ID="gvHeader" runat="server" AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3"
                                ShowHeaderWhenEmpty="True">
                                <AlternatingRowStyle BackColor="#CCFFCC" />
                                <Columns>
                                    <asp:TemplateField HeaderText="InvoiceNo">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtn" runat="server" Text='<%# Bind("INVOICE_NO") %>' OnClick="lnkbtn_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Invoice_Date ">

                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("INVOIC_DATE", "{0:dd MMM yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="INVOICE_VALUE" HeaderText="INVOICE_VALUE" DataFormatString="{0:n2}">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="DistCode" HeaderText="DistCode">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DistName" HeaderText="Dist Name">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>

                                </Columns>
                                <EmptyDataTemplate>
                                    No Record Found...
                                </EmptyDataTemplate>
                                <FooterStyle CssClass="table-condensed" BackColor="White" ForeColor="#4A3C8C" />
                                <HeaderStyle BackColor="#05345C" Font-Bold="True" ForeColor="#F7F7F7" />
                                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <RowStyle BackColor="White" ForeColor="#4A3C8C" />
                                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                <SortedAscendingCellStyle BackColor="#F4F4FD" />
                                <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                                <SortedDescendingCellStyle BackColor="#D8D8F0" />
                                <SortedDescendingHeaderStyle BackColor="#3E3277" />
                            </asp:GridView>
                        </td>

                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>


</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPurchaseRegister.aspx.cs"
    EnableEventValidation="false" Inherits="CreamBell_DMS_WebApps.frmPurchaseRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/ucRoleFilters.ascx" TagPrefix="uc1" TagName="ucRoleFilters" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gridPurchaseRegister.ClientID%>').clone(true);
                /*Code to remove first ror which is header row*/
                $(gridHeader).find("tr:gt(0)").remove();
                $('#<%=gridPurchaseRegister.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead2").append(gridHeader);
            $('#controlHead2').css('position', 'absolute');
            $('#controlHead2').css('top', '129');

            });

    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="imgBtnExportToExcel" />
            <asp:PostBackTrigger ControlID="BtnSearch" />
            <asp:PostBackTrigger ControlID="ucRoleFilters" />
        </Triggers>
        <ContentTemplate>
            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
                Purchase Register
            </div>
            <uc1:ucRoleFilters runat="server" ID="ucRoleFilters" />
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
            <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
            <div id="Filter" style="width: 99%; border-radius: 1px; margin: 10px 0px 0px 5px; color: black; padding: 2px 0px 0px 0px; border-style: groove">
                <table style="width: 90%; text-align: center">
                    <tr>
                        <td colspan="7">
                            <asp:Label ID="LblMessage" runat="server" Font-Bold="true" Font-Names="Seoge UI" ForeColor="DarkBlue" Text=""> </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style5">
                            <asp:RadioButton ID="rdoDetailedView" runat="server" AutoPostBack="true" Text="Detailed View" Checked="true" OnCheckedChanged="rdoDetailedView_CheckedChanged"
                                GroupName="radio" />
                        </td>
                        <td class="auto-style5">
                            <asp:RadioButton ID="rdoSummarizedView" runat="server" AutoPostBack="true" Text="Summarised View" OnCheckedChanged="rdoDetailedView_CheckedChanged"
                                GroupName="radio" />
                        </td>
                        <td>From Date :</td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" Enabled="true" placeholder="dd-MMM-yyyy"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" runat="server" ImageAlign="Bottom" ImageUrl="~/Images/calendar.jpg" />
                        </td>
                        <td>To Date :</td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" Enabled="true" placeholder="dd-MMM-yyyy"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" runat="server" ImageAlign="Bottom" ImageUrl="~/Images/calendar.jpg" />
                        </td>
                        <td>
                            <asp:Button ID="BtnSearch" runat="server" CssClass="ReportSearch" OnClick="BtnSearch_Click" Text="Search" Width="80px" />
                        </td>
                        <td>
                            <asp:ImageButton ID="imgBtnExportToExcel" runat="server" ImageUrl="~/Images/excel-24.ico" OnClick="imgBtnExportToExcel_Click" ToolTip="Click To Generate Excel Report" />
                        </td>

                    </tr>
                </table>
            </div>
            <%--<div id="controlHead2" style="margin-top:5px; margin-left:5px;padding-right:10px;width: 1720px;"></div>--%>
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; table-layout: fixed;">
                <tr>
                    <td valign="top" style="width: 100%" align="left">
                        <div style="overflow: auto; margin-top: 5px; margin-left: 5px; width: 99%; height: 430px; table-layout: fixed;">
                            <asp:GridView runat="server" ID="gridPurchaseRegister" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="True" Width="105%" BackColor="White"
                                HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="Right"
                                BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                                <AlternatingRowStyle BackColor="#CCFFCC" />
                                <%--<Columns>
                <asp:BoundField HeaderText="Receipt No" DataField="DOCUMENT_NO" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>
               <asp:BoundField DataField="RECEIPTDATE" HeaderText="Receipt Date" >
                <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                <ItemStyle HorizontalAlign="Left" Width="80px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Invoice No" DataField="SALE_INVOICENO" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Invoice Date" DataField="SALE_INVOICEDATE" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Indent No" DataField="PURCH_INDENTNO" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="ProductGroup" DataField="PRODUCT_GROUP" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="ProductCode" DataField="PRODUCT_Code" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Product" DataField="PRODUCT" >
                <HeaderStyle Width="200px" HorizontalAlign="Left" />
                <ItemStyle Width="200px" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="CRATES" HeaderText="Qty(Crates)" DataFormatString="{0:n2}" >
                <HeaderStyle HorizontalAlign="Right" Width="80px" />
                <ItemStyle HorizontalAlign="Right" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>                
                <asp:BoundField HeaderText="Qty(LTR)" DataField="LTR" DataFormatString="{0:n2}" >
                <HeaderStyle HorizontalAlign="Right" Width="80px"/>
                <ItemStyle HorizontalAlign="Right" Width="80px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Qty(Box)" DataField="BOXQTY" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Qty(PCS)" DataField="PCSQty" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Qty(BoxPCS)" DataField="TotalBoxPCS" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField HeaderText="FOCQty" DataField="FOCQTY" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="TotalQty" DataField="TotalQty" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="BASICVALUE" DataField="BASICVALUE" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                 <asp:BoundField HeaderText="TRADDISCPERC" DataField="TRDDISCPERC" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="TRADDISCVALUE" DataField="TRDDISCVALUE" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="PRICE_EQUALVALUE" DataField="PRICE_EQUALVALUE" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="TAX %" DataField="TAX" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                 <asp:BoundField HeaderText="TAXAMOUNT" DataField="TAXAMOUNT" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="AMOUNT" DataField="AMOUNT" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
          </Columns>--%>
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
                            <br />
                            <br />
                            <asp:GridView runat="server" ID="gridpurchaseregisterbottom" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="True" Width="98%" BackColor="White"
                                HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="Right"
                                BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" Visible="false">
                                <AlternatingRowStyle BackColor="#CCFFCC" />
                                <%--<Columns>
                <asp:BoundField HeaderText="Receipt No" DataField="DOCUMENT_NO" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>
               <asp:BoundField DataField="RECEIPTDATE" HeaderText="Receipt Date" >
                <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                <ItemStyle HorizontalAlign="Left" Width="80px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Invoice No" DataField="SALE_INVOICENO" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Invoice Date" DataField="SALE_INVOICEDATE" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Indent No" DataField="PURCH_INDENTNO" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="ProductGroup" DataField="PRODUCT_GROUP" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="ProductCode" DataField="PRODUCT_Code" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Product" DataField="PRODUCT" >
                <HeaderStyle Width="200px" HorizontalAlign="Left" />
                <ItemStyle Width="200px" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="CRATES" HeaderText="Qty(Crates)" DataFormatString="{0:n2}" >
                <HeaderStyle HorizontalAlign="Right" Width="80px" />
                <ItemStyle HorizontalAlign="Right" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>                
                <asp:BoundField HeaderText="Qty(LTR)" DataField="LTR" DataFormatString="{0:n2}" >
                <HeaderStyle HorizontalAlign="Right" Width="80px"/>
                <ItemStyle HorizontalAlign="Right" Width="80px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Qty(Box)" DataField="BOXQTY" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Qty(PCS)" DataField="PCSQty" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Qty(BoxPCS)" DataField="TotalBoxPCS" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField HeaderText="FOCQty" DataField="FOCQTY" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="TotalQty" DataField="TotalQty" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="BASICVALUE" DataField="BASICVALUE" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                 <asp:BoundField HeaderText="TRADDISCPERC" DataField="TRDDISCPERC" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="TRADDISCVALUE" DataField="TRDDISCVALUE" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="PRICE_EQUALVALUE" DataField="PRICE_EQUALVALUE" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="TAX %" DataField="TAX" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                 <asp:BoundField HeaderText="TAXAMOUNT" DataField="TAXAMOUNT" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="AMOUNT" DataField="AMOUNT" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
          </Columns>--%>
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
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

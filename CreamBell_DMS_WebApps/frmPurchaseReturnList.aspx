<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPurchaseReturnList.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPurchaseReturnList" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
    <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gridLineDetails.ClientID%>').clone(true);
           /*Code to remove first ror which is header row*/
           $(gridHeader).find("tr:gt(0)").remove();
           $('#<%=gridLineDetails.ClientID%> tr th').each(function (i) {
                 /* Here Set Width of each th from gridview to new table th */
                 $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
             });
             $("#controlHead").append(gridHeader);
             $('#controlHead').css('position', 'absolute');
             $('#controlHead').css('top', '129');

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
    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px; text-align:center">
        <span style="font-family: Segoe UI; color: white; font-size: 11px; font-weight: bold; margin: 0px 0px 0px 10px;">Purchase Return List</span>
    </div>

     <div >
        <table style="width:100%">
            <tr>
                <td style="text-align: right">
                    
                    <asp:DropDownList ID="DDLSearchFilter" runat="server" CssClass="textboxStyleNew" data-toggle="dropdown" Width="200"
                        Style="margin-left: 0px">
                        <asp:ListItem>Receipt Number</asp:ListItem>
                        <asp:ListItem>Receipt Date</asp:ListItem>
                        <asp:ListItem>Return Number</asp:ListItem>
                        <asp:ListItem>Return Date</asp:ListItem>
                    </asp:DropDownList>
                &nbsp;:
                        <asp:TextBox ID="txtSearch" runat="server" placeholder="Search here..." Height="16px" CssClass="textboxStyleNew" />
                        <span id="span1" style="text-align: right">
                             <asp:Button ID="BtnSearch" runat="server" CssClass="textboxStyleNew" Style="margin: 0px 0px 0px -2px" Text="Search" OnClick="BtnSearch_Click"></asp:Button>
                        </span>
                    
                </td>
            </tr>
        </table>
    </div>
    

    
    <div class="form-style-6" style="margin-left: 0px; width: 95%; overflow: auto; text-align: left">

        <asp:GridView ID="GridHeaderDetail" runat="server" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" Width="95%" BackColor="White"
            BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" OnPageIndexChanging="GridHeaderDetail_PageIndexChanging" PageSize="10" AllowPaging="True">

            <AlternatingRowStyle BackColor="#F7F7F7" />
            <Columns>
                <asp:TemplateField HeaderText="ReturnNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="LnkReturnNo" runat="server" Text='<%# Bind("PURCH_RETURNNO") %>' OnClick="LnkReturnNo_Click"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle Font-Bold="True" HorizontalAlign="Left" />
                </asp:TemplateField>

                <asp:BoundField HeaderText="Return Date" DataField="PURCH_RETURNDATE" DataFormatString="{0:dd/MMM/yyyy}">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="ReceiptNo" DataField="PURCH_RECIEPTNO">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Receipt Date" DataField="PURCH_RECIEPTDATE" DataFormatString="{0:dd/MMM/yyyy}">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Invoice No" DataField="SALE_INVOICENO">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Return Amount" DataField="TOTAMOUNT" DataFormatString="₹ {0:n}">
                    <HeaderStyle HorizontalAlign="Left" />
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
    
    <div id="controlHead" style="margin-top: 5px; margin-left: 0px; padding-right: 10px; text-align: left;" ></div>
    <div style="height: 200px; overflow: auto; margin-top: 5px; margin-left: 0px; padding-right: 10px; text-align: left;" >

        <asp:GridView runat="server" ID="gridLineDetails" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
            BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">


            <AlternatingRowStyle BackColor="#F7F7F7" />
            <Columns>

                <asp:BoundField HeaderText="Return Number" DataField="PURCH_RETURNNO">
                    <HeaderStyle HorizontalAlign="Left" />
                    <%--<ItemStyle Font-Bold="True" ForeColor="#009900" />--%>
                </asp:BoundField>
                <asp:BoundField HeaderText="Product Group" DataField="PRODUCT_GROUP">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Product Code" DataField="PRODUCT_CODE">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Product Name" DataField="PRODUCT_NAME">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="UOM" DataField="UOM">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Return Qty (Crates)" DataField="CRATES" DataFormatString="{0:n2}">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Return Qty (Box)" DataField="BOX" DataFormatString="{0:n2}">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Return Qty (Ltr)" DataField="LTR" DataFormatString="{0:n2}">
                    <HeaderStyle HorizontalAlign="Left" />
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


    <%--</div>--%>
</asp:Content>

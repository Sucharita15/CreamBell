<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmDiscountView.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmDiscountView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <link href="css/style.css" rel="stylesheet" />

    <script src="Javascript/custom.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gridViewCustomers.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=gridViewCustomers.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead").append(gridHeader);
            $('#controlHead').css('position', 'absolute');
            $('#controlHead').css('top', '129');

        });


        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gridView1.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=gridView1.ClientID%> tr th').each(function (i) {
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


    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
        &nbsp;&nbsp; Discount View
    </div>
    <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
        <table style="width: 100%">
            <tr>
                <td style="width: 10%;text-align:center">State</td>
                <td style="width: 25%;">
                    <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" Width="180px">
                    </asp:DropDownList>
                </td>
                <td style="width: 10%; text-align:center">Distributor</td>
                <td style="width: 25%;">
                    <asp:DropDownList ID="ddlSiteId" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="180px" OnSelectedIndexChanged="ddlSiteId_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>

                <td style="width: 15%;">
                    <asp:Button ID="btnGenerate" runat="server" CssClass="ReportSearch" Height="31px" OnClick="btnGenerate_Click" Text="Generate" Width="96px" />
                </td>
                <td style="width: 15%;">
                    <asp:Button ID="btnExportToExcel" runat="server" CssClass="ReportSearch" Height="31px" OnClick="btnExportToExcel_Click" Text="Export To Excel" Width="96px" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="small"> </asp:Label>
                </td>

            </tr>
        </table>
    </asp:Panel>
    <div style="height: auto; overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; height: 240px">

        <asp:GridView ID="gridViewCustomers" runat="server" AutoGenerateColumns="False" Width="100%"
            BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">

            <AlternatingRowStyle BackColor="#CCFFCC" />
            <Columns>
                <asp:TemplateField HeaderText="Starting_Date">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("STARTINGDATE", "{0:dd MMM yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ending_Date">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("ENDINGDATE", "{0:dd MMM yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField HeaderText="Sales Type" DataField="Sales Type">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Sales Code" DataField="SalesCode">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Sales Description" DataField="SALESDESCRIPTION">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Customer Type" DataField="CustomerType">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="CUSTOMERCODE" HeaderText="Customer Code">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Scheme Item Type" HeaderText="Scheme Item Type">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Scheme Item Group">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkbtn" runat="server" Text='<%# Bind("SCHEMEITEMGROUP") %>' OnClick="lnkbtn_Click"></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField DataField="SCHEMEITEMGROUPNAME" HeaderText="Scheme Item Group Name">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="CalculationBase" HeaderText="Calculation Base">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Calculation Type" HeaderText="Calculation Type">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Value" HeaderText="Value" DataFormatString="{0:n2}">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>

            </Columns>
            <EmptyDataTemplate>
                No Record Found...
            </EmptyDataTemplate>
            <FooterStyle BackColor="#bfbfbf" />
            <HeaderStyle BackColor="#05345C" ForeColor="#000000" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#007DBB" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#00547E" />
        </asp:GridView>
    </div>

    <%-- <asp:Panel ID="pnl" runat="server" GroupingText="Item Details">--%>

    <%--<div id="controlHead1" style="margin-top:5px; margin-left:5px;padding-right:10px;"></div>--%>
    <div style="overflow: auto; margin-top: 10px; margin-left: 5px; padding-right: 10px; height: 220px">

        <asp:GridView ID="gridView1" runat="server" AutoGenerateColumns="False" Width="100%"
            BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">

            <AlternatingRowStyle BackColor="#CCFFCC" />
            <Columns>
                <asp:BoundField HeaderText="ITEMID" DataField="ITEMID">
                    <HeaderStyle HorizontalAlign="Left" />
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="GROUP" DataField="GROUP">
                    <HeaderStyle HorizontalAlign="Left" />
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="ITEMNAME" DataField="ITEMNAME">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="GROUPNAME" DataField="GROUPNAME">
                    <HeaderStyle HorizontalAlign="Left" />
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>

            </Columns>
            <EmptyDataTemplate>
                No Record Found...
            </EmptyDataTemplate>
            <FooterStyle BackColor="#bfbfbf" />
            <HeaderStyle BackColor="#05345C" ForeColor="#000000" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#007DBB" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#00547E" />
        </asp:GridView>
    </div>

    <%--  </asp:Panel>--%>
</asp:Content>

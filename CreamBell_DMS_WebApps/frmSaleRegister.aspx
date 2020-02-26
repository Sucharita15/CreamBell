<%@ Page Title="Sale Register" MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="frmSaleRegister.aspx.cs" EnableEventValidation="false" Inherits="CreamBell_DMS_WebApps.frmSaleRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gridSaleRegister.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=gridSaleRegister.ClientID%> tr th').each(function (i) {
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

    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
        Sale Register
    </div>
    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>

    <div id="Filter" style="width: 99%; height: 34px; border-radius: 1px; margin: 10px 0px 0px 5px; color: black; padding: 2px 0px 0px 0px; border-style: groove">
        <table style="width: 95%">
            <tr>
                <td class="auto-style5">
                           <asp:RadioButton ID="rdoDetailedView" runat="server" AutoPostBack="true" Text="Detailed View" Checked="true" OnCheckedChanged="rdoDetailedView_CheckedChanged"
                               GroupName="radio" />
                </td>
                <td class="auto-style5">
                    <asp:RadioButton ID="rdoSummarisedView" runat="server" AutoPostBack="true" Text="Summarised View" OnCheckedChanged="rdoDetailedView_CheckedChanged"
                        GroupName="radio" />
                </td>
                <td>From Date :</td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" Enabled="true" placeholder="dd-MMM-yyyy"></asp:TextBox>
                    <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>
                <td>To Date :</td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" Enabled="true" placeholder="dd-MMM-yyyy"></asp:TextBox>
                    <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>
                <td >Business Unit</td>
                <td>
                    <asp:DropDownList ID="DDLBusinessUnit" runat="server" Width="95%" AutoPostBack="true"></asp:DropDownList>
                </td>
                <td style="text-align: left; display:none; width: 80px;">
                    <asp:Button ID="BtnSearch" runat="server" Text="Search" CssClass="ReportSearch" OnClick="BtnSearch_Click" />
                </td>
                <td style="text-align: left">
                    <asp:ImageButton ID="imgBtnExportToExcel" runat="server" ImageUrl="~/Images/excel-24.ico" ToolTip="Click To Generate Excel Report" OnClick="imgBtnExportToExcel_Click" />
                </td>
                <td>
                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkBlue" Font-Names="Seoge UI"> </asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <%--<div id="controlHead2" style="margin-top:5px; margin-left:5px;padding-right:10px;width: 100%;"></div>--%>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; table-layout: fixed;">
        <tr>
            <td  style="width: 100%;vertical-align:top;text-align:left">
                    <div style="overflow: auto; margin-top: 5px; margin-left: 5px; width: 99%; height: 430px; table-layout: fixed;">
                     <asp:GridView runat="server" ID="gridSaleRegister" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="true" Width="105%" BackColor="White"
                        HeaderStyle-HorizontalAlign="Left"
                        ItemStyle-HorizontalAlign="Right"
                        BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">

                        <AlternatingRowStyle BackColor="#CCFFCC" />

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
                    <asp:GridView runat="server" ID="gridSaleRegisterbottom" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="True" Width="98%" BackColor="White"
                        HeaderStyle-HorizontalAlign="Left"
                        ItemStyle-HorizontalAlign="Right"
                        BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" Visible="false">
                        <AlternatingRowStyle BackColor="#CCFFCC" />
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
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmVatReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmVatReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/SSheet.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gvDetail.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=gvDetail.ClientID%> tr th').each(function (i) {
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
    <asp:UpdateProgress ID="upprogress" runat="server" AssociatedUpdatePanelID="upreport" DisplayAfter="0">
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
    <asp:UpdatePanel runat="server" ID="upreport">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
            <asp:PostBackTrigger ControlID="imgBtnExportToExcel" />
        </Triggers>
        <ContentTemplate>

            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
                Vat Report
            </div>

            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Small"> </asp:Label>
                    </td>
                </tr>
            </table>

            <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
                <table style="width: 100%;">

                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <tr>
                        <td style="width: 5%; text-align: center">From Date :</td>
                        <td style="width: 12%; text-align: left">
                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>

                        <td style="vertical-align: top; text-align: left; width: 17%;" rowspan="3">State :
                    <div style="max-height: 80px; overflow-y: auto;">
                        <asp:CheckBoxList ID="chkListState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkListState_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </div>
                        </td>
                        <td style="vertical-align: top; text-align: left; width: 17%;" rowspan="3">Distributor :
                    <div style="max-height: 80px; overflow-y: auto;">
                        <asp:CheckBoxList ID="chkListSite" runat="server" AutoPostBack="True">
                        </asp:CheckBoxList>
                    </div>
                        </td>

                        <td style="vertical-align: top; text-align: left;" rowspan="3">Report                          
                           
                        </td>
                        <td style="vertical-align: top; text-align: left;">
                            <asp:DropDownList ID="ddlReportType" runat="server" CssClass="dropdown" Width="250px" AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Text="-Select-" Value="0">-Select-</asp:ListItem>
                                <asp:ListItem Text="Sale Tax Detail Report" Value="1">Sales Tax Report - Detail</asp:ListItem>
                                <asp:ListItem Text="Sales Summary TAX Report" Value="2">Sales Tax Report - Summary</asp:ListItem>
                                <asp:ListItem Text="Retailer Wise SlabWise VAT Report" Value="3">Retailer Wise SlabWise VAT Report</asp:ListItem>
                                <asp:ListItem Text="Purchase Tax Detail Report" Value="4">Purchase Tax Report - Detail</asp:ListItem>
                                <asp:ListItem Text="Purchase Summary TAX Report" Value="5">Purchase Tax Report - Summary</asp:ListItem>
                                <asp:ListItem Text="Productwise INPUT-OUTPUT" Value="6">Productwise INPUT-OUTPUT</asp:ListItem>
                                <asp:ListItem Text="Output VAT report" Value="7">Output VAT Report</asp:ListItem>
                                <asp:ListItem Text="Input VAT report" Value="12">Input VAT Report</asp:ListItem>
                                <asp:ListItem Text="Purchase Register" Value="8">Purchase Register</asp:ListItem>
                                <asp:ListItem Text="Sales Register" Value="9">Sales Register</asp:ListItem>
                                <asp:ListItem Text="24 Annexure AB-Sales" Value="10">24 Annexure AB-Sales</asp:ListItem>
                                <asp:ListItem Text="24 Annexure AB-Purchase" Value="11">24 Annexure AB-Purchase</asp:ListItem>
                            </asp:DropDownList>
                        </td>

                        <td style="width: 10%; text-align: center">
                            <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" Text="Show Report" Width="96px" OnClick="BtnShowReport0_Click1" />

                            <td>
                                <asp:ImageButton ID="imgBtnExportToExcel" runat="server" ImageUrl="~/Images/excel-24.ico" ToolTip="Click To Generate Excel Report" OnClick="BtnShowReport0_Click" />
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; text-align: center">To Date :</td>
                        <td style="width: 10%; text-align: left">
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <td colspan="2" style="text-align: right">
                            <asp:CheckBoxList runat="server" RepeatDirection="Horizontal" ID="chkCustomerType" Visible="false"
                                RepeatColumns="2" RepeatLayout="Table">
                                <asp:ListItem Text="UnRegistered" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Registered" Value="2"></asp:ListItem>
                            </asp:CheckBoxList>

                        </td>


                    </tr>
                </table>
            </asp:Panel>

            <div id="controlHead" style="margin-top: 4px; margin-left: 5px; padding-right: 10px;"></div>
            <div style="height: 455px; overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px;">

                <asp:GridView ID="gvDetail" runat="server" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="true" Width="100%" BackColor="White"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
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

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

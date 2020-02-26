<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmExpenseCalculation.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmExpenseCalculation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/SSheet.css" rel="Stylesheet" type="text/css" />
    <link href="css/btnSearch.css" rel="stylesheet" />

    <style type="text/css">
        .button {
        }
    </style>

    <script type="text/javascript">
      $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=grvDetail.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=grvDetail.ClientID%> tr th').each(function (i) {
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
    <asp:UpdateProgress ID="upprogress" runat="server" AssociatedUpdatePanelID="sdsds" DisplayAfter="0">
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
    <div style="width: 99%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #0085ca; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px; color: #FFFFFF;">Expense Calculation</span>
    </div>
    <div style="width: 99%">
        <asp:UpdatePanel ID="sdsds" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <table style="width: 99%">
                    <tr>
                        <td>
                            <asp:Button ID="btnSubmit" runat="server" Text="View" CssClass="button" ValidationGroup="btnValidate" Height="25px" Width="64px" OnClick="btnSubmit_Click" />
                            &nbsp;&nbsp; &nbsp;&nbsp;
                            <asp:Button ID="btnsave" Visible="false" runat="server" CssClass="button" Height="25px" Text="Save" Width="68px" OnClick="btnsave_Click" />
                            &nbsp;&nbsp;</td>
                        <td style="width: 69%">
                            <asp:ImageButton ID="imgBtnExportToExcel" runat="server" ImageUrl="~/Images/excel-24.ico" ToolTip="Click To Generate Excel Report" OnClick="imgBtnExportToExcel_Click" Style="width: 24px" />
                            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="imgBtnExportToExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <div style="width: 100%; text-align: left">
        <asp:UpdatePanel ID="upselection" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table style="width: 100%; text-align: left">
                    <tr>
                        <td style="width: 10%;">Business Unit :</td>
                        <td style="width: 25%;">
                            <asp:DropDownList ID="ddlBusinessUnit" runat="server" CssClass="textboxStyleNew" AutoPostBack="true" Font-Size="Smaller" Height="21px" Width="98%">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 10%;">Expense Type :</td>
                        <td style="width: 25%;">
                            <asp:DropDownList ID="drptargetType" runat="server" CssClass="textboxStyleNew" AutoPostBack="true" Font-Size="Smaller" Height="21px" Width="98%" OnSelectedIndexChanged="drptargetType_SelectedIndexChanged">
                                <asp:ListItem Text="Distributor Expense" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Annual Expense" Value="2"></asp:ListItem>
                                <asp:ListItem Text="PSR Stipend Expense" Value="3"></asp:ListItem>
                                <%--<asp:ListItem Text="VRS Commission" Value="4"></asp:ListItem>
                                <asp:ListItem Text="Vending Scheme Claim" Value="5"></asp:ListItem>
                                <asp:ListItem Text="VRS Claim SetUp" Value="6"></asp:ListItem>
                                <asp:ListItem Text="Vending Scheme Claim PRESENT DAYS" Value="7"></asp:ListItem>
                                <asp:ListItem Text="Vending Scheme Claim PRESENT DAYS WITH TARGET" Value="8"></asp:ListItem>
                                <asp:ListItem Text="Vending Scheme Claim WITH TARGET" Value="9"></asp:ListItem>
                                <asp:ListItem Text="Vending Scheme Claim Yearly" Value="10"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 10%; text-align: right;">
                            <asp:Label ID="lbldate" runat="server" Text="Expense Month"></asp:Label>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFromDate" Display="Dynamic" ErrorMessage="Select Date.." ForeColor="#FF3300" ValidationGroup="btnValidate"></asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 10%;">
                            <asp:TextBox ID="txtFromDate" AutoPostBack="true" runat="server" Width="98%" Font-Size="Smaller" CssClass="textboxStyleNew" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                        </td>
                        <td style="width: 10%; text-align: right;"></td>
                        <td style="width: 15%;">
                            <%--<asp:DropDownList ID="ddlClaimCategory" AutoPostBack="true" runat="server" Height="21px" Width="98%" OnSelectedIndexChanged="ddlClaimCategory_SelectedIndexChanged"
                                 Font-Size="Smaller" CssClass="textboxStyleNew">
                            </asp:DropDownList>--%>
                        </td>
                        <td style="width: 10%; text-align: right;"></td>
                        <td style="width: 20%;">
                            <%--<asp:DropDownList ID="ddlClaimSubCategory" AutoPostBack="true" runat="server" Height="21px" Width="98%" Font-Size="Smaller" CssClass="textboxStyleNew">
                            </asp:DropDownList>--%>
                        </td>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="txtFromDate" TargetControlID="txtFromDate" Format="MMM-yyyy"></asp:CalendarExtender>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div style="overflow: auto;">
        <asp:UpdatePanel ID="UPPSRStipend" runat="server">
            <ContentTemplate>
                <table style="width: 100%; table-layout: fixed;">
                    <tr>
                        <td>
                            <div style="overflow: auto; margin: 10px 0px 0px 10px; text-align: left">
                                <%-- <asp:GridView ID="gvDetailsPSRStipend" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3"
                                    ShowHeaderWhenEmpty="True" EnableViewState="true">
                                    <AlternatingRowStyle BackColor="#CCFFCC" />--%>
                                <asp:GridView runat="server" ID="gvDetailsPSRStipend" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                                    CellPadding="3" ShowHeaderWhenEmpty="True">
                                    <AlternatingRowStyle BackColor="#CCFFCC" />
                                    <Columns>
                                        <asp:BoundField HeaderText="PSR_Code" DataField="PSRCode"></asp:BoundField>
                                        <asp:BoundField HeaderText="PSR_Name" DataField="PSR_NAME"></asp:BoundField>
                                        <asp:BoundField HeaderText="Employee_NO" DataField="EMPLOYEE_NO"></asp:BoundField>
                                        <asp:BoundField HeaderText="ContributionStipend" DataField="CONTRIBUTIONSTIPEND" DataFormatString="{0:n2}" />
                                        <asp:BoundField HeaderText="BUCODE" DataField="BUCODE"/>
                                        <asp:BoundField HeaderText="TA" DataField="TA" DataFormatString="{0:n2}"></asp:BoundField>
                                        <asp:BoundField HeaderText="DA" DataField="DA" DataFormatString="{0:n2}" />
                                        <asp:BoundField HeaderText="Mobile" DataField="MOBILE" DataFormatString="{0:n2}"></asp:BoundField>
                                        <asp:BoundField HeaderText="TotalStipend" DataField="TotalStipend" DataFormatString="{0:n2}" Visible="false"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Stipend Days">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtStipendDays" runat="server" Font-Size="X-Small" AutoPostBack="true" onkeypress="CheckNumeric(event);" OnTextChanged="txtStipendDays_TextChanged" Width="40px" Text="0"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TA-DA Days">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTADADays" runat="server" Font-Size="X-Small" AutoPostBack="true" onkeypress="CheckNumeric(event);" OnTextChanged="txtTADADays_TextChanged" Width="40px" Text="0"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Actual Stipend" DataFormatString="{0:n2}" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Record Found...
                                    </EmptyDataTemplate>
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
    </div>
    <div style="overflow: auto; height: 400px;">
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <table style="width: 100%; table-layout: fixed;">
                    <tr>
                        <td>
                            <div id="controlHead" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 100%"></div>
                            <div style="height: auto; overflow: auto; margin-top: 5px; height: 100%; width: 100%">
                                <%--<div style="overflow: auto; height: 390px; margin: 10px 0px 0px 10px; text-align: left">--%>
                                <asp:GridView ID="grvDetail" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3"
                                    ShowHeaderWhenEmpty="True" EnableViewState="true">
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

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


    <script src="jquery/ScrollableGridViewPlugin_ASP.NetAJAXmin.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=grvDetail.ClientID %>').Scrollable({
                ScrollHeight: 300,
                IsInUpdatePanel: true
            });
        });
    </script>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmStockMovementReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmStockMovementReport" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

           $(document).ready(function () {
               /*Code to copy the gridview header with style*/
               var gridLine = $('#<%=gridStockMovementReport.ClientID%>').clone(true);
                /*Code to remove first ror which is header row*/
                $(gridLine).find("tr:gt(0)").remove();
                $('#<%=gridStockMovementReport.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridLine).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead2").append(gridLine);
            $('#controlHead2').css('position', 'absolute');
            $('#controlHead2').css('top', '129');

           });

         
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
         <Triggers>
            <asp:PostBackTrigger ControlID="BtnExportToExel" />
                   </Triggers> 
        <ContentTemplate>
            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
                Stock Movement Report
            </div>
            <%--<asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 100%; margin: 0px 0px 0px 10px;">--%>
                <table style="width: 100%">

                    <%--<asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="MMM-yyyy"></asp:CalendarExtender>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="MMM-yyyy"></asp:CalendarExtender>--%>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd MMM yyyy"></asp:CalendarExtender>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd MMM yyyy"></asp:CalendarExtender>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="small"> </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>From Date :</td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server"  placeholder="From Date" Width="107px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <td>To Date</td>
                        <td>
                            <%--<asp:TextBox ID="txtToDate" runat="server" placeholder="MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />--%>
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="To Date" Width="107px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <td>Trans Type</td>
                        <td>
                            <%--<asp:DropDownList ID="ddlPSR" runat="server" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="200px">
                            </asp:DropDownList>--%>
                            <asp:DropDownList ID="BulletinTypeDropDown" runat="server" Width="150px" Font-Size="Smaller" Height="22px">
                                <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>
                                <asp:ListItem Text="Usable To Non Usable" Value="Usable To Non Usable"></asp:ListItem>
                                <asp:ListItem Text="Non Usable To Usable" Value="Non Usable To Usable"></asp:ListItem>
                                <asp:ListItem Text="Non Usable To Scrap" Value="Non Usable To Scrap"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>State</td>
                        <td>
                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" Width="120px">
                            </asp:DropDownList>
                        </td>

                        <td>
                            <%--<asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" />--%>
                        </td>

                    </tr>
                    <tr>
                     <td>
                         <asp:Label ID="Label1" runat="server" ></asp:Label>
                     </td>
                    </tr>
                    <tr>
                        <td>Distributor</td>
                        
                        <td>
                            <asp:DropDownList ID="ddlSiteId" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px">
                            </asp:DropDownList>
                        </td>
                        <td></td>
                        <%-- <td>
                            <asp:DropDownList ID="ddlState" runat="server" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="200px" OnSelectedIndexChanged="ddlPSR_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>--%>
                        <td>
                            <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Generate" Width="96px" />
                        </td>
                        <td>
                            <asp:Button ID="BtnExportToExel" runat="server" CssClass="ReportSearch" Height="31px"  Text="Export to Excel" Width="96px" OnClick="BtnExportToExel_Click" />
                        </td>
                    </tr>
                </table>
            <%--</asp:Panel>--%>
            <div style="margin: 10px">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td valign="top" style="width: 100%" align="left">
                            <div style="overflow: auto;  margin-top: 10px; margin-left: 5px; padding-right: 10px;">                                
                                <asp:GridView runat="server" ID="gridStockMovementReport" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%"  BackColor="White"
                                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                                    <AlternatingRowStyle BackColor="#CCFFCC" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Item Code" DataField="ProductCode">
                                            <HeaderStyle Width="50px" HorizontalAlign="Left" />
                                            <ItemStyle Width="50px" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Item Name" DataField="PRODUCT_NAME">
                                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Date" DataField="DocumentDate">
                                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="From WareHouse" DataField="FROM_LOCATION">
                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                            <ItemStyle HorizontalAlign="Left" Width="110px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="To WareHouse" DataField="TO_LOCATION">
                                            <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                            <ItemStyle Width="100px" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Box" DataField="TransQty" DataFormatString="{0:n0}">
                                            <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                            <ItemStyle Width="100px" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="₹ Base Rate" DataField="BASERATE"  DataFormatString="{0:n2}">
                                            <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                            <ItemStyle Width="100px" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                         <asp:BoundField HeaderText="₹ Base Value" DataField="BaseValue" DataFormatString="{0:n2}" >
                                            <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                            <ItemStyle Width="100px" HorizontalAlign="Left" />
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
                        </td>
                    </tr>
                </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

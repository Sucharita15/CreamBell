<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmBulkBuyerReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmBulkBuyerReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script src="Javascript/DateValidation.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
   <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
 
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
        </Triggers>
        <ContentTemplate>

            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
                Bulk Buyer Report
            </div>

            <table style="width: 100%">
                <tr>
                     <td id="tclink" runat="server" >
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" ForeColor="Black">Hide sales person filter</asp:LinkButton>
                        </td>
                        <td runat="server" style=" text-align:center" id="tclabel" >
                    </td>
                </tr>
            </table>

            <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
                <table style="width: 100%">
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                     <td colspan="9">
                            <asp:Panel ID="Panel1" runat="server" GroupingText="Sale Person Filter" Style="width: 99%; margin: 0px 0px 0px 2px;">
                                <table>
                                <tr>
                              
                                 
                                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 11%;">HOS :
                                   <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                         <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="chkAll_CheckedChanged" Text="Select All" />
                         <asp:CheckBoxList ID="chkListHOS" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstHOS_SelectedIndexChanged" >
                            </asp:CheckBoxList>
                        </div>
                     
                            </td>
                                   <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 11%;">VP :
                        <div class="checkboxlistHeader"; style="max-height: 80px; width:150px;overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListVP" runat="server" OnSelectedIndexChanged="lstVP_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                            <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 11%;">GM :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox2_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListGM" runat="server" OnSelectedIndexChanged="lstGM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                                             <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 13%;">DGM :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox3" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox3_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListDGM" runat="server"  OnSelectedIndexChanged="lstDGM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                             <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 14%;">RM :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox4" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox4_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListRM" runat="server"  OnSelectedIndexChanged="lstRM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                                             <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 14%;">ZM :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox5" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox5_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListZM" runat="server" OnSelectedIndexChanged="lstZM_SelectedIndexChanged"  AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                                             <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 14%;">ASM :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox6" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox6_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListASM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstASM_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                                                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 16%;">EXECUTIVE :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox7" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox7_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListEXECUTIVE" runat="server" OnSelectedIndexChanged="lstEXECUTIVE_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                                </tr> 
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>

                     <tr>
                        <td style="width: 5%; text-align: center">From Date :</td>
                        <td style="width: 12%; text-align: left">
                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"  onchange="ValidateDate(this)" required></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>

                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 17%;" rowspan="2">State :
                    <div style="max-height: 80px; overflow-y: auto;">
                        <asp:CheckBoxList ID="chkListState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkListState_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </div>
                        </td>
                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 17%;" rowspan="2">Distributor :
                    <div style="max-height: 80px; overflow-y: auto;">
                        <asp:CheckBox ID="CheckBox8" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox8_CheckedChanged" Text="Select All" />
                        <asp:CheckBoxList ID="chkListSite" runat="server" OnSelectedIndexChanged="chkListSite_SelectedIndexChanged" AutoPostBack="True">
                        </asp:CheckBoxList>
                    </div>
                        </td>
                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 17%;" rowspan="2">Customer Group:
                    <div style="max-height: 80px; overflow-y: auto; width: 100%;">
                        <asp:CheckBoxList ID="chkListCustomerGroup" runat="server" OnSelectedIndexChanged="chkListCustomerGroup_SelectedIndexChanged" AutoPostBack="true">
                        </asp:CheckBoxList>
                    </div>
                        </td>

                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 17%;" rowspan="2">Customer Name:
                   
                        <asp:DropDownList ID="chkCustomerName" runat="server" CssClass="dropdown" Width="200px">
                        </asp:DropDownList>
                 

                        </td>
                         <td style="width: 10%; text-align: center">
                            <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport0_Click" Text="ExportToExcel" Width="96px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; text-align: center">To Date :</td>
                        <td style="width: 10%; text-align: left">
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"  onchange="ValidateDate(this)" required></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <td style="width: 10%; text-align: center">
                            <asp:Button ID="btnShow" runat="server" CssClass="ReportSearch" Height="31px" OnClick="btnShow_Click" Text="Show Report" Width="96px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="small"> </asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <div style="margin: 10px">
                <asp:UpdatePanel ID="grdpnl" runat="server">
                    <ContentTemplate>
                <asp:Panel ID="Panel2" runat="server" GroupingText="Bulk Buyer Report" Width="100%" BackColor="White">
                  <asp:GridView ID="GvBulkBuyer" CssClass = "GridViewClass" runat="server" GridLines="Horizontal" AutoGenerateColumns="true" Width="100%" BackColor="White"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                      ItemStyle HorizontalAlign ="Center" >
                  
                    <EmptyDataTemplate>
                        No Record Found...
                    </EmptyDataTemplate>
                    <FooterStyle CssClass="table-condensed" BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <HeaderStyle BackColor="#05345C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="center" VerticalAlign="Middle" />
                    <RowStyle BackColor="White" ForeColor="#4A3C8C" HorizontalAlign="center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <SortedAscendingCellStyle BackColor="#F4F4FD" />
                    <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                    <SortedDescendingCellStyle BackColor="#D8D8F0" />
                    <SortedDescendingHeaderStyle BackColor="#3E3277" />
                </asp:GridView>
                </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                <%--<asp:Panel ID="PanelReport" runat="server" GroupingText="Customer Ledger Report" Width="100%" BackColor="White">
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="500px"></rsweb:ReportViewer>
                </asp:Panel>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

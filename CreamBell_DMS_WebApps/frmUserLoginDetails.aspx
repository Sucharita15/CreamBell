<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmUserLoginDetails.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmUserLoginDetails" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>

            <asp:PostBackTrigger ControlID="btnExportToExcel" />
        </Triggers>
        <ContentTemplate>

            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
                &nbsp;&nbsp; USER LOGIN DETAILS
            </div>

            <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
                <table style="width: 100%">

                    <tr>
                        <td>State</td>
                        <td>
                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>Site ID</td>
                        <td>
                            <asp:DropDownList ID="ddlSiteId" runat="server" AutoPostBack="True" Font-Size="Smaller" Height="22px" Width="260px">
                            </asp:DropDownList>
                        </td>

                        <td>
                            <asp:RadioButton ID="rdoDistributor" runat="server" AutoPostBack="true" Text="Distributor" Checked="true" Width="100px"
                                OnCheckedChanged="rdoDistributor_CheckedChanged" GroupName="radio" />
                            <asp:RadioButton ID="rdoPSRVRS" runat="server" AutoPostBack="true" Text="PSR-VRS" Width="100px"
                                OnCheckedChanged="rdoDistributor_CheckedChanged" GroupName="radio" />
                        </td>
                        <td>
                            <asp:Button ID="btnGenerate" runat="server" CssClass="ReportSearch" Height="31px" OnClick="btnGenerate_Click" Text="Generate" Width="96px" />
                        </td>
                        <td>
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

            <table style="width: 100%; table-layout: fixed;">
                <tr>
                    <td vertical-align: top;">

                        <div style="overflow: auto; height: 390px; margin-top: 10px; margin-left: 5px; padding-right: 10px;">

                            <asp:GridView runat="server" ID="gridLoginDetails" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="true" Width="100%" BackColor="White"
                                BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" ShowHeaderWhenEmpty="True">

                                <AlternatingRowStyle BackColor="#CCFFCC" />

                                <EmptyDataTemplate>
                                    No Record Found...
                                </EmptyDataTemplate>
                                <FooterStyle  BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                <HeaderStyle BackColor="#05345C" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="True" ForeColor="#F7F7F7" />
                                 <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                               <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C"  />
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

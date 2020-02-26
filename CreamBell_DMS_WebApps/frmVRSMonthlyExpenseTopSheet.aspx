<%@ Page Language="C#" Title=" Monitoring  Summary" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmVRSMonthlyExpenseTopSheet.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmVRSMonthlyExpenseTopSheet" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/ucRoleFilters.ascx" TagPrefix="uc1" TagName="ucRoleFilters" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
        </Triggers>
        <ContentTemplate>
            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
                VRS Monthly Expense Top Sheet
            </div>
            <uc1:ucRoleFilters runat="server" ID="ucRoleFilters" />
            <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
                <table style="width: 100%">
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="MMM-yyyy"></asp:CalendarExtender>
                    
                    <tr>

                        <td colspan="7">
                            <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Small"> </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <asp:PlaceHolder runat="server" ID="phState">
                            <td style="text-align: right">State</td>
                            <td>

                                <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" Width="120px">
                                </asp:DropDownList>

                            </td>
                            <td style="text-align: right">Distributor : </td>
                            <td>

                                <asp:DropDownList ID="ddlSiteId" runat="server" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlSiteId_SelectedIndexChanged">
                                </asp:DropDownList>

                            </td>
                        </asp:PlaceHolder>
                        <td style="text-align: right">VRS : </td>
                        <td>

                            <asp:DropDownList ID="ddlVRS" runat="server" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px">
                            </asp:DropDownList>

                        </td>
                        <td style="text-align: right">Business Unit: </td>
                        <td>
                            <asp:DropDownList ID="ddlBusinessUnit" runat="server" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right">Month :</td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" Width="107px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" />
                        </td>

                    </tr>
                </table>
            </asp:Panel>

            <div style="margin: 10px">
                <asp:Panel ID="PanelReport" runat="server" GroupingText="VRS Monthly Expense Top Sheet" Width="100%" BackColor="White">
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="500px"></rsweb:ReportViewer>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



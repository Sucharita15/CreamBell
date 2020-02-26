<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPartyWiseSchemeDetailsonPurchaseRaterReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPartyWiseSchemeDetailsonPurchaseRaterReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
        </Triggers>
        <ContentTemplate>
            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
                &nbsp;&nbsp; Consolidated Claim Scheme Report
            </div>
            <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
                <table style="width: 100%">
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <tr>
                        <td>From Date :</td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px" OnTextChanged="txtFromDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <td>State</td>
                        <td>
                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" Width="120px">
                            </asp:DropDownList>
                        </td>
                         <asp:UpdatePanel ID="Scheme" runat="server" UpdateMode="Conditional">
                         <ContentTemplate>
                        <td rowspan="2" style="background-color: aliceblue; vertical-align: top; text-align: left; width: 350px">Scheme:
                    <div style="max-height: 60px; overflow-y: auto; width: 100%;">
                        <asp:CheckBoxList ID="chkScheme" runat="server">
                        </asp:CheckBoxList>
                    </div>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td rowspan="2" style="background-color: aliceblue; vertical-align: top; text-align: left; width: 250px">Customer Group:
                    <div style="max-height: 60px; overflow-y: auto; width: 100%;">
                        <asp:CheckBoxList ID="chkListCustomerGroup" runat="server">
                        </asp:CheckBoxList>
                    </div>
                        </td>
                        <td>
                            <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" />
                        </td>
                    </tr>
                    <tr>
                        <td>To Date :</td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px" OnTextChanged="txtToDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <td>Site ID</td>
                        <td>
                            <asp:UpdatePanel ID="SD" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlSiteId" runat="server" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="ddlSiteId_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Small"> </asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="margin: 10px">
                <asp:Panel ID="PanelReport" runat="server" GroupingText="Consolidated Claim Scheme Report" Width="100%" BackColor="White">
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="500px"></rsweb:ReportViewer>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

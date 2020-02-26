<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmMobileAppSyncReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmMobileAppSyncReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
        Mobile App Sync REPORT
    </div>

    <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
        <table style="width: 100%">
            <tr>
                <td>State: </td>
                <td>
                    <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>User Type: </td>
                <td>
                    <asp:DropDownList ID="ddlUserType" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px">
                    </asp:DropDownList>
                </td>
                <td>User:</td>
                <td>
                    <asp:DropDownList ID="ddlUser" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px">
                    </asp:DropDownList>
                </td>
                <td>Sync Version</td>
                <td>
                    <asp:DropDownList ID="ddlSyncVersion" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px">
                    </asp:DropDownList></td>
                <td>
                    <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" />
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Large"> </asp:Label>
                </td>

            </tr>


        </table>
    </asp:Panel>

    <div style="margin: 10px">
        <asp:Panel ID="PanelReport" runat="server" GroupingText="Mobile App Sync Report" Width="100%" BackColor="White">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="500px"></rsweb:ReportViewer>
        </asp:Panel>
    </div>
</asp:Content>

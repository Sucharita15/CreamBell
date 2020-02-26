<%@ Page Title="Stock Ledger Report" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmStockLedgerReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmStockLedgerReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style2 {
            width: 230px;
        }
        .auto-style3 {
            width: 31px;
        }
        .auto-style4 {
            width: 220px;
        }
        .auto-style5 {
            width: 121px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGenerate" />
            <asp:PostBackTrigger ControlID="btnExporttoExcel" />
        </Triggers>
        <ContentTemplate>

            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
                &nbsp;&nbsp;Stock Ledger Report
            </div>

            <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
                <table style="width: 100%">

                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>

                    <tr>
                        <td>From Date :</td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" Width="100px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>

                        <td>To Date :</td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" Width="100px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <td class="auto-style5">Business Unit :</td>
                        <td class="auto-style4" >
                            <asp:DropDownList ID="ddlBusinessUnit" runat="server" AutoPostBack="true" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px" OnSelectedIndexChanged="ddlBusinessUnit_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style3">Item:</td>
                        <td>
                            <asp:DropDownList ID="drpProduct" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="drpProduct_SelectedIndexChanged" Width="175px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>State:</td>
                        <td>
                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>Distributor:</td>
                        <td>
                            <asp:DropDownList ID="ddlSiteId" runat="server" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="150px" OnSelectedIndexChanged="ddlSiteId_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style5">Warehouse:</td>
                        <td class="auto-style4">
                            <asp:DropDownList ID="ddlWarehouse" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="ddlWarehouse_SelectedIndexChanged" Width="200px">
                            </asp:DropDownList>
                        </td>

                    </tr>

                    <tr>
                        <td >
                            <asp:RadioButton ID="rdoDetail" runat="server" AutoPostBack="true" Text="Detail" Checked="true"
                                OnCheckedChanged="rdoDetail_CheckedChanged" GroupName="radio" />
                        </td>
                        <td >
                            <asp:RadioButton ID="rdoSummary" runat="server" AutoPostBack="true" Text="Summary" 
                                OnCheckedChanged="rdoSummary_CheckedChanged" GroupName="radio" />
                        </td>
                        
                        <td colspan="2">
                            <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="small"> </asp:Label>
                        </td>
                        <td class="auto-style5">
                            <asp:Button ID="btnGenerate" runat="server" CssClass="ReportSearch" Height="31px" OnClick="btnGenerate_Click" Text="Generate" Width="96px" />
                        </td>
                        <td class="auto-style4">
                            <asp:Button ID="btnExporttoExcel" runat="server" CssClass="ReportSearch" Height="31px" OnClick="btnExporttoExcel_Click" Text="Export to Excel" Width="96px" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <div style="margin: 10px">
                <asp:Panel ID="PanelReport" runat="server" GroupingText="Stock Ledger Report" Width="100%" BackColor="White">
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="500px"></rsweb:ReportViewer>
                </asp:Panel>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

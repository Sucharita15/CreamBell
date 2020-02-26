<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmGSTR1Page.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmGSTR1Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp1" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style2 {
            width: 35%;
        }
        .progress
        {
            background: #FFFFFF;
            font-family: Verdana,Arial, Helvetica;
            color: Black;
            font-size: 11.5px;  
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
        GSTR REPORT
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnReport"/>
        </Triggers>
        <ContentTemplate>
            <table style="width: 100%;">
                <tr>
                    <td colspan="3" style="width: 100%">
                        <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Small"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;text-align:center">From Date :</td>
                    <td style="width:15%;text-align:left">
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                        <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" Width="130px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                    </td>
                    <td style="width:10%;text-align:center">To Date :</td>
                    <td style="width:15%;text-align:left">
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                        <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" Width="130px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rdoGSTR1" Text="GSTR1 Report" GroupName="Reportrdo" Checked="true" runat="server" />
                        <asp:RadioButton ID="rdoGSTR2" Text="GSTR2 Report" GroupName="Reportrdo" runat="server" />
                        <asp:RadioButton ID="rdoGSTR3" Text="GSTR3B Report" GroupName="Reportrdo" runat="server" />
                    </td>
                    <td >
                        <asp:Button ID="btnReport" runat="server" CssClass="ReportSearch" Height="31px" OnClick="btnReport_Click" Text="Generate GSTR Data" Width="150px" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

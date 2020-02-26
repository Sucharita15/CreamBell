<%@ Page Title="Vending Expen" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmVendingExpensesReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmVendingExpensesReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
         function IsValidDate(myDate) {
             var filter = /^([Jj][Aa][Nn]|[Ff][Ee][bB]|[Mm][Aa][Rr]|[Aa][Pp][Rr]|[Mm][Aa][Yy]|[Jj][Uu][Nn]|[Jj][u]l|[aA][Uu][gG]|[Ss][eE][pP]|[oO][Cc][Tt]|[Nn][oO][Vv]|[Dd][Ee][Cc])-(19|20)\d\d$/
    return filter.test(myDate);
}
function ValidateDate(e)
{

    //debugger;
    var isValid = IsValidDate(e.value);
    if (isValid) {
        //alert('Correct format');
    }
    else {
       
        alert('Please Enter The Date In Format: dd-MMM-yyyy');
        e.value = '';
        
    }
    return isValid
}


     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport" />
        </Triggers>
        <ContentTemplate>
            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
                VENDING EXPENSES REPORT
            </div>
            <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
                <table style="width: 100%">

                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtMonth" Format="MMM-yyyy"></asp:CalendarExtender>                    
                    <tr>
                        
                        <td colspan="3"><asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="small"> </asp:Label></td>
                        <td>State</td>
                        <td><asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" Width="120px"></asp:DropDownList></td>
                        <td>Distributor :</td>
                        <td><asp:DropDownList ID="ddlSiteId" runat="server" AutoPostBack="True"  CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px" OnSelectedIndexChanged="ddlSiteId_SelectedIndexChanged"></asp:DropDownList></td>
                        <td>VRS :</td>
                        <td><asp:DropDownList ID="drpVRS" runat="server" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px"></asp:DropDownList></td>
                        <td>Month:</td>
                        <td>
                            <asp:TextBox ID="txtMonth" runat="server" placeholder="MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"  onchange="ValidateDate(this)" required></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <td><asp:Button ID="BtnShowReport" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" /></td>
                    </tr>
                </table>
            </asp:Panel>

            <div style="margin: 10px">
                <asp:Panel ID="PanelReport" runat="server" GroupingText="VENDING EXPENSES REPORT" Width="100%" BackColor="White">
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="500px"></rsweb:ReportViewer>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

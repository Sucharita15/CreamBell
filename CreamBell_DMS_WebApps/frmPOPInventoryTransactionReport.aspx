<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPOPInventoryTransactionReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPOPInventoryTransactionReport" %>
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
     <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
                <span style="font-family: Segoe UI; color: white; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px;">POP Inventory Transaction Report</span>
            </div> 
          <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" style="width: 99%;margin: 0px 0px 0px 10px;" >            
    <table>
       <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
             <tr>
                  
                    <td style="width: 85%">
                        <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="small"> </asp:Label>

                    </td>
                </tr>
    </table>
     <table style="width: 100%; text-align: left">
        <tr>
            <td style="font-weight: 700">
                From Date:
             </td>
            <td>
               <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"  onchange="ValidateDate(this)" required></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
            </td>
            <td style="font-weight: 700">
                To Date:
            </td>
            <td>
                <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"  onchange="ValidateDate(this)" required></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
            </td>
            <td style="font-weight: 700">
                State:
            </td>
            <td>
                <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="true" width="175px" CssClass="textboxStyleNew" OnSelectedIndexChanged="drpState_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td style="font-weight: 700">
                Distributor:
            </td>
            <td>
             <asp:DropDownList ID="ddlSiteId" runat="server"  placeholder="dd-MMM-yyyy" Width="175px" CssClass="textboxStyleNew"></asp:DropDownList> 
            </td>
             <td style="width: 10%; text-align:left">
       <asp:Button ID="BtnShowReport0" runat="server" Text="Show" OnClick="BtnShowReport0_Click" CssClass="ReportSearch" width="70px" />
                    </td>
        </tr>
   </table>
   </asp:Panel>
 <div style="margin:10px">
                 <asp:Panel ID="PanelReport" runat="server" GroupingText="POP Inventory Transaction Report" Width="100%"  BackColor="White" >
             <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="340px"></rsweb:ReportViewer>
                </asp:Panel>
        </div>
     </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Title="POP PURCHASE REGISTER" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPOPPurchaseRegisterReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPOPPurchaseRegisterReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
     <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
      <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
           <asp:PostBackTrigger ControlID="imgBtnExportToExcel" />
                   </Triggers>  
      <ContentTemplate>

    <div style="width: 100%;height: 18px;border-radius: 4px;margin: 5px 0px 0px 10px;background-color: #1564ad;color:white ;padding: 2px 0px 0px 0px; font-weight:bold" >
           POP Purchase Register Report</div>
       
    <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" style="width: 99%;margin: 0px 0px 0px 10px;" >
        <table style="width:100%">           
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
            <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
            <tr>
                <td>From Date :</td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server"  placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px" ></asp:TextBox> 
                    <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>                
                <td>To Date :</td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server"  placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox> 
                    <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>                
                
                <td colspan="3">
                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Large"> </asp:Label>
                </td>
         
                <td>State</td>
                <td>
                    <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" Width="120px">
                    </asp:DropDownList>
                </td>
                <td >Site ID</td>
                <td >
                    <asp:DropDownList ID="ddlSiteId" runat="server" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px">
                    </asp:DropDownList>
                </td>
                
                <td >
                    <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" />
                </td>
               <td>
                    <asp:ImageButton ID="imgBtnExportToExcel" runat="server"  ImageUrl="~/Images/excel-24.ico"  ToolTip="Click To Generate Excel Report" OnClick="imgBtnExportToExcel_Click" style="width: 24px"/>
                </td>
                 </tr>
        </table>
   </asp:Panel>

     <div style="margin:10px">
          <asp:Panel ID="PanelReport" runat="server" GroupingText="POP Purchase Register Report" Width="100%"  BackColor="White" >
             <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="400px"></rsweb:ReportViewer>
          </asp:Panel>
     </div>
     </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

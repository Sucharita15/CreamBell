﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmTargetReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmTargetReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <link href="css/SSheet.css" rel="Stylesheet" type="text/css" />
</asp:Content>
 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdateProgress ID="upprogress" runat="server" AssociatedUpdatePanelID="upshow" DisplayAfter="0">
        <ProgressTemplate>
            <div class="overlay"></div>
            <div class="overlayContent">
                <center>
                    Please Wait...don't close this window until processing is being done.
                     <br />
                    <img src="../../IMAGES/bar.gif" alt="" />
                </center>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport" />
        </Triggers>
      <ContentTemplate>
      <div style="width: 100%;height: 18px;border-radius: 4px;margin: 5px 0px 0px 10px;background-color: #1564ad;color:white ;padding: 2px 0px 0px 0px; font-weight:bold" >
          Expense Report</div>
       
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
             
          <td>State</td>
                <td>
                    <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" Width="120px">
                    </asp:DropDownList>
                </td>
                <td >Site ID</td>
                <td >
                    <asp:DropDownList ID="ddlSiteId" runat="server" AutoPostBack="True"  CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px" OnSelectedIndexChanged="ddlSiteId_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td >Business Unit </td>
                <td >
                    <asp:DropDownList ID="ddlBusinessUnit" runat="server" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px">
                    </asp:DropDownList>
                </td>
                <td >
                    Cliam Cat</td>
                <td >
                    <asp:DropDownList ID="ddlClaimCat" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="ddlClaimCat_SelectedIndexChanged" Width="120px">
                    </asp:DropDownList>
                </td>
                 </tr>
              <tr>
                <td >
                    Claim Sub Cat</td>
                <td>
                    <asp:DropDownList ID="ddlClaimSubCat" runat="server" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px">
                    </asp:DropDownList>
                </td>
               <td>
                    Claim Type</td>
                <td>
                    <asp:DropDownList ID="ddlClaimType" runat="server" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="100px">
                        <asp:ListItem>Select...</asp:ListItem>
                        <asp:ListItem>Sale</asp:ListItem>
                        <asp:ListItem>Purchase</asp:ListItem>
                    </asp:DropDownList>
                </td>
           
                <td >
                    <asp:RadioButton ID="rdoDetail" runat="server" GroupName="RadioButton" Text="Detail Report" Checked="true" />
              </td>
                <td >
                    <asp:RadioButton ID="rdoSummary" runat="server" GroupName="RadioButton" Text="Summary Report" />
                </td>
                <td >
                    <asp:UpdatePanel ID="upshow" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:Button ID="BtnShowReport" runat="server" Text="Show Report" CssClass="ReportSearch" OnClick="BtnShowReport_Click" Height="31px" Width="96px" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
              </td>
                   <td colspan="3">
                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI"> </asp:Label>
                </td>
            </tr>
        </table>
   </asp:Panel>

     <div style="margin:10px">
         <asp:MultiView ID="MultiView1" runat="server">
             <asp:View ID="ReportViewerTargetSummary" runat="server">
                 <asp:Panel ID="PanelReport" runat="server" GroupingText="Target Summary Report" Width="100%"  BackColor="White" >
             <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="340px"></rsweb:ReportViewer>
                </asp:Panel>
             </asp:View>

            <asp:View ID="ReportViewerTargetDetail" runat="server">
                    <asp:Panel ID="Panel1" runat="server" GroupingText="Target Detail Report" Width="100%"  BackColor="White" >
        <rsweb:ReportViewer ID="ReportViewer2" runat="server" Width="100%" BackColor="GhostWhite" Height="340px" AsyncRendering="false"></rsweb:ReportViewer>
                </asp:Panel>
             </asp:View>
         </asp:MultiView>
 </div>
      </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

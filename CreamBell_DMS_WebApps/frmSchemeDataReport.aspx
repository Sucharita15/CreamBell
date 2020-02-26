<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmSchemeDataReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmSchemeDataReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/ucRoleFilters.ascx" TagPrefix="uc1" TagName="ucRoleFilters" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
         $(document).ready(function () {
             $('.single-selection').multiselect({
                 enableFiltering: true,
                 enableCaseInsensitiveFiltering: true,
                 nonSelectedText: 'Select'
             });
         });
        </script>
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/DropDown.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />

     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" >
        <Triggers>
            <asp:PostBackTrigger ControlID="ddlSchemeNew" />
            <asp:PostBackTrigger ControlID="BtnShowReport" />
        </Triggers>
      <ContentTemplate>
    <div style="width: 100%;height: 18px;border-radius: 4px;margin: 5px 0px 0px 10px;background-color: #1564ad;color:white ;padding: 2px 0px 0px 0px; font-weight:bold" >
           Scheme Data Report</div>
          <uc1:ucRoleFilters runat="server" ID="ucRoleFilters" />
    <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" style="width: 99%;margin: 0px 0px 0px 10px;" >
        <table style="width:100%">
           
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
            <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>

            <tr>
                <td>From Date :</td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server"  placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"  ></asp:TextBox> 
                    <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>
                
                <td>To Date :</td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server"  placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox> 
                    <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>
                 <td colspan="3">
                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="small"> </asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblScheme" runat="server" Text="Scheme"></asp:Label>
                </td>
                <td>
                   <%-- <asp:DropDownList ID="ddlScheme" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="ddlScheme_SelectedIndexChanged" Width="120px">
                    </asp:DropDownList>--%>
                      <asp:ListBox ID="ddlSchemeNew" ClientIDMode="Static" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlScheme_SelectedIndexChanged" CssClass="single-selection"></asp:ListBox>

                </td>
                <td >Scheme code</td>
                <td >
                  <%--  <asp:DropDownList ID="ddlSiteId" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px">
                    </asp:DropDownList>--%>

                      <asp:ListBox ID="ddlSiteIdNew" ClientIDMode="Static" runat="server"  CssClass="single-selection"></asp:ListBox>
                </td>
               
                <td >
                    <asp:Button ID="BtnShowReport" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" />
                </td>
        </tr>
        </table>
   </asp:Panel>
          

     <div style="margin:10px">
                 <asp:Panel ID="PanelReport" runat="server" GroupingText="Sale Register Report" Width="100%"  BackColor="White" >
             <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="340px"></rsweb:ReportViewer>
                </asp:Panel>
        </div>
          </ContentTemplate>
    </asp:UpdatePanel>
     
</asp:Content>



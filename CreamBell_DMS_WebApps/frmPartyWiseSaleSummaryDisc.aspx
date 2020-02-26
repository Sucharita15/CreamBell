<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPartyWiseSaleSummaryDisc.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPartyWiseSaleSummaryDisc" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/ucRoleFilters.ascx" TagPrefix="uc1" TagName="ucRoleFilters" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
  <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
  <link href="css/btnSearch.css" rel="stylesheet" />  
  <%--<link href="css/style.css" rel="stylesheet" />--%>
  <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
  <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
  <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>
  <script type="text/javascript">
                $(function () {
                    $('#chkListState1').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select State',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '140px',
                        maxHeight: 300
                    });


                   
                    $('#chkListSite1').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select Site',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '140px',
                        maxHeight: 300,
                        maxWidth: 50
                    });
                    $('#chkListCustomerGroup').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select Group',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '140px',
                        maxHeight: 300,
                        maxWidth: 50
                    });

                    $('#chkProductGroup').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select Product Group',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '140px',
                        maxHeight: 300,
                        maxWidth: 50
                    });

                    $(document).on("click", ".multiselect" , function(e) {
                         $(this).parent().parent().find(".btn-group").toggleClass("open");
                    });

                });
            </script>
    <style>
         .nav ul li a {
            width: 270px !important;
        }
    </style>
    <script src="Javascript/custom.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="imgBtnExportToExcel" />
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
        </Triggers>
        <ContentTemplate>

            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
                Outlet Discount Report
            </div>
            <uc1:ucRoleFilters runat="server" ID="ucRoleFilters" />
            <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
                <table style="width: 100%">
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <tr>
                        <td>From Date :</td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" ></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <asp:PlaceHolder ID="phState" runat="server">
                        <td rowspan="3" style="background-color: aliceblue; vertical-align: top; text-align: left">State :
                   
                       <%-- <asp:CheckBoxList ID="chkListState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkListState_SelectedIndexChanged">
                        </asp:CheckBoxList>--%>

                        <asp:ListBox ID="chkListState1" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="chkListState_SelectedIndexChanged" SelectionMode="Multiple" ClientIDMode="Static" Width="200px"></asp:ListBox>

                   
                        </td>
                        
                        <td rowspan="3" style="background-color: aliceblue; vertical-align: top; text-align: left">Distributor :
                    
                        <%--<asp:CheckBoxList ID="chkListSite" runat="server">
                        </asp:CheckBoxList>--%>

                            <asp:ListBox ID="chkListSite1" runat="server" SelectionMode="Multiple" ClientIDMode="Static" Width="200px"></asp:ListBox>
                    
                        </td>
                       </asp:PlaceHolder>
                        <td rowspan="3" style="background-color: aliceblue; vertical-align: top; text-align: left">Customer Group:
                   
                        <asp:ListBox ID="chkListCustomerGroup" runat="server" SelectionMode="Multiple" ClientIDMode="Static" Width="200px">                           
                        </asp:ListBox>


                 
                        </td>
                       
                        <td rowspan="3" style="background-color: aliceblue; vertical-align: top; text-align: left">Product Group:
                   
                        <asp:ListBox ID="chkProductGroup" runat="server" SelectionMode="Multiple" ClientIDMode="Static" Width="200px">
                             <asp:ListItem Text="BULK FIX" Value="BULK FIX" />
                            <asp:ListItem Text="BULK VARIABLE" Value="BULK VARIABLE" />
                            <asp:ListItem Text="IMP, TAKE HOME (All)" Value="IMP, TAKE HOME (All)" />
                        </asp:ListBox>
                   
                        </td>

                        <td></td>
                        <td>
                            <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" />
                        </td>
                        <td>
                            <asp:ImageButton ID="imgBtnExportToExcel" runat="server" ImageUrl="~/Images/excel-24.ico" ToolTip="Click To Generate Excel Report" OnClick="imgBtnExportToExcel_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>To Date :</td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Small"> </asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="margin: 10px">
                <asp:Panel ID="PanelReport" runat="server" GroupingText="Outlet Discount Report" Width="100%" BackColor="White">
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="340px"></rsweb:ReportViewer>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

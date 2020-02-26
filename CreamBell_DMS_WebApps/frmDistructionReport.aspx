<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmDistructionReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmDistructionReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/ucRoleFilters.ascx" TagPrefix="uc1" TagName="ucRoleFilters" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
         $(document).ready(function () {
             $('#chkSelectionNew').multiselect({
                 includeSelectAllOption: true,
                 nonSelectedText: 'Select Unit',
                 enableFiltering: true,
                 enableCaseInsensitiveFiltering: true,
                 buttonWidth: '140px',
                 maxHeight: 300
             });

             $('#DDLBusinessUnitNew').multiselect({
                 includeSelectAllOption: true,
                 nonSelectedText: 'Select',
                 enableFiltering: true,
                 enableCaseInsensitiveFiltering: true,
                 buttonWidth: '140px',
                 maxHeight: 300
             });



             
         });
        </script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
             <asp:PostBackTrigger ControlID="chkSelectionNew" />
        </Triggers>
        <ContentTemplate>

            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
                Destruction Report
            </div>
            <uc1:ucRoleFilters runat="server" ID="ucRoleFilters" />
            <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
                <table style="width: 100%">
                    
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="small"> </asp:Label>
                        </td>

                    </tr>
                    <tr>
                        <td>From Date :</td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                     
                        <td>
                            <asp:TextBox ID="txtState" runat="server" AutoPostBack="true" Width="100px" ReadOnly="True" CssClass="textboxStyleNew" Visible="False"></asp:TextBox>
                          
                        </td>
                                                                                             
                        <td rowspan="4" style="vertical-align: top; text-align: left; font-size: 12px; " class="auto-style6">
                           <%-- <div style="overflow-y: auto; background-color: aliceblue; width: 140px">
                                <asp:CheckBox ID="chkAllUnit" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllUnit_CheckedChanged" Font-Bold="True" ForeColor="#009933" Text="Select All" />
                                <asp:Panel ID="Panel2" runat="server" Height="80px" ScrollBars="Auto" Width="140px">                               

                                    <asp:ListBox ID="chkSelectionNew" ClientIDMode="Static" runat="server" AutoPostBack="true"
                                        SelectionMode="Multiple" OnSelectedIndexChanged="chkSelection_SelectedIndexChanged" >
                                         <asp:ListItem Value="0" Text="Primary"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="ColdRoom"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Market Return"></asp:ListItem>
                                    </asp:ListBox>


                              
                                </asp:Panel>
                            </div>--%>

                             <asp:ListBox ID="chkSelectionNew" ClientIDMode="Static" runat="server" AutoPostBack="true"
                                        SelectionMode="Multiple" OnSelectedIndexChanged="chkSelection_SelectedIndexChanged" >
                                         <asp:ListItem Value="0" Text="Primary"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="ColdRoom"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Market Return"></asp:ListItem>
                                    </asp:ListBox>
                        </td>
                        <td>Business Unit</td>
                        <td>
                           <%-- <asp:DropDownList ID="DDLBusinessUnit" runat="server" Width="150px"></asp:DropDownList>--%>
                                <asp:ListBox ID="DDLBusinessUnitNew" ClientIDMode="Static" runat="server" CssClass="single-selection"></asp:ListBox>
                        </td>
                        <td>
                            <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport0_Click" Text="Show Report" Width="96px" />
                        </td>
                    </tr>
                    <tr>
                        <td>To Date :</td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                         <td>
                            <asp:TextBox ID="txtSiteId" runat="server" AutoPostBack="true" Width="100px" ReadOnly="True" CssClass="textboxStyleNew" Visible="False"></asp:TextBox>
                    </tr>
                </table>
            </asp:Panel>

            <div style="margin: 10px">
                <asp:Panel ID="PanelReport" runat="server" GroupingText="Destruction Report" Width="100%" BackColor="White">
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="500px"></rsweb:ReportViewer>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

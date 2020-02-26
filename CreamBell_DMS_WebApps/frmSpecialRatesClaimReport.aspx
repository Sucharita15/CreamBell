<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Main.Master" CodeBehind="frmSpecialRatesClaimReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmDiscountClaimCalculationReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/ucRoleFilters.ascx" TagPrefix="uc1" TagName="ucRoleFilters" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
        </Triggers>
        <ContentTemplate>

            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
               Special Rates Claim Report
            </div>

             <table style="width:100%">
              <tr>
                  <td>
                       <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Small"> </asp:Label>
                  </td>
              </tr>
          </table>

             <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
                 <uc1:ucRoleFilters runat="server" ID="ucRoleFilters" />
                <table style="width: 100%">

                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>

                    <tr>
                        <td style="width:10%;text-align:center">From Date :</td>
                        <td style="width:10%;text-align:center">
                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" Width="80px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>

                        <asp:PlaceHolder ID="phState" runat="server">
                            <td rowspan="3" style="background-color:aliceblue; vertical-align:top;text-align:left;width:17%;"> State :
                                <div style=" max-height:80px;overflow-y:auto;">
                                <asp:CheckBoxList ID="chkListState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkListState_SelectedIndexChanged"  >
                                </asp:CheckBoxList>
                                </div>                    
                            </td>   
                            <td rowspan="3" style="background-color:aliceblue; vertical-align:top;text-align:left;width:17%;"> Distributor :
                                <div style=" max-height:80px;overflow-y:auto;">
                                <asp:CheckBoxList ID="chkListSite" runat="server" OnSelectedIndexChanged="chkListSite_SelectedIndexChanged" AutoPostBack="True"  >
                                </asp:CheckBoxList>
                                </div>
                            </td>  
                        </asp:PlaceHolder>
                <td rowspan="2" style="background-color: aliceblue; vertical-align: top; text-align: left; width:16%; ">Customer Group:
                    <div style="max-height: 60px; overflow-y: auto; width: 100%;">
                        <asp:CheckBoxList ID="chkListCustomerGroup" runat="server" >
                        </asp:CheckBoxList>
                    </div>
                        </td>
                        <td rowspan="2" style="background-color: aliceblue; vertical-align: top; text-align: left; width:16%; ">Business Unit:
                    <div style="max-height: 60px; overflow-y: auto; width: 100%;">
                        <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                        </asp:CheckBoxList>
                    </div>
                        </td>
                            <td style="width:10%;text-align:center">
                            <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport0_Click" Text="Show Report" Width="96px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%;text-align:center">To Date :</td>
                        <td style="width:10%;text-align:center">
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" Width="80px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>

                         </tr>
                </table>
            </asp:Panel>

            <div style="margin: 10px">
                <asp:Panel ID="PanelReport" runat="server" GroupingText="Special Rates Claim Report" Width="100%" BackColor="White">
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="500px"></rsweb:ReportViewer>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


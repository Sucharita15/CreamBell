<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmOutStandingAgingReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmOutStandingAgingReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/ucRoleFilters.ascx" TagPrefix="uc1" TagName="ucRoleFilters" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2"  ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
        </Triggers>
        <ContentTemplate>

            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
                OutStanding Report Age Wise
            </div>

            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Small"> </asp:Label>
                    </td>
                </tr>
            </table>

            <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
                <uc1:ucRoleFilters runat="server" ID="ucRoleFilters" />
                <table style="width: 100%">
                    <tr>
                        <td colspan="6">
                            <table style="width: 100%; text-align: left">
                                <tr>
                                    <td style="width: 10%">Age Range</td>
                                    <td style="width: 10%">Range (0-30)  </td>
                                    <td style="width: 10%">
                                        <asp:TextBox ID="txt30" runat="server" CssClass="textboxStyleNew" Width="40px" onkeypress="return IsNumeric(event);" ondrop="return false;" onpaste="return false;" MaxLength="4"></asp:TextBox>
                                        <asp:RangeValidator ID="RangeValidator1" Type="Integer" MinimumValue="0" MaximumValue="30" ControlToValidate="txt30" runat="server" ValidationGroup="ShowReport" ErrorMessage="enter only numbers between 1 and 30" ForeColor="Red" SetFocusOnError="true">*</asp:RangeValidator>
                                    </td>
                                    <td style="width: 10%">Range (31-60)</td>
                                    <td style="width: 10%">
                                        <asp:TextBox ID="txt60" runat="server" CssClass="textboxStyleNew" Width="40px" onkeypress="return IsNumeric(event)" MaxLength="4"></asp:TextBox>
                                         <asp:RangeValidator ID="RangeValidator2" Type="Integer" MinimumValue="31" MaximumValue="60" ControlToValidate="txt60" runat="server" ValidationGroup="ShowReport" ErrorMessage="enter only numbers between 31 and 60" ForeColor="Red" SetFocusOnError="true">*</asp:RangeValidator>

                                    </td>
                                    <td style="width: 10%">Range (61-90)</td>
                                    <td style="width: 10%">
                                        <asp:TextBox ID="txt90" runat="server" CssClass="textboxStyleNew" Width="40px" onkeypress="return IsNumeric(event)" MaxLength="4"></asp:TextBox>
                                          <asp:RangeValidator ID="RangeValidator3" Type="Integer" MinimumValue="61" MaximumValue="90" ControlToValidate="txt90" runat="server" ValidationGroup="ShowReport" ErrorMessage="enter only numbers between 60 and 90" ForeColor="Red" SetFocusOnError="true">*</asp:RangeValidator>

                                    </td>
                                    <td style="width: 10%">Range (91-120)</td>
                                    <td style="width: 10%">
                                        <asp:TextBox ID="txt120" runat="server" CssClass="textboxStyleNew" Width="40px" onkeypress="return IsNumeric(event)" MaxLength="4"></asp:TextBox>
                                        <asp:RangeValidator ID="RangeValidator4" Type="Integer" MinimumValue="91" MaximumValue="120" ControlToValidate="txt120" runat="server" ValidationGroup="ShowReport" ErrorMessage="enter only numbers between 91 and 120" ForeColor="Red" SetFocusOnError="true">*</asp:RangeValidator>

                                    </td>
                                    <td style="width: 10%">Over (120)</td>
                                    <td style="width: 10%">
                                        <asp:TextBox ID="txtOver120" runat="server" CssClass="textboxStyleNew" Width="40px" onkeypress="return IsNumeric(event)" MaxLength="4"></asp:TextBox>
                                          <asp:RangeValidator ID="RangeValidator5" Type="Integer" MinimumValue="121" MaximumValue="1000" ControlToValidate="txtOver120" runat="server" ValidationGroup="ShowReport" ErrorMessage="enter only numbers over 121" ForeColor="Red" SetFocusOnError="true">*</asp:RangeValidator>

                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>

                        <asp:PlaceHolder ID="phState" runat="server">
                            <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 17%;">State :
                                <div style="max-height: 80px; overflow-y: auto;">
                                    <asp:CheckBoxList ID="chkListState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkListState_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                            <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 17%;">Distributor :
                                <div style="max-height: 80px; overflow-y: auto;">
                                    <asp:CheckBoxList ID="chkListSite" runat="server" OnSelectedIndexChanged="chkListSite_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                        </asp:PlaceHolder>
                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 17%;">Customer Group:
                    <div style="max-height: 60px; overflow-y: auto; width: 100%;">
                        <asp:CheckBoxList ID="chkListCustomerGroup" runat="server" OnSelectedIndexChanged="chkListCustomerGroup_SelectedIndexChanged" AutoPostBack="true">
                        </asp:CheckBoxList>
                    </div>
                        </td>

                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 17%;">Customer Name:
                    <div style="max-height: 60px; overflow-y: auto; width: 100%;">
                        <asp:CheckBoxList ID="chkCustomerName" runat="server">
                        </asp:CheckBoxList>
                    </div>

                        </td>

                        <td style="width: 10%; text-align: center">
                            <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport0_Click" Text="Show Report" Width="96px"  ValidationGroup="ShowReport" />
                            <asp:ValidationSummary ID="vSummary" runat="server"  ShowSummary="false" ShowMessageBox="true" ValidationGroup="ShowReport"/>
                        </td>
                    </tr>


                </table>
            </asp:Panel>

            <div style="margin: 10px">
                <asp:Panel ID="PanelReport" runat="server" GroupingText="OutStanding Report Age Wise" Width="100%" BackColor="White">
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="500px"></rsweb:ReportViewer>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

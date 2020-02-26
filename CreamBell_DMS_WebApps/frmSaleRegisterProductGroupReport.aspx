<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmSaleRegisterProductGroupReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmSaleRegisterProductGroupReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
            <asp:PostBackTrigger ControlID="Button1" />
        </Triggers>
        <ContentTemplate>
            <div style="width: 100%; height: 20px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 0px 0px 0px 0px; font-weight: bold">
                <table>
                    <tr>
                        <td runat="server" style="text-align: left" id="tclink">
                            <asp:LinkButton ID="LinkButton1" runat="server" ForeColor="White" OnClick="LinkButton1_Click" style=" text-align:left">Hide sales person filter</asp:LinkButton>
                        </td>
                        
                            <td id="tclabel" runat="server" style="text-align: center">Sale Register Product Group Wise Report</td>
                        
                    </tr>
                </table>
            </div>

            <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 2px;">


                <table style="width: 100%;">
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                <tr>
                        <td colspan="7">
                            <asp:UpdatePanel runat="server" ID="upsale" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="Panel1" runat="server"  GroupingText="Sale Person Filter" Style="width: 99%; margin: 0px 0px 0px 2px;">
                                <table>
                            <tr>
                              
                                 
                                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 11%;">HOS :
                                   <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                         <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="chkAll_CheckedChanged" Text="Select All" />
                         <asp:CheckBoxList ID="chkListHOS" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstHOS_SelectedIndexChanged" >
                            </asp:CheckBoxList>
                        </div>
                     
                            </td>
                                   <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 11%;">VP :
                        <div class="checkboxlistHeader"; style="max-height: 80px; width:150px;overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListVP" runat="server" OnSelectedIndexChanged="lstVP_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                            <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 11%;">GM :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox2_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListGM" runat="server" OnSelectedIndexChanged="lstGM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                                             <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 13%;">DGM :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox3" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox3_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListDGM" runat="server"  OnSelectedIndexChanged="lstDGM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                             <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 14%;">RM :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox4" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox4_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListRM" runat="server"  OnSelectedIndexChanged="lstRM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                                             <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 14%;">ZM :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox5" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox5_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListZM" runat="server" OnSelectedIndexChanged="lstZM_SelectedIndexChanged"  AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                                             <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 14%;">ASM :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox6" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox6_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListASM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstASM_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                                                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 16%;">EXECUTIVE :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox7" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox7_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListEXECUTIVE" runat="server" OnSelectedIndexChanged="lstEXECUTIVE_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                                </tr>   
                            </table>
                            </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="chkListHOS" />
                                <asp:PostBackTrigger ControlID="chkListVP" />
                                <asp:PostBackTrigger ControlID="chkListGM" />
                                <asp:PostBackTrigger ControlID="chkListDGM" />
                                <asp:PostBackTrigger ControlID="chkListRM" />
                                <asp:PostBackTrigger ControlID="chkListZM" />
                                <asp:PostBackTrigger ControlID="chkListASM" />
                               
                                </Triggers>
                            </asp:UpdatePanel>
                            
                        </td>
                    </tr>
                        <tr>
                        <td>From Date :</td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <td>State:</td>
                        <td>
                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px"  OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" Width="200px">
                            </asp:DropDownList>
                        </td>
                             <td>Sales Product Group</td>
                        <td>
                            <asp:DropDownList ID="ddlProductGroup" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="200px">
                            </asp:DropDownList>
                        </td>
                       
                        <td>
                            <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Sale Data Download" Width="130px" Visible="true" />
                        </td>
                       
                    </tr>
                    <tr>
                        <td>To Date :</td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <td>Site ID</td>
                        <td>
                            <asp:DropDownList ID="ddlSiteId" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="200px" OnSelectedIndexChanged="ddlSiteId_SelectedIndexChanged">
                            </asp:DropDownList>

                        </td>
                         <td>PSR:</td>
                        <td>
                            <asp:DropDownList ID="ddlPSR" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="200px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="Button1" runat="server" CssClass="ReportSearch" Height="31px" Text="Master Data Download" Width="130px" OnClick="Button1_Click" Visible="true" />
                        </td>
                    </tr>
                    <tr>
                         <td colspan="7">
                            <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Large"> </asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

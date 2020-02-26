<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmDailySaleTrackingReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmDailySaleTrackingReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="Javascript/DateValidation.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="imgBtnExportToExcel" />
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
            <asp:PostBackTrigger ControlID="LinkButton1" />
            <asp:PostBackTrigger ControlID="chkListHOS" />
            <asp:PostBackTrigger ControlID="chkListVP" />
            <asp:PostBackTrigger ControlID="chkListGM" />
            <asp:PostBackTrigger ControlID="chkListDGM" />
            <asp:PostBackTrigger ControlID="chkListRM" />
            <asp:PostBackTrigger ControlID="chkListZM" />
            <asp:PostBackTrigger ControlID="chkListASM" />
            <asp:PostBackTrigger ControlID="lstSTATE" />
            <asp:PostBackTrigger ControlID="lstDIS" />
            <asp:PostBackTrigger ControlID="lstPSRNAME" />
        </Triggers>
        <ContentTemplate>

            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
                <table>
                    <tr>
                        <td runat="server" id="tclink" style=" text-align:left;">
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" ForeColor="White">Hide sales person filter</asp:LinkButton>
                        </td>
                        <td runat="server" id="tclabel" style=" text-align:center">
                            Daily Sale Tracking
                        </td>
                    </tr>
                </table>
            </div>

            <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
                <table style="width: 100%">
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <tr>
                        <td colspan="12">
                            <asp:Panel ID="Panel1" runat="server" GroupingText="Sale Person Filter" Style="width: 99%; margin: 0px 0px 0px 2px;">
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
                        </td>
                    </tr>
                    <tr>
                        <td>From Date :</td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"  onchange="ValidateDate(this)" required></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <td></td>
                        <td></td>
                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 14%;">STATE :
                    <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                        <asp:CheckBox ID="CheckBox10" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox10_CheckedChanged" Text="Select All" />
                        <asp:CheckBoxList ID="lstSTATE" runat="server" OnSelectedIndexChanged="lstSTATE_SelectedIndexChanged" AutoPostBack="True">
                        </asp:CheckBoxList>
                    </div>
                        </td>
                        
                        
                    
                        
                        <td></td>
                                       
                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 14%;">Distributor : 
                    <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                        <asp:CheckBox ID="CheckBox8" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox8_CheckedChanged" Text="Select All" />
                        <asp:CheckBoxList ID="lstDIS" runat="server" OnSelectedIndexChanged="lstDIS_SelectedIndexChanged" AutoPostBack="True">
                        </asp:CheckBoxList>
                    </div>
                        </td>
                        
                        <td></td>
                                         
                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 14%;">PSR CODE : 
                    <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                        <asp:CheckBox ID="CheckBox9" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox9_CheckedChanged" Text="Select All" />
                        <asp:CheckBoxList ID="lstPSRNAME" runat="server"  AutoPostBack="True">
                        </asp:CheckBoxList>
                    </div>
                        </td>
                        
                        <td>
                            <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" />
                        </td>
                        <td>
                            <asp:ImageButton ID="imgBtnExportToExcel" runat="server" ImageUrl="~/Images/excel-24.ico" ToolTip="Click To Generate Excel Report" OnClick="imgBtnExportToExcel_Click" Style="width: 24px" Visible="false" />
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
                <asp:Panel ID="PanelReport" runat="server" GroupingText="Daily Sale Tracking Report" Width="100%" BackColor="White">
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="340px"></rsweb:ReportViewer>
                </asp:Panel>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

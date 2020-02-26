<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmKeyCustomerSale.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmKeyCustomerSale" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script src="Javascript/DateValidation.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
      <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
          <asp:PostBackTrigger ControlID="LinkButton1" />
            <asp:PostBackTrigger ControlID="chkListHOS" />
            <asp:PostBackTrigger ControlID="chkListVP" />
            <asp:PostBackTrigger ControlID="chkListGM" />
            <asp:PostBackTrigger ControlID="chkListDGM" />
            <asp:PostBackTrigger ControlID="chkListRM" />
            <asp:PostBackTrigger ControlID="chkListZM" />
            <asp:PostBackTrigger ControlID="chkListASM" />
                   </Triggers>  
      <ContentTemplate>
         
    <div style="width: 100%; height: 20px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 0px 0px 0px 0px; font-weight: bold">
                <table>
                    <tr>
                        <td    runat="server" id="tclink" style=" text-align:left;">
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" ForeColor="White">Hide sales person filter</asp:LinkButton>
                        </td>
                        <td runat="server" id="tclabel" style=" text-align:center">
                            Key Customer Sale
                        </td>
                    </tr>
                </table>
            </div>
       
     <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" style="width: 99%;margin: 0px 0px 0px 10px;" >
        <table style="width:100%">
           
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
            <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
            <tr>
                        <td colspan="12">
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
                        </td>
                    </tr>

            <tr>
                <td>From Date :</td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server"  placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"  onchange="ValidateDate(this)" required></asp:TextBox> 
                    <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>
                
                <td>To Date :</td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server"  placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"  onchange="ValidateDate(this)" required></asp:TextBox> 
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
                    <asp:DropDownList ID="ddlSiteId" runat="server" AutoPostBack="true" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px" OnSelectedIndexChanged="ddlSiteId_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>Business Unit</td>
                <td>
                    <asp:DropDownList ID="DDLBusinessUnit" runat="server" Width="120px"></asp:DropDownList>
                </td>
                <td >
                    <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" />
                </td>
          </tr>
        </table>
     </asp:Panel>

     <div style="margin:10px">
                <asp:Panel ID="PanelReport" runat="server" GroupingText="Key Customer Sale" Width="100%"  BackColor="White" >
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="500px"></rsweb:ReportViewer>
                </asp:Panel>
     </div>
 </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

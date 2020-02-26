<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmRetailerStatusreport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmRetailerStatusreport" EnableEventValidation="false" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ccp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Javascript/DateValidation.js" type="text/javascript"></script>
      <script  src="Javascript/index.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.pane--table2').find('.fxdheader').parent('div').addClass('ganerateDiv');    
        });
    </script>

<style type="text/css">

table.fxdheader {
  border-collapse:collapse;
  table-layout:fixed;
}


 table.fxdheader td {
  padding:8px 0px;
  border:1px solid #ddd;
  overflow:hidden;
}

 table.fxdheader th{
  padding:8px 0px;
  border:1px solid #ddd;
  overflow:hidden;
}
.pane-hScroll {
  overflow:auto;
  width:100%;
  background:green;
}
.pane-vScroll {
  overflow-y:auto;
  overflow-x:hidden;
  height:250px;
  background:red;
}

.pane--table2 {
  width:100%;
  overflow-x:scroll;
}
.pane--table2 th, 
.pane--table2 td {
  width: auto;
  min-width: 150px;
}
.pane--table2 tbody {
  overflow-y: scroll;
  overflow-x: hidden;
  display: block;
  height: 310px;
}
.pane--table2 thead {
    display: table-row;
}
.fxdheader tbody::-webkit-scrollbar {
    width:12px;
    background-color: #F5F5F5;
}
.fxdheader tbody::-webkit-scrollbar-thumb {
    /*border-radius:10px;*/
    -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,.3);
    background-color: #1564ad;
}
.fxdheader tbody::-webkit-scrollbar-track {
    -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
    /*border-radius: 10px;*/
    background-color:#F5F5F5;
}
.scroll_track::-webkit-scrollbar {
    width:12px;
    background-color: #F5F5F5;
}
.scroll_track::-webkit-scrollbar-thumb {
    /*border-radius:10px;*/
    -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,.3);
    background-color: #1564ad;
}

.scroll_track::-webkit-scrollbar-track {
    -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
    /*border-radius: 10px;*/
    background-color: #F5F5F5;
}
</style>

   
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
   
     <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
            <asp:PostBackTrigger ControlID="BtnExcel" />
            </Triggers>
        <ContentTemplate>
            <div style="width: 100%; height: 20px; border-radius: 4px; margin: 5px 0px 0px 0px; background-color: #1564ad; color: white; padding: 0px 0px 0px 0px; font-weight: bold">
                <table>
                    <tr>
                        <td runat="server" style=" text-align:center" id="tclabel">
                            Retailer Status Report
                        </td>
                    </tr>
                </table>
            </div>
            
           <div>
               <asp:UpdatePanel ID="pnlupd" runat="server">
                <ContentTemplate>
             <asp:Panel ID="pnlHeader" runat="server"  GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 2px;" >


                <table style="width: 100%; height: 99px; margin:0px 0px 0px 0px">

                    <ccp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></ccp:CalendarExtender>
                    <ccp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></ccp:CalendarExtender>
                    <tr>
                        <td colspan="9">
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
                              
                                <asp:PostBackTrigger ControlID="drpPSR" />
                                <asp:PostBackTrigger ControlID="drpBEAT" />
                                <asp:PostBackTrigger ControlID="drpStatus" />
                                <asp:PostBackTrigger ControlID="ddlState" />
                                <asp:PostBackTrigger ControlID="ddlSiteId" />
                                <asp:PostBackTrigger ControlID="chkListHOS" />
                                <asp:PostBackTrigger ControlID="chkListVP" />
                                <asp:PostBackTrigger ControlID="chkListGM" />
                                <asp:PostBackTrigger ControlID="chkListDGM" />
                                <asp:PostBackTrigger ControlID="chkListRM" />
                                <asp:PostBackTrigger ControlID="chkListZM" />
                                <asp:PostBackTrigger ControlID="chkListASM" />
                              <asp:PostBackTrigger ControlID="chkAll" />
                                <asp:PostBackTrigger ControlID="CheckBox1" />
                                <asp:PostBackTrigger ControlID="CheckBox2" />
                                <asp:PostBackTrigger ControlID="CheckBox3" />
                                <asp:PostBackTrigger ControlID="CheckBox4" />
                                <asp:PostBackTrigger ControlID="CheckBox5" />
                                <asp:PostBackTrigger ControlID="CheckBox6" />
                                <asp:PostBackTrigger ControlID="CheckBox7" />
                                    <asp:PostBackTrigger ControlID="ddlSiteId" />
                                    <asp:PostBackTrigger ControlID="ddlState" />
                                    <asp:PostBackTrigger ControlID="ddlState" />
                            </Triggers>
                            </asp:UpdatePanel>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>From Date :</td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="yyyy-MM-dd" Width="107px" CssClass="textboxStyleNew" Height="11px" onchange="ValidateDate(this)"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>

                        <td class="auto-style11">To Date :</td>
                        <td class="auto-style10">
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="yyyy-MM-dd" Width="107px" CssClass="textboxStyleNew" Height="11px" onchange="ValidateDate(this)"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <td class="auto-style5">State</td>
                        <td class="auto-style6">
                             <asp:DropDownCheckBoxes ID="ddlState" runat="server" AutoPostBack="true"  Width="150" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" CssClass="auto-style13">
                            <Style SelectBoxWidth="170" DropDownBoxBoxHeight="170" > </Style>
                             </asp:DropDownCheckBoxes>
                           
                        </td>
                        <td class="auto-style7">Site ID</td>
                        <td class="auto-style6">
                            <asp:DropDownCheckBoxes ID="ddlSiteId" runat="server" AutoPostBack="true"  Width="150" OnSelectedIndexChanged="ddlSiteId_SelectedIndexChanged" CssClass="auto-style13">
                        <Style SelectBoxWidth="170" DropDownBoxBoxHeight="170" > </Style>
                             </asp:DropDownCheckBoxes>
                            </td>
                        <td><asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" />
                       </td>
                    </tr>
                    <tr>
                        <td>PSR Code</td>
                        <td><asp:DropDownCheckBoxes ID="drpPSR" runat="server" AutoPostBack="true"  Width="150" OnSelectedIndexChanged="drpPSR_SelectedIndexChanged1" CssClass="auto-style13">
                        <Style SelectBoxWidth="170" DropDownBoxBoxHeight="170" > </Style>
                             </asp:DropDownCheckBoxes></td>
                        <td class="auto-style11">Beat Code</td>
                        <td><asp:DropDownCheckBoxes ID="drpBEAT" runat="server" AutoPostBack="true"  Width="150" CssClass="auto-style13">
                        <Style SelectBoxWidth="170" DropDownBoxBoxHeight="170" > </Style>
                             </asp:DropDownCheckBoxes></td>
                        <td>Retailer Status</td>
                        <td><asp:DropDownCheckBoxes ID="drpStatus" runat="server" AutoPostBack="true"  Width="150">
                        <Style SelectBoxWidth="170" DropDownBoxBoxHeight="170" > </Style>
                             </asp:DropDownCheckBoxes></td>
                        <td></td>
                        <td></td>
                        <td>  <asp:Button ID="BtnExcel" runat="server" CssClass="ReportSearch" Height="31px" Text="Export to Excel" Width="96px" OnClick="BtnExcel_Click" /></td>
                        </tr>
                    <tr>
                       
                        <td class="auto-style2"></td>
                        <td class="auto-style3">
                            <asp:DropDownList ID="drpCategory" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px"  Visible="false" Width="120px">
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style11" ></td>
                        <td class="auto-style10">
                            <asp:DropDownList ID="drpSubCategory" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Visible="false" Width="120px">
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style5"></td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="drpProduct" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px"  Visible="false" Width="120px">
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style7"></td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="drpCustGroup" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller"  Visible="false" Width="120px">
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style8"></td>
                    </tr>


                    <tr>
                        <td colspan="3">
                            <asp:Label ID="LblMessage" runat="server" Font-Bold="true" Font-Names="Seoge UI" Font-Size="Large" ForeColor="DarkRed" Text=""> </asp:Label>
                        </td>
                    </tr>


                </table>
            </asp:Panel>
         </ContentTemplate>
             </asp:UpdatePanel>
         </div>

             <div class="pane pane--table2 scroll_track">               
                <asp:UpdatePanel ID="grdpnl" runat="server">
                    <ContentTemplate>
                <asp:Panel ID="Panel2" runat="server" GroupingText="Retailer Status Report" Width="100%" BackColor="White">
                  <asp:GridView ID="GridView2" CssClass="fxdheader" runat="server" GridLines="Horizontal" AutoGenerateColumns="true" BackColor="White"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="0px" CellPadding="3">
                    <EmptyDataTemplate>
                        No Record Found...
                    </EmptyDataTemplate>
                    <FooterStyle CssClass="table-condensed" BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <HeaderStyle BackColor="#05345C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="center" VerticalAlign="Middle" />
                    <RowStyle BackColor="White" ForeColor="#4A3C8C" HorizontalAlign="center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <SortedAscendingCellStyle BackColor="#F4F4FD" />
                    <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                    <SortedDescendingCellStyle BackColor="#D8D8F0" />
                    <SortedDescendingHeaderStyle BackColor="#3E3277" />
                </asp:GridView>
                </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
            </div>
            <div style="margin: 10px">
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Visible="false" BackColor="GhostWhite" Height="500px"></rsweb:ReportViewer>
             </div>
                </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
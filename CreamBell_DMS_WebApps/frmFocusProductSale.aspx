<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmFocusProductSale.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmFocusProductSale" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .auto-style1 {
        width: 60px;
    }
        .auto-style2 {
            width: 60px;
            height: 17px;
        }
        .auto-style4 {
            width: 166px;
        }
    </style>
     <script src="Javascript/DateValidation.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
     <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
      <Triggers>
          <asp:PostBackTrigger ControlID="BtnShowReport0" />
                   </Triggers>  
      <ContentTemplate>
          
        <div style="width: 100%;height: 18px;border-radius: 4px;margin: 5px 0px 0px 10px;background-color: #1564ad;color:white ;padding: 2px 0px 0px 0px; font-weight:bold" >
          <table>
                    <tr>
                        <td id="tclink" runat="server" style="text-align:left;">
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" ForeColor="White">Hide sales person filter</asp:LinkButton>
                        </td>
                        <td runat="server" style=" text-align:center" id="tclabel" >
                           Focus Product Sale Report
                        </td>
                    </tr>
                </table></div>
       
     <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" style="width: 99%;margin: 0px 0px 0px 10px;" >
        <table style="width:100%">
           
            <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
            <asp:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
            <tr>
                        <td colspan="9">
                            <asp:Panel ID="Panel1" runat="server" GroupingText="Sale Person Filter" Style="width: 132%; margin: 0px 0px 0px 2px;">
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
              <td class="auto-style1" style="text-align: left">From Date:</td>
                <td style="text-align: left" class="auto-style4">
                    <asp:TextBox ID="txtFromDate" runat="server"  placeholder="dd-MMM-yyyy" Width="80px" CssClass="textboxStyleNew" Height="11px"  onchange="ValidateDate(this)" required ></asp:TextBox> 
                    <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>
              <td class="auto-style1" style="text-align: left">To Date:</td>
                <td style="text-align: left" class="auto-style4">
                    <asp:TextBox ID="txtToDate" runat="server"  placeholder="dd-MMM-yyyy" Width="80px" CssClass="textboxStyleNew" Height="11px"  onchange="ValidateDate(this)" required ></asp:TextBox> 
                    <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>
                <td rowspan="3" style="background-color:aliceblue; vertical-align:top;text-align:left;border-spacing:0px"> State :
                    <div style=" max-height:80px;overflow-y:auto;">    
                    <asp:CheckBoxList ID="chkListState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkListState_SelectedIndexChanged"  >
                    </asp:CheckBoxList>
                    </div>                    
                </td>   
                <td rowspan="3" style="background-color:aliceblue; vertical-align:top;text-align:left"> Distributor :
                    <div style=" max-height:80px;overflow-y:auto;">
                        <asp:CheckBox ID="CheckBox9" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox9_CheckedChanged" Text="Select All" />
                    <asp:CheckBoxList ID="chkListSite" runat="server" OnSelectedIndexChanged="chkListSite_SelectedIndexChanged" AutoPostBack="True"  >
                    </asp:CheckBoxList>
                    </div>
                </td>                
                 <td rowspan="3" style="background-color:aliceblue; vertical-align:top;text-align:left"> PSR Selection :
                    <div style=" max-height:80px;overflow-y:auto;">
                        <asp:CheckBox ID="CheckBox8" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox8_CheckedChanged" Text="Select All" />
                    <asp:CheckBoxList ID="ChkListPSR" runat="server"  >
                    </asp:CheckBoxList>
                    </div>
                </td>  
                  <td rowspan="3" style="background-color:aliceblue; vertical-align:top;text-align:left;border-spacing:0px"> Item Cat. :
                    <div style=" max-height:80px;overflow-y:auto;">
                    <asp:CheckBoxList ID="ChkItemCat" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ChkItemCat_SelectedIndexChanged"  >
                    </asp:CheckBoxList>
                    </div>
                </td>  
                  <td rowspan="3" style="background-color:aliceblue; vertical-align:top;text-align:left"> Item Sub Cat. :
                    <div style=" max-height:80px;overflow-y:auto;">
                    <asp:CheckBoxList ID="ChkItemSub" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ChkItemSub_SelectedIndexChanged"  >
                    </asp:CheckBoxList>
                    </div>
                </td>  
                  <td rowspan="3" style="background-color:aliceblue; vertical-align:top;text-align:left;width:200px"> Item&nbsp; Name. :
                    <div style=" max-height:80px;overflow-y:auto;">
                    <asp:CheckBoxList ID="ChkItem" runat="server"  >
                    </asp:CheckBoxList>
                    </div>
                </td>  
                <td >
                    <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" />
                </td> 
                
             <tr>
                <td class="auto-style2" style="text-align: left">Display :</td>
                <td class="auto-style4" style="text-align: left">
                    <asp:RadioButton ID="rdoSubCatWise" runat="server" Checked="True" GroupName="rdoDisplay" Text="SubCat_Wise" />
                    &nbsp;<asp:RadioButton ID="rdoItemWise" runat="server" GroupName="rdoDisplay" Text="Item_Wise" />
&nbsp;</td>                                                              
             </tr>                   
             <tr>                                
                <td colspan="3">
                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Large"> </asp:Label>
                </td>
               
             </tr>             
        </table>
   </asp:Panel>
    
    <div style="margin:10px">
         <asp:Panel ID="PanelReport" runat="server" GroupingText="Focus Product Sale Report" Width="100%"  BackColor="White" >
             <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="340px"></rsweb:ReportViewer>
         </asp:Panel>
    </div>
          </table>
           </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

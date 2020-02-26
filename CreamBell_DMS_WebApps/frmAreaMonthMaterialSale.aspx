<%@ Page Title="Area Month Material Sale" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmAreaMonthMaterialSale.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmAreaMonthMaterialSale" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />
  
     <style type="text/css">
        .ReportSearch {
            background: #3498db;
            background-image: -webkit-linear-gradient(top, #3498db, #2980b9);
            background-image: -moz-linear-gradient(top, #3498db, #2980b9);
            background-image: -ms-linear-gradient(top, #3498db, #2980b9);
            background-image: -o-linear-gradient(top, #3498db, #2980b9);
            background-image: linear-gradient(to bottom, #3498db, #2980b9);
            -webkit-border-radius: 0;
            -moz-border-radius: 0;
            border-radius: 0px;
            font-family: Arial;
            color: #ffffff;
            font-size: 11px;
            padding: 5px 7px 6px 8px;
            text-decoration: none;
        }

            .ReportSearch:hover {
                background: #3cb0fd;
                background-image: -webkit-linear-gradient(top, #3cb0fd, #3498db);
                background-image: -moz-linear-gradient(top, #3cb0fd, #3498db);
                background-image: -ms-linear-gradient(top, #3cb0fd, #3498db);
                background-image: -o-linear-gradient(top, #3cb0fd, #3498db);
                background-image: linear-gradient(to bottom, #3cb0fd, #3498db);
                text-decoration: none;
            }

        .auto-style1 {
            height: 27px;
        }

         .auto-style6 {
             width: 86px;
         }
         .auto-style8 {
             width: 34px;
         }
         .auto-style9 {
             width: 164px;
         }
         .auto-style10 {
             width: 99px;
         }
    </style>

     <script  type="text/javascript">

         function IsValidDate(myDate) {
    var filter = /([Jj][Aa][Nn]|[Ff][Ee][bB]|[Mm][Aa][Rr]|[Aa][Pp][Rr]|[Mm][Aa][Yy]|[Jj][Uu][Nn]|[Jj][u]l|[aA][Uu][gG]|[Ss][eE][pP]|[oO][Cc][Tt]|[Nn][oO][Vv]|[Dd][Ee][Cc])-(19|20)\d\d$/
    return filter.test(myDate);
}
function ValidateDate(e)
{

    //debugger;
    var isValid = IsValidDate(e.value);
    if (isValid) {
        //alert('Correct format');
    }
    else {
       
        alert('Please Enter The Date In Format: dd-MMM-yyyy');
        e.value = '';
        
    }
    return isValid
}

     </script>

 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
       <table>
                    <tr>
                        <td id="tclink" runat="server" style="text-align:left;">
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" ForeColor="White">Hide sales person filter</asp:LinkButton>
                        </td>
                        <td runat="server" style=" text-align:center" id="tclabel" >
                            Area Month Material Sale
                        </td>
                    </tr>
                </table>
    </div>
<asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
<table style="width: 100%">
           <tr>
                <td style="width: 60%">
                    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
                        <Triggers>
            
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
                            <table style="width: 100%">

                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtCurrentDate" Format="MMM-yyyy"></asp:CalendarExtender>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtBaseDate" Format="MMM-yyyy"></asp:CalendarExtender>
                                <tr>
                        <td colspan="10">
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
                                    <td>
                                    <td>Current Date :</td>

                                    <td>
                                        <asp:TextBox ID="txtCurrentDate" runat="server" placeholder="MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"  onchange="ValidateDate(this)" required></asp:TextBox>
                                        <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                                    </td>
                                                                        


                                    <td>Base Date :</td>

                                    <td>
                                        <asp:TextBox ID="txtBaseDate" runat="server" placeholder="MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"  onchange="ValidateDate(this)" required></asp:TextBox>
                                        <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                                    </td>
                                        <td >State :</td> <td> <asp:UpdatePanel ID="sdas" runat="server">
                    <Triggers>
            
            <asp:PostBackTrigger ControlID="LinkButton1" />
            <asp:PostBackTrigger ControlID="drpSTATE" />
            <asp:PostBackTrigger ControlID="drpCAT" />
            <asp:PostBackTrigger ControlID="drpSUBCAT" />
                        <asp:PostBackTrigger ControlID="btnGenerateExcel" />
            
            
        </Triggers>
                            <ContentTemplate>
                               <%-- <asp:DropDownCheckBoxes ID="drpSTATE" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="drpSTATE_SelectedIndexChanged"  style="top: 0px; left: 159px"  >
                                              <Style2 SelectBoxWidth="120" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="80" /> 
                                                
                                            </asp:DropDownCheckBoxes>--%>
                               
                                <asp:DropDownList ID="drpSTATE" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="drpSTATE_SelectedIndexChanged"  CssClass="textboxStyleNew" Font-Size="14px" Height="26px" Width="150px"  >
                 
                                    </asp:DropDownList>
                            </ContentTemplate>
                         </asp:UpdatePanel>
                   
                </td>

                                    <td>Site ID</td>
                                    <td>
                                        <asp:DropDownList ID="ddlSiteId" runat="server" CssClass="textboxStyleNew" Font-Size="14px" Height="26px" Width="150px" OnSelectedIndexChanged="ddlSiteId_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                     <td>
                                <asp:Button ID="btnGenerateExcel" runat="server" CssClass="ReportSearch" Height="31px" OnClick="btnGenerateExcel_Click" Text="Generate Excel" Width="96px" />
                            </td>
                                        </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                         </asp:UpdatePanel>
                    
                </td>
               
            </tr>
      <tr>
                <td colspan="3">
                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Large"> </asp:Label>
                </td>
               
            </tr>
 </table>
  <table style="height: 50px; width: 100%;">
            <tr>
                
                <td></td>
                <td class="auto-style10">Category : </td> 
                <td class="auto-style9"> 
                    
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                            <asp:DropDownCheckBoxes ID="drpCAT" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="drpCAT_SelectedIndexChanged" style="top: 0px; left: 159px"  >
                                              <Style2 SelectBoxWidth="120" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="80" /> 
                                                
                             </asp:DropDownCheckBoxes>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                   
                </td>
                <td class="auto-style8"></td>

                <td class="auto-style6">Sub Category : </td>
                 <td> 
                    
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                            <asp:DropDownCheckBoxes ID="drpSUBCAT" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="drpSUBCAT_SelectedIndexChanged" style="top: 0px; left: -132px; width: 252px;"  >
                                              <Style2 SelectBoxWidth="120" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="80" /> 
                                                
                             </asp:DropDownCheckBoxes>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                   
                </td>
                <td>Business Unit</td>
                <td>
                    <asp:DropDownList ID="DDLBusinessUnit" runat="server" Width="120px"></asp:DropDownList>
                </td>
 </tr>
 </table>
    </asp:Panel>
</asp:Content>

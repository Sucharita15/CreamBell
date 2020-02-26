<%--<%@ Page Language="C#" MasterPageFile="~/New.Master"  AutoEventWireup="true" CodeBehind="frmSaleRegisterDateWiseReportNew.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmSaleRegisterDateWiseReportNew" %>--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/New.Master" AutoEventWireup="true" CodeBehind="frmSaleRegisterDateWiseReportNew.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmSaleRegisterDateWiseReportNew" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
   <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
   <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ccp" %>
   <%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
    <script src="Javascript/DateValidation.js" type="text/javascript"></script>
    <link href="css/btnSearch.css" rel="stylesheet" />  
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
	<link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>
                
	<script type="text/javascript">
					
                $(function () {
                    $('#lstState').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select State',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '130px',
                        maxHeight: 300
                    });
                    $('#lstSiteId').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select Site ID',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '130px',
                        maxHeight: 300,
                        maxWidth: 50
                    });
                    $('#lstbu').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select BU',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '130px',
                        maxHeight: 300,
                        maxWidth: 50
                    });
                });
            </script>
    <script src="Javascript/custom.js"></script>

    <script type="text/javascript">
        function test() {
            $(".arrow_box").addClass("arrow_box1")
            // remove a class
            $(".arrow_box").removeClass("arrow_box")
        }
        function test1() {
            $(".arrow_box1").addClass("arrow_box")
            // remove a class
            $(".arrow_box1").removeClass("arrow_box1")
        }
    </script>
    
    <style type="text/css">
    	
        .input1 {
            border-style: none;
            border-color: inherit;
            border-width: 0;
            padding: 10px 5px;
            float: left;
            background: #eee;
            -moz-border-radius: 3px 0 0 3px;
            -webkit-border-radius: 3px 0 0 3px;
            border-radius: 3px 0 0 3px;
        }

        .input1:focus {
                outline: 0;
                background: #fff;
                -moz-box-shadow: 0 0 2px rgba(0,0,0,.8) inset;
                -webkit-box-shadow: 0 0 2px rgba(0,0,0,.8) inset;
                box-shadow: 0 0 2px rgba(0,0,0,.8) inset;
            }

        .input1::-webkit-input-placeholder {
                color: #999;
                font-weight: normal;
                font-style: italic;
            }

        .input1:-moz-placeholder {
                color: #999;
                font-weight: normal;
                font-style: italic;
            }

        .input1:-ms-input-placeholder {
                color: #999;
                font-weight: normal;
                font-style: italic;
            }
          
        .btn-group {
                width: 170px !important;
            }

        .checkboxlistHeader{
                float:left;
            }

        .checkboxlistHeader span{
                display:block;
            }

        .paneltbl table td{
                border:0px solid !important;
                width:158px;
            }

        .paneltbl{
                background:aliceblue;
                float:left;
                width:100%;
            }
        .textboxStyleNew{
            width:90px;
        }
    	.nav ul li a {
    		width: 270px !important;
		}

        .ReportSearch{
            float:inherit !important;
        }

    </style>

  <script type="text/javascript">
        $(document).ready(function () {
            //alert('hello');
            $('.pane--table2').find('.fxdheader').parent('div').addClass('ganerateDiv');
        });
    </script>

<style type="text/css">
*{
    box-sizing:border-box;
 }

.txtLegend fieldset legend:nth-child(1){
    text-align:center;
}

.txtLegend fieldset fieldset legend{
    text-align:left !important;
}
</style>
<script src="Javascript/DateValidation.js" type="text/javascript"></script>

</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPage" runat="server">
     
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">  
        
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
             <asp:PostBackTrigger ControlID="btnExport2Excel" />
        </Triggers>
        <ContentTemplate>
            <div style="width: 100%;height: 18px;border-radius: 4px;margin: 5px 0px 0px 0px;background-color: #1564ad;color:white ;padding: 2px 0px 0px 0px; font-weight:bold" >
          Sale Register Date Wise Report</div>
            <%-- <div style="width: 100%; height: 20px; border-radius: 4px; margin: 5px 0px 0px 0px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold; text-align:center">
                               <div class="col-lg-12">                
                                 <label runat="server" style="text-align:center" id="tclabel">
                                      &nbsp;&nbsp; Sale Register Date Wise Report
                                   </label>

                                </div>      
                    </div>--%>
       </ContentTemplate>
        </asp:UpdatePanel>
     <asp:UpdatePanel ID="pnlupd" runat="server">
                <ContentTemplate>   
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                   
                    <asp:Panel ID="pnlHeader" runat="server"  GroupingText="Filter" CssClass="txtLegend" Style="width: 100%; margin: 0px 0px 0px 0px;">
                    
                            <asp:UpdatePanel runat="server" ID="upsale" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server"  GroupingText="Sale Person Filter" Style="width: 100%; margin: 0px 0px 0px 0px;">                                        
                        
                                   <div class="paneltbl">                                    
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto; width:160px">
                            <span>HOS :</span>
                            <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="chkAll_CheckedChanged" Text="Select All"></asp:CheckBox>
                            <asp:CheckBoxList ID="chkListHOS" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstHOS_SelectedIndexChanged" >
                            </asp:CheckBoxList>
                        </div>
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto; width:160px">
                            <span> VP :</span>
                            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListVP" runat="server" OnSelectedIndexChanged="lstVP_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto; width:160px">
                            <span> GM :</span>
                            <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox2_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListGM" runat="server" OnSelectedIndexChanged="lstGM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto; width:160px">
                            <span>DGM :</span>
                            <asp:CheckBox ID="CheckBox3" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox3_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListDGM" runat="server"  OnSelectedIndexChanged="lstDGM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                           
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto; width:160px">
                            <span> RM :</span>
                            <asp:CheckBox ID="CheckBox4" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox4_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListRM" runat="server"  OnSelectedIndexChanged="lstRM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                           
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto; width:160px">
                            <span> ZM :</span>
                            <asp:CheckBox ID="CheckBox5" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox5_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListZM" runat="server" OnSelectedIndexChanged="lstZM_SelectedIndexChanged"  AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                          
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto; width:160px">
                            <span> ASM :</span>
                            <asp:CheckBox ID="CheckBox6" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox6_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListASM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstASM_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </div>
                            
                         <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto; width:160px">
                           <span>EXECUTIVE :</span>
                            <asp:CheckBox ID="CheckBox7" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox7_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListEXECUTIVE" runat="server" OnSelectedIndexChanged="lstEXECUTIVE_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                        </div>
                           
                        </asp:Panel>
                        </ContentTemplate>
                            <Triggers>                              
                                <asp:PostBackTrigger ControlID="lstState" />
                                <asp:PostBackTrigger ControlID="lstSiteId" />
                                <asp:PostBackTrigger ControlID="lstbu" />
                                <asp:PostBackTrigger ControlID="chkListEXECUTIVE" />
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
                            </Triggers>
                       </asp:UpdatePanel>  
           
                       <div>
                          <label  style="margin-right:4px"> From Date :</label>
                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" CssClass="textboxStyleNew" onchange="ValidateDate(this)" ></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />

                          <label style="margin-left:15px; margin-right:4px"> To Date :</label>
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" CssClass="textboxStyleNew" onchange="ValidateDate(this)"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                           
                           <label style="margin-left:15px; margin-right:8px">State :</label>
                           <asp:ListBox ID="lstState" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" ClientIDMode="Static" CssClass="txt-width"></asp:ListBox>
                             
                           <label style="margin-left:15px; margin-right:8px">Site ID :</label> 
                           <asp:ListBox ID="lstSiteId" runat="server" SelectionMode="Multiple" ClientIDMode="Static" AutoPostBack="true" Width="200px" OnSelectedIndexChanged="lstSiteId_SelectedIndexChanged"></asp:ListBox>                            
                    
                           <label style="margin-left:15px; margin-right:4px">Business Unit : </label>
                           <asp:ListBox ID="lstbu" runat="server" SelectionMode="Multiple" ClientIDMode="Static" Width="200px"></asp:ListBox>                            
                         
						   <td>
								<asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" />
						   </td>
                           <td>
                                <asp:Button ID="btnExport2Excel" runat="server" CssClass="ReportSearch" Text="Export To Excel" Height="31px" OnClick="btnExport2Excel_Click"></asp:Button></td>
                           </td>
                           <td colspan="3">
                                <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="small" style="display:block;"> </asp:Label>
                           </td>
                        </div>
                    </table>
 
           <div style="margin:10px">
                 <asp:Panel ID="PanelReport" runat="server" GroupingText="Sale Register Date Wise Report" Width="100%"  BackColor="White" >
					<rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="99%" BackColor="GhostWhite" Height="400px"></rsweb:ReportViewer>
                </asp:Panel>
        </div>
                        </asp:Panel>
          </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


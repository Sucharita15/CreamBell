<%@ Page Title="VRS Transaction Details" MasterPageFile="~/New.Master"  Language="C#" AutoEventWireup="true"   EnableEventValidation="false" CodeBehind="frmVRSTransactionDetails.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmVRSTransactionDetails" %>
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
                nonSelectedText: 'Select Distributor',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '130px',
                maxHeight: 300,
                maxWidth: 50
            });

            $('#ddlVRSNew').multiselect({
                includeSelectAllOption: true,
                nonSelectedText: 'Select VRS',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '130px',
                maxHeight: 300,
                maxWidth: 50
            });

            $('#DDLSubCategoryNew').multiselect({
                includeSelectAllOption: true,
                nonSelectedText: 'Select Sub Category',
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

        input[type="submit"]{ height:35px !important; }

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
     <script type="text/javascript">

         $(document).ready(function () {
             /*Code to copy the gridview header with style*/
             var gridHeader = $('#<%=gridVRSDetails.ClientID%>').clone(true);
               /*Code to remove first ror which is header row*/
               $(gridHeader).find("tr:gt(0)").remove();
               $('#<%=gridVRSDetails.ClientID%> tr th').each(function (i) {
                   /* Here Set Width of each th from gridview to new table th */
                   $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
               });
               $("#controlHead2").append(gridHeader);
               $('#controlHead2').css('position', 'absolute');
               $('#controlHead2').css('top', '129');

           });

    </script>
</asp:Content>    
      

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
     <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
      <Triggers>
            <asp:PostBackTrigger ControlID="imgBtnExportToExcel" />
           <asp:PostBackTrigger ControlID="BtnSearch" />
          <asp:PostBackTrigger ControlID="ddlSiteId" />
          <asp:PostBackTrigger ControlID="ddlState" />
          <asp:PostBackTrigger ControlID="ddlVRS" />
          
                   </Triggers>  
      <ContentTemplate>
        
 <div style="width: 100%;height: 18px;border-radius: 4px;margin: 10px 0px 0px 5px;background-color: #1564ad;color:white ;padding: 2px 0px 0px 0px; font-weight:bold" >
         VRS Transaction Details
     </div>
          </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="pnlupd" runat="server">
        <ContentTemplate>
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
            <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>

            <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" CssClass="txtLegend" Style="width: 100%; margin: 0px 0px 0px 0px;">

                <asp:UpdatePanel runat="server" ID="upsale" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="VRS-VD ATTENDANCE REPORT" Style="width: 100%; margin: 0px 0px 0px 0px;">

                            <div class="paneltbl">
                                <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto; width: 160px">
                                    <span>HOS :</span>
                                    <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="chkAll_CheckedChanged" Text="Select All"></asp:CheckBox>
                                    <asp:CheckBoxList ID="chkListHOS" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstHOS_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </div>
                                <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto; width: 160px">
                                    <span>VP :</span>
                                    <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Select All" />
                                    <asp:CheckBoxList ID="chkListVP" runat="server" OnSelectedIndexChanged="lstVP_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </div>
                                <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto; width: 160px">
                                    <span>GM :</span>
                                    <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox2_CheckedChanged" Text="Select All" />
                                    <asp:CheckBoxList ID="chkListGM" runat="server" OnSelectedIndexChanged="lstGM_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </div>

                                <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto; width: 160px">
                                    <span>DGM :</span>
                                    <asp:CheckBox ID="CheckBox3" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox3_CheckedChanged" Text="Select All" />
                                    <asp:CheckBoxList ID="chkListDGM" runat="server" OnSelectedIndexChanged="lstDGM_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </div>

                                <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto; width: 160px">
                                    <span>RM :</span>
                                    <asp:CheckBox ID="CheckBox4" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox4_CheckedChanged" Text="Select All" />
                                    <asp:CheckBoxList ID="chkListRM" runat="server" OnSelectedIndexChanged="lstRM_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </div>

                                <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto; width: 160px">
                                    <span>ZM :</span>
                                    <asp:CheckBox ID="CheckBox5" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox5_CheckedChanged" Text="Select All" />
                                    <asp:CheckBoxList ID="chkListZM" runat="server" OnSelectedIndexChanged="lstZM_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </div>

                                <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto; width: 160px">
                                    <span>ASM :</span>
                                    <asp:CheckBox ID="CheckBox6" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox6_CheckedChanged" Text="Select All" />
                                    <asp:CheckBoxList ID="chkListASM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstASM_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </div>

                                <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto; width: 160px">
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
                    <label style="margin-right: 4px">From Date :</label>
                    <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" CssClass="textboxStyleNew" onchange="ValidateDate(this)"></asp:TextBox>
                    <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />

                    <label style="margin-left: 15px; margin-right: 4px">To Date :</label>
                    <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" CssClass="textboxStyleNew" onchange="ValidateDate(this)"></asp:TextBox>
                    <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />



                    <label style="margin-left: 15px; margin-right: 8px">State :</label>
                    <asp:ListBox ID="lstState" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" ClientIDMode="Static" CssClass="txt-width"></asp:ListBox>

                    <label>Distributor :</label>
                    <asp:ListBox ID="lstSiteId" runat="server" SelectionMode="Multiple" ClientIDMode="Static" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlSiteId_SelectedIndexChanged"></asp:ListBox>

                    <label>VRS :</label>
                    <%--<asp:DropDownList ID="ddlVRS" runat="server" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="200px" OnSelectedIndexChanged="ddlVRS_SelectedIndexChanged"></asp:DropDownList>--%>
                    <asp:ListBox ID="ddlVRSNew" runat="server" ClientIDMode="Static" Width="200px"></asp:ListBox>

                
              

          </div>

           <div style="margin-top:10px;" class="col-md-3">
                <%--<td style="text-align:center">VRS</td>
                <td >
                    <asp:DropDownList ID="ddlVRS" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="180px" OnSelectedIndexChanged="ddlVRS_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>--%>
                 <label>Sub Category :</label>                 
              
                   <%-- <asp:DropDownList ID="DDLSubCategory" runat="server" AutoPostBack="True" Width="100px">
                    </asp:DropDownList>--%>
                <asp:ListBox ID="DDLSubCategoryNew" runat="server" ClientIDMode="Static" Width="200px"></asp:ListBox>

                <asp:Button ID="BtnSearch" runat="server" Text="Search" CssClass="ReportSearch" OnClick="BtnSearch_Click"/>
               
                 </div>

               <div style="margin-top:10px;" class="col-md-6">
          
                          
                   
                    <asp:ImageButton ID="imgBtnExportToExcel" CssClass="ddl" runat="server"  ImageUrl="~/Images/excel-24.ico"  ToolTip="Click To Generate Excel Report" OnClick="imgBtnExportToExcel_Click"/>
               
                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkBlue" Font-Names="Seoge UI"> </asp:Label>
                
           
        </div>

   <%-- <div id="controlHead2" style="margin-top:5px; margin-left:5px;padding-right:10px;width: 1600px;"></div>--%>
             <div style="overflow: auto;margin-top:10px; margin-left:5px;padding-right:10px;width: 100%;">
                     
        <asp:GridView runat="server" ID="gridVRSDetails"  ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="true" Width="100%" BackColor="White" 
           BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" > 
               <AlternatingRowStyle BackColor="#CCFFCC" />
            <%--<Columns>
                <asp:BoundField HeaderText="VRS_CODE" DataField="VRS_CODE" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                 <asp:BoundField HeaderText="VRSNAME" DataField="VRSNAME" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="VDCODE" DataField="VDCODE" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="VDNAME" DataField="VDNAME" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>
                
                <asp:BoundField HeaderText="VRSISSUE_NO" DataField="VRSISSUE_NO" >
                <HeaderStyle Width="100px" HorizontalAlign="Left" />
                <ItemStyle Width="100px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="DATE" DataField="DATE" DataFormatString="{0:dd-MMM-yyyy}" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="ORDERTYPE" DataField="ORDERTYPE"  >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="ITEMCODE" DataField="ITEMCODE" >
                <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                <ItemStyle HorizontalAlign="Left" Width="80px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="PRODUCT_NAME" DataField="PRODUCT_NAME" >
                <HeaderStyle Width="200px" HorizontalAlign="Left" />
                <ItemStyle Width="200px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="PRODUCT_NICKNAME" DataField="PRODUCT_NICKNAME" >
                <HeaderStyle Width="150px" HorizontalAlign="Left" />
                <ItemStyle Width="150px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="PRODUCT_GROUP" DataField="PRODUCT_GROUP" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="BOX" DataField="BOX" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" />
                </asp:BoundField>

                <asp:BoundField HeaderText="LTR" DataField="LTR" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" />
                </asp:BoundField>
                                            
            </Columns>--%>
            <EmptyDataTemplate>
                 No Record Found...
             </EmptyDataTemplate>
            <FooterStyle CssClass="table-condensed" BackColor="#B5C7DE" ForeColor="#4A3C8C" />
            <HeaderStyle BackColor="#05345C" Font-Bold="True" ForeColor="#F7F7F7" />
            <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Left" VerticalAlign="Middle" />
            <RowStyle BackColor="White" ForeColor="#4A3C8C" />
            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
            <SortedAscendingCellStyle BackColor="#F4F4FD" />
            <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
            <SortedDescendingCellStyle BackColor="#D8D8F0" />
            <SortedDescendingHeaderStyle BackColor="#3E3277" />
        </asp:GridView>
   
     </div>
                </asp:Panel>
     </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.Master" CodeBehind="frmFreightHandlingReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmFreightHandlingReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ccp" %>

<asp:Content ID="Content" ContentPlaceHolderID="head" runat="server">
  <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
  <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
  <link href="css/btnSearch.css" rel="stylesheet" /> 
  <link href="css/style.css" rel="stylesheet" /> 
  <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
  <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
  <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>
   
   <script  src="Javascript/index.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //alert('hello');
            $('.pane--table2').find('.fxdheader').parent('div').addClass('ganerateDiv');
 
        });
    </script>

    <script type="text/javascript">
                $(function () {
                    $('#lstState').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select State',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '140px',
                        maxHeight: 300
                    });
                    $('#lstSiteId').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select Site',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '140px',
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
                width: 195px !important;
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
        th.ForWidth {
                width: 190px;
            }
         th.ForWidth1 {
                width: 140px;
            }
        .nav ul li a {
            width: 270px !important;
        }

        .option_menu span {
            margin-right: 20px !important;
        }

        .for_left_side{
            margin-right:32px !important;
        }

        .ReportSearch{
            float:left;
        }

        *{
    box-sizing:border-box;
 }

/*.ganerateDiv {
    width: 1600px;
    float: left;
    background:#fff;
}*/


table {
  border-collapse:collapse;
  table-layout:fixed;
}

.pane {}
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
  min-width: 140px;
}

.pane--table2 tbody {
  overflow-y: scroll;
  overflow-x: hidden;
  display: block;
  height: 220px;
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

.txtLegend fieldset legend:nth-child(1){
    text-align:center;
}

.txtLegend fieldset fieldset legend{
    text-align:left !important;
}
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gridViewCustomers.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=gridViewCustomers.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead").append(gridHeader);
            $('#controlHead').css('position', 'absolute');
            $('#controlHead').css('top', '129');

        });

       <%-- $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gridView1.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=gridView1.ClientID%> tr th').each(function (i) {
                 /* Here Set Width of each th from gridview to new table th */
                 $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
             });
             $("#controlHead1").append(gridHeader);
             $('#controlHead1').css('position', 'absolute');
             $('#controlHead1').css('top', '129');
        });--%>
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
        <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGenerate" />
            <asp:PostBackTrigger ControlID="btnExportToExcel" />
            <asp:PostBackTrigger ControlID="btnSearchCustomer" />
            </Triggers>
        <ContentTemplate>
            <div style="width: 100%; height: 20px; border-radius: 4px; margin: 5px 0px 0px 0px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold; text-align:center">
               <div class="col-lg-12">                
                 <label runat="server" style="text-align:center" id="tclabel">
                      &nbsp;&nbsp; Freight & Handling Report
                 </label>
               </div>      
            </div>
            </ContentTemplate>
        </asp:UpdatePanel>
     <asp:UpdatePanel ID="pnlupd" runat="server">
                <ContentTemplate>                    
                            <asp:Panel ID="Panel2" runat="server"  GroupingText="Filter" CssClass="txtLegend" Style="width: 100%; margin: 0px 0px 0px 0px;">
                  
     <asp:UpdatePanel runat="server" ID="upsale" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="Panel1" runat="server"  GroupingText="Sale Person Filter" Style="width: 100%; margin: 0px 0px 0px 0px;">
                                            
                                       <div class="paneltbl">                                    
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto; width:160px;">
                            <span>HOS :</span>
                            <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="chkAll_CheckedChanged" Text="Select All"></asp:CheckBox>
                            <asp:CheckBoxList ID="chkListHOS" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstHOS_SelectedIndexChanged" >
                            </asp:CheckBoxList>
                        </div>
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto; width:160px;">
                            <span> VP :</span>
                            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListVP" runat="server" OnSelectedIndexChanged="lstVP_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto; width:160px;">
                            <span> GM :</span>
                            <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox2_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListGM" runat="server" OnSelectedIndexChanged="lstGM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto; width:160px;">
                            <span>DGM :</span>
                            <asp:CheckBox ID="CheckBox3" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox3_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListDGM" runat="server"  OnSelectedIndexChanged="lstDGM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                           
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto; width:160px;">
                            <span> RM :</span>
                            <asp:CheckBox ID="CheckBox4" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox4_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListRM" runat="server"  OnSelectedIndexChanged="lstRM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                           
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto; width:160px;">
                            <span> ZM :</span>
                            <asp:CheckBox ID="CheckBox5" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox5_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListZM" runat="server" OnSelectedIndexChanged="lstZM_SelectedIndexChanged"  AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                          
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto; width:160px;">
                            <span> ASM :</span>
                            <asp:CheckBox ID="CheckBox6" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox6_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListASM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstASM_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </div>
                            
                         <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto; width:160px;">
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
                                <asp:PostBackTrigger ControlID="rdRunningC" />
                                <asp:PostBackTrigger ControlID="rdBlockC" />
                                <asp:PostBackTrigger ControlID="rdBothC" />
                            </Triggers>
                       </asp:UpdatePanel>


    <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
        <table style="width: 100%">
            <div class="option_menu">
                        State : 
                            <asp:ListBox ID="lstState" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" ClientIDMode="Static" CssClass="txt-width"></asp:ListBox>
                             
                       Site ID : 
                            <asp:ListBox ID="lstSiteId" runat="server" SelectionMode="Multiple" ClientIDMode="Static" Width="200px"></asp:ListBox>
                            

                       Search Customer By :
                        <asp:DropDownList ID="DDLSearchType" runat="server" CssClass="multiselect dropdown-toggle btn btn-default" data-toggle="dropdown"  Style="text-align: right; width:200px;">
                            <asp:ListItem>Customer Code</asp:ListItem>
                            <asp:ListItem>Customer Name</asp:ListItem>
                            <%--<asp:ListItem>PSR Code</asp:ListItem>--%>
                        </asp:DropDownList>
                           </div>
                   
                        <div class="for_left_side">
                            <asp:TextBox ID="txtSearch" runat="server" placeholder="Search here..." Width="150px" style="float:left;" CssClass="multiselect dropdown-toggle btn btn-default" />
                            <span id="span1" onmouseover="test()" onmouseout="test1()">
                                <asp:Button ID="btnSearchCustomer" runat="server" Style="margin: 0px 6px 0px 0px" Height="31px" class="ReportSearch" Text="Search"
                                    OnClick="btnSearchCustomer_Click"></asp:Button>
                            </span>
                        
                         <asp:Button ID="btnGenerate" runat="server" CssClass="ReportSearch" Height="31px" OnClick="btnGenerate_Click" Text="Generate" Width="96px" />
                         <asp:Button ID="btnExportToExcel" runat="server" CssClass="ReportSearch" Height="31px" OnClick="btnExportToExcel_Click" Text="Export To Excel" Width="96px" />
                         
                        </div>
                      
                       
                       <div class="radio_button">
                       <b>Filter Freight & Handling by status :&nbsp;</b>
                            <asp:RadioButton ID="rdRunningC" runat="server" AutoPostBack="true" Text="Running" ToolTip="RunningCustomers" Checked="true" ValidationGroup="Customers" GroupName="RdCustomers" OnCheckedChanged="rdRunningC_CheckedChanged" />
                            <asp:RadioButton ID="rdBlockC" runat="server" AutoPostBack="true" Text="Expired" CheToolTip="BlockCustomers" ValidationGroup="Customers" GroupName="RdCustomers" OnCheckedChanged="rdRunningC_CheckedChanged" />
                            <asp:RadioButton ID="rdBothC" runat="server" AutoPostBack="true" Text="Both" ToolTip="BothCustomers" ValidationGroup="Customers" GroupName="RdCustomers" OnCheckedChanged="rdRunningC_CheckedChanged" />
                       </div>
            <div>
                 <td colspan="3">
                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="small"> </asp:Label>
                </td>
            </div>
        </table>
    </asp:Panel>
                                </asp:Panel>                   
                </ContentTemplate>
            </asp:UpdatePanel>
    <div class="pane pane--table2 scroll_track">         
              <asp:GridView ID="gridViewCustomers" runat="server" ShowFooter="false" AutoGenerateColumns="False"
                 Width="100%" BackColor="White" BorderWidth="0"  CellPadding="3" ShowHeaderWhenEmpty="True"
                  CssClass="fxdheader">

            <AlternatingRowStyle BackColor="#CCFFCC" />
            <Columns>
                <asp:BoundField HeaderText="STATE" DataField="STATE">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="SITE CODE" DataField="SITE CODE">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="SITE NAME" DataField="SITE NAME">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="CUSTOMER CODE" DataField="CUSTOMER CODE">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="CUSTOMER NAME" DataField="CUSTOMER NAME" >
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left"  CssClass="itemfeild"  />
                </asp:BoundField>
                <asp:BoundField DataField="CUSTOMER GROUP CODE" HeaderText="CUSTOMER GROUP CODE">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="CUSTOMER GROUP NAME" HeaderText="CUSTOMER GROUP NAME">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="CUSTOMER STATUS" HeaderText="CUSTOMER STATUS">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField DataField="DISTANCE KM" HeaderText="DISTANCE KM">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" cssclass="ForWidth SecondWidth" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="BU CODE" HeaderText="BU CODE">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="BU DESCRIPTION" HeaderText="BU DESCRIPTION">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="FREIGHT %" DataField="FREIGHT %">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>

                 <asp:BoundField HeaderText="FREIGHT FIX" DataField="FREIGHT FIX">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                 <asp:BoundField HeaderText="FREIGHT LTR %" DataField="FREIGHT LTR %">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                 <asp:BoundField HeaderText="FREIGHT LTR FIX" DataField="FREIGHT LTR FIX">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                 <asp:BoundField HeaderText="HANDLING %" DataField="HANDLING %">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="HANDLING FIX" DataField="HANDLING FIX">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                 <asp:BoundField HeaderText="FROM DATE" DataField="FROM DATE" DataFormatString="{0:dd-MM-yyyy}">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                 <asp:BoundField HeaderText="TO DATE" DataField="TO DATE" DataFormatString="{0:dd-MM-yyyy}">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                 <asp:BoundField HeaderText="STATUS" DataField="STATUS">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>

            </Columns>
            <EmptyDataTemplate>
                No Record Found...
            </EmptyDataTemplate>
            <FooterStyle BackColor="#bfbfbf" />
            <HeaderStyle BackColor="#05345C" ForeColor="#000000" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#007DBB" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#00547E" />
        </asp:GridView>
    </div>
    
    <%--<div style="overflow: auto; margin-top: 10px; margin-left: 5px; padding-right: 10px; height: 220px">

        <asp:GridView ID="gridView1" runat="server" AutoGenerateColumns="False" Width="100%"
            BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">

            <AlternatingRowStyle BackColor="#CCFFCC" />
            <Columns>
                <asp:BoundField HeaderText="ITEMID" DataField="ITEMID">
                    <HeaderStyle HorizontalAlign="Left" />
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="GROUP" DataField="GROUP">
                    <HeaderStyle HorizontalAlign="Left" />
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="ITEMNAME" DataField="ITEMNAME">
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="GROUPNAME" DataField="GROUPNAME">
                    <HeaderStyle HorizontalAlign="Left" />
                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>

            </Columns>
            <EmptyDataTemplate>
                No Record Found...
            </EmptyDataTemplate>
            <FooterStyle BackColor="#bfbfbf" />
            <HeaderStyle BackColor="#05345C" ForeColor="#000000" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#007DBB" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#00547E" />
        </asp:GridView>
    </div>--%>
        <script  src="Javascript/index.js"></script>
</asp:Content>

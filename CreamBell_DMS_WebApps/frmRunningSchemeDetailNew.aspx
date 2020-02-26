<%--<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Main.Master" CodeBehind="frmRunningSchemeDetailNew.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmRunningSchemeDetailNew" %>--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/New.Master" AutoEventWireup="true" CodeBehind="frmRunningSchemeDetailNew.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmRunningSchemeDetailNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ccp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
  <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
  <link href="css/btnSearch.css" rel="stylesheet" />  
  <%--<link href="css/style.css" rel="stylesheet" />--%>
  <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
  <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
  <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>
   
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
                        nonSelectedText: 'Select Site ID',
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

table {
  border-collapse:collapse;
  table-layout:fixed;
}

 table th,table td {
  padding:8px 0px;
  border:1px solid #ddd;  
  overflow:hidden;
  white-space:nowrap;
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

.txtLegend fieldset legend:nth-child(1){
    text-align:center;
}

.txtLegend fieldset fieldset legend{
    text-align:left !important;
}
</style>
<script src="Javascript/DateValidation.js" type="text/javascript"></script>

    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
         .nav ul li a {
            width: 270px !important;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gvSchemeDetail.ClientID%>').clone(true);
              /*Code to remove first ror which is header row*/
              $(gridHeader).find("tr:gt(0)").remove();
              $('#<%=gvSchemeDetail.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead").append(gridHeader);
            $('#controlHead').css('position', 'absolute');
            $('#controlHead').css('top', '129');
          });


        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gridViewSlabDetail.ClientID%>').clone(true);
              /*Code to remove first ror which is header row*/
              $(gridHeader).find("tr:gt(0)").remove();
              $('#<%=gridViewSlabDetail.ClientID%> tr th').each(function (i) {
                  /* Here Set Width of each th from gridview to new table th */
                  $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
              });
              $("#controlHead0").append(gridHeader);
              $('#controlHead0').css('position', 'absolute');
              $('#controlHead0').css('top', '129');

          });


          $(document).ready(function () {
              /*Code to copy the gridview header with style*/
              var gridHeader = $('#<%=gridViewSchemeItemGroup.ClientID%>').clone(true);
              /*Code to remove first ror which is header row*/
              $(gridHeader).find("tr:gt(0)").remove();
              $('#<%=gridViewSchemeItemGroup.ClientID%> tr th').each(function (i) {
                  /* Here Set Width of each th from gridview to new table th */
                  $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
              });
              $("#controlHead1").append(gridHeader);
              $('#controlHead1').css('position', 'absolute');
              $('#controlHead1').css('top', '129');

          });

          $(document).ready(function () {
              /*Code to copy the gridview header with style*/
              var gridHeader = $('#<%=gridViewFreeItemGroup.ClientID%>').clone(true);
              /*Code to remove first ror which is header row*/
              $(gridHeader).find("tr:gt(0)").remove();
              $('#<%=gridViewFreeItemGroup.ClientID%> tr th').each(function (i) {
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
            <asp:PostBackTrigger ControlID="btnGenerate" />
        </Triggers>
        <ContentTemplate>
            <div style="width: 100%;height: 18px;border-radius: 4px;margin: 5px 0px 0px 0px;background-color: #1564ad;color:white ;padding: 2px 0px 0px 0px; font-weight:bold" >
                <div class="col-lg-12">                
                    <%--<td runat="server" style="text-align:center" id="tclabel">Customer Party Master</td>--%>
                    <label runat="server" style="text-align:center" id="tclabel">Running Scheme Detail</label>
                </div>                           
            </div>
            
            <asp:UpdatePanel ID="pnlupd" runat="server">
                <ContentTemplate>      
                            <asp:Panel ID="pnlHeader" runat="server"  GroupingText="Filter" CssClass="txtLegend" Style="width: 100%; margin: 0px 0px 0px 0px;">
                  
                        <asp:UpdatePanel runat="server" ID="upsale" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="Panel3" runat="server"  GroupingText="Sale Person Filter" Style="width: 100%; margin: 0px 0px 0px 0px;">                                        
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
                          
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;  width:160px;">
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
                            </Triggers>
                       </asp:UpdatePanel>                  
                        
                       <div class="option_menu">
                           State : <asp:ListBox ID="lstState" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" ClientIDMode="Static" CssClass="txt-width"></asp:ListBox>
                             
                           Site ID : <asp:ListBox ID="lstSiteId" runat="server" SelectionMode="Multiple" ClientIDMode="Static" Width="200px" OnSelectedIndexChanged="lstSiteId_SelectedIndexChanged"></asp:ListBox>                            
                       </div>
                   
                       <div class="for_left_side">
                            <asp:Button ID="btnGenerate" runat="server" CssClass="ReportSearch" Height="31px" OnClick="btnGenerate_Click" Text="Generate" Width="96px" />
                       </div>
                            <div style="width: 30%;text-align:right" colspan="3">
                            <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="small"> </asp:Label>
                        </div>                 
                     
                    </asp:Panel>                   
                </ContentTemplate>
            </asp:UpdatePanel>
             
   <%-- <div style="width: 98%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #0085ca; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px; color: #FFFFFF;">Running Scheme Detail</span>
    </div>
    <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 98%; margin: 0px 0px 0px 10px;">
                <table style="width: 100%">

                    <tr>
                        <td style="width: 5%;text-align:left">State</td>
                        <td style="width: 15%;text-align:left">
                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" Width="180px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 5%;text-align:left">Distributor</td>
                        <td style="width: 25%;text-align:left">
                            <asp:DropDownList ID="ddlSiteId" runat="server" AutoPostBack="True"  CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="90%" OnSelectedIndexChanged="ddlSiteId_SelectedIndexChanged" >
                            </asp:DropDownList>
                        </td>
                       
                        <td style="width: 10%;text-align:center">
                            <asp:Button ID="btnGenerate" runat="server" CssClass="ReportSearch" Height="31px" OnClick="btnGenerate_Click" Text="Generate" Width="96px" />
                        </td>
                        <td style="width: 30%;text-align:right" colspan="3">
                            <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="small"> </asp:Label>
                        </td>
                    </tr>
                   
                </table>
            </asp:Panel>--%>
    <table>
        <tr>
            <td>
                <asp:Panel ID="pnlSchemeDetail" runat="server" GroupingText="Scheme Details" Width="650px">
                    <div id="controlHead" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 600px"></div>
                    <div style="overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; height: 138px; width: 600px">
                        <asp:GridView ID="gvSchemeDetail" runat="server" AutoGenerateColumns="False"
                            BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" Width="600px">
                            <AlternatingRowStyle BackColor="#CCFFCC" />
                            <Columns>
                                <asp:TemplateField HeaderText="SchemeCode">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnSchemeDetail" runat="server" OnClick="lnkbtnSchemeDetail_Click" Text='<%# Bind("SCHEMECODE") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Scheme Name" />
                                <asp:BoundField DataField="SCHEME TYPE" HeaderText="Scheme Type" />
                                <asp:BoundField DataField="STARTINGDATE" HeaderText="Valid From " DataFormatString="{0:dd/MMM/yyyy}" />
                                <asp:BoundField DataField="ENDINGDATE" HeaderText="Valid To" DataFormatString="{0:dd/MMM/yyyy}" />
                            </Columns>
                            <EmptyDataTemplate>
                                No Record Found...
                            </EmptyDataTemplate>
                            <FooterStyle BackColor="#bfbfbf" />
                            <HeaderStyle BackColor="#05345C" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>

            <td>
                <asp:Panel ID="Pnlslabdetails" runat="server" GroupingText="Slab Details" Width="650px">
                    <div id="controlHead0" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 600px">
                    </div>
                    <div style="overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; height: 138px; width: 600px">
                        <asp:GridView ID="gridViewSlabDetail" runat="server" AutoGenerateColumns="False" Width="600px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                            <AlternatingRowStyle BackColor="#CCFFCC" />
                            <Columns>

                                <asp:TemplateField HeaderText="SlabDetail">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtngridViewSlabDetail" runat="server" Text='<%# Bind("slabdetail") %>' OnClick="lnkbtngridViewSlabDetail_Click"></asp:LinkButton>
                                        <asp:HiddenField ID="hiddenSchemeItemGroup" Visible="false" runat="server" Value='<%# Eval("Scheme Item group") %>' />
                                        <asp:HiddenField ID="HiddenFieldSchemeLineNo" Visible="false" runat="server" Value='<%# Eval("SchemeLineNo") %>' />
                                        <asp:HiddenField ID="HiddenFieldSchemeCode" Visible="false" runat="server" Value='<%# Eval("SCHEMECODE") %>' />
                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="CustType" DataField="TYPE" />
                                <asp:TemplateField HeaderText="Item Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSchemeItemType" runat="server" Text='<%# Bind("[Scheme Item Type]") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Sales Type" HeaderText="SalesType" />
                                <asp:BoundField HeaderText="Description" DataField="Name">
                                    <ControlStyle Width="150px" />
                                    <ItemStyle Width="150px" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Buying Qty Box" DataField="Buying Quantity Box" DataFormatString="{0:n2}" />
                                <asp:BoundField DataField="Buying Quantity PCS" DataFormatString="{0:n2}" HeaderText="Buying Qty PCS" />
                            </Columns>
                            <EmptyDataTemplate>
                                No Record Found...
                            </EmptyDataTemplate>
                            <FooterStyle BackColor="#bfbfbf" />
                            <HeaderStyle BackColor="#05345C" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>


<td>
            <asp:Panel ID="Panel1" runat="server" GroupingText="Minimum Purchase" Width="650px">
                    <div id="controlHead3" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 600px">
                    </div>
                    <div style="overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; height:79px; width: 600px">

                        <asp:GridView ID="gridView1" runat="server" Width="600px" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC"
                            BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                            <AlternatingRowStyle BackColor="#CCFFCC" />
                            <Columns>
                                <asp:BoundField DataField="MINIMUMPURCHASEITEMTYPE" HeaderText="PURCHITEMTYPE" />
                                <asp:BoundField DataField="MINIMUMPURCHASEITEMGROUP" HeaderText="PURCHITEMGROUP" />
                                <asp:BoundField DataField="MINIMUMPURCHASEITEMDESCRIPTION" HeaderText="PURCHITEMDESCR" />
                                <asp:BoundField DataField="MINIMUMPURCHASEBOX" HeaderText="PURCHASEBOX" />
                                <asp:BoundField DataField="MINIMUMPURCHASEPCS" HeaderText="PURCHASEPCS" />
                            </Columns>
                            <FooterStyle BackColor="#bfbfbf" />
                            <HeaderStyle BackColor="#05345C" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>

                    </div>
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="Panel2" runat="server" GroupingText="Additional Discount" Width="650px">
                    <div id="controlHead4" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 600px">
                    </div>
                    <div style="overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; height:79px; width: 600px">

                        <asp:GridView ID="gridView2" runat="server" Width="600px" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC"
                            BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                            <AlternatingRowStyle BackColor="#CCFFCC" />
                            <Columns>
                                <asp:BoundField DataField="ADDITIONDISCOUNTITEMTYPE" HeaderText="DISCITEMTYPE" />
                                <asp:BoundField DataField="ADDITIONDISCOUNTITEMGROUP" HeaderText="DISCITEMGROUP" />
                                <asp:BoundField DataField="ADDITIONDISCOUNTITEMGROUPDESC" HeaderText="DISCITEMGROUPDESCR" />
                                <asp:BoundField DataField="ADDITIONDISCOUNTPERCENT" HeaderText="DISC%" />
                                <asp:BoundField DataField="ADDITIONDISCOUNTVALUEOFF" HeaderText="DISCVALUEOFF" />
                            </Columns>
                            <FooterStyle BackColor="#bfbfbf" />
                            <HeaderStyle BackColor="#05345C" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>

                    </div>
                </asp:Panel>
            </td>
            </tr>

        <tr>
            <td>
                <asp:Panel ID="pnlitemdetails" runat="server" GroupingText="Item Details" Width="650px">
                    <div id="controlHead1" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 600px">
                    </div>
                    <div style="overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; height: 128px; width: 600px">
                        <asp:GridView ID="gridViewSchemeItemGroup" runat="server" Width="600px" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                            <AlternatingRowStyle BackColor="#CCFFCC" />
                            <Columns>
                                <asp:BoundField HeaderText="ProductGroup" DataField="PRODUCT GROUP" />
                                <asp:BoundField HeaderText="Product Sub Category" DataField="ProductSubCat" />
                                <asp:BoundField HeaderText="ItemCode" DataField="ITEMID" />
                                <asp:BoundField HeaderText="Item Description" DataField="ITEMNAME" />
                            </Columns>
                            <FooterStyle BackColor="#bfbfbf" />
                            <HeaderStyle BackColor="#05345C" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>

            <td>
                <asp:Panel ID="pnlfreeitemdetails" runat="server" GroupingText="Free Item Details" Width="650px">
                    <div id="controlHead2" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 600px">
                    </div>
                    <div style="overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; height: 128px; width: 600px">

                        <asp:GridView ID="gridViewFreeItemGroup" runat="server" Width="600px" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC"
                            BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                            <AlternatingRowStyle BackColor="#CCFFCC" />
                            <Columns>
                                <asp:BoundField HeaderText="ProductGroup" DataField="PRODUCT_GROUP" />
                                <asp:BoundField HeaderText="Product Sub Category" DataField="ProductSubCat" />
                                <asp:BoundField HeaderText="ItemCode" DataField="ITEMID" />
                                <asp:BoundField HeaderText="Item Description" DataField="PRODUCT_NAME" />
                                <asp:BoundField DataField="FREEQTY" HeaderText="Free Qty Box" />
                                <asp:BoundField DataField="FREEQTYPCS" HeaderText="Free Qty PCS" />
                                <asp:BoundField DataField="SETNO" HeaderText="SET NO" />
                                <asp:BoundField DataField="PERCENTSCHEME" HeaderText="SCHEME%" />
                                <asp:BoundField DataField="SCHEMEVALUEOFF" HeaderText="SCHEMEVALUEOFF" />
                            </Columns>
                            <FooterStyle BackColor="#bfbfbf" />
                            <HeaderStyle BackColor="#05345C" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>

                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>
             </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmSaleOrderDetailsNew.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmSaleOrderDetailsNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
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

    <%-- <script type="text/javascript">

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

        $(document).ready(function () {
            $("#gridViewCustomers tbody").addClass('scroll_track');
        });
    </script>--%>

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

        .checkboxlistHeader {
            float: left;
        }

            .checkboxlistHeader span {
                display: block;
            }

        .paneltbl table td {
            border: 0px solid !important;
            width: 158px;
        }

        .paneltbl {
            background: aliceblue;
            float: left;
            width: 100%;
        }

        .ReportSearch {
            float: left !important;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            //alert('hello');
            $('.pane--table2').find('.fxdheader').parent('div').addClass('ganerateDiv');
        });
    </script>

    <style type="text/css">
        * {
            box-sizing: border-box;
        }

        table {
            border-collapse: collapse;
            table-layout: fixed;
        }

            table th, table td {
                padding: 8px 0px;
                border: 1px solid #ddd;
                overflow: hidden;
                white-space: nowrap;
            }

        .pane {
        }

        .pane-hScroll {
            overflow: auto;
            width: 100%;
            background: green;
        }

        .pane-vScroll {
            overflow-y: auto;
            overflow-x: hidden;
            height: 250px;
            background: red;
        }

        .pane--table2 {
            width: 100%;
            overflow-x: scroll;
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
            width: 12px;
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
            background-color: #F5F5F5;
        }

        .scroll_track::-webkit-scrollbar {
            width: 12px;
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

        .txtLegend fieldset legend:nth-child(1) {
            text-align: center;
        }

        .txtLegend fieldset fieldset legend {
            text-align: left !important;
        }

        th.widthTh {
            min-width: 246px;
        }

        .nav ul li a {
            width: 270px !important;
        }
    </style>
    <script src="Javascript/DateValidation.js" type="text/javascript"></script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPage" runat="server">

    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">

        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport2Excel" />
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
            <%--<asp:PostBackTrigger ControlID="btnSearchCustomer" />--%>
        </Triggers>
        <ContentTemplate>
            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 0px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold; text-align: center">
                <div class="col-lg-12">
                    <%--<td runat="server" style="text-align:center" id="tclabel">Customer Party Master</td>--%>
                    <label runat="server" style="text-align: center" id="tclabel">Sale Order Details</label>
                </div>
            </div>

            <asp:UpdatePanel ID="pnlupd" runat="server">
                <ContentTemplate>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>

                    <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" CssClass="txtLegend" Style="width: 100%; margin: 0px 0px 0px 0px;">

                        <asp:UpdatePanel runat="server" ID="upsale" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server" GroupingText="Sale Person Filter" Style="width: 100%; margin: 0px 0px 0px 0px;">
                                    <div class="paneltbl">
                                        <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto; width: 160px;">
                                            <span>HOS :</span>
                                            <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="chkAll_CheckedChanged" Text="Select All"></asp:CheckBox>
                                            <asp:CheckBoxList ID="chkListHOS" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstHOS_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </div>
                                        <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto; width: 160px;">
                                            <span>VP :</span>
                                            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Select All" />
                                            <asp:CheckBoxList ID="chkListVP" runat="server" OnSelectedIndexChanged="lstVP_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </div>
                                        <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto; width: 160px;">
                                            <span>GM :</span>
                                            <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox2_CheckedChanged" Text="Select All" />
                                            <asp:CheckBoxList ID="chkListGM" runat="server" OnSelectedIndexChanged="lstGM_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </div>

                                        <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto; width: 160px;">
                                            <span>DGM :</span>
                                            <asp:CheckBox ID="CheckBox3" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox3_CheckedChanged" Text="Select All" />
                                            <asp:CheckBoxList ID="chkListDGM" runat="server" OnSelectedIndexChanged="lstDGM_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </div>

                                        <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto; width: 160px;">
                                            <span>RM :</span>
                                            <asp:CheckBox ID="CheckBox4" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox4_CheckedChanged" Text="Select All" />
                                            <asp:CheckBoxList ID="chkListRM" runat="server" OnSelectedIndexChanged="lstRM_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </div>

                                        <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto; width: 160px;">
                                            <span>ZM :</span>
                                            <asp:CheckBox ID="CheckBox5" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox5_CheckedChanged" Text="Select All" />
                                            <asp:CheckBoxList ID="chkListZM" runat="server" OnSelectedIndexChanged="lstZM_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </div>

                                        <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto; width: 160px;">
                                            <span>ASM :</span>
                                            <asp:CheckBox ID="CheckBox6" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox6_CheckedChanged" Text="Select All" />
                                            <asp:CheckBoxList ID="chkListASM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstASM_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </div>

                                        <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto; width: 160px;">
                                            <span>EXECUTIVE :</span>
                                            <asp:CheckBox ID="CheckBox7" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox7_CheckedChanged" Text="Select All" />
                                            <asp:CheckBoxList ID="chkListEXECUTIVE" runat="server" OnSelectedIndexChanged="lstEXECUTIVE_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                    <%--<div class="paneltbl">                                    
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <span>HOS :</span>
                            <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="chkAll_CheckedChanged" Text="Select All"></asp:CheckBox>
                            <asp:CheckBoxList ID="chkListHOS" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstHOS_SelectedIndexChanged" >
                            </asp:CheckBoxList>
                        </div>
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <span> VP :</span>
                            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListVP" runat="server" OnSelectedIndexChanged="lstVP_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <span> GM :</span>
                            <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox2_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListGM" runat="server" OnSelectedIndexChanged="lstGM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <span>DGM :</span>
                            <asp:CheckBox ID="CheckBox3" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox3_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListDGM" runat="server"  OnSelectedIndexChanged="lstDGM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                           
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <span> RM :</span>
                            <asp:CheckBox ID="CheckBox4" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox4_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListRM" runat="server"  OnSelectedIndexChanged="lstRM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                           
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <span> ZM :</span>
                            <asp:CheckBox ID="CheckBox5" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox5_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListZM" runat="server" OnSelectedIndexChanged="lstZM_SelectedIndexChanged"  AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                          
                        <div class="checkboxlistHeader"; style="max-height: 80px;  width:155px; overflow-y: auto;">
                            <span> ASM :</span>
                            <asp:CheckBox ID="CheckBox6" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox6_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListASM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstASM_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </div>
                            
                         <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                           <span>EXECUTIVE :</span>
                            <asp:CheckBox ID="CheckBox7" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox7_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListEXECUTIVE" runat="server" OnSelectedIndexChanged="lstEXECUTIVE_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                        </div>--%>
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
                            From Date :
                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" CssClass="textboxStyleNew" onchange="ValidateDate(this)"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" Style="margin-right: 30px;" />

                            To Date :
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" CssClass="textboxStyleNew" onchange="ValidateDate(this)"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" Style="margin-right: 30px;" />

                            State :
                            <asp:ListBox ID="lstState" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" ClientIDMode="Static" CssClass="txt-width"></asp:ListBox>

                            Site ID :
                            <asp:ListBox ID="lstSiteId" runat="server" SelectionMode="Multiple" ClientIDMode="Static" Width="200px"></asp:ListBox>

                        </div>

                        <div class="for_right_side">
                            <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" Style="margin-right: 25px;" />
                        </div>
                        <div class="for_left_side">
                            <asp:RadioButtonList ID="rdbListExcelFileFormat" CssClass="ReportSearch" runat="server" Style="float: left; font-weight: 200">
                                <asp:ListItem Enabled="true" Text="xlsb">XLSB</asp:ListItem>
                                <asp:ListItem Text="xlsx">XLSX</asp:ListItem>
                                <asp:ListItem Text="xls">XLS</asp:ListItem>
                            </asp:RadioButtonList>

                            <asp:Button ID="btnExport2Excel" runat="server" CssClass="ReportSearch" Text="Export To Excel" Height="31px" OnClick="btnExport2Excel_Click"></asp:Button></td>
                        </div>

                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

            <%--<div style="overflow: auto; margin-top: 5px;  height: 330px; width: 100%">--%>
            <div class="pane pane--table2 scroll_track">

                <asp:Repeater ID="rptCustomers" runat="server">
                    <HeaderTemplate>
                        <table class="table" >
                        <tr class="fxdheader">
                            <th>STATE</th>
                            <th>SITEID</th>
                            <th>SITE NAME</th>
                            <th>SO NO</th>
                            <th>SO DATE</th>
                            <th>CUSTOMER CODE</th>
                            <th>CUSTOMER NAME</th>
                            <th>CUSTGROUP NAME</th>
                            <th>PRODUCT CODE</th>
                            <th>PRODUCT NAME</th>
                            <th>PRODUCT SUBCATEGORY</th>
                            <th>PRODUCT GROUP</th>
                            <th>DISCOUNT ITEM</th>
                            <th>SCHEME ITEM</th>
                            <th>BOX</th>
                            <th>CRATES</th>
                            <th>LTR</th>
                            <th>MRP</th>
                            <th>RATE</th>
                            <th>DISC PER</th>
                            <th>DISC VALUE</th>
                            <th>DISC AMOUNT</th>
                            <th>TAX CODE</th>
                            <th>ADDTAX CODE</th>
                            <th>ADDTAX AMOUNT</th>
                            <th>LINEAMOUNT</th>
                            <th>AMOUNT</th>
                            <th>LOADSHEET NO</th>
                            <th>PSR CODE</th>
                            <th>PSR Name</th>
                            <th>PSR BEAT</th>
                            <th>INVOICE NO</th>
                            <th>INVOICE DATE</th>
                            <th>INVOICE QTY</th>
                            <th>INVOICE AMOUNT</th>
                            <th>STATUS</th>
                            <th>ORDER TYPE</th>
                            <th>JUMP CALL</th>
                            <th>LAT</th>
                            <th>LONG</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td> <%# DataBinder.Eval(Container.DataItem, "STATE") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "SITEID") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "SITENAME") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "SO_NO") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "SO_DATE") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "CUSTOMER_CODE") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "CUSTOMER_NAME") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "CUSTGROUP_NAME") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "PRODUCT_CODE") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "PRODUCT_NAME") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "PRODUCT_SUBCATEGORY") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "PRODUCT_GROUP") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "DiscountItem") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "SchemeItem") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "BOX") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "CRATES") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "LTR") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "MRP") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "RATE") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "DiscPer") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "DiscValue") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "Disc_Amount") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "TAX_CODE") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "ADDTAX_CODE") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "ADDTAX_AMOUNT") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "LINEAMOUNT") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "AMOUNT") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "LOADSHEET_NO") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "PSR_Code") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "PSR_Name") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "PSR_BEAT") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "Invoice_No") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "INVOIC_DATE") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "INVOICE_QTY") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "INVOICE_AMT") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "Status") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "OrderType") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "JumpCall") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "LAT") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "LONG") %> </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td> <%# DataBinder.Eval(Container.DataItem, "STATE") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "SITEID") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "SITENAME") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "SO_NO") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "SO_DATE") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "CUSTOMER_CODE") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "CUSTOMER_NAME") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "CUSTGROUP_NAME") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "PRODUCT_CODE") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "PRODUCT_NAME") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "PRODUCT_SUBCATEGORY") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "PRODUCT_GROUP") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "DiscountItem") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "SchemeItem") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "BOX") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "CRATES") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "LTR") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "MRP") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "RATE") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "DiscPer") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "DiscValue") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "Disc_Amount") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "TAX_CODE") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "ADDTAX_CODE") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "ADDTAX_AMOUNT") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "LINEAMOUNT") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "AMOUNT") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "LOADSHEET_NO") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "PSR_Code") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "PSR_Name") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "PSR_BEAT") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "Invoice_No") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "INVOIC_DATE") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "INVOICE_QTY") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "INVOICE_AMT") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "Status") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "OrderType") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "JumpCall") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "LAT") %> </td>
                            <td> <%# DataBinder.Eval(Container.DataItem, "LONG") %> </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>







                <%--<script src='http://cdnjs.cloudflare.com/ajax/libs/jquery/2.2.2/jquery.min.js'></script>--%>
                <script src="Javascript/index.js"></script>
            </div>
            <%-- </div>  --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


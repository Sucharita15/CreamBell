<%@ Page Title="VRS Special Discount Report" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmVRSSpecialDiscountReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmVRSSpecialDiscountReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/ucRadioButtonList.ascx" TagPrefix="uc1" TagName="ucRadioButtonList" %>

<%--<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>

    <script src="Javascript/DateValidation.js" type="text/javascript"></script>
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
            $('#lstCustGroup').multiselect({
                includeSelectAllOption: true,
                nonSelectedText: 'Select Customer Group',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '140px',
                maxHeight: 300
            });
            $('#lstCust').multiselect({
                includeSelectAllOption: true,
                nonSelectedText: 'Select Customer Name',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '140px',
                maxHeight: 300
            });
            $('#lstbu').multiselect({
                includeSelectAllOption: true,
                nonSelectedText: 'Select Business Unit',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '140px',
                maxHeight: 300
            });
        });
    </script>
    <script type="text/javascript">

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gvDetails.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();

            $('#<%=gvDetails.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });

            $("#controlHead2").append(gridHeader);
            $('#controlHead2').css('position', 'absolute');
            $('#controlHead2').css('top', '129');

        });

    </script>

    <script src="Javascript/custom.js">
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

        .nav ul li a {
            width: 270px !important;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            //alert('hello');
            $('.pane--table2').find('.fxdheader').parent('div').addClass('ganerateDiv');
        });
    </script>
    <script type="text/javascript">

        function IsValidDate(myDate) {
            var filter = /([Jj][Aa][Nn]|[Ff][Ee][bB]|[Mm][Aa][Rr]|[Aa][Pp][Rr]|[Mm][Aa][Yy]|[Jj][Uu][Nn]|[Jj][u]l|[aA][Uu][gG]|[Ss][eE][pP]|[oO][Cc][Tt]|[Nn][oO][Vv]|[Dd][Ee][Cc])-(19|20)\d\d$/
            return filter.test(myDate);
        }
        function ValidateDate(e) {
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

        .forLeftMargin {
            margin-right: 7px;
            float: right;
        }

        .forLeftSide .btn-group button {
            padding: 4px 0 4px 0 !important;
        }

        .forBtnStyle .btn-group button {
            padding: 4px 0 4px 0 !important;
        }

        .tableTdStyle table tr td {
            padding: 0 !important;
        }

        #ctl00_ContentPage_upsale {
            margin-bottom: 20px;
        }

        .forTableScroll {
            width: 1800px;
        }

            .forTableScroll table th, table td {
                white-space: inherit !important;
                padding-left: 4px;
            }

                .forTableScroll table th:nth-child(8) {
                    width: 56px;
                }
    </style>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport2Excel" />
            <%--<asp:PostBackTrigger ControlID="BtnShowReport0" />--%>
        </Triggers>
        <ContentTemplate>
            <div style="width: 100%; height: 20px; border-radius: 4px; margin: 5px 0px 0px 0px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold; text-align: center">
                <div class="col-lg-12">
                    <label runat="server" style="text-align: center" id="tclabel">
                        VRS Special Discount Report
                    </label>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="pnlupd" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" CssClass="txtLegend" Style="width: 100%; margin: 0px 0px 0px 0px;">

                <asp:UpdatePanel runat="server" ID="upsale" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="Sale Person Filter" Style="width: 100%; margin: 0px 0px 0px 0px;">

                            <div class="paneltbl tableTdStyle">

                                <div class="checkboxlistHeader" style="max-height: 100px; overflow-y: auto; width: 12.5%;">
                                    <span>HOS :</span>
                                    <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="chkAll_CheckedChanged" Text="Select All"></asp:CheckBox>
                                    <asp:CheckBoxList ID="chkListHOS" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstHOS_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </div>

                                <div class="checkboxlistHeader" style="max-height: 100px; overflow-y: auto; width: 12.5%;">
                                    <span>VP :</span>
                                    <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Select All" />
                                    <asp:CheckBoxList ID="chkListVP" runat="server" OnSelectedIndexChanged="lstVP_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </div>

                                <div class="checkboxlistHeader" style="max-height: 100px; overflow-y: auto; width: 12.5%;">
                                    <span>GM :</span>
                                    <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox2_CheckedChanged" Text="Select All" />
                                    <asp:CheckBoxList ID="chkListGM" runat="server" OnSelectedIndexChanged="lstGM_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </div>

                                <div class="checkboxlistHeader" style="max-height: 100px; overflow-y: auto; width: 12.5%;">
                                    <span>DGM :</span>
                                    <asp:CheckBox ID="CheckBox3" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox3_CheckedChanged" Text="Select All" />
                                    <asp:CheckBoxList ID="chkListDGM" runat="server" OnSelectedIndexChanged="lstDGM_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </div>

                                <div class="checkboxlistHeader" style="max-height: 100px; overflow-y: auto; width: 12.5%;">
                                    <span>RM :</span>
                                    <asp:CheckBox ID="CheckBox4" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox4_CheckedChanged" Text="Select All" />
                                    <asp:CheckBoxList ID="chkListRM" runat="server" OnSelectedIndexChanged="lstRM_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </div>

                                <div class="checkboxlistHeader" style="max-height: 100px; overflow-y: auto; width: 12.5%;">
                                    <span>ZM :</span>
                                    <asp:CheckBox ID="CheckBox5" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox5_CheckedChanged" Text="Select All" />
                                    <asp:CheckBoxList ID="chkListZM" runat="server" OnSelectedIndexChanged="lstZM_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </div>

                                <div class="checkboxlistHeader" style="max-height: 100px; overflow-y: auto; width: 12.5%;">
                                    <span>ASM :</span>
                                    <asp:CheckBox ID="CheckBox6" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox6_CheckedChanged" Text="Select All" />
                                    <asp:CheckBoxList ID="chkListASM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstASM_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </div>

                                <div class="checkboxlistHeader" style="max-height: 100px; overflow-y: auto; width: 12.5%;">
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


                <div class="option_menu forBtnStyle">
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>

                    <td>
                        <span style="margin-right: 35px;">From Date :</span>
                        <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" Width="140px" CssClass="textboxStyleNew" Height="26px" onchange="ValidateDate(this)" required></asp:TextBox>
                        <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                    </td>

                    <td>
                        <span style="margin-left: 52px; margin-right: 14px">To Date :</span>
                        <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" Width="140px" CssClass="textboxStyleNew" Height="26px" margin-left="26px" onchange="ValidateDate(this)" required></asp:TextBox>
                        <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                    </td>
                    <span style="margin-left: 48px; margin-right: 43px;">State :</span>
                    <asp:ListBox ID="lstState" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="lstState_SelectedIndexChanged" ClientIDMode="Static" CssClass="txt-width"></asp:ListBox>

                    <span style="margin-left: -20px; margin-right: 6px;">Distributor :</span>
                    <asp:ListBox ID="lstSiteId" runat="server" SelectionMode="Multiple" AutoPostBack="true" ClientIDMode="Static" Width="200px" OnSelectedIndexChanged="lstSiteId_SelectedIndexChanged"></asp:ListBox>
                </div>

                <%--<div class="for_left_side">
                    <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" />

                </div>--%>

                <div class="forLeftSide">
                    <span style="margin-right: 6px;">Customer Group :</span>
                    <asp:ListBox ID="lstCustGroup" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="lstCustGroup_SelectedIndexChanged" ClientIDMode="Static" Height="27px" Width="200px" CssClass="txt-width"></asp:ListBox>
                    <span style="margin-right: 4px; margin-left: 19px;">Customer :</span>
                    <asp:ListBox ID="lstCust" runat="server" SelectionMode="Multiple" ClientIDMode="Static" Height="30px" Width="200px" CssClass="txt-width txtHeight"></asp:ListBox>

                    <span style="margin-right: 11px; margin-left: 12px;">Business Unit </span>
                    <asp:ListBox ID="lstbu" runat="server" SelectionMode="Multiple" ClientIDMode="Static" Height="30px" Width="200px" Style="border-radius: 4px;"></asp:ListBox>
                    <%--<asp:DropDownList ID="DDLBusinessUnit" runat="server" Height="30px" Width="200px" style="border-radius:4px;"></asp:DropDownList>--%>
                </div>
                <%--<div class="radio_button">
                    <asp:RadioButtonList ID="rdbListExcelFileFormat" CssClass="ReportSearch" runat="server" Style="float: left; font-weight: 200">
                        <asp:ListItem Enabled="true" Text="xlsb">XLSB</asp:ListItem>
                        <asp:ListItem Text="xlsx">XLSX</asp:ListItem>
                        <asp:ListItem Text="xls">XLS</asp:ListItem>
                    </asp:RadioButtonList>
                </div>--%>
                <div class="radio_button">
                    <uc1:ucRadioButtonList runat="server" ID="ucRadioButtonList" />
                    <asp:Button ID="btnExport2Excel" runat="server" Text="Export To Excel" Height="31px" OnClick="btnExport2Excel_Click" CssClass="ReportSearch forLeftMargin"></asp:Button>
                </div>
            </asp:Panel>

            <div style="overflow-y: scroll; margin-top: 5px; height: 400px; margin-left: 5px; padding-right: 10px; width: 100%;" class="TheadStyling">
                <div class="forTableScroll">
                    <asp:GridView runat="server" ID="gvDetails" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="true" Width="100%" BackColor="White"
                        BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
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
            </div>



            <div style="overflow-y: scroll; margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 100%;" class="TheadStyling">
                <div class="forTableScroll">
                    <asp:GridView runat="server" ID="grdSummary" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="true" Width="100%" BackColor="White"
                        BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
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
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

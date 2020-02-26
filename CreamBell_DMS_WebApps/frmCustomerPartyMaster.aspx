<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="frmCustomerPartyMaster.aspx.cs" Inherits="CreamBell_DMS_WebApps.CustomerPartyMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ccp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<%@ Register Src="~/UserControl/ucRadioButtonList.ascx" TagPrefix="uc1" TagName="ucRadioButtonList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="css/btnSearch.css" rel="stylesheet" />
	<!--<link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />-->
    
    <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
	<link href="css/style.css" rel="stylesheet" />
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    
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
    </style>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport2Excel" />
            <%--<asp:PostBackTrigger ControlID="BtnShowReport0" />--%>
            <asp:PostBackTrigger ControlID="btnSearchCustomer" />
        </Triggers>

        <ContentTemplate>
            <div style="width: 100%; height: 20px; border-radius: 4px; margin: 5px 0px 0px 0px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold; text-align: center;display: flex;">


                <div class="col-lg-12">

                    <%-- <td runat="server" style=" text-align:center" id="tclabel" >

                               Customer Party Master
                                    
                                </td>--%>

                    <label runat="server" style="text-align: center" id="tclabel">
                        Customer Party Master  
                    </label>

                </div>

            </div>

            <asp:UpdatePanel ID="pnlupd" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" CssClass="txtLegend" Style="width: 100%; margin: 0px 0px 0px 0px;">

                        <asp:UpdatePanel runat="server" ID="upsale" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server" GroupingText="Sale Person Filter" Style="width: 100%; margin: 0px 0px 0px 0px;">

                                    <div class="paneltbl">

                                        <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto;">
                                            <span>HOS :</span>
                                            <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="chkAll_CheckedChanged" Text="Select All"></asp:CheckBox>
                                            <asp:CheckBoxList ID="chkListHOS" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstHOS_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </div>




                                        <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto;">
                                            <span>VP :</span>
                                            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Select All" />
                                            <asp:CheckBoxList ID="chkListVP" runat="server" OnSelectedIndexChanged="lstVP_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </div>




                                        <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto;">
                                            <span>GM :</span>
                                            <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox2_CheckedChanged" Text="Select All" />
                                            <asp:CheckBoxList ID="chkListGM" runat="server" OnSelectedIndexChanged="lstGM_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </div>

                                        <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto;">
                                            <span>DGM :</span>
                                            <asp:CheckBox ID="CheckBox3" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox3_CheckedChanged" Text="Select All" />
                                            <asp:CheckBoxList ID="chkListDGM" runat="server" OnSelectedIndexChanged="lstDGM_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </div>

                                        <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto;">
                                            <span>RM :</span>
                                            <asp:CheckBox ID="CheckBox4" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox4_CheckedChanged" Text="Select All" />
                                            <asp:CheckBoxList ID="chkListRM" runat="server" OnSelectedIndexChanged="lstRM_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </div>

                                        <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto;">
                                            <span>ZM :</span>
                                            <asp:CheckBox ID="CheckBox5" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox5_CheckedChanged" Text="Select All" />
                                            <asp:CheckBoxList ID="chkListZM" runat="server" OnSelectedIndexChanged="lstZM_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </div>

                                        <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto;">
                                            <span>ASM :</span>
                                            <asp:CheckBox ID="CheckBox6" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox6_CheckedChanged" Text="Select All" />
                                            <asp:CheckBoxList ID="chkListASM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstASM_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </div>

                                        <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto;">
                                            <span>EXECUTIVE :</span>
                                            <asp:CheckBox ID="CheckBox7" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox7_CheckedChanged" Text="Select All" />
                                            <asp:CheckBoxList ID="chkListEXECUTIVE" runat="server" OnSelectedIndexChanged="lstEXECUTIVE_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>

                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>

                                <%--<asp:PostBackTrigger ControlID="lstState" />--%>
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

                        <br />
                        <div class="option_menu">
                            State : 
                            <asp:ListBox ID="lstState" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" ClientIDMode="Static" CssClass="txt-width"></asp:ListBox>

                            Site ID : 
                            <asp:ListBox ID="lstSiteId" runat="server" SelectionMode="Multiple" ClientIDMode="Static" Width="200px"></asp:ListBox>


                            Search Customer By :
                        <asp:DropDownList ID="DDLSearchType" runat="server" CssClass="multiselect dropdown-toggle btn btn-default" data-toggle="dropdown" Style="text-align: right; width: 200px;">
                            <asp:ListItem>Customer Code</asp:ListItem>
                            <asp:ListItem>Customer Name</asp:ListItem>
                            <asp:ListItem>PSR Code</asp:ListItem>
                        </asp:DropDownList>
                        </div>

                        <div class="for_left_side">
                            <asp:TextBox ID="txtSearch" runat="server" placeholder="Search here..." Width="150px" CssClass="multiselect dropdown-toggle btn btn-default" />
                            <span id="span1" onmouseover="test()" onmouseout="test1()">
                                <asp:Button ID="btnSearchCustomer" runat="server" Style="margin: 0px 0px 0px -2px" Height="31px" Text="Search" OnClick="btnSearchCustomer_Click"></asp:Button>
                            </span>


                            <%--<asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" />--%>

                        </div>
						<div class="clearfix"></div>

                        <div class="radio_button">
                            <b>Filter Customer By Status:&nbsp;</b>
                            <asp:RadioButton ID="rdRunningC" runat="server" Text="Running" ToolTip="RunningCustomers" Checked="true" ValidationGroup="Customers" GroupName="RdCustomers" OnCheckedChanged="rdRunningC_CheckedChanged" />
                            <asp:RadioButton ID="rdBlockC" runat="server" Text="Block" CheToolTip="BlockCustomers" ValidationGroup="Customers" GroupName="RdCustomers" />
                            <asp:RadioButton ID="rdBothC" runat="server" Text="Both" ToolTip="BothCustomers" ValidationGroup="Customers" GroupName="RdCustomers" />


                            <div class="for_excel_margin">
                                Cust Opening :
                        <asp:HyperLink ID="CustOpening" runat="server" Font-Size="Small" ForeColor="Blue" Style="margin-left: 0px; width: 90%;" ToolTip="Click to download Cust Opening excel template !!">
                                 <a href="ExcelTemplate/CustomerOpeningTemplate.xlsx" target="_blank">
                                 <img src="Images/DownloadTemplate.gif" alt="Download Template" style="border-style: none"  /></a> </asp:HyperLink>
                            </div>
                            <div class="for_excel_margin">
                                Cust Discount :
                        <asp:HyperLink ID="CustDiscount" runat="server" Font-Size="Small" ForeColor="Blue" Style="margin-left: -10px" ToolTip="Click to download Cust Discount excel template !!">
                                 <a href="ExcelTemplate/CustomerDiscount.xlsx" target="_blank">
                                 <img src="Images/DownloadTemplate.gif" alt="Download Template" style="border-style: none"  /></a> </asp:HyperLink>
                            </div>
                        </div>
                        <%--<div class="radio_button">
                            <b>Select Excel Export Type:&nbsp;</b>
                            <asp:RadioButtonList ID="rdbListExcelFileFormat" runat="server" Style="float: left; font-weight: 200">
                                <asp:ListItem Enabled="true" Text="xlsb">XLSB</asp:ListItem>
                                <asp:ListItem Text="xlsx">XLSX</asp:ListItem>
                                <asp:ListItem Text="xls">XLS</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>--%>
                        <div class="radio_button,  for_left_side last">
							<div style="float: left; margin-right: 15px;">
								<uc1:ucRadioButtonList runat="server" ID="ucRadioButtonList" />
							</div>
                            <asp:Button ID="btnExport2Excel" runat="server" Text="Export To Excel" CssClass="ReportSearch" OnClick="btnExport2Excel_Click"></asp:Button>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>



            <%--<div style="overflow: auto; margin-top: 5px;  height: 330px; width: 100%">--%>
            <div class="pane pane--table2 scroll_track">

                <asp:GridView ID="gridViewCustomers" runat="server" ShowFooter="false" AutoGenerateColumns="False"
                    Width="100%" BackColor="White" BorderWidth="0" CellPadding="3" ShowHeaderWhenEmpty="True"
                    OnRowDataBound="gridViewCustomers_RowDataBound" OnPageIndexChanging="gridViewCustomers_PageIndexChanging" CssClass="fxdheader">



                    <AlternatingRowStyle BackColor="#CCFFCC" />
                    <Columns>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="LnkView" runat="server" OnClick="LnkView_Click" CommandArgument='<%# Bind("Customer_Code") %>' Text="View"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="DistributorCode" DataField="DistributorCode">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CustomerCode" DataField="Customer_Code">
                            <HeaderStyle ForeColor="White" />
                            <%--<ItemStyle Font-Bold="True" Font-Names="Segoe UI" ForeColor="#009900" />--%>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CustomerName" DataField="Customer_Name">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Contact Name" DataField="Contact_Name">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Mobile No" DataField="Mobile_No">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Phone No" DataField="Phone_No">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Email ID" DataField="EMAILID">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Bill To Address" DataField="BILLTOADDRESS">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="City" DataField="City">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="State" DataField="State">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="PIN Code" DataField="ZipCode">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Area" DataField="Area">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Ship To Address 1" DataField="SHIPTOADDRESS1">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Ship To Address State 1" DataField="SHIPTOSTATE1">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Ship To Address 2" DataField="SHIPTOADDRESS2">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Ship To Address State 2" DataField="SHIPTOSTATE2">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="GSTIN" DataField="GSTINNO">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="GST REGISTRATION DATE" DataField="GSTREGISTRATIONDATE">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="COMPOSITION SCHEME" DataField="COMPOSITIONSCHEME">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="PAN NO" DataField="PAN">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="TAN NO" DataField="TAN">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="VAT NO" DataField="Vat">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CUSTOMER BLOCKED" DataField="BLOCKED">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CART OPERATOR" DataField="CARTOPERATOR">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="OUTLET TYPE" DataField="OUTLET_TYPE">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CHANNELTYPECODE" DataField="CHANNELTYPECODE">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CLOSING DATE" DataField="CLOSING_DATE">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Deep Freezer" DataField="DEEP_FRIZER">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="KEY CUSTOMER" DataField="KEY_CUSTOMER">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Payment Mode" DataField="Payment_Mode">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Payment Term" DataField="Payment_Term">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="PriceGroup" DataField="PriceGroup">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="PSR BEAT" DataField="PSR_BEAT">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <%-------------------------------------------%>
                        <%-- <asp:BoundField HeaderText="PSR BEAT CODE" DataField="BEATCODE">
                        <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>--%>
                        <asp:BoundField HeaderText="PSRCode" DataField="PSR_CODE">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="PSR Week Day" DataField="PSR_Day">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Register_Date" DataField="Register_Date" DataFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CUSTOMER TYPE" DataField="CUSTOMERTYPE">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Customer Group" DataField="Cust_Group">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CHANNELGROUPCODE" DataField="CHANNELGROUPCODE">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CHANNEL TYPE" DataField="CHANNEL_TYPE">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <%--                     <asp:BoundField HeaderText="CHANNEL_TYPE" DataField="CHANNEL_TYPE">
                        <HeaderStyle ForeColor="White" />
                     <ItemStyle HorizontalAlign="Left" />
                        <ItemStyle Font-Bold="True" Font-Names="Segoe UI" ForeColor="#009900" />
                    </asp:BoundField>--%>
                        <asp:BoundField HeaderText="Adhaar No." DataField="AADHARNO">
                            <HeaderStyle ForeColor="White" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <%--                    <asp:BoundField HeaderText="Channel Type Code" DataField="CHANNELTYPECODE">
                        <HeaderStyle ForeColor="White" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>--%>

                        <asp:BoundField HeaderText="Monday" DataField="MONDAY">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Tuesday" DataField="TUESDAY">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Wednesday" DataField="WEDNESDAY">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Thursday" DataField="THURSDAY">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>


                        <asp:BoundField HeaderText="Friday" DataField="FRIDAY">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Saturday" DataField="SATURDAY">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Sunday" DataField="SUNDAY">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Visit Frequency" DataField="VISITFREQUENCY">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Repeat Week" DataField="REPEATWEEK">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Sequence No." DataField="SEQUENCENO">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CST" DataField="CST">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CHANNELTYPENAME" DataField="CHANNELTYPENAME">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="DISTRICT" DataField="DISTRICT">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <%--                <asp:BoundField HeaderText="Channel Group" DataField="CHANNELGROUPCODE">
                        <HeaderStyle ForeColor="White" />
                   <ItemStyle HorizontalAlign="Left" />
                         </asp:BoundField>--%>

                        <asp:BoundField HeaderText="CustGroupName" DataField="CUSTGROUP_NAME">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CHANNELGROUPNAME" DataField="CHANNELGROUPNAME">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="DistributorName" DataField="DistributorName">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="PSR NAME" DataField="PSR_NAME">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="PSR BEAT NAME" DataField="PSRBEATNAME">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="BEATCODE" DataField="BEATCODE">
                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="itemfeild" />
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


                <%--<script src='http://cdnjs.cloudflare.com/ajax/libs/jquery/2.2.2/jquery.min.js'></script>--%>

                <script src="Javascript/index.js"></script>

            </div>

            <%-- </div>  --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

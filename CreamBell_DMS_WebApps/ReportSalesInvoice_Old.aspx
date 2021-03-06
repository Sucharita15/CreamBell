﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ReportSalesInvoice_Old.aspx.cs" Inherits="CreamBell_DMS_WebApps.ReportSalesInvoice_Old" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/ucRoleFilters.ascx" TagPrefix="uc1" TagName="ucRoleFilters" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />
    <link href="css/SSheet.css" rel="Stylesheet" type="text/css" />

    <script src="Javascript/DateValidation.js" type="text/javascript"></script>

    <script type="text/javascript">
        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        // row.style.backgroundColor = "aqua";
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        //and change rowcolor back to original
                        if (row.rowIndex % 2 == 0) {
                            //Alternating Row Color
                            //  row.style.backgroundColor = "#C2D69B";
                        }
                        else {
                            row.style.backgroundColor = "white";
                        }
                        inputList[i].checked = false;
                    }
                }
            }
        }


    </script>


    <script type="text/javascript">

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=GridView1.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=GridView1.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead1").append(gridHeader);
            $('#controlHead1').css('position', 'absolute');
            $('#controlHead1').css('top', '129');

        });


        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=GridView2.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=GridView2.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead2").append(gridHeader);
            $('#controlHead2').css('position', 'absolute');
            $('#controlHead2').css('top', '129');

        });

        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        function IsNumeric(e) {

            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
            //document.getElementById("lblError") = ret ? "none" : "inline";
            if (keyCode == 8 || keyCode == 46 || keyCode == 189 || keyCode == 61 || keyCode == 45) { ret = true; }
            return ret;
        }

        function ActiveSearch() {
            var ddlSearchNew = $('#ctl00_ContentPage_ddlSearchNew').val();
            if (ddlSearchNew != "All") {
                $('#ctl00_ContentPage_txtSearch').removeAttr("disabled");
            }
            else {
                $('#ctl00_ContentPage_txtSearch').attr("disabled", "disabled");
                $('#txtSearch').val("");
            }
        }
    </script>

    <style type="text/css">
        .ddl {
            background-image: url('Images/arrow-down-icon-black.png');
        }

        .ddl {
            background-color: #eeeeee;
            padding: 5px;
            border: 1px solid #7d6754;
            border-radius: 4px;
            padding: 3px;
            -webkit-appearance: none;
            background-image: url('Images/arrow-down-icon-black.png');
            background-position: right;
            background-repeat: no-repeat;
            text-indent: 0.01px; /*In Firefox*/
            text-overflow: ''; /*In Firefox*/
        }

        .input1 {
            width: 270px;
            height: 10px;
            padding: 10px 5px;
            float: left;
            border: 0;
            background: #eee;
            -moz-border-radius: 3px 0 0 3px;
            -webkit-border-radius: 3px 0 0 3px;
            border-radius: 3px 0 0 3px;
        }

        .arrow_box {
            position: relative;
            background: #16365c;
        }

        .button1 {
            overflow: visible;
            position: relative;
            float: right;
            border: 0;
            padding: 0;
            cursor: pointer;
            height: 30px;
            width: 68px;
            color: #fff;
            text-transform: uppercase;
            background: #16365c;
            -moz-border-radius: 0 3px 3px 0;
            -webkit-border-radius: 0 3px 3px 0;
            border-radius: 0 3px 3px 0;
            text-shadow: 0 -1px 0 rgba(0, 0,0, .3);
        }

        .auto-style2 {
            width: 166px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <asp:UpdatePanel ID="upanel" runat="server">
        <ContentTemplate>
            <uc1:ucRoleFilters runat="server" ID="ucRoleFilters" />
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
            <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>

            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px;">
                Sales Invoice
            </div>

            <table style="width: 100%; vertical-align: top;">
                <tr>
                    <td colspan="10" style="text-align: center">
                        <asp:Label ID="LblMessage" runat="server" Font-Bold="True" Font-Names="Seoge UI" Font-Size="Small" ForeColor="DarkRed"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>From Date :</td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtFromDate" runat="server" Enabled="true" placeholder="dd-MMM-yyyy" Width="100px" onchange="ValidateDate(this)" required></asp:TextBox>
                        <asp:ImageButton ID="imgBtnFrom" runat="server" ImageAlign="Bottom" ImageUrl="~/Images/calendar.jpg" />
                    </td>
                    <td>To Date :</td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" Enabled="true" placeholder="dd-MMM-yyyy" Width="100px" onchange="ValidateDate(this)" required></asp:TextBox>
                        <asp:ImageButton ID="imgBtnTo" runat="server" ImageAlign="Bottom" ImageUrl="~/Images/calendar.jpg" />
                    </td>
                    <td class="auto-style3">PSR : </td>
                    <td class="auto-style2">
                        <%-- <asp:DropDownList ID="drpPSR" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpPSR_SelectedIndexChanged" Width="150">
                        </asp:DropDownList>--%>
                        <script type="text/javascript">
                            $(document).ready(function () {
                                $('.single-selection').multiselect({
                                    enableFiltering: true,
                                    enableCaseInsensitiveFiltering: true,
                                    nonSelectedText: 'Select'
                                  
                                });
                            });
                        </script>
                        <asp:ListBox ID="drpPSRNew" ClientIDMode="Static" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="drpPSR_SelectedIndexChanged" CssClass="single-selection"></asp:ListBox>
                    </td>

                    <td>BEAT:</td>
                    <td>
                        <%--  <asp:DropDownList ID="drpBeat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpBeat_SelectedIndexChanged" Width="150">
                        </asp:DropDownList>--%>
                        <asp:ListBox ID="drpBeatNew" ClientIDMode="Static" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="drpBeat_SelectedIndexChanged" CssClass="single-selection"></asp:ListBox>
                    </td>
                    <td class="auto-style2" style="text-align: right">
                      <%--  <asp:DropDownList ID="ddlSearch" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged" data-toggle="dropdown" Height="22px" Style="margin-left: 0px" Width="150px">
                            <asp:ListItem>All</asp:ListItem>
                            <asp:ListItem>Sales Invoice No</asp:ListItem>
                            <asp:ListItem>Customer</asp:ListItem>
                        </asp:DropDownList>--%>
                        <select id="ddlSearchNew" runat="server" class='single-selection' onchange="ActiveSearch();">
                            <option selected="selected">All</option>
                            <option>Sales Invoice No</option>
                            <option>Customer</option>
                        </select>

                         <%--<asp:ListBox ID="ddlSearchNew" ClientIDMode="Static" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="drpBeat_SelectedIndexChanged" CssClass="single-selection"></asp:ListBox>--%>

                    </td>
                    <td>
                        <asp:TextBox ID="txtSearch" Enabled="false" runat="server" placeholder="Search here...." Width="170px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>InvoiceType :</td>
                    <td>
                        <asp:RadioButton ID="rdoBoth" runat="server" AutoPostBack="true" Text="All" Checked="true"
                            OnCheckedChanged="rdoBoth_CheckedChanged" GroupName="radio" />

                        <asp:RadioButton ID="rdoSI" runat="server" AutoPostBack="true" Text="SI"
                            OnCheckedChanged="rdoBoth_CheckedChanged" GroupName="radio" />

                        <asp:RadioButton ID="rdoSR" runat="server" AutoPostBack="true" Text="SR"
                            OnCheckedChanged="rdoBoth_CheckedChanged" GroupName="radio" />

                        <asp:RadioButton ID="rdoFOC" runat="server" AutoPostBack="true" Text="FOC"
                            OnCheckedChanged="rdoBoth_CheckedChanged" GroupName="radio" />

                    </td>

                    <td>InvoiceNo From :</td>
                    <td>
                        <asp:TextBox ID="txtInvoiceNoStart" runat="server" Enabled="true" onkeypress="return IsNumeric(event)" Width="100px"></asp:TextBox>
                    </td>
                    <td class="auto-style3">To :</td>
                    <td class="auto-style2">
                        <asp:TextBox ID="txtInvoiceNoEnd" runat="server" Enabled="true" onkeypress="return IsNumeric(event)" Width="100px"></asp:TextBox>
                    </td>
                    <td>Customer   :</td>
                    <td>
                      <%--  <asp:DropDownList ID="drpCustomer" runat="server" Width="150"></asp:DropDownList>--%>
                         <asp:ListBox ID="drpCustomerNew" ClientIDMode="Static" runat="server" 
                             CssClass="single-selection"></asp:ListBox>


                    </td>

                    <td style="text-align: left">
                        <asp:Button ID="btnSearch" runat="server" CssClass="ReportSearch" Height="30px" Text="Search" Width="70px" OnClick="btnSearch_Click" />
                    </td>
                    <td>
                        <asp:Button ID="Button2" runat="server" Text="MultiplePrint" CssClass="ReportSearch" Height="31px" OnClick="Button2_Click" />
                    </td>
                </tr>
            </table>

            <div id="controlHead1" style="margin-top: 10px; margin-left: 10px; padding-right: 10px;">
            </div>
            <div style="overflow: auto; height: 260px; margin: 10px 0px 0px 10px;">
                <asp:GridView ID="GridView1" runat="server" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" OnRowDataBound="GridView1_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" AutoPostBack="true" OnCheckedChanged="checkAll_CheckedChanged" Text="All" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%--<asp:CheckBox ID="chklist" runat="server" Text='<%# Bind("INVOICE_NO") %>'   />--%>
                                <asp:CheckBox ID="chklist" Text="  " runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice No">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtn" runat="server" Text='<%# Bind("INVOICE_NO") %>' OnClick="lnkbtn_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Invoice Date" DataField="INVOIC_DATE" DataFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Width="120px" HorizontalAlign="Left" />
                            <ItemStyle Width="120px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SO_NO" HeaderText="SO No.">
                            <ControlStyle Width="250px" />
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SO_DATE" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="SO Date">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="CUSTOMER" HeaderText="Customer">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="INVOICE_VALUE" HeaderText="Invoice Value" DataFormatString="Rs.{0:n}">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderImageUrl="~/Images/printicon.png">
                            <ItemTemplate>
                                <asp:HyperLink ID="HPLinkPrint" runat="server" Font-Bold="True">View</asp:HyperLink>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                    </Columns>
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

            <div id="controlHead2" style="margin-top: 5px; margin-left: 5px; padding-right: 10px;"></div>
            <div style="overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px;">

                <asp:GridView ID="GridView2" runat="server" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">

                    <Columns>
                        <asp:BoundField HeaderText="Product Group" DataField="product_group">
                            <HeaderStyle Width="80px" />
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="product_code" HeaderText="Product Code">
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Product Name" DataField="product_name">
                            <HeaderStyle Width="80px" HorizontalAlign="Left" />
                            <ItemStyle Width="100px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CRATES" HeaderText="Invoice Qty(Crates)" DataFormatString="{0:n2}" Visible="False">
                            <HeaderStyle HorizontalAlign="Right" Width="80px" />
                            <ItemStyle HorizontalAlign="Right" Width="100px" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText=" Qty(Box)" DataField="BOXQTY" DataFormatString="{0:n2}">
                            <HeaderStyle Width="80px" HorizontalAlign="Right" />
                            <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Qty(PCS)" DataField="PCSQty" DataFormatString="{0:n2}">
                            <HeaderStyle Width="80px" HorizontalAlign="Right" />
                            <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Invoice Qty(BoxPcs)" DataField="BOXPCS" DataFormatString="{0:n2}">
                            <HeaderStyle Width="80px" HorizontalAlign="Right" />
                            <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Total Qty(Box)" DataField="BOX" DataFormatString="{0:n2}">
                            <HeaderStyle Width="80px" HorizontalAlign="Right" />
                            <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Invoice Qty(LTR)" DataField="LTR" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="MRP" DataField="MRP" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="rate" DataFormatString="{0:n2}" HeaderText="Rate">
                            <HeaderStyle HorizontalAlign="Right" Width="80px" />
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="tax_code" DataFormatString="{0:n2}" HeaderText="Tax Code">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="amount" DataFormatString="{0:n2}" HeaderText="Value">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
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
            <rsweb:ReportViewer ID="ReportViewer1" Visible="false" runat="server" Width="100%" Height="800px">
            </rsweb:ReportViewer>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Button2" />
            <asp:PostBackTrigger ControlID="ucRoleFilters" />
            <asp:PostBackTrigger ControlID="btnSearch" />
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>

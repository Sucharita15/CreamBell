<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmIndentConsolidationToSO.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmIndentConsolidationToSO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />

    <script type="text/javascript">
        //
        $(document).ready(function () {
            $("select").searchable();
        });

    </script>

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

        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        function IsNumeric(e) {
            //debugger;
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
            //document.getElementById("lblError") = ret ? "none" : "inline";
            if (keyCode == 8 || keyCode == 46 || keyCode == 189 || keyCode == 61 || keyCode == 45)
            { ret = true; }
            return ret;
        }

    </script>


    <%--       <script language="javascript" type="text/javascript">
            // This Script is used to maintain Grid Scroll on Partial Postback

            var scrollTop;
            //Register Begin Request and End Request 
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            //Get The Div Scroll Position
            function BeginRequestHandler(sender, args) {

                var m = document.getElementById('divGrid');
                scrollTop = m.scrollTop;
                var m = document.getElementById('controlHead');
                scrollTop = m.scrollTop;
            }
            //Set The Div Scroll Position
            function EndRequestHandler(sender, args) {

                var m = document.getElementById('divGrid');
                m.scrollTop = scrollTop;
                var m = document.getElementById('controlHead');
                scrollTop = m.scrollTop;
            }
    </script>--%>

   <script src="Javascript/DateValidation.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 0px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; color: white; margin: 6px 0px 0px 0px;">Indent Consolidation</span>
    </div>

    <%-- <div style="border-bottom-style: solid; border-bottom-width: 5px; border-bottom-color: #000066" >--%>
    <table style="width: 100%; text-align: left">
        <tr>
            <td style="width: 8%; padding-left:10px;">From Date </td>
            <td style="width: 15%">
                <div>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="From Date..." Height="15px" Width="100%" TabIndex="0"  onchange="ValidateDate(this)" required />
                </div>
                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
            <td style="width: 8%;padding-left:10px;">To Date </td>
            <td style="width: 15%">
                <div>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="To Date..." Height="15px" Width="100%" TabIndex="1"  onchange="ValidateDate(this)" required/>
                </div>
                <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
            <td style="width: 15%;padding-left:10px;">
                Distributor Name
            </td>
            <td>
                <asp:UpdatePanel runat="server" ID="upDistributor">
                    <ContentTemplate>
                        <asp:DropDownList ID="DrpDistributor" runat="server" AutoPostBack="true" Width="180px" CssClass="textboxStyleNew" TabIndex="2" OnSelectedIndexChanged="DrpDistributor_SelectedIndexChanged"></asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
            <span id="span2" onmouseover="test()" onmouseout="test1()">
                <asp:Button ID="btn2" runat="server" Style="margin: 0px 0px 0px 12px; top: 0px; left: 0px;" Text="Search" OnClick="btn2_Click" TabIndex="3"></asp:Button>
            </span>
            </td>
            <td style="width: 100%">

            </td>
        </tr>
        <tr>
            <td style="width: 15%;padding-left:10px;">
                Order Type
            </td>
            <td style="width: 15%">
                <asp:UpdatePanel runat="server" ID="upOrderType">
                    <ContentTemplate>
                    <asp:DropDownList ID="drpOrderType" runat="server" AutoPostBack="true" Width="100%" CssClass="textboxStyleNew" TabIndex="4" OnSelectedIndexChanged="drpOrderType_SelectedIndexChanged"></asp:DropDownList>
                </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td style="width: 8%;padding-left:10px;"> Plant Code </td>
            <td style="width: 15%;"">
                <asp:UpdatePanel runat="server" ID="upPlantName">
                    <ContentTemplate>
                <asp:DropDownList ID="DrpPlant" runat="server" AutoPostBack="true" Width="100%" CssClass="textboxStyleNew" TabIndex="5"></asp:DropDownList>
                        </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td style="width: 7%;padding-left:10px">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ReportSearch" Height="31px" OnClick="btnSave_Click"  OnClientClick="btnSave_OnClientClick" TabIndex="6"/>
            </td>
            <td style="width: 100%"></td>
        </tr>
        </table>
    <%--    <div id="controlHead2" style="margin-top:10px; margin-left:5px;padding-right:10px;"></div>--%>
    <div id="divGrid" style="height: auto; overflow: auto; margin-top: 10px; margin-left: 5px; padding-right: 10px; height: 200px;">
        <asp:UpdatePanel ID="sdsad" runat="server">
            <ContentTemplate>
                <asp:GridView runat="server" ID="gvDetails" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" OnRowDataBound="gvDetails_RowDataBound"
                    OnSelectedIndexChanged="gvDetails_SelectedIndexChanged">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" AutoPostBack="true" OnCheckedChanged="checkAll_CheckedChanged" Text="Indent_No" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkStatus" runat="server" AutoPostBack="true" OnCheckedChanged="chkStatus_OnCheckedChanged" Text='<%# Eval("Indent_No").ToString() %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="170px" />
                            <ItemStyle HorizontalAlign="Left" Font-Bold="True" ForeColor="#000066" />
                        </asp:TemplateField>

                        <asp:BoundField HeaderText="Indent Date" DataField="Indent_Date" DataFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DistributorID" HeaderText="Distributor Code ">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Distributor Name" DataField="DistributorName">
                            <HeaderStyle Width="250px" HorizontalAlign="Left" />
                            <ItemStyle Width="250px" HorizontalAlign="Left" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    
    <div style="overflow: auto; height: 250px; margin: 10px 0px 0px 10px;">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" ShowFooter="True" GridLines="Horizontal" Width="1050px" BackColor="White"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True"
                    OnRowDataBound="GridView2_RowDataBound">
                    <Columns>
                        <%--<asp:BoundField HeaderText="Indent No" DataField="Indent_No">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Indent Date" DataField="Indent_Date" DataFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>--%>
                        <asp:BoundField HeaderText="Sales Office Code" DataField="SalesOfficeCode">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Distributor Code" DataField="DistributorID">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Product Code" DataField="Product_Code">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Product Name" DataField="PRODUCT_NAME">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="MRP" DataField="MRP" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Crate Qty" DataField="CRATES" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Box Qty" DataField="BOX" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="LTR Qty" DataField="LTR" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Indent No" DataField="Indent_No" Visible="false">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--</ContentTemplate>
      
    </asp:UpdatePanel> --%>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmLoadSheetCreation.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmLoadSheetCreation" %>

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
        function IsNumericn(e) {
            //debugger;
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
            //document.getElementById("lblError") = ret ? "none" : "inline";
            if (keyCode == 8 || keyCode == 189)
            { ret = true; }
            return ret;
        }

       
    </script>

     <style type="text/css">
       .hiddencol
            {
            display: none;
            }
    </style>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 0px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; color: white; margin: 6px 0px 0px 0px;">Load Sheet Creation</span>
    </div>

    <%-- <div style="border-bottom-style: solid; border-bottom-width: 5px; border-bottom-color: #000066" >--%>
    <table style="width: 100%; text-align: left">
        <tr>
            <td style="width: 3%">
                 <asp:Button ID="Button1" runat="server" Text="New" CssClass="button" Visible="False" />
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ReportSearch" OnClientClick="return confirm('Are you sure to save record?');" Height="31px" OnClick="btnSave_Click" />
               </td>
            <td style="width: 3%">
                <asp:Button ID="btnSavePrint" runat="server" Text="Save & Print" CssClass="ReportSearch" OnClientClick="return confirm('Are you sure to save record?');" Height="31px" OnClick="btnSavePrint_Click" />
            </td>
           
             <td style="width: 50%">
                <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Text="Label" Visible="false"></asp:Label>
            </td>
            <td style="width: 15%; text-align: right">
                <asp:DropDownList ID="drpSearch" runat="server" AutoPostBack="true" Width="200px" OnSelectedIndexChanged="drpSearch_SelectedIndexChanged">
                    <asp:ListItem>SO Number</asp:ListItem>
                    <asp:ListItem>Customer Group</asp:ListItem>
                    <asp:ListItem>PSR Code</asp:ListItem>
                    <asp:ListItem>Date</asp:ListItem>
                </asp:DropDownList>
            </td>

            <td style="width: 25%; text-align: center">
                <div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtSerch" runat="server" placeholder="Search here..." Height="15px" Width="187px" />
                            <span id="span1" onmouseover="test()" onmouseout="test1()">
                                <asp:Button ID="btn2" runat="server" Style="margin: 0px 0px 0px 12px; top: 0px; left: 0px;" Text="Search" OnClick="btn2_Click"></asp:Button>
                            </span>
                            </div>
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtSerch" Format="dd/MMM/yyyy"></asp:CalendarExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
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
                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" AutoPostBack="true" OnCheckedChanged="checkAll_CheckedChanged" Text="So_No" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkStatus" runat="server" AutoPostBack="true" OnCheckedChanged="chkStatus_OnCheckedChanged" Text='<%# Eval("SO_No").ToString() %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="170px" />
                            <ItemStyle HorizontalAlign="Left" Font-Bold="True" ForeColor="#000066" />
                        </asp:TemplateField>

                        <asp:BoundField HeaderText="So Date" DataField="SO_DATE" DataFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Width="80px" HorizontalAlign="Left" />
                            <ItemStyle Width="80px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Customer Group" DataField="CUST_GROUP">
                            <HeaderStyle Width="80px" HorizontalAlign="Left" />
                            <ItemStyle Width="80px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Customer_Code" HeaderText="Customer Code ">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Customer Name" DataField="CUSTOMER_NAME">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Customer Address" DataField="ADDRESS1">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="PSR Code" DataField="PSR_CODE">
                            <HeaderStyle Width="80px" HorizontalAlign="Left" />
                            <ItemStyle Width="80px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PSR_Name" HeaderText="PSR Name" />
                        <asp:BoundField DataField="PSR_BEAT" HeaderText="Beat Code" />
                        <asp:BoundField HeaderText="Beat Name" DataField="BeatName">
                            <HeaderStyle Width="80px" HorizontalAlign="Left" />
                            <ItemStyle Width="80px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Delievery Date" DataField="DELIVERY_DATE" DataFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Width="80px" HorizontalAlign="Left" />
                            <ItemStyle Width="80px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Value" DataField="SO_Value" DataFormatString="{0:n2}">
                            <HeaderStyle Width="80px" HorizontalAlign="Left" />
                            <ItemStyle Width="80px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="SOTYPE" DataField="SOTYPE">
                            <HeaderStyle Width="80px" HorizontalAlign="Left" />
                            <ItemStyle Width="80px" HorizontalAlign="Left" />
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


    <%-- <asp:UpdatePanel ID ="UppnalegridDetails" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
    <div style="overflow: auto; height: 250px; margin: 10px 0px 0px 10px;">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" ShowFooter="True" GridLines="Horizontal" Width="1050px" BackColor="White"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True"
                    OnRowDataBound="GridView2_RowDataBound">
                    <Columns>
                        <asp:BoundField HeaderText="PRODUCT GROUP" DataField="PRODUCT_GROUP">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="PRODUCT CODE" DataField="PRODUCT_CODE">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="PRODUCT NAME" DataField="PRODUCT_NAME">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CRATES" DataField="CRATES" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        
                          <asp:TemplateField HeaderText="Box">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenField2" Visible="false" runat="server" Value='<%# Eval("Product_PackSize") %>' />
                                <asp:TextBox ID="txtBox" runat="server" onkeypress="return IsNumericn(event)" AutoPostBack="true" Height="16px" Width="69px" BackColor="SkyBlue" OnTextChanged="txtQtyBox_TextChanged"
                                    MaxLength="10" Text='<%# Convert.ToInt32(Eval("Box")) %>' BorderStyle="None"  DataFormatString="{0:n2}" style="text-align:right"></asp:TextBox>
                            </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                          <asp:TemplateField HeaderText="PCS">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPCS" runat="server" onkeypress="return IsNumericn(event)" AutoPostBack="true" Height="16px" Width="69px" BackColor="SkyBlue" OnTextChanged="txtQtyBox_TextChanged"
                                    MaxLength="10" Text='<%# Convert.ToDecimal(Eval("PCS")) %>' BorderStyle="None"  DataFormatString="{0:n2}" style="text-align:right"></asp:TextBox>
                            </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="TotalQtyConv"  >
                            <ItemTemplate>
                                <asp:TextBox ID="txtqty" runat="server" onkeypress="return IsNumericn(event)" ReadOnly="true" Height="16px" Width="69px" BackColor="SkyBlue" 
                                    MaxLength="10" Text='<%# Convert.ToDecimal(Eval("TotalQty")) %>' BorderStyle="None"  DataFormatString="{0:n2}" style="text-align:right"></asp:TextBox>
                            </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="TotalBoxPcs" >  <%--HeaderStyle-CssClass="hiddencol" --%>
                            <ItemTemplate>
                                <asp:TextBox ID="txtBoxPcs" runat="server" onkeypress="return IsNumericn(event)" ReadOnly="true" Height="16px" Width="69px" BackColor="SkyBlue" 
                                    MaxLength="10" Text='<%# Convert.ToDecimal(Eval("BoxPcs")) %>' BorderStyle="None"  DataFormatString="{0:n2}" style="text-align:right"></asp:TextBox>
                            </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="LTR" DataField="LTR" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="StockBox" DataField="StockBox" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="StockLtr" HeaderText="StockLtr" DataFormatString="{0:n2}">
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--</ContentTemplate>
      
    </asp:UpdatePanel> --%>
</asp:Content>

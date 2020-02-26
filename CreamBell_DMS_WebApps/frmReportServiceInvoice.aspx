<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmReportServiceInvoice.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmReportServiceInvoice" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
            if (keyCode == 8 || keyCode == 46 || keyCode == 189 || keyCode == 61 || keyCode == 45)
            { ret = true; }
            return ret;
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
        .auto-style7 {
            width: 49px;
        }
        .auto-style12 {
            width: 61px;
        }
        .auto-style13 {
            width: 10%;
        }
        .auto-style14 {
            width: 132px;
        }
        .auto-style15 {
            width: 8%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <asp:UpdatePanel ID="upanel" runat="server">
        <ContentTemplate>
        
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
            <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>

            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px;">
                Service Invoice
            </div>

            <table style="width: 100%; vertical-align:top;">
                <tr>
                    <td>From Date :</td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px" onchange="ValidateDate(this)" required></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <td class="auto-style9">To Date :</td>
                        <td class="auto-style10">
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px" onchange="ValidateDate(this)" required></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                     <td >State</td>
                        <td class="auto-style2">
                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" Height="22px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" Width="120px">
                            </asp:DropDownList>
                        </td>
                    <td class="auto-style13">Distributor: </td>
                    <td >
                        <asp:DropDownList ID="ddlSiteId" runat="server" AutoPostBack="True" Height="22px" Width="150">
                        </asp:DropDownList>
                    </td>
                    <td>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="ReportSearch" Height="31px" Width="68px" />
                            <asp:Button ID="btnMultiPrint" runat="server" Text="Multipleprint" CssClass="ReportSearch" OnClick="btnMultiPrint_Click" Height="31px" Width="100px" />
                     </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Medium" Visible="false"></asp:Label>
                    </td>
                 </tr>
            </table>
            <div style="overflow: auto; height: 260px; margin: 10px 0px 0px 10px;">
           <asp:GridView ID="GridView1" runat="server" ShowFooter="false" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                            BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True"  OnRowDataBound="GridView1_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" AutoPostBack="true" OnCheckedChanged="checkAll_CheckedChanged" Text="All" />
                            </HeaderTemplate>
                                    <ItemTemplate>
                                        <%--<asp:CheckBox ID="chklist" runat="server" Text='<%# Bind("INVOICE_NO") %>'   />--%>
                                        <asp:CheckBox ID="chklist" Text ="  " runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice No">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtn" runat="server" Text='<%# Bind("INVOICENO") %>' OnClick="lnkbtn_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                    <ItemStyle HorizontalAlign="Left" Width="120px" />
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Invoice Date" DataField="INVOICEDATE" DataFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle Width="120px" HorizontalAlign="Left" />
                                    <ItemStyle Width="120px" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="SiteId" DataField="SITEID" DataFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle Width="120px" HorizontalAlign="Left" />
                                    <ItemStyle Width="120px" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Site Name" DataField="NAME" DataFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle Width="120px" HorizontalAlign="Left" />
                                    <ItemStyle Width="120px" HorizontalAlign="Left" />
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
                                        <asp:HiddenField ID="hndSiteid" Value='<%# Eval("SITEID") %>' Visible="false" runat="server" />
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
               
                <asp:GridView ID="GridView2" runat="server" ShowFooter="false" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                    
                    <Columns>
                        <asp:BoundField HeaderText="Service Code" DataField="SERVICE_CODE">
                            <HeaderStyle HorizontalAlign="Center" Width="80px" />
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SERVICE_NAME" HeaderText="Service Name">
                            <HeaderStyle HorizontalAlign="Center" Width="200px" />
                            <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Line Amount" DataField="LINEAMOUNT">
                            <HeaderStyle Width="80px" HorizontalAlign="Center" />
                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TAX PER" DataFormatString="{0:n2}" HeaderText="Tax1%">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TAX AMOUNT" DataFormatString="{0:n2}" HeaderText="Tax1 Amount">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ADD TAX PER" DataFormatString="{0:n2}" HeaderText="Tax2%">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ADD TAX AMOUNT" DataFormatString="{0:n2}" HeaderText="Tax2 Amount">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AMOUNT" DataFormatString="{0:n2}" HeaderText="Amount">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
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
            <asp:PostBackTrigger ControlID="btnMultiPrint" />
            <asp:PostBackTrigger ControlID="txtFromDate" />
        </Triggers>
        </asp:UpdatePanel>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmVenderPayment.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmVenderPayment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        /*DropDownCss*/
        .ddl {
            background-image: url('Images/arrow-down-icon-black.png');
        }

            .ddl:hover {
                background-image: url('Images/arrow-down-icon-black.png');
            }

        .cf {
        }

        .button {
        }
    </style>
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
            $("#controlHead1").append(gridHeader);
            $('#controlHead1').css('position', 'absolute');
            $('#controlHead1').css('top', '129');

        });

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

        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to save data?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <script type="text/javascript">

        function checkDate(sender, args) {
            if (sender._selectedDate >= new Date()) {
                alert("You cannot select a day after than today!");
                sender._selectedDate = new Date();
                // set the date back to the current date
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
        }
    </script>
  <%--  <script type="text/javascript">
        function fnc(value, remaining) {
            if (parseInt(value) < parseInt(remaining))
                return value;

            else return;
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px;">
        <b>Payment Entry</b>
    </div>
    <div>
        <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
            <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSave" />
                    <asp:PostBackTrigger ControlID="tnShow" />
                </Triggers>
                <ContentTemplate>
                    <table style="width: 100%; border-spacing: 0px">
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                        <tr>
                            <td>From Date :</td>
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" Width="90px" AutoPostBack="true" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                            </td>

                            <td>To Date :</td>
                            <td>
                                <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" Width="90px" AutoPostBack="true" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                            </td>
                            <td>Supplier Name</td>
                            <td>
                                <asp:DropDownList ID="drpPlant" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPlant_SelectedIndexChanged" Style="width: 215px" Height="20px"></asp:DropDownList></td>
                            <td>&nbsp;</td>
                            <td>Payment Date</td>
                            <td>
                                <asp:TextBox ID="txtPaymentDate" runat="server" Width="200"></asp:TextBox>
                                <asp:CheckBox ID="chkboxSelectAll" runat="server" Text="Auto Fill Amt" AutoPostBack="true" OnCheckedChanged="chkboxSelectAll_CheckedChanged"  />
                                <%-- <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtPaymentDate" Format="dd/MMM/yyyy" OnClientDateSelectionChanged="checkDate"></asp:CalendarExtender>--%> 
                            </td>


                            <td>
                                <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Names="Seoge UI"> </asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="tnShow" runat="server" Text="Show" CssClass="button" Height="31px" OnClick="tnShow_Click" Width="70" AutoPostBack="True" />
                            </td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" Height="31px" OnClick="btnSave_Click" Width="70" />
                            </td>

                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>


    <%--  <div id="controlHead1" style="margin-top: 10px; margin-left: 10px; padding-right: 10px;width:98%"></div>--%>

    <div style="overflow: auto; height: 500px; margin: 10px 0px 0px 10px; width: 99%">

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:GridView ID="gvDetails" runat="server" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" BackColor="White" Width="99%"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="2" ShowHeaderWhenEmpty="True" OnRowDataBound="gvDetails_RowDataBound" PageSize="20">
                    <AlternatingRowStyle BackColor="#ccffcc" />
                    <Columns>
                        <asp:TemplateField Visible="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenField2" Visible="false" runat="server" Value='<%# Eval("supplier_code") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="0px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Supplier Code" DataField="supplier_code">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Name" DataField="suppliername">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Address" DataField="address">
                            <HeaderStyle Width="200px" HorizontalAlign="Left" />
                            <ItemStyle Width="200px" HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="Instrument">
                            <ItemTemplate>
                                <asp:DropDownList ID="drpInstrument" Width="100px" CssClass="dropdown" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="InstrumentNo">
                            <ItemTemplate>
                                <asp:TextBox ID="txtInstrument" Width="70px" runat="server" CssClass="textboxStyleNew" MaxLength="25" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:TemplateField>

                        <asp:BoundField HeaderText="RefDoc.No" DataField="DOCUMENT_NO">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="RefDoc.Date" DataField="documentdate">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Total Amount" DataField="Total_Amount">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Pending Amount" DataField="RemainingAmount">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:BoundField>

                        <%--<asp:TemplateField HeaderText="RefDoc.No">
                            <ItemTemplate>
                                <asp:DropDownList ID="drpRefDocument" Width="120px" CssClass="dropdown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpRefDocument_SelectedIndexChanged1" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="RefDoc.Date">
                            <ItemTemplate>
                                <asp:Label ID="lblRefDocDate" Width="70px" DataFormatString="{0:dd-MMM-yyyy}" runat="server" AutoPostBack="true" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="70px" />
                            <ItemStyle HorizontalAlign="Left" Width="70px" />
                        </asp:TemplateField>--%>


                        <%--<asp:TemplateField HeaderText="PendingAmount">
                            <ItemTemplate>
                                <asp:Label ID="lblPendingAmount" Width="70px" DataFormatString="{0:n2}" runat="server" AutoPostBack="true" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Right" Width="70px" />
                            <ItemStyle HorizontalAlign="Right" Width="70px" />
                        </asp:TemplateField>--%>

                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAmount" Width="90px" runat="server" CssClass="textboxStyleNew"  onkeypress="return isNumberKeyWithDecimal(event,this)"  OnTextChanged="txtAmount_TextChanged" AutoPostBack="true" MaxLength="15" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="90px" />
                            <ItemStyle HorizontalAlign="Center" Width="90px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <asp:TextBox ID="txtRemark" Width="120px" runat="server" CssClass="textboxStyleNew" TextMode="MultiLine" MaxLength="500" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle HorizontalAlign="Left" Width="120px" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

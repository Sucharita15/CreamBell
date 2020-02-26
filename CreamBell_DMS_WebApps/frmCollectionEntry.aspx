<%@ Page Title="Collection Entry" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmCollectionEntry.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmCollectionEntry" %>

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

    <script type="text/javascript">

        function imgBtnSave_Click(btn) {
            var answer = confirm('Are you sure you want to Save?');
            if (answer) {
                btn.style.display = "none";
                return true;
            }
            else
                return false;

        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <div style="width: 99%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px;">
        <b>Collection Entry</b>
    </div>
    <div>
        <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
            <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSave" />
                    <asp:PostBackTrigger ControlID="tnShow" />
                    <asp:PostBackTrigger ControlID="gvDetails" />
                </Triggers>
                <ContentTemplate>
                    <table style="width: 100%; border-spacing: 0px">
                        <tr>
                            <td>Customer Group</td>
                            <td>
                                <asp:DropDownList ID="drpCustomerGroup" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpCustomerGroup_SelectedIndexChanged" Style="width: 215px" Height="20px"></asp:DropDownList></td>
                            <td>&nbsp;</td>
                            <td>Collection Date</td>
                            <td>
                                <asp:TextBox ID="txtCollectionDate" runat="server" Width="150"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtCollectionDate" Format="dd/MMM/yyyy" OnClientDateSelectionChanged="checkDate"></asp:CalendarExtender>
                            </td>
                            <td>Customer Name
                            </td>
                            <td>
                                <asp:DropDownList ID="drpCustomerName" runat="server" AutoPostBack="True" Style="width: 215px" Height="20px"></asp:DropDownList>

                            </td>
                            <td>
                                <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Names="Seoge UI"> </asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="tnShow" runat="server" Text="Show" CssClass="button" Height="31px" OnClick="tnShow_Click" Width="70" AutoPostBack="True" />
                            </td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" Height="31px" OnClick="btnSave_Click" Width="70" OnClientClick="javascript:return imgBtnSave_Click(this);" />
                            </td>
                            <td>
                                <asp:Button ID="btnGetData" runat="server" Text="Get Data" CssClass="button" Height="31px" OnClick="btnGetData_Click" Width="70" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>


    <%--  <div id="controlHead1" style="margin-top: 10px; margin-left: 10px; padding-right: 10px;width:98%"></div>--%>

    <div style="overflow: auto; margin: 10px 0px 0px 10px; width: 99%">

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="overflow: auto; height: 300px; margin: 10px 0px 0px 10px; width: 99%">
                <asp:GridView ID="gvDetails" runat="server" ShowFooter="false" GridLines="Horizontal" AutoGenerateColumns="False" BackColor="White" Width="99%" AllowPaging="true"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="2" ShowHeaderWhenEmpty="True" OnPageIndexChanging="OnPaging" OnRowDataBound="gvDetails_RowDataBound" PageSize="20">
                    <AlternatingRowStyle BackColor="#ccffcc" />
                    <Columns>
                        <asp:TemplateField Visible="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenField2" Visible="false" runat="server" Value='<%# Eval("CUSTOMER_CODE") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="0px"></ItemStyle>
                        </asp:TemplateField>

                        <asp:BoundField HeaderText="Customer Code - Name" DataField="Customer">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Address" HeaderText="Address">
                            <HeaderStyle HorizontalAlign="Left" Width="200px" />
                            <ItemStyle HorizontalAlign="Left" Width="200px" />
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
                                <asp:TextBox ID="txtInstrument" Width="100px" runat="server" CssClass="textboxStyleNew" MaxLength="25" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="RefDoc.No">
                            <ItemTemplate>
                                <asp:DropDownList ID="drpRefDocument" Width="120px" CssClass="dropdown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpRefDocument_SelectedIndexChanged" />
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
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PendingAmount">
                            <ItemTemplate>
                                <asp:Label ID="lblPendingAmount" Width="70px" DataFormatString="{0:n2}" runat="server" AutoPostBack="true" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Right" Width="70px" />
                            <ItemStyle HorizontalAlign="Right" Width="70px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <%--<asp:TextBox ID="txtAmount" Width="90px" runat="server" CssClass="textboxStyleNew" onkeypress="return IsNumeric(event)" OnTextChanged="txtAmount_TextChanged1" AutoPostBack="true" MaxLength="15" />--%>
                                <asp:TextBox ID="txtAmount" Width="90px" runat="server" CssClass="textboxStyleNew" onkeypress="return IsNumeric(event)" MaxLength="15" />
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
                    </div>

                 <div style=" width: 98%; height: 200px; margin-top: 20px">
                                    <asp:GridView ID="gridCollectionData" AutoGenerateColumns="true" ShowFooter="true" runat="server" Style="width: 100%" Font-Size="Small">
                                        <AlternatingRowStyle BackColor="#CCFFCC" />
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" CssClass="pgr" ForeColor="#000066"/>
                                        <RowStyle ForeColor="#000066" HorizontalAlign="Center"/>
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

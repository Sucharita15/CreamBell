<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmCustomerOutStandingWriteOff.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmCustomerOutStandingWriteOff" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            if (objRef.checked) {
                //If checked change color to Aqua
                row.style.backgroundColor = "aqua";
            }
            else {
                //If not checked change back to original color
                if (row.rowIndex % 2 == 0) {
                    //Alternating Row Color
                    row.style.backgroundColor = "#C2D69B";
                }
                else {
                    row.style.backgroundColor = "white";
                }
            }

            //Get the reference of GridView
            var GridView = row.parentNode;

            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];

                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;

        }
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
                        row.style.backgroundColor = "aqua";
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        //and change rowcolor back to original
                        if (row.rowIndex % 2 == 0) {
                            //Alternating Row Color
                            row.style.backgroundColor = "#C2D69B";
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
        function MouseEvents(objRef, evt) {
            var checkbox = objRef.getElementsByTagName("input")[0];
            if (evt.type == "mouseover") {
                objRef.style.backgroundColor = "orange";
            }
            else {
                if (checkbox.checked) {
                    objRef.style.backgroundColor = "aqua";
                }
                else if (evt.type == "mouseout") {
                    if (objRef.rowIndex % 2 == 0) {
                        //Alternating Row Color
                        objRef.style.backgroundColor = "#C2D69B";
                    }
                    else {
                        objRef.style.backgroundColor = "white";
                    }
                }
            }
        }
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
            $("#controlHead1").append(gridHeader);
            $('#controlHead1').css('position', 'absolute');
            $('#controlHead1').css('top', '129');

        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px;">
        <b>Outstanding Write-Off (Customer)</b>
    </div>

    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
        </Triggers>
        <ContentTemplate>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Small"> </asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
                <table style="width: 100%">
                    <tr>
                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 17%;">State :
                    <div style="max-height: 80px; overflow-y: auto;">
                        <asp:CheckBoxList ID="chkListState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkListState_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </div>
                        </td>
                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 17%;">Distributor :
                    <div style="max-height: 80px; overflow-y: auto;">
                        <asp:CheckBoxList ID="chkListSite" runat="server" OnSelectedIndexChanged="chkListSite_SelectedIndexChanged" AutoPostBack="True">
                        </asp:CheckBoxList>
                    </div>
                        </td>
                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 17%;">Customer Group:
                    <div style="max-height: 60px; overflow-y: auto; width: 100%;">
                        <asp:CheckBoxList ID="chkListCustomerGroup" runat="server" OnSelectedIndexChanged="chkListCustomerGroup_SelectedIndexChanged" AutoPostBack="true">
                        </asp:CheckBoxList>
                    </div>
                        </td>

                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 17%;">Customer Name:
                    <div style="max-height: 60px; overflow-y: auto; width: 100%;">
                        <asp:CheckBoxList ID="chkCustomerName" runat="server">
                        </asp:CheckBoxList>
                    </div>

                        </td>

                        <td style="width: 10%; text-align: center">
                            <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport0_Click" Text="Show Outstanding" Width="120px" ValidationGroup="ShowReport" />
                            <asp:ValidationSummary ID="vSummary" runat="server" ShowSummary="false" ShowMessageBox="true" ValidationGroup="ShowReport" />
                        </td>
                    </tr>


                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table style="width: 100%">
        <tr>
            <td style="width: 50%; text-align: left">
                <asp:Button ID="btnSave" runat="server" CssClass="ReportSearch" Height="31px" Text="Save" Width="60px" OnClick="btnSave_Click" /></td>
            <td style="width: 50%"><b>Total Outstanding :</b> &nbsp;
        <asp:Label ID="lblShowTotalOutStaning" runat="server" ForeColor="Red"></asp:Label></td>
        </tr>
    </table>
    <div id="Div1" style="margin-top: 10px; margin-left: 1px; padding-right: 10px; width: 99%; text-align: center">
    </div>
    <div style="overflow: auto; height: 500px; margin: 10px 0px 0px 10px; width: 99%">

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:GridView ID="gvDetails" runat="server" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" BackColor="White" Width="99%" DataKeyNames="Invoice_No"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="2" ShowHeaderWhenEmpty="True" PageSize="20" OnRowDataBound="gvDetails_RowDataBound">
                    <AlternatingRowStyle BackColor="#ccffcc" />
                    <Columns>

                        <asp:TemplateField Visible="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdInvoice_No" Visible="false" runat="server" Value='<%# Eval("Invoice_No") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="0px"></ItemStyle>
                        </asp:TemplateField>

                        <asp:BoundField HeaderText="SiteCode" DataField="SiteCode" Visible="false">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:BoundField>


                        <asp:TemplateField HeaderText="Customer Code" ItemStyle-Width="150px">
                            <ItemTemplate>
                                <asp:Label ID="lblCUSTOMER_CODE" runat="server" Text='<%# Eval("CUSTOMER_CODE") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="150px"></ItemStyle>
                        </asp:TemplateField>



                        <asp:BoundField HeaderText="Customer Name" DataField="Customer_Name">
                            <HeaderStyle Width="250px" HorizontalAlign="Left" />
                            <ItemStyle Width="250px" HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Invoice No." DataField="Invoice_No">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="Invoice Date" ItemStyle-Width="150px">
                            <ItemTemplate>
                                <asp:Label ID="lblDocument_Date" runat="server" Text='<%# Eval("Document_Date") %>' DataFormatString="{0:dd-M-yyyy}"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="150px"></ItemStyle>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Remaining Amount" ItemStyle-Width="150px">
                            <ItemTemplate>
                                <asp:Label ID="lblRemainingAmount" runat="server" Text='<%# Eval("RemainingAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="150px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkInvoice" runat="server" onclick="Check_Click(this)" />
                            </ItemTemplate>
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
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

<%@ Page Title="Primary Destruction Note" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmProductDistructionNote.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmProductDistructionNote" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />
    <link href="css/SSheet.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("select").searchable();
        });
    </script>
    <script type="text/javascript">

        function CheckNumeric(e) {          //--Only For Numbers //
            if (window.event) // IE 
            {
                if ((e.keyCode < 48 || e.keyCode > 57) & e.keyCode != 8) {
                    event.returnValue = false;
                    return false;
                }
            }
            else { // Fire Fox
                if ((e.which < 48 || e.which > 57) & e.which != 8) {
                    e.preventDefault();
                    return false;
                }
            }
        }
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
        .ddl {
            background-image: url('Images/arrow-down-icon-black.png');
        }

            .ddl:hover {
                background-image: url('Images/arrow-down-icon-black.png');
            }
    </style>
    <style type="text/css">
        /*DropDownCss*/
        .pnlCSS {
            font-weight: bold;
            cursor: pointer;
            /*border: solid 1px #c0c0c0;*/
            margin-left: 20px;
        }

        .CalendarCss {
            background-color: azure;
            color: darkblue;
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
            $("#controlHead").append(gridHeader);
            $('#controlHead').css('position', 'absolute');
            $('#controlHead').css('top', '129');
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdateProgress ID="upprogress" runat="server" AssociatedUpdatePanelID="sdsd" DisplayAfter="0">
        <ProgressTemplate>
            <div class="overlay"></div>
            <div class="overlayContent">
                <center>
                    Please Wait...don't close this window until processing is being done.
                     <br />
                    <img src="../../IMAGES/bar.gif" alt="" />
                </center>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div style="width: 98%; height: 18px; border-radius: 4px; margin: 6px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; color: white; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px;">DESTRUCTION NOTE</span>
        <asp:Label ID="Label1" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Names="Segoe UI" Font-Italic="true"></asp:Label>
    </div>

    <div style="width: 100%; padding: 3px 0px 5px 13px;">
        <table style="width: 100%">
            <%--<tr>
                <td style="padding: 0px">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" Height="24px" OnClick="btnSave_Click" Width="169px" />
                    &nbsp;
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="button" Height="24px" OnClick="btnRefresh_Click" />
                </td>
                <td>&nbsp;</td>
                <td>
                    <asp:UpdatePanel ID="updpnlErrorMsg" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Names="Segoe UI" Font-Italic="true"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>--%>
        </table>
    </div>

    <div style="width: 100%; margin-left: 4px">

        <asp:Panel ID="panelHeader" runat="server" GroupingText="Destruction Note" Width="98%">
            <asp:UpdatePanel ID="sdsd" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <table style="width: 98%; border-spacing: 0px">
                        <tr>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" Height="24px" OnClick="btnSave_Click" Width="50px" />
                            </td>
                            <td>
                                <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="button" Height="24px" OnClick="btnRefresh_Click" />
                            </td>


                            <td>Reciept No</td>
                            <td>
                                <asp:DropDownList ID="drpRecieptNo" runat="server" Width="140px" Height="23px" AutoPostBack="true"
                                    OnSelectedIndexChanged="drpRecieptNo_SelectedIndexChanged" CssClass="textboxStyleNew" Font-Size="8pt">
                                </asp:DropDownList>
                            </td>

                            <td>Reciept Date</td>
                            <td>
                                <asp:TextBox ID="txtReceiptDate" runat="server" CssClass="textboxStyleNew" Height="13px" ReadOnly="true" Width="100px"></asp:TextBox>
                                <%--<cc1:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="CalendarCss" Format="dd-MMM-yyyy" TargetControlID="txtReceiptDate">
                                </cc1:CalendarExtender>--%>
                            </td>

                            <td>Invoice No</td>
                            <td>
                                <asp:DropDownList ID="drpInvoceNo" runat="server" AutoPostBack="true" Height="23px" OnSelectedIndexChanged="drpInvoceNo_SelectedIndexChanged" CssClass="textboxStyleNew" Width="140px" Font-Size="8pt">
                                </asp:DropDownList>
                            </td>
                            <td>Invoice Date</td>
                            <td>

                                <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="textboxStyleNew" ReadOnly="true" Height="13px" Width="100px" />
                                <%-- <cc1:CalendarExtender ID="txtInvoiceDate_CalendarExtender" runat="server" CssClass="CalendarCss" Format="dd-MMM-yyyy" TargetControlID="txtInvoiceDate">
                                </cc1:CalendarExtender>--%>

                            </td>
                            <td class="auto-style2">Destruction Value</td>
                            <td>
                                <asp:TextBox ID="txtReceiptValue" runat="server" CssClass="textboxStyleNew" Height="13px" Width="60px" />
                            </td>
                        </tr>

                    </table>
                    <table style="width: 98%; border-spacing: 0px">
                        <tr>
                            <td style="width: 100%; text-align: left">

                                <div id="panelAdd" style="margin-top: 0px; width: 100%;">
                                    <asp:Panel ID="panelAddLine" runat="server" Width="100%">
                                        <table style="width: 100%;" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td style="width: 30%;">Product<asp:Label ID="lblHidden" runat="server" Visible="False"></asp:Label>
                                                </td>
                                                <td style="width: 5%;">Box</td>
                                                <td></td>
                                                <td style="width: 5%;">Used Qty</td>
                                                <td style="width: 5%;">Stock Qty</td>
                                                <td style="width: 15%;">Batch No</td>
                                                <td style="width: 5%;">MFD</td>
                                                <td style="width: 15%;">Remark</td>
                                                <td style="width: 5%;">Price</td>
                                                <td style="width: 5%;">TaxAmt</td>
                                                <td style="width: 5%;">Value</td>
                                                <td></td>
                                                <td style="width: 1%;"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="DDLMaterialCode" runat="server" TabIndex="1" OnSelectedIndexChanged="DDLMaterialCode_SelectedIndexChanged" CssClass="dropdownField" AutoPostBack="true" Width="100%" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBoxQty" runat="server" AutoPostBack="true" TabIndex="2" onkeypress="return IsNumeric(event)" Width="90%" CssClass="textfield" OnTextChanged="txtBoxQty_TextChanged" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPcsQty" runat="server" AutoPostBack="true" onkeypress="return IsNumeric(event)" Width="90%" CssClass="textfield" Visible="false"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUsedQty" runat="server" ReadOnly="True" Width="90%" CssClass="textfield" Style="background-color: rgb(235, 235, 228)" OnTextChanged="txtUsedQty_TextChanged"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStockQty" runat="server" ReadOnly="True" Width="90%" CssClass="textfield" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBatch" runat="server" AutoPostBack="true" TabIndex="3" onkeypress="return IsNumeric(event)" Width="98%" CssClass="textfield" /></td>
                                                <td>
                                                    <asp:TextBox ID="txtMDF" runat="server" CssClass="textfield" TabIndex="4" Height="13px" Width="100px" />
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtMDF">
                                                    </cc1:CalendarExtender>

                                                    <td>
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="textfield" TabIndex="5" Height="13px" Width="98%" /></td>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPrice" runat="server" ReadOnly="True" Width="90%" CssClass="textfield" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTaxAmt" runat="server" ReadOnly="True" Width="90%" CssClass="textfield" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValue" runat="server" ReadOnly="True" Width="90%" CssClass="textfield" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                                                </td>

                                                <td>
                                                    <asp:Button ID="BtnAddItem" runat="server" TabIndex="6" Text="Add" OnClick="BtnAddItem_Click" CssClass="button" BackColor="#0066CC" ForeColor="White" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPerBoxValue" runat="server" Visible="false" Width="0px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTotalTax" runat="server" Visible="false" Width="0px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtActualQty" runat="server" Visible="false" Width="0px"></asp:TextBox>
                                                </td>
                                            </tr>


                                        </table>
                                    </asp:Panel>
                                </div>

                            </td>
                        </tr>

                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="drpRecieptNo" />
                    <asp:PostBackTrigger ControlID="btnSave" />
                    <asp:PostBackTrigger ControlID="btnRefresh" />
                    <asp:PostBackTrigger ControlID="drpInvoceNo" />
                </Triggers>
            </asp:UpdatePanel>

        </asp:Panel>
    </div>

    <div id="controlHead" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; text-align: left"></div>

    <div style="height: 300px; overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; text-align: left">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:GridView runat="server" ID="gvDetails" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                    CellPadding="3" ShowHeaderWhenEmpty="True">

                    <AlternatingRowStyle BackColor="#CCFFCC" />
                    <Columns>
                        <asp:TemplateField HeaderText="SNo">
                            <ItemTemplate>
                                <span><%#Container.DataItemIndex + 1%></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                        <asp:BoundField DataField="PRODUCTName" HeaderText="Product Name" />
                        <asp:BoundField DataField="QtyBox" DataFormatString="{0:n2}" HeaderText="Box" />
                        <asp:BoundField DataField="QtyPcs" DataFormatString="{0:n2}" HeaderText="Pcs" Visible="false" />
                        <asp:BoundField DataField="BatchNo" HeaderText="BatchNo" />
                        <asp:BoundField DataField="MFD" HeaderText="MFD" />
                        <asp:BoundField DataField="REMARK" HeaderText="REMARK" />
                        <asp:BoundField DataField="Price" DataFormatString="{0:n2}" HeaderText="Price" />
                        <asp:BoundField DataField="Value" DataFormatString="{0:n2}" HeaderText="Value" />
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnDel" runat="server" Text='Delete' OnClick="lnkbtnDel_Click" ForeColor="Black"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>

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

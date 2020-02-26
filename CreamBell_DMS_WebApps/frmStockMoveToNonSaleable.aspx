<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmStockMoveToNonSaleable.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmStockMoveToNonSaleble" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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

        /*.ddlFont {  
    font-weight:bold !important; 
    font-size:10pt !important; 
    }*/

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
    <asp:UpdateProgress ID="upprogress" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
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
        <span style="font-family: Segoe UI; color: white; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px;">Stock Movement (Main to Non-Saleable)</span>
    </div>


    <div style="width: 100%; padding: 3px 0px 5px 13px;">
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:RadioButton ID="rdprimarytononsalable" runat="server" Text="Primary to Non-Saleable" AutoPostBack="true" OnCheckedChanged="rdprimarytononsalable_CheckedChanged" ValidationGroup="aa" GroupName="aa" Checked="true" />
                    <asp:RadioButton ID="rdnonsaletostock" runat="server" Text="Non-Saleable to Stock" AutoPostBack="true" OnCheckedChanged="rdprimarytononsalable_CheckedChanged" ValidationGroup="aa" GroupName="aa" />
                </td>
                <td style="padding: 0px">
                    <asp:Button ID="BtnSavePurchaseReturn" runat="server" Text="Stock Move " CssClass="button" Height="20px" OnClick="BtnSave_Click" Width="169px" />
                    &nbsp;
                    <asp:Button ID="BtnRefresh" runat="server" Text="Refresh" CssClass="button" Height="20px" OnClick="BtnRefresh_Click" />
                </td>
                <td>&nbsp;</td>
                <td>
                    <asp:UpdatePanel ID="updpnlErrorMsg" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Names="Segoe UI" Font-Italic="true"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>


    <div style="width: 100%; margin-left: 4px">

        <asp:Panel ID="panelHeader" runat="server" GroupingText="Stock Move To Non Saleable" Width="98%">
            <asp:UpdatePanel ID="sdsd" runat="server">
                <ContentTemplate>

                    <table style="width: 98%; border-spacing: 0px">
                        <tr>
                            <td>Reciept No</td>
                            <td>
                                <asp:DropDownList ID="drpRecieptNo" runat="server" Width="160px" Height="23px" AutoPostBack="true"
                                    OnSelectedIndexChanged="drpRecieptNo_SelectedIndexChanged" CssClass="textboxStyleNew" Font-Size="8pt">
                                </asp:DropDownList>
                            </td>

                            <td>Reciept Date</td>
                            <td>
                                <asp:TextBox ID="txtReceiptDate" runat="server" CssClass="textboxStyleNew" Height="13px" ReadOnly="true" Width="150px"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="CalendarCss" Format="dd-MMM-yyyy" TargetControlID="txtReceiptDate">
                                </cc1:CalendarExtender>
                            </td>

                            <td>Invoice No</td>
                            <td>
                                <asp:DropDownList ID="ddlInvoceNo" runat="server" AutoPostBack="true" Height="23px" OnSelectedIndexChanged="drpRecieptNo_SelectedIndexChanged" Width="160px" CssClass="textboxStyleNew" Font-Size="8pt">
                                </asp:DropDownList>
                            </td>
                            <td>Invoice Date</td>
                            <td>

                                <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="textboxStyleNew" Height="13px" ReadOnly="true" Width="150px" />
                                <cc1:CalendarExtender ID="txtInvoiceDate_CalendarExtender" runat="server" CssClass="CalendarCss" Format="dd-MMM-yyyy" TargetControlID="txtInvoiceDate">
                                </cc1:CalendarExtender>

                            </td>
                            <td class="auto-style2">Receipt Value</td>
                            <td>
                                <asp:TextBox ID="txtReceiptValue" runat="server" CssClass="textboxStyleNew" Height="13px" ReadOnly="true" Width="150px" />

                            </td>
                        </tr>

                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>

        </asp:Panel>
    </div>

    <div id="controlHead" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; text-align: left"></div>

    <div style="height: 350px; overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; text-align: left">
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
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenValueLineNo" runat="server" Value='<%# Eval("LINE_NO") %>' Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PRODUCTDESC" HeaderText="Code/Product Name" />
                        <asp:BoundField DataField="PRODUCT_MRP" DataFormatString="{0:n2}" HeaderText="MRP" />
                        <asp:TemplateField HeaderText="BOX">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQty" runat="server" AutoPostBack="true" Font-Size="X-Small" onkeypress="CheckNumeric(event);" OnTextChanged="txtBox_TextChanged" Text='<%# Bind("BOX") %>' Width="40px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ValidateBoxQty" HeaderText="Box" />
                        <asp:BoundField DataField="ACTUAL_BOX" HeaderText="Inv. Box" />
                        <asp:BoundField DataField="CRATES" DataFormatString="{0:n2}" HeaderText="Crates" />
                        <asp:BoundField DataField="UOM" HeaderText="UT" />
                        <asp:BoundField DataField="LTR" DataFormatString="{0:n2}" HeaderText="Volume[Ltr]" />
                        <asp:BoundField DataField="RATE" DataFormatString="{0:n2}" HeaderText="Rate" />
                        <%--<asp:BoundField DataField="BASICVALUE" DataFormatString="{0:n2}" HeaderText="Value" />
                 <asp:BoundField DataField="TRDDISCPERC" DataFormatString="{0:n2}" HeaderText="TRD%" />
                 <asp:BoundField DataField="TRDDISCVALUE" DataFormatString="{0:n2}" HeaderText="TRDValue" />
                 <asp:BoundField DataField="PRICE_EQUALVALUE" DataFormatString="{0:n2}" HeaderText="Price Equal." />
                 <asp:BoundField DataField="VAT_INC_PERC" DataFormatString="{0:n2}" HeaderText="VATInc%" />
                 <asp:BoundField DataField="VAT_INC_PERCVALUE" DataFormatString="{0:n2}" HeaderText="VATIncValue" />
                 <asp:BoundField DataField="GROSSRATE" DataFormatString="{0:n2}" HeaderText="GrossRate" />--%>
                        <asp:BoundField DataField="AMOUNT" DataFormatString="{0:n2}" HeaderText="NetValue" />
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

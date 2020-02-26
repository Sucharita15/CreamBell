<%@ Page Title="Purchase Reversal" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPurchaseReversal.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPurchaseReversal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />

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


        function isNumberKey(evt) {

            var charCode = (evt.which) ? evt.which : event.keyCode
            //if (charCode > 31 && charCode < 46 && (charCode < 48 || charCode > 57))
            if (charCode < 46 || charCode > 57 || charCode == 47)
                return false;
            return true;
        }

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <div style="width: 98%; height: 18px; border-radius: 4px; margin: 6px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; color: white; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px;">Purchase Reversal</span>
    </div>


    <div style="width: 100%; padding: 3px 0px 5px 13px;">
        <table style="width: 100%">
            <tr>
                <td style="padding: 0px">
                    <asp:Button ID="BtnSavePurchaseReturn" runat="server" Text="Save Purchase Return " CssClass="button" Height="24px" OnClick="BtnSave_Click" Width="169px" />
                    &nbsp;
                    <asp:Button ID="BtnRefresh" runat="server" Text="Refresh" CssClass="button" Height="24px" />
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

        <asp:Panel ID="panelHeader" runat="server" GroupingText="Purchase Return Header Section" Width="98%">
            <asp:UpdatePanel ID="sdsd" runat="server">
                <ContentTemplate>



                    <table style="width: 99%; border-spacing: 0px">
                        <tr>
                            <td style="width:10%;">Reciept No</td>
                            <td style="width:20%;">
                                <asp:DropDownList ID="drpRecieptNo" runat="server" Width="98%" Height="23px" AutoPostBack="true"
                                    OnSelectedIndexChanged="drpRecieptNo_SelectedIndexChanged" CssClass="textboxStyleNew" Font-Size="8pt">
                                </asp:DropDownList>
                            </td>
                            <td style="width:5%;"></td>
                            <td style="width:10%;">Invoice No</td>
                            <td style="width:20%;">
                                <asp:DropDownList ID="ddlInvoceNo" runat="server" AutoPostBack="true" Height="23px" OnSelectedIndexChanged="drpRecieptNo_SelectedIndexChanged" Width="99%" CssClass="textboxStyleNew" Font-Size="8pt">
                                </asp:DropDownList>
                            </td>
                            <td style="width:5%;"></td>
                            <td style="width:10%;">Transporter Name
                            </td>
                            <td style="width:20%;">
                                <asp:TextBox ID="txtTransporterName" runat="server" Width="98%" CssClass="textboxStyleNew" Height="13px" />
                            </td>
                        </tr>

                        <tr>
                            <td style="width:10%;">Reciept Date</td>
                            <td style="width:20%;">
                                <asp:TextBox ID="txtReceiptDate" runat="server" Width="98%" ReadOnly="true" CssClass="textboxStyleNew" Height="13px"></asp:TextBox></td>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtReceiptDate" Format="dd-MMM-yyyy" CssClass="CalendarCss">
                            </cc1:CalendarExtender>
                            <td style="width:5%;"></td>
                            <td style="width:10%;">Invoice Date</td>
                            <td style="width:20%;">
                                <asp:TextBox ID="txtInvoiceDate" runat="server" Width="98%" CssClass="textboxStyleNew" Height="13px" /></td>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtInvoiceDate" Format="dd-MMM-yyyy" CssClass="CalendarCss"></cc1:CalendarExtender>
                            <td style="width:5%;"></td>
                            <td style="width:10%;">Vehicle Number</td>
                            <td style="width:20%;">
                                <asp:TextBox ID="txtVehicleNumber" runat="server" Width="98%" CssClass="textboxStyleNew" Height="13px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:10%;">Receipt Value</td>
                            <td style="width:20%;">
                                <asp:TextBox ID="txtReceiptValue" runat="server" Width="98%" CssClass="textboxStyleNew" Height="13px" />
                            </td>
                            <td style="width:5%;">
                            <td style="width:10%;">Indent No </td>
                            <td style="width:20%;">
                                <asp:TextBox ID="txtIndentNo" runat="server" Width="98%" CssClass="textboxStyleNew" Height="13px" />
                            </td>
                            <td style="width:5%;">
                            <td style="width:10%;">Vehicle Type</td>
                            <td style="width:20%;">
                                <asp:TextBox ID="txtVehicleType" runat="server" Width="98%" CssClass="textboxStyleNew" Height="13px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:10%;">Return Reason</td>
                            <td style="width:20%;">
                                <asp:DropDownList ID="drpReturnReason" runat="server" Height="23px" Width="98%" CssClass="textboxStyleNew" Font-Size="8pt">
                                </asp:DropDownList>
                            </td>
                            <td style="width:5%;"></td>
                            <td style="width:10%;">Indent Date</td>
                            <td style="width:20%;text-align:left;">
                                <asp:TextBox ID="txtIndentDate" runat="server" Width="98%" CssClass="textboxStyleNew" Height="13px" />
                                <cc1:CalendarExtender ID="txtIndentDate_CalendarExtender" runat="server" CssClass="CalendarCss" Format="dd-MMM-yyyy" TargetControlID="txtIndentDate">
                                </cc1:CalendarExtender>
                            </td>
                            <td style="width:5%;"> </td>
                            <td style="width:10%;">Driver Number</td>
                            <td style="width:20%;">
                                <asp:TextBox ID="txtDriverNumber" runat="server" Width="98%" CssClass="textboxStyleNew" Height="13px" />
                            </td>
                        </tr>

                        <tr>
                            <td style="width:10%;">Remark</td>
                            <td style="width:20%;" colspan="2">
                                <asp:TextBox ID="txtRemark" runat="server" Width="99%" CssClass="textboxStyleNew" Height="13px" />
                            </td>
                            <td></td>
                            <td style="width:20%;text-align:left;">
                            <asp:CheckBox ID="chkCompReversal" Width="98%" runat="server" AutoPostBack="true" Checked="false" Text="Complete Reversal" OnCheckedChanged="chkCompReversal_CheckedChanged"/>
                                </td>
                            <td style="width:5%;">
                            <td style="width:10%;">Reversal Value:</td>
                            <td style="width:20%;"><asp:TextBox ID="txtreversalVal" runat="server" CssClass="textboxStyleNew" Width="98%"></asp:TextBox> </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>

        </asp:Panel>
    </div>

    <div id="controlHead" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; text-align: left"></div>

    <div style="height: 200px; overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; text-align: left">
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
                    </Columns>

                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenValueLineNo" Visible="false" runat="server" Value='<%# Eval("LINE_NO") %>' />
                            </ItemTemplate>

                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Code/Product Name" DataField="PRODUCTDESC" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="MRP" DataField="PRODUCT_MRP" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Rcpt. Box" DataField="BOX" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="BOX">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQty" runat="server" Font-Size="X-Small" AutoPostBack="true" onkeypress="CheckNumeric(event);" OnTextChanged="txtBox_TextChanged" Width="40px" Text='<%# Bind("RBOX") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Crates" DataField="RCRATES" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="UT" DataField="UOM" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Volume[Ltr]" DataField="RLTR" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Rate" DataField="RATE" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Value" DataField="RBASICVALUE" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="DISC" DataField="RDISCOUNT" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="SEC DISC" DataField="RSEC_DISC_VAL" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="SCHEME DISC" DataField="RSCH_DISC_VAL" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="TRDValue" DataField="RTRDDISCVALUE" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Price Equal." DataField="RPRICE_EQUALVALUE" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="TAX1 VALUE" DataField="RTAXAMOUNT" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="TAX2 VALUE" DataField="RADD_TAX_AMOUNT" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="TAX1 COMPONENT" DataField="TAXCOMPONENT" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="TAX2 COMPONENT" DataField="ADDTAXCOMPONENT" />
                    </Columns>
                    <%--<Columns>
                        <asp:BoundField HeaderText="GrossRate" DataField="GROSSRATE" DataFormatString="{0:n2}" />
                    </Columns>--%>
                    <Columns>
                        <asp:BoundField HeaderText="NetValue" DataField="RAMOUNT" DataFormatString="{0:n2}" />
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

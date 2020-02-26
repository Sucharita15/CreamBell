<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmSDPurchaseInvoiceReceipt.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmSDPurchaseInvoiceReceipt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />

    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />

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

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31
              && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>

    >

        <script type="text/javascript">

            $(document).ready(function () {
                /*Code to copy the gridview header with style*/
                var gridHeader = $('#<%=GridPurchItems.ClientID%>').clone(true);
                /*Code to remove first ror which is header row*/
                $(gridHeader).find("tr:gt(0)").remove();
                $('#<%=GridPurchItems.ClientID%> tr th').each(function (i) {
                    /* Here Set Width of each th from gridview to new table th */
                    $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
                });
                $("#controlHead").append(gridHeader);
                $('#controlHead').css('position', 'absolute');
                $('#controlHead').css('top', '129');

            });
        </script>

    <style type="text/css">
        .auto-style1 {
            width: 98px;
        }

        .auto-style2 {
            width: 151px;
        }

        .auto-style3 {
            width: 123px;
        }

        .auto-style4 {
            width: 172px;
        }

        .auto-style6 {
            width: 138px;
        }

        .auto-style7 {
            width: 129px;
        }

        .auto-style8 {
            width: 100px;
        }
    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="sadasd" runat="server">
        <ContentTemplate>


            <div>
                <table style="width: 100%">
                    <tr>
                        <td style="padding: 0px">

                            <asp:Button ID="btnPostPurchaseInvoice" runat="server" Text="Post" CssClass="ReportSearch" Height="31px" OnClick="btnPostPurchaseInvoice_Click" />
                            &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="BtnRefresh" runat="server" Text="Refresh" CssClass="ReportSearch" Height="31px" OnClick="BtnRefresh_Click" />
                        </td>
                        <td style="padding: 0px 0px 0px 100px;" class="auto-style22">
                            <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Names="Segoe UI" Font-Italic="true"></asp:Label>
                        </td>
                        <td>
                            <%--<asp:TextBox ID="txtPurchDocumentNo" runat="server" Visible="False"></asp:TextBox>--%>
                        </td>
                    </tr>
                </table>
            </div>

            <div style="width: 98%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
                <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; color: white; margin: 6px 0px 0px 10px;">SD Purchase Invoice Reciept</span>
            </div>

            <div class="form-style-6" style="margin-left: -15px; width: 96%; height: 75px; margin-top: -10px">

                <asp:Panel ID="panelHeader" runat="server" GroupingText="Purchase Invoice Header Section" Width="1060px" >

                    <table style="width: 100%; border-spacing: 2px">
                        <tr>
                            <td class="auto-style1">Indent No</td>
                            <td class="auto-style2">
                                <%--<asp:DropDownList ID="DrpIndentNo" runat="server" AutoPostBack="True" Width="125px" >
                        </asp:DropDownList>--%>
                                <asp:TextBox ID="txtIndentNo" runat="server" Width="125px" />
                            </td>

                            <td class="auto-style8">
                                <%-- <asp:TextBox ID="txtIndentNo" runat="server" Visible="False"></asp:TextBox>--%>

                            </td>
                            <td class="auto-style3">Invoice No</td>
                            <td class="auto-style6">
                                <asp:TextBox ID="txtInvoiceNo" runat="server" Width="125px" ForeColor="DarkBlue" Font-Bold="true" Enabled="false" />
                            </td>
                            <td class="auto-style7">&nbsp;INV DATE :
                            </td>
                            <td class="auto-style4">
                                <asp:TextBox ID="txtReceiptDate" runat="server" Width="125px" />
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtReceiptDate" Format="dd-MMM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>INV. VALUE (₹) :
                            </td>
                            <td>
                                <asp:TextBox ID="txtReceiptValue" runat="server" onkeypress="return isNumberKey(event)" Width="125px" ForeColor="DarkBlue" Font-Bold="true" />
                            </td>

                        </tr>

                        <tr>
                            <td class="auto-style1">Indent Date</td>
                            <td class="auto-style2">
                                <asp:TextBox ID="txtIndentDate" runat="server" Width="125px"></asp:TextBox></td>

                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtIndentDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                            <td class="auto-style8">&nbsp;</td>
                            <td class="auto-style3">SO No</td>
                            <td class="auto-style6">
                                <asp:TextBox ID="txtSONo" runat="server" Width="125px" Enabled="false" /></td>
                            <%-- <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID ="txtInvoiceDate" Format="dd-MMM-yyyy" >

                        </asp:CalendarExtender>--%>

                            <td class="auto-style7">DistributorCode
                            </td>
                            <td>
                                <asp:TextBox ID="txtDistributorCode" runat="server" Width="125px" Enabled="false" />
                            </td>
                            <td>Distributor Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtDistributorName" runat="server" Width="125px" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">Transporter</td>
                            <td class="auto-style2">
                                <asp:TextBox ID="txtTransporterName" runat="server" Width="125px" /></td>
                            <td class="auto-style8">&nbsp;</td>
                            <td class="auto-style3">Driver</td>
                            <td class="auto-style6">
                                <asp:TextBox ID="txttransporterNo" runat="server" Width="125px" /></td>

                            <td class="auto-style7">Vehicle Number</td>
                            <td>
                                <asp:TextBox ID="txtvehicleNo" runat="server" MaxLength="50" Width="125px" />
                            </td>
                            <td class="auto-style3">Vehicle Type</td>
                            <td>
                                <asp:TextBox ID="txtVehicleType" runat="server" Width="125px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">Registration Date</td>
                            <td class="auto-style2">
                                <asp:TextBox ID="txtRegistrationdate" ReadOnly="true" runat="server" Width="125px" /></td>
                            <td class="auto-style8">&nbsp;</td>
                            <td class="auto-style3">GST TIN NO</td>
                            <td class="auto-style6">
                                <asp:TextBox ID="txtGstno" ReadOnly="true" runat="server" Width="125px" /></td>

                            <td class="auto-style7">Composition Scheme</td>
                            <td>
                                <asp:TextBox ID="txtcomposition" ReadOnly="true" runat="server" MaxLength="50" Width="125px" />
                            </td>
                            <td class="auto-style3"></td>
                            <td>
                                
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>

            <div id="controlHead" style="margin-top: 3px; margin-left: 5px; padding-right: 10px;visibility:hidden"></div>
            <div style="height: 306px; overflow: auto; margin-top: 30px; margin-left: 5px; padding-right: 10px;">

                <asp:GridView runat="server" ID="GridPurchItems" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                    CellPadding="3" ShowFooter="true">

                    <AlternatingRowStyle BackColor="PaleTurquoise" />
                    <%--<Columns>
                 <asp:TemplateField HeaderText="SNo">
                     <ItemTemplate>
                         <span><%#Container.DataItemIndex + 1%></span>
                     </ItemTemplate>
                 </asp:TemplateField>
               </Columns> --%>

                    <Columns>
                        <asp:TemplateField Visible="false" HeaderText="LINE NO">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenValueLineNo" runat="server" Value='<%# Eval("LINE_NO") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <Columns>
                        <asp:BoundField HeaderText="LINE NO" DataField="LINE_NO" />
                    </Columns>

                    <Columns>
                        <asp:BoundField HeaderText="PRODUCT" DataField="PRODUCTDESC" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="MRP" DataField="MRP" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />

                        </asp:BoundField>
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="BOX" DataField="BOX" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>

                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="CRATE" DataField="CRATES" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="LTR" DataField="LTR" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="UT" DataField="UOM">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>

                    </Columns>

                    <Columns>
                        <asp:BoundField HeaderText="RATE" DataField="RATE" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>

                    </Columns>
                    
                    <Columns>
                        <asp:BoundField HeaderText="DISC AMT" DataField="DISC_AMOUNT" DataFormatString="{0:n2}">

                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>
                      <Columns>
                        <asp:BoundField HeaderText="SEC DISC AMT" DataField="SEC_DISC_AMOUNT" DataFormatString="{0:n2}">

                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>
                      <Columns>
                        <asp:BoundField HeaderText="SCHEME DISC" DataField="SCHEMEDISCVALUE" DataFormatString="{0:n2}">

                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="TD" DataField="TDVALUE" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>
                    
                    <%--<Columns>
                 <asp:BoundField HeaderText="Value" DataField="BASICVALUE" DataFormatString="{0:n2}"/>
             </Columns>--%>
                    
                    <Columns>
                        <asp:BoundField HeaderText="TAX1 %" DataField="TAX_CODE" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>

                    <Columns>
                        <asp:BoundField HeaderText="TAX1 AMT" DataField="TAX_AMOUNT" DataFormatString="{0:n2}">

                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>

                    <Columns>
                        <asp:BoundField HeaderText="TAX2 %" DataField="ADDTAX_CODE" DataFormatString="{0:n2}">

                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>

                    <Columns>
                        <asp:BoundField HeaderText="TAX2 AMT" DataField="ADDTAX_AMOUNT" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>
                    
                    
                    <%--<Columns>
                 <asp:BoundField HeaderText="PRICE EQL." DataField="PRICE_EQUALVALUE" DataFormatString="{0:n2}"/>
             </Columns>--%>


                    <%--<Columns>
                 <asp:BoundField HeaderText="VAT INC%" DataField="VAT_INC_PERC" DataFormatString="{0:n2}" />
             </Columns>
             <Columns>
                 <asp:BoundField HeaderText="VAT INC Value" DataField="VAT_INC_PERCVALUE" DataFormatString="{0:n2}"/>
             </Columns>--%>
                    <Columns>
                        <asp:BoundField HeaderText="GROSS" DataField="LINEAMOUNT" DataFormatString="{0:n2}">

                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="NET" DataField="AMOUNT" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>

                    <Columns>
                        <asp:TemplateField HeaderText="REMARK">
                            <ItemTemplate >
                                <asp:TextBox ID="txtRemark" runat="server" Font-Size="X-Small" Width="80px" MaxLength="100"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="HSN CODE" DataField="HSNCODE" >

                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="TAX1 COMPONENT" DataField="TAXCOMPONENT" >

                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="TAX2 COMPONENT" DataField="ADDTAXCOMPONENT" >
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>

                    <HeaderStyle BackColor="#05345C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <RowStyle BackColor="White" ForeColor="#4A3C8C" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <SortedAscendingCellStyle BackColor="#F4F4FD" />
                    <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                    <SortedDescendingCellStyle BackColor="#D8D8F0" />
                    <SortedDescendingHeaderStyle BackColor="#3E3277" />
                    <FooterStyle BackColor="AliceBlue" CssClass="footerColor" />

                </asp:GridView>

            </div>

            <style type="text/css">
                .footerColor {
                    color: white;
                }
            </style>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

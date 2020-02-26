 <%@ Page Title="Sale Invoice Return" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmSaleInvoiceReturn.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmSaleInvoiceReturn" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <link href="css/btnSearch.css" rel="stylesheet" />
    
    <link href="css/Polaroide.css" rel="stylesheet" />
    
    <link href="css/textBoxDesign.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("select").searchable();
        });

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

        function BoxQtyError()
        {
            alert("Return Quantity cannot exceed the Balance Quantity")
        }

    </script>

    <style type="text/css">
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

            .input1:focus {
                outline: 0;
                background: #fff;
                -moz-box-shadow: 0 0 2px rgba(0,0,0,.8) inset;
                -webkit-box-shadow: 0 0 2px rgba(0,0,0,.8) inset;
                box-shadow: 0 0 2px rgba(0,0,0,.8) inset;
            }

            .input1::-webkit-input-placeholder {
                color: #999;
                font-weight: normal;
                font-style: italic;
            }

            .input1:-moz-placeholder {
                color: #999;
                font-weight: normal;
                font-style: italic;
            }

            .input1:-ms-input-placeholder {
                color: #999;
                font-weight: normal;
                font-style: italic;
            }
    </style>
    <style type="text/css">
        /*DropDownCss*/
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

            .ddl:hover {
                background: #add8e6;
                background-image: url('Images/arrow-down-icon-black.png');
                background-position: right;
                background-repeat: no-repeat;
                text-indent: 0.01px; /*In Firefox*/
                text-overflow: ''; /*In Firefox*/
            }
    </style>

    <style type="text/css">
        .ob_gBody tbody .ob_gC, .ob_gBody tbody .ob_gCW {
            height: 25px !important;
        }

        .ob_gHCnC {
            height: 0;
            background-image: none !important;
        }

        .ob_gFCont {
            font-size: 11px !important;
            color: #FF0000 !important;
        }

        .ob_gFCnC, .ob_gBLS, .ob_gBRS, .ob_gHCnL, .ob_gFCnL, .ob_gFCnR, .ob_gHCnR {
            background-image: none !important;
        }

        .ob_gHContWG .ob_gH .ob_gC, .ob_gHContWG .ob_gH .ob_gCW,
        .ob_gHCont .ob_gH .ob_gC, .ob_gHCont .ob_gH .ob_gCW {
            background-image: none !important;
            background-color: #DCE3E8 !important;
            border-bottom: 1px #A8AEBD solid !important;
            height: 25px !important;
        }

        .auto-style1 {
            height: 38px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 0px; background-color: #0085ca; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px; color: #FFFFFF;">Sale Reversal</span>
    </div>
    <%-- <div style="border-bottom-style: solid; border-bottom-width: 5px; border-bottom-color: #000066; width:1200px" >--%>
    <table>
        <tr>
            <td style="text-align: left">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ReportSearch" OnClick="btnSave_Click" />
            </td>
            <%--  <td>
                    &nbsp;</td>
                <td style="padding: 0px 0px 0px 300px;">
&nbsp;

&nbsp;</td>
                <td>
                    <div>
                   </div>
                   
                    </td>--%>
        </tr>
    </table>
    <%-- </div>--%>

    <%-- <div class="form-style-6">--%>
    <table style="width: 100%; text-align: left;">
        <tr>
            <td style="width: 8%;">Invoice Number</td>
            <td>
                <asp:DropDownList ID="drpInvNo" runat="server" Width="91%" OnSelectedIndexChanged="drpInvNo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></td>
            <td>Invoice Return Number</td>
            <td>
                <asp:TextBox ID="txtInvoiceReturnNo" runat="server" Width="91%" ReadOnly="true"></asp:TextBox></td>

            <td>Invoice Date</td>
            <td>
                <asp:TextBox ID="txtInvoiceDate" runat="server" Width="90%" ReadOnly="true"></asp:TextBox></td>
            <td>Invoice Return Date</td>
            <td>
                <asp:TextBox ID="txtInvoiceReturnDate" runat="server" readonly="true" Width="90%"></asp:TextBox>

                <%--<asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtInvoiceReturnDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>--%>
            </td>
        </tr>
        <tr>
            <td>Customer Group</td>
            <td>
                <asp:DropDownList ID="drpCustomerGroup" runat="server" Width="90%"></asp:DropDownList></td>

            <td>Transporter Name</td>
            <td>
                <asp:TextBox ID="txtTransporterName" runat="server" Width="90%" OnTextChanged="txtTransporterName_TextChanged"></asp:TextBox></td>
            <%--    </tr>
                        <tr>--%>
            <td>Customer Code-Name </td>
            <td>
                <asp:DropDownList ID="drpCustomerCode" runat="server" Width="91%"></asp:DropDownList></td>
            <td>Driver Name</td>
            <td>
                <asp:TextBox ID="txtDriverName" runat="server" Width="90%"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Mobile No</td>
            <td>
                <asp:TextBox ID="txtMobileNO" runat="server" Width="90%" ReadOnly="true"></asp:TextBox></td>
            <td>Driver Contact No</td>
            <td>
                <asp:TextBox ID="txtDriverContact" runat="server" Width="90%" OnTextChanged="txtDriverContact_TextChanged"></asp:TextBox></td>
            <%--   </tr>
                        <tr>--%>
            <td>TIN/VAT</td>
            <td>
                <asp:TextBox ID="txtTIN" runat="server" Width="90%" ReadOnly="true"></asp:TextBox></td>
            <td>Vehicle No</td>
            <td>
                <asp:TextBox ID="txtVehicleNo" runat="server" Width="90%" OnTextChanged="txtVehicleNo_TextChanged"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Address</td>
            <td>
                <asp:TextBox ID="txtAddress" runat="server" Width="90%" ReadOnly="true"></asp:TextBox></td>
                       <td>
             <asp:CheckBox ID="chkCompReversal" runat="server" AutoPostBack="true" Checked="false" Text="Complete Reversal" OnCheckedChanged="chkCompReversal_CheckedChanged"/>
            </td>
             <td>
                <asp:Label ID="lblsite" runat="server" Visible="false"></asp:Label></td>
 
        </tr>
    </table>


    <br />




<div style='overflow-x:scroll;width:1337px; margin:0 auto;'>

    <%--<div id="controlHead" style="margin-top:5px; margin-left:5px;padding-right:10px;width: 100%;"></div>--%>
    <div style="overflow: auto; margin-top: 0px; width: 100%;">


        <asp:GridView runat="server" ID="gvDetails" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" Width="99%" BackColor="White"
            BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" OnRowDataBound="gvDetails_RowDataBound">
            <AlternatingRowStyle BackColor="#CCFFCC" />
            <Columns>

                <%--0--%>
                <asp:TemplateField Visible="false" ItemStyle-Width="0px">
                    <ItemTemplate>
                        <asp:HiddenField ID="HiddenField2" Visible="false" runat="server" Value='<%# Eval("Product_Code") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="0px"></ItemStyle>
                </asp:TemplateField>
                <%--1--%>
                <asp:TemplateField HeaderText="Line_No" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="Line_No" runat="server" Text='<%#  Eval("Line_No") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                </asp:TemplateField>
                <%--2--%>
                <asp:TemplateField HeaderText="Product_Code-Name" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="Product" runat="server" Text='<%#  Eval("Product") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                </asp:TemplateField>
                <%--3--%>
                <asp:TemplateField HeaderText="Pack">
                    <ItemTemplate>
                        <asp:Label ID="Pack" runat="server" Text='<%#  Eval("Pack","{0:n2}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                </asp:TemplateField>
                <%--4--%>
                <asp:TemplateField HeaderText="MRP">
                    <ItemTemplate>
                        <asp:Label ID="MRP" runat="server" Text='<%#  Eval("MRP","{0:n2}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                </asp:TemplateField>
                <%----%>
                <%-- <asp:TemplateField HeaderText="SO Qty">
                     <ItemTemplate>
                         <asp:Label ID="SO_Qty" runat="server" Text='<%#  Eval("SO_Qty") %>'/>
                     </ItemTemplate>  
                      <HeaderStyle HorizontalAlign="Left" width="60px"/>
                      <ItemStyle HorizontalAlign="Left" width="60px" />
                 </asp:TemplateField>--%>
                <%--5--%>
                <asp:TemplateField HeaderText="Invoice_Qty">
                    <ItemTemplate>
                        <asp:Label ID="txtBox" Width="60px" runat="server" Text='<%#  Eval("Invoice_Qty","{0:n6}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                </asp:TemplateField>

                  <asp:TemplateField HeaderText="Balance_Qty">
                    <ItemTemplate>
                        <asp:Label ID="txtBalQty" Width="60px" runat="server" Text='<%#  Eval("Balance_Qty","{0:n6}") %>' />                        
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                </asp:TemplateField>

                  <asp:TemplateField HeaderText="Rerturn_Qty(Box)">
                    <ItemTemplate>
                        <asp:TextBox ID="txtRetQtyBox" Width="60px" onkeypress="CheckNumeric(event)" runat="server" placeholder="0" OnTextChanged="txtRetQtyBox_TextChanged" Text='<%#  Eval("Rerturn_Qty_Box") %>' AutoPostBack="True"></asp:TextBox>
                        <asp:HiddenField ID="hfRetQtyBox" runat="server" Value='<%#  Eval("Rerturn_Qty_Box") %>'/>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                </asp:TemplateField>

                  <asp:TemplateField HeaderText="Rerturn_Qty(Pcs)">
                    <ItemTemplate>
                        <asp:TextBox ID="txtRetQtyBPcs" Width="60px" runat="server" placeholder="0" OnTextChanged="txtRetQtyBox_TextChanged" onkeypress="CheckNumeric(event)" Text='<%#  Eval("Rerturn_Qty_Pcs") %>' AutoPostBack="True"></asp:TextBox>
                        <asp:HiddenField ID="hfRetQtyBPcs" runat="server" Value='<%#  Eval("Rerturn_Qty_Pcs") %>'/>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Total_Return Qty">
                    <ItemTemplate>
                        <asp:Label ID="txtTotalReturnQty" Width="60px" runat="server"   Text='<%#  Eval("Total_ReturnQty","{0:n6}") %>' />                        
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                </asp:TemplateField>
                <%----%>
                <%-- <asp:TemplateField HeaderText="StockQty"  >
                     <ItemTemplate >  
                         <asp:Label ID="StockQty" runat="server" Text='<%#  Eval("StockQty") %>' />
                     </ItemTemplate>  
                      <HeaderStyle HorizontalAlign="Right" width="60px"/>
                      <ItemStyle HorizontalAlign="Right" width="60px" />
                 </asp:TemplateField>--%>
                <%--6--%>
                <asp:TemplateField HeaderText="LTR">
                    <ItemTemplate>
                        <asp:Label ID="LTR" runat="server" Text='<%#  Eval("R_Ltr","{0:n2}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="60px" />
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                </asp:TemplateField>
                <%--7--%>
                <asp:TemplateField HeaderText="Rate">
                    <ItemTemplate>
                        <asp:Label ID="txtRate" Width="60px" runat="server" Text='<%#  Eval("Rate","{0:n2}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="60px" />
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                </asp:TemplateField>
                <%--8--%>
                <asp:TemplateField HeaderText="Tax1%">
                    <ItemTemplate>
                        <asp:Label ID="Tax" runat="server" Text='<%#  Eval("Tax_Code","{0:n2}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="60px" />
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                </asp:TemplateField>
                <%--9--%>
                <asp:TemplateField HeaderText="Tax2_Value">
                    <ItemTemplate>
                        <asp:Label ID="TaxValue" runat="server" Text='<%#  Eval("R_TAX_AMOUNT","{0:n2}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="60px" />
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                </asp:TemplateField>
                <%--10--%>
                <asp:TemplateField HeaderText="Tax2%">
                    <ItemTemplate>
                        <asp:Label ID="AddTax" runat="server" Text='<%#  Eval("AddTax_Code","{0:n2}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="60px" />
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                </asp:TemplateField>
                <%--11--%>
                <asp:TemplateField HeaderText="A.Tax2_Value">
                    <ItemTemplate>
                        <asp:Label ID="AddTaxValue" runat="server" Text='<%#  Eval("R_AddTax_Amount","{0:n2}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="60px" />
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                </asp:TemplateField>
                <%--12--%>
                <asp:TemplateField HeaderText="Disc %">
                    <ItemTemplate>
                        <asp:Label ID="Disc" runat="server" Text='<%#  Eval("Disc","{0:n2}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="60px" />
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                </asp:TemplateField>
                <%--13--%>
                <asp:TemplateField HeaderText="Disc Value">
                    <ItemTemplate>
                        <asp:Label ID="DiscValue" runat="server" Text='<%#  Eval("R_Disc_Amount","{0:n2}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="60px" />
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                </asp:TemplateField>
                <%--14--%>
                <asp:TemplateField HeaderText="SecDisc %">
                    <ItemTemplate>
                        <asp:Label ID="SecDisc" runat="server"  />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="60px" />
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                </asp:TemplateField>
                <%--15--%>
                <asp:TemplateField HeaderText="S.Disc Value">
                    <ItemTemplate>
                        <asp:Label ID="SecDiscValue" runat="server" Text='<%#  Eval("R_Sec_Disc_Amount","{0:n2}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="60px" />
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                </asp:TemplateField>
                 <%----%>
                <asp:TemplateField HeaderText="TDValue" >
                    <ItemTemplate>
                        <asp:Label ID="lblTDValue" runat="server" Text='<%#  Eval("R_TDValue","{0:n2}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="60px" />
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                </asp:TemplateField>
                 <%----%>
                <asp:TemplateField HeaderText="PEValue" >
                    <ItemTemplate>
                        <asp:Label ID="lblPEValue" runat="server" Text='<%#  Eval("R_PEValue","{0:n2}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="60px" />
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                </asp:TemplateField>
                  <%----%>
                <asp:TemplateField HeaderText="SCHEMEDISCPER" >
                    <ItemTemplate>
                        <asp:Label ID="lblSCHEMEDISCPER" runat="server" Text='<%#  Eval("SCHEMEDISCPER","{0:n2}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="60px" />
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                </asp:TemplateField>
                  <%----%>
                <asp:TemplateField HeaderText="SCHEMEDISCVALUE" >
                    <ItemTemplate>
                        <asp:Label ID="lblSCHEMEDISCVALUE" runat="server" Text='<%#  Eval("R_SCHEMEDISCVALUE","{0:n2}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="60px" />
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                </asp:TemplateField>
                <%--16--%>
                <asp:TemplateField HeaderText="Amount">
                    <ItemTemplate>
                        <asp:Label ID="Amount" runat="server" Text='<%#  Eval("R_Amount","{0:n2}") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="60px" />
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                </asp:TemplateField>
                <%--17--%>
                <asp:TemplateField HeaderText="Scheme" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblScheme" runat="server" Text='<%#  Eval("SchemeCode") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="60px" />
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                </asp:TemplateField>

                <asp:TemplateField Visible="false" ItemStyle-Width="0px">
                    <ItemTemplate>
                        <asp:HiddenField ID="hLineAmount" Visible="false" runat="server" Value='<%# Eval("R_LineAmount") %>' />
                        <asp:HiddenField ID="hDiscType" Visible="false" runat="server" Value='<%# Eval("DiscType") %>' />
                        <asp:HiddenField ID="hDiscCalculationType" Visible="false" runat="server" Value='<%# Eval("DiscCalculationBase") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="0px"></ItemStyle>
                </asp:TemplateField>
                
            </Columns>
            <EmptyDataTemplate>
                No Record Found...
            </EmptyDataTemplate>
            <FooterStyle CssClass="table-condensed" BackColor="White" ForeColor="#4A3C8C" />
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

    </div>
       <%--<div style="width: 80%; margin: 0px 0px 0px 50px">--%>

    <div style='overflow-x:scroll;width:1337px; margin:0 auto;'>

            <div style="overflow: auto; margin-top: 0px; width: 100%;">
                           
                           

                                      <%--  <div style="background-color: #C0C0C0; text-align: center; border-style: solid; border-width: thin; font-weight: bold; font-size: 15px; width: 100%">
                                            <strong>Scheme Detail</strong>
                                        </div>--%>

                                        <asp:GridView ID="gvScheme" runat="server" AutoGenerateColumns="False" GridLines="Horizontal" BackColor="White" Width="100%"
                                            BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" CellPadding="3"   style="margin-right: 0px">
                                            <AlternatingRowStyle BackColor="#CCFFCC" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                        <asp:HiddenField ID="hdnSchemeLine" Visible="false" runat="server" Value='<%# Eval("SchemeLineNo") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Scheme Code" DataField="SCHEMECODE"></asp:BoundField>
                                                <asp:BoundField HeaderText="Scheme Name" DataField="Scheme Description"></asp:BoundField>
                                                <asp:BoundField HeaderText="Item Group Name" DataField="Item Group Name"></asp:BoundField>
                                                <asp:BoundField HeaderText="Free Item Code" DataField="Free Item Code"></asp:BoundField>
                                                <asp:BoundField HeaderText="Free Item Name" DataField="Free Item Name"></asp:BoundField>
                                                <asp:BoundField HeaderText="Slab" DataField="FREEQTY"></asp:BoundField>
                                                <asp:BoundField HeaderText="Set" DataField="SetNo"></asp:BoundField>

                                                <asp:BoundField HeaderText="FreeBox" DataField="TotalFreeQty"></asp:BoundField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnTotalFreeQty" Visible="false" runat="server" Value='<%# Eval("TotalFreeQty") %>' />
                                                        <asp:HiddenField ID="hdnTotalFreeQtyPcs" Visible="false" runat="server" Value='<%# Eval("TotalFreeQtyPcs") %>' />
                                                    </ItemTemplate>

                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="BoxQty">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQty" runat="server" AutoPostBack="True" onkeypress="CheckNumeric(event)" OnTextChanged="txtQty_TextChanged" ReadOnly="True" Width="40px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="FreePcs" DataField="TotalFreeQtyPcs"></asp:BoundField>
                                                <asp:TemplateField HeaderText="PcsQty">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQtyPcs" runat="server" AutoPostBack="True" onkeypress="CheckNumeric(event)" OnTextChanged="txtQtyPcs_TextChanged" ReadOnly="True" Width="40px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="PackSize" DataField="Product_PackSize" DataFormatString="{0:0}"></asp:BoundField> 
                                                <asp:BoundField HeaderText="ConvBox" ></asp:BoundField>
                                                <asp:BoundField HeaderText="Rate" DataField="Rate" DataFormatString="{0:0.00}"></asp:BoundField>
                                                <asp:BoundField HeaderText="BasicAmt" DataFormatString="{0:n2}"></asp:BoundField>
                                                <asp:BoundField HeaderText="DiscPer" DataFormatString="{0:n2}"></asp:BoundField>
                                                <asp:BoundField HeaderText="DiscVal" DataFormatString="{0:n2}"></asp:BoundField>
                                                <asp:BoundField HeaderText="TaxableAmt" DataFormatString="{0:n2}"></asp:BoundField>
                                                <asp:BoundField HeaderText="Tax1" DataField="Tax1Per" DataFormatString="{0:n2}"></asp:BoundField>
                                                <asp:BoundField HeaderText="Tax1Amt" DataFormatString="{0:n2}"></asp:BoundField>
                                                <asp:BoundField HeaderText="Tax2" DataField="Tax2Per" DataFormatString="{0:n2}"></asp:BoundField>
                                                <asp:BoundField HeaderText="Tax2Amt" DataFormatString="{0:n2}"></asp:BoundField>
                                                <asp:BoundField HeaderText="Amount" DataFormatString="{0:n2}"></asp:BoundField>
                                                <asp:BoundField HeaderText="HSNCODE" DataField="HSNCODE"></asp:BoundField>
                                                <asp:BoundField HeaderText="Tax1Comp" DataField="Tax1Comp"></asp:BoundField>
                                                <asp:BoundField HeaderText="Tax2Comp" DataField="Tax2Comp"></asp:BoundField>
                                            </Columns>
                                            <HeaderStyle BackColor="#003366" ForeColor="White" />
                                        </asp:GridView>

                                   
                            <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                        </div>

        </div>
</asp:Content>

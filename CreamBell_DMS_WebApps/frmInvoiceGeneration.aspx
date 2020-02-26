<%@ Page Title="Invoice Generation" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmInvoiceGeneration.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmInvoiceGeneration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />

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

    <script type="text/javascript">
        $(document).ready(function () {
            $("select").searchable();
        });

        function InIEvent() {
            $(document).ready(function () {
                $("select").searchable();
            });
        }

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

        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        function IsNumeric(e) {
            //debugger;
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
            //document.getElementById("lblError") = ret ? "none" : "inline";
            if (keyCode == 8 || keyCode == 189 || keyCode == 61 || keyCode == 45)
            { ret = true; }
            return ret;
        }

        function isNumberKey(evt, obj) {
            if (obj.value.trim().indexOf('.') >= 0) {
                if (obj.value.trim().indexOf('.') != -1) {
                    return false;
                }
            }
            if (obj.value.trim().length - obj.value.trim().indexOf('.') > 2) {
                return false;
            }
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode != 46 && (charCode < 48 || charCode > 57)))
                return false;
            return true;
        }

        function OpenConfirmDialog() {
            if (confirm('Allowed Secondary/Trade Discount value will be reset due to change in quantity or new product addition')) {
                document.getElementById('<%= Hidden1.ClientID %>').value = true;
                //return true;
            }
            else {
                document.getElementById('<%= Hidden1.ClientID %>').value = false;
                //return false;
            }
        }


        function isNumberKeyWithDecimal(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31
              && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function IsNumericn(e) {
            //debugger;
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
            //document.getElementById("lblError") = ret ? "none" : "inline";
            if (keyCode == 8 || keyCode == 46 || keyCode == 189)
            { ret = true; }
            return ret;
        }

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

        .hiddencol {
            display: none;
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
        }

            .ddl:hover {
                background: #add8e6;
                background-image: url('Images/arrow-down-icon-black.png');
                background-position: right;
                background-repeat: no-repeat;
                text-indent: 0.01px; /*In Firefox*/
            }

        .button {
        }

        .auto-style2 {
            -webkit-border-radius: 0;
            -moz-border-radius: 0;
            border-radius: 0px;
            font-family: Arial;
            color: #ffffff;
            font-size: 11px;
            text-decoration: none;
            margin-bottom: 0px;
            padding-left: 8px;
            padding-right: 7px;
            padding-top: 5px;
            padding-bottom: 6px;
            background: #3498db url('linear-gradient(to%20bottom,%20#3498db, #2980b9)');
        }

        .auto-style3 {
            width: 6%;
        }

        .auto-style4 {
            width: 3%;
        }

        .auto-style5 {
            width: 3%;
        }

        .auto-style6 {
            width: 8%;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 98%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px; text-align: center;">
                <span style="font-family: Segoe UI; color: white; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px;">Invoice Save </span>
            </div>

            <table style="width: 100%; text-align: left">
                <tr>
                    <td class="auto-style4">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return confirm('Are you sure to save record?');" CssClass="ReportSearch" OnClick="btnSave_Click" /></td>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave"
                                    EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>

                    <td class="auto-style3">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnSavePrint" runat="server" Text="Save and Print" OnClientClick="return confirm('Are you sure you want to Save and Print the record?');" CssClass="ReportSearch" OnClick="btnSavePrint_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSavePrint"
                                    EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>

                    <td class="auto-style5">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="ReportSearch" OnClick="btnPreview_Click" />
                                <%--<asp:Button ID="Button1" runat="server" Text="Preview" CssClass="ReportSearch" OnClick="btnPreview_Click" OnClientClick="window.open('frmReport.aspx?SaleInvoiceNo=Temp-001&Type=SaleInvoicePreview','frmReport')" />--%>
                                <%--<asp:LinkButton ID="btn" runat="server" Text="Preview" CssClass="ReportSearch"></asp:LinkButton>--%>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSavePrint"
                                    EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>

                    <td class="auto-style5">
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ReportSearch" OnClick="btnBack_Click" /></td>
                    <td class="auto-style6">
                        <asp:RadioButton ID="rdAll" runat="server" AutoPostBack="true" Text="All Product" ToolTip="All Products Lists" ValidationGroup="ValItem" OnCheckedChanged="rdAll_CheckedChanged" GroupName="Rd" />
                        <asp:RadioButton ID="rdStock" runat="server" AutoPostBack="true" Text="Stock Product" Checked="true" ToolTip="Products available in stock" ValidationGroup="ValItem" GroupName="Rd" OnCheckedChanged="rdAll_CheckedChanged" />
                    </td>
                    <td class="auto-style5"><b>Data Display</b></td>
                    <td class="auto-style6">
                      <asp:RadioButton ID="rdAsc"  runat="server" AutoPostBack="true" Text="ASC" CheToolTip="Ascending order" Checked="true" ValidationGroup="Ordering"  GroupName="RdOrdering" OnCheckedChanged="rdAsc_CheckedChanged" />                      
                      <asp:RadioButton ID="rdDesc"  runat="server" AutoPostBack="true" Text="DESC"  ToolTip="Descending order" ValidationGroup="Ordering" GroupName="RdOrdering" OnCheckedChanged="rdAsc_CheckedChanged" />
                    </td>
                    <td style="width: 25%">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="#CC0000"></asp:Label>
                    </td><td></td>
                    <td>
                        <asp:HyperLink id="EwayBillLink" NavigateUrl="https://ewaybill.nic.in/" Text="ewaybill.nic.in" Target="_blank" runat="server"/> 
                    </td>
                </tr>
            </table>

            <div style="width: 99%; text-align: left">
                <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                    <ContentTemplate>
                        <asp:HiddenField ID="Hidden1" runat="server" />
                        <table style="width: 99%; table-layout: fixed;">
                            <tr>
                                <td style="width: 8%;">Customer Group</td>
                                <td style="width: 17%;">
                                    <asp:DropDownList ID="drpCustomerGroup" runat="server" Width="90%"></asp:DropDownList></td>
                                <td style="width: 8%;">SO Number</td>
                                <td style="width: 17%;">
                                    <asp:DropDownList ID="drpSONumber" runat="server" Width="90%"></asp:DropDownList></td>
                                <td style="width: 8%;">Transporter Name</td>
                                <td style="width: 17%;">
                                    <asp:TextBox ID="txtTransporterName" runat="server" Width="90%" OnTextChanged="txtTransporterName_TextChanged"></asp:TextBox></td>
                                <td style="width: 8%;">Remark</td>
                                <td style="width: 17%;" rowspan="2">
                                    <asp:TextBox ID="txtRemark" runat="server" Width="90%" TextMode="MultiLine" Height="40px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Customer Code-Name</td>
                                <td>
                                    <asp:DropDownList ID="drpCustomerCode" runat="server" Width="90%"></asp:DropDownList></td>
                                <td>SO Date</td>
                                <td>
                                    <asp:TextBox ID="txtSODate" runat="server" Width="90%" ReadOnly="True"></asp:TextBox></td>
                                <td>Driver Name</td>
                                <td>
                                    <asp:TextBox ID="txtDriverName" runat="server" Width="90%" OnTextChanged="txtDriverName_TextChanged"></asp:TextBox></td>
                                <td>
                                    <asp:Label ID="lblsite" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Bill To Address</td>
                                <td>
                                    <asp:TextBox ID="txtAddress" runat="server" Enabled="false" Width="90%" ReadOnly="true"></asp:TextBox></td>
                                <td>Loadsheet Number</td>
                                <td>
                                    <asp:TextBox ID="txtLoadSheetNumber" runat="server" Width="90%" ReadOnly="true"></asp:TextBox></td>
                                <td>Driver Contact No</td>
                                <td>
                                    <asp:TextBox ID="txtDriverContact" runat="server" Width="90%" OnTextChanged="txtDriverContact_TextChanged"></asp:TextBox></td>
                                <td>GST TIN No:</td>
                                <td>
                                    <asp:TextBox ID="txtGSTtin" runat="server" Width="90%" ReadOnly="true" />
                                </td>

                            </tr>
                            <tr>
                                <td>Bill To State</td>
                                <td>
                                    <asp:TextBox ID="txtBillToState" runat="server" ReadOnly="true" Width="90%"></asp:TextBox></td>
                                <td>Loadsheet Date</td>
                                <td>
                                    <asp:TextBox ID="txtLoadsheetDate" runat="server" Width="90%" ReadOnly="true"></asp:TextBox></td>
                                <td>Vehicle No</td>
                                <td>
                                    <asp:TextBox ID="txtVehicleNo" runat="server" Width="90%"></asp:TextBox></td>
                                <td>Mobile No</td>

                                <td>
                                    <asp:TextBox ID="txtMobileNO" runat="server" ReadOnly="true" Width="90%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Shipped To Address</td>
                                <td>
                                    <asp:DropDownList ID="ddlShipToAddress" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlShipToAddress_SelectedIndexChanged" CssClass="dropdownField" Width="90%"></asp:DropDownList></td>
                                <td>Invoice Value:</td>
                                <td aria-readonly="true">
                                    <asp:TextBox ID="txtinvoicevalue" runat="server" Text="" Width="90%" Enabled="false" ReadOnly="true"></asp:TextBox>
                                </td>


                                <td>Invoice Date</td>
                                <td>
                                    <asp:TextBox ID="txtInvoiceDate" runat="server" Width="90%" ReadOnly="true"></asp:TextBox></td>
                                <td>CompositonScheme:</td>
                                <td aria-readonly="true">
                                    <asp:CheckBox ID="chkCompositionScheme" runat="server" ReadOnly="true" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>Business Unit: </td>
                                <td>
                                    <asp:DropDownList ID="ddlBusinessUnit" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBusinessUnit_SelectedIndexChanged" CssClass="dropdownField" Width="90%"></asp:DropDownList>
                                </td>
                                <td>Customer Outstanding</td>
                                <td>
                                    <asp:TextBox ID="txtoutstanding" runat="server" Width="90%" ReadOnly="true"></asp:TextBox></td>
                                <td style="visibility: hidden">TIN/VAT</td>
                                <td>
                                    <asp:TextBox ID="txtTIN" Visible="false" runat="server" Width="90%" ReadOnly="true"></asp:TextBox></td>

                                <td style="visibility: hidden">Invoice Number</td>
                                <td>
                                    <asp:TextBox ID="txtInvoiceNo" Visible="false" runat="server" Width="90%" ReadOnly="true"></asp:TextBox></td>

                                <td style="visibility: hidden">GSTTIN Reg.Date :</td>
                                <td>
                                    <asp:TextBox ID="txtGSTtinRegistration" Visible="false" runat="server" Width="90%" ReadOnly="true" />
                                </td>

                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <%-- <div id="panelAdd" style="margin-top: 5px; width: 100%;">--%>

                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnAddItem" />
                            <asp:PostBackTrigger ControlID="DDLMaterialCode" />
                            <asp:PostBackTrigger ControlID="DDLProductGroup" />
                            <asp:PostBackTrigger ControlID="DDLProductSubCategory" />
                        </Triggers>
                      <ContentTemplate>--%>

                <asp:Panel ID="panelAddLine" runat="server" Width="100%">
                    <table style="border-spacing: 0px; width: 99%; table-layout: fixed;">
                        <tr>
                            <td style="width: 5%">Product Group</td>
                            <td style="width: 5%">Product Sub Category</td>
                            <td style="width: 10%">Product<asp:Label ID="lblHidden" runat="server" Visible="False"></asp:Label></td>
                            <td style="width: 5%">CrateQty</td>
                            <td style="width: 5%">BoxQty</td>
                            <td style="width: 5%">PCSQty</td>
                            <td style="width: 5%">TotalBox</td>
                            <td style="width: 5%">
                                <asp:Label ID="lblTotalPcs" runat="server" Text="TotalPcs"></asp:Label></td>
                            <td style="width: 5%">Total QtyConv</td>
                            <td style="width: 5%">BOXPCS</td>
                            <td style="width: 5%">Qty[Ltr]</td>
                            <td style="width: 5%">Price</td>
                            <td style="width: 5%">Value</td>
                            <td style="width: 5%">StockQty</td>
                            <td style="width: 3%">&nbsp;</td>
                            <td style="width: 5%">S.Dics%</td>
                            <td style="width: 5%">S.DicsValue</td>
                            <td style="width: 3%">&nbsp;</td>
                            <td style="width: 5%">TD Value</td>
                            <td style="width: 4%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="DDLProductGroup" runat="server" Width="98%" AutoPostBack="true" OnSelectedIndexChanged="ddlProductGroup_SelectedIndexChanged" TabIndex="1"></asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="DDLProductSubCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLProductSubCategory_SelectedIndexChanged" Width="98%" TabIndex="2"></asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="DDLMaterialCode" runat="server" Width="98%" OnSelectedIndexChanged="DDLMaterialCode_SelectedIndexChanged" AutoPostBack="true" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCrateQty" runat="server" AutoPostBack="true" placeholder="0" onkeypress="CheckNumeric(event)" Width="98%" OnTextChanged="txtCrateQty_TextChanged" TabIndex="14" MaxLength="8" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtBoxqty" runat="server" AutoPostBack="true" placeholder="0" onkeypress="CheckNumeric(event)" Width="98%" OnTextChanged="txtBoxqty_TextChanged" TabIndex="14" MaxLength="8" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtPCSQty" Enabled="false" runat="server" AutoPostBack="true" placeholder="0" onkeypress="CheckNumeric(event)" Width="98%" OnTextChanged="txtPCSQty_TextChanged" TabIndex="14" MaxLength="8" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtViewTotalBox" runat="server" ReadOnly="True" CssClass="textfield" Width="90%" Style="background-color: rgb(235, 235, 228)"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtViewTotalPcs" runat="server" ReadOnly="True" CssClass="textfield" Width="90%" Style="background-color: rgb(235, 235, 228)"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtQtyBox" runat="server" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" Width="90%"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtBoxPcs" runat="server" ReadOnly="True" CssClass="textfield" Width="90%" Style="background-color: rgb(235, 235, 228)"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtLtr" runat="server" Width="50px" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" /></td>
                            <td>
                                <asp:TextBox ID="txtPrice" runat="server" Width="50px" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" /></td>
                            <td>
                                <asp:TextBox ID="txtValue" runat="server" Width="50px" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" /></td>
                            <td>
                                <asp:TextBox ID="txtStockQty" runat="server" Width="50px" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" /></td>
                            <td>
                                <asp:Button ID="BtnAddItem" runat="server" CssClass="button" Height="25px" OnClick="BtnAddItem_Click" TabIndex="5" Text="+" Width="30px" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtSecDiscPer" runat="server" AutoPostBack="true" onkeypress="return isNumberKeyWithDecimal(event)" OnTextChanged="txtSecDiscPer_TextChanged" TabIndex="4" Width="50px" Style="height: 22px" />
                            </td>
                            <td >
                                <asp:TextBox ID="txtSecDiscValue" runat="server" AutoPostBack="true" onkeypress="return isNumberKeyWithDecimal(event)" OnTextChanged="txtSecDiscValue_TextChanged" TabIndex="4" Width="50px" /></td>
                            <td >
                                <asp:Button ID="btnGO" runat="server" CssClass="auto-style2" Width="30px" Font-Size="XX-Small" OnClick="btnGO_Click" TabIndex="5" Text="GO " />
                            </td>
                            <td >
                                <asp:TextBox ID="txtTDValue" runat="server" AutoPostBack="true" onkeypress="return IsNumeric(event)" TabIndex="4" Width="50px" OnTextChanged="txtTDValue_TextChanged" />
                                <asp:TextBox ID="txtHdnTDValue" runat="server"  placeholder="0" MaxLength="6" AutoPostBack="true" Visible="false" onkeypress="CheckNumeric(event)"></asp:TextBox></td>
                            </td>
                            <td >
                                <asp:Button ID="btnApply" runat="server" CssClass="ReportSearch" Font-Size="XX-Small" OnClick="btnApply_Click" TabIndex="5" Text="Apply" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <%--  </ContentTemplate>
                    </asp:UpdatePanel>--%>
                <%-- </div>--%>
                <div>
                    <asp:TextBox ID="txtEnterQty" runat="server" AutoPostBack="true" Visible="false" ReadOnly="True" onkeypress="return IsNumeric(event)" Width="0%" TabIndex="4" />
                    <asp:TextBox ID="txtQtyCrates" runat="server" ReadOnly="True" Width="1px" Style="background-color: rgb(235, 235, 228)" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="txtCrateSize" runat="server" Width="1px" Visible="false" ReadOnly="True" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                    <asp:TextBox ID="txtPack" runat="server" Width="1px" Visible="false" ReadOnly="True" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                    <asp:TextBox ID="txtMRP" runat="server" Width="1px" Visible="false" ReadOnly="True" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                </div>
                <%--  <div id="controlHead" style="margin-top: 10px; padding-right: 10px; width: 99%;"></div>--%>


                <div style='overflow-x: scroll; width: 100%; margin-top: 5px;'>

                    <asp:UpdatePanel ID="Upnel" runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="gvDetails" GridLines="Horizontal" AutoGenerateColumns="False" Width="98%" BackColor="White"
                                BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True"
                                OnRowDataBound="gvDetails_RowDataBound" ShowFooter="True">
                                <AlternatingRowStyle BackColor="#CCFFCC" />
                                <Columns>

                                    <%--0--%>
                                    <asp:TemplateField Visible="false" ItemStyle-Width="0px">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="HiddenField2" Visible="false" runat="server" Value='<%# Eval("Product_Code") %>' />
                                            <asp:HiddenField ID="hdnBasePrice" Visible="false" runat="server" Value='<%# Eval("BasePrice","{0:n2}") %>' />
                                            <asp:HiddenField ID="hdnTaxableAmount" Visible="false" runat="server" Value='<%# Eval("TaxableAmount","{0:n2}") %>' />
                                            <asp:HiddenField ID="hdfLineNo" Visible="false" runat="server" Value='<%# Eval("Line_No") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="0px"></ItemStyle>
                                    </asp:TemplateField>
                                    <%--1--%>
                                    <asp:TemplateField HeaderText="Line No" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="Line_No" runat="server" Text='<%#  Eval("Line_No") %>' Visible="false" />
                                            <span>
                                                <%#Container.DataItemIndex + 1 %>
                                            </span>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                                    </asp:TemplateField>
                                    <%--2--%>
                                    <asp:TemplateField HeaderText="Product Code/Name" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="Product" runat="server" Text='<%#  Eval("Product") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                    </asp:TemplateField>
                                    <%--3--%>
                                    <asp:TemplateField HeaderText="Pack">
                                        <ItemTemplate>
                                            <asp:Label ID="Pack" runat="server" Text='<%#  Eval("Pack","{0:n2}") %>' DataFormatString="{0:n2}" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                                    </asp:TemplateField>
                                    <%--4--%>
                                    <asp:TemplateField HeaderText="MRP">
                                        <ItemTemplate>
                                            <asp:Label ID="MRP" runat="server" Text='<%#  Eval("MRP","{0:n2}") %>' DataFormatString="{0:n2}" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                                    </asp:TemplateField>
                                    <%--5--%>
                                    <asp:TemplateField HeaderText="SO Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="SO_Qty" runat="server" Text='<%#  Eval("SO_Qty","{0:n2}") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                                    </asp:TemplateField>
                                    <%--6--%>
                                    <asp:TemplateField HeaderText="Box">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBoxQtyGrid" Width="40px" runat="server" Text='<%#  Eval("Box_Qty") %>' AutoPostBack="true" placeholder="0" onkeypress="CheckNumeric(event)" OnTextChanged="txtBoxQtyGrid_TextChanged" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                        <ItemStyle HorizontalAlign="Left" Width="40px" />
                                    </asp:TemplateField>
                                    <%--7--%>
                                    <asp:TemplateField HeaderText="Pcs">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPcsQtyGrid" Width="40px" runat="server" Text='<%#  Eval("Pcs_Qty") %>' AutoPostBack="true" placeholder="0" onkeypress="CheckNumeric(event)" OnTextChanged="txtPcsQtyGrid_TextChanged" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                        <ItemStyle HorizontalAlign="Left" Width="40px" />
                                    </asp:TemplateField>
                                    <%--8--%>
                                    <asp:TemplateField HeaderText="Total Box"><%--ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"--%>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBox" ReadOnly="true" Width="50px" runat="server" Text='<%#  Eval("Invoice_Qty") %>' AutoPostBack="true" onkeypress="CheckNumeric(event)" OnTextChanged="txtTotBoxGrid_TextChanged" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                    </asp:TemplateField>
                                    <%--9--%>
                                    <asp:TemplateField HeaderText="Box Pcs"><%--ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"--%>
                                        <ItemTemplate>
                                            <asp:Label ID="txtBoxPcs" ReadOnly="true" Width="50px" runat="server" Text='<%#  Eval("BoxPcs") %>' AutoPostBack="true" onkeypress="CheckNumeric(event)" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                    </asp:TemplateField>

                                    <%--10--%>
                                    <asp:TemplateField HeaderText="Stock Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="StockQty" runat="server" Text='<%#  Eval("StockQty","{0:n2}") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--11--%>
                                    <asp:TemplateField HeaderText="LTR">
                                        <ItemTemplate>
                                            <asp:Label ID="LTR" runat="server" Text='<%#  Eval("Ltr","{0:n2}") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--12--%>
                                    <asp:TemplateField HeaderText="Rate">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRate" Width="50px" runat="server" Text='<%#  Eval("Rate","{0:n2}") %>' ReadOnly="true" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="50px" />
                                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                                    </asp:TemplateField>
                                    <%--13--%>
                                    <asp:TemplateField HeaderText="Tax1 %">
                                        <ItemTemplate>
                                            <asp:Label ID="Tax" runat="server" Text='<%#  Eval("TaxPer","{0:n2}") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--14--%>
                                    <asp:TemplateField HeaderText="Tax1 Value">
                                        <ItemTemplate>
                                            <asp:Label ID="TaxValue" runat="server" Text='<%#  Eval("TaxValue","{0:n2}") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--15--%>
                                    <asp:TemplateField HeaderText="Tax2 %">
                                        <ItemTemplate>
                                            <asp:Label ID="AddTax" Text='<%#  Eval("AddTaxPer","{0:n2}") %>' runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--16--%>
                                    <asp:TemplateField HeaderText="Tax2 Value">
                                        <ItemTemplate>
                                            <asp:Label ID="AddTaxValue" runat="server" Text='<%#  Eval("AddTaxValue","{0:n2}") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--17--%>
                                    <asp:TemplateField HeaderText="Disc %">
                                        <ItemTemplate>
                                            <asp:Label ID="Disc" runat="server" Text='<%#  Eval("Disc","{0:n2}") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--18--%>
                                    <asp:TemplateField HeaderText="Disc Value">
                                        <ItemTemplate>
                                            <asp:Label ID="DiscValue" runat="server" Text='<%#  Eval("DiscVal","{0:n2}") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--19--%>
                                    <asp:TemplateField HeaderText="SecDisc %">
                                        <ItemTemplate>
                                            <asp:TextBox ID="SecDisc" runat="server" Width="60px" AutoPostBack="true" Text='<%#  Eval("SecDiscPer","{0:n2}") %>' onkeypress="return isNumberKeyWithDecimal(event)" OnTextChanged="SecDisc_TextChanged" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--20--%>
                                    <asp:TemplateField HeaderText="SecDisc Value">
                                        <ItemTemplate>
                                            <asp:TextBox ID="SecDiscValue" runat="server" Width="60px" AutoPostBack="true" Text='<%#  Eval("SecDiscAmount","{0:n2}") %>' onkeypress="return isNumberKeyWithDecimal(event)" OnTextChanged="SecDiscValue_TextChanged" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--21--%>
                                    <asp:TemplateField HeaderText="Amount" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="Amount" runat="server" Text='<%#  Eval("Amount","{0:n2}") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--22--%>
                                    <asp:TemplateField HeaderText="Scheme" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblScheme" runat="server" Text='<%#  Eval("SchemeCode") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--23--%>
                                    <asp:TemplateField Visible="false" ItemStyle-Width="0px">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hDiscType" Visible="false" runat="server" Value='<%# Eval("DiscType") %>' />
                                            <asp:HiddenField ID="hDiscCalculationType" Visible="false" runat="server" Value='<%# Eval("DiscCalculationBase") %>' />
                                            <asp:HiddenField ID="hClaimDiscAmt" Visible="false" runat="server" Value='<%# Eval("ClaimDiscAmt") %>' />
                                            <asp:HiddenField ID="hTax1" Visible="false" runat="server" Value='<%# Eval("TAX1") %>' />
                                            <asp:HiddenField ID="hTax2" Visible="false" runat="server" Value='<%# Eval("TAX2") %>' />
                                            <asp:HiddenField ID="hTax1component" Visible="false" runat="server" Value='<%# Eval("TAX1COMPONENT") %>' />
                                            <asp:HiddenField ID="hTax2component" Visible="false" runat="server" Value='<%# Eval("TAX2COMPONENT") %>' />

                                        </ItemTemplate>
                                        <ItemStyle Width="0px"></ItemStyle>
                                    </asp:TemplateField>
                                    <%--24--%>
                                    <asp:TemplateField HeaderText="TD">
                                        <ItemTemplate>
                                            <asp:Label ID="TD" runat="server" Text='<%#  Eval("TD","{0:n2}") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--25--%>
                                    <asp:TemplateField HeaderText="PE">
                                        <ItemTemplate>
                                            <asp:Label ID="PE" runat="server" Text='<%#  Eval("PE","{0:n2}") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="50px" />
                                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                                    </asp:TemplateField>
                                    <%--26--%>
                                    <%--<asp:TemplateField HeaderText="Before Tax">
                                        <ItemTemplate>
                                            <asp:Label ID="ToatlBeforeTax" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="50px" />
                                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                                    </asp:TemplateField>--%>
                                    <%--27--%>
                                    <%--<asp:TemplateField HeaderText="Tax After PE">
                                        <ItemTemplate>
                                            <asp:Label ID="VatAfterPE" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="50px" />
                                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                                    </asp:TemplateField>--%>
                                    <%--28--%>
                                    <asp:TemplateField HeaderText="Total">
                                        <ItemTemplate>
                                            <asp:Label ID="Total" runat="server" Text='<%#  Eval("Total","{0:n2}") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="50px" />
                                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                                    </asp:TemplateField>
                                    <%--29--%>
                                    <asp:BoundField HeaderText="Disc. On" DataField="CALCULATIONON">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="HSNCODE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHSNCODE" runat="server" Text='<%#  Eval("HSNCODE") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Tax1">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTAXCOMPONENT" runat="server" Text='<%#  Eval("TAXCOMPONENT") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Tax2">
                                        <ItemTemplate>
                                            <asp:Label ID="lblADDTAXCOMPONENT" runat="server" Text='<%#  Eval("ADDTAXCOMPONENT") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Scheme%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSchemeDiscPer" runat="server" Text='<%#  Eval("SchemeDisc","{0:n2}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Scheme Disc">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSchemeDiscVal" runat="server" Text='<%#  Eval("SchemeDiscVal","{0:n2}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Add SchPer" DataField="ADDSCHDISCPER" DataFormatString="{0:n2}">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Add SchVal" DataField="ADDSCHDISCVAL" DataFormatString="{0:n2}">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Add SchAmt" DataField="ADDSCHDISCAMT" DataFormatString="{0:n2}">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
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
                <%--</div>--%>
            </div>
            <br />
            <%-- <div id="Div1" style="margin-top: 10px; margin-left: 1px; padding-right: 10px; width: 100%; text-align: center"></div>--%>
            <div class="polaroid" style="width: 99%; text-align: center; table-layout: fixed;">
                <div style="background-color: #C0C0C0; border-style: solid; border-width: thin; width: 100%; text-align: center"><strong>Scheme Detail</strong></div>

                <div style='overflow-x: scroll; width: 100%;'>
                    <asp:UpdatePanel ID="upnel2" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvScheme" runat="server" AutoGenerateColumns="False" GridLines="Horizontal" Width="100%" BackColor="White"
                                BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" CellPadding="3">
                                <AlternatingRowStyle BackColor="#CCFFCC" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>

                                            <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" Checked='<%# Convert.ToBoolean(Eval("ChkStatus")) %>' OnCheckedChanged="chkSelect_CheckedChanged" />

                                            <asp:HiddenField ID="hdnSchemeLine" Visible="false" runat="server" Value='<%# Eval("SchemeLineNo") %>' />
                                            <asp:HiddenField ID="hdnSchemeType" Visible="false" runat="server" Value='<%# Eval("Schemetype") %>' />
                                            <asp:HiddenField ID="hdnSchSrlNo" Visible="false" runat="server" Value='<%# Eval("SRNO") %>' />
                                            <asp:HiddenField ID="hdnAddSchType" Visible="false" runat="server" Value='<%# Eval("ADDITIONDISCOUNTITEMTYPE") %>' />
                                            <asp:HiddenField ID="hdntotSchemeValueoff" Visible="false" runat="server" Value='<%# Eval("TotalSchemeValueoff") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Scheme Code" DataField="SCHEMECODE"></asp:BoundField>
                                    <asp:BoundField HeaderText="Scheme Name" DataField="Scheme Description"></asp:BoundField>
                                    <asp:BoundField HeaderText="Item Group Name" DataField="Item Group Name"></asp:BoundField>
                                    <asp:BoundField HeaderText="Free Item Code" DataField="Free Item Code"></asp:BoundField>
                                    <asp:BoundField HeaderText="Free Item Name" DataField="Free Item Name"></asp:BoundField>
                                    <asp:BoundField HeaderText="Slab" DataField="FREEQTY"></asp:BoundField>
                                    <asp:BoundField HeaderText="Set" DataField="SetNo"></asp:BoundField>

                                    <asp:BoundField HeaderText="FreeQty" DataField="TotalFreeQty"></asp:BoundField>
                                    <asp:TemplateField Visible="false" ItemStyle-Width="0px">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnTotalFreeQty" Visible="false" runat="server" Value='<%# Eval("TotalFreeQty") %>' />
                                            <asp:HiddenField ID="hdnTotalFreeQtyPcs" Visible="false" runat="server" Value='<%# Eval("TotalFreeQtyPcs") %>' />
                                            <asp:HiddenField ID="hScTax1" Visible="false" runat="server" Value='<%# Eval("TAX1") %>' />
                                            <asp:HiddenField ID="hScTax2" Visible="false" runat="server" Value='<%# Eval("TAX2") %>' />
                                            <asp:HiddenField ID="hScTax1component" Visible="false" runat="server" Value='<%# Eval("TAX1COMPONENT") %>' />
                                            <asp:HiddenField ID="hScTax2component" Visible="false" runat="server" Value='<%# Eval("TAX2COMPONENT") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="0px"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQty" runat="server" AutoPostBack="True" onkeypress="return IsNumeric(event)" OnTextChanged="txtQty_TextChanged" ReadOnly="True" Width="40px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="FreePcs" DataField="TotalFreeQtyPcs"></asp:BoundField>
                                    <asp:TemplateField HeaderText="PcsQty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQtyPcs" runat="server" AutoPostBack="True" onkeypress="return IsNumeric(event)" OnTextChanged="txtQtyPcs_TextChanged" ReadOnly="True" Width="40px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Scheme% /Val">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSchemeDiscPer" runat="server" Text='<%# Eval("SCHEME_PER_VAL","{0:n2}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Pack Size" DataField="Product_PackSize" DataFormatString="{0:0.00}"></asp:BoundField>
                                    <asp:BoundField HeaderText="ConvBox"></asp:BoundField>
                                    <asp:BoundField HeaderText="Rate" DataField="Rate" DataFormatString="{0:0.00}"></asp:BoundField>
                                    <asp:BoundField HeaderText="BasicAmt"></asp:BoundField>
                                    <asp:BoundField HeaderText="DiscPer"></asp:BoundField>
                                    <asp:BoundField HeaderText="DiscVal"></asp:BoundField>
                                    <asp:BoundField HeaderText="Taxable Amount"></asp:BoundField>
                                    <asp:BoundField HeaderText="Tax1" DataField="Tax1Per" DataFormatString="{0:0.00}"></asp:BoundField>
                                    <asp:BoundField HeaderText="Tax1 Amt"></asp:BoundField>
                                    <asp:BoundField HeaderText="Tax2" DataField="Tax2Per" DataFormatString="{0:0.00}"></asp:BoundField>
                                    <asp:BoundField HeaderText="Tax2 Amt"></asp:BoundField>
                                    <asp:BoundField HeaderText="Amount"></asp:BoundField>
                                    <asp:BoundField HeaderText="HSNCODE" DataField="HSNCODE"></asp:BoundField>
                                    <asp:BoundField HeaderText="Tax1 Comp" DataField="Tax1Comp"></asp:BoundField>
                                    <asp:BoundField HeaderText="Tax2 Comp" DataField="Tax2Comp"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Add SCH Group">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddSchemeGroup" runat="server" Text='<%# Eval("ADDITIONDISCOUNTITEMGROUP") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Add SCH Group Desc">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddSchemeGroupDesc" runat="server" Text='<%# Eval("ADDITIONDISCOUNTITEMGROUPDESC") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Add SCH Disc%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddSchemePer" runat="server" Text='<%# Eval("ADDITIONDISCOUNTPERCENT","{0:n2}") %>' DataFormatString="{0:n2}" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Add SCH Disc Val Off (Per Box)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddSchemeVal" runat="server" Text='<%# Eval("ADDITIONDISCOUNTVALUEOFF","{0:n2}") %>' DataFormatString="{0:n2}" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Achievement">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAchievement" runat="server" Text='<%# Eval("SCHBOX","{0:n2}") %>' DataFormatString="{0:n2}" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#003366" ForeColor="White" />
                            </asp:GridView>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <table style="table-layout: fixed; width: 98%;">
                <tr>
                    <td style="padding: 10px" class="auto-style1">
                        <asp:Button ID="btnScheme" runat="server" Text="Scheme Details" CssClass="button" Height="31px" Width="140px" Visible="False" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

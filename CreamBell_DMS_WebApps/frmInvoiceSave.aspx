<%@ Page Title="Invoice Save" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmInvoiceSave.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmInvoiceSave" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
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
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">


    <asp:UpdatePanel runat="server" ID="UpdatePanel1sds">
        <ContentTemplate>

            <div style="width: 98%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
                <span style="font-family: Segoe UI; color: white; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px;">Invoice Save </span>
            </div>

            <table style="width: 100%; text-align: left">
                <tr>
                    <td style="width: 5%">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return confirm('Are you sure to save record?');" CssClass="ReportSearch" OnClick="btnSave_Click" /></td>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave"
                                    EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <td style="width: 10%">
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ReportSearch" OnClick="btnBack_Click" /></td>
                        <td style="width: 85%">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="#CC0000"></asp:Label></td>
                </tr>
            </table>

            <div style="width: 100%; text-align: left">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 8%;">Customer Group</td>
                        <td style="width: 16%;">
                            <asp:DropDownList ID="drpCustomerGroup" runat="server" Width="90%" OnSelectedIndexChanged="drpCustomerGroup_SelectedIndexChanged"></asp:DropDownList></td>
                        <td style="width: 8%;">SO Number</td>
                        <td style="width: 16%;">
                            <asp:DropDownList ID="drpSONumber" runat="server" Width="90%"></asp:DropDownList></td>
                        <td style="width: 8%;">Transporter Name</td>
                        <td style="width: 15%;">
                            <asp:TextBox ID="txtTransporterName" runat="server" Width="90%" OnTextChanged="txtTransporterName_TextChanged"></asp:TextBox></td>
                        <td style="width: 8%;">Remark</td>
                        <td style="width: 13%;" rowspan="3">
                            <asp:TextBox ID="txtRemark" runat="server" Width="90%" TextMode="MultiLine" Height="40px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 8%;">Customer Code-Name</td>
                        <td style="width: 16%;">
                            <asp:DropDownList ID="drpCustomerCode" runat="server" Width="90%"></asp:DropDownList></td>
                        <td style="width: 8%;">SO Date</td>
                        <td style="width: 16%;">
                            <asp:TextBox ID="txtSODate" runat="server" Width="90%" ReadOnly="True"></asp:TextBox></td>
                        <td style="width: 8%;">Driver Name</td>
                        <td style="width: 15%;">
                            <asp:TextBox ID="txtDriverName" runat="server" Width="90%" OnTextChanged="txtDriverName_TextChanged"></asp:TextBox></td>
                        <td>
                            <asp:Label ID="lblsite" runat="server" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 8%;">TIN/VAT</td>
                        <td style="width: 16%;">
                            <asp:TextBox ID="txtTIN" runat="server" Width="90%" ReadOnly="true"></asp:TextBox></td>
                        <td style="width: 8%;">Loadsheet Number</td>
                        <td style="width: 16%;">
                            <asp:TextBox ID="txtLoadSheetNumber" runat="server" Width="90%" ReadOnly="true"></asp:TextBox></td>
                        <td style="width: 8%;">Driver Contact No</td>
                        <td style="width: 15%;">
                            <asp:TextBox ID="txtDriverContact" runat="server" Width="90%" OnTextChanged="txtDriverContact_TextChanged"></asp:TextBox></td>
                        <td></td>

                    </tr>
                    <tr>
                        <td style="width: 8%;">Bill To Address</td>
                        <td style="width: 16%;">
                            <asp:TextBox ID="txtAddress" runat="server" Width="90%" ReadOnly="true"></asp:TextBox></td>
                        <td style="width: 8%;">Loadsheet Date</td>
                        <td style="width: 16%;">
                            <asp:TextBox ID="txtLoadsheetDate" runat="server" Width="90%" ReadOnly="true"></asp:TextBox></td>
                        <td style="width: 8%;">Vehicle No</td>
                        <td style="width: 15%;">
                            <asp:TextBox ID="txtVehicleNo" runat="server" Width="90%"></asp:TextBox></td>
                        <td style="width: 5%;">Mobile No</td>

                        <td>
                            <asp:TextBox ID="txtMobileNO" runat="server" ReadOnly="true" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 8%;">Bill To State</td>
                        <td style="width: 16%;">

                            <asp:TextBox ID="txtBillToState" runat="server" ReadOnly="true" Width="90%"></asp:TextBox>

                            <td style="width: 8%;">Invoice Number</td>
                            <td style="width: 16%;">
                                <asp:TextBox ID="txtInvoiceNo" runat="server" Width="90%" ReadOnly="true"></asp:TextBox></td>
                            <td style="width: 8%;">Invoice Date</td>
                            <td style="width: 15%;">
                                <asp:TextBox ID="txtInvoiceDate" runat="server" Width="90%" ReadOnly="true"></asp:TextBox></td>
                            <td style="width: 5%;">CompositonScheme:</td>
                        <td aria-readonly="true">
                            <asp:CheckBox ID="chkCompositionScheme" runat="server" ReadOnly="true" Enabled="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%;">Shipped To Address</td>
                        <td style="width: 16%;">
                            <asp:DropDownList ID="ddlShipToAddress" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlShipToAddress_SelectedIndexChanged" CssClass="dropdownField" Width="90%"></asp:DropDownList></td>
                        <td style="width: 10%;">GST TIN No:</td>
                        <td style="width: 16%;">
                            <asp:TextBox ID="txtGSTtin" runat="server" Width="90%" ReadOnly="true" />
                        </td>
                        <td style="width: 8%;">GSTTIN Reg.Date :</td>
                        <td style="width: 15%;">
                            <asp:TextBox ID="txtGSTtinRegistration" runat="server" Width="90%" ReadOnly="true" />
                        </td>
                        <td style="width: 5%;">Invoice Value:</td>
                        <td aria-readonly="true">
                            <asp:TextBox ID="txtinvoicevalue" runat="server" Text="" Width="90%" Enabled="false" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>


                </table>

                <div id="panelAdd" style="margin-top: 5px; width: 100%;">

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnAddItem" />
                            <asp:PostBackTrigger ControlID="DDLMaterialCode" />
                            <asp:PostBackTrigger ControlID="DDLProductGroup" />
                            <asp:PostBackTrigger ControlID="DDLProductSubCategory" />
                        </Triggers>

                        <ContentTemplate>
                            <asp:Panel ID="panelAddLine" runat="server" Width="100%">
                                <table style="border-spacing: 4px; width: 100%;">
                                    <tr>
                                        <td>Product Group</td>
                                        <td>Product Sub Category</td>
                                        <td>Product<asp:Label ID="lblHidden" runat="server" Visible="False"></asp:Label></td>
                                        <td>CrateQty</td>
                                        <td>BoxQty</td>
                                        <td>PCSQty</td>
                                        <td>TotalBox</td>
                                        <td>
                                            <asp:Label ID="lblTotalPcs" runat="server" Text="TotalPcs"></asp:Label></td>
                                        <td>Total QtyConv</td>
                                        <td>BOXPCS</td>
                                        <td>Qty[Ltr]</td>
                                        <td>Price</td>
                                        <td>Value</td>
                                        <td>StockQty</td>
                                        <td></td>
                                        <td>S.Dics%</td>
                                        <td>S.DicsValue</td>
                                        <td></td>
                                        <td>TD Value</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 7%">
                                            <asp:DropDownList ID="DDLProductGroup" runat="server" Width="98%" AutoPostBack="true" OnSelectedIndexChanged="ddlProductGroup_SelectedIndexChanged" TabIndex="1"></asp:DropDownList></td>
                                        <td style="width: 10%">
                                            <asp:DropDownList ID="DDLProductSubCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLProductSubCategory_SelectedIndexChanged" Width="98%" TabIndex="2"></asp:DropDownList></td>
                                        <td style="width: 25%">
                                            <asp:DropDownList ID="DDLMaterialCode" runat="server" Width="98%" OnSelectedIndexChanged="DDLMaterialCode_SelectedIndexChanged" AutoPostBack="true" /></td>
                                        <td style="width: 5%">
                                            <asp:DropDownList ID="ddlEntryType" runat="server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlProductGroup_SelectedIndexChanged" TabIndex="1" Width="1px">
                                                <asp:ListItem Value="Box">BOX</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtCrateQty" runat="server" AutoPostBack="true" placeholder="0" onkeypress="CheckNumeric(event)" Width="98%" OnTextChanged="txtCrateQty_TextChanged" TabIndex="14" MaxLength="8" />
                                        </td>
                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtBoxqty" runat="server" AutoPostBack="true" placeholder="0" onkeypress="CheckNumeric(event)" Width="98%" OnTextChanged="txtBoxqty_TextChanged" TabIndex="14" MaxLength="8" /></td>
                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtPCSQty" Enabled="false" runat="server" AutoPostBack="true" placeholder="0" onkeypress="CheckNumeric(event)" Width="98%" OnTextChanged="txtPCSQty_TextChanged" TabIndex="14" MaxLength="8" /></td>
                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtViewTotalBox" runat="server" ReadOnly="True" CssClass="textfield" Width="90%" Style="background-color: rgb(235, 235, 228)"></asp:TextBox></td>
                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtViewTotalPcs" runat="server" ReadOnly="True" CssClass="textfield" Width="90%" Style="background-color: rgb(235, 235, 228)"></asp:TextBox></td>
                                        <%--<td style="width: 0%"></td>--%>
                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtQtyBox" runat="server" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" Width="90%"></asp:TextBox></td>
                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtBoxPcs" runat="server" ReadOnly="True" CssClass="textfield" Width="90%" Style="background-color: rgb(235, 235, 228)"></asp:TextBox></td>
                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtLtr" runat="server" Width="50px" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" OnTextChanged="txtLtr_TextChanged" /></td>
                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtPrice" runat="server" Width="50px" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" /></td>
                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtValue" runat="server" Width="50px" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" /></td>
                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtStockQty" runat="server" Width="50px" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" /></td>
                                        <td style="width: 5%">
                                            <asp:Button ID="BtnAddItem" runat="server" CssClass="button" Height="25px" OnClick="BtnAddItem_Click" TabIndex="5" Text="+" Width="30px" />
                                            <td style="width: 5%">
                                                <asp:TextBox ID="txtSecDiscPer" runat="server" AutoPostBack="true" onkeypress="return isNumberKeyWithDecimal(event)" OnTextChanged="txtSecDiscPer_TextChanged" TabIndex="4" Width="50px" />
                                                <td style="width: 5%">
                                                    <asp:TextBox ID="txtSecDiscValue" runat="server" AutoPostBack="true" onkeypress="return isNumberKeyWithDecimal(event)" OnTextChanged="txtSecDiscValue_TextChanged" TabIndex="4" Width="50px" /></td>
                                                <td style="width: 5%">
                                                    <asp:Button ID="btnGO" runat="server" CssClass="ReportSearch" Font-Size="XX-Small" OnClick="btnGO_Click" TabIndex="5" Text="GO" /></td>
                                                <td style="width: 5%">
                                                    <asp:TextBox ID="txtTDValue" runat="server" AutoPostBack="true" onkeypress="return IsNumeric(event)" OnTextChanged="txtTDValue_TextChanged" TabIndex="4" Width="50px" /></td>
                                                <td style="width: 5%">
                                                    <asp:Button ID="btnApply" runat="server" CssClass="ReportSearch" Font-Size="XX-Small" OnClick="btnApply_Click" TabIndex="5" Text="Apply" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div>
                    <asp:TextBox ID="txtEnterQty" runat="server" AutoPostBack="true" Visible="false" ReadOnly="True" onkeypress="return IsNumeric(event)" Width="0%" OnTextChanged="txtQtyBox_TextChanged" TabIndex="4" />
                    <asp:TextBox ID="txtQtyCrates" runat="server" ReadOnly="True" Width="1px" Style="background-color: rgb(235, 235, 228)" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="txtCrateSize" runat="server" Width="1px" Visible="false" ReadOnly="True" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                    <asp:TextBox ID="txtPack" runat="server" Width="1px" Visible="false" ReadOnly="True" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                    <asp:TextBox ID="txtMRP" runat="server" Width="1px" Visible="false" ReadOnly="True" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                </div>

                <div id="controlHead" style="margin-top: 10px; padding-right: 10px; width: 99%;"></div>
                <div style="overflow: auto; margin-top: 10px; padding-right: 10px; width: 100%;">

                    <asp:UpdatePanel ID="Upnel" runat="server">
                        <ContentTemplate>

                            <asp:GridView runat="server" ID="gvDetails" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True"
                                OnRowCancelingEdit="gvDetails_RowCancelingEdit" OnRowEditing="gvDetails_RowEditing"
                                OnRowUpdating="gvDetails_RowUpdating" OnRowDeleting="gvDetails_RowDeleting"
                                OnSelectedIndexChanged="gvDetails_SelectedIndexChanged" OnRowDataBound="gvDetails_RowDataBound" OnRowCommand="gvDetails_RowCommand" ShowFooter="True">
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
                                            <asp:Label ID="Pack" runat="server" Text='<%#  Eval("Pack") %>' DataFormatString="{0:n2}" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                                    </asp:TemplateField>
                                    <%--4--%>
                                    <asp:TemplateField HeaderText="MRP">
                                        <ItemTemplate>
                                            <asp:Label ID="MRP" runat="server" Text='<%#  Eval("MRP") %>' DataFormatString="{0:n2}" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                                    </asp:TemplateField>
                                    <%--5--%>
                                    <asp:TemplateField HeaderText="SO Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="SO_Qty" runat="server" Text='<%#  Eval("SO_Qty") %>' />
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
                                            <asp:TextBox ID="txtBox" ReadOnly="true" Width="50px" runat="server" Text='<%#  Eval("Invoice_Qty") %>' AutoPostBack="true" onkeypress="CheckNumeric(event)" OnTextChanged="txtBox_TextChanged" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                    </asp:TemplateField>
                                    <%--9--%>
                                    <asp:TemplateField HeaderText="BoxPcs"><%--ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"--%>
                                        <ItemTemplate>
                                            <asp:Label ID="txtBoxPcs" ReadOnly="true" Width="50px" runat="server" Text='<%#  Eval("BoxPcs") %>' AutoPostBack="true" onkeypress="CheckNumeric(event)" OnTextChanged="txtBox_TextChanged" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                    </asp:TemplateField>

                                    <%--10--%>
                                    <asp:TemplateField HeaderText="Stock Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="StockQty" runat="server" Text='<%#  Eval("StockQty") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--11--%>
                                    <asp:TemplateField HeaderText="LTR">
                                        <ItemTemplate>
                                            <asp:Label ID="LTR" runat="server" Text='<%#  Eval("Ltr") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--12--%>
                                    <asp:TemplateField HeaderText="Rate">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRate" Width="50px" runat="server" OnTextChanged="txtRate_TextChanged" Text='<%#  Eval("Rate","{0:n2}") %>' ReadOnly="true" />
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
                                    <asp:TemplateField HeaderText="Tax1 Value" >
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
                                            <asp:Label ID="AddTaxValue" runat="server"  Text='<%#  Eval("AddTaxValue","{0:n2}") %>'  />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--17--%>
                                    <asp:TemplateField HeaderText="Disc %">
                                        <ItemTemplate>
                                            <asp:Label ID="Disc" runat="server" Text='<%#  Eval("Disc") %>' />
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
                                            <asp:Label ID="TD" runat="server" Text='<%#  Eval("TD") %>'/>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--25--%>
                                    <asp:TemplateField HeaderText="PE">
                                        <ItemTemplate>
                                            <asp:Label ID="PE" runat="server" Text='<%#  Eval("PE") %>'/>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="50px" />
                                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                                    </asp:TemplateField>
                                    <%--26--%>
                                    <asp:TemplateField HeaderText="Before Tax">
                                        <ItemTemplate>
                                            <asp:Label ID="ToatlBeforeTax" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="50px" />
                                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                                    </asp:TemplateField>
                                    <%--27--%>
                                    <asp:TemplateField HeaderText="Tax After PE">
                                        <ItemTemplate>
                                            <asp:Label ID="VatAfterPE" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="50px" />
                                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                                    </asp:TemplateField>
                                    <%--28--%>
                                    <asp:TemplateField HeaderText="Total">
                                        <ItemTemplate>
                                            <asp:Label ID="Total" runat="server" Text='<%#  Eval("Total","{0:n2}") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="50px" />
                                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                                    </asp:TemplateField>
                                    <%--29--%>
                                    <asp:BoundField HeaderText="Calc.On" DataField="CALCULATIONON">
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

                                    <asp:TemplateField HeaderText="SchemePer">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSchemeDiscPer" runat="server" Text='<%#  Eval("SchemeDisc","{0:n2}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="SchemeDisc">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSchemeDiscVal" runat="server" Text='<%#  Eval("SchemeDiscVal","{0:n2}") %>' />
                                        </ItemTemplate>
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
                    <%--</div>--%>
                </div>

            </div>

            <div id="Div1" style="margin-top: 10px; margin-left: 1px; padding-right: 10px; width: 100%; text-align: center"></div>
            <div class="polaroid" style="width: 90%; text-align: center">
                <div style="background-color: #C0C0C0; border-style: solid; border-width: thin; width: 100%; text-align: center"><strong>Scheme Detail</strong></div>

                <asp:GridView ID="gvScheme" runat="server" AutoGenerateColumns="False" GridLines="Horizontal" Width="100%" BackColor="White"
                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" CellPadding="3">
                    <AlternatingRowStyle BackColor="#CCFFCC" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelect_CheckedChanged" />
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

                        <asp:BoundField HeaderText="PackSize" DataField="Product_PackSize" DataFormatString="{0:0.00}"></asp:BoundField>
                        <asp:BoundField HeaderText="ConvBox"></asp:BoundField>
                        <asp:BoundField HeaderText="Rate" DataField="Rate" DataFormatString="{0:0.00}"></asp:BoundField>
                        <asp:BoundField HeaderText="BasicAmt"></asp:BoundField>
                        <asp:BoundField HeaderText="DiscPer"></asp:BoundField>
                        <asp:BoundField HeaderText="DiscVal"></asp:BoundField>
                        <asp:BoundField HeaderText="TaxableAmt"></asp:BoundField>
                        <asp:BoundField HeaderText="Tax1" DataField="Tax1Per" DataFormatString="{0:0.00}"></asp:BoundField>
                        <asp:BoundField HeaderText="Tax1Amt"></asp:BoundField>
                        <asp:BoundField HeaderText="Tax2" DataField="Tax2Per" DataFormatString="{0:0.00}"></asp:BoundField>
                        <asp:BoundField HeaderText="Tax2Amt"></asp:BoundField>
                        <asp:BoundField HeaderText="Amount"></asp:BoundField>
                        <asp:BoundField HeaderText="HSNCODE" DataField="HSNCODE"></asp:BoundField>
                        <asp:BoundField HeaderText="Tax1Comp" DataField="Tax1Comp"></asp:BoundField>
                        <asp:BoundField HeaderText="Tax2Comp" DataField="Tax2Comp"></asp:BoundField>
                    </Columns>
                    <HeaderStyle BackColor="#003366" ForeColor="White" />
                </asp:GridView>


            </div>
            <table>
                <tr>
                    <td style="padding: 10px" class="auto-style1">
                        <asp:Button ID="btnScheme" runat="server" Text="Scheme Details" CssClass="button" Height="31px" Width="140px" Visible="False" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

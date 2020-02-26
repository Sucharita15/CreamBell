<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.Master" CodeBehind="frmSDSaleOrder.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmSDSaleOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />


    <script type="text/javascript">

        function uploadStart(sender, args) {
            var fileName = args.get_fileName();
            var fileExt = fileName.substring(fileName.lastIndexOf(".") + 1);

            if (fileExt == "xls" || fileExt == "xlsx") {
                return true;
            } else {
                //To cancel the upload, throw an error, it will fire OnClientUploadError
                var err = new Error();
                err.name = "Upload Error";
                err.message = "Please upload only Excel files (.xls,.xlsx)";
                throw (err);

                return false;
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
        .ModalPoupBackgroundCssClass {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-width: 2px;
            border-style: inset;
            width: auto;
            height: auto;
        }

        .hiddencol {
            display: none;
        }
    </style>

    <style type="text/css">
        .pnlCSS {
            font-weight: bold;
            cursor: pointer;
            /*border: solid 1px #c0c0c0;*/
            margin-left: 20px;
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


        //
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        function IsNumeric(e) {

            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
            //document.getElementById("lblError") = ret ? "none" : "inline";
            if (keyCode == 8 || keyCode == 46 || keyCode == 189 || keyCode == 61 || keyCode == 45)
            { ret = true; }
            return ret;
        }

        $(document).ready(function () {
            $("select").searchable();
        });

    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #0085ca; padding: 2px 0px 0px 0px;">
                <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px; color: #FFFFFF;">SD Manual Sale Order</span>
            </div>

            <table style="width: 100%">
                <tr>
                    <td style="text-align: left; width: 7%">
                        <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="button" Height="31px" Width="70px" OnClick="BtnSave_Click" /></td>
                    <td style="text-align: left; width: 13%">
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="button" Height="31px" Width="70px" OnClick="btnBack_Click" /></td>
                    <td style="width: 80%">
                        <asp:Label ID="LblMessage1" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Names="Segoe UI" Font-Italic="true"></asp:Label></td>
                </tr>
            </table>
            <div style="margin-left: 0px; width: 100%;">
                <table style="width: 100%" cellpadding="1" cellspacing="0">
                    <tr>
                        <td style="width: 8%;">Customer Group</td>
                        <td style="width: 17%;">
                            <asp:DropDownList ID="ddlCustomerGroup" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCustomerGroup_SelectedIndexChanged" Width="95%"></asp:DropDownList></td>
                        <td style="width: 8%;">
                            <asp:Label ID="lblPSRName" runat="server" Text="PSR Name" Visible="False"></asp:Label></td>
                        <td style="width: 17%;">
                            <asp:DropDownList ID="ddlPSRName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPSRName_SelectedIndexChanged" Width="95%" Visible="False" TabIndex="1"></asp:DropDownList></td>
                        <td style="width: 10%;">Contact</td>
                        <td style="width: 17%;">
                            <asp:TextBox ID="txtContactNo" runat="server" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" Width="95%" /></td>
                    </tr>
                    <tr>
                        <td style="width: 8%;">Customer Name</td>
                        <td style="width: 17%;">
                            <asp:DropDownList ID="ddlCustomer" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" Width="95%" TabIndex="1"></asp:DropDownList></td>
                        <td style="width: 8%;">GST TIN No:</td>
                        <td style="width: 17%;">
                            <asp:TextBox ID="txtGSTtin" runat="server" CssClass="textfield" Height="13px" ReadOnly="true" Width="95%" />
                        </td>

                        <td style="width: 10%;">GST TIN Reg. Date :</td>
                        <td style="width: 17%;">
                            <asp:TextBox ID="txtGSTtinRegistration" runat="server" Width="95%" CssClass="textfield" Height="13px" ReadOnly="true" />
                        </td>
                        </td>
                                            <td style="width: 8%;">
                                                <asp:Label ID="lblBeatName" runat="server" Text="Beat Name" Visible="False"></asp:Label>
                                            </td>
                        <td>
                            <asp:DropDownList ID="ddlBeatName" runat="server" AutoPostBack="True" CssClass="dropdownField" OnSelectedIndexChanged="ddlBeatName_SelectedIndexChanged" Visible="False" Width="95%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 8%;">Bill To Address</td>
                        <td style="width: 17%;">
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="textfield" ReadOnly="True" TextMode="MultiLine" Width="95%" />
                        </td>
                        <td style="width: 8%;">Ship to Address</td>
                        <td style="width: 17%;">
                            <asp:DropDownList ID="ddlShipToAddress" runat="server" AutoPostBack="True" CssClass="dropdownField" Width="95%"></asp:DropDownList>
                        </td>
                        <td>Compositon Scheme :</td>
                        <td>
                            <asp:CheckBox ID="chkCompositionScheme" runat="server" ReadOnly="true" />
                        </td>
                        <td>Delivery Date</td>
                        <td style="padding: 0px; margin: 0px">
                            <asp:TextBox ID="txtDeliveryDate" runat="server" Width="120px" Height="18px" TabIndex="3" />
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDeliveryDate" Format="dd/MMM/yyyy"></asp:CalendarExtender>
                        </td>
                        <td>Indent No</td>
                        <td>
                            <asp:TextBox ID="txtIndentNo" runat="server" Width="120px" Height="18px" TabIndex="3" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td style="width: 8%;">Bill To State</td>
                        <td style="width: 17%;">
                            <asp:TextBox ID="txtBilltoState" runat="server" CssClass="textfield" Height="13px" ReadOnly="true" Width="95%" />
                        </td>
                        <td style="width: 8%;">&nbsp;</td>
                        <td style="width: 17%;">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </div>

            <div id="panelAdd" style="margin-top: 5px; width: 100%;">

                <asp:Panel ID="panelAddLine" runat="server" Width="100%">
                    <table style="border-spacing: 4px; width: 100%;">
                        <tr>
                            <td>Product Group</td>
                            <td>Product Sub Category</td>
                            <td>Product<asp:Label ID="lblHidden" runat="server" Visible="False"></asp:Label></td>
                            <td>Crate Qty</td>
                            <td>Box Qty</td>
                            <td>PCS Qty</td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Total Box"></asp:Label></td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Total Pcs"></asp:Label></td>
                            <td>TotalQtyConv</td>
                            <td>TotalBoxPcs</td>
                            <td>Qty[Ltr]</td>
                            <td>Price</td>
                            <td>Value</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="DDLProductGroup" runat="server" Width="110px" AutoPostBack="true" OnSelectedIndexChanged="ddlProductGroup_SelectedIndexChanged" TabIndex="1"></asp:DropDownList></td>
                            <td>
                                <asp:DropDownList ID="DDLProductSubCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLProductSubCategory_SelectedIndexChanged" Width="110px" TabIndex="2"></asp:DropDownList></td>
                            <td>
                                <asp:DropDownList ID="DDLMaterialCode" runat="server" Width="230px" OnSelectedIndexChanged="DDLMaterialCode_SelectedIndexChanged" TabIndex="3" AutoPostBack="true" /></td>
                            <td style="width: 7%">
                                <asp:TextBox ID="txtCrateQty" runat="server" AutoPostBack="true" onkeypress="return IsNumeric(event)" Width="90%" OnTextChanged="TextCrateQty_TextChanged" TabIndex="14" MaxLength="8" />
                            </td>
                            <td style="width: 7%">
                                <asp:TextBox ID="txtBoxqty" runat="server" AutoPostBack="true" onkeypress="return IsNumeric(event)" Width="90%" OnTextChanged="txtBoxqty_TextChanged" TabIndex="14" MaxLength="8" /></td>
                            <td style="width: 7%">
                                <asp:TextBox ID="txtPCSQty" runat="server" Enabled="false" AutoPostBack="true" onkeypress="return IsNumeric(event)" Width="90%" OnTextChanged="txtPCSQty_TextChanged" TabIndex="14" MaxLength="8" /></td>
                            <td style="width: 7%">
                                <asp:TextBox ID="txtViewTotalBox" runat="server" ReadOnly="True" CssClass="textfield" Width="90%" Style="background-color: rgb(235, 235, 228)"></asp:TextBox></td>
                            <td style="width: 7%">
                                <asp:TextBox ID="txtViewTotalPcs" runat="server" ReadOnly="True" CssClass="textfield" Width="90%" Style="background-color: rgb(235, 235, 228)"></asp:TextBox></td>
                            <td style="width: 7%">
                                <asp:TextBox ID="txtQtyBox" runat="server" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" Width="90%" OnTextChanged="txtQtyBox_TextChanged1"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBoxPcs" runat="server" ReadOnly="True" CssClass="textfield" Width="90%" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLtr" runat="server" Width="50px" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" /></td>
                            <td>
                                <asp:TextBox ID="txtPrice" runat="server" Width="50px" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" /></td>
                            <td>
                                <asp:TextBox ID="txtValue" runat="server" Width="50px" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" /></td>
                            <td>
                                <asp:Button ID="BtnAddItem" runat="server" Text="Add" OnClick="BtnAddItem_Click" CssClass="button" Height="33px" Width="62px" TabIndex="5" /></td>

                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtQtyCrates" Visible="false" runat="server" ReadOnly="True" Width="0px" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                                <%--<asp:TextBox ID="txtEnterQty" runat="server" AutoPostBack="true" onkeypress="return IsNumeric(event)" Width="0px" OnTextChanged="txtQtyBox_TextChanged" TabIndex="4" />
                                
                                <asp:TextBox ID="txtPack" runat="server" Width="1px" Visible="false" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" />
                                <asp:TextBox ID="txtCrateSize" runat="server" Width="1px" Visible="false" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" />--%>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>

            <%--<div style="height: auto; overflow: auto; margin-top: 10px; margin-left: 5px; padding-right: 10px;">--%>
            <%--<div style="height: auto; overflow: auto; margin-top: 5px; margin-left: 0px; padding-right: 10px;">--%>
            <div style='overflow-x:scroll;width:1330px;'>  
                <asp:GridView runat="server" ID="gvDetails" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True"
                    AllowPaging="false" PageSize="20" OnRowDeleting="gvDetails_RowDeleting" OnRowEditing="gvDetails_RowEditing" ShowFooter="True">

                    <AlternatingRowStyle BackColor="#CCFFCC" />
                    <Columns>
                        <asp:TemplateField Visible="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenField2" Visible="false" runat="server" Value='<%# Eval("SNO") %>' />
                                <asp:HiddenField ID="HiddenFieldCalculation" Visible="false" runat="server" Value='<%# Eval("CalculationBase") %>' />
                                <asp:HiddenField ID="hdnBasePrice" Visible="false" runat="server" Value='<%# Eval("BasePrice") %>' />
                                <asp:HiddenField ID="hdnTaxableAmount" Visible="false" runat="server" Value='<%# Eval("TaxableAmount") %>' />

                            </ItemTemplate>
                            <ItemStyle Width="0px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SNO">
                            <ItemTemplate>
                                <span>
                                    <%#Container.DataItemIndex + 1 %>
                                </span>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Product Group" DataField="MaterialGroup">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Product" DataField="ProductCodeName">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="QtyConv" DataField="QtyBox"><%--ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"--%>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="BoxPcs" DataField="BoxPcs" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" Width="0px" />
                            <ItemStyle HorizontalAlign="Left" Width="0px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Qty Box" DataField="OnlyQtyBox" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Qty Pcs" DataField="QtyPcs" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="UOM" DataField="UOM">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Qty[Crates]" DataField="QtyCrates" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Qty[Ltr]" DataField="QtyLtr" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Price" DataField="Price" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="MRP" DataField="MRP" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Disc Type" DataField="DiscType">
                            <HeaderStyle HorizontalAlign="left" />
                            <ItemStyle HorizontalAlign="left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Disc" DataField="Disc" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Disc Value" DataField="DiscVal" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Tax1 %" DataField="Tax_Code" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" Width="50px" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Tax1 Amount" DataField="Tax_Amount" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" Width="80px" />
                            <ItemStyle HorizontalAlign="Right" Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Tax2 %" DataField="AddTax_Code" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" Width="50px" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Tax2 Amount" DataField="AddTax_Amount" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" Width="50px" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Value" DataField="Value" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CalculationOn" DataField="CALCULATIONON">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="HSN Code" DataField="HSNCODE">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Tax1Comp" DataField="TAXCOMPONENT">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Tax2Comp" DataField="ADDTAXCOMPONENT">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="SchemePer" DataField="SchemeDisc" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="SchemeDisc" DataField="SchemeDiscVal" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="AddSchPer"  DataField="ADDSCHDISCPER" DataFormatString="{0:n2}">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="AddSchVal"  DataField="ADDSCHDISCVAL" DataFormatString="{0:n2}">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="AddSchAmt"  DataField="ADDSCHDISCAMT" DataFormatString="{0:n2}">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnDel" runat="server" Text='Delete' OnClick="lnkbtnDel_Click" ForeColor="Black"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField />
                    </Columns>
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

            <%-- Starting Changes for Addition of Scheme --%>


            <%--<div class="polaroid">
                <div style="background-color: #C0C0C0; border-style: solid; border-width: thin; width: 596px;"><strong>Scheme Detail</strong></div>
                <asp:GridView ID="gvScheme" runat="server" AutoGenerateColumns="False" GridLines="Horizontal" Width="600px" BackColor="White"
                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" OnSelectedIndexChanged="gvScheme_SelectedIndexChanged" OnRowDataBound="gvScheme_RowDataBound">
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

                        <asp:BoundField HeaderText="FreeQty" DataField="TotalFreeQty"></asp:BoundField>
                        <asp:TemplateField Visible="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnTotalFreeQty" Visible="false" runat="server" Value='<%# Eval("TotalFreeQty") %>' />
                             <asp:HiddenField ID="hdnTotalFreeQtyPcs" Visible="false" runat="server" Value='<%# Eval("TotalFreeQtyPcs") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="0px">
                                </ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BoxQty">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQty" runat="server" AutoPostBack="True" onkeypress="return IsNumeric(event)" OnTextChanged="txtQty_TextChanged" ReadOnly="False" Width="40px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField HeaderText="FreePcs" DataField="TotalFreeQtyPcs"></asp:BoundField>
                         <asp:TemplateField HeaderText="PcsQty">
                                <ItemTemplate>
                                      <asp:TextBox ID="txtQtyPcs" runat="server" AutoPostBack="True" onkeypress="return IsNumeric(event)" OnTextChanged="txtQtyPcs_TextChanged" ReadOnly="False" Width="40px"></asp:TextBox>
                                         </ItemTemplate>
                     </asp:TemplateField>
                                                                      
                    </Columns>
                    <HeaderStyle BackColor="#003366" ForeColor="White" />
                </asp:GridView>
            </div>--%>

            <div style="width: 100%; margin: 0px 0px 0px 50px">

                <table style="width: 100%;" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 100%">

                            <div style="background-color: #C0C0C0; text-align: center; border-style: solid; border-width: thin; font-weight: bold; font-size: 15px; width: 100%">
                                <strong>Scheme Detail</strong>
                            </div>

                            <asp:GridView ID="gvScheme" runat="server" AutoGenerateColumns="False" GridLines="Horizontal" BackColor="White" Width="100%"
                                BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" OnSelectedIndexChanged="gvScheme_SelectedIndexChanged" OnRowDataBound="gvScheme_RowDataBound1" Style="margin-right: 0px">
                                <AlternatingRowStyle BackColor="#CCFFCC" />
                                <Columns>
                                    <%--1--%>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelect_CheckedChanged" />
                                            <asp:HiddenField ID="hdnSchemeLine" Visible="false" runat="server" Value='<%# Eval("SchemeLineNo") %>' />
                                            <asp:HiddenField ID="hdnSchemeType" Visible="false" runat="server" Value='<%# Eval("Schemetype") %>' />
                                            <asp:HiddenField ID="hdnSchSrlNo" Visible="false" runat="server" Value='<%# Eval("SRNO") %>' />
                                            <asp:HiddenField ID="hdnAddSchType" Visible="false" runat="server" Value='<%# Eval("ADDITIONDISCOUNTITEMTYPE") %>' />
                                            <asp:HiddenField ID="hdntotSchemeValueoff" Visible="false" runat="server" Value='<%# Eval("TotalSchemeValueoff") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--2--%>
                                    <asp:BoundField HeaderText="Scheme Code" DataField="SCHEMECODE"></asp:BoundField>
                                    <%--3--%>
                                    <asp:BoundField HeaderText="Scheme Name" DataField="Scheme Description"></asp:BoundField>
                                    <%--4--%>
                                    <asp:BoundField HeaderText="Item Group Name" DataField="Item Group Name"></asp:BoundField>
                                    <%--5--%>
                                    <asp:BoundField HeaderText="Free Item Code" DataField="Free Item Code"></asp:BoundField>
                                    <%--6--%>
                                    <asp:BoundField HeaderText="Free Item Name" DataField="Free Item Name"></asp:BoundField>
                                    <%--7--%>
                                    <asp:BoundField HeaderText="Slab" DataField="FREEQTY"></asp:BoundField>
                                    <%--8--%>
                                    <asp:BoundField HeaderText="Set" DataField="SetNo"></asp:BoundField>
                                    <%--9--%>
                                    <asp:BoundField HeaderText="FreeBox" DataField="TotalFreeQty"></asp:BoundField>
                                    <%--10--%>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnTotalFreeQty" Visible="false" runat="server" Value='<%# Eval("TotalFreeQty") %>' />
                                            <asp:HiddenField ID="hdnTotalFreeQtyPcs" Visible="false" runat="server" Value='<%# Eval("TotalFreeQtyPcs") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--11--%>
                                    <asp:TemplateField HeaderText="BoxQty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQty" runat="server" AutoPostBack="True" onkeypress="CheckNumeric(event)" OnTextChanged="txtQty_TextChanged" ReadOnly="True" Width="40px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--12--%>
                                    <asp:BoundField HeaderText="FreePcs" DataField="TotalFreeQtyPcs"></asp:BoundField>
                                    <%--13--%>
                                    <asp:TemplateField HeaderText="PcsQty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQtyPcs" runat="server" AutoPostBack="True" onkeypress="CheckNumeric(event)" OnTextChanged="txtQtyPcs_TextChanged" ReadOnly="True" Width="40px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Scheme%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSchemeDiscPer" runat="server" Text='<%# Eval("PERCENTSCHEME") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--14--%>
                                    <asp:BoundField HeaderText="PackSize" DataField="Product_PackSize" DataFormatString="{0:n0}"></asp:BoundField>
                                    <%--15--%>
                                    <asp:BoundField HeaderText="ConvBox"></asp:BoundField>
                                    <%--16--%>
                                    <asp:BoundField HeaderText="Rate" DataField="Rate" DataFormatString="{0:n2}"></asp:BoundField>
                                    <%--17--%>
                                    <asp:BoundField HeaderText="BasicAmt" DataFormatString="{0:n2}"></asp:BoundField>
                                    <%--18--%>
                                    <asp:BoundField HeaderText="DiscPer" DataFormatString="{0:n2}"></asp:BoundField>
                                    <%--19--%>
                                    <asp:BoundField HeaderText="DiscVal" DataFormatString="{0:n2}"></asp:BoundField>
                                    <%--20--%>
                                    <asp:BoundField HeaderText="TaxableAmt" DataFormatString="{0:n2}"></asp:BoundField>
                                    <%--21--%>
                                    <asp:BoundField HeaderText="Tax1" DataField="Tax1Per" DataFormatString="{0:n2}"></asp:BoundField>
                                    <%--22--%>
                                    <asp:BoundField HeaderText="Tax1Amt" DataFormatString="{0:n2}"></asp:BoundField>
                                    <%--23--%>
                                    <asp:BoundField HeaderText="Tax2" DataField="Tax2Per" DataFormatString="{0:n2}"></asp:BoundField>
                                    <%--24--%>
                                    <asp:BoundField HeaderText="Tax2Amt" DataFormatString="{0:n2}"></asp:BoundField>
                                    <%--25--%>
                                    <asp:BoundField HeaderText="Amount" DataFormatString="{0:n2}"></asp:BoundField>
                                    <%--26--%>
                                    <asp:BoundField HeaderText="HSNCODE" DataField="HSNCODE"></asp:BoundField>
                                    <%--27--%>
                                    <asp:BoundField HeaderText="Tax1Comp" DataField="Tax1Comp"></asp:BoundField>
                                    <%--28--%>
                                                <asp:TemplateField HeaderText="Add SCH Group">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAddSchemeGroup" runat="server" Text='<%# Eval("ADDITIONDISCOUNTITEMGROUP") %>'  DataFormatString="{0:n2}"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Add SCH Group Desc">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAddSchemeGroupDesc" runat="server" Text='<%# Eval("ADDITIONDISCOUNTITEMGROUPDESC") %>'  DataFormatString="{0:n2}"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Add SCH Disc%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAddSchemePer" runat="server" Text='<%# Eval("ADDITIONDISCOUNTPERCENT") %>'  DataFormatString="{0:n2}"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Add SCH Disc Val Off (Per Box)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAddSchemeVal" runat="server" Text='<%# Eval("ADDITIONDISCOUNTVALUEOFF") %>'  DataFormatString="{0:n2}"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                </Columns>
                                <HeaderStyle BackColor="#003366" ForeColor="White" />
                            </asp:GridView>

                        </td>
                    </tr>
                </table>
                <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
            </div>


            <%-- Ending Changes for Addition of Scheme --%>

            <div>
                <asp:Button ID="Button2" runat="Server" Text="" Style="display: none;" />
                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button2"
                    PopupControlID="Panel1" CancelControlID="Button4"
                    BackgroundCssClass="ModalPoupBackgroundCssClass" BehaviorID="ModalPopupExtender1">
                </asp:ModalPopupExtender>
                <asp:Panel ID="Panel1" runat="server" Style="display: none; background-color: silver">
                    <div align="center">
                        <span style="color: red; font-weight: 600; text-align: center">Records which are not uploaded !!</span>
                        <asp:Button ID="Button4" runat="server" CssClass="Operationbutton" data-dismiss="modal" Text="Close" aria-hidden="true"></asp:Button>
                    </div>
                    <p></p>
                    <div style="overflow-x: scroll; width: 100%; height: 200px">
                        <asp:GridView ID="gridviewRecordNotExist" runat="server" Style="width: 100%" Font-Size="Small" CssClass="Grid" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                            <AlternatingRowStyle CssClass="alt" />
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="White" CssClass="pgr" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
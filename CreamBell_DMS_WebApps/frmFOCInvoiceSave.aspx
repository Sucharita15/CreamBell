<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmFOCInvoiceSave.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmFOCInvoiceSave" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
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

        function validate()
        {
            //debugger;
            var select = document.getElementById("<%=drpCustomerGroup.ClientID%>");
            if (select.options[select.selectedIndex].value == "Select...") {
                alert('Please Select Customer Group.....!');
                select.focus();
                return false;
            }

            var select = document.getElementById("<%=drpCustomerCode.ClientID%>");
            if (select.options[select.selectedIndex].value == "Select...") {
                alert('Please Select Customer.....!');
                select.focus();
                return false;
            }

            var select = document.getElementById("<%=DDLProductGroup.ClientID%>");
            if (select.options[select.selectedIndex].value == "Select...") {
                alert('Please Select Product Group.....!');
                select.focus();
                return false;
            }

            var select = document.getElementById("<%=DDLProductSubCategory.ClientID%>"); 
            if (select.options[select.selectedIndex].value == "Select...") {
                alert('Please Select Product Sub Category.....!');
                select.focus();
                return false;
            }
        
            var select = document.getElementById("<%=DDLMaterialCode.ClientID%>"); 
            if (select.options[select.selectedIndex].value == "Select...") {
                alert('Please Select Product.....!');
                select.focus();
                return false;
            }

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
            if (keyCode == 8 || keyCode == 46 || keyCode == 189 || keyCode == 61 || keyCode == 45)
            { ret = true; }
            // $('#<%=BtnAddItem.ClientID%>').focus();        
            //document.getElementById("BtnAddItem").focus();
            return ret;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

            <div style="width: 98%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
                <span style="font-family: Segoe UI; color: white; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px;">FOC Invoice Save </span>
            </div>
         
    <asp:UpdatePanel ID="upanel" runat="server">
        <ContentTemplate>

            <table style="width: 98%; text-align: left">
                <tr>
                    <td style="width: 5%">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ReportSearch" OnClick="btnSave_Click" />
                    </td>
                    <td style="width: 10%">
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ReportSearch" OnClick="btnBack_Click" Visible="false" />
                    </td>
                    <td style="width: 85%">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="#CC0000"></asp:Label>

                    </td>
                </tr>
            </table>
            
            <div style="width: 100%; text-align: left">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 8%;">Customer Group</td>
                        <td style="width: 15%;">
                            <asp:DropDownList ID="drpCustomerGroup" runat="server" Width="98%" OnSelectedIndexChanged="drpCustomerGroup_SelectedIndexChanged" AutoPostBack="true" TabIndex="1"></asp:DropDownList></td>
                        <td style="width: 10%;">Customer Code-Name </td>
                        <td style="width: 15%;">
                            <asp:DropDownList ID="drpCustomerCode" runat="server" Width="98%" OnSelectedIndexChanged="drpCustomerCode_SelectedIndexChanged" AutoPostBack="true" TabIndex="2"></asp:DropDownList>
                        </td>

                        <td style="width: 8%;">TIN/VAT</td>
                        <td style="width: 15%;">
                            <asp:TextBox ID="txtTIN" runat="server" Width="90%" ReadOnly="true" TabIndex="3"></asp:TextBox>
                            <%--<asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtInvoiceDate" Format="dd/MMM/yyyy"></asp:CalendarExtender>--%>

                        </td>
                        <td style="width: 10%;">Address</td>
                        <td style="width: 15%;">
                            <asp:TextBox ID="txtAddress" runat="server" ReadOnly="true" Width="90%" TabIndex="4" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 8%;">Mobile No</td>
                        <td style="width: 15%;">
                            <asp:TextBox ID="txtMobileNO" runat="server" ReadOnly="true" Width="94%" TabIndex="5"></asp:TextBox>

                        </td>
                        <td style="width: 10%;">Transporter Name</td>
                        <td style="width: 15%;">
                            <asp:TextBox ID="txtTransporterName" runat="server" OnTextChanged="txtTransporterName_TextChanged" MaxLength="50" Width="94%" TabIndex="6"></asp:TextBox>
                        </td>
                        <td style="width: 8%;">Driver Name</td>
                        <td style="width: 15%;">
                            <asp:TextBox ID="txtDriverName" runat="server" OnTextChanged="txtDriverName_TextChanged" MaxLength="50" Width="90%" TabIndex="7"></asp:TextBox>
                        </td>
                        <td style="width: 10%;">Driver Contact No</td>
                        <td style="width: 15%;">
                            <asp:TextBox ID="txtDriverContact" runat="server" OnTextChanged="txtDriverContact_TextChanged" onkeypress="CheckNumeric(event)"  MaxLength="12" Width="90%" TabIndex="8"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td style="width: 8%;">Vehicle No</td>
                        <td style="width: 15%;">
                            <asp:TextBox ID="txtVehicleNo" runat="server" Width="94%" MaxLength="20" TabIndex="8"></asp:TextBox>

                        </td>
                        <td style="width: 10%;">Remark</td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Width="90%" TabIndex="10" MaxLength="250"></asp:TextBox>

                        </td>
                        <td style="width: 15%;">&nbsp;</td>
                        <td style="width: 10%;"></td>
                        <td style="width: 15%;"></td>
                    </tr>

                </table>

                <div id="panelAdd" style="margin-top: 5px; width: 100%;">
                          <asp:Panel ID="panelAddLine" runat="server" Width="100%">
                                <table style="border-spacing: 4px; width: 100%;">
                                    <tr>
                                        <td style="width: 11%">Product Group
                                        </td>
                                        <td style="width: 11%">Product Sub Category</td>
                                        <td style="width: 18%">Product
                                    <asp:Label ID="lblHidden" runat="server" Visible="False"></asp:Label></td>
                                        <%--<td>Entry Type</td>--%>
                                        <td style="width: 5%">Crate Qty</td>
                                        <td class="auto-style2">Box Qty</td>
                                        <td style="width: 5%">PCS Qty</td>
                                        <td style="width: 5%">TotalBox</td>
                                        <td style="width: 5%">TotalPcs</td>
                                        <td style="width: 5%">TotalQty</td>
                                        <td style="width: 5%">BOXPCS</td>
                                        <%--<td>Qty[Box]</td>--%>


                                        <%--<td>Qty[Crates]</td>--%>


                                        <td style="width: 5%">Ltr</td>
                                        <td style="width: 5%">Price</td>

                                        <td style="width: 5%">Value</td>
                                        <td style="width: 5%">StockQty</td>

                                        <td style="width: 5%"></td>



                                    </tr>
                                    <tr style="width: 100%">
                                        <td style="width: 11%">
                                            <asp:DropDownList ID="DDLProductGroup" runat="server" Width="90%" AutoPostBack="true" OnSelectedIndexChanged="ddlProductGroup_SelectedIndexChanged" TabIndex="11">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 11%">
                                            <asp:DropDownList ID="DDLProductSubCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLProductSubCategory_SelectedIndexChanged" Width="90%" TabIndex="12">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 18%">
                                            <asp:DropDownList ID="DDLMaterialCode" runat="server" Width="90%" OnSelectedIndexChanged="DDLMaterialCode_SelectedIndexChanged" AutoPostBack="true" TabIndex="13" />
                                        </td>

                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtCrateQty" runat="server" AutoPostBack="true" onkeypress="CheckNumeric(event)" onchange="validate()" Width="90%" OnTextChanged="TextCrateQty_TextChanged" TabIndex="14" MaxLength="3" />
                                        </td>
                                        <td class="auto-style2">
                                            <asp:TextBox ID="txtBoxqty" runat="server" AutoPostBack="true" onkeypress="CheckNumeric(event)" onchange="validate()" Width="90%" OnTextChanged="txtBoxqty_TextChanged" TabIndex="14" MaxLength="4" />
                                        </td>
                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtPCSQty" runat="server" AutoPostBack="true" onkeypress="CheckNumeric(event)" onchange="validate();" Width="90%" OnTextChanged="txtPCSQty_TextChanged" TabIndex="14" MaxLength="8" />
                                        </td>
                                        <td style="width: 5%"><asp:TextBox ID="txtViewTotalBox" runat="server" ReadOnly="True" CssClass="textfield" Width="90%" Style="background-color: rgb(235, 235, 228)"></asp:TextBox></td>
                                        <td style="width: 5%"><asp:TextBox ID="txtViewTotalPcs" runat="server" ReadOnly="True" CssClass="textfield" Width="90%" Style="background-color: rgb(235, 235, 228)"></asp:TextBox></td>
                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtEnterQty" Enabled="false" runat="server" AutoPostBack="true" onkeypress="return IsNumeric(event)" Width="88%" OnTextChanged="txtQtyBox_TextChanged" TabIndex="14" />
                                        </td>
                                        <td style="width: 5%"><asp:TextBox ID="txtBoxPcs" runat="server" ReadOnly="True" CssClass="textfield" Width="90%" Style="background-color: rgb(235, 235, 228)"></asp:TextBox></td>

                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtLtr" runat="server" Width="98%" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" /></td>
                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtPrice" runat="server" Width="98%" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" /></td>

                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtValue" runat="server" Width="98%" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" /></td>
                                        <td style="width: 5%">
                                            <asp:TextBox ID="txtStockQty" runat="server" Width="98%" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" /></td>
                                        <td style="width: 5%">

                                            <asp:Button ID="BtnAddItem" runat="server" Text="Add" OnClick="BtnAddItem_Click" OnClientClick="return validate();" CssClass="ReportSearch" Height="25px" Width="50px" TabIndex="15" />

                                        </td>


                                    </tr>

                                    <tr>
                                        <td colspan="14">
                                            <asp:TextBox ID="txtSecDiscPer" runat="server" AutoPostBack="true" onkeypress="return IsNumeric(event)" Visible="false" Width="50px" />
                                            <asp:TextBox ID="txtSecDiscValue" runat="server" AutoPostBack="true" onkeypress="return IsNumeric(event)" Visible="false" Width="50px" />
                                            <asp:Button ID="btnGO" runat="server" CssClass="ReportSearch" Font-Size="XX-Small" OnClick="btnGO_Click" Text="GO" Visible="false" />
                                            <asp:TextBox ID="txtTDValue" runat="server" AutoPostBack="true" onkeypress="return IsNumeric(event)" Visible="false" Width="50px" />
                                            <asp:Button ID="btnApply" runat="server" CssClass="ReportSearch" Font-Size="XX-Small" OnClick="btnApply_Click" Text="Apply" Visible="false" />
                                            <asp:TextBox ID="txtQtyBox" runat="server" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" Width="1px" Visible="false"></asp:TextBox>
                                            <asp:TextBox ID="txtQtyCrates" runat="server" ReadOnly="True" Width="1px" Style="background-color: rgb(235, 235, 228)" Visible="false"></asp:TextBox>
                                            <asp:DropDownList ID="ddlEntryType" runat="server" Visible="false" Width="1px">
                                                <asp:ListItem Value="Box">BOX</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtPack" runat="server" Width="1px" Visible="false" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" />
                                            <asp:TextBox ID="txtCrateSize" runat="server" Width="1px" Visible="false" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" />
                                            <asp:TextBox ID="txtMRP" runat="server" Width="1px" Visible="false" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" />
                                            <asp:TextBox ID="txtSODate" runat="server" Visible="false"></asp:TextBox>
                                            <asp:TextBox ID="txtLoadSheetNumber" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                                            <asp:Label ID="lblsite" runat="server" Visible="false"></asp:Label>
                                            <asp:TextBox ID="txtInvoiceDate" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                                            <asp:TextBox ID="txtInvoiceNo" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                                            <asp:DropDownList ID="drpSONumber" runat="server" Visible="false">
                                            </asp:DropDownList>

                                            <asp:TextBox ID="txtLoadsheetDate" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                                            <asp:TextBox ID="TxtSetFocus" runat="server" Visible="false"></asp:TextBox>
                                        </td>


                                    </tr>

                                </table>
                            </asp:Panel>
                </div>
                
                <div id="controlHead" style="margin-top: 10px; padding-right: 10px; width: 99%;"></div>
                <div style="overflow: auto; margin-top: 10px; padding-right: 10px; width: 100%;">

                     <asp:GridView runat="server" ID="gvDetails" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True"
                                OnRowDataBound="gvDetails_RowDataBound" ShowFooter="True" OnRowDeleting="gvDetails_RowDeleting" DataKeyNames="Line_No">
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
                                            <asp:Label ID="Line_No" runat="server" Text='<%#  Eval("Line_No") %>' Visible="false" />
                                             <asp:HiddenField ID="HiddenFieldLineNo" Visible="false" runat="server" Value='<%# Eval("SNO") %>' />
                                            <span>
                                                <%#Container.DataItemIndex + 1 %>
                                            </span>
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
                                    <asp:TemplateField HeaderText="SO Qty" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="SO_Qty" runat="server" Text='<%#  Eval("SO_Qty") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                                    </asp:TemplateField>

                                    <%--6--%>
                                    <asp:TemplateField HeaderText="Box">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBoxQtyGrid" Width="40px" runat="server" Text='<%#  Eval("Box_Qty") %>' AutoPostBack="true" placeholder="0" onkeypress="CheckNumeric(event)" OnTextChanged="txtBoxQtyGrid_TextChanged"  DataFormatString="{0:n2}" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                        <ItemStyle HorizontalAlign="Left" Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pcs">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPcsQtyGrid" Width="40px" runat="server" Text='<%#  Eval("Pcs_Qty") %>' AutoPostBack="true" placeholder="0" onkeypress="CheckNumeric(event)" OnTextChanged="txtPcsQtyGrid_TextChanged"  DataFormatString="{0:n2}"  />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                        <ItemStyle HorizontalAlign="Left" Width="40px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Total_Box">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBox" Width="60px" runat="server" ReadOnly="true" AutoPostBack="true" onkeypress="return IsNumeric(event)" OnTextChanged="txtBox_TextChanged" Text='<%#  Eval("Invoice_Qty") %>'  DataFormatString="{0:n2}"  />
                                            <%-- AutoPostBack="true" onkeypress="return IsNumeric(event)" OnTextChanged="txtBox_TextChanged"--%>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="BoxPcs" > <%--ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"--%>
                                        <ItemTemplate>
                                            <asp:Label ID="txtBoxPcs" ReadOnly="true" Width="50px" runat="server" Text='<%#  Eval("BoxPcs") %>' AutoPostBack="true" onkeypress="CheckNumeric(event)" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                    </asp:TemplateField>
                                    <%--7--%>
                                    <asp:TemplateField HeaderText="StockQty">
                                        <ItemTemplate>
                                            <asp:Label ID="StockQty" runat="server" Text='<%#  Eval("StockQty") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--8--%>
                                    <asp:TemplateField HeaderText="LTR">
                                        <ItemTemplate>
                                            <asp:Label ID="LTR" runat="server" Text='<%#  Eval("Ltr") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--9--%>
                                    <asp:TemplateField HeaderText="Rate">
                                        <ItemTemplate>
                                            <asp:Label ID="txtRate" runat="server" Text='<%#  Eval("Rate") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--10--%>
                                    <asp:TemplateField HeaderText="Tax %">
                                        <ItemTemplate>
                                            <asp:Label ID="Tax" runat="server" DataFormatString="{0:n2}" Text='<%#  Eval("TaxPer") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--11--%>
                                    <asp:TemplateField HeaderText="Tax Value">
                                        <ItemTemplate>
                                            <asp:Label ID="TaxValue" runat="server" Text='<%#  Eval("TaxValue") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--12--%>
                                    <asp:TemplateField HeaderText="AddTax%">
                                        <ItemTemplate>
                                            <asp:Label ID="AddTax" Text='<%#  Eval("AddTaxPer") %>' runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--13--%>
                                    <asp:TemplateField HeaderText="A.Tax Value">
                                        <ItemTemplate>
                                            <asp:Label ID="AddTaxValue" runat="server" Text='<%#  Eval("AddTaxValue") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--14--%>
                                    <asp:TemplateField HeaderText="Disc %">
                                        <ItemTemplate>
                                            <asp:Label ID="Disc" runat="server" Text='<%#  Eval("Disc") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--15--%>
                                    <asp:TemplateField HeaderText="Disc Value">
                                        <ItemTemplate>
                                            <asp:Label ID="DiscValue" runat="server" Text='<%#  Eval("DiscVal") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--16--%>
                                    <asp:TemplateField HeaderText="SecDisc %" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="SecDisc" runat="server" Text="0.00" Width="60px" AutoPostBack="true" onkeypress="return IsNumeric(event)" OnTextChanged="SecDisc_TextChanged" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--17--%>
                                    <asp:TemplateField HeaderText="S.DiscValue" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="SecDiscValue" runat="server" Text="0.00" Width="60px" AutoPostBack="true" onkeypress="return IsNumeric(event)" OnTextChanged="SecDiscValue_TextChanged" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>
                                    <%--18--%>
                                    <asp:TemplateField HeaderText="Amount" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="Amount" runat="server" Text='<%#  Eval("Amount") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Scheme" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblScheme" runat="server" Text='<%#  Eval("SchemeCode") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField Visible="false" ItemStyle-Width="0px">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hDiscType" Visible="false" runat="server" Value='<%# Eval("DiscType") %>' />
                                            <asp:HiddenField ID="hDiscCalculationType" Visible="false" runat="server" Value='<%# Eval("DiscCalculationBase") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="0px"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="TD" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="TD" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="PE" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="PE" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Before Tax" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="ToatlBeforeTax" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="VAT AFTER PE" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="VatAfterPE" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Total">
                                        <ItemTemplate>
                                            <asp:Label ID="Total" runat="server" Text='<%#  Eval("Total") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="MRPVALUE">
                                        <ItemTemplate>
                                            <asp:Label ID="MRPVALUE" runat="server" Text='<%#  Eval("MRPVALUE") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkdelete" runat="server" CommandName="Delete">Delete</asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SO Qty" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="hdInvoice_Qty" runat="server" Text='<%#  Eval("Invoice_Qty") %>' />
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
                  
                </div>
                <table>
                    <tr>
                        <td style="padding: 10px" class="auto-style1">
                            <asp:Button ID="btnScheme" runat="server" Text="Scheme Details" CssClass="button" Height="31px" Width="140px" Visible="False" />
                        </td>
                    </tr>
                </table>
            </div>
            
            <asp:GridView ID="gvScheme" runat="server" AutoPostBack ="true" AutoGenerateColumns="False" GridLines="Horizontal" Width="100%" BackColor="White"
                BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" Visible="false">
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
                    <asp:BoundField HeaderText="FreeQty" DataField="TotalFreeQty"></asp:BoundField>
                    <asp:TemplateField Visible="false" ItemStyle-Width="0px">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnTotalFreeQty" Visible="false" runat="server" Value='<%# Eval("TotalFreeQty") %>' />
                        </ItemTemplate>
                        <ItemStyle Width="0px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qty">
                        <ItemTemplate>
                            <asp:TextBox ID="txtQty" runat="server" AutoPostBack="True" onkeypress="return IsNumeric(event)" OnTextChanged="txtQty_TextChanged" ReadOnly="True" Width="40px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="#003366" ForeColor="White" />
            </asp:GridView>
            
            </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

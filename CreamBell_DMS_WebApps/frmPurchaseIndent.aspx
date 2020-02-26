<%@ Page Title="Purchase Indent Creation" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPurchaseIndent.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPurchaseIndent" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/SSheet.css" rel="Stylesheet" type="text/css" />
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />

    <script type="text/javascript">

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
    <script language="javascript" type="text/javascript">

        var arr = [];
        function CheckDouble(Del) {
            //debugger;
            if (Del === "Delete") {
                var result = confirm("Do You want to delete that Item..??");
                if (result) {
                    while (arr.length > 0) {
                        arr.pop();
                    }

                    return true;

                } else { return false; }
            }


            //debugger;
            var bool = true;
            if (bool) {
                var select = document.getElementById("<%=DDLMaterialCode.ClientID%>");
                if (select.options[select.selectedIndex].value == "-1") {
                    alert('Please Select Item.....!');
                    select.focus();
                    return false;
                }
            }


            //if (bool) {
            //    var select = document.getElementById("<%=DDLMaterialCode.ClientID%>");
            //    var inputs = select.options[select.selectedIndex].value;
            //    for (var i = 0; i < arr.length; i++) {
            //        if (arr[i] === inputs) {
            //            alert("Same Item not allowed multiple times......!!");
            //            return false;
            //        }
            //    }
            //    arr.push(inputs);
            //}
            //else {
            //    return false;
            //}
        }


        var submit = 0;
        debugger
        function CheckIsRepeat() {
            //debugger;
            $("input[tabindex], textarea[tabindex]").each(function () {
                $(this).on("keypress", function (e) {
                    if (e.keyCode === 13) {
                        var nextElement = $('[tabindex="' + (this.tabIndex + 1) + '"]');
                        if (nextElement.length) {
                            $('[tabindex="' + (this.tabIndex + 1) + '"]').focus();
                            e.preventDefault();
                        } else
                            $('[tabindex="1"]').focus();
                    }
                });
            });
            if (++submit > 1) {
                alert('An attempt was made to submit this form more than once; this extra attempt will be ignored.');

                return false;
            }
        }


    </script>


    <style type="text/css">
        .auto-style2 {
            width: 8%;
        }

        .auto-style3 {
            width: 6%;
        }

        .auto-style6 {
            width: 10%;
        }
    </style>

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

        .pnlCSS {
            font-weight: bold;
            cursor: pointer;
            margin-left: 20px;
        }

        .auto-style2 {
            width: 6%;
        }

        .cbsam_select {
            width: 192px !important;
        }

        .auto-style6 {
            width: 9%;
        }

        .auto-style7 {
            width: 5%;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
            <asp:PostBackTrigger ControlID="txtSerch" />
            <asp:PostBackTrigger ControlID="btnNew" />
            <asp:PostBackTrigger ControlID="btnSave" />
            <asp:PostBackTrigger ControlID="btnConfirm" />
            <asp:PostBackTrigger ControlID="rdoManualEntry" />
            <asp:PostBackTrigger ControlID="ImDnldTemp" />
            <asp:AsyncPostBackTrigger ControlID="AsyncFileUpload1" />
            <asp:PostBackTrigger ControlID="btnUplaod" />
        </Triggers>
        <ContentTemplate>
            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 5px; background-color: #0085ca; padding: 2px 0px 0px 0px;">
                <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px; color: #FFFFFF;">Purchase Indent Creation</span>
            </div>
            <table style="width: 100%; text-align: left">
                <tr>
                    <td style="width: 5%">
                        <asp:Button ID="btnNew" runat="server" Text="New" CssClass="ReportSearch" OnClick="btnNew_Click" />
                    </td>

                    <td style="width: 5%">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ReportSearch" OnClick="btnSave_Click" TabIndex="1" />
                    </td>
                    <td style="width: 5%">
                        <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="ReportSearch" OnClick="btnConfirm_Click" TabIndex="2" />
                    </td>
                    <td style="width: 10%;">
                        <asp:RadioButton ID="rdoManualEntry" runat="server" AutoPostBack="true" Text="Manual Entry" Checked="true"
                            OnCheckedChanged="rdoManualEntry_CheckedChanged" GroupName="radio" />
                        <br />
                        <asp:RadioButton ID="rdoExcelEntry" runat="server" AutoPostBack="true" Text="Excel Upload"
                            OnCheckedChanged="rdoManualEntry_CheckedChanged" GroupName="radio" />
                    </td>
                    <td style="width: 10%;">
                        <asp:RadioButton ID="rdExempt" runat="server" AutoPostBack="true" Text="Exempt" CheToolTip="ExemptProducts" ValidationGroup="Exemption" GroupName="RdExemption" OnCheckedChanged="Exempt_CheckedChanged" />
                        <br />
                        <asp:RadioButton ID="rdNonExempt" runat="server" AutoPostBack="true" Text="NonExempt" ToolTip="NonExemptProducts" Checked="true" ValidationGroup="Exemption" GroupName="RdExemption" OnCheckedChanged="Exempt_CheckedChanged" />
                    </td>
                    <td style="width: 15%">
                        <%--<asp:UpdatePanel runat="server" ID="UploadPanel">
                                            <ContentTemplate>--%>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" Visible="false" OnClientUploadStarted="uploadStart" Height="18px" />
                                </td>
                            </tr>
                        </table>
                        <%--    </ContentTemplate>

                                        </asp:UpdatePanel>--%>
                    </td>
                    <td style="width: 5%;">&nbsp;</td>
                    <td style="width: 5%;">
                        <asp:ImageButton ID="ImDnldTemp" runat="server" AutoPostBack="true" src="Images/DownloadTemplate.gif" OnClick="ImDnldTemp_Click" Visible="false" ToolTip="Click to download excel template !!" />
                    </td>
                    <%--<asp:UpdatePanel runat="server" ID="UpdatePanel13">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnUplaod" />
                                        </Triggers>
                                        <ContentTemplate>--%>
                    <td style="width: 5%;">
                        <%--<asp:LinkButton ID="btnUplaod1" runat="server" Text="Upload" OnClick="btnUplaod_Click" CssClass="ReportSearch"></asp:LinkButton>--%>
                        <asp:Button ID="btnUplaod" runat="server" Text="Upload" OnClick="btnUplaod_Click" CssClass="ReportSearch" Visible="False" />
                    </td>
                    <%--    </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                    <td style="width: 10%">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    </td>

                    <td style="width: 15%; text-align: right">
                        <asp:Label ID="lblSearch" runat="server" Text="Indent No  :"></asp:Label>
                    </td>
                    <td style="width: 15%">
                        <asp:TextBox ID="txtSerch" runat="server" placeholder="Search here..." OnTextChanged="txtSerch_TextChanged" CssClass="textboxStyleNew" Width="90%" />
                    </td>

                    <td style="width: 10%">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="ReportSearch" TabIndex="1"></asp:Button>
                    </td>
                    <td style="width: 10%">
                        <asp:Label ID="lblsite" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>

            <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>

            <%--   <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>--%>

            <div style="margin-top: 10px">
                <asp:Panel ID="pnlFilter" CssClass="pnlCSS" runat="server">
                    <table style="width: 100%; border-spacing: 0px">
                        <tr>
                            <td style="text-align: left">Plant Code :</td>
                            <td style="text-align: left" class="auto-style2">
                                <asp:TextBox ID="txtSalesOfficeCode" runat="server" Width="189px" ReadOnly="true" CssClass="textboxStyleNew" Height="13px"></asp:TextBox></td>
                            <td style="text-align: left">Plant Name :</td>
                            <td class="auto-style5">
                                <asp:DropDownList ID="drpSalesOff" runat="server" AutoPostBack="true" CssClass="textboxStyleNew" OnSelectedIndexChanged="drpSalesOff_SelectedIndexChanged" Width="209px" Font-Size="8pt" TabIndex="3">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left">Indent Number :</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtIndentNo" runat="server" CssClass="textboxStyleNew" ReadOnly="true" Style="margin-left: 0px" Width="200px" Height="13px"></asp:TextBox>
                            </td>
                            <td style="text-align: left" class="auto-style4">Required Date :</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtRequiredDate" runat="server" Width="200px" CssClass="textboxStyleNew" Height="13px"></asp:TextBox>
                                <%--<asp:RangeValidator runat="server" ID="RangeValidator1" Type="Date" ControlToValidate="txtRequiredDate" MaximumValue="DateTime.Today" ></asp:RangeValidator>--%>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtRequiredDate" Format="dd/MMM/yyyy"></asp:CalendarExtender>
                            </td>
                        </tr>

                    </table>
                </asp:Panel>
            </div>
            <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
            <%--<div style="margin-left:6px">--%>

            <%--<asp:UpdatePanel ID="UpdatePanel13" runat="server">
            <Triggers>
                <asp:PostBackTrigger ="panelAdd" />
            </Triggers>
            <ContentTemplate>--%>
            <div id="panelAdd" style="margin-top: 10px; width: 100%;">

                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">--%>

                <%--<ContentTemplate>--%>
                <asp:Panel ID="panelAddLine" runat="server" CssClass="pnlCSS" Width="100%">
                    <table style="border-spacing: 0px; width: 100%; text-align: left">
                        <tr>
                            <td style="width: 10%">Business Unit
                            </td>
                            <td style="width: 15%">Product Group
                            </td>
                            <td style="width: 15%">Product Sub Category</td>
                            <td style="width: 20%">Product<asp:Label ID="lblHidden" runat="server" Visible="False"></asp:Label>
                            </td>
                            <td style="width: 2%" class="auto-style3">Stock Qty</td>
                            <td style="width: 10%" class="auto-style3">Enter Crate</td>
                            <td style="width: 10%">
                                <asp:Label ID="Label1" runat="server" Text="Enter Box"></asp:Label>
                            </td>
                            <td style="width: 5%">Box</td>
                            <td style="width: 5%">Crates</td>
                            <td style="width: 5%">Ltr</td>
                            <td style="width: 5%"></td>

                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlBusinessUnit" Visible="true" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBusinessUnit_SelectedIndexChanged" CssClass="dropdownField" Width="90%"></asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="DDLProductGroup" runat="server" Width="90%" AutoPostBack="true" OnSelectedIndexChanged="ddlProductGroup_SelectedIndexChanged" TabIndex="4" CssClass="textboxStyleNew" Font-Size="8pt" Height="24px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="DDLProductSubCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLProductSubCategory_SelectedIndexChanged" Width="90%" TabIndex="5" CssClass="textboxStyleNew" Font-Size="8pt">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="DDLMaterialCode" runat="server" Width="95%" OnSelectedIndexChanged="DDLMaterialCode_SelectedIndexChanged" TabIndex="8" AutoPostBack="true" CssClass="textboxStyleNew" Font-Size="8pt" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td class="auto-style6">
                                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtStockQty" runat="server" ReadOnly="true" placeholder="0" onkeypress="CheckNumeric(event)" Width="60%" CssClass="textboxStyleNew" Height="13px" MaxLength="6" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td class="auto-style3">
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <%--<asp:DropDownList ID="ddlEntryType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEntryType_SelectedIndexChanged" TabIndex="6" Width="90%" CssClass="textboxStyleNew" Font-Size="8pt">
                                    <asp:ListItem Value="Box">BOX</asp:ListItem>
                                    <asp:ListItem Value="Crate">CRATE</asp:ListItem>
                                </asp:DropDownList>--%>
                                        <asp:TextBox ID="txtEnterCrate" runat="server" AutoPostBack="true" placeholder="0" onkeypress="CheckNumeric(event)" Width="80%" OnTextChanged="txtEnterCrate_TextChanged" TabIndex="7" CssClass="textboxStyleNew" Height="13px" MaxLength="6" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtEnterQty" runat="server" AutoPostBack="true" placeholder="0" onkeypress="CheckNumeric(event)" Width="80%" OnTextChanged="txtQtyBox_TextChanged" TabIndex="7" CssClass="textboxStyleNew" Height="13px" MaxLength="6" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtQtyBox" runat="server" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" Width="50px" CssClass="textboxStyleNew" Height="13px"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtQtyCrates" runat="server" ReadOnly="True" Width="50px" Style="background-color: rgb(235, 235, 228)" CssClass="textboxStyleNew" Height="13px"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtLtr" runat="server" Width="50px" ReadOnly="True" Style="background-color: rgb(235, 235, 228)" CssClass="textboxStyleNew" Height="13px" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <%--<asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                <Triggers>
                            <asp:PostBackTrigger ControlID="BtnAddItem" />
                        </Triggers>--%>
                                <%-- <ContentTemplate>--%>
                                <asp:Button ID="BtnAddItem" runat="server" AutoPostBack="True" Text="Add" OnClick="BtnAddItem_Click"  OnClientClick="this.disabled=true;" UseSubmitBehavior="false" CssClass="button" BackColor="#0066CC" ForeColor="White" TabIndex="8" />
                                <%-- </ContentTemplate>
                                              </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
            </div>
            <%--  </ContentTemplate>
        </asp:UpdatePanel>--%>
            <table style="width: 100%">
                <tr>
                    <td style="width: 100%">
                        <div id="controlHead" style="margin-top: 5px; margin-left: 0px; padding-right: 10px; text-align: left"></div>
                        <div style="height: 330px; overflow: auto; margin-top: 5px; margin-left: 0px; padding-right: 10px;">

                            <asp:GridView runat="server" ID="gvDetails" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True"
                                OnRowCancelingEdit="gvDetails_RowCancelingEdit" OnRowEditing="gvDetails_RowEditing"
                                OnRowUpdating="gvDetails_RowUpdating" OnRowDeleting="gvDetails_RowDeleting"
                                OnSelectedIndexChanged="gvDetails_SelectedIndexChanged" OnRowDataBound="gvDetails_RowDataBound" OnRowCommand="gvDetails_RowCommand" showFooterWhenEmpty="True">

                                <AlternatingRowStyle BackColor="#CCFFCC" />
                                <Columns>

                                    <asp:TemplateField HeaderText="Line_No">
                                        <ItemTemplate>
                                            <span>
                                                <%#Container.DataItemIndex + 1 %>
                                            </span>
                                            <asp:Label ID="Line_No" runat="server" Text='<%# Eval("Line_No") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Product Group">
                                        <ItemTemplate>
                                            <asp:Label ID="Product_GroupCode" runat="server" Text='<%# Eval("Product_Group") %>' />
                                        </ItemTemplate>
                                        <%--  <FooterTemplate>
                                                <asp:DropDownList ID="drpProduct_Group_Entry" Width="140px" Font-Size="XX-Small" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Product_Group_Entry_SelectedIndexChanged" />
                                            </FooterTemplate>--%>
                                        <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                        <ItemStyle HorizontalAlign="Left" Width="120px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Product SubCategory">
                                        <ItemTemplate>
                                            <asp:Label ID="Product_SubCategory" runat="server" Text='<%# Eval("Product_SubCategory") %>' />
                                        </ItemTemplate>
                                        <%--<FooterTemplate>
                                                <asp:DropDownList ID="drpProduct_SubCategory_Entry" Width="111px" Font-Size="XX-Small" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpProduct_SubCategory_Entry_SelectedIndexChanged" />
                                            </FooterTemplate>--%>
                                        <HeaderStyle HorizontalAlign="Left" Width="111px" />
                                        <ItemStyle HorizontalAlign="Left" Width="111px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Product Code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="Product_Code" runat="server" Text='<%# Eval("Product_Code") %>' />
                                        </ItemTemplate>
                                        <%--<FooterTemplate>
                                                <asp:DropDownList ID="drpProduct_Code_Entry" Width="10px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpProduct_Code_Entry_SelectedIndexChanged" />
                                            </FooterTemplate>--%>
                                        <HeaderStyle HorizontalAlign="Left" Width="10px" />
                                        <ItemStyle HorizontalAlign="Left" Width="10px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="ProductCode " Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Product_Code") %>' />
                                        </ItemTemplate>
                                        <%--<FooterTemplate>
                                                <asp:TextBox ID="txtProductCode_Entry" Width="70px" runat="server" AutoPostBack="true" placeholder="Type Product Code" OnTextChanged="txtProductCode_Entry_TextChanged" align="Right" />
                                            </FooterTemplate>--%>
                                        <HeaderStyle HorizontalAlign="left" Width="70px" />
                                        <ItemStyle HorizontalAlign="left" Width="70px" />
                                        <FooterStyle HorizontalAlign="left" Width="70px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Product Name">
                                        <ItemTemplate>
                                            <asp:Label ID="Product_Name" runat="server" Text='<%# Eval("Product_Name") %>' />
                                        </ItemTemplate>
                                        <%--<FooterTemplate>
                                                <asp:DropDownList ID="drpProduct_Name_Entry" Width="330px" Font-Size="XX-Small" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpProduct_Name_Entry_SelectedIndexChanged" />
                                            </FooterTemplate>--%>
                                        <HeaderStyle HorizontalAlign="Left" Width="260px" />
                                        <ItemStyle HorizontalAlign="Left" Width="260px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Stock Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="Stock_Qty" runat="server" Text='<%# Eval("Stock_Qty") %>' />
                                        </ItemTemplate>
                                        <%--<FooterTemplate>
                                                <asp:DropDownList ID="drpProduct_Name_Entry" Width="330px" Font-Size="XX-Small" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpProduct_Name_Entry_SelectedIndexChanged" />
                                            </FooterTemplate>--%>
                                        <HeaderStyle HorizontalAlign="Left" Width="260px" />
                                        <ItemStyle HorizontalAlign="Left" Width="260px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="EntryType" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEntryType" runat="server" />
                                        </ItemTemplate>
                                        <%--<FooterTemplate>
                                                <asp:DropDownList ID="drpEntryType" Width="60px" Font-Size="XX-Small" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpEntryType_SelectedIndexChanged" />
                                            </FooterTemplate>--%>
                                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="QTY (Box) ">
                                        <ItemTemplate>
                                            <asp:Label ID="Box" runat="server" Text='<%# Eval("Box") %>' />
                                        </ItemTemplate>
                                        <%--<FooterTemplate>
                                                <asp:TextBox ID="txtBox_Entry" Width="80px" runat="server" AutoPostBack="true" onkeypress="return IsNumeric(event)" OnTextChanged="txtBox_Entry_TextChanged" align="Right" />
                                            </FooterTemplate>--%>
                                        <HeaderStyle HorizontalAlign="Right" Width="80px" />
                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                        <FooterStyle HorizontalAlign="Right" Width="80px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="QTY (Crates)  ">
                                        <ItemTemplate>
                                            <asp:Label ID="Crates" runat="server" Text='<%# Eval("Crates") %>' />
                                        </ItemTemplate>
                                        <%--<FooterTemplate>
                                                <asp:TextBox ID="txtCrates_Entry" Width="80px" runat="server" AutoPostBack="true" onkeypress="return IsNumeric(event)" OnTextChanged="txtCrates_Entry_TextChanged" />
                                            </FooterTemplate>--%>
                                        <HeaderStyle HorizontalAlign="Right" Width="80px" />
                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                        <FooterStyle HorizontalAlign="Right" Width="80px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="QTY (Ltr)  ">
                                        <ItemTemplate>
                                            <asp:Label ID="Ltr" runat="server" Text='<%# Eval("Ltr") %>' />
                                        </ItemTemplate>
                                        <%--<FooterTemplate>
                                                <asp:TextBox ID="txtLtr_Entry" Width="80px" runat="server" ReadOnly="true" />
                                            </FooterTemplate>--%>
                                        <HeaderStyle HorizontalAlign="Right" Width="80px" />
                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                        <FooterStyle HorizontalAlign="Right" Width="80px" />
                                    </asp:TemplateField>


                                    <asp:TemplateField ShowHeader="False" HeaderStyle-Width="50px">
                                        <EditItemTemplate>
                                            <%--<asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update" ></asp:LinkButton>--%>
                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <%--<asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" ></asp:LinkButton>--%>
                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                            <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                         <ContentTemplate>--%>
                                            <%--<asp:Button ID="btnAdd" CommandName="AddNew" runat="server" CssClass="button" ValidationGroup="FooterGroup" Text="Add" stye="width: 55px; height: 27px" />--%>
                                            <%-- </ContentTemplate>
                         </asp:UpdatePanel>--%>
                                        </FooterTemplate>

                                        <HeaderStyle Width="50px"></HeaderStyle>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="id" Visible="False" />

                                </Columns>
                                <EmptyDataTemplate>
                                    No Record Found...
                                </EmptyDataTemplate>
                                <FooterStyle CssClass="table-condensed" BackColor="#B5C7DE" ForeColor="#4A3C8C" />

                                <HeaderStyle CssClass="GVFixedHeader" BackColor="#05345C" Font-Bold="True" ForeColor="#F7F7F7" />

                                <RowStyle BackColor="White" ForeColor="#4A3C8C" />
                                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                <SortedAscendingCellStyle BackColor="#F4F4FD" />
                                <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                                <SortedDescendingCellStyle BackColor="#D8D8F0" />
                                <SortedDescendingHeaderStyle BackColor="#3E3277" />
                            </asp:GridView>

                        </div>
                        <%--<asp:UpdatePanel ID="fgyhhj" runat="server">
                    <ContentTemplate>--%>
                        <div>
                            <%--<asp:Button ID="Button2" runat="Server" Text="" Style="display: none;" />
                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button2"
                        PopupControlID="Panel1" CancelControlID="Button4"
                        BackgroundCssClass="ModalPoupBackgroundCssClass" BehaviorID="ModalPopupExtender1">
                    </asp:ModalPopupExtender>--%>

                            <asp:Panel ID="Panel1" runat="server" Visible="false">
                                <div>
                                    <span style="color: red; font-weight: 600; text-align: center">Records which are not uploaded !!</span>

                                    <%--<asp:Button ID="Button4" runat="server" CssClass="Operationbutton" data-dismiss="modal" Text="Close" aria-hidden="true"></asp:Button>--%>
                                </div>
                                <p></p>
                                <div style="overflow-x: scroll; width: 98%; height: 200px">
                                    <asp:GridView ID="gridviewRecordNotExist" runat="server" AutoGenerateColumns="true" Style="width: 100%" Font-Size="Small" CssClass="Grid" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                                        <AlternatingRowStyle BackColor="#CCFFCC" />
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
                        <%--    </ContentTemplate>
                </asp:UpdatePanel>--%>
                

                    </td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



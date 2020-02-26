<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmAdjustmentEntry.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmCreditDebitNotePurchaser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />
    <link href="css/SSheet.css" rel="Stylesheet" type="text/css" />
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

    <style type="text/css">
        .BlueButton {
            background: #3498db;
            background-image: -webkit-linear-gradient(top, #3498db, #2980b9);
            background-image: -moz-linear-gradient(top, #3498db, #2980b9);
            background-image: -ms-linear-gradient(top, #3498db, #2980b9);
            background-image: -o-linear-gradient(top, #3498db, #2980b9);
            background-image: linear-gradient(to bottom, #3498db, #2980b9);
            -webkit-border-radius: 0;
            -moz-border-radius: 0;
            border-radius: 0px;
            font-family: Arial;
            color: #ffffff;
            font-size: 11px;
            padding: 5px 7px 6px 8px;
            text-decoration: none;
        }

            .BlueButton:hover {
                background: #3cb0fd;
                background-image: -webkit-linear-gradient(top, #3cb0fd, #3498db);
                background-image: -moz-linear-gradient(top, #3cb0fd, #3498db);
                background-image: -ms-linear-gradient(top, #3cb0fd, #3498db);
                background-image: -o-linear-gradient(top, #3cb0fd, #3498db);
                background-image: linear-gradient(to bottom, #3cb0fd, #3498db);
                text-decoration: none;
            }

        .Legend1 {
            font-weight: bold;
            font-size: 12px;
            color: #4C99CC;
            text-align: left;
            text-align: left;
        }

        .Fieldset1 {
            border-color: #4C99CC;
            border-width: 1px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdateProgress ID="upprogress" runat="server" AssociatedUpdatePanelID="upadj" DisplayAfter="0">
        <ProgressTemplate>
            <div class="overlay">
            </div>
            <div class="overlayContent">
                <center>
                    Please Wait...don't close this window until processing is being done.
                     <br />
                    <img src="../../IMAGES/bar.gif" alt="" />
                </center>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="upadj">
        <ContentTemplate>
            <div style="width: 100%; height: 20px; border-radius: 4px; background-color: #1564ad; padding: 0px 0px 0px 0px;">
                <span style="font-family: Segoe UI; color: white; font-size: 11px; font-weight: bold;">Adjustment Entry </span>
            </div>

            <div style="margin-top: 2px; text-align: left; height: 30px;">
                &nbsp;&nbsp;
        <asp:Button ID="BtnSaveAdjustment" runat="server" Text="Save " ToolTip="Click To Save the Stock Adjustment" CssClass="BlueButton" Width="90px" OnClick="BtnSaveAdjustment_Click1" />

                <asp:Button ID="BtnRefresh" runat="server" Text="Refresh" ToolTip="Click To Refresh Screen" CssClass="BlueButton" Width="80px" OnClick="BtnRefresh_Click" />
            </div>

            <div style="margin-left: 1px; width: 98%">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="panelHeader" runat="server" GroupingText="Search Section">

                            <table style="width: 99%; text-align: left">
                                <tr>

                                    <td style="width: 99%;" colspan="15">
                                        <asp:Label ID="LblMessage" runat="server" Font-Bold="true" ForeColor="Red" Font-Names="Segoe UI"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 5%">Invoice No :</td>
                                    <td style="width: 15%">
                                        <asp:DropDownList ID="ddlInvoiceNo" runat="server" CssClass="dropdown" Width="90%" AutoPostBack="true" OnSelectedIndexChanged="ddlInvoiceNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5%">Invoice Date :</td>
                                    <td style="width: 10%;">
                                        <b>
                                            <asp:Label ID="lblInvoiceDate" runat="server"></asp:Label></b>
                                    </td>
                                    <td style="width: 6%">Invoice Value :</td>
                                    <td style="width: 10%">
                                        <b>
                                            <asp:Label ID="lblInvoiceValue" runat="server"></asp:Label>
                                        </b>
                                    </td>
                                    <td style="width: 6%">Supplier Name :</td>
                                    <td style="width: 25%">
                                        <b>
                                            <asp:Label ID="lblSupplierName" runat="server"></asp:Label>
                                        </b>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="panelAdjustmentEntry" runat="server" GroupingText="Adjustment Entry Section">

                            <table style="width: 99%; text-align: left">
                                <tr>
                                    <td style="width: 15%">Reason Type</td>
                                    <td style="width: 15%">Product Group</td>
                                    <td style="width: 15%">Product Sub Category</td>
                                    <td style="width: 35%">Product Description</td>
                                    <td style="width: 10%">Entry Type</td>

                                    <td style="width: 10%">Adjustment Qty (+/-)</td>
                                    <td style="width: 5%"></td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        <asp:DropDownList ID="DDLReason" runat="server" Width="95%" AutoPostBack="true" OnSelectedIndexChanged="DDLReason_SelectedIndexChanged"></asp:DropDownList></td>
                                    <td style="width: 15%">
                                        <asp:DropDownList ID="DDLProductGroup" runat="server" Width="95%" AutoPostBack="true"
                                            OnSelectedIndexChanged="DDLProductGroup_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:DropDownList ID="DDLProdSubCategory" runat="server" Width="95%" AutoPostBack="true" OnSelectedIndexChanged="DDLProdSubCategory_SelectedIndexChanged">
                                        </asp:DropDownList></td>
                                    <td style="width: 35%">
                                        <asp:DropDownList ID="DDLProductDesc" runat="server" Width="95%" AutoPostBack="true" OnSelectedIndexChanged="DDLProductDesc_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 10%">
                                        <asp:DropDownList ID="DDLEntryType" runat="server" Width="95%">
                                            <asp:ListItem>Box</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>


                                    <td style="width: 10%">
                                        <asp:TextBox ID="txtAdjQty" runat="server" Width="60px" AutoPostBack="true" OnTextChanged="txtAdjValue_TextChanged">
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ForeColor="Red" ControlToValidate="txtAdjQty" Display="Dynamic"
                                            ErrorMessage="*Invalid" Font-Size="10pt" ValidationExpression="^-?[0-9]\d*(\.\d+)?$">
                                        </asp:RegularExpressionValidator>

                                    </td>
                                    <td style="width: 5%">
                                        <asp:Button ID="BtnAdd" runat="server" Text="Add" ToolTip="Click To Add Entered Records.!" Width="40px" OnClick="BtnAdd_Click" CssClass="BlueButton" /></td>
                                </tr>
                                <tr>
                                    <td colspan="7" style="text-align: center; width: 99%">

                                        <div style="width: 65%; text-align: center; padding-left: 150px">
                                            <fieldset id="fieldset4" runat="server" class="Fieldset1">
                                                <legend id="legend4" class="Legend1">
                                                    <asp:Label ID="lblProductName" runat="server" Text="Product Detail"></asp:Label>
                                                </legend>
                                                <table style="width: 100%">
                                                    <tr>


                                                        <td style="width: 5%">Qty</td>
                                                        <td style="width: 10%">
                                                            <b>
                                                                <asp:Label ID="lblPreBoxQty" runat="server"></asp:Label></b>
                                                        </td>
                                                        <td style="width: 10%">Rate</td>
                                                        <td style="width: 10%">
                                                            <b>
                                                                <asp:Label ID="lblRate" runat="server"></asp:Label></b>
                                                        </td>
                                                        <td style="width: 10%">VAT INC PERC</td>
                                                        <td style="width: 10%">
                                                            <b>
                                                                <asp:Label ID="lblTaxPer" runat="server"></asp:Label></b>
                                                        </td>
                                                        <td style="width: 10%">Basic Value</td>
                                                        <td style="width: 10%">
                                                            <b>
                                                                <asp:Label ID="lblBasicValue" runat="server"></asp:Label></b>
                                                        </td>

                                                        <td style="width: 10%">Amount</td>
                                                        <td style="width: 10%">
                                                            <b>
                                                                <asp:Label ID="lblAmount" runat="server"></asp:Label></b>
                                                        </td>

                                                    </tr>

                                                </table>
                                            </fieldset>
                                        </div>

                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="PanelGrid" runat="server" GroupingText=" Adjustment Entry Grid Details" ScrollBars="Vertical">
                            <asp:GridView ID="gridAdjusment" runat="server" AutoGenerateColumns="False" Width="100%">
                                <AlternatingRowStyle BackColor="#CCFFCC" />
                                <Columns>
                                    <asp:TemplateField Visible="false" ItemStyle-Width="0px" HeaderText="SNo">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="HiddenField2" Visible="false" runat="server" Value='<%# Eval("SNO") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="0px" HorizontalAlign="Left"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField Visible="false" ItemStyle-Width="0px" HeaderText="PRODUCT_CODE">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdProductCode" Visible="false" runat="server" Value='<%# Eval("PRODUCT_CODE") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="0px" HorizontalAlign="Left"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField Visible="false" ItemStyle-Width="0px" HeaderText="Sale_InvoiceNo">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdSale_InvoiceNo" Visible="false" runat="server" Value='<%# Eval("Sale_InvoiceNo") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="0px" HorizontalAlign="Left"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:BoundField HeaderText="Reason" DataField="Reason" Visible="false">
                                        <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>

                                    <asp:BoundField HeaderText="PURCH_RECIEPTNO" DataField="PURCH_RECIEPTNO" Visible="false">
                                        <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>

                                    <asp:BoundField HeaderText="Product Group" DataField="ProductGroup">
                                        <HeaderStyle Width="90px" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Sub Category" DataField="ProductSubCategory">
                                        <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Product Description" DataField="ProductDesc">
                                        <HeaderStyle Width="190px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="UOM" DataField="UOM">
                                        <HeaderStyle Width="40px" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Reason" DataField="ReasonName">
                                        <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="GRN Qty" DataField="PreBoxQty">
                                        <HeaderStyle Width="90px" HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField HeaderText="Rate" DataField="PreRate">
                                        <HeaderStyle Width="90px" HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField HeaderText="Tax" DataField="PreTax">
                                        <HeaderStyle Width="90px" HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField HeaderText="Basic Value" DataField="PreBasicValue">
                                        <HeaderStyle Width="90px" HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField HeaderText="GRN Amt" DataField="Amount">
                                        <HeaderStyle Width="90px" HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField HeaderText="Adj Qty" DataField="AdjustmentValue">
                                        <HeaderStyle Width="80px" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>

                                    <asp:BoundField HeaderText="Adj Value" DataField="CreditDebitValue">
                                        <HeaderStyle Width="80px" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnDel" runat="server" Text="Delete" ForeColor="Red" OnClick="lnkbtnDel_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                </Columns>
                                <HeaderStyle BackColor="#05345C" Font-Bold="True" ForeColor="#F7F7F7" />
                                <RowStyle BackColor="White" ForeColor="#4A3C8C" />
                                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                <SortedAscendingCellStyle BackColor="#F4F4FD" />
                                <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                                <SortedDescendingCellStyle BackColor="#D8D8F0" />
                                <SortedDescendingHeaderStyle BackColor="#3E3277" />

                            </asp:GridView>

                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

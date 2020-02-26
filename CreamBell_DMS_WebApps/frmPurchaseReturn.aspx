<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPurchaseReturn.aspx.cs"
    Inherits="CreamBell_DMS_WebApps.frmPurchaseReturn" EnableEventValidation="false" %>

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
    <style type="text/css">
        .ddl {
            background-image: url('Images/arrow-down-icon-black.png');
        }

            .ddl:hover {
                background-image: url('Images/arrow-down-icon-black.png');
            }

        .auto-style1 {
            height: 26px;
        }

        .auto-style2 {
            height: 30px;
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div style="width: 98%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
                <span style="font-family: Segoe UI; color: white; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px;">Purchase Return </span>
            </div>


            <%--  <div style="border-bottom-style: solid; border-bottom-width: 5px; border-bottom-color: #000066">
        <table>
            <tr>
                <td style="padding: 10px">
                    <asp:Button ID="BtnNew" runat="server" Text="New" CssClass="button" Visible="false" Height="31px" />
                    &nbsp;&nbsp;
                   
                </td>
                <td style="padding: 0px 0px 0px 300px;">
                    <asp:DropDownList ID="DDLSearch" runat="server" CssClass="ddl" data-toggle="dropdown" Width="200px" Style="margin-left: 0px" Visible="false">
                        <asp:ListItem>Material_Group</asp:ListItem>
                        <asp:ListItem>Material_Code</asp:ListItem>
                        <asp:ListItem>Material_Name</asp:ListItem>
                    </asp:DropDownList>

                </td>
                <td>
                    <div>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="input1 cf" placeholder="Search here..." Visible="false" />
                        <span id="span1" class="arrow_box" onmouseover="test()" onmouseout="test1()" style="visibility: hidden">
                            <asp:Button ID="btn2" runat="server" CssClass="button1 cf" Style="margin: 0px 0px 0px -2px" Text="Search" Visible="false"></asp:Button>
                        </span>
                    </div>

                </td>
            </tr>
        </table>
    </div>--%>

            <div style="margin-left: 10px; width: 100%; text-align: left">

                <table style="width: 100%; border-spacing: 0px">
                    <tr>
                        <td colspan="7" style="text-align: left">
                            <asp:Button ID="BtnSave" runat="server" Text="Save " CssClass="button" Height="25px" OnClick="BtnSave_Click" Width="63px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style9">Reciept No</td>
                        <td>
                            <asp:TextBox ID="txtReceiptNo" runat="server" Width="200px" CssClass="textboxStyleNew" Height="13px"></asp:TextBox></td>
                        <td class="auto-style10">&nbsp;</td>
                        <td>Invoice No</td>
                        <td class="auto-style12">
                            <asp:TextBox ID="txtInvoiceNo" runat="server" Width="200px" CssClass="textboxStyleNew" Height="13px" />

                        </td>
                        <td class="tdpadding">
                            <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/Images/searchicon.gif" />
                        </td>
                    </tr>

                    <tr>
                        <td class="auto-style9">Reciept Date</td>
                        <td>
                            <asp:TextBox ID="txtReceiptDate" runat="server" Width="200px" CssClass="textboxStyleNew" Height="13px"></asp:TextBox></td>
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtReceiptDate" Format="dd-MMM-yyyy" CssClass="CalendarCss">
                        </cc1:CalendarExtender>
                        <td class="auto-style11">&nbsp;</td>
                        <td>Invoice Date</td>
                        <td class="auto-style12">
                            <asp:TextBox ID="txtInvoiceDate" runat="server" Width="200px" CssClass="textboxStyleNew" Height="13px" />
                        </td>
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtInvoiceDate" Format="dd-MMM-yyyy" CssClass="CalendarCss"></cc1:CalendarExtender>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style9">Transporter Name</td>
                        <td>
                            <asp:TextBox ID="txtTransporterName" runat="server" Width="200px" CssClass="textboxStyleNew" Height="13px" /></td>
                        <td class="auto-style11">&nbsp;</td>
                        <td>Driver Number</td>
                        <td class="auto-style12">
                            <asp:TextBox ID="txtDriverNumber" runat="server" Width="200px" CssClass="textboxStyleNew" Height="13px" /></td>
                        <td>&nbsp;</td>
                    </tr>

                    <tr>
                        <td class="auto-style1">Vehicle Number</td>
                        <td class="auto-style1">
                            <asp:TextBox ID="txtVehicleNumber" runat="server" Width="200px" CssClass="textboxStyleNew" Height="13px" /></td>
                        <td class="auto-style1"></td>
                        <td class="auto-style1">Vehicle Type</td>
                        <td class="auto-style1">
                            <asp:TextBox ID="txtVehicleType" runat="server" Width="200px" CssClass="textboxStyleNew" Height="13px" /></td>
                        <td class="auto-style1"></td>
                    </tr>

                    <tr>
                        <td class="auto-style9">Return Reason</td>
                        <td>
                            <%--<asp:TextBox ID="txtReturnReason" runat="server" Width="200px" />--%>
                            <asp:DropDownList ID="DDLReturnReason" runat="server" Width="209px" Height="23px" CssClass="textboxStyleNew" Font-Size="8pt"></asp:DropDownList>
                        </td>
                        <td class="auto-style11">&nbsp;</td>
                        <td>Remark</td>
                        <td class="auto-style12">
                            <asp:TextBox ID="txtRemark" runat="server" Width="200px" CssClass="textboxStyleNew" Height="13px" /></td>
                        <td>&nbsp;</td>
                    </tr>

                </table>
            </div>


            <%-- <asp:Panel ID="pnlClick" runat="server" CssClass="pnlCSS">
           <div style=" height:30px; vertical-align:middle">
                <div style="float:left; color:darkgreen;padding:5px 5px 0 0; font-weight:200; font-size:large">
                    Add Material
                </div>
            <div style="float:right; color:White; padding:5px 5px 0 0">
                <asp:Label ID="lblMessage" runat="server" Text="+" Font-Bold="true" ForeColor="Black" Font-Size="Large"/>
                <asp:Image ID="imgArrows" runat="server" />
            </div>
            <div style="clear:both"></div>
           </div>
        </asp:Panel>--%>

            <div id="panelAdd" style="margin-top: 5px; width: 100%;">
                <asp:Panel ID="panelAddLine" runat="server" Width="100%" GroupingText="Items Section">
                    <%--  <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>--%>
                    <table style="border-spacing: 4px; width: 100%; text-align: left">
                        <tr>

                            <td class="auto-style14">Product Group
                            </td>
                            <td class="auto-style19">Product SubCategory</td>
                            <td class="auto-style15">Product Name</td>
                            <td class="auto-style16">Entry Type</td>
                            <td class="auto-style17">
                                <asp:Literal ID="LiteralEntryType" runat="server" Text="Enter Box"></asp:Literal>
                            </td>
                            <td>Price</td>
                            <td>Discount%</td>
                            <td>Tax%</td>
                            <%--<td>Value</td>--%>

                            <%--<td>Qty[Box]</td>
                <td>Qty[Crates]</td>
                <td>Qty[Ltr]</td>
                <td>Price</td>
                <td>Tax%</td>
                <td>Discount</td>
                <td>Value</td>--%>
                        </tr>
                        <tr>
                            <td class="auto-style2">
                                <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server" EnableViewState="true" UpdateMode="Conditional">--%>
                                <%-- <Triggers>
                            <asp:PostBackTrigger ControlID="DDLMaterialGroup"  />
                        </Triggers>--%>
                                <%-- <ContentTemplate>--%>
                                <asp:DropDownList ID="DDLMaterialGroup" runat="server" Width="100px" AutoPostBack="true" Font-Size="8pt" Font-Names="Arial"
                                    OnSelectedIndexChanged="DDLMaterialGroup_SelectedIndexChanged" CssClass="textboxStyleNew">
                                </asp:DropDownList>


                                <%--  </ContentTemplate>
                         
                    </asp:UpdatePanel>--%>
                            </td>
                            <td class="auto-style2">
                                <%--  <asp:UpdatePanel ID="UpdatePanel2" runat="server" EnableViewState="true" UpdateMode="Conditional">--%>
                                <%--<Triggers>
                            <asp:PostBackTrigger ControlID="DDLProductSubCategory"  />
                        </Triggers>--%>
                                <%-- <ContentTemplate>--%>
                                <asp:DropDownList ID="DDLProductSubCategory" runat="server" Width="120px" AutoPostBack="true" Font-Size="8pt" Font-Names="Arial"
                                    OnSelectedIndexChanged="DDLProductSubCategory_SelectedIndexChanged" TabIndex="1" CssClass="textboxStyleNew">
                                </asp:DropDownList>
                                <%--  </ContentTemplate>
                         
                    </asp:UpdatePanel>--%>
                            </td>
                            <td class="auto-style2">

                                <asp:DropDownList ID="DDLMaterialCode" runat="server" Width="250px" Font-Size="8pt" AutoPostBack="true" Font-Names="Arial"
                                    OnSelectedIndexChanged="DDLMaterialCode_SelectedIndexChanged" CssClass="textboxStyleNew" TabIndex="2">
                                </asp:DropDownList>

                            </td>
                            <td class="auto-style2">

                                <asp:DropDownList ID="DDLEntryType" runat="server" AutoPostBack="true" Width="60px" Font-Size="8pt" Font-Names="Arial"
                                    OnSelectedIndexChanged="DDLEntryType_SelectedIndexChanged" CssClass="textboxStyleNew">
                                    <asp:ListItem>Box</asp:ListItem>
                                    <asp:ListItem>Crate</asp:ListItem>
                                </asp:DropDownList>

                            </td>

                            <td class="auto-style2">
                                <asp:TextBox ID="txtEntryValue" runat="server" Width="60px" Enabled="true" TabIndex="3" AutoPostBack="true" OnTextChanged="txtEntryValue_TextChanged" CssClass="textboxStyleNew" Height="13px"></asp:TextBox></td>

                            <td class="auto-style2">
                                <asp:TextBox ID="txtPrice" runat="server" Width="60px" Enabled="true" AutoPostBack="true" OnTextChanged="txtPrice_TextChanged" CssClass="textboxStyleNew" Height="13px" /></td>
                            <td class="auto-style2">
                                <asp:TextBox ID="txtDiscount" runat="server" Width="60px" Enabled="false" Text="0" CssClass="textboxStyleNew" Height="13px" /></td>
                            <td class="auto-style2">
                                <asp:TextBox ID="txtTaxPerc" runat="server" Width="60px" Text="" Enabled="true" AutoPostBack="true" OnTextChanged="txtTaxPerc_TextChanged" CssClass="textboxStyleNew" Height="13px" /></td>
                            <td class="auto-style2">
                                <asp:TextBox ID="txtValue" runat="server" Width="60px" Enabled="false" Visible="false" CssClass="textboxStyleNew" Height="13px" /></td>

                            <%--<td><asp:TextBox ID="txtQtyBox" runat="server" Width="60px"  AutoPostBack="true" OnTextChanged="txtQtyBox_TextChanged" /></td>
                <td><asp:TextBox ID="txtQtyCrates" runat="server" Width="60px" Enabled="false"  ></asp:TextBox></td>
                <td><asp:TextBox ID="txtLtr" runat="server" Width="60px" Enabled="false" /></td>
                <td><asp:TextBox ID="txtPrice" runat="server" Width="60px" Enabled="false" /></td>
                <td><asp:TextBox ID="txtTaxPerc" runat="server" Width="60px"  Text="12.23"   Enabled="false"/></td>
                <td><asp:TextBox ID="txtDiscount" runat="server"  Width="60px" Enabled="false" Text="2"   /></td>
                <td><asp:TextBox ID="txtValue" runat="server" Width="60px"  Enabled="false"/></td>--%>

                            <td class="auto-style2">
                                <asp:Button ID="BtnAddItem" runat="server" Text="Add" OnClick="BtnAddItem_Click" CssClass="button" TabIndex="4" />
                            </td>
                        </tr>

                        <tr>
                            <td class="auto-style22">&nbsp;</td>
                            <td class="auto-style23">&nbsp;</td>
                            <td class="auto-style24">
                                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtQtyCrates" 
                            Display="Dynamic" ErrorMessage="*Invalid" Font-Size="10pt" ForeColor="Red" ValidationExpression="((\d+)((\.\d{1,2})?))$">

                        </asp:RegularExpressionValidator>--%>
                            </td>
                            <td class="auto-style25">
                                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"  ForeColor="Red" ControlToValidate="txtQtyBox" Display="Dynamic" 
                            ErrorMessage="*Invalid" Font-Size="10pt" ValidationExpression="((\d+)((\.\d{1,2})?))$">

                        </asp:RegularExpressionValidator>--%>
                            </td>
                            <td class="auto-style26">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ForeColor="Red" ControlToValidate="txtEntryValue" Display="Dynamic"
                                    ErrorMessage="*Invalid" Font-Size="10pt" ValidationExpression="((\d+)((\.\d{1,2})?))$">

                                </asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ForeColor="Red" ControlToValidate="txtPrice" Display="Dynamic"
                                    ErrorMessage="*Invalid" Font-Size="10pt"
                                    ValidationExpression="((\d+)((\.\d{1,2})?))$">

                                </asp:RegularExpressionValidator>
                            </td>
                            <td></td>
                            <td>
                                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server"  ForeColor="Red" ControlToValidate="txtDiscount"
                             Display="Dynamic" ErrorMessage="*Invalid" Font-Size="10pt" ValidationExpression="((\d+)((\.\d{1,2})?))$">

                        </asp:RegularExpressionValidator>--%>

                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ForeColor="Red" ControlToValidate="txtTaxPerc" Display="Dynamic"
                                    ErrorMessage="*Invalid" Font-Size="10pt" ValidationExpression="((\d+)((\.\d{1,2})?))$">

                                </asp:RegularExpressionValidator>
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>

                    </table>
                    <%-- </ContentTemplate>

                
            <Triggers>
                <asp:PostBackTrigger ControlID="BtnAddItem"  />
            </Triggers>
        
        </asp:UpdatePanel>--%>
                </asp:Panel>

                <%-- <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" CollapseControlID="pnlClick" Collapsed="true" ExpandControlID="pnlClick"
             TextLabelID="lblMessage" CollapsedText="+" ExpandedText="-" TargetControlID="panelAddLine" ScrollContents="false">

         </cc1:CollapsiblePanelExtender>--%>
            </div>

            <%-- <div style="overflow: auto; height: 260px; margin: 10px 0px 0px 10px;">--%>


            <div id="controlHead" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 98%"></div>
            <div style="height: 200px; overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px;">

                <%--<asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
                <asp:GridView runat="server" ID="gvDetails" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" Width="98%" BackColor="White"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                    AllowPaging="True" PageSize="20" OnRowDeleting="gvDetails_RowDeleting" OnRowEditing="gvDetails_RowEditing">

                    <AlternatingRowStyle BackColor="PaleTurquoise" />
                    <Columns>
                        <asp:TemplateField Visible="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenField2" Visible="false" runat="server" Value='<%# Eval("SNO") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />

                            <ItemStyle Width="0px" HorizontalAlign="Left"></ItemStyle>
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


                        <asp:BoundField HeaderText="Product Group" DataField="ProductGroup">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Sub Category" DataField="ProductSubCategory">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Product Description" DataField="ProductName">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <%--<asp:BoundField HeaderText="Product Name" DataField="ProductName" >
                <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>--%>
                        <asp:BoundField HeaderText="Qty[Box]" DataField="QtyBox">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Qty[Crates]" DataField="QtyCrates">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Qty[Ltr]" DataField="QtyLtr">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="UOM" DataField="UOM">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Price" DataField="Price" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Discount" DataField="Discount" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Tax%" DataField="Tax%" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="TaxAmount" DataField="TaxAmount" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="NetAmount" DataField="Value" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnDel" runat="server" Text="Delete" OnClick="lnkbtnDel_Click" ForeColor="Red"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

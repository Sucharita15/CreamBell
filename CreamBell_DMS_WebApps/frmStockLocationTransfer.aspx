<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmStockLocationTransfer.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmStockLocationTransfer" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdateProgress ID="upprogress" runat="server" AssociatedUpdatePanelID="upmain" DisplayAfter="0">
        <ProgressTemplate>
            <div class="overlay"></div>
            <div class="overlayContent">
                <center>
                    Please Wait...don't close this window until processing is being done.
                     <br />
                    <img src="../../IMAGES/bar.gif" alt="" />
                </center>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="upmain" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 100%; height: 18px; border-radius: 4px; background-color: #1564ad;">
                <span style="font-family: Segoe UI; color: white; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px;">Stock Location Transfer </span>
            </div>

            <div style="margin-top: 2px; text-align: left; height: 30px;">
                <asp:Button ID="BtnSaveStockTransfer" runat="server" Text="Move Stock" ToolTip="Click To Save the Stock Location Transfer"
                    CssClass="BlueButton" Width="140px" OnClick="BtnSaveStockTransfer_Click" />
                &nbsp;&nbsp;
        <asp:Button ID="BtnRefresh" runat="server" Text="Refresh" ToolTip="Click To Refresh Screen"
            CssClass="BlueButton" Width="80px" OnClick="BtnRefresh_Click" />
            </div>
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtDate" TargetControlID="txtDate" Format="yyyy-MM-dd" CssClass="CalendarCSS"></asp:CalendarExtender>
            <div style="margin-left: 1px; width: 100%">

                <asp:Panel ID="panelHeader" runat="server" GroupingText="Warehouse Selection Section">

                    <table style="width: 100%">
                        <tr>
                            <td style="width: 8%">Warehouse From :</td>
                            <td style="width: 25%">
                                <%--<asp:TextBox ID="txtStockMovementReferenceNo" runat="server"></asp:TextBox>--%>
                                <asp:DropDownList ID="DDLWarehouseFrom" runat="server" Width="98%" Font-Bold="true" ForeColor="MidnightBlue" AutoPostBack="true" OnSelectedIndexChanged="DDLWarehouseFrom_SelectedIndexChanged">
                                </asp:DropDownList>

                            </td>
                            <td style="width: 8%">Warehouse To :</td>
                            <td style="width: 25%">
                                <asp:DropDownList ID="DDLWarehouseTo" runat="server" Width="98%" Font-Bold="true" ForeColor="MidnightBlue">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 4%">Date :</td>
                            <td style="width: 15%">

                                <asp:TextBox ID="txtDate" runat="server" placeholder="Click Calendar Icon" Width="80%" Height="20px" Font-Bold="true" ForeColor="MidnightBlue"></asp:TextBox>
                                &nbsp;
                        <asp:ImageButton ID="imgBtDate" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                            </td>

                            <td style="width: 15%">
                                <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Names="Segoe UI" Font-Italic="true"></asp:Label>
                            </td>

                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="panelStockSelection" runat="server" GroupingText="Product Selection Section">
                    <table style="width: 100%; text-align: left">
                        <tr>
                            <td style="width: 100%" colspan="7">
                                <table style="width: 100%">
                                    <tr style="visibility: hidden;">
                                        <td style="color: darkblue; font-weight: bold; width: 15%; font-family: 'Segoe UI'">Type Product Code Here :</td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtProductCode" Visible="false" runat="server" Width="90%" Height="20px" Font-Bold="true" ForeColor="MidnightBlue">

                                            </asp:TextBox>
                                        </td>

                                        <td style="width: 10%">
                                            <asp:Button ID="BtnGetProductDetails" Visible="false" runat="server" Text="Get Details" BackColor="Aquamarine" OnClick="BtnGetProductDetails_Click" />
                                            <%--<asp:ImageButton ID="imgBtnSearch" ImageUrl="~/Images/searchicon.gif" ImageAlign="Bottom" runat="server" />--%>
                                        </td>
                                        <td style="width: 10%">
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtProductCode" MinimumPrefixLength="1" EnableCaching="true"
                                                CompletionSetCount="1" CompletionInterval="1000" ServiceMethod="GetProductDescription">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                        <td style="width: 60%">
                                            <asp:Label ID="LblProductMessage" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Names="Segoe UI" Font-Italic="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>

                            </td>

                        </tr>
                        <tr>
                            <td colspan="9">
                                <hr />
                            </td>

                        </tr>

                        <tr>
                            <td style="width: 10%">Business Unit</td>
                            <td style="width: 10%">Product Group</td>
                            <td style="width: 10%">Product Sub Category</td>
                            <td style="width: 22%">Product Description</td>
                            <td style="width: 10%">UOM</td>
                            <td style="width: 12%">Reason Type</td>
                            <td style="width: 10%">Move Qty(+)</td>
                            <td style="width: 10%">Stock Qty</td>
                            <td style="width: 5%"></td>
                        </tr>
                        <tr>
                            <td style="width:10%">
                                <asp:DropDownList ID="DDLBusinessUnit" runat="server" Width="95%" AutoPostBack="true" Font-Bold="true" ForeColor="MidnightBlue" OnSelectedIndexChanged="DDLBusinessUnit_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td style="width: 10%">
                                <asp:DropDownList ID="DDLProductGroup" runat="server" Width="98%" AutoPostBack="true" Font-Bold="true" ForeColor="MidnightBlue" OnSelectedIndexChanged="DDLProductGroup_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td style="width: 10%">
                                <asp:DropDownList ID="DDLProdSubCategory" runat="server" Width="98%" AutoPostBack="true" Font-Bold="true" ForeColor="MidnightBlue" OnSelectedIndexChanged="DDLProdSubCategory_SelectedIndexChanged">
                                </asp:DropDownList></td>
                            <td style="width: 22%">
                                <asp:DropDownList ID="DDLProductDesc" runat="server" Width="98%" AutoPostBack="true" Font-Bold="true" ForeColor="MidnightBlue" OnSelectedIndexChanged="DDLProductDesc_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 10%">
                                <asp:DropDownList ID="DDLEntryType" runat="server" Font-Bold="true" ForeColor="MidnightBlue" Width="98%">
                                    <asp:ListItem>Box</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 12%">
                                <asp:DropDownList ID="DDLReason" runat="server" Width="98%"></asp:DropDownList></td>

                            <td style="width: 10%">
                                <asp:TextBox ID="txtStockMoveQty" runat="server" Width="95%" AutoPostBack="true" Font-Bold="true" ForeColor="MidnightBlue" Font-Size="Medium">
                                </asp:TextBox>

                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ForeColor="Red" ControlToValidate="txtStockMoveQty" Display="Dynamic"
                                    ErrorMessage="*Invalid" Font-Size="10pt" ValidationExpression="((\d+)((\.\d{1,2})?))$">
                                </asp:RegularExpressionValidator>
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox ID="txtstock" runat="server" Width="95%" ReadOnly="true" Font-Bold="true" ForeColor="MidnightBlue" Font-Size="Medium">
                                </asp:TextBox>
                            </td>
                            <td style="width: 5%">
                                <asp:Button ID="BtnAdd" runat="server" Text="Add" ToolTip="Click To Add Entered Records.!" Width="40px"
                                    BackColor="Aquamarine" OnClick="BtnAdd_Click" /></td>
                        </tr>

                    </table>
                </asp:Panel>

                <br />

                <asp:Panel ID="PanelGrid" runat="server" GroupingText=" Stock Transfer Product Section" ScrollBars="Vertical">

                    <asp:GridView ID="gridStockTransferItems" runat="server" AutoGenerateColumns="False" Width="100%">

                        <Columns>
                            <asp:TemplateField Visible="false" ItemStyle-Width="0px" HeaderText="SNo">
                                <ItemTemplate>
                                    <asp:HiddenField ID="HiddenField2" Visible="false" runat="server" Value='<%# Eval("SNO") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />

                                <ItemStyle Width="0px" HorizontalAlign="Left"></ItemStyle>
                            </asp:TemplateField>

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
                            <asp:BoundField HeaderText="Reason" DataField="Reason">
                                <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Qty" DataField="MoveQty">
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


            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="DDLProductDesc" />
            <asp:PostBackTrigger ControlID="DDLProdSubCategory" />
            <asp:PostBackTrigger ControlID="DDLProductGroup" />
            <asp:PostBackTrigger ControlID="DDLBusinessUnit" />
            <asp:PostBackTrigger ControlID="txtStockMoveQty" />
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>

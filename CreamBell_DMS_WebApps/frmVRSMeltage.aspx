<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmVRSMeltage.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmVRSMeltage" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />
    <link href="css/SSheet.css" rel="Stylesheet" type="text/css" />
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
        .ddl {
            background-image: url('Images/arrow-down-icon-black.png');
        }

            .ddl:hover {
                background-image: url('Images/arrow-down-icon-black.png');
            }
    </style>
    <style type="text/css">
        /*DropDownCss*/
        .pnlCSS {
            font-weight: bold;
            cursor: pointer;
            /*border: solid 1px #c0c0c0;*/
            margin-left: 20px;
        }

        .CalendarCss {
            background-color: azure;
            color: darkblue;
        }

        .button {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <div style="width: 98%; height: 18px; border-radius: 4px; margin: 6px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; color: white; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px;">Vending Meltage Expense </span>
        <asp:Label ID="Label1" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Names="Segoe UI" Font-Italic="true"></asp:Label>
    </div>
    <div style="width: 100%; margin-left: 4px">
        <asp:Panel ID="panelHeader" runat="server" GroupingText="Meltage Expense" Width="98%">
            <asp:UpdatePanel ID="sdsd" runat="server">
                <ContentTemplate>
                    <table style="width: 98%; border-spacing: 0px">
                        <tr>
                            <td style="width:10%;">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" Height="24px" OnClick="btnSave_Click" Width="98%" />
                            </td>
                            <td style="width:10%;">
                                <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="button" Height="24px" Width="98%" OnClick="btnRefresh_Click" />
                            </td>
                            <td style="width:30%;"></td>
                            <td>
                                <asp:TextBox ID="txtClaimAllowedPer" ReadOnly="true" Visible="false" runat="server" CssClass="textboxStyleNew" Height="13px" Width="70px" />
                            </td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txtClaimReceived" Visible="false" ReadOnly="true" runat="server" CssClass="textboxStyleNew" Height="13px" Width="70px" />
                            </td>
                            <td class="auto-style2"></td>
                            <td>
                                <asp:TextBox ID="txtReceiptValue" ReadOnly="true" runat="server" CssClass="textboxStyleNew" Visible="false" Height="13px" Width="100px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Select VRS
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlVrs" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlVrs_SelectedIndexChanged" CssClass="dropdownField" Width="98%"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table style="width: 98%; border-spacing: 0px">
                        <tr>
                            <td style="width: 100%; text-align: left">

                                <div id="panelAdd" style="margin-top: 0px; width: 100%;">
                                    <asp:Panel ID="panelAddLine" runat="server" Width="100%">
                                        <table style="width: 100%;" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td style="width: 30%;">Product<asp:Label ID="lblHidden" runat="server" Visible="False"></asp:Label></td>
                                                <td style="width: 5%;">Box</td>
                                                <td style="width: 5%;">Pcs</td>
                                                
                                                <td style="width: 20%;">Batch No</td>
                                                <td style="width: 5%;">MFD</td>
                                                <td style="width: 15%;">Remark</td>
                                                <td style="width: 5%;">Price</td>
                                                <td style="width: 5%;">ScrapValue</td>
                                                <td style="width: 5%;"></td>
                                            </tr>
                                            <tr>
                                                <td style="margin-left: 40px">
                                                    <asp:DropDownList ID="DDLMaterialCode" runat="server" CssClass="dropdownField" OnSelectedIndexChanged="DDLMaterialCode_SelectedIndexChanged" AutoPostBack="true" Width="100%" />
                                                </td>
                                                <td style="padding:1px 1px 1px 1px;">
                                                    <asp:TextBox ID="txtBoxQty" runat="server" AutoPostBack="true" onkeypress="return IsNumeric(event)" Width="90%" CssClass="textfield" OnTextChanged="txtBoxQty_TextChanged" />
                                                </td>
                                                <td style="padding:1px 1px 1px 1px;">
                                                    <asp:TextBox ID="txtPcsQty" runat="server" AutoPostBack="true" onkeypress="return IsNumeric(event)" Width="90%" CssClass="textfield" OnTextChanged="txtPcsQty_TextChanged" ></asp:TextBox>
                                                    <asp:TextBox ID="txtUsedQty" Visible="false" runat="server" ReadOnly="True" Width="90%" CssClass="textfield" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                                                </td>
                                                
                                                <td style="padding:1px 1px 1px 1px;">
                                                    <asp:TextBox ID="txtBatch" runat="server" AutoPostBack="true" onkeypress="return IsNumeric(event)" Width="95%" CssClass="textfield" /></td>
                                                <td style="padding:1px 1px 1px 1px;">
                                                    <asp:TextBox ID="txtMDF" runat="server" CssClass="textfield" Width="100px" />
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtMDF">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td style="padding:1px 1px 1px 1px;">
                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="textfield" Width="98%" /></td>
                                                <td style="padding:1px 1px 1px 1px;">
                                                    <asp:TextBox ID="txtPrice" runat="server" ReadOnly="True" Width="90%" CssClass="textfield" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                                                </td>
                                                <td style="padding:1px 1px 1px 1px;">
                                                    <asp:TextBox ID="txtValue" runat="server" ReadOnly="True" Width="90%" CssClass="textfield" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                                                    <asp:TextBox ID="txtActualClaim" Visible="false" runat="server" ReadOnly="True" Width="90%" CssClass="textfield" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                                                </td>
                                                <td style="padding:1px 1px 1px 1px;">
                                                    <asp:Button ID="BtnAddItem" runat="server" Text="Add" CssClass="button" BackColor="#0066CC" ForeColor="White" OnClick="BtnAddItem_Click" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPackSize" runat="server" Visible="false" Width="0px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtClaimCalculation" runat="server" Visible="false" Width="0px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <caption>
                                            </caption>
                                        </table>
                                    </asp:Panel>
                                </div>
                            </td>
                        </tr>

                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="DDLMaterialCode" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="txtBoxQty" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="txtPcsQty" EventName="TextChanged" />

                </Triggers>
            </asp:UpdatePanel>

        </asp:Panel>
    </div>
    <div style="height: 200px; overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; text-align: left">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:GridView runat="server" ID="gvDetails" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                    CellPadding="3" ShowHeaderWhenEmpty="True">
                    <AlternatingRowStyle BackColor="#CCFFCC" />
                    <Columns>
                        <asp:TemplateField HeaderText="SNo">
                            <ItemTemplate>
                                <span><%#Container.DataItemIndex + 1%></span>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ProductCode" HeaderText="Product Code">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PRODUCTName" HeaderText="Product Name">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyBox" DataFormatString="{0:n2}" HeaderText="Box">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyPcs" DataFormatString="{0:n2}" HeaderText="Pcs" Visible="false">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="BatchNo" HeaderText="BatchNo">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MFD" HeaderText="MFD" DataFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="REMARK" HeaderText="REMARK">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Price" DataFormatString="{0:n2}" HeaderText="Price">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Value" DataFormatString="{0:n2}" HeaderText="ActualClaim">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnDel" runat="server" Text='Delete' OnClick="lnkbtnDel_Click" ForeColor="Black"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>

                    <HeaderStyle BackColor="#05345C" Font-Bold="True" ForeColor="#F7F7F7" HorizontalAlign="Left" />
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
</asp:Content>

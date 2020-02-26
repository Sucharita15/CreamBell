<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmStockAdjustment.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmStockAdjustment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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

        function uploadStart(sender, args) {
            var fileName = args.get_fileName();
            var fileExt = fileName.substring(fileName.lastIndexOf(".") + 1);

            if (fileExt == "xls" || fileExt == "xlsx") {
                return true;
            } else {
                //To cancel the upload, throw an error, it will fire OnClientUploadError
                var err = new Error();
                err.name = "Upload Error";
                err.message = "Please upload only Excel files (.xls ,.xlsx)";
                throw (err);

                return false;
            }
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

    <asp:UpdatePanel ID="mainup" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="imgBtnExportToExcel" />
            <asp:AsyncPostBackTrigger ControlID="AsyncFileUpload1" />
            <asp:PostBackTrigger ControlID="rdoExcelEntry" />
            <asp:PostBackTrigger ControlID="rdoManualEntry" />
            <asp:PostBackTrigger ControlID="btnUpload" />
            <asp:PostBackTrigger ControlID="BtnSaveAdjustment" />
        </Triggers>
        <ContentTemplate>

            <div style="width: 100%; height: 20px; border-radius: 4px; background-color: #1564ad; padding: 0px 0px 0px 0px;">
                <span style="font-family: Segoe UI; color: white; font-size: 11px; font-weight: bold;">Stock Adjustment </span>
            </div>

            <div style="margin-top:2px; text-align: left; height: 30px; width:100%">
               
                 <table> <tr>  

               <td> <asp:Button ID="BtnSaveAdjustment" runat="server" Text="Save " ToolTip="Click To Save the Stock Adjustment"
                    CssClass="BlueButton" Width="90px" OnClick="BtnSaveAdjustment_Click1" /></td>

               <td> <asp:Button ID="BtnRefresh" runat="server" Text="Refresh" ToolTip="Click To Refresh Screen"
                    CssClass="BlueButton" Width="80px" OnClick="BtnRefresh_Click" /></td>

                <td><asp:RadioButton ID="rdoManualEntry" runat="server" AutoPostBack="true" Text="Manual Entry" Checked="true"
                    OnCheckedChanged="rdoManualEntry_CheckedChanged" GroupName="radio" /></td>

               <td> <asp:RadioButton ID="rdoExcelEntry" runat="server" AutoPostBack="true" Text="Excel Upload" 
                    OnCheckedChanged="rdoManualEntry_CheckedChanged" GroupName="radio" /></td>

               <td> <asp:ImageButton ID="imgBtnExportToExcel" runat="server" ImageUrl="~/Images/excel-24.ico" OnClick="ImDnldTemp_Click" ToolTip="Click To Download Excel" />
                </td>
                     <td>
                <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" Visible="false" OnClientUploadStarted="uploadStart" Height="18px" /></td>
                     <td>
                
                <asp:Button ID="btnUpload" runat="server"  Text="Upload" AutoPostBack="true" OnClick="btnUpload_Click" CssClass="ReportSearch" Visible="False" /></td>

                     </tr></table>
            </div>    
            <div id="Filter" style="width: 98%; height: 40px; border-radius: 1px; margin: 5px 0px 0px 5px; color: black; padding: 2px 0px 0px 0px; border-style: groove">

                <table>
                    <tr>
                        <td>

                            <asp:Label ID="Label1" runat="server" Visible="false" Text="" Font-Bold="true" ForeColor="DarkBlue" Font-Names="Seoge UI"> </asp:Label>
                            <asp:Label ID="Label2" runat="server" Text="" Visible="false" Font-Bold="true" ForeColor="DarkBlue" Font-Names="Seoge UI"> </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; text-align: left; width: 5%;">State </td>
                        <td style="vertical-align: top; text-align: left; width: 15%;">
                            <asp:DropDownList ID="ddlstate" runat="server" Width="98%" AutoPostBack="True" OnSelectedIndexChanged="ddlstate_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="vertical-align: top; text-align: left; width: 5%;">Distributor </td>
                        <td style="vertical-align: top; text-align: left; width: 22%;">
                            <asp:DropDownList ID="ddldistributor" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddldistributor_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                      </tr>
                   
               </table>
            </div>
            
            <div style="margin-left: 1px; width: 98%">

                <asp:Panel ID="panelHeader" runat="server" GroupingText="Search Section">

                    <table style="width: 99%; text-align: left">
                        <tr>

                            <td>Reference No:</td>
                            <td>
                                <asp:TextBox ID="txtAdjustmentRefNo" runat="server"></asp:TextBox></td>
                            <td>Search :</td>
                            <td>
                                <asp:DropDownList ID="DDlSearchType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDlSearchType_SelectedIndexChanged">
                                    <asp:ListItem>Adjustment No</asp:ListItem>
                                </asp:DropDownList>
                            </td>

                            <td>
                                <asp:TextBox ID="txtSearchAdjustmentNo" runat="server" placeholder="Type Adjustment No"></asp:TextBox>
                            </td>

                            <td>
                                <asp:ImageButton ID="imgBtDate" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                            </td>
                            <td>
                                <asp:Button ID="BtnSearch" runat="server" Text="Search" Width="60px" OnClick="BtnSearch_Click" /></td>
                            <td>
                                <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkBlue" Font-Names="Segoe UI"></asp:Label>
                            </td>

                            <td>WareHouse :
                            </td>
                            <td>
                                <asp:Label ID="LblWarehouse" runat="server" Text="" Font-Bold="true" ForeColor="DarkBlue" Font-Names="Segoe UI"></asp:Label>
                            </td>

                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="panelAdjustmentEntry" runat="server" GroupingText="Adjustment Entry Section">
                    <table style="width: 99%; text-align: left">
                        <tr>
                            <td style="width: 11%">Business Unit</td>
                            <td style="width: 12%">Product Group</td>
                            <td style="width: 12%">Product Sub Category</td>
                            <td style="width: 22%">Product Description</td>
                            <td style="width: 8%">Reason Type</td>
                            <td style="width: 6%">Box Qty</td>
                            <td style="width: 6%">Pcs Qty</td>
                            <td style="width: 6%">Box/Pcs</td>
                            <td style="width: 10%">Adjustment Qty (+/-)</td>
                            <td style="width: 5%"></td>
                        </tr>
                        <tr>
                            <td style="width: 13%">
                                <asp:DropDownList ID="DDLBusinessUnit" runat="server" Width="95%" AutoPostBack="true" OnSelectedIndexChanged="DDLBusinessUnit_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td style="width: 12%">
                                <asp:DropDownList ID="DDLProductGroup" runat="server" Width="95%" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDLProductGroup_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 12%">
                                <asp:DropDownList ID="DDLProdSubCategory" runat="server" Width="95%" AutoPostBack="true" OnSelectedIndexChanged="DDLProdSubCategory_SelectedIndexChanged">
                                </asp:DropDownList></td>
                            <td style="width: 18%">
                                <asp:DropDownList ID="DDLProductDesc" runat="server" Width="95%" AutoPostBack="true" OnSelectedIndexChanged="DDLProductDesc_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdfPacksize" runat="server" />
                            </td>
                            <td style="width: 10%">
                                <asp:DropDownList ID="DDLReason" runat="server" Width="95%"></asp:DropDownList></td>
                            <td style="width: 6%">
                                <asp:TextBox ID="txtBoxQty" runat="server" Width="60px" onkeypress="IsNumeric(event)" AutoPostBack="true" OnTextChanged="txtBoxQty_TextChanged">
                                </asp:TextBox>
                            </td>
                            <td style="width: 6%">
                                <asp:TextBox ID="txtPcsQty" runat="server" Width="60px" onkeypress="IsNumeric(event)" AutoPostBack="true" OnTextChanged="txtBoxQty_TextChanged">
                                </asp:TextBox>
                            </td>
                            <td style="width: 6%">
                                <asp:TextBox ID="txtBoxPcs" runat="server" Width="60px" onkeypress="CheckNumeric(event)" ReadOnly="true" Enabled="false">
                                </asp:TextBox>
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox ID="txtAdjValue" runat="server" Width="60px" ReadOnly="true" Enabled="false">
                                </asp:TextBox>
                            </td>
                            <td style="width: 5%">
                                <asp:Button ID="BtnAdd" runat="server" Text="Add" ToolTip="Click To Add Entered Records.!" Width="40px" OnClick="BtnAdd_Click" /></td>
                        </tr>

                    </table>

                </asp:Panel>

                <br />

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
                            <asp:BoundField HeaderText="Box Qty" DataField="BoxQty">
                                <HeaderStyle Width="80px" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="PCs Qty" DataField="PcsQty">
                                <HeaderStyle Width="80px" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Box/Pcs" DataField="BoxPcs">
                                <HeaderStyle Width="80px" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Adj Value" DataField="AdjustmentValue">
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
             <div>
                <asp:Button ID="Button2" runat="Server" Text="" Style="display: none;" />
                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button2"
                    PopupControlID="Panel1" CancelControlID="Button4"
                    BackgroundCssClass="ModalPoupBackgroundCssClass" BehaviorID="ModalPopupExtender1">
                </asp:ModalPopupExtender>

                <asp:Panel ID="Panel1" runat="server" Style="display: none; background-color: silver">
                    <div>
                        <span style="color: red; font-weight: 600; text-align: center">Records which are not uploaded !!</span>

                        <asp:Button ID="Button4" runat="server" CssClass="Operationbutton" data-dismiss="modal" Text="Close" aria-hidden="true"></asp:Button>
                    </div>
                    <p></p>
                    <div style="overflow-x: scroll; width: 98%; height: 200px">
                        <asp:GridView ID="gridviewRecordNotExist" runat="server" Style="width: 100%" Font-Size="Small" CssClass="Grid" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
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


        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

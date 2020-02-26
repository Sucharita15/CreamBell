<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmFOCPurchaseInvoice.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmFOCPurchaseInvoice" %>

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
    </style>

    <style type="text/css">
        .prettyFile > input {
            display: none !important;
        }
        /*  The rest is from Twitter Bootstrap */
        input,
        .input-append {
            display: inline-block;
            vertica-align: middle;
        }

        .inputupload {
            border: 1px solid rgba(82, 168, 236, 0.8);
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075), 0 0 8px rgba(82, 168, 236, .6);
            border-radius: 3px 0 0 3px;
            font-size: 14px;
            height: 20px;
            color: #555;
            padding: 4px 6px;
            margin-right: -4px;
            width: 360px;
            height: 33px;
        }

        .btnupload {
            background-image: -webkit-linear-gradient(top, white, #E6E6E6);
            background-repeat: repeat-x;
            border: 1px solid rgba(0, 0, 0, 0.14902);
            box-shadow: rgba(255, 255, 255, 0.2) 0px 1px 0px 0px inset, rgba(0, 0, 0, 0.0470588) 0px 1px 2px 0px;
            color: #333;
            display: inline-block;
            font-family: Tahoma, sans-serif;
            font-size: 14px;
            margin: 0 0 0 -1px;
            padding: 4px 14px;
            height: 20px;
            line-height: 20px;
            text-align: center;
            text-decoration: none;
            text-shadow: rgba(255, 255, 255, 0.74902) 0px 1px 1px;
            vertical-align: top;
            height: 33px;
            width: 81px;
            border: 2px solid #74B9EF;
        }


        /*DropDownCss*/
        .ddl {
            background-image: url('Images/arrow-down-icon-black.png');
        }

            .ddl:hover {
                background-image: url('Images/arrow-down-icon-black.png');
            }
    </style>



    <script type="text/javascript">

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=GridFOCPurchItems.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=GridFOCPurchItems.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead").append(gridHeader);
            $('#controlHead').css('position', 'absolute');
            $('#controlHead').css('top', '129');

        });


    </script>

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

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31
              && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <asp:UpdatePanel ID="sdasd" runat="server">
        <ContentTemplate>


            <div>
                <asp:Panel ID="pnlexcelupload" runat="server">
                    <table style="width: 100%; text-align: left">
                        <tr>
                            <td style="width: 5%">
                                <asp:Button ID="btnPostPurchaseInvoice" runat="server" Text="Post" CssClass="ReportSearch" OnClick="btnPostPurchaseInvoice_Click" />
                            </td>
                            <td style="width: 5%">
                                <asp:Button ID="BtnRefresh" runat="server" Text="Refresh" CssClass="ReportSearch" OnClick="BtnRefresh_Click" />
                            </td>


                            <td style="width: 5%">
                                <asp:TextBox ID="txtPurchDocumentNo" runat="server" Visible="False"></asp:TextBox>
                            </td>
                            <td style="width: 25%">
                                <asp:RadioButton ID="rdoManualEntry" runat="server" AutoPostBack="true" Text="Manual Entry" Checked="true" GroupName="radio" OnCheckedChanged="rdoManualEntry_CheckedChanged" />
                                &nbsp;
                    <asp:RadioButton ID="rdoExcelEntry" runat="server" AutoPostBack="true" Text="Excel Upload" GroupName="radio" OnCheckedChanged="rdoManualEntry_CheckedChanged" />
                            </td>
                            <td style="width: 20%">
                                  <asp:UpdatePanel runat="server" ID="UploadPanel">
                                    <ContentTemplate>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 10%;">
                                            <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" Height="18px" Visible="False" OnClientUploadStarted="uploadStart" />

                                        </td>
                                        <td style="width: 5%;">
                                            <asp:Button ID="btnUplaod" runat="server" Text="Upload" BackColor="#0066CC" ForeColor="White" Visible="False" OnClick="btnUplaod_Click" />
                                        </td>
                                    </tr>
                                </table>
                                 </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUplaod" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>

                            <td style="width: 10%; text-align: left">
                                <asp:HyperLink ID="hypISPUpload" runat="server" Font-Size="Small" ForeColor="Blue" Style="margin-left: 0px" ToolTip="Click to download excel template !!">
                                 <a href="ExcelTemplate/FOCPurchaseInvoice.xlsx" target="_blank">
                                 <img src="Images/DownloadTemplate.gif" alt="Download Template" style="border-style: none"  /></a> </asp:HyperLink>
                            </td>
                            <td style="width: 30%">
                                <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Names="Segoe UI" Font-Italic="true"></asp:Label>
                            </td>

                        </tr>
                    </table>
                </asp:Panel>
            </div>

            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
                <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; color: white; margin: 6px 0px 0px 10px;">FOC Purchase Invoice Reciept</span>
            </div>

            <div class="form-style-6" style="margin-left: -15px; width: 100%; height: 116px; margin-top: -10px">

                <asp:Panel ID="panelHeader" runat="server" GroupingText="Purchase Invoice Header Section" Width="100%">

                    <table style="width: 100%; text-align: left">
                        <tr>
                            <td style="width: 10%">Indent No</td>
                            <td style="width: 35%">
                                <asp:DropDownList ID="DrpIndentNo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DrpIndentNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>

                            <td style="width: 10%">
                                <asp:TextBox ID="txtIndentNo" runat="server" Visible="False"></asp:TextBox>

                            </td>
                            <td style="width: 10%">Invoice No</td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="10" />
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="**" ControlToValidate="txtInvoiceNo" ForeColor="#CC3300"></asp:RequiredFieldValidator>--%>
                            </td>

                        </tr>

                        <tr>
                            <td class="auto-style19">Indent Date</td>
                            <td class="auto-style23">
                                <asp:TextBox ID="txtIndentDate" runat="server"></asp:TextBox></td>

                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtIndentDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                            <td class="auto-style21">&nbsp;</td>
                            <td class="auto-style8">Invoice Date</td>
                            <td class="auto-style12">
                                <asp:TextBox ID="txtInvoiceDate" runat="server" /></td>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtInvoiceDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>

                        </tr>
                        <tr>
                            <td class="auto-style19">Transporter Code</td>
                            <td class="auto-style23">
                                <asp:TextBox ID="txtTransporterName" runat="server" MaxLength="10" /></td>
                            <td class="auto-style21">&nbsp;</td>
                            <td class="auto-style8">Driver Number</td>
                            <td class="auto-style12">
                                <asp:TextBox ID="txttransporterNo" runat="server" MaxLength="20" /></td>

                        </tr>

                        <tr>
                            <td class="auto-style19">Vehicle Number</td>
                            <td class="auto-style23">
                                <asp:TextBox ID="txtvehicleNo" runat="server" MaxLength="20" /></td>
                            <td class="auto-style21">&nbsp;</td>
                            <td class="auto-style8">Vehicle Type</td>
                            <td class="auto-style12">
                                <asp:TextBox ID="txtVehicleType" runat="server" MaxLength="15" /></td>

                        </tr>
                        <tr>
                            <td class="auto-style19">RECEIPT DATE :
                            </td>
                            <td class="auto-style23">
                                <asp:TextBox ID="txtReceiptDate" runat="server" />
                            </td>
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtReceiptDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                            <td class="auto-style21">&nbsp;</td>
                            <td>RECEIPT VALUE (₹) :
                            </td>
                            <td>
                                <asp:TextBox ID="txtReceiptValue" runat="server" onkeypress="return isNumberKey(event)" Enabled="false" Text="0" />
                            </td>
                        </tr>

                    </table>

                </asp:Panel>

                <div id="UpdateHeader" style="margin-left: 869px; margin-top: -129px; width: 136px;">
                    <asp:Button ID="BtnUpdateHeader" runat="server" Text="Update Header" OnClick="BtnUpdateHeader_Click" />
                </div>

            </div>

            <asp:Panel ID="panelAddLine" runat="server" CssClass="pnlCSS" Width="100%" GroupingText="Product Section">

                <table style="width: 100%; text-align: left">
                    <tr>
                        <td style="width: 100%" colspan="15">
                            <table style="width: 100%">
                                <tr>
                                    <td style="color: darkblue; font-weight: bold; width: 10%; font-family: 'Segoe UI'">Product Code </td>
                                    <td style="width: 20%">
                                        <asp:TextBox ID="txtProductCode" runat="server" Width="90%" Font-Bold="true" ForeColor="MidnightBlue" Style="margin-right: 0px">
                                        </asp:TextBox></td>
                                    <td style="width: 70%" colspan="9">
                                        <asp:Button ID="BtnGetProductDetails" runat="server" Text="Get Details" BackColor="Aquamarine" Width="86px" OnClick="BtnGetProductDetails_Click" />
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtProductCode" MinimumPrefixLength="1" EnableCaching="true"
                                            CompletionSetCount="1" CompletionInterval="1000" ServiceMethod="GetProductDescription">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                            </table>
                        </td>

                    </tr>
                    <tr>
                        <td style="width: 10%">Product Details
                           
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox ID="txtProductDesc" runat="server" Width="90%" Font-Bold="false" ForeColor="MidnightBlue"
                                placeholder="Product Code/Product Name" Font-Size="Smaller" Enabled="False" />
                        </td>

                        <td style="width: 5%">MRP(₹)
                        </td>

                        <td style="width: 5%">
                            <asp:TextBox ID="txtMRP" runat="server" Width="60px" Height="15px" Font-Bold="true" ForeColor="MidnightBlue" placeholder="0" Enabled="False" />
                        </td>

                        <td style="width: 5%">Weight[Kg]
                        </td>
                        <td style="width: 5%">
                            <asp:TextBox ID="txtWeight" runat="server" Width="60px" Height="15px" Font-Bold="true" ForeColor="MidnightBlue"
                                placeholder="0Kg" Enabled="false" Font-Size="Smaller" />
                        </td>

                        <td style="width: 5%">Volume[Ltr]
                        </td>
                        <td style="width: 5%">
                            <asp:TextBox ID="txtVolume" runat="server" Width="60px" Height="15px" Font-Bold="true" ForeColor="MidnightBlue" placeholder="0Ltr"
                                Enabled="false" Font-Size="Smaller" />
                        </td>

                        <td style="width: 5%">Entry Type
                        </td>
                        <td style="width: 5%">
                            <asp:DropDownList ID="DDLEntryType" runat="server" AutoPostBack="true" CssClass="ddlFont" Font-Names="Arial" Font-Size="Small" Height="18px"
                                Width="100px">

                                <asp:ListItem>Box</asp:ListItem>

                            </asp:DropDownList>
                        </td>
                        <td style="width: 5%">
                            <asp:Literal ID="LTEntryType" runat="server">Box Qty</asp:Literal>
                        </td>
                        <td style="width: 5%">
                            <asp:TextBox ID="txtEntryValue" runat="server" AutoPostBack="true" onkeypress="CheckNumeric(event);"
                                placeholder="0" TabIndex="1" Width="60px" OnTextChanged="txtEntryValue_TextChanged" />
                        </td>
                        <td style="width: 10%">
                            <asp:Button ID="BtnSavePreData" runat="server" CssClass="ReportSearch" Height="28px" Text="Save" Width="75px" OnClick="BtnSavePreData_Click" />
                        </td>
                    </tr>


                </table>
            </asp:Panel>
            <div id="controlHead" style="margin-top: 5px; margin-left: 5px; padding-right: 10px;"></div>
            <div style="height: 250px; overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px;">

                <asp:GridView runat="server" ID="GridFOCPurchItems" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                    CellPadding="3" AutoGenerateDeleteButton="True" OnRowDeleting="GridFOCPurchItems_RowDeleting">

                    <AlternatingRowStyle BackColor="#CCFFCC" />
                    <Columns>
                        <asp:TemplateField HeaderText="SNo">
                            <ItemTemplate>
                                <span><%#Container.DataItemIndex + 1%></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenValueLineNo" Visible="false" runat="server" Value='<%# Eval("LINE_NO") %>' />
                            </ItemTemplate>

                        </asp:TemplateField>
                    </Columns>


                    <Columns>
                        <asp:BoundField HeaderText="Code/Product Name" DataField="PRODUCTDESC" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="MRP" DataField="PRODUCT_MRP" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Box" DataField="BOX" DataFormatString="{0:n}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Crates" DataField="CRATES" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="UT" DataField="UOM" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Volume[Ltr]" DataField="LTR" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Rate" DataField="RATE" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Value" DataField="BASICVALUE" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="TRD%" DataField="TRDDISCPERC" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="TRDValue" DataField="TRDDISCVALUE" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Price Equal." DataField="PRICE_EQUALVALUE" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="VATInc%" DataField="VAT_INC_PERC" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="VATIncValue" DataField="VAT_INC_PERCVALUE" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="GrossRate" DataField="GROSSRATE" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="NetValue" DataField="AMOUNT" DataFormatString="{0:n2}" />
                    </Columns>


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

            <div style="width: 503px">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>

                        <asp:Button ID="Button2" runat="Server" Text="" Style="display: none;" />
                        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button2"
                            PopupControlID="Panel1" CancelControlID="Button4"
                            BackgroundCssClass="ModalPoupBackgroundCssClass" BehaviorID="ModalPopupExtender1">
                        </asp:ModalPopupExtender>

                        <asp:Panel ID="Panel1" runat="server" Style="display: none; background-color: silver" Width="503px">
                            <div align="center">
                                <span style="color: red; font-weight: 600">Unploaded Product List

                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 

                                </span>
                                <asp:Button ID="Button4" runat="server" CssClass="Operationbutton" data-dismiss="modal" Text="Close" aria-hidden="true"></asp:Button>

                            </div>

                            <hr />


                            <div style="overflow-x: scroll; width: 500px; height: 200px">
                                <asp:GridView ID="gridviewRecordNotExist" runat="server" Style="width: 100%" Font-Size="Small" CssClass="Grid"
                                    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" BackColor="White" BorderColor="#CCCCCC"
                                    BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                    <AlternatingRowStyle CssClass="alt" />
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                    <PagerStyle BackColor="White" CssClass="pgr" ForeColor="#000066" HorizontalAlign="Center" />

                                    <RowStyle ForeColor="#000066" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

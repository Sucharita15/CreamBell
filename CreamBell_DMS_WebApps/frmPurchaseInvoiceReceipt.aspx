<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPurchaseInvoiceReceipt.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPurchaseInvoiceReceipt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />
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

    <script type="text/javascript">
        function Refresh()
        {
           // $find('ModalPopupExtender1').show();
            document.getElementById("<%=Btnclick.ClientID %>").click();
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
        .prettyFile > input {
            display: none !important;
        }
        /*  The rest is from Twitter Bootstrap */
        input,
        .input-append {
            display: inline-block;
            vertical-align: middle;
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

        .auto-style5 {
            width: 10%;
            height: 18px;
            margin-left: 40px;
        }

        .auto-style8 {
            height: 27px;
        }

        .auto-style9 {
            height: 38px;
        }

        .auto-style10 {
            width: 70px;
        }

        .auto-style11 {
            height: 27px;
            width: 70px;
        }

        .auto-style12 {
            color: red;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=GridPurchItems.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=GridPurchItems.ClientID%> tr th').each(function (i) {
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
    <div style="width: 99%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; color: white; margin: 6px 0px 0px 10px;">Purchase Invoice Reciept</span>
    </div>
    <div>
        <asp:Panel ID="pnlexcelupload" runat="server">
            <table style="width: 100%">
                <tr>
                    <td style="width: 100%">
                        <table style="width: 100%">
                            <tr>
                                <td class="auto-style5">
                                    <asp:Button ID="btnPostPurchaseInvoice" runat="server" Text="Post" CssClass="button" Height="31px" OnClick="btnPostPurchaseInvoice_Click" />
                                    &nbsp;
                                    <asp:Button ID="BtnRefresh" runat="server" CssClass="button" Height="31px" OnClick="BtnRefresh_Click" Text="Refresh" />
                                </td>
                                <td class="auto-style5">
                                    <asp:RadioButton ID="rdoManualEntry" runat="server" AutoPostBack="true" Text="Manual Entry" Visible="false"
                                        OnCheckedChanged="rdoManualEntry_CheckedChanged" GroupName="radio" />
                                </td>
                                <td class="auto-style5">
                                    <asp:RadioButton ID="rdoExcelEntry" runat="server" AutoPostBack="true" Text="Excel Upload" Visible="false"
                                        OnCheckedChanged="rdoManualEntry_CheckedChanged" GroupName="radio" />
                                </td>
                                <td class="auto-style5">
                                    <asp:RadioButton ID="rdoSAPFetchData" runat="server" AutoPostBack="true" Text="SAP" Checked="true" Visible="false"
                                        OnCheckedChanged="rdoManualEntry_CheckedChanged" GroupName="radio" />
                                </td>
                                <td colspan="2" style="width: 15%">
                                    <asp:UpdatePanel runat="server" ID="UploadPanel">
                                        <ContentTemplate>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 10%;">
                                                        <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" Visible="false" OnClientUploadStarted="uploadStart" Height="18px" />
                                                    </td>
                                                    <td style="width: 5%;">
                                                        <asp:Button ID="btnUplaod" runat="server" Text="Upload" OnClick="btnUpload_Click" CssClass="ReportSearch" Visible="False" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnUplaod" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>

                                <td class="auto-style5">
                                    <asp:HyperLink ID="hypISPUpload" runat="server" Font-Size="8pt" ForeColor="Blue" Style="margin-left: 0px" ToolTip="Click to download excel template !!">
                                        <a href="ExcelTemplate/PurchInvoiceReceipt.xlsx" target="_blank">
                                            <img src="Images/DownloadTemplate.gif" alt="Download Template" style="border-style: none" /></a></asp:HyperLink></td>
                                <td class="auto-style5">
                                    <asp:TextBox ID="txtPurchDocumentNo" runat="server" Visible="False"></asp:TextBox>
                                    <asp:Button ID="BtnUpdateHeader" runat="server" Text="Update Header" OnClick="BtnUpdateHeader_Click" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8" style="text-align: center">
                                    <asp:UpdatePanel ID="lblmsgUpdate" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="LblMessage" runat="server" Font-Bold="True" Font-Italic="True" Font-Names="Segoe UI" ForeColor="Red"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <asp:UpdatePanel ID="HeaderSection" runat="server">
        <ContentTemplate>
            <div>
                <asp:Panel ID="panelHeader" runat="server" GroupingText="Purchase Invoice Header Section" Width="99%">
                    <table style="width: 99%; border-spacing: 0px">
                        <tr>
                            <td>
                                <asp:Label ID="lblIndno" runat="server" Text="Indent No"></asp:Label></td>
                            <td style="margin-left: 40px">
                                <asp:DropDownList ID="DrpIndentNo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DrpIndentNo_SelectedIndexChanged" CssClass="textboxStyleNew" Width="177px">
                                </asp:DropDownList>
                                <asp:DropDownList ID="drpGstInvoice" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpGstInvoice_SelectedIndexChanged" Visible="false" Width="177px">
                                </asp:DropDownList>
                            </td>

                            <td>
                                <asp:TextBox ID="txtIndentNo" runat="server" Visible="False"></asp:TextBox>
                                <asp:TextBox ID="txtGstinvoice" runat="server" Visible="False"></asp:TextBox>
                            </td>
                            <td><span class="auto-style12"><strong>*</strong></span> Invoice No</td>
                            <td>
                                <asp:DropDownList ID="DrpSalesInvoice" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DrpSalesInvoice_SelectedIndexChanged"  CssClass="textboxStyleNew" Width="177px">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="10" Width="100px" CssClass="textboxStyleNew"
                                    Height="13px" Visible="false" />
                                &nbsp;<asp:ImageButton ID="imgBtnGetInvoiceData" runat="server" ImageUrl="~/Images/arrow.png" ImageAlign="Bottom"
                                    ToolTip="Click To Get Data From SAP Invoice !!" OnClick="imgBtnGetInvoiceData_Click" Visible="false" />
                            </td>

                            <td><span class="auto-style12"><strong>*</strong></span> Order Type</td>
                            <td>
                                <asp:DropDownList ID="drpOrderType" runat="server" AutoPostBack="true" Width="177px" CssClass="textboxStyleNew" TabIndex="4" OnSelectedIndexChanged="drpOrderType_SelectedIndexChanged"></asp:DropDownList>
                                <asp:TextBox ID="ordertype" runat="server" MaxLength="10" Width="100px" CssClass="textboxStyleNew"
                                    Height="13px" Visible="false" Enabled="false"/>
                            </td>
                            <td>GSTTIN NO.</td>
                            <td>
                                <asp:TextBox ID="txtGSTNNumber" runat="server" CssClass="textboxStyleNew" Height="13px" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td><span class="auto-style12"><strong></strong></span> Indent Date</td>
                            <td>
                                <asp:TextBox ID="txtIndentDate" runat="server" CssClass="textboxStyleNew" Height="13px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtIndentDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                            </td>


                            <td>&nbsp;</td>
                            <td><span class="auto-style12"><strong>*</strong></span> Invoice Date</td>
                            <td>
                                <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="textboxStyleNew" Height="13px" />
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtInvoiceDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                            </td>
                            <td><span class="auto-style12"><strong>*</strong></span> Plant Name</td>
                            <td>
                                <asp:DropDownList ID="DrpPlant" runat="server" AutoPostBack="true" Width="177px" CssClass="textboxStyleNew" TabIndex="5"></asp:DropDownList>
                                <asp:TextBox ID="Plant" runat="server" MaxLength="10" Width="100px" CssClass="textboxStyleNew"
                                    Height="13px" Visible="false" Enabled="false"/>
                            </td>

                            <td>Composition Scheme</td>
                            <td>
                                <asp:DropDownList runat="server" ID="DrpCompScheme" Width="130px" >
                                    <asp:ListItem Value="-1" Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Transporter</td>
                            <td>
                                <asp:TextBox ID="txtTransporterName" runat="server" MaxLength="10" CssClass="textboxStyleNew" Height="13px" /></td>
                            <td></td>
                            <td>Driver</td>
                            <td>
                                <asp:TextBox ID="txttransporterNo" runat="server" MaxLength="10" CssClass="textboxStyleNew" Height="13px" /></td>

                            <td><span class="auto-style12"><strong>*</strong></span> Plant Address</td>
                            <td>
                                <asp:TextBox ID="txtAddr" runat="server" MaxLength="10" CssClass="textboxStyleNew" Height="13px" />
                            </td>
                            <td>GST Reg. Date</td>
                            <td>
                                <asp:TextBox ID="txtGSTRegistrationDate" runat="server" CssClass="textboxStyleNew" Height="13px" ></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtGSTRegistrationDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td><span class="auto-style12"><strong>*</strong></span> Vehicle Number</td>
                            <td>
                                <asp:TextBox ID="txtvehicleNo" runat="server" MaxLength="50" CssClass="textboxStyleNew" Height="13px" /></td>
                            <td>&nbsp;</td>
                            <td>Vehicle Type</td>
                            <td>
                                <asp:TextBox ID="txtVehicleType" runat="server" CssClass="textboxStyleNew" Height="13px" /></td>

                            <td>Plant City</td>
                            <td>
                                <asp:TextBox ID="txtPlantCity" runat="server" CssClass="textboxStyleNew" Height="13px" />
                            </td>
                            <td><span class="auto-style12"><strong>*</strong></span> State</td>
                            <td>
                                <asp:DropDownList ID="DrpState" runat="server" OnSelectedIndexChanged="DrpState_SelectedIndexChanged" AutoPostBack="True" CssClass="textboxStyleNew" Width="130px">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtState" runat="server" MaxLength="10" Width="100px" CssClass="textboxStyleNew"
                                    Height="13px" Visible="false" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>RECEIPT DATE :</td>
                            <td>
                                <asp:TextBox ID="txtReceiptDate" runat="server" CssClass="textboxStyleNew" />
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtReceiptDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                            </td>
                            <td></td>
                            <td>RECEIPT VALUE (₹) :</td>
                            <td>
                                <asp:TextBox ID="txtReceiptValue" runat="server" ReadOnly="true" onkeypress="return isNumberKey(event)" CssClass="textboxStyleNew" />
                            </td>
                            <td>Plant_PostCode</td>
                            <td>
                                <asp:TextBox ID="txtPostalCode" runat="server" onkeypress="return isNumberKey(event)" CssClass="textboxStyleNew" Height="13px" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpanelManual" runat="server">
        <ContentTemplate>
            <div style="width: 99%;">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 50%">
                            <asp:Panel ID="panelAddLine" runat="server" GroupingText="Product Section" Width="100%">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="color: darkblue; font-weight: bold; font-family: 'Segoe UI'" class="auto-style9">Product Code :</td>
                                        <td class="auto-style9">
                                            <asp:TextBox ID="txtProductCode" runat="server" Width="169px" Height="13px" Font-Bold="true" ForeColor="MidnightBlue" Style="margin-right: 0px" CssClass="textboxStyleNew"></asp:TextBox>
                                            &nbsp;
                                            <asp:Button ID="BtnGetProductDetails" runat="server" Text="Get Details" BackColor="Aquamarine" Width="86px"
                                                OnClick="BtnGetProductDetails_Click" CssClass="textboxStyleNew" />
                                        </td>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtProductCode" MinimumPrefixLength="1" EnableCaching="true"
                                            CompletionSetCount="1" CompletionInterval="1000" ServiceMethod="GetProductDescription">
                                        </asp:AutoCompleteExtender>
                                    </tr>
                                    <tr>
                                        <td>Product Details :</td>
                                        <td>
                                            <asp:TextBox ID="txtProductDesc" runat="server" Width="264px" Height="13px" Font-Bold="false" ForeColor="MidnightBlue"
                                                placeholder="Product Code/Product Name" Font-Size="Smaller" Enabled="False" CssClass="textboxStyleNew" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>MRP(₹):</td>
                                        <td>
                                            <asp:TextBox ID="txtMRP" runat="server" Width="100px" Height="13px" Font-Bold="true" ForeColor="MidnightBlue" placeholder="0" Enabled="False" CssClass="textboxStyleNew" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Entry Type :</td>
                                        <td>
                                            <asp:DropDownList ID="DDLEntryType" runat="server" AutoPostBack="true" CssClass="textboxStyleNew" Font-Names="Arial" Font-Size="8pt" Height="23px"
                                                Width="110px" OnSelectedIndexChanged="DDLEntryType_SelectedIndexChanged">
                                                <asp:ListItem>Box</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="LTEntryType" runat="server">Box Qty</asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEntryValue" runat="server" Width="100px" placeholder="0" OnTextChanged="txtEntryValue_TextChanged" onkeypress="CheckNumeric(event);" AutoPostBack="True" TabIndex="1" CssClass="textboxStyleNew" Height="13px" ToolTip="Box" MaxLength="6" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Weight[Kg] :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtWeight" runat="server" Width="100px" Height="13px" Font-Bold="true" ForeColor="MidnightBlue"
                                                placeholder="0Kg" Enabled="false" Font-Size="Smaller" CssClass="textboxStyleNew" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Volume[Ltr] :</td>
                                        <td>
                                            <asp:TextBox ID="txtVolume" runat="server" Width="100px" Height="13px" Font-Bold="true" ForeColor="MidnightBlue" placeholder="0Ltr"
                                                Enabled="false" Font-Size="Smaller" CssClass="textboxStyleNew" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td style="width: 50%">
                            <asp:Panel ID="PanelManaulEntry" runat="server" Width="100%" GroupingText="Value Entry Section">
                                <table style="width: 100%; height: 100%">
                                    <tr>
                                        <td>
                                            <tr>
                                                <th style="background-color: DimGray; color: white"><b>BASIC</b> </th>
                                                <td style="text-align: right" class="auto-style10">Rate(₹) : </td>
                                                <td>
                                                    <asp:TextBox ID="txtRate" runat="server" AutoPostBack="true" Font-Bold="True" Font-Size="Smaller" ForeColor="MidnightBlue" Height="13px" onkeypress="return isNumberKey(event)" OnTextChanged="txtRate_TextChanged" placeholder="0" TabIndex="2" Width="90px" CssClass="textboxStyleNew" ToolTip="Rate" />
                                                </td>
                                                <td>Value(₹) : </td>
                                                <td>
                                                    <asp:TextBox ID="txtValueRate" runat="server" Enabled="false" Font-Bold="true" Font-Size="Smaller" ForeColor="MidnightBlue" Height="13px" onkeypress="return isNumberKey(event)" placeholder="0" Width="90px" CssClass="textboxStyleNew" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <th style="background-color: DimGray; color: white" class="auto-style8"><b>TRD. DISC.</b> </th>
                                                <td style="text-align: right" class="auto-style11">TRD % </td>
                                                <td class="auto-style8">
                                                    <asp:TextBox ID="txtTRDDiscPerc" runat="server" AutoPostBack="true" Font-Bold="true" Font-Size="Smaller" ForeCotxtTotalValuelor="MidnightBlue" Height="13px" onkeypress="return isNumberKey(event)" OnTextChanged="txtTRDDiscPerc_TextChanged" placeholder="0%" TabIndex="3" Width="90px" CssClass="textboxStyleNew" ToolTip="TRD" />
                                                </td>
                                                <td class="auto-style8">Value(₹) : </td>
                                                <td class="auto-style8">
                                                    <asp:TextBox ID="txtTRDpercValue" runat="server" Enabled="false" Font-Bold="true" Font-Size="Smaller" ForeColor="MidnightBlue" Height="13px" onkeypress="return isNumberKey(event)" placeholder="0" Width="90px" CssClass="textboxStyleNew" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <th style="background-color: DimGray; color: white"><b>Special Discount</b> </th>
                                                <td style="text-align: right" class="auto-style11">SD % </td>
                                                <td>
                                                    <asp:TextBox ID="txtSDPerc" runat="server" AutoPostBack="true" Font-Bold="true" Font-Size="Smaller" ForeColor="MidnightBlue" Height="13px" onkeypress="return isNumberKey(event)" OnTextChanged="txtSpecialDiscountPerc_TextChanged" placeholder="0%" TabIndex="5" Width="90px" CssClass="textboxStyleNew" ToolTip="SD" />
                                                </td>
                                                <td>SD Value : </td>
                                                <td>
                                                    <asp:TextBox ID="txtSDValue" runat="server" Enabled="false" Font-Bold="true" Font-Size="Smaller" ForeColor="MidnightBlue" Height="15px" onkeypress="return isNumberKey(event)" placeholder="0" Width="90px" CssClass="textboxStyleNew" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <th style="background-color: DimGray; color: white"><b>PRICE EQUAL</b> </th>
                                                <td style="text-align: right" class="auto-style11">Value : </td>
                                                <td>
                                                    <asp:TextBox ID="txtPriceEqualValue" runat="server" ReadOnly="true" Font-Bold="true" Font-Size="Smaller" ForeColor="MidnightBlue" Height="13px" AutoPostBack="true" OnTextChanged="txtPriceEqualValue_TextChanged" textonkeypress="return isNumberKey(event)" placeholder="0" TabIndex="4" Width="90px" CssClass="textboxStyleNew" ToolTip="Value" />
                                                </td>
                                            </tr>
                                            <tr runat="server" visible="false" id="IGST">
                                                <th style="background-color: DimGray; color: white"><b>IGST</b> </th>
                                                <td style="text-align: right" class="auto-style11">IGST % </td>
                                                <td>
                                                    <asp:TextBox ID="txtIGSTPerc" runat="server" AutoPostBack="true" Font-Bold="true" Font-Size="Smaller" ForeColor="MidnightBlue" Height="13px" onkeypress="return isNumberKey(event)" OnTextChanged="txtIGSTPerc_TextChanged" placeholder="0%" TabIndex="5" Width="90px" CssClass="textboxStyleNew" ToolTip="IGST %" />
                                                </td>
                                                <td>IGST Value : </td>
                                                <td>
                                                    <asp:TextBox ID="txtIGSTValue" runat="server" Enabled="false" Font-Bold="true" Font-Size="Smaller" ForeColor="MidnightBlue" Height="15px" onkeypress="return isNumberKey(event)" placeholder="0" Width="90px" CssClass="textboxStyleNew" />
                                                </td>
                                            </tr>
                                            <tr runat="server" visible="false" id="SGST">
                                                <th style="background-color: DimGray; color: white"><b>SGST</b> </th>
                                                <td style="text-align: right" class="auto-style11">SGST % </td>
                                                <td>
                                                    <asp:TextBox ID="txtSGSTPerc" runat="server" AutoPostBack="true" Font-Bold="true" Font-Size="Smaller" ForeColor="MidnightBlue" Height="13px" onkeypress="return isNumberKey(event)" OnTextChanged="txtSGSTPerc_TextChanged" placeholder="0%" TabIndex="5" Width="90px" CssClass="textboxStyleNew" ToolTip="SGST %" />
                                                </td>
                                                <td>SGST Value : </td>
                                                <td>
                                                    <asp:TextBox ID="txtSGSTValue" runat="server" Enabled="false" Font-Bold="true" Font-Size="Smaller" ForeColor="MidnightBlue" Height="15px" onkeypress="return isNumberKey(event)" placeholder="0" Width="90px" CssClass="textboxStyleNew" />
                                                </td>
                                            </tr>
                                            <tr runat="server" visible="false" id="UGST">
                                                <th style="background-color: DimGray; color: white"><b>UGST</b> </th>
                                                <td style="text-align: right" class="auto-style11">UGST % </td>
                                                <td>
                                                    <asp:TextBox ID="txtUGSTPerc" runat="server" AutoPostBack="true" Font-Bold="true" Font-Size="Smaller" ForeColor="MidnightBlue" Height="13px" onkeypress="return isNumberKey(event)" OnTextChanged="txtUGSTPerc_TextChanged" placeholder="0%" TabIndex="5" Width="90px" CssClass="textboxStyleNew" ToolTip="UGST %" />
                                                </td>
                                                <td>UGST Value : </td>
                                                <td>
                                                    <asp:TextBox ID="txtUGSTValue" runat="server" Enabled="false" Font-Bold="true" Font-Size="Smaller" ForeColor="MidnightBlue" Height="15px" onkeypress="return isNumberKey(event)" placeholder="0" Width="90px" CssClass="textboxStyleNew" />
                                                </td>
                                            </tr>
                                            <tr runat="server" visible="false" id="CGST">
                                                <th style="background-color: DimGray; color: white"><b>CGST</b> </th>
                                                <td style="text-align: right" class="auto-style11">CGST % </td>
                                                <td>
                                                    <asp:TextBox ID="txtCGSTPerc" runat="server" AutoPostBack="true" Font-Bold="true" Font-Size="Smaller" ForeColor="MidnightBlue" Height="13px" onkeypress="return isNumberKey(event)" OnTextChanged="txtCGSTPerc_TextChanged" placeholder="0%" TabIndex="5" Width="90px" CssClass="textboxStyleNew" ToolTip="CGST %" />
                                                </td>
                                                <td>CGST Value : </td>
                                                <td>
                                                    <asp:TextBox ID="txtCGSTValue" runat="server" Enabled="false" Font-Bold="true" Font-Size="Smaller" ForeColor="MidnightBlue" Height="15px" onkeypress="return isNumberKey(event)" placeholder="0" Width="90px" CssClass="textboxStyleNew" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <th style="background-color: DimGray; color: white">Gross Rate(₹): </th>
                                                <td class="auto-style10">&nbsp;</td>
                                                <td>
                                                    <asp:TextBox ID="txtGrossRate" runat="server" AutoPostBack="true" Enabled="False" Font-Bold="true" Font-Size="Smaller" ForeColor="MidnightBlue" Height="13px" onkeypress="return isNumberKey(event)" OnTextChanged="txtGrossRate_TextChanged" placeholder="0.00" Width="90px" CssClass="textboxStyleNew" ToolTip="GrossRate" />
                                                </td>
                                                <td>Remark</td>
                                                <td>
                                                    <asp:TextBox ID="txtRemark" runat="server" Font-Bold="true" Font-Size="Smaller" ForeColor="MidnightBlue" Height="13px" placeholder="Enter Remark" Width="90px" CssClass="textboxStyleNew" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <th style="background-color: DimGray; color: white">Total Value(₹): </th>
                                                <td class="auto-style10">&nbsp;</td>
                                                <td>
                                                    <asp:TextBox ID="txtTotalValue" runat="server" Enabled="false" Font-Bold="true" Font-Size="Smaller" ForeColor="MidnightBlue" Height="13px" onkeypress="return isNumberKey(event)" placeholder="0.00" Width="90px" CssClass="textboxStyleNew" ToolTip="TotalValue" />
                                                </td>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <asp:Button ID="BtnAddItem" runat="server" CssClass="button" Height="31px" OnClick="btnSave_Click" Text="Save" Width="61px" />
                                                </td>
                                            </tr>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div id="controlHead" style="margin-top: 0px; margin-left: 5px; padding-right: 10px;"></div>
            <div style="height: 250px; overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px;">
                <asp:GridView runat="server" ID="GridPurchItems" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                    CellPadding="3" PageSize="20" AutoGenerateDeleteButton="True" OnRowDeleting="GridPurchItems_RowDeleting" ShowFooter="true">
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
                        <asp:BoundField HeaderText="Tax1 %" DataField="TAX_PERC" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Tax1 Comp." DataField="TAXCOMPONENT" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Tax1 Value" DataField="TAX_AMOUNT" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Tax2 %" DataField="ADD_TAX_PERC" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Tax2 Comp." DataField="ADDTAXCOMPONENT" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Tax2 Value" DataField="ADD_TAX_AMOUNT" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Total Tax%" DataField="VAT_INC_PERC" DataFormatString="{0:n2}" HtmlEncode="false" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="Total Tax Value" DataField="VAT_INC_PERCVALUE" DataFormatString="{0:n2}" HtmlEncode="false" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="NetValue" DataField="AMOUNT" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="GrossRate" DataField="GROSSRATE" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="SP DISC%" DataField="SPECIALDISCP" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:BoundField HeaderText="SP DISC VALUE" DataField="SPECIALDISCV" DataFormatString="{0:n2}" />
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <asp:TextBox ID="txtRemark" runat="server" Text='<%# Eval("Remark") %>' CssClass="textboxStyleNew" Width="60px" MaxLength="500"></asp:TextBox>
                            </ItemTemplate>

                        </asp:TemplateField>
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
        </ContentTemplate>
    </asp:UpdatePanel>

    
  

    <div style="width: 100%">
        <asp:Button ID="Button2" runat="Server" Text="" Style="display: none;" />
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button2"
            PopupControlID="Panel1" CancelControlID="Button4"
            BackgroundCssClass="ModalPoupBackgroundCssClass" BehaviorID="ModalPopupExtender1">
        </asp:ModalPopupExtender>

        <asp:Panel ID="Panel1" runat="server" Style="display: none; background-color: silver">
            <div style="align-content:center;" >
                 
                <span style="color: red; font-weight: 600; text-align: center">Records which are not uploaded !!</span>
                <asp:Button ID="Btnclick" runat="Server" Text="newpopup" OnClick="Btnclick_Click" Style="display: none;" />
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
</asp:Content>

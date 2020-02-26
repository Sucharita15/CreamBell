<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmSaleOrder.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmSaleOrder" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
  <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
  <link href="css/btnSearch.css" rel="stylesheet" /> 
    <%--<link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />--%>
    <link href="css/style.css" rel="stylesheet" /> 
        <link href="css/textBoxDesign.css" rel="stylesheet" />
        <link href="css/SSheet.css" rel="Stylesheet" type="text/css" />

  <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
  
  <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.single-selection').multiselect({
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                nonSelectedText: 'Select'
            });
        });
        </script>
    <style type="text/css">
        .loader {
            border: 8px solid #f3f3f3;
            border-radius: 50%;
            border-top: 8px solid #3498db;
            width: 30px;
            height: 30px;
            -webkit-animation: spin 2s linear infinite; /* Safari */
            animation: spin 2s linear infinite;
            margin-left:50%;
        }

        /* Safari */
        @-webkit-keyframes spin {
            0% { -webkit-transform: rotate(0deg); }
            100% { -webkit-transform: rotate(360deg); }
        }

        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
    </style>
    <script type="text/javascript">
        function showSaveAction() {
           
            if (confirm('Are you sure to save record?')) {  $("#divloader").css({'display': 'block'}); return true; } else { return false; }
        }

        function HideLoader() {
            $("#divloader").css({'display': 'none'});
        }
        function validatedate(inputText) {
            //debugger;
            var dateformat = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
            // Match the date format through regular expression
            if (inputText.value.match(dateformat)) {
                //Test which seperator is used '/' or '-'
                var opera1 = inputText.value.split('/');
                var opera2 = inputText.value.split('-');
                lopera1 = opera1.length;
                lopera2 = opera2.length;
                // Extract the string into month, date and year
                if (lopera1 > 1) {
                    var pdate = inputText.value.split('/');
                }
                else if (lopera2 > 1) {
                    var pdate = inputText.value.split('-');
                }
                var dd = parseInt(pdate[0]);
                var mm = parseInt(pdate[1]);
                var yy = parseInt(pdate[2]);
                // Create list of days of a month [assume there is no leap year by default]
                var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
                if (mm == 1 || mm > 2) {
                    if (dd > ListofDays[mm - 1]) {
                        alert('Invalid date format!');
                        inputText.value = "";
                        return false;
                    }
                }
                if (mm == 2) {
                    var lyear = false;
                    if ((!(yy % 4) && yy % 100) || !(yy % 400)) {
                        lyear = true;
                    }
                    if ((lyear == false) && (dd >= 29)) {
                        alert('Invalid date format!');
                        inputText.value = "";
                        return false;
                    }
                    if ((lyear == true) && (dd > 29)) {
                        alert('Invalid date format!');
                        inputText.value = "";
                        return false;
                    }
                }
            }
            else {
                alert("Invalid date format!");
                inputText.value = "";
                return false;
            }
        }
        function changeColor() {
         
            var btnclr = document.getElementById('<%=BtnAddItem.ClientID%>');
            btnclr.style.backgroundColor = "red"
        }

        function checkTabPress(e)
        {
            //debugger;
            "use strict";
            e = e || event;      
           
               var btnclr = document.getElementById('<%=BtnAddItem.ClientID%>');
               btnclr.style.backgroundColor = "#3498db"

        }

        var body = document.querySelector('body');
        body.addEventListener('keyup', checkTabPress);

        function ResetColor()
        {
            var btnclr = document.getElementById('<%=BtnAddItem.ClientID%>');
            btnclr.style.backgroundColor = "#3498db"
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
            //debugger;
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
            //debugger;
            $(".arrow_box").addClass("arrow_box1")
            // remove a class
            $(".arrow_box").removeClass("arrow_box")
        }
        function test1() {
            //debugger;
            $(".arrow_box1").addClass("arrow_box")
            // remove a class
            $(".arrow_box1").removeClass("arrow_box1")
        }

        

        function isNumberKeyWithDecimal(evt) {
            //debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31
              && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }


        function checkEnterKey(e) {

            if (e.keyCode == 13) {
                //alert("test");
                return false;
            }
        }


    </script>

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

        function InIEvent() {
            $(document).ready(function () {
                $("select").searchable();
            });
        }

        $('#SecDisc').keyup(function () {
            if ($(this).val() > 100) {
                alert('Alert');
                $(this).val('100');
            }
            ////});
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <div class="top-title">
                <span>Manual Sale Order</span>
            </div>
                <div id="divloader" class="loader" style="display:none;"></div>
                <%--<asp:UpdateProgress ID="progress" runat="server" DynamicLayout="true" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="divloader" class="loader" visible="false"></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>--%>

            <div style="width: 99%; text-align: left">
                <asp:UpdatePanel ID="upheader" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="rdoManualEntry" />
                        <asp:PostBackTrigger ControlID="ImDnldTemp" />
                        <asp:PostBackTrigger ControlID="btnApply" />      
                         <asp:PostBackTrigger ControlID="btnGO" />      
                         <asp:PostBackTrigger ControlID="BtnAddItem" />      
                    </Triggers>
                    <ContentTemplate>
                        <asp:Panel ID="pnlexcelupload" runat="server">
                            <table style="width: 100%; table-layout: fixed;">
                                <tr>
                                    <td style="width: 4%;">
                                        <asp:UpdatePanel ID="UpdatePanel1sds" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="showSaveAction();" CssClass="common_button" TabIndex="11" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="BtnSave"
                                                    EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td style="width: 8%;">
                                        <asp:RadioButton ID="rdoManualEntry" runat="server" AutoPostBack="true" Text="Manual Entry" Checked="true"
                                            OnCheckedChanged="rdoManualEntry_CheckedChanged" GroupName="radio" />
                                        <br />
                                        <asp:RadioButton ID="rdoExcelEntry" runat="server" AutoPostBack="true" Text="Excel Upload"
                                            OnCheckedChanged="rdoManualEntry_CheckedChanged" GroupName="radio" />
                                    </td>
                                    <td style="width: 15%">
                                        <asp:UpdatePanel runat="server" ID="UploadPanel">
                                            <ContentTemplate>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" Visible="false" OnClientUploadStarted="uploadStart" Height="18px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>

                                        </asp:UpdatePanel>
                                    </td>
                                    <td style="width: 5%;">
                                        &nbsp;</td>
                                    <td style="width: 5%;">
                                        <asp:ImageButton ID="ImDnldTemp" runat="server" AutoPostBack="true" src="Images/DownloadTemplate.gif" OnClick="ImDnldTemp_Click" Visible="false" ToolTip="Click to download excel template !!" />
                                    </td>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnUplaod" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <td style="width: 5%;">
                                                <asp:Button ID="btnUplaod" runat="server" Text="Upload" OnClick="btnUplaod_Click" CssClass="ReportSearch" Visible="False" />
                                            </td>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <td style="width: 10%;">
                                        <asp:RadioButton ID="rdAll" runat="server" AutoPostBack="true" Text="All Product" ToolTip="All Products Lists" ValidationGroup="ValItem" OnCheckedChanged="rdAll_CheckedChanged" GroupName="Rd" />
                                        <br />
                                        <asp:RadioButton ID="rdStock" runat="server" AutoPostBack="true" Text="Stock Product" Checked="true" ToolTip="Products available in stock" ValidationGroup="ValItem" GroupName="Rd" OnCheckedChanged="rdAll_CheckedChanged" />
                                    </td>
                                    <td style="width: 3%;">
                                        <b>Data</b>
                                        <br />
                                        <b>Display</b>
                                    </td>
                                    <td style="width: 5%;">
                                        <asp:RadioButton ID="rdAsc" runat="server" AutoPostBack="true" Text="ASC" CheToolTip="Ascending order" Checked="true" ValidationGroup="Ordering" OnCheckedChanged="rdAsc_CheckedChanged" GroupName="RdOrdering" />
                                        <br />
                                        <asp:RadioButton ID="rdDesc" runat="server" AutoPostBack="true" Text="DESC"  ToolTip="Descending order" ValidationGroup="Ordering" GroupName="RdOrdering" OnCheckedChanged="rdAsc_CheckedChanged" />
                                    </td>
                                    <td style="width: 7%;">
                                        <asp:RadioButton ID="rdExempt" runat="server" AutoPostBack="true" Text="Exempt" CheToolTip="ExemptProducts"  ValidationGroup="Exemption" GroupName="RdExemption" OnCheckedChanged="Exempt_CheckedChanged" />
                                        <br />
                                        <asp:RadioButton ID="rdNonExempt" runat="server" AutoPostBack="true" Text="NonExempt"  ToolTip="NonExemptProducts" Checked="true" ValidationGroup="Exemption" GroupName="RdExemption" OnCheckedChanged="Exempt_CheckedChanged" />
                                    </td>
                                    <td style="width: 20%;">
                                        <asp:Label ID="lblMessage" runat="server" AutoPostBack="true" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td style="width: 20%;">
                                        <asp:Button ID="btnInvPre" runat="server" Text="Goto Invoice Prepration" OnClick="btnInvPre_Click" CssClass="common_button" TabIndex="12" />
                                        <asp:Label ID="LblMessage1" runat="server" Text="" Font-Bold="true" AutoPostBack="true" ForeColor="Red" Font-Names="Segoe UI" Font-Italic="true"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div style="width: 99%; text-align: left">
                <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%; table-layout: fixed;">
                            <tr>
                                <td style="width: 10%;">Customer Group</td>
                                <td style="width: 15%;">
                                    <%--<asp:DropDownList ID="ddlCustomerGroup" runat="server" AutoPostBack="True" TabIndex="51" OnSelectedIndexChanged="ddlCustomerGroup_SelectedIndexChanged" CssClass="saleorder_select"></asp:DropDownList>--%>
                                 <asp:ListBox ID="ddlCustomerGroupNew" ClientIDMode="Static" runat="server"    AutoPostBack="True"
                                   OnSelectedIndexChanged="ddlCustomerGroup_SelectedIndexChanged"  CssClass="single-selection"></asp:ListBox>
                                </td>
                                <td style="width: 10%;">
                                    <asp:Label ID="lblPSRName" runat="server" Text="PSR Name" Visible="False"></asp:Label>
                                </td>
                                <td style="width: 15%;">
                                    <%--<asp:DropDownList ID="ddlPSRName" runat="server" AutoPostBack="True" TabIndex="53" OnSelectedIndexChanged="ddlPSRName_SelectedIndexChanged" Visible="False" CssClass="saleorder_select"></asp:DropDownList>--%>
                                  <asp:ListBox ID="ddlPSRNameNew" ClientIDMode="Static" runat="server"    AutoPostBack="True"
                                   OnSelectedIndexChanged="ddlPSRName_SelectedIndexChanged"  CssClass="single-selection"></asp:ListBox>
                                    </td>
                                <td style="width: 10%;">Contact</td>
                                <td style="width: 15%;">
                                    <asp:TextBox ID="txtContactNo" runat="server" ReadOnly="True" TabIndex="54" CssClass="saleorder_input" />
                                </td>
                                <td style="width: 10%;">Scheme/Discount</td>
                                <td style="width: 15%;">
                                    <asp:TextBox ID="txtSchemeDisc" runat="server" ReadOnly="true" TabIndex="55" CssClass="saleorder_input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Customer Name</td>
                                <td>
                                    <%--<asp:DropDownList ID="ddlCustomer" runat="server" AutoPostBack="True" TabIndex="52" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" CssClass="saleorder_select"></asp:DropDownList>--%>
                                <asp:ListBox ID="ddlCustomerNew" ClientIDMode="Static" runat="server"    AutoPostBack="True"
                                   OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged"  CssClass="single-selection"></asp:ListBox>
                                    
                                </td>
                                <td>GST TIN No:</td>
                                <td>
                                    <asp:TextBox ID="txtGSTtin" runat="server" CssClass="saleorder_input" TabIndex="56" ReadOnly="true" />
                                </td>
                                <td>GST TIN Reg. Date :</td>
                                <td>
                                    <asp:TextBox ID="txtGSTtinRegistration" runat="server" CssClass="saleorder_input" TabIndex="57" ReadOnly="true" />
                                </td>
                                <td>Delivery Date</td>
                                <td>
                                    <asp:TextBox ID="txtDeliveryDate" runat="server" CssClass="saleorder_input" TabIndex="58" OnTextChanged="txtDeliveryDate_TextChanged" />
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MMM/yyyy" TargetControlID="txtDeliveryDate">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>Bill To Address</td>
                                <td>
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="saleorder_input" ReadOnly="True" TabIndex="59" TextMode="MultiLine" />
                                </td>
                                <td>Ship to Address</td>
                                <td>
                                    <%--<asp:DropDownList ID="ddlShipToAddress" runat="server" AutoPostBack="True" TabIndex="60" OnSelectedIndexChanged="ddlShift_SelectedIndexChanged" CssClass="saleorder_select"></asp:DropDownList>--%>
                                   <asp:ListBox ID="ddlShipToAddressNew" ClientIDMode="Static" runat="server"    AutoPostBack="True"
                                   OnSelectedIndexChanged="ddlShift_SelectedIndexChanged"  CssClass="single-selection"></asp:ListBox>
                                    </td>
                                <td>Compositon Scheme :</td>
                                <td>
                                   <%-- <asp:CheckBox ID="chkCompositionScheme" runat="server" ReadOnly="true" TabIndex="61" />--%>
                                        <asp:ListBox ID="chkCompositionSchemeNew" ClientIDMode="Static" runat="server"   ReadOnly="true"
                                  CssClass="single-selection"></asp:ListBox>
                                </td>
                                <td>Order Value :</td>
                                <td>
                                    <asp:TextBox ID="InvTot" runat="server" CssClass="saleorder_input" TabIndex="62" Enabled="false"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Bill To State</td>
                                <td>
                                    <asp:TextBox ID="txtBilltoState" runat="server" CssClass="saleorder_input" TabIndex="63" ReadOnly="true" />
                                </td>
                                <td>Business Unit: </td>
                                <td>
                                    <%--<asp:DropDownList ID="ddlBusinessUnit" runat="server" AutoPostBack="True" TabIndex="64" OnSelectedIndexChanged="ddlBusinessUnit_SelectedIndexChanged" CssClass="saleorder_select"></asp:DropDownList>--%>
                                  <asp:ListBox ID="ddlBusinessUnitNew" ClientIDMode="Static" runat="server"    AutoPostBack="True"
                                   OnSelectedIndexChanged="ddlBusinessUnit_SelectedIndexChanged"  CssClass="single-selection"></asp:ListBox>
                                    </td>
                                <td>
                                    <asp:Label ID="lblBeatName" runat="server" Text="Beat Name" Visible="False" TabIndex="65"></asp:Label>
                                </td>
                                <td>
                                  <%--  <asp:DropDownList ID="ddlBeatName" runat="server" AutoPostBack="True" TabIndex="66" CssClass="saleorder_select" OnSelectedIndexChanged="ddlBeatName_SelectedIndexChanged" Visible="False">
                                    </asp:DropDownList>--%>
                                        <asp:ListBox ID="ddlBeatNameNew" ClientIDMode="Static" runat="server"    AutoPostBack="True"
                                   OnSelectedIndexChanged="ddlBeatName_SelectedIndexChanged"  CssClass="single-selection"></asp:ListBox>
                                </td>
                                <td>Total Qty:</td>
                                <td>
                                    <asp:TextBox ID="TotQty" runat="server" CssClass="saleorder_input" TabIndex="67" Enabled="false"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Indent No</td>

                                <td>
                                    <asp:TextBox ID="txtIndentNo" runat="server" CssClass="saleorder_input" TabIndex="68" ReadOnly="true" />
                                </td>
                                <td>Customer Outstanding</td>
                                <td>
                                    <asp:TextBox ID="txtOutstandingAmount" runat="server" TabIndex="69" CssClass="saleorder_input" ReadOnly="true" />
                                </td>
                                <td>Total Ltr:</td>
                                <td>
                                    <asp:TextBox ID="txttotltr" runat="server" CssClass="saleorder_input" TabIndex="70" Enabled="false"></asp:TextBox></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <asp:Panel ID="panelAddLine" runat="server" Width="100%">
                <table style="border-spacing: 0px; width: 99%;">
                    <tr>
                        <td style="width: 10%;">Product Group</td>
                        <td style="width: 10%;">Product Sub Category</td>
                        <td style="width: 10%;">Product<asp:Label ID="lblHidden" TabIndex="71" runat="server" Visible="False"></asp:Label></td>
                        <td style="width: 5%;">Enter Crates</td>
                        <td style="width: 5%;">
                            <asp:Label ID="Label1" runat="server" TabIndex="72" Text="Enter Box"></asp:Label></td>
                        <td style="width: 5%" id="tdEnterPcs" class="auto-style2">
                            <asp:Label ID="lblEnterPcs" runat="server" TabIndex="73" Text="Enter Pcs"></asp:Label></td>
                        <td style="width: 5%;">Total Box Qty</td>
                        <td style="width: 5%;" id="tdTotalPcs">
                            <asp:Label ID="lblTotalPcs" runat="server" TabIndex="74" Text="Total Pcs"></asp:Label>&nbsp;</td>
                        <td style="width: 5%;">Total Qty Con</td>
                        <td style="width: 5%;">BOX PCS</td>
                        <td style="width: 3%;">Crates</td>
                        <td style="width: 5%;">Ltr</td>
                        <td style="width: 5%;">Price</td>
                        <td style="width: 4%;">Value</td>
                        <td style="width: 2%;"></td>
                        <td style="width: 4%;"></td>
                        <td style="width: 4%;">S.Discs%</td>
                        <td style="width: 4%;">S.DiscsValue</td>
                        <td style="width: 4%"></td>
                        <td class="auto-style5">TD Value</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                  <%--  <asp:DropDownList ID="DDLProductGroup" runat="server" TabIndex="75" AutoPostBack="true" Width="99%" 
                                        OnSelectedIndexChanged="ddlProductGroup_SelectedIndexChanged" CssClass="saleorder_select">
                                    </asp:DropDownList>--%>

                                      <asp:ListBox ID="DDLProductGroupNew" ClientIDMode="Static" runat="server"    AutoPostBack="True"
                                   OnSelectedIndexChanged="ddlProductGroup_SelectedIndexChanged"  CssClass="single-selection"></asp:ListBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <%--<asp:DropDownList ID="DDLProductSubCategory" runat="server" TabIndex="76" AutoPostBack="true" Width="99%" 
                                        OnSelectedIndexChanged="DDLProductSubCategory_SelectedIndexChanged" CssClass="saleorder_select">
                                    </asp:DropDownList>--%>

                                      <asp:ListBox ID="DDLProductSubCategoryNew" ClientIDMode="Static" runat="server"    AutoPostBack="True"
                                   OnSelectedIndexChanged="DDLProductSubCategory_SelectedIndexChanged"  CssClass="single-selection"></asp:ListBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                   <%-- <asp:DropDownList ID="DDLMaterialCode" runat="server" TabIndex="77" OnSelectedIndexChanged="DDLMaterialCode_SelectedIndexChanged" Width="99%" onkeydown="return checkEnterKey(event);" CssClass="saleorder_select" AutoPostBack="true" >
                                    </asp:DropDownList>--%>

                                    <asp:ListBox ID="DDLMaterialCodeNew" ClientIDMode="Static" runat="server"    AutoPostBack="True"
                                   OnSelectedIndexChanged="DDLMaterialCode_SelectedIndexChanged"  CssClass="single-selection"></asp:ListBox>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCrate" runat="server" AutoPostBack="true" TabIndex="78" placeholder="0" MaxLength="10" onkeypress="CheckNumeric(event)" Width="90%" OnTextChanged="txtCrate_TextChanged" CssClass="saleorder_input" /></td>
                        <td>
                            <asp:TextBox ID="txtEnterQty" runat="server" MaxLength="6" AutoPostBack="true" TabIndex="79" placeholder="0" onkeypress="CheckNumeric(event)"  Width="90%" OnTextChanged="txtQtyBox_TextChanged"  CssClass="saleorder_input"  /></td>
                        <td class="auto-style2" style="width: 3%;">
                            <asp:TextBox ID="txtPcs" runat="server" Enabled="False" AutoPostBack="true" placeholder="0" TabIndex="80" MaxLength="6" onkeypress="CheckNumeric(event)" Width="90%" OnTextChanged="txtPcs_TextChanged" CssClass="saleorder_input" /></td>
                        <td>
                            <asp:TextBox ID="txtViewTotalBox" runat="server" ReadOnly="True" CssClass="saleorder_input" Width="90%" Enabled="false" TabIndex="81" Style="background-color: rgb(235, 235, 228)"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtViewTotalPcs" runat="server" ReadOnly="True" CssClass="saleorder_input" Width="90%" Enabled="false" TabIndex="82" Style="background-color: rgb(235, 235, 228)"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtQtyBox" runat="server" ReadOnly="True" CssClass="saleorder_input" Width="90%" TabIndex="83" Enabled="false" Style="background-color: rgb(235, 235, 228)"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtBoxPcs" runat="server" ReadOnly="True" CssClass="saleorder_input" Width="90%" TabIndex="84" Enabled="false" Style="background-color: rgb(235, 235, 228)"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtQtyCrates" runat="server" ReadOnly="True" Width="70%" CssClass="saleorder_input" TabIndex="85" Enabled="false" Style="background-color: rgb(235, 235, 228)"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtLtr" runat="server" Width="90%" ReadOnly="True" CssClass="saleorder_input" TabIndex="86" Enabled="false" Style="background-color: rgb(235, 235, 228)" /></td>
                        <td>
                            <asp:TextBox ID="txtPrice" runat="server" Width="90%" ReadOnly="True" CssClass="saleorder_input" TabIndex="87" Enabled="false" Style="background-color: rgb(235, 235, 228)" /></td>
                        <td>
                            <asp:TextBox ID="txtValue" runat="server" Width="80%" ReadOnly="True" CssClass="saleorder_input" TabIndex="88" Enabled="false" Style="background-color: rgb(235, 235, 228)" /></td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                           <asp:Button ID="btnrecalculate" runat="server" Text="*" OnClick="btnrecalculate_Click" TabIndex="90"  UseSubmitBehavior="false"  CssClass="common_button"  />                                </ContentTemplate>
                            </asp:UpdatePanel>
                            </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                             <asp:Button ID="BtnAddItem" runat="server" Text="Add" OnClick="BtnAddItem_Click" TabIndex="89"  UseSubmitBehavior="false" onfocus="changeColor()" onkeypress="ResetColor()" CssClass="common_button"  />
<%--<asp:Button ID="Button1" runat="server" Text="Add" OnClick="BtnAddItem_Click" OnClientClick="this.disabled='true';" UseSubmitBehavior="false" CssClass="common_button" TabIndex="1" /></td>--%>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSDiscPer" runat="server" AutoPostBack="true" placeholder="0" TabIndex="90" MaxLength="10" Width="90%" CssClass="saleorder_input" onkeypress="return isNumberKeyWithDecimal(event)" OnTextChanged="txtSDiscPer_TextChanged" /></td>
                            <td>
                                <asp:TextBox ID="txtSDiscVal" runat="server" AutoPostBack="true" CssClass="saleorder_input" placeholder="0" MaxLength="10" Width="90%" onkeypress="CheckNumeric(event)" TabIndex="91" OnTextChanged="txtSDiscVal_TextChanged"></asp:TextBox></td>
                            <td>
                                <asp:Button ID="btnGO" runat="server" Text="Go" CssClass="common_button" TabIndex="92" OnClick="btnGO_Click" meta:resourcekey="btnGOResource1" /></td>
                            <td>
                                <asp:TextBox ID="txtTDValue" runat="server" TabIndex="93" CssClass="saleorder_input" placeholder="0" MaxLength="6" AutoPostBack="true" Width="50px" onkeypress="CheckNumeric(event)" OnTextChanged="txtTDValue_TextChanged"></asp:TextBox></td>
                                <asp:TextBox ID="txtHdnTDValue" runat="server" CssClass="saleorder_input" TabIndex="94" placeholder="0" MaxLength="6" AutoPostBack="true" Visible="false" onkeypress="CheckNumeric(event)"></asp:TextBox></td>
                            <td>
                                <asp:Button ID="btnApply" runat="server" Text="Apply" TabIndex="95" CssClass="common_button" OnClick="btnApply_Click" Width="48px" /></td>
                    </tr>
                </table>
            </asp:Panel>


            <div class="bottom_Grid_section" style="width: 100%; table-layout: fixed;">

                <div class="product_details_Grid" style="width: 100%; table-layout: fixed;">
                    <div style='overflow-x: scroll; width: 100%;'>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:GridView runat="server" ID="gvDetails" GridLines="Horizontal" CssClass="table" AutoGenerateColumns="False" Width="100%" BackColor="White"
                                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" TabIndex="96"
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
                                                <asp:Label ID="Line_No" runat="server" Text='<%#  Eval("SNO") %>' Visible="false" TabIndex="100" />
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
                                        <asp:BoundField HeaderText="Product Name" DataField="Product_Name">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="QtyConv" DataField="QtyBox" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" Width="0px" />
                                            <ItemStyle HorizontalAlign="Left" Width="0px" />
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
                                        <asp:BoundField HeaderText="Qty [Crates]" DataField="QtyCrates" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Qty [Ltr]" DataField="QtyLtr" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Price" DataField="Price" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="MRP" DataField="MRP" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Disc Type" DataField="DiscType">
                                            <HeaderStyle HorizontalAlign="left" />
                                            <ItemStyle HorizontalAlign="left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Disc" DataField="Disc" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Disc Value" DataField="DiscVal" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Tax1 %" DataField="Tax_Code" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Tax1 Amt" DataField="Tax_Amount" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Tax2 %" DataField="AddTax_Code" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Tax2 Amt" DataField="AddTax_Amount" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Value" DataField="Value" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Disc. On" DataField="CALCULATIONON">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="HSN Code" DataField="HSNCODE">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Tax1 Comp" DataField="TAXCOMPONENT">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Tax2 Comp" DataField="ADDTAXCOMPONENT">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <%-- <asp:BoundField HeaderText="Sec Dis %" DataField="Sec_Disc_Per" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Sec Dis Value" DataField="SEC_DISC_AMOUNT" DataFormatString="{0:n2}" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>  --%>

                                        <asp:TemplateField HeaderText="Sec Dis %">
                                            <ItemTemplate>
                                                <asp:TextBox ID="SecDisc" runat="server" Width="60px" AutoPostBack="true" Text='<%#  Eval("Sec_Disc_Per","{0:n2}") %>' onkeypress="return isNumberKeyWithDecimal(event)" TabIndex="101" OnTextChanged="SecDisc_TextChanged"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sec Dis Value">
                                            <ItemTemplate>
                                                <asp:TextBox ID="SecDiscValue" runat="server" Width="60px" AutoPostBack="true" Text='<%#  Eval("SEC_DISC_AMOUNT","{0:n2}") %>' onkeypress="return isNumberKeyWithDecimal(event)" TabIndex="102" OnTextChanged="SecDiscValue_TextChanged"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="TD Value" DataField="TDValue" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="PE " DataField="PE" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Scheme Per" DataField="SchemeDisc" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Scheme Disc" DataField="SchemeDiscVal" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Add SchPer" DataField="ADDSCHDISCPER" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Add SchVal" DataField="ADDSCHDISCVAL" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Add SchAmt" DataField="ADDSCHDISCAMT" DataFormatString="{0:n2}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnDel" runat="server"  Text='Delete' OnClick="lnkbtnDel_Click" TabIndex="104" ForeColor="Black"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <br />

                <div class="polaroid scheme_table_details" style="text-align: center; width: 98%; table-layout: fixed;">
                    <div style="background: #C0C0C0; border-style: solid; border-width: thin; width: 98%; padding: 4px 0px; text-align: center"><strong>Scheme Detail</strong></div>
                    <div style='overflow-x: scroll; width: 100%;'>
                        <asp:UpdatePanel ID="upnel2" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvScheme" runat="server" AutoGenerateColumns="False" GridLines="Horizontal" Width="100%" BackColor="White"
                                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" TabIndex="105">
                                    <AlternatingRowStyle BackColor="#CCFFCC" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" TabIndex="106" Checked='<%# Convert.ToBoolean(Eval("ChkStatus")) %>' OnCheckedChanged="chkSelect_CheckedChanged" />
                                                <asp:HiddenField ID="hdnSchemeLine" Visible="false" runat="server" Value='<%# Eval("SchemeLineNo") %>'  />
                                                <asp:HiddenField ID="hdnSchemeType" Visible="false" runat="server" Value='<%# Eval("Schemetype") %>' />
                                                <asp:HiddenField ID="hdnSchSrlNo" Visible="false" runat="server" Value='<%# Eval("SRNO") %>' />
                                                <asp:HiddenField ID="hdnAddSchType" Visible="false" runat="server" Value='<%# Eval("ADDITIONDISCOUNTITEMTYPE") %>' />
                                                <asp:HiddenField ID="hdntotSchemeValueoff" Visible="false" runat="server" Value='<%# Eval("TotalSchemeValueoff") %>' />
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
                                                <asp:HiddenField ID="hScTax1" Visible="false" runat="server" Value='<%# Eval("TAX1") %>' />
                                                <asp:HiddenField ID="hScTax2" Visible="false" runat="server" Value='<%# Eval("TAX2") %>' />
                                                <asp:HiddenField ID="hScTax1component" Visible="false" runat="server" Value='<%# Eval("TAX1COMPONENT") %>' />
                                                <asp:HiddenField ID="hScTax2component" Visible="false" runat="server" Value='<%# Eval("TAX2COMPONENT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="0px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQty" runat="server" AutoPostBack="True" onkeypress="return IsNumeric(event)" TabIndex="118" OnTextChanged="txtQty_TextChanged" ReadOnly="True" Width="40px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="FreePcs" DataField="TotalFreeQtyPcs"></asp:BoundField>
                                        <asp:TemplateField HeaderText="PcsQty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQtyPcs" runat="server" AutoPostBack="True" onkeypress="return IsNumeric(event)" TabIndex="119" OnTextChanged="txtQtyPcs_TextChanged" ReadOnly="True" Width="40px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Scheme% /Val">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSchemeDiscPer" runat="server" Text='<%# Eval("SCHEME_PER_VAL","{0:n2}") %>' TabIndex="120"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Pack Size" DataField="Product_PackSize" DataFormatString="{0:0.00}"></asp:BoundField>
                                        <asp:BoundField HeaderText="ConvBox"></asp:BoundField>
                                        <asp:BoundField HeaderText="Rate" DataField="Rate" DataFormatString="{0:0.00}"></asp:BoundField>
                                        <asp:BoundField HeaderText="BasicAmt" NullDisplayText="0.00" DataFormatString="{0:0.00}"></asp:BoundField>
                                        <asp:BoundField HeaderText="DiscPer" DataFormatString="{0:0.00}"></asp:BoundField>
                                        <asp:BoundField HeaderText="DiscVal" DataFormatString="{0:0.00}"></asp:BoundField>
                                        <asp:BoundField HeaderText="Taxable Amount"></asp:BoundField>
                                        <asp:BoundField HeaderText="Tax1" DataField="Tax1Per" DataFormatString="{0:0.00}"></asp:BoundField>
                                        <asp:BoundField HeaderText="Tax1 Amt"></asp:BoundField>
                                        <asp:BoundField HeaderText="Tax2" DataField="Tax2Per" DataFormatString="{0:0.00}"></asp:BoundField>
                                        <asp:BoundField HeaderText="Tax2 Amt"></asp:BoundField>
                                        <asp:BoundField HeaderText="Amount"></asp:BoundField>
                                        <asp:BoundField HeaderText="HSNCODE" DataField="HSNCODE"></asp:BoundField>
                                        <asp:BoundField HeaderText="Tax1 Comp" DataField="Tax1Comp"></asp:BoundField>
                                        <asp:BoundField HeaderText="Tax2 Comp" DataField="Tax2Comp"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Add SCH Group">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddSchemeGroup" runat="server" Text='<%# Eval("ADDITIONDISCOUNTITEMGROUP") %>' TabIndex="120"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Add SCH Group Desc">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddSchemeGroupDesc" runat="server" Text='<%# Eval("ADDITIONDISCOUNTITEMGROUPDESC") %>' TabIndex="121"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Add SCH Disc%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddSchemePer" runat="server" Text='<%# Eval("ADDITIONDISCOUNTPERCENT","{0:n2}") %>' DataFormatString="{0:n2}" TabIndex="122"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Add SCH Disc Val Off (Per Box)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddSchemeVal" runat="server" Text='<%# Eval("ADDITIONDISCOUNTVALUEOFF","{0:n2}") %>' DataFormatString="{0:n2}" TabIndex="123"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                <asp:TemplateField HeaderText="Achievement">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAchievement" runat="server" Text='<%# Eval("SCHBOX","{0:n2}") %>' DataFormatString="{0:n2}" TabIndex="124"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       
                                <%--        <asp:TemplateField HeaderText="SCHEME BOX">
                                            <ItemTemplate>

                                                <asp:Label ID="lblAddSchemeVal" runat="server" Text='<%# Eval("MINIMUMQUANTITY","{0:n2}") %>' DataFormatString="{0:n2}" />
                                                <%--<asp:Label ID="lblSchemeBox" runat="server"Text='<%# Eval("MINIMUMQUANTITY","{0:n2}") %>' DataFormatString="{0:n2}"></asp:Label>  --%>
                                                <%--   </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                    <HeaderStyle BackColor="#003366" ForeColor="White" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </div>

            <div>
                <asp:Button ID="Button2" runat="Server" Text="" Style="display: none;" TabIndex="125"/>
                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button2"
                    PopupControlID="Panel1" CancelControlID="Button4" 
                    BackgroundCssClass="ModalPoupBackgroundCssClass" BehaviorID="ModalPopupExtender1">
                </asp:ModalPopupExtender>

                <asp:Panel ID="Panel1" runat="server" Style="display: none; background-color: silver">
                    <div>
                        <span style="color: red; font-weight: 600; text-align: center">Records which are not uploaded !!</span>

                        <asp:Button ID="Button4" runat="server" CssClass="Operationbutton" TabIndex="126" data-dismiss="modal" Text="Close" aria-hidden="true"></asp:Button>
                    </div>
                    <p></p>
                    <div style="overflow-x: scroll; width: 98%; height: 200px">
                        <asp:GridView ID="gridviewRecordNotExist" runat="server" Style="width: 100%" TabIndex="127" Font-Size="Small" CssClass="Grid" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
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

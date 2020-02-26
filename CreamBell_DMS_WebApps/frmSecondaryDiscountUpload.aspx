<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmSecondaryDiscountUpload.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmSecondaryDiscountUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
  <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
  <link href="css/btnSearch.css" rel="stylesheet" /> 
    <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
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
         .input1 {
        width: 270px;
        height: 10px;
        padding: 10px 5px;
        float: left;    
        border: 0;
        background: #eee;
        -moz-border-radius: 3px 0 0 3px;
        -webkit-border-radius: 3px 0 0 3px;
        border-radius: 3px 0 0 3px;      
    }
    
    .input1:focus {
        outline: 0;
        background: #fff;
        -moz-box-shadow: 0 0 2px rgba(0,0,0,.8) inset;
        -webkit-box-shadow: 0 0 2px rgba(0,0,0,.8) inset;
        box-shadow: 0 0 2px rgba(0,0,0,.8) inset;
    }
    
    .input1::-webkit-input-placeholder {
       color: #999;
       font-weight: normal;
       font-style: italic;
    }
    
    .input1:-moz-placeholder {
        color: #999;
        font-weight: normal;
        font-style: italic;
    }
    
    .input1:-ms-input-placeholder {
        color: #999;
        font-weight: normal;
        font-style: italic;
    }
    </style>
    <script type="text/javascript">

        function isNumberKey(evt, obj) {

            var charCode = (evt.which) ? evt.which : event.keyCode
            var value = obj.value;
            var dotcontains = value.indexOf(".") != -1;
            if (dotcontains)
                if (charCode == 46) return false;
            if (charCode == 46) return true;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }



    </script>
    <script src="Javascript/DateValidation.js" type="text/javascript"></script>
    <script type="text/javascript">
        function validatedate(inputText) {
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

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px;">
        <b>Secondary Discount Upload</b>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="ImDnldTemp" />
            <asp:PostBackTrigger ControlID="rdoManualEntry" />
            <asp:PostBackTrigger ControlID="rdoExcelEntry" />
            <asp:PostBackTrigger ControlID="btnUplaod" />
            <asp:PostBackTrigger ControlID="LnkDownloadSiteMaster" />
        </Triggers>
        <ContentTemplate>
            <table style="width: 100%">
                <tr>
                    <td style="width: 100%; text-align: center; vertical-align: top">
                        <table style="width: 100%; text-align: left" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 100%">
                                    <asp:Panel ID="pnlexcelupload" runat="server">
                                        <table style="width: 100%">
                                            <tr>

                                                <td style="width: 9%;">
                                                    <asp:RadioButton ID="rdoManualEntry" runat="server" AutoPostBack="true" Text="Manual Entry" Checked="true"
                                                        OnCheckedChanged="rdoManualEntry_CheckedChanged" GroupName="radio" />
                                                </td>

                                                <td style="width: 9%;">
                                                    <asp:RadioButton ID="rdoExcelEntry" runat="server" AutoPostBack="true" Text="Excel Upload"
                                                        OnCheckedChanged="rdoManualEntry_CheckedChanged" GroupName="radio" />
                                                </td>
                                                <td colspan="2" style="width: 15%">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 20%;">
                                                                <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" Visible="false" OnClientUploadStarted="uploadStart" Height="18px" />
                                                            </td>
                                                            <td style="width: 5%;">
                                                                <asp:Button ID="btnUplaod" runat="server" Text="Upload" OnClick="btnUplaod_Click" CssClass="ReportSearch" Visible="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="width: 5%;">                                                   
                                               <asp:ImageButton ID="ImDnldTemp" runat="server" AutoPostBack="true" src="Images/DownloadTemplate.gif" OnClick="ImDnldTemp_Click" ToolTip="Click to download excel template !!" />
                                                </td>
                                                <td style="width: 2%;">
                                                    <asp:Label ID="LblMessage1" runat="server" Text="" Font-Bold="true" AutoPostBack="true" ForeColor="Green" Font-Names="Segoe UI" Font-Italic="true"></asp:Label>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:LinkButton ID="LnkDownloadSiteMaster" runat="server" AutoPostBack="true" OnClick="LnkDownloadSiteMaster_Click">Download Site Master</asp:LinkButton>
                                                </td>


                                                <td style="width: 3%;">
                                                    <asp:Label ID="Lblstate" runat="server" Text="State" Visible="True"></asp:Label>
                                                </td>
                                                <td style="width: 10%;">
                                                    <%--<asp:DropDownList ID="ddlState" runat="server" CssClass="cbsam_select" Width="95%" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"></asp:DropDownList>--%>
                                                 <asp:ListBox ID="ddlStateNew" ClientIDMode="Static" runat="server"    AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"  CssClass="single-selection"></asp:ListBox>
                                                </td>
                                                <td style="width: 3%;">
                                                    <asp:Label ID="LblSiteId" runat="server" Text="SiteId" Visible="True"></asp:Label>
                                                </td>
                                                <td style="width: 18%;">
                                                 <%--   <asp:DropDownList ID="ddlSiteId" runat="server" Width="100%" OnSelectedIndexChanged="ddlSiteId_SelectedIndexChanged"
                                                        AutoPostBack="true" CssClass="cbsam_select"></asp:DropDownList>--%>

                                                      <asp:ListBox ID="ddlSiteIdNew" ClientIDMode="Static" runat="server"    AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlSiteId_SelectedIndexChanged"  CssClass="single-selection"></asp:ListBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <%--   </ContentTemplate>
                            </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnGetData" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <div id="panelAddd" style="margin-top: 0px; width: 100%;">
                                                <asp:Panel ID="panel2" runat="server" Width="100%">
                                                    <table style="width: 100%" cellpadding="1" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 8%;">
                                                                <asp:Label ID="lblCustomerType" runat="server" Text="Customer Type" Visible="False"></asp:Label>
                                                            </td>
                                                            <td style="width: 8%;">
                                                                <%--<asp:DropDownList ID="ddlCustomerType" CssClass="cbsam_select" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCustomerType_SelectedIndexChanged" Width="95%"></asp:DropDownList>--%>
                                                                  <asp:ListBox ID="ddlCustomerTypeNew" ClientIDMode="Static" runat="server"    AutoPostBack="True"
                                                                      OnSelectedIndexChanged="ddlCustomerType_SelectedIndexChanged"  CssClass="single-selection"></asp:ListBox>
                                                            </td>
                                                            <td style="width: 8%;">
                                                                <asp:Label ID="lblCustomer" runat="server" Text="CustomerName" Visible="False" CssClass="cbsam_select"></asp:Label>
                                                            </td>
                                                            <td style="width: 5%;">
                                                                <%--<asp:DropDownList ID="ddlCustomer" runat="server" AutoPostBack="True" Visible="False" Width="70%" CssClass="cbsam_select">
                                                                </asp:DropDownList>--%>
                                                                <asp:ListBox ID="ddlCustomerNew" ClientIDMode="Static" runat="server"    AutoPostBack="True" Visible="False"
                                                                     CssClass="single-selection"></asp:ListBox>

                                                            </td>
                                                            <td style="width: 8%;">
                                                                <asp:Button ID="btnGetData" runat="server" Text="GetStateWiseData" CssClass="button" BackColor="#0066CC" ForeColor="White" OnClick="btnGetData_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height=20px"></td>
                                                        </tr>
                                                        <%--space between 2 rows--%>
                                                        <tr>
                                                            <td style="width: 8%;">
                                                                <asp:Label ID="lblItemType" runat="server" Text="Item Type" Visible="False"></asp:Label>
                                                            </td>
                                                            <td style="width: 17%;">
                                                                <%--<asp:DropDownList ID="ddlItemType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlItemType_SelectedIndexChanged" CssClass="cbsam_select" Width="95%"></asp:DropDownList>--%>
                                                                     <asp:ListBox ID="ddlItemTypeNew" ClientIDMode="Static" runat="server"    AutoPostBack="True"
                                                                      OnSelsectedIndexChanged="ddlItemType_SelectedIndexChanged"  CssClass="single-selection"></asp:ListBox>
                                                                </td>
                                                            <td style="width: 8%;">
                                                                <asp:Label ID="ItemTypeCode" runat="server" Text="Item Code" Visible="True"></asp:Label>
                                                            </td>
                                                            <td style="width: 17%;">
                                                                <%--<asp:DropDownList ID="ddlItemName" runat="server" AutoPostBack="True" Width="95%" CssClass="cbsam_select"></asp:DropDownList>--%>
                                                                  <asp:ListBox ID="ddlItemNameNew" ClientIDMode="Static" runat="server" AutoPostBack="True" 
                                                                     CssClass="single-selection"></asp:ListBox>
                                                            </td>
                                                            <%--<td style="width: 8%;">GST TIN Reg. Date :</td>
                                              <td style="width: 17%;">
                                                <asp:TextBox ID="txtGSTtinRegistration" runat="server" Width="95%" CssClass="textfield" Height="13px" ReadOnly="true"/>
                                                  </td>
                                            </td>--%>
                                                            <%--          <td>
                                                <asp:Label ID="lblBeatName" runat="server" Text="Beat Name" Visible="False"></asp:Label>
                                </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBeatName" runat="server" AutoPostBack="True" CssClass="dropdownField" OnSelectedIndexChanged="ddlBeatName_SelectedIndexChanged" Visible="False" Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 8%;">
                                                &nbsp;</td>
                                            <td style="width: 17%;">
                                                &nbsp;</td>--%>
                                                        </tr>
                                                        <%--<tr>
                                          <td style="width: 8%;">Bill To Address</td>
                                            <td style="width: 17%;">
                                                <asp:TextBox ID="txtAddress" runat="server" CssClass="textfield" ReadOnly="True" TextMode="MultiLine" Width="95%" />
                                            </td>
                                          <td style="width: 8%;">Ship to Address</td>
                                            <td style="width: 17%;">
                                                <asp:DropDownList ID="ddlShipToAddress" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlShift_SelectedIndexChanged" CssClass="dropdownField" Width="95%" ></asp:DropDownList>
                                            </td>
                                            <td>Compositon Scheme :</td>
                                            <td>
                                                <asp:CheckBox ID="chkCompositionScheme" runat="server" ReadOnly="true" />
                                            </td>
                                            <td>Delivery Date</td>
                                            <td>
                                                <asp:TextBox ID="txtDeliveryDate" runat="server" CssClass="textfield" Width="95%" />
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MMM/yyyy" TargetControlID="txtDeliveryDate">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>--%>
                                                        <%--<tr>
                                        <td style="width: 8%;">Bill To State</td>
                                        <td style="width: 17%;">
                                            <asp:TextBox ID="txtBilltoState" runat="server" CssClass="textfield" Height="13px" ReadOnly="true" Width="95%" />
                                        </td>
                                        <td style="width: 8%;">&nbsp;</td>
                                        <td style="width: 17%;">&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                            </tr>--%>
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <%--  </ContentTemplate>
                            </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="height=20px"></td>
                            </tr>
                            <%--space between 2 rows--%>
                            <tr>
                                <td style="width: 100%; text-align: left">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnExport2Excel" />
                                            <asp:PostBackTrigger ControlID="ddlSchemeTypeNew" />
                                            
                                        </Triggers>
                                        <ContentTemplate>
                                            <div id="panelAdd" style="margin-top: 0px; width: 100%;">
                                                <asp:Panel ID="panelAddLine" runat="server" Width="100%">
                                                    <table style="width: 100%;" cellpadding="1" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 10%;">
                                                                <asp:Label ID="lblType" runat="server" Text="Scheme Type" Visible="True"></asp:Label></td>
                                                            </td>
                                                            
                                                            <td style="width: 10%;">
                                                                <asp:Label ID="lblSchemeType" runat="server" Text="Enter Percentage" Visible="True"></asp:Label>
                                                            </td>

                                                            <td style="width: 9%;">From Date</td>
                                                            <td style="width: 1%;"></td>
                                                            <td style="width: 9%;">To Date</td>

                                                            <td style="width: 1%;"></td>

                                                            <td style="width: 10%;">&nbsp;</td>



                                                            <td style="width: 10%;"></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 10%;">
                                                               <%-- <asp:DropDownList ID="ddlSchemeType" runat="server" Width="95%" AutoPostBack="true" OnSelectedIndexChanged="ddlSchemeType_SelectedIndexChanged">
                                                                </asp:DropDownList>--%>

                                                                <asp:ListBox ID="ddlSchemeTypeNew" ClientIDMode="Static" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="ddlSchemeType_SelectedIndexChanged"
                                                                    CssClass="single-selection"></asp:ListBox>
                                                            </td>
                                                            <td style="width: 10%;">
                                                                <asp:TextBox ID="txtValue" runat="server" Width="90%" CssClass="textfield" placeholder="0" onkeypress="return isNumberKey(event,this)" />

                                                            </td>
                                                            <td style="width: 9%;">
                                                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="textfield" Width="95%" onchange="ValidateDate(this)" required />
                                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtFromDate">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td style="width: 1%;"></td>
                                                            <td style="width: 9%;">
                                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="textfield" Width="95%" onchange="ValidateDate(this)" required />
                                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtToDate">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td style="width: 1%;"></td>

                                                            <td style="width: 10%;">
                                                                <asp:Button ID="BtnAddItem" runat="server" Text="Add SD" OnClick="BtnAddItem_Click" OnClientClick="return confirm('Do you want to save this record?');" CssClass="button" BackColor="#0066CC" ForeColor="White" TabIndex="1" />
                                                            </td>
                                                            <td style="width: 10%;">
                                                                <asp:Button ID="btnExport2Excel" runat="server" Text="Export To Excel" CssClass="button" BackColor="#0066CC" ForeColor="White" OnClick="btnExport2Excel_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Label ID="lblMessage" runat="server" AutoPostBack="true" ForeColor="Red"></asp:Label>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>

                        <div style="height: auto; overflow: auto; margin-top: 5px; margin-left: 0px; padding-right: 10px;">
                            <%--<asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>--%>
                            <asp:GridView runat="server" ID="gvDetails" GridLines="Horizontal" OnSelectedIndexChanged="OnStatusSelect" AutoGenerateColumns="False" Width="100%" BackColor="White"
                                BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True"
                                AllowPaging="false" PageSize="20" ShowFooter="True">
                                <AlternatingRowStyle BackColor="#CCFFCC" />
                                <Columns>
                                    <asp:BoundField HeaderText="SITEID" DataField="SITEID">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="SITENAME" DataField="SITENAME">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Customer Type" DataField="customertype">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Customer Type Code" DataField="customertypecode">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Item Type" DataField="ITEMTYPE">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Item Type Code" DataField="ITEMTYPECODE">
                                        <HeaderStyle HorizontalAlign="Left" Width="0px" />
                                        <ItemStyle HorizontalAlign="Left" Width="0px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="SD type" DataField="sdtype">
                                        <HeaderStyle HorizontalAlign="Left" Width="0px" />
                                        <ItemStyle HorizontalAlign="Left" Width="0px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="SD Value" DataField="sdvalue" DataFormatString="{0:n2}">
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="From Date" DataField="startingdate">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="TO Date" DataField="endingdate">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <%--<asp:BoundField HeaderText="Status" DataField="status" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>--%>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="Confirm" runat="server" Text='<%# Eval("Confirm")%>'></asp:Label>--%>
                                            <asp:LinkButton ID="lnkbtnstatus" runat="server" Text='<%#Eval("status") %>' OnClick="lnkbtnStatus_Click" OnClientClick="return confirm('Are you sure you want to change the status of this SD?');"></asp:LinkButton>
                                            <%--<asp:ButtonField Text='<%#Eval("status") %>' CommandName="Select" ItemStyle-Width="150" />--%>
                                        </ItemTemplate>

                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Remark" DataField="Remark">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hndcusttype" Value='<%# Eval("Ctype") %>' Visible="false" runat="server" />
                                            <asp:HiddenField ID="hnditemtype" Value='<%# Eval("Itype") %>' Visible="false" runat="server" />
                                            <asp:HiddenField ID="sdtype" Value='<%# Eval("Stype") %>' Visible="false" runat="server" />
                                            <asp:HiddenField ID="hndrecid" Value='<%# Eval("recid") %>' Visible="false" runat="server" />
                                        </ItemTemplate>
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



                        <div>
                            <asp:Button ID="Button2" runat="Server" Text="" Style="display: none;" />
                            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button2"
                                PopupControlID="Panel1" CancelControlID="Button4"
                                BackgroundCssClass="ModalPoupBackgroundCssClass" BehaviorID="ModalPopupExtender1">
                            </asp:ModalPopupExtender>
                            <%--<asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>--%>
                            <asp:Panel ID="Panel1" runat="server" Style="display: none; background-color: silver">
                                <div align="center">
                                    <span style="color: red; font-weight: 600; text-align: center">Upload Summary !!</span>

                                    <asp:Button ID="Button4" runat="server" CssClass="Operationbutton" data-dismiss="modal" Text="Close" aria-hidden="true"></asp:Button>
                                </div>
                                <p></p>
                                <div style="overflow-x: scroll; width: 100%; height: 200px">
                                    <asp:GridView ID="gridviewRemarks" AutoGenerateColumns="true" runat="server" Style="width: 100%" Font-Size="Small" CssClass="Grid" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
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
                            <%--   </ContentTemplate>
                    </asp:UpdatePanel>--%>
                        </div>
                    </td>
                </tr>
            </table>

        </ContentTemplate>
        <%--<Triggers>
            <asp:PostBackTrigger ControlID="BtnSave" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>

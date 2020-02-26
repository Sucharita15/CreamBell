<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmCreditNoteReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmSalesReturnReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <%-- <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />
    <link href="css/SSheet.css" rel="Stylesheet" type="text/css" />


     <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
  <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
  <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
                $(function () {
                    $('#lstState').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select State',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '140px',
                        maxHeight: 300
                    });
                    $('#lstSiteId').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select Site',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '140px',
                        maxHeight: 300,
                        maxWidth: 50
                    });
                });
            </script>--%>


      <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
  <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
  <link href="css/btnSearch.css" rel="stylesheet" /> 
    <link href="css/style.css" rel="stylesheet" /> 
        <link href="css/textBoxDesign.css" rel="stylesheet" />
        <link href="css/SSheet.css" rel="Stylesheet" type="text/css" />

  <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
  <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
  <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
                $(function () {
                    $('#lstState').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select State',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '140px',
                        maxHeight: 300
                    });
                    $('#lstSiteId').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select Site',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '140px',
                        maxHeight: 300,
                        maxWidth: 50
                    });

                    $('#drpPSRNew').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select PSR',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '140px',
                        maxHeight: 300,
                        maxWidth: 50
                    });

                    $('#drpBeatNew').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select BEAT',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '140px',
                        maxHeight: 300,
                        maxWidth: 50
                    });

                    $('#ddlSearchNew').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select All',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '140px',
                        maxHeight: 300,
                        maxWidth: 50
                    });

                    $('#drpCustomerNew').multiselect({
                        includeSelectAllOption: true,
                        nonSelectedText: 'Select Customer',
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '140px',
                        maxHeight: 300,
                        maxWidth: 50
                    });

                    $('#drpCustomerNew').multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '140px',
                        maxHeight: 300,
                        maxWidth: 50
                    });
                    
                });
            </script>

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





    <script src="Javascript/custom.js"></script>

    <script src="Javascript/DateValidation.js" type="text/javascript"></script>
    <script type="text/javascript">
        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        // row.style.backgroundColor = "aqua";
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        //and change rowcolor back to original
                        if (row.rowIndex % 2 == 0) {
                            //Alternating Row Color
                            //  row.style.backgroundColor = "#C2D69B";
                        }
                        else {
                            row.style.backgroundColor = "white";
                        }
                        inputList[i].checked = false;
                    }
                }
            }
        }


    </script>


    <script type="text/javascript">

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=GridView1.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=GridView1.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead1").append(gridHeader);
            $('#controlHead1').css('position', 'absolute');
            $('#controlHead1').css('top', '129');

        });


        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=GridView2.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=GridView2.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead2").append(gridHeader);
            $('#controlHead2').css('position', 'absolute');
            $('#controlHead2').css('top', '129');

        });

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

        function ActiveSearch()
        {
            var ddlSearchNew = $('#ddlSearchNew').val();
            if (ddlSearchNew != "All") {
                $('#txtSearch').removeAttr("disabled");
            }
            else {
                $('#txtSearch').attr("disabled", "disabled");
                $('#txtSearch').val("");
            }
        }
    </script>

    


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server" ClientIDMode="Static" >

    <asp:UpdatePanel ID="upanel" runat="server" style=" overflow-x: hidden;" UpdateMode="Conditional">
        <ContentTemplate>
               <asp:UpdatePanel runat="server" ID="upsale" UpdateMode="Conditional" ClientIDMode="Static">
                                <ContentTemplate>
                                    <asp:Panel ID="Panel1" runat="server"  GroupingText="Sale Person Filter" Style="width: 100%; margin: 0px 0px 0px 0px;">
                                            
                                        <div class="paneltbl">
                                        
                                                <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                                                    <span>HOS :</span>
                                                    <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="chkAll_CheckedChanged" Text="Select All"></asp:CheckBox>
                                                    <asp:CheckBoxList ID="chkListHOS" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstHOS_SelectedIndexChanged" ClientIDMode="Static" >
                                                    </asp:CheckBoxList>
                                                </div>



                                           
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <span> VP :</span>
                            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListVP" runat="server" OnSelectedIndexChanged="lstVP_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                           



                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <span> GM :</span>
                            <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox2_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListGM" runat="server" OnSelectedIndexChanged="lstGM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <span>DGM :</span>
                            <asp:CheckBox ID="CheckBox3" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox3_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListDGM" runat="server"  OnSelectedIndexChanged="lstDGM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                           
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <span> RM :</span>
                            <asp:CheckBox ID="CheckBox4" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox4_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListRM" runat="server"  OnSelectedIndexChanged="lstRM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                           
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <span> ZM :</span>
                            <asp:CheckBox ID="CheckBox5" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox5_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListZM" runat="server" OnSelectedIndexChanged="lstZM_SelectedIndexChanged"  AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                          
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <span> ASM :</span>
                            <asp:CheckBox ID="CheckBox6" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox6_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListASM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstASM_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </div>
                           
                        <div class="checkboxlistHeader"; style="overflow-y: auto;">
                            <span>EXECUTIVE :</span>
                            <asp:CheckBox ID="CheckBox7" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox7_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListEXECUTIVE" runat="server" OnSelectedIndexChanged="lstEXECUTIVE_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                                            </div>
                           
                                    </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                              
                                <%--<asp:PostBackTrigger ControlID="lstState" />--%>
                                <asp:PostBackTrigger ControlID="lstSiteId" />
                                <asp:PostBackTrigger ControlID="chkListEXECUTIVE" />
                                <asp:PostBackTrigger ControlID="chkListHOS" />
                                <asp:PostBackTrigger ControlID="chkListVP" />
                                <asp:PostBackTrigger ControlID="chkListGM" />
                                <asp:PostBackTrigger ControlID="chkListDGM" />
                                <asp:PostBackTrigger ControlID="chkListRM" />
                                <asp:PostBackTrigger ControlID="chkListZM" />
                                <asp:PostBackTrigger ControlID="chkListASM" />
                                <asp:PostBackTrigger ControlID="chkAll" />
                                <asp:PostBackTrigger ControlID="CheckBox1" />
                                <asp:PostBackTrigger ControlID="CheckBox2" />
                                <asp:PostBackTrigger ControlID="CheckBox3" />
                                <asp:PostBackTrigger ControlID="CheckBox4" />
                                <asp:PostBackTrigger ControlID="CheckBox5" />
                                <asp:PostBackTrigger ControlID="CheckBox6" />
                                <asp:PostBackTrigger ControlID="CheckBox7" />
                            </Triggers>
                       </asp:UpdatePanel>


            <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
            <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>

            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px;">
                Credit Note
            </div>


            <table style="width: 100%; vertical-align: top;">
                <tr>
                    <td colspan="10" style="text-align: center">
                        <asp:Label ID="LblMessage" runat="server" Font-Bold="True" Font-Names="Seoge UI" Font-Size="Small" ForeColor="DarkRed"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <%--<td class="auto-style1">From Date :</td>
                    <td style="text-align: left" class="auto-style10">
                        <asp:TextBox ID="txtFromDate" runat="server" Enabled="true"   placeholder="dd-MMM-yyyy" Width="100px" onchange="ValidateDate(this)" required ></asp:TextBox>
                        <asp:ImageButton ID="imgBtnFrom" runat="server" ImageAlign="Bottom"  ImageUrl="~/Images/calendar.jpg"  />
                    </td>
                    <td class="auto-style4">To Date :</td>
                    <td class="auto-style4">
                        <asp:TextBox ID="txtToDate" runat="server" Enabled="true" placeholder="dd-MMM-yyyy" Width="100px" onchange="ValidateDate(this)" required ></asp:TextBox>
                        <asp:ImageButton ID="imgBtnTo" runat="server" ImageAlign="Bottom" ImageUrl="~/Images/calendar.jpg" />
                    </td>--%>
                    <td style="text-align: left" colspan="4">
                        From Date :<asp:TextBox ID="txtFromDate" runat="server" Enabled="true" placeholder="dd-MMM-yyyy" Width="100px" OnTextChanged="txtFromDate_TextChanged" AutoPostBack="true" ></asp:TextBox>
                        <asp:ImageButton ID="imgBtnFrom" runat="server" Style="margin-right: 5px;" ImageAlign="Bottom"  ImageUrl="~/Images/calendar.jpg"   />
                   
                            To Date :<asp:TextBox ID="txtToDate" runat="server" Enabled="true" placeholder="dd-MMM-yyyy" Width="100px" onchange="ValidateDate(this)" required ></asp:TextBox>
                        <asp:ImageButton ID="imgBtnTo" runat="server" ImageAlign="Bottom" ImageUrl="~/Images/calendar.jpg" />
                    </td>
                    <td class="auto-style2">State :</td>
                    <td class="auto-style2">
                      <%--  <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" Height="22px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" Width="120px">
                        </asp:DropDownList>--%>
                            <asp:ListBox ID="lstState" ClientIDMode="Static" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"  CssClass="txt-width"></asp:ListBox>

                    </td>
                    <td class="auto-style2">Distributor : </td>
                    <td class="auto-style2">
                       <%-- <asp:DropDownList ID="ddlSiteId" runat="server" AutoPostBack="True" Height="22px" Width="150" OnSelectedIndexChanged="drpDIST_SelectedIndexChanged">
                        </asp:DropDownList>--%>
                                                    <asp:ListBox ID="lstSiteId"  ClientIDMode="Static" runat="server" SelectionMode="Multiple"  Width="200px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="true" ></asp:ListBox>

                    </td>
                    <td class="auto-style2">PSR : </td>
                    <td class="auto-style2">
                       <%-- <asp:DropDownList ID="drpPSR" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpPSR_SelectedIndexChanged" Width="150" class=" btn btn-default">
                        </asp:DropDownList>--%>

                        <asp:ListBox ID="drpPSRNew"  ClientIDMode="Static" runat="server" Width="200px" 
                            OnSelectedIndexChanged="drpPSR_SelectedIndexChanged" AutoPostBack="true" ></asp:ListBox>

                    </td>
                </tr>
                <tr>
                    <td>BEAT:</td>
                    <td>
                       <%-- <asp:DropDownList ID="drpBeat" class=" btn btn-default" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpBeat_SelectedIndexChanged" Width="150">
                        </asp:DropDownList>--%>

                          <asp:ListBox ID="drpBeatNew"  ClientIDMode="Static" runat="server" Width="200px" 
                            OnSelectedIndexChanged="drpBeat_SelectedIndexChanged" AutoPostBack="true" ></asp:ListBox>
                    </td>
                    <td class="auto-style2" style="text-align: right">
                       <%-- <asp:DropDownList ID="ddlSearch" runat="server" CssClass=" btn btn-default"  AutoPostBack="true" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged" data-toggle="dropdown" Style="margin-left: 0px" Width="150px">
                            <asp:ListItem>All</asp:ListItem>
                            <asp:ListItem>Sales Invoice No</asp:ListItem>
                            <asp:ListItem>Customer</asp:ListItem>
                        </asp:DropDownList>--%>

                          <select id="ddlSearchNew" runat="server" class='single-selection' onchange="ActiveSearch();">
                            <option selected="selected">All</option>
                            <option>Sales Invoice No</option>
                            <option>Customer</option>
                        </select>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSearch" CssClass="form-control" Enabled="false" runat="server" placeholder="Search here...." Width="150px"></asp:TextBox>
                    </td>


                    <%--       <td>InvoiceType :</td>
                    <td>
                        <asp:RadioButton ID="rdoBoth" runat="server" AutoPostBack="true" Text="All" Checked="true"
                            OnCheckedChanged="rdoBoth_CheckedChanged" GroupName="radio" />

                        <asp:RadioButton ID="rdoSI" runat="server" AutoPostBack="true" Text="SI"
                            OnCheckedChanged="rdoBoth_CheckedChanged" GroupName="radio" />

                        <asp:RadioButton ID="rdoSR" runat="server" AutoPostBack="true" Text="SR"
                            OnCheckedChanged="rdoBoth_CheckedChanged" GroupName="radio" />

                        <asp:RadioButton ID="rdoFOC" runat="server" AutoPostBack="true" Text="FOC"
                            OnCheckedChanged="rdoBoth_CheckedChanged" GroupName="radio" />

                    </td>--%>

                    <td class="auto-style2">InvoiceNo From:</td>
                    <td class="auto-style2">
                        <asp:TextBox ID="txtInvoiceNoStart" CssClass="form-control" runat="server" Enabled="true" onkeypress="return IsNumeric(event)" Width="100px"></asp:TextBox>
                    </td>
                    <td class="auto-style2">To :</td>
                    <td class="auto-style2">
                        <asp:TextBox ID="txtInvoiceNoEnd" runat="server" Enabled="true" onkeypress="return IsNumeric(event)" Width="100px"></asp:TextBox>
                    </td>
                    <td style="width: 5%">Customer   :</td>
                    <td>
                        <%--<asp:DropDownList ID="drpCustomer" class=" btn btn-default" runat="server" Width="150"></asp:DropDownList></td>--%>

                     <asp:ListBox ID="drpCustomerNew"  ClientIDMode="Static" runat="server" Width="150" ></asp:ListBox>

                    <td style="text-align: left">
                        <asp:Button ID="btnSearch" runat="server" CssClass="ReportSearch" Height="30px" Text="Search" Width="70px" OnClick="btnSearch_Click" />
                    </td>
                    <td>
                        <asp:Button ID="Button2" runat="server" Text="MultiplePrint" CssClass="ReportSearch" Height="31px" OnClick="Button2_Click" Visible="false" />
                    </td>
                </tr>
            </table>
              <div id="controlHead1" style="margin-top: 10px; margin-left: 10px; padding-right: 10px;">
            </div>
            <div style="overflow: auto; height: 260px; margin: 10px 0px 0px 10px;">
                <asp:GridView ID="GridView1" runat="server" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" OnRowDataBound="GridView1_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" AutoPostBack="true" OnCheckedChanged="checkAll_CheckedChanged" Text="All" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%--<asp:CheckBox ID="chklist" runat="server" Text='<%# Bind("INVOICE_NO") %>'   />--%>
                                <asp:CheckBox ID="chklist" Text="  " runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice No">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtn" runat="server" Text='<%# Bind("INVOICE_NO") %>' OnClick="lnkbtn_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Invoice Date" DataField="INVOIC_DATE" DataFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Width="120px" HorizontalAlign="Left" />
                            <ItemStyle Width="120px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SO_NO" HeaderText="Ref. Invoice No.">
                            <ControlStyle Width="250px" />
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="SiteId" DataField="SiteId" DataFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Width="120px" HorizontalAlign="Left" />
                            <ItemStyle Width="120px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Site Name" DataField="Name" DataFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Width="120px" HorizontalAlign="Left" />
                            <ItemStyle Width="120px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <%-- <asp:BoundField DataField="SO_DATE" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="SO Date">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>--%>

                        <asp:BoundField DataField="CUSTOMER" HeaderText="Customer">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="INVOICE_VALUE" HeaderText="Invoice Value" DataFormatString="Rs.{0:n}">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderImageUrl="~/Images/printicon.png">
                            <ItemTemplate>
                                <asp:HiddenField ID="hndSiteid" Value='<%# Eval("SITEID") %>' Visible="false" runat="server" />
                                <asp:HyperLink ID="HPLinkPrint" runat="server" Font-Bold="True">View</asp:HyperLink>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                    </Columns>
                    <EmptyDataTemplate>
                        No Record Found...
                    </EmptyDataTemplate>
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

            <div id="controlHead2" style="margin-top: 5px; margin-left: 5px; padding-right: 10px;"></div>
            <div style="overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px;">

                <asp:GridView ID="GridView2" runat="server" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">

                    <Columns>
                        <asp:BoundField HeaderText="Product Group" DataField="product_group">
                            <HeaderStyle Width="80px" />
                            <ItemStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="product_code" HeaderText="Product Code">
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Product Name" DataField="product_name">
                            <HeaderStyle Width="80px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CRATES" HeaderText="Invoice Qty(Crates)" DataFormatString="{0:n2}" Visible="False">
                            <HeaderStyle HorizontalAlign="Right" Width="80px" />
                            <ItemStyle HorizontalAlign="Right" Width="150px" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText=" Qty(Box)" DataField="BOXQTY" DataFormatString="{0:n2}">
                            <HeaderStyle Width="80px" HorizontalAlign="Right" />
                            <ItemStyle Width="150px" HorizontalAlign="Right" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Qty(PCS)" DataField="PCSQty" DataFormatString="{0:n2}">
                            <HeaderStyle Width="80px" HorizontalAlign="Right" />
                            <ItemStyle Width="150px" HorizontalAlign="Right" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Invoice Qty(BoxPcs)" DataField="BOXPCS" DataFormatString="{0:n2}">
                            <HeaderStyle Width="80px" HorizontalAlign="Right" />
                            <ItemStyle Width="150px" HorizontalAlign="Right" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Total Qty(Box)" DataField="BOX" DataFormatString="{0:n2}">
                            <HeaderStyle Width="80px" HorizontalAlign="Right" />
                            <ItemStyle Width="150px" HorizontalAlign="Right" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Invoice Qty(LTR)" DataField="LTR" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="MRP" DataField="MRP" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="rate" DataFormatString="{0:n2}" HeaderText="Rate">
                            <HeaderStyle HorizontalAlign="Right" Width="80px" />
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="tax_code" DataFormatString="{0:n2}" HeaderText="Tax Code">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="amount" DataFormatString="{0:n2}" HeaderText="Value">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        No Record Found...
                    </EmptyDataTemplate>
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
            <rsweb:ReportViewer ID="ReportViewer1" Visible="false" runat="server" Width="100%" Height="800px">
            </rsweb:ReportViewer>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Button2" />
     <asp:PostBackTrigger ControlID="btnSearch" />

        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

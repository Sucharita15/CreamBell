<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmIndentVSInvoice.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmIndentVSInvoice" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
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

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=GridView1.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=GridView1.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead").append(gridHeader);
            $('#controlHead').css('position', 'absolute');
            $('#controlHead').css('top', '129');

        });

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=GridView1.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=GridView1.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#ControlHead1").append(gridHeader);
            $('#ControlHead1').css('position', 'absolute');
            $('#ControlHead1').css('top', '129');

        });

    </script>
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


    <style type="text/css">
        /*DropDownCss*/
        .ddl {
            background-image: url('Images/arrow-down-icon-black.png');
        }

            .ddl:hover {
                background-image: url('Images/arrow-down-icon-black.png');
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

   
     <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
      <Triggers>
            
            <asp:PostBackTrigger ControlID="LinkButton1" />
            <asp:PostBackTrigger ControlID="chkListHOS" />
            <asp:PostBackTrigger ControlID="chkListVP" />
            <asp:PostBackTrigger ControlID="chkListGM" />
            <asp:PostBackTrigger ControlID="chkListDGM" />
            <asp:PostBackTrigger ControlID="chkListRM" />
            <asp:PostBackTrigger ControlID="chkListZM" />
            <asp:PostBackTrigger ControlID="chkListASM" />
                   </Triggers>  
      <ContentTemplate>
         
     
        <div style="width: 100%;height: 18px;border-radius: 4px;margin: 5px 0px 0px 10px;background-color: #1564ad;color:white ;padding: 2px 0px 0px 0px; font-weight:bold" >
           <table>
                    <tr>
                        <td id="tclink" runat="server" style="text-align:left;">
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" ForeColor="White">Hide sales person filter</asp:LinkButton>
                        </td>
                        <td runat="server" style=" text-align:center" id="tclabel" >
                            Indent Vs Invoice
                        </td>
                    </tr>
                </table></div>

    <%--<asp:UpdatePanel runat="server" ID="dsdas">
        <ContentTemplate>--%>

    <table style="width: 100%; text-align: left">
        <tr>
            <td style="width: 100%">
                <asp:Panel ID="Panel1" runat="server" GroupingText="Sale Person Filter" Style="width: 100%; margin: 0px 0px 0px 2px;">
                                <table style="width: 100%">
                                   
                               <tr>
                              
                                 
                                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 11%;">HOS :
                                   <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                         <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="chkAll_CheckedChanged" Text="Select All" />
                         <asp:CheckBoxList ID="chkListHOS" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstHOS_SelectedIndexChanged" >
                            </asp:CheckBoxList>
                        </div>
                     
                            </td>
                                   <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 11%;">VP :
                        <div class="checkboxlistHeader"; style="max-height: 80px; width:150px;overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListVP" runat="server" OnSelectedIndexChanged="lstVP_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                            <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 11%;">GM :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox2_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListGM" runat="server" OnSelectedIndexChanged="lstGM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                                             <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 13%;">DGM :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox3" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox3_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListDGM" runat="server"  OnSelectedIndexChanged="lstDGM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                             <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 14%;">RM :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox4" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox4_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListRM" runat="server"  OnSelectedIndexChanged="lstRM_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                                             <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 14%;">ZM :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox5" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox5_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListZM" runat="server" OnSelectedIndexChanged="lstZM_SelectedIndexChanged"  AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                                             <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 14%;">ASM :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox6" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox6_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListASM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstASM_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                                                        <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 16%;">EXECUTIVE :
                        <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                            <asp:CheckBox ID="CheckBox7" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox7_CheckedChanged" Text="Select All" />
                            <asp:CheckBoxList ID="chkListEXECUTIVE" runat="server" OnSelectedIndexChanged="lstEXECUTIVE_SelectedIndexChanged" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </div>
                            </td>
                                </tr>
                                </table>
                      </asp:Panel>
                        </td>
                    </tr>
                </table>
          <table style="width: 100%; text-align: left">
                    <tr>
                        <td style="width: 8%">From Date :</td>
                        <td style="width: 15%">
                            <asp:TextBox ID="txtFromDate"  runat="server" placeholder="dd-MMM-yyyy" />
                            <asp:ImageButton ID="imgFromDate" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgFromDate" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                        </td>
                        <td style="width: 8%">
                            To Date:
                        </td>
                        <td style="width: 15%">
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy"/>
                            <asp:ImageButton ID="imgToDate" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgToDate" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                        </td>
                    </tr>
                
            </td>
           <tr>
            <td style="width: 5%">
                Indent No: 
            </td>
         
            <td>
                <asp:TextBox ID="txtIndentNo" runat="server"/>
            </td>
             
                        <td style="width: 10%; text-align: left"><span id="span1" onmouseout="test1()" onmouseover="test()">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btn2" runat="server" OnClick="btn2_Click" Style="margin: 0px 0px 0px 2px" Text="Search" Width="80px" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            </span></td>
                    </tr>
              </table>

        </tr>
    </table>
     </ContentTemplate>
    </asp:UpdatePanel>

    <div>
        <div id="controlHead" style="margin-top: 0px; margin-left: 9px; padding-right: 10px;"></div>
        <div style="height: 400px; overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px;">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="100%"
                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                        ShowHeaderWhenEmpty="True" PageSize="5">
                        <AlternatingRowStyle BackColor="#CCFFCC" />
                        <Columns>
                            <asp:TemplateField HeaderText="Indent Number">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIndentNumber" runat="server" Text='<%# Bind("INDENTNUMBER") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Indent Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIndentDate" runat="server" Text='<%# Bind("INDENTDATE","{0:d}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="PO Number">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPONumber" runat="server" Text='<%# Bind("PONUMBER") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="PO Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPODate" runat="server" Text='<%# Bind("PODATE","{0:d}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="SO Number">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSONumber" runat="server" Text='<%# Bind("SONUMBER") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="SO Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSODate" runat="server" Text='<%# Bind("SODATE","{0:d}") %>' ></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Invoice Number">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%# Bind("INVOICENUMBER") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Invoice Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Bind("INVOICEDATE","{0:d}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Supplying Plant/Depot">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSupplyPlantDepot" runat="server" Text='<%# Bind("PLANTCODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Plant/Depot Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSupplyPlantDepotName" runat="server" Text='<%# Bind("PLANTNAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Receiving Distributor">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDistributorCode" runat="server" Text='<%# Bind("DISTRIBUTORCODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                 <asp:TemplateField HeaderText="Distributor Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDistributorName" runat="server" Text='<%# Bind("DISTRIBUTORNAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Material Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMaterialCode" runat="server" Text='<%# Bind("MATERIALCODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Material Descriptrion">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMaterialName" runat="server" Text='<%# Bind("MATERIALNAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Order Qty">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOrderQty" runat="server" Text='<%# Bind("ORDERQTY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Issued Qty">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIssuedQty" runat="server" Text='<%# Bind("ISSUEDQTY") %>'></asp:Label>
                                                                    </ItemTemplate>
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
             
                 </ContentTemplate>
            </asp:UpdatePanel>


        </div>

    </div>

    <div id="fixedHeaderRow" runat="server" style="height: 30px; width: 100%; margin: 0; padding: 0">
    </div>
    
</asp:Content>

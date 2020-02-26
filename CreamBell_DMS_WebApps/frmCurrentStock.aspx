<%@ Page Language="C#" Title="Current Stock" MasterPageFile="~/Main.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="frmCurrentStock.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmCurrentStock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ccp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ccp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="css/btnSearch.css" rel="stylesheet" />  
    <link href="css/style.css" rel="stylesheet" />
  <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
  <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
  <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            //debugger;
            $('#lstState').multiselect({
                includeSelectAllOption: true,
                nonSelectedText: 'Select State',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '200px',
                maxHeight: 300
            });
            $('#lstSiteId').multiselect({
                includeSelectAllOption: true,
                nonSelectedText: 'Select Site',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '200px',
                maxHeight: 300,
                maxWidth: 50
            });


            $('#ddlWarehouse').multiselect({
                includeSelectAllOption: true,
                nonSelectedText: 'Select Warehouse',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '220px',
                maxHeight: 300,
                maxWidth: 50
            });



        });
            </script>
    <script src="Javascript/custom.js"></script>
    <style type="text/css">
        .ReportSearch {
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

            .ReportSearch:hover {
                background: #3cb0fd;
                background-image: -webkit-linear-gradient(top, #3cb0fd, #3498db);
                background-image: -moz-linear-gradient(top, #3cb0fd, #3498db);
                background-image: -ms-linear-gradient(top, #3cb0fd, #3498db);
                background-image: -o-linear-gradient(top, #3cb0fd, #3498db);
                background-image: linear-gradient(to bottom, #3cb0fd, #3498db);
                text-decoration: none;
            }

            .nav ul li a{
                width:270px !important;
            }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gridCurrentStcok.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=gridCurrentStcok.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead2").append(gridHeader);
            $('#controlHead2').css('position', 'absolute');
            $('#controlHead2').css('top', '129');

        });
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel runat="server" ID="uppanel" UpdateMode="Conditional">
                <Triggers>
            <%--<asp:PostBackTrigger ControlID="lstState" />
            <asp:PostBackTrigger ControlID="lstSiteId" />--%>
            <%--<asp:PostBackTrigger ControlID="ddcbWarehouse" />--%>
            <asp:PostBackTrigger ControlID="ddlBuunit" />
            <asp:PostBackTrigger ControlID="imgBtnExportToExcel" />
            <asp:PostBackTrigger ControlID="btnShow" />
        </Triggers>
        <ContentTemplate runat="server">
            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
        <table>
              <tr>
                  <td runat="server" style=" text-align:center" id="tclabel" >
                      Current Stock
                  </td>
              </tr>
        </table>
    </div>

           <asp:UpdatePanel ID="pnlupd" runat="server">
                       <ContentTemplate>
                    
                    <asp:Panel ID="pnlHeader" runat="server"  GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 2px;">
                    <table style="width: 100%; margin:0px 0px 0px 0px;">
                    <tr>
                        <td colspan="9">
                            <asp:UpdatePanel runat="server" ID="upsale" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="Panel1" runat="server"  GroupingText="Sale Person Filter" Style="width: 99%; margin: 0px 0px 0px 2px;">
                                    <table>
                                        <tr>
                                            <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 11%;">HOS :
                                                <div class="checkboxlistHeader"; style="max-height: 80px; overflow-y: auto;">
                                                    <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="chkAll_CheckedChanged" Text="Select All"></asp:CheckBox>
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
                   </td>
                    </tr>
                        </table>
                   <table style="width: 100%; margin:0px 0px 0px 0px;">
                    <tr style="width:100%">
                        <td style="width:0%">
                            <asp:Label ID="lblWareHouse" runat="server" Visible="false" Text="" Font-Bold="true" ForeColor="DarkBlue" Font-Names="Seoge UI"> </asp:Label>
                            <asp:Label ID="LblMessage" runat="server" Text="" Visible="false" Font-Bold="true" ForeColor="DarkBlue" Font-Names="Seoge UI"> </asp:Label>
                        </td>
                                <td style="width:auto">State : 
                            <asp:ListBox ID="lstState" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" ClientIDMode="Static" CssClass="txt-width"></asp:ListBox>
                        </td>
                        
                        <td style="width:auto">Site ID : 
                            <asp:ListBox ID="lstSiteId" runat="server" SelectionMode="Multiple" AutoPostBack="true" ClientIDMode="Static" CssClass="txt-width" OnSelectedIndexChanged="ddldistributor_SelectedIndexChanged"></asp:ListBox>
                        </td>


                          <td style="width:auto">Warehouse : 
                            <asp:ListBox ID="ddlWarehouse" runat="server" SelectionMode="Multiple" ClientIDMode="Static" CssClass="txt-width" Width="220px"></asp:ListBox>
                        </td>

                        <%--<td style="width:20%">Warehouse :       
                            <asp:DropDownCheckBoxes ID="ddcbWarehouse" runat="server" Width="100%" UseSelectAllNode="true" Visible="false">
                                <Style SelectBoxWidth="90%" DropDownBoxBoxWidth="100%" DropDownBoxBoxHeight="90" />
                            </asp:DropDownCheckBoxes>

                            <asp:ExtendedRequiredFieldValidator ID="ExtendedRequiredFieldValidator3" runat="server" ControlToValidate="ddcbWarehouse" ErrorMessage="Required" ForeColor="Red"></asp:ExtendedRequiredFieldValidator>
                        </td>--%>

                        <td style="width:auto">Business Unit :</td>                      
                        <td style="width:auto">    
                            <asp:DropDownList ID="ddlBuunit" runat="server" CssClass="form-control dropdown txt-width"></asp:DropDownList>
                        </td>
                        <td style="width:auto">
                            <asp:Button ID="btnShow" runat="server" AutoPostBack="True" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" CssClass="ReportSearch" Height="31px" OnClick="btnShow_Click" Text="Show" Width="70px" />
                        </td>
                        <td style="width:auto">
                            <asp:ImageButton ID="imgBtnExportToExcel" runat="server" ImageUrl="~/Images/excel-24.ico" OnClick="imgBtnExportToExcel_Click" ToolTip="Click To Generate Excel Report" />
                        </td>
                        </tr>
            </table>
        </asp:Panel>                   
    </ContentTemplate>
           </asp:UpdatePanel>
           <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; table-layout: fixed;">
                <tr>
            <td style="width: 100%; vertical-align: top; text-align: left;">

                <div style="overflow: auto; height: 450px; margin-top: 10px; margin-left: 5px; padding-right: 10px;">

                    <asp:GridView runat="server" ID="gridCurrentStcok" ShowFooter="False" GridLines="Horizontal" AutoGenerateColumns="true" Width="100%" Height="450px" BackColor="White"
                        BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">

                        <AlternatingRowStyle BackColor="#CCFFCC" />

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
            </td>
        </tr>
           </table>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
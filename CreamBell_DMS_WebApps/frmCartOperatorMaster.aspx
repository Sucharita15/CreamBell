<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="frmCartOperatorMaster.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmCartOperatorMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ccp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
     <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
   <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
   <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
  <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
  <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
                $(function () {
                    //debugger;
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
            </script>

    <script src="Javascript/custom.js"></script>

     <script type="text/javascript">

         $(document).ready(function () {
             /*Code to copy the gridview header with style*/
             var gridHeader = $('#<%=gridViewCustomers.ClientID%>').clone(true);
             /*Code to remove first ror which is header row*/
             $(gridHeader).find("tr:gt(0)").remove();
             $('#<%=gridViewCustomers.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead").append(gridHeader);
            $('#controlHead').css('position', 'absolute');
            $('#controlHead').css('top', '129');

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
        /*.notbold{
                text-align: left;
                vertical-align: top;
                font-family: "Trebuchet MS", "Helvetica", "Arial", "Verdana", "sans-serif";
                font-size: 85.5%;
            }*/
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

           .report-table table tr th{
                font-weight:normal !important;

            }

           .btn-group {
    width: 155px !important;
}



    </style>

   <style type="text/css">
       /*DropDownCss*/
       .ddl {
           background-color: #eeeeee;
           padding: 5px;
           border: 1px solid #7d6754;
           border-radius: 4px;
           padding: 3px;
           -webkit-appearance: none;
           background-image: url('Images/arrow-down-icon-black.png');
           background-position: right;
           background-repeat: no-repeat;
           text-indent: 0.01px; /*In Firefox*/
           text-overflow: ''; /*In Firefox*/
       }

           .ddl:hover {
               background: #add8e6;
               background-image: url('Images/arrow-down-icon-black.png');
               background-position: right;
               background-repeat: no-repeat;
               text-indent: 0.01px; /*In Firefox*/
               text-overflow: ''; /*In Firefox*/
           }

            .nav ul li a{
                width:270px !important;
            }
   </style>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport2Excel" />
            <asp:PostBackTrigger ControlID="BtnShowReport0" />
            <asp:PostBackTrigger ControlID="btnSearchCustomer" />
            </Triggers>
        <ContentTemplate>
            <div style="width: 100%; height: 20px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 0px 0px 0px 0px; font-weight: bold">
                        <table>
                            <tr>
                                <td runat="server" style=" text-align:center" id="tclabel" >
                                    Cart Operator Master
                                </td>
                            </tr>
                        </table>
                    </div>
            <div>
                <asp:UpdatePanel ID="pnlupd" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlHeader" runat="server"  GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 2px;">
                    <table style="width: 100%; height: 99px; margin:0px 0px 0px 0px">

                    <tr>
                        <td colspan="9">
                            <asp:UpdatePanel runat="server" ID="upsale" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="Panel1" runat="server"  GroupingText="Sale Person Filter"  Style="width: 99%; margin: 0px 0px 0px 2px;">
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
                              
                                <asp:PostBackTrigger ControlID="lstState" />
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
                    <tr>                       
                       
                        <td>State : 
                            <asp:ListBox ID="lstState" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" ClientIDMode="Static"></asp:ListBox>
                             <%--<asp:DropDownCheckBoxes ID="ddlState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" CssClass="auto-style13">
                            <Style SelectBoxWidth="170" DropDownBoxBoxHeight="250" DropDownBoxBoxWidth="170" >
                            </Style>
                             </asp:DropDownCheckBoxes>--%>
                             <%--<asp:DropDownCheckBoxes ID="ddlState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" CssClass="auto-style13">
                            <Style SelectBoxWidth="170" DropDownBoxBoxHeight="250" DropDownBoxBoxWidth="170" >
                            </Style>
                             </asp:DropDownCheckBoxes>--%>
                        </td>
                        <td>SiteID : 
                        
                            <asp:ListBox ID="lstSiteId" runat="server" SelectionMode="Multiple" ClientIDMode="Static"></asp:ListBox>
                            <%--<asp:DropDownCheckBoxes ID="ddlSiteId" runat="server"  CssClass="auto-style13">
                                <Style SelectBoxWidth="170" DropDownBoxBoxHeight="250" DropDownBoxBoxWidth="270" >
                                </Style>
                             </asp:DropDownCheckBoxes>--%>
                        </td>

                        <td>Search Customer By :
                                    <asp:DropDownList ID="DDLSearchType" runat="server" CssClass="multiselect dropdown-toggle btn btn-default" data-toggle="dropdown" Width="200px" Style="text-align: right" >
                                        <asp:ListItem>Customer Code</asp:ListItem>
                                        <asp:ListItem>Customer Name</asp:ListItem>
                                        <asp:ListItem>PSR Code</asp:ListItem>
                                    </asp:DropDownList>
                                        
                            </td>

                        <td>
                                <div>
                                    <asp:TextBox ID="txtSearch" runat="server" placeholder="Search here..." Width="150px"  CssClass="multiselect dropdown-toggle btn btn-default" />
                                    <span id="span1" onmouseover="test()" onmouseout="test1()">
                                        <asp:Button ID="btnSearchCustomer" runat="server" Style="margin: 0px 0px 0px -2px" Height="31px" Text="Search"
                                            OnClick="btnSearchCustomer_Click"></asp:Button>
                                    </span>
                                </div>
                            </td>

                        <td>
                            <asp:Button ID="BtnShowReport0" runat="server" CssClass="ReportSearch" Height="31px" OnClick="BtnShowReport_Click" Text="Show Report" Width="96px" />
                        </td>                   

                    <td><asp:Button ID="btnExport2Excel" runat="server" Text="Export To Excel" Height="31px" OnClick="btnExport2Excel_Click"></asp:Button></td>      

                     </tr>
                    <tr>
                        <td colspan="3"> <b>Filter Customer By Status:&nbsp;</b>
                            <asp:RadioButton ID="rdRunningC" runat="server"  Text="Running&nbsp;&nbsp;" ToolTip="RunningCustomers" Checked="true" ValidationGroup="Customers" GroupName="RdCustomers" />
                            <asp:RadioButton ID="rdBlockC" runat="server"  Text="Block&nbsp;&nbsp;" CheToolTip="BlockCustomers" ValidationGroup="Customers" GroupName="RdCustomers" />
                            <asp:RadioButton ID="rdBothC" runat="server" Text="Both&nbsp;&nbsp;" ToolTip="BothCustomers" ValidationGroup="Customers" GroupName="RdCustomers" />
                        </td>
                    </tr>    
                    
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
         </div>

    <div style="background-color:#c5d9f1;width:100%" >      

        
             <%--<div id="controlHead" style="margin-top:5px; margin-left:5px;padding-right:10px;width:100%"></div>--%>
                      <div style="height:auto;overflow: auto;margin-top:5px; height:387px;width:100%">

   <%-- <div style="overflow:auto;height:400px;margin: 10px 0px 0px 10px;" > --%>

        <asp:GridView ID="gridViewCustomers" runat="server" AutoGenerateColumns="False"  ShowFooter="False" Width="100%"  
             BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" 
             OnRowDataBound="gridViewCustomers_RowDataBound" OnPageIndexChanging="gridViewCustomers_PageIndexChanging" CssClass="report-table" >

             <AlternatingRowStyle BackColor="#CCFFCC" />
             <Columns>
                <%-- <asp:BoundField HeaderText="Contact Person" DataField="Contact_Name" >
                 <HeaderStyle Width="80px" ForeColor="White"  />
                 </asp:BoundField>--%>
                 <%--<asp:BoundField HeaderText="Address2"  DataField="Address2">
                 <HeaderStyle Width="80px" ForeColor="White" />
                 </asp:BoundField>--%>
                <%-- <asp:BoundField HeaderText="Area"  DataField="Area">
                 <HeaderStyle ForeColor="White" />
                 </asp:BoundField>
                 <asp:BoundField HeaderText="Distict" DataField="District" >
                 <HeaderStyle Width="80px" ForeColor="White" />
                 </asp:BoundField>--%>
                 <%--<asp:BoundField HeaderText="Phone No"  DataField="Phone_No">
                 <HeaderStyle Width="80px" ForeColor="White" />
                 </asp:BoundField>
                 <asp:BoundField HeaderText="Email ID" DataField="EmailId" >
                 <HeaderStyle ForeColor="White" />
                 </asp:BoundField>--%>
                 <asp:TemplateField>
                     <ItemTemplate>
                         <asp:LinkButton ID="LnkView" runat="server"  OnClick="LnkView_Click" CommandArgument='<%# Bind("Customer_Code") %>' Text="View"></asp:LinkButton>
                     </ItemTemplate>
                     <HeaderStyle ForeColor="White" HorizontalAlign="Left" Width="20px" />
                     <ItemStyle HorizontalAlign="Left" />
                 </asp:TemplateField>

                 <asp:BoundField HeaderText="Customer Code" DataField="Customer_Code" >
                 <HeaderStyle Width="35px" ForeColor="White" />
                 <%--<ItemStyle Font-Bold="True" Font-Names="Segoe UI" ForeColor="#009900" />--%>
                 </asp:BoundField>
                 <asp:BoundField HeaderText="Customer Name" DataField="Customer_Name" >
                 <HeaderStyle Width="100px" ForeColor="White"  />
                 </asp:BoundField>
                 <asp:BoundField HeaderText="Address1" DataField="Address1" >
                 <HeaderStyle Width="120px" ForeColor="White"  />
                 </asp:BoundField>
                 
                 <asp:BoundField HeaderText="Mobile No" DataField="Mobile_No" >
                 <HeaderStyle ForeColor="White" Width="50px" />
                 <ItemStyle HorizontalAlign="Center" />
                 </asp:BoundField>
                 <asp:BoundField HeaderText="City" DataField="City" >
                 <HeaderStyle ForeColor="White" Width="50px" />
                 <ItemStyle HorizontalAlign="Center"  />
                 </asp:BoundField>
                 <asp:BoundField HeaderText="ZipCode" DataField="ZipCode" >
                 <HeaderStyle ForeColor="White"  Width="30px"/>
                 <ItemStyle HorizontalAlign="Center" />
                 </asp:BoundField>
                 <asp:BoundField HeaderText="State" DataField="State" >
                 <HeaderStyle ForeColor="White" Width="50px" />
                 <ItemStyle HorizontalAlign="Center" />
                 </asp:BoundField>
                 <asp:BoundField HeaderText="PSR Code" DataField="PSR_CODE" >
                 <HeaderStyle ForeColor="White" Width="50px" />
                 <ItemStyle HorizontalAlign="Center" />
                 </asp:BoundField>
                 <asp:BoundField HeaderText="PSR Beat" DataField="PSRBEATNAME" >
                 <HeaderStyle ForeColor="White" Width="50px" />
                 <ItemStyle HorizontalAlign="Center" />
                 </asp:BoundField>
                  <asp:BoundField HeaderText="Deep Freezer" DataField="DEEP_FRIZER" >
                 <HeaderStyle ForeColor="White" Width="50px" />
                 <ItemStyle HorizontalAlign="Center" />
                 </asp:BoundField>
                 <asp:BoundField HeaderText="VRS Code" DataField="VRSCODE" >
                 <HeaderStyle ForeColor="White" Width="50px" />
                 <ItemStyle HorizontalAlign="Center" />
                 </asp:BoundField>
                 <asp:BoundField HeaderText="VRS Name" DataField="VRSNAME" >
                 <HeaderStyle ForeColor="White" Width="50px" />
                 <ItemStyle HorizontalAlign="Center" />
                 </asp:BoundField>
             </Columns>
             <EmptyDataTemplate>
                 No Record Found...
             </EmptyDataTemplate>
             <FooterStyle BackColor="#bfbfbf" />
             <HeaderStyle BackColor="#05345C"  ForeColor="#000000" />
             <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
             <RowStyle ForeColor="#000066" />
             <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
             <SortedAscendingCellStyle BackColor="#F1F1F1" />
             <SortedAscendingHeaderStyle BackColor="#007DBB" />
             <SortedDescendingCellStyle BackColor="#CAC9C9" />
             <SortedDescendingHeaderStyle BackColor="#00547E" />
         </asp:GridView>
    </div>  
</div>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>

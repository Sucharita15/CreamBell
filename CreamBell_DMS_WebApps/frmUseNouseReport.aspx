<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmUseNouseReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmUseNouseReport" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/SSheet.css" rel="Stylesheet" type="text/css" />
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />
    <script src="Javascript/DateValidation.js" type="text/javascript"></script>
    <%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>

  
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

        .hiddencol {
            display: none;
        }
    </style>

    <style type="text/css">
        .pnlCSS {
            font-weight: bold;
            cursor: pointer;
            /*border: solid 1px #c0c0c0;*/
            margin-left: 20px;
        }

        .auto-style2 {
            width: 6%;
        }
        .auto-style3 {
            width: 8%;
            height: 24px;
        }
        .auto-style4 {
            width: 17%;
            height: 24px;
        }
        .cand {
            position:relative;
        }
        .callender {
            position: absolute;
            right: 14px;
            top: 5px;
        }
        
    </style>
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
        <b>Use/Non-Use Report</b>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
       <Triggers>
           <asp:PostBackTrigger ControlID="ImDnldTemp" />
       </Triggers>
        <ContentTemplate>
       <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
            <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                 <table style="width: 100%">
                <tr>
                    <td style="width: 100%; text-align: center; vertical-align: top">
                        <table style="width: 100%; text-align: left" cellpadding="0" cellspacing="0">
                           
                            <tr>
                                <td style="width: 100%">
                                     <%-- <div style="background-color: #C0C0C0; text-align: center; border-style: solid; border-width: thin; font-weight: bold; font-size: 15px; width: 100%">
                                            <strong>Use/Non-Use Report</strong>
                                        </div>--%>
                          
                                    <table style="width: 100%" cellpadding="1" cellspacing="0">
                                       
                                        <tr>
                                          
                                            <td style="width: 8%; text-align:right" class="auto-style3" >From Date : </td>
                                            <td style="width: 17%;" class="auto-style4 cand">
                                                <asp:TextBox ID="txtFromDate" runat="server"  placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px"  onchange="ValidateDate(this)" required></asp:TextBox> 
                                                <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                                                <%--<asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MMM/yyyy" TargetControlID="txtFromDate">
                                                </asp:CalendarExtender>--%>
                                              <%--<div class="callender"><img src="Images/calendar.png"  /> </div> --%> 
                                                
                                            </td >


                                           
                                            
                                            <td style="width: 8%; text-align:right" class="auto-style3">To Date : </td>
                                            <td style="width: 17%;" class="auto-style4 cand">
                                                <asp:TextBox ID="txtToDate" runat="server"  placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px" onchange="ValidateDate(this)" required></asp:TextBox> 
                                                <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                                                <%--<asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MMM/yyyy" TargetControlID="txtToDate" >
                                                </asp:CalendarExtender>--%>
                                                <%--<div class="callender "><img src="Images/calendar.png"  /> </div>  --%>
                                            </td>
                                            <td style="width: 8%; text-align:right">State : </td>
                                            <td style="width: 17%;">
                                                <asp:DropDownList ID="ddlstate" runat="server" CssClass="dropdownField" Width="95%" ></asp:DropDownList>
                                            </td>        
                                              <td  colspan="2" style="width: 20%;">
                                                    <asp:RadioButton ID="rdWorking" runat="server" Text="Working" ToolTip="Working User"  GroupName="Rd" />
                                                   <asp:RadioButton ID="rdNotWorking" runat="server" Text="Not Working" ToolTip="Non Working User" GroupName="Rd" />
                                                    <asp:RadioButton ID="rdAll" runat="server" Text="All" Checked="true" ToolTip="All User" GroupName="Rd"  />

                                                </td>                                   

                                        </tr>
                                         
                                    <tr >
                                       <td  colspan ="8" style="width: 5%; text-align:center;">
                                            <asp:Button  ID="btnReport" runat="server" Text="Show Report" CssClass="ReportSearch" OnClick="btnReport_Click" />
                                        </td>
                                                 <td style="width: 5%;">

                                                    <asp:ImageButton ID="ImDnldTemp" runat="server" AutoPostBack="true" src="Images/DownloadTemplate.gif" OnClick="ImDnldTemp_Click" ToolTip="Click to download excel template !!" />

                                                </td>
                                    </tr>
                                    </table>
                                    <%--  </ContentTemplate>
                            </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            
                        </table>

                        <div style="height: auto; overflow: auto; margin-top: 5px; margin-left: 0px; padding-right: 10px;">
                            <%--<asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>--%>
                            <asp:GridView runat="server" ID="gvDetails" GridLines="Horizontal" AutoGenerateColumns="true" Width="100%" BackColor="White"
                                BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True"
                                AllowPaging="false" PageSize="20" ShowFooter="True">
                                <AlternatingRowStyle BackColor="#CCFFCC" />
                                <Columns>
                                    <%--<asp:TemplateField Visible="false" ItemStyle-Width="0px">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="HiddenField2" Visible="false" runat="server" Value='<%# Eval("SNO") %>' />
                                            <asp:HiddenField ID="HiddenFieldCalculation" Visible="false" runat="server" Value='<%# Eval("CalculationBase") %>' />
                                            <asp:HiddenField ID="hdnBasePrice" Visible="false" runat="server" Value='<%# Eval("BasePrice") %>' />
                                            <asp:HiddenField ID="hdnTaxableAmount" Visible="false" runat="server" Value='<%# Eval("TaxableAmount") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="0px"></ItemStyle>
                                    </asp:TemplateField>--%>

                                    
                                <%--    <asp:BoundField HeaderText="State Code" DataField="StateCode">

                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="State Name" DataField="StateName">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Site Code" DataField="SiteCode">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField HeaderText="Site Name" DataField="SiteName">
                                        <HeaderStyle HorizontalAlign="Left" Width="0px" />
                                        <ItemStyle HorizontalAlign="Left" Width="0px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="No. of Bills" DataField="bills" >
                                        <HeaderStyle HorizontalAlign="Left" Width="0px" />
                                        <ItemStyle HorizontalAlign="Left" Width="0px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Total LTR" DataField="ltr" DataFormatString="{0:n2}">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Total QTY" DataField="qty" DataFormatString="{0:n2}">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Amount" DataField="amount" DataFormatString="{0:n2}">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Status" DataField="status" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="No. of PSR" DataField="psr" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="No. of VSR" DataField="vsr" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField HeaderText="Distributor/ Sub-Distributor" DataField="Distributor">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>--%>

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
                       
                    </td>
                </tr>
            </table>

        </ContentTemplate>
      
    </asp:UpdatePanel>
</asp:Content>

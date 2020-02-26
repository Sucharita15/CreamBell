<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmDashboard.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmDashboard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/Dashboard.css" rel="stylesheet" />
<style>
    .nav ul li:hover > a, .nav ul li a:hover {
    background: #36f;
    color: #fff;
    border-bottom: 1px solid #03f;
    }

 

.nav ul .dropdown:hover ul {
    left: 270px;
    top: 0px;
    }

</style>
    <%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <%--<script src="jquery/jquery.counterup.js"></script>
    <script src="jquery/jquery.counterup.min.js"></script>
    <script>
        jQuery(document).ready(function ($) {
            $('.counter').counterUp({
                delay: 30,
                time: 1000
            });
        });
    </script> --%>   
    <div id="topheader">
        <div class="dashtopbe">
            <div class="matrixtop" style="font-size: 12px; float: left;text-align:left; color:#f53d2d; font-weight:bold;">
                CBSAM Autherization Matrix
            </div>
        </div>
    </div>
    <div class="container">
        <div class="innercontent">
            <div class="ctleftouter">
                <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="200px">
                    <asp:GridView ID="gridViewCustomers" runat="server" AutoGenerateColumns="False" ShowFooter="false" Width="100%"
                        BackColor="#fbb02b" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                        <AlternatingRowStyle BackColor="#57c8f2" />
                        <Columns>
                            <asp:TemplateField HeaderText="SNo">
                                <ItemTemplate>
                                    <span><%#Container.DataItemIndex + 1%></span>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Width="20px" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="MASTER" DataField="MASTER">
                                <HeaderStyle Width="120px" ForeColor="White" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="APPROVER1" DataField="APPROVER1">
                                <HeaderStyle ForeColor="White" Width="50px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                           <%-- <asp:BoundField HeaderText="APPROVER2" DataField="APPROVER2">
                                <HeaderStyle ForeColor="White" Width="50px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>--%>
                            <asp:BoundField HeaderText="EMAIL" DataField="EMAIL">
                                <HeaderStyle ForeColor="White" Width="30px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="CONTACTNO" DataField="CONTACTNO">
                                <HeaderStyle ForeColor="White" Width="50px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle BackColor="#418cf0" ForeColor="#000000" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <RowStyle ForeColor="#000066" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                    </asp:GridView>
                </asp:Panel>
            </div>
            <div class="ctrightouter">
                <asp:Panel ID="Panel2" runat="server" ScrollBars="Vertical" Height="200px">
                    <asp:GridView ID="gridViewCustomers1" runat="server" AutoGenerateColumns="False" ShowFooter="false" Width="100%"
                        BackColor="#fbb02b" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                        <AlternatingRowStyle BackColor="#57c8f2" />
                        <Columns>
                            <asp:BoundField HeaderText="ESCALATION" DataField="ESCALATION">
                                <HeaderStyle Width="120px" ForeColor="White" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="CONTACTPERSON" DataField="CONTACTPERSON">
                                <HeaderStyle ForeColor="White" Width="50px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="MAIL ID & CONTACT">
                                <ItemTemplate>
                                    <span><%#Eval("EMAIL")%><br />
                                        <%#Eval("CONTACTNO")%></span>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Width="20px" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                        </Columns>

                        <HeaderStyle BackColor="#418cf0" ForeColor="#000000" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <RowStyle ForeColor="#000066" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                    </asp:GridView>
                </asp:Panel>

            </div>
        </div>
        <div class="innercontentgraph" style="float: left; text-align: left; color: #d4463a; font-size: 11px; font-weight:bold;">
            <asp:Label ID="Label1" runat="server"></asp:Label>
        </div>
        <div class="innercontentgraph" style="float: left; text-align: left;padding-top:10px;">
            Sales Value[Lacs] 
        </div>
       <%-- <div class="innercontentgraph" style="float: left; text-align: left;">
            <asp:Chart ID="Chart1" runat="server" Width="800px" Height="213">
                    <series>
                <asp:Series Name="Series1" XValueMember="2" YValueMembers="3">
                </asp:Series>
            </series>
                    <chartareas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </chartareas>
                </asp:Chart>
        </div>--%>
        <div class="innercontentgraph21">
            <div class="connerleftimg">
                
              <asp:Chart ID="Chart1" runat="server" Width="358px" Height="213">
                    <series>
                <asp:Series Name="Series1" XValueMember="2" YValueMembers="3">
                </asp:Series>
            </series>
                    <chartareas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </chartareas>
                </asp:Chart>
            </div>
            <div class="connerRightimg">
                <div class="innergraphside">
                    <div class="upperside">
                        <div class="partup1">
                            <a href="frmPurchaseIndentList.aspx">
                                <img src="images/img1.png" border="0px" alt="CBSM Graph" title="CBSM Graph" /></a>
                        </div>
                        <div class="partup1text">
                            <div class="part1head">
                                <asp:Label ID="LblPurchaseIndent" runat="server" Text=""></asp:Label>
                            </div>
                            <p style="font-size: 10px; font-family: Arial, Helvetica, sans-serif;">Total Purchase Indent</p>
                        </div>
                        <div class="partup1">
                            <a href="frmPurchReceiptList.aspx">
                                <img src="images/img2.png" border="0px" alt="CBSM Graph" title="CBSM Graph" /></a>
                        </div>
                        <div class="partup1text">
                            <div class="part1head">
                                <asp:Label ID="LblPurchaseInvoice" runat="server" Text=""></asp:Label>
                            </div>
                            <p style="font-size: 10px; font-family: Arial, Helvetica, sans-serif;">Total Purchase Invoice</p>
                        </div>
                    </div>
                    <div class="upperside21">
                        <div class="partup1">
                            <a href="frmPurchaseReturnList.aspx">
                                <img src="images/img3.png" border="0px" alt="CBSM Graph" title="CBSM Graph" /></a>
                        </div>
                        <div class="partup1text">
                            <div class="part1head">
                                <asp:Label ID="LblPurchaseReturn" runat="server" Text=""></asp:Label>
                            </div>
                            <p style="font-size: 10px; font-family: Arial, Helvetica, sans-serif;">Total Purchase Return</p>
                        </div>
                        <div class="partup1">
                            <a href="ReportSalesInvoice.aspx">
                                <img src="images/img4.png" border="0px" alt="CBSM Graph" title="CBSM Graph" /></a>
                        </div>
                        <div class="partup1text">
                            <div class="part1head">
                                <asp:Label ID="LblSaleInvoice" runat="server" Text=""></asp:Label>
                            </div>
                            <p style="font-size: 10px; font-family: Arial, Helvetica, sans-serif;">Total Sale Invoice</p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="connerRightimg211">
                <div class="innergraphside101">
                    <div class="uppersiderecord1">
                        <div class="records1">
                            <div class="lastleft1">
                                <div class="lastleft1txt">
                                    <a href="frmCustomerPartyGroup.aspx">
                                        <asp:Label ID="LblDistributorGroup" runat="server" Text=""></asp:Label></a>
                                </div>
                                <div class="lastleft1txt1">
                                    Total Customer Group
                                </div>
                            </div>
                        </div>
                        <div class="records12">
                            <div class="lastleft1">
                                <div class="lastleft1txt">
                                    <a href="frmCustomerPartyMaster.aspx">


                                        <asp:Label ID="LblTotDistributor" runat="server" Text=""></asp:Label></a>
                                </div>
                                <div class="lastleft1txt1">
                                    Total Customers
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="uppersiderecord16">
                        <div class="records17">
                            <div class="lastleft1">
                                <div class="lastleft1txt01">
                                    <a href="frmLoadSheetCreation.aspx" class="lgtout">
                                        <asp:Label ID="LblSaleOrder" runat="server" Text=""></asp:Label></a>
                                </div>
                                <div class="lastleft1txt101">
                                    Total Sale Orders
                                </div>
                            </div>
                        </div>
                        <div class="records127">
                            <div class="lastleft1">
                                <div class="lastleft1txt01">
                                    <span class="counter">
                                    <a href="frmProductMaster.aspx">
                                        <asp:Label ID="LblTotProduct" class="counter" runat="server" Text=""></asp:Label></a></span>
                                </div>
                                <div class="lastleft1txt101">
                                    Total Products
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <div class="innercontfooter">
            <%--<div class="footertxt11">
                TargetIcon
            </div>
            <div class="footertxt1">
                Developed and maintained by&nbsp;
            </div>--%>

        </div>
    </div>
</asp:Content>

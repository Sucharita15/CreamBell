<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmRunningSchemeDetail.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmRunningSchemeDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
    <link href="css/style.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gvSchemeDetail.ClientID%>').clone(true);
              /*Code to remove first ror which is header row*/
              $(gridHeader).find("tr:gt(0)").remove();
              $('#<%=gvSchemeDetail.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead").append(gridHeader);
            $('#controlHead').css('position', 'absolute');
            $('#controlHead').css('top', '129');
          });


        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gridViewSlabDetail.ClientID%>').clone(true);
              /*Code to remove first ror which is header row*/
              $(gridHeader).find("tr:gt(0)").remove();
              $('#<%=gridViewSlabDetail.ClientID%> tr th').each(function (i) {
                  /* Here Set Width of each th from gridview to new table th */
                  $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
              });
              $("#controlHead0").append(gridHeader);
              $('#controlHead0').css('position', 'absolute');
              $('#controlHead0').css('top', '129');

          });


          $(document).ready(function () {
              /*Code to copy the gridview header with style*/
              var gridHeader = $('#<%=gridViewSchemeItemGroup.ClientID%>').clone(true);
              /*Code to remove first ror which is header row*/
              $(gridHeader).find("tr:gt(0)").remove();
              $('#<%=gridViewSchemeItemGroup.ClientID%> tr th').each(function (i) {
                  /* Here Set Width of each th from gridview to new table th */
                  $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
              });
              $("#controlHead1").append(gridHeader);
              $('#controlHead1').css('position', 'absolute');
              $('#controlHead1').css('top', '129');

          });

          $(document).ready(function () {
              /*Code to copy the gridview header with style*/
              var gridHeader = $('#<%=gridViewFreeItemGroup.ClientID%>').clone(true);
              /*Code to remove first ror which is header row*/
              $(gridHeader).find("tr:gt(0)").remove();
              $('#<%=gridViewFreeItemGroup.ClientID%> tr th').each(function (i) {
                  /* Here Set Width of each th from gridview to new table th */
                  $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
              });
              $("#controlHead2").append(gridHeader);
              $('#controlHead2').css('position', 'absolute');
              $('#controlHead2').css('top', '129');

          });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <div style="width: 98%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #0085ca; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px; color: #FFFFFF;">Running Scheme Detail</span>
    </div>
    <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 98%; margin: 0px 0px 0px 10px;">
                <table style="width: 100%">

                    <tr>
                        <td style="width: 5%;text-align:left">State</td>
                        <td style="width: 15%;text-align:left">
                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" Width="180px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 5%;text-align:left">Distributor</td>
                        <td style="width: 25%;text-align:left">
                            <asp:DropDownList ID="ddlSiteId" runat="server" AutoPostBack="True"  CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="90%" OnSelectedIndexChanged="ddlSiteId_SelectedIndexChanged" >
                            </asp:DropDownList>
                        </td>
                       
                        <td style="width: 10%;text-align:center">
                            <asp:Button ID="btnGenerate" runat="server" CssClass="ReportSearch" Height="31px" OnClick="btnGenerate_Click" Text="Generate" Width="96px" />
                        </td>
                        <td style="width: 30%;text-align:right" colspan="3">
                            <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="small"> </asp:Label>
                        </td>
                    </tr>
                   
                </table>
            </asp:Panel>
    <table>
        <tr>
            <td>
                <asp:Panel ID="pnlSchemeDetail" runat="server" GroupingText="Scheme Details" Width="650px">
                    <div id="controlHead" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 600px"></div>
                    <div style="overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; height: 138px; width: 600px">
                        <asp:GridView ID="gvSchemeDetail" runat="server" AutoGenerateColumns="False"
                            BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" Width="600px">
                            <AlternatingRowStyle BackColor="#CCFFCC" />
                            <Columns>
                                <asp:TemplateField HeaderText="SchemeCode">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnSchemeDetail" runat="server" OnClick="lnkbtnSchemeDetail_Click" Text='<%# Bind("SCHEMECODE") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Scheme Name" />
                                <asp:BoundField DataField="SCHEME TYPE" HeaderText="Scheme Type" />
                                <asp:BoundField DataField="STARTINGDATE" HeaderText="Valid From " DataFormatString="{0:dd/MMM/yyyy}" />
                                <asp:BoundField DataField="ENDINGDATE" HeaderText="Valid To" DataFormatString="{0:dd/MMM/yyyy}" />
                            </Columns>
                            <EmptyDataTemplate>
                                No Record Found...
                            </EmptyDataTemplate>
                            <FooterStyle BackColor="#bfbfbf" />
                            <HeaderStyle BackColor="#05345C" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>

            <td>
                <asp:Panel ID="Pnlslabdetails" runat="server" GroupingText="Slab Details" Width="650px">
                    <div id="controlHead0" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 600px">
                    </div>
                    <div style="overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; height: 138px; width: 600px">
                        <asp:GridView ID="gridViewSlabDetail" runat="server" AutoGenerateColumns="False" Width="600px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                            <AlternatingRowStyle BackColor="#CCFFCC" />
                            <Columns>

                                <asp:TemplateField HeaderText="SlabDetail">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtngridViewSlabDetail" runat="server" Text='<%# Bind("slabdetail") %>' OnClick="lnkbtngridViewSlabDetail_Click"></asp:LinkButton>
                                        <asp:HiddenField ID="hiddenSchemeItemGroup" Visible="false" runat="server" Value='<%# Eval("Scheme Item group") %>' />
                                        <asp:HiddenField ID="HiddenFieldSchemeLineNo" Visible="false" runat="server" Value='<%# Eval("SchemeLineNo") %>' />
                                        <asp:HiddenField ID="HiddenFieldSchemeCode" Visible="false" runat="server" Value='<%# Eval("SCHEMECODE") %>' />
                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="CustType" DataField="TYPE" />
                                <asp:TemplateField HeaderText="Item Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSchemeItemType" runat="server" Text='<%# Bind("[Scheme Item Type]") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Sales Type" HeaderText="SalesType" />
                                <asp:BoundField HeaderText="Description" DataField="Name">
                                    <ControlStyle Width="150px" />
                                    <ItemStyle Width="150px" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Buying Qty Box" DataField="Buying Quantity Box" DataFormatString="{0:n2}" />
                                <asp:BoundField DataField="Buying Quantity PCS" DataFormatString="{0:n2}" HeaderText="Buying Qty PCS" />
                            </Columns>
                            <EmptyDataTemplate>
                                No Record Found...
                            </EmptyDataTemplate>
                            <FooterStyle BackColor="#bfbfbf" />
                            <HeaderStyle BackColor="#05345C" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" GroupingText="Minimum Purchase" Width="650px">
                    <div id="controlHead3" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 600px">
                    </div>
                    <div style="overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; height: 50px; width: 600px">
                        <asp:GridView ID="gridView1" runat="server" Width="600px" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                            <AlternatingRowStyle BackColor="#CCFFCC" />
                            <Columns>
                                <asp:BoundField DataField="MINIMUMPURCHASEITEMTYPE" HeaderText="PURCHITEMTYPE" />
                                <asp:BoundField DataField="MINIMUMPURCHASEITEMGROUP" HeaderText="PURCHITEMGROUP" />
                                <asp:BoundField DataField="MINIMUMPURCHASEITEMDESCRIPTION" HeaderText="PURCHITEMDESCR" />
                                <asp:BoundField DataField="MINIMUMPURCHASEBOX" HeaderText="PURCHASEBOX" />
                                <asp:BoundField DataField="MINIMUMPURCHASEPCS" HeaderText="PURCHASEPCS" />
                            </Columns>
                            <FooterStyle BackColor="#bfbfbf" />
                            <HeaderStyle BackColor="#05345C" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>

            <td>
                <asp:Panel ID="Panel2" runat="server" GroupingText="Additional Discount" Width="650px">
                    <div id="controlHead4" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 600px">
                    </div>
                    <div style="overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; height: 50px; width: 600px">

                        <asp:GridView ID="gridView2" runat="server" Width="600px" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC"
                            BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                            <AlternatingRowStyle BackColor="#CCFFCC" />
                            <Columns>
                                <asp:BoundField DataField="ADDITIONDISCOUNTITEMTYPE" HeaderText="DISCITEMTYPE" />
                                <asp:BoundField DataField="ADDITIONDISCOUNTITEMGROUP" HeaderText="DISCITEMGROUP" />
                                <asp:BoundField DataField="ADDITIONDISCOUNTITEMGROUPDESC" HeaderText="DISCITEMGROUPDESCR" />
                                <asp:BoundField DataField="ADDITIONDISCOUNTPERCENT" HeaderText="DISC%" />
                                <asp:BoundField DataField="ADDITIONDISCOUNTVALUEOFF" HeaderText="DISCVALUEOFF" />
                            </Columns>
                            <FooterStyle BackColor="#bfbfbf" />
                            <HeaderStyle BackColor="#05345C" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>

                    </div>
                </asp:Panel>
            </td>
            </tr>

        <tr>
            <td>
                <asp:Panel ID="pnlitemdetails" runat="server" GroupingText="Item Details" Width="650px">
                    <div id="controlHead1" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 600px">
                    </div>
                    <div style="overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; height: 128px; width: 600px">
                        <asp:GridView ID="gridViewSchemeItemGroup" runat="server" Width="600px" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                            <AlternatingRowStyle BackColor="#CCFFCC" />
                            <Columns>
                                <asp:BoundField HeaderText="ProductGroup" DataField="PRODUCT GROUP" />
                                <asp:BoundField HeaderText="Product Sub Category" DataField="ProductSubCat" />
                                <asp:BoundField HeaderText="ItemCode" DataField="ITEMID" />
                                <asp:BoundField HeaderText="Item Description" DataField="ITEMNAME" />
                            </Columns>
                            <FooterStyle BackColor="#bfbfbf" />
                            <HeaderStyle BackColor="#05345C" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>

            <td>
                <asp:Panel ID="pnlfreeitemdetails" runat="server" GroupingText="Free Item Details" Width="650px">
                    <div id="controlHead2" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 600px">
                    </div>
                    <div style="overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; height: 128px; width: 600px">

                        <asp:GridView ID="gridViewFreeItemGroup" runat="server" Width="600px" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC"
                            BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
                            <AlternatingRowStyle BackColor="#CCFFCC" />
                            <Columns>
                                <asp:BoundField HeaderText="ProductGroup" DataField="PRODUCT_GROUP" />
                                <asp:BoundField HeaderText="Product Sub Category" DataField="ProductSubCat" />
                                <asp:BoundField HeaderText="ItemCode" DataField="ITEMID" />
                                <asp:BoundField HeaderText="Item Description" DataField="PRODUCT_NAME" />
                                <asp:BoundField DataField="FREEQTY" HeaderText="Free Qty Box" />
                                <asp:BoundField DataField="FREEQTYPCS" HeaderText="Free Qty PCS" />
                                <asp:BoundField DataField="SETNO" HeaderText="SET NO" />
                                <asp:BoundField DataField="PERCENTSCHEME" HeaderText="SCHEME%" />
                                <asp:BoundField DataField="SCHEMEVALUEOFF" HeaderText="SCHEMEVALUEOFF" />
                            </Columns>
                            <FooterStyle BackColor="#bfbfbf" />
                            <HeaderStyle BackColor="#05345C" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>

                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="frmProductMaster.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmProductMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />



    <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
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
             $("#controlHead").append(gridHeader);
             $('#controlHead').css('position', 'absolute');
             $('#controlHead').css('top', '129');

  });
    </script>


    <script type="text/javascript" src="JS/jquery.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            jQuery('[id$="LinkProducts"]').click(function () {
                var customID = $(this).attr('myCustomID');
                $('#DisplayDetails').load("ShowProductDetails.aspx?productId=" + customID);
                return false;
            });

        });

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; ">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; color: white; margin: 0px 0px 0px 0px;">Product Master
        </span>
    </div>

    <div style="width:100%; text-align:left">
        <asp:Button ID="btnExport2Excel" runat="server" Text="Export To Excel" CssClass="button" Height="31px" OnClick="btnExport2Excel_Click" />
    </div>
   <%-- <div id="controlHead" style="margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 98%">
    </div>--%>
    <div style="overflow: auto; margin-top: 5px;  height: 450px; width: 100%">


        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowFooter="True" 
            Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
            CellPadding="3" ShowHeaderWhenEmpty="True" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
            <AlternatingRowStyle BackColor="#CCFFCC" />
            <Columns>
                <asp:TemplateField HeaderText="Product  Code">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkbtn" runat="server" Text='<%# Bind("ITEMID") %>' OnClick="lnkbtn_Click"></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                </asp:TemplateField>
                <asp:BoundField DataField="HSNCODE" HeaderText="HSN CODE">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="PRODUCT_GROUP" HeaderText="Product  Group">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="PRODUCT_SUBCATEGORY" HeaderText="Product Sub Category">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="PRODUCT_NAME" HeaderText="Product Name">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Product  Category" DataField="" Visible="False">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="EXEMPT" HeaderText="Is Exempted">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="BLOCK" HeaderText="Is Blocked">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="BLOCKDATE" HeaderText="Blocked On" DataFormatString="{0:dd/MMM/yyyy}">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Product Pack Size" DataField="PRODUCT_PACKSIZE" DataFormatString="{0:n}">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Product Crate Size" DataField="PRODUCT_CRATE_PACKSIZE" DataFormatString="{0:n}">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                 <asp:BoundField HeaderText="ML/PC" DataField="ML_PC" DataFormatString="{0:n}">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Flavor" DataField="FLAVOUR" >
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Product Nature" DataField="PRODUCT_NATURE" >
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Volume (Ltr)" DataField="Volume" DataFormatString="{0:n}">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
               
                <asp:BoundField DataField="PRODUCT_MRP" DataFormatString="{0:n}" HeaderText="Product  MRP">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

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


</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmSalesOrderPreList.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmSalesOrderPreList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />

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
    </style>        
       <script type="text/javascript">

           $(document).ready(function () {
               /*Code to copy the gridview header with style*/
               var gridHeader = $('#<%=GvSaleOrderPre.ClientID%>').clone(true);
               /*Code to remove first ror which is header row*/
               $(gridHeader).find("tr:gt(0)").remove();
               $('#<%=GvSaleOrderPre.ClientID%> tr th').each(function (i) {
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

        <div style="width: 100%;height: 18px;border-radius: 4px;margin: 10px 0px 0px 5px;background-color: #1564ad;color:white ;padding: 2px 0px 0px 0px; font-weight:bold" >
         Sale Order Pre List
     </div>

   
<div id="controlHead2" style="margin-top:10px; margin-left:5px;padding-right:10px;"></div>
             <div style="overflow: auto;margin-top:10px; margin-left:5px;padding-right:10px;height:500px">
                     
        <asp:GridView runat="server" ID="GvSaleOrderPre"  ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White" 
           BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" > 
               <AlternatingRowStyle BackColor="#CCFFCC" />
            <Columns>
                <asp:TemplateField HeaderText="SO NO">
                    
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkSONo" runat="server" Text='<%# Bind("SO_NO") %>' CommandArgument='<%# Bind("SO_NO") %>' OnClick="lnkSONo_Click"></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                </asp:TemplateField>

               <asp:BoundField HeaderText="SO Date" DataField="SO_DATE" >
                <HeaderStyle Width="150px" HorizontalAlign="Left" />
                <ItemStyle Width="150px" HorizontalAlign="Left" />
                </asp:BoundField>

                 <asp:BoundField HeaderText="Customer" DataField="CUSTOMER_CODE" >
                <HeaderStyle Width="300px" HorizontalAlign="Left" />
                <ItemStyle Width="300px" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Delivery Date" DataField="DELIVERY_DATE" >
                <HeaderStyle Width="150px" HorizontalAlign="Left" />
                <ItemStyle Width="150px" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Amount" DataField="AMOUNT" DataFormatString="{0:n2}" >
                <HeaderStyle Width="200px" HorizontalAlign="Left" />
                <ItemStyle Width="200px" HorizontalAlign="Left" />
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

</asp:Content>

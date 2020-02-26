<%@ Page Title="PSR-Distributor Linking" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPSRDistributorLinkingMaster.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPSRDistributorLinkingMaster" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="css/btnSearch.css" rel="stylesheet" />
     <link href="css/style.css" rel="stylesheet" />
   
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
</script>

      <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>

        <script type ="text/javascript">
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
    </script>

     <style type="text/css">
           
        .ddl
        {  
            background-image:url('Images/arrow-down-icon-black.png');
         
        }
        .ddl:hover{		
        background-image:url('Images/arrow-down-icon-black.png');
        
    }
    </style>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
     <div style="width: 98%;height: 18px;border-radius: 4px;margin: 10px 0px 0px 5px;background-color: #1564ad;padding: 2px 0px 0px 0px;" >
      <span style="font-family: Segoe UI;color:white; font-size: 11px;font-weight: bold;margin: 6px 0px 0px 10px;">PSR - Distributor Linking Master</span>
    </div>
      <%-- <div style="border-bottom-style: solid; border-bottom-width: 5px; border-bottom-color: #000066" >--%>
        <table style="text-align:right;width:100%">
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnExportToExcel" runat="server" ImageUrl="~/Images/excel-24.ico" style="margin:0px 0px 0px 10px" ToolTip="Click To Generate Excel Report" OnClick="imgBtnExportToExcel_Click" />
                </td>
              <td style="width:80%; text-align:right">
                    <asp:DropDownList ID="DDLSearchFilter" runat="server" CssClass="ddl" data-toggle="dropdown" Width="200" style="margin-left: 0px" >                       
                       <asp:ListItem>PSR Name</asp:ListItem>
                   </asp:DropDownList>
</td>
                <td style="width:20%">
                    <div>
	                    <asp:TextBox ID="txtSearch" runat="server" placeholder="Search here..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" />
                        <span id="span1" onmouseover="test()" onmouseout="test1()">
	                        <asp:Button ID="BtnSearch" runat="server" style="margin:0px 0px 0px -2px"  Text="Search" OnClick="BtnSearch_Click"></asp:Button>&nbsp;&nbsp;
                        </span>
                   </div>
                   
                    </td>
                </tr>
       </table>
   

   

     <%--<div style="width: 98%;height: 18px;border-radius: 4px;margin: 10px 0px 0px 5px;background-color: #1564ad;padding: 2px 0px 0px 0px;" >
         PSR - Distributor Linking Master

     </div>--%>
    
   
    
    <div style="margin-left: 0px; width: 99%; overflow:auto">
        
         <asp:GridView  ID="gvDetails" runat="server" GridLines="Horizontal" AutoGenerateColumns="False" Width="99%" BackColor="White" 
           BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" OnPageIndexChanging="gvDetails_PageIndexChanging"
             AllowPaging="True">
               
            <AlternatingRowStyle BackColor="#CCFFCC" />
            <Columns>                
                <asp:BoundField HeaderText="Distributor Code"  DataField="Site_Code" >
                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Distributor Name"  DataField="Site_Name" >
                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                <ItemStyle Width="150px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="PSR Code"  DataField="PSRCode" >
                <HeaderStyle HorizontalAlign="Left" Width="100px"/>
                <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="PSR Name"  DataField="PSRName" >
                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                <ItemStyle Width="100px" />
                </asp:BoundField>                
                <asp:BoundField HeaderText="From Date" DataField="FromDate"  dataformatstring="{0:dd/MMM/yyyy}" >
                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                <ItemStyle Width="80px" />
                </asp:BoundField>
                 <asp:BoundField HeaderText="TO Date" DataField="TODate" dataformatstring="{0:dd/MMM/yyyy}" >
                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                <ItemStyle Width="80px" />
                </asp:BoundField>
           
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
</asp:Content>

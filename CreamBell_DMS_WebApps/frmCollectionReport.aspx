<%@ Page Title="Collection Report" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmCollectionReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmCollectionReport" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Src="~/UserControl/ucRoleFilters.ascx" TagPrefix="uc1" TagName="ucRoleFilters" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('.single-selection').multiselect({
                 enableFiltering: true,
                 enableCaseInsensitiveFiltering: true,
                 nonSelectedText: 'Select'
             });
         });
        </script>
     <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="css/btnSearch.css" rel="stylesheet" />
     <link href="css/style.css" rel="stylesheet" />  
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />
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
    </style>

   
    
     
      <style type="text/css">
       /*DropDownCss*/
        .ddl
        {  
            background-color: #eeeeee;
            padding:5px ;
            border:1px solid #7d6754;
            border-radius:4px;
            padding:3px;
            -webkit-appearance: none; 
            background-image:url('Images/arrow-down-icon-black.png');
            background-position:right;
            background-repeat:no-repeat;
            text-indent: 0.01px;/*In Firefox*/
            text-overflow: '';/*In Firefox*/
        }
        .ddl:hover{		
        background: #add8e6;
        background-image:url('Images/arrow-down-icon-black.png');
        background-position:right;
        background-repeat:no-repeat;
        text-indent: 0.01px;/*In Firefox*/
        text-overflow: '';/*In Firefox*/
       
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

             $("#controlHead2").append(gridHeader);
             $('#controlHead2').css('position', 'absolute');
             $('#controlHead2').css('top', '129');

        });

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
      <Triggers>
            <asp:PostBackTrigger ControlID="imgBtnExportToExcel" />
            <asp:PostBackTrigger ControlID="BtnSearch" />
            <asp:PostBackTrigger ControlID="ucRoleFilters" />
            <asp:PostBackTrigger ControlID="drpCustomerGroupNew" />
        </Triggers>  
      <ContentTemplate>
         
    <div style="width: 100%;height: 18px;border-radius: 4px;margin: 10px 0px 0px 5px;background-color: #1564ad;color:white ;padding: 2px 0px 0px 0px; font-weight:bold" >
        Collection Report
     </div>
          <uc1:ucRoleFilters runat="server" ID="ucRoleFilters" />
    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>

   <%-- <div id="Filter" style="width: 100%;height: 34px;border-radius: 1px;margin: 10px 0px 0px 5px;background-color: lightskyblue;color:black ;padding: 2px 0px 0px 0px; border-style:groove" >--%>
       
        <table style="width:100%">
            <tr>
                <td>From Date :</td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" Enabled="true" placeholder="dd-MMM-yyyy" ></asp:TextBox>
                    <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>
                
                <td>To Date :</td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" Enabled="true" placeholder="dd-MMM-yyyy"></asp:TextBox>
                    <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>
               
                <td> Customer Group :</td>
                <td>
                   <%-- <asp:DropDownList ID="drpCustomerGroup" runat="server" CssClass="ddl" data-toggle="dropdown"   Width="200" style="margin-left: 0px" >                       
                                                       
                   </asp:DropDownList>--%>
                      <asp:ListBox ID="drpCustomerGroupNew" ClientIDMode="Static" runat="server"     CssClass="single-selection"></asp:ListBox>
                 </td>
              <td>
                    <asp:Button ID="BtnSearch" runat="server" Text="Search" CssClass="ReportSearch" OnClick="BtnSearch_Click"/>
                </td>
           <td style="text-align:left">
                    <asp:ImageButton ID="imgBtnExportToExcel" runat="server"  ImageUrl="~/Images/excel-24.ico"  ToolTip="Click To Generate Excel Report" OnClick="imgBtnExportToExcel_Click"/>
                </td>
                 <td>
                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkBlue" Font-Names="Seoge UI"> </asp:Label>
                </td>
            </tr>
        </table>

  <%--  </div>--%>

    <%-- <div id="controlHead1" style="margin-top:5px; margin-left:5px;padding-right:10px;width: 1060px;"></div>--%>
             <div style="overflow: auto;margin-top:5px; margin-left:5px;padding-right:10px;width: 100%;"">

         <asp:GridView runat="server" ID="gvDetails"  ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White" 
           BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" > 
        <Columns>
                <asp:BoundField HeaderText="DocumentNumber" DataField="Document_No" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="CollectionDate" DataField="Collection_Date" DataFormatString="{0:dd-MMM-yyyy}">
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                 <asp:BoundField HeaderText="CustomerCode" DataField="Customer_Code" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="CustomerName" DataField="Customer_Name" >
                <HeaderStyle Width="50px" HorizontalAlign="Left" />
                <ItemStyle Width="50px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Address" DataField="Address" >
                <HeaderStyle Width="200px" HorizontalAlign="Left" />
                <ItemStyle Width="200px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Instrument" DataField="Instrument_Description" >
                <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                <ItemStyle HorizontalAlign="Left" Width="80px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="InstrumentNo" DataField="Instrument_No" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                 <asp:BoundField HeaderText="RefDoc.No" DataField="Ref_DocumentNo" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="CollectionAmmount" DataField="Collection_Amount" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" />
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
           </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
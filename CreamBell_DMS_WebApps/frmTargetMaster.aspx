<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmTargetMaster.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmTargetMaster" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
             var gridHeader = $('#<%=gvSKUDetails.ClientID%>').clone(true);             
               /*Code to remove first ror which is header row*/
             $(gridHeader).find("tr:gt(0)").remove();          

               $('#<%=gvSKUDetails.ClientID%> tr th').each(function (i) {
                    /* Here Set Width of each th from gridview to new table th */
                    $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
               });
           
                $("#controlHead3").append(gridHeader);
                $('#controlHead3').css('position', 'absolute');
                $('#controlHead3').css('top', '129');

           });

    </script>

    <script type="text/javascript">

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gvOtherDetails.ClientID%>').clone(true);
             /*Code to remove first ror which is header row*/
             $(gridHeader).find("tr:gt(0)").remove();

             $('#<%=gvOtherDetails.ClientID%> tr th').each(function (i) {
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
         
     <div style="width: 98%;height: 18px;border-radius: 4px;margin: 10px 0px 0px 5px;background-color: #1564ad;padding: 2px 0px 0px 0px;" >
      <span style="font-family: Segoe UI;color:white; font-size: 11px;font-weight: bold;margin: 6px 0px 0px 10px;">Target Master</span>
    </div>   
    
      <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>


    <div id="Filter" style="width: 1060px;height: 40px;border-radius: 1px;margin: 10px 0px 0px 5px;background-color: lightskyblue;color:black ;padding: 2px 0px 0px 0px; border-style:groove" >
       
        <table>
            <tr>
                <td>From Date :</td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" Enabled="true" placeholder="dd-MMM-yyyy" ></asp:TextBox> &nbsp;
                    <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>
                <td></td>
                <td>To Date :</td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" Enabled="true" placeholder="dd-MMM-yyyy"></asp:TextBox> &nbsp;
                    <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>
                <td></td>
                <td> Target Object :</td>
                <td>
                    <asp:DropDownList ID="drpTargetObject" runat="server" CssClass="ddl" data-toggle="dropdown"   Width="200" style="margin-left: 0px" >                       
                                                       
                   </asp:DropDownList>
                 </td>
                
                <td></td>
                <td>
                    <asp:Button ID="BtnSearch" runat="server" Text="Search" CssClass="ReportSearch" OnClick="BtnSearch_Click"/>
                </td>
                <td></td>
                <td>
                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkBlue" Font-Names="Seoge UI"> </asp:Label>
                </td>
                <td></td>
                <td>
                    <asp:ImageButton ID="imgBtnExportToExcel" runat="server"  ImageUrl="~/Images/excel-24.ico"  ToolTip="Click To Generate Excel Report" OnClick="imgBtnExportToExcel_Click"/>
                </td>
               
            </tr>
        </table>

    </div>
        
     <div style="display:block;width:100%" class="form-style-6">
        <table>
            <tr>
                <asp:RadioButtonList ID="rdoOption" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" OnSelectedIndexChanged="rdoOption_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Text="Other" Value="Other" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="SKU Wise" Value="SKU_Wise"></asp:ListItem>
               </asp:RadioButtonList>
            </tr>

        </table>
    </div>
    <div id="controlHead2" style="margin-top:5px; margin-left:5px;padding-right:10px;"></div>
             <div style="overflow: auto;margin-top:5px; margin-left:5px;padding-right:10px;"">
          <asp:GridView runat="server" ID="gvOtherDetails"  ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" Width="1060px" BackColor="White" 
             BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" > 
        <%--  <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="748px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" EnableSortingAndPagingCallbacks="True">--%>
             <Columns>

                 <asp:BoundField HeaderText="Target Object" DataField="TARGETOBJECTTYPE_NAME" >
                 <HeaderStyle Width="100px" HorizontalAlign="Left" />
                 <ItemStyle Width="100px" HorizontalAlign="Left" />
                 </asp:BoundField>

                 <asp:BoundField HeaderText="Object Code" DataField="TARGET_OBJECT" >
                 <HeaderStyle Width="100px" HorizontalAlign="Left" />
                 <ItemStyle Width="100px" HorizontalAlign="Left" />
                 </asp:BoundField>
                 
                 <asp:BoundField HeaderText="Object Name" DataField="TARGETOBJECT_NAME" >
                 <HeaderStyle Width="100px" HorizontalAlign="Left" />
                 <ItemStyle Width="100px" HorizontalAlign="Left" />
                 </asp:BoundField>
                 
                 <asp:BoundField HeaderText="Target Type" DataField="TARGETTYPE_NAME" >
                 <HeaderStyle Width="100px" HorizontalAlign="Left" />
                 <ItemStyle Width="100px" HorizontalAlign="Left" />
                 </asp:BoundField>

                 <asp:BoundField HeaderText="Target Units" DataField="TARGET" DataFormatString=" {0:n2}" >
               <HeaderStyle Width="100px" HorizontalAlign="Left" />
                 <ItemStyle Width="100px" HorizontalAlign="Left" />
                 </asp:BoundField>
                
                 <asp:BoundField HeaderText="From Date" DataField="VALIDFROM">
                 <HeaderStyle Width="100px" HorizontalAlign="Left" />
                 <ItemStyle Width="100px" HorizontalAlign="Left" />
                 </asp:BoundField>
                 
                 <asp:BoundField HeaderText="To Date" DataField="VALIDTO">
                 <HeaderStyle Width="100px" HorizontalAlign="Left" />
                 <ItemStyle Width="100px" HorizontalAlign="Left" />
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


                 <div id="controlHead3" style="margin-top:5px; margin-left:5px;padding-right:10px;"></div>
    <div style="overflow: auto;margin-top:5px; margin-left:5px;padding-right:10px;"">

                <%-- <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="998px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" EnableSortingAndPagingCallbacks="True">--%>
             <asp:GridView runat="server" ID="gvSKUDetails"  ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" Width="1060px" BackColor="White" 
             BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" > 
             <Columns>

                 <asp:BoundField HeaderText="TargetObject" DataField="TARGETOBJECTTYPE_NAME" >
                 <HeaderStyle Width="80px" HorizontalAlign="Left" />
                 <ItemStyle Width="80px" HorizontalAlign="Left" />
                 </asp:BoundField>

                  <asp:BoundField HeaderText="Object Name" DataField="TARGET_OBJECT" >
                 <HeaderStyle Width="100px" HorizontalAlign="Left" />
                 <ItemStyle Width="100px" HorizontalAlign="Left" />
                 </asp:BoundField>

                  <asp:BoundField HeaderText="Target Type" DataField="TARGETTYPE_NAME" >
                 <HeaderStyle Width="80px" HorizontalAlign="Left" />
                 <ItemStyle Width="80px" HorizontalAlign="Left" />
                 </asp:BoundField>

                 <asp:BoundField HeaderText="Product Group" DataField="PRODUCT_GROUP" >
                 <HeaderStyle Width="100px" HorizontalAlign="Left" />
                 <ItemStyle Width="100px" HorizontalAlign="Left" />
                 </asp:BoundField>
                 
                 <asp:BoundField HeaderText="ProductCategory" DataField="PRODUCT_CATEGORY" >
                 <HeaderStyle Width="100px" HorizontalAlign="Left" />
                 <ItemStyle Width="100px" HorizontalAlign="Left" />
                 </asp:BoundField>

                  <asp:BoundField HeaderText="ProductSubCategory" DataField="PRODUCT_SUBCATEGORY" >
                 <HeaderStyle Width="100px" HorizontalAlign="Left" />
                 <ItemStyle Width="100px" HorizontalAlign="Left" />
                 </asp:BoundField>
                 
                 <asp:BoundField HeaderText="Product Code-Name"  DataField="Product">
                 <HeaderStyle Width="200px" HorizontalAlign="Left" />
                 <ItemStyle Width="200px" HorizontalAlign="Left" />
                 </asp:BoundField>
                 
                 <asp:BoundField HeaderText="Target(Box)" DataField="TARGET" DataFormatString="{0:n2}">
                 <HeaderStyle Width="100px" HorizontalAlign="Center" />
                 <ItemStyle Width="100px" HorizontalAlign="Center" />
                 </asp:BoundField>
                 
                <%-- <asp:BoundField HeaderText="Targget (Ltr)" DataField="TARGETLtr" >
                 <HeaderStyle Width="100px" HorizontalAlign="Right" />
                 <ItemStyle Width="100px" HorizontalAlign="Right" />
                 </asp:BoundField>--%>

                 <asp:BoundField HeaderText="From Date" DataField="VALIDFROM">
                 <HeaderStyle Width="100px" HorizontalAlign="Left" />
                 <ItemStyle Width="100px" HorizontalAlign="Left" />
                 </asp:BoundField>
                 
                 <asp:BoundField HeaderText="To Date" DataField="VALIDTO">
                 <HeaderStyle Width="100px" HorizontalAlign="Left" />
                 <ItemStyle Width="100px" HorizontalAlign="Left" />
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

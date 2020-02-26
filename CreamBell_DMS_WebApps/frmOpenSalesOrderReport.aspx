<%@ Page Title="Open Sales Order" MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="frmOpenSalesOrderReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmOpenSalesOrderReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <script type="text/javascript">

           $(document).ready(function () {
               /*Code to copy the gridview header with style*/
               var gridLine = $('#<%=gvDetails.ClientID%>').clone(true);
                /*Code to remove first ror which is header row*/
                $(gridLine).find("tr:gt(0)").remove();
                $('#<%=gvDetails.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridLine).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead2").append(gridLine);
            $('#controlHead2').css('position', 'absolute');
            $('#controlHead2').css('top', '129');

           });

          <%-- $(function () {
               $('#<%= BtnSearch.ClientID %>').click(function () {
                    $.blockUI({ message: '<h1>Sumitting your request..</h1>' });
                });
            });--%>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
 <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
      <Triggers>
            <asp:PostBackTrigger ControlID="imgBtnExportToExcel" />
                   </Triggers>  
      <ContentTemplate>
         
<div style="width: 100%;height: 18px;border-radius: 4px;margin: 10px 0px 0px 5px;background-color: #1564ad;color:white ;padding: 2px 0px 0px 0px; font-weight:bold" >
        Open Sales Order
 </div>

    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>

    <%--<div id="Filter" style="width: 2000px;height: 34px;border-radius: 1px;margin: 10px 0px 0px 5px;background-color: lightskyblue;color:black ;padding: 2px 0px 0px 0px; border-style:groove" >--%>
       
        <table style="width:98%">
            <tr>
                <td colspan="10" style="text-align: center">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True"  Font-Names="Seoge UI" Font-Size="Small" ForeColor="DarkRed"></asp:Label>
                </td>
            </tr>
            <tr>
                <td >
                    From Date :</td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" Enabled="true" placeholder="dd-MMM-yyyy" ></asp:TextBox>
                    <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>
               
                <td>To Date :</td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" Enabled="true" placeholder="dd-MMM-yyyy"></asp:TextBox> 
                    <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>
              
                <td>State</td>
                <td>
                    <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" Width="120px">
                    </asp:DropDownList>
                </td>
                <td >Site ID</td>
                <td >
                    <asp:DropDownList ID="ddlSiteId" runat="server" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px">
                    </asp:DropDownList>
                </td>
              <td>
                    <asp:Button ID="BtnSearch" runat="server" Text="Search" Width="80px" CssClass="ReportSearch" OnClick="BtnSearch_Click" Style="position: static"/>
                </td>
               
                <td>
                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkBlue" Font-Names="Seoge UI"> </asp:Label>
                </td>
               <td>
                    <asp:ImageButton ID="imgBtnExportToExcel" runat="server"  ImageUrl="~/Images/excel-24.ico" width="30px" ToolTip="Click To Generate Excel Report" OnClick="imgBtnExportToExcel_Click"/>
                </td> 
            </tr>
        </table>

  <%--  </div>--%>
<%--<div id="controlHead2" style="margin-top:5px; margin-left:5px;padding-right:10px;width: 1800px;"></div>
<div style="overflow: auto;margin-top:5px; margin-left:5px;padding-right:10px;width: 1800px;"">--%>

    <%--<div id="controlHead2" style="margin-top:5px; margin-left:5px;padding-right:10px;width: 2700px;"></div>--%>
         <div style="height:400px;overflow: auto;margin-top:5px; margin-left:5px;padding-right:10px;width: 100%;">
        
    <asp:GridView runat="server" ID="gvDetails"  ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="true" Width="100%" BackColor="White" 
           BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" > 
                                       
            <%--<Columns>
                <asp:BoundField HeaderText="SO_NO" DataField="SO_NO" >
                <HeaderStyle Width="100px" HorizontalAlign="Left" />
                <ItemStyle Width="100px" HorizontalAlign="Left" />
                </asp:BoundField>

                 <asp:BoundField HeaderText="SO_Date" DataField="SO_Date" DataFormatString="{0:dd-MMM-yyyy}" >
                <HeaderStyle Width="100px" HorizontalAlign="Left" />
                <ItemStyle Width="100px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Customer Code" DataField="Customer_Code" >
                <HeaderStyle Width="70px" HorizontalAlign="Left" />
                <ItemStyle Width="70px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Customer Name" DataField="CUSTOMER_NAME" >
                <HeaderStyle Width="150px" HorizontalAlign="Left" />
                <ItemStyle Width="150px" HorizontalAlign="Left" />
                </asp:BoundField>
                
                <asp:BoundField HeaderText="Customer Group" DataField="CUSTGROUP_NAME" >
                <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                <ItemStyle HorizontalAlign="Left" Width="80px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="PSR CODE" DataField="PSR_CODE" >
                <HeaderStyle Width="70px" HorizontalAlign="Left" />
                <ItemStyle Width="70px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="PSR NAME" DataField="PSR_NAME" >
                <HeaderStyle Width="70px" HorizontalAlign="Left" />
                <ItemStyle Width="70px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="BEAT NAME" DataField="PSR_BEAT" >
                <HeaderStyle Width="70px" HorizontalAlign="Left" />
                <ItemStyle Width="70px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Product Code" DataField="Product_Code" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                 <asp:BoundField HeaderText="Product Name" DataField="Product_Name" >
                <HeaderStyle Width="150px" HorizontalAlign="Left" />
                <ItemStyle Width="150px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Sub Category" DataField="PRODUCT_SUBCATEGORY" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Product Group" DataField="Product_Group" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="LoadSheet No" DataField="LOADSHEET_NO" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="LoadSheet Date" DataField="LOADSHEET_DATE" DataFormatString="{0:dd-MMM-yyyy}" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Disc Item" DataField="DiscountItem" >
                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Scheme Item" DataField="SchemeItem" >
                <HeaderStyle Width="50px" HorizontalAlign="Left" />
                <ItemStyle Width="50px" HorizontalAlign="Left" />
                </asp:BoundField>
               
                <asp:BoundField HeaderText="QtyBOX" DataField="BOX" DataFormatString="{0:n2}" >
                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField HeaderText="QTYLTR" DataField="LTR" DataFormatString="{0:n2}" >
                 <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                 <asp:BoundField HeaderText="MRP" DataField="MRP" DataFormatString="{0:n2}" >
                 <HeaderStyle Width="80px" HorizontalAlign="Right" />
                <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField DataField="RATE" HeaderText="RATE" DataFormatString="{0:n2}" >
                <HeaderStyle HorizontalAlign="Right" Width="80px" />
                <ItemStyle HorizontalAlign="Right" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>

                 <asp:BoundField DataField="DISCPer" HeaderText="DISC %" DataFormatString="{0:n2}" >
                <HeaderStyle HorizontalAlign="Right" Width="80px" />
                <ItemStyle HorizontalAlign="Right" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>

                 <asp:BoundField DataField="DiscValue" HeaderText="DISCValue" DataFormatString="{0:n2}" >
                <HeaderStyle HorizontalAlign="Right" Width="80px" />
                <ItemStyle HorizontalAlign="Right" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>

                 <asp:BoundField DataField="Disc_AMOUNT" HeaderText="DiscAMOUNT" DataFormatString="{0:n2}" >
                <HeaderStyle HorizontalAlign="Right" Width="80px" />
                <ItemStyle HorizontalAlign="Right" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>

                <asp:BoundField DataField="TAX_CODE" HeaderText="TAX%" DataFormatString="{0:n2}" >
                <HeaderStyle HorizontalAlign="Right" Width="80px" />
                <ItemStyle HorizontalAlign="Right" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>

                <asp:BoundField DataField="TAX_AMOUNT" HeaderText="TAXValue" DataFormatString="{0:n2}" >
                <HeaderStyle HorizontalAlign="Right" Width="80px" />
                <ItemStyle HorizontalAlign="Right" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>

                <asp:BoundField DataField="ADDTAX_CODE" HeaderText="ADDTAX%" DataFormatString="{0:n2}" >
                <HeaderStyle HorizontalAlign="Right" Width="80px" />
                <ItemStyle HorizontalAlign="Right" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>

                <asp:BoundField DataField="ADDTAX_AMOUNT" HeaderText="ADDTAX Value" DataFormatString="{0:n2}" >
                <HeaderStyle HorizontalAlign="Right" Width="80px" />
                <ItemStyle HorizontalAlign="Right" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>

                 <asp:BoundField DataField="LINEAMOUNT" HeaderText="LINE VALUE" DataFormatString="{0:n2}" >
                <HeaderStyle HorizontalAlign="Right" Width="80px" />
                <ItemStyle HorizontalAlign="Right" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>

                <asp:BoundField DataField="AMOUNT" HeaderText="AMOUNT" DataFormatString="{0:n2}" >
                <HeaderStyle HorizontalAlign="Right" Width="80px" />
                <ItemStyle HorizontalAlign="Right" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>
                
                <asp:BoundField DataField="INVOICE_NO" HeaderText="INVOICE NO"  >
                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                <ItemStyle HorizontalAlign="Left" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>
                
                <asp:BoundField DataField="INVOIC_DATE" HeaderText="INVOICE DATE" nulldisplaytext="" DataFormatString="{0:dd-MMM-yyyy}" >
                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                <ItemStyle HorizontalAlign="Left" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>

                <asp:BoundField DataField="STATUS" HeaderText="STATUS"  >
                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                <ItemStyle HorizontalAlign="Left" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>

                <asp:BoundField DataField="OrderType" HeaderText="OrderType"  >
                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                <ItemStyle HorizontalAlign="Left" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>

                <asp:BoundField DataField="JumpCall" HeaderText="JumpCall"  >
                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                <ItemStyle HorizontalAlign="Left" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>
                
                <asp:BoundField DataField="LAT" HeaderText="LAT"  >
                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                <ItemStyle HorizontalAlign="Left" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>

                <asp:BoundField DataField="LONG" HeaderText="LONG"  >
                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                <ItemStyle HorizontalAlign="Left" Width="80px" VerticalAlign="Middle"/>
                </asp:BoundField>
            </Columns>--%>
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
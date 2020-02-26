<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPriceMaster.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPriceMaster" %>

<%--<%@ Register TagPrefix="ob" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>--%>
<%--<%@ Register TagPrefix="ob" Namespace="Obout.SuperForm" Assembly="obout_SuperForm" %>--%>
<%@ Register TagPrefix="owd" Namespace="OboutInc.Window" Assembly="obout_Window_NET" %>
<%@ Register TagPrefix="ob" Namespace="Obout.Interface" Assembly="obout_Interface" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

     <div style="border-bottom-style: solid; border-bottom-width: 5px; border-bottom-color: #000066" >
        <table>
            <tr>
                <td>
                   <asp:Button ID="Button2" runat="server" Text="New" CssClass="button" Height="31px" />
                    &nbsp;&nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Save" CssClass="button" Height="31px" />
                </td>
                <td style="padding: 0px 0px 0px 300px;">
                    <asp:DropDownList ID="DropDownList3" runat="server" CssClass="ddl" data-toggle="dropdown"   Width="200" style="margin-left: 0px" >
                      <asp:ListItem>Pricing Pattern</asp:ListItem>
                       <asp:ListItem>Customer Group</asp:ListItem>
                       <asp:ListItem>Customer Channel</asp:ListItem>
                       <asp:ListItem>Sate</asp:ListItem>
                   </asp:DropDownList>
&nbsp;:

&nbsp;</td>
                <td>
                    <div>
	                    <asp:TextBox ID="TextBox3" runat="server" CssClass="input1 cf" placeholder="Search here..." />
                        <span id="span1" class="arrow_box" onmouseover="test()" onmouseout="test1()">
	                       <asp:Button ID="Button4" runat="server"  CssClass="button1 cf" style="margin:0px 0px 0px -2px"  Text="Search"></asp:Button>     
                        </span>
                   </div>
                   
                    </td>
                </tr>
       </table>
    </div>
  
    <div style="width: 98%;height: 18px;border-radius: 4px;margin: 10px 0px 0px 5px;background-color: #1564ad;padding: 2px 0px 0px 0px;" >
      <span style="font-family: Segoe UI;font-size: 11px;font-weight: bold;margin: 6px 0px 0px 10px;">Price Master</span>
    </div>
     <div class="form-style-6">
           <table style="width:50%;border-spacing:0px">
                       <tr>
                            <td>State</td>
                            <td><asp:DropDownList ID="drpState" runat="server" Width="200" style="margin-left: 0px"></asp:DropDownList></td>
                            <td class="tdpadding">&nbsp;</td>                            
                        </tr>
                        <tr>
                              <td>Pricing Pattern</td>                            
                             <td><asp:DropDownList ID="drpPricePattern" runat="server" Width="200" style="margin-left: 0px"></asp:DropDownList></td>
                              <td>&nbsp;</td>
                        </tr>
                        <tr>
                              <td>Customer Group</td>
                              <td><asp:TextBox ID="txtCustomerGroup" runat="server" Width="200"  ></asp:TextBox></td>
                              <td>&nbsp;</td>                              
                        </tr>
                        <tr>
                              <td>Customer Channel</td>
                              <td><asp:TextBox ID="txtCustomerChannel" runat="server" Width="200"  ></asp:TextBox></td>
                              <td>&nbsp;</td>                              
                        </tr>                       
         </table>
         <%--<ob:Grid id="grid1" runat="server" AllowPageSizeSelection="False"  AutoGenerateColumns="False" PageSize="4">
              <AddEditDeleteSettings AddLinksPosition="Bottom" NewRecordPosition="Bottom"></AddEditDeleteSettings>
              <Columns>
                  
                  <ob:Column AccessibleHeaderText="Test" HeaderText="Customer Group"  Index="1" SortOrder="None" DataField="Customer_Group" Width="140">                    
                  </ob:Column>
                  <ob:Column Index="2" SortOrder="None" HeaderText="Customer Code" DataField="Customer_Code" Width="140">                     
                  </ob:Column>
                  <ob:Column Index="3" SortOrder="None" HeaderText="Customer Name" DataField="Customer_Name" Width="140">                     
                  </ob:Column>
                  <ob:Column Index="4" SortOrder="None" HeaderText="Address" DataField="Address"  Width="120">                     
                  </ob:Column>                 
              </Columns>
          
            <ExportingSettings Encoding="Default" ExportedFilesTargetWindow="Current"></ExportingSettings>

            <FilteringSettings FilterLinksPosition="Bottom" FilterPosition="Bottom" InitialState="Hidden" MatchingType="AllFilters"></FilteringSettings>

            <MasterDetailSettings LoadingMode="OnCallback" State="Collapsed"></MasterDetailSettings>

            <PagingSettings PageSizeSelectorPosition="Bottom" Position="Bottom"></PagingSettings>

            <ScrollingSettings FixedColumnsPosition="Left"></ScrollingSettings>
        </ob:Grid>
         <span style="font-family: Segoe UI;font-size: 11px;font-weight: bold;margin: 6px 0px 0px 10px;">Material Group for Normal Pattern</span>
         <ob:Grid id="grid2" runat="server" AllowPageSizeSelection="False"  AutoGenerateColumns="False" PageSize="4">
              <AddEditDeleteSettings AddLinksPosition="Bottom" NewRecordPosition="Bottom"></AddEditDeleteSettings>
              <Columns>
                  
                  <ob:Column AccessibleHeaderText="Test" HeaderText="Material Group"  Index="1" SortOrder="None" DataField="Material_Group" Width="140">                    
                  </ob:Column>
                  <ob:Column Index="2" SortOrder="None" HeaderText="R M %" DataField="RM" Width="100">                     
                  </ob:Column>
                  <ob:Column Index="3" SortOrder="None" HeaderText="Value" DataField="RMValue" Width="100">                     
                  </ob:Column>
                  <ob:Column Index="4" SortOrder="None" HeaderText="D M %" DataField="DM"  Width="100">                     
                  </ob:Column> 
                  <ob:Column Index="5" SortOrder="None" HeaderText="Value" DataField="DMValue"  Width="100">                     
                  </ob:Column>    
                   <ob:Column Index="6" SortOrder="None" HeaderText="Vat %" DataField="Vat"  Width="100">                     
                  </ob:Column> 
                  <ob:Column Index="7" SortOrder="None" HeaderText="Value" DataField="VatValue"  Width="100">                     
                  </ob:Column>    
                  <ob:Column Index="8" SortOrder="None" HeaderText="FromDate" DataField="FromDate"  Width="100">                     
                  </ob:Column>    
                  <ob:Column Index="9" SortOrder="None" HeaderText="ToDate" DataField="ToDate"  Width="100">                     
                  </ob:Column>                 
              </Columns>
          
            <ExportingSettings Encoding="Default" ExportedFilesTargetWindow="Current"></ExportingSettings>

            <FilteringSettings FilterLinksPosition="Bottom" FilterPosition="Bottom" InitialState="Hidden" MatchingType="AllFilters"></FilteringSettings>

            <MasterDetailSettings LoadingMode="OnCallback" State="Collapsed"></MasterDetailSettings>

            <PagingSettings PageSizeSelectorPosition="Bottom" Position="Bottom"></PagingSettings>

            <ScrollingSettings FixedColumnsPosition="Left"></ScrollingSettings>
        </ob:Grid>--%>

         <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="772px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="2" ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="3">
             <Columns>
                 <asp:BoundField HeaderText="Customer Group" DataField="Customer_Group" />
                 <asp:TemplateField HeaderText="Customer Code">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Customer_Code") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:DropDownList ID="DropDownList1" runat="server">
                         </asp:DropDownList>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Customer Name">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Customer_Name") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:DropDownList ID="DropDownList2" runat="server">
                         </asp:DropDownList>
                     </ItemTemplate>
                 </asp:TemplateField>
                
             </Columns>
             <EmptyDataTemplate>
                 No Record Found...
             </EmptyDataTemplate>
             <FooterStyle BackColor="#bfbfbf" />
             <HeaderStyle BackColor="#bfbfbf"  ForeColor="#000000" />
             <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
             <RowStyle ForeColor="#000066" />
             <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
             <SortedAscendingCellStyle BackColor="#F1F1F1" />
             <SortedAscendingHeaderStyle BackColor="#007DBB" />
             <SortedDescendingCellStyle BackColor="#CAC9C9" />
             <SortedDescendingHeaderStyle BackColor="#00547E" />
         </asp:GridView>
          <span style="font-family: Segoe UI;font-size: 11px;font-weight: bold;margin: 6px 0px 0px 10px;">
           <br />
               Material Group for Normal Pattern<br />
           </span>

     &nbsp;<asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="772px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="4">
             <Columns>
                 <asp:BoundField HeaderText="Material Group" DataField="Customer_Group" />
                 <asp:BoundField HeaderText="R M %" DataField="RM" />
                 <asp:BoundField HeaderText="Value" DataField="RMValue" />                 
                 <asp:BoundField HeaderText="D M %" DataField="DM" />
                 <asp:BoundField HeaderText="Value" DataField="DMValue" />  
                 <asp:BoundField HeaderText="Vat %" DataField="Vat" />
                 <asp:BoundField HeaderText="Value" DataField="VatValue" />  
                 <asp:BoundField HeaderText="Valid From" DataField="ValidFrom" />
                 <asp:BoundField HeaderText="Valid To" DataField="ValidTo" />  
             </Columns>
             <EmptyDataTemplate>
                 No Record Found...
             </EmptyDataTemplate>
             <FooterStyle BackColor="#bfbfbf" />
             <HeaderStyle BackColor="#bfbfbf"  ForeColor="#000000" />
             <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
             <RowStyle ForeColor="#000066" />
             <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
             <SortedAscendingCellStyle BackColor="#F1F1F1" />
             <SortedAscendingHeaderStyle BackColor="#007DBB" />
             <SortedDescendingCellStyle BackColor="#CAC9C9" />
             <SortedDescendingHeaderStyle BackColor="#00547E" />
         </asp:GridView>
          <span style="font-family: Segoe UI;font-size: 11px;font-weight: bold;margin: 6px 0px 0px 10px;">
           <br />
               Material List for Normal Pattern<br />
           </span>
          &nbsp;
         <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="True" ShowFooter="True" Width="772px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True"  AllowPaging="True" PageSize="4">
             <Columns>
                 <asp:BoundField HeaderText="Material Group" DataField="Material_Group" />
                 <asp:BoundField HeaderText="Material Code" DataField="Material_Code" />
                 <asp:BoundField HeaderText="Material Name" DataField="Material_Name" />
                 <asp:BoundField HeaderText="R M %" DataField="RM" />
                 <asp:BoundField HeaderText="Value" DataField="RMValue" />                 
                 <asp:BoundField HeaderText="D M %" DataField="DM" />
                 <asp:BoundField HeaderText="Value" DataField="DMValue" />  
                 <asp:BoundField HeaderText="Vat %" DataField="Vat" />
                 <asp:BoundField HeaderText="Value" DataField="VatValue" />  
                 <asp:BoundField HeaderText="Valid From" DataField="ValidFrom" />
                 <asp:BoundField HeaderText="Valid To" DataField="ValidTo" />  
             </Columns>
             <EmptyDataTemplate>
                 No Record Found...
             </EmptyDataTemplate>
             <FooterStyle BackColor="#bfbfbf" />
             <HeaderStyle BackColor="#bfbfbf"  ForeColor="#000000" />
             <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
             <RowStyle ForeColor="#000066" />
             <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
             <SortedAscendingCellStyle BackColor="#F1F1F1" />
             <SortedAscendingHeaderStyle BackColor="#007DBB" />
             <SortedDescendingCellStyle BackColor="#CAC9C9" />
             <SortedDescendingHeaderStyle BackColor="#00547E" />
         </asp:GridView>

         <span style="font-family: Segoe UI;font-size: 11px;font-weight: bold;margin: 6px 0px 0px 10px;">
           <br />
              Material List for MRP Based Patter<br />
           </span>

     &nbsp;<asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="772px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="4">
             <Columns>                
                 <asp:BoundField HeaderText="Material Code" DataField="Material_Code" />
                 <asp:BoundField HeaderText="Material Name" DataField="Material_Name" />
                 <asp:BoundField HeaderText="MRP" DataField="MRP" />
                 <asp:BoundField HeaderText="Reversal Component of Vat" DataField="Reversal_Component_of_Vat" />                                 
                 <asp:BoundField HeaderText="Vat %" DataField="Vat" />
                 <asp:BoundField HeaderText="Value" DataField="VatValue" />  
                 <asp:BoundField HeaderText="Price" DataField="Price" />  
                 <asp:BoundField HeaderText="Valid From" DataField="ValidFrom" />
                 <asp:BoundField HeaderText="Valid To" DataField="ValidTo" />  
             </Columns>
             <EmptyDataTemplate>
                 No Record Found...
             </EmptyDataTemplate>
             <FooterStyle BackColor="#bfbfbf" />
             <HeaderStyle BackColor="#bfbfbf"  ForeColor="#000000" />
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

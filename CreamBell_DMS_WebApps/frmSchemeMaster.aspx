<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmSchemeMaster.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmSchemeMaster" %>

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
                <td style="padding: 10px" class="auto-style1">
                   <asp:Button ID="Button2" runat="server" Text="New" CssClass="button" Height="31px" />
                </td>
                <td style="padding: 0px 0px 0px 300px;" class="auto-style1">                  
                   <asp:DropDownList ID="DropDownList2" runat="server" CssClass="ddl" Width="200" style="margin-left: 0px" >
                         <asp:ListItem>Scheme Name</asp:ListItem>
                       <asp:ListItem>Scheme Code</asp:ListItem>
                       <asp:ListItem>Scheme Type</asp:ListItem>
                   </asp:DropDownList>
                   
                </td>
               <td>
                    <div>
	                    <asp:TextBox ID="TextBox2" runat="server" CssClass="input1 cf" placeholder="Search here..." />
                        <span id="span1" class="arrow_box" onmouseover="test()" onmouseout="test1()">
	                       <asp:Button ID="Button4" runat="server"  CssClass="button1 cf" style="margin:0px 0px 0px -2px"  Text="Search"></asp:Button>     
                        </span>
                   </div>
                   
                    </td>
                </tr>   
     </table>
   </div>
    <div style="width: 98%;height: 18px;border-radius: 4px;margin: 10px 0px 0px 5px;background-color: #1564ad;padding: 2px 0px 0px 0px;" >
      <span style="font-family: Segoe UI;font-size: 11px;font-weight: bold;margin: 6px 0px 0px 10px;">Scheme Master</span>
    </div>
    <div class="form-style-6">
           <table style="width:50%;border-spacing:0px">
                       <tr>
                            <td>Scheme Code</td>
                            <td><asp:TextBox ID="txtCustomerGroup" runat="server" Width="200"  ></asp:TextBox></td>
                            <td class="tdpadding">&nbsp;</td>                            
                        </tr>
                        <tr>
                              <td>Name</td>                            
                             <td><asp:TextBox ID="TextBox3" runat="server" Width="200"  ></asp:TextBox></td>
                              <td>&nbsp;</td>
                        </tr>
                        <tr>
                              <td>Customer Group</td>
                              <td><asp:DropDownList ID="drpDiscountType" runat="server" Width="200" style="margin-left: 0px"></asp:DropDownList></td>
                              <td>&nbsp;</td>                              
                        </tr>
                        <tr>
                              <td>Scheme Type</td>
                              <td><asp:DropDownList ID="drpCustomerGroup" runat="server" Width="200" style="margin-left: 0px"></asp:DropDownList></td>
                              <td>&nbsp;</td>                              
                        </tr>
                        <tr>
                              <td>From Date</td>
                              <td>  <asp:TextBox ID="TextBox4" runat="server" Width="200"  ></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TextBox4" Format="dd/mm/yyyy"></asp:CalendarExtender>                            
                              </td>
                              <td>&nbsp;</td>                              
                        </tr>  
                        <tr>
                              <td>TO Date</td>
                              <td><asp:TextBox ID="TextBox5" runat="server" Width="200"  ></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TextBox5" Format="dd/mm/yyyy"></asp:CalendarExtender>                            
                              </td>
                              <td>&nbsp;</td>                              
                        </tr>                                              
         </table>
           <br />
         <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="772px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="2" ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="3">
             <Columns>
                 
                 <asp:TemplateField HeaderText="State">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("State") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:DropDownList ID="DropDownList1" runat="server">
                         </asp:DropDownList>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Area">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Area") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:DropDownList ID="DropDownList1" runat="server">
                         </asp:DropDownList>
                     </ItemTemplate>
                 </asp:TemplateField>
                  <asp:TemplateField HeaderText="Distributor">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Distributor") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:DropDownList ID="DropDownList1" runat="server">
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
           <br />
        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="True" ShowFooter="True" Width="567px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
             <Columns>
                 <asp:BoundField HeaderText="Scheme Code" DataField="Scheme_Code" />
                 <asp:BoundField HeaderText="Name" DataField="Test1" />
                 <asp:BoundField HeaderText="Customer Group" DataField="Test1" />
                 <asp:BoundField HeaderText="Type"  DataField="Test1"/>
                 
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
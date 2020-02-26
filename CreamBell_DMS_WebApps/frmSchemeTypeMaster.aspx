<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmSchemeTypeMaster.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmSchemeTypeMaster" %>

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
                <td style="padding: 10px">
                   <asp:Button ID="Button2" runat="server" Text="New" CssClass="button" Height="31px" />
                    &nbsp;&nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Save" CssClass="button" Height="31px" />
                </td>
                <td style="padding: 0px 0px 0px 300px;">
                    <asp:DropDownList ID="DropDownList3" runat="server" CssClass="ddl" data-toggle="dropdown"   Width="200" style="margin-left: 0px" >
                      <asp:ListItem>Scheme Name</asp:ListItem>
                       <asp:ListItem>Customer Group</asp:ListItem> 
                   </asp:DropDownList>
&nbsp;:

&nbsp;</td>
                <td>
                    <div>
	                    <asp:TextBox ID="TextBox4" runat="server" CssClass="input1 cf" placeholder="Search here..." />
                        <span id="span1" class="arrow_box" onmouseover="test()" onmouseout="test1()">
	                       <asp:Button ID="Button4" runat="server"  CssClass="button1 cf" style="margin:0px 0px 0px -2px"  Text="Search"></asp:Button>     
                        </span>
                   </div>
                   
                    </td>
                </tr>
       </table>
    </div>


    <div style="width: 98%;height: 18px;border-radius: 4px;margin: 10px 0px 0px 5px;background-color: #1564ad;padding: 2px 0px 0px 0px;" >
      <span style="font-family: Segoe UI;font-size: 11px;font-weight: bold;margin: 6px 0px 0px 10px;">Scheme Type Master</span>
    </div>
    <div class="form-style-6">
        <table style="width:50%;border-spacing:0px">
                       <tr>
                            <td>Customer Group</td>
                            <td><asp:TextBox ID="txtCustomerGroup" runat="server" Width="200"  ></asp:TextBox></td>
                            <td class="tdpadding">&nbsp;</td>                            
                        </tr>
                        <tr>
                              <td>Scheme Type</td>                            
                             <td><asp:TextBox ID="TextBox3" runat="server" Width="200"  ></asp:TextBox></td>
                              <td>&nbsp;</td>
                        </tr>
                        <tr>
                              <td>Description</td>
                              <td><asp:TextBox ID="TextBox6" runat="server" Width="200"  ></asp:TextBox></td>
                              <td>&nbsp;</td>                              
                        </tr>
                        <tr>
                              <td>Condition</td>
                              <td><asp:TextBox ID="TextBox7" runat="server" Width="200"  ></asp:TextBox></td>
                              <td>&nbsp;</td>                              
                        </tr>
                        <tr>                             
                              <td>Material Group<asp:CheckBoxList ID="CheckBoxList1" runat="server" Height="18px" Width="181px"></asp:CheckBoxList></td>
                              <td>Material Category<asp:CheckBoxList ID="CheckBoxList2" runat="server" Height="18px" Width="181px"></asp:CheckBoxList></td>                              
                        </tr>
                                                                  
         </table>     
        
        <table>
            <tr>
                <td>Scheme SKU</td>
                <td>

                </td>
                <td>Free SKU &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 Value <asp:TextBox ID="TextBox2" runat="server" Width="100"  ></asp:TextBox></td>
            </tr>
            <tr>
                <td>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="493px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="2" ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="4">
             <Columns>
                 
                 <asp:TemplateField HeaderText="Meterial Group">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Meterial_Group") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:DropDownList ID="DropDownList1" runat="server">
                         </asp:DropDownList>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Material Category">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Material_Category") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:DropDownList ID="DropDownList1" runat="server">
                         </asp:DropDownList>
                     </ItemTemplate>
                 </asp:TemplateField>
                  <asp:TemplateField HeaderText="Material Name">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Material_Name") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:DropDownList ID="DropDownList1" runat="server">
                         </asp:DropDownList>
                     </ItemTemplate>
                 </asp:TemplateField>
                  <asp:TemplateField HeaderText="Material Code">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Material_Code") %>'></asp:TextBox>
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
                </td>
                <td>
                    
                </td>
                <td>
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="493px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="2" ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="4">
             <Columns>
                 
                 <asp:TemplateField HeaderText="Meterial Group">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Meterial_Group") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:DropDownList ID="DropDownList1" runat="server">
                         </asp:DropDownList>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Material Category">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Material_Category") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:DropDownList ID="DropDownList1" runat="server">
                         </asp:DropDownList>
                     </ItemTemplate>
                 </asp:TemplateField>
                  <asp:TemplateField HeaderText="Material Name">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Material_Name") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:DropDownList ID="DropDownList1" runat="server">
                         </asp:DropDownList>
                     </ItemTemplate>
                 </asp:TemplateField>
                  <asp:TemplateField HeaderText="Material Code">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Material_Code") %>'></asp:TextBox>
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
             </td>
                
        </tr>
   </table>         
        <br />        
        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="True" ShowFooter="True" Width="1013px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
             <Columns>
                 <asp:BoundField HeaderText="Coustomer Group" DataField="Customer Name" />
                 <asp:BoundField HeaderText="Scheme Type" DataField="Test1" />
                 <asp:BoundField HeaderText="Condition" DataField="Test1" />
                 <asp:BoundField HeaderText="Description"  DataField="Test1"/>
                 
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
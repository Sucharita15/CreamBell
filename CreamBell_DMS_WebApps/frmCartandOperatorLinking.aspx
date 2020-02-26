<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmCartandOperatorLinking.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmCartandOperatorLinking" %>
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
                   <asp:Button ID="Button1" runat="server" Text="New" CssClass="button" Height="31px" />
                    &nbsp;&nbsp;
                    <asp:Button ID="Button2" runat="server" Text="Save" CssClass="button" Height="31px" />
                </td>
                <td style="padding: 0px 0px 0px 300px;">
                    <asp:DropDownList ID="drpSearch" runat="server" CssClass="ddl caret" data-toggle="dropdown"   Width="200" style="margin-left: 0px" >
                       <asp:ListItem>Target Object</asp:ListItem>
                       <asp:ListItem>Distributor Name</asp:ListItem>
                   </asp:DropDownList>
&nbsp;:

&nbsp;</td>
                <td>
                    <div>
	                    <asp:TextBox ID="txtSerch" runat="server" CssClass="input1 cf" placeholder="Search here..." />
                        <span id="span1" class="arrow_box" onmouseover="test()" onmouseout="test1()">
	                       <asp:Button ID="btn2" runat="server"  CssClass="button1 cf" style="margin:0px 0px 0px -2px"  Text="Search"></asp:Button>     
                        </span>
                   </div>
                   
                    </td>
                </tr>
       </table>
    </div> 
     <div style="width: 98%;height: 18px;border-radius: 4px;margin: 10px 0px 0px 5px;background-color: #1564ad;padding: 2px 0px 0px 0px;" >
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         Vending Cart- Operator Linking

     </div>
     <div>
          <div style="overflow:auto;height:400px;margin: 10px 0px 0px 10px;" > 
         <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="998px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True">
             <Columns>
                 <asp:TemplateField HeaderText="VSRCode - Name">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("VSRCode") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:DropDownList ID="DropDownList1" runat="server">
                         </asp:DropDownList>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Cart Code">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Test1") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:DropDownList ID="DropDownList2" runat="server">
                         </asp:DropDownList>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:BoundField HeaderText="Cart Type" DataField="Test1" />
                 <asp:BoundField HeaderText="Cart Size" DataField="Test1" />
                 <asp:TemplateField HeaderText="Operator Name">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Test1") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:DropDownList ID="DropDownList3" runat="server">
                         </asp:DropDownList>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:BoundField HeaderText="Operator Code" />
                 <asp:BoundField HeaderText="Operator Contract" />
                 <asp:BoundField HeaderText="From Date"  DataField="Test1"/>
                 <asp:BoundField HeaderText="To Date" DataField="Test1" />
                 
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
    </div>
</asp:Content>

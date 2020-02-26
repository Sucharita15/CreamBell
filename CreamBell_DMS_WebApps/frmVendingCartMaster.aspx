<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmVendingCartMaster.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmVendingCartMaster" %>
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
                    <asp:DropDownList ID="drpSearch" runat="server" CssClass="ddl" data-toggle="dropdown"   Width="200" style="margin-left: 0px" >
                       <asp:ListItem>Cart Serial No</asp:ListItem>
                       <asp:ListItem>Cart Master Name</asp:ListItem>
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
         Vending Cart Master&nbsp;
    </div>
     <div style="display:block;width:100%" class="form-style-6">
        <table>
             <tr>
                <td>Cart Serial No</td>
               
                <td>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                 </td>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
                <td rowspan="18" style="text-align: left; vertical-align: top">
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="True">
                        <Columns>
                            <asp:BoundField HeaderText="Target Object" />
                            <asp:BoundField HeaderText="Object Name" />
                            <asp:BoundField HeaderText="Target Type" />
                            <asp:BoundField HeaderText="Target" />
                            <asp:BoundField HeaderText="Month-Year" />
                        </Columns>
                    </asp:GridView>
                 </td>
            </tr>
             <tr>
                <td>Cart Model No</td>
               
                <td>
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                 </td>
                <td></td>
            </tr>
             <tr>
                <td>Cart Type</td>
               
                <td>
                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                 </td>
                <td></td>
            </tr>
             <tr>
                <td>Cart Size</td>
               
                <td>
                    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                 </td>
                <td></td>
            </tr>
             <tr>
                <td>Cart Asset Number </td>
                
                <td>
                    <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                 </td>
                <td></td>
             </tr>
             <tr>
                <td>Cart Capacity</td>
              
                <td>
                    <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                 </td>
                <td></td>
                 </tr>
            <tr>
                <td>Cart Make</td>
             
                <td><asp:TextBox ID="TextBox7" runat="server"></asp:TextBox></td>
                <td></td>
            </tr>
                                                          <tr>
                <td>&nbsp;Cart Purchase Date</td>
                
                <td><asp:TextBox ID="TextBox8" runat="server"></asp:TextBox></td>
                <td>&nbsp;&nbsp;</td>

            </tr>
            
                                                          <tr>
                <td>Cart Company Name</td>
               
                <td><asp:TextBox ID="TextBox9" runat="server"></asp:TextBox></td>
                <td>&nbsp;</td>

            </tr>
            
                                                          <tr>
                <td>Cart Locaton-Adda</td>
               
                <td><asp:TextBox ID="TextBox10" runat="server"></asp:TextBox></td>
                <td>&nbsp;</td>

            </tr>
            
                                                          <tr>
                <td>Info2</td>
                
                <td><asp:TextBox ID="TextBox11" runat="server"></asp:TextBox></td>
                <td>&nbsp;</td>

            </tr>
            
                                                          <tr>
                <td>VRS Name</td>
               
                <td><asp:TextBox ID="TextBox12" runat="server"></asp:TextBox></td>
                <td>&nbsp;</td>

            </tr>
            
                                                          <tr>
                <td>VRS Contact</td>
               
                <td><asp:TextBox ID="TextBox13" runat="server"></asp:TextBox></td>
                <td>&nbsp;</td>

            </tr>
            
                                                          <tr>
                <td>VRS Address</td>
               
                <td><asp:TextBox ID="TextBox14" runat="server"></asp:TextBox></td>
                <td>&nbsp;</td>

            </tr>
            
                                                          <tr>
                <td>Cart Operator Name</td>
               
                <td><asp:TextBox ID="TextBox15" runat="server"></asp:TextBox></td>
                <td>&nbsp;</td>

            </tr>
            
                                                          <tr>
                <td>Cart Operator Contact</td>
               
                <td><asp:TextBox ID="TextBox16" runat="server"></asp:TextBox></td>
                <td>&nbsp;</td>

            </tr>
            
                                                          <tr>
                <td>Cart Operator Address</td>
               
                <td><asp:TextBox ID="TextBox17" runat="server"></asp:TextBox></td>
                <td>&nbsp;</td>

            </tr>
            
                                                          <tr>
                <td>Cart Condition</td>
               
                <td><asp:TextBox ID="TextBox18" runat="server"></asp:TextBox></td>
                <td>&nbsp;</td>

            </tr>
            
        </table>
    </div>
  
</asp:Content>

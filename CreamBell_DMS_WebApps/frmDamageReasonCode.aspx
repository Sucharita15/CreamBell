<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmDamageReasonCode.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmDamageReasonCode" %>
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

    <style type="text/css">
           /*DropDownCss*/
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
      <div style="width: 100%;height: 18px;border-radius: 4px;margin: 10px 0px 0px 5px; color:white ;background-color: #1564ad;padding: 2px 0px 0px 0px;" >
          Damage Reason Code <br />
 </div>
      <div >
        <table>
            <tr>
                <td style="padding: 10px">
                   <asp:Button ID="Button1" runat="server" Text="New" CssClass="button" Visible="false" Height="31px" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" Visible="false" Height="31px" OnClick="btnSave_Click" />
                </td>
                <td style="padding: 0px 0px 0px 150px;">
                    <asp:DropDownList ID="ddlSearch" runat="server" CssClass="ddl" data-toggle="dropdown"   Width="200" style="margin-left: 0px" Visible="False" >
                       <asp:ListItem>Damage Reason Code</asp:ListItem>
                       <asp:ListItem>Distributor Name</asp:ListItem>
                   </asp:DropDownList>
</td>
                <td>
                    <div>
	                    <asp:TextBox ID="txtSerch" runat="server" CssClass="input1 cf" placeholder="Search here..." Height="12px" Width="264px" Visible="False" />
                        <%--<span id="span1" class="arrow_box" onmouseover="test()" onmouseout="test1()" >--%>
	                        <%--<asp:Button ID="btnSearch" runat="server"  CssClass="button1 cf" style="margin:0px 0px 0px -2px"  Text="Search" OnClick="btnSearch_Click" Visible="False"></asp:Button>          
                        </span>--%>
                   </div>
                   
                    </td>
                </tr>
       </table>
    </div> 
    
      <%--<div style="width: 98%;height: 18px;border-radius: 4px;margin: 5px 0px 0px 5px;padding: 2px 0px 0px 0px;" >--%>
         <table style="display:none">
             <tr>
                 <td>Damage&nbsp;&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="Code :"></asp:Label>
                     <asp:TextBox ID="txtDamageCode" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
               <td>
                     <asp:Label ID="Label2" runat="server" Text="Description"></asp:Label>
                     <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                 </td>
             </tr>
           
         </table>

 <%--    </div>--%>
  <div>
          <div style="overflow:auto;height:400px;margin: 10px 0px 0px 10px;" > 
         <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="460px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
             <Columns>
                 <asp:TemplateField HeaderText="Code" >
                 <ItemTemplate>
                        <asp:LinkButton ID="lnkbtn" runat="server" Text='<%# Bind("DamageReason_Code") %>' OnClick="lnkbtn_Click" ></asp:LinkButton>
                 </ItemTemplate>                                      
                 <HeaderStyle HorizontalAlign="Left" Width="180px" />
                 <ItemStyle HorizontalAlign="Left" Width="180px" />
            </asp:TemplateField>
                 <asp:BoundField HeaderText="Damage Code" DataField="DamageReason_Code" Visible="False" >
                 <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                 </asp:BoundField>
                 <asp:BoundField HeaderText="Description" DataField="DamageReason_Name" >
                 
                 <HeaderStyle HorizontalAlign="Left" />
                 
                 <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
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
    </div>
</asp:Content>

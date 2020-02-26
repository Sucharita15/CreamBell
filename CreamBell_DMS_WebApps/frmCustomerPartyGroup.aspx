<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmCustomerPartyGroup.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmCustomerPartyGroup" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <script src="Javascript/custom.js"></script>
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


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <div style="height: 15px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #538dd5; padding: 0px 0px 5px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; color: white; margin: 0px 0px 0px 10px;">Customer Group</span>
    </div>
    <div style="width: 100%;">
        <table style="width: 100%;">
            <tr>
                  <td style="width: 10%;">
                    <asp:Button ID="Button1" runat="server" Text="New" CssClass="button" Visible="false" Height="31px" />


                </td>
                <td style="width: 20%;">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" Visible="false" Height="31px" OnClick="btnSave_Click" />
                </td>
                 <td style="width: 10%;">
                       </td>
                <td style="width: 30%;text-align:right">
                    <asp:DropDownList ID="ddlSearch" runat="server" CssClass="ddl" data-toggle="dropdown" Width="200px" Style="margin-left: 0px">
                        <asp:ListItem>Customer Group Code</asp:ListItem>
                        <asp:ListItem>Customer Group Name</asp:ListItem>
                    </asp:DropDownList>
              </td>
             <td style="width: 30%; text-align:left">
                    <div>
                        <asp:TextBox ID="txtSerch" runat="server"  placeholder="Search here..." />
                        <span id="span1"  onmouseover="test()" onmouseout="test1()">
                            <asp:Button ID="btnSearch" runat="server" Style="margin: 0px 0px 0px 0px" Text="Search" OnClick="btnSearch_Click"></asp:Button>
                        </span>
                    </div>
                     </td>
          </tr>
        </table>
    </div>



    <div style="width: 100%;">
        <table style="display: none">


            <tr>
                <td>Customer Group
                     <asp:Label ID="Label1" runat="server" Text="Code :"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
                <td>
                    <asp:TextBox ID="txtCustomergroupcode" runat="server" Visible="false"></asp:TextBox>
                </td>
                <td>&nbsp;&nbsp;&nbsp; &nbsp;</td>
                <td rowspan="3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;Customer Group Name:
                </td>
                <td>
                    <asp:TextBox ID="txtCustomergroupname" Visible="false" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Active/Inactive:</td>
                <td>
                    <asp:DropDownList ID="DropActive" runat="server" Visible="false">
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>

        <br />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="100%" BackColor="White" BorderColor="#CCCCCC"
            BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" PageSize="5" OnPageIndexChanging="GridView1_PageIndexChanging" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
           <AlternatingRowStyle BackColor="#CCFFCC" />
             <Columns>
                <asp:TemplateField HeaderText="Customer Group Code">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkbtn" runat="server" Text='<%# Bind("CustGroup_Code") %>' OnClick="lnkbtn_Click" OnClientClick="return false;"></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="180px" />
                    <ItemStyle HorizontalAlign="Left" Width="180px" />
                </asp:TemplateField>

                <asp:BoundField HeaderText="Customer Group Name" DataField="CustGroup_Name">

                    <HeaderStyle HorizontalAlign="Left" />

                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Active" DataField="Blocked">

                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
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

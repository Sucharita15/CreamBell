<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmVersionInfo.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmVersionInfo" %>


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

        function confirmChecked(ischecked) {
            if (ischecked) {
                //Will assign value from source to destination
                document.getElementById(‘txtDestination’).value = document.getElementById(‘txtSource’).value;
            }
            else {
                //Will assign value of destination to empty
                document.getElementById(‘txtDestination’).value = ”;
            }
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <div style="height: 15px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #538dd5; padding: 0px 0px 5px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; color: white; margin: 0px 0px 0px 10px;">Version Info</span>
    </div>
    <div style="width: 100%;">
        <table style="width: 100%;">
            <tr>
                <td style="width: 8%;text-align:left;padding-left:10px">
                      Version :</td>
                <td style="width: 10%">
                        <asp:TextBox ID="txtVersion" runat="server" width="100%" placeholder="Version Code..." MaxLength="50"  TabIndex="0"/>
                        </td>
                <td style="width: 8%;text-align:left;padding-left:10px">Description :
                </td>
                <td style="width: 30%">
                <asp:TextBox ID="txtDescription" runat="server"  width="100%" placeholder="Description..." MaxLength="150" TabIndex="1"/>
                </td>
              <td style="width: 8%;text-align:left;padding-left:10px">Download Link :
                </td>
                <td style="width: 30%">
                <asp:TextBox ID="txtDownloadLink" runat="server" width="100%" placeholder="Download Link..." MaxLength="150" TabIndex="2"/>
                </td>  
                <td>
                    <span id="span1"  onmouseover="test()" onmouseout="test1()">
                            <asp:Button ID="btnSave" runat="server" Style="margin: 0px 0px 0px 0px" Text="Save" OnClick="btnSave_Click" TabIndex="3"></asp:Button>
                        </span>
                </td>
          </tr>
        </table>
    </div>
    <div style="width: 100%;">
        <br />
        <asp:GridView ID="gvVersionInfo" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="100%" BackColor="White" BorderColor="#CCCCCC"
            BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" PageSize="5" OnPageIndexChanging="gvVersionInfo_PageIndexChanging">
           <AlternatingRowStyle BackColor="#CCFFCC" />
             <Columns>
                <asp:BoundField HeaderText="Version" DataField="VersionName">
                    <HeaderStyle HorizontalAlign="Left" Width="10%"/>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Description" DataField="Description">
                    <HeaderStyle HorizontalAlign="Left" Width="30%"/>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="30%" />
                </asp:BoundField>

                 <asp:TemplateField HeaderText="Download Link">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkURLAddress" runat="server" Text='<%# Bind("UrlAddress") %>'></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"  Width="30%" />
                    <ItemStyle HorizontalAlign="Left"  Width="30%" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Confirm">
                    <ItemTemplate>
                        <asp:HiddenField ID="hfVersionCode" runat="server" Value='<%# Bind("VersionCode") %>'></asp:HiddenField>
                        <asp:LinkButton ID="lnkbtn" runat="server" Text='<%# Eval("IsConfirm") %>' OnClick="lnkbtn_Click" OnClientClick="return confirm('Are You Sure?');" ></asp:LinkButton>
                        <%--<asp:CheckBox ID="chkConfirm" runat="server" AutoPostBack="true" onclick="confirmChecked(this.checked)" OnCheckedChanged="chkConfirm_CheckedChanged" Text='<%# Eval("IsConfirm").ToString() %>'/>--%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                    <ItemStyle HorizontalAlign="Left" Font-Bold="True" ForeColor="#000066" Width="10%"/>
                </asp:TemplateField>
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

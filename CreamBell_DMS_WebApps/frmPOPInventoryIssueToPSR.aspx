<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPOPInventoryIssueToPSR.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPOPInventoryIssueToPSR" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function CheckNumeric(e) {          //--Only For Numbers //

            if (window.event) // IE 
            {
                if ((e.keyCode < 48 || e.keyCode > 57) & e.keyCode != 8) {
                    event.returnValue = false;
                    return false;

                }
            }
            else { // Fire Fox
                if ((e.which < 48 || e.which > 57) & e.which != 8) {
                    e.preventDefault();
                    return false;

                }
            }
        }

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gvDetails.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=gvDetails.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead").append(gridHeader);
            $('#controlHead').css('position', 'absolute');
            $('#controlHead').css('top', '129');

        });

    </script>

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

        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        function IsNumeric(e) {

            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
            //document.getElementById("lblError") = ret ? "none" : "inline";
            if (keyCode == 8 || keyCode == 46 || keyCode == 189 || keyCode == 61 || keyCode == 45)
            { ret = true; }
            return ret;
        }


        $(document).ready(function () {
            $("select").searchable();
        });

        function InIEvent() {
            $(document).ready(function () {
                $("select").searchable();
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
                <span style="font-family: Segoe UI; color: white; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px;">POP Inventory Issue To PSR</span>
            </div>

    <asp:UpdatePanel ID="UpPanelPOPInventoryIssue" runat="server" UpdateMode="Conditional">
       <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
           <asp:PostBackTrigger ControlID="btnAdd" />
           <asp:PostBackTrigger ControlID="drpItemCategory" />
           <asp:PostBackTrigger ControlID="drpItemCode" />
           <asp:PostBackTrigger ControlID="DrpItemDescription" />
           <asp:PostBackTrigger ControlID="drpPSRName" />
           <asp:PostBackTrigger ControlID="txtPSRContact" />
           <asp:PostBackTrigger ControlID="txtQTYPcs" />
                        </Triggers>           
         
         <ContentTemplate>
    <table style="width: 100%; text-align: left">
        <tr>
                    <td style="width: 5%">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ReportSearch" AutoPostBack="true" width="70px" OnClick="btnSave_Click" TabIndex="9" />
                    </td>
                   
                    <td style="width: 85%">
                         <asp:Label ID="lblMessage" runat="server" Font-Size="Medium" Font-Italic="true" ForeColor="#CC0000"></asp:Label>
                   </td>
             <td style="width: 10%">
                        <asp:Label ID="lblsite" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>

    <table style="width: 100%; text-align: left">
        <tr>
            <td style="font-weight: 700">
                PSR Name:
            </td>
            <td>
                <asp:DropDownList ID="drpPSRName" runat="server" AutoPostBack="true" width="280px" CssClass="textboxStyleNew" OnSelectedIndexChanged="drpPSRName_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td style="font-weight: 700">
                PSR Contact:
            </td>
            <td>
                <asp:TextBox ID="txtPSRContact" runat="server" AutoPostBack="true" Width="100px" ReadOnly="True" CssClass="textboxStyleNew" TabIndex="1"></asp:TextBox>
            </td>
            <td style="font-weight: 700; ">
                Item Group:
             </td>
            <td >
           <asp:DropDownList ID="drpItemCategory" runat="server" AutoPostBack="true" width="185px" CssClass="textboxStyleNew" OnSelectedIndexChanged="drpItemCategory_SelectedIndexChanged" TabIndex="2">
              <asp:ListItem Text="POP" Value="POP"></asp:ListItem>
             </asp:DropDownList>
              </td>
            <td style="font-weight: 700;">
                Item Sub Group:
            </td>
            <td >
                <asp:DropDownList ID="drpItemCode" runat="server" AutoPostBack="true" Width="185px" CssClass="textboxStyleNew" OnSelectedIndexChanged="drpItemCode_SelectedIndexChanged" TabIndex="4"></asp:DropDownList>
              </td>
              </tr>
         <tr>
             <td style="font-weight: 700;">
                Item :
            </td>
            <td>
                <asp:DropDownList ID="DrpItemDescription" runat="server" AutoPostBack="true" Width="280px" CssClass="textboxStyleNew" TabIndex="5" OnSelectedIndexChanged="DrpItemDescription_SelectedIndexChanged"></asp:DropDownList>
              </td>
              <td style="font-weight: 700;">
                Available Quantity:</td>
             <td>
                  <asp:TextBox ID="txtAvailableQuantity" runat="server" width="90px" CssClass="textboxStyleNew" ReadOnly="True"  TabIndex="6" TextMode="Number"></asp:TextBox>
             </td>
              <td >
                Remark:</td>
               <td>
                <asp:TextBox ID="txtRemark" runat="server" width="175px" CssClass="textboxStyleNew" TextMode="MultiLine" MaxLength="200" TabIndex="7"></asp:TextBox>
            </td> 
            <td style="font-weight: 700;">
                QTY in PCs:
            </td>
            <td >
                <asp:TextBox ID="txtQTYPcs" runat="server" AutoPostBack="true" width="90px" onkeypress="CheckNumeric(event);" CssClass="textboxStyleNew" OnTextChanged="txtQTYPcs_TextChanged" TabIndex="8"></asp:TextBox>
            </td>
            <td  colspan="2" style="vertical-align: middle; text-align: center">
               <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="ReportSearch" width="71px" AutoPostBack="true" OnClick="btnAdd_Click" TabIndex="8"/>
            </td>
        </tr>
        </table>
 <div style="overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 100%;">
       <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None"
                BorderWidth="1px" CellPadding="3" GridLines="Horizontal" ShowFooter="True" ShowHeaderWhenEmpty="True" Width="100%">
                <AlternatingRowStyle BackColor="#F7F7F7" />
                <Columns>

                 <asp:TemplateField HeaderText="SLNO">
                        <ItemTemplate>
                            <span>
                                <%#Container.DataItemIndex + 1 %>
                            </span>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                  <asp:BoundField DataField="Item_Category" HeaderText="ITEM CATEGORY">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="ITEM SubCategory">
                        <ItemTemplate>
                            <asp:Label ID="Item_SubCategory" runat="server" Text='<%# Bind("Item_SubCategory") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="ITEM Code and Name">
                        <ItemTemplate>
                            <asp:Label ID="Item_Name" runat="server" Text='<%# Bind("Item_Name") %>'></asp:Label>
                            <asp:HiddenField ID="HiddenValueItemCode" Visible="false" runat="server" Value='<%# Bind("Item_Code") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                   
                     <asp:TemplateField HeaderText="QTY">
                         <ItemTemplate>
                             <asp:Label ID="QTY" runat="server" Text='<%# Bind("QTY") %>'></asp:Label>
                         </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" />
                         <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                </Columns>
                <FooterStyle BackColor="#B5C7DE" CssClass="table-condensed" ForeColor="#4A3C8C" />
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

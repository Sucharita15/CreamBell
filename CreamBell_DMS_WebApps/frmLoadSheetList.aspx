<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmLoadSheetList.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmLoadSheetList" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
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

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=GridView1.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=GridView1.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead").append(gridHeader);
            $('#controlHead').css('position', 'absolute');
            $('#controlHead').css('top', '129');

        });

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=GridView1.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            GridView1    $('#<%=GridView1.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#ControlHead1").append(gridHeader);
            $('#ControlHead1').css('position', 'absolute');
            $('#ControlHead1').css('top', '129');

        });

    </script>
    <script type="text/javascript">
        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        // row.style.backgroundColor = "aqua";
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        //and change rowcolor back to original
                        if (row.rowIndex % 2 == 0) {
                            //Alternating Row Color
                            //  row.style.backgroundColor = "#C2D69B";
                        }
                        else {
                            row.style.backgroundColor = "white";
                        }
                        inputList[i].checked = false;
                    }
                }
            }
        }
    </script>


    <style type="text/css">
        /*DropDownCss*/
        .ddl {
            background-image: url('Images/arrow-down-icon-black.png');
        }

            .ddl:hover {
                background-image: url('Images/arrow-down-icon-black.png');
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; color: white; margin: 0px 0px 0px 10px;">Load Sheet List</span>
    </div>


    <%--<asp:UpdatePanel runat="server" ID="dsdas">
        <ContentTemplate>--%>
    <table style="width: 100%; text-align: left">
        <tr>
            <td style="width: 50%">
                <asp:Button ID="Button2" runat="server" Text="MultiplePrint" CssClass="ReportSearch" Height="31px" OnClick="Button2_Click" />
            </td>
            <td style="width: 20%; text-align: right">
                <asp:DropDownList ID="ddlSearch" runat="server" Width="200px">
                    <asp:ListItem>Date</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 20%; text-align: right">
                <asp:TextBox ID="txtSerch" runat="server" placeholder="dd-MMM-yyyy" Width="187px" />

                <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtSerch" Format="dd-MMM-yyyy"></asp:CalendarExtender>

            </td>
            <td style="width: 10%; text-align: left">
                <span id="span1" onmouseover="test()" onmouseout="test1()">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                        <ContentTemplate>
                            <asp:Button ID="btn2" runat="server" Style="margin: 0px 0px 0px 2px" Width="80px" Text="Search" OnClick="btn2_Click"></asp:Button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </span>
            </td>

        </tr>
    </table>
    <%--  </ContentTemplate>
    </asp:UpdatePanel>--%>

    <div>
        <div id="controlHead" style="margin-top: 0px; margin-left: 9px; padding-right: 10px;"></div>
        <div style="height: 300px; overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px;">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <%--<div style="height: 200px; overflow: auto; margin-top: 10px; margin-left: 5px; padding-right: 10px; width: 98%;">--%>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="100%"
                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                        ShowHeaderWhenEmpty="True" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" PageSize="5"
                        OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound">
                        <AlternatingRowStyle BackColor="#CCFFCC" />
                        <Columns>
                            <asp:TemplateField>

                                <ItemTemplate>
                                    <asp:CheckBox ID="chklist" runat="server" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ident No">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtn" runat="server" Text='<%# Bind("LoadSheet_No") %>' OnClick="lnkbtn_Click"></asp:LinkButton>

                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="80px" />
                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                            </asp:TemplateField>


                            <%--  <asp:TemplateField HeaderText="Select">
                     <ItemTemplate>
                         <asp:CheckBox ID="CheckBox1" runat="server" Text='<%# Bind("LoadSheet_No") %>'  AutoPostBack ="true"  OnCheckedChanged="CheckBox1_CheckedChanged"/>
                     </ItemTemplate>
                 </asp:TemplateField>--%>

                            <%--<asp:TemplateField>  
                        <HeaderTemplate>   
                            <asp:CheckBox ID="chkAllSelect" runat="server" Text='<%# Bind("LoadSheet_No") %>' onclick="CheckAll(this);" />  
                        </HeaderTemplate>  
                        <ItemTemplate>  
                            <asp:CheckBox ID="chkSelect" runat="server" Text='<%# Bind("LoadSheet_No") %>'/>  
                        </ItemTemplate>  
                    </asp:TemplateField> --%>


                            <asp:BoundField HeaderText="Load Sheet Date" DataField="LoadSheet_Date" DataFormatString="{0:dd/MM/yy}">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Value" DataField="Value" DataFormatString="₹ {0:n2}">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>

                            <asp:TemplateField HeaderImageUrl="~/Images/printicon.png">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HPLinkPrint" runat="server" Font-Bold="True">View</asp:HyperLink>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" ForeColor="DarkGreen" />
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
                </ContentTemplate>
            </asp:UpdatePanel>


        </div>

    </div>

    <div id="fixedHeaderRow" runat="server" style="height: 30px; width: 100%; margin: 0; padding: 0">
    </div>
    <div style="height: 250px; overflow: auto; margin-top: 5px; margin-left: 5px; padding-right: 10px; width: 100%;">
        <asp:UpdatePanel runat="server" ID="UpdatePanel2">
            <ContentTemplate>
                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
                    CellPadding="3" ShowHeaderWhenEmpty="True" Width="98%" OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="5">
                    <AlternatingRowStyle BackColor="#CCFFCC" />
                    <Columns>
                        <asp:BoundField HeaderText="SO Number" DataField="SO_NO" Visible="False">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Customer Code" DataField="Customer_Code">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="address1" HeaderText="Customer Address">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="product_group" HeaderText="Product Group">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Product Code" DataField="product_code">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="product_name" HeaderText=" Product Name" ApplyFormatInEditMode="True">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Qty(Crates)" DataField="Crates" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Qty(Conv)" DataField="BOX" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Qty(LTR)" DataField="LTR" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                         <asp:BoundField HeaderText="Qty(Box)" DataField="BOXQTY" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                         <asp:BoundField HeaderText="Qty(Pcs)" DataField="PCSQTY" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                         <asp:BoundField HeaderText="Qty(BoxPcs)" DataField="BOXPCS" DataFormatString="{0:n2}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
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
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    <%--<div style="height: 380px; overflow: auto;margin-top:5px; margin-left:5px;padding-right:10px;">

          <asp:Panel ID="panelContainer" runat="server" Height="200px" Width="100%"  ScrollBars="Vertical">--%>
    <div style="width: 100%">
        <asp:UpdatePanel runat="server" ID="UpdatePanel3">
            <ContentTemplate>

                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3"
                    ShowHeaderWhenEmpty="True" Width="100%" PageSize="5" OnPageIndexChanging="GridView1_PageIndexChanging" ShowHeader="False" Visible="False" Height="16px">
                    <Columns>
                        <asp:BoundField HeaderText="Product Group" DataField="product_group">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Product Code" DataField="product_code">
                            <HeaderStyle Width="80px" />
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="product_name" HeaderText="Product Name">
                            <HeaderStyle Width="80px" />
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText=" Qty(Crates)" DataField="Crates" DataFormatString="{0:n2}">
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Qty(Box)" DataField="BOX" DataFormatString="{0:n2}">
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Qty(LTR)" DataField="LTR" DataFormatString="{0:n2}">
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--</asp:Panel>--%>

    <%--</div>--%>
    <div style="width: 100%">
        <%--  <asp:UpdatePanel runat="server" ID="UpdatePanel4">
            <ContentTemplate>--%>
        <rsweb:ReportViewer ID="ReportViewer1" Visible="false" runat="server" Width="100%" Height="800px">
        </rsweb:ReportViewer>
        <%--  </ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>


</asp:Content>

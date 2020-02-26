<%@ Page Title="Invoice Preparation" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmInvoicePrepration.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmInvoicePrepration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />

    <script type="text/javascript">

        function checkAll(objRef) {
            //debugger;
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox") {
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
        .ModalPoupBackgroundCssClass {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.4;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-width: 2px;
            border-style: inset;
            width: 90%;
            height: auto;
        }

        .hiddencol {
            display: none;
        }

        div#ctl00_ContentPage_Panel1 {
            background: transparent !important;
        }

        table#ctl00_ContentPage_gridviewRecords.Grid td {
            padding-left: 6px;
        }

        .alignment {
            text-align: center;
            background: #dedede;
            padding: 8px 0px;
            width: 100%;
        }

        .Operationbutton {
            float: right;
            margin-right: 15px;
            margin-bottom: 2px;
            .nav ul li a;

        {
            width: 270px !important;
        }

        .nav ul .dropdown:hover ul {
            left: 270px;
            top: 0px;
        }

        .nav ul li a {
            /* Layout */
            padding: 8px 20px;
            width: 270px !important;
            display: block;
            position: relative;
            /* Text */
            text-decoration: none;
            font-size: 13px;
            /* Background & effects */
            background: #ddd;
            border-bottom: 1px solid #ddd;
            /*
                background: rgba(0, 0, 0, .75);
                */
            /* -webkit-transition: color .3s ease-in, background .3s ease-in;
            -moz-transition: color .3s ease-in, background .3s ease-in;
            -o-transition: color .3s ease-in, background .3s ease-in;
            -ms-transition: color .3s ease-in, background .3s ease-in;*/
        }
    </style>


    <script type="text/javascript">
        function ModalClose() {
            window.location = "frmInvoicePrepration.aspx";
        };
        function MyMessage(x, y) {

            alert("No. of Sale Order Selected : " + x + "\nNo. of Invoices Generated : " + y);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 0px 0px 0px 0px; background-color: #0085ca; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; margin: 0px 0px 0px 0px; color: #FFFFFF;">Invoice Preparation</span>
    </div>

    <div style="width: 100%;">
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Label ID="lblMessge" runat="server" ForeColor="Red" Font-Size="Small"></asp:Label>
                </td>
                <td style="width: 35%">
                    <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>--%>
                    <asp:Button ID="btnInvPreparation" runat="server" Text="Generate Invoice" CssClass="ReportSearch" Height="31px" ToolTip="Click To Generate Sale Invoice !" OnClick="btnInvPreparation_Click" />
                    <asp:Button ID="btnSaleOrder" runat="server" Text="Create Sale Order" CssClass="ReportSearch" Height="31px" ToolTip="Click To Generate Sale Order !" OnClick="btnSaleOrder_Click" />
                    <asp:Button ID="btnBulkInv" runat="server" Text="Generate Bulk Invoice" CssClass="ReportSearch" Height="31px" ToolTip="Click To Generate Bulk Invoice !" OnClick="btnBulkInv_Click" />
                    <asp:Button ID="Button2" runat="Server" Text="Show Report" Style="display: none;" />
                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button2"
                        PopupControlID="Panel1" CancelControlID="Button4"
                        BackgroundCssClass="ModalPoupBackgroundCssClass" BehaviorID="ModalPopupExtender1">
                    </asp:ModalPopupExtender>
                    <%--  </ContentTemplate>
                        </asp:UpdatePanel>--%>
                </td>
                <td style="width: 15%">
                    <asp:Label ID="lblsord" Text="Sort On" runat="server"></asp:Label>
                    <asp:DropDownList Width="100%" ID="ddlsort" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsort_SelectedIndexChanged">
                        <asp:ListItem Text="SO NO" Value="so_no"></asp:ListItem>
                        <asp:ListItem Text="SO Date" Value="SO_Date"></asp:ListItem>
                        <asp:ListItem Text="Customer Name" Value="C_Name"></asp:ListItem>
                        <asp:ListItem Text="Customer Code" Value="C_Code"></asp:ListItem>
                        <asp:ListItem Text="Customer Group" Value="Customer_Group"></asp:ListItem>
                        <asp:ListItem Text="LoadSheet NO" Value="LoadSheet_No"></asp:ListItem>
                        <asp:ListItem Text="LoadSheet Date" Value="LoadSheet_Date"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 10%">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton AutoPostBack="true" ID="rddesc" Text="Descending" Checked="true" GroupName="sort" OnCheckedChanged="rddesc_CheckedChanged" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButton AutoPostBack="true" ID="rdasc" Text="Ascending" GroupName="sort" OnCheckedChanged="rddesc_CheckedChanged" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>

                <td style="width: 10%; text-align: right">
                    <asp:UpdatePanel ID="upanelddlsearch" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlSerch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSerch_SelectedIndexChanged" Width="200px">
                                <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                <asp:ListItem Text="SO No" Value="SO_No"></asp:ListItem>
                                <asp:ListItem Text="Load Sheet No" Value="LoadSheet_No"></asp:ListItem>
                                <asp:ListItem Text="Customer Name" Value="Customer Name"></asp:ListItem>
                                <asp:ListItem>Customer Group</asp:ListItem>
                                <asp:ListItem Value="SO_Date">Date</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlSerch" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>

                </td>
                <td style="width: 20%; text-align: right">

                    <asp:TextBox ID="txtSearch" runat="server" placeholder="Search here..."> </asp:TextBox>
                    &nbsp;&nbsp;&nbsp;
                     <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtSearch" MinimumPrefixLength="1" EnableCaching="true"
                         CompletionSetCount="1" CompletionInterval="1000" ServiceMethod="GetProductDescription">
                     </asp:AutoCompleteExtender>

                    <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>
                <td style="width: 10%; text-align: right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"></asp:Button>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtSearch" Format="dd-MMM-yyyy"></asp:CalendarExtender>

                </td>
            </tr>
        </table>
    </div>


    <%--kushagra --%>


    <div style="height: 442px; overflow: auto; margin-top: 3px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <div>


                    <asp:Panel ID="Panel1" runat="server" Style="display: none; width: 85%; background-color: silver">
                        <div class="alignment">
                            <asp:Label ID="InvoiceGenerated" runat="server" Text="" Font-Bold="true" ForeColor="DarkBlue" Font-Names="Seoge UI"> </asp:Label>
                            <span style="color: darkblue; font-weight: 600; text-align: center">Status!!</span>
                            <asp:Button ID="Button4" runat="server" CssClass="Operationbutton" data-dismiss="modal" Text="Close" aria-hidden="true" OnClientClick="ModalClose()"></asp:Button>
                        </div>
                        <p></p>
                        <div style="overflow-x: scroll; width: 100%; height: 100%">
                            <asp:GridView ID="gridviewRecords" runat="server" Style="width: 100%" Font-Size="Small" CssClass="Grid" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                <AlternatingRowStyle BackColor="#CCFFCC" />
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="White" CssClass="pgr" ForeColor="#000066" HorizontalAlign="Left" />
                                <RowStyle ForeColor="#000066" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </div>
                <asp:GridView ID="gvHeader" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" Width="100%"
                    ShowHeaderWhenEmpty="True" OnRowDataBound="gvHeader_RowDataBound" OnSelectedIndexChanged="gvHeader_SelectedIndexChanged">
                    <%--        <AlternatingRowStyle BackColor="#CCFFCC" />--%>
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" AutoPostBack="true" OnCheckedChanged="checkAll_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSONO" runat="server" AutoPostBack="true"></asp:CheckBox>
                                <%--<asp:LinkButton ID="lnkbtn" runat="server" Text='<%# Bind("SO_NO") %>' OnClick="lnkbtn_Click" ></asp:LinkButton>--%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="20px" />
                            <ItemStyle HorizontalAlign="Left" Width="20px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="SO NO">
                            <ItemTemplate>
                                <%-- <asp:CheckBox ID="chkSONO" runat="server"  OnCheckedChanged="chkSONO_CheckedChanged" ></asp:CheckBox>                       --%>
                                <asp:LinkButton ID="lnkbtn" Enabled="false" runat="server" Text='<%# Bind("SO_NO") %>' OnClick="lnkbtn_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="105px" />
                            <ItemStyle HorizontalAlign="Left" Width="105px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SO Date">
                            <ItemTemplate>
                                <asp:Label ID="SO_Date" runat="server" Text='<%# Eval("SO_Date", "{0:dd MMM yyyy}")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" Delivery Date">
                            <ItemTemplate>
                                <asp:Label ID="Delivery_Date" runat="server" Text='<%# Eval("Delivery_Date")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Group">
                            <ItemTemplate>
                                <asp:Label ID="Customer_Group" runat="server" Text='<%# Eval("Customer_Group")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Name">
                            <ItemTemplate>
                                <asp:Label ID="Customer_Name" runat="server" Text='<%# Eval("Customer_Name")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="130px" />
                            <ItemStyle HorizontalAlign="Left" Width="130px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Load Sheet No">
                            <ItemTemplate>
                                <asp:Label ID="LoadSheet_No" runat="server" Text='<%# Eval("LoadSheet_No")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Load Sheet Date">
                            <ItemTemplate>
                                <asp:Label ID="LoadSheet_Date" runat="server" Text='<%# Eval("LoadSheet_Date", "{0:dd MMM yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Load Sheet Qty(LTR)">
                            <ItemTemplate>
                                <asp:Label ID="LoadSheet_Qty" runat="server" Text='<%# Eval("Qty")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice No">
                            <ItemTemplate>
                                <asp:Label ID="Invoice_No" runat="server" Text='<%# Eval("Invoice_No")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="SOTYPE" DataField="SOTYPE">
                            <HeaderStyle Width="80px" HorizontalAlign="Left" />
                            <ItemStyle Width="80px" HorizontalAlign="Left" />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        No Record Found...
                    </EmptyDataTemplate>
                    <%-- <FooterStyle BackColor="#bfbfbf" />
             <HeaderStyle BackColor="#bfbfbf"  ForeColor="#000000" />
             <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
             <RowStyle ForeColor="#000066" />
             <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
             <SortedAscendingCellStyle BackColor="#F1F1F1" />
             <SortedAscendingHeaderStyle BackColor="#007DBB" />
             <SortedDescendingCellStyle BackColor="#CAC9C9" />
             <SortedDescendingHeaderStyle BackColor="#00547E" />--%>
                    <FooterStyle CssClass="table-condensed" BackColor="White" ForeColor="#4A3C8C" />
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

    <div style="overflow: auto; height: 10px; margin: 10px 0px 0px 10px;">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvLineDetails" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" EnableViewState="False" AllowPaging="True" PageSize="5" OnPageIndexChanging="gvLineDetails_PageIndexChanging" OnSelectedIndexChanged="gvLineDetails_SelectedIndexChanged" Width="99%">
                    <Columns>
                        <asp:TemplateField HeaderText="SO No" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="SO_No" runat="server" Text='<%#  Eval("SO_No") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SO Date">
                            <ItemTemplate>
                                <asp:Label ID="SO_Date" runat="server" Text='<%#  Eval("SO_Date") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Group">
                            <ItemTemplate>
                                <asp:Label ID="Customer_Group" runat="server" Text='<%#  Eval("Customer_Group") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Code - Name">
                            <ItemTemplate>
                                <asp:Label ID="Customer" runat="server" Text='<%#  Eval("Customer") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Group">
                            <ItemTemplate>
                                <asp:Label ID="Product_Group" runat="server" Text='<%#  Eval("Product_Group") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Code-Name">
                            <ItemTemplate>
                                <asp:Label ID="Product" runat="server" Text='<%#  Eval("Product") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qty (Box)">
                            <ItemTemplate>
                                <asp:Label ID="BOX" runat="server" Text='<%#  Eval("BOX") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Right" Width="80px" />
                            <ItemStyle HorizontalAlign="Right" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qty (Crates)">
                            <ItemTemplate>
                                <asp:Label ID="Crates" runat="server" Text='<%#  Eval("Crates") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Right" Width="80px" />
                            <ItemStyle HorizontalAlign="Right" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qty (Ltr)">
                            <ItemTemplate>
                                <asp:Label ID="Ltr" runat="server" Text='<%#  Eval("Ltr") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Right" Width="80px" />
                            <ItemStyle HorizontalAlign="Right" Width="80px" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        No Record Found...
                    </EmptyDataTemplate>
                    <%-- <FooterStyle BackColor="#bfbfbf" />
             <HeaderStyle BackColor="#bfbfbf"  ForeColor="#000000" />
             <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
             <RowStyle ForeColor="#000066" />
             <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
             <SortedAscendingCellStyle BackColor="#F1F1F1" />
             <SortedAscendingHeaderStyle BackColor="#007DBB" />
             <SortedDescendingCellStyle BackColor="#CAC9C9" />
             <SortedDescendingHeaderStyle BackColor="#00547E" />--%>

                    <FooterStyle CssClass="table-condensed" BackColor="White" ForeColor="#4A3C8C" />
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
</asp:Content>

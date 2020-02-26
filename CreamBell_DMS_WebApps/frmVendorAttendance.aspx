<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmVendorAttendance.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmVendorAttendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/style.css" rel="stylesheet" />
    <script type="text/javascript">

        $(document).ready(function () {
            /*Code to copy the gridview header with style*/
            var gridHeader = $('#<%=gvDetails.ClientID%>').clone(true);
            /*Code to remove first ror which is header row*/
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=gvDetails.ClientID%> tr th').each(function (i) {
                /* Here Set Width of each th from gridview to new table th */
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#controlHead2").append(gridHeader);
            $('#controlHead2').css('position', 'absolute');
            $('#controlHead2').css('top', '129');

        });
      

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
                        //inputList[i].disabled = true;
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
        fun
        
        


        function checkDate(sender, args) {
            debugger
            if (sender._selectedDate < new Date().add(-2) ) {
                alert("You cannot select a day earlier than today!");
                sender._selectedDate = new Date();
                // set the date back to the current date
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
        }

    </script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnShow" />
            <asp:PostBackTrigger ControlID="btnSave" />
            <asp:PostBackTrigger ControlID="btnEdit" />
        </Triggers>
        <ContentTemplate>
            <div style="width: 100%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
                Vendor Attendance
            </div>

            <div id="Filter" style="width: 98%; height: 40px; border-radius: 1px; margin: 5px 0px 0px 5px; color: black; padding: 2px 0px 0px 0px; border-style: groove">
                <asp:CalendarExtender ID="CalendarExtender1"  runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: center; width: 10%">Date :</td>
                        <td style="text-align: left; width: 20%">
                            <asp:TextBox ID="txtFromDate" runat="server" Enabled="true"  placeholder="dd-MMM-yyyy" Width="150px" required ></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" runat="server" ImageAlign="Bottom" ImageUrl="~/Images/calendar.jpg" />
                        </td>
                        <td style="width: 10%">
                            <asp:Button ID="btnShow" runat="server" CssClass="ReportSearch" Height="31px" OnClick="btnShow_Click" Text="Show" Width="96px" />
                        </td>
                        <td style="width: 10%">
                            <asp:Button ID="btnSave" runat="server" CssClass="ReportSearch" Height="31px" Text="Save" OnClick="btnSaveNew_Click" Width="96px" />
                        </td>
                        <td style="width: 10%">
                            <asp:Button ID="btnEdit" runat="server" CssClass="ReportSearch" Height="31px" Text="Edit" Visible="false" OnClick="btnEdit_Click" Width="96px" />
                        </td>
                        <td style="text-align: center; width: 40%">
                            <asp:Label ID="LblMessage" runat="server" Font-Bold="True" Font-Names="Seoge UI" Font-Size="Small" ForeColor="DarkRed"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        
             <div style="overflow: auto; margin-top: 5px;  height: 450px; width: 99%">
             <asp:GridView runat="server" ID="gvDetails" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="false" CssClass="GVFixedHeader" Width="100%" BackColor="White"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" OnRowDataBound="gvDetails_RowDataBound">
                    <AlternatingRowStyle BackColor="#CCFFCC" />
                    <Columns>
                        <asp:BoundField HeaderText="Distributor Code" DataField="DISTRIBUTOR_CODE">
                            <HeaderStyle Width="120px" HorizontalAlign="Left" />
                            <ItemStyle Width="120px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Distributor Name" DataField="DISTRIBUTOR_NAME">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField  HeaderText="Vendor Code" DataField="VENDOR_CODE">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                       <asp:BoundField HeaderText="Vendor Name" DataField="VENDOR_NAME">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Mobile No." DataField="MOBILE_NO">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" AutoPostBack="true" Text="Mark Present" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                   <asp:HiddenField ID="HiddenField1"  runat="server" Value='<%# Eval("Cust_Code") %>' />
                                <asp:CheckBox ID="chkStatus" runat="server" onclick="CheckLck(this)" Checked='<%#Convert.ToBoolean(Eval("Status")) %>' Enabled='<%#  (Convert.ToBoolean(Eval("Status")) == true ? false: true) %>' AutoPostBack="true" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Font-Bold="True" ForeColor="#000066" />
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
                 </ContentTemplate>
    </asp:UpdatePanel>
       </asp:Content>

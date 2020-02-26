<%@ Page Title="Collection Entry" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmNewCollectionEntry.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmNewCollectionEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <script type="text/javascript">        
    function checkBoxConfirmClick(elementRef) {
        if (elementRef.checked) {
            if (window.confirm('You have selected cash instrument for all invoices do you want to continue !!') == false)
                elementRef.checked = false;
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

    .overlayPopup {
        position: absolute;
        left: 0;
        right: 0;
        width: 100%;
        height: 100%;
        top: 0;
        background: rgba(0, 0, 0, 0.5);
        z-index: 999999;
    }

    .popupHere {
        position: absolute;
        left: 50%;
        top: 50%;
        transform: translate(-50%, -50%);
        background: #f2f2f2;
        text-align: center;
        padding: 50px 40px 30px 40px;
        height: 110px;
    }

    .popupHere h3{
        font-size:16px;
        margin:0;
    }

    .popupHere button {
        background: blue;
        padding: 8px 26px 8px 26px;
        color: #fff;
        font-size: 16px;
        border: #fff;
        margin: 20px;
    }

    </style>

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
            $("#controlHead1").append(gridHeader);
            $('#controlHead1').css('position', 'absolute');
            $('#controlHead1').css('top', '129');
        });

        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        function IsNumeric(e) {
            //debugger;
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
            //document.getElementById("lblError") = ret ? "none" : "inline";
            if (keyCode == 8 || keyCode == 46 || keyCode == 189 || keyCode == 61 || keyCode == 45)
            { ret = true; }
            return ret;
        }

        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to save data?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <script type="text/javascript">

        function checkDate(sender, args) {
            if (sender._selectedDate >= new Date()) {
                alert("You cannot select a day after than today!");
                sender._selectedDate = new Date();
                // set the date back to the current date
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
        }
    </script>

    <script type="text/javascript">

        function imgBtnSave_Click(btn) {
            var answer = confirm('Are you sure you want to Save?');
            if (answer) {
                btn.style.display = "none";
                return true;
            }
            else
                return false;
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".onclickBtn").click(function () {
                $(this).toggleClass('Active');
                $(".forToggle").toggleClass('active');
            });            
        });
    </script>
   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    
    <%--<div class="overlayPopup">
        <div class="popupHere">
            <h3>You have selected cash instrument for all invoices<br>Do you want to continue</h3>
            <button title="Close" onclick="closemsgbox()">Yes</button>
            <button title="Close" onclick="closemsgbox()">No</button>
        </div>
     </div>--%>

    <div style="width: 99%; height: 18px; border-radius: 4px; margin: 0px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px;">
        <b> New Collection Entry</b>
    </div>
    <div>
        <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 10px;">
            <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSave" />
                    <asp:PostBackTrigger ControlID="tnShow" />
                    <asp:PostBackTrigger ControlID="gvDetails" />
                    <asp:PostBackTrigger ControlID="rdPenInv" />
                    <asp:PostBackTrigger ControlID="rdWitInv" />
                    <asp:PostBackTrigger ControlID="rdActCust" />
                    <asp:PostBackTrigger ControlID="rdAllCust" />
                    <asp:PostBackTrigger ControlID="GridView1" />
                    <asp:PostBackTrigger ControlID="chkPenAmt" />
                </Triggers>
                <ContentTemplate>
                    <div class="MainTableClass">
                        <div class="TableClass">
                        <div class="mainSection">
                             <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Names="Seoge UI" CssClass="labelAlert"></asp:Label>
                            <div class="firstSection">
                                <span>Customer Group</span>
                                <asp:DropDownList ID="drpCustomerGroup" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpCustomerGroup_SelectedIndexChanged" Style="width: 215px" Height="30px"></asp:DropDownList>
                            </div>
                                                    
                            <div class="secondSection">
                                <span>Collection Date</span>
                                <asp:TextBox ID="txtCollectionDate" runat="server" Width="150" Height="25px" style="margin-left:20px; padding-left:5px;"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtCollectionDate" Format="dd/MMM/yyyy" OnClientDateSelectionChanged="checkDate"></asp:CalendarExtender>
                            </div>
                            <div class="CstmButton">                                
                                <asp:Button ID="tnShow" runat="server" Text="Show" CssClass="button" Height="31px" OnClick="tnShow_Click" Width="70" AutoPostBack="True" />                               
                            </div>                                            
                        </div>
                        <div class="MainSecondSection">                           
                            <div class="ThirdSection">
                                 <span>Customer</span>
                                <asp:DropDownList ID="drpCustomerName" runat="server" AutoPostBack="True" Style="width: 215px" Height="30px"></asp:DropDownList>
                            </div>
                             <div class="RadioButton">
                                <asp:RadioButton ID="rdActCust" runat="server" AutoPostBack="true" OnCheckedChanged="rdActCust_CheckedChanged" Text="Active Customer&nbsp;&nbsp;"  Checked="true"  GroupName="RdCustomers" />
                                <asp:RadioButton ID="rdAllCust" runat="server" AutoPostBack="true" OnCheckedChanged="rdActCust_CheckedChanged" Text="All Customer&nbsp;&nbsp;"  GroupName="RdCustomers" />
                               
                                <div class="InnerRadio">
                                    <asp:RadioButton ID="rdPenInv" runat="server" AutoPostBack = "true" Text="Pending Invoice&nbsp;&nbsp;" OnCheckedChanged="rdPenInv_CheckedChanged" ToolTip="PendingCustomers" Checked="true" ValidationGroup="Customers" GroupName="RdCustomer" />
                                    <asp:RadioButton ID="rdWitInv" runat="server" AutoPostBack = "true" Text="Without Invoice&nbsp;&nbsp;" OnCheckedChanged="rdPenInv_CheckedChanged" CheToolTip="WithoutCustomers" ValidationGroup="Customers" GroupName="RdCustomer" style="margin-left:4px;" />
                                </div>

                             </div>
                            <div class="lastButton"><asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button forBtnMargin" Height="31px" OnClick="btnSave_Click" Width="70" OnClientClick="javascript:return imgBtnSave_Click(this);" /></div>
                            <div class="lastButtonCheckBox"><label><asp:CheckBox ID="chkPenAmt" runat="server" AutoPostBack="true" onclick="checkBoxConfirmClick(this);" OnCheckedChanged="chkPenAmt_CheckedChanged" />Auto Fill Amount</label></div>
                             </div>
                        </div>

                <div class="GridSection">
                    <asp:GridView ID="GridView1" BorderColor="#E7E7FF" BorderStyle="None" CellPadding="3" GridLines="Horizontal" 
                        BorderWidth="1px" BackColor="White" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" ShowFooter="true" runat="server" 
                        Style="width: 100%" Font-Size="Small">
                        <AlternatingRowStyle BackColor="#ccffcc" />
                        <Columns>
                            <asp:BoundField HeaderText="Cash" DataField="CASH">
                                <HeaderStyle CssClass="cashtd" />
                                <ItemStyle CssClass="cashreportbox" HorizontalAlign="Right" />
                            </asp:BoundField>                           
                            
                            <asp:BoundField HeaderText="Cheque" DataField="CHEQUE">
                                <HeaderStyle CssClass="cashtd" />
                                <ItemStyle CssClass="cashreportbox" HorizontalAlign="Right" />
                            </asp:BoundField>

                            <asp:BoundField DataField="RTGS" HeaderText="RTGS">
                                <HeaderStyle CssClass="cashtd"  />
                                <ItemStyle HorizontalAlign="Right" CssClass="cashreportbox" />
                            </asp:BoundField>
                           
                            <asp:BoundField DataField="NEFT" HeaderText="NEFT">
                                <HeaderStyle CssClass="cashtd" />
                                <ItemStyle HorizontalAlign="Right" CssClass="cashreportbox2" />
                            </asp:BoundField>                                                                                                                                               
                        </Columns>
                        <%--<EmptyDataTemplate>
                                No Record Found...
                        </EmptyDataTemplate>--%>
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>


    <div style="overflow: auto; margin: -15px 0px 0px 10px; width: 99%">
        <%--<span class="onclickBtn">+</span>--%>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div  class="forAbsolute">
                    
                <div style=" width: 99%; margin-top: 20px; margin-left:10px" class="forToggle">
                    <asp:GridView ID="gridCollectionData" BorderColor="#E7E7FF" BorderStyle="None" CellPadding="3" GridLines="Horizontal" 
                        BorderWidth="1px" BackColor="White" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" ShowFooter="true" CssClass="onclickBtn" runat="server" 
                        Style="width: 100%" Font-Size="Small">
                        <AlternatingRowStyle BackColor="#ccffcc" />
                        <Columns>
                            <asp:BoundField HeaderText="Document No" DataField="Document_No">
                                <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                            </asp:BoundField>                           
                            
                            <asp:BoundField HeaderText="Customer Code - Name" DataField="Customer">
                                <HeaderStyle Width="200px" HorizontalAlign="Center" />
                                <ItemStyle Width="200px" HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Instrument_Code" HeaderText="Instrument">
                                <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                            </asp:BoundField>
                           
                            <asp:BoundField DataField="Instrument_No" HeaderText="Instrument No">
                                <HeaderStyle HorizontalAlign="Center" Width="150px" />
                                <ItemStyle HorizontalAlign="Center" Width="150px" />
                            </asp:BoundField>
                            
                             <asp:BoundField DataField="Ref_DocumentNo" HeaderText="Ref.Doc.No.">
                                <HeaderStyle HorizontalAlign="Center" Width="150px" />
                                <ItemStyle HorizontalAlign="Center" Width="150px" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Ref_DocumentDate" HeaderText="RefDoc.Date">
                                <HeaderStyle HorizontalAlign="Center" Width="150px" />
                                <ItemStyle HorizontalAlign="Center" Width="150px" />
                            </asp:BoundField>
                           
                            <asp:BoundField HeaderText="Collection Amount" DataField="Collection_Amount">
                                <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField HeaderText="Collection Date" DataField="Collection_Date">
                                <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                            </asp:BoundField>    
                            
                            <asp:BoundField HeaderText="Remark" DataField="Remark">
                                <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                <ItemStyle Width="150px" HorizontalAlign="Center" />
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
             <div style="overflow: auto; height: 365px; margin: 10px 0px 0px 10px; width: 99%">
                <asp:GridView ID="gvDetails" runat="server" ShowFooter="false" GridLines="Horizontal" AutoGenerateColumns="False" BackColor="White" Width="99%" AllowPaging="true"
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="2" ShowHeaderWhenEmpty="True" OnPageIndexChanging="OnPaging" OnRowDataBound="gvDetails_RowDataBound" PageSize="25">
                    <AlternatingRowStyle BackColor="#ccffcc" />
                    <Columns>                        

                        <asp:TemplateField Visible="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenField2" Visible="false" runat="server" Value='<%# Eval("CUSTOMER_CODE") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="0px"></ItemStyle>
                        </asp:TemplateField>
                        
                        <asp:BoundField HeaderText="Customer Code - Name" DataField="Customer">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Address" HeaderText="Address">
                            <HeaderStyle HorizontalAlign="Left" Width="200px" />
                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="Instrument">
                            <ItemTemplate>
                                <asp:DropDownList ID="drpInstrument" Width="100px" CssClass="dropdown" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="InstrumentNo">
                            <ItemTemplate>
                                <asp:TextBox ID="txtInstrument" Width="100px" runat="server" CssClass="textboxStyleNew" MaxLength="25" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="RefDoc.No">
                            <ItemTemplate>
                                <asp:Label ID="lblRefDocNo" Width="100px" runat="server" Text='<%# Bind("Document_No") %>' AutoPostBack="true"  />
                                <%--<asp:DropDownList ID="drpRefDocument" Width="120px" CssClass="dropdown" OnSelectedIndexChanged="drpRefDocument_SelectedIndexChanged" runat="server" AutoPostBack="true" />--%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="70px" />
                            <ItemStyle HorizontalAlign="Left" Width="70px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="RefDoc.Date">
                            <ItemTemplate>
                                <asp:Label ID="lblRefDocDate" Width="70px" DataFormatString="{0:dd-MMM-yyyy}" runat="server" Text='<%# Bind("Document_Date") %>' AutoPostBack="true" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="70px" />
                            <ItemStyle HorizontalAlign="Left" Width="70px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="PendingAmount">
                            <ItemTemplate>
                                <asp:Label ID="lblPendingAmount" Width="70px" DataFormatString="{0:n2}" runat="server" Text='<%# Bind("RemainingAmount") %>' AutoPostBack="true" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="70px" />
                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                        </asp:TemplateField>                        

                        <asp:TemplateField HeaderText="Collection Amount">
                            <ItemTemplate>                                
                                <asp:TextBox ID="txtAmount" Width="90px" runat="server" CssClass="textboxStyleNew" MaxLength="15" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="90px" />
                            <ItemStyle HorizontalAlign="Center" Width="90px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <asp:TextBox ID="txtRemark" Width="120px" runat="server" CssClass="textboxStyleNew" TextMode="MultiLine" MaxLength="500" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="120px" />
                            <ItemStyle HorizontalAlign="Center" Width="120px" />
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
    </div>
</asp:Content>

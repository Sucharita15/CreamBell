<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.Master" CodeBehind="frmServiceInvoiceReversal.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmServiceInvoiceReversal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
     <script type="text/javascript" >
        
        function CheckNumeric(e) {          //--Only For Numbers //

            if (window.event) // IE 
            {
                if ((e.keyCode < 46 || e.keyCode > 57) & e.keyCode != 8) {
                    event.returnValue = false;
                    return false;

                }
            }
            else { 
                if ((e.which < 46 || e.which > 57) & e.which != 8) {
                    e.preventDefault();
                    return false;

                }
            }
        }
     </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
     <div style="width: 100%; text-align:center; height: 18px; border-radius: 4px; margin: 10px 0px 0px 0px; background-color: #0085ca; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px; color: #FFFFFF;">Service Invoice Reversal</span>
    </div>

  <div class="MainContainer">
       <div class="FirstColumn"> 
            <div class="FirstInnerClm">
                <label>Service Invoice Number</label>
                <asp:DropDownList CssClass="InnerInput" ID="drpInvNo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpGetInvoiceData" ></asp:DropDownList>
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ReportBTN" OnClick="btnSave_Click" /> 
            </div>
       </div>


         <div class="FirstColumn">
            <div class="SecondInnerClm">
                 <label>Service Invoice Date</label>
                <asp:TextBox ID="txtInvoiceDate" runat="server" Width="90%" ReadOnly="true"></asp:TextBox>
            </div>
           
            <div class="SecondInnerClm">
                 <label>Service Invoice Return Date</label>
                <asp:TextBox ID="txtInvoiceReturnDate"  runat="server" readonly="true" Width="90%"></asp:TextBox>
            </div>

          <div class="FourthCLM ForLabel_Margin">
            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Checked="false" Text="Complete Reversal" OnCheckedChanged="onCompleteReversal" />
            <asp:Label ID="lblsite" runat="server" Visible="false"></asp:Label>
        </div>
        </div>
        
    <div class="FirstColumn">
        <div class="ThirdInnerClm">
             <label>Balance Amount</label>
            <asp:TextBox ID="txtBalanceAmount" runat="server" Width="90%" ReadOnly="true"></asp:TextBox>
        </div>
         <div class="FourthCLM" style="margin-left:28px; margin-right:25px;">
           <label>Address</label>
        </div>
        <div class="LastCLM forMargin">
            
            <asp:TextBox ID="txtAddress" runat="server" Width="90%" ReadOnly="true"></asp:TextBox>
        </div>

        <div class="LastCLM">
            <label>State</label>
            <asp:TextBox ID="txtState" runat="server" Width="90%" ReadOnly="true"></asp:TextBox>

        </div>
       
      
    </div>
      <div class="SecondInnerClm2">
                <label>Remarks</label>
            <asp:TextBox ID="txtRemarks" runat="server" readonly="false" onkeypress="return this.value.length<=200" TextMode="MultiLine" Rows="2" Width="90%"></asp:TextBox>
        </div>
    </div>
    <div style='overflow-x:scroll;width:1330px; margin: 0px auto;'>

    <%--<div id="controlHead" style="margin-top:5px; margin-left:5px;padding-right:10px;width: 100%;"></div>--%>
    <div style="overflow: auto; margin-top: 0px; width: 100%;">

    </div>
         <asp:GridView runat="server" ID="gvDetails" ShowFooter="True" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
            BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" >
            <AlternatingRowStyle BackColor="#CCFFCC" />
            <Columns>

              
                <%--0--%>
                <asp:TemplateField HeaderText="SITEID" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="SITEID" runat="server" Text='<%#  Eval("SITEID") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="90px" />
                    <ItemStyle HorizontalAlign="Left" Width="90px" />
                </asp:TemplateField>
                <%--1--%>
                <asp:TemplateField HeaderText="Company Code" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="COMPANYCODE" runat="server" Text='<%#  Eval("COMPANYCODE") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                </asp:TemplateField>
                  <%--2--%>
                <asp:TemplateField  HeaderText="Customer Code">
                    <ItemTemplate>
                        <asp:HiddenField ID="Product_Code" runat="server" Value='<%# Eval("CUSTOMERCODE") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="0px"></ItemStyle>
                </asp:TemplateField>
                <%--3--%>
                <asp:TemplateField HeaderText="Item Id">
                    <ItemTemplate>
                        <asp:Label ID="ITEMID" runat="server" Text='<%#  Eval("ITEMID") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                </asp:TemplateField>
                <%--4--%>
                <asp:TemplateField HeaderText="Item Name">
                    <ItemTemplate>
                        <asp:Label ID="ITEMNAME" runat="server" Text='<%#  Eval("ITEMNAME") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="center" Width="100px" />
                    <ItemStyle HorizontalAlign="center" Width="100px" />
                </asp:TemplateField>
                <%--5--%>
                <asp:TemplateField HeaderText="Line Amount">
                    <ItemTemplate>
                        <asp:TextBox ID="Rate" Width="130px" runat="server"  AutoPostBack="true" Text='<%#  Eval("RATE","{0:n2}") %>' OnTextChanged="onRateChange" onkeypress="CheckNumeric(event)" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                </asp:TemplateField>
                <%--6--%>
                  <asp:TemplateField HeaderText="Tax1 %">
                    <ItemTemplate>
                        <asp:Label ID="Tax1" Width="60px" runat="server" Text='<%#  Eval("TAX1") %>' />                        
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                </asp:TemplateField>

                <%--7--%> 
                <asp:TemplateField HeaderText="Tax1 AMT">
                    <ItemTemplate>
                        <asp:Label ID="Tax1_AMT" runat="server" Text='<%#  Eval("TAX1AMT") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="center" Width="74px" />
                    <ItemStyle HorizontalAlign="center" Width="74px" />
                </asp:TemplateField>
                <%--8--%>
                <asp:TemplateField HeaderText="TAX1 Component">
                    <ItemTemplate>
                        <asp:Label ID="TAX1_Component" Width="120px" runat="server" Text='<%#  Eval("TAX1COMPONENT") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="center" Width="120px" />
                    <ItemStyle HorizontalAlign="center" Width="120px" />
                </asp:TemplateField>
                
                <%--9--%>
                  <asp:TemplateField HeaderText="Tax2 %">
                    <ItemTemplate>
                        <asp:Label ID="TAX2" Width="60px" runat="server" Text='<%#  Eval("TAX2 ") %>' />                        
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                </asp:TemplateField>
                 <%--10--%>
                <asp:TemplateField HeaderText="Tax2 AMT">
                    <ItemTemplate>
                        <asp:Label ID="Tax2_AMT" runat="server" Text='<%#  Eval("TAX2AMT") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="center" Width="60px" />
                    <ItemStyle HorizontalAlign="center" Width="60px" />
                </asp:TemplateField>
                <%--11--%>
                <asp:TemplateField HeaderText="TAX2 Component">
                    <ItemTemplate>
                        <asp:Label ID="TAX2_Component" Width="110px" runat="server" Text='<%#  Eval("TAX2COMPONENT") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="110px" />
                    <ItemStyle HorizontalAlign="Right" Width="110px" />
                </asp:TemplateField>
                <%--12--%>
                <asp:TemplateField HeaderText="Amount">
                    <ItemTemplate>
                        <asp:Label ID="Amount" runat="server" Text='<%#  Eval("AMOUNT") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="center" Width="80px" />
                    <ItemStyle HorizontalAlign="center" Width="80px" />
                </asp:TemplateField>
                <%--13--%>
                <asp:TemplateField HeaderText="Balance Amount" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="Balance_Amount" runat="server" Text='<%#  Eval("BALANCEAMOUNT") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" Width="92px" />
                    <ItemStyle HorizontalAlign="Right" Width="92px" />
                </asp:TemplateField>

                
            </Columns>
            <EmptyDataTemplate>
                No Record Found...
            </EmptyDataTemplate>
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
    </div>

    </asp:Content>
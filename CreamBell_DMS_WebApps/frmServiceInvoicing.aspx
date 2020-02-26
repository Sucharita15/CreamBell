<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmServiceInvoicing.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmServiceInvoicing" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ccp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
       function checkDec() {
           var val = document.getElementById("<%=txtAmount.ClientID%>").value;
           var regex = /^([0-9]+[\.]?[0-9]?[0-9]?|[0-9]+)$/g;
           if (regex.test(val)) {

            } else if (val != "") {
                alert("Enter Value is not in Correct Format or Amount will accept only numbers upto two decimal places.");
                document.getElementById("<%=txtAmount.ClientID%>").value = "";
            }
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnSavePrint" />--%>
        </Triggers>
        <ContentTemplate>
            <div style="width: 100%; height: 20px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 0px 0px 0px 0px; font-weight: bold">
                <table>
                    <tr>
                        <td runat="server" style=" text-align:center" id="tclabel" >
                            Service Invoicing
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Panel ID="pnlHeader" runat="server"  GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 2px;" >
                <table style="width: 100%; height: 99px; margin:0px 0px 0px 0px">

                    <ccp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtMonth" Format="MMM-yyyy"></ccp:CalendarExtender>

                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return confirm('Are you sure to save record?');" OnClick="btnSave_Click" CssClass="ReportSearch" Height="31px" Width="68px" />
                            <asp:Button ID="btnSavePrint" runat="server" Text="Save and Print" OnClientClick="return confirm('Are you sure to save and print record?');" OnClick="btnSavePrint_Click" CssClass="ReportSearch" Height="31px" Width="100px" />
                            <asp:RadioButton ID="rdoSelf" runat="server" AutoPostBack="true" Text="Self" Checked="true"
                                            OnCheckedChanged="rdoSelf_CheckedChanged" GroupName="radio" Width="50px" />

                            <asp:RadioButton ID="rdoCustomer" runat="server" AutoPostBack="true" Text="Customer"
                                OnCheckedChanged="rdoSelf_CheckedChanged" GroupName="radio" Width="100px"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style5">Company:</td>
                        <td class="auto-style6 forFontSize">
                            <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="true" CssClass="textboxStyleNew" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" Font-Size="Smaller" Height="22px" Width="300px">
                            </asp:DropDownList>
                        </td>
                        <td>Month :</td>
                        <td>
                            <asp:TextBox ID="txtMonth" runat="server" AutoPostBack="true" placeholder="MMM-yyyy" Width="107px" OnTextChanged="txtMonth_TextChanged" CssClass="textboxStyleNew" Height="11px" ></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <td class="auto-style7" id="lblCustomer" runat="server" visible="false">Customer :</td>
                        <td class="auto-style6 forFontSize" id="drpCustomer" runat="server" visible="false">
                            <asp:DropDownList ID="ddlCustomers" runat="server" AutoPostBack="true" CssClass="textboxStyleNew" OnSelectedIndexChanged="ddlCustomers_SelectedIndexChanged" Font-Size="Smaller" Height="22px" Width="250px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>                       
                        <td class="auto-style5">Product :</td>
                        <td class="forFontSize">
                            <asp:DropDownList ID="ddlProduct" runat="server" CssClass="textboxStyleNew" AutoPostBack="true" Font-Size="Smaller" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged" Height="22px" Width="280px">
                            </asp:DropDownList>
                        </td> 
                        <td class="auto-style5">Amount :</td>
                        <td>
                            <asp:TextBox ID="txtAmount" runat="server" AutoPostBack="true" Autocomplete="off" placeholder="0.00" Width="190px" CssClass="textboxStyleNew" onkeyUp="checkDec();" OnTextChanged="txtAmount_TextChanged" MaxLength="8"/>
                        </td>      
                        <td class="auto-style5">TotTax :</td>
                        <td>
                            <asp:TextBox ID="txtTotTax" runat="server" Width="110px" CssClass="textboxStyleNew" ReadOnly="true"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style5">Final Amount :</td>
                        <td>
                            <asp:TextBox ID="txtFAmount" runat="server" Width="270px" CssClass="textboxStyleNew" ReadOnly="true"/>
                        </td>
                        <td>
                            <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" CssClass="ReportSearch" Height="31px" Width="68px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Medium" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
                    <div style='overflow-x: scroll; width: 100%;'>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server" GroupingText="Service Line Items" Width="100%" BackColor="White">
                                 <asp:GridView ID="gvDetails" CssClass="table" runat="server" GridLines="Horizontal" AutoGenerateColumns="true" Width="100%" BackColor="White"
                                   BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="false"
                                     ItemStyle HorizontalAlign ="Center" >
                                      <Columns>
                                       <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                             <asp:LinkButton ID="lkbtn" runat="server" Text="Delete" CommandName="Delete" CausesValidation="False" OnClick="lnkbtnDel_Click" ToolTip="Delete" />
                                        </ItemTemplate>
                                       </asp:TemplateField>
                                      </Columns>
                                   <EmptyDataTemplate>
                                       No Record Found...
                                   </EmptyDataTemplate>
                                   <FooterStyle CssClass="table-condensed" BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                   <HeaderStyle BackColor="#05345C" Font-Bold="True" ForeColor="#F7F7F7" />
                                   <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="center" VerticalAlign="Middle" />
                                   <RowStyle BackColor="White" ForeColor="#4A3C8C" HorizontalAlign="center" />
                                   <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                   <SortedAscendingCellStyle BackColor="#F4F4FD" />
                                   <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                                   <SortedDescendingCellStyle BackColor="#D8D8F0" />
                                   <SortedDescendingHeaderStyle BackColor="#3E3277" />
                                      
                                 </asp:GridView>

                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmNotificationMaster.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmNotificationMaster" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ccp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Javascript/DateValidation.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnClear" />--%>
        </Triggers>
        <ContentTemplate>
            <div style="width: 100%; height: 20px; border-radius: 4px; margin: 5px 0px 0px 10px; background-color: #1564ad; color: white; padding: 0px 0px 0px 0px; font-weight: bold">
                <table>
                    <tr>
                        <td runat="server" style=" text-align:center" id="tclabel" >
                            Notification Master
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Panel ID="pnlHeader" runat="server"  GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 2px;" >
                <table style="width: 100%; height: 99px; margin:0px 0px 0px 0px">

                    <ccp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></ccp:CalendarExtender>
                    <ccp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></ccp:CalendarExtender>

                    <tr>
                        <td class="auto-style5">State:</td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style5">User Type:</td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlUserType" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged" Width="120px">
                                <asp:ListItem Text="ALL" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Distributor" Value="1"></asp:ListItem>
                                <asp:ListItem Text="VRS" Value="2"></asp:ListItem>
                                <asp:ListItem Text="PSR" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style7">Users:</td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlUsers" runat="server" AutoPostBack="true" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px">
                            </asp:DropDownList>
                        </td>
                       <td>From Date :</td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px" onchange="ValidateDate(this)" required></asp:TextBox>
                            <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                        <td class="auto-style9">To Date :</td>
                        <td class="auto-style10">
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd-MMM-yyyy" Width="107px" CssClass="textboxStyleNew" Height="11px" onchange="ValidateDate(this)" required></asp:TextBox>
                            <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                        </td>
                    </tr>
                    <tr>         
                        <td class="auto-style5">Display On:</td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlDisplayOn" runat="server" AutoPostBack="True" CssClass="textboxStyleNew" Font-Size="Smaller" Height="22px" Width="120px">
                                <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                <%--<asp:ListItem Text="Both" Value="1"></asp:ListItem>--%>
                                <asp:ListItem Text="Portal" Value="2"></asp:ListItem>
                                <%--<asp:ListItem Text="Mobile" Value="3"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style5">Message:</td>
                        <td colspan="6">
                            <asp:TextBox ID="txtMessage" runat="server" Width="700px" CssClass="textboxStyleNew" TextMode="MultiLine"/>
                        </td>      
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="ReportSearch" Height="31px" Width="68px" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="ReportSearch" OnClick="btnClear_Click" UseSubmitBehavior="false" Height="31px" Width="68px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Medium" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="height: auto; overflow: auto; margin-top: 5px; margin-left: 9px; padding-right: 10px;">
                            <asp:GridView runat="server" ID="gvDetails" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" BackColor="White"
                                BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True"
                                AllowPaging="false" PageSize="20" ShowFooter="false">
                                <AlternatingRowStyle BackColor="#CCFFCC" />
                                <Columns>
                                    <asp:BoundField HeaderText="STATE" DataField="STATE">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="USERTYPE" DataField="USERTYPE">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="USER" DataField="USER">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="MESSAGE" DataField="MESSAGE">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="DISPLAYON" DataField="DISPLAYON">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="FROMDATE" DataField="FROMDATE">
                                        <HeaderStyle HorizontalAlign="Left" Width="0px" />
                                        <ItemStyle HorizontalAlign="Left" Width="0px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="TODATE" DataField="TODATE">
                                        <HeaderStyle HorizontalAlign="Left" Width="0px" />
                                        <ItemStyle HorizontalAlign="Left" Width="0px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnstatus" runat="server" Text='<%#Eval("STATUS") %>' OnClick="lnkbtnstatus_Click" OnClientClick="return confirm('Are you sure you want to change the status of this Notification?');"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hndrecid" Value='<%# Eval("RECID") %>' Visible="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
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

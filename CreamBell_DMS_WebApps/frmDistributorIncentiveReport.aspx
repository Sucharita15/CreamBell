<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmDistributorIncentiveReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmDistributorIncentiveReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/SSheet.css" rel="Stylesheet" type="text/css" />
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />
    <style type="text/css">
        table.header_Gridview tr th,
        table.header_Gridview tr td {
            width: 6.25% !important;
        }
        table.header_Gridview {
            width: auto !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdateProgress ID="upprogress" runat="server" AssociatedUpdatePanelID="sdsds" DisplayAfter="0">
        <ProgressTemplate>
            <div class="overlay"></div>
            <div class="overlayContent">
                <center>
                    Please Wait...don't close this window until processing is being done.
                     <br />
                    <img src="../../IMAGES/bar.gif" alt="" />
                </center>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div style="width: 99%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 5px; background-color: #0085ca; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px; color: #FFFFFF;">Auto Claim Calculation</span>
    </div>
            <div style="width: 100%">
                <asp:UpdatePanel ID="sdsds" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: left">&nbsp;
                            <asp:Button ID="btnSubmit" runat="server" Text="View" CssClass="button" OnClick="btnSubmit_Click" ValidationGroup="btnValidate" Height="25px" Width="72px" />
                                    &nbsp;
                            <asp:Button ID="btnCalculate" Enabled="true" runat="server" CssClass="button" Height="25px" OnClick="btnCalculate_Click" Text="Calculate" Width="82px" />
                                    &nbsp;
                            <asp:Button ID="btnsave" Visible="false" runat="server" CssClass="button" Height="25px" OnClick="btnsave_Click" Text="Save" Width="71px" />
                                </td>
                                <td style="text-align: left;" class="auto-style1">
                                    <asp:ImageButton ID="imgBtnExportToExcel" runat="server" ImageUrl="~/Images/excel-24.ico" ToolTip="Click To Generate Excel Report" OnClick="imgBtnExportToExcel_Click" />
                                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="imgBtnExportToExcel" />
                        <asp:PostBackTrigger ControlID="btnSubmit" />
                        <asp:PostBackTrigger ControlID="btnCalculate" />
                        <asp:PostBackTrigger ControlID="btnsave" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div style="width: 100%; text-align: left">
                <asp:UpdatePanel ID="upselection" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="width: 100%; text-align: left">
                            <tr>
                                <td>Select Target Type</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="drptargetType" runat="server" CssClass="textboxStyleNew" AutoPostBack="true" Font-Size="Smaller" OnSelectedIndexChanged="drptargetType_SelectedIndexChanged" Height="21px" Width="150px">
                                        <asp:ListItem Text="Sale" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Frieght" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Handling" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Electricity" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                                <td>From Date</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" OnTextChanged="txtFromDate_TextChanged" AutoPostBack="true" runat="server" Width="140px" CssClass="textboxStyleNew" Height="12px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFromDate" Display="Dynamic" ErrorMessage="Select Date.." ForeColor="#FF3300" ValidationGroup="btnValidate"></asp:RequiredFieldValidator>
                                </td>
                                <td>To Date</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txttoDate" runat="server" AutoPostBack="true" OnTextChanged="txttoDate_TextChanged" Width="140px" CssClass="textboxStyleNew" Height="12px"></asp:TextBox>
                                </td>

                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txttoDate" Display="Dynamic" ErrorMessage="Select Date.." ForeColor="#FF3300" ValidationGroup="btnValidate"></asp:RequiredFieldValidator>
                                </td>
                                <td>Business Unit</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="ddlBusinessUnit" AutoPostBack="true" runat="server" Height="21px" Width="150px" Font-Size="Smaller" CssClass="textboxStyleNew">
                                    </asp:DropDownList>
                                </td>

                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="txtFromDate" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="txttoDate" TargetControlID="txttoDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                            </tr>
                            <tr runat="server" id="trCategory" visible="true">

                                <td>Claim Category</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="ddlClaimCategory" AutoPostBack="true" OnSelectedIndexChanged="ddlClaimCategory_SelectedIndexChanged" runat="server" Height="21px" Width="150px" Font-Size="Smaller" CssClass="textboxStyleNew">
                                    </asp:DropDownList>
                                </td>
                                <td>&nbsp;</td>
                                <td>Claim Sub Category</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="ddlClaimSubCategory" AutoPostBack="true" OnSelectedIndexChanged="ddlClaimSubCategory_SelectedIndexChanged" runat="server" Height="21px" Width="150px" Font-Size="Smaller" CssClass="textboxStyleNew">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                                <td>Object Type</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="ddlobject" AutoPostBack="true" OnSelectedIndexChanged="ddlobject_SelectedIndexChanged" runat="server" Height="21px" Width="150px" Font-Size="Smaller" CssClass="textboxStyleNew">
                                        <asp:ListItem Text="Select" Value="-1" Selected="True"></asp:ListItem>

                                        <asp:ListItem Text="Site" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Customer Group" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                                <td>Select Object</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="ddlobjectname" OnSelectedIndexChanged="ddlobjectname_SelectedIndexChanged" AutoPostBack="true" runat="server" Height="21px" Width="150px" Font-Size="Smaller" CssClass="textboxStyleNew">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div style="overflow: auto; height: 410px">
                <asp:UpdatePanel ID="up" runat="server">
                    <ContentTemplate>
                        <div style="overflow: auto; height: 400px; margin: 10px 0px 0px 10px;">
                            <asp:GridView ID="grvDetail" CssClass="header_Gridview" runat="server" AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3"
                                ShowHeaderWhenEmpty="True" EnableViewState="true">
                                <AlternatingRowStyle BackColor="#CCFFCC" />
                                <Columns>
                                    <asp:BoundField HeaderText="Site Code" DataField="siteid"></asp:BoundField>
                                    <asp:BoundField HeaderText="Target Code" DataField="TARGETCODE"></asp:BoundField>
                                    <asp:BoundField HeaderText="Target Name" DataField="DESCRIPTION"></asp:BoundField>
                                    <asp:BoundField HeaderText="RECID" DataField="RECID"></asp:BoundField>
                                    <asp:BoundField HeaderText="Date From" DataField="From_Date" DataFormatString="{0:dd/MMM/yyyy}"></asp:BoundField>
                                    <asp:BoundField HeaderText="Date To" DataField="To_Date" DataFormatString="{0:dd/MMM/yyyy}"></asp:BoundField>
                                    <asp:BoundField HeaderText="Object" DataField="Object"></asp:BoundField>
                                    <asp:BoundField HeaderText="objectcode" DataField="objectcode"></asp:BoundField>
                                    <asp:BoundField HeaderText="Object Name" DataField="ObjectName"></asp:BoundField>
                                    <asp:BoundField HeaderText="targetsubobject" DataField="targetsubobject"></asp:BoundField>
                                    <asp:BoundField HeaderText="Sub Object Name" DataField="Sub_Object_Name"></asp:BoundField>
                                    <asp:BoundField HeaderText="Calculation" DataField="Pattern"></asp:BoundField>
                                    <asp:BoundField HeaderText="Cal_On" DataField="Cal_On"></asp:BoundField>
                                    <asp:BoundField HeaderText="Target Group" DataField="productgroup"></asp:BoundField>
                                    <asp:BoundField HeaderText="targetcategory" DataField="targetcategory"></asp:BoundField>
                                    <asp:BoundField HeaderText="Claim Category" DataField="Target_Cat"></asp:BoundField>
                                    <asp:BoundField HeaderText="targetsubcategory" DataField="targetsubcategory"></asp:BoundField>
                                    <asp:BoundField HeaderText="Claim Sub Category" DataField="Target_Sub_Cat"></asp:BoundField>
                                    <asp:BoundField HeaderText="Target" DataField="Target" DataFormatString="{0:n}">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Target Incentive" DataField="Incentive" DataFormatString="{0:n}">
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Achivement" DataFormatString="{0:n}" HeaderText="Achivement">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Calculate Incentive">
                                          <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Status" />
                                    <asp:BoundField HeaderText="Incentive Base" DataField="incentivebase" />
                                    <asp:BoundField HeaderText="AutoCaluculate" DataField="Actual_Incentive" DataFormatString="{0:n}">
                                        <ItemStyle Width="0px" />
                                        <HeaderStyle Width="0px" />
                                    </asp:BoundField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Record Found...
                                </EmptyDataTemplate>
                                <FooterStyle CssClass="table-condensed" BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                <HeaderStyle BackColor="#05345C" Font-Bold="True" ForeColor="#F7F7F7" CssClass="headerstyle" />
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
                    <Triggers>
                        <%--<asp:PostBackTrigger ControlID="btnSubmit" />
                        <asp:PostBackTrigger ControlID="btnCalculate" />
                        <asp:PostBackTrigger ControlID="btnsave" />--%>
                    </Triggers>
                </asp:UpdatePanel>
            </div>

    <script src="jquery/ScrollableGridViewPlugin_ASP.NetAJAXmin.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=grvDetail.ClientID %>').Scrollable({
                ScrollHeight: 300,
                IsInUpdatePanel: true
            });
        });
    </script>

</asp:Content>

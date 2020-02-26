<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmPurchaseAutoClaim.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPurchaseAutoClaim" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/SSheet.css" rel="Stylesheet" type="text/css" />
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />
    <style type="text/css">
        .button {
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdateProgress ID="upprogress" runat="server" AssociatedUpdatePanelID="upGetdata" DisplayAfter="0">
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
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="up" DisplayAfter="0">
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
    <div style="width: 99%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #0085ca; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px; color: #FFFFFF;">Auto Claim Calculation</span>
    </div>

    <div style="width: 99%">
        <asp:UpdatePanel ID="upGetdata" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <table style="width: 99%">
                    <tr>
                        <td>
                            <asp:Button ID="btnSubmit" runat="server" Text="View" CssClass="button" OnClick="btnSubmit_Click" ValidationGroup="btnValidate" Height="25px" Width="64px" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnCalculate" Enabled="true" runat="server" CssClass="button" Height="25px" OnClick="btnCalculate_Click" Text="Calculate" Width="80px" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnsave" Visible="false" runat="server" CssClass="button" Height="25px" OnClick="btnsave_Click" Text="Save" Width="68px" />
                            &nbsp;&nbsp;</td>
                        <td style="width: 69%">
                            <asp:ImageButton ID="imgBtnExportToExcel" runat="server" ImageUrl="~/Images/excel-24.ico" ToolTip="Click To Generate Excel Report" OnClick="imgBtnExportToExcel_Click" Style="width: 24px" />
                            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="imgBtnExportToExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div style="width: 100%; text-align: left">
        <asp:UpdatePanel ID="upselection" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table style="width: 100%; text-align: left">
                    <tr>
                        <td>Select Target Type :</td>
                        <td>
                            <asp:DropDownList ID="drptargetType" runat="server" CssClass="textboxStyleNew" AutoPostBack="true" Font-Size="Smaller" OnSelectedIndexChanged="drptargetType_SelectedIndexChanged" Height="21px" Width="150px">
                                <asp:ListItem Text="Purchase" Value="1" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>From Date :</td>
                        <td>
                            <asp:TextBox ID="txtFromDate" OnTextChanged="txtFromDate_TextChanged" AutoPostBack="true" runat="server" Width="140px" Font-Size="Smaller" CssClass="textboxStyleNew"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFromDate" Display="Dynamic" ErrorMessage="Select Date.." ForeColor="#FF3300" ValidationGroup="btnValidate"></asp:RequiredFieldValidator>

                        </td>
                        <td>To Date :</td>
                        <td>
                            <asp:TextBox ID="txttoDate" runat="server" AutoPostBack="true" OnTextChanged="txttoDate_TextChanged" Width="140px" Font-Size="Smaller" CssClass="textboxStyleNew"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txttoDate" Display="Dynamic" ErrorMessage="Select Date.." ForeColor="#FF3300" ValidationGroup="btnValidate"></asp:RequiredFieldValidator>
                        </td>
                        <td>Business Unit :</td>
                        <td>
                         <asp:DropDownList ID="ddlBusinessUnit" AutoPostBack="true" runat="server" Height="21px" Width="150px" Font-Size="Smaller" CssClass="textboxStyleNew" OnSelectedIndexChanged="ddlBusinessUnit_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="txtFromDate" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="txttoDate" TargetControlID="txttoDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                    </tr>
                    <tr>
                        <td>Claim Category:</td>
                        <td>
                            <asp:DropDownList ID="ddlClaimCategory" AutoPostBack="true" OnSelectedIndexChanged="ddlClaimCategory_SelectedIndexChanged" runat="server" Height="21px" Width="150px" Font-Size="Smaller" CssClass="textboxStyleNew">
                            </asp:DropDownList>
                        </td>
                        <td>Claim Sub Category :</td>
                        <td>
                            <asp:DropDownList ID="ddlClaimSubCategory" AutoPostBack="true" OnSelectedIndexChanged="ddlClaimSubCategory_SelectedIndexChanged" runat="server" Height="21px" Width="150px" Font-Size="Smaller" CssClass="textboxStyleNew">
                            </asp:DropDownList>
                        </td>
                        <td>Object Type :</td>
                        <td>
                            <asp:DropDownList ID="ddlobject" AutoPostBack="true" OnSelectedIndexChanged="ddlobject_SelectedIndexChanged" runat="server" Height="21px" Width="150px" Font-Size="Smaller" CssClass="textboxStyleNew">
                                <asp:ListItem Text="Select" Value="-1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="PSR" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Site" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Customer Group" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>Select Object :</td>
                        <td>
                            <asp:DropDownList ID="ddlobjectname" OnSelectedIndexChanged="ddlobjectname_SelectedIndexChanged" AutoPostBack="true" runat="server" Height="21px" Width="150px" Font-Size="Smaller" CssClass="textboxStyleNew">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div style="overflow: auto; height: 400px;">

        <asp:UpdatePanel ID="up" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div style="overflow: auto; height: 390px; margin: 10px 0px 0px 10px; text-align: left">
                    <asp:GridView ID="grvDetail" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3"
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
                                <%-- <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />--%>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Target Incentive" DataField="Incentive" DataFormatString="{0:n}">
                                <%--<HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Achivement" DataFormatString="{0:n}" HeaderText="Achivement" />
                            <asp:BoundField HeaderText="Calculate Incentive" />
                            <asp:BoundField HeaderText="Status" />
                            <asp:BoundField DataField="PURCHASE_VALUE" DataFormatString="{0:n}" HeaderText="Purchase Value" />
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
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSubmit" />
                <asp:PostBackTrigger ControlID="btnCalculate" />
                <asp:PostBackTrigger ControlID="btnsave" />

            </Triggers>
        </asp:UpdatePanel>

    </div>
</asp:Content>

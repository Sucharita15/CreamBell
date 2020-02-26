<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmManualClaim.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmManualClaim" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        var check = false;

        function RejectedItem() {
            var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
            //debugger;
            if (document.getElementById('txtEmailId').value.trim() != "") {
                if (reg.test(document.getElementById('txtEmailId').value.trim()) == false) {
                    alert('Invalid Email Address');
                    check = false;
                    return false;
                }
            }

            var select = document.getElementById('ddlCriateria');
            if (select.options[select.selectedIndex].value == "0" || select.options[select.selectedIndex].value == "1" || select.options[select.selectedIndex].value == "-1") {

                var select = document.getElementById('ddlCriateria');
                if (select.options[select.selectedIndex].value == "-1") {
                    alert('Please select Incident Type!');
                    check = false;
                    return false;
                }
                else { check = true; }

                var select = document.getElementById('ddlIncidentSubCat');
                if (select.options[select.selectedIndex].value == "--Select--") {
                    alert('Please select Incident_Sub_Category !');
                    check = false;
                    return false;
                }
                else { check = true; }


                var select = document.getElementById('ddlSubCategory');
                if (select.options[select.selectedIndex].value == "--Select--") {
                    alert('Please select Product!');
                    check = false;
                    return false;
                }
                else { check = true; }

                var select = document.getElementById('ddlProduct');
                if (select.options[select.selectedIndex].value == "--Select--") {
                    alert('Please select Pack!');
                    check = false;
                    return false;
                }
                else { check = true; }

                var select = document.getElementById('ddlCriateria');
                if (select.options[select.selectedIndex].value == "0") {
                    var pgng = document.getElementById('txtBatchNo').value.trim();
                    if (pgng == "" || pgng == "Enter Batch No") {
                        alert('In product complaint \'Batch no\' should not be empty...');
                        document.getElementById('txtBatchNo').focus();
                        check = false;
                        return false;
                    }
                    else { check = true; }
                }
                var select = document.getElementById('ddlCriateria');
                if (select.options[select.selectedIndex].value == "1") {
                    var pgng = document.getElementById('txtInvoiceNo').value.trim();
                    if (pgng == "" || pgng == "Enter Invoice No") {
                        alert('In service complaint \'Invoice\' no should not be empty...');
                        document.getElementById('txtInvoiceNo').focus();
                        check = false;
                        return false;
                    }
                    else { check = true; }
                }

            }
            if (select.options[select.selectedIndex].value == "2") {

                var select = document.getElementById('ddlIncidentSubCat');
                if (select.options[select.selectedIndex].value == "--Select--") {
                    alert('Please select Incident_Sub_Category !');
                    check = false;
                    return false;
                }
                else { check = true; }

            }

            var pgng = document.getElementById('txtEmailId').value.trim();
            if (pgng == "") {
                alert('Email-Id should not be empty...');
                document.getElementById('txtEmailId').focus();
                check = false;
                return false;
            }
            else { check = true; }
        }

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <div style="width: 99%; height: 18px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #0085ca; padding: 2px 0px 0px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; margin: 6px 0px 0px 10px; color: #FFFFFF;">Manual Claim Creation</span>
    </div>
    <div style="width: 99%;">
        <asp:UpdatePanel ID="uplanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnladd" runat="server" GroupingText="Manual Entry" Style="margin:5px;" Width="99%">
                    <table style="width: 98%;">
                        <tr>
                            <td>From Date</td>
                            <td class="auto-style6">To Date</td>
                            <td class="auto-style3">Claim Category</td>
                            <td>Claim Sub Cat</td>

                            <td>Object Type</td>
                            <td>Sub Object</td>

                            <td>Obejct Code</td>
                             <td>Business Unit</td>
                            <td>Descritpion</td>

                            <td>Incentive</td>

                            <td></td>

                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server" Width="75px" Height="13px" CssClass="textboxStyleNew"></asp:TextBox>

                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="txtFromDate" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                            </td>
                            <td class="auto-style6">
                                <asp:TextBox ID="txttoDate" runat="server" Width="75 px" Height="13px" CssClass="textboxStyleNew"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="txttoDate" TargetControlID="txttoDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                            </td>
                            <td class="auto-style3">
                                <asp:DropDownList ID="ddlClaimCategory" runat="server" Width="100px" Font-Size="8pt" AutoPostBack="True" Height="21px" OnSelectedIndexChanged="ddlClaimCategory_SelectedIndexChanged" CssClass="textboxStyleNew">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlClaimSubCategory" runat="server" Width="100px" Font-Size="8pt" Height="21px" CssClass="textboxStyleNew">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlObjectType" runat="server" AutoPostBack="true" Width="100px" Font-Size="8pt" Height="21px" OnSelectedIndexChanged="ddlObjectType_SelectedIndexChanged" CssClass="textboxStyleNew">
                                    <asp:ListItem Value="PSR">PSR</asp:ListItem>
                                    <asp:ListItem Value="SITE">SITE</asp:ListItem>
                                    <asp:ListItem Value="CUSTOMERGROUP">CUSTOMERGROUP</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSubObejctType" runat="server" Font-Size="8pt" Height="21px" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlSubObejctType_SelectedIndexChanged" CssClass="textboxStyleNew">
                                </asp:DropDownList>
                            </td>

                            <td>
                                <asp:DropDownList ID="ddlObjectCode" runat="server" AutoPostBack="True" Font-Size="8pt" Height="21px" Width="120px" CssClass="textboxStyleNew">
                                </asp:DropDownList>
                            </td>

                            <td>
                                <asp:DropDownList ID="ddlBusinessUnit" runat="server" AutoPostBack="True" Font-Size="8pt" Height="21px" Width="120px" CssClass="textboxStyleNew">
                                </asp:DropDownList>
                            </td>

                            <td>
                                <asp:TextBox ID="txtDescription" runat="server" Width="100px" MaxLength="200" CssClass="textboxStyleNew" Height="13px"></asp:TextBox>
                            </td>

                            <td>
                                <asp:TextBox ID="txtIncentive" runat="server" Width="55px" TextMode="Number" CssClass="textboxStyleNew" Height="13px"></asp:TextBox>
                            </td>
                            <td>

                                <asp:Button ID="btnAdd" runat="server" CssClass="button" Height="30px" OnClick="btnAdd_Click" Text="+" ValidationGroup="btnValidate" Width="47px" />

                            </td>
                            <td>
                                <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="button" Height="31px" OnClick="btnSubmit_Click1" />

                            </td>
                            <td>
                                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlClaimCategory" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="updatepnlGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 100%;">
                <tr>
                    <td>

                        <div style="margin: 10px 10px 10px 10px; overflow: auto; height: 250px">
                            <asp:GridView ID="grvDetail" runat="server" AutoGenerateColumns="False" Width="100%">
                                <Columns>
                                    <asp:BoundField HeaderText="Date From" DataField="DateFrom" DataFormatString="{0:dd/MMM/yyyy}"></asp:BoundField>
                                    <asp:BoundField HeaderText="Date To" DataField="DateTo" DataFormatString="{0:dd/MMM/yyyy}"></asp:BoundField>
                                    <asp:BoundField DataField="ClaimCat" HeaderText="Claim Cat">
                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CatName" HeaderText="Cat Name" />
                                    <asp:BoundField DataField="ClaimSub" HeaderText="Claim Sub"></asp:BoundField>
                                    <asp:BoundField DataField="SubCatName" HeaderText="Sub Cat Name" />
                                    <asp:BoundField HeaderText="Object Type" DataField="Object"></asp:BoundField>
                                    <asp:BoundField HeaderText="Sub Object" DataField="SubObject"></asp:BoundField>
                                    <asp:BoundField DataField="ObjectCode" HeaderText="Obejct Code" />
                                     <asp:BoundField DataField="BUName" HeaderText="Business Unit " />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:BoundField DataField="Incentive" HeaderText="Incentive" />
                                    <asp:TemplateField Visible="false" ItemStyle-Width="0px">
                                            <ItemTemplate>
                                                       
                                      <asp:HiddenField ID="BUCode" Visible="false" runat="server" Value='<%# Eval("BUCode") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="0px"></ItemStyle>
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

                    </td>
                </tr>
            </table>
        </ContentTemplate>

    </asp:UpdatePanel>


</asp:Content>

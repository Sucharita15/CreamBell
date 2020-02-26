<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucRoleFilters.ascx.cs" Inherits="CreamBell_DMS_WebApps.UserControl.ucRoleFilters" %>
<%--<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>--%>

<%--<script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>--%>
<%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
<link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
<%--<script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
<link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
<script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>--%>

<script type="text/javascript">
    $(function () {

        $('#lstState').multiselect({
            includeSelectAllOption: true,
            nonSelectedText: 'Select State',
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            buttonWidth: '140px',
            maxHeight: 300,
            maxWidth: 50
        });

        $('#lstSiteId').multiselect({
            includeSelectAllOption: true,
            nonSelectedText: 'Select SiteId',
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            buttonWidth: '140px',
            maxHeight: 300,
            maxWidth: 50
        });
    });

    $(document).ready(function () {

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        function EndRequestHandler(sender, args) {
            $('#lstState').multiselect({
                includeSelectAllOption: true,
                nonSelectedText: 'Select State',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '140px',
                maxHeight: 300,
                maxWidth: 50
            });

            $('#lstSiteId').multiselect({
                includeSelectAllOption: true,
                nonSelectedText: 'Select SiteId',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '140px',
                maxHeight: 300,
                maxWidth: 50
            });

        }
    });

</script>

<asp:Panel ID="Panel3" runat="server">
    <div style="width: 100%; height: 20px; border-radius: 4px; margin: 5px 0px 0px 5px; background-color: #1564ad; color: white; padding: 0px 0px 0px 0px; font-weight: bold">
        <table>
            <tr>
                <td runat="server" id="tclink" style="text-align: left;">
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" ForeColor="White">Hide sales person filter</asp:LinkButton>
                </td>
                <td runat="server" id="tclabel" style="text-align: center">User Control Key Customer Sale
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>



<asp:UpdatePanel ID="pnlupd" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlHeader" runat="server" GroupingText="Filter" Style="width: 99%; margin: 0px 0px 0px 5px;">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="Sale Person Filter" Style="width: 99%; margin: 0px 0px 0px 2px;">
                            <table>
                                <tr>
                                    <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 11%;">HOS :
                               <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto;">
                                   <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="chkAll_CheckedChanged" Text="Select All" />
                                   <asp:CheckBoxList ID="chkListHOS" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstHOS_SelectedIndexChanged">
                                   </asp:CheckBoxList>
                               </div>

                                    </td>
                                    <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 11%;">VP :
                    <div class="checkboxlistHeader" style="max-height: 80px; width: 150px; overflow-y: auto;">
                        <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Select All" />
                        <asp:CheckBoxList ID="chkListVP" runat="server" OnSelectedIndexChanged="lstVP_SelectedIndexChanged" AutoPostBack="True">
                        </asp:CheckBoxList>
                    </div>
                                    </td>
                                    <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 11%;">GM :
                    <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto;">
                        <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox2_CheckedChanged" Text="Select All" />
                        <asp:CheckBoxList ID="chkListGM" runat="server" OnSelectedIndexChanged="lstGM_SelectedIndexChanged" AutoPostBack="True">
                        </asp:CheckBoxList>
                    </div>
                                    </td>
                                    <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 13%;">DGM :
                    <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto;">
                        <asp:CheckBox ID="CheckBox3" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox3_CheckedChanged" Text="Select All" />
                        <asp:CheckBoxList ID="chkListDGM" runat="server" OnSelectedIndexChanged="lstDGM_SelectedIndexChanged" AutoPostBack="True">
                        </asp:CheckBoxList>
                    </div>
                                    </td>
                                    <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 14%;">RM :
                    <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto;">
                        <asp:CheckBox ID="CheckBox4" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox4_CheckedChanged" Text="Select All" />
                        <asp:CheckBoxList ID="chkListRM" runat="server" OnSelectedIndexChanged="lstRM_SelectedIndexChanged" AutoPostBack="True">
                        </asp:CheckBoxList>
                    </div>
                                    </td>
                                    <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 14%;">ZM :
                    <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto;">
                        <asp:CheckBox ID="CheckBox5" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox5_CheckedChanged" Text="Select All" />
                        <asp:CheckBoxList ID="chkListZM" runat="server" OnSelectedIndexChanged="lstZM_SelectedIndexChanged" AutoPostBack="True">
                        </asp:CheckBoxList>
                    </div>
                                    </td>
                                    <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 14%;">ASM :
                    <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto;">
                        <asp:CheckBox ID="CheckBox6" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox6_CheckedChanged" Text="Select All" />
                        <asp:CheckBoxList ID="chkListASM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstASM_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </div>
                                    </td>
                                    <td style="background-color: aliceblue; vertical-align: top; text-align: left; width: 16%;">EXECUTIVE :
                    <div class="checkboxlistHeader" style="max-height: 80px; overflow-y: auto;">
                        <asp:CheckBox ID="CheckBox7" runat="server" AutoPostBack="true" Font-Bold="True" ForeColor="#009933" OnCheckedChanged="CheckBox7_CheckedChanged" Text="Select All" />
                        <asp:CheckBoxList ID="chkListEXECUTIVE" runat="server" OnSelectedIndexChanged="lstEXECUTIVE_SelectedIndexChanged" AutoPostBack="True">
                        </asp:CheckBoxList>
                    </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>

            </table>
            <br />
            <div class="option_menu">
                State : 
                <asp:ListBox ID="lstState" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="lstState_SelectedIndexChanged" ClientIDMode="Static" CssClass="txt-width"></asp:ListBox>

                Site ID : 
                <asp:ListBox ID="lstSiteId" runat="server" SelectionMode="Multiple" AutoPostBack="true" ClientIDMode="Static" Width="200px" OnSelectedIndexChanged="lstSiteId_SelectedIndexChanged"></asp:ListBox>
                <%-- Search Customer By :
                <asp:DropDownList ID="DDLSearchType" runat="server" CssClass="multiselect dropdown-toggle btn btn-default" data-toggle="dropdown" Style="text-align: right; width: 200px;">
                <asp:ListItem>Customer Code</asp:ListItem>
                <asp:ListItem>Customer Name</asp:ListItem>
                <asp:ListItem>PSR Code</asp:ListItem>
                </asp:DropDownList>--%>
            </div>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <%--<asp:PostBackTrigger ControlID="lstState" />--%>
        <asp:PostBackTrigger ControlID="lstSiteId" />
        <asp:PostBackTrigger ControlID="lstState" />
        <asp:PostBackTrigger ControlID="chkListEXECUTIVE" />
        <asp:PostBackTrigger ControlID="chkListHOS" />
        <asp:PostBackTrigger ControlID="chkListVP" />
        <asp:PostBackTrigger ControlID="chkListGM" />
        <asp:PostBackTrigger ControlID="chkListDGM" />
        <asp:PostBackTrigger ControlID="chkListRM" />
        <asp:PostBackTrigger ControlID="chkListZM" />
        <asp:PostBackTrigger ControlID="chkListASM" />
        <asp:PostBackTrigger ControlID="chkAll" />
        <asp:PostBackTrigger ControlID="CheckBox1" />
        <asp:PostBackTrigger ControlID="CheckBox2" />
        <asp:PostBackTrigger ControlID="CheckBox3" />
        <asp:PostBackTrigger ControlID="CheckBox4" />
        <asp:PostBackTrigger ControlID="CheckBox5" />
        <asp:PostBackTrigger ControlID="CheckBox6" />
        <asp:PostBackTrigger ControlID="CheckBox7" />
    </Triggers>

</asp:UpdatePanel>

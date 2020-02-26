<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmVersionReleaseRegister.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmVersionReleaseRegister" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <script src="Javascript/custom.js"></script>
    <link href="css/textBoxDesign.css" rel="stylesheet" />
    <link href="css/Polaroide.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("select").searchable();
        });
    </script>
    <style type="text/css">
        .ReportSearch {
            background: #3498db;
            background-image: -webkit-linear-gradient(top, #3498db, #2980b9);
            background-image: -moz-linear-gradient(top, #3498db, #2980b9);
            background-image: -ms-linear-gradient(top, #3498db, #2980b9);
            background-image: -o-linear-gradient(top, #3498db, #2980b9);
            background-image: linear-gradient(to bottom, #3498db, #2980b9);
            -webkit-border-radius: 0;
            -moz-border-radius: 0;
            border-radius: 0px;
            font-family: Arial;
            color: #ffffff;
            font-size: 11px;
            padding: 5px 7px 6px 8px;
            text-decoration: none;
        }
            .ReportSearch:hover {
                background: #3cb0fd;
                background-image: -webkit-linear-gradient(top, #3cb0fd, #3498db);
                background-image: -moz-linear-gradient(top, #3cb0fd, #3498db);
                background-image: -ms-linear-gradient(top, #3cb0fd, #3498db);
                background-image: -o-linear-gradient(top, #3cb0fd, #3498db);
                background-image: linear-gradient(to bottom, #3cb0fd, #3498db);
                text-decoration: none;
            }
    </style>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <asp:UpdatePanel runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID = "btnSearch"/>
        <asp:PostBackTrigger ControlID = "btnVersionHistory"/>
        <asp:PostBackTrigger ControlID = "chkStateAll"/>
        <asp:PostBackTrigger ControlID = "chkUserTypeAll"/>
        <asp:PostBackTrigger ControlID = "chkVersionAll"/>
        <asp:PostBackTrigger ControlID = "chkBlockAll"/>
    </Triggers>
    <ContentTemplate>
    <div style="width: 100%; height: 18px; border-radius: 4px; margin: 5px 0px 0px 5px; background-color: #1564ad; color: white; padding: 2px 0px 0px 0px; font-weight: bold">
        Version Release Report
    </div>
    <table style="width: 100%">
        <tr>
            <td>
                <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Small"> </asp:Label>
            </td>
        </tr>
    </table>
    <div style="width: 100%;">
        <table style="width: 100%;">
            <tr>
                <td style="vertical-align: top;width:5%;text-align:left;padding-left:10px">
                    State
                </td>
                <td style="vertical-align: top;width:15%;">
                    <div style="width:100%;max-height: 80px; overflow-y: auto;">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkStateAll" runat="server" AutoPostBack="true"  OnCheckedChanged="chkAll_CheckedChanged" Text="Select All" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="chkListState" runat="server" AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                        
                    </div>
                </td>
                <td style="vertical-align: top;width:5%;text-align:left;padding-left:5px">
                    User Type
                </td>
                <td style="vertical-align: top;width:8%;">
                    <div style="width:100%;max-height: 80px; overflow-y: auto;">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkUserTypeAll" runat="server" AutoPostBack="true"  OnCheckedChanged="chkUserTypeAll_CheckedChanged" Text="Select All" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="chkListUserType" runat="server" AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td style="vertical-align: top;width:5%;text-align:left;padding-left:5px">
                    Version
                </td>
                <td style="vertical-align: top;width:10%;">
                    <div style="width:100%;max-height: 80px; overflow-y: auto;">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkVersionAll" runat="server" AutoPostBack="true"  OnCheckedChanged="chkVersionAll_CheckedChanged" Text="Select All" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="chkListVersion" runat="server" AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td style="vertical-align: top;width:5%;text-align:left;padding-left:5px">
                    Block
                </td>
                <td style="vertical-align: top;width:10%;">
                    <div style="width:100%;max-height: 80px; overflow-y: auto;">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkBlockAll" runat="server" AutoPostBack="true"  OnCheckedChanged="chkBlockAll_CheckedChanged" Text="Select All" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="chkListBlock" runat="server" AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td>
                    <table>
                        <tr>
                            <td style="vertical-align: top;width:5%;text-align:left;padding-left:5px">
                                user :
                            </td>
                            <td style="vertical-align: top;width:10%;">
                                <asp:TextBox ID ="txtUserCode" runat ="server" style="width:100%;" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top;width:5%;text-align:left;padding-left:5px">
                                Distributor :
                            </td>
                            <td style="vertical-align: top;width:10%;text-align:left;">
                                <asp:TextBox ID ="txtDistributor" runat ="server" style="width:100%;" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top;width:100%;padding-left:5px" colspan="2">
                                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" text="Current Version"/>
                                <asp:Button ID="btnVersionHistory" runat="server" OnClick="btnVersionHistory_Click" text="Version History "/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
 <div style="margin: 10px">
    <asp:Panel ID="PanelReport" runat="server" GroupingText="Version Release Report" Width="100%" BackColor="White">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="500px"></rsweb:ReportViewer>
    </asp:Panel>
 </div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmVersionReleaseInfo.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmVersionReleaseInfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
  <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
  <link href="css/btnSearch.css" rel="stylesheet" /> 
    <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" /> 
        <link href="css/textBoxDesign.css" rel="stylesheet" />
        <link href="css/SSheet.css" rel="Stylesheet" type="text/css" />

  <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
  
  <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.single-selection').multiselect({
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                nonSelectedText: 'Select'
            });
        });
        </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("select").searchable();
        });
    </script>
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
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <div style="height: 15px; border-radius: 4px; margin: 10px 0px 0px 5px; background-color: #538dd5; padding: 0px 0px 5px 0px;">
        <span style="font-family: Segoe UI; font-size: 11px; font-weight: bold; color: white; margin: 0px 0px 0px 10px;">Version Release Info</span>
    </div>
    <div style="width: 100%;">
        <table style="width: 100%;">
            <tr>
                <td style="width: 8%; text-align: left; padding-left: 10px">Version :</td>
                <td style="width: 8%">
                   <%-- <asp:DropDownList ID="ddlVersion" runat="server" Width="183px" TabIndex="0" />--%>

                     <asp:ListBox ID="ddlVersionNew" ClientIDMode="Static" runat="server" CssClass="single-selection"></asp:ListBox>
                </td>
                <td style="width: 10%">
                    <asp:Button ID="btnRelease" runat="server" Style="margin: 0px 0px 0px 0px" Text="Release" OnClick="btnRelease_Click" TabIndex="3"></asp:Button>
                </td>
                <td style="width: 100%; text-align: left">
                    <asp:ImageButton ID="imgBtnExportToExcel" runat="server" ImageUrl="~/Images/excel-24.ico" ToolTip="Click To Generate Excel Report" Style="margin: 0px 0px 0px 10px" OnClick="imgBtnExportToExcel_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div style="width: 100%;">
        <br />
        <table style="width: 100%;">
            <tr>
                <td style="vertical-align: top; width: 8%; text-align: left; padding-left: 10px">State :
                </td>
                <td style="vertical-align: top; width: 8%;">
                    <%--<asp:DropDownList ID="ddlState" runat="server" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" style="width:100%;" >
                   </asp:DropDownList>--%>
                 <%--   <asp:DropDownCheckBoxes ID="ddlState" runat="server" Width="100%" UseSelectAllNode="true">
                        <Style SelectBoxWidth="195" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="90" />
                    </asp:DropDownCheckBoxes>--%>
                     <asp:ListBox ID="ddlStateNew" ClientIDMode="Static" runat="server" 
                       CssClass="single-selection"></asp:ListBox>
                    <%--<asp:ExtendedRequiredFieldValidator ID="ExtendedRequiredFieldValidator3" runat="server" ControlToValidate="ddlState" ErrorMessage="Required" ForeColor="Red"></asp:ExtendedRequiredFieldValidator>--%>
                </td>
                <td style="vertical-align: top; width: 7%; text-align: left; padding-left: 5px">Distributor :
                </td>
                <td style="vertical-align: top; width: 15%; text-align: left;">
                    <asp:TextBox ID="txtDistributor" runat="server" Style="width: 100%;"></asp:TextBox>
                </td>
                <td style="vertical-align: top; width: 6%; text-align: left; padding-left: 5px">User Type :
                </td>
                <td style="vertical-align: top; width: 8%; text-align: left; padding-left: 5px">
                    <%-- <asp:DropDownList ID="ddlUserType" runat="server" OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged" style="width:100%;" >
                   </asp:DropDownList>--%>
                  <%--  <asp:DropDownCheckBoxes ID="ddlUserType" runat="server" Width="100%" UseSelectAllNode="true">
                        <Style SelectBoxWidth="100" DropDownBoxBoxWidth="100" DropDownBoxBoxHeight="90" />
                    </asp:DropDownCheckBoxes>--%>

                     <asp:ListBox ID="ddlUserTypeNew" ClientIDMode="Static" runat="server" 
                      CssClass="single-selection"></asp:ListBox>
                    <%--<asp:ExtendedRequiredFieldValidator ID="ExtendedRequiredFieldValidator1" runat="server" ControlToValidate="ddlUserType" ErrorMessage="Required" ForeColor="Red"></asp:ExtendedRequiredFieldValidator>--%>

                </td>
                <td style="vertical-align: top; width: 5%; text-align: left; padding-left: 5px">user :
                </td>
                <td style="vertical-align: top; width: 15%;">
                    <asp:TextBox ID="txtUserCode" runat="server" Style="width: 100%;"></asp:TextBox>
                </td>
                <td style="vertical-align: top; width: 5%; text-align: left; padding-left: 5px">Version :
                </td>
                <td style="vertical-align: top; width: 8%;">
                    <%--<asp:DropDownList ID="ddlVersionCode" runat="server" OnSelectedIndexChanged="ddlVersionCode_SelectedIndexChanged" style="width:100%;" >
                   </asp:DropDownList>--%>
                 <%--   <asp:DropDownCheckBoxes ID="ddlVersionCode" runat="server" Width="100%" UseSelectAllNode="true">
                        <Style SelectBoxWidth="100" DropDownBoxBoxWidth="100" DropDownBoxBoxHeight="90" />
                    </asp:DropDownCheckBoxes>--%>

                     <asp:ListBox ID="ddlVersionCodeNew" ClientIDMode="Static" runat="server"   
                      CssClass="single-selection"></asp:ListBox>
                    <%--<asp:ExtendedRequiredFieldValidator ID="ExtendedRequiredFieldValidator4" runat="server" ControlToValidate="ddlUserType" ErrorMessage="Required" ForeColor="Red"></asp:ExtendedRequiredFieldValidator>--%>

                </td>
                <td style="vertical-align: top; width: 5%; text-align: left; padding-left: 5px">Block :
                </td>
                <td style="vertical-align: top; width: 8%;">
                    <%--<asp:DropDownList ID="ddlIsBlock" runat="server" OnSelectedIndexChanged="ddlIsBlock_SelectedIndexChanged" style="width:100%;" >
                   </asp:DropDownList>--%>
                   <%-- <asp:DropDownCheckBoxes ID="ddlIsBlock" runat="server" Width="100%" UseSelectAllNode="true">
                        <Style SelectBoxWidth="100" DropDownBoxBoxWidth="100" DropDownBoxBoxHeight="90" />
                    </asp:DropDownCheckBoxes>--%>

                    <asp:ListBox ID="ddlIsBlockNew" ClientIDMode="Static" runat="server" 
                        CssClass="single-selection"></asp:ListBox>
                    <%--<asp:ExtendedRequiredFieldValidator ID="ExtendedRequiredFieldValidator2" runat="server" ControlToValidate="ddlIsBlock" ErrorMessage="Required" ForeColor="Red"></asp:ExtendedRequiredFieldValidator>--%>
                </td>
                <td style="vertical-align: top; width: 100%; padding-left: 5px">
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvVersionInfo" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="100%" BackColor="White" BorderColor="#CCCCCC"
            BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeaderWhenEmpty="True" PageSize="5" OnPageIndexChanging="gvVersionInfo_PageIndexChanging">
            <AlternatingRowStyle BackColor="#CCFFCC" />
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" AutoPostBack="true" OnCheckedChanged="checkAll_CheckedChanged" Text="All" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkStatus" runat="server" AutoPostBack="true" OnCheckedChanged="chkStatus_OnCheckedChanged" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                    <ItemStyle HorizontalAlign="Left" Font-Bold="True" ForeColor="#000066" />
                </asp:TemplateField>

                <asp:BoundField HeaderText="State Code" DataField="STATECODE">
                    <HeaderStyle HorizontalAlign="Left" Width="8%" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="8%" />
                </asp:BoundField>
                <asp:BoundField HeaderText="State Name" DataField="STATENAME">
                    <HeaderStyle HorizontalAlign="Left" Width="8%" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="8%" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Distributor Code" DataField="DISTRIBUTORCODE">
                    <HeaderStyle HorizontalAlign="Left" Width="8%" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="8%" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Distributor Name" DataField="DISTRIBUTORNAME">
                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="8%" />
                </asp:BoundField>

                <asp:BoundField HeaderText="User Type" DataField="USERTYPE">
                    <HeaderStyle HorizontalAlign="Left" Width="8%" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="8%" />
                </asp:BoundField>

                <asp:BoundField HeaderText="User Code" DataField="USERCODE">
                    <HeaderStyle HorizontalAlign="Left" Width="8%" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="8%" />
                </asp:BoundField>

                <asp:BoundField HeaderText="User Name" DataField="USERNAME">
                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="8%" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Version" DataField="VERSIONNAME">
                    <HeaderStyle HorizontalAlign="Left" Width="8%" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="8%" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Description" DataField="DESCRIPTION">
                    <HeaderStyle HorizontalAlign="Left" Width="8%" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="8%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Is Blocked">
                    <ItemTemplate>
                        <asp:HiddenField ID="hfRecId" runat="server" Value='<%# Bind("RECID") %>'></asp:HiddenField>
                        <asp:LinkButton ID="lnkbtn" runat="server" Text='<%# Eval("ISBLOCK") %>' OnClick="lnkbtn_Click" OnClientClick="return confirm('Are You Sure?');"></asp:LinkButton>
                        <%--<asp:CheckBox ID="chkConfirm" runat="server" AutoPostBack="true" onclick="confirmChecked(this.checked)" OnCheckedChanged="chkConfirm_CheckedChanged" Text='<%# Eval("IsConfirm").ToString() %>'/>--%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="8%" />
                    <ItemStyle HorizontalAlign="Left" Font-Bold="True" ForeColor="#000066" Width="10%" />
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
    </asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmItemSKUWise.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmItemSKUWise" %>
<%--<%@ Page Title="" Language="C#" MasterPageFile="~/New.Master" AutoEventWireup="true" CodeBehind="frmItemSKUWise.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmItemSKUWise" %>--%>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/ucRoleFilters.ascx" TagPrefix="uc1" TagName="ucRoleFilters" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
                            $(document).ready(function () {
                                $('.single-selection').multiselect({
                                    enableFiltering: true,
                                    enableCaseInsensitiveFiltering: true,
                                    nonSelectedText: 'Select'
                                });
                            });
        </script>
    <link href="css/btnSearch.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />
    <link href="css/SSheet.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">      

        .input1 {
            width: 270px;
            height: 10px;
            padding: 10px 5px;
            float: left;
            border: 0;
            background: #eee;
            -moz-border-radius: 3px 0 0 3px;
            -webkit-border-radius: 3px 0 0 3px;
            border-radius: 3px 0 0 3px;
        }

        .arrow_box {
            position: relative;
            background: #16365c;
        }

        .button1 {
            overflow: visible;
            position: relative;
            float: right;
            border: 0;
            padding: 0;
            cursor: pointer;
            height: 30px;
            width: 68px;
            color: #fff;
            text-transform: uppercase;
            background: #16365c;
            -moz-border-radius: 0 3px 3px 0;
            -webkit-border-radius: 0 3px 3px 0;
            border-radius: 0 3px 3px 0;
            text-shadow: 0 -1px 0 rgba(0, 0,0, .3);
        }

        .auto-style3 {
            width: 157px;
        }
        .auto-style4 {
            width: 84px;
        }
        .auto-style5 {
            width: 201px;
        }
        .auto-style6 {
            width: 85px;
        }
        .auto-style8 {
            width: 126px;
        }
        .auto-style9 {
            width: 258px;
        }
        .auto-style10 {
            width: 179px;
        }
        .auto-style11 {
            width: 121px;
        }
        .rr {
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

     
    
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
      
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport" />
            <asp:PostBackTrigger ControlID="ucRoleFilters" />
        </Triggers>
           <ContentTemplate>
               <uc1:ucRoleFilters runat="server" ID="ucRoleFilters" />
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
            <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                   <div style="width: 100%;height: 18px;border-radius: 4px;margin: 10px 0px 0px 5px;background-color: #1564ad;color:white ;padding: 2px 0px 0px 0px; font-weight:bold" >
    Item (SKU) Wise Sale Report
     </div>
        <table style="width:100%">
           

            <tr>
                <td>From Date :</td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server"  placeholder="dd-MMM-yyyy" Width="90px" ></asp:TextBox> 
                    <asp:ImageButton ID="imgBtnFrom" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>
                
                <td>To Date :</td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server"  placeholder="dd-MMM-yyyy" Width="90px"></asp:TextBox> 
                    <asp:ImageButton ID="imgBtnTo" ImageUrl="~/Images/calendar.jpg" ImageAlign="Bottom" runat="server" />
                </td>
                  
              

                 <td>
                    Customers Group:
                </td>
                <td>
                   <%-- <asp:DropDownList ID="DDLCustGroup" runat="server" AutoPostBack="True" Width="200px" OnSelectedIndexChanged="DDLCustGroup_SelectedIndexChanged">
                    </asp:DropDownList>--%>
                      <asp:ListBox ID="DDLCustGroupNew" ClientIDMode="Static" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="DDLCustGroup_SelectedIndexChanged" CssClass="single-selection"></asp:ListBox>
                </td>
                <td>
                    Customers:
                </td>
                <td>
                   <%-- <asp:DropDownList ID="DDLCustomers" runat="server" AutoPostBack="True" Width="200px" >
                    </asp:DropDownList>--%>
                     <asp:ListBox ID="DDLCustomersNew" ClientIDMode="Static" runat="server" 
                           CssClass="single-selection"></asp:ListBox>
                </td>
                <tr>

                <td>
                     
                    Product Group:
                </td>
                <td>
                    <%--<asp:DropDownList ID="DDLProductGroup" runat="server" AutoPostBack="True" Width="100px" 
                        OnSelectedIndexChanged="DDLProductGroup_SelectedIndexChanged"> </asp:DropDownList>--%>

                     <asp:ListBox ID="DDLProductGroupNew" ClientIDMode="Static" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="DDLProductGroup_SelectedIndexChanged" CssClass="single-selection"></asp:ListBox>
                </td>
                <td>
                    Sub Category:
                </td>
                <td>
                   <%-- <asp:DropDownList ID="DDLSubCategory" runat="server" AutoPostBack="True" Width="100px" OnSelectedIndexChanged="DDLSubCategory_SelectedIndexChanged">
                    </asp:DropDownList>--%>
                     <asp:ListBox ID="DDLSubCategoryNew" ClientIDMode="Static" runat="server" 
                            CssClass="single-selection"></asp:ListBox>
                </td>
         <td>
                    <asp:Button ID="BtnShowReport" runat="server" Text="Show Report" CssClass="ReportSearch" OnClick="BtnShowReport_Click"  />
                    
                </td>
                

                <td>
                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Large"> </asp:Label>
                </td>
                </tr>
                             
            </tr>
  </table>
        </ContentTemplate>
        </asp:UpdatePanel>


    <p></p>
    <asp:Panel ID="PanelReport" runat="server" GroupingText="Report" Width="100%"  BackColor="White" Style="margin-left:0px" >

         <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="375px">

         </rsweb:ReportViewer>

    </asp:Panel>
  
</asp:Content>

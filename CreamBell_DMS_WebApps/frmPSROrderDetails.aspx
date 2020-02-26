<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.Master" CodeBehind="frmPSROrderDetails.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmPSROrderDetails" %>

 <%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="css/btnSearch.css" rel="stylesheet" />
     <link href="css/style.css" rel="stylesheet" />
    
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />

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

    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <div style="width: 1000px;height: 18px;border-radius: 4px;margin: 10px 0px 0px 5px;background-color: #1564ad;color:white ;padding: 2px 0px 0px 0px; font-weight:bold" >
      &nbsp;&nbsp; PSR Order Details
     </div>

      <asp:Panel ID="PanelFilter" runat="server" GroupingText="Filters"  Height="103px" Width="1008px" >       
    
     <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport" />
        </Triggers>
           <ContentTemplate>
        <table>
             <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
    <asp:CalendarExtender ID="CalendarExtender" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>

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
                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Large"> </asp:Label>
                </td>
                    
         </tr>
            <tr>
                <td>
                    PSR Code:
                </td>
                <td>
                    <asp:DropDownList ID="DrpPSRDetails" runat="server" Font-Size="Small"  Width="150px" OnSelectedIndexChanged="DrpPSRDetails_SelectedIndexChanged" >
                    </asp:DropDownList>
                </td>
                <td>
                    Product Group:
                </td>
                <td>
                    <asp:DropDownList ID="DrpProductGrp" runat="server"  Width="120px" OnSelectedIndexChanged="DrpProductGrp_SelectedIndexChanged" >
                    </asp:DropDownList>
                </td>

                 <td>
                    <asp:Button ID="BtnShowReport" runat="server" Text="Show Report" CssClass="ReportSearch" OnClick="BtnShowReport_Click"  />
                    
                </td>

             </tr>
            </td>
               <%-- <td>
                    <asp:ImageButton ID="imgBtnRefresh" runat="server" ImageUrl="~/Images/refresh.png"  Height="20px" 
                        ToolTip="Click To Refresh the Filters" OnClick="imgBtnRefresh_Click" />
                </td>--%>
                 </tr>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>

      </asp:Panel>

    <p></p>
    <center>
     <asp:Panel ID="PanelReport" runat="server" GroupingText="Report" Width="1008px"  BackColor="White" Style="margin-left:-62px" >

         <rsweb:ReportViewer ID="rptViewer" runat="server" Width="996px" BackColor="GhostWhite" Height="375px">

         </rsweb:ReportViewer>

    </asp:Panel>
    </center>

    </asp:Content>


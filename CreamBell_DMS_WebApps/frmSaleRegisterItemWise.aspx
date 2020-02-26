<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmSaleRegisterItemWise.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmSaleRegisterItemWise" %>

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
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowReport" />
            <asp:PostBackTrigger ControlID="DDLCustGroupNew" />
            <asp:PostBackTrigger ControlID="ucRoleFilters" />
        </Triggers>
           <ContentTemplate>
               <uc1:ucRoleFilters runat="server" ID="ucRoleFilters" />
    <div style="width: 100%;height: 18px;border-radius: 4px;margin: 10px 0px 0px 5px;background-color: #1564ad;color:white ;padding: 2px 0px 0px 0px; font-weight:bold" >
      Sale Register Item Wise
     </div>
       
       <asp:Panel ID="PanelFilter" runat="server" GroupingText="Filters"  Height="58px" Width="100%" >       
    
      
        <table style="width:100%">
           
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnFrom" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
            <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgBtnTo" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>

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
                   <%-- <asp:DropDownList ID="DDLCustGroup" runat="server" AutoPostBack="True" Width="200px">
                    </asp:DropDownList>--%>
                       <asp:ListBox ID="DDLCustGroupNew" ClientIDMode="Static" runat="server" 
                             CssClass="single-selection"></asp:ListBox>
                </td>
                <td>
                    <asp:Button ID="BtnShowReport" runat="server" Text="Show Report" CssClass="ReportSearch" OnClick="BtnShowReport_Click"   />
                    
                </td>
                

                <td>
                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed" Font-Names="Seoge UI" Font-Size="Large"> </asp:Label>
                </td>
                             
            </tr>

        </table>
    </asp:Panel>
    <p></p>
    
     <asp:Panel ID="PanelReport" runat="server" GroupingText="Report" Width="100%"  BackColor="White" Style="margin-left:0px" >

         <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="GhostWhite" Height="375px">

         </rsweb:ReportViewer>

    </asp:Panel>
  </ContentTemplate>
        </asp:UpdatePanel>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmCustomerPartyMasterNew.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmCustomerPartyMasterNew" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/textBoxDesign.css" rel="stylesheet" />
     <script src="DatePicker/jquery.js"></script>
       <script src="DatePicker/moment.min.js"></script>
       <link href="DatePicker/BootStrapCalendar.css" rel="stylesheet" />
       <script src="Bootstrap/jquery.min.js"></script>
       <script src="Bootstrap/bootstrap.min.js"></script>
       <script src="Bootstrap/bootstrap-select.js"></script>
       <link href="Bootstrap/bootstrap-select.css" rel="stylesheet" />

    <style type="text/css">
        .auto-style2 {
            height: 26px;
        }
        .auto-style3 {
            padding-right: 20px;
            height: 26px;
        }
    </style>

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
     
   
    
    <div style="width: 98%;height: 18px;border-radius: 4px;margin: 0px 0px 0px 0px;background-color: #1564ad;padding: 2px 0px 0px 0px;" >
      <span style="font-family: Segoe UI;font-size: 11px;font-weight: bold;margin: 0px 0px 0px 10px;  color:white">Customer / Party Master
        &nbsp;&nbsp;&nbsp;  <asp:Literal ID="LiteralCustID" runat="server" ></asp:Literal>
      </span>
    </div>
    

    <div style="width:100%; text-align:left">
            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="button" style="font-size: 12px;font-family: Segoe UI;
text-decoration: none;vertical-align: middle;border-radius: 5px" Height="28px" Width="69px" OnClick="btnBack_Click" />
          <table style="width:100%;border-spacing:0px">
                        <tr>
                            <td class="auto-style2">Customer Name</td>
                            <td class="auto-style2"><asp:TextBox ID="txtCustomerName" runat="server" Enabled="false"></asp:TextBox></td>
                            <td class="auto-style3"></td>
                            <td class="auto-style2">CST</td>                         
                            
                            <td class="auto-style2"><asp:TextBox ID="txtCST" runat="server" Enabled="false" /></td>
                            <td class="auto-style3"></td>
                            <%--<td>Channel Type</td>
                            <td><asp:TextBox ID="txtChannelType" runat="server"  Enabled="false" /></td>--%>
                            <td class="auto-style2">Visit Frequency</td>
                            <td class="auto-style2"><asp:TextBox ID="txtVisitFrequency" runat="server" Enabled="false"></asp:TextBox></td>
                        </tr>

                        <tr>
                              <td>Contact Person</td>
                              <td><asp:TextBox ID="txtContactPerson" runat="server" Enabled="false" ></asp:TextBox></td>
                              <td>&nbsp;</td>
                            <td>TAN</td>
                              <td><asp:TextBox ID="txtTAN" runat="server" Enabled="false" /></td>
                            
                              <td>&nbsp;</td>
                              <td>Repeat Week</td>
                              <td><asp:TextBox ID="txtRepeatWeek" runat="server" Enabled="false"></asp:TextBox></td>
                              
                        </tr>
                        <tr>
                            <td>Address 1</td>
                            <td><asp:TextBox ID="txtAddress1" runat="server" Enabled="false" /></td>
                            <td>&nbsp;</td>
                            <td>Register Date</td>
                            <td><asp:TextBox ID="txtRegDate" runat="server" Enabled="false"/></td>
                            
                            <td>&nbsp;</td>
                            <td>Sequence No.</td>
                            <td><asp:TextBox ID="txtSequenceno" runat="server" Enabled="false"></asp:TextBox></td>                            
                            
                        </tr>

                        <tr>
                            <td>Address 2</td>
                            <td><asp:TextBox ID="txtAddress2" runat="server" Enabled="false"/></td>
                            <td>&nbsp;</td>
                            <td>Closing Date</td>
                            <td><asp:TextBox ID="txtClosingDate" runat="server" Enabled="false" /></td>
                            
                            
                            <td>&nbsp;</td>
                            <td>Key Customer</td>
                            <td><asp:TextBox ID="txtKeyCustomer" runat="server"  Enabled="false"/></td>
                            
                        </tr>

                        <tr>
                            <td>City</td>
                            <td><asp:TextBox ID="txtCity" runat="server" Enabled="false" /></td>
                            <td>&nbsp;</td>
                            <td>Customer Group</td>
                            <td><asp:TextBox ID="txtCustGroup" runat="server" Enabled="false"></asp:TextBox></td>
                            <td>&nbsp;</td>
                            <td>Active/Inactive</td>
                              <td><asp:TextBox ID="txtActiveInactive" runat="server" Enabled="false" /></td>
                        </tr>

                        <tr>
                              <td>Zip Code</td>
                              <td><asp:TextBox ID="txtZipCode" runat="server" Enabled="false" ></asp:TextBox></td>
                              <td>&nbsp;</td>
                            <td>Distance [SS to Distributor]</td>
                            <td><asp:TextBox ID="txtDistance" runat="server" Enabled="false"/></td>
                              
                              <td>&nbsp;</td>
                              <td>Outlet Type</td>
                            <td><asp:TextBox ID="txtOutletType" runat="server" Enabled="false" /></td>                            
                        </tr>
                        <tr>
                            <td>Area</td>
                            <td><asp:TextBox ID="txtArea" runat="server" Enabled="false" ></asp:TextBox></td>
                            <td>&nbsp;</td>
                            <td>PSR Name</td>
                            <td><asp:TextBox ID="txtPSRName" runat="server" Enabled="false"/></td>
                            <td>&nbsp;</td>
                            <td>Scheme/Discount Applicable</td>
                            <td ><asp:TextBox ID="txtSchemeDisc" runat="server" Enabled="false" /></td>
                            
                            
                        </tr>
                        <tr>
                            <td>District</td>
                            <td><asp:TextBox ID="txtDistrict" runat="server" Enabled="false" ></asp:TextBox></td>
                            <td>&nbsp;</td>
                            <td>PSR Beat Name</td>
                            <td><asp:TextBox ID="txtPSRBeatName" runat="server" Enabled="false"></asp:TextBox></td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            
                        </tr>
                        <tr>
                            <td>State</td>
                            <td><asp:TextBox ID="txtState" runat="server" Enabled="false" ></asp:TextBox></td>
                            <td>&nbsp;</td>
                            <td>Channel Type</td>
                            <td><asp:TextBox ID="txtChannelType" runat="server" Enabled="false"></asp:TextBox></td>
                            <td>&nbsp;</td>
                            <td colspan="2" style="border: 2px solid #000000; background-color:#8db4e2; text-align: center;">Claim Parameter</td>
                        </tr>
                        <tr>
                            <td>Mobile No</td>
                            <td><asp:TextBox ID="txtMobileNo" runat="server" Enabled="false" ></asp:TextBox></td>
                            <td>&nbsp;</td>
                            <td>Deep Freezer</td>
                            <td><asp:TextBox ID="txtDeepFreeer" runat="server" Enabled="false"/></td>
                            <td>&nbsp;</td>
                            <td style="border-left-style: solid; border-left-width: 2px; border-left-color: #000000">Handling Charges %</td>
                            <td style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000">
                                <asp:TextBox ID="txtHandlingChargePerc" runat="server" Enabled="false" /></td>
                            
                        </tr>
                        <tr>
                            <td>Phone No</td>
                            <td><asp:TextBox ID="txtPhoneNo" runat="server" Enabled="false"></asp:TextBox></td>
                            <td>&nbsp;</td>
                            <td>Monday</td>
                            <td><asp:TextBox ID="txtMonday" runat="server" Enabled="false"></asp:TextBox></td>
                            <td>&nbsp;</td>
                            <td style="border-left-style: solid; border-left-width: 2px; border-left-color: #000000">Handling Charges Fixed per LTR</td>
                            <td style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000">
                                <asp:TextBox ID="txtHandlingChargeFixedperLitre" runat="server" Enabled="false"/></td>
                        </tr>
                        <tr>
                            <td>Email ID</td>
                            <td><asp:TextBox ID="txtEmailID" runat="server" Enabled="false"></asp:TextBox></td>
                            <td>&nbsp;</td>
                            <td>Tuesday</td>
                            <td><asp:TextBox ID="txtTuesday" runat="server" Enabled="false" /></td>
                            <td>&nbsp;</td>
                            <td style="border-left-style: solid; border-left-width: 2px; border-left-color: #000000">Secondary Charges</td>
                            <td style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000">
                                <asp:TextBox ID="txtSecondaryTransprt" runat="server" Enabled="false" /></td>
                        </tr>
                        <tr>
                           <td>Channel Group</td>
                           <td><asp:TextBox ID="txtChannelGroup" runat="server" Enabled="false"></asp:TextBox></td>
                           <td>&nbsp;</td>
                           <td>Wednesday</td>
                           <td><asp:TextBox ID="txtWednesday" runat="server" Enabled="false" /></td>
                           <td>&nbsp;</td>
                           <td style="border-left-style: solid; border-left-width: 2px; border-left-color: #000000">Secondary Transportation</td>
                            <td style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000">
                                <asp:TextBox ID="txt" runat="server" Enabled="false" /></td>
                            
                           
                          
                        </tr>
                        <tr>
                           <td>Payment Terms</td>
                           <td><asp:TextBox ID="txtPaymentTerm" runat="server" Enabled="false" /></td>
                           <td>&nbsp;</td>
                           <td>Thursday</td>
                           <td><asp:TextBox ID="txtThursday" runat="server" Enabled="false" /></td>
                           <td>&nbsp;</td>
                           <td style="border-left-style: solid; border-left-width: 2px; border-left-color: #000000">&nbsp;</td>
                            <td style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000">&nbsp;</td>
                           
                        </tr>
                        <tr>
                              <td>Payment Mode</td>
                              <td><asp:TextBox ID="txtPaymentMode" runat="server"  Enabled="false"/></td>
                            <td>&nbsp;</td>
                             <td>Friday</td>
                             <td><asp:TextBox ID="txtFriday" runat="server" Enabled="false"></asp:TextBox></td>
                             <td>&nbsp;</td>
                             <td style="border-left-style: solid; border-left-width: 2px; border-left-color: #000000">&nbsp;</td>
                           <td style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000">&nbsp;</td>
                          
                        </tr>
                <tr>
                            <td>PAN</td>
                            <td><asp:TextBox ID="txtPAN" runat="server" Enabled="false" /></td>                            
                            <td>&nbsp;</td>
                             <td>Saturday</td>
                             <td><asp:TextBox ID="txtSaturday" runat="server" Enabled="false"></asp:TextBox></td>
                             <td>&nbsp;</td>
                            <td style="border-left-style: solid; border-left-width: 2px; border-left-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;">&nbsp;</td>
                           <td style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;">&nbsp;</td>
                        </tr>

                        <tr>
                            <td>TIN/VAT</td>
                            <td><asp:TextBox ID="txtTINVAT" runat="server" Enabled="false" /></td>
                            <td>&nbsp;</td>
                            <td>Sunday</td>
                            <td><asp:TextBox ID="txtSunday" runat="server" Enabled="false"></asp:TextBox></td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td></td>
                        </tr>


                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                             <td>&nbsp;&nbsp;</td>
                             <td>&nbsp;</td>
                             <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td></td>
                        </tr>
                         <tr>
                            <td colspan="2" style="border: 2px solid #000000; background-color: #8db4e2; text-align: center;">Primary Discount</td>
                            <td>&nbsp;</td>
                             <td colspan="2" class="tdpadding" style="border: 2px solid #000000; background-color: #8db4e2; text-align: center;">&nbsp;&nbsp;Secondary Discount</td>
                             <td >&nbsp;</td>
                             <td colspan="2" style="border: 2px solid #000000; background-color: #8db4e2; text-align: center;">Pricing Patern</td>
                            
                            
                        </tr>
 <tr>
                            <td style="border-left-style: solid; border-left-width: 2px; border-left-color: #000000">Discount Pattern</td>
                            <td style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000">
                                <asp:TextBox ID="txtDiscountPatternPrimary" runat="server" Enabled="false"></asp:TextBox></td>
                            <td>&nbsp;</td>
                             <td style="border-left-style: solid; border-left-width: 2px; border-left-color: #000000">Discount Pattern</td>
                             <td style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000">
                                 <asp:TextBox ID="txtDiscountPatternSecondary" runat="server" Enabled="false" /></td>
                             <td>&nbsp;</td>
                            <td style="border-left-style: solid; border-left-width: 2px; border-left-color: #000000">Pricing Pattern</td>
                            <td style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000">
                                <asp:TextBox ID="txtPricingPattern" runat="server" Enabled="false"/></td>
                        </tr>
 <tr>
                            <td style="border-left-style: solid; border-left-width: 2px; border-left-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;">Discount Type</td>
                            <td style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;">
                                <asp:TextBox ID="TextBox38" runat="server" Enabled="false"></asp:TextBox></td>
                            <td>&nbsp;</td>
                             <td style="border-left-style: solid; border-left-width: 2px; border-left-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;">Discount Type</td>
                             <td style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;">
                                 <asp:TextBox ID="TextBox41" runat="server" Enabled="false"/></td>
                             <td>&nbsp;</td>
                            <td style="border-left-style: solid; border-left-width: 2px; border-left-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;">&nbsp;</td>
                            <td style="border-right-style: solid; border-right-width: 2px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;"></td>
                        </tr>


                  </table>
    </div>

</asp:Content>

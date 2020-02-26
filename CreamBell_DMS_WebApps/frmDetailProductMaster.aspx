<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmDetailProductMaster.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmDetailProductMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
         <link href="css/style.css" rel="stylesheet" />
        <link href="css/textBoxDesign.css" rel="stylesheet" />

     <style type="text/css">
         .auto-style1
         {
             height: 20px;
         }
     </style>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <div style="width: 100%;height: 18px;border-radius: 4px;background-color: #1564ad;padding: 2px 0px 0px 0px;" >
      <span style="font-family: Segoe UI;font-size: 11px;font-weight: bold;color:white ;margin: 6px 0px 0px 0px;">Product Master
      </span>
    </div>
      <%--<div style="border-bottom-style: solid; border-bottom-width: 5px; border-bottom-color: #000066" >--%>
        <table>
          
          
       </table>
  <%--<div class="form-style-6" style="margin-left: 0px;width: 60.2%">--%>

           <table style="width:100%; border-spacing:1px; height: 213px;">
                 <tr>
                <td style="text-align:left">
                  <asp:Button ID="btnBack" runat="server" Text="Back"  style="font-size: 12px;font-family: Segoe UI;
                   " Height="24px" Width="69px" OnClick="btnBack_Click" />
                </td>
                       </tr>
                        <tr>
                            <td>Product Name</td>
                            <td><asp:TextBox ID="txtProductName" runat="server" Width="250px" Wrap="False" Enabled="False"></asp:TextBox></td>
                            <%--<td class="tdpadding">&nbsp;</td>--%>
                            <td>ML/PC</td>
                            <td><asp:TextBox ID="txtLTR" runat="server" Width="250px" Enabled="False" /></td>
                            <%--<td class="tdpadding">&nbsp;</td>--%>
                        </tr>
                            <tr>
                              <td>Product Category</td>
                              <td><asp:TextBox ID="txtMaterialCategory" runat="server" Width="250px" Enabled="False" ></asp:TextBox></td>
                              <%--<td>&nbsp;</td>--%>
                              <td>Volume(LTR)</td>
                              <td><asp:TextBox ID="txtVolume" runat="server" Width="250px" Enabled="False" /></td>
                              <%--<td>&nbsp;</td>--%>
                        </tr>
                        <tr>
                            <td>Product Group</td>
                            <td><asp:TextBox ID="txtMaterialGroup" runat="server" Width="250px" Enabled="False"  /></td>
                            <%--<td>&nbsp;</td>--%>
                            <td>Gross Weight</td>
                            <td><asp:TextBox ID="txtGrossWt" runat="server" Width="250px" Enabled="False"  /></td>                            
                           <%-- <td>&nbsp;</td>                            --%>
                        </tr>

                        <tr>
                            <td>Product Code</td>
                            <td><asp:TextBox ID="txMaterialCode" runat="server" Width="250px" Enabled="False" /></td>
                            <%--<td>&nbsp;</td>--%>
                            <td>Net Weight</td>
                            <td><asp:TextBox ID="txtNetWt" runat="server" Width="250px" Enabled="False"  /></td>
                            <%--<td>&nbsp;</td>--%>
                        </tr>

                        <tr>
                            <td>Product Nick Name</td>
                            <td><asp:TextBox ID="txtMaterialNickName" runat="server" Width="250px" Enabled="False"  /></td>
                            <%--<td>&nbsp;</td>--%>
                            <td>Barcode Number</td>
                            <td><asp:TextBox ID="txtBarcodeNumber" runat="server" Width="250px" Enabled="False"  /></td>
                            <%--<td>&nbsp;</td>--%>
                        </tr>

                        <tr>
                              <td>Product MRP</td>
                              <td><asp:TextBox ID="txtMaterialMRP" runat="server" Width="250px" Enabled="False"  ></asp:TextBox></td>
                              <%--<td>&nbsp;</td>--%>
                              <td>Default Warehouse</td>
                              <td><asp:TextBox ID="DefaultWarehouse" runat="server" Width="250px" Enabled="False" /></td>
                              <%--<td>&nbsp;</td>--%>
                        </tr>

                        <tr>
                              <td>Product Pack Size</td>
                              <td><asp:TextBox ID="txtMaterialPackSize" runat="server" Width="250px" Enabled="False" ></asp:TextBox></td>
                              <%--<td>&nbsp;</td>--%>
                              <td>Product Nature</td>
                              <td><asp:TextBox ID="txtproductNature" runat="server" Width="250px" Enabled="False" /></td>
                              <%--<td>&nbsp;</td>--%>
                        </tr>

                        <tr>
                              <td>Product CRATE Pack Size</td>
                              <td><asp:TextBox ID="txtMaterialCratePackSize" runat="server" Width="250px" Enabled="False" ></asp:TextBox></td>
                              <%--<td>&nbsp;</td>--%>
                              <td>Product Sub Category</td>
                              <td><asp:TextBox ID="txtMaterialSubCatoery" runat="server" Width="250px" Enabled="False" ></asp:TextBox></td>
                        </tr>
                        <tr>
                              <td >UOM</td>
                              <td ><asp:TextBox ID="txtUOM" runat="server" Width="250px" Enabled="False" ></asp:TextBox></td>
                              <td >Flavor</td>
                              <td ><asp:TextBox ID="txtFlavor" runat="server" Width="250px" Enabled="False" ></asp:TextBox></td>
                        </tr>
                        <tr>
                              <td >Machine Category</td>
                              <td ><asp:TextBox ID="TextBox1" runat="server" Width="250px" Enabled="False" ></asp:TextBox></td>
                              <td >HSN Code</td>
                              <td ><asp:TextBox ID="txtHSNCode" runat="server" Width="250px" Enabled="False" ></asp:TextBox></td>
                        </tr>
                        <tr>
                              <td >Is Blocked</td>
                              <td ><asp:CheckBox ID="chkBlocked" runat="server" Width="250px" Enabled="False" ></asp:CheckBox></td>
                              <td >Blocked On</td>
                              <td ><asp:TextBox ID="txtBlockedOn" runat="server" Width="250px" Enabled="False" ></asp:TextBox></td>
                        </tr>
                        <tr>
                              <td >Is Exempted From GST</td>
                              <td ><asp:CheckBox ID="chkExempted" runat="server" Width="250px" Enabled="False" ></asp:CheckBox></td>
                        </tr>
                     </table>
   <%-- </div>--%>
   
</asp:Content>

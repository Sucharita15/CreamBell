<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucRadioButtonList.ascx.cs" Inherits="CreamBell_DMS_WebApps.UserControl.ucRadioButtonList" %>

<div>
    <b>Select Export Excel Type:&nbsp;</b>
    <asp:RadioButton Text="XLSB" Checked="true" runat="server" ID="rdbXLSB" GroupName="rdXLExport" ValidationGroup="rdXLExport" />
    <asp:RadioButton Text="XLSX" runat="server" ID="rdbXLSX" GroupName="rdXLExport" ValidationGroup="rdXLExport" />
    <asp:RadioButton Text="XLS" runat="server" ID="rdbXLS" GroupName="rdXLExport" ValidationGroup="rdXLExport" />
</div>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReport.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CBSAM Report</title>
    <link href="css/GhostButton.css" rel="stylesheet" />
    <style type="text/css">
        .Report 
        {
            height:auto;
            
            overflow:scroll;
        }
    </style>

    <script type="text/javascript">
      <%--function doPrint()
        {
            var prtContent = document.getElementById('<%= ReportViewer1.ClientID %>');
            prtContent.border = 0; //set no border here
            var WinPrint = window.open('', '', 'left=150,top=100,width=1000,height=1000,toolbar=0,scrollbars=1,status=0,resizable=1');
            WinPrint.document.write(prtContent.outerHTML);
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
        }--%>
   </script>

</head>
<body style=" background-color:white">

    <form id="form1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div id="print">
    <center> <asp:Button ID="BtnPrint" runat="server" Text="Print" CssClass="btn btn-medium btn-blue btn-radius"  ToolTip="Print Report" OnClick="BtnPrint_Click"/> </center>
    </div>
     
    <div id="report" style="height:auto; width:auto; vertical-align:central"">
        <center>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="780px" Height="800px" >

            </rsweb:ReportViewer>
        </center>  
    </div>

    </form>
</body>
</html>



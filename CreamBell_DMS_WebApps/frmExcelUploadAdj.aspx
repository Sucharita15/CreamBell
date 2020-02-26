<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="frmExcelUploadAdj.aspx.cs" Inherits="CreamBell_DMS_WebApps.frmExcelUploadAdj" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="css/style.css" rel="stylesheet" />
     <style type="text/css">   
         .ModalPoupBackgroundCssClass
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
    .modalPopup {
        background-color:#FFFFFF;
        border-width:2px;
        border-style:inset ; 
        width:auto;
        height:auto ;
    }
   </style>

       <script type="text/javascript">

       function uploadStart(sender, args) {
           var fileName = args.get_fileName();
           var fileExt = fileName.substring(fileName.lastIndexOf(".") + 1);

           if (fileExt == "xls" || fileExt == "xlsx") {
               return true;
           } else {
               //To cancel the upload, throw an error, it will fire OnClientUploadError
               var err = new Error();
               err.name = "Upload Error";
               err.message = "Please upload only Excel files (.xls,.xlsx)";
               throw (err);

               return false;
           }
       }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">

    <div style="width: 100%;height: 18px;border-radius: 4px;margin: 10px 0px 0px 5px;background-color: #0085ca;padding: 2px 0px 0px 0px;" >
      <span style="font-family: Segoe UI;font-size: 11px;font-weight: bold;margin: 6px 0px 0px 10px;color: #FFFFFF;"> Opening Stock Excel Upload Utility</span>
    </div>  
    <div style="width: 100%;height: 38px;border-radius: 4px;/*margin: 2px 0px 0px 5px;padding: 2px 0px 0px 0px;*/">
        <asp:Label ID="lblError" runat="server" Font-Bold="True" ></asp:Label>
        <asp:Label ID="lblEror1" runat="server" Font-Bold="True" ></asp:Label>
        <asp:Label ID="lblUpload" runat="server" Font-Bold="True" ></asp:Label>
    <div style="width: 100%;height: 18px;border-radius: 4px;">
        <table>
            <tr>
                <td>
                     <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" Visible="true" OnClientUploadStarted="uploadStart" Height="18px"  />
                </td>
                <td>
                    <asp:Button ID="btnExcelUpload" AutoPostBack="True" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" runat="server" CssClass="button" OnClick="btnExcelUpload_Click" Text="Upload" />
                </td>

                <td>
                          <asp:HyperLink ID="hypsoUpload" runat="server" Font-Size="Small" ForeColor="Blue" style="margin-left: 0px" ToolTip="Click to download excel template !!">
                                 <a href="ExcelTemplate/OpeningStock.xlsx" target="_blank">
                                 <img src="Images/DownloadTemplate.gif" alt="Download Template" style="border-style: none"  /></a> </asp:HyperLink>
                </td>
            </tr>
        </table>
    </div>
      </div>

    <%--show the popup for unuploaddata--%>

     <div>
     <asp:UpdatePanel ID="UpdatePanel3" runat="server">
             <ContentTemplate>

         <asp:Button ID="Button2" runat="Server" Text="" Style="display: none;" />
           <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button2"
                PopupControlID="Panel1" CancelControlID="Button4"
                BackgroundCssClass="ModalPoupBackgroundCssClass" BehaviorID="ModalPopupExtender1">
           </asp:ModalPopupExtender>

           <asp:Panel ID="Panel1" runat="server" style="display: none ;background-color:silver">
                     <div align="center"> <span style="color:red;font-weight:600"> Records not uploaded...</span>
                         <asp:Button ID="Button4"  runat="server" CssClass="Operationbutton" data-dismiss="modal" Text ="Close" aria-hidden="true"></asp:Button> </div>      
                       
                       <div style="overflow-x:scroll;width:700px;height:200px" >
                          <asp:GridView ID="gridviewRecordNotExist" runat="server" style="width:100%" Font-Size="Small" CssClass="Grid" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" >
                              <AlternatingRowStyle CssClass="alt" />
                              <FooterStyle BackColor="White" ForeColor="#000066" />
                              <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                              <PagerStyle BackColor="White" CssClass="pgr" ForeColor="#000066" HorizontalAlign="Left" />
                              <RowStyle ForeColor="#000066" />
                              <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                              <SortedAscendingCellStyle BackColor="#F1F1F1" />
                              <SortedAscendingHeaderStyle BackColor="#007DBB" />
                              <SortedDescendingCellStyle BackColor="#CAC9C9" />
                              <SortedDescendingHeaderStyle BackColor="#00547E" />
                            </asp:GridView>
                    </div>
                   </asp:Panel>
                </ContentTemplate>       
            </asp:UpdatePanel>
        </div>

</asp:Content>

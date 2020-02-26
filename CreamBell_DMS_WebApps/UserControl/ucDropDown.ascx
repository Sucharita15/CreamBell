<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDropDown.ascx.cs" Inherits="CreamBell_DMS_WebApps.UserControl.ucDropDown" %>

<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>

<script type="text/javascript" src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
<link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
<script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>

<script type="text/javascript">
    $(function () {
        
        $('#lstControl').multiselect({
            includeSelectAllOption: true,
            nonSelectedText: '--Select--',
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            buttonWidth: '140px',
            maxHeight: 300,
            maxWidth: 50
        });
    });
</script>

<asp:ListBox ID="lstControl" runat="server" SelectionMode="Multiple" ClientIDMode="Static" Width="200px"></asp:ListBox>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="MITD.Main.Service.Host.Reports.ReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reports Viewer</title>
</head>
<body>
    <form id="MainForm" runat="server">
        <asp:Label ID="lblExpiredSessionTitle" runat="server" Text="Your session has expired." ForeColor="Red" Font-Bold="True" Font-Size="20pt" Visible="False" />
        <asp:Label ID="lblExpiredSessionDescription" runat="server" Text="Your session has expired. <br/>Please close this page sign-out and re-login from application main page." ForeColor="Black" Font-Bold="True" Font-Size="15pt" Visible="False" />
        <asp:Label ID="lblUnauthorizedAccess" runat="server" Text="Unauthorized Access." ForeColor="Red" Font-Bold="True" Font-Size="20pt" Visible="False" />
        <rsweb:ReportViewer ID="MainReportViewer" runat="server" Width="100%"></rsweb:ReportViewer>
        <asp:ScriptManager ID="MainScriptManager" runat="server" ViewStateMode="Enabled" EnableViewState="True"></asp:ScriptManager>
        <asp:HiddenField ID="ClientSizeHeight" runat="server" />
    </form>
    <script type="text/javascript">
        //window.onresize = function () {
        //    var clientSizeHeightDIV = document.getElementById("ClientSizeHeight");
        //    clientSizeHeightDIV.value = (window.innerHeight
		//									|| document.documentElement.clientHeight
		//									|| document.body.clientHeight) - 20;

        //    recalculateReportViewerLayout("MainReportViewer");

        //    Document.form.submit();
        //}

        //window.onresize();

        function recalculateReportViewerLayout(id, heightValue) {
            var viewer = $find(id);

            if (viewer != null && !viewer.isLoading && !viewer.get_isLoading()) {

                var clientsizeheightdiv = document.getElementById("ClientSizeHeight");
                clientsizeheightdiv.value = heightValue;
                
                viewer.refreshReport();
            }
        }

        window.onresize = function () {
            var clientsizeheightdiv = document.getElementById("ClientSizeHeight");
            var heightValue = (window.innerHeight
											|| document.documentElement.clientHeight
											|| document.body.clientHeight) - 20;

            if (clientsizeheightdiv.value != heightValue) {
                recalculateReportViewerLayout("MainReportViewer", heightValue);
            }
        }

        window.onresize();

        setInterval(function () { window.onresize(); }, 2000);

        // <asp:ScriptReference Path="ClientCode.js" />
    </script>
</body>
</html>

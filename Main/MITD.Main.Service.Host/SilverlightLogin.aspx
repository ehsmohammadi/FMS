<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SilverlightLogin.aspx.cs" Inherits="MITD.Main.Service.Host.SilverlightLogin" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body onload="setTimeout('window.parent.hidDivLogin()',2000)">
    <form id="form1" runat="server">
    <div>
    <p>تا چند لحظه دیگر به سایت باز خواهید گشت ...</p>
        <p>
            
            <input type="button" value="بازگشت" onclick="window.parent.hidDivLogin()"/>
        </p>
    </div>
    </form>
</body>
</html>

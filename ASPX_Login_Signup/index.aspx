<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ASPX_Login_Signup.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    
    <div id="divBody"></div>
        <asp:Label ID="txtLabel" runat="server" Text="Welcome"></asp:Label>
        <br />
        <br />
        <br />
        <asp:Label ID="usrLabel" Visible="true" runat="server" Text="User Name:"></asp:Label>
&nbsp;<asp:TextBox ID="txtUserName" Visible="true" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="pswLabel" Visible="true" runat="server" Text="Password:"></asp:Label>
        &nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtPassword" Visible="true" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="btnSignup" Visible="true" runat="server" Text="Signup" onclick="btnSignup_Click" />
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnLogin" Visible="true" runat="server" Text="Login" OnClick="btnLogin_Click" />

        

    &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnLogout" OnClick="btnLogout_Click" Visible="false" runat="server" Text="Logout" />

        

    </form>
</body>
</html>

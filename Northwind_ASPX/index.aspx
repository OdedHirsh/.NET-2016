<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Northwind_ASPX.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <br />
        <asp:Label ID="lblMSG" runat="server" Text="Select a Customer"></asp:Label>
        <br />
        <asp:SqlDataSource ID="SqlDataSourceOrders" runat="server" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" SelectCommand="SELECT [OrderID], [OrderDate], [Freight] FROM [Orders] WHERE ([CustomerID] = @CustomerID or @CustomerID = ' All' )">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddListClient" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceCostemers" runat="server" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" SelectCommand="SELECT ' All' AS CustomerID, ' All' AS CompanyName
UNION
SELECT [CustomerID], [CompanyName] FROM [Customers]"></asp:SqlDataSource>
        <br />
        <asp:DropDownList ID="ddListClient" runat="server" DataSourceID="SqlDataSourceCostemers" DataTextField="CompanyName" DataValueField="CustomerID" OnSelectedIndexChanged="ddListClient_SelectedIndexChanged">
        </asp:DropDownList>
&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnChoose" runat="server" OnClick="btnChoose_Click" Text="Choose" />
        <br />
        <br />
        <asp:GridView ID="GridViewOrders" runat="server" AutoGenerateColumns="False" DataKeyNames="OrderID" DataSourceID="SqlDataSourceOrders">
            <Columns>
                <asp:BoundField DataField="OrderID" HeaderText="OrderID" InsertVisible="False" ReadOnly="True" SortExpression="OrderID" />
                <asp:BoundField DataField="OrderDate" HeaderText="OrderDate" SortExpression="OrderDate" />
                <asp:BoundField DataField="Freight" HeaderText="Freight" SortExpression="Freight" />
            </Columns>
        </asp:GridView>
    
    </div>
        <br />
        <br />
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </form>
</body>
</html>

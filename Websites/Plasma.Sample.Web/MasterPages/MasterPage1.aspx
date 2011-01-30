<%@ Page Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" Inherits="MasterPages_MasterPage1" Title="Untitled Page" Codebehind="MasterPage1.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><br />
    <br />
    <asp:DropDownList ID="DropDownList1" runat="server">
        <asp:ListItem Value="Fee"></asp:ListItem>
        <asp:ListItem Value="Fi"></asp:ListItem>
        <asp:ListItem Value="Foo"></asp:ListItem>
        <asp:ListItem Value="Fum"></asp:ListItem>
    </asp:DropDownList><br />
    <br />
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" /><br />
    <br />
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />




</asp:Content>


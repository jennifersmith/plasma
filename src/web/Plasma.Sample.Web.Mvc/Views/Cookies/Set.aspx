<%@ Page Title="SetCookie" Language="C#" Inherits="System.Web.Mvc.ViewPage<object>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">SetCookie</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <% HttpContext.Current.Response.Cookies.Add(new HttpCookie("Test", "Cookie Set By Host")); %>
</asp:Content>
    
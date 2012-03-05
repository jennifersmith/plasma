<%@ Page Title="SetManyCookies" Language="C#" Inherits="System.Web.Mvc.ViewPage<object>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">SetManyCookies</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <% HttpContext.Current.Response.Cookies.Add(new HttpCookie("Test1", "Cookie Set By Host")); %>
    <% HttpContext.Current.Response.Cookies.Add(new HttpCookie("Test2", "Cookie Set By Host")); %>
</asp:Content>

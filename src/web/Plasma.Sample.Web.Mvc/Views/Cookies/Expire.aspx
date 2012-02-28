<%@ Page Title="CookiesExpire" Language="C#" Inherits="System.Web.Mvc.ViewPage<object>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">CookiesExpire</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <%
        var httpCookie = new HttpCookie("Test", string.Empty);
        httpCookie.Expires = DateTime.Now.AddDays(-10);
        HttpContext.Current.Response.Cookies.Set(httpCookie); 
    %>
</asp:Content>

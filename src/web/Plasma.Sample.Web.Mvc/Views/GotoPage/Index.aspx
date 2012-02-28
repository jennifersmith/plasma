<%@ Page Title="GotoPage" Language="C#" Inherits="System.Web.Mvc.ViewPage<object>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">GotoPage</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <%= Html.ActionLink("OtherPage", "OtherPage") %>
</asp:Content>

<%@ Page Title="FormWithTextBox" Language="C#" Inherits="System.Web.Mvc.ViewPage<object>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">FormWithTextBox</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <% using(Html.BeginForm("Show", "FormWithTextBox", FormMethod.Post)) { %>
        <%= Html.TextBox("textBox") %>
    <% } %>
    <p id="textBoxValue"><%= ViewData["value"] %></p>
</asp:Content>

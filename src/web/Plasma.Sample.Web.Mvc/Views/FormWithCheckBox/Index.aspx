<%@ Page Title="FormWithCheckBox" Language="C#" Inherits="System.Web.Mvc.ViewPage<object>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">FormWithCheckBox</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <% using(Html.BeginForm("Show", "FormWithCheckBox", FormMethod.Post)) { %>
        <%= Html.CheckBox("checkBox") %>
    <% } %>
    <p id="checkBoxValue"><%= ViewData["value"] %></p>
</asp:Content>

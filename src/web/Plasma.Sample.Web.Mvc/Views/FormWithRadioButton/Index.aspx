<%@ Page Title="FormWithRadioButton" Language="C#" Inherits="System.Web.Mvc.ViewPage<object>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">FormWithRadioButton</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
        <% using(Html.BeginForm("Show", "FormWithRadioButton", FormMethod.Post)) { %>
        <input type="radio" id="radioButton1" name="radioButton" value="One"/>
        <input type="radio" id="radioButton2" name="radioButton" value="Two"/>
        <input type="radio" id="radioButton3" name="radioButton" value="Three"/>
    <% } %>
    <p id="radioButtonValue"><%= ViewData["value"] %></p>
</asp:Content>
        
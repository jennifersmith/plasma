<%@ Page Title="Html5Elements" Language="C#" Inherits="System.Web.Mvc.ViewPage<object>"
    MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">
    Html5Elements</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <% using (Html.BeginForm("Show", "Html5Elements", FormMethod.Post))
       { %>
    <input id="emailBox" name="emailBox" type="email">
    <input id="numberBox" name="numberBox" type="number">
    <input id="weekBox" name="weekBox" type="week">
    <input id="monthBox" name="monthBox" type="month">
    <input id="dateBox" name="dateBox" type="date">
    <input id="timeBox" name="timeBox" type="time">
    <input id="datetimeBox" name="datetimeBox" type="datetime">
    <input id="rangeBox" name="rangeBox" type="range">
    <input id="telBox" name="telBox" type="tel">
    <input id="urlBox" name="urlBox" type="url">
    <% } %>
    <p id="emailBoxValue"><%= ViewData["emailBoxValue"]%></p>
    <p id="numberBoxValue"><%= ViewData["numberBoxValue"]%></p>
    <p id="weekBoxValue"><%= ViewData["weekBoxValue"]%></p>
    <p id="monthBoxValue"><%= ViewData["monthBoxValue"]%></p>
    <p id="dateBoxValue"><%= ViewData["dateBoxValue"]%></p>
    <p id="timeBoxValue"><%= ViewData["timeBoxValue"]%></p>
    <p id="datetimeBoxValue"><%= ViewData["datetimeBoxValue"]%></p>
    <p id="rangeBoxValue"><%= ViewData["rangeBoxValue"]%></p>
    <p id="telBoxValue"><%= ViewData["telBoxValue"]%></p>
    <p id="urlBoxValue"><%= ViewData["urlBoxValue"]%></p>
</asp:Content>

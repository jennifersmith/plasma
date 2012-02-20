<%@ Page Title="FindElementsByXPath" Language="C#" Inherits="System.Web.Mvc.ViewPage<object>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">FindElementsByXPath</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <div class="className">
        Found By XPath
    </div>

    <div class="outerClassName">
        <div class="innerClassName">Found By XPath</div>
    </div>

    <div>
        <p class="classNameThatsUsedManyTimes">Found By XPath</p>
        <p class="classNameThatsUsedManyTimes">Found By XPath</p>
        <p class="classNameThatsUsedManyTimes">Found By XPath</p>
        <p class="classNameThatsUsedManyTimes">Found By XPath</p>
    </div>
</asp:Content>

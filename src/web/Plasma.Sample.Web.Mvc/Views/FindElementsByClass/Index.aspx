<%@ Page Title="FindElementsByClass" Language="C#" Inherits="System.Web.Mvc.ViewPage<object>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">FindElementsByClass</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <div class="className">
        Found By class : className
    </div>

    <div class="outerClassName">
        <div class="innerClassName">Found By class : innerClassName</div>
    </div>

    <div>
        <p class="classNameThatsUsedManyTimes">Found By class : classNameThatsUsedManyTimes</p>
        <p class="classNameThatsUsedManyTimes">Found By class : classNameThatsUsedManyTimes</p>
        <p class="classNameThatsUsedManyTimes">Found By class : classNameThatsUsedManyTimes</p>
        <p class="classNameThatsUsedManyTimes">Found By class : classNameThatsUsedManyTimes</p>
    </div>

    <div class="firstClass secondClass">Found by class: firstClass or secondClass</div>
</asp:Content>

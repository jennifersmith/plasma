<%@ Page Title="FindElementsByName" Language="C#" Inherits="System.Web.Mvc.ViewPage<object>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">FindElementsByName</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <input name="name" value="Found By Name : name" />

    <div name="outerName">
        <div name="innerName">Found By Name : innerClassName</div>
    </div>

    <div>
        <input name="nameThatsUsedManyTimes" value="Found By Name : nameThatsUsedManyTimes" />
        <input name="nameThatsUsedManyTimes" value="Found By Name : nameThatsUsedManyTimes" />
        <input name="nameThatsUsedManyTimes" value="Found By Name : nameThatsUsedManyTimes" />
        <input name="nameThatsUsedManyTimes" value="Found By Name : nameThatsUsedManyTimes" />
    </div>

    <div name="firstName secondName">Found by Name: firstClass or secondClass</div>
</asp:Content>

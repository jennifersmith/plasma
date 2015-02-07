<%@ Page Title="Title" Language="C#" Inherits="System.Web.Mvc.ViewPage<Plasma.Sample.Web.Mvc.Controllers.PostResponseModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent"></asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    
    Post page. You posted: <% Html.Raw(Model.Value); %>

</asp:Content>

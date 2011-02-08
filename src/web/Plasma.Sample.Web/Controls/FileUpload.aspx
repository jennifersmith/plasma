<%@ Page Language="C#" AutoEventWireup="true" Trace="false" Inherits="Controls_FileUpload" Codebehind="FileUpload.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:FileUpload ID="FileUpload1" runat="server" /><br />
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" 
         Text="Upload File" />&nbsp;<br />
        <br />
        <%if (FileUpload1.HasFile) {%>
            <p>File name: <asp:Label ID="FileName" runat="server"></asp:Label></p>
            <p>Content length: <asp:Label ID="ContentLength" runat="server"></asp:Label> kb</p>
            <p>Content type: <asp:Label ID="ContentType" runat="server"></asp:Label></p>
            <p>Saved to: <asp:Label ID="SavedTo" runat="server"></asp:Label></p>
        <%
          } %>
        </div>
    </form>
</body>
</html>

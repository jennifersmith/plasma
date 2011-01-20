
Partial Class Controls_LinkButton
    Inherits System.Web.UI.Page

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        Label1.Text = "LinkButton Pushed!"
    End Sub
End Class

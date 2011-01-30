using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

public partial class Basic_Redirect : Page
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Response.Redirect("querystring.aspx");
    }
}
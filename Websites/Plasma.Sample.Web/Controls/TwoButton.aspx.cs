using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Plasma.Sample.Web.Controls
{
    public partial class TwoButtons : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Button1_Click(object sender, EventArgs e)
        {
            Label1.Text = "Value: " + TextBox1.Text;
        }
        public void Button2_Click(object sender, EventArgs e)
        {
            Label1.CssClass = "Selected";
        }
    }
}
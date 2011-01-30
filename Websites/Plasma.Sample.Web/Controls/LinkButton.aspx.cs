using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

    public partial class Controls_LinkButton : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LinkButton1.Click += new EventHandler(LinkButton1_Click);
        }

        void LinkButton1_Click(object sender, EventArgs e)
        {
            Label1.Text = "LinkButton Pushed!";
        }
    }

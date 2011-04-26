/* **********************************************************************************
 *
 * Copyright (c) Microsoft Corporation. All rights reserved.
 *
 * Portions Copyright 2010 ThoughtWorks, Inc.
 * ThoughtWorks provides the software "as is" without warranty of any kind, either express or implied, including but not limited to, 
 * the implied warranties of merchantability, satisfactory quality, non-infringement and fitness for a particular purpose.
 *
 * This source code is subject to terms and conditions of the Microsoft Permissive
 * License (MS-PL).
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
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
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plasma.Core;

namespace Plasma.Samples.MSTestLibrary.Basic
{
    [TestClass]
    public class Redirect
    {
        [TestMethod]
        public void Basic_Redirect()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test Verifying a Redirect on Redirect.aspx

            AspNetResponse response = WebApp.ProcessRequest("~/Basic/Redirect.aspx");
            Assert.AreEqual(response.Status, 302);
        }
    }
}

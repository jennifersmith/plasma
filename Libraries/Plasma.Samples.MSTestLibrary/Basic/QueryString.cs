using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plasma.Core;

namespace Plasma.Samples.MSTestLibrary.Basic
{
    [TestClass]
    public class QueryString
    {
        [TestMethod]
        public void Basic_QueryString()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test passing a QueryString value to QueryString.aspx

            AspNetResponse response = WebApp.ProcessRequest("~/Basic/Querystring.aspx?test=Hello");
            string message = response.FindHtmlElementById("Label1").InnerHtml;
            Assert.AreEqual(message, "Hello", "QueryString Value Not Output Back Correctly");
        }
    }
}

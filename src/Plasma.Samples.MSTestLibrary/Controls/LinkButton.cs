using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Plasma.Core;

namespace Plasma.Samples.MSTestLibrary.Controls
{
    [TestClass]
    public class LinkButton
    {
        [TestMethod]
        public void LinkButton_Test()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing a LinkButton on LinkButton.aspx

            AspNetResponse firstResponse = WebApp.ProcessRequest("~/Controls/LinkButton.aspx");

            AspNetForm form = firstResponse.GetForm();
            form["__EVENTTARGET"] = "LinkButton1";

            AspNetResponse secondResponse = WebApp.ProcessRequest(form.GenerateFormPostRequest());
            string label1 = secondResponse.FindElement(By.Id("Label1")).InnerHtml();

            Assert.AreEqual(label1, "LinkButton Pushed!");                
        }
    }
}
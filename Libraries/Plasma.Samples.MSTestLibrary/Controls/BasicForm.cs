using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Plasma.Core;

namespace Plasma.Samples.MSTestLibrary.Controls
{
    [TestClass]
    public class BasicForm
    {
        [TestMethod]
        public void Basic_Form()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing a Button on BasicForm.aspx

            AspNetResponse firstResponse = WebApp.ProcessRequest("~/Controls/BasicForm.aspx");

            AspNetForm form = firstResponse.GetForm();
            form["TextBox1"] = "Testing";
            form["Button1"] = "Button";
            
            AspNetResponse secondResponse = WebApp.ProcessRequest(form.GenerateFormPostRequest());
            string label1 = secondResponse.FindElement(By.Id("Label1")).InnerHtml();

            Assert.AreEqual(label1, "Value: Testing");
        }
    }
}
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Plasma.Core;

namespace Plasma.Samples.MSTestLibrary.Controls
{
    [TestClass]
    public class TwoButtons
    {
        [TestMethod]
        public void TwoButtons_Test()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing Button1 on TwoButton.aspx

            AspNetResponse firstResponse = WebApp.ProcessRequest("~/Controls/TwoButton.aspx");

            AspNetForm form = firstResponse.GetForm();
            form["TextBox1"] = "Testing";
            form["Button1"] = "Button1";

            AspNetResponse secondResponse = WebApp.ProcessRequest(form.GenerateFormPostRequest());
            string label1 = secondResponse.InnerHtml(secondResponse.FindElement(By.Id("Label1")));

            Assert.AreEqual(label1, "Value: Testing");


            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing Button2 on TwoButton.aspx

            form = secondResponse.GetForm();
            form["Button2"] = "Button2";

            AspNetResponse thirdResponse = WebApp.ProcessRequest(form.GenerateFormPostRequest());
            string cssName = thirdResponse.FindElement(By.Id("Label1")).GetAttribute("class");

            Assert.AreEqual("Selected", cssName);
        }
    }
}
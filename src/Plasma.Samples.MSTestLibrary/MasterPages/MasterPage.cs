using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Plasma.Core;

namespace Plasma.Samples.MSTestLibrary.MasterPages
{
    [TestClass]
    public class MasterPage
    {
        [TestMethod]
        public void MasterPage_Form()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing a Button with a DropDownList within a MasterPage

            AspNetResponse firstResponse = WebApp.ProcessRequest("~/MasterPages/MasterPage1.aspx");

            AspNetForm form = firstResponse.GetForm();
            form["ctl00$ContentPlaceHolder1$TextBox1"] = "Scott";
            form["ctl00$ContentPlaceHolder1$DropDownList1"] = "Foo";
            form["ctl00$ContentPlaceHolder1$Button1"] = "Button";

            AspNetResponse secondResponse = WebApp.ProcessRequest(form.GenerateFormPostRequest());
            string label1 = secondResponse.FindElement(By.Id("ctl00_ContentPlaceHolder1_Label1")).InnerHtml();

            Assert.AreEqual("Hello Scott you selected: Foo", label1);
        }
    }
}
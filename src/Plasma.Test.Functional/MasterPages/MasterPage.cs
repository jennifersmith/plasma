/* **********************************************************************************
 *
 * Copyright (c) Microsoft Corporation. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Permissive
 * License (MS-PL).
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
using NUnit.Framework;
using OpenQA.Selenium;
using Plasma.WebDriver;

namespace Plasma.Test.Functional.MasterPages
{
    [TestFixture]
    public class MasterPage
    {
        [Test, Ignore]
        public void MasterPage_Form()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing a Button with a DropDownList within a MasterPage
            var driver = new PlasmaDriver(WebApplicationFixture.AppInstance);
            driver.Navigate().GoToUrl("~/MasterPages/MasterPage1.aspx");


            driver.FindElement(By.Name("ctl00$ContentPlaceHolder1$TextBox1")).SendKeys("Scott");

            driver.FindElement(By.Name("ctl00$ContentPlaceHolder1$DropDownList1")).SendKeys("Foo");

            driver.FindElement(By.Name("ctl00$ContentPlaceHolder1$Button1")).Click();
            
//            HtmlNavigator secondHtml = WebApplicationFixture.ProcessRequest(Button.Click(form, "ctl00$ContentPlaceHolder1$Button1")).Html();
//            Assert.AreEqual("Hello Scott you selected: Foo", secondHtml.FindElement(By.Id("ctl00_ContentPlaceHolder1_Label1")).InnerHtml());

            Assert.AreEqual("Hello Scott you selected: Foo", driver.FindElement(By.Id("ContentPlaceHolder1_Label1")).Text);
        }
    }
}
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

namespace Plasma.Test.Functional.Controls
{
    [TestFixture]
    public class LinkButtonTest
    {
        [Test, Ignore("This is an ASPNET Forms test that is not supported with this version of Plasma")]
        public void LinkButton_Test()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing a LinkButton on LinkButton.aspx

            var plasmaDriver = new PlasmaDriver(WebApplicationFixture.AppInstance);

            plasmaDriver.Navigate().GoToUrl("~/Controls/LinkButton.aspx");

            plasmaDriver.FindElement(By.Id("LinkButton1")).Click();
//            HtmlNavigator secondHtml = WebApplicationFixture.ProcessRequest(LinkButton.Click(form, "LinkButton1")).Html();

            Assert.AreEqual("LinkButton Pushed!", plasmaDriver.FindElement(By.Id("Label1")).InnerHtml());                
        }
    }
}
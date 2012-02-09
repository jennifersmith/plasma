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
    public class BasicForm
    {
        [Test]
        public void Basic_Form()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing a Button on BasicForm.aspx

            var plasmaDriver = new PlasmaDriver(WebApplicationFixture.AppInstance);

            plasmaDriver.Navigate().GoToUrl("~/Controls/BasicForm.aspx");

            plasmaDriver.FindElement(By.Id("TextBox1")).SendKeys("Testing");
            plasmaDriver.FindElement(By.Id("Button1")).Click();

            Assert.AreEqual("Value: Testing", plasmaDriver.FindElement(By.Id("Label1")).Text, plasmaDriver.PageSource);
        }
    }
}
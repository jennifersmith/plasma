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
    public class TwoButtons
    {
        [Test, Ignore]
        public void TwoButtons_Test()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Initial request for TwoButton.aspx page

            var plasmaDriver = new PlasmaDriver(WebApplicationFixture.AppInstance);
            plasmaDriver.Navigate().GoToUrl("~/Controls/TwoButton.aspx");

            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing Button1 with value in TextBox

            plasmaDriver.FindElement(By.Id("TextBox1")).SendKeys("Testing");
            plasmaDriver.FindElement(By.Id("Button1")).Click();

            Assert.AreEqual("Value: Testing", plasmaDriver.FindElement(By.Id("Label1")).Text);

            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing Button2 on TwoButton.aspx

            plasmaDriver.FindElement(By.Id("Button2")).Click();

            Assert.AreEqual("Selected", plasmaDriver.FindElement(By.Id("Label1")).GetAttribute("class"));
        }
    }
}

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

namespace Plasma.Test.Functional.Caching
{
    [TestFixture]
    public class OutputCache
    {
        [Test]
        public void TimestampCheck()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test OutputCaching

            var driver = new PlasmaDriver(WebApplicationFixture.AppInstance);

            driver.Navigate().GoToUrl("~/Caching/OutputCache.aspx");
            string timestamp1 = driver.FindElement(By.Id("Label1")).InnerHtml();

            driver.Navigate().GoToUrl("~/Caching/OutputCache.aspx");
            string timestamp2 = driver.FindElement(By.Id("Label1")).InnerHtml();

            Assert.AreEqual(timestamp1, timestamp2);
        }
    }
}


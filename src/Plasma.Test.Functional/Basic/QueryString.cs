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

namespace Plasma.Test.Functional.Basic
{
    [TestFixture]
    public class QueryString
    {
        [Test]
        public void Basic_QueryString()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test passing a QueryString value to QueryString.aspx

            var driver = new PlasmaDriver(WebApplicationFixture.AppInstance);
            driver.Navigate().GoToUrl("~/Basic/Querystring.aspx?test=Hello");

            var message = driver.FindElement(By.Id("Label1")).InnerHtml();

            Assert.AreEqual("Hello", message);
        }
    }
}

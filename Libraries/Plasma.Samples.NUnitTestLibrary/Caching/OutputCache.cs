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
using System;
using NUnit.Framework;
using OpenQA.Selenium;
using Plasma.Core;

namespace Plasma.Samples.NUnitTestLibrary.Caching
{
    [TestFixture]
    public class OutputCache
    {
        [Test]
        public void TimestampCheck()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test OutputCaching

            AspNetResponse responseFirst = WebApp.ProcessRequest("~/Caching/OutputCache.aspx");
            string timestamp1 = responseFirst.FindElement(By.Id("Label1")).InnerHtml();

            AspNetResponse responseSecond = WebApp.ProcessRequest("~/Caching/OutputCache.aspx");
            string timestamp2 = responseSecond.FindElement(By.Id("Label1")).InnerHtml();

            Assert.AreEqual(timestamp1, timestamp2);
        }
    }
}
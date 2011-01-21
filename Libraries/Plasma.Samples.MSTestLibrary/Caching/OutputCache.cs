using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Plasma.Core;

namespace Plasma.Samples.MSTestLibrary.Caching
{
    [TestClass]
    public class OutputCache
    {
        [TestMethod]
        public void TimestampCheck()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test OutputCaching

            AspNetResponse responseFirst = WebApp.ProcessRequest("~/Caching/OutputCache.aspx");
            string timestamp1 = responseFirst.InnerHtml(responseFirst.FindElement(By.Id("Label1")));

            AspNetResponse responseSecond = WebApp.ProcessRequest("~/Caching/OutputCache.aspx");
            string timestamp2 = responseSecond.InnerHtml(responseSecond.FindElement(By.Id("Label1")));

            Assert.AreEqual(timestamp1, timestamp2);
        }
    }
}
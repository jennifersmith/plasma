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

namespace Plasma.Core.Test.Controls
{
    [TestFixture]
    public class LinkButtonTest
    {
        [Test]
        public void LinkButton_Test()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing a LinkButton on LinkButton.aspx

            HtmlNavigator firstHtml = WebApplicationFixture.ProcessRequest("~/Controls/LinkButton.aspx").Html();

            AspNetForm form = firstHtml.GetForm();

            HtmlNavigator secondHtml = WebApplicationFixture.ProcessRequest(LinkButton.Click(form, "LinkButton1")).Html();

            Assert.AreEqual("LinkButton Pushed!", secondHtml.FindElement(By.Id("Label1")).InnerHtml());                
        }
    }
}
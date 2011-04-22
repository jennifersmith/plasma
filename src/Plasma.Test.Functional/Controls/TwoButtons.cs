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
        [Test]
        public void TwoButtons_Test()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Initial request for TwoButton.aspx page

            HtmlNavigator firstHtml = WebApplicationFixture.ProcessRequest("~/Controls/TwoButton.aspx").Html();


            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing Button1 with value in TextBox

            AspNetForm form = firstHtml.GetForm();
            form["TextBox1"] = "Testing";

            HtmlNavigator secondHtml = WebApplicationFixture.ProcessRequest(Button.Click(form, "Button1")).Html();

            Assert.AreEqual("Value: Testing", secondHtml.FindElement(By.Id("Label1")).InnerHtml());


            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing Button2 on TwoButton.aspx

            form = secondHtml.GetForm();

            HtmlNavigator thirdHtml = WebApplicationFixture.ProcessRequest(Button.Click(form, "Button2")).Html();

            Assert.AreEqual("Selected", thirdHtml.FindElement(By.Id("Label1")).GetAttribute("class"));
        }
    }
}

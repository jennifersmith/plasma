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

            HtmlNavigator firstHtml = WebApplicationFixture.ProcessRequest("~/Controls/BasicForm.aspx").Html();

            AspNetForm form = firstHtml.GetForm();
            form["TextBox1"] = "Testing";
            
            HtmlNavigator secondHtml = WebApplicationFixture.ProcessRequest(Button.Click(form, "Button1")).Html();

            Assert.AreEqual("Value: Testing", secondHtml.FindElement(By.Id("Label1")).InnerHtml());
        }
    }
}
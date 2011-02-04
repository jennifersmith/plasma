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

namespace Plasma.Core.Test.Controls
{
    [TestFixture]
    public class BasicForm
    {
        [Test]
        public void Basic_Form()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing a Button on BasicForm.aspx

            AspNetResponse firstResponse = WebApplicationFixture.ProcessRequest("~/Controls/BasicForm.aspx");

            AspNetForm form = firstResponse.GetForm();
            form["TextBox1"] = "Testing";
            
            AspNetResponse secondResponse = WebApplicationFixture.ProcessRequest(Button.Click(form, "Button1"));

            Assert.AreEqual("Value: Testing", secondResponse.FindElement(By.Id("Label1")).InnerHtml());
        }
    }
}
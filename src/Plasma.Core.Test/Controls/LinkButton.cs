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
    public class LinkButtonTest
    {
        [Test]
        public void LinkButton_Test()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing a LinkButton on LinkButton.aspx

            AspNetResponse firstResponse = WebApplicationFixture.ProcessRequest("~/Controls/LinkButton.aspx");

            AspNetForm form = firstResponse.GetForm();

            AspNetResponse secondResponse = WebApplicationFixture.ProcessRequest(LinkButton.Click(form, "LinkButton1"));

            Assert.AreEqual("LinkButton Pushed!", secondResponse.FindElement(By.Id("Label1")).InnerHtml());                
        }
    }
}
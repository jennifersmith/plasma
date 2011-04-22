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

namespace Plasma.Test.Functional.MasterPages
{
    [TestFixture]
    public class MasterPage
    {
        [Test]
        public void MasterPage_Form()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing a Button with a DropDownList within a MasterPage

            HtmlNavigator firstHtml = WebApplicationFixture.ProcessRequest("~/MasterPages/MasterPage1.aspx").Html();

            AspNetForm form = firstHtml.GetForm();
            form["ctl00$ContentPlaceHolder1$TextBox1"] = "Scott";
            form["ctl00$ContentPlaceHolder1$DropDownList1"] = "Foo";

            HtmlNavigator secondHtml = WebApplicationFixture.ProcessRequest(Button.Click(form, "ctl00$ContentPlaceHolder1$Button1")).Html();

            Assert.AreEqual("Hello Scott you selected: Foo", secondHtml.FindElement(By.Id("ctl00_ContentPlaceHolder1_Label1")).InnerHtml());
        }
    }
}
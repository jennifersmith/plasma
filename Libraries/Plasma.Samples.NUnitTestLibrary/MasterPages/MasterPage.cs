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
using Plasma.Core;

namespace Plasma.Samples.NUnitTestLibrary.MasterPages
{
    [TestFixture]
    public class MasterPage
    {
        [Test]
        public void MasterPage_Form()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test Pushing a Button with a DropDownList within a MasterPage

            AspNetResponse firstResponse = WebApp.ProcessRequest("~/MasterPages/MasterPage1.aspx");

            AspNetForm form = firstResponse.GetForm();
            form["ctl00$ContentPlaceHolder1$TextBox1"] = "Scott";
            form["ctl00$ContentPlaceHolder1$DropDownList1"] = "Foo";

            AspNetResponse secondResponse = WebApp.ProcessRequest(Button.Click(form, "ctl00$ContentPlaceHolder1$Button1"));

            Assert.AreEqual("Hello Scott you selected: Foo", secondResponse.FindHtmlElementById("ctl00_ContentPlaceHolder1_Label1").InnerHtml);
        }
    }
}
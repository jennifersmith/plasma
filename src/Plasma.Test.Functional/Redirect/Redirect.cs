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
using Plasma.Core;
using Plasma.WebDriver;

namespace Plasma.Test.Functional.Redirect
{
    [TestFixture]
    public class Redirect
    {
        [Test]
        public void Basic_Redirect()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test Verifying a Redirect on Redirect.aspx

            AspNetResponse aspNetResponse = WebApplicationFixture.ProcessRequest("~/Basic/Redirect.aspx");
            HtmlNavigator html = aspNetResponse.Html();

            Assert.AreEqual(aspNetResponse.Status, 302);
        }
    }
}

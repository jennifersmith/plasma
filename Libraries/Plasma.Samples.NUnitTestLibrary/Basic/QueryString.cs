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

namespace Plasma.Samples.NUnitTestLibrary.Basic
{
    [TestFixture]
    public class QueryString
    {
        [Test]
        public void Basic_QueryString()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test passing a QueryString value to QueryString.aspx

            AspNetResponse response = WebApp.ProcessRequest("~/Basic/Querystring.aspx?test=Hello");

            string message = response.FindHtmlElementById("Label1").InnerHtml;

            Assert.AreEqual("Hello", message);
        }
    }
}

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
    public class Redirect
    {
        [Test]
        public void Basic_Redirect()
        {
            /////////////////////////////////////////////////////////////////////////////
            // Test Verifying a Redirect on Redirect.aspx

            AspNetResponse response = WebApplicationFixture.ProcessRequest("~/Basic/Redirect.aspx");

            Assert.AreEqual(response.Status, 302);
        }
    }
}

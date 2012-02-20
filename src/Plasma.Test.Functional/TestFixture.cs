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
using System.IO;
using NUnit.Framework;
using Plasma.Core;
using Plasma.WebDriver;

namespace Plasma.Test.Functional
{
    [SetUpFixture]
    public class TestFixture
    {
        private static AspNetApplication _appInstance;
        private static PlasmaDriver _driver;

        public static PlasmaDriver Driver 
        {
            get
            {
                return _driver;
            }
        }

        [SetUp]
        public void SetUp() 
        {
            _appInstance = new AspNetApplication("/", Path.GetFullPath(@".\..\..\..\web\Plasma.Sample.Web.Mvc"));
            _driver = new PlasmaDriver(_appInstance);
        }

        [TearDown]
        public void TearDown() 
        {
            _appInstance.Close();
        }
    }
}

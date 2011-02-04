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

namespace Plasma.Core.Test
{
    [SetUpFixture]
    public class WebApplicationFixture
    {
        static AspNetApplication _appInstance;

        public static AspNetApplication AppInstance 
        {
            get
            {
                return _appInstance;
            }
        }

        [SetUp]
        public void SetUp() 
        {
            _appInstance = new AspNetApplication("/", Path.GetFullPath(@".\..\..\..\web\Plasma.Sample.Web"));
        }

        [TearDown]
        public void TearDown() 
        {
            _appInstance.Close();
        }

        public static AspNetResponse ProcessRequest(string url)
        {
            return AppInstance.ProcessRequest(url);
        }

        public static AspNetResponse ProcessRequest(AspNetRequest request)
        {
            return AppInstance.ProcessRequest(request);
        }
    }
}

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
using Plasma.Core;

namespace Plasma.Samples.NUnitTestLibrary
{
    public class WebApp
    {
        static AspNetApplication _appInstance;

        public static AspNetApplication AppInstance {

            get
            {
                // Todo: Make this configurable from a configuration file

                if (_appInstance == null)
                    _appInstance = new AspNetApplication("/", @"C:\Users\Steve\Documents\VS2005\PlasmaRoot\Plasma_v1.0.07015.0\Websites\PlasmaSampleCS");

                return _appInstance;
            }
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

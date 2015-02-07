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
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace Plasma.Core
{
    internal class Host : MarshalByRefObject, IRegisteredObject
    {
        private AspNetApplication _app;

        public Host()
        {
            HostingEnvironment.RegisterObject(this);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Configure(AspNetApplication app)
        {
            _app = app;
        }

        void IRegisteredObject.Stop(bool immediate)
        {
            if (_app != null)
            {
                _app.HostStopped();
            }

            HostingEnvironment.UnregisterObject(this);
        }

        public void InvokeInAspAppDomain(Action func)
        {
            func.Invoke();
        }

        public void InvokeInAspAppDomain<T>(Action<T> func, T arg)
        {
            func.Invoke(arg);
        }


        public TR InvokeInAspAppDomain<TR>(Func<TR> func)
        {
            return func.Invoke();
        }

        public TR InvokeInAspAppDomain<T, TR>(Func<T, TR> func, T arg)
        {
            return func.Invoke(arg);
        }

        public TR InvokeInAspAppDomain<T1, T2, TR>(Func<T1, T2, TR> func, T1 arg1, T2 arg2)
        {
            return func.Invoke(arg1, arg2);
        }

        internal int ProcessRequest(
            string requestFilePath,
            string requestPathInfo,
            string requestQueryString,
            string requestMethod,
            IEnumerable<KeyValuePair<string, string>> requestHeaders,
            byte[] requestBody,
            out List<KeyValuePair<string, string>> responseHeaders,
            out byte[] responseBody)
        {

            var wr = new WorkerRequest(requestFilePath, requestPathInfo,
                requestQueryString, requestMethod, requestHeaders, requestBody);

            HttpRuntime.ProcessRequest(wr);

            while (!wr.Completed)
            {
                Thread.Sleep(50);
            }

            responseHeaders = wr.ResponseHeaders;
            responseBody = wr.ResponseBody;
            return wr.ResponseStatus;
        }

        public void Close()
        {
            _app = null;
            HttpRuntime.Close();
        }
    }
}
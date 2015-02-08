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
using System.Globalization;
using System.Reflection;
using System.Web;
using System.Web.Hosting;

namespace Plasma.Core 
{
    public sealed class AspNetApplication<TTypeThatLivesInYourApplication> : AspNetApplication
    {
        public AspNetApplication() : base(typeof(TTypeThatLivesInYourApplication))
        {
        }
    }

    public class AspNetApplication : MarshalByRefObject, IRequestProcessor
    {
        private readonly IList<Assembly> _references = new List<Assembly>();

        private Host _host;
        private readonly string _virtualPath;
        private readonly string _physicalPath;
        
        public AspNetApplication(string virtualPath, string physicalPath) {
            _virtualPath = virtualPath;
            _physicalPath = physicalPath;
        }

        public AspNetApplication(string physicalPath) {
            _virtualPath = "/";
            _physicalPath = physicalPath;
        }

        public AspNetApplication(Type typeThatLivesInYourApplication)
        {
            _virtualPath = "/";
            _physicalPath = typeThatLivesInYourApplication.GetPhysicalLocation();
        }

        public void AddReference(Assembly assembly) {
            _references.Add(assembly);
        }

        public TR InvokeInAspAppDomain<T, TR>(Func<T, TR> func, T arg) {
            return GetHost().InvokeInAspAppDomain(func, arg);
        }

        public TR InvokeInAspAppDomain<T1, T2, TR>(Func<T1, T2, TR> func, T1 arg1, T2 arg2) {
            return GetHost().InvokeInAspAppDomain(func, arg1, arg2);
        }

        public void InvokeInAspAppDomain<T>(Action<T> func, T arg) {
            GetHost().InvokeInAspAppDomain(func, arg);
        }

        public void InvokeInAspAppDomain(Action func) {
            GetHost().InvokeInAspAppDomain(func);
        }

        public TR InvokeInAspAppDomain<TR>(Func<TR> func) {
            return GetHost().InvokeInAspAppDomain(func);
        }

        public override object InitializeLifetimeService() {
            return null;
        }

        public AspNetResponse ProcessRequest(AspNetRequest request) {
            List<KeyValuePair<string, string>> responseHeaders;
            string responseStatusDescrption;
            byte[] responseBody;

            string requestVirtualPath = VirtualPathUtility.ToAbsolute(request.FilePath, _virtualPath);

            int status = GetHost().ProcessRequest(requestVirtualPath, 
                request.PathInfo, 
                request.QueryString,
                request.Method, 
                request.Headers, 
                request.Body, 
                out responseHeaders,
                out responseBody, 
                out responseStatusDescrption);

            return new AspNetResponse(requestVirtualPath, request.QueryString, request.HashUri, status, responseHeaders, responseBody, responseStatusDescrption);
        }

        public AspNetResponse ProcessRequest(string requestPath) {
            return ProcessRequest(new AspNetRequest(requestPath));
        }

        private Host GetHost() {
            if (_host == null) {
                lock (this) {
                    if (_host == null) {
                        var hostInApplicationDomain = (Host)CreateWorkerAppDomainWithHost(_virtualPath, _physicalPath, typeof(Host), _references);
                        hostInApplicationDomain.Configure(this);
                        _host = hostInApplicationDomain;
                    }
                }
            }

            return _host;
        }

        private static object CreateWorkerAppDomainWithHost(string virtualPath, string physicalPath, Type hostType, IEnumerable<Assembly> references) {
            // this creates worker app domain in a way that host doesn't need to be in GAC or bin
            // using BuildManagerHost via private reflection
            ApplicationManager appManager = ApplicationManager.GetApplicationManager();
            string uniqueAppString = String.Concat(virtualPath, physicalPath).ToLowerInvariant();
            string appId = (uniqueAppString.GetHashCode()).ToString("x", CultureInfo.InvariantCulture);

            // create BuildManagerHost in the worker app domain
            Type buildManagerHostType = typeof(HttpRuntime).Assembly.GetType("System.Web.Compilation.BuildManagerHost");
            object buildManagerHost = appManager.CreateObject(appId, buildManagerHostType, virtualPath, physicalPath,
                                                              false);

            // call BuildManagerHost.RegisterAssembly to make Host type loadable in the worker app domain
            RegisterAssembly(buildManagerHostType, buildManagerHost, hostType.Assembly);

            foreach (var assembly in references) {
                RegisterAssembly(buildManagerHostType, buildManagerHost, assembly);
            }

            // create Host in the worker app domain
            return appManager.CreateObject(appId, hostType, virtualPath, physicalPath, false);
        }

        private static void RegisterAssembly(Type buildManagerHostType, object buildManagerHost, Assembly assembly) {
            buildManagerHostType.InvokeMember(
            "RegisterAssembly",
            BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic,
            null,
            buildManagerHost,
            new object[] { assembly.FullName, assembly.Location });
    
        }

        internal void HostStopped() {
            _host = null;
        }

        public void Close() {
            GetHost().Close();
            HostStopped();
        }
    }
}
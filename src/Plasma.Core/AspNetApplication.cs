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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using Microsoft.Win32.SafeHandles;

namespace Plasma.Core {

    public sealed class AspNetApplication : MarshalByRefObject
    {
        private readonly IList<Assembly> _references = new List<Assembly>();

        private Host _host;
        private readonly string _virtualPath;
        private readonly string _physicalPath;
        
        public AspNetApplication(string virtualPath, string physicalPath) {
            _virtualPath = virtualPath;
            _physicalPath = physicalPath;
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
            byte[] responseBody;

            string requestVirtualPath = VirtualPathUtility.ToAbsolute(request.FilePath, _virtualPath);

            int status = ProcessRequest(requestVirtualPath, request.PathInfo, request.QueryString,
                                        request.Method, request.Headers, request.Body,
                                        out responseHeaders, out responseBody);

            return new AspNetResponse(requestVirtualPath, status, responseHeaders, responseBody);
        }

        public AspNetResponse ProcessRequest(string requestPath) {
            return ProcessRequest(new AspNetRequest(requestPath));
        }

        private int ProcessRequest(
            string requestFilePath,
            string requestPathInfo,
            string requestQueryString,
            string requestMethod,
            IEnumerable<KeyValuePair<string, string>> requestHeaders,
            byte[] requestBody,
            out List<KeyValuePair<string, string>> responseHeaders,
            out byte[] responseBody) {

            return GetHost().ProcessRequest(requestFilePath, requestPathInfo, requestQueryString,
                                            requestMethod, requestHeaders, requestBody, out responseHeaders,
                                            out responseBody);
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


        private class Host : MarshalByRefObject, IRegisteredObject {
            private AspNetApplication _app;

            public Host() {
                HostingEnvironment.RegisterObject(this);
            }

            public override object InitializeLifetimeService() {
                return null;
            }

            public void Configure(AspNetApplication app) {
                _app = app;
            }

			void IRegisteredObject.Stop(bool immediate) {
                if (_app != null) {
                    _app.HostStopped();
                }

                HostingEnvironment.UnregisterObject(this);
            }
            
            public void InvokeInAspAppDomain(Action func) {
                func.Invoke();
            }

            public void InvokeInAspAppDomain<T>(Action<T> func, T arg) {
                func.Invoke(arg);
            }


            public TR InvokeInAspAppDomain<TR>(Func<TR> func) {
                return func.Invoke();
            }

            public TR InvokeInAspAppDomain<T, TR>(Func<T, TR> func, T arg) {
                return func.Invoke(arg);
            }

            public TR InvokeInAspAppDomain<T1, T2, TR>(Func<T1, T2, TR> func, T1 arg1, T2 arg2) {
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
                out byte[] responseBody) {

                var wr = new WorkerRequest(requestFilePath, requestPathInfo,
                                           requestQueryString, requestMethod, requestHeaders, requestBody);

                HttpRuntime.ProcessRequest(wr);

                while (!wr.Completed) {
                    Thread.Sleep(50);
                }

                responseHeaders = wr.ResponseHeaders;
                responseBody = wr.ResponseBody;
                return wr.ResponseStatus;
            }

            public void Close() {
                _app = null;
                HttpRuntime.Close();
            }
        }

        private class WorkerRequest : SimpleWorkerRequest {
            // request data
            private readonly string _filePath;
            private readonly string _method;
            private readonly string _path;
            private readonly string _pathInfo;
            private readonly string _physicalPath;
            private readonly byte[] _preloadedContent;
            private readonly string _queryString;
            private readonly string _rawUrl;
            
            private string _allRawHeaders;
            private string[][] _unknownRequestHeaders;
            private string[] _knownRequestHeaders;

            // response data
            private int _responseStatus;
            private readonly List<KeyValuePair<string, string>> _responseHeaders;
            private readonly List<byte[]> _responseBuilder;

            // status
            private volatile bool _completed;
            
            internal WorkerRequest(
                string requestFilePath,
                string requestPathInfo,
                string requestQueryString,
                string requestMethod,
                IEnumerable<KeyValuePair<string, string>> requestHeaders,
                byte[] requestBody)
                : base(String.Empty, String.Empty, null) {
                
                _filePath = requestFilePath;
                _pathInfo = requestPathInfo;
                _path = _filePath + _pathInfo;
                _physicalPath = HostingEnvironment.MapPath(_filePath);
                _queryString = requestQueryString;
                _rawUrl = string.IsNullOrEmpty(_queryString) ? _path : _path + "?" + _queryString;
                _method = requestMethod;
                _preloadedContent = requestBody;
                ParseHeaders(requestHeaders);

                _responseStatus = 200;
                _responseHeaders = new List<KeyValuePair<string, string>>();
                _responseBuilder = new List<byte[]>();
            }

            internal bool Completed {
                get { return _completed; }
            }

            internal byte[] ResponseBody {
                get {
                    int responseLength = 0;

                    foreach (var buffer in _responseBuilder) {
                        if (buffer != null) {
                            responseLength += buffer.Length;
                        }
                    }

                    var responseBuffer = new byte[responseLength];
                    int offset = 0;

                    foreach (var buffer in _responseBuilder) {
                        if (buffer != null && buffer.Length > 0) {
                            Array.Copy(buffer, 0, responseBuffer, offset, buffer.Length);
                            offset += buffer.Length;
                        }
                    }

                    return responseBuffer;
                }
            }

            internal int ResponseStatus {
                get { return _responseStatus; }
            }

            internal List<KeyValuePair<string, string>> ResponseHeaders {
                get { return _responseHeaders; }
            }

            private void ParseHeaders(IEnumerable<KeyValuePair<string, string>> requestHeaders) {
                _knownRequestHeaders = new string[RequestHeaderMaximum];

                // construct unknown headers as array list of name1,value1,...
                var headers = new ArrayList();
                var allRaw = new StringBuilder();

                if (requestHeaders != null) {
                    foreach (var header in requestHeaders) {
                        string name = header.Key;
                        string value = header.Value;

                        // remember
                        int knownIndex = GetKnownRequestHeaderIndex(name);
                        if (knownIndex >= 0) {
                            _knownRequestHeaders[knownIndex] = value;
                        }
                        else {
                            headers.Add(name);
                            headers.Add(value);
                        }

                        allRaw.Append(name);
                        allRaw.Append(": ");
                        allRaw.Append(value);
                    }
                }

                // copy to array unknown headers
                int n = headers.Count / 2;
                _unknownRequestHeaders = new string[n][];
                int j = 0;

                for (int i = 0; i < n; i++) {
                    _unknownRequestHeaders[i] = new string[2];
                    _unknownRequestHeaders[i][0] = (string)headers[j++];
                    _unknownRequestHeaders[i][1] = (string)headers[j++];
                }

                // remember all raw headers as one string
                _allRawHeaders = allRaw.ToString();
            }

            public override string GetUriPath() {
                return _path;
            }

            public override string GetQueryString() {
                return _queryString;
            }

            public override string GetRawUrl() {
                return _rawUrl;
            }

            public override string GetHttpVerbName() {
                return _method;
            }

            public override string GetFilePath() {
                return _filePath;
            }

            public override string GetFilePathTranslated() {
                return _physicalPath;
            }

            public override string GetPathInfo() {
                return _pathInfo;
            }

            public override string GetAppPath() {
                return HostingEnvironment.ApplicationVirtualPath;
            }

            public override string GetAppPathTranslated() {
                return HostingEnvironment.ApplicationPhysicalPath;
            }

            public override byte[] GetPreloadedEntityBody() {
                return _preloadedContent;
            }

            public override bool IsEntireEntityBodyIsPreloaded() {
                return true;
            }

            public override int ReadEntityBody(byte[] buffer, int size) {
                return 0;
            }

            public override string GetKnownRequestHeader(int index) {
                return _knownRequestHeaders[index];
            }

            public override string GetUnknownRequestHeader(string name) {
                int n = _unknownRequestHeaders.Length;

                for (int i = 0; i < n; i++) {
                    if (string.Compare(name, _unknownRequestHeaders[i][0], StringComparison.OrdinalIgnoreCase) == 0) {
                        return _unknownRequestHeaders[i][1];
                    }
                }

                return null;
            }

            public override string[][] GetUnknownRequestHeaders() {
                return _unknownRequestHeaders;
            }

            public override string GetServerVariable(string name) {
                string s = String.Empty;

                switch (name) {
                    case "ALL_RAW":
                        s = _allRawHeaders;
                        break;

                    case "SERVER_PROTOCOL":
                        s = GetProtocol();
                        break;

                    case "LOGON_USER":
                        break;

                    case "AUTH_TYPE":
                        break;
                }

                return s;
            }

            public override IntPtr GetUserToken() {
                return IntPtr.Zero;
            }

            public override string MapPath(string path) {
                return HostingEnvironment.MapPath(path);
            }

            public override void SendStatus(int statusCode, string statusDescription) {
                _responseStatus = statusCode;
            }

            public override void SendKnownResponseHeader(int index, string value) {
                _responseHeaders.Add(new KeyValuePair<string, string>(GetKnownResponseHeaderName(index), value));
            }

            public override void SendUnknownResponseHeader(string name, string value) {
                _responseHeaders.Add(new KeyValuePair<string, string>(name, value));
            }

            public override void SendCalculatedContentLength(int contentLength) {
                _responseHeaders.Add(new KeyValuePair<string, string>("Content-Length",
                                                                     contentLength.ToString(
                                                                         CultureInfo.InvariantCulture)));
            }

            public override bool HeadersSent() {
                return false;
            }

            public override bool IsClientConnected() {
                return true;
            }

            public override void CloseConnection() {
                // do nothing
            }

            public override void SendResponseFromMemory(byte[] data, int length) {
                if (length > 0) {
                    var buffer = new byte[length];
                    Array.Copy(data, buffer, length);
                    _responseBuilder.Add(buffer);
                }
            }

            public override void SendResponseFromFile(string filename, long offset, long length) {
                if (length > 0) {
                    using (var f = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        SendResponseFromStream(f, offset, length);
                    }
                }
            }

            public override void SendResponseFromFile(IntPtr handle, long offset, long length) {
                if (length > 0) {
                    var sfh = new SafeFileHandle(handle, false);

                    using (var f = new FileStream(sfh, FileAccess.Read)) {
                        SendResponseFromStream(f, offset, length);
                    }
                }
            }

            private void SendResponseFromStream(Stream f, long offset, long length) {
                long fileSize = f.Length;

                if (length == -1) {
                    length = fileSize - offset;
                }

                if (length == 0 || offset < 0 || length > fileSize - offset) {
                    return;
                }

                if (offset > 0) {
                    f.Seek(offset, SeekOrigin.Begin);
                }

                var fileBytes = new byte[(int)length];
                int bytesRead = f.Read(fileBytes, 0, (int)length);
                SendResponseFromMemory(fileBytes, bytesRead);
            }

            public override void FlushResponse(bool finalFlush) {
                // do nothing
            }

            public override void EndOfRequest() {
                _completed = true;
            }
        }

        public void Close() {
            GetHost().Close();
            HostStopped();
        }
    }
}
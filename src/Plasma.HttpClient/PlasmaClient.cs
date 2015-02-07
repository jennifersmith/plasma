using System;
using Plasma.Core;

namespace Plasma.HttpClient
{
    public static class PlasmaClient
    {
        public static System.Net.Http.HttpClient For(IRequestProcessor application)
        {
            return BuildClient(application);
        }

        public static System.Net.Http.HttpClient For(string physicalPath, string virtualPath = "/")
        {
            var app = new AspNetApplication(virtualPath, physicalPath);
            return BuildClient(app);
        }

        public static System.Net.Http.HttpClient For<TApplicationType>()
        {
            var app = new AspNetApplication(typeof(TApplicationType));
            return BuildClient(app);
        }

        private static System.Net.Http.HttpClient BuildClient(IRequestProcessor app)
        {
            var messageHandler = new PlasmaMessageHandler(app);
            var client = new System.Net.Http.HttpClient(messageHandler) {BaseAddress = new Uri("http://localhost")};
            return client;
        }
    }
}
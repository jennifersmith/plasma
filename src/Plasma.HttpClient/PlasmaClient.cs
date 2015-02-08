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
            return BuildClient(new AspNetApplication(virtualPath, physicalPath));
        }

        public static System.Net.Http.HttpClient For<TApplicationType>()
        {
            return BuildClient(new AspNetApplication<TApplicationType>());
        }

        private static System.Net.Http.HttpClient BuildClient(IRequestProcessor app)
        {
            var messageHandler = new PlasmaMessageHandler(app);
            var client = new System.Net.Http.HttpClient(messageHandler) {BaseAddress = new Uri("http://localhost")};
            return client;
        }
    }
}
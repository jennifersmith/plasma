using System;
using System.IO;
using Plasma.Core;

namespace Plasma.Samples.MSTestLibrary
{
    public class WebApp
    {
        static AspNetApplication _appInstance;

        public static AspNetApplication AppInstance {

            get
            {
                // Todo: Make this configurable from a configuration file

                if (_appInstance == null)
                    _appInstance = new AspNetApplication("/", Path.GetFullPath(@".\..\..\..\..\Websites\PlasmaSampleCS"));

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

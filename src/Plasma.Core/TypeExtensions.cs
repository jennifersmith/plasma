using System;
using System.IO;

namespace Plasma.Core
{
    public static class TypeExtensions
    {
        public static string GetPhysicalLocation(this Type t)
        {
            var codeBase = t.Assembly.CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);

            var physicalPath = Path.GetDirectoryName(path) ?? string.Empty;
            if (physicalPath.EndsWith("\\bin"))
            {
                var length = "\\bin".Length;
                physicalPath = physicalPath.Substring(0, physicalPath.Length - length);
            }

            return physicalPath;
        }
    }
}
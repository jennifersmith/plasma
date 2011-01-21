using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml;

namespace Plasma.Core
{
    public class LocalEntityResolver : XmlResolver
    {
        private readonly Dictionary<Uri, Stream> _dtds = new Dictionary<Uri, Stream>();

        public LocalEntityResolver()
        {
            const string baseName = "Plasma.Core.Resources";

            Assembly asm = GetType().Assembly;

            _dtds.Add(
                new Uri("urn:-//W3C//DTD XHTML 1.0 Transitional//EN"),
                asm.GetManifestResourceStream(baseName + ".xhtml1-transitional.dtd"));
            _dtds.Add(
                new Uri("urn:-//W3C//DTD XHTML 1.0 Strict//EN"),
                asm.GetManifestResourceStream(baseName + ".xhtml1-strict.dtd"));
            _dtds.Add(
                new Uri("urn:xhtml-lat1.ent"),
                asm.GetManifestResourceStream(baseName + ".xhtml-lat1.ent"));
            _dtds.Add(
                new Uri("urn:xhtml-special.ent"),
                asm.GetManifestResourceStream(baseName + ".xhtml-special.ent"));
            _dtds.Add(
                new Uri("urn:xhtml-symbol.ent"),
                asm.GetManifestResourceStream(baseName + ".xhtml-symbol.ent"));
        }

        public override ICredentials Credentials
        {
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            Stream s;
            if (_dtds.TryGetValue(absoluteUri, out s))
            {
                return s;
            }

            return null;
        }

        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            return new Uri("urn:" + relativeUri);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Plasma.WebDriver.Finders
{
    public class ElementByClassNameFinder
    {
        private readonly string _className;

        public ElementByClassNameFinder(string className)
        {
            _className = className;
        }

        public IEnumerable<XmlElement> FindWithin(XmlElement xmlElement)
        {
            return FindElementsByXPathTempHack(xmlElement, String.Format(
                                           "descendant::node()[contains( normalize-space( @class ), ' {0} ' ) " +
                                           "or substring( normalize-space( @class ), 1, string-length( '{0}' ) + 1 ) = '{0} ' " +
                                           "or substring( normalize-space( @class ), string-length( @class ) - string-length( '{0}' ) ) = ' {0}' " +
                                           "or @class = '{0}']", _className));
        }

        private IEnumerable<XmlElement> FindElementsByXPathTempHack(XmlElement xmlElement, string xpath)
        {
            const string xhtmlNamespacePrefix = "xhtml";
            var namespaceManager = new XmlNamespaceManager(xmlElement.OwnerDocument.NameTable);
            namespaceManager.AddNamespace(xhtmlNamespacePrefix, "http://www.w3.org/1999/xhtml");

            var nodes = xmlElement.SelectNodes(xpath, namespaceManager);
            if(nodes!=null)
            {
                return nodes.Cast<XmlElement>();
            }
            return new XmlElement[0];
        }
    }
}
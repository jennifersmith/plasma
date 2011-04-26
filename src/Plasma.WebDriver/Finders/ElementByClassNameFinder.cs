using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Plasma.WebDriver.Finders
{
    public class ElementFinder
    {
        protected static IEnumerable<XmlElement> FindElementsByXPathTempHack(XmlElement xmlElement, string xpath)
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

    public class ElementByClassNameFinder : ElementFinder
    {
        private readonly string _className;

        public ElementByClassNameFinder(string className)
        {
            _className = className;
        }

        public IEnumerable<XmlElement> FindWithin(XmlElement xmlElement)
        {
            IEnumerable<XmlElement> candidateMatches = GetCandidateMatchesForClassName(xmlElement);
            return candidateMatches.Where(HasMatchingClassName);
        }

        private bool HasMatchingClassName(XmlElement element)
        {
            string classAttributeValue = element.GetAttribute("class");

            string[] classes = classAttributeValue.Split(new string[]{}, StringSplitOptions.RemoveEmptyEntries);
            return classes.Contains(_className);
        }

        private IEnumerable<XmlElement> GetCandidateMatchesForClassName(XmlElement xmlElement)
        {
            return FindElementsByXPathTempHack(xmlElement, String.Format("descendant::node()[contains( normalize-space( @class ), '{0}' )]", _className));
        }
    }


    public class ElementByIdFinder : ElementFinder
    {
        private readonly string _id;

        public ElementByIdFinder(string id)
        {
            _id = id;
        }

        public IEnumerable<XmlElement> FindWithin(XmlElement xmlElement)
        {
            return FindElementsByXPathTempHack(xmlElement,String.Format("descendant::node()[@id='{0}']", _id));
        }
    }


    public class ElementByNameFinder : ElementFinder
    {
        private readonly string _name;

        public ElementByNameFinder(string name)
        {
            _name = name;
        }

        public IEnumerable<XmlElement> FindWithin(XmlElement xmlElement)
        {
            return FindElementsByXPathTempHack(xmlElement, String.Format("descendant::node()[@name='{0}']", _name));
        }
    }
}
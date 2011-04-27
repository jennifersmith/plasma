using System;
using System.Collections.Generic;
using System.Xml;

namespace Plasma.WebDriver.Finders
{
    public class ElementByTagNameFinder : ElementFinder
    {
        private readonly string _name;

        public ElementByTagNameFinder(string name)
        {
            _name = name;
        }

        public IEnumerable<XmlElement> FindWithin(XmlElement xmlElement)
        {
            return FindElementsByXPathTempHack(xmlElement, String.Format("descendant::{0}:{1}", "xhtml", _name));
        }
    }
}
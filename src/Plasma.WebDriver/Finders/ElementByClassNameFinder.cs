/* **********************************************************************************
 *
 * Copyright 2010 ThoughtWorks, Inc.  
 * ThoughtWorks provides the software "as is" without warranty of any kind, either express or implied, including but not limited to, 
 * the implied warranties of merchantability, satisfactory quality, non-infringement and fitness for a particular purpose.
 *
 * This source code is subject to terms and conditions of the Microsoft Permissive
 * License (MS-PL).  
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
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
            var nodes = xmlElement.SelectNodes(xpath);
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
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Plasma.WebDriver.Finders
{
    public class ElementFinderContext : ISearchContext, IFindsByClassName, IFindsByXPath, IFindsByTagName, IFindsById, IFindsByName, IFindsByCssSelector
    {
        private readonly XElement _xElement;

        public ElementFinderContext(XmlElement xmlElement, XElement xElement)
        {
            _xElement = xElement;
        }

        public IWebElement FindElement(By by)
        {
            return by.FindElement(this);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return  by.FindElements(this);
        }


        public IWebElement FindElementByClassName(string className)
        {
            var elements = FindElementsByClassName(className);
            if (elements.Any())
            {
                return elements.First();
            }
            throw new NotFoundException("ClassName: " + className);
        }

        public ReadOnlyCollection<IWebElement> FindElementsByClassName(string className)
        {
            return new ElementByClassNameFinder(className).FindWithin(_xElement).AsReadonlyCollection();
        }


        public IWebElement FindElementById(string id)
        {
            var elements = FindElementsById(id);
            if (elements.Any())
            {
                return elements.First();
            }
            throw new NotFoundException("Id: " + id);
        }

        public ReadOnlyCollection<IWebElement> FindElementsById(string id)
        {
            return new ElementByIdFinder(id).FindWithin(_xElement).AsReadonlyCollection();
        }


        public IWebElement FindElementByName(string name)
        {
            var elements = FindElementsByName(name);
            if (elements.Any())
            {
                return elements.First();
            }
            throw new NotFoundException("Name: " + name);
        }

        public ReadOnlyCollection<IWebElement> FindElementsByName(string name)
        {
            return new ElementByNameFinder(name).FindWithin(_xElement).AsReadonlyCollection();
        }

        public IWebElement FindElementByTagName(string tagName)
        {
            var elements = FindElementsByTagName(tagName);
            if (elements.Any())
            {
                return elements.First();
            }
            throw new NotFoundException("TagName: " + tagName);
        }

        public ReadOnlyCollection<IWebElement> FindElementsByTagName(string tagName)
        {
            return new ElementByTagNameFinder(tagName).FindWithin(_xElement).AsReadonlyCollection();
        }

        public IWebElement FindElementByXPath(string xpath)
        {
            IEnumerator<IWebElement> enumerator = FindElementsByXPath(xpath).GetEnumerator();
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }
            throw new NotFoundException("XPath: " + xpath);
        }

        public ReadOnlyCollection<IWebElement> FindElementsByXPath(string xpath)
        {
            return new ElementByXpathFinder(xpath).FindWithin(_xElement).AsReadonlyCollection();
        }

        public IWebElement FindElementByCssSelector(string cssSelector)
        {
            ReadOnlyCollection<IWebElement> elements = FindElementsByCssSelector(cssSelector);
            if( elements.Any())
            {
                return elements.Single();
            }
            throw new NotFoundException("Css selector: " + cssSelector);
        }

        public ReadOnlyCollection<IWebElement> FindElementsByCssSelector(string cssSelector)
        {
            return new ElementByCssSelectorFinder(cssSelector).FindWithin(null).AsReadonlyCollection();
        }
    }

    public class ElementByCssSelectorFinder
    {
        private readonly string _cssSelector;

        public ElementByCssSelectorFinder(string cssSelector)
        {
            _cssSelector = cssSelector;
        }

        public IEnumerable<XmlElement> FindWithin(XmlElement xmlElement)
        {
            throw new NotImplementedException();
        }
    }
}

﻿/* **********************************************************************************
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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Plasma.WebDriver.Finders
{
    public class ElementFinderContext : ISearchContext, IFindsByClassName, IFindsByXPath, IFindsByTagName, IFindsById, IFindsByName, IFindsByCssSelector
    {
        private readonly HtmlNode currentNode;
        private readonly WebBrowser webBrowser;

        public ElementFinderContext(HtmlNode currentNode, WebBrowser webBrowser)
        {
            this.currentNode = currentNode;
            this.webBrowser = webBrowser;
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
            return AsReadonlyCollection(new ElementByClassNameFinder(className).FindWithin(currentNode));
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
            return AsReadonlyCollection(new ElementByIdFinder(id).FindWithin(currentNode));
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
            return AsReadonlyCollection(new ElementByNameFinder(name).FindWithin(currentNode));
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
            return AsReadonlyCollection(new ElementByTagNameFinder(tagName).FindWithin(currentNode));
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
            return AsReadonlyCollection(new ElementByXpathFinder(xpath).FindWithin(currentNode));
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
            return AsReadonlyCollection(new ElementByCssSelectorFinder(cssSelector).FindWithin(currentNode));
        }

        private ReadOnlyCollection<IWebElement> AsReadonlyCollection(IEnumerable<HtmlNode> htmlNodes)
        {
            return new ReadOnlyCollection<IWebElement>(htmlNodes.Select(x => new HtmlElement(x, webBrowser)).Cast<IWebElement>().ToList());
        }
    }
}

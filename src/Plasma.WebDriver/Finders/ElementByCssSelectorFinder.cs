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
using System.IO;
using System.Xml;
using System.Xml.Linq;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Linq;

namespace Plasma.WebDriver.Finders
{
    public class ElementByCssSelectorFinder : IElementFinder
    {
        private readonly string _cssSelector;

        public ElementByCssSelectorFinder(string cssSelector)
        {
            _cssSelector = cssSelector;
        }

        public IEnumerable<XElement> FindWithin(XElement xmlElement)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(xmlElement.ToString(SaveOptions.DisableFormatting));

            IEnumerable<HtmlNode> nodes = htmlDocument.DocumentNode.QuerySelectorAll(_cssSelector);

            return nodes.Select(x => XElement.Parse(x.OuterHtml));
        }
    }
}
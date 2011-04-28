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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Plasma.WebDriver.Finders
{
    public class ElementByClassNameFinder : IElementFinder
    {
        private readonly string _className;

        public ElementByClassNameFinder(string className)
        {
            _className = className;
        }

        public IEnumerable<XElement> FindWithin(XElement xmlElement)
        {
             return GetElementsWithClassName(xmlElement);
        }

        private IEnumerable<XElement> GetElementsWithClassName(XElement xmlElement)
        {
            return
                xmlElement.Descendants()
                .Attributes("class")
                .Where(x => x.Value.Split().Contains(_className))
                .Select( x => x.Parent);
        }
    }


    public class ElementByIdFinder
    {
        private readonly string _id;

        public ElementByIdFinder(string id)
        {
            _id = id;
        }

        public IEnumerable<XElement> FindWithin(XElement xmlElement)
        {
            return xmlElement.Descendants().Attributes("id").Where(x => x.Value == _id).Select(x => x.Parent);
        }
    }


    public class ElementByNameFinder
    {
        private readonly string _name;

        public ElementByNameFinder(string name)
        {
            _name = name;
        }

        public IEnumerable<XElement> FindWithin(XElement xmlElement)
        {
            return xmlElement.Descendants().Attributes("name").Where(x => x.Value == _name).Select(x => x.Parent);
        }
    }
}
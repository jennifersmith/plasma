using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Plasma.WebDriver.Finders
{
    public class ElementByNameFinder : IElementFinder
    {
        private readonly string name;

        public ElementByNameFinder(string name)
        {
            this.name = name;
        }

        public IEnumerable<HtmlNode> FindWithin(HtmlNode htmlNodes)
        {
            return htmlNodes.Descendants().Where(x => x.GetAttributeValue("name", string.Empty).ToLower() == name.ToLower());
        }
    }
}
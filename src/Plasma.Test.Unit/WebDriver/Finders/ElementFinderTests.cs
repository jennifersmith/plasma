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
using System.Xml;
using NUnit.Framework;
using Plasma.WebDriver.Finders;

namespace Plasma.Test.Unit.WebDriver.Finders
{
    [TestFixture]
    public class ElementFinderTests
    {
       
        private static XmlDocument MakeXmlDocument(string inner)
        {
            string xmlSource = @"<html>
                                                    <body>
                                                        <div>
                                                            <div>
                                                                " + inner+ @"
                                                            </div>
                                                         </div>
                                                    </body>
                                                </html>";
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlSource);
            return document;
        }


        [TestCase("foo", "foo", Description = "Basic fully matching class name")]
        [TestCase("foo", "foo", Description = "Matching one of several class names - at beginning")]
        [TestCase("foo", "foo bar baz wibble", Description = "Matching one of several class names - at beginning")]
        [TestCase("foo", "bar baz wibble foo", Description = "Matching one of several class names - at end")]
        [TestCase("foo", "bar baz foo wibble", Description = "Matching one of several class names - in middle")]
        [TestCase("foo", "       foo      ")]
        [TestCase("foo", "       bar      foo     ")]
        [TestCase("foo", "       foo      bar     ")]
        public void ByClass_ShouldFindAnElementThatHasClassNameInClassAttribute(string classToFind, string classAttributeValue)
        {
            string tag = string.Format(@"<div id='elementToFind' class='{0}'></div>>", classAttributeValue);

            var document = MakeXmlDocument(tag);

            IEnumerable<XmlElement> xmlElements = new ElementByClassNameFinder(classToFind).FindWithin(document.DocumentElement);

            Assert.That(xmlElements.Count(), Is.EqualTo(1), "Failed to locate a single element with class name " + classToFind + " within XML Source " + document.InnerXml);
            Assert.That(xmlElements.Single().GetAttribute("id"), Is.EqualTo("elementToFind"));
        }

        [TestCase("foo", "bar")]
        [TestCase("foo", "foobar")]
        [TestCase("foo", "barfoo")]
        [TestCase("foo", "whizz barfoobar")]
        public void ByClass_ShouldFindNoMatchesWhereCannotFindElementWithTheClassName(string classNotToFind, string classAttributeValue)
        {

            string xmlSource = string.Format(@"<html>
                                                    <body>
                                                        <div>
                                                            <div>
                                                                <div class='{0}'></div>
                                                            </div>
                                                         </div>
                                                    </body>
                                                </html>", classAttributeValue);

            var document = new XmlDocument();
            document.LoadXml(xmlSource);

            IEnumerable<XmlElement> xmlElements = new ElementByClassNameFinder(classNotToFind).FindWithin(document.DocumentElement);

            Assert.That(xmlElements.Count(), Is.EqualTo(0), "Should not have found " + classNotToFind + " within XML Source " + xmlSource);
        }


        [Test]
        public void ById_ShouldMatchIfThereIsAnElementWithTheGivenId()
        {
            var document = MakeXmlDocument(@"<div id='elementToFind'>Find this</div>");

            IEnumerable<XmlElement> xmlElements = new ElementByIdFinder("elementToFind").FindWithin(document.DocumentElement);

            Assert.That(xmlElements.Count(), Is.EqualTo(1));
            Assert.That(xmlElements.Single().InnerText, Is.EqualTo("Find this"));
        }

        [Test]
        public void ById_ShouldNotMatchIfThereIsAnElementWithTheGivenId()
        {
            var document = MakeXmlDocument(@"<div id='dontFindThis'>Don't find this</div>");

            IEnumerable<XmlElement> xmlElements = new ElementByIdFinder("elementToFind").FindWithin(document.DocumentElement);

            Assert.That(xmlElements.Count(), Is.EqualTo(0));
        }


        [Test]
        public void ByName_ShouldMatchIfThereIsAnElementWithTheGivenName()
        {
            var document = MakeXmlDocument(@"<div name='elementToFind'>Find this</div>");


            IEnumerable<XmlElement> xmlElements = new ElementByNameFinder("elementToFind").FindWithin(document.DocumentElement);

            Assert.That(xmlElements.Count(), Is.EqualTo(1));
            Assert.That(xmlElements.Single().InnerText, Is.EqualTo("Find this"));
        }

        [Test]
        public void ByName_ShouldNotMatchIfThereIsAnElementWithTheGivenName()
        {
            var document = MakeXmlDocument(@"<div id='dontFindThis'>Don't find this</div>");

            IEnumerable<XmlElement> xmlElements = new ElementByNameFinder("elementToFind").FindWithin(document.DocumentElement);

            Assert.That(xmlElements.Count(), Is.EqualTo(0));
        }
        [Test]
        public void ByTag_ShouldMatchIfFindsGivenTag()
        {
            var document = MakeXmlDocument(@"<foo>Find this</foo>");


            IEnumerable<XmlElement> xmlElements = new ElementByTagNameFinder("foo").FindWithin(document.DocumentElement);

            Assert.That(xmlElements.Count(), Is.EqualTo(1));
            Assert.That(xmlElements.Single().InnerText, Is.EqualTo("Find this"));
            
        }
        [Test]
        public void ByTag_ShouldNotMatchIfDoesntFindGivenTag()
        {
            var document = MakeXmlDocument(@"<foo>Don't find this</foo>");


            IEnumerable<XmlElement> xmlElements = new ElementByTagNameFinder("bar").FindWithin(document.DocumentElement);

            Assert.That(xmlElements.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ByXpath_ShouldFindByXPath()
        {
            var document = MakeXmlDocument(@"<a>
                                                <b>
                                                </b>
                                                <b attr='foo'>
                                                  <c>Find this</c>
                                                </b>
                                                </a>");

            var result = new ElementByXpathFinder("//a/b[@attr='foo']/c").FindWithin(document.DocumentElement);
            Assert.That(result.Count(),Is.EqualTo(1));
            Assert.That(result.Single().InnerText, Is.EqualTo("Find this"));

            result = new ElementByXpathFinder("//a/b[@attr='bar']/c").FindWithin(document.DocumentElement);
            Assert.That(result.Count(), Is.EqualTo(0));
        }
    
    }
}
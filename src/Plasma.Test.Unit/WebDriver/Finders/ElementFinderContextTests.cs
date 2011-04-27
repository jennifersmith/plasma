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
using System.Linq;
using System.Xml;
using NUnit.Framework;
using OpenQA.Selenium;
using Plasma.WebDriver.Finders;

namespace Plasma.Test.Unit.WebDriver.Finders
{
    [TestFixture]
    public class ElementFinderContextTests
    {
       
        private static ElementFinderContext MakeElementFinderContext(string inner)
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
            var document = new XmlDocument();
            document.LoadXml(xmlSource);
            return new ElementFinderContext(document.DocumentElement);
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

            var document = MakeElementFinderContext(tag);

            var result = document.FindElement(By.ClassName(classToFind)); 

            Assert.That(result.GetAttribute("id"), Is.EqualTo("elementToFind"));
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

            var document = MakeElementFinderContext(xmlSource);
            var elements = document.FindElements(By.ClassName(classNotToFind));

            Assert.That(elements.Count(), Is.EqualTo(0), "Should not have found " + classNotToFind + " within XML Source " + xmlSource);
        }


        [Test]
        public void ById_ShouldMatchIfThereIsAnElementWithTheGivenId()
        {
            var document = MakeElementFinderContext(@"<div id='elementToFind'>Find this</div>");

            var elements = document.FindElement(By.Id("elementToFind"));

            Assert.That(elements.Text, Is.EqualTo("Find this"));
        }

        [Test]
        public void ById_ShouldNotMatchIfThereIsAnElementWithTheGivenId()
        {
            var document = MakeElementFinderContext(@"<div id='dontFindThis'>Don't find this</div>");
            var elements = document.FindElements(By.Id("elementToFind"));
            Assert.That(elements.Count(), Is.EqualTo(0));
        }


        [Test]
        public void ByName_ShouldMatchIfThereIsAnElementWithTheGivenName()
        {
            var document = MakeElementFinderContext(@"<div name='elementToFind'>Find this</div>");
            var element = document.FindElement(By.Name("elementToFind"));
            Assert.That(element.Text, Is.EqualTo("Find this"));
        }

        [Test]
        public void ByName_ShouldNotMatchIfThereIsAnElementWithTheGivenName()
        {
            var document = MakeElementFinderContext(@"<div id='dontFindThis'>Don't find this</div>");
            var elements = document.FindElements(By.Name("elementToFind"));
            Assert.That(elements.Count(), Is.EqualTo(0));
        }
        [Test]
        public void ByTag_ShouldMatchIfFindsGivenTag()
        {
            var document = MakeElementFinderContext(@"<foo>Find this</foo>");
            var element = document.FindElement(By.TagName("foo"));
            Assert.That(element.Text, Is.EqualTo("Find this"));
            
        }
        [Test]
        public void ByTag_ShouldNotMatchIfDoesntFindGivenTag()
        {
            var document = MakeElementFinderContext(@"<foo>Don't find this</foo>");
            var elements = document.FindElements(By.TagName("bar"));
            Assert.That(elements.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ByXpath_ShouldFindByXPath()
        {
            var document = MakeElementFinderContext(@"<a>
                                                <b>
                                                </b>
                                                <b attr='foo'>
                                                  <c>Find this</c>
                                                </b>
                                                </a>");

            var element = document.FindElement(By.XPath("//a/b[@attr='foo']/c"));

            Assert.That(element.Text, Is.EqualTo("Find this"));

            var elements = document.FindElements(By.XPath("//a/b[@attr='bar']/c"));
            Assert.That(elements.Count(), Is.EqualTo(0));
        }
    
    }
}
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
using NUnit.Framework;
using OpenQA.Selenium;
using Plasma.WebDriver;

namespace Plasma.Test.Functional.WebDriver.FindingElements
{
    [TestFixture]
    public class FindElementsByTagNameTests
    {
        private PlasmaDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = TestFixture.Driver;
            driver.Navigate().GoToUrl("/FindElementsByTagName/");
        }

        [Test]
        public void ShouldReturnElementIfAnElementIsFound()
        {
            var findElement = driver.FindElement(By.TagName("span"));
            Assert.That(findElement.Text, Is.StringContaining("Found By TagName : span"), driver.PageSource);
        }

        [Test, ExpectedException(typeof(NotFoundException))]
        public void ShouldThrowExceptionIfAnElementIsNotFound()
        {
            driver.FindElement(By.TagName("tagNameThatDoesNotExistOnPage"));
        }

        [Test]
        public void ShouldReturnElementIfAnElementIsFoundWithinAnotherElement()
        {
            var outerElement = driver.FindElement(By.TagName("ul"));
            var innerElement = outerElement.FindElement(By.TagName("li"));
            Assert.That(innerElement.Text, Is.StringContaining("Found By TagName : li"), driver.PageSource);
        }

        [Test, ExpectedException(typeof(NotFoundException))]
        public void ShouldNotReturnElementIfAnElementLiesOutsideAnotherElement()
        {
            var innerElement = driver.FindElement(By.TagName("li"));
            innerElement.FindElement(By.TagName("ul"));
        }

        [Test]
        public void ShouldReturnCollectionContainingMultipleElementWhenElementsFound()
        {
            var elements = driver.FindElements(By.TagName("p"));
            Assert.That(elements.Any(), Is.True);
            Assert.That(elements.All(x => x.Text.Contains("Found By TagName : p")), Is.True, driver.PageSource);
        }

        [Test]
        public void ShouldReturnEmptyCollectionWhenElementsNotFound()
        {
            var elements = driver.FindElements(By.TagName("tagNameThatDoesNotExistOnThePage"));
            Assert.That(elements.Any(), Is.False, "Found matching element when none should exist.");
        }
    }
}

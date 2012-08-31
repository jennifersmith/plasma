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
    public class FindingElementsByIdTests
    {
        private PlasmaDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = TestFixture.Driver;
            TestFixture.Driver.Navigate().GoToUrl("/FindElementsById/#someRandomAnchor");
        }

        [Test]
        public void ShouldReturnElementIfAnElementIsFoundWithAMatchingId()
        {
            var findElement = driver.FindElement(By.Id("elementId"));
            Assert.That(findElement.Text, Is.StringContaining("Found By Id"), driver.PageSource);
        }

        [Test, ExpectedException(typeof(NotFoundException))]
        public void ShouldThrowExceptionIfAnElementIsNotFound()
        {
            driver.FindElement(By.Id("elementIdThatDoesNotExist"));
        }

        [Test]
        public void ShouldReturnElementIfAnElementIsFoundWithinAnotherElement()
        {
            var outerElement = driver.FindElement(By.Id("outerElementId"));
            var innerElement = outerElement.FindElement(By.Id("innerElementId"));
            Assert.That(innerElement.Text, Is.StringContaining("Found Inner By Id"), driver.PageSource);
        }

        [Test, ExpectedException(typeof(NotFoundException))]
        public void ShouldNotReturnElementIfAnElementLiesOutsideAnotherElement()
        {
            var innerElement = driver.FindElement(By.Id("innerElementId"));
            innerElement.FindElement(By.Id("outerElementId"));
        }

        [Test]
        public void ShouldReturnCollectionContainingSingleElementWhenElementsFoundById()
        {
            var elements = driver.FindElements(By.Id("elementId"));
            Assert.That(elements.Any(), Is.True, "Found no matching element on page.");
            Assert.That(elements.First().Text, Is.StringContaining("Found By Id"), driver.PageSource);
        }

        [Test]
        public void ShouldReturnEmptyCollectionWhenElementsNotFoundById()
        {
            var elements = driver.FindElements(By.Id("elementIdThatDoesNotExist"));
            Assert.That(elements.Any(), Is.False, "Found matching element when none should exist.");
        }
    }
}

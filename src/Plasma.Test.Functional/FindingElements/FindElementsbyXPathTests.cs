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

namespace Plasma.Test.Functional.FindingElements
{
    [TestFixture]
    public class FindElementsbyXPathTests
    {
        private PlasmaDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = TestFixture.Driver;
            driver.Navigate().GoToUrl("/FindElementsByXPath/");
        }

        [Test]
        public void ShouldReturnElementIfAnElementIsFound()
        {
            var findElement = driver.FindElement(By.XPath("//*[@class='className']"));
            Assert.That(findElement.Text, Is.StringContaining("Found By XPath"), driver.PageSource);
        }

        [Test, ExpectedException(typeof(NotFoundException))]
        public void ShouldThrowExceptionIfAnElementIsNotFound()
        {
            driver.FindElement(By.XPath("//*[@class='classNameThatDoesNotExist']"));
        }

        [Test]
        public void ShouldReturnElementIfAnElementIsFoundWithinAnotherElement()
        {
            var outerElement = driver.FindElement(By.XPath("//*[@class='outerClassName']"));
            var innerElement = outerElement.FindElement(By.XPath("//*[@class='innerClassName']"));
            Assert.That(innerElement.Text, Is.StringContaining("Found By XPath"), driver.PageSource);
        }

        [Test]
        public void ShouldReturnCollectionContainingMultipleElementWhenElementsFound()
        {
            var elements = driver.FindElements(By.XPath("//*[@class='classNameThatsUsedManyTimes']"));
            Assert.That(elements.Any(), Is.True);
            Assert.That(elements.All(x => x.Text.Contains("Found By XPath")), Is.True, driver.PageSource);
        }

        [Test]
        public void ShouldReturnEmptyCollectionWhenElementsNotFound()
        {
            var elements = driver.FindElements(By.XPath("//*[@class='elementClassThatDoesNotExist']"));
            Assert.That(elements.Any(), Is.False, "Found matching element when none should exist.");
        }
    }
}
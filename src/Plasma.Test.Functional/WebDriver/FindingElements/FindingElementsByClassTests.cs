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
    public class FindingElementsByClassTests
    {
        private PlasmaDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = TestFixture.Driver;
            driver.Navigate().GoToUrl("/FindElementsByClass/");
        }

        [Test]
        public void ShouldReturnElementIfAnElementIsFound()
        {
            var findElement = driver.FindElement(By.ClassName("className"));
            Assert.That(findElement.Text, Is.StringContaining("Found By class : className"), driver.PageSource);
        }

        [Test]
        public void ShouldReturnElementWhichHasManyClassesIfFoundByAtLeastOne()
        {
            var findElementByFirstClass = driver.FindElement(By.ClassName("firstClass"));
            Assert.That(findElementByFirstClass.Text, Is.StringContaining("Found by class: firstClass or secondClass"), driver.PageSource);

            var findElementBySecondClass = driver.FindElement(By.ClassName("secondClass"));
            Assert.That(findElementBySecondClass.Text, Is.StringContaining("Found by class: firstClass or secondClass"), driver.PageSource);
        }

        [Test, ExpectedException(typeof(NotFoundException))]
        public void ShouldThrowExceptionIfAnElementIsNotFound()
        {
            driver.FindElement(By.ClassName("classNameThatDoesNotExist"));
        }

        [Test]
        public void ShouldReturnElementIfAnElementIsFoundWithinAnotherElement()
        {
            var outerElement = driver.FindElement(By.ClassName("outerClassName"));
            var innerElement = outerElement.FindElement(By.ClassName("innerClassName"));
            Assert.That(innerElement.Text, Is.StringContaining("Found By class : innerClassName"), driver.PageSource);
        }

        [Test, ExpectedException(typeof(NotFoundException))]
        public void ShouldNotReturnElementIfAnElementLiesOutsideAnotherElement()
        {
            var innerElement = driver.FindElement(By.ClassName("innerClassName"));
            innerElement.FindElement(By.ClassName("outerClassName"));
        }

        [Test]
        public void ShouldReturnCollectionContainingMultipleElementWhenElementsFound()
        {
            var elements = driver.FindElements(By.ClassName("classNameThatsUsedManyTimes"));
            Assert.That(elements.Any(), Is.True);
            Assert.That(elements.All(x => x.Text.Contains("Found By class : classNameThatsUsedManyTimes")), Is.True, driver.PageSource);
        }

        [Test]
        public void ShouldReturnEmptyCollectionWhenElementsNotFound()
        {
            var elements = driver.FindElements(By.ClassName("elementClassThatDoesNotExist"));
            Assert.That(elements.Any(), Is.False, "Found matching element when none should exist.");
        }
    }
}

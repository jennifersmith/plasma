using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using Plasma.WebDriver;

namespace Plasma.Test.Functional.FindingElements
{
    public class FindingElementsByCssSelectorTests
    {
        private PlasmaDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new PlasmaDriver(WebApplicationFixture.AppInstance);
            driver.Navigate().GoToUrl("/FindElementsByClass/");
        }

        [Test]
        public void ShouldReturnElementIfAnElementIsFound()
        {
            var findElement = driver.FindElement(By.CssSelector(".className"));
            Assert.That(findElement.Text, Is.StringContaining("Found By class : className"), driver.PageSource);
        }

        [Test]
        public void ShouldReturnElementWhichHasManyClassesIfFoundByAtLeastOne()
        {
            var findElementByFirstClass = driver.FindElement(By.CssSelector(".firstClass"));
            Assert.That(findElementByFirstClass.Text, Is.StringContaining("Found by class: firstClass or secondClass"), driver.PageSource);

            var findElementBySecondClass = driver.FindElement(By.CssSelector(".secondClass"));
            Assert.That(findElementBySecondClass.Text, Is.StringContaining("Found by class: firstClass or secondClass"), driver.PageSource);
        }

        [Test, ExpectedException(typeof(NotFoundException))]
        public void ShouldThrowExceptionIfAnElementIsNotFound()
        {
            driver.FindElement(By.CssSelector(".classNameThatDoesNotExist"));
        }

        [Test]
        public void ShouldReturnElementIfAnElementIsFoundWithinAnotherElement()
        {
            var outerElement = driver.FindElement(By.CssSelector(".outerClassName"));
            var innerElement = outerElement.FindElement(By.CssSelector(".innerClassName"));
            Assert.That(innerElement.Text, Is.StringContaining("Found By class : innerClassName"), driver.PageSource);
        }

        [Test, ExpectedException(typeof(NotFoundException))]
        public void ShouldNotReturnElementIfAnElementLiesOutsideAnotherElement()
        {
            var innerElement = driver.FindElement(By.CssSelector(".innerClassName"));
            innerElement.FindElement(By.CssSelector(".outerClassName"));
        }

        [Test]
        public void ShouldReturnCollectionContainingMultipleElementWhenElementsFound()
        {
            var elements = driver.FindElements(By.CssSelector(".classNameThatsUsedManyTimes"));
            Assert.That(elements.Any(), Is.True);
            Assert.That(elements.All(x => x.Text.Contains("Found By class : classNameThatsUsedManyTimes")), Is.True, driver.PageSource);
        }

        [Test]
        public void ShouldReturnEmptyCollectionWhenElementsNotFound()
        {
            var elements = driver.FindElements(By.CssSelector(".elementClassThatDoesNotExist"));
            Assert.That(elements.Any(), Is.False, "Found matching element when none should exist.");
        }
    }
}
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using Plasma.WebDriver;

namespace Plasma.Test.Functional.FindingElements
{
    [TestFixture]
    public class FindingElementsByIdTests
    {
        private PlasmaDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new PlasmaDriver(WebApplicationFixture.AppInstance);
            driver.Navigate().GoToUrl("/FindElementsById/");
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

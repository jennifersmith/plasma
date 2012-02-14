using NUnit.Framework;
using OpenQA.Selenium;
using Plasma.WebDriver;

namespace Plasma.Test.Functional.Navigation
{
    [TestFixture]
    public class GotoPageTests
    {
        [Test]
        public void ShouldNavigateToTheGivenUrl()
        {
            var driver = new PlasmaDriver(WebApplicationFixture.AppInstance);
            driver.Navigate().GoToUrl("~/GotoPage");
            var titleElement = driver.FindElement(By.TagName("title"));
            Assert.That(titleElement.Text, Is.StringContaining("GotoPage"));
        }

        [Test]
        public void ShouldFollow302RedirectsWhenNavigatingToAGivenUrl()
        {
            var driver = new PlasmaDriver(WebApplicationFixture.AppInstance);
            driver.Navigate().GoToUrl("~/GotoPage/ThreeOhTwo");
            var titleElement = driver.FindElement(By.TagName("title"));
            Assert.That(driver.Url, Is.EqualTo("/GotoPage"));
            Assert.That(titleElement.Text, Is.StringContaining("GotoPage"));
        }

        [Test]
        public void ShouldFollow301RedirectsWhenNavigatingToAGivenUrl()
        {
            var driver = new PlasmaDriver(WebApplicationFixture.AppInstance);
            driver.Navigate().GoToUrl("~/GotoPage/ThreeOhOne");
            var titleElement = driver.FindElement(By.TagName("title"));
            Assert.That(driver.Url, Is.EqualTo("/GotoPage"));
            Assert.That(titleElement.Text, Is.StringContaining("GotoPage"));
        }
    }
}

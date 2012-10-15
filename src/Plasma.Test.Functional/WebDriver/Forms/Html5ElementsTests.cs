using NUnit.Framework;
using OpenQA.Selenium;
using Plasma.WebDriver;

namespace Plasma.Test.Functional.WebDriver.Forms
{
    [TestFixture]
    public class Html5ElementsTests
    {
        private PlasmaDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = TestFixture.Driver;
            _driver.Navigate().GoToUrl("/Html5Elements/");
        }

        [Test]
        public void ShouldBeAbleToPostEmailInputType()
        {
            const string value = "email@adress.com";
            _driver.FindElement(By.Name("emailBox")).SendKeys(value);
            _driver.FindElement(By.TagName("form")).Submit();
            Assert.That(_driver.FindElement(By.Id("emailBoxValue")).Text, Is.StringContaining(value), _driver.PageSource);
        }

        [Test]
        public void ShouldBeAbleToPostNumberInputType()
        {
            const string value = "123";
            _driver.FindElement(By.Name("numberBox")).SendKeys(value);
            _driver.FindElement(By.TagName("form")).Submit();
            Assert.That(_driver.FindElement(By.Id("numberBoxValue")).Text, Is.StringContaining(value), _driver.PageSource);
        }
    }
}
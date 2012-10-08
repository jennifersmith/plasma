using NUnit.Framework;
using OpenQA.Selenium;
using Plasma.WebDriver;

namespace Plasma.Test.Functional.WebDriver.Forms
{
    [TestFixture]
    public class Html5ElementsTests
    {
        private PlasmaDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = TestFixture.Driver;
            driver.Navigate().GoToUrl("/Html5Elements/");
        }

        [Test]
        public void ShouldBeAbleToPostEmailInputType()
        {
            const string value = "email@adress.com";
            driver.FindElement(By.Name("emailBox")).SendKeys(value);
            driver.FindElement(By.TagName("form")).Submit();
            Assert.That(driver.FindElement(By.Id("emailBoxValue")).Text, Is.StringContaining(value), driver.PageSource);
        }

        [Test]
        public void ShouldBeAbleToPostNumberInputType()
        {
            const string value = "123";
            driver.FindElement(By.Name("numberBox")).SendKeys(value);
            driver.FindElement(By.TagName("form")).Submit();
            Assert.That(driver.FindElement(By.Id("numberBoxValue")).Text, Is.StringContaining(value), driver.PageSource);
        }
    }
}
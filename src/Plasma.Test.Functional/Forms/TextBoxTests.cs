using NUnit.Framework;
using OpenQA.Selenium;
using Plasma.WebDriver;

namespace Plasma.Test.Functional.Forms
{
    [TestFixture]
    public class TextBoxTests
    {
        private PlasmaDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = TestFixture.Driver;
            driver.Navigate().GoToUrl("/FormWithTextBox/");
        }

        [Test]
        public void ShouldBeAbleToSetValueOfATextBox()
        {
            const string value = "the value";
            driver.FindElement(By.Name("textBox")).SendKeys(value);
            driver.FindElement(By.TagName("form")).Submit();
            Assert.That(driver.FindElement(By.Id("textBoxValue")).Text, Is.StringContaining(value), driver.PageSource);
        }

        [Test]
        public void ShouldBeAbleToClearTheValue()
        {
            const string value = "the value";
            driver.FindElement(By.Name("textBox")).SendKeys(value);
            driver.FindElement(By.Name("textBox")).Clear();
            Assert.That(driver.FindElement(By.Name("textBox")).GetAttribute("value"), Is.Empty, driver.PageSource);
        }
    }
}

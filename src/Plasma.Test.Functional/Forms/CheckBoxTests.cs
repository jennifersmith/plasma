using NUnit.Framework;
using OpenQA.Selenium;
using Plasma.WebDriver;

namespace Plasma.Test.Functional.Forms
{
    [TestFixture]
    public class CheckBoxTests
    {
        private PlasmaDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = TestFixture.Driver;
            driver.Navigate().GoToUrl("/FormWithCheckBox/");
        }

        [Test]
        public void ShouldBeAbleToCheckACheckBox()
        {
            driver.FindElement(By.Name("checkBox")).Click();
            driver.FindElement(By.TagName("form")).Submit();
            Assert.That(driver.FindElement(By.Id("checkBoxValue")).Text, Is.StringContaining("True"), driver.PageSource);
        }

        [Test, Ignore("This is yet to be implemented. Found this edge case while writing tests.")]
        public void ShouldBeAbleToClearTheValue()
        {
            driver.FindElement(By.Name("checkBox")).Click();
            driver.FindElement(By.Name("checkBox")).Clear();
            Assert.That(driver.FindElement(By.Name("checkBox")).Selected, Is.False, driver.PageSource);            
        }
    }
}
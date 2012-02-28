using NUnit.Framework;
using OpenQA.Selenium;

namespace Plasma.Test.Functional.WebDriver.Navigation
{
    [TestFixture]
    public class FollowingLinkTests
    {
        [Test]
        public void ShouldNavigateToTheGivenUrl()
        {
            TestFixture.Driver.Navigate().GoToUrl("/GotoPage");
            TestFixture.Driver.FindElement(By.TagName("a")).Click();
            Assert.That(TestFixture.Driver.Title, Is.StringContaining("OtherPage"));
        }
    }
}

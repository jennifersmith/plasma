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
using NUnit.Framework;
using OpenQA.Selenium;
using Plasma.WebDriver;

namespace Plasma.Test.Functional.WebDriver.Forms
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

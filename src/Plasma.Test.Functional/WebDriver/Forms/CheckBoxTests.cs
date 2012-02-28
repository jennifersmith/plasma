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
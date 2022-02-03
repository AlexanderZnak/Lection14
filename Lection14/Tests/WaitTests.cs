using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Lection14.Tests
{
    public class WaitTests : BaseTest
    {
        // CheckBox's locators
        private readonly By _removeButton = By.XPath("//*[text()='Remove']");
        private readonly By _checkBoxMessage = By.XPath("//*[text()=\"It's gone!\"]");
        private readonly By _checkBox = By.CssSelector("[type=checkbox]");

        // Enable's locators
        private readonly By _inputField = By.CssSelector("[type=text]");
        private readonly By _enableButton = By.XPath("//*[text()='Enable']");
        private readonly By _inputMessage = By.XPath("//*[text()=\"It's enabled!\"]");

        [Test]
        public void CheckboxIsRemoved_ImplicitWait()
        {
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            Driver.Navigate().GoToUrl(HerokuUrl + "dynamic_controls");

            var removeButton = Driver.FindElement(_removeButton);
            removeButton.Click();

            var checkBoxDeletedMessage = Driver.FindElement(_checkBoxMessage).Text;

            Assert.AreEqual("It's gone!", checkBoxDeletedMessage);
        }

        [Test]
        public void CheckboxIsRemoved_ExplicitWait()
        {
            var webDriverWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            Driver.Navigate().GoToUrl(HerokuUrl + "dynamic_controls");
            var isPageLoaded = webDriverWait.Until(d =>
            {
                var result = ((IJavaScriptExecutor) d).ExecuteScript("return document.readyState");
                return result.Equals("complete");
            });
            
            PageLoadWait();

            // var removeButton = Driver.FindElement(_removeButton);
            // removeButton.Click();
            //
            // var checkBoxDeletedMessage = webDriverWait.Until(ExpectedConditions.AlertIsPresent);
            // var textCheckBoxMessage = Driver.FindElement(_checkBoxMessage).Text;
            //
            // Assert.Multiple((() =>
            // {
            //     Assert.IsTrue(checkBoxDeletedMessage);
            //     Assert.AreEqual("It's gone!", textCheckBoxMessage);
            // }));
        }

        public void PageLoadWait()
        {
            var webDriverWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            try
            {
                var isPageLoaded = webDriverWait.Until(d =>
                {
                    var result = ((IJavaScriptExecutor) d).ExecuteScript("return document.readyState");
                    return result.Equals("complete");
                });
            }
            catch (WebDriverTimeoutException e)
            {
                Console.WriteLine(e);
            }
        }

        [Test]
        public void CheckboxIsRemoved_FluentWait()
        {
            // DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(Driver);
            var fluentWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(11));
            fluentWait.Timeout = TimeSpan.FromSeconds(5);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);
            /* Ignore the exception - NoSuchElementException that indicates that the element is not present */
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            // DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(Driver);
        }

        [Test]
        public void InputIsEnabled()
        {
            Driver.Navigate().GoToUrl(HerokuUrl + "dynamic_controls");
        }
    }
}
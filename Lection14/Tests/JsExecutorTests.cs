using System;
using System.IO;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Lection14.Tests
{
    public class JsExecutorTests : BaseTest
    {
        // CheckBox's locators
        private readonly By _removeButton = By.XPath("//*[text()='Remove']");
        private readonly By _checkBoxMessage = By.XPath("//*[text()=\"It's gone!\"]");
        private readonly By _checkBox = By.CssSelector("[type=checkbox]");

        // Enable's locators
        private readonly By _inputField = By.CssSelector("[type=text]");
        private readonly By _enableButton = By.XPath("//*[text()='Enable']");
        private readonly By _inputMessage = By.XPath("//*[text()=\"It's enabled!\"]");

        // Main Page locators
        private readonly By _seleniumLink = By.XPath("//*[text()='Elemental Selenium']");

        [Test]
        public void Click_JsExecutor()
        {
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(8);
            Driver.Navigate().GoToUrl(HerokuUrl + "dynamic_controls");

            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor) Driver;
            
            var removeButton = Driver.FindElement(_removeButton);
            jsExecutor.ExecuteScript("arguments[0].click();", removeButton);

            var checkBoxDeletedMessageElement = Driver.FindElement(_checkBoxMessage);
            var checkBoxDeletedMessage =
                (string) jsExecutor.ExecuteScript("arguments[0].textContent;", checkBoxDeletedMessageElement);
            
            Assert.AreEqual("It's gone!", checkBoxDeletedMessage);
        }

        [Test]
        public void SendKeys()
        {
            Driver.Navigate().GoToUrl(HerokuUrl + "dynamic_controls");
            var driverWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(7));
            var jsExecutor = (IJavaScriptExecutor) Driver;

            var isEnabled = Driver.FindElement(_inputField).Enabled;
            Driver.FindElement(_enableButton).Click();

            isEnabled = driverWait.Until(d => d.FindElement(_inputField).Enabled);

            if (isEnabled)
            {
                var inputElement = Driver.FindElement(_inputField);
                jsExecutor.ExecuteScript("arguments[0].value = 'Hello World'", inputElement);
            }
        }

        [Test]
        public void ScrollToElement_JavaScriptExecutor()
        {
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(8);
            Driver.Navigate().GoToUrl(HerokuUrl);
            var jsExecutor = (IJavaScriptExecutor) Driver;

            var seleniumLinkElement = Driver.FindElement(_seleniumLink);
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", seleniumLinkElement);
            jsExecutor.ExecuteScript("arguments[0].style.border = '3px solid red';", seleniumLinkElement);
            seleniumLinkElement.Click();
        }

        [Test]
        public void GetCheckBoxState_JavaScriptExecutor()
        {
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(8);
            Driver.Navigate().GoToUrl(HerokuUrl + "dynamic_controls");
            var jsExecutor = (IJavaScriptExecutor) Driver;

            var checkBoxElement = Driver.FindElement(_checkBox);
            var isChecked = (bool) jsExecutor.ExecuteScript(
                "return (function (element) {return element.checked;})(arguments[0]);",
                checkBoxElement);
            checkBoxElement.Click();
            isChecked = (bool) jsExecutor.ExecuteScript(
                "return (function (element) {return element.checked;})(arguments[0]);",
                checkBoxElement);
        }

        [Test]
        public void InputFile_JavaScriptExecutor()
        {
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(8);
            Driver.Navigate().GoToUrl(HerokuUrl + "upload");
            var jsExecutor = (IJavaScriptExecutor) Driver;

            var jsDropFile =
                "var target = arguments[0]," +
                "    offsetX = arguments[1]," +
                "    offsetY = arguments[2]," +
                "    document = target.ownerDocument || document," +
                "    window = document.defaultView || window;" +
                "" +
                "var input = document.createElement('INPUT');" +
                "input.type = 'file';" +
                "input.style.display = 'none';" +
                "input.onchange = function () {" +
                "  var rect = target.getBoundingClientRect()," +
                "      x = rect.left + (offsetX || (rect.width >> 1))," +
                "      y = rect.top + (offsetY || (rect.height >> 1))," +
                "      dataTransfer = { files: this.files };" +
                "" +
                "  ['dragenter', 'dragover', 'drop'].forEach(function (name) {" +
                "    var evt = document.createEvent('MouseEvent');" +
                "    evt.initMouseEvent(name, !0, !0, window, 0, 0, 0, x, y, !1, !1, !1, !1, 0, null);" +
                "    evt.dataTransfer = dataTransfer;" +
                "    target.dispatchEvent(evt);" +
                "  });" +
                "" +
                "  setTimeout(function () { document.body.removeChild(input); }, 25);" +
                "};" +
                "document.body.appendChild(input);" +
                "return input;";

            var input = (IWebElement) jsExecutor.ExecuteScript(jsDropFile,
                Driver.FindElement(By.Id("drag-drop-upload")), 0, 0);

            input.SendKeys(@"C:\Users\Aliaksandr.Znak\RiderProjects\Lection14\Lection14\NewFile1.txt");
        }
    }
}
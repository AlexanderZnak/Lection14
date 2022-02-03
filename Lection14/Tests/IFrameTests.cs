using System;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Lection14.Tests
{
    public class IFrameTests : BaseTest
    {
        [Test]
        public void GetTextFromIFrame()
        {
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(11);
            Driver.Navigate().GoToUrl(HerokuUrl + "iframe");
            var iframeElement = Driver.FindElement(By.Id("mce_0_ifr"));
            Driver.SwitchTo().Frame("mce_0_ifr");
            
            var textIframeElement = Driver.FindElement(By.Id("tinymce"));
            var text = textIframeElement.Text;

            Driver.SwitchTo().DefaultContent();

            Assert.AreEqual("Your content goes here.", text);
        }
    }
}
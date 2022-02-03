using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Lection14.Tests
{
    public abstract class BaseTest
    {
        protected IWebDriver Driver;
        protected readonly string HerokuUrl = "http://the-internet.herokuapp.com/"; 

        [SetUp]
        protected void InitializeDriver()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();
        }
        
        [TearDown]
        public void TearDown()
        {
            Driver.Quit();
        }
    }
}
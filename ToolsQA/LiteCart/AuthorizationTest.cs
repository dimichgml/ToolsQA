using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace ToolsQA.LiteCart
{
    [TestFixture]
    class AuthorizationTest
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [SetUp]
        public void Initialize()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }
        [Test]
        public void TestApp()
        {
            _driver.Url = "http://localhost/litecart/admin/";
            _driver.FindElement(By.Name("username")).SendKeys("admin");
            _driver.FindElement(By.Name("password")).SendKeys("admin");
            _driver.FindElement(By.Name("remember_me")).Click();
            _driver.FindElement(By.Name("login")).Click();
        }
        [TearDown]
        public void EndTest()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}

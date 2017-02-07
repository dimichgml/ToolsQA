using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ToolsQA.LiteCart
{
    [TestFixture]
    class AdminTest
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [SetUp]
        public void Initialize()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
        }
        [Test]
        public void TestAuthorization()
        {
            _driver.Url = "http://localhost/litecart/admin/";
            _driver.FindElement(By.Name("username")).SendKeys("admin");
            _driver.FindElement(By.Name("password")).SendKeys("admin");
            _driver.FindElement(By.Name("remember_me")).Click();
            _driver.FindElement(By.Name("login")).Click();
        }
        [Test]
        public void NemuClick()
        {
            /*Т.к. после каждого клика страница обновляется, приходится вызывать поиск все элементов после каждого клика. При этом обращаться к элементу, который следующий
             * по списку.
             * Если существуют элементы подменю, их прокликиваем по тому же принципу, что и главное меню.
             * В случае, если не обнаружен на странице заголовок (то есть элемента с тегом h1), то результат теста false
             */
            TestAuthorization();
            int count = _driver.FindElements(By.CssSelector("#app- > a")).Count;
            List<IWebElement> _menu;
            for (int i = 0; i < count; i++)
            {
                _menu = _driver.FindElements(By.CssSelector("#app- > a")).ToList();
                _menu[i].Click();                                
                Assert.False(!IsElementExists(By.TagName("h1")));
                if (IsElementExists(By.CssSelector("#app- > ul > li")))
                {
                    int subCount = _driver.FindElements(By.CssSelector("#app- > ul > li")).Count;
                    if (subCount > 0)
                    {
                        List<IWebElement> submenu;
                        for (int j = 0; j < subCount; j++)
                        {
                            submenu = _driver.FindElements(By.CssSelector("#app- > ul > li")).ToList();
                            submenu[j].Click();                            
                            Assert.False(!IsElementExists(By.TagName("h1")));
                        }
                    }

                }                            
            }
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        }
        [TearDown]
        public void EndTest()
        {
            _driver.Quit();
            _driver = null;
        }

        public bool IsElementExists(By iCssSelector)
        {
            try
            {
                _driver.FindElement(iCssSelector);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}

using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace ToolsQA.LiteCart
{
    class StickerTest
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
        public void GoToURL(string url)
        {
            _driver.Url = url;
        }
        [Test]
        public void StikerFinder()
        {
            /*
             * Т.к. кликать не будем, то страничка у нас не изменится. Ищем все товары. Дальше пробегаемся по всем товарам.
             * Проверяем количество стикеров для этого товара. Если больше 1 - выводим ошибку.
             */
            GoToURL("http://localhost/litecart/en/");
            List<IWebElement> _items;
            List<IWebElement> _stickers;
            _items = _driver.FindElements(By.CssSelector("ul.listing-wrapper.products > li")).ToList();
            for (int i = 0; i < _items.Count; i++)
            {
                _stickers = _items[i].FindElements(By.CssSelector("div > div.sticker")).ToList();
                Assert.False(_stickers.Count > 1, "More than one sticker per item this location (X,Y): " + _items[i].Location.X.ToString() + ", " + _items[i].Location.Y.ToString());
            }
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



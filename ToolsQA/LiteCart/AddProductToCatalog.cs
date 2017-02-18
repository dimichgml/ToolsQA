using System;
using System.Reflection;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using NUnit.Framework;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Security.Policy;

namespace ToolsQA.LiteCart
{
    class AddProductToCatalog
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private string driverName;
        [SetUp]
        public void Initialize()
        {

            _driver = new ChromeDriver();            
           
            driverName = _driver.GetType().Name;

        }
        public void GoToURL(string url)
        {
            _driver.Url = url;
        }

        [Test]
        public void AdminAuthorization()
        {
            _driver.Url = "http://localhost/litecart/admin/";
            _driver.FindElement(By.Name("username")).SendKeys("admin");
            _driver.FindElement(By.Name("password")).SendKeys("admin");
            _driver.FindElement(By.Name("remember_me")).Click();
            _driver.FindElement(By.Name("login")).Click();
        }
        [Test]
        public void CreateProduct()
        {
            /*
             * Выполняем авторизацию в админке.
             * Переходим на страницу каталог. Нажимаем на кнопку добавления нового продукта.
             * Заполняем поля, переходим во вкладку цены. Там тоже заполнямем поля. Жмём кнопку Save. Ожидаем 3 секунды. 
             * Проверяем наличие товара. Проверяем название этого товара - если среди товаров нет созданого, то тест результат теста "неудачный".
             */

            #if DEBUG
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug",""),
                "Images", "0180801_PE332785_S5.JPG");
            #else
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Release",""),
                "Images", "0180801_PE332785_S5.JPG");
            #endif
            IWebElement _inputQuantity;
            IWebElement _inputPurchasePrice;
            IWebElement _selectPurchasePriceCurrenCode;
            IWebElement _selectManufacturerID;          
            AdminAuthorization();
            _driver.FindElement(By.CssSelector("ul.list-vertical > li:nth-child(2)")).Click();
            _driver.FindElement(By.CssSelector("#content > div:nth-child(2) > a:nth-child(2)")).Click();
            _driver.FindElement(By.CssSelector("td > label:nth-child(3)")).Click();
            _driver.FindElement(By.CssSelector("input[name*=name]")).SendKeys("Super table");
            _driver.FindElement(By.CssSelector("input[name=code]")).SendKeys("122-dfs-4434");
            _driver.FindElement(By.CssSelector("tr:nth-child(4) > td:nth-child(1) > input[type=checkbox]")).Click();
            _driver.FindElement(By.CssSelector("input[name=quantity]")).Click();
            _inputQuantity = _driver.FindElement(By.CssSelector("input[name=quantity]"));
            _inputQuantity.Click();
            new Actions(_driver)
                .KeyDown(Keys.Control)
                .SendKeys("a")
                .KeyUp(Keys.Control)
                .Perform();
            _inputQuantity.SendKeys("2");
            _driver.FindElement(By.CssSelector("input[type=file]")).SendKeys(filePath);
            _driver.FindElement(By.CssSelector("input[name=date_valid_from]")).SendKeys("18.02.2017");
            _driver.FindElement(By.CssSelector("input[name=date_valid_to]")).SendKeys("28.02.2017");
            _driver.FindElement(By.CssSelector("ul.index > li:nth-child(4)")).Click();           
            _inputPurchasePrice = _driver.FindElement(By.CssSelector("input[name=purchase_price]"));
            _inputPurchasePrice.Click();
            new Actions(_driver)
                .KeyDown(Keys.Control)
                .SendKeys("a")
                .KeyUp(Keys.Control)
                .Perform();
            _inputPurchasePrice.SendKeys("49,99");
            _selectPurchasePriceCurrenCode = _driver.FindElement(By.CssSelector("select[name=purchase_price_currency_code]"));
            _selectPurchasePriceCurrenCode.Click();
            _selectPurchasePriceCurrenCode.SendKeys("US" + Keys.Enter);
            _driver.FindElement(By.CssSelector("input[name='prices[USD]'")).SendKeys("52");
            _driver.FindElement(By.CssSelector("input[name='prices[EUR]'")).SendKeys("80");
            _driver.FindElement(By.CssSelector("ul.index > li:nth-child(2)")).Click();       
            _selectManufacturerID = _driver.FindElement(By.CssSelector("select[name=manufacturer_id]"));
            _selectManufacturerID.Click();
            _selectManufacturerID.SendKeys("A" + Keys.Enter);
            _driver.FindElement(By.CssSelector("input[name=keywords]"));
            _driver.FindElement(By.CssSelector("input[name='short_description[en]']")).SendKeys("Bla-bla");
            _driver.FindElement(By.CssSelector("div.trumbowyg-editor")).SendKeys("Bla-bla-bla-bla-bla-bla-bla-bla-bla-bla-bla");_driver.FindElement(By.CssSelector("div.trumbowyg-editor")).SendKeys("Bla-bla-bla-bla-bla-bla-bla-bla-bla-bla-bla");
            _driver.FindElement(By.CssSelector("input[name='head_title[en]'")).SendKeys("text head");
            _driver.FindElement(By.CssSelector("input[name='meta_description[en]'")).SendKeys("text meta");
            _driver.FindElement(By.CssSelector("button[name=save")).Click();
            Thread.Sleep(3000);
            List<IWebElement> _items;
            if (IsElementExists(By.CssSelector("td:nth-child(3) > a")))
            {
                string _nameProduct;                
                _items = _driver.FindElements(By.CssSelector("td:nth-child(3) > a")).ToList();
                bool flag = false;
                foreach (var _item in _items)
                {
                    _nameProduct = _item.GetAttribute("textContent");
                    if (_nameProduct == "Super table")
                    {
                        flag = true;
                        break;
                    }
                    
                }
                Assert.True(flag, "Product wasn't created");
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

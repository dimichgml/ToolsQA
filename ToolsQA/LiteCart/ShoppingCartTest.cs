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
    class ShoppingCartTest
    {
        private IWebDriver _driver;
        private WebDriverWait wait;
        private string driverName;
        [SetUp]
        public void Initialize()
        {

            _driver = new ChromeDriver();
            driverName = _driver.GetType().Name;
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _driver.Navigate().Refresh();

        }
        public void GoToURL(string url)
        {
            _driver.Url = url;
        }

        [Test]
        public void AddToCartAndDelete()
        {
            /*
             * Счётчик не пропадает. Его id до добавления товара и после добавления товара один и тот же. Меняется только свойство textContent.
             * Потому ждём появление увеличенного на 1 textContent элемента.
             * Переходим в корзину. Ждём пока прогрузиться страница (появится кнопка удалить). Запоминаем количество строк с SKU товара в таблице.
             * В цикле выполняем удаление товара и ждём пока обновится таблица. Здесь использовал самописаное явное ожидание изменения количества строк с SKU товара в таблице.             
             */
            IWebElement _quantity;           
            IWebElement _selectSize;
            int amount, amount2, amountCart;
            
            GoToURL("http://localhost/litecart/en/");
            for (int i = 0; i < 3; i++)
            {
                _driver.FindElement(By.CssSelector("#box-most-popular > div > ul > li:nth-child(1)")).Click();
                if (IsElementExists(By.CssSelector("select[name='options[Size]']")))
                {
                    _selectSize = _driver.FindElement(By.CssSelector("select[name='options[Size]']"));
                    _selectSize.Click();
                    _selectSize.SendKeys("S" + Keys.Enter);

                }                
                _driver.FindElement(By.CssSelector("button[name=add_cart_product]")).Click();
                _quantity = _driver.FindElement(By.CssSelector("span.quantity"));
                amount = Int32.Parse(_quantity.GetAttribute("textContent").ToString());
                wait.Until(ExpectedConditions.TextToBePresentInElement(_quantity, (amount + 1).ToString()));
                //amount2 = Int32.Parse(_quantity.GetAttribute("textContent").ToString());
                //Assert.False(amount == amount2, "Ooops");
                _driver.FindElement(By.CssSelector("#logotype-wrapper")).Click();                
            }
            _driver.FindElement(By.CssSelector("#cart > a.link")).Click();
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("button[name=remove_cart_item]")));
            amountCart = _driver.FindElements(By.CssSelector("table.dataTable.rounded-corners > tbody > tr > td.sku")).Count;
            int count = 0, amountCart2;
            while (amountCart > 0)
            {
                _driver.FindElement(By.CssSelector("button[name=remove_cart_item]")).Click();
                for (int i = 0;; i++)
                {
                    if (i >= 5)
                        throw new TimeoutException();
                    amountCart2 = _driver.FindElements(By.CssSelector("table.dataTable.rounded-corners > tbody > tr > td.sku")).Count;
                    if (amountCart2 == amountCart - 1)
                    {
                        amountCart = amountCart2;
                        break;
                    }
                    Thread.Sleep(1000);
                }                                                
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

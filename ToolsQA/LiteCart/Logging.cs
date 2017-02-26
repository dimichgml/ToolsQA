using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System.Collections.Generic;
using OpenQA.Selenium.Remote;
using System.Linq;


namespace ToolsQA.LiteCart
{
    class Logging
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
        public void GoToURL(string url)
        {
            _driver.Url = url;
        }

        public void GetBrowserLogs()
        {
            List<LogEntry> logs;        
            logs = _driver.Manage().Logs.GetLog("browser").ToList();
            foreach (LogEntry l in logs)
            {
                Console.WriteLine(l);
            }
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
        public void GoodsLogs()
        {
            //Выполнил авторизацию. Получить все товары (без товаров в подкатегориях). Получить ссылки на страницы товаров. Пройтись по всем ссылкам в цикле.
            //Ожидаем загрузки страницы путём ожидания локатора вкладки General. Выполняем выводов логов браузера на консоль. Возвращаемся к странице с списком товаров (хотя это совсем не обязательно)
            TestAuthorization();
            List<IWebElement> _linkItems;
            List<string> _links = new List<string>();            
            GoToURL("http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1");
            _linkItems = _driver.FindElements(By.CssSelector("td:nth-child(3) > a[href*='category_id=1']")).ToList();
            foreach (var _linkItem in _linkItems)
            {
                _links.Add(_linkItem.GetAttribute("href"));
            }
            foreach (var _link in _links)
            {
                GoToURL(_link);
                _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#content > form > div > ul > li.active > a")));
                GetBrowserLogs();
                GoToURL("http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1");
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

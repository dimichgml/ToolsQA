using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Threading;

namespace ToolsQA.Images
{
    class CountriesLinksTest
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
        public void TestAuthorization()
        {
            _driver.Url = "http://localhost/litecart/admin/";
            _driver.FindElement(By.Name("username")).SendKeys("admin");
            _driver.FindElement(By.Name("password")).SendKeys("admin");
            _driver.FindElement(By.Name("remember_me")).Click();
            _driver.FindElement(By.Name("login")).Click();
        }       
        [Test]
        public void CountiesLinksTest()
        {
            /*
             Авторизуемся в админке, переходим на страницу со странами. Ищем все ссылки. Проходим в цикле по всем ссылкам.
             Для ожидания нового окна браузера используется самописное явное ожидание. В нём мы каждую секудну получаем список новых окон, из него удаляем список окон до клика по линке. 
             В определённый момент времени в списке остаётся новое окно. Переключаемся в новое окно. Закрываем его. Переключачемся в прежнее окно.
             */
            List<IWebElement> _links;
            TestAuthorization();
            int amountWindows = 0;
            GoToURL("http://localhost/litecart/admin/?app=countries&doc=countries");
            _driver.FindElement(By.CssSelector("tr:nth-child(2) > td:nth-child(7) > a")).Click();
            _links = _driver.FindElements(By.CssSelector("td > a[target=_blank]")).ToList();
            string mainWindow = _driver.CurrentWindowHandle;           
            var oldWindows = new List<string>(_driver.WindowHandles);            
            foreach (var _link in _links)
            {                                               
                string newWindow;
                _link.Click();            
                for (int i = 0; ; i++)
                {
                    if (i >= 5)
                        throw new TimeoutException();                    
                    var newWindows = new List<string>(_driver.WindowHandles);                    
                    foreach (var oldWindow in oldWindows)
                    {
                        newWindows.Remove(oldWindow);
                    }                
                    amountWindows = newWindows.Count;
                    if (amountWindows > 0)
                    {
                        newWindow = newWindows[0];
                        newWindows.Clear();
                        break;   
                    }
                    Thread.Sleep(1000);
                }                
                _driver.SwitchTo().Window(newWindow);
                newWindow = "";
                //Thread.Sleep(1000);
                _driver.Close();
                _driver.SwitchTo().Window(mainWindow);
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

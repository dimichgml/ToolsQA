using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Security.Policy;

namespace ToolsQA.LiteCart
{
    class CountryListProperties
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
        public void TestAuthorization()
        {
            _driver.Url = "http://localhost/litecart/admin/";
            _driver.FindElement(By.Name("username")).SendKeys("admin");
            _driver.FindElement(By.Name("password")).SendKeys("admin");
            _driver.FindElement(By.Name("remember_me")).Click();
            _driver.FindElement(By.Name("login")).Click();
        }
        [Test]
        public void SortCountryCheck()
        {
            /*Выполняем выборку всех ячеек колонки с странами. Получаем свойство текстконтекта для каждого элемента выборки
             * Выполняем проверку методом сравнения элементов списка. Если сортировка не по возврастанию - сообщаем об ошибке.
             */
            TestAuthorization();
            List<IWebElement> _items;
            List<string> _countries;          
            _countries = new List<string>();
            GoToURL("http://localhost/litecart/admin/?app=countries&doc=countries");
            _items =  _driver.FindElements(By.CssSelector("td:nth-child(5) > a")).ToList();
            foreach (var _item in _items)
            {
                _countries.Add(_item.GetAttribute("textContent").ToString());    
            }
            Assert.False(!IsSort(_countries), "List contries sort by Z-A");
        }
        [Test]
        public void SortCountryCheckWithGeoZon()
        {
            /*Выполняем выборку всех ячеек колонки с странами и геозонами в различные списки. Выполняем поиск стран с геозонами отличными от 0.
             * Если таковые имеются - запоминаем ссылку на эту страну.
             * Переходим в цикле по ссылкам собранным в результате поиска. Проверяем сортировку аналогично public void SortCountryCheck() 
          */
            TestAuthorization();
            List<IWebElement> _geoitems;
            List<IWebElement> _countries;
            List<IWebElement> _geozones;
            List<string> _geocountries;
            List<string> _links;
            int geoCount;
            _geocountries = new List<string>();
            _links = new List<string>();
            GoToURL("http://localhost/litecart/admin/?app=countries&doc=countries");
            _geoitems = _driver.FindElements(By.CssSelector("td:nth-child(6)")).ToList();
            _countries = _driver.FindElements(By.CssSelector("td:nth-child(5) > a")).ToList();
            for (int i = 0; i < _geoitems.Count; i++)
            {
                geoCount = 0;
                geoCount = Int32.Parse(_geoitems[i].GetAttribute("textContent").ToString());
                if (geoCount > 0)
                {
                    _links.Add(_countries[i].GetAttribute("href").ToString());
                }
            }
            foreach (var _link in _links)
            {
                 
                GoToURL(_link);
                _geozones = _driver.FindElements(By.CssSelector("table-zones > tbody > tr:nth-child(2) > td:nth-child(3)")).ToList();
                foreach (var _geozone in _geozones)
                {
                    _geocountries.Add(_geozone.GetAttribute("textContent").ToString());
                }
                Assert.False(!IsSort(_geocountries), "List contries sort by Z-A");
            }

        }

        [Test]
        public void SortCountryInGeozones()
        {
            /*
             * Выполняем поиск ссылок на страницы геозон стран. Переходим на эти страницы в цикле. Находим все комбобоксы на странице. Для каждого комбобокса
             * проверяем свойство выбранного элемента. Записываем этот элмент в список. По окончанию сбора данных выбранных списков выполняем проверку 
             * выборки сортировки в алфавитном порядке.
             */
            TestAuthorization();
            List<IWebElement> _countries;
            List<IWebElement> _comboboxes;
            List<IWebElement> _options;
            List<string> _links;
            List<string> _selection;
            _links = new List<string>();
            _selection = new List<string>();
            GoToURL("http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones");
            _countries = _driver.FindElements(By.CssSelector("td:nth-child(3) > a")).ToList();
            for (int i = 0; i < _countries.Count; i++)
            {
                _links.Add(_countries[i].GetAttribute("href").ToString());                
            }
            foreach (var _link in _links)
            {
                GoToURL(_link);
                _selection.Clear();
                _comboboxes = _driver.FindElements(By.CssSelector("td:nth-child(3) > select")).ToList();                
                foreach (var _combobox in _comboboxes)
                {
                    _options = null;
                    _options = _combobox.FindElements(By.CssSelector("option")).ToList();
                    foreach (var _option in _options)
                    {                       
                        if (Convert.ToBoolean(_option.GetAttribute("selected")))
                        {                            
                            _selection.Add(_option.GetAttribute("textContent").ToString());
                        }
                    }
                }
                Assert.False(!IsSort(_selection), "List contries sort by Z-A");               
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
        public bool IsSort(List<string> _items)
        {
            bool desc = true;
            for (int i = 1; i < _items.Count; i++)
            {
                if (_items[i].CompareTo(_items[i - 1]) < 0)
                {
                    desc = false;
                    break;
                }
            }
            return desc;
        }
    }
}

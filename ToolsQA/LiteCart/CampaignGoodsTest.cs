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

namespace ToolsQA.LiteCart
{
    class CampaignGoodsTest
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private string driverName;
        [SetUp]
        public void Initialize()
        {
          /*
            _driver = new ChromeDriver();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));

           */
            FirefoxOptions options = new FirefoxOptions();
            options.UseLegacyImplementation = false;
            _driver = new FirefoxDriver(options);

            driverName = _driver.GetType().Name;

        }       
        public void GoToURL(string url)
        {
            _driver.Url = url;
        }
        [Test]
        public void GoodsNameTest()
        {
            /*
             *На главной странице получаем название товара. Переходим на страницу товара и там получаем название. Производим сравнение.
             */
            GoToURL("http://localhost/litecart/en/");
            string _goodsNameMain = "";
            string _goodsNamePage = "";
            string _link = "";
            _goodsNameMain = _driver.FindElement(By.CssSelector("#box-campaigns > div > ul > li > a.link > div.name")).GetAttribute("textContent");            
            _link = _driver.FindElement(By.CssSelector("#box-campaigns > div > ul > li > a.link")).GetAttribute("href");
            GoToURL(_link);
            _goodsNamePage = _driver.FindElement(By.CssSelector("#box-product > div:nth-child(1) > h1")).GetAttribute("textContent");
            Assert.False(!(_goodsNameMain == _goodsNamePage), "Goods name is different");               
        }
        [Test]
        public void GoodsTadsTest()
        {
            /*
             * На главной странице получаем старую цену и цену распродажи. На странице товара получаем старую цену и цену распродажи. Сравниваем цены на разных страницах.
             */
            GoToURL("http://localhost/litecart/en/");
            string _goodsTagMain = "";
            string _goodsTagSaleMain = "";
            string _goodsTagPage = "";
            string _goodsTagSalePage = "";
            string _link = "";
            _goodsTagMain = _driver.FindElement(By.CssSelector("#box-campaigns > div > ul > li > a.link > div.price-wrapper > s")).GetAttribute("textContent");
            _goodsTagSaleMain = _driver.FindElement(By.CssSelector("#box-campaigns > div > ul > li > a.link > div.price-wrapper > strong")).GetAttribute("textContent");
            _link = _driver.FindElement(By.CssSelector("#box-campaigns > div > ul > li > a.link")).GetAttribute("href");
            GoToURL(_link);
            _goodsTagPage = _driver.FindElement(By.CssSelector("#box-product > div.content > div.information > div.price-wrapper > s")).GetAttribute("textContent");
            _goodsTagSalePage = _driver.FindElement(By.CssSelector("#box-product > div.content > div.information > div.price-wrapper > strong")).GetAttribute("textContent");            
            Assert.False(!(_goodsTagMain == _goodsTagPage), "Goods tag is different");
            Assert.False(!(_goodsTagSalePage == _goodsTagSaleMain), "Goods sale tag  is different");
        }
        [Test]
        public void GoodsTadsStyleTest()
        {
            /* Поскольку точных значений стиливых свойств мы определить не можем "красный" или "серый" цвет, например. Условимся, что мы обладаем эталонными значениями
             * и будем сравнивать с ними полученный результат теста.
             * Сравнение выполняем для каждой страницы в отдельности. 
             * Прогон в разных браузерах лишний раз подтвердил, что GetCssValue() для некоторых свойств возвращает различные значения. Потому для различных бразуеров выполняем
             * сравнение с эталоном в зависимости от используемого бразуера.
             * Ещё одно наблюдение. Значение свойства цвета различно при написание этого свойства Color и color. В случае с Chrome, Color вернёт RGB, а color вернёт RGBa.
             * При этом firefox мне упорно возвращает RGB при различных написаниях.
             * Потому решил использовать Color для уменьшения количества проверок на разилчность браузеров.
             */
            string _TagColorMain = "";
            string _TagColorMain1 = "";
            string _TagSaleMain = "";
            string _TagColorPage = "";
            string _TagColorSalePage = "";
            string _TagThroughMain = "";
            string _TagThroughPage = "";
            string _TagSaleBoldMain = "";
            string _TagSaleBoldPage = "";
            string _TagSizeMain = "";            
            string _TagSizeSaleMain = "";
            string _TagSizePage = "";
            string _TagSizeSalePage = "";
            double _TagSizeMainDouble;
            double _TagSizeSaleMainDouble;
            double _TagSizePageDouble;
            double _TagSizeSalePageDouble;
            string _link = "";
            char[] charsToTrim = { 'p', 'x' };
            GoToURL("http://localhost/litecart/en/");            
            _TagColorMain = _driver.FindElement(By.CssSelector("#box-campaigns > div > ul > li > a.link > div.price-wrapper > s")).GetCssValue("Color");
            _TagSaleMain = _driver.FindElement(By.CssSelector("#box-campaigns > div > ul > li > a.link > div.price-wrapper > strong")).GetCssValue("Color");  
            _TagThroughMain = _driver.FindElement(By.CssSelector("#box-campaigns > div > ul > li > a.link > div.price-wrapper > s")).GetCssValue("text-decoration");
            _TagSaleBoldMain = _driver.FindElement(By.CssSelector("#box-campaigns > div > ul > li > a.link > div.price-wrapper > strong")).GetCssValue("font-weight");
            _TagSizeMain = _driver.FindElement(By.CssSelector("#box-campaigns > div > ul > li > a.link > div.price-wrapper > s")).GetCssValue("font-size").Replace(".", ",");
            _TagSizeSaleMain = _driver.FindElement(By.CssSelector("#box-campaigns > div > ul > li > a.link > div.price-wrapper > strong")).GetCssValue("font-size").Replace(".", ",");
            _TagSizeMain = _TagSizeMain.Trim(charsToTrim);
            _TagSizeSaleMain = _TagSizeSaleMain.Trim(charsToTrim);
            _TagSizeMainDouble = Double.Parse(_TagSizeMain);
            _TagSizeSaleMainDouble = Double.Parse(_TagSizeSaleMain);
            Assert.False(!(_TagColorMain == "rgb(119, 119, 119)"), "Color old tag is not gray (Main page)");
            Assert.False(!(_TagSaleMain == "rgb(204, 0, 0)"), "Color sale tag is not red (Main page)");
            Assert.False(!(_TagThroughMain == "line-through"), "Tag without line-through");
            if (driverName == "FirefoxDriver")
            {
                Assert.False(!(_TagSaleBoldMain == "900"), "Tag with sale is not bold");
            }
            if (driverName == "ChromeDriver")
            {
                Assert.False(!(_TagSaleBoldMain == "bold"), "Tag with sale is not bold");
            }
            Assert.False(!(_TagSizeMainDouble < _TagSizeSaleMainDouble), "Sale tag a few (Main page)");
            _link = _driver.FindElement(By.CssSelector("#box-campaigns > div > ul > li > a.link")).GetAttribute("href");            
            GoToURL(_link);
            _TagColorPage = _driver.FindElement(By.CssSelector("#box-product > div.content > div.information > div.price-wrapper > s")).GetCssValue("Color");
            _TagColorSalePage = _driver.FindElement(By.CssSelector("#box-product > div.content > div.information > div.price-wrapper > strong")).GetCssValue("Color");
            _TagThroughPage = _driver.FindElement(By.CssSelector("#box-product > div.content > div.information > div.price-wrapper > s")).GetCssValue("text-decoration");
            _TagSaleBoldPage = _driver.FindElement(By.CssSelector("#box-product > div.content > div.information > div.price-wrapper > strong")).GetCssValue("font-weight");
            _TagSizePage = _driver.FindElement(By.CssSelector("#box-product > div.content > div.information > div.price-wrapper > s")).GetCssValue("font-size").Replace(".", ",");
            _TagSizeSalePage = _driver.FindElement(By.CssSelector("#box-product > div.content > div.information > div.price-wrapper > strong")).GetCssValue("font-size").Replace(".", ",");
            _TagSizePage = _TagSizePage.Trim(charsToTrim);
            _TagSizeSalePage = _TagSizeSalePage.Trim(charsToTrim);
            _TagSizePageDouble = Double.Parse(_TagSizePage);
            _TagSizeSalePageDouble = Double.Parse(_TagSizeSalePage);            
            Assert.False(!(_TagColorPage == "rgb(102, 102, 102)"), "Color old tag is not gray (Doods page)");
            Assert.False(!(_TagColorSalePage == "rgb(204, 0, 0)"), "Color sale tag is not red (Doods page)");
            Assert.False(!(_TagThroughPage == "line-through"), "Tag without line-through (Doods page)");            
            if (driverName == "FirefoxDriver")
            {
                Assert.False(!(_TagSaleBoldPage == "700"), "Tag with sale is not bold (Doods page)");
            }
            if (driverName == "ChromeDriver")
            {
                Assert.False(!(_TagSaleBoldPage == "bold"), "Tag with sale is not bold (Doods page)");
            }
            Assert.False(!(_TagSizePageDouble < _TagSizeSalePageDouble), "Sale tag a few (Doods page)");            
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

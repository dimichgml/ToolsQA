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
    class SignUpTest
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private string driverName;
        [SetUp]
        public void Initialize()
        {
           
            _driver = new ChromeDriver();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            driverName = _driver.GetType().Name;

        }
        public void GoToURL(string url)
        {
            _driver.Url = url;
        }
       
        public void Logout()
        {
            /*Если на странице существует локатор с ссылкой, то получаем эту ссылку и кликаем по ней
             */
        
            IWebElement _logoutLink;
            if (IsElementExists(By.CssSelector("div[id=box-account] > div.content > ul > li:nth-child(4) > a")))
            {
                _logoutLink = _driver.FindElement(By.CssSelector("div[id=box-account] > div.content > ul > li:nth-child(4) > a"));
                _logoutLink.Click();
            }
        }
        
        public void Login(string _login = "litecart01@yandex.ru", string _psw = "zaq123456-")
        {
            /* 
             * Передаются параметры по умолчанию для самостоятельного использования.
             * Ищём и заполняем поля почты и пароля.
             * Кликаем по кнопке Login
             */
            IWebElement _inputEmail;
            IWebElement _inputPassword;
            IWebElement _btnLogin;           
                  
            _inputEmail = _driver.FindElement(By.CssSelector("input[name=email]"));
            _inputEmail.SendKeys(_login);
            _inputPassword = _driver.FindElement(By.CssSelector("input[name=password]"));
            _inputPassword.SendKeys(_psw);
            _btnLogin = _driver.FindElement(By.CssSelector("button[name=login]"));
            _btnLogin.Click();

        }
        [Test]
        public void SignUp()
        {
            /*
             *Переходим на основную страницу магазина. Находим ссылку "New customers click here". Кликаем по ней. Заполняем поля ввода.
             * Для заполнения поля ввода выбора страны сначала кликаем по нему, потом заполняем текстовое поле внутри и жмякаем Enter.
             * Заканчиваем регистрацию нажатием на кнопку Create Account.
             * После регистрации происходит авторизация автоматически.
             * Вызываем метод выхода из аккаунта.
             * Выполняем авторизацию и снова выходим из аккаунта.
             */
            IWebElement _linkCretaAcc;
            IWebElement _inputTaxID;
            IWebElement _inputCompany;
            IWebElement _inputFirstName;
            IWebElement _inputLastName;
            IWebElement _inputAddress1;
            IWebElement _inputPostcode;
            IWebElement _inputCity;
            IWebElement _inputPassword;
            IWebElement _inputConfirmedPassword;
            IWebElement _inputEmail;
            IWebElement _inputPhone;
            IWebElement _selectCountry;            
            IWebElement _inputSelect2SearchF;
            IWebElement _btnCreateAccount;

            GoToURL("http://localhost/litecart/en/");
            _linkCretaAcc = _driver.FindElement(By.CssSelector("form[name=login_form] > table > tbody > tr:nth-child(5) > td > a"));
            _linkCretaAcc.Click();
            _inputTaxID = _driver.FindElement(By.CssSelector("input[name=tax_id]"));
            _inputTaxID.SendKeys("12345");
            _inputCompany = _driver.FindElement(By.CssSelector("input[name=company]"));
            _inputCompany.SendKeys("Company Name");
            _inputFirstName = _driver.FindElement(By.CssSelector("input[name=firstname]"));
            _inputFirstName.SendKeys("Dmitriy");
            _inputLastName = _driver.FindElement(By.CssSelector("input[name=lastname]"));
            _inputLastName.SendKeys("Sergeev");
            _inputAddress1 = _driver.FindElement(By.CssSelector("input[name=address1]"));
            _inputAddress1.SendKeys("walking street");
            _inputPostcode = _driver.FindElement(By.CssSelector("input[name=postcode]"));
            _inputPostcode.SendKeys("55555");
            _inputCity = _driver.FindElement(By.CssSelector("input[name=city]"));
            _inputCity.SendKeys("Dream");
            _selectCountry = _driver.FindElement(By.CssSelector("span.select2-selection.select2-selection--single"));
            _selectCountry.Click();
            _inputSelect2SearchF = _driver.FindElement(By.CssSelector("input.select2-search__field"));
            _inputSelect2SearchF.SendKeys("United States" + Keys.Enter);           
            _inputPassword = _driver.FindElement(By.CssSelector("input[name=password]"));
            _inputPassword.SendKeys("zaq123456-");
            _inputConfirmedPassword = _driver.FindElement(By.CssSelector("input[name=confirmed_password]"));
            _inputConfirmedPassword.SendKeys("zaq123456-");
            _inputEmail = _driver.FindElement(By.CssSelector("input[name=email]"));
            _inputEmail.SendKeys("litecart12@yandex.ru");
            _inputPhone = _driver.FindElement(By.CssSelector("input[name=phone]"));
            _inputPhone.SendKeys("89832545487");            
            _btnCreateAccount = _driver.FindElement(By.CssSelector("button[name=create_account]"));
            _btnCreateAccount.Click();
            Logout();
            Login("litecart12@yandex.ru","zaq123456-");
            Logout();
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

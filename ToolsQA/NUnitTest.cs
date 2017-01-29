using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;


namespace ToolsQA
{
    [TestFixture]
    public class NUnitTest
    {
        private IWebDriver _driver;
              
        [SetUp]
        public void Initialize()
        {
            _driver = new ChromeDriver();          
        }
        [Test]
        public void TestApp()
        {
            _driver.Url = "http://www.demoqa.com";

        }
        [TearDown]
        public void EndTest()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}

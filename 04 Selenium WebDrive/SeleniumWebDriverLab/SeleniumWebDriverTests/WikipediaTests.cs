using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumWebDriverTests
{
    public class WikipediaTests
    {
        private IWebDriver driver;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.driver = new ChromeDriver();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            this.driver.Quit();
        }

        [Test]
        public void Test_HomePageSearch()
        {
            this.driver.Navigate().GoToUrl("https://wikipedia.org");

            string initTitle = this.driver.Title;

            Assert.AreEqual("Wikipedia", initTitle);

            IWebElement searchBox = this.driver.FindElement(By.Id("searchInput"));
            searchBox.SendKeys("Vasil Levski");

            IWebElement searchBtn = this.driver.FindElement(By.CssSelector("#search-form > fieldset > button > i"));
            searchBtn.Click();

            string newTitle = this.driver.Title;

            Assert.AreNotEqual(initTitle, newTitle);
            Assert.AreEqual("Vasil Levski - Wikipedia", newTitle);
        }

        [Test]
        public void Test_HomePageChangeLanguageToBgImplicitWait()
        {
            this.driver.Navigate().GoToUrl("https://wikipedia.org");

            //this.driver.Manage().Window.Maximize();

            string initTitle = this.driver.Title;

            Assert.AreEqual("Wikipedia", initTitle);

            Thread.Sleep(1000);

            this.driver.FindElement(By.CssSelector("#js-lang-list-button > span")).Click();

            Thread.Sleep(1000);

            this.driver.FindElement(By.CssSelector("#js-lang-lists > div:nth-child(4) > ul > li:nth-child(5) > a")).Click();

            string newTitle = this.driver.Title;

            Assert.AreNotEqual(initTitle, newTitle);
            Assert.AreEqual("Уикипедия", newTitle);
        }

        [Test]
        public void Test_HomePageChangeLanguageToBgExplicitWait()
        {
            this.driver.Navigate().GoToUrl("https://wikipedia.org");

            //this.driver.Manage().Window.Maximize();

            string initTitle = this.driver.Title;

            Assert.AreEqual("Wikipedia", initTitle);

            WebDriverWait waitDriver = new WebDriverWait(this.driver, TimeSpan.FromSeconds(10));

            IWebElement langOptBtn = waitDriver.Until<IWebElement>(driver => {
                return driver.FindElement(By.CssSelector("#js-lang-list-button > span")); 
            });

            langOptBtn.Click();

            IWebElement bgLangBtn = waitDriver.Until<IWebElement>(driver => {
                return driver.FindElement(By.CssSelector("#js-lang-lists > div:nth-child(4) > ul > li:nth-child(5) > a"));
            });

            bgLangBtn.Click();

            string newTitle = this.driver.Title;

            Assert.AreNotEqual(initTitle, newTitle);
            Assert.AreEqual("Уикипедия", newTitle);
        }

        [Test]
        public void Test_HomePageChangeLanguageToBgExpectedConditions()
        {
            this.driver.Navigate().GoToUrl("https://wikipedia.org");

            //this.driver.Manage().Window.Maximize();

            string initTitle = this.driver.Title;

            Assert.AreEqual("Wikipedia", initTitle);

            WebDriverWait waitDriver = new WebDriverWait(this.driver, TimeSpan.FromSeconds(10));

            IWebElement langOptBtn = waitDriver.Until<IWebElement>(
                ExpectedConditions.ElementIsVisible(By.CssSelector("#js-lang-list-button > span")));

            langOptBtn.Click();

            IWebElement bgLangBtn = waitDriver.Until<IWebElement>(
                ExpectedConditions.ElementIsVisible(By.CssSelector("#js-lang-lists > div:nth-child(4) > ul > li:nth-child(5) > a")));
            
            bgLangBtn.Click();

            string newTitle = this.driver.Title;

            Assert.AreNotEqual(initTitle, newTitle);
            Assert.AreEqual("Уикипедия", newTitle);
        }
    }
}

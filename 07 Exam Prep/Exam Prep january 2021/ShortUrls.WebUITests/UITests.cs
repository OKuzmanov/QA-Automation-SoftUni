using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;

namespace ShortUrls.WebUITests
{
    public class UITests
    {
        private const string url = "https://shorturl.nakov.repl.co";
        private WebDriver driver;

        [OneTimeSetUp]
        public void Setup()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--headless");

            this.driver = new ChromeDriver(chromeOptions);

            this.driver.Manage().Window.Maximize();
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            this.driver.Quit();
        }

        [Test]
        public void Test_ShortUrlsPage()
        {
            const string expectedHeading = "Short URLs";

            const string originalColText = "Original URL";
            const string shortUrlColText = "Short URL";
            const string dateCreatedColText = "Date Created";
            const string visitsColText = "Visits";

            this.driver.Navigate().GoToUrl(url);

            IWebElement ShortUrlsHref = this.driver.FindElement(By.CssSelector("body > header > a:nth-child(3)"));
            ShortUrlsHref.Click();

            IWebElement heading = this.driver.FindElement(By.CssSelector("body > main > h1"));

            Assert.That(heading.Text, Is.EqualTo(expectedHeading));

            IWebElement originalUrlCol = this.driver.FindElement(By.CssSelector("body > main > table > thead > tr > th:nth-child(1)"));
            IWebElement shortUrlCol = this.driver.FindElement(By.CssSelector("body > main > table > thead > tr > th:nth-child(2)"));
            IWebElement dateCreatedCol = this.driver.FindElement(By.CssSelector("body > main > table > thead > tr > th:nth-child(3)"));
            IWebElement visitsCol = this.driver.FindElement(By.CssSelector("body > main > table > thead > tr > th:nth-child(4)"));

            
            Assert.That(originalUrlCol.Text, Is.EqualTo(originalColText));
            Assert.That(shortUrlCol.Text, Is.EqualTo(shortUrlColText));
            Assert.That(dateCreatedCol.Text, Is.EqualTo(dateCreatedColText));
            Assert.That(visitsCol.Text, Is.EqualTo(visitsColText));
        }

        [Test]
        public void Test_CreateNewShortUrl_ValidData()
        {
            const string testUrl = "https://softuni.bg";
            string testCode = "te" + DateTime.Now.Ticks;

            this.driver.Navigate().GoToUrl(url);

            var addUrlHref = this.driver.FindElement(By.CssSelector("body > header > a:nth-child(5)"));
            addUrlHref.Click();

            var urlInput = this.driver.FindElement(By.Id("url"));
            urlInput.Clear();
            urlInput.SendKeys(testUrl);

            var codeInput = this.driver.FindElement(By.Id("code"));
            codeInput.Clear();
            codeInput.SendKeys(testCode);

            var createBtn = this.driver.FindElement(By.CssSelector("body > main > form > table > tbody > tr:nth-child(3) > td > button"));
            createBtn.Click();

            var urls = this.driver.FindElements(By.CssSelector("body > main > table > tbody > tr"));

            var cols = urls.Last().FindElements(By.TagName("td"));

            Assert.That(cols[0].Text, Is.EqualTo(testUrl));

            string httpUrl = url.Substring(0, 4) + url.Substring(5);

            Assert.That(cols[1].Text, Is.EqualTo(httpUrl + "/go/" + testCode));
        }

        [Test]
        public void Test_CreateNewShortUrl_InvalidData_MissingUrl()
        {
            //const string testUrl = "https://softuni.bg";
            string testCode = "te" + DateTime.Now.Ticks;
            const string expectedErrorMsg = "URL cannot be empty!";

            this.driver.Navigate().GoToUrl(url);

            var addUrlHref = this.driver.FindElement(By.CssSelector("body > header > a:nth-child(5)"));
            addUrlHref.Click();

            //var urlInput = this.driver.FindElement(By.Id("url"));
            //urlInput.Clear();
            //urlInput.SendKeys(testUrl);

            var codeInput = this.driver.FindElement(By.Id("code"));
            codeInput.Clear();
            codeInput.SendKeys(testCode);

            var createBtn = this.driver.FindElement(By.CssSelector("body > main > form > table > tbody > tr:nth-child(3) > td > button"));
            createBtn.Click();

            var errorMsg = this.driver.FindElement(By.CssSelector("body > div"));

            Assert.That(errorMsg.Text, Is.EqualTo(expectedErrorMsg));
        }

        [Test]
        public void Test_CreateNewShortUrl_InvalidData_MissingCode()
        {
            const string testUrl = "https://softuni.bg";
            //string testCode = "te" + DateTime.Now.Ticks;
            const string expectedErrorMsg = "Short code cannot be empty!";

            this.driver.Navigate().GoToUrl(url);

            var addUrlHref = this.driver.FindElement(By.CssSelector("body > header > a:nth-child(5)"));
            addUrlHref.Click();

            var urlInput = this.driver.FindElement(By.Id("url"));
            urlInput.Clear();
            urlInput.SendKeys(testUrl);

            var codeInput = this.driver.FindElement(By.Id("code"));
            codeInput.Clear();
            //codeInput.SendKeys(testCode);

            var createBtn = this.driver.FindElement(By.CssSelector("body > main > form > table > tbody > tr:nth-child(3) > td > button"));
            createBtn.Click();

            var errorMsg = this.driver.FindElement(By.CssSelector("body > div"));

            Assert.That(errorMsg.Text, Is.EqualTo(expectedErrorMsg));
        }

        [Test]
        public void Test_VisitShortUrl_ValidUrl()
        {
            const string testUrl = "https://softuni.bg";
            string testCode = "te" + DateTime.Now.Ticks;

            this.driver.Navigate().GoToUrl(url);

            var addUrlHref = this.driver.FindElement(By.CssSelector("body > header > a:nth-child(5)"));
            addUrlHref.Click();

            var urlInput = this.driver.FindElement(By.Id("url"));
            urlInput.Clear();
            urlInput.SendKeys(testUrl);

            var codeInput = this.driver.FindElement(By.Id("code"));
            codeInput.Clear();
            codeInput.SendKeys(testCode);

            var createBtn = this.driver.FindElement(By.CssSelector("body > main > form > table > tbody > tr:nth-child(3) > td > button"));
            createBtn.Click();

            var rows = this.driver.FindElements(By.CssSelector("body > main > table > tbody > tr"));

            var cols = rows.Last().FindElements(By.TagName("td"));
            var visitorsBefore = cols.Last().Text;

            cols[1].FindElement(By.ClassName("shorturl")).Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(d =>
            {
                var handlers = this.driver.WindowHandles;
                this.driver.SwitchTo().Window(handlers[1]);

                return this.driver.FindElement(By.CssSelector("body > div.content > div > header > div > div.header-section-body > div.header-section-buttons > a"));
            });


            var handlers = this.driver.WindowHandles;
            this.driver.SwitchTo().Window(handlers[0]);

            var rowsAfter = this.driver.FindElements(By.CssSelector("body > main > table > tbody > tr"));

            var colsAfter = rowsAfter.Last().FindElements(By.TagName("td"));
            var visitorsAfter = colsAfter.Last().Text;

            Assert.That(visitorsAfter, Is.GreaterThan(visitorsBefore));
        }

        [Test]
        public void Test_VisitShortUrl_InvalidUrl()
        {
            string invalidCode = "rand" + DateTime.Now.Ticks;

            const string expectedDivErrMSg = "Cannot navigate to given short URL";
            const string expectedHeadErrMSg = "Error: Cannot navigate to given short URL";
            const string expectedPErrMSg = "Invalid short URL code: ";

            this.driver.Navigate().GoToUrl(url + "/go/" + invalidCode);

            var errorMsgDiv = this.driver.FindElement(By.CssSelector("body > div")).Text;

            Assert.That(errorMsgDiv, Is.EqualTo(expectedDivErrMSg));

            var errorMsgHead = this.driver.FindElement(By.CssSelector("body > main > h1")).Text;

            Assert.That(errorMsgHead, Is.EqualTo(expectedHeadErrMSg));

            var errorMsgP = this.driver.FindElement(By.CssSelector("body > main > p")).Text;

            Assert.That(errorMsgP, Is.EqualTo(expectedPErrMSg + invalidCode));
        }
    }
}
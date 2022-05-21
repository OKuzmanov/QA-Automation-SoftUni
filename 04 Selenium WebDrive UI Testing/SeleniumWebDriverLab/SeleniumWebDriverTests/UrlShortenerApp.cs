using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumWebDriverTests
{
    public class UrlShortenerAppTests
    {
        private ChromeDriver driver;

        [OneTimeSetUp]
        public void SetUp()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            this.driver = new ChromeDriver(options);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            this.driver.Quit();
        }

        [Test]
        public void Test_HomePage()
        {
            this.driver.Navigate().GoToUrl("https://shorturl.nakov.repl.co");

            string homePageTitle = this.driver.Title;

            Assert.AreEqual("URL Shortener", homePageTitle);
        }

        [Test]
        public void Test_ShortUrlsPage()
        {
            this.driver.Navigate().GoToUrl("https://shorturl.nakov.repl.co");

            this.driver.FindElement(By.CssSelector("body > header > a:nth-child(3)")).Click();

            IWebElement originalUrl = this.driver.FindElement(By.CssSelector("body > main > table > tbody > tr:nth-child(1) > td:nth-child(1)"));

            Assert.AreEqual("https://nakov.com", originalUrl.Text);

            IWebElement shortUrl = this.driver.FindElement(By.CssSelector("body > main > table > tbody > tr:nth-child(1) > td:nth-child(2)"));

            Assert.AreEqual("http://shorturl.nakov.repl.co/go/nak", shortUrl.Text);

            IWebElement dateCreated = this.driver.FindElement(By.CssSelector("body > main > table > tbody > tr:nth-child(1) > td:nth-child(3)"));

            Assert.AreEqual("2021-02-17 14:41:33", dateCreated.Text);
        }

        [Test]
        public void Test_AddUrlValidUrl()
        {
            this.driver.Navigate().GoToUrl("https://shorturl.nakov.repl.co");

            this.driver.FindElement(By.CssSelector("body > header > a:nth-child(5)")).Click();

            IWebElement urlBox = this.driver.FindElement(By.Id("url"));
            urlBox.Click();
            urlBox.SendKeys("https://www.selenium.dev/documentation/webdriver/");

            IWebElement codeBox = this.driver.FindElement(By.Id("code"));
            codeBox.Click();
            codeBox.Clear();
            long code = DateTime.Now.Ticks;
            codeBox.SendKeys(code.ToString());

            this.driver.FindElement(By.CssSelector("body > main > form > table > tbody > tr:nth-child(3) > td > button")).Click();

            string shortUrl = "http://shorturl.nakov.repl.co/go/";
            ReadOnlyCollection<IWebElement> shortUrlsList = this.driver.FindElements(By.LinkText(shortUrl + code.ToString()));

            Assert.That(shortUrlsList.Count(), Is.EqualTo(1));

            Assert.AreEqual(String.Join("", new string[] { shortUrl, code.ToString()}), shortUrlsList[0].Text);
        }

        [Test]
        public void Test_AddUrlInvalidValidUrl()
        {
            this.driver.Navigate().GoToUrl("https://shorturl.nakov.repl.co");

            this.driver.FindElement(By.CssSelector("body > header > a:nth-child(5)")).Click();

            IWebElement urlBox = this.driver.FindElement(By.Id("url"));
            urlBox.Click();
            urlBox.SendKeys("Invalid url");

            this.driver.FindElement(By.CssSelector("body > main > form > table > tbody > tr:nth-child(3) > td > button")).Click();

            Assert.AreEqual("Add Short URL", driver.Title);

            IWebElement error = this.driver.FindElement(By.ClassName("err"));

            Assert.AreEqual("Invalid URL!", error.Text);
        }

        [Test]
        public void Test_AddUrlInvalidCode()
        {
            this.driver.Navigate().GoToUrl("https://shorturl.nakov.repl.co");

            this.driver.FindElement(By.CssSelector("body > header > a:nth-child(5)")).Click();

            IWebElement urlBox = this.driver.FindElement(By.Id("url"));
            urlBox.Click();
            urlBox.SendKeys("https://nakov.com");

            IWebElement codeBox = this.driver.FindElement(By.Id("code"));
            codeBox.Click();
            codeBox.Clear();
            codeBox.SendKeys("nak");

            this.driver.FindElement(By.CssSelector("body > main > form > table > tbody > tr:nth-child(3) > td > button")).Click();

            Assert.AreEqual("Add Short URL", driver.Title);

            IWebElement error = this.driver.FindElement(By.ClassName("err"));

            Assert.AreEqual("Short code already exists!", error.Text);
        }

        [Test]
        public void Test_VisitUrl()
        {
            this.driver.Navigate().GoToUrl("https://shorturl.nakov.repl.co/");

            this.driver.FindElement(By.CssSelector("body > header > a:nth-child(3)")).Click();

            long oldViewCount = long.Parse(this.driver.FindElement(By.CssSelector("body > main > table > tbody > tr:nth-child(1) > td:nth-child(4)")).Text);

            this.driver.FindElement(By.ClassName("shorturl")).Click();

            Thread.Sleep(1000);

            long newViewCount = long.Parse(this.driver.FindElement(By.CssSelector("body > main > table > tbody > tr:nth-child(1) > td:nth-child(4)")).Text);

            Assert.Greater(newViewCount, oldViewCount);

            this.driver.FindElement(By.CssSelector("body > main > table > tbody > tr:nth-child(1) > td:nth-child(1) > a")).Click();

            ReadOnlyCollection<string> windowHandles = this.driver.WindowHandles;

            for (int i = 1; i < windowHandles.Count; i++)
            {
                this.driver.SwitchTo().Window(windowHandles[i]);
                string title = this.driver.Title;
                Assert.AreEqual("Svetlin Nakov - Svetlin Nakov – Official Web Site and Blog", title);
            }
        }

        [Test]
        public void Test_VisitUrlInvalidShortUrl()
        {
            this.driver.Navigate().GoToUrl("http://shorturl.nakov.repl.co/go/invalidShortUrl");

            Assert.AreEqual("Error", this.driver.Title);

            IWebElement errorElement = this.driver.FindElement(By.ClassName("err"));

            Assert.AreEqual("Cannot navigate to given short URL", errorElement.Text);
        }
    }
}
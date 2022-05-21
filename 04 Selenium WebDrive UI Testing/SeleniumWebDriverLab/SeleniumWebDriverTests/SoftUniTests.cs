using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SoftUniTests
{
    public class SoftUniWebSiteTests
    {
        private IWebDriver driver;

        [OneTimeSetUp]
        public void SetUp()
        {
            //ChromeOptions options = new ChromeOptions();
            //options.AddArguments("--headless");
            driver = new ChromeDriver();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void Test_HomePageTitle()
        {
            driver.Navigate().GoToUrl("https://softuni.bg/");
            driver.Manage().Window.Size = new System.Drawing.Size(1920, 1040);
            driver.FindElement(By.CssSelector(".header-section-headings")).Click();
            Assert.That(driver.Title, Is.EqualTo("Обучение по програмиране - Софтуерен университет"));
            driver.Close();
        }

        [Test]
        public void Test_AboutUsTitle()
        {
            this.driver.Navigate().GoToUrl("https://softuni.bg");

            this.driver.Manage().Window.Maximize();

            IWebElement aboutUsBtn = this.driver.FindElement(By.CssSelector("#header-nav > div.toggle-nav.toggle-holder > ul > li:nth-child(1) > a > span"));
            aboutUsBtn.Click();

            IWebElement aboutUsHead = driver.FindElement(By.CssSelector("body > div.content > div > div.about-page-header.page-wrapper > div > h2"));

            Assert.That("Ние правим хората, които обучаваме, истински професионалисти в софтуерната индустрия и им съдействаме за техния кариерен старт!",
                Is.EqualTo(aboutUsHead.Text));
        }
    }
}
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students_Registry_Automated_Testsa.Pages
{
    public class BasePage
    {
        protected readonly IWebDriver driver;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public virtual string PageUrl { get; }

        public IWebElement LinkHomePage => driver.FindElement(By.CssSelector("body > a:nth-child(1)"));

        public IWebElement LinkViewStudentsPage => driver.FindElement(By.CssSelector("body > a:nth-child(3)"));

        public IWebElement LinkAddStudentPage => driver.FindElement(By.CssSelector("body > a:nth-child(5)"));

        public IWebElement ElementTextHeading => driver.FindElement(By.CssSelector("body > h1"));

        public void Open()
        {
            this.driver.Navigate().GoToUrl(PageUrl);
        }

        public bool IsOpen()
        {
            return this.driver.Url == this.PageUrl;
        }

        public string GetPageTitle()
        {
            return this.driver.Title;
        }

        public string GetPageHeading()
        {
            return this.ElementTextHeading.Text;
        }
    }
}

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students_Registry_Automated_Testsa.Tests
{
    public class BaseTests
    {
        public IWebDriver driver;

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
    }
}

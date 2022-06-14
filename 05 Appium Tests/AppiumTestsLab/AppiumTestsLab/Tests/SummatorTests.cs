using AppiumTestsLab.Windows;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppiumTestsLab.Tests
{
    public class SummatorTests
    {
        private WindowsDriver<WindowsElement> driver;
        private const string AppiumServer = "http://127.0.0.1:4723/wd/hub";
        private AppiumOptions options;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability(MobileCapabilityType.App, @"C:\WindowsFormsApp-Summator.exe");
            this.driver = new WindowsDriver<WindowsElement>(new Uri(AppiumServer), options);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            this.driver.CloseApp();
            this.driver.Quit();
        }

        [TestCase("1", "1", "2")]
        [TestCase("100", "1", "101")]
        [TestCase("-1", "-1", "-2")]
        [TestCase("3.14", "2.958", "6.098")]
        [TestCase("3.14", "-2.958", "0.182")]
        [TestCase("-5.59", "3.14", "-2.45")]
        [TestCase("0", "0", "0")]
        public void Test_SumValidNumbers(string num1, string num2, string result)
        {
            SummatorHome smtr = new SummatorHome(this.driver);

            string actual = smtr.Sum(num1, num2);

            Assert.AreEqual(result, actual);
        }

        [TestCase("", "")]
        [TestCase("1", "")]
        [TestCase("", "2")]
        [TestCase("asd", "2")]
        [TestCase("asd", "asd")]
        [TestCase("", "!")]
        [TestCase("*", "")]
        public void Test_ErrMsgInValidNumbers(string num1, string num2)
        {
            string errMsg = "error";

            SummatorHome smtr = new SummatorHome(this.driver);

            string actual = smtr.Sum(num1, num2);

            Assert.AreEqual(errMsg, actual);
        }
    }
}

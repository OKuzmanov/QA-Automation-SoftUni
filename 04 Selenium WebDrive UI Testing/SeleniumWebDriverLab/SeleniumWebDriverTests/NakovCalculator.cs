using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumWebDriverTests
{
    public class NakovCalculator
    {
        private IWebDriver driver;

        [OneTimeSetUp]
        public void setUp()
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

        [TestCase("1", "+", "1", "Result: 2")]
        [TestCase("10", "-", "5", "Result: 5")]
        [TestCase("20", "*", "3", "Result: 60")]
        [TestCase("100", "/", "10", "Result: 10")]
        [TestCase("3.012", "-", "12.934", "Result: -9.922")]
        [TestCase("3.012", "*", "-7.534", "Result: -22.692408")]
        [TestCase("asdads", "*", "7", "Result: invalid input")]
        [TestCase("", "*", "fasfafa", "Result: invalid input")]
        [TestCase("12", "asdasd", "35", "Result: invalid operation")]
        [TestCase("Infinity", "+", "1", "Result: Infinity")]
        [TestCase("Infinity", "-", "Infinity", "Result: invalid calculation")]
        [TestCase("Infinity", "/", "Infinity", "Result: invalid calculation")]
        public void Test_CalculatorFunctionality(string num1, string operatr, string num2, string result)
        {
            this.driver.Navigate().GoToUrl("https://number-calculator.nakov.repl.co/");

            this.driver.FindElement(By.Name("number1")).SendKeys(num1);

            this.driver.FindElement(By.Name("operation")).SendKeys(operatr);

            this.driver.FindElement(By.Name("number2")).SendKeys(num2);

            this.driver.FindElement(By.Id("calcButton")).Click();

            Assert.AreEqual(result, this.driver.FindElement(By.Id("result")).Text);

            this.driver.FindElement(By.Id("resetButton")).Click();
        }
    }
}

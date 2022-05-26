using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NakovCalcWebDriverTestsPOM.Pages
{
    public class SumNumbersPage
    {
        private IWebDriver driver;

        public SumNumbersPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        const string PageUrl = "https://number-calculator.nakov.repl.co/";

        private IWebElement FieldNum1 => this.driver.FindElement(By.Id("number1"));
        private IWebElement FieldNum2 => this.driver.FindElement(By.Id("number2"));
        private IWebElement OperandField => this.driver.FindElement(By.Id("operation"));
        private IWebElement CalcButton => this.driver.FindElement(By.Id("calcButton"));
        private IWebElement ResetButton => this.driver.FindElement(By.Id("resetButton"));
        private IWebElement ElementResult => this.driver.FindElement(By.CssSelector("#result"));

        public void OpenPage()
        {
            this.driver.Navigate().GoToUrl(PageUrl);
        }

        public string AddNumbers(string num1, string num2)
        {
            FieldNum1.SendKeys(num1);
            FieldNum2.SendKeys(num2);
            OperandField.SendKeys("+");
            CalcButton.Click();
            string result = ElementResult.Text;
            return result;
        }

        public string SubtractNumbers(string num1, string num2)
        {
            FieldNum1.SendKeys(num1);
            FieldNum2.SendKeys(num2);
            OperandField.SendKeys("-");
            CalcButton.Click();
            string result = ElementResult.Text;
            return result;
        }

        public string DivideNumbers(string num1, string num2)
        {
            FieldNum1.SendKeys(num1);
            FieldNum2.SendKeys(num2);
            OperandField.SendKeys("/");
            CalcButton.Click();
            string result = ElementResult.Text;
            return result;
        }

        public string MultiplyNumbers(string num1, string num2)
        {
            FieldNum1.SendKeys(num1);
            FieldNum2.SendKeys(num2);
            OperandField.SendKeys("*");
            CalcButton.Click();
            string result = ElementResult.Text;
            return result;
        }

        public void ResetForm()
        {
            ResetButton.Click();
        }

        public bool IsFormEmpty()
        {
            return FieldNum1.Text + FieldNum2.Text + ElementResult.Text == "";
        }
    }
}

using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppiumTestsLab.Windows
{
    public class SummatorHome
    {
        private readonly WindowsDriver<WindowsElement> driver;

        public SummatorHome(WindowsDriver<WindowsElement> driver)
        {
            this.driver = driver;
        }

        WindowsElement textBox1 => driver.FindElementByAccessibilityId("textBoxFirstNum");

        WindowsElement textBox2 => driver.FindElementByAccessibilityId("textBoxSecondNum");

        WindowsElement textBoxSum = this.driver.FindElementByAccessibilityId("textBoxSum");

        WindowsElement calcButton => this.driver.FindElementByAccessibilityId("buttonCalc");

        public string Sum(string num1, string num2)
        {
            textBox1.Click();
            textBox1.Clear();
            textBox1.SendKeys(num1);

            textBox2.Click();
            textBox2.Clear();
            textBox2.SendKeys(num2);

            calcButton.Click();

            return textBoxSum.Text;
        }
    }
}

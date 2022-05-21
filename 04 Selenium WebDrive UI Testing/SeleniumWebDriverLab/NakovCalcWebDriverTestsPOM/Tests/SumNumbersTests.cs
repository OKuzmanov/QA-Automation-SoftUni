using NakovCalcWebDriverTestsPOM.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NakovCalcWebDriverTestsPOM
{
    public class SumNumberTests
    {

        private IWebDriver driver;

        [OneTimeSetUp]
        public void Setup()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--headless", "--window-size=1920,1200");
            this.driver = new ChromeDriver(chromeOptions);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            this.driver.Quit();
        }

        [TestCase("1", "2", "Result: 3")]
        [TestCase("5", "5", "Result: 10")]
        [TestCase("100", "100", "Result: 200")]
        public void Test_AddNumbers_ValidInput(string num1, string num2, string resultExpected)
        {
            SumNumbersPage sumNumbersPage = new SumNumbersPage(this.driver);

            sumNumbersPage.OpenPage();

            string resultActual = sumNumbersPage.AddNumbers(num1, num2);

            Assert.AreEqual(resultExpected, resultActual);
        }

        [TestCase("1", "", "Result: invalid input")]
        [TestCase("!", "20", "Result: invalid input")]
        [TestCase("!", "-", "Result: invalid input")]
        [TestCase("", "", "Result: invalid input")]
        public void Test_AddNumbers_InvalidInput(string num1, string num2, string resultExpected)
        {
            SumNumbersPage sumNumbersPage = new SumNumbersPage(this.driver);

            sumNumbersPage.OpenPage();

            string resultActual = sumNumbersPage.AddNumbers(num1, num2);

            Assert.AreEqual(resultExpected, resultActual);
        }

        [TestCase("10", "5", "Result: 5")]
        [TestCase("10", "100", "Result: -90")]
        [TestCase("-1", "2", "Result: -3")]
        [TestCase("10.55", "2.87", "Result: 7.68")]
        [TestCase("2.87", "10.55", "Result: -7.68")]
        public void Test_SubtractNumbers_ValidInput(string num1, string num2, string resultExpected)
        {
            SumNumbersPage sumNumbersPage = new SumNumbersPage(this.driver);

            sumNumbersPage.OpenPage();

            string resultActual = sumNumbersPage.SubtractNumbers(num1, num2);

            Assert.AreEqual(resultExpected, resultActual);
        }

        [TestCase("10", "-", "Result: invalid input")]
        [TestCase("", "5", "Result: invalid input")]
        [TestCase("1", "!", "Result: invalid input")]
        [TestCase("@", "!", "Result: invalid input")]
        public void Test_SubtractNumbers_InvalidInput(string num1, string num2, string resultExpected)
        {
            SumNumbersPage sumNumbersPage = new SumNumbersPage(this.driver);

            sumNumbersPage.OpenPage();

            string resultActual = sumNumbersPage.SubtractNumbers(num1, num2);

            Assert.AreEqual(resultExpected, resultActual);
        }

        [TestCase("1", "5", "Result: 5")]
        [TestCase("10", "-100", "Result: -1000")]
        [TestCase("10.55", "2.87", "Result: 30.2785")]
        [TestCase("0", "10.55", "Result: 0")]
        public void Test_MultiplyNumbers_ValidInput(string num1, string num2, string resultExpected)
        {
            SumNumbersPage sumNumbersPage = new SumNumbersPage(this.driver);

            sumNumbersPage.OpenPage();

            string resultActual = sumNumbersPage.MultiplyNumbers(num1, num2);

            Assert.AreEqual(resultExpected, resultActual);
        }

        [TestCase("", "5", "Result: invalid input")]
        [TestCase("1", "!", "Result: invalid input")]
        [TestCase("", "", "Result: invalid input")]
        [TestCase("a", "s", "Result: invalid input")]
        public void Test_MultiplyNumbers_InvalidInput(string num1, string num2, string resultExpected)
        {
            SumNumbersPage sumNumbersPage = new SumNumbersPage(this.driver);

            sumNumbersPage.OpenPage();

            string resultActual = sumNumbersPage.MultiplyNumbers(num1, num2);

            Assert.AreEqual(resultExpected, resultActual);
        }

        [TestCase("1", "5", "Result: 0.2")]
        [TestCase("100", "10", "Result: 10")]
        [TestCase("-100", "10", "Result: -10")]
        [TestCase("-10", "5", "Result: -2")]
        [TestCase("10", "0", "Result: Infinity")]
        public void Test_DivideNumbers_ValidInput(string num1, string num2, string resultExpected)
        {
            SumNumbersPage sumNumbersPage = new SumNumbersPage(this.driver);

            sumNumbersPage.OpenPage();

            string resultActual = sumNumbersPage.DivideNumbers(num1, num2);

            Assert.AreEqual(resultExpected, resultActual);
        }

        [TestCase("!", "0", "Result: invalid input")]
        [TestCase("1", "", "Result: invalid input")]
        [TestCase("a", "s", "Result: invalid input")]
        public void Test_DivideNumbers_InvalidInput(string num1, string num2, string resultExpected)
        {
            SumNumbersPage sumNumbersPage = new SumNumbersPage(this.driver);

            sumNumbersPage.OpenPage();

            string resultActual = sumNumbersPage.DivideNumbers(num1, num2);

            Assert.AreEqual(resultExpected, resultActual);
        }

        [Test]
        public void Test_ResetButton()
        {
            SumNumbersPage sumNumbersPage = new SumNumbersPage(this.driver);

            sumNumbersPage.OpenPage();

            Assert.That(sumNumbersPage.IsFormEmpty, Is.True);

            string resultActual = sumNumbersPage.DivideNumbers("1", "2");

            Assert.That(sumNumbersPage.IsFormEmpty, Is.False);
        }

        //TODO: Infinity Tests
    }
}
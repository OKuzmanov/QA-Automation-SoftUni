using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace ContactBook.UITests
{
    public class UITests
    {
        public const string url = "http://localhost:8080";
        public WebDriver driver; 

        [OneTimeSetUp]
        public void Setup()
        {
            this.driver = new ChromeDriver();
            this.driver.Manage().Window.Maximize();
            this.driver.Manage().Timeouts().ImplicitWait = (TimeSpan.FromSeconds(5));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            this.driver.Quit();
        }

        [Test]
        public void Test_ListContacts_AssertFirstOneIsSteveJobs()
        {
            this.driver.Navigate().GoToUrl(url);

            IWebElement contactsButton = this.driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(2) > a"));

            contactsButton.Click();

            string firstName = this.driver.FindElement(By.CssSelector("#contact1 > tbody > tr.fname > td")).Text;
            string lastName = this.driver.FindElement(By.CssSelector("#contact1 > tbody > tr.lname > td")).Text;

            Assert.That(firstName, Is.EqualTo("Steve"));
            Assert.That(lastName, Is.EqualTo("Jobs"));
        }

        [Test]
        public void Test_SearchByValidKeyword_AssertCorrectResult()
        {
            const string keyword = "albert";

            this.driver.Navigate().GoToUrl(url);

            IWebElement searchButton = this.driver.FindElement(By.CssSelector("body > main > div > a:nth-child(3) > span:nth-child(2)"));

            searchButton.Click();

            IWebElement searchBar = this.driver.FindElement(By.Id("keyword"));

            searchBar.Click();
            searchBar.SendKeys(keyword);

            IWebElement submitButton = this.driver.FindElement(By.Id("search"));
            submitButton.Click();

            string firstName = this.driver.FindElement(By.CssSelector("#contact3 > tbody > tr.fname > td")).Text;
            string lastName = this.driver.FindElement(By.CssSelector("#contact3 > tbody > tr.lname > td")).Text;

            Assert.That(firstName, Is.EqualTo("Albert"));
            Assert.That(lastName, Is.EqualTo("Einstein"));
        }

        [Test]
        public void Test_SearchByInvalidKeyword_AssertEmptyResult()
        {
            const string keyword = "invalid2635";

            this.driver.Navigate().GoToUrl(url);

            IWebElement searchButton = this.driver.FindElement(By.CssSelector("body > main > div > a:nth-child(3) > span:nth-child(2)"));

            searchButton.Click();

            IWebElement searchBar = this.driver.FindElement(By.Id("keyword"));

            searchBar.Click();
            searchBar.SendKeys(keyword);

            IWebElement submitButton = this.driver.FindElement(By.Id("search"));
            submitButton.Click();

            string textResult = this.driver.FindElement(By.Id("searchResult")).Text;

            Assert.That(textResult, Is.EqualTo("No contacts found."));
        }

        [Test]
        public void Test_CreateNewContactInvalidData_AssertFirstNameCannotBeEmptyErrorIsShown()
        {
            const string errorMsg = "Error: First name cannot be empty!";

            this.driver.Navigate().GoToUrl(url);

            IWebElement createContactBtn = this.driver.FindElement(By.CssSelector("body > main > div > a:nth-child(2)"));

            createContactBtn.Click();

            IWebElement firstNameInput = this.driver.FindElement(By.Id("firstName"));
            IWebElement lastNameInput = this.driver.FindElement(By.Id("lastName"));
            IWebElement emailInput = this.driver.FindElement(By.Id("email"));
            IWebElement phoneInput = this.driver.FindElement(By.Id("phone"));
            IWebElement commentsInput = this.driver.FindElement(By.Id("comments"));

            lastNameInput.Click();
            lastNameInput.SendKeys("Invalid1231");

            emailInput.Click();
            emailInput.SendKeys("test@mail.com");

            phoneInput.Click();
            phoneInput.SendKeys("0878639910");

            commentsInput.Click();
            commentsInput.SendKeys("Test comment section.");

            IWebElement createBtn = this.driver.FindElement(By.Id("create"));
            createBtn.Click();

            string errorBarMsg = this.driver.FindElement(By.CssSelector("body > main > div")).Text;

            Assert.That(errorBarMsg, Is.EqualTo(errorMsg));
        }

        [Test]
        public void Test_CreateNewContactInvalidData_AssertLastNameCannotBeEmptyErrorIsShown()
        {
            const string errorMsg = "Error: Last name cannot be empty!";

            this.driver.Navigate().GoToUrl(url);

            IWebElement createContactBtn = this.driver.FindElement(By.CssSelector("body > main > div > a:nth-child(2)"));

            createContactBtn.Click();

            IWebElement firstNameInput = this.driver.FindElement(By.Id("firstName"));
            IWebElement lastNameInput = this.driver.FindElement(By.Id("lastName"));
            IWebElement emailInput = this.driver.FindElement(By.Id("email"));
            IWebElement phoneInput = this.driver.FindElement(By.Id("phone"));
            IWebElement commentsInput = this.driver.FindElement(By.Id("comments"));

            firstNameInput.Click();
            firstNameInput.SendKeys("Invalid1231");

            emailInput.Click();
            emailInput.SendKeys("test@mail.com");

            phoneInput.Click();
            phoneInput.SendKeys("0878639910");

            commentsInput.Click();
            commentsInput.SendKeys("Test comment section.");

            IWebElement createBtn = this.driver.FindElement(By.Id("create"));
            createBtn.Click();

            string errorBarMsg = this.driver.FindElement(By.CssSelector("body > main > div")).Text;

            Assert.That(errorBarMsg, Is.EqualTo(errorMsg));
        }

        [Test]
        public void Test_CreateNewContactInvalidData_AssertInvalidEmailErrorIsShown()
        {
            const string errorMsg = "Error: Invalid email!";

            this.driver.Navigate().GoToUrl(url);

            IWebElement createContactBtn = this.driver.FindElement(By.CssSelector("body > main > div > a:nth-child(2)"));

            createContactBtn.Click();

            IWebElement firstNameInput = this.driver.FindElement(By.Id("firstName"));
            IWebElement lastNameInput = this.driver.FindElement(By.Id("lastName"));
            IWebElement emailInput = this.driver.FindElement(By.Id("email"));
            IWebElement phoneInput = this.driver.FindElement(By.Id("phone"));
            IWebElement commentsInput = this.driver.FindElement(By.Id("comments"));

            firstNameInput.Click();
            firstNameInput.SendKeys("Invalid1231");

            lastNameInput.Click();
            lastNameInput.SendKeys("ivalid222");

            phoneInput.Click();
            phoneInput.SendKeys("0878639910");

            commentsInput.Click();
            commentsInput.SendKeys("Test comment section.");

            IWebElement createBtn = this.driver.FindElement(By.Id("create"));
            createBtn.Click();

            string errorBarMsg = this.driver.FindElement(By.CssSelector("body > main > div")).Text;

            Assert.That(errorBarMsg, Is.EqualTo(errorMsg));
        }

        [Test]
        public void Test_CreateNewContactValidData_AssertContactIsAddedAndProperlyListed()
        {
            string firstName = "Oleg" + DateTime.Now.Ticks;
            string lastName = "Kuzmanov" + DateTime.Now.Ticks;
            string email = DateTime.Now.Ticks + "ok@gmail.com";
            string phone = DateTime.Now.Ticks + "";
            string comments = "Test Comment number: " + DateTime.Now.Ticks;

            this.driver.Navigate().GoToUrl(url);

            IWebElement createContactBtn = this.driver.FindElement(By.CssSelector("body > main > div > a:nth-child(2)"));

            createContactBtn.Click();

            IWebElement firstNameInput = this.driver.FindElement(By.Id("firstName"));
            IWebElement lastNameInput = this.driver.FindElement(By.Id("lastName"));
            IWebElement emailInput = this.driver.FindElement(By.Id("email"));
            IWebElement phoneInput = this.driver.FindElement(By.Id("phone"));
            IWebElement commentsInput = this.driver.FindElement(By.Id("comments"));

            firstNameInput.Click();
            firstNameInput.SendKeys(firstName);

            lastNameInput.Click();
            lastNameInput.SendKeys(lastName);

            emailInput.Click();
            emailInput.SendKeys(email);

            phoneInput.Click();
            phoneInput.SendKeys(phone);

            commentsInput.Click();
            commentsInput.SendKeys(comments);

            IWebElement createBtn = this.driver.FindElement(By.Id("create"));
            createBtn.Click();

            var allContacts = this.driver.FindElements(By.ClassName("contact-entry"));
            var lastContact = allContacts[allContacts.Count - 1];

            Assert.That(lastContact.FindElement(By.CssSelector("tr.fname > td")).Text, Is.EqualTo(firstName));
            Assert.That(lastContact.FindElement(By.CssSelector("tr.lname > td")).Text, Is.EqualTo(lastName));
        }
    }
}
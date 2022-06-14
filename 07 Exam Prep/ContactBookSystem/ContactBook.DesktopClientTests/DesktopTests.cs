using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace ContactBook.DesktopClientTests
{
    public class DesktopTests
    {
        private const string ApiKey = "https://contactbook.nakov.repl.co/api";
        private AppiumLocalService appiumLocalService;
        private WindowsDriver<WindowsElement> driver;

        [OneTimeSetUp]
        public void Setup()
        {
            //Start Appium Local Service
            appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            appiumLocalService.Start();

            // Initialize and setup Appium Options
            AppiumOptions appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Windows");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.App, @"C:\Users\Oleg\Downloads\ContactBook-DesktopClient\ContactBook-DesktopClient.exe");

            // Initialize WebDriver
            driver = new WindowsDriver<WindowsElement>(appiumLocalService, appiumOptions);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            
        }

        [Test]
        public void Test_SearchWIthKeywordSteve_AssertResultIsSteveJobs()
        {
            const string keyword = "steve";

            const string expectedLabelSearchresult = "Contacts found:";

            const string expectedFirstName = "Steve";
            const string expectedLastName = "Jobs";

            WindowsElement ApiInputField = this.driver.FindElementByAccessibilityId("textBoxApiUrl");
            ApiInputField.Clear();
            ApiInputField.SendKeys(ApiKey);

            WindowsElement connectBtn = this.driver.FindElementByAccessibilityId("buttonConnect");
            connectBtn.Click();

            //Change Windows
            var windowHandlers = this.driver.WindowHandles;
            var searchContactsWindow = windowHandlers[0];
            this.driver.SwitchTo().Window(searchContactsWindow);

            var searchInputBox = this.driver.FindElementByAccessibilityId("textBoxSearch");
            searchInputBox.Clear();
            searchInputBox.SendKeys(keyword);

            var btnSearch = this.driver.FindElementByAccessibilityId("buttonSearch");
            btnSearch.Click();

            // Option 1: Not Working
            //this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            //Option 2: Slow
            //Thread.Sleep(1000);

            //Option 3: Explicit Wait
            WebDriverWait wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));

            var isLabelChanged = wait.Until(d =>
            {
                var labelSearchResult = this.driver.FindElementByAccessibilityId("labelResult").Text;
                return labelSearchResult.StartsWith(expectedLabelSearchresult);
            });

            var labelSearchResult = this.driver.FindElementByAccessibilityId("labelResult").Text;

            Assert.That(labelSearchResult.Substring(0, expectedLabelSearchresult.Length), Is.EqualTo(expectedLabelSearchresult));

            var firstName = this.driver.FindElementByXPath("//Edit[@Name=\"FirstName Row 0, Not sorted.\"]").Text;
            var lastName = this.driver.FindElementByXPath("//Edit[@Name=\"LastName Row 0, Not sorted.\"]").Text;

            Assert.That(firstName, Is.EqualTo(expectedFirstName));
            Assert.That(lastName, Is.EqualTo(expectedLastName));
        }
    }
}
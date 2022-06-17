using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;

namespace DesktopTests
{
    public class Desktop_Tests
    {
        private const string AppiumUrl = "http://127.0.0.1:4723/wd/hub";
        private const string appUrl = "https://taskboard.nakov.repl.co/api";
        private const string appLocation = @"C:\Users\Oleg\Downloads\TaskBoard.DesktopClient-v1.0\TaskBoard.DesktopClient.exe";
        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;
        private AppiumLocalService appiumLocalService;

        [SetUp]
        public void Setup()
        {
            appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            appiumLocalService.Start();

            options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability("app", appLocation);

            driver = new WindowsDriver<WindowsElement>(appiumLocalService, options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]
        public void ShutDown()
        {
            driver.Quit();
        }

        [Test]
        public void Test_AddTask_ValidInput()
        {
            string newTitle = "New Task " + DateTime.Now.Ticks;
            string newDesc = "New Description " + DateTime.Now.Ticks;

            var conectionField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            conectionField.Clear();
            conectionField.SendKeys(appUrl);

            var connectButton = driver.FindElementByAccessibilityId("buttonConnect");
            connectButton.Click();

            var countTasksBefore = this.driver.FindElementsByXPath("/Window/List/Group[@Name=\"Open\"]/ListItem").Count;

            driver.FindElementByAccessibilityId("buttonAdd").Click();

            //Thread.Sleep(3000);
            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(d =>
            {
                string windowsName = driver.WindowHandles[0];
                driver.SwitchTo().Window(windowsName);

                return this.driver.FindElementByAccessibilityId("buttonCreate");
            });

            string windowsName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsName);

            driver.FindElementByAccessibilityId("textBoxTitle").SendKeys(newTitle);
            driver.FindElementByAccessibilityId("textBoxDescription").SendKeys(newDesc);
            driver.FindElementByAccessibilityId("buttonCreate").Click();

            var countTasksAfter = this.driver.FindElementsByXPath("/Window/List/Group[@Name=\"Open\"]/ListItem").Count;

            // To be further reworked:
            //var listItems = this.driver.FindElementsByXPath("/Window/List/Group[@Name=\"Open\"]/ListItem");
            //var smt = listItems[listItems.Count - 1];
            //var smt1 = listItems[listItems.Count - 1].Text;

            //var xpath = "/Window/List/Group[@Name=\"Open\"]/ListItem[@Name=\"" + smt + "\"]/Text";
            //var elements = smt.FindElementsByName(xpath);

            Assert.That(countTasksAfter, Is.GreaterThan(countTasksBefore));
        }

        [Test]
        public void Test_AddTask_InvalidInput()
        {
            var conectionField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            conectionField.Clear();
            conectionField.SendKeys(appUrl);

            var connectButton = driver.FindElementByAccessibilityId("buttonConnect");
            connectButton.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(d =>
            {
                return this.driver.FindElementsByXPath("/Window/List/Group/ListItem").Count > 0;
            });

            var countTasksBefore = this.driver.FindElementsByXPath("/Window/List/Group/ListItem").Count;

            driver.FindElementByAccessibilityId("buttonAdd").Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(d =>
            {
                string windowsName = driver.WindowHandles[0];
                driver.SwitchTo().Window(windowsName);

                return this.driver.FindElementByAccessibilityId("buttonCreate");
            });

            string windowsName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsName);

            driver.FindElementByAccessibilityId("textBoxTitle").Clear();
            driver.FindElementByAccessibilityId("textBoxDescription").Clear();
            driver.FindElementByAccessibilityId("buttonCreate").Click();

            var countTasksAfter = this.driver.FindElementsByXPath("/Window/List/Group/ListItem").Count;

            Assert.That(countTasksAfter == countTasksBefore);
        }

        [Test]
        public void Test_SearchTask_ValidKeyword()
        {
            string newTitle = "New Task " + DateTime.Now.Ticks;
            string newDesc = "New Description " + DateTime.Now.Ticks;

            // Connect To API
            var conectionField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            conectionField.Clear();
            conectionField.SendKeys(appUrl);

            var connectButton = driver.FindElementByAccessibilityId("buttonConnect");
            connectButton.Click();

            //Create New Task
            driver.FindElementByAccessibilityId("buttonAdd").Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(d =>
            {
                string windowsName = driver.WindowHandles[0];
                driver.SwitchTo().Window(windowsName);

                return this.driver.FindElementByAccessibilityId("buttonCreate");
            });

            string windowsName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsName);

            driver.FindElementByAccessibilityId("textBoxTitle").SendKeys(newTitle);
            driver.FindElementByAccessibilityId("textBoxDescription").SendKeys(newDesc);
            driver.FindElementByAccessibilityId("buttonCreate").Click();

            var textBoxSearch = driver.FindElementByAccessibilityId("textBoxSearchText");
            textBoxSearch.SendKeys(newTitle);

            driver.FindElementByAccessibilityId("buttonSearch").Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(d =>
            {
                return this.driver.FindElementsByXPath("/Window/List/Group/ListItem");
            });

            var tasks = this.driver.FindElementsByXPath("/Window/List/Group/ListItem");

            Assert.That(tasks.Count, Is.GreaterThan(0));
        }

        [Test]
        public void Test_SearchTask_InvalidKeyword()
        {
            string keyword = "Invalid Rand " + DateTime.Now.Ticks;

            // Connect To API
            var conectionField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            conectionField.Clear();
            conectionField.SendKeys(appUrl);

            var connectButton = driver.FindElementByAccessibilityId("buttonConnect");
            connectButton.Click();

            //Search
            var textBoxSearch = driver.FindElementByAccessibilityId("textBoxSearchText");
            textBoxSearch.SendKeys(keyword);

            driver.FindElementByAccessibilityId("buttonSearch").Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(d =>
            {
                return this.driver.FindElementsByXPath("/Window/List/Group/ListItem");
            });

            var tasks = this.driver.FindElementsByXPath("/Window/List/Group/ListItem");

            Assert.That(tasks.Count == 0);
        }
    }
}
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;

namespace FinalExam.WindowsAppTests
{
    public class DesktopTests
    {
        private const string appUrl = "https://taskboard.okuzmanov.repl.co/api";
        private const string appLocation = @"C:\TaskBoard.DesktopClient-v1.0\TaskBoard.DesktopClient.exe";
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
        public void Test_SearchValidKeyword()
        {
            const string keyword = "Project skeleton";

            var conectionField = this.driver.FindElementByAccessibilityId("textBoxApiUrl");
            conectionField.Clear();
            conectionField.SendKeys(appUrl);

            var connectButton = this.driver.FindElementByAccessibilityId("buttonConnect");
            connectButton.Click();

            var searchInput = this.driver.FindElementByAccessibilityId("textBoxSearchText");
            searchInput.Clear();
            searchInput.SendKeys(keyword);

            var searchBtn = this.driver.FindElementByAccessibilityId("buttonSearch");
            searchBtn.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(d =>
            {
                return this.driver.FindElementsByXPath("/Window/List/Group/ListItem").Count > 0;
            });

            var tasks = this.driver.FindElementsByXPath("/Window/List/Group/ListItem");

            Assert.That(tasks.Count > 0);
        }

        [Test]
        public void Test_CreateNewTask_ValidData()
        {
            string title = "New Title" + DateTime.Now.Ticks;
            string description = "New Description" + DateTime.Now.Ticks;

            var conectionField = this.driver.FindElementByAccessibilityId("textBoxApiUrl");
            conectionField.Clear();
            conectionField.SendKeys(appUrl);

            var connectButton = this.driver.FindElementByAccessibilityId("buttonConnect");
            connectButton.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(d =>
            {
                return this.driver.FindElementsByXPath("/Window/List/Group/ListItem").Count>0;
            });

            var countTaskBefore = this.driver.FindElementsByXPath("/Window/List/Group/ListItem").Count;

            //Create Task
            var createBtn = this.driver.FindElementByAccessibilityId("buttonAdd");
            createBtn.Click();

            var titleInput = this.driver.FindElementByAccessibilityId("textBoxTitle");
            titleInput.SendKeys(title);

            var descriptionInput = this.driver.FindElementByAccessibilityId("textBoxDescription");
            descriptionInput.SendKeys(description);

            var submit = this.driver.FindElementByAccessibilityId("buttonCreate");
            submit.Click();

            var reload = this.driver.FindElementByAccessibilityId("buttonReload");
            reload.Click();

            Thread.Sleep(5000);

            var countTaskAfter = this.driver.FindElementsByXPath("/Window/List/Group/ListItem").Count;

            Assert.That(countTaskAfter > countTaskBefore);

            var searchInput = this.driver.FindElementByAccessibilityId("textBoxSearchText");
            searchInput.Clear();
            searchInput.SendKeys(title);

            var searchBtn = this.driver.FindElementByAccessibilityId("buttonSearch");
            searchBtn.Click();

            Thread.Sleep(5000);

            var tasks = this.driver.FindElementsByXPath("/Window/List/Group/ListItem");

            Assert.That(tasks.Count == 1);
        }

        [Test]
        public void Test_CreateNewTask_InvalidData()
        {
            var conectionField = this.driver.FindElementByAccessibilityId("textBoxApiUrl");
            conectionField.Clear();
            conectionField.SendKeys(appUrl);

            var connectButton = this.driver.FindElementByAccessibilityId("buttonConnect");
            connectButton.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(d =>
            {
                return this.driver.FindElementsByXPath("/Window/List/Group/ListItem").Count > 0;
            });

            var countTaskBefore = this.driver.FindElementsByXPath("/Window/List/Group/ListItem").Count;

            //Create Task
            var createBtn = this.driver.FindElementByAccessibilityId("buttonAdd");
            createBtn.Click();

            var submit = this.driver.FindElementByAccessibilityId("buttonCreate");
            submit.Click();

            var reload = this.driver.FindElementByAccessibilityId("buttonReload");
            reload.Click();

            Thread.Sleep(5000);

            var countTaskAfter = this.driver.FindElementsByXPath("/Window/List/Group/ListItem").Count;

            Assert.That(countTaskAfter == countTaskBefore);
        }
    }
}
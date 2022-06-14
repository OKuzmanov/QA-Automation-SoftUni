using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.IO;
using System.Threading;

namespace AppiumTests7ZipExercise
{
    public class SevenZipTests
    {
        private WindowsDriver<WindowsElement> driver;
        private WindowsDriver<WindowsElement> desktopDriver;
        //private const string appiumServerUrl = "http://127.0.0.1:4723/wd/hub";
        private AppiumLocalService appiumLocalService; 
        private string workDir;

        [OneTimeSetUp]
        public void Setup()
        {
            appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            appiumLocalService.Start();

            AppiumOptions options = new AppiumOptions();
            options.AddAdditionalCapability("platform", "Widnows");
            options.AddAdditionalCapability("app", @"C:\Program Files\7-Zip\7zFM.exe");
            this.driver = new WindowsDriver<WindowsElement>(appiumLocalService, options);

            var desktopOptions = new AppiumOptions();
            desktopOptions.AddAdditionalCapability("platform", "Windows");
            desktopOptions.AddAdditionalCapability("app", "Root");
            this.desktopDriver = new WindowsDriver<WindowsElement>(appiumLocalService, desktopOptions);

            workDir = Directory.GetCurrentDirectory() + @"\workdir";
            if (Directory.Exists(workDir))
            {
                Directory.Delete(workDir, true);
            }

            Directory.CreateDirectory(workDir);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            this.driver.CloseApp();
            this.driver.Quit();
        }

        [Test]
        public void Test_7Zip()
        {
            WindowsElement textBoxLocationFolder = this.driver.FindElementByXPath("/Window/Pane/Pane/ComboBox/Edit");
            textBoxLocationFolder.SendKeys(@"C:\Program Files\7-Zip" + Keys.Enter);

            //WindowsElement listBoxFiles = this.driver.FindElementByXPath("/Window/Pane/List");
            //WindowsElement listBoxFiles = this.driver.FindElementByAccessibilityId("1001");
            WindowsElement listBoxFiles = this.driver.FindElementByClassName("SysListView32");
            listBoxFiles.SendKeys(Keys.Control + "a");

            WindowsElement addBtn = this.driver.FindElementByName("Add");
            addBtn.Click();

            Thread.Sleep(500);
            WindowsElement windowAddToArchive = desktopDriver.FindElementByName("Add to Archive");

            AppiumWebElement textBoxArchiveName = windowAddToArchive.FindElementByXPath("/Window/ComboBox/Edit[@Name='Archive:']");
            string archiveFileName = workDir + "\\" + DateTime.Now.Ticks + ".7z";
            textBoxArchiveName.SendKeys(archiveFileName);

            AppiumWebElement archiveFormat =  windowAddToArchive.FindElementByXPath("/Window/ComboBox[@Name='Archive format:']");
            archiveFormat.SendKeys("7z");

            AppiumWebElement compressionLevel = windowAddToArchive.FindElementByXPath("/Window/ComboBox[@Name='Compression level:']");
            compressionLevel.SendKeys(Keys.End);

            //AppiumWebElement dictionarySize = windowAddToArchive.FindElementByXPath("/Window/ComboBox[@Name='Dictionary size:']");
            //dictionarySize.SendKeys(Keys.End);

            //AppiumWebElement wordSize = windowAddToArchive.FindElementByXPath("/Window/ComboBox[@Name='Word size:']");
            //wordSize.SendKeys(Keys.End);

            AppiumWebElement okBttn = windowAddToArchive.FindElementByXPath("/Window/Button[@Name='OK']");
            okBttn.Click();

            Thread.Sleep(1000);

            textBoxLocationFolder.SendKeys(archiveFileName + Keys.Enter);

            WindowsElement extractBttn = this.driver.FindElementByXPath("/Window/ToolBar/Button[@Name='Extract']");
            extractBttn.Click();

            WindowsElement okExtractBttn = this.driver.FindElementByXPath("/Window/Window/Button[@Name='OK']");
            okExtractBttn.Click();

            Thread.Sleep(1000);

            string executable7ZipOriginal = @"C:\Program Files\7-Zip\7zFM.exe";
            string executable7ZipExtracted = workDir + @"\7zFM.exe";

            FileAssert.AreEqual(executable7ZipOriginal, executable7ZipExtracted);
        }
    }
}
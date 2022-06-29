using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace FinalExam.WebUITests
{
    public class WebUITests
    {
        const string baseUrl = "https://taskboard.nakov.repl.co";
        private WebDriver driver;

        [OneTimeSetUp]
        public void Setup()
        {
            this.driver = new ChromeDriver();

            this.driver.Manage().Window.Maximize();

            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            this.driver.Quit();
        }

        [Test]
        public void Test_TaskBoard_AssertFirstTaskDoneBoard()
        {
            const string firstTaskTitleDoneBoardExpected = "Project skeleton";
            const string TaskBoardTitle = "Task Board – Task Board";

            this.driver.Navigate().GoToUrl(baseUrl);

            this.driver.FindElement(By.LinkText("Task Board")).Click();

            Assert.AreEqual(TaskBoardTitle, this.driver.Title);

            var firstTaskTitleDoneBoardActual = this.driver.FindElement(By.CssSelector("#task1 > tbody > tr.title > td")).Text;

            Assert.AreEqual(firstTaskTitleDoneBoardExpected, firstTaskTitleDoneBoardActual);
        }

        [Test]
        public void Test_SearchByValidKeyword_Homepage()
        {
            const string keyword = "Home page";
            const string TaskBoardTitle = "Search Tasks – Task Board";

            string expectedTitle = "Home page";
            string expectedDescription = "Create the [Home] page and list tasks count by board";

            this.driver.Navigate().GoToUrl(baseUrl);

            this.driver.FindElement(By.LinkText("Search")).Click();

            Assert.AreEqual(TaskBoardTitle, this.driver.Title);

            IWebElement keywordInput = this.driver.FindElement(By.Id("keyword"));
            keywordInput.SendKeys(keyword);

            IWebElement searchBtn = this.driver.FindElement(By.Id("search"));
            searchBtn.Click();

            var tasks = this.driver.FindElements(By.ClassName("task"));

            var firstTask = tasks[0];

            Assert.That(tasks.Count > 0);
            Assert.That(expectedTitle, Is.EqualTo(firstTask.FindElement(By.CssSelector("#task2 > tbody > tr.title > td")).Text));
            Assert.That(expectedDescription, Is.EqualTo(firstTask.FindElement(By.CssSelector("#task2 > tbody > tr.description > td > div")).Text));
        }

        [Test]
        public void Test_SearchByInvalidKeyword()
        {
            string keyword = "missing" + DateTime.Now.Ticks;
            const string TaskBoardTitle = "Search Tasks – Task Board";
            const string errMsg = "No tasks found.";

            this.driver.Navigate().GoToUrl(baseUrl);

            this.driver.FindElement(By.LinkText("Search")).Click();

            Assert.AreEqual(TaskBoardTitle, this.driver.Title);

            IWebElement keywordInput = this.driver.FindElement(By.Id("keyword"));
            keywordInput.SendKeys(keyword);

            IWebElement searchBtn = this.driver.FindElement(By.Id("search"));
            searchBtn.Click();

            var tasks = this.driver.FindElements(By.ClassName("task"));

            IWebElement errTextBox = this.driver.FindElement(By.Id("searchResult"));

            Assert.That(tasks.Count == 0);
            Assert.That(errTextBox.Text, Is.EqualTo(errMsg));
        }

        [Test]
        public void Test_CreateTask_ValidData()
        {
            string newTitle = "missing" + DateTime.Now.Ticks;
            string newDescription = "missing" + DateTime.Now.Ticks;
            string board = "Open";

            const string TaskBoardTitle = "Create Task – Task Board";

            this.driver.Navigate().GoToUrl(baseUrl);

            //Get All Tasks Before Create
            this.driver.FindElement(By.LinkText("Task Board")).Click();
            var allTasksBefore = this.driver.FindElements(By.ClassName("task-entry"));

            //Create Task
            this.driver.FindElement(By.LinkText("Create")).Click();

            Assert.AreEqual(TaskBoardTitle, this.driver.Title);

            IWebElement titleInput = this.driver.FindElement(By.Id("title"));
            titleInput.SendKeys(newTitle);

            IWebElement descriptionInput = this.driver.FindElement(By.Id("description"));
            descriptionInput.SendKeys(newDescription);

            SelectElement drpTaskState = new SelectElement(driver.FindElement(By.Id("boardName")));
            drpTaskState.SelectByValue(board);

            IWebElement searchBtn = this.driver.FindElement(By.Id("create"));
            searchBtn.Click();

            var allTasksAfter = this.driver.FindElements(By.ClassName("task-entry"));

            Assert.That(allTasksBefore.Count < allTasksAfter.Count);

            var boardTasks = this.driver.FindElements(By.ClassName("task"));

            if (board.Equals("Open"))
            {
                IWebElement lastTask = boardTasks[0].FindElements(By.ClassName("task-entry")).Last();
                Assert.AreEqual(newTitle, lastTask.FindElement(By.CssSelector("tbody > tr.title > td")).Text);
                Assert.AreEqual(newDescription, lastTask.FindElement(By.CssSelector("tbody > tr.description > td > div")).Text);
            } else if (board.Equals("In Progress"))
            {
                IWebElement lastTask = boardTasks[1].FindElements(By.ClassName("task-entry")).Last();
                Assert.AreEqual(newTitle, lastTask.FindElement(By.CssSelector("tbody > tr.title > td")).Text);
                Assert.AreEqual(newDescription, lastTask.FindElement(By.CssSelector("tbody > tr.description > td > div")).Text);
            } else if (board.Equals("Done"))
            {
                IWebElement lastTask = boardTasks[2].FindElements(By.ClassName("task-entry")).Last();
                Assert.AreEqual(newTitle, lastTask.FindElement(By.CssSelector("tbody > tr.title > td")).Text);
                Assert.AreEqual(newDescription, lastTask.FindElement(By.CssSelector("tbody > tr.description > td > div")).Text);
            }
        }

        [Test]
        public void Test_CreateTask_InvalidData()
        {
            const string TaskBoardTitle = "Create Task – Task Board";
            const string errMsg = "Error: Title cannot be empty!";

            this.driver.Navigate().GoToUrl(baseUrl);

            //Get All Tasks Before Create
            this.driver.FindElement(By.LinkText("Task Board")).Click();
            var allTasksBefore = this.driver.FindElements(By.ClassName("task-entry"));

            //Create Task
            this.driver.FindElement(By.LinkText("Create")).Click();

            Assert.AreEqual(TaskBoardTitle, this.driver.Title);

            IWebElement searchBtn = this.driver.FindElement(By.Id("create"));
            searchBtn.Click();

            var errLabel = this.driver.FindElement(By.ClassName("err"));

            Assert.That(errLabel.Text, Is.EqualTo(errMsg));
            Assert.AreEqual(TaskBoardTitle, this.driver.Title);

            //Get All Tasks After
            this.driver.FindElement(By.LinkText("Task Board")).Click();
            var allTasksAfter = this.driver.FindElements(By.ClassName("task-entry"));

            Assert.That(allTasksBefore.Count == allTasksAfter.Count);
        }
    }
}
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace TaskBoard.WebUITests
{
    public class UITests
    {
        public const string url = "https://taskboard.nakov.repl.co";
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
        public void Test_CreateOpenTask_ValidData()
        {
            driver.Navigate().GoToUrl(url);

            var countAllTasksBefore = this.driver.FindElement(By.CssSelector("body > main > section > span > b")).Text;

            var countOpenTasksBefore = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(1) > b")).Text;

            driver.FindElement(By.LinkText("Create")).Click();

            Assert.That(driver.Title, Is.EqualTo("Create Task – Task Board"));

            string newTitle = "New task " + DateTime.Now.Ticks;
            string newDescription = "My new task to test the app " + DateTime.Now.Ticks;

            driver.FindElement(By.Id("title")).SendKeys(newTitle);
            driver.FindElement(By.Id("description")).SendKeys(newDescription);
            SelectElement drpTaskState = new SelectElement(driver.FindElement(By.Id("boardName")));
            drpTaskState.SelectByValue("Open");
            driver.FindElement(By.Id("create")).Click();

            Assert.That(driver.Title, Is.EqualTo("Task Board – Task Board"));

            var taskSections = driver.FindElements(By.ClassName("task"));

            var openTasks = taskSections[0].FindElements(By.TagName("table"));

            var taskTitle = openTasks.Last().FindElement(By.ClassName("title")).Text;
            var taskDescription = openTasks.Last().FindElement(By.ClassName("description")).Text;

            taskTitle = taskTitle.Substring("Title ".Length);
            taskDescription = taskDescription.Substring("Description\r\n".Length);

            Assert.That(newTitle, Is.EqualTo(taskTitle));
            Assert.That(newDescription, Is.EqualTo(taskDescription));

            driver.Navigate().GoToUrl(url);

            var countAllTasksAfter = this.driver.FindElement(By.CssSelector("body > main > section > span > b")).Text;

            var countOpenTasksAfter = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(1) > b")).Text;

            Assert.That(int.Parse(countAllTasksAfter), Is.GreaterThan(int.Parse(countAllTasksBefore)));
            Assert.That(int.Parse(countOpenTasksAfter), Is.GreaterThan(int.Parse(countOpenTasksBefore)));
        }

        [Test]
        public void Test_CreateInProgressTask_ValidData()
        {
            driver.Navigate().GoToUrl(url);

            var countAllTasksBefore = this.driver.FindElement(By.CssSelector("body > main > section > span > b")).Text;

            var countInProgressTasksBefore = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(2) > b")).Text;

            driver.FindElement(By.LinkText("Create")).Click();

            Assert.That(driver.Title, Is.EqualTo("Create Task – Task Board"));

            string newTitle = "New task " + DateTime.Now.Ticks;
            string newDescription = "My new task to test the app " + DateTime.Now.Ticks;

            driver.FindElement(By.Id("title")).SendKeys(newTitle);
            driver.FindElement(By.Id("description")).SendKeys(newDescription);
            SelectElement drpTaskState = new SelectElement(driver.FindElement(By.Id("boardName")));
            drpTaskState.SelectByValue("In Progress");
            driver.FindElement(By.Id("create")).Click();

            Assert.That(driver.Title, Is.EqualTo("Task Board – Task Board"));

            var taskSections = driver.FindElements(By.ClassName("task"));

            var inProgressTasks = taskSections[1].FindElements(By.TagName("table"));

            var taskTitle = inProgressTasks.Last().FindElement(By.ClassName("title")).Text;
            var taskDescription = inProgressTasks.Last().FindElement(By.ClassName("description")).Text;

            taskTitle = taskTitle.Substring("Title ".Length);
            taskDescription = taskDescription.Substring("Description\r\n".Length);

            Assert.That(newTitle, Is.EqualTo(taskTitle));
            Assert.That(newDescription, Is.EqualTo(taskDescription));

            driver.Navigate().GoToUrl(url);

            var countAllTasksAfter = this.driver.FindElement(By.CssSelector("body > main > section > span > b")).Text;

            var countInProgressTasksAfter = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(2) > b")).Text;

            Assert.That(int.Parse(countAllTasksAfter), Is.GreaterThan(int.Parse(countAllTasksBefore)));
            Assert.That(int.Parse(countInProgressTasksAfter), Is.GreaterThan(int.Parse(countInProgressTasksBefore)));
        }

        [Test]
        public void Test_CreateDone_ValidData()
        {
            driver.Navigate().GoToUrl(url);

            var countAllTasksBefore = this.driver.FindElement(By.CssSelector("body > main > section > span > b")).Text;

            var countDoneTasksBefore = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(3) > b")).Text;

            driver.FindElement(By.LinkText("Create")).Click();

            Assert.That(driver.Title, Is.EqualTo("Create Task – Task Board"));

            string newTitle = "New task " + DateTime.Now.Ticks;
            string newDescription = "My new task to test the app " + DateTime.Now.Ticks;

            driver.FindElement(By.Id("title")).SendKeys(newTitle);
            driver.FindElement(By.Id("description")).SendKeys(newDescription);
            SelectElement drpTaskState = new SelectElement(driver.FindElement(By.Id("boardName")));
            drpTaskState.SelectByValue("Done");
            driver.FindElement(By.Id("create")).Click();

            Assert.That(driver.Title, Is.EqualTo("Task Board – Task Board"));

            var taskSections = driver.FindElements(By.ClassName("task"));

            var doneTasks = taskSections[2].FindElements(By.TagName("table"));

            var taskTitle = doneTasks.Last().FindElement(By.ClassName("title")).Text;
            var taskDescription = doneTasks.Last().FindElement(By.ClassName("description")).Text;

            taskTitle = taskTitle.Substring("Title ".Length);
            taskDescription = taskDescription.Substring("Description\r\n".Length);

            Assert.That(newTitle, Is.EqualTo(taskTitle));
            Assert.That(newDescription, Is.EqualTo(taskDescription));

            driver.Navigate().GoToUrl(url);

            var countAllTasksAfter = this.driver.FindElement(By.CssSelector("body > main > section > span > b")).Text;

            var countDoneTasksAfter = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(3) > b")).Text;

            Assert.That(int.Parse(countAllTasksAfter), Is.GreaterThan(int.Parse(countAllTasksBefore)));
            Assert.That(int.Parse(countDoneTasksAfter), Is.GreaterThan(int.Parse(countDoneTasksBefore)));
        }

        [Test]
        public void Test_CreateTask_InvalidData_NoTitle()
        {
            const string errMessage = "Error: Title cannot be empty!";

            driver.Navigate().GoToUrl(url);

            driver.FindElement(By.LinkText("Create")).Click();

            Assert.That(driver.Title, Is.EqualTo("Create Task – Task Board"));

            string newDescription = "My new task to test the app " + DateTime.Now.Ticks;

            driver.FindElement(By.Id("description")).SendKeys(newDescription);

            driver.FindElement(By.Id("create")).Click();

            var errElem = this.driver.FindElement(By.CssSelector("body > main > div"));

            Assert.That(errElem.Text, Is.EqualTo(errMessage));
        }

        [Test]
        public void Test_SearchByValidKeyword()
        {
            const string errSearchMsg = "No tasks found.";

            driver.Navigate().GoToUrl(url);

            driver.FindElement(By.LinkText("Create")).Click();

            string newTitle = "New task " + DateTime.Now.Ticks;
            string newDescription = "My new task to test the app " + DateTime.Now.Ticks;

            driver.FindElement(By.Id("title")).SendKeys(newTitle);
            driver.FindElement(By.Id("description")).SendKeys(newDescription);
            SelectElement drpTaskState = new SelectElement(driver.FindElement(By.Id("boardName")));
            drpTaskState.SelectByValue("Open");
            driver.FindElement(By.Id("create")).Click();

            var searchHref = this.driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(4) > a"));
            searchHref.Click();

            Assert.That(driver.Title, Is.EqualTo("Search Tasks – Task Board"));

            var searchInput = this.driver.FindElement(By.Id("keyword"));
            searchInput.SendKeys(newTitle);

            var btnSearch = this.driver.FindElement(By.Id("search"));
            btnSearch.Click();

            var searchResult = this.driver.FindElement(By.Id("searchResult"));
            Assert.AreNotEqual(errSearchMsg, searchResult.Text);

            var tasks = this.driver.FindElements(By.ClassName("task"));

            var firstSearchResultTitle = tasks[0].FindElement(By.ClassName("title")).Text.Substring("Title ".Length);
            var firstSearchResultDescription = tasks[0].FindElement(By.ClassName("description")).Text.Substring("Description\r\n".Length);

            Assert.That(firstSearchResultTitle, Is.EqualTo(newTitle));
            Assert.That(firstSearchResultDescription, Is.EqualTo(newDescription));
        }

        [Test]
        public void Test_SearchByInvalidKeyword()
        {
            const string errSearchMsg = "No tasks found.";
            string keyword = "InvalidRand " + DateTime.Now.Ticks;

            driver.Navigate().GoToUrl(url);

            var searchHref = this.driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(4) > a"));
            searchHref.Click();

            var searchInput = this.driver.FindElement(By.Id("keyword"));
            searchInput.SendKeys(keyword);

            var btnSearch = this.driver.FindElement(By.Id("search"));
            btnSearch.Click();

            var searchResult = this.driver.FindElement(By.Id("searchResult"));

            Assert.AreEqual(errSearchMsg, searchResult.Text);
        }

        [Test]
        public void Test_EditTask_MoveToInProgress()
        {
            string newTitle = "New task " + DateTime.Now.Ticks;
            string newDescription = "My new task to test the app " + DateTime.Now.Ticks;

            string editedTitle = "Edited task " + DateTime.Now.Ticks;
            string editedDescription = "Edited task to test the app " + DateTime.Now.Ticks;

            driver.Navigate().GoToUrl(url);

            driver.FindElement(By.LinkText("Create")).Click();

            driver.FindElement(By.Id("title")).SendKeys(newTitle);
            driver.FindElement(By.Id("description")).SendKeys(newDescription);
            SelectElement drpTaskState = new SelectElement(driver.FindElement(By.Id("boardName")));
            drpTaskState.SelectByValue("Open");
            driver.FindElement(By.Id("create")).Click();

            driver.Navigate().GoToUrl(url);

            var countAllTasksBefore = this.driver.FindElement(By.CssSelector("body > main > section > span > b")).Text;

            var countOpenTasksBefore = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(1) > b")).Text;

            var countInProgressTasksBefore = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(2) > b")).Text;

            this.driver.FindElement(By.LinkText("Search")).Click();

            var searchInput = this.driver.FindElement(By.Id("keyword"));
            searchInput.SendKeys(newTitle);

            var btnSearch = this.driver.FindElement(By.Id("search"));
            btnSearch.Click();

            var tasks = this.driver.FindElements(By.ClassName("task"));

            IWebElement btnEdit = tasks[0].FindElement(By.CssSelector("tbody > tr.actions > td > a:nth-child(3)"));
            btnEdit.Click();

            var editTitleInput = this.driver.FindElement(By.Id("title"));
            editTitleInput.Clear();
            editTitleInput.SendKeys(editedTitle);

            var editDescInput = this.driver.FindElement(By.Id("description"));
            editDescInput.Clear();
            editDescInput.SendKeys(editedDescription);

            SelectElement drpBoard = new SelectElement(this.driver.FindElement(By.Id("boardName")));
            drpBoard.SelectByValue("In Progress");

            this.driver.FindElement(By.Id("create")).Click();

            var inProgressBoard = this.driver.FindElements(By.ClassName("task"))[1];
            var lastTask = inProgressBoard.FindElements(By.ClassName("task-entry")).Last();

            var titleLastInProgressTask = lastTask.FindElement(By.ClassName("title")).Text.Substring("Title ".Length);
            var descriptionLastInProgressTask = lastTask.FindElement(By.ClassName("description")).Text.Substring("Description\r\n".Length);

            Assert.That(titleLastInProgressTask, Is.EqualTo(editedTitle));
            Assert.That(descriptionLastInProgressTask, Is.EqualTo(editedDescription));

            this.driver.FindElement(By.LinkText("Home")).Click();

            var countAllTasksAfter = this.driver.FindElement(By.CssSelector("body > main > section > span > b")).Text;

            var countOpenTasksAfter = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(1) > b")).Text;

            var countInProgressTasksAfter = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(2) > b")).Text;

            Assert.That(int.Parse(countAllTasksBefore), Is.EqualTo(int.Parse(countAllTasksAfter)));
            Assert.That(int.Parse(countOpenTasksAfter), Is.LessThan(int.Parse(countOpenTasksBefore)));
            Assert.That(int.Parse(countInProgressTasksAfter), Is.GreaterThan(int.Parse(countInProgressTasksBefore)));
        }

        [Test]
        public void Test_EditTask_MoveToDone()
        {
            string newTitle = "New task " + DateTime.Now.Ticks;
            string newDescription = "My new task to test the app " + DateTime.Now.Ticks;

            string editedTitle = "Edited task " + DateTime.Now.Ticks;
            string editedDescription = "Edited task to test the app " + DateTime.Now.Ticks;

            driver.Navigate().GoToUrl(url);

            driver.FindElement(By.LinkText("Create")).Click();

            driver.FindElement(By.Id("title")).SendKeys(newTitle);
            driver.FindElement(By.Id("description")).SendKeys(newDescription);
            SelectElement drpTaskState = new SelectElement(driver.FindElement(By.Id("boardName")));
            drpTaskState.SelectByValue("In Progress");
            driver.FindElement(By.Id("create")).Click();

            driver.Navigate().GoToUrl(url);

            var countAllTasksBefore = this.driver.FindElement(By.CssSelector("body > main > section > span > b")).Text;

            var countInProgressTasksBefore = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(2) > b")).Text;

            var countDoneTasksBefore = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(3) > b")).Text;

            this.driver.FindElement(By.LinkText("Search")).Click();

            var searchInput = this.driver.FindElement(By.Id("keyword"));
            searchInput.SendKeys(newTitle);

            var btnSearch = this.driver.FindElement(By.Id("search"));
            btnSearch.Click();

            var tasks = this.driver.FindElements(By.ClassName("task"));

            IWebElement btnEdit = tasks[0].FindElement(By.CssSelector("tbody > tr.actions > td > a:nth-child(3)"));
            btnEdit.Click();

            var editTitleInput = this.driver.FindElement(By.Id("title"));
            editTitleInput.Clear();
            editTitleInput.SendKeys(editedTitle);

            var editDescInput = this.driver.FindElement(By.Id("description"));
            editDescInput.Clear();
            editDescInput.SendKeys(editedDescription);

            SelectElement drpBoard = new SelectElement(this.driver.FindElement(By.Id("boardName")));
            drpBoard.SelectByValue("Done");

            this.driver.FindElement(By.Id("create")).Click();

            var inProgressBoard = this.driver.FindElements(By.ClassName("task"))[2];
            var lastTask = inProgressBoard.FindElements(By.ClassName("task-entry")).Last();

            var titleLastInProgressTask = lastTask.FindElement(By.ClassName("title")).Text.Substring("Title ".Length);
            var descriptionLastInProgressTask = lastTask.FindElement(By.ClassName("description")).Text.Substring("Description\r\n".Length);

            Assert.That(titleLastInProgressTask, Is.EqualTo(editedTitle));
            Assert.That(descriptionLastInProgressTask, Is.EqualTo(editedDescription));

            this.driver.FindElement(By.LinkText("Home")).Click();

            var countAllTasksAfter = this.driver.FindElement(By.CssSelector("body > main > section > span > b")).Text;

            var countInProgressTasksAfter = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(2) > b")).Text;

            var countDoneTasksAfter = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(3) > b")).Text;

            Assert.That(int.Parse(countAllTasksBefore), Is.EqualTo(int.Parse(countAllTasksAfter)));
            Assert.That(int.Parse(countInProgressTasksAfter), Is.LessThan(int.Parse(countInProgressTasksBefore)));
            Assert.That(int.Parse(countDoneTasksAfter), Is.GreaterThan(int.Parse(countDoneTasksBefore)));
        }

        [Test]
        public void Test_EditTask_MoveToOpen()
        {
            string newTitle = "New task " + DateTime.Now.Ticks;
            string newDescription = "My new task to test the app " + DateTime.Now.Ticks;

            string editedTitle = "Edited task " + DateTime.Now.Ticks;
            string editedDescription = "Edited task to test the app " + DateTime.Now.Ticks;

            driver.Navigate().GoToUrl(url);

            driver.FindElement(By.LinkText("Create")).Click();

            driver.FindElement(By.Id("title")).SendKeys(newTitle);
            driver.FindElement(By.Id("description")).SendKeys(newDescription);
            SelectElement drpTaskState = new SelectElement(driver.FindElement(By.Id("boardName")));
            drpTaskState.SelectByValue("Done");
            driver.FindElement(By.Id("create")).Click();

            driver.Navigate().GoToUrl(url);

            var countAllTasksBefore = this.driver.FindElement(By.CssSelector("body > main > section > span > b")).Text;

            var countDoneTasksBefore = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(3) > b")).Text;

            var countOpenTasksBefore = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(1) > b")).Text;

            this.driver.FindElement(By.LinkText("Search")).Click();

            var searchInput = this.driver.FindElement(By.Id("keyword"));
            searchInput.SendKeys(newTitle);

            var btnSearch = this.driver.FindElement(By.Id("search"));
            btnSearch.Click();

            var tasks = this.driver.FindElements(By.ClassName("task"));

            IWebElement btnEdit = tasks[0].FindElement(By.CssSelector("tbody > tr.actions > td > a:nth-child(3)"));
            btnEdit.Click();

            var editTitleInput = this.driver.FindElement(By.Id("title"));
            editTitleInput.Clear();
            editTitleInput.SendKeys(editedTitle);

            var editDescInput = this.driver.FindElement(By.Id("description"));
            editDescInput.Clear();
            editDescInput.SendKeys(editedDescription);

            SelectElement drpBoard = new SelectElement(this.driver.FindElement(By.Id("boardName")));
            drpBoard.SelectByValue("Open");

            this.driver.FindElement(By.Id("create")).Click();

            var OpenBoard = this.driver.FindElements(By.ClassName("task"))[0];
            var lastTask = OpenBoard.FindElements(By.ClassName("task-entry")).Last();

            var titleLastOpenTask = lastTask.FindElement(By.ClassName("title")).Text.Substring("Title ".Length);
            var descriptionLastOpenTask = lastTask.FindElement(By.ClassName("description")).Text.Substring("Description\r\n".Length);

            Assert.That(titleLastOpenTask, Is.EqualTo(editedTitle));
            Assert.That(descriptionLastOpenTask, Is.EqualTo(editedDescription));

            this.driver.FindElement(By.LinkText("Home")).Click();

            var countAllTasksAfter = this.driver.FindElement(By.CssSelector("body > main > section > span > b")).Text;

            var countDoneTasksAfter = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(3) > b")).Text;

            var countOpenTasksAfter = this.driver.FindElement(By.CssSelector("body > main > section > ul > li:nth-child(1) > b")).Text;

            Assert.That(int.Parse(countAllTasksBefore), Is.EqualTo(int.Parse(countAllTasksAfter)));
            Assert.That(int.Parse(countDoneTasksAfter), Is.LessThan(int.Parse(countDoneTasksBefore)));
            Assert.That(int.Parse(countOpenTasksAfter), Is.GreaterThan(int.Parse(countOpenTasksBefore)));
        }
    }
}
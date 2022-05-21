using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

var driver = new ChromeDriver();

driver.Navigate().GoToUrl("https:/softuni.bg");

Console.WriteLine("Page title: " + driver.Title);

driver.FindElement(By.TagName("a")).Click();

IWebElement searchBtn = driver.FindElement(By.Id("searchInput"));


searchBtn.SendKeys("Bulgaria");
searchBtn.SendKeys(Keys.Enter);

Console.WriteLine("New page title: " + driver.Title);


driver.Quit();

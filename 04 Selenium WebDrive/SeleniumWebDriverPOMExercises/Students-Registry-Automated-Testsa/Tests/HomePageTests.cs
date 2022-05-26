using NUnit.Framework;
using Students_Registry_Automated_Testsa.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students_Registry_Automated_Testsa.Tests
{
    public class HomePageTests : BaseTests
    {
        [Test]
        public void Test_HomePage_Content()
        {
            HomePage page = new HomePage(base.driver);

            page.Open();

            Assert.AreEqual("MVC Example", page.GetPageTitle());

            Assert.AreEqual("Students Registry", page.GetPageHeading());

            Assert.That(page.GetStudentsCount(), Is.AtLeast(3));
        }

        [Test]
        public void Test_HomePage_Links()
        {
            HomePage page = new HomePage(base.driver);

            page.Open();
            page.LinkHomePage.Click();
            Assert.AreEqual(page.PageUrl, driver.Url);

            page.Open();
            page.LinkViewStudentsPage.Click();
            Assert.AreEqual(new ViewStudentsPage(base.driver).PageUrl, driver.Url);

            page.Open();
            page.LinkAddStudentPage.Click();
            Assert.AreEqual(new AddStudentsPage(base.driver).PageUrl, driver.Url);
        }
    }
}

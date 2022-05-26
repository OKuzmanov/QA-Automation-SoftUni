using NUnit.Framework;
using Students_Registry_Automated_Testsa.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students_Registry_Automated_Testsa.Tests
{
    public class ViewStudetnsPageTests : BaseTests
    {
        [Test]
        public void Test_ViewStudentsPage_Content()
        {
            ViewStudentsPage viewPage = new ViewStudentsPage(base.driver);

            viewPage.Open();

            Assert.AreEqual("Students", viewPage.GetPageTitle());

            Assert.AreEqual("Registered Students", viewPage.GetPageHeading());

            HomePage homePage = new HomePage(base.driver);
            homePage.Open();
            int studentsCount = homePage.GetStudentsCount();

            viewPage.Open();

            Assert.AreEqual(studentsCount, viewPage.CountRegisteredStudents());
            
            string[] students = viewPage.GetRegisteredStudents();

            foreach (string student in students)
            {
                Assert.IsTrue(student.IndexOf("(") > 0);
                Assert.IsTrue(student.IndexOf(")") == student.Length - 1);
            }
        }

        [Test]
        public void Test_ViewStudentsPage_Links()
        {
            ViewStudentsPage page = new ViewStudentsPage(base.driver);

            page.Open();
            page.LinkHomePage.Click();
            Assert.AreEqual(new HomePage(base.driver).PageUrl, driver.Url);

            page.Open();
            page.LinkViewStudentsPage.Click();
            Assert.AreEqual(page.PageUrl, driver.Url);

            page.Open();
            page.LinkAddStudentPage.Click();
            Assert.AreEqual(new AddStudentsPage(base.driver).PageUrl, driver.Url);
        }


    }
}

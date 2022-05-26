using NUnit.Framework;
using Students_Registry_Automated_Testsa.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students_Registry_Automated_Testsa.Tests
{
    public class AddStudentsPageTests : BaseTests
    {

        [Test]
        public void Test_AddStudentsPage_Content()
        {
            AddStudentsPage page = new AddStudentsPage(base.driver);

            page.Open();

            Assert.AreEqual("Add Student", page.GetPageTitle());
            Assert.AreEqual("Register New Student", page.GetPageHeading());
            Assert.AreEqual("", page.FieldStudentName.Text);
            Assert.AreEqual("", page.FieldStudentEmail.Text);
            Assert.AreEqual("Add", page.ButtonAdd.Text);
        }

        [Test]
        public void Test_AddStudentsPage_Links()
        {
            AddStudentsPage page = new AddStudentsPage(base.driver);

            page.Open();
            page.LinkHomePage.Click();
            Assert.AreEqual(new HomePage(base.driver).PageUrl, driver.Url);

            page.Open();
            page.LinkViewStudentsPage.Click();
            Assert.AreEqual(new ViewStudentsPage(base.driver).PageUrl, driver.Url);

            page.Open();
            page.LinkAddStudentPage.Click();
            Assert.AreEqual(page.PageUrl, driver.Url);
        }

        [TestCase("George", "george@gmail.com")]
        [TestCase("Joan", "joan@gmail.com")]
        [TestCase("Phillip", "phillip@gmail.com")]
        [TestCase("Danielle", "danielle@gmail.com")]
        public void Test_AddStudents_ValidInput(string name, string email)
        {
            HomePage homepage = new HomePage(base.driver);
            homepage.Open();
            int studentsCountBefore = homepage.GetStudentsCount();

            AddStudentsPage addStudentsPage = new AddStudentsPage(base.driver);

            addStudentsPage.Open();

            addStudentsPage.AddStudent(name, email);

            ViewStudentsPage viewStudentsPage = new ViewStudentsPage(base.driver);

            Assert.AreEqual(viewStudentsPage.PageUrl, driver.Url);

            string lastStudent = viewStudentsPage.GetRegisteredStudents()[viewStudentsPage.CountRegisteredStudents() - 1];
            string lastStudentName = lastStudent.Substring(0, name.Length);
            string lastStudentEmail = lastStudent.Substring(lastStudent.IndexOf("(") + 1, email.Length);

            Assert.AreEqual(name, lastStudentName);
            Assert.AreEqual(email, lastStudentEmail);

            homepage.Open();
            int studentsCountAfter = homepage.GetStudentsCount();

            Assert.Greater(studentsCountAfter, studentsCountBefore);
        }

        [TestCase("", "")]
        [TestCase("", "joan@gmail.com")]
        [TestCase("Phillip", "")]
        public void Test_AddStudents_InValidInput(string name, string email)
        {
            HomePage homepage = new HomePage(base.driver);
            homepage.Open();
            int studentsCountBefore = homepage.GetStudentsCount();

            AddStudentsPage addStudentsPage = new AddStudentsPage(base.driver);

            addStudentsPage.Open();

            addStudentsPage.AddStudent(name, email);

            Assert.AreEqual(addStudentsPage.PageUrl, driver.Url);

            Assert.AreEqual("Cannot add student. Name and email fields are required!", addStudentsPage.ElementErrorMsg.Text);

            homepage.Open();
            int studentsCountAfter = homepage.GetStudentsCount();

            Assert.AreEqual(studentsCountAfter, studentsCountBefore);
        }
    }
}

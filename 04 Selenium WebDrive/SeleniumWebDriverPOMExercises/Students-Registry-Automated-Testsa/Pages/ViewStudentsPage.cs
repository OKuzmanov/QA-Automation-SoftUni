using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students_Registry_Automated_Testsa.Pages
{
    public class ViewStudentsPage : BasePage
    {

        public ViewStudentsPage(IWebDriver driver) : base(driver)
        {
        }

        public override string PageUrl => "https://mvc-app-node-express.nakov.repl.co/students";

        public ReadOnlyCollection<IWebElement> ListStudents => driver.FindElements(By.CssSelector("body > ul > li"));

        public string[] GetRegisteredStudents()
        {
            return this.ListStudents.Select(s => s.Text.ToString()).ToArray();
        }

        public int CountRegisteredStudents()
        {
            return this.ListStudents.Count();
        }
    }
}

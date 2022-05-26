using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students_Registry_Automated_Testsa.Pages
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver)
        {
        }

        public override string PageUrl => "https://mvc-app-node-express.nakov.repl.co/";

        public IWebElement StudentsCountElement => driver.FindElement(By.CssSelector("body > p > b"));

        public int GetStudentsCount()
        {
            return int.Parse(StudentsCountElement.Text);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;

namespace Automation.ProductModel.Repository
{
    public class LoginRepository
    {
        public static By FirstName = By.Name("firstname");
        public static By Surname = By.Name("lastname");
        public static By MobileNumber = By.Name("reg_email__");
        public static By ReenterEmail = By.Name("reg_email_confirmation__");
        public static By NewPassword = By.Name("reg_passwd__");
        public static By Date = By.Id("day");
        public static By Month = By.Id("month");
        public static By Year = By.Id("year");
        public static By Gender = By.Name("sex");
        public static By SignUp = By.Name("websubmit");

        public static By UserID = By.Id("email");
        public static By Password = By.Id("pass");
        public static By LoginButton = By.Id("loginbutton");

    }
}

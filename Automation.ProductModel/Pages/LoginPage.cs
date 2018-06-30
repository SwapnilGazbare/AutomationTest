using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using HelperLibrary;
using Automation.ProductModel.Repository;

namespace Automation.ProductModel.Pages
{
    public class LoginPage
    {

        #region Properties
        private IWebElement FirstName
        {
            get
            {
                Utils.wait.ElementIsVisible(LoginRepository.FirstName);
                return Utils.driver.FindElement(LoginRepository.FirstName);
            }
        }

        private IWebElement Surname
        {
            get
            {
                Utils.wait.ElementIsVisible(LoginRepository.Surname);
                return Utils.driver.FindElement(LoginRepository.Surname);
            }
        }

        private IWebElement MobileNumberOREmail
        {
            get
            {
                Utils.wait.ElementIsVisible(LoginRepository.MobileNumber);
                return Utils.driver.FindElement(LoginRepository.MobileNumber);
            }
        }

        private IWebElement ReenterEmail
        {
            get
            {
                Utils.wait.ElementIsVisible(LoginRepository.ReenterEmail);
                return Utils.driver.FindElement(LoginRepository.ReenterEmail);
            }
        }

        private IWebElement NewPassword
        {
            get
            {
                Utils.wait.ElementIsVisible(LoginRepository.NewPassword);
                return Utils.driver.FindElement(LoginRepository.NewPassword);
            }
        }

        private IWebElement Day
        {
            get
            {
                Utils.wait.ElementIsVisible(LoginRepository.Date);
                return Utils.driver.FindElement(LoginRepository.Date);
            }
        }

        private IWebElement Month
        {
            get
            {
                Utils.wait.ElementIsVisible(LoginRepository.Month);
                return Utils.driver.FindElement(LoginRepository.Month);
            }
        }

        private IWebElement Year
        {
            get
            {
                Utils.wait.ElementIsVisible(LoginRepository.Year);
                return Utils.driver.FindElement(LoginRepository.Year);
            }
        }

        private IList<IWebElement> Gender
        {
            get
            {
                Utils.wait.ElementIsVisible(LoginRepository.Gender);
                return Utils.driver.FindElements(LoginRepository.Gender);
            }
        }

        private IWebElement Signup
        {
            get
            {
                Utils.wait.ElementIsVisible(LoginRepository.SignUp);
                return Utils.driver.FindElement(LoginRepository.SignUp);
            }
        }       

        private IWebElement UserID
        {
            get
            {
                Utils.wait.ElementIsVisible(LoginRepository.UserID);
                return Utils.driver.FindElement(LoginRepository.UserID);
            }
        }

        private IWebElement Password
        {
            get
            {
                Utils.wait.ElementIsVisible(LoginRepository.Password);
                return Utils.driver.FindElement(LoginRepository.Password);
            }
        }

        private IWebElement SubmitButton
        {
            get
            {
                Utils.wait.ElementIsVisible(LoginRepository.LoginButton);
                return Utils.driver.FindElement(LoginRepository.LoginButton);
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Desc : Create New Account
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="surName"></param>
        /// <param name="email"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="gender"></param>
        public void CreateNewAccount(string firstName, string surName, string newPassword, string email, string gender, string day = "", string month = "", string year = "")
        {
            try
            {
                FirstName.SetText(firstName);
                Surname.SetText(surName);
                MobileNumberOREmail.SetText(email);
                ReenterEmail.SetText(email);
                NewPassword.SetText(newPassword);
                Utils.SelectRadioButton(gender, Gender);
                Signup.Click();
            }
            catch (Exception)
            {
            }
  
        }

        /// <summary>
        /// Login to application
        /// </summary>
        /// <param name="userName">Login user name</param>
        /// <param name="password">Login password</param>
        /// <returns></returns>
        public HomePage Login(string userName, string password)
        {
            UserID.Clear();
            UserID.SendKeys(userName);

            Password.Clear();
            Password.SendKeys(password);

            SubmitButton.Click();

            return new HomePage();
        }

        #endregion
    }
}

/* ***************************************************************
* CCH Integrator Automation Framework.
* © 2017, CCH Incorporated.  All rights reserved.
* Author: Gaurav Goyal
* Date: 3 August, 2017
*****************************************************************/
using GI.Automation.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;

namespace CCH.Automation.LP.ProductModel
{
    public class AddNewUserPage : BaseComponent
    {
        public AddNewUserPage()
        {
            PageFactory.InitElements(this, new RetryingElementLocator(SeleniumDriver.driver, TimeSpan.FromSeconds(Convert.ToInt32(Config.WaitTime))));
        }

        #region Properties
#pragma warning disable 0649
        [FindsBy(How = How.Id, Using = "FirstName")]
        protected IWebElement firstName;
        [FindsBy(How = How.Id, Using = "LastName")]
        protected IWebElement lastName;
        [FindsBy(How = How.Id, Using = "LogonID")]
        protected IWebElement userID;
        [FindsBy(How = How.Id, Using = "Email")]
        protected IWebElement emailAdress;
        [FindsBy(How = How.Id, Using = "CountryCode")]
        protected IWebElement countryCode;
        [FindsBy(How = How.Id, Using = "MobileNumber")]
        protected IWebElement mobileNumber;
        [FindsBy(How = How.Id, Using = "Active")]
        public IWebElement activeCheckbox;
        [FindsBy(How = How.Id, Using = "AllAccess")]
        private IWebElement accessAllLodements;
        [FindsBy(How = How.Id, Using = "Password")]
        private IWebElement password;
        [FindsBy(How = How.Id, Using = "ConfirmPassword")]
        private IWebElement confirmPassword;
        [FindsBy(How = How.Id, Using = "btnSave")]
        private IWebElement saveUser;

        public string FirstName
        {
            set { firstName.SetText(value); }
            get { return firstName.GetValue(); }
        }

        public string LastName
        {
            set { lastName.SetText(value); }
            get { return lastName.GetValue(); }

        }

        public string UserID
        {
            set { userID.SetText(value); }
            get { return userID.GetValue(); }
        }

        public string EmailAddress
        {
            set { emailAdress.SetText(value); }
            get { return emailAdress.GetValue(); }
        }

        public string CountryCode
        {
            set { HelperLibrary.SelectItem(countryCode, value); }
            get { return countryCode.GetValue(); }
        }

        public string MobileNumber
        {
            set { mobileNumber.SetText(value); }
            get { return mobileNumber.GetValue(); }
        }

        public bool ActiveCheckbox
        {
            set
            {
                activeCheckbox.DoClick();
            }
        }

        public bool AccessAll
        {
            set
            {
                accessAllLodements.DoClick();
            }
        }

        public string Password
        {
            set
            {
                password.SetText(TestExecutionDetails.StringCipher.Decrypt(value, ""));
            }

            get { return password.GetValue(); }
        }

        public string ConfirmPassword
        {
            set { confirmPassword.SetText(TestExecutionDetails.StringCipher.Decrypt(value, "")); }
            get { return confirmPassword.GetValue(); }
        }

        #endregion

        #region Actions
        public bool SaveUserDetails()
        {
            HelperLibrary.wait.UntilEnabled(saveUser);
            saveUser.Click();

            bool flag = ClickOnConfirmUserUpdate();
            return flag;
        }

        #endregion
    }
}

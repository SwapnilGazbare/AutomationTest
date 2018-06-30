/* ***************************************************************
* CCH Integrator Automation Framework.
* © 2017, CCH Incorporated.  All rights reserved.
* Author: Gaurav Goyal
* Date: 4 August, 2017
*****************************************************************/

using GI.Automation.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Threading;
using System.Collections.Generic;
using CCH.Automation.LP.ProductModel.Pages;

namespace CCH.Automation.LP.ProductModel
{
    public class UsersPage
    {
        public UsersPage()
        {
            PageFactory.InitElements(this, new RetryingElementLocator(SeleniumDriver.driver, TimeSpan.FromSeconds(Convert.ToInt32(Config.WaitTime))));
        }
#pragma warning disable 0649
        #region Properties
        [FindsBy(How = How.XPath, Using = "//a[contains(text(),'Add New User')]")]
        private IWebElement addNewUser;
        [FindsBy(How = How.Id, Using = "UserName")]
        private IWebElement userName;
        [FindsBy(How = How.Id, Using = "UserId")]
        private IWebElement userID;
        [FindsBy(How = How.Id, Using = "btnSearchUser")]
        public IWebElement searchUser;
        [FindsBy(How = How.XPath, Using = "//div[text()='No records found.']")]
        public IList<IWebElement> noRecordFound;

        #endregion

        #region Actions
        public AddNewUserPage AddNewUser()
        {
            LodgementPortalApp.log.LogInfo("Click on Add New User Link");
            addNewUser.DoClick();
            HelperLibrary.wait.UntilDocumentReady();
            return new AddNewUserPage();
        }

        public void SearchByUserName(string UserName)
        {
            LodgementPortalApp.log.LogInfo("Enter UserName");
            userName.SetText(UserName);
            LodgementPortalApp.log.LogInfo("Click on Search Button");
            searchUser.Click();
            Thread.Sleep(2000);
        }
        public bool SearchByUserID(string UserID)
        {
            LodgementPortalApp.log.LogInfo("Enter UserID");
            userID.SetText(UserID);
            LodgementPortalApp.log.LogInfo("Click on search button");
            searchUser.DoClick();
            Thread.Sleep(2000);
            if (noRecordFound != null)
                LodgementPortalApp.log.LogInfo(true, "User found" + userID);
            return true;
        }

        public UpdateUserPage ClickOnSearchedUser(string userID)
        {
            SeleniumDriver.driver.FindElement(By.LinkText(userID)).Click();
            return new UpdateUserPage();
        }

        #endregion
    }

    public class UpdateUserPage : AddNewUserPage
    {
        public UpdateUserPage()
        {
            PageFactory.InitElements(this, new RetryingElementLocator(SeleniumDriver.driver, TimeSpan.FromSeconds(Convert.ToInt32(Config.WaitTime))));
        }

        #region Properties
#pragma warning disable 0649
        [FindsBy(How = How.XPath, Using = "//a[contains(text(),'Data Access')]")]
        private IWebElement dataAccess;

        [FindsBy(How = How.Id, Using = "btnUpdate")]
        private IWebElement updateUserButton;

        [FindsBy(How = How.Id, Using = "btnDelete")]
        private IWebElement deleteUserButton;

        #endregion

        #region Actions

        public DataAccessPage ClickDataAccess()
        {
            LodgementPortalApp.log.LogInfo("Click on Data Access Page");
            dataAccess.Click();
            return new DataAccessPage();
        }

        public bool ValidateUserDetails(UpdateUserPage update)
        {
            bool flag = false;

            if (update.FirstName == firstName.GetValue())
            {
                flag = true;
                HelperLibrary.log.LogInfo("User First Name matched. Expected : " + firstName.GetValue());
            }

            if (update.LastName == lastName.GetValue())
            {
                flag &= true;
                HelperLibrary.log.LogInfo("User Last Name not matched. Expected : " + lastName.Text);
            }


            if (update.EmailAddress == emailAdress.GetValue())
            {
                flag &= true;
                HelperLibrary.log.LogInfo("User mail id not matched. Expected : " + emailAdress.Text);
            }

            if (flag)
            {
                HelperLibrary.log.LogInfo(true, "User information updated successfully.");
                flag = true;
            }
            else
                HelperLibrary.log.LogInfo(false, "User information not updated successfully.");

            return flag;
        }

        #endregion

        public bool ModifyUserDetails()
        {
            LodgementPortalApp.log.LogInfo("Click on update User button");
            updateUserButton.Click();
            bool flag = ClickOnConfirmUserUpdate();
            return flag;
        }

        /// <summary>
        /// Desc : Delete user 
        /// </summary>
        /// <returns></returns>
        public bool DeleteUser(string UserID)
        {
            bool flag = false;
            LodgementPortalApp.log.LogInfo("Click on delete User button");
            deleteUserButton.Click();
            if (ClickOnConfirmDeleteUser(LodgementMessages.DeleteUserMessage))
            {
                LodgementPortalApp.log.LogInfo("Enter UserID");
                SeleniumDriver.driver.FindElement(ObjectRepository.userID).SetText(UserID);
                LodgementPortalApp.log.LogInfo("Click on search button");
                SeleniumDriver.driver.FindElement(ObjectRepository.searchButton).DoClick();
                Thread.Sleep(2000);
                if (SeleniumDriver.driver.FindElements(ObjectRepository.recordsList).Count ==1)
                {
                    LodgementPortalApp.log.LogTest("User deleted successfully -" + UserID);
                    flag = true;
                }
                else
                    LodgementPortalApp.log.LogResult(false, "User Not deleted -" + UserID);
            }
            return flag;

        }

        public void UpdateUserButton()
        {
            updateUserButton.DoClick();
        }

        

    }

}

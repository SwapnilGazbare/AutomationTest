/* ***************************************************************
* CCH Integrator Automation Framework.
* © 2017, CCH Incorporated.  All rights reserved.
* Author: Gaurav Goyal
* Date: 3 August, 2017
*****************************************************************/
using GI.Automation.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace CCH.Automation.LP.ProductModel
{
    public class FunctionAccessPage : BaseComponent
    {
        public FunctionAccessPage()
        {
            PageFactory.InitElements(this, new RetryingElementLocator(SeleniumDriver.driver, TimeSpan.FromSeconds(Convert.ToInt32(Config.WaitTime))));
        }


        #region Properties
#pragma warning disable 0649
        [FindsBy(How = How.Id, Using = "UserName")]
        private IWebElement userName;
        [FindsBy(How = How.Id, Using = "UserId")]
        private IWebElement userID;
        [FindsBy(How = How.Id, Using = "btnSearchUser")]
        private IWebElement searchUser;
        [FindsBy(How = How.Id, Using = "btnSave")]
        private IWebElement saveFunctionAccess;
        [FindsBy(How = How.Id, Using = "accessType_1")]
        private IWebElement selectAllAdministration;
        [FindsBy(How = How.Id, Using = "accessType_2")]
        private IWebElement selectAllUserSetting;
        [FindsBy(How = How.Id, Using = "accessType_3")]
        private IWebElement selectAllLodgement;
        [FindsBy(How = How.Id, Using = "1_accessItem_9")]
        private IWebElement systemOptionCheckbox;
        [FindsBy(How = How.XPath, Using = "//div[text()='No records found.']")]
        private IList<IWebElement> noRecordFound;

        #endregion

        #region Actions

        public void SearchByUserName(string UserName)
        {
            LodgementPortalApp.log.LogInfo("Enter UserName");
            userName.SetText(UserName);
            LodgementPortalApp.log.LogInfo("Click on search button");
            searchUser.Click();
        }

        public bool SearchByUserId(string UserID)
        {
            LodgementPortalApp.log.LogInfo("Enter UserID");
            userID.SetText(UserID);
            LodgementPortalApp.log.LogInfo("Click on search button");
            searchUser.Click();
            System.Threading.Thread.Sleep(2000);
            if (noRecordFound != null)
            {
                LodgementPortalApp.log.LogInfo(true, "User found" + userID);
                return true;
            }
            else
            {
                LodgementPortalApp.log.LogResult(false, "User not found" + userID);
                return false;
            }
        }

        public void ClickOnSearchedUser(string userID)
        {
            SeleniumDriver.driver.FindElement(By.LinkText(userID)).DoClick();
            System.Threading.Thread.Sleep(2000);
        }

        public void GiveAdminFunctionAccess()
        {
            var isAdminSectionExpanded = SeleniumDriver.driver.FindElement(By.Id("accessType_1")).FindElement(By.XPath("..")).FindElement(By.TagName("i"));

            if (isAdminSectionExpanded.GetAttribute("class").Contains("plus"))
            {
                isAdminSectionExpanded.DoClick();

                //Click on Admin checkbox only if it is unchecked
                if (selectAllAdministration.GetAttribute("checked") == null)
                    Retry.Do(selectAllAdministration.Click);
                System.Threading.Thread.Sleep(2000);

                //Click on Admin checkbox only if it is checked
                if (systemOptionCheckbox.GetAttribute("checked") != null)
                    Retry.Do(systemOptionCheckbox.Click);
            }

            //Click on User Settings checkbox only if it is unchecked
            if (selectAllUserSetting.GetAttribute("checked") == null)
            {
                Retry.Do(selectAllUserSetting.Click);
            }

            //Click on Lodgement checkbox only if it is checked
            if (selectAllLodgement.GetAttribute("checked") != null)
            {
                Retry.Do(selectAllLodgement.Click);
            }
            SaveFunctionAccess();
        }

        public void GiveTaxAgentFunctionAccess()
        {
            var isUserSettingsSectionExpanded = SeleniumDriver.driver.FindElement(By.Id("accessType_2")).FindElement(By.XPath("..")).FindElement(By.TagName("i"));
            var isLodgementExpanded = SeleniumDriver.driver.FindElement(By.Id("accessType_2")).FindElement(By.XPath("..")).FindElement(By.TagName("i"));

            //Click on Admin checkbox only if it is checked
            if (selectAllAdministration.GetAttribute("checked") != null)
            {
                Retry.Do(selectAllAdministration.Click);
            }

            //Click on User Settings checkbox only if it is unchecked
            if (selectAllUserSetting.GetAttribute("checked") == null)
            {
                Retry.Do(selectAllUserSetting.Click);
            }

            //Click on Lodgement checkbox only if it is unchecked
            if (selectAllLodgement.GetAttribute("checked") == null)
            {
                Retry.Do(selectAllLodgement.Click);
            }
            SaveFunctionAccess();
        }

        public void SaveFunctionAccess()
        {
            saveFunctionAccess.Click();
            HelperLibrary.wait.Until(ExpectedConditions.ElementToBeClickable(OkDialog));
            OkDialog.Click();
            System.Threading.Thread.Sleep(2000);
        }

        #endregion
    }

}

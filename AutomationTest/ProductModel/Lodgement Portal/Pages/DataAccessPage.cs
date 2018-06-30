/* ***************************************************************
* CCH Integrator Automation Framework.
* © 2017, CCH Incorporated.  All rights reserved.
* Author: Mayur Pawar
* Date: 10 August, 2017
* Desc : File contains fields related to Data Access Page
*****************************************************************/

using GI.Automation.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;


namespace CCH.Automation.LP.ProductModel
{
    public class DataAccessPage : BaseComponent
    {
        public DataAccessPage()
        {
            PageFactory.InitElements(this, new RetryingElementLocator(SeleniumDriver.driver, TimeSpan.FromSeconds(Convert.ToInt32(Config.WaitTime))));
        }

        #region properties
        [FindsBy(How = How.XPath, Using = "//a[contains(text(),'New Data Access')]")]
        public IWebElement btnNewDataAccess;

        [FindsBy(How = How.XPath, Using = "//a[contains(text(),'Cancel')]")]
        public IWebElement btnCancel;

        [FindsBy(How = How.XPath, Using = "//form[@action = '/Account/UserAccess']/table")]
        public IWebElement tblDataAccessRecords;


        #endregion properties


        #region Actions
        public ManageDataAccess ClickNewDataAccess()
        {
            LodgementPortalApp.log.LogInfo("Click on New Data Access");
            btnNewDataAccess.Click();
            return new ManageDataAccess();
        }

        public void ClickCancel()
        {
            btnCancel.Click();
        }

        #endregion
    }
}

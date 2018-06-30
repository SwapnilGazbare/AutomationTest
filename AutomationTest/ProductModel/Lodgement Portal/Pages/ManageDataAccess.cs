/* ***************************************************************
* CCH Integrator Automation Framework.
* © 2017, CCH Incorporated.  All rights reserved.
* Author: Mayur Pawar
* Date: 10 August, 2017
* Desc : File contains fields related to Manage Data Access Page
*****************************************************************/

using GI.Automation.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;

namespace CCH.Automation.LP.ProductModel
{
    public class ManageDataAccess : BaseComponent
    {
        public ManageDataAccess()
        {
            PageFactory.InitElements(this, new RetryingElementLocator(SeleniumDriver.driver, TimeSpan.FromSeconds(Convert.ToInt32(Config.WaitTime))));
        }

        #region Properties

        [FindsBy(How = How.XPath, Using = "//input[@id = 'SourceName']")]
        public IWebElement txtSourceName;

        [FindsBy(How = How.XPath, Using = "//input[@id = 'RequestOwnerIdentifier']")]
        public IWebElement txtRequestOwnerIdentifier;

        [FindsBy(How = How.XPath, Using = "//select[contains(@id,'RequestOwnerIdentifierType')]")]
        public IWebElement drpdwnRequestOwnerIdentifierType;

        [FindsBy(How = How.XPath, Using = "//button[@id = 'btnSaveDataAccess']")]
        public IWebElement btnSaveDataAccess;

        [FindsBy(How = How.XPath, Using = "//a[contains(text(),'Cancel')")]
        public IWebElement btnCancel;


        public string RequestSourceName
        {
            set { txtSourceName.SetText(value); }
            get { return txtSourceName.GetValue(); }
        }


        public string RequestOwnerIdentifier
        {
            set { txtRequestOwnerIdentifier.SetText(value); }
            get { return txtRequestOwnerIdentifier.GetValue(); }
        }

        public string RequestOwnerIdentifierType
        {
            set { HelperLibrary.SelectItem(drpdwnRequestOwnerIdentifierType, value); }
            get { return drpdwnRequestOwnerIdentifierType.GetValue(); }
        }

        public bool Cancel
        {
            set { btnCancel.Click(); }
        }


        #endregion Properties

        #region Method
        public void SaveDataAccess()
        {
            LodgementPortalApp.log.LogInfo("Click on Save Manage Data Access");
            btnSaveDataAccess.Click();
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// MayurP on 10/08/2017
        /// Function to validate Data Access saved record from Manage Data Access
        /// </summary>
        /// <param name="sourceName"> string Source Name </param>
        /// <param name="objDataAccessPage"> Data Access Page Object</param>
        /// <param name="objManageDataAccess">Manage Data Access Page Object</param>
        /// <returns> true if Data is present in Data Access Table ,false if Data is not present in the same</returns>
        public bool ValidateSavedDataAccess(string sourceName, LP.ProductModel.DataAccessPage objDataAccessPage, LP.ProductModel.ManageDataAccess objManageDataAccess)
        {
            try
            {
                bool resultFlag = false;

                int colIndex = WebTableHelper.GetColumnIndex(objDataAccessPage.tblDataAccessRecords, "Source Name");
                int rowIndex = WebTableHelper.GetRowIndexFromTable(WebTableHelper.GetAllRowsFromTable(objDataAccessPage.tblDataAccessRecords), sourceName, colIndex);

                if (colIndex != -1 && rowIndex != -1)
                {
                    string cellValue = WebTableHelper.GetCellFromTable(objDataAccessPage.tblDataAccessRecords, rowIndex - 1, colIndex);
                    if (cellValue == sourceName)
                    {
                        resultFlag = true;
                    }
                }
                else
                {
                    resultFlag = false;
                }
                return resultFlag;
            }
            catch (Exception ex)
            {
                LodgementPortalApp.log.LogInfo(false, ex.Message);
                return false;
            }
        }
        /// <summary>
        /// MayurP on 10/08/2017
        /// Enter Details for Manage Data Access
        /// </summary>
        /// <param name="sourceName"> Name of link to be enter as a string</param>
        /// <param name="objManageDataAccess">ManageDataAccess objec </param>
        /// <returns>Name of link to be enter as a string </returns>
        public string EnterManageDataAccessDetails(ManageDataAccess objManageDataAccess)
        {
            string configUrl = Config.LPApplicationURL;

            //MayurP on 02022018 Shrinking the url upto the 50 characters only
            if (configUrl.Length > 50)
            {
                configUrl= configUrl.Mid(0, 49);
            }

            string sourceName = objManageDataAccess.RequestSourceName = configUrl;//;Config.LPApplicationURL;
            objManageDataAccess.RequestOwnerIdentifier = "44565554";
            objManageDataAccess.RequestOwnerIdentifierType = "TFN";
            objManageDataAccess.SaveDataAccess();
            return sourceName;
        }
        #endregion Method


    }

}

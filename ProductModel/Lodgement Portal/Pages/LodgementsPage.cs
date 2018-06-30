using GI.Automation.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using GI.Automation.Entity;
using System.Collections;

namespace CCH.Automation.LP.ProductModel
{
    public class LodgementsPage : BaseComponent
    {
        public LodgementsPage()
        {
            PageFactory.InitElements(SeleniumDriver.driver, this);
        }

        #region Properties
#pragma warning disable 0649
        [FindsBy(How = How.Id, Using = "btnDeleteLodgements")]
        [CacheLookup]
        public IWebElement DeleteButton;

        [FindsBy(How = How.Id, Using = "btnUnDeleteLodgements")]
        [CacheLookup]
        public IWebElement UndeleteButton;

        [FindsBy(How = How.ClassName, Using = "deletedItems")]
        public IList<IWebElement> DeleteIcon;

        [FindsBy(How = How.XPath, Using = "//input[contains(@class,'requestCheckbox')]")]
        [CacheLookup]
        public IWebElement FirstRecordCheckbox;

        [FindsBy(How = How.Id, Using = "btnLodge")]
        [CacheLookup]
        public IWebElement LodgeButton;

        [FindsBy(How = How.Id, Using = "btnPreLodge")]
        [CacheLookup]
        public IWebElement PreLodgeButton;

        [FindsBy(How = How.XPath, Using = "//a[contains(@href,'/ExportToExcel')]")]
        [CacheLookup]
        public IWebElement ExcelReportButton;

        #endregion

        #region Actions

        public void DeleteFirstRecord()
        {
            // select first record checkbox
            FirstRecordCheckbox.Click();

            // Clcik on delete button
            DeleteButton.Click();

            // Click on OK button
            System.Threading.Thread.Sleep(3000);
            confirmDialog.Click();
        }

        public void UndeleteFirstRecord()
        {
            // select first record checkbox
            FirstRecordCheckbox.Click();

            // Clcik on delete button
            UndeleteButton.Click();
        }

        /// <summary>
        /// Author : Kavita Nunse <11-Aug-2017>
        /// Desc : Checks whether button is visible or not depending on flag passed through the script
        /// </summary>
        public bool IsLodgeAndPreLodgeButtonVisible(bool isDisplayed)
        {
            try
            {
                if (isDisplayed) // Case when lodge button should be displayed
                {
                    if (LodgeButton.Displayed && PreLodgeButton.Displayed)
                        isDisplayed = true; 
                }
                else // Case when lodge button should not be displayed
                {
                    if (LodgeButton.Displayed && PreLodgeButton.Displayed) //Button should not be visible
                        isDisplayed = false;
                }
                return isDisplayed;
            }
            catch(Exception ex)
            {
                LodgementPortalApp.log.LogException(ex, ex.StackTrace);
                return !isDisplayed; 
            }
        }

        /// <summary>
        /// Function to Export LP record to excel
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <returns>Complete File Path</returns>
        public string ExportToExcel(string fileName)
        {
            ExcelReportButton.DoClick();
            HelperLibrary.wait.UntilDocumentReady();
            System.Threading.Thread.Sleep(5000);
            string filePath = HelperLibrary.SaveDownloadedFile(fileName);
            return filePath;
        }

        public bool VerifyColumnHeader(List<string> expectedColHeader, List<string> actualColHeader)
        {
            bool isVerify = true;
            if (expectedColHeader.Count == actualColHeader.Count)
            {
                foreach (string colHeader in expectedColHeader)
                {
                    if (actualColHeader.Contains(colHeader))
                    {
                        LodgementPortalApp.log.LogInfo(true, "Verified column Header: " + colHeader);
                    }
                    else
                    {
                        LodgementPortalApp.log.LogInfo(false, "Failed to verify column Header: " + colHeader);
                        isVerify = false;
                    }
                }
            }
            else
            {
                LodgementPortalApp.log.LogInfo(false, "Mismatch in column header count");
                isVerify = false;
            }

            return isVerify;
        }

        /// <summary>
        /// Desc : Get a pericular joib details feild from excel
        /// </summary>
        /// <param name="filePath">string : File path</param>
        /// <param name="jobID">string : Job id</param>
        /// <param name="entityName">string : Entity Name</param>
        /// <returns></returns>
        public List<string> GetJobDetailsFromExcel(string filePath, string jobID, string entityName)
        {
            List<string> rowData = new List<string>();
            //rowData = ExcelFunctions.GetRowData(filePath, "Select * from [Sheet1$] where [Job ID]=" + jobID + "and [Entity Name]='" + entityName + "'");
            List<List<string>> rowsData = ExcelFunctions.GetRowData(filePath, "Select * from [Sheet1$] where [Job ID]=" + jobID + "and [Entity Name]='" + entityName + "'");

            rowData=rowsData[0];
            return rowData;

        }

        #endregion
    }
}

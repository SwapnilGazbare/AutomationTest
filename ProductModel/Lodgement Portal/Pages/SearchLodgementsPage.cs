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
using System.Linq;

namespace CCH.Automation.LP.ProductModel
{
    public class SearchLodgementsPage
    {
        public SearchLodgementsPage()
        {
            PageFactory.InitElements(SeleniumDriver.driver, this);
        }

        #region Properties
#pragma warning disable 0649
        [FindsBy(How = How.Id, Using = "Entity")]
        public IWebElement entityName;

        [FindsBy(How = How.Id, Using = "txtFiledDate")]
        public IWebElement submissionDate;

        [FindsBy(How = How.Id, Using = "txtEntityTFN")]
        public IWebElement entityTFN;

        [FindsBy(How = How.Id, Using = "txtLodgementTimestamp")]
        public IWebElement lodgementTimestamp;

        [FindsBy(How = How.Id, Using = "txtWorkBookName")]
        public IWebElement workbookName;

        [FindsBy(How = How.Id, Using = "txtTaxAgentId")]
        public IWebElement taxAgentNumber;

        [FindsBy(How = How.Id, Using = "txtPeriodEndDate")]
        public IWebElement periodEndDate;

        [FindsBy(How = How.Id, Using = "StatusId")]
        public IWebElement status;

        [FindsBy(How = How.Id, Using = "txtJobId")]
        public IWebElement jobIdentifier;

        [FindsBy(How = How.Id, Using = "chkShowDeleted")]
        public IWebElement showDeleted;

        [FindsBy(How = How.CssSelector, Using = ".table.table-striped.table-hover")]
        public IWebElement searchGrid;

        [FindsBy(How = How.XPath, Using = "//table[@class='table table-striped table-hover']//tbody//tr")]
        public IList<IWebElement> searchGridRecords;

        [FindsBy(How = How.Id, Using = "btnSearch")]
        public IWebElement SearchButton;

        [FindsBy(How = How.XPath, Using = "//div[contains(text(),'No records found')]")]
        public IWebElement NoRecordFound;

        [FindsBy(How = How.CssSelector, Using = ".glyphicon.glyphicon-search")]
        public IWebElement SearchMagnifyingIcon;

        #endregion

        #region Actions

        bool ZeroRecordExpected = false;
        bool RecordExpected = false;

        public string EntityName
        {
            set { entityName.SetText(value); }
            get { return entityName.GetValue(); }
        }
        public string EntityTFN
        {
            set { entityTFN.SetText(value); }
            get { return entityTFN.GetValue(); }
        }
        public string SubmissionDate
        {
            set { submissionDate.SetText(value); }
            get { return submissionDate.GetValue(); }
        }
        public string LodgementTimestamp
        {
            set { lodgementTimestamp.SetText(value); }
            get { return lodgementTimestamp.GetValue(); }
        }
        public string WorkbookName
        {
            set { workbookName.SetText(value); }
            get { return workbookName.GetValue(); }
        }
        public string TaxAgentNumber
        {
            set { taxAgentNumber.SetText(value); }
            get { return taxAgentNumber.GetValue(); }
        }
        public string PeriodEndDate
        {
            set { periodEndDate.SetText(value); }
            get { return periodEndDate.GetValue(); }
        }
        public string Status
        {
            set { HelperLibrary.SelectItem(status, value); }
            get { return status.GetValue(); }
        }
        public string JobIdentifier
        {
            set { jobIdentifier.SetText(value); }
            get { return jobIdentifier.GetValue(); }
        }
        public bool ShowDeleted
        {
            set { showDeleted.DoClick(); }
            get { return ShowDeleted; }
        }
        public bool isZeroRecordExpected
        {
            set { ZeroRecordExpected = isZeroRecordExpected; }
            get { return ZeroRecordExpected; }
        }
        public bool isRecordExpected
        {
            set { RecordExpected = isRecordExpected; }
            get { return RecordExpected; }
        }

        /// <summary>
        /// Author : Kavita Nunse
        /// Date : 10-Aug-2017
        /// Desc : Clicks on Search's Magnifying icon if not expanded
        /// </summary>
        public void ClickSearchMagnifyingIcon()
        {
            // click on search magnifying button
            LodgementPortalApp.log.LogInfo("Click on Search Magnifying Icon");
            if (SearchMagnifyingIcon.FindElement(By.XPath("..")).GetAttribute("class").Contains("collapsed"))
                Retry.Do(SearchMagnifyingIcon.Click);
        }

        /// <summary>
        /// Author : Kavita Nunse
        /// Date : 10-Aug-2017
        /// Desc : Clicks on Search button and verify recorded is displayed or not
        /// </summary>
        public bool ClickSearch(bool isRecordExpected)
        {
            bool flag = true;
            LodgementPortalApp.log.LogInfo("Click on Search Button");
            SearchButton.Click();
            System.Threading.Thread.Sleep(2000);
            // Is zero record expected
            if (!isRecordExpected)
            {
                try
                {
                    if (NoRecordFound.Displayed)
                    {
                        HelperLibrary.log.LogInfo(true, "Successfully verified that no record is been displayed");
                        flag = true;
                    }
                }
                catch
                {
                    HelperLibrary.log.LogInfo(false, "Failed to verify that no record is been displayed");
                    flag = false;
                }
            }

            // Is record expected
            if (isRecordExpected)
            {
                try
                {
                    if (searchGrid.Displayed)
                    {
                        int count = searchGridRecords.Count;
                        if (count > 0)
                        {
                            HelperLibrary.log.LogInfo(true, "Successfully verified that " + count + " record has been searched");
                            flag = true;
                        }
                        else
                        {
                            HelperLibrary.log.LogInfo(false, "Failed to verify that record has been searched");
                            flag = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    HelperLibrary.log.LogInfo(false, "Failed to verify that record has been searched");
                    HelperLibrary.log.LogException(ex, ex.StackTrace);
                    flag = false;
                }
            }
            return flag;
        }

        /// <summary>
        /// Kavita Nunse <3-Aug-2017>
        /// Desc: Search Single values
        /// </summary>
        /// <param name="searchFields"></param>
        /// <returns></returns>
        public bool searchLodgements(Dictionary<IWebElement, string> searchFields, Dictionary<IWebElement, string> searchColumn)
        {
            bool flag = true;
            bool findSubmissionDate = false;
            int columnIndex = 0;
            int submissionDateColumnIndex = 0;
            string columnValue = null;
            string submissionDateColumnValue = null;
            var searchCriteria = searchFields.ToList();
            var column = searchColumn.ToList();

            try
            {
                for (int i = 0, j = 0; i < searchCriteria.Count && j < column.Count; i++, j++) //Iterate 2 dictionaries together
                {
                    // Click on Search Magnifying Icon present on My Lodgement tab
                    LodgementPortalApp.log.LogInfo("Click on Search Magnifying Icon");

                    if (SearchMagnifyingIcon.FindElement(By.XPath("..")).GetAttribute("class").Contains("collapsed"))
                        SearchMagnifyingIcon.Click();
                    LodgementPortalApp.log.LogInfo("Search Criteria is : "+ column[j].Value + " AND value :" +searchCriteria[i].Value);

                    // Select Status from dropdown
                    if (searchCriteria[i].Key.GetAttribute("name").Equals("StatusId"))
                    {
                        HelperLibrary.SelectItem(searchCriteria[i].Key, searchCriteria[i].Value);
                        findSubmissionDate = true;
                    }
                    // Click on show deleted checkbox
                    else if (searchCriteria[i].Key.GetAttribute("type").Equals("checkbox"))
                        searchCriteria[i].Key.Click();

                    else
                    {
                        // Enter Value to be searched
                        searchCriteria[i].Key.Clear();
                        searchCriteria[i].Key.SendKeys(searchCriteria[i].Value);
                        //MayurP added following line for period end date 
                        searchCriteria[i].Key.SendKeys(Keys.Tab);
                    }

                    // Click on search button
                    SearchButton.Click();

                    // Get Column data
                    if (!findSubmissionDate)
                    {
                        columnIndex = WebTableHelper.GetColumnIndex(searchGrid, column[j].Value);
                        columnValue = WebTableHelper.GetCellFromTable(searchGrid, 0, columnIndex);
                    }
                    else // code to search and save submission date, required for searching submission date
                    {
                        columnIndex = WebTableHelper.GetColumnIndex(searchGrid, column[j].Value);
                        columnValue = WebTableHelper.GetCellFromTable(searchGrid, 0, columnIndex);
                        submissionDateColumnIndex = WebTableHelper.GetColumnIndex(searchGrid, LodgementPortalConstants.SUBMISSION_DATE);
                        submissionDateColumnValue = WebTableHelper.GetCellFromTable(searchGrid, 0, submissionDateColumnIndex);
                        if (submissionDateColumnValue != null)
                        {
                            string[] split = submissionDateColumnValue.Split(' ');
                            HelperLibrary.RunTimeXMl.WriteNode("submissionDate", split[0]);
                        }
                    }
                    if (columnValue.Contains(searchCriteria[i].Value))
                        HelperLibrary.log.LogInfo(true, string.Format("Successfully Searched Record {0} with value {1} in My Lodgement tab", column[j].Value, searchCriteria[i].Value));
                    else if (NoRecordFound.Displayed)
                    {
                        HelperLibrary.log.LogInfo(false, string.Format("Failed to Search Record {0} with value {1} in My Lodgement tab", column[j].Value, searchCriteria[i].Value));
                        flag = false;
                    }

                    // Click on Search Magnifying Icon present on My Lodgement tab
                    SeleniumDriver.driver.FindElement(By.CssSelector(".glyphicon.glyphicon-search"));
                    LodgementPortalApp.log.LogInfo("Click on Search Magnifying Icon");
                    Retry.Do(SearchMagnifyingIcon.Click);
                    if (!searchCriteria[i].Key.GetAttribute("name").Equals("StatusId"))
                        searchCriteria[i].Key.Clear(); //As we do not have clear button, so we will clear field as soon as verification is done
                }
                return flag;
            }
            catch (Exception ex)
            {
                HelperLibrary.log.LogException(ex, ex.StackTrace);
                HelperLibrary.log.LogInfo("Failed to search record");
                return false;
            }
        }

        /// <summary>
        /// Author : Kavita Nunse
        /// Desc : Sets different status on search lodgement page
        /// </summary>
        /// <param name="statusList"></param>
        /// <returns></returns>
        public bool SelectDifferentLodgeStatus(params string[] statusList)
        {
            try
            {
                bool flag = true;
                for (int i = 0; i < statusList.Length; i++)
                {
                    // Click on Search Magnifying Icon present on My Lodgement tab
                    if (SearchMagnifyingIcon.FindElement(By.XPath("..")).GetAttribute("class").Contains("collapsed"))
                        Retry.Do(SearchMagnifyingIcon.Click);

                    HelperLibrary.SelectItem(LodgementPortalApp.SearchLodgementPage.status, statusList[i]);

                    // Click on search button
                    SearchButton.Click();

                    // Get Column data
                    int columnIndex = WebTableHelper.GetColumnIndex(searchGrid, LodgementPortalConstants.STATUS);
                    string columnValue = WebTableHelper.GetCellFromTable(searchGrid, 0, columnIndex);
                    if (columnValue.Contains(statusList[i]))
                        HelperLibrary.log.LogInfo(true, string.Format("Successfully Searched Record Status with value {0} in My Lodgement tab", statusList[i]));
                    else if (NoRecordFound.Displayed)
                    {
                        HelperLibrary.log.LogInfo(false, string.Format("Failed to Search Record Status with value {0} in My Lodgement tab", statusList[i]));
                        flag = false;
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {
                HelperLibrary.log.LogException(ex, ex.StackTrace);
                return false;
            }
        }


        #endregion
    }
}

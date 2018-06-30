/*Author: Kavita Nunse
 * Date : 4-August-2017
 * Desc : File contains fields related to Home Page
 */

using GI.Automation.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;

namespace CCH.Automation.LP.ProductModel
{
    public class HomePage : BaseComponent
    {
        public HomePage()
        {
            var menu = SeleniumDriver.driver.FindElement(By.CssSelector(".navbar.navbar-default.navbar-fixed-top"));
            PageFactory.InitElements(this, new RetryingElementLocator(menu, TimeSpan.FromSeconds(Convert.ToInt32(Config.WaitTime))));
        }

        #region Properties
#pragma warning disable 0649
        [FindsBy(How = How.ClassName, Using = "img-responsive")]
        public IWebElement lodgementPortalLogo { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".fa.fa-cog.fa-lg")]
        public IWebElement settingsButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".fa.fa-user-circle.fa-lg")]
        public IWebElement userProfile { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[text()='Users']")]
        public IWebElement users { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[text()='Function Access']")]
        public IWebElement functionAccess { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[text()='Version History']")]
        public IWebElement versionHistory { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[text()='Change Password']")]
        public IWebElement changePassword { get; set; }

        [FindsBy(How = How.Id, Using = "btnLogOff")]
        public IWebElement LogOff { get; set; }

        #endregion

        #region Actions

        public void ClickUsersMenu()
        {
            try
            {
                LodgementPortalApp.log.LogInfo("Clicking on settings icon from toolbar");
                settingsButton.Click();
                HelperLibrary.wait.UntilDisplayed(users);
                LodgementPortalApp.log.LogInfo("Clicking on users menu");
                users.Click();
            }
            catch (Exception ex)
            {
                LodgementPortalApp.log.LogException(ex, ex.StackTrace);
            }
        }
        public FunctionAccessPage ClickFunctionAccesssMenu()
        {
            settingsButton.Click();
            HelperLibrary.wait.UntilDisplayed(functionAccess);
            functionAccess.Click();
            return new FunctionAccessPage();
        }
        public bool ValidateVersionHistory(string siteVersion)
        {
            try
            {
                bool testResultFlag = false;
                LodgementPortalApp.log.LogInfo("Clicking on settings icon from toolbar");
                settingsButton.Click();
                HelperLibrary.wait.UntilDisplayed(versionHistory);
                LodgementPortalApp.log.LogInfo("Clicking on versionHistory menu from toolbar");
                versionHistory.Click();
                System.Threading.Thread.Sleep(2000);
                string version = SeleniumDriver.driver.FindElement(By.CssSelector("table.table tr:last-child td:first-child")).Text;

                if (version == siteVersion)
                {
                    testResultFlag = true;
                    LodgementPortalApp.log.LogInfo("Version is as expected :" + siteVersion);
                }
                else
                    LodgementPortalApp.log.LogInfo("Version is NOT as expected " + siteVersion);
                return testResultFlag;
            }
            catch (Exception ex)
            {
                LodgementPortalApp.log.LogResult(false, "Version is NOT as expected " + siteVersion);
                LodgementPortalApp.log.LogException(ex, Environment.StackTrace);
                return false;
            }
        }

        public ChangePasswordPage ChangePassword()
        {
            userProfile.Click();
            HelperLibrary.wait.UntilDisplayed(changePassword);
            changePassword.Click();
            return new ChangePasswordPage();

        }

        /// <summary>
        /// Author: Kavita Nunse 
        /// Date : 8-Aug-2017
        /// Desc : Function will return logged in user name
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            string userName = null;
            userName = userProfile.FindElement(By.TagName("text")).Text;
            return userName;
        }

        public void ClickLoggOff()
        {
            userProfile.Click();
            HelperLibrary.wait.UntilDisplayed(LogOff);
            LogOff.Click();
            ClickOnConfirmLogOff();
            HelperLibrary.wait.UntilDisplayed(LodgementPortalApp.LoginPage.userName);
        }
        #endregion


    }
}

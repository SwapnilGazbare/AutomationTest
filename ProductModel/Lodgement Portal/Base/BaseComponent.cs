using GI.Automation.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using CCH.Automation.LP.ProductModel.Pages;

namespace CCH.Automation.LP.ProductModel
{
    public class BaseComponent
    {
        public BaseComponent()
        {
            PageFactory.InitElements(this, new RetryingElementLocator(SeleniumDriver.driver, TimeSpan.FromSeconds(Convert.ToInt32(Config.WaitTime))));
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = ".modal-footer #btnModalConfirm")]
        public IWebElement confirmDialog;

        [FindsBy(How = How.Id, Using = "btnModalOk")]
        public IWebElement OkDialog;

        [FindsBy(How = How.ClassName, Using = "modal-body")]
        public IWebElement dialogBox;

        #endregion

        #region Action

        public bool ClickOnConfirmUserUpdate()
        {
            bool flag = false;
            HelperLibrary.wait.Until(ExpectedConditions.ElementExists(By.ClassName("modal-body")));
            System.Threading.Thread.Sleep(1000);
            if (dialogBox.Text.Contains(LodgementMessages.GrantAccess))
            {
                LodgementPortalApp.log.LogInfo("Clicking on confirmation dialog");
                confirmDialog.Click();
                System.Threading.Thread.Sleep(1000);
                flag = true;
            }
            else
            {
                LodgementPortalApp.log.LogResult(false, "Confirmation dialog not found");
            }

            HelperLibrary.wait.Until(ExpectedConditions.ElementExists(By.ClassName("modal-body")));
            System.Threading.Thread.Sleep(1000);
            if (dialogBox.Text.Contains(LodgementMessages.OkToProceed))
            {
                LodgementPortalApp.log.LogInfo("Clicking on OK to proceed dialog");
                OkDialog.Click();
                System.Threading.Thread.Sleep(1000);
                flag &= true;
            }
            else
                LodgementPortalApp.log.LogResult(false, "Proceed dialog not found");
            return flag;
        }

        public bool ClickOnConfirmLogOff()
        {
            bool flag = false;
            //string alertText = null;

            HelperLibrary.wait.Until(ExpectedConditions.ElementExists(By.ClassName("modal-body")));
            System.Threading.Thread.Sleep(1000);
            
            if (SeleniumDriver.driver.FindElement(By.ClassName("modal-body")).Text.Contains(LodgementMessages.LogOffMessage))
            {
                LodgementPortalApp.log.LogInfo("Clicking on confirmation dialog");
                SeleniumDriver.driver.FindElement(ObjectRepository.confirmDialog).Click();
                //confirmDialog.Click();
                System.Threading.Thread.Sleep(1000);
                flag = true;
            }
            else
                LodgementPortalApp.log.LogResult(false, "Confirmation dialog not found");

            return flag;
        }

        public bool ClickOnConfirmDeleteUser(string message)
        {
            bool flag = false;
            HelperLibrary.wait.ElementIsVisible(dialogBox);
            if (SeleniumDriver.driver.FindElement(By.ClassName("modal-body")).Text.Contains(message))
            {
                LodgementPortalApp.log.LogInfo("Clicking on confirmation dialog");
                SeleniumDriver.driver.FindElement(ObjectRepository.confirmDialog).Click();
                System.Threading.Thread.Sleep(1000);
                flag = true;
            }
            else
                LodgementPortalApp.log.LogResult(false, "Confirmation dialog not found");

            return flag;
        }
        #endregion

    }
}

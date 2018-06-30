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
    public class ChangePasswordPage : BaseComponent
    {
#pragma warning disable 0649
        [FindsBy(How = How.Id, Using = "OldPassword")]
        [CacheLookup]
        private IWebElement oldPasswordField;

        [FindsBy(How = How.Id, Using = "NewPassword")]
        [CacheLookup]
        private IWebElement newPasswordField;

        [FindsBy(How = How.Id, Using = "ConfirmPassword")]
        [CacheLookup]
        private IWebElement confirmPasswordField;

        [FindsBy(How = How.XPath, Using = "//button[normalize-space(text())='Change Password']")]
        [CacheLookup]
        private IWebElement changePassword;

        public ChangePasswordPage()
        {
            PageFactory.InitElements(SeleniumDriver.driver, this);
        }

        public void ClickChangePasswordButton()
        {
            changePassword.DoClick();
            //HelperLibrary.wait.UntilElementExists(By.Id("btnModalOk"));
            System.Threading.Thread.Sleep(3000);
            //OkDialog.Click();
            //System.Threading.Thread.Sleep(1000);
        }

        #region Actions
        /// <summary>
        /// Author : Kavita Nunse <14-Aug-2017>
        /// Desc : Function will change password for the user provided by encrypting and decrypting the password and later will revert back to original password
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool ChangePassword(string userId, string password)
        {
            string encryptedstring = null;
            string decryptedstring = null;
            bool testResult = true;

            try
            {
                // Change Password
                LodgementPortalApp.HomePage.ChangePassword();

                //Decrypt old Password
                string oldPassword = HelperLibrary.RunTimeXMl.ReadNode("OtherUser_Password");
                decryptedstring = TestExecutionDetails.StringCipher.Decrypt(oldPassword, "");
                oldPasswordField.SetText(decryptedstring);

                //Enter New Password and encrypt it

                string newPassword = password;//decryptedstring + HelperLibrary.GenerateRandomString(3);

                newPasswordField.SetText(newPassword);
                confirmPasswordField.SetText(newPassword);

                //Write the encrypted new password to RunTimexml
                encryptedstring = TestExecutionDetails.StringCipher.Encrypt(newPassword, "");
                HelperLibrary.RunTimeXMl.WriteNode("OtherUser_Password", encryptedstring);

                //Click on Change Password Button
                ClickChangePasswordButton();

                // Perform Login to check password updated or not
                if (!LodgementPortalApp.ReLaunch(userId, encryptedstring))
                    testResult = false;
                return testResult;
            }
            catch (Exception ex)
            {
                testResult = false;
                LodgementPortalApp.log.LogResult(testResult, "Failed to change password for " + userId + " User");
                LodgementPortalApp.log.LogException(ex, ex.StackTrace);
                return testResult;
            }
        }
        #endregion

    }
}

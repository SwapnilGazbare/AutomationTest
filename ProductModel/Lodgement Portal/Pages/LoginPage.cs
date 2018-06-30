/*Author: Kavita Nunse
 * Date : 4-August-2017
 * Desc : File contains fields related to Login Page
 */

using GI.Automation.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using TestExecutionDetails;

namespace CCH.Automation.LP.ProductModel
{
    public class LoginPage : BaseComponent
    {
        public LoginPage()
        {
            PageFactory.InitElements(this, new RetryingElementLocator(SeleniumDriver.driver, TimeSpan.FromSeconds(Convert.ToInt32(Config.WaitTime))));
        }

        #region Properties

        [FindsBy(How = How.Id, Using = "UserName")]
        [CacheLookup]
        public IWebElement userName;

        [FindsBy(How = How.Id, Using = "Password")]
        [CacheLookup]
        public IWebElement password;

        [FindsBy(How = How.XPath, Using = "//button[@type='submit']")]
        [CacheLookup]
        public IWebElement loginButton;

        [FindsBy(How = How.LinkText, Using = "/Account/ForgotPassword")]
        [CacheLookup]
        public IWebElement forgotPassword;

        [FindsBy(How = How.XPath, Using = "//span[contains(@class,'text-info')]")]
        public IWebElement siteVersion;


        #endregion

        #region Actions
        public HomePage login(string UserName, string Password)
        {
            try
            {
                LodgementPortalApp.log.LogInfo("Enter User Name " + UserName);
                userName.SetText(UserName);
                LodgementPortalApp.log.LogInfo("Enter Password");
                password.Clear();
                TestControl.EnterDecryptedString(password, Password);
                LodgementPortalApp.log.LogInfo("Click on Login Button");
                loginButton.Click();
                HelperLibrary.wait.Until(ExpectedConditions.ElementExists(By.ClassName("img-responsive")));
                HelperLibrary.wait.UntilDocumentReady();
                if (LodgementPortalApp.HomePage.lodgementPortalLogo.Displayed)
                {
                    LodgementPortalApp.log.LogInfo(true, "Successfully logged in with valid user");
                    return new HomePage();
                }
                else
                {
                    LodgementPortalApp.log.LogInfo(false, "Login Failed...!!!!!");
                    return null;
                }
            }
            catch(Exception ex)
            {
                LodgementPortalApp.log.LogInfo(false, "Login Failed...!!!!!");
                LodgementPortalApp.log.LogException(ex, ex.StackTrace);
                return null;
            }
        }

        public string GetSiteVersion 
        {
            get
            {
                System.Threading.Thread.Sleep(3000);
                return siteVersion.Text; 
            }
        }
                                                  
        #endregion
    }
}

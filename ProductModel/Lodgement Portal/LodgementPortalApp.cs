using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GI.Automation.Helpers;
using OpenQA.Selenium.Support.UI;
using CCH.Automation.LP.ProductModel;
using OpenQA.Selenium;

namespace CCH.Automation.LP.ProductModel
{
    public class LodgementPortalApp
    {
        public static Logs log = Logs.GetInstance();
        static LodgementPortalApp()
        {
            HelperLibrary.log = LodgementPortalApp.log;
        }

        public static bool Launch()
        {
            try
            {
                SeleniumDriver.driver = SeleniumDriver.StartDriver(BrowserType.IE.ToString());
                HelperLibrary.wait = new WebDriverWait(HelperLibrary.driver, TimeSpan.FromSeconds(Convert.ToInt16(Config.WaitTime)));

                SeleniumDriver.NavigateTo(Config.LPApplicationURL, false);

                return true;
            }
            catch (Exception ex)
            {
                HelperLibrary.log.LogException(ex, ex.StackTrace);
                return false;
            }
        }

        public static LoginPage LoginPage
        {
            get
            {
                return new LoginPage();
            }
        }

        public static HomePage HomePage
        {
            get { return new HomePage(); }
        }

        public static AddNewUserPage AddNewUserPage
        {
            get { return new AddNewUserPage(); }
        }

        public static UsersPage UsersPage
        {
            get { return new UsersPage(); }
        }
        public static UpdateUserPage UpdateUserPage
        {
            get { return new UpdateUserPage(); }
        }

        public static SearchLodgementsPage SearchLodgementPage
        {
            get { return new SearchLodgementsPage(); }
        }

        public static LodgementsPage LodgementsPage
        {
            get { return new LodgementsPage(); }
        }
        public static ChangePasswordPage ChangePasswordPage
        {
            get { return new ChangePasswordPage(); }
        }
        public static DataAccessPage DataAccessPage
        {
            get { return new DataAccessPage(); }
        }

        public static ManageDataAccess ManageDataAccess
        {
            get { return new ManageDataAccess(); }
        }
        public static bool ReLaunch(string userId, string password)
        {
            try
            {
                // Click on Logg out button
                LodgementPortalApp.HomePage.ClickLoggOff();

                // Perform Login
                LodgementPortalApp.LoginPage.login(userId, password);
                HelperLibrary.wait.UntilDisplayed(LodgementPortalApp.HomePage.lodgementPortalLogo);
                return true;
            }
            catch (Exception ex)
            {
                LodgementPortalApp.log.LogException(ex, ex.StackTrace + " Failed to relaunch and login with username");
                return false;
            }
        }
    }
}

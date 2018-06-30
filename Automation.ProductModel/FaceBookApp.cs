using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.ProductModel.Pages;
using HelperLibrary;
using OpenQA.Selenium.Support.UI;

namespace Automation.ProductModel
{
    public class FaceBookApp
    {
        public static bool Launch(string url, string Browser)
        {
            try
            {
                SeleniumDriver.driver = SeleniumDriver.StartDriver(Browser);
                Utils.wait = new WebDriverWait(Utils.driver, TimeSpan.FromSeconds(Convert.ToInt16(Config.WaitTime)));
                SeleniumDriver.NavigateTo(url , false);
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        public static void LaunchAndLogin(string url, string userId, string Password, string browser)
        {
            Launch(url, browser);
            FaceBookApp.LoginPage.Login(userId, Password);
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

    }
}

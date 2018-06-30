using System;

namespace GI.Automation.Repository
{
    public class LoginRepository
    {
        //public static string loginFrame = "frmLoginValidate";
        public static string userID = "txtUsername";
        public static string password = "txtPassword";
        public static string logon = "btnLogon";
        public static string siteVersion = "//div[@id='VersionControl']/table/tbody/tr[1]/td[2]";
        public static string siteBuild = "//div[@id='VersionControl']/table/tbody/tr[2]/td[2]";
        public static string logonTable = "//table[@id='table2']/tbody/tr[2]/td[2]/a";
        public static string loginPageLink = "//table[@id='tbl_logout']/tbody/tr[3]/td[2]/a";
        public static string cchIntegratorLogo = "//table[@id='logo']/tbody/tr/td[1]/a/img";

        //Window application controls
        public static string windowSecurity = "Windows Security";
        public static string windowUserName = "User name";
        public static string windowPassword = "Password";
        public static string windowOkButton = "OK";
    }
}

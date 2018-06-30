using OpenQA.Selenium;

namespace CCH.Automation.LP.ProductModel.Pages
{
    public class ObjectRepository
    {

        public static By userID = By.Id ("UserId");
        public static By searchButton = By.Id("btnSearchUser");
        public static By recordsList = By.XPath("//div[text()='No records found.']");
        public static By confirmDialog = By.CssSelector(".modal-footer #btnModalConfirm");

    }
    public class LodgementMessages
    {
        public const string DeleteUserMessage = "Delete this user from the Lodgement Portal?";
        public const string LogOffMessage = "Are you sure you want to log off";
        public const string GrantAccess = "The user will be granted";
        public const string OkToProceed = "Click OK to proceed";
    }
}

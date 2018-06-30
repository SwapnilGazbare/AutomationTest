using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.ProductModel.Repository
{
    public class HomePageRepository
    {
        public static By statusInput = By.XPath("//div[@data-testid='status-attachment-mentions-input']");
        public static By emojiButton = By.XPath("//a[@data-tooltip-content='Insert an emoji' and @aria-label='Insert an emoji']");
        public static By emojiType = By.XPath("//img[@alt='😀']");
        public static By postButton = By.XPath("//button[@data-testid='react-composer-post-button']");
        public static By profilePicSelector = By.XPath("//div[contains(@class,'fbTimelineProfilePicSelector')]");
        public static By updateProfilePic = By.XPath("//a[text()='Update Profile Picture']");
        public static By addProfilePic = By.XPath("//a[text()='Add Photo']");
        public static By uploadPhoto = By.XPath("//a[@data-action-type='upload_photo']");
        public static By profilePicSaveButton = By.XPath("//button[@data-testid='profilePicSaveButton']");
        public static By logoutMenu = By.Id("logoutMenu");
        public static By logoutButton = By.XPath("//span[text()='Log Out']");
        public static By EmojiAssert = By.XPath("//span[text()='😀']");

        public static By EventLink = By.XPath("//div[text()='Events']");
        public static By privateEvent = By.XPath("//a[@data-testid='event-create-split-menu']");
    }
}

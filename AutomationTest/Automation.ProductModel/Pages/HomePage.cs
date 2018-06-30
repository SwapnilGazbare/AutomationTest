using HelperLibrary;
using Automation.ProductModel.Repository;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Automation.ProductModel.Pages
{
    public class HomePage
    {
        #region Properties
        private IWebElement StatusInput
        {
            get
            {
                Utils.wait.ElementIsVisible(HomePageRepository.statusInput);
                return Utils.driver.FindElement(HomePageRepository.statusInput);
            }
        }

        private IWebElement EmojiButton
        {
            get
            {
                Utils.wait.ElementIsVisible(HomePageRepository.emojiButton);
                return Utils.driver.FindElement(HomePageRepository.emojiButton);
            }
        }

        private IWebElement EmojiType
        {
            get
            {
                Utils.wait.ElementIsVisible(HomePageRepository.emojiType);
                return Utils.driver.FindElement(HomePageRepository.emojiType);
            }
        }

        private IWebElement PostButton
        {
            get
            {
                Utils.wait.ElementIsVisible(HomePageRepository.postButton);
                return Utils.driver.FindElement(HomePageRepository.postButton);
            }
        }

        private IWebElement EmojiAssert
        {
            get
            {
                Utils.wait.ElementIsVisible(HomePageRepository.EmojiAssert);
                return Utils.driver.FindElement(HomePageRepository.EmojiAssert);
            }
        }

        private IWebElement ProfilePicSelector
        {
            get
            {
                Utils.wait.ElementIsVisible(HomePageRepository.profilePicSelector);
                return Utils.driver.FindElement(HomePageRepository.profilePicSelector);
            }
        }

        private IList<IWebElement> UpdateProfilePic
        {
            get
            {
                return Utils.driver.FindElements(HomePageRepository.updateProfilePic);
            }
        }

        private IList<IWebElement> AddProfilePic
        {
            get
            {
                return Utils.driver.FindElements(HomePageRepository.addProfilePic);
            }
        }

        private IWebElement UploadPhoto
        {
            get
            {
                Utils.wait.ElementIsVisible(HomePageRepository.uploadPhoto);
                return Utils.driver.FindElement(HomePageRepository.uploadPhoto);
            }
        }

        private IWebElement ProfilePicSaveButton
        {
            get
            {
                Utils.wait.ElementIsVisible(HomePageRepository.profilePicSaveButton);
                return Utils.driver.FindElement(HomePageRepository.profilePicSaveButton);
            }
        }

        private IWebElement LogOutMenu
        {
            get
            {
                Utils.wait.ElementIsVisible(HomePageRepository.logoutMenu);
                return Utils.driver.FindElement(HomePageRepository.logoutMenu);
            }
        }

        private IWebElement LogOutButton
        {
            get
            {
                Utils.wait.ElementIsVisible(HomePageRepository.logoutButton);
                return Utils.driver.FindElement(HomePageRepository.logoutButton);
            }
        }

        private IWebElement EventLink
        {
            get
            {
                Utils.wait.ElementIsVisible(HomePageRepository.EventLink);
                return Utils.driver.FindElement(HomePageRepository.EventLink);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validate If login Successfully
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool ValidateUserNameAtHomePage(string UserID)
        {
            try
            {
                var profileName = Utils.driver.FindElements(By.XPath("//a[@title='Profile']//span[text()='" + UserID + "']"));
                if (profileName.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (System.Exception ex)
            {
                ex.ToString();
                return false;
            }

        }

        /// <summary>
        /// Desc : Search Friends
        /// </summary>
        /// <param name="friendName"></param>
        public void SearchFriends(string friendName)
        {
            try
            {
                var Name = Utils.driver.FindElement(By.XPath("//input[@name='q']"));
                Name.SendChar(friendName);
                Actions act = new Actions(Utils.driver);
                act.Click();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Desc : Update Status
        /// </summary>
        /// <param name="status"></param>
        public bool AddWhatIsInYourMind(string status)
        {
            try
            {
                Utils.driver.FindElement(By.LinkText("Home")).Click();
                StatusInput.DoClick();
                StatusInput.SendChar(status);

                if (EmojiButton.Displayed)
                    EmojiButton.Click();

                EmojiType.Click();
                StatusInput.Click();
                PostButton.Click();
                System.Threading.Thread.Sleep(3000);
                if (Utils.driver.FindElements(By.XPath("//p[contains(text(),'" + status + "')]")).Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Desc : upload Profile Pic
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="filePath"></param>
        public void Uploadphoto(string userName, string filePath)
        {
            try
            {
                Utils.driver.FindElement(By.XPath("//div[text()='" + userName + "']")).Click();
                Actions Action = new Actions(Utils.driver);
                if (UpdateProfilePic.Count > 0)
                    Action.MoveToElement(ProfilePicSelector).Click(UpdateProfilePic[0]).Build().Perform();
                else
                    Action.MoveToElement(ProfilePicSelector).Click(AddProfilePic[0]).Build().Perform();
                UploadPhoto.Click();
                System.Threading.Thread.Sleep(3000);
                SendKeys.SendWait(filePath);
                SendKeys.SendWait("{Enter}");
                UploadPhoto.SendKeys(filePath);

                ProfilePicSaveButton.Click();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        /// <summary>
        /// Desc : Create Private Event
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public bool CreatePrivateEvent(string eventName)
        {
            EventLink.Click();

            Utils.driver.FindElement(By.XPath("//a[@data-testid='event-create-split-menu']")).Click();

            Utils.driver.FindElement(By.XPath("//a[contains(@ajaxify,'/events/dialog/create/')]")).Click();

            Utils.driver.FindElement(By.XPath("//input[@data-testid ='event-create-dialog-name-field']")).SendKeys(eventName);
            Utils.driver.FindElement(By.XPath("//input[@data-testid ='event-create-dialog-where-field']")).SendKeys("Pune");

            var buttonCount = Utils.driver.FindElements(By.XPath("//button[@data-testid ='event-create-dialog-confirm-button']"));
            for (int i = 0; i < buttonCount.Count; i++)
            {
                if (buttonCount[i].Displayed)
                    buttonCount[i].DoClick();
            }

            System.Threading.Thread.Sleep(3000);
            Utils.wait.ElementIsVisible(Utils.driver.FindElement(By.XPath("//h1[@data-testid='event-permalink-event-name']")));

            if (Utils.driver.FindElement(By.XPath("//h1[@data-testid='event-permalink-event-name']")).Text.Equals(eventName))
                return true;

            else
                return false;
        }

        /// <summary>
        /// Desc : Log Out
        /// </summary>
        public void Logoff()
        {
            LogOutMenu.Click();
            LogOutButton.Click();
        }
        #endregion
    }
}
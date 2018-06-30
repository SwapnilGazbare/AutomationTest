using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelperLibrary;
using Automation.ProductModel;
using System;

namespace FacebookTests
{
    [TestClass]
    public class Tests : BaseTest
    {
        [TestMethod, TestProperty("BrowserType", "Chrome")]
        public void AddStatusAndValidate()
        {
            try
            {
                bool flag = false;
                flag = FaceBookApp.HomePage.ValidateUserNameAtHomePage(InputDataFiles.InputData.FirstName);
                flag = flag && FaceBookApp.HomePage.AddWhatIsInYourMind(InputDataFiles.InputData.Status);

                Assert.IsTrue(flag);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                FaceBookApp.HomePage.Logoff();
            }
        }

        [TestMethod, TestProperty("BrowserType", "Chrome")]
        public void UploadPhotoAndCreatePrivateEvent()
        {
            try
            {
                FaceBookApp.HomePage.Uploadphoto(InputDataFiles.InputData.FirstName + " " + InputDataFiles.InputData.Surname, Config.SolutionPath + InputDataFiles.InputData.filePath);
                bool flag = FaceBookApp.HomePage.CreatePrivateEvent(InputDataFiles.InputData.EventStatus);

                Assert.IsTrue(flag);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                FaceBookApp.HomePage.Logoff();
            }

        }

    }
}

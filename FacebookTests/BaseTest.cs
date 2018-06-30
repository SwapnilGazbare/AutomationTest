using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using HelperLibrary;
using Automation.ProductModel;

namespace FacebookTests
{
    [TestClass]
    public class BaseTest
    {
        public BaseTest()
        {
        }
        private static string runningTestSuite = string.Empty;

        [AssemblyInitialize]
        public static void ExecutionInitialization(TestContext _testContext)
        {
            Utils.assembly = Assembly.Load(Config.ProjectName);
            Utils.inputDataFolderPath = string.Format("{0}.InputDataFiles.", Config.ProjectName);
            Utils.inputDataFilePath = Utils.inputDataFolderPath + "InputData";
            runningTestSuite = _testContext.FullyQualifiedTestClassName;
        }

        /// <summary>
        /// Cleanup method after completing the execution to generate final report
        /// </summary>
        [AssemblyCleanup]
        public static void ExecutionCleanup()
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        #region Additional test attributes

        [TestInitialize()]
        public void TestInitialize()
        {
            string testName = TestContext.TestName;
            string browserType = string.Empty;
            Type t = GetType();

            MethodInfo mi = t.GetMethod(TestContext.TestName);
            Type MyType = typeof(TestPropertyAttribute);
            object[] attributes = mi.GetCustomAttributes(MyType, false);

            string browser = string.Empty;
            foreach (TestPropertyAttribute obj in attributes)
            {
                if (obj.Name == ConstantVariables.BROWSER_TYPE)
                {
                    browser = obj.Value;
                }
            }       
            if (browser.Equals("IE"))
                FaceBookApp.LaunchAndLogin(Config.URL, Config.UserID, Config.Password, BrowserType.IE.ToString());
            if (browser.Equals("Chrome"))
                FaceBookApp.LaunchAndLogin(Config.URL, Config.UserID, Config.Password, BrowserType.Chrome.ToString());
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TestCleanup()
        {
            if (SeleniumDriver.driver != null)
            {
                SeleniumDriver.driver.Dispose();
                SeleniumDriver.driver.Quit();
                SeleniumDriver.driver = null;
            }
            Utils.KillProcessByName("firefox");
            Utils.KillProcessByName("geckodriver");
            Utils.KillProcessByName("EXCEL");
        }

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;
    }

    internal class ConstantVariables
    {
        public const string TEST_CASE_NAME = "TestCaseName";

        public const string TEST_CASE_ID = "TestCaseID";

        public const string BROWSER_TYPE = "BrowserType";
        public const string ROLE = "Role";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace GI.Automation.Utility
{

    #region Config
    public class Config
    {
        private static string solutionPath;
        private static string resultsLogDirectory;
        private static string resultsLogFileName;
        private static string resultsLogFile = null;
        private static string executionLogDirectory;
        private static string executionLogFileName;
        private static string executionLogFile = null;
        private static string language;
        private static string siteVerion;
        private static string siteBuild;
        private static string browser;
        private static string testSuite;
        private static string fromEmailID;
        private static string encryptedEmailPassword;
        private static string logDirectory;

        /// <summary>
        /// Initialize and assigns absolute path of solution to static field solutionpath
        /// </summary>
        static Config()
        {
            try
            {
                //solutionPath = Directory.GetParent(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).Parent.Parent.FullName;
                //solutionPath = (Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent).Parent.FullName;           
                if (System.IO.Directory.GetCurrentDirectory().Contains("\\bin\\Debug"))
                    solutionPath = (Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent).Parent.Parent.Parent.Parent.FullName;
                else
                    solutionPath = (Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent).Parent.FullName;

                resultsLogDirectory = Config.solutionPath + ConfigurationManager.AppSettings["ResultLogDirectory"].ToString();
                resultsLogFileName = ConfigurationManager.AppSettings["ResultLogFileName"].ToString();
                executionLogDirectory = Config.solutionPath + ConfigurationManager.AppSettings["ExecutionLogDirectory"].ToString();
                executionLogFileName = ConfigurationManager.AppSettings["ExecutionLogFileName"].ToString();
                language = ConfigurationManager.AppSettings["Language"].ToString();
                siteVerion = ConfigurationManager.AppSettings["siteVerion"].ToString();
                siteBuild = ConfigurationManager.AppSettings["siteBuild"].ToString();
                browser = ConfigurationManager.AppSettings["Browser"].ToString();
                testSuite = ConfigurationManager.AppSettings["TestSuite"].ToString();
                fromEmailID = ConfigurationManager.AppSettings["EmailID"].ToString();
                encryptedEmailPassword = ConfigurationManager.AppSettings["EncryptedEmailPassword"].ToString();
                logDirectory = ConfigurationManager.AppSettings["LogDirectory"].ToString();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        public static string SolutionPath
        {
            get
            { return solutionPath; }
        }

        /// <summary>
        /// Gets absolute path of Selenium browser drivers for win32 and x64 OS from appconfig
        /// </summary>
        public static string BrowserDrivers
        {
            get
            {
                if (System.Environment.Is64BitOperatingSystem)
                    return solutionPath + ConfigurationManager.AppSettings["BrowserDriversx64"].ToString();
                else
                    return solutionPath + ConfigurationManager.AppSettings["BrowserDriversWin32"].ToString();
            }
        }

        /// <summary>
        /// Gets Application URL
        /// </summary>
        public static string ApplicationURL
        {
            get { return ConfigurationManager.AppSettings["URL"].ToString(); }
        }

        public static string WaitTime
        {
            get { return ConfigurationManager.AppSettings["WaitTime"].ToString(); }
        }

        /// <summary>
        /// Gets Application TestSuite Name
        /// </summary>
        public static string TestSuite
        {
            get { return ConfigurationManager.AppSettings["TestSuite"].ToString(); }
        }

        /// <summary>
        /// Gets absolute path of TestDataDirectory from appConfig
        /// </summary>
        public static string TestDataDirectory
        {
            get { return solutionPath + ConfigurationManager.AppSettings["TestDataDirectory"].ToString(); }
        }

        /// <summary>
        /// Gets name of InputTestDataFile from appConfig
        /// </summary>
        public static string InputTestDataFileName
        {
            get { return ConfigurationManager.AppSettings["InputTestDataFileName"].ToString(); }
        }

        /// <summary>
        /// Gets absolute path of ResultLogDirectory from appConfig
        /// </summary>
        public static string ResultLogDirectory
        {
            get { return resultsLogDirectory; }
        }

        /// <summary>
        /// Gets/Sets name of ResultLogFile from/into appConfig
        /// </summary>
        public static string ResultLogFileName
        {
            get { return resultsLogFileName; }

            set { resultsLogFileName = value; }
        }

        /// <summary>
        /// Gets the created log file with fully qualified name
        /// </summary>
        public static string ResultLogFile
        {
            get { return resultsLogFile; }
            set { resultsLogFile = value; }
        }

        /// <summary>
        /// Gets absolute path of ExecutionLogDirectory from appConfig
        /// </summary>
        public static string ExecutionLogDirectory
        {
            get { return executionLogDirectory; }
        }

        /// <summary>
        ///Gets/Sets name of ExecutionLogFile from/into appConfig 
        /// </summary>
        public static string ExecutionLogFileName
        {
            get { return executionLogFileName; }
            set { executionLogFileName = value; }
        }

        /// <summary>
        /// Gets absolute path of ExecutionLogFilePath
        /// </summary>
        public static string ExecutionLogFile
        {
            get { return executionLogFile; }
            set { executionLogFile = value; }
        }

        /// <summary>
        /// Gets bool value of EnableResultLogging from app.config
        /// </summary>
        public static bool IsResultLogEnabled
        {
            get { return ConfigurationManager.AppSettings["EnableResultLogging"].ToString().ToLower().Equals("yes") ? true : false; }
        }

        /// <summary>
        /// Gets bool value of EnableExecutionLogging from app.config
        /// </summary>
        public static bool IsExecutionLogEnabled
        {
            get { return ConfigurationManager.AppSettings["EnableExecutionLogging"].ToString().ToLower().Equals("yes") ? true : false; ;}
        }

        /// <summary>
        /// Gets/sets Language from/into app.config
        /// </summary>
        public static string Language
        {
            get { return language; }
        }

        public static string FromEmailID
        {
            get { return fromEmailID; }
        }

        public static string EncryptedEmailPassword
        {
            get { return encryptedEmailPassword; }
        }

        /// <summary>
        /// Gets/sets Language from/into app.config
        /// </summary>
        public static string SiteVerion
        {
            get { return siteVerion; }
        }

        /// <summary>
        /// Gets/sets Language from/into app.config
        /// </summary>
        public static string SiteBuild
        {
            get { return siteBuild; }
        }

        /// <summary>
        /// Gets/sets Browser from/into app.config
        /// </summary>
        public static string Browser
        {
            get { return browser; }
            set { browser = value; }
        }

        public static TestEnvironment Environtmentdetails
        {
            get
            {
                try
                {
                    System.Collections.Hashtable environmentDetails = (System.Collections.Hashtable)ConfigurationManager.GetSection("Environment");
                    return new TestEnvironment(environmentDetails);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Environment details were not found -  {0}", ex));
                }
            }
        }

        public static RunTimeDataDetails RunTimeData
        {
            get
            {
                try
                {
                    System.Collections.Hashtable runTimeDataDetails = (System.Collections.Hashtable)ConfigurationManager.GetSection("RunTimeDataDetails");
                    return new RunTimeDataDetails(runTimeDataDetails);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("RunTimeData details were not found -  {0}", ex));
                }
            }
        }

        public static string LogDirectory
        {
            get
            { return logDirectory; }
        }
    }
    #endregion Config

    public class EnvironmentDetails : ConfigurationSection
    {
        [ConfigurationProperty("Environments")]
        public TestEnvironmentCollection TestEnvironmentItems
        {
            get { return ((TestEnvironmentCollection)(base["Environments"])); }
        }
    }

    [ConfigurationCollection(typeof(TestEnvironmentElement))]
    public class TestEnvironmentCollection : ConfigurationElementCollection
    {
        internal const string PropertyName = "Element";

        protected override ConfigurationElement CreateNewElement()
        {
            return new TestEnvironmentElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TestEnvironmentElement)element).UserID;
        }
        public TestEnvironmentElement this[int index]
        {
            get { return (TestEnvironmentElement)BaseGet(index); }
        }

        protected override string ElementName
        {
            get
            {
                return PropertyName;
            }
        }
    }


    public class TestEnvironmentElement : ConfigurationElement
    {
        //[ConfigurationProperty("Environment",DefaultValue="", IsKey=true, IsRequired=true)]
        //public string TestEnvironment
        //{
        //    get{return ((string)base["Environment"]);}
        //    set{base["Environment"] = value;}
        //}

        [ConfigurationProperty("URL", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string URL
        {
            get { return ((string)base["URL"]); }
            set { base["URL"] = value; }
        }

        [ConfigurationProperty("UserID", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string UserID
        {
            get { return ((string)base["UserID"]); }
            set { base["UserID"] = value; }
        }

        [ConfigurationProperty("Password", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Password
        {
            get { return ((string)base["Password"]); }
            set { base["Password"] = value; }
        }
    }


    public class TestEnvironment
    {
        private System.Collections.Hashtable environmentDetails;
        public TestEnvironment(System.Collections.Hashtable environmentDetails)
        {
            this.environmentDetails = environmentDetails;
        }

        [ConfigurationProperty("URL", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string URL
        {
            get { return ((string)environmentDetails["URL"]); }
            set { environmentDetails["URL"] = value; }
        }

        [ConfigurationProperty("UserID", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string UserID
        {
            get { return ((string)environmentDetails["UserID"]); }
            set { environmentDetails["UserID"] = value; }
        }

        [ConfigurationProperty("Password", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Password
        {
            get { return ((string)environmentDetails["Password"]); }
            set { environmentDetails["Password"] = value; }
        }
    }


    public class RunTimeDataDetails
    {
        private System.Collections.Hashtable runTimeDataDetails;
        public RunTimeDataDetails(System.Collections.Hashtable runTimeDataDetails)
        {
            this.runTimeDataDetails = runTimeDataDetails;
        }

        [ConfigurationProperty("EntityName", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string EntityName
        {
            get { return ((string)runTimeDataDetails["EntityName"]); }
            set { runTimeDataDetails["EntityName"] = value; }
        }

        [ConfigurationProperty("ChildEntity1", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ChildEntity1
        {
            get { return ((string)runTimeDataDetails["ChildEntity1"]); }
            set { runTimeDataDetails["ChildEntity1"] = value; }
        }

        [ConfigurationProperty("ChildEntity2", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ChildEntity2
        {
            get { return ((string)runTimeDataDetails["ChildEntity2"]); }
            set { runTimeDataDetails["ChildEntity2"] = value; }
        }

        [ConfigurationProperty("MasterTemplate", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string MasterTemplate
        {
            get { return ((string)runTimeDataDetails["MasterTemplate"]); }
            set { runTimeDataDetails["MasterTemplate"] = value; }
        }
    }
}

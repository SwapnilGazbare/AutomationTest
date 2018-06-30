using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Xml;

namespace GI.Automation.Helpers
{

    #region Config
    public class Config
    {
        const string lpPIV = "lodgementportal";
        private static string solutionPath;
        public static string testsuiteName;
        private static string projectPath;
        private static string resultsLogDirectory;
        private static string resultsLogFileName;
        private static string resultsLogFile = null;
        private static string executionLogDirectory;
        private static string executionLogFileName;
        private static string executionLogFile = null;
        private static string language;
        private static string siteVerion;
        private static string siteBuild;

        #region MayurP on 04/01/2018 for comparing LPSiteVersion and Sitebuild
        private static string LPsiteVerion;
        private static string LPsiteBuild;
        #endregion

        private static string browser;
        private static string testSuite;
        //  private static string fromEmailID;
        //  private static string encryptedEmailPassword;
        //  private static bool moveLogToRemoteLocation;
        //   private static string remoteLocationPath;
        private static string logDirectory;
        private static string emailIDsToSendTestResults;
        private static string emailSubjectLine;
        //  private static bool mailTestResults;
        private static bool reRunFailedScenarios;
        private static bool windowsAuthentication;
        private static bool runTimeTestDataCleanup;
        //MayurP on 19/01/2018 making xml as a public
       public static WriteXMLOperations xmlFile = null;
        private static string configPath;

        private static string userId;
        private static string password;

        private static string lpUserId;
        private static string lpPassword;


        //reporting year variables
        public static string createReportingYear = "";
        public static string automationReportingYear = "";
        public static string reportingPeriod01 = "";
        public static string reportingPeriod02 = "";
        public static string reportingPeriod01FromDate = "";
        public static string reportingPeriod02FromDate = "";


        //MayurP on 18/01/2018
        public static string zenDeskTicket = "";
        /// <summary>
        /// Initialize and assigns absolute path of solution to static field solutionpath
        /// </summary>
        static Config()
        {
            try
            {

                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configPath = config.FilePath;

                config.AppSettings.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Modified);


                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                solutionPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(baseDirectory)));

                projectPath = Path.GetDirectoryName(Path.GetDirectoryName(baseDirectory));
                configPath = projectPath + @"\App.config";

                xmlFile = new WriteXMLOperations(configPath);

                resultsLogDirectory = Config.solutionPath + ConfigurationManager.AppSettings["ResultLogDirectory"].ToString();
                resultsLogFileName = ConfigurationManager.AppSettings["ResultLogFileName"].ToString();
                executionLogDirectory = Config.solutionPath + ConfigurationManager.AppSettings["ExecutionLogDirectory"].ToString();
                executionLogFileName = ConfigurationManager.AppSettings["ExecutionLogFileName"].ToString();
                language = ConfigurationManager.AppSettings["Language"].ToString();
                siteVerion = ConfigurationManager.AppSettings["siteVerion"].ToString();
                siteBuild = ConfigurationManager.AppSettings["siteBuild"].ToString();


                browser = ConfigurationManager.AppSettings["Browser"].ToString();
                testSuite = ConfigurationManager.AppSettings["TestSuite"].ToString();
                emailIDsToSendTestResults = ConfigurationManager.AppSettings["EmailIDsToSendTestResults"].ToString();
                emailSubjectLine = ConfigurationManager.AppSettings["EmailSubjectLine"].ToString();
                logDirectory = ConfigurationManager.AppSettings["LogDirectory"].ToString();
                reRunFailedScenarios = ConfigurationManager.AppSettings["ReRunFailedScenarios"].ToString().ToLower() == "yes" ? true : false;
                windowsAuthentication = ConfigurationManager.AppSettings["WindowsAuthentication"].ToString().ToLower() == "yes" ? true : false;
                runTimeTestDataCleanup = ConfigurationManager.AppSettings["RunTimeTestDataCleanup"].ToString().ToLower() == "yes" ? true : false;

                #region MayurP on 18/01/2018
                zenDeskTicket = ConfigurationManager.AppSettings["zenDeskTicket"].ToString();
                #endregion 

                #region Reporing Year For PIV
                createReportingYear = ConfigurationManager.AppSettings["CreateReportingYear"].ToString();
                automationReportingYear = ConfigurationManager.AppSettings["AutomationReportingYear"].ToString();
                reportingPeriod01 = ConfigurationManager.AppSettings["Q1"].ToString();
                reportingPeriod02 = ConfigurationManager.AppSettings["Q2"].ToString();

                reportingPeriod01FromDate = ConfigurationManager.AppSettings["Q1_FromDate"].ToString();
                reportingPeriod02FromDate = ConfigurationManager.AppSettings["Q2_FromDate"].ToString();
                #endregion

                userId = ConfigurationManager.AppSettings["UserID"].ToString();
                password = ConfigurationManager.AppSettings["Password"].ToString();

                //GI.Automation.PIV.Test

                string[] projectNameChunks =  Config.ProjectName.Split('.');
                switch (projectNameChunks[projectNameChunks.Length - 2].ToLower().Trim())
                {
                   
                    case lpPIV:

                        lpUserId = ConfigurationManager.AppSettings["LPUserID"].ToString();
                        lpPassword = ConfigurationManager.AppSettings["LPPassword"].ToString();

                        #region MayurP on 04/01/2018

                        LPsiteVerion = ConfigurationManager.AppSettings["LPsiteVerion"].ToString();
                        LPsiteBuild = ConfigurationManager.AppSettings["LPsiteBuild"].ToString();
                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        public static string UserID
        {
            get
            { return xmlFile.ReadNode("UserID"); }
            set
            { ConfigurationManager.AppSettings["UserID"] = value; }

        }
        public static string Password
        {
            get
            { return xmlFile.ReadNode("Password"); }
            set
            { ConfigurationManager.AppSettings["Password"] = value; }
        }

        public static string LPUserID
        {
            get
            { return xmlFile.ReadNode("LPUserID"); }
            set
            { ConfigurationManager.AppSettings["LPUserID"] = value; }

        }
        public static string LPPassword
        {
            get
            { return xmlFile.ReadNode("LPPassword"); }
            set
            { ConfigurationManager.AppSettings["LPPassword"] = value; }
        }


        public static string SolutionPath
        {
            get
            { return solutionPath; }
        }

        public static string ProjectPath
        {
            get
            {
                return projectPath;
            }
        }

        public static string ProjectName
        {
            get
            {
                return System.IO.Path.GetFileName(Config.projectPath);
            }
        }

        
        /// <summary>
        /// Gets absolute path of Selenium browser drivers for win32 and x64 OS from appconfig
        /// </summary>
        public static string BrowserDrivers
        {
            get
            {
                if (System.Environment.Is64BitOperatingSystem)
                    return solutionPath + ConfigurationManager.AppSettings["BrowserDriversWin32"].ToString();
                else
                    return solutionPath + ConfigurationManager.AppSettings["BrowserDriversWin32"].ToString();
            }
        }

        /// <summary>
        /// Gets Application URL
        /// </summary>
        public static string ApplicationURL
        {
            get
            {
                return xmlFile.ReadNode("URL");
            }
        }

        /// <summary>
        /// Gets Application URL
        /// </summary>
        public static string LPApplicationURL
        {
            get
            {
                return xmlFile.ReadNode("LPURL");
            }
        }

        public static string WaitTime
        {
            get { return ConfigurationManager.AppSettings["WaitTime"].ToString(); }
        }

        public static string PopUpWaitTime
        {
            get { return ConfigurationManager.AppSettings["PopUpWaitTime"].ToString(); }
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
            get
            {
                return xmlFile.ReadNode("EmailID");
            }
        }
        public static string EncryptedEmailPassword
        {
            get
            {
                return xmlFile.ReadNode("EncryptedEmailPassword");
            }
        }

        public static string EmailIDsToSendTestResults
        {
            get { return emailIDsToSendTestResults; }
        }

        public static string EmailSubjectLine
        {
            get { return emailSubjectLine; }
            set { emailSubjectLine = value; }
        }

        public static bool MailTestResults
        {
            get
            {
                return xmlFile.ReadNode("MailTestResults").ToString().ToLower() == "yes" ? true : false;
            }
        }

        public static bool ReRunFailedScenarios
        {
            get { return reRunFailedScenarios; }
        }

        public static bool MoveLogToRemoteLocation
        {
            get
            {
                return xmlFile.ReadNode("MoveLogToRemoteLocation").ToString().ToLower() == "yes" ? true : false;
            }
        }

        public static bool PromptForExecutionDetails
        {
            get
            {
                return ConfigurationManager.AppSettings["PromptForExecutionDetails"].ToString().ToLower() == "yes" ? true : false;
            }
        }

        public static bool RunTimeTestDataCleanup
        {
            get
            {
                return ConfigurationManager.AppSettings["RunTimeTestDataCleanup"].ToString().ToLower() == "yes" ? true : false;
            }
        }

        public static string RemoteLocationPath
        {
            get
            {
                return xmlFile.ReadNode("RemoteLocationPath");
            }
        }

        /// <summary>
        /// Gets/sets Language from/into app.config
        /// </summary>
        public static string SiteVerion
        {
            get { return siteVerion; }
        }

        #region MayurP 0n 04/01/2018
        /// <summary>
        /// Gets/sets Language from/into app.config
        /// </summary>
        public static string LPSiteBuild
        {
            get { return LPsiteBuild; }
        }

        // MayurP on 04/01/2018

        /// <summary>
        /// Gets/sets Language from/into app.config
        /// </summary>
        public static string LPSiteVerion
        {
            get { return LPsiteVerion; }
        }

        #endregion

        /// <summary>
        /// Gets/sets Language from/into app.config
        /// </summary>
        public static string SiteBuild
        {
            get { return siteBuild; }
        }

        //End MayurP on 04/01/2018
        /// <summary>
        /// Gets/sets Browser from/into app.config
        /// </summary>
        public static string Browser
        {
            get { return browser; }
            set { browser = value; }
        }

        public static string LogDirectory
        {
            get
            { return logDirectory; }
        }

        public static bool WindowsAuthentication
        {
            get
            { return windowsAuthentication; }
        }


#region MayurP on 18/01/2018
        public static string ZenDeskTicket 
        {
            get { return zenDeskTicket; }
            set { zenDeskTicket = value; }
        }
#endregion
    }
    #endregion Config

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

    public class WriteXMLOperations
    {
        private string fileName = string.Empty;
        XmlDocument xmlDoc = null;
        XmlNode rootNode = null;
        public WriteXMLOperations(string fileName)
        {
            this.fileName = fileName;
            this.xmlDoc = new XmlDocument();
        }

        public void Close()
        {
            this.rootNode = null;
            this.xmlDoc = null;
        }
        private bool RootNode
        {
            get
            {
                if (this.rootNode == null)
                    return false;
                return true;
            }
            set
            {
                if (value && !this.RootNode)
                {
                    if (File.Exists(this.fileName))
                    {
                        xmlDoc.Load(this.fileName);
                        XmlNodeList userNodes = this.xmlDoc.SelectNodes(string.Format("//configuration//appSettings"));
                        this.rootNode = userNodes[0];
                    }
                    else
                    {
                        this.rootNode = this.xmlDoc.CreateElement("TestData");
                        this.xmlDoc.AppendChild(this.rootNode);
                    }
                }
            }
        }
        /// <summary>
        /// Writes the value to XML Files
        /// </summary>
        /// <param name="node">string : XML node name to write</param>
        /// <param name="nodeValue">string : XML node value to write</param>
        public void WriteNode(string node, string nodeValue)
        {

            this.RootNode = true;
            XmlNodeList userNodes1 = null;
            if (node.ToLower().Equals("UserID".ToLower()))
            {
                XmlNode xmlNode = this.xmlDoc.SelectSingleNode(string.Format("//configuration/Environment"));
                xmlNode.Attributes[1].Value = nodeValue;
            }
            else if (node.ToLower().Equals("password".ToLower()))
            {
                XmlNode xmlNode = this.xmlDoc.SelectSingleNode(string.Format("//configuration/Environment"));
                xmlNode.Attributes[2].Value = nodeValue;
            }
            else
            {
                userNodes1 = this.xmlDoc.SelectNodes(string.Format("//configuration/appSettings/add"));
                foreach (XmlNode userNode in userNodes1)
                {
                    if (userNode.Attributes[0].Value.ToLower().Equals(node.ToLower()))
                        userNode.Attributes[1].Value = nodeValue;
                }
            }

            this.xmlDoc.Save(this.fileName);
        }

        /// <summary>
        /// Writes the value to XML Files
        /// </summary>
        /// <param name="node">string : XML node name to write</param>
        /// <param name="nodeValue">string : XML node value to write</param>
        /// <param name="parentnode">string : XML parent node name to write</param>
        public void WriteNode(string node, string nodeValue, string parentnode)
        {

            this.RootNode = true;
            this.xmlDoc.AppendChild(this.rootNode);

            XmlNode userNode = xmlDoc.CreateElement(node);
            userNode.InnerText = nodeValue;
            this.rootNode.AppendChild(userNode);

            throw new Exception("Method is not yet fully implemented");

            //this.SaveXML();
        }

        /// <summary>
        /// Reads the value from the XML Files
        /// </summary>
        /// <param name="node">string : XML node name to read</param>
        /// <returns> node ; if not found node returns null. </returns>
        public string ReadNode(string node)
        {
            if (this.rootNode == null)
                xmlDoc.Load(this.fileName);

            XmlNodeList userNodes1 = null;
            userNodes1 = this.xmlDoc.SelectNodes(string.Format("//configuration/appSettings/add"));
            foreach (XmlNode userNode in userNodes1)
            {
                if (userNode.Attributes[0].Value.ToLower().Equals(node.ToLower()))
                    return userNode.Attributes[1].Value.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// Removes all the node from XML file.
        /// </summary>
        public void RemoveNodes()
        {
            this.rootNode.RemoveAll();
            xmlDoc.Save(this.fileName);
        }

    }
}

using System;
using System.Configuration;
using System.IO;

namespace HelperLibrary
{
    public class Config
    {
        private static string configPath;
        private static string solutionPath;
        private static string projectPath;
        private static string browser;
        private static string logDirectory;
        private static string userId;
        private static string password;

        /// <summary>
        /// Initialize and assigns absolute path of solution to static field solutionpath
        /// </summary>
        static Config()
        {
            try
            {
                Configuration config =  ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configPath = config.FilePath;

                config.AppSettings.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Modified);
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                solutionPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(baseDirectory)));

                projectPath = Path.GetDirectoryName(Path.GetDirectoryName(baseDirectory));
                configPath = projectPath + @"\App.config";

                browser = ConfigurationManager.AppSettings["Browser"].ToString();
                //emailIDsToSendTestResults = ConfigurationManager.AppSettings["EmailIDsToSendTestResults"].ToString();
                //emailSubjectLine = ConfigurationManager.AppSettings["EmailSubjectLine"].ToString();
                logDirectory = ConfigurationManager.AppSettings["LogDirectory"].ToString();

                userId = ConfigurationManager.AppSettings["UserID"].ToString();
                password = ConfigurationManager.AppSettings["Password"].ToString();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public static string UserID
        {
            get
            { return userId; }
        }

        public static string Password
        {
            get
            {
                return password;
            }
        }

        public static string URL
        {
            get
            { return ConfigurationManager.AppSettings["URL"].ToString(); }
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
                return Path.GetFileName(Config.projectPath);
            }
        }

        public static string WaitTime
        {
            get { return ConfigurationManager.AppSettings["WaitTime"].ToString(); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace GI.Automation.Utility
{
    public class Logs : IDisposable
    {
        #region Private Members

        private static Logs log;
        private FileInfo file;
        private FileStream fileStream;
        private StreamWriter streamWriter;
        private string resultLogDirectory;
        private string resultLogFileName;
        private string executionLogDirectory;
        public string executionLogFileName;
        public string resultLogFile = String.Empty;
        public string executionLogFile = String.Empty;
        bool isResultLogEnabled;
        bool isExecutionLogEnabled;
        string serviceName = String.Empty;
        string operationName = String.Empty;
        string dataRowId;
        bool finalResult;
        bool dataResult;
        DateTime startTime;
        bool isErrorFound;
        string buildNumber = string.Empty;
        string bindingName = string.Empty;
        string hostPlatform = string.Empty;
        string clientPlatform = string.Empty;
        string hostIp = string.Empty;
        string clientIp = string.Empty;


        private string logDirectory;
        public string logFolder;

        #endregion


        #region Constructor
        public static Logs GetInstance()
        {
            if (log == null)
                return new Logs();

            return log;
        }

        public Logs()
        {
            //this.resultLogDirectory = Config.ResultLogDirectory;
            //this.resultLogFileName = Config.ResultLogFileName;
            //this.resultLogFileName = this.resultLogFileName + DateTime.Today.Date.ToString("ddMMyyyy");
            //this.resultLogFileName = Utility.GetNextFileName(Path.Combine(this.resultLogDirectory, this.resultLogFileName));
            //this.resultLogFile= this.resultLogFileName + ".xml";
            //Config.ResultLogFile = this.resultLogFile;

            //this.executionLogDirectory = Config.ExecutionLogDirectory;
            //this.executionLogFileName = Config.ExecutionLogFileName;
            //this.executionLogFileName = this.executionLogFileName + DateTime.Today.Date.ToString("ddMMyyyy");
            //this.executionLogFileName = Utility.GetNextFileName(Path.Combine(this.executionLogDirectory, this.executionLogFileName));
            //this.executionLogFile = this.executionLogFileName + ".xml";
            //Config.ExecutionLogFile = this.executionLogFile;

            //this.isResultLogEnabled = Config.IsResultLogEnabled;
            //this.isExecutionLogEnabled = Config.IsExecutionLogEnabled;

            //this.hostPlatform = Config.GetHostPlatform;
            //this.clientPlatform = Config.GetClientPlatform;
            //this.hostIp = Config.GetHostIp;
            //this.clientIp = Config.GetClientIp;


            this.logDirectory = Config.LogDirectory;

            this.logFolder = Directory.CreateDirectory(Config.SolutionPath + this.logDirectory + "\\PIVLog_" + DateTime.Now.ToString("ddMMyyyy_HHmmss")).FullName;

            string sourcePath = Config.SolutionPath + this.logDirectory;
            string destinationPath = this.logFolder;

            foreach (var file in Directory.GetFiles(sourcePath))
                File.Copy(file, Path.Combine(destinationPath, Path.GetFileName(file)), true);


            this.resultLogFileName = Config.ResultLogFileName;
            this.resultLogFileName = Utility.GetNextFileName(Path.Combine(this.logFolder, this.resultLogFileName));
            this.resultLogFile = this.resultLogFileName + ".xml";
            Config.ResultLogFile = this.resultLogFile;

            this.executionLogFileName = Config.ExecutionLogFileName;
            this.executionLogFileName = Utility.GetNextFileName(Path.Combine(this.logFolder, this.executionLogFileName));
            this.executionLogFile = this.executionLogFileName + ".xml";
            Config.ExecutionLogFile = this.executionLogFile;

            this.isResultLogEnabled = Config.IsResultLogEnabled;
            this.isExecutionLogEnabled = Config.IsExecutionLogEnabled;
        }

        public Logs(string serviceName, string operationName)
            : this()
        {
            this.serviceName = serviceName;
            this.operationName = operationName;
        }
        #endregion Constructor

        #region Properties
        public string ServiceName
        {
            get { return serviceName; }
            set { serviceName = value; }
        }

        public string OperationName
        {
            get { return operationName; }
            set { operationName = value; }
        }
        #endregion Properties

        #region Private Methods

        /// <summary>
        /// LoggerGen : Opens the log file and creates file stream
        /// </summary>
        /// <param name="fileToBeOpened">Name of the file to open</param>
        private void OpenFile(string fileToBeOpened)
        {
            try
            {
                //opens the file specified
                this.file = new FileInfo(fileToBeOpened);
                //if file does not exist then create new file
                if (!this.file.Exists)
                    fileStream = this.file.Create();
                else // else append to the existing file
                    fileStream = this.file.Open(FileMode.Append, FileAccess.Write);
            }
            catch (IOException e)
            {
                Console.WriteLine("Exception " + e.Message);
                this.CloseFiles();
            }

        }

        /// <summary>
        /// LoggerGen : Closes the output stream as well as file stream
        /// </summary>
        private void CloseFiles()
        {
            // Close the output stream and the file stream
            if (null != streamWriter)
                streamWriter.Close();

            if (null != fileStream)
                fileStream.Close();
        }


        /// <summary>
        /// LoggerGen : Adds a XML Tag Name and Value to the log file
        /// </summary>
        /// <param name="tagName">Name of the XML tag</param> 
        /// <param name="tagValue">Value of the XML tag</param> 
        /// 
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [SuppressMessage("Microsoft.Usage", "CA2200:Rethrowtopreservestackdetails")]
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        private void AddTag(string tagName, string tagValue)
        {
            try
            {
                // Result log in XML 
                if (isResultLogEnabled)
                {
                    if (string.Compare(tagName, "PLAIN", true, CultureInfo.InvariantCulture) != 0)
                    {
                        this.OpenFile(resultLogFile);
                        streamWriter = new StreamWriter(fileStream);
                        if (string.Compare(tagName, "TEXT", true, CultureInfo.InvariantCulture) == 0)
                            streamWriter.WriteLine(tagValue);
                        else if (string.Compare(tagName, "INFO", true, CultureInfo.InvariantCulture) == 0 ||
                            string.Compare(tagName, "PASS", true, CultureInfo.InvariantCulture) == 0 ||
                            string.Compare(tagName, "FAIL", true, CultureInfo.InvariantCulture) == 0 ||
                            string.Compare(tagName, "ERROR", true, CultureInfo.InvariantCulture) == 0 ||
                            string.Compare(tagName, "EXCEPTION", true, CultureInfo.InvariantCulture) == 0)
                        {
                            //streamWriter.WriteLine("\r<" + tagName + "><VALUE>" + DateTime.Now.ToString("HH:mm:ss.ff") + " - " +
                            //    tagValue.Replace("<", "\n&lt;").Replace(">", "&gt;").Replace("&", "&amp;").
                            //    Replace("\"", "&quot;").Replace("'", "&apos;") +
                            //    "</VALUE></" + tagName + ">");
                        }
                        else
                        {
                            streamWriter.WriteLine("<" + tagName + ">" +
                                tagValue.Replace("<", "\n&lt;").Replace(">", "&gt;").Replace("&", "&amp;").
                                Replace("\"", "&quot;").Replace("'", "&apos;") +
                                "</" + tagName + ">");
                            //streamWriter.WriteLine("\r<" + tagName + ">" +
                            //    tagValue.Replace("<", "\n&lt;").Replace(">", "&gt;").Replace("&", "&amp;").
                            //    Replace("\"", "&quot;").Replace("'", "&apos;") +
                            //    "</" + tagName + ">");
                        }
                        this.CloseFiles();
                    }
                }
                // Execution log in the text file
                if (isExecutionLogEnabled)
                {
                    ////if (string.Compare(tagName, "TEXT", true, CultureInfo.InvariantCulture) != 0 &&
                    ////    string.Compare(tagName, "TESTCASENUMBER", true, CultureInfo.InvariantCulture) != 0)
                    ////{
                    //this.OpenFile(executionLogFile);
                    //streamWriter = new StreamWriter(fileStream);
                    //streamWriter.WriteLine((tagName != "PLAIN" ?
                    //    DateTime.Now.ToString("HH:mm:ss.ff") + " - " + tagName + ": " : "") + tagValue);
                    //this.CloseFiles();
                    ////}

                    if (string.Compare(tagName, "PLAIN", true, CultureInfo.InvariantCulture) != 0)
                    {
                        this.OpenFile(executionLogFile);
                        streamWriter = new StreamWriter(fileStream);
                        if (string.Compare(tagName, "TEXT", true, CultureInfo.InvariantCulture) == 0)
                        {
                            tagValue = tagValue.Replace("ResultLog1.0.xslt", "ExecutionLog1.0.xsl");
                            streamWriter.WriteLine(tagValue);
                        }

                        else if (string.Compare(tagName, "INFO", true, CultureInfo.InvariantCulture) == 0 ||
                            string.Compare(tagName, "PASS", true, CultureInfo.InvariantCulture) == 0 ||
                            string.Compare(tagName, "FAIL", true, CultureInfo.InvariantCulture) == 0 ||
                            string.Compare(tagName, "ERROR", true, CultureInfo.InvariantCulture) == 0 ||
                            string.Compare(tagName, "EXCEPTION", true, CultureInfo.InvariantCulture) == 0)
                        {
                            streamWriter.WriteLine("<" + tagName + "><VALUE>" + DateTime.Now.ToString("HH:mm:ss.ff") + " - " +
                                tagValue.Replace("<", "\n&lt;").Replace(">", "&gt;").Replace("&", "&amp;").
                                Replace("\"", "&quot;").Replace("'", "&apos;") +
                                "</VALUE></" + tagName + ">");
                            //streamWriter.WriteLine("\r<" + tagName + "><VALUE>" + DateTime.Now.ToString("HH:mm:ss.ff") + " - " +
                            //    tagValue.Replace("<", "\n&lt;").Replace(">", "&gt;").Replace("&", "&amp;").
                            //    Replace("\"", "&quot;").Replace("'", "&apos;") +
                            //    "</VALUE></" + tagName + ">");
                        }
                        else
                        {
                            streamWriter.WriteLine("<" + tagName + ">" +
                                tagValue.Replace("<", "\n&lt;").Replace(">", "&gt;").Replace("&", "&amp;").
                                Replace("\"", "&quot;").Replace("'", "&apos;") +
                                "</" + tagName + ">");

                            //streamWriter.WriteLine("\r<" + tagName + ">" +
                            //    tagValue.Replace("<", "\n&lt;").Replace(">", "&gt;").Replace("&", "&amp;").
                            //    Replace("\"", "&quot;").Replace("'", "&apos;") +
                            //    "</" + tagName + ">");
                        }
                        this.CloseFiles();
                    }

                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("IOException occured: " + ex);
                this.CloseFiles();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occured: " + ex.Message);
                this.CloseFiles();
            }
        }

        #endregion

        #region Public Methods

        #region 01. Start
        /// <summary>
        /// LoggerGen : Starts the logging activity by adding the initial log to the log file
        /// </summary>
        /// <param name="testCaseInfo">Test Case Info</param>
        public void Start(string testCaseNumber, string testCaseName, string testcasePriority)
        {
            startTime = DateTime.Now;

            // Remove xml end tags
            int tcCounter = 1;
            finalResult = true;
            if (File.Exists(resultLogFile))
            {
                string tmpFile = Path.Combine(Environment.GetEnvironmentVariable("TMP"), "tmpfile");
                StreamWriter sw = new StreamWriter(tmpFile, false);
                StreamReader sr = new StreamReader(resultLogFile);
                string line = string.Empty;
                string previous_line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Equals("")) continue;
                    if (line.Equals("<TESTCASE>"))
                    {
                        if (!previous_line.Equals("</TESTCASE>") && !previous_line.Equals("</TESTCASESUMMARY>"))
                            sw.WriteLine("</TESTCASE>");
                    }

                    if (!line.Equals("</TESTCASES>"))
                        sw.WriteLine(line);

                    if (line.Equals("</TESTCASE>"))
                        tcCounter++;

                    // If log doesn't end with </TESTCASE>, add Test Case ending.
                    if (sr.Peek() == -1)
                    {
                        if (!line.Equals("</TESTCASES>") && !previous_line.Equals("</TESTCASE>"))
                        {
                            sw.WriteLine("<FAIL><VALUE>" + "\nTest Execution interrupted in middle by user.\n" + "</VALUE></FAIL>");
                            sw.WriteLine("<FINISHTIME>" + startTime.ToString("G", CultureInfo.InvariantCulture) +
                                "</FINISHTIME>");
                            sw.WriteLine("</TESTCASE>");
                            //sw.WriteLine("\r<FAIL><VALUE>" + "\nTest Execution interrupted in middle by user.\n" + "</VALUE></FAIL>");
                            //sw.WriteLine("\r<FINISHTIME>" + startTime.ToString("G", CultureInfo.InvariantCulture) +
                            //    "</FINISHTIME>");
                            //sw.WriteLine("</TESTCASE>");

                        }
                    }
                    previous_line = line;
                }
                sw.Close();
                sr.Close();
                File.Delete(resultLogFile);
                File.Move(tmpFile, resultLogFile);
            }
            if (File.Exists(executionLogFile))
            {
                string tmpFile = Path.Combine(Environment.GetEnvironmentVariable("TMP"), "tmpfile");
                StreamWriter executionlog_sw = new StreamWriter(tmpFile, false);
                StreamReader executionlog_sr = new StreamReader(executionLogFile);
                string line = string.Empty;
                string previous_line = string.Empty;
                while ((line = executionlog_sr.ReadLine()) != null)
                {
                    if (line.Equals("")) continue;
                    if (line.Equals("<TESTCASE>"))
                    {
                        if (!previous_line.Equals("</TESTCASE>") && tcCounter < 1)
                            executionlog_sw.WriteLine("</TESTCASE>");
                    }

                    if (line.Contains("ResultLog1.0.xslt"))
                        executionlog_sw.WriteLine("<?xml version=\"1.0\"?><?xml-stylesheet type=\"text/xsl\" href=\"ExecutionLog1.0.xsl\"?>");

                    if (!line.Equals("</TESTCASES>"))
                        executionlog_sw.WriteLine(line);

                    if (line.Equals("</TESTCASE>"))
                        tcCounter++;

                    // If log doesn't end with </TESTCASE>, add Test Case ending.
                    if (executionlog_sr.Peek() == -1)
                    {
                        if (!line.Equals("</TESTCASES>") && !previous_line.Equals("</TESTCASE>"))
                        {
                            executionlog_sw.WriteLine("\r<FAIL><VALUE>" + "\nTest Execution interrupted in middle by user.\n" + "</VALUE></FAIL>");
                            executionlog_sw.WriteLine("\r<FINISHTIME>" + startTime.ToString("G", CultureInfo.InvariantCulture) +
                                "</FINISHTIME>");
                            executionlog_sw.WriteLine("</TESTCASE>");

                        }
                    }
                    previous_line = line;
                }
                executionlog_sw.Close();
                executionlog_sr.Close();
                File.Delete(executionLogFile);
                File.Move(tmpFile, executionLogFile);
            }
            else
            {
                // Create a fresh xml
                this.AddTag("TEXT", "<?xml version=\"1.0\"?><?xml-stylesheet type=\"text/xsl\" href=\"ResultLog1.0.xslt\"?>");
                this.AddTag("TEXT", "<TESTCASES>");
            }

            this.AddTag("TEXT", "<TESTCASE>");
            this.AddTag("PLAIN", "========================================================================");
            this.AddTag("SRNUMBER", testCaseNumber);
            this.AddTag("TESTCASENUMBER", testCaseNumber);
            this.AddTag("TESTCASETTITLE", testCaseName);
            this.AddTag("TESTCASEPRIORITY", testcasePriority);
            this.AddTag("TESTCASESTARTTIME", startTime.ToString("G", CultureInfo.InvariantCulture));
            //this.AddTag("SERVICENAME", serviceName);
            //this.AddTag("OPERATIONNAME", operationName);
        }

        /// <summary>
        /// LoggerGen : Starts the logging activity by adding the initial log to the log file
        /// </summary>
        /// <param name="testCaseInfo">Test Case Info</param>
        public void Start(string testCaseNumber, string testCaseName)
        {
            startTime = DateTime.Now;

            // Remove xml end tags
            int tcCounter = 1;
            finalResult = true;
            if (File.Exists(resultLogFile))
            {
                string tmpFile = Path.Combine(Environment.GetEnvironmentVariable("TMP"), "tmpfile");
                StreamWriter sw = new StreamWriter(tmpFile, false);
                StreamReader sr = new StreamReader(resultLogFile);
                string line = string.Empty;
                string previous_line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Equals("")) continue;
                    if (line.Equals("<TESTCASE>"))
                    {
                        if (!previous_line.Equals("</TESTCASE>") && !previous_line.Equals("</TESTCASESUMMARY>"))
                            sw.WriteLine("</TESTCASE>");
                    }

                    if (!line.Equals("</TESTCASES>"))
                        sw.WriteLine(line);

                    if (line.Equals("</TESTCASE>"))
                        tcCounter++;

                    // If log doesn't end with </TESTCASE>, add Test Case ending.
                    if (sr.Peek() == -1)
                    {
                        if (!line.Equals("</TESTCASES>") && !previous_line.Equals("</TESTCASE>"))
                        {
                            sw.WriteLine("\r<FAIL><VALUE>" + "\nTest Execution interrupted in middle by user.\n" + "</VALUE></FAIL>");
                            sw.WriteLine("\r<FINISHTIME>" + startTime.ToString("G", CultureInfo.InvariantCulture) +
                                "</FINISHTIME>");
                            sw.WriteLine("</TESTCASE>");

                        }
                    }
                    previous_line = line;
                }
                sw.Close();
                sr.Close();
                File.Delete(resultLogFile);
                File.Move(tmpFile, resultLogFile);
            }
            if (File.Exists(executionLogFile))
            {
                string tmpFile = Path.Combine(Environment.GetEnvironmentVariable("TMP"), "tmpfile");
                StreamWriter executionlog_sw = new StreamWriter(tmpFile, false);
                StreamReader executionlog_sr = new StreamReader(executionLogFile);
                string line = string.Empty;
                string previous_line = string.Empty;
                while ((line = executionlog_sr.ReadLine()) != null)
                {
                    if (line.Equals("")) continue;
                    if (line.Equals("<TESTCASE>"))
                    {
                        if (!previous_line.Equals("</TESTCASE>") && tcCounter < 1)
                            executionlog_sw.WriteLine("</TESTCASE>");
                    }

                    if (line.Contains("ResultLog1.0.xslt"))
                        executionlog_sw.WriteLine("<?xml version=\"1.0\"?><?xml-stylesheet type=\"text/xsl\" href=\"ExecutionLog1.0.xsl\"?>");

                    if (!line.Equals("</TESTCASES>"))
                        executionlog_sw.WriteLine(line);

                    if (line.Equals("</TESTCASE>"))
                        tcCounter++;

                    // If log doesn't end with </TESTCASE>, add Test Case ending.
                    if (executionlog_sr.Peek() == -1)
                    {
                        if (!((line.Equals("</TESTCASES>") && previous_line.Equals("</TESTCASE>")) || line.Equals("</PreRequisite>")))
                        {
                            executionlog_sw.WriteLine("\r<FAIL><VALUE>" + "\nTest Execution interrupted in middle by user.\n" + "</VALUE></FAIL>");
                            executionlog_sw.WriteLine("\r<FINISHTIME>" + startTime.ToString("G", CultureInfo.InvariantCulture) +
                                "</FINISHTIME>");
                            executionlog_sw.WriteLine("</TESTCASE>");
                        }
                    }
                    previous_line = line;
                }
                executionlog_sw.Close();
                executionlog_sr.Close();
                File.Delete(executionLogFile);
                File.Move(tmpFile, executionLogFile);
            }
            else
            {
                // Create a fresh xml
                this.AddTag("TEXT", "<?xml version=\"1.0\"?><?xml-stylesheet type=\"text/xsl\" href=\"ResultLog1.0.xslt\"?>");
                this.AddTag("TEXT", "<TESTCASES>");
            }

            this.AddTag("TEXT", "<TESTCASE>");
            this.AddTag("PLAIN", "========================================================================");
            this.AddTag("SRNUMBER", testCaseNumber);
            this.AddTag("TESTCASENUMBER", testCaseNumber);
            this.AddTag("TESTCASETTITLE", testCaseName);
            this.AddTag("TESTCASESTARTTIME", startTime.ToString("G", CultureInfo.InvariantCulture));
            //this.AddTag("SERVICENAME", serviceName);
            //this.AddTag("OPERATIONNAME", operationName);
        }

        public void LogTitle(string testSuiteName)
        {
            startTime = DateTime.Now;
            this.AddTag("TEXT", "<?xml version=\"1.0\"?><?xml-stylesheet type=\"text/xsl\" href=\"ResultLog1.0.xslt\"?>");

            this.AddTag("TEXT", "<TESTCASES>");
            this.AddTag("TEXT", "<TestExecutionDetails>");
            this.AddTag("TESTSUITENAME", testSuiteName);
            this.AddTag("TESTEXECUTIONDATE", startTime.ToString("MM/dd/yyyy"));
            this.AddTag("TESTEXECUTIONSTARTTIME", startTime.ToString("HH:mm:ss"));
            this.AddTag("TEXT", "</TestExecutionDetails>");
        }

        public void LogStartPreRequisite()
        {
            this.AddTag("TEXT", "<PreRequisite>");
        }
        public void LogEndPreRequisite()
        {
            this.AddTag("TEXT", "</PreRequisite>");
        }
        #endregion

        #region 02. LogInfo
        /// <summary>
        /// LoggerGen : Adds the specified line to the log file
        /// </summary>
        /// <param name="line">Line to log</param>
        public void LogInfo(string line)
        {
            try
            {
                this.AddTag("INFO", line);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Exception " + ex);
                this.CloseFiles();
            }
        }

        public void LogInfo(bool result, string line)
        {
            try
            {
                if (result)
                    this.AddTag("INFO", "PASS: " + line);
                else
                {
                    this.AddTag("INFO", "FAIL: " + line);                         
                }
                    
                    
            }
            catch (IOException ex)
            {
                Console.WriteLine("Exception " + ex);
                this.CloseFiles();
            }
        }

        /// <summary>
        /// Add Data Row 
        /// </summary>
        /// <param name="row"></param>
        public void AddDataRow(string dataId)
        {
            try
            {
                dataRowId = dataId;
                dataResult = true;
                this.AddTag("TEXT", "<DATAID>");
                this.AddTag("TEXT", "<ID>" + dataRowId + "</ID>");
            }
            catch (IOException ex)
            {
                Console.WriteLine("Exception " + ex);
                this.CloseFiles();
            }
        }

        /// <summary>
        /// This function adds the result of particuler data in tag in DATAID tag
        /// </summary>
        public void AddDataResult()
        {
            try
            {
                if (dataResult == true)
                    this.AddTag("TEXT", "<RESULT>PASS</RESULT>");
                else
                    this.AddTag("TEXT", "<RESULT>FAIL</RESULT>");

                this.AddTag("TEXT", "</DATAID>");
            }
            catch (IOException ex)
            {
                Console.WriteLine("Exception " + ex);
                this.CloseFiles();
            }
        }

        #endregion

        #region 03. LogError
        /// <summary>
        /// LoggerGen : Logs error message with ERROR tag
        /// </summary>
        /// <param name="line">Line to log</param>
        public void LogError(string line)
        {
            try
            {
                isErrorFound = true;
                this.AddTag("ERROR", line);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Exception " + ex);
                this.CloseFiles();
            }
        }

        #endregion

        #region 04. LogException
        /// <summary>
        /// LoggerGen : Logs the exception generated in to the log file
        /// </summary>
        /// <param name="ex">Exception to be logged</param>
        public void LogException(Exception ex)
        {
            this.AddTag("EXCEPTION", "Exception: " + ex.Message);
            if (null != ex.InnerException)
                this.AddTag("EXCEPTION", "InnerException: " + ex.InnerException.Message);
        }
        #endregion

        #region 05. LogResult
        /// <summary>
        /// LoggerGen : Logs the result of test case
        /// </summary>
        /// <param name="result">Result of the test case</param>
        /// <param name="description">Description of the test case</param>
        public void LogResult(bool result, string description)
        {
            // Log the test case result to the log file
            DateTime finishTime = DateTime.Now;
            if (isErrorFound)
            {
                this.AddTag("FAIL", description + " - " + (!description.StartsWith("Error") ? " could not be verified." : ""));
                isErrorFound = false;
                dataResult = false;
                finalResult = false;
            }
            else
            {
                if (result)
                    this.AddTag("PASS", description + " verified.");
                else
                {
                    this.AddTag("FAIL", description + " - " + (!description.StartsWith("Error") ? " could not be verified." : ""));
                    dataResult = false;
                    finalResult = false;
                }
            }
        }
        #endregion

        #region 06. End
        /// <summary>
        /// LoggerGen : Ends the logging activity by adding necessary tags to the log file
        /// </summary>
        /// 
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [SuppressMessage("Microsoft.Usage", "CA2200:Rethrowtopreservestackdetails")]
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public void EndLog()
        {
            try
            {
                string componentName = new System.Diagnostics.StackTrace().GetFrames()[1].GetMethod().DeclaringType.FullName.Split('.')[1];
                componentName = Path.GetFileNameWithoutExtension(componentName);
                DateTime finishTime = DateTime.Now;
                if (finalResult == true)
                    this.AddTag("TEXT", "<TESTCASERESULT>PASS</TESTCASERESULT>");
                else
                    this.AddTag("TEXT", "<TESTCASERESULT>FAIL</TESTCASERESULT>");

                this.AddTag("TESTCASEEXECUTIONTIME", (finishTime - startTime).ToString() + " (hh:mm:ss.MSec)");
                this.AddTag("TESTCASEFINISHTIME", finishTime.ToString("G", CultureInfo.InvariantCulture));
                this.AddTag("TEXT", "</TESTCASE>");
                this.AddTag("TEXT", "</TESTCASES>");
                #region Add Test case execution summary in the log
                if (File.Exists(resultLogFile))
                {
                    // Figure out PASS,FAIL,ERROR and EXCEPTION test cases in the existing log
                    int tcCounter = 0;
                    int passCounter = 0;
                    int failCounter = 0;
                    int errorCounter = 0;
                    int exceptionCounter = 0;
                    // Set to maximum
                    DateTime firstTcStartTime = DateTime.MaxValue;
                    TimeSpan totalExecutionTime = TimeSpan.Zero;

                    StreamReader sr = new StreamReader(resultLogFile);
                    string line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Equals("")) continue;
                        else if (line.Equals("</TESTCASE>"))
                            tcCounter++;
                        else if (line.StartsWith("<TESTCASERESULT>"))
                        {
                            if (line.IndexOf("PASS") > 0)
                                passCounter++;
                            else if (line.IndexOf("FAIL") > 0)
                                failCounter++;
                        }
                        else if (line.StartsWith("<ERROR>"))
                            errorCounter++;
                        else if (line.StartsWith("<EXCEPTION>"))
                            exceptionCounter++;
                        else if (line.IndexOf("<EXECUTIONTIME>") > -1)
                        {
                            string[] tmp = line.Split('>');
                            tmp = tmp[1].Split('<');
                            tmp = tmp[0].Split(' ');
                            TimeSpan currentTimeSpan = TimeSpan.Parse(tmp[0]);
                            totalExecutionTime += currentTimeSpan;
                        }
                    }
                    sr.Close();

                    // Now add the counts in the log file
                    string tmpFile = Path.Combine(Environment.GetEnvironmentVariable("TMP"), "tmpfile");
                    StreamWriter sw = new StreamWriter(tmpFile, false);
                    sr = new StreamReader(resultLogFile);
                    line = string.Empty;
                    string f = componentName;
                    string[] strTmp = null;
                    if (f.IndexOf("_") > -1)
                        strTmp = f.Split('_');

                    while ((line = sr.ReadLine()) != null)
                    {
                        sw.WriteLine(line);
                        if (line.Equals("<TESTCASES>") || line.Equals("<TestExecutionDetails>"))
                        {
                            sw.WriteLine("<TESTCASESUMMARY>");
                            if (strTmp != null)
                            {
                                f = string.Empty;
                                foreach (string t in strTmp)
                                {
                                    if (t.IndexOf("TestExecution") > -1)
                                        f += t.Substring("TestExecution".Length);
                                    else
                                        f += t;

                                    f += " ";
                                }
                            }

                            sw.WriteLine("<COMPONENTNAME>" + f.Trim() + "</COMPONENTNAME>");
                            sw.WriteLine("<DATETIME>" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss") + "</DATETIME>");
                            sw.WriteLine("<TOTAL>" + Convert.ToInt32(tcCounter) + "</TOTAL>");
                            sw.WriteLine("<ALLPASS>" + Convert.ToInt32(passCounter) + "</ALLPASS>");
                            sw.WriteLine("<ALLFAIL>" + Convert.ToInt32(failCounter) + "</ALLFAIL>");
                            sw.WriteLine("<ALLERROR>" + Convert.ToInt32(errorCounter) + "</ALLERROR>");
                            sw.WriteLine("<ALLEXCEPTION>" + Convert.ToInt32(exceptionCounter) + "</ALLEXCEPTION>");
                            sw.WriteLine("<TOTALEXECUTION>" + totalExecutionTime.ToString() + " (hh:mm:ss.MSec)" + "</TOTALEXECUTION>");
                            sw.WriteLine("<BUILDVersion>" + buildNumber + "</BUILDVersion>");
                            sw.WriteLine("<SITEVersion>" + buildNumber + "</SITEVersion>");
                            //sw.WriteLine("<BINDING>" + bindingName + "</BINDING>");
                            sw.WriteLine("<HOSTPLATFORM>" + hostPlatform + "</HOSTPLATFORM>");
                            sw.WriteLine("<CLIENTPLATFORM>" + clientPlatform + "</CLIENTPLATFORM>");
                            sw.WriteLine("<HOSTIP>" + hostIp + "</HOSTIP>");
                            sw.WriteLine("<CLIENTIP>" + clientIp + "</CLIENTIP>");
                            sw.WriteLine("</TESTCASESUMMARY>");
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (line.Equals("<TESTCASE>"))
                                {
                                    sw.WriteLine(line);
                                    break;
                                }
                                else
                                    continue;
                            }
                        }
                    }
                    sw.Close();
                    sr.Close();
                    File.Delete(resultLogFile);
                    File.Move(tmpFile, resultLogFile);
                }

                #endregion
                this.AddTag("PLAIN", "========================================================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

        #endregion

        #region Destructor

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        private void Dispose(bool disposing)
        {
            // If disposing equals true, dispose all managed 
            // and unmanaged resources.
            if (disposing)
            {
                // Dispose managed resources.
            }

            // Call the appropriate methods to clean up 
            // unmanaged resources here.
            // If disposing is false, 
            // only the following code is executed.
            if (null != streamWriter)
            {
                streamWriter.Close();
            }
            if (null != fileStream)
            {
                fileStream.Close();
            }
        }


        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// LoggerGen : Destructor for the LogFile class
        /// </summary>
        ~Logs()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }
        #endregion
    }
}

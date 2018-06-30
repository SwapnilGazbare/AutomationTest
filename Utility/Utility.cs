using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace GI.Automation.Utility
{
    public static class Utility
    {
        #region STRING UTILITY
        /// <summary>
        /// This function is returns a string containing a specified number of characters from the left side of a string.
        /// </summary>
        /// <param name="myString"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Left(this string myString, int length)
        {
            string tmpString = myString.Substring(0, length);
            return tmpString;
        }

        /// <summary>
        /// This function returns a string containing a specified number of characters from the left side of a string.
        /// </summary>
        /// <param name="myString"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Right(this string myString, int length)
        {
            string tmpString = myString.Substring(myString.Length - length, length);
            return tmpString;
        }

        /// <summary>
        /// This function returns a string containing a specified number of characters from a string.
        /// </summary>
        /// <param name="myString"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Mid(this string myString, int startIndex, int length)
        {
            string tmpString = myString.Substring(startIndex, length);
            return tmpString;
        }

        /// <summary>
        /// This function returns a string containing a specified number of characters from a string.
        /// </summary>
        /// <param name="myString"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static string Mid(this string myString, int startIndex)
        {
            string tmpString = myString.Substring(startIndex);
            return tmpString;
        }
        #endregion

        #region IO UTILITY
        /// <summary>
        /// This function creates directory if it is not there in disk
        /// </summary>
        /// <param name="pathIn"></param>
        /// <returns></returns>
        /// 
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [SuppressMessage("Microsoft.Usage", "CA2200:Rethrowtopreservestackdetails")]
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static bool MakeDirectory(string directoryPath)
        {
            bool status = true;

            try
            {
                Directory.CreateDirectory(directoryPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception occured in making directory {0} : Exception : {1}", directoryPath, ex.Message));
                status = false;
            }
            return status;
        }

        /// <summary>
        /// Gives fully qualified path input path
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns>string</returns>
        public static string GetProperFolderName(string folderPath)
        {
            string actualFolderPath;
            if (folderPath.Trim() == String.Empty)
                actualFolderPath = Environment.CurrentDirectory.ToString();
            else
                if (folderPath.IndexOf(":") == -1)
                    actualFolderPath = Path.Combine(Environment.CurrentDirectory.ToString(), folderPath);
                else
                    actualFolderPath = folderPath;

            return actualFolderPath;
        }

        /// <summary>
        /// This function give the file name for log
        /// </summary>
        /// <param name="fileNameWithPath"></param>
        /// <returns></returns>
        public static string GetNextFileName(string fileNameWithPath)
        {
            //Verify the passed string is not blank
            if (fileNameWithPath.Trim() == String.Empty)
                return String.Empty;

            //Get Next file Name
            else
            {
                FileInfo logFileInfo = new FileInfo(fileNameWithPath);
                string tempFileName = logFileInfo.Name;
                string fileNumber;
                int cntr = 0;
                int fileCntr = 0;
                bool isNum;
                string directoryName = logFileInfo.DirectoryName;
                DirectoryInfo logDirectoryInfo = new DirectoryInfo(directoryName);
                //Added to verify the directory 
                if (!logDirectoryInfo.Exists) { logDirectoryInfo.Create(); }

                foreach (FileInfo fileInfo in logDirectoryInfo.GetFiles(tempFileName + "*.*"))
                {
                    fileNumber = fileInfo.Name.Substring(tempFileName.Length + 1, fileInfo.Name.Length - tempFileName.Length - fileInfo.Extension.Length - 1);
                    isNum = int.TryParse(fileNumber, out fileCntr);
                    if (isNum == true)
                    {
                        if (fileCntr > cntr)
                        {
                            cntr = fileCntr;
                        }
                    }
                }
                tempFileName = directoryName + "\\" + tempFileName + "_" + Convert.ToString(cntr + 1);
                return tempFileName;
            }
        }


        #endregion

        #region Utility Functions

        /// <summary>
        /// Purpose : This function converts the string to byte array 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetByteArrayFromString(string data)
        {
            System.Text.Encoding encoder = Encoding.GetEncoding("ISO-8859-1");
            byte[] value = encoder.GetBytes(data);
            return value;
        }

        /// <summary>
        /// Purpose : This function converts the byte array to string 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetStringFromByteArray(byte[] data)
        {
            System.Text.Encoding encoder = Encoding.GetEncoding("ISO-8859-1");
            string value = encoder.GetString(data);
            encoder = null;
            return value;
        }

        /// <summary>
        /// Purpose : This function compare the byte array and result the compare status
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <returns></returns>
        public static bool CompareByteArrays(byte[] data1, byte[] data2)
        {
            // If both are null, they're equal
            if (data1 == null && data2 == null)
            {
                return true;
            }
            // If either but not both are null, they're not equal
            if (data1 == null || data2 == null)
            {
                return false;
            }
            if (data1.Length != data2.Length)
            {
                return false;
            }
            for (int i = 0; i < data1.Length; i++)
            {
                if (data1[i] != data2[i])
                {
                    return false;
                }
            }
            return true;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [SuppressMessage("Microsoft.Usage", "CA2200:Rethrowtopreservestackdetails")]
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static string GetCurrentTestCaseName()
        {
            string retVal = "";
            try
            {
                // Get Current Test Case Name from Stack
                StackTrace stackTrace = new StackTrace();
                // Get First frame
                StackFrame stackFrame = stackTrace.GetFrame(1);
                // Get Method
                MethodBase methodBase = stackFrame.GetMethod();
                // Return method name
                retVal = methodBase.Name;
            }
            catch (Exception)
            {
                retVal = "Unknown TestCase Name";
                throw;
            }
            return retVal;
        }


        #endregion

    }
}

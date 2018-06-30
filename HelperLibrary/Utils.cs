using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace HelperLibrary
{
    public class Utils
    {
        public static string inputDataFilePath = string.Empty;
        public static string inputDataFolderPath = string.Empty;
        public static System.Reflection.Assembly assembly;
        public static string dateTimeStamp;// = DateTime.Now.ToString("_yyMMdd_HHmm");
        public static IWebDriver driver;
        //public static ChromeDriver Chromedriver;
        //public static FirefoxDriver firefoxDriver;
        public static WebDriverWait wait;
        public static System.Resources.ResourceWriter resourceWriter = new System.Resources.ResourceWriter(string.Format("{0}.InputDataFiles.RunTimeData", ""));
        public static InternetExplorerOptions options = new InternetExplorerOptions();

        static Utils()
        {
            dateTimeStamp = DateTime.Now.ToString("_yyMMdd_HHmm");
        }

        /// <summary>
        /// Method to get Internet Explorer drivers
        /// </summary>
        /// <returns></returns>
        public static IWebDriver GetInternetExplorerDriver()
        {
            KillProcessByName("IExplore");
            KillProcessByName("IEDriverServer");

            var options = new InternetExplorerOptions
            {
                RequireWindowFocus = false,
                EnableNativeEvents = false,
                IgnoreZoomLevel = true,
                IntroduceInstabilityByIgnoringProtectedModeSettings = true
            };
            options.AddAdditionalCapability("disable-popup-blocking", true);
            TimeSpan timeout = new TimeSpan(1200000000);

            var driverService = InternetExplorerDriverService.CreateDefaultService(Config.SolutionPath + @"\packages\Selenium.WebDriver.IEDriver.3.11.1\driver\");
            driverService.HideCommandPromptWindow = true;

            Utils.driver = new InternetExplorerDriver(driverService, options, timeout);

            return Utils.driver;
        }

        /// <summary>
        /// Method to get Internet Explorer drivers
        /// </summary>
        /// <returns></returns>
        public static IWebDriver GetChromeDriver()
        {
            KillProcessByName("chrome");
            var options = new ChromeOptions
            {
                BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
            };
            options.AddArguments("--disable-notifications");
            TimeSpan timeout = new TimeSpan(1200000000);

            var driverService = ChromeDriverService.CreateDefaultService(Config.SolutionPath + @"\packages\Selenium.WebDriver.ChromeDriver.2.40.0\driver\win32\");
            driverService.HideCommandPromptWindow = true;

            Utils.driver = new ChromeDriver(driverService, options, timeout);
            return Utils.driver;
        }

        /// <summary>
        /// Method to get Internet Explorer drivers
        /// </summary>
        /// <returns></returns>
        public static IWebDriver GetFirefoxDriver()
        {
            KillProcessByName("firefox");
            System.Environment.SetEnvironmentVariable("webdriver.gecko.driver", @"D:\GIAutomationNew\PuneAutomationTeam\GIAutomation\packages\Selenium.WebDriver.GeckoDriver.Win64.0.18.0\driver\geckodriver.exe");
            var driverService = FirefoxDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            driverService.FirefoxBinaryPath = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
            Utils.driver = new FirefoxDriver(driverService);
            return Utils.driver;
        }

        /// <summary>
        /// Do cleanup of the driver created
        /// </summary>
        public static void Cleanup()
        {
            if (driver != null)
            {
                driver.Dispose();
                driver.Quit();
                driver = null;
                GC.Collect();
            }
        }
        /// <summary>
        ///Kills all the processes for the given name.
        /// </summary>
        /// <param name="ProcessName"></param>
        /// <returns>bool</returns>
        public static bool KillProcessByName(string processName)
        {
            // Close all Internet Explorer Instances.
            System.Diagnostics.Process[] procs;
            procs = System.Diagnostics.Process.GetProcessesByName(processName);

            foreach (System.Diagnostics.Process proc in procs)
            {
                try
                {
                    proc.Kill();
                }
                catch
                {
                }
            }

            procs = System.Diagnostics.Process.GetProcessesByName(processName);
            if (procs.Length == 0)
                return true;
            else
                return false;
        }

        public static string AppendDateTime(string identifier)
        {
            return string.Concat(identifier, dateTimeStamp);
        }


        /// <summary>
        /// Select combo box value based on Value
        /// </summary>
        /// <param name="element">Combo box web element</param>
        /// <param name="item">Value to be selected in combo box</param>
        public static void SelectItemByValue(IWebElement element, string value)
        {
            Thread.Sleep(500);
            //Utils.driver.("arguments[0].scrollIntoView(true);", element);
            SelectElement selector = new SelectElement(element);
            selector.SelectByValue(value);
        }



        /// <summary>
        /// Action to perform Right Click
        /// </summary>
        /// <param name="element">element to be right clicked</param>
        public static void RightClick(IWebElement element)
        {
            IWebDriver driver = ((OpenQA.Selenium.Internal.IWrapsDriver)element).WrappedDriver;
            var action = new OpenQA.Selenium.Interactions.Actions(driver);
            action.ContextClick(element);
            action.Perform();
            action.ContextClick().Perform();
        }

        public static void DoubleClick(IWebElement element)
        {
            Actions action = new Actions(driver).DoubleClick(element);
            action.Build().Perform();
        }

        public static void SelectRadioButton(string value, IList<IWebElement> radiobuttons)
        {
            for (int i = 0; i < radiobuttons.Count; i++)
            {
                string UIvalue = radiobuttons[i].GetAttribute("value");
                if (UIvalue.Equals(value))
                {
                    radiobuttons[i].Click();
                    break;
                }
            }
        }
    }

    public static class GenericMethods
    {
        /// <summary>
        /// Extension method of IWebelement class to set text box value
        /// </summary>
        /// <param name="element">Text webelement</param>
        /// <param name="value">Text value to be set</param>
        public static void SetText(this IWebElement element, string value)
        {
            try
            {
                //Sendkeys using selenium
                element.Clear();
                element.SendKeys(value);

                //Sendkeys Using Javascripts
                if (!GetValue(element).Equals(value))
                {
                    element.Clear();
                    IJavaScriptExecutor executor = (IJavaScriptExecutor)Utils.driver;
                    executor.ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2]);", element, "value", value);
                }

                //Sendkeys using Action Class
                if (!GetValue(element).Equals(value))
                {
                    Actions act = new Actions(Utils.driver);
                    element.Clear();
                    act.SendKeys(element, value).Build().Perform();
                }
            }
            catch (Exception)
            {
            }

        }

        public static void SendChar(this IWebElement element, string value)
        {
            element.Clear();
            element.Click();
            char[] c = value.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                element.SendKeys(c[i].ToString());
                element.SendKeys(Keys.End);
            }
        }

        public static string GetValue(this IWebElement element)
        {
            Thread.Sleep(500);
            return element.GetAttribute("value");
        }

        /// <summary>
        /// Extension method of IWebelement class to perform click action
        /// </summary>
        /// <param name="element">Web element</param>
        public static void DoClick(this IWebElement element)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)Utils.driver;
                js.ExecuteScript("arguments[0].click();", element);
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// Select combo box value based on index number
        /// </summary>
        /// <param name="element">Combo box web element</param>
        /// <param name="index">Index number to be selected in combo box</param>
        public static void SelectItem(this IWebElement element, string value)
        {
            Thread.Sleep(500);
            IJavaScriptExecutor js = (IJavaScriptExecutor)Utils.driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            SelectElement selector = new SelectElement(element);
            selector.SelectByText(value);
        }
    }
}

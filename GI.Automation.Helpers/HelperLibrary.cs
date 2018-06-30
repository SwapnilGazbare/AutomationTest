using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
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
using System.Windows.Automation;
using System.Windows.Forms;
using System.Xml;

namespace Automation.Helpers
{
    /// <summary>
    /// This class contains generic methods to be consumed by Page Entities
    /// </summary>
    public static class HelperLibrary
    {
        public static System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo(Config.Language);
        public static System.Reflection.Assembly assembly;
        public static string inputDataFilePath = string.Empty;
        public static string inputDataFolderPath = string.Empty;
        public static string dateTimeStamp;// = DateTime.Now.ToString("_yyMMdd_HHmm");
        public static InternetExplorerDriver driver;
        public static IWebDriver ChromeFirefoxdriver;
        public static WebDriverWait wait;
        public static System.Resources.ResourceWriter resourceWriter = new System.Resources.ResourceWriter(string.Format("{0}.InputDataFiles.RunTimeData", Config.ProjectName));
        public static InternetExplorerOptions options = new InternetExplorerOptions();

        static HelperLibrary()
        {
            dateTimeStamp = DateTime.Now.ToString("_yyMMdd_HHmm");
        }

        /// <summary>
        /// Method to get Internet Explorer drivers
        /// </summary>
        /// <returns></returns>
        public static InternetExplorerDriver GetInternetExplorerDriver()
        {
            KillProcessByName("IExplore");
            KillProcessByName("IEDriverServer");

            var options = new InternetExplorerOptions
            {
                RequireWindowFocus = false,
                EnableNativeEvents = false,
                IgnoreZoomLevel = true,
                UnexpectedAlertBehavior = InternetExplorerUnexpectedAlertBehavior.Accept,
                IntroduceInstabilityByIgnoringProtectedModeSettings = true
            };
            options.AddAdditionalCapability("disable-popup-blocking", true);
            TimeSpan timeout = new TimeSpan(1200000000);

            var driverService = InternetExplorerDriverService.CreateDefaultService(@Config.BrowserDrivers);
            driverService.HideCommandPromptWindow = true;

            HelperLibrary.driver = new InternetExplorerDriver(driverService, options, timeout);

            return HelperLibrary.driver;
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
                BinaryLocation = @"C:\Program Files (x86)\Google\Chrome Beta\Application\chrome.exe"
            };
            TimeSpan timeout = new TimeSpan(1200000000);

            var driverService = ChromeDriverService.CreateDefaultService(@Config.BrowserDrivers);
            driverService.HideCommandPromptWindow = true;

            HelperLibrary.ChromeFirefoxdriver = new ChromeDriver(@"D:\GIAutomationNew\PuneAutomationTeam\GIAutomation\packages\Selenium.Chrome.WebDriver.2.30\driver", options);
            return HelperLibrary.driver;
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
            HelperLibrary.ChromeFirefoxdriver = new FirefoxDriver(driverService);
            return HelperLibrary.driver;
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

        /// <summary>
        /// Wait for the required process to load and open
        /// </summary>
        /// <param name="processName">Process name</param>
        /// <param name="windowName">Window title of the process</param>
        /// <returns></returns>
        public static bool WaitForProcess(string processName, string windowName)
        {
            System.Threading.Thread.Sleep(1000);
            if (Path.HasExtension(windowName))
            {
                string[] filePath = windowName.Split('.');
                windowName = filePath[0];
            }

            TimeSpan initialTicks = new TimeSpan(DateTime.Now.Ticks);
            double initialSeconds = initialTicks.TotalSeconds;

            System.Diagnostics.Process[] procs;
            bool processFound = false;
            do
            {
                TimeSpan currentTicks = new TimeSpan(DateTime.Now.Ticks);
                double currentSeconds = currentTicks.TotalSeconds;

                if (currentSeconds - initialSeconds <= int.Parse(Config.WaitTime))
                {
                    procs = System.Diagnostics.Process.GetProcessesByName(processName);
                    foreach (System.Diagnostics.Process proc in procs)
                    {
                        if (proc.MainWindowTitle.Contains(windowName))
                        {
                            processFound = true;
                            break;
                        }
                    }
                    System.Threading.Thread.Sleep(200);
                }
                else
                    break;

            } while (!processFound);
            return processFound;
        }

        public static string AppendDateTime(string identifier)
        {
            return string.Concat(identifier, HelperLibrary.dateTimeStamp);
        }

        /// <summary>
        /// Get the default selected value of the Combo Box
        /// </summary>
        /// <param name="element">Combo box web element</param>
        /// <returns></returns>
        public static string GetDropDownValue(IWebElement element)
        {
            SelectElement selector = new SelectElement(element);
            return selector.SelectedOption.Text;
        }

        /// <summary>
        /// Select combo box value based on item name
        /// </summary>
        /// <param name="element">Combo box web element</param>
        /// <param name="item">Value to be selected in combo box</param>
        public static void SelectItem(IWebElement element, string item)
        {
            Thread.Sleep(500);
            HelperLibrary.driver.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            SelectElement selector = new SelectElement(element);
            selector.SelectByText(item);
        }

        /// <summary>
        /// Select combo box value based on Value
        /// </summary>
        /// <param name="element">Combo box web element</param>
        /// <param name="item">Value to be selected in combo box</param>
        public static void SelectItemByValue(IWebElement element, string value)
        {
            Thread.Sleep(500);
            HelperLibrary.driver.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            SelectElement selector = new SelectElement(element);
            selector.SelectByValue(value);
        }

        /// <summary>
        /// Select combo box value based on index number
        /// </summary>
        /// <param name="element">Combo box web element</param>
        /// <param name="index">Index number to be selected in combo box</param>
        public static void SelectItem(IWebElement element, int index)
        {
            Thread.Sleep(500);
            HelperLibrary.driver.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            SelectElement selector = new SelectElement(element);
            selector.SelectByIndex(index);
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

        /// <summary>
        /// Hover Mouse over a element
        /// </summary>
        /// <param name="MenuXPath"> string : Xpath of element</param>
        public static void MouseHover(string MenuXPath)
        {
            var element = HelperLibrary.wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(MenuXPath)));
            Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
            //Waiting for the menu to be displayed    
            System.Threading.Thread.Sleep(2000);
        }

        public static void SwitchToIFrame(IWebElement frame)
        {
            HelperLibrary.wait.FrameToBeAvailableAndSwitchToIt(frame);
            //driver.SwitchTo().Frame(frame);
        }

        /// <summary>
        /// Switch web driver to window recognized by name
        /// </summary>
        /// <param name="windowName">Window name</param>
        public static bool GetWindowByName(string windowName)
        {
            TimeSpan initialTicks = new TimeSpan(DateTime.Now.Ticks);
            double initialSeconds = initialTicks.TotalSeconds;

            bool windowExits = false;
            Thread.Sleep(5000);

            do
            {
                TimeSpan currentTicks = new TimeSpan(DateTime.Now.Ticks);
                double currentSeconds = currentTicks.TotalSeconds;

                if (currentSeconds - initialSeconds <= int.Parse(Config.WaitTime))
                {
                    var allWindowHandles = driver.WindowHandles;
                    foreach (string currentWindow in allWindowHandles)
                    {
                        driver.SwitchTo().Window(currentWindow);
                        if (driver.Title.Contains(windowName))
                        {
                            if (driver.Title.Contains("Report Navigator"))
                                driver.Manage().Window.Maximize();

                            windowExits = true;
                            break;
                        }
                    }
                }
                else
                {
                    //HelperLibrary.GetWindowByName(currentWindowTitle);
                    throw new Exception(string.Format("Window with {0} name is not displaying.", windowName));
                }
            } while (!windowExits);

            return windowExits;
        }

        /// <summary>
        /// Get Window By URL
        /// </summary>
        /// <param name="windowURL">Window URL to be searched</param>
        public static void GetWindowByUrl(string windowURL)
        {
            bool windowExits = false;
            System.Threading.Thread.Sleep(5000);
            do
            {
                var allWindowHandles = driver.WindowHandles;
                foreach (string currentWindow in allWindowHandles)
                {
                    driver.SwitchTo().Window(currentWindow);
                    if (driver.Url.Contains(windowURL))
                    {
                        if (driver.Title.Contains("Report Navigator"))
                            driver.Manage().Window.Maximize();

                        windowExits = true;
                        break;
                    }
                }
            } while (!windowExits);
        }

        public static string GetScreenShot()
        {
            string savePath = string.Empty;

            StackTrace stackTrace = new StackTrace();
            StackFrame[] stackFrame = stackTrace.GetFrames();
            var method = stackFrame[2].GetMethod();


            savePath = @"C:\Results" + string.Format("\\{0}.jpg", method.Name + dateTimeStamp);
  
            System.Drawing.Rectangle bounds = System.Windows.Forms.Screen.GetBounds(System.Drawing.Point.Empty);
            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(bounds.Width, bounds.Height))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(System.Drawing.Point.Empty, System.Drawing.Point.Empty, bounds.Size);
                }
                bitmap.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            return savePath;
        }
        public static void SetAttribute(this IWebElement element, string attributeName, string value)
        {
            IWrapsDriver wrappedElement = element as IWrapsDriver;
            if (wrappedElement == null)
                throw new ArgumentException("element", "Element must wrap a web driver");

            IWebDriver driver = wrappedElement.WrappedDriver;
            IJavaScriptExecutor javascript = driver as IJavaScriptExecutor;
            if (javascript == null)
                throw new ArgumentException("element", "Element must wrap a web driver that supports javascript execution");

            javascript.ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2])", element, attributeName, value);
        }
    
    }    

    /// <summary>
    /// This class represents the extension method
    /// </summary>
    public static class GeneralUtility
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
                HelperLibrary.wait.Until(ExpectedConditions.ElementToBeClickable(element));
                element.Clear();
                element.SendKeys(value);

                //Sendkeys Using Javascripts
                if (!GetValue(element).Equals(value))
                {
                    element.Clear();
                    IJavaScriptExecutor executor = (IJavaScriptExecutor)HelperLibrary.driver;
                    executor.ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2]);", element, "value", value);
                }
                //Sendkeys using windows event
                if (!GetValue(element).Equals(value))
                {
                    element.Clear();
                    SendKeys.SendWait(value);
                }
                //Sendkeys using codedUi
                if (!GetValue(element).Equals(value))
                {
                    element.Clear();
                    Keyboard.SendKeys(value);
                }
                //Sendkeys using Action Class
                if (!GetValue(element).Equals(value))
                {
                    Actions act = new Actions(HelperLibrary.driver);
                    element.Clear();
                    act.SendKeys(element, value).Build().Perform();
                }
            }
            catch (Exception ex)
            {
            }

        }

        public static void SendChar(IWebElement element, string value)
        {
            element.Clear();
            element.Click();
            char[] c = value.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                Keyboard.SendKeys(c[i].ToString());
                SendKeys.SendWait("{END}");
            }
            SendKeys.SendWait("{TAB}");
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
                IJavaScriptExecutor js = (IJavaScriptExecutor)HelperLibrary.driver;
                js.ExecuteScript("arguments[0].click();", element);
            }
            catch (Exception ex)
            {
            }

        }

    }

}

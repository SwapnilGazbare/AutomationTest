using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary
{
    public class SeleniumDriver
    {
        public static IWebDriver driver;
        public static Size BrowserSize;

        public static IWebDriver StartDriver(string driverOptions, Size? windowSize = null)
        {
            try
            {
                BrowserSize = windowSize ?? Size.Empty;
                BrowserType browserType = (BrowserType)Enum.Parse(typeof(BrowserType), driverOptions);
                switch (browserType)
                {
                    case BrowserType.Chrome:
                        driver = Utils.GetChromeDriver();
                        break;

                    case BrowserType.Firefox:
                        driver = Utils.GetFirefoxDriver();
                        driver.Manage().Timeouts().ImplicitWait.Seconds.Equals(TimeSpan.FromSeconds(20));
                        break;

                    case BrowserType.IE:
                        driver = Utils.GetInternetExplorerDriver();
                        break;

                    case BrowserType.Safari:
                        driver = new SafariDriver();
                        break;
                }
                ResizeWindow(BrowserSize);
                return driver;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

        /// <summary>Resizes the window.</summary>
        /// <param name="windowSize">Size of the window.</param>
        private static void ResizeWindow(Size windowSize)
        {
            if (windowSize == Size.Empty)
            {
               driver.Manage().Window.Maximize();
            }
            else
            {
                driver.Manage().Window.Size = windowSize;
            }
        }

        /// <summary>Navigates to given URL.</summary>
        /// <param name="url">The URL.</param>
        /// <param name="overrideSecurityWarning">if set to <c>true</c> [override security warning].</param>
        /// <returns>true if navigation was successful</returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static bool NavigateTo(string url, bool overrideSecurityWarning = false)
        {
            if (url != null && Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) &&
                (new List<string> { Uri.UriSchemeHttp, Uri.UriSchemeHttps }.Contains(uriResult.Scheme)))
            {
                driver.Navigate().GoToUrl(url);
                if (overrideSecurityWarning)
                {
                    return OverrideSecurityWarning();
                }
                return true;
            }
            else
            {
                throw new ArgumentException(string.Format("'{0} is an invalid URL'", url));
            }
        }

        /// <summary>Overrides the security warning.</summary>
        /// <returns>true if override was successful</returns>
        protected static bool OverrideSecurityWarning()
        {
            switch (driver.GetType().Name)
            {
                case "InternetExplorerDriver":
                case "ChromeDriver":
                    if (driver.FindElement(By.XPath(@"//a[@id='overridelink']")).Displayed)
                    {
                        driver.Navigate().GoToUrl("javascript:document.getElementById('overridelink').click()");
                        //Log.WriteLine("Overrode security warning");
                    }
                    else
                    {
                        //Log.WriteLine("Did not find 'Override Security Warning' Link", Log.LogType.ERROR);
                        return false;
                    }
                    break;

                case "FirefoxDriver":
                    DesiredCapabilities cap = (DesiredCapabilities)((RemoteWebDriver)driver).Capabilities;
                    cap.SetCapability("acceptSslCerts", true);
                    //driver = new FirefoxDriver(cap);
                    //Log.WriteLine("Restarted Firefox driver with override security warning capability");
                    break;

                default:
                    break;
            }
            return true;
        }
    }

    public enum BrowserType
    {
        /// <summary>Chrome Browser</summary>
        Chrome,

        /// <summary>Firefox Browser</summary>
        Firefox,

        /// <summary>IE Browser</summary>
        IE,

        /// <summary>Safari browser</summary>
        Safari
    }
}


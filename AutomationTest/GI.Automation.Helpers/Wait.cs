using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;


namespace Automation.Helpers
{
    public static class Wait
    { 

        /// <summary>
        /// Des: Wait for a document is ready. Use same WaitForPageLoad()
        /// </summary>s
        public static void UntilDocumentReady(this WebDriverWait wait)
        {
            var javascript = HelperLibrary.driver as IJavaScriptExecutor;
            if (javascript == null)
                throw new ArgumentException("driver", "Driver must support javascript execution");

            wait.Until((d) =>
            {
                try
                {
                    string readyState = javascript.ExecuteScript(
                        "if (document.readyState) return document.readyState;").ToString();
                    return readyState.ToLower() == "complete";
                }
                catch (InvalidOperationException e)
                {
                    //Window is no longer available
                    return e.Message.ToLower().Contains("unable to get browser");
                }
                catch (WebDriverException e)
                {
                    //Browser is no longer available
                    return e.Message.ToLower().Contains("unable to connect");
                }
                catch (Exception)
                {
                    return false;
                }
            });

        }        

        /// <summary>
        /// Des: Wait until an element is no longer attached to the DOM
        /// </summary>
        public static void StalenessOf(this WebDriverWait wait, IWebElement ele)
        {
            wait.Until(element =>
            {
                try
                {
                    return ele == null || !ele.Enabled;
                }
                catch
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// Des: An expectation for checking an element is visible and enabled such that you can click on it
        /// </summary>
        public static void ElementToBeClickable(this WebDriverWait wait, By locator)
        {
            wait.Until(element =>
            {
                var ele = HelperLibrary.driver.FindElement(locator);
                try
                {
                    if (ele != null && ele.Enabled)
                    {
                        return ele;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });
        }

        /// <summary>
        /// Des: An expectation for checking an element is visible and enabled such that you can click on it
        /// </summary>
        public static void ElementToBeClickable(this WebDriverWait wait, IWebElement ele)
        {
            wait.Until(element =>
            {
                try
                {
                    if (ele != null && ele.Enabled)
                    {
                        return ele;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });
        }

        /// <summary>
        /// Des: Wait until element is visible
        /// </summary>
        public static IWebElement ElementIsVisible(this WebDriverWait wait, By loc)
        {
            wait.Until(element =>
            {
                try
                {
                    var ele = HelperLibrary.driver.FindElement(loc);
                    return ele.Displayed ? ele : null;
                }
                catch
                {
                    return null;
                }
            });
            return null;
        }

        /// <summary>
        /// Des: Wait until element is visible
        /// </summary>
        public static bool ElementIsVisible(this WebDriverWait wait, IWebElement ele)
        {
            bool flag = false;
            wait.Until(element =>
            {
                try
                {
                    flag = ele.Displayed ? true : false;
                }
                catch
                {
                    flag = false;
                }
                return flag;
            });
            return flag;
        }

        /// <summary>
        /// Des: An expectation for checking whether the given frame is available to switch to. If the frame is available it switches the given driver to the specified frame.
        /// </summary>
        public static void FrameToBeAvailableAndSwitchToIt(this WebDriverWait wait, string frameName)
        {
            wait.Until(element =>
            {
                try
                {
                    return HelperLibrary.driver.SwitchTo().Frame(frameName);
                }
                catch
                {
                    return null;
                }
            });
        }

        /// <summary>
        /// Des: An expectation for checking whether the given frame is available to switch to. If the frame is available it switches the given driver to the specified frame.
        /// </summary>
        public static void FrameToBeAvailableAndSwitchToIt(this WebDriverWait wait, IWebElement loc)
        {
            wait.Until(element =>
            {
                try
                {
                    return HelperLibrary.driver.SwitchTo().Frame(loc);
                }
                catch
                {
                    return null;
                }
            });
        }

        /// <summary>
        /// Des: An expectation for checking the AlterIsPresent
        /// </summary>
        public static void AlertIsPresent(this WebDriverWait wait)
        {
            wait.Until(element =>
            {
                try
                {
                    return HelperLibrary.driver.SwitchTo().Alert();
                }
                catch (NoAlertPresentException)
                {
                    return null;
                }
            });
        }

        /// <summary>
        /// Des: An expectation for checking the AlterIsPresent
        /// </summary>
        public static void InvisibilityOfElementLocated(this WebDriverWait wait, By loc)
        {
            wait.Until(element =>
            {
                try
                {
                    var ele = HelperLibrary.driver.FindElement(loc);
                    return !ele.Displayed;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            });
        }

        /// <summary>
        /// Des: An expectation for checking the AlterIsPresent
        /// </summary>
        public static void InvisibilityOfElementWithText(this WebDriverWait wait, By loc, string text)
        {
            wait.Until(element =>
            {
                try
                {
                    var ele = HelperLibrary.driver.FindElement(loc);
                    var elementText = ele.Text;
                    if (string.IsNullOrEmpty(elementText))
                    {
                        return true;
                    }
                    return !elementText.Equals(text);
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            });
        }
    }
}

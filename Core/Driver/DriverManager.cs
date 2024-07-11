using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using OpenQA.Selenium;
using Microsoft.Extensions.Configuration;
using System.Net.WebSockets;
using Core.Driver;

namespace Core.Driver
{
    public static class DriverManager
    {
        [ThreadStatic]
        public static IWebDriver WebDriver;
        public static void InitializeBrowser(string browser, bool headless = false)
        {
            if (WebDriver != null) return;

            WebDriver = headless? BrowserFactory.CreateHeadlessBrowser(browser) : BrowserFactory.CreateBrowser(browser);
        }

        public static void CloseDriver()
        {
            if (WebDriver != null)
            {
                WebDriver.Dispose();
                WebDriver.Quit();
                WebDriver = null;
            }
        }

        public static IWebDriver Instance => WebDriver ?? throw new NullReferenceException("Driver is not initialized");

    }
}
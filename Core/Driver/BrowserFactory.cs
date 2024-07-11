using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Driver.ConcreteBrowser;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace Core.Driver
{
    public static class BrowserFactory
    {
        public static IWebDriver CreateBrowser(string browser)
        {
            IBrowserFactory factory = browser.ToLower() switch
            {
                "chrome" => new ChromeFactory(),
                "firefox" => new FirefoxFactory(),
                "edge" => new EdgeFactory(),
                _ => throw new ArgumentException($"Browser '{browser}' is not supported."),
            };

            return factory.CreateDriver();
        }

        public static IWebDriver CreateHeadlessBrowser(string browser)
        {
            IBrowserFactory factory = browser.ToLower() switch
            {
                "chrome" => new ChromeFactory(),
                "firefox" => new FirefoxFactory(),
                "edge" => new EdgeFactory(),
                _ => throw new ArgumentException($"Browser '{browser}' is not supported."),
            };

            return factory.CreateHeadlessDriver();
        }
    }
}
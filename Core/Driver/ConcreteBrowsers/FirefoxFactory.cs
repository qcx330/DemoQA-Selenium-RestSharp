using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Core.Driver.ConcreteBrowser
{
    public class FirefoxFactory : IBrowserFactory
    {
        public IWebDriver CreateDriver()
        {
            var config = BrowserOptions.GetBrowserConfig("Firefox");
            var options = new FirefoxOptions();
            if (config.Headless)
            {
                options.AddArgument("--headless");
            }
            foreach (var arg in config.Arguments)
            {
                options.AddArgument(arg);
            }
            return new FirefoxDriver(options);
        }

        public IWebDriver CreateHeadlessDriver()
        {
            var options = new FirefoxOptions();
            options.AddArgument("--headless");
            var config = BrowserOptions.GetBrowserConfig("Firefox");
            foreach (var arg in config.Arguments)
            {
                options.AddArgument(arg);
            }
            return new FirefoxDriver(options);
        }
    }
}
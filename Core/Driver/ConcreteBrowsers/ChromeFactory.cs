using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
// using TMS.Core.Driver;

namespace Core.Driver.ConcreteBrowser
{
    public class ChromeFactory: IBrowserFactory
    {
        public IWebDriver CreateDriver()
    {
        var config = BrowserOptions.GetBrowserConfig("Chrome");
        var options = new ChromeOptions();
        if (config.Headless)
        {
            options.AddArgument("--headless");
        }
        foreach (var arg in config.Arguments)
        {
            options.AddArgument(arg);
        }
        return new ChromeDriver(options);
    }

    public IWebDriver CreateHeadlessDriver()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        var config = BrowserOptions.GetBrowserConfig("Chrome");
        foreach (var arg in config.Arguments)
        {
            options.AddArgument(arg);
        }
        return new ChromeDriver(options);
    }
    }
}
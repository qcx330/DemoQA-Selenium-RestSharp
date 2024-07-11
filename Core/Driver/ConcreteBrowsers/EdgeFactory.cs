using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Core.Driver.ConcreteBrowser
{
    public class EdgeFactory: IBrowserFactory
    {
        public IWebDriver CreateDriver()
    {
        var config = BrowserOptions.GetBrowserConfig("Edge");
        var options = new EdgeOptions();
        if (config.Headless)
        {
            options.AddArgument("headless");
        }
        foreach (var arg in config.Arguments)
        {
            options.AddArgument(arg);
        }
        return new EdgeDriver(options);
    }

    public IWebDriver CreateHeadlessDriver()
    {
        var options = new EdgeOptions();
        options.AddArgument("headless");
        var config = BrowserOptions.GetBrowserConfig("Edge");
        foreach (var arg in config.Arguments)
        {
            options.AddArgument(arg);
        }
        return new EdgeDriver(options);
    }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Core.Driver
{
    public interface IBrowserFactory
    {
        IWebDriver CreateDriver();
        IWebDriver CreateHeadlessDriver();
    }
}
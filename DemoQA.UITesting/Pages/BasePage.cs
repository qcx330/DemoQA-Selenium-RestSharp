using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Driver;
using OpenQA.Selenium;

namespace DemoQA.UITesting.Pages
{
    public class BasePage
    {
        IWebDriver webDriver;
        protected BasePage(){
            webDriver = DriverManager.WebDriver;
        }
        public string GoToUrl(string url){
            return webDriver.Url = url;
        }
    }
}
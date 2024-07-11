using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using Core.Configuration;
using Core.Driver;
using Core.Reports;
using DemoQA.UITesting.Pages;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using OpenQA.Selenium;

namespace final.Test
{
    [TestFixture]
    public class BaseTest
    {
        private LoginPage loginPage;
        public BaseTest(){
            ExtentTestManager.CreateParentTest(TestContext.CurrentContext.Test.ClassName);
            
        }

        [SetUp]
        public void Setup()
        {
            string browser = ConfigurationHelper.GetConfiguration()["browser"];
            double timeOutSec = double.Parse(ConfigurationHelper.GetConfiguration()["timeout.webdriver.wait.seconds"]);
            
            loginPage = new();

            ExtentTestManager.CreateTest(TestContext.CurrentContext.Test.Name);
            

            DriverManager.InitializeBrowser(browser);
            DriverManager.WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeOutSec);
            Console.WriteLine("Base Test set up"); 
        }

        protected void LoginToDemoQA(){
            var account = Hook.Accounts["user"];
            loginPage.GoToUrl("https://demoqa.com/login");
            loginPage.Login(account.Username,account.Password);
        }

        [TearDown]
        public void TearDown()
        {   
            ExtentTestManager.LogTestOutcome(TestContext.CurrentContext, DriverManager.WebDriver);
            ReportLog.Info("Close webdriver");
            
            DriverManager.CloseDriver();
            Console.WriteLine("Base Test tear down");
        }
    }
}
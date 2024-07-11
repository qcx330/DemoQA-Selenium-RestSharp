using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.API;
using Core.Configuration;
using Core.Reports;
using Core.ShareData;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using WebDriverManager;

namespace DemoQA.APITesting.Test
{
    public class BaseTest
    {
        protected static APIClient ApiClient;
        public BaseTest()
        {
            ApiClient = new APIClient(ConfigurationHelper.GetConfiguration()["url"]);
            ExtentTestManager.CreateParentTest(TestContext.CurrentContext.Test.ClassName);
        }
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Base Test set up");
            ExtentTestManager.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void TearDown()
        {
            DataStorage.ClearData();
            ExtentTestManager.LogTestOutcome(TestContext.CurrentContext);
            Console.WriteLine("Base Test tear down");
        }

    }
}
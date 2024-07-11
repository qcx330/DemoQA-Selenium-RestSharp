using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Core.Configuration;
using Core.Reports;
using Core.Utilities;
using DemoQA.UITesting.Constant;
using DemoQA.UITesting.Model;

namespace final.Test
{
    [SetUpFixture]
    public class Hook
    {

        public static Dictionary<string, User> Accounts;
        public static Dictionary<string, Student> Students;
        const string AppSettingPath = "Configurations\\appsettings.json";

        [OneTimeSetUp]
        public void MySetup()
        {
            TestContext.Progress.WriteLine("===> Global one time setup");
            var config = ConfigurationHelper.ReadConfiguration(AppSettingPath);
            Students = JsonUtils.ReadDictionaryJson<Dictionary<string, Student>>(JsonPathConstant.StudentsPath);
            Accounts = JsonUtils.ReadDictionaryJson<Dictionary<string, User>>(JsonPathConstant.AccountsPath);
        }
        
        
        [OneTimeTearDown]
        public void End(){
            ExtentReportManager.GenerateReport();
        }
    }
}
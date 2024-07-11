using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using Core.Configuration;
using Core.Reports;
using Core.ShareData;
using Core.Utilities;
using DemoQA.APITesting.DataObject;
using NUnit.Framework;

namespace DemoQA.APITesting.Test
{
    [SetUpFixture]
    public class Hook
    {
        const string AppSettingPath = "appsettings.json";
        public static Dictionary<string, UserDto> Users;
        public static Dictionary<string, BookDto> Books;

        [OneTimeSetUp]
        public void OneTimeStartup()
        {
            ConfigurationHelper.ReadConfiguration(AppSettingPath);
            DataStorage.InitData();
            Users = JsonUtils.ReadDictionaryJson<Dictionary<string, UserDto>>(Constant.JsonPathConstant.UsersPath);
            Books = JsonUtils.ReadDictionaryJson<Dictionary<string, BookDto>>(Constant.JsonPathConstant.BooksPath);
        }
        [OneTimeTearDown]
        public void OneTimeTearDown(){
            ExtentReportManager.GenerateReport();
        }
    }

}
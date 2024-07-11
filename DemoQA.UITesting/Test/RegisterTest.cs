using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using Core.Configuration;
using Core.Reports;
using final.Pages;
using NUnit.Framework;
using OpenQA.Selenium;

namespace final.Test
{
    public class RegisterTest : BaseTest
    {
        private RegisterPage registerPage;
        [SetUp]
        public new void Setup()
        {
            registerPage = new();
            registerPage.GoToUrl(ConfigurationHelper.GetConfiguration()["registerURL"]);
        }
        [Test]
        [Category("RegisterStudent")]
        [TestCase("StudentAllFields")]
        public void RegisterAllFieldsSuccessfully(string key)
        {
            var student = Hook.Students[key];
            ReportLog.Info("1. Register student with all fields ");
            registerPage.Register(student);
            ReportLog.Info("2. Verify register result ");
            registerPage.VerifyRegister(student);
        }
        [Test]
        [Category("RegisterStudent")]
        [TestCase("StudentMandatoryFields")]
        public void RegisterMandatoryFieldsSuccessfully(string key)
        {
            var student = Hook.Students[key];
            ReportLog.Info("1. Register student with mandatory fields ");
            registerPage.Register(student);
            ReportLog.Info("2. Verify register result ");
            registerPage.VerifyRegister(student);
        }
    }
}
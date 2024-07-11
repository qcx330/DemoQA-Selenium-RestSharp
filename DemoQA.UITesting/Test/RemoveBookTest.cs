using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using Core.API;
using Core.Configuration;
using Core.Reports;
using DemoQA.Service.Service;
using DemoQA.UITesting.Pages;
using final.Pages;
using NUnit.Framework;

namespace final.Test
{
    public class RemoveBookTest:BaseTest
    {
        private ProfilePage profilePage;
        private LoginPage loginPage;
        private UserService _userServices;
        private BookService _bookServices;
        protected static APIClient ApiClient;
        [SetUp]
        public new void Setup()
        {
            profilePage = new();
            loginPage = new();
            ApiClient = new APIClient(ConfigurationHelper.GetConfiguration()["appUrl"]);
            _userServices = new UserService(ApiClient);
            _bookServices = new BookService(ApiClient);
            LoginToDemoQA();
        }

        [Test]
        [Category("RemoveBook")]
        [TestCase("Git Pocket Guide")]
        public async Task RemoveBook(string title)
        {
            var account = Hook.Accounts["account"];
            var response = _userServices.GenerateToken(account.Username,account.Password);
            var token = response.Data.Token;
            await _bookServices.AddBookToCollectionAsync(account.Id, "9781449325862", token);

            ReportLog.Info("1. Remove book " + title);
            profilePage.RemoveBook(title);
            ReportLog.Info("2. Verify remove result ");
            profilePage.AssertBookRemoved(title);
        }
    }
}
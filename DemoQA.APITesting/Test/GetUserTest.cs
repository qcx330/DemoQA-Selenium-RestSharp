using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Extentions;
using Core.Reports;
using Core.Utilities;
using DemoQA.Service.Service;
using DemoQA.APITesting.DataObject;
using FluentAssertions;
using MongoDB.Bson;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DemoQA.APITesting.Test
{
    [TestFixture("user"), Category("GetUser")]
    [Parallelizable(ParallelScope.None)]
    public class GetUserTest:BaseTest
    {
        private UserService _userServices;
        private string userDataKey, token;
        UserDto user;        
        public GetUserTest(string userKey)
        {
            _userServices = new UserService(ApiClient);
            userDataKey = userKey;
            user = Hook.Users[userDataKey];
        }
        [SetUp]
        public new void Setup(){
            ReportLog.Info("Generate token for account");
            _userServices.StoreUserToken(userDataKey, user.Username, user.Password);
            token = _userServices.GetUserToken(userDataKey);
        }
        [Test]
        public async Task GetUserDetailSuccessfully()
        {
            ReportLog.Info("1. Get user by User ID");
            var response = await _userServices.GetUserAsync(user.UserId, token);
            ReportLog.Info("2. Verify get User response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Data.UserID.Should().Be(user.UserId);
            response.Data.UserName.Should().BeEquivalentTo(user.Username);
            response.Data.Books.Count().Should().BeGreaterThanOrEqualTo(0);
            response.VerifySchema(SchemaConstant.GetUserSchema);
        }
        [Test]
        public async Task GetUserDetailWithoutAuthorized()
        {
            ReportLog.Info("1. Get user by User ID");
            var response = await _userServices.GetUserAsync(user.UserId, null);
            ReportLog.Info("2. Verify get User response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
            var result = (dynamic)JsonConvert.DeserializeObject(response.Content);
            ((int)result["code"]).Should().Be(1200);
            ((string)result["message"]).Should().Be("User not authorized!");
        }
        [Test]
        public async Task GetUserDetailWithWrongUserId()
        {
            ReportLog.Info("1. Get user by User ID");
            var response = await _userServices.GetUserAsync(user.UserId+"444", token);
            ReportLog.Info("2. Verify get User response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
            var result = (dynamic)JsonConvert.DeserializeObject(response.Content);
            ((int)result["code"]).Should().Be(1207);
            ((string)result["message"]).Should().Be("User not found!");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.API;
using Core.ShareData;
using DemoQA.Service.Model.Request;
using DemoQA.Service.Model.Response;
using FluentAssertions;
using Newtonsoft.Json;
using OpenQA.Selenium.DevTools.V123.IndexedDB;
using RestSharp;

namespace DemoQA.Service.Service
{
    public class UserService
    {
        private readonly APIClient _client;
        public UserService(APIClient client){
            _client = client;
        }
        public async Task<RestResponse<GetUserResponseDto>> GetUserAsync(string userId, string token)
        {
            return await _client.CreateRequest(String.Format(APIConstant.GetUserEndPoint, userId))
                .AddContentTypeHeader("application/json")
                .AddHeaderBearerToken(token)
                .ExecuteGetAsync<GetUserResponseDto>();
        }
        public  RestResponse<GenerateTokenResponseDto> GenerateToken(string username, string password){
            GenerateTokenRequestDto request = new(){
                UserName = username,
                Password = password
            };
            return _client.CreateRequest(String.Format(APIConstant.GenerateToken))
                .AddContentTypeHeader("application/json")
                .AddBody(request)
                .ExecutePost<GenerateTokenResponseDto>();
        }
        public void StoreUserToken(string accountKey, string username, string password)
        {
            if (DataStorage.GetData(accountKey) is null)
            {
                var response = GenerateToken(username, password );
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
                var result = (dynamic)JsonConvert.DeserializeObject(response.Content);
                DataStorage.SetData(accountKey, result["token"]);
            }
        }
        public string GetUserToken(string accountKey)
        {
            if (DataStorage.GetData(accountKey) is null)
            {
                throw new Exception("Token is not stored with account " + accountKey);
            }

            return DataStorage.GetData(accountKey).ToString();
        }
    }
}
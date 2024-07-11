using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DemoQA.APITesting.DataObject
{
    public class UserDto
    {
        [JsonProperty("userId")]
        public string UserId {get; set;}
        [JsonProperty("username")]
        public string Username {get; set;}
        [JsonProperty("password")]
        public string Password {get; set;}
    }
}
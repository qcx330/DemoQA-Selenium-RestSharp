using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DemoQA.Service.Model.Response
{
    public class GetUserResponseDto
    {
        [JsonProperty("userId")]
        public string UserID {get; set;}
        [JsonProperty("username")]
        public string UserName {get; set;}
        [JsonProperty("books")]
        public List<BookDataResponseDto> Books {get; set;}
    }
}
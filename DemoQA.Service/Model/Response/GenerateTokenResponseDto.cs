using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DemoQA.Service.Model.Response
{
    public class GenerateTokenResponseDto
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expires")]
        public DateTime Expires { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }
    }
}
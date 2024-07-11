using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DemoQA.APITesting.DataObject
{
    public class BookDto
    {
        [JsonProperty("isbn")]
        public string Isbn { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("publish_date")]
        public DateTime PublishDate { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("pages")]
        public int Pages { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }
    }
}
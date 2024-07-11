using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoQA.Service.Model.Response;
using Newtonsoft.Json;

namespace DemoQA.Service.Model.Request
{
    public class AddBookToCollectionRequestDto
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("collectionOfIsbns")]
        public List<BookRequestDto> CollectionOfIsbns { get; set; }
    }
}
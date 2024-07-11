using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.Utilities;
using FluentAssertions;
using Newtonsoft.Json;
using NJsonSchema;
using RestSharp;

namespace Core.Extentions
{
    public static class RestExtensions
    {
        public static async Task VerifySchema(this RestResponse response, string pathFile)
        {
            var schema = await JsonSchema.FromJsonAsync(JsonUtils.ReadJsonFile(pathFile));
            schema.Validate(response.Content).Should().BeEmpty();
        }

        public static dynamic ConvertToDynamicObject(this RestResponse response)
        {
            return (dynamic)JsonConvert.DeserializeObject(response.Content);
        }

        public static void VerifyStatusCodeOk(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public static void VerifyStatusCodeCreated(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        public static void VerifyStatusCodeBadRequest(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public static void VerifyStatusCodeUnauthorized(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        public static void VerifyStatusCodeForbidden(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        public static void VerifyStatusCodeInternalSeverError(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

    }
}
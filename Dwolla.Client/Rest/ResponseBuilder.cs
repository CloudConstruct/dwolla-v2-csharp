﻿using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dwolla.Client.Models.Responses;
using Newtonsoft.Json;

namespace Dwolla.Client.Rest
{
    public interface IResponseBuilder
    {
        Task<RestResponse<T>> Build<T>(HttpResponseMessage response);
        RestResponse<T> Error<T>(HttpResponseMessage response, string code, string message, string rawContent = null);
    }

    public class ResponseBuilder : IResponseBuilder
    {
        private static readonly Regex ErrorRegex = new Regex("^{.*\"code\":.*\"message\":.*}$", RegexOptions.IgnoreCase);

        public async Task<RestResponse<T>> Build<T>(HttpResponseMessage response)
        {
            using var content = response.Content;
            if (content == null) return Error<T>(response, "NullResponse", "Response content is null", null);
            var rawContent = await content.ReadAsStringAsync().ConfigureAwait(false);

            try
            {
                return ErrorRegex.IsMatch(rawContent)
                    ? Error<T>(response, JsonConvert.DeserializeObject<ErrorResponse>(rawContent), rawContent)
                    : new RestResponse<T>(response, JsonConvert.DeserializeObject<T>(rawContent), rawContent);
            }
            catch (Exception e)
            {
                return Error<T>(response, "DeserializationException", e.Message, rawContent);
            }
        }

        public RestResponse<T> Error<T>(HttpResponseMessage response, string code, string message, string rawContent) =>
            Error<T>(response, new ErrorResponse { Code = code, Message = message }, rawContent);

        private static RestResponse<T> Error<T>(HttpResponseMessage response, ErrorResponse error, string rawContent) =>
            new RestResponse<T>(response, default, rawContent, error);
    }
}

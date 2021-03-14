using System;
using Disqord.Http;
using Disqord.Rest.Api;

namespace Disqord.Rest
{
    public class RestApiException : Exception
    {
        public HttpResponseStatusCode StatusCode { get; }

        public RestApiErrorJsonModel ErrorModel { get; }

        public RestApiException(HttpResponseStatusCode statusCode, RestApiErrorJsonModel errorModel)
            : base($"HTTP {(Enum.IsDefined(statusCode) ? $"{(int) statusCode} {statusCode}" : statusCode)}. Error message: {errorModel.Message}")
        {
            StatusCode = statusCode;
            ErrorModel = errorModel;
        }
    }
}

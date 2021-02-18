using System;
using Disqord.Http;

namespace Disqord.Rest.Api
{
    public class RestApiException : Exception
    {
        public HttpResponseStatusCode StatusCode { get; }

        public RestApiErrorJsonModel ErrorModel { get; }

        public RestApiException(HttpResponseStatusCode statusCode, RestApiErrorJsonModel errorModel)
        {
            StatusCode = statusCode;
            ErrorModel = errorModel;
        }

        public override string ToString()
            => $"HTTP {(Enum.IsDefined(StatusCode) ? $"{(int) StatusCode} {StatusCode}" : StatusCode)}. Error code: {ErrorModel.Code}. Error message: {ErrorModel.Message}";
    }
}

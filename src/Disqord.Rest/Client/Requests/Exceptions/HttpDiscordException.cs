using System;
using System.Net;

namespace Disqord.Rest
{
    public sealed class DiscordHttpException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }

        public JsonErrorCode? JsonErrorCode { get; }

        public DiscordHttpException(HttpStatusCode httpStatusCode, int? jsonErrorCode, string message) : base(message)
        {
            HttpStatusCode = httpStatusCode;
            JsonErrorCode = (JsonErrorCode?) jsonErrorCode;
        }

        public override string ToString()
            => $"{(int) HttpStatusCode} {HttpStatusCode}: {Message}";
    }
}

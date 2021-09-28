using System;
using Disqord.Http;
using Qommon;

namespace Disqord.Rest.Api
{
    /// <summary>
    ///     Represents a REST route of the Discord API.
    ///     E.g. <c>GET /channels/{0:channel_id}/messages/{1:message_id}</c>.
    /// </summary>
    public sealed partial class Route
    {
        /// <summary>
        ///     Gets the HTTP method of this route.
        /// </summary>
        public HttpRequestMethod Method { get; }

        /// <summary>
        ///     Gets the formattable relative path of this route.
        /// </summary>
        public string Path { get; }

        /// <summary>
        ///     Instantiates a new <see cref="Route"/>.
        /// </summary>
        /// <param name="method"> The HTTP request method. </param>
        /// <param name="path"> The formattable relative path. </param>
        public Route(HttpRequestMethod method, string path)
        {
            Guard.IsDefined(method);
            Guard.IsNotNullOrWhiteSpace(path);

            if (path.StartsWith('/'))
                Throw.FormatException("The path must be a relative path with no leading slash.");

            Method = method;
            Path = path;
        }

        public override int GetHashCode()
            => HashCode.Combine(Method, Path);

        public override bool Equals(object obj)
            => obj is Route route && Method == route.Method && Path == route.Path;

        public override string ToString()
            => $"{Method}|{Path}";
    }
}

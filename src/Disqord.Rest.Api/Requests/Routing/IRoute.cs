using System;
using Disqord.Http;

namespace Disqord.Rest.Api;

/// <summary>
///     Represents a REST route of the Discord API.
/// </summary>
public interface IRoute : IEquatable<IRoute>
{
    /// <summary>
    ///     Gets the HTTP method of this route.
    /// </summary>
    HttpRequestMethod Method { get; }

    /// <summary>
    ///     Gets the relative path of this route.
    /// </summary>
    string Path { get; }
}

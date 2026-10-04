using System;
using Disqord.Http;
using Qommon;

namespace Disqord.Rest.Api;

/// <summary>
///     Represents a REST route of the Discord API formatted with parameters.
///     E.g. <c>GET /channels/123/messages/321</c>.
/// </summary>
public class FormattedRoute : IFormattedRoute
{
    /// <summary>
    ///     Gets the base <see cref="IFormattableRoute"/> this route has been formatted from.
    /// </summary>
    public IFormattableRoute BaseRoute { get; }

    /// <inheritdoc />
    public HttpRequestMethod Method => BaseRoute.Method;

    /// <summary>
    ///     Gets the formatted path of the <see cref="BaseRoute"/>.
    /// </summary>
    public string Path { get; }

    /// <summary>
    ///     Gets the route parameters found in this route.
    /// </summary>
    public IRouteParameters Parameters { get; }

    /// <inheritdoc/>
    public string RedactedPath { get; }

    /// <summary>
    ///     Instantiates a new <see cref="FormattedRoute"/> whose <see cref="RedactedPath"/> is the unformatted path of the base route.
    /// </summary>
    public FormattedRoute(IFormattableRoute baseRoute, string path, IRouteParameters parameters)
        : this(baseRoute, path, baseRoute?.Path!, parameters)
    { }

    public FormattedRoute(IFormattableRoute baseRoute, string path, string redactedPath, IRouteParameters parameters)
    {
        Guard.IsNotNull(baseRoute);
        Guard.IsNotNull(path);
        Guard.IsNotNull(redactedPath);
        Guard.IsNotNull(parameters);

        BaseRoute = baseRoute;
        Path = path;
        RedactedPath = redactedPath;
        Parameters = parameters;
    }

    /// <inheritdoc/>
    public bool Equals(IRoute? other)
    {
        return other != null && Method == other.Method && Path == other.Path;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Method, Path);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is IRoute other && Equals(other);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Method}|{Path}";
    }
}

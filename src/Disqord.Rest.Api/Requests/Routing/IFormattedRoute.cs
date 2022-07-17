namespace Disqord.Rest.Api;

/// <summary>
///     Represents a formatted REST route of the Discord API.
///     E.g. <c>GET /channels/123/messages/321</c>.
/// </summary>
public interface IFormattedRoute : IRoute
{
    IFormattableRoute BaseRoute { get; }

    IRouteParameters Parameters { get; }
}

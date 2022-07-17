namespace Disqord.Rest.Api;

/// <summary>
///     Represents a formattable REST route of the Discord API.
///     E.g. <c>GET /channels/{0:channel_id}/messages/{1:message_id}</c>.
/// </summary>
public interface IFormattableRoute : IRoute
{ }

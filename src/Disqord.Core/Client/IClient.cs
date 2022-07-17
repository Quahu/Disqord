using Disqord.Api;
using Disqord.Logging;

namespace Disqord;

/// <summary>
///     Represents a high-level client for the Discord API.
/// </summary>
public interface IClient : ILogging
{
    /// <summary>
    ///     Gets the low-level version of this client.
    /// </summary>
    /// <remarks>
    ///     Do not use this unless you are well aware of how it works.
    /// </remarks>
    IApiClient ApiClient { get; }
}
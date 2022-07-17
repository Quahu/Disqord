using Disqord.Logging;

namespace Disqord.Api;

/// <summary>
///     Represents a low-level client for the Discord API.
/// </summary>
/// <remarks>
///     Do not use this unless you are well aware of how it works.
/// </remarks>
public interface IApiClient : ILogging
{
    /// <summary>
    ///     Gets the token used by this API client.
    /// </summary>
    Token Token { get; }
}
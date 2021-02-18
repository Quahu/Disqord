using System;
using Disqord.Api;
using Disqord.Logging;

namespace Disqord
{
    /// <summary>
    ///     Represents a high-level client for the Discord API.
    /// </summary>
    public interface IClient : ILogging, IDisposable
    {
        /// <summary>
        ///     Gets the low-level version of this client.
        ///     Do not use this unless you are well aware of how it works.
        /// </summary>
        IApiClient ApiClient { get; }
    }
}

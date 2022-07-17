using System;
using System.Net.Http;

namespace Disqord.Http.Default;

public class DefaultHttpClientFactoryConfiguration
{
    /// <summary>
    ///     Gets or sets the action that configures the <see cref="SocketsHttpHandler"/>
    ///     created by the <see cref="DefaultHttpClientFactory"/>.
    /// </summary>
    /// <remarks>
    ///     This should only be used for, for example, setting the proxy.<br/>
    ///     Do <b>not</b> modify <see cref="SocketsHttpHandler.AutomaticDecompression"/> or <see cref="SocketsHttpHandler.PooledConnectionLifetime"/>.
    /// </remarks>
    public Action<SocketsHttpHandler>? ClientConfiguration { get; set; }
}

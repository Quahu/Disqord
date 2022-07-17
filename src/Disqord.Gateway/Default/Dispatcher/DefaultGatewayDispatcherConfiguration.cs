namespace Disqord.Gateway.Default;

public class DefaultGatewayDispatcherConfiguration
{
    /// <summary>
    ///     Gets or sets whether the <see cref="DefaultGatewayDispatcher.ReadyEvent"/> should be delayed.
    ///     Defaults to <see cref="Disqord.ReadyEventDelayMode.Guilds"/>.
    /// </summary>
    public virtual ReadyEventDelayMode ReadyEventDelayMode { get; set; } = ReadyEventDelayMode.Guilds;
}
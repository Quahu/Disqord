namespace Disqord.Gateway.Default
{
    public class DefaultGatewayCacheProviderConfiguration
    {
        /// <summary>
        ///     Gets or sets the amount of messages that should be cached per-channel.
        ///     Defaults to <c>100</c>. Set to <c>0</c> to disable message caching.
        /// </summary>
        public virtual int MessagesPerChannel { get; set; } = 100;
    }
}
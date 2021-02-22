using System;
using System.Collections.Generic;

namespace Disqord.Gateway.Default
{
    public class DefaultGatewayCacheProviderConfiguration
    {
        /// <summary>
        ///     Gets or sets the amount of messages that should be cached per-channel.
        ///     Defaults to <c>100</c>.
        /// </summary>
        public virtual int MessagesPerChannel { get; set; } = 100;

        public virtual IEnumerable<Type> SupportedTypes { get; set; } = new[] { typeof(CachedSharedUser), typeof(CachedGuild) };

        public virtual IEnumerable<Type> SupportedNestedTypes { get; set; } = new[] { typeof(CachedMember), typeof(CachedGuildChannel), typeof(CachedUserMessage) };
    }
}
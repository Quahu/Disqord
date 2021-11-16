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

        public virtual IList<Type> SupportedTypes { get; set; } = new List<Type>
        {
            typeof(CachedSharedUser),
            typeof(CachedGuild)
        };

        public virtual IList<Type> SupportedNestedTypes { get; set; } = new List<Type>
        {
            typeof(CachedMember),
            typeof(CachedGuildChannel),
            typeof(CachedRole),
            typeof(CachedVoiceState),
            typeof(CachedPresence),
            typeof(CachedUserMessage),
            typeof(CachedStage),
            typeof(CachedGuildEvent)
        };
    }
}

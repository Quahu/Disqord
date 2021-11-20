using System;
using System.ComponentModel;
using Qommon;

namespace Disqord.Extensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DiscordClientExtensionExtensions
    {
        internal static DiscordClientBase GetDiscordClient(this IClientEntity entity)
        {
            Guard.IsNotNull(entity);

            if (entity.Client is not DiscordClientBase client)
                throw new InvalidOperationException("This entity's client is not a Discord client implementation.");

            return client;
        }
    }
}

using System;
using System.ComponentModel;

namespace Disqord.Extensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DiscordClientExtensionExtensions
    {
        internal static DiscordClientBase GetDiscordClient(this IClientEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Client is not DiscordClientBase client)
                throw new InvalidOperationException("This entity's client is not a Discord client implementation.");

            return client;
        }
    }
}

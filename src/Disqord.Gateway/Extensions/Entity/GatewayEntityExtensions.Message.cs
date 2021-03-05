using System;
using System.ComponentModel;

namespace Disqord.Gateway
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class GatewayEntityExtensions
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IGatewayClient GetGatewayClient(this IEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Client is not IGatewayClient client)
                throw new InvalidOperationException("This entity's client is not a gateway client implementation.");

            return client;
        }

        /// <summary>
        ///     Gets the <see cref="CachedTextChannel"/> for the specified gateway message.
        ///     Returns <see langword="null"/> if the channel is not cached or the message was sent in a private channel.
        /// </summary>
        /// <param name="message"> The message to get the channel for. </param>
        /// <returns>
        ///     The cached channel of this message.
        /// </returns>
        public static CachedTextChannel GetChannel(this IGatewayMessage message)
        {
            if (message.GuildId == null)
                return null;

            var client = message.GetGatewayClient();
            return client.GetChannel(message.GuildId.Value, message.ChannelId) as CachedTextChannel;
        }
    }
}

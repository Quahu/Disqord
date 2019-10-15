using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    internal static class RestImplementation
    {
        public static Task AddReactionAsync(IMessage message, IEmoji emoji, RestRequestOptions options)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            return message.Client.AddReactionAsync(message.ChannelId, message.Id, emoji, options);
        }

        public static Task RemoveReactionAsync(IMessage message, Snowflake memberId, IEmoji emoji, RestRequestOptions options)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            return message.Client.RemoveMemberReactionAsync(message.ChannelId, message.Id, memberId, emoji, options);
        }

        public static Task RemoveOwnReactionAsync(IMessage message, IEmoji emoji, RestRequestOptions options)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            return message.Client.RemoveOwnReactionAsync(message.ChannelId, message.Id, emoji, options);
        }

        public static Task MarkAsReadAsync(IMessageChannel channel, RestRequestOptions options)
        {
            var lastMessageId = channel.LastMessageId;
            if (!lastMessageId.HasValue)
                throw new InvalidOperationException("Channel has no last message id.");

            return channel.Client.MarkMessageAsReadAsync(channel.Id, lastMessageId.Value, options);
        }
    }
}

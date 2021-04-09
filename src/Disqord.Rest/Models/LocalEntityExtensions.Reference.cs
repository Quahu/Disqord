using Disqord.Models;

namespace Disqord.Rest.Models
{
    public static partial class LocalEntityExtensions
    {
        public static MessageReferenceJsonModel ToModel(this LocalReference reference)
            => reference == null ? null : new MessageReferenceJsonModel
            {
                MessageId = reference.MessageId,
                ChannelId = Optional.FromNullable(reference.ChannelId),
                GuildId = Optional.FromNullable(reference.GuildId),
                FailIfNotExists = reference.FailOnInvalid
            };
    }
}

using Qommon;

namespace Disqord.Models
{
    public static partial class LocalEntityExtensions
    {
        public static MessageReferenceJsonModel ToModel(this LocalMessageReference reference)
            => reference == null ? null : new MessageReferenceJsonModel
            {
                MessageId = reference.MessageId,
                ChannelId = Optional.FromNullable(reference.ChannelId),
                GuildId = Optional.FromNullable(reference.GuildId),
                FailIfNotExists = reference.FailOnUnknownMessage
            };
    }
}

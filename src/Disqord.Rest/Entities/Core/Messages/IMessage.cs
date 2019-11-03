using System.Collections.Generic;

namespace Disqord
{
    public partial interface IMessage : ISnowflakeEntity, IDeletable
    {
        Snowflake ChannelId { get; }

        IUser Author { get; }

        string Content { get; }

        IReadOnlyList<IUser> MentionedUsers { get; }

        IReadOnlyDictionary<IEmoji, ReactionData> Reactions { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord
{
    public interface IMessage : ISnowflakeEntity, IDeletable
    {
        Snowflake ChannelId { get; }

        IUser Author { get; }

        string Content { get; }

        DateTimeOffset Timestamp { get; }

        IReadOnlyList<IUser> UserMentions { get; }

        // TODO: activity

        // TODO: application

        IReadOnlyDictionary<IEmoji, ReactionData> Reactions { get; }

        Task AddReactionAsync(IEmoji emoji, RestRequestOptions options = null);

        Task RemoveOwnReactionAsync(IEmoji emoji, RestRequestOptions options = null);

        Task RemoveMemberReactionAsync(Snowflake memberId, IEmoji emoji, RestRequestOptions options = null);

        Task MarkAsReadAsync(RestRequestOptions options = null);
    }
}

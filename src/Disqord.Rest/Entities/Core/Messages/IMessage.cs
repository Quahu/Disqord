using System;
using System.Collections.Generic;

namespace Disqord
{
    public partial interface IMessage : ISnowflakeEntity, IDeletable
    {
        Snowflake ChannelId { get; }

        IUser Author { get; }

        string Content { get; }

        DateTimeOffset Timestamp { get; }

        IReadOnlyList<IUser> UserMentions { get; }

        // TODO: activity

        // TODO: application
    }
}

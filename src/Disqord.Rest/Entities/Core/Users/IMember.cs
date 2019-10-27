using System;
using System.Collections.Generic;

namespace Disqord
{
    public partial interface IMember : IUser
    {
        string Nick { get; }

        string DisplayName { get; }

        IReadOnlyCollection<Snowflake> RoleIds { get; }

        DateTimeOffset JoinedAt { get; }

        bool IsMuted { get; }

        bool IsDeafened { get; }

        Snowflake GuildId { get; }

        DateTimeOffset? BoostedAt { get; }

        bool IsBoosting { get; }
    }
}

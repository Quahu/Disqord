using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IGuildEmoji : ISnowflakeEntity, ICustomEmoji, IDeletable
    {
        IReadOnlyList<Snowflake> RoleIds { get; }

        Snowflake GuildId { get; }

        bool RequiresColons { get; }

        bool IsManaged { get; }

        bool IsAvailable { get; }
    }
}

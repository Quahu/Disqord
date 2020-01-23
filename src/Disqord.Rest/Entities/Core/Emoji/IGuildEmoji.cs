using System.Collections.Generic;

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

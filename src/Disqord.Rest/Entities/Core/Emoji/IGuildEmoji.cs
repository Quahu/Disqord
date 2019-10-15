using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord
{
    public interface IGuildEmoji : ISnowflakeEntity, ICustomEmoji, IDeletable
    {
        IReadOnlyList<Snowflake> RoleIds { get; }

        Snowflake GuildId { get; }

        bool RequiresColons { get; }

        bool IsManaged { get; }

        Task ModifyAsync(Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null);
    }
}

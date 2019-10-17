using System.Collections.Generic;

namespace Disqord
{
    public interface IGuildChannel : IChannel, IDeletable
    {
        Snowflake GuildId { get; }

        int Position { get; }

        IReadOnlyList<IOverwrite> Overwrites { get; }
    }
}

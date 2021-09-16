using System.Collections.Generic;

namespace Disqord
{
    public interface IApplicationCommand : ISnowflakeEntity, IPossibleGuildEntity, INamable
    {
        ApplicationCommandType Type { get; }

        Snowflake ApplicationId { get; }

        string Description { get; }

        IReadOnlyList<IApplicationCommandOption> Options { get; }

        bool IsEnabledByDefault { get; }

        Snowflake Version { get; }
    }
}

using Disqord.Models;

namespace Disqord
{
    public interface IApplicationCommand : ISnowflakeEntity, IPossibleGuildEntity, INamable, IJsonUpdatable<ApplicationCommandJsonModel>
    {
        ApplicationCommandType Type { get; }

        Snowflake ApplicationId { get; }

        bool IsEnabledByDefault { get; }

        Snowflake Version { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disqord
{
    public interface IApplicationCommand : IPossibleGuildEntity, ISnowflakeEntity, INamable
    {
        ApplicationCommandType? Type { get; }

        Snowflake ApplicationId { get; }

        string Description { get; }

        IReadOnlyList<IApplicationCommandOption> Options { get; }

        bool? IsEnabledByDefault { get; } 
    }
}

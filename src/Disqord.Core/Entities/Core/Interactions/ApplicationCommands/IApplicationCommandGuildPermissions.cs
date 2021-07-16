using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public interface IApplicationCommandGuildPermissions : ISnowflakeEntity, IGuildEntity, IJsonUpdatable<ApplicationCommandGuildPermissionsJsonModel>
    {
        Snowflake ApplicationId { get; }

        IReadOnlyList<IApplicationCommandPermissions> Permissions { get; }
    }
}
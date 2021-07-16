using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public interface IApplicationCommand : ISnowflakeEntity, INamable, IJsonUpdatable<ApplicationJsonModel>
    {
        Snowflake ApplicationId { get; }

        string Description { get; }

        IReadOnlyList<IApplicationCommandOption> Options { get; }

        bool HasDefaultPermission { get; }
    }
}
using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public interface IApplicationCommandInteractionResolvedData : IJsonUpdatable<ApplicationCommandInteractionDataResolvedJsonModel>
    {
        IReadOnlyDictionary<Snowflake, IUser> Users { get; }

        IReadOnlyDictionary<Snowflake, IMember> Members { get; }

        IReadOnlyDictionary<Snowflake, IRole> Roles { get; }

        IReadOnlyDictionary<Snowflake, IPartialChannel> Channels { get; }
    }
}
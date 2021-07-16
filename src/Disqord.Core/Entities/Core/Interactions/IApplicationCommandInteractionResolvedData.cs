using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public interface IApplicationCommandInteractionResolvedData : IJsonUpdatable<ApplicationCommandInteractionDataResolvedJsonModel>
    {
        /// <summary>
        ///     Gets the resolved user of this interaction
        /// </summary>
        IReadOnlyDictionary<Snowflake, IUser> Users { get; }

        /// <summary>
        ///     Gets the resolved members of this interaction
        /// </summary>
        IReadOnlyDictionary<Snowflake, IMember> Members { get; }

        /// <summary>
        ///     Gets the resolved roles of this interaction
        /// </summary>
        IReadOnlyDictionary<Snowflake, IRole> Roles { get; }

        /// <summary>
        ///     Gets the resolved channels of this interaction
        /// </summary>
        IReadOnlyDictionary<Snowflake, IPartialChannel> Channels { get; }
    }
}
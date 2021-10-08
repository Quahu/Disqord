using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents the role tags of a role.
    ///     <para>
    ///         <see href="https://discord.com/developers/docs/topics/permissions#role-object-role-tags-structure"/>
    ///     </para>
    /// </summary>
    public interface IRoleTags
    {
        /// <summary>
        ///     Represents empty role tags.
        ///     Returned for all non-tagged roles.
        /// </summary>
        public static readonly IRoleTags Empty = new TransientRoleTags(new RoleTagsJsonModel());

        /// <summary>
        ///     Gets the ID of the bot the role is for.
        ///     Returns <see langword="null"/> it is not a bot role.
        /// </summary>
        Snowflake? BotId { get; }

        /// <summary>
        ///     Gets the ID of the integration the role is for.
        ///     Returns <see langword="null"/> it is not an integration role.
        /// </summary>
        Snowflake? IntegrationId { get; }

        /// <summary>
        ///     Gets whether the role is the role Nitro boosters receive when boosting the guild.
        /// </summary>
        bool IsNitroBooster { get; }
    }
}

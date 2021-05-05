using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents the role tags of a role.
    ///     <para>
    ///         <see href="https://discord.com/developers/docs/topics/permissions#role-object-role-tags-structure"/>
    ///     </para>
    /// </summary>
    public class RoleTags
    {
        /// <summary>
        ///     Represents empty role tags.
        ///     Used for all non-tagged roles.
        /// </summary>
        public static readonly RoleTags Empty = new();

        /// <summary>
        ///     Gets the ID of the bot the role is for.
        ///     Returns <see langword="null"/> it is not a bot role.
        /// </summary>
        public Snowflake? BotId { get; }

        /// <summary>
        ///     Gets the ID of the integration the role is for.
        ///     Returns <see langword="null"/> it is not an integration role.
        /// </summary>
        public Snowflake? IntegrationId { get; }

        /// <summary>
        ///     Gets whether the role is the role Nitro boosters receive when boosting the guild.
        /// </summary>
        public bool IsNitroBooster { get; }

        private RoleTags()
        { }

        public RoleTags(RoleTagsJsonModel model)
        {
            BotId = model.BotId.GetValueOrNullable();
            IntegrationId = model.IntegrationId.GetValueOrNullable();
            IsNitroBooster = model.PremiumSubscriber.HasValue;
        }
    }
}

using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents the role tags of a role.<br/>
///     See <a href="https://discord.com/developers/docs/topics/permissions#role-object-role-tags-structure">Discord documentation</a>.
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
    /// </summary>
    /// <returns>
    ///     The ID of the bot or <see langword="null"/> the role is not a bot role.
    /// </returns>
    Snowflake? BotId { get; }

    /// <summary>
    ///     Gets the ID of the integration the role is for.
    /// </summary>
    /// <returns>
    ///     The ID of the integration or <see langword="null"/> it the role is not an integration role.
    /// </returns>
    Snowflake? IntegrationId { get; }

    /// <summary>
    ///     Gets whether the role is the role Nitro boosters receive when boosting the guild.
    /// </summary>
    bool IsNitroBooster { get; }
}

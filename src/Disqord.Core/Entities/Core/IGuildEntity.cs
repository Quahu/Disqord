namespace Disqord;

/// <summary>
///     Represents a Discord entity that exists within a guild.
/// </summary>
public interface IGuildEntity : IPossiblyGuildEntity
{
    /// <summary>
    ///     Gets the ID of the guild this entity is tied to.
    /// </summary>
    new Snowflake GuildId { get; }

    Snowflake? IPossiblyGuildEntity.GuildId => GuildId;
}

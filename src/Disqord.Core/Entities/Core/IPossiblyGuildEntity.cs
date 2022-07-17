namespace Disqord;

/// <summary>
///     Represents a Discord entity that might exist within a guild.
/// </summary>
public interface IPossiblyGuildEntity : IEntity
{
    /// <summary>
    ///     Gets the ID of the guild of this entity.
    /// </summary>
    /// <returns>
    ///     Returns the ID of the guild or <see langword="null"/>
    ///     if this entity has no guild attached to it.
    /// </returns>
    Snowflake? GuildId { get; }
}

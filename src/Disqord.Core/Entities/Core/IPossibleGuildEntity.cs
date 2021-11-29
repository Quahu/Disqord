namespace Disqord
{
    /// <summary>
    ///     Represents a Discord entity that might exist within a guild.
    /// </summary>
    public interface IPossibleGuildEntity
    {
        /// <summary>
        ///     Gets the ID of the guild of this entity.
        ///     Returns <see langword="null"/> if this entity has no guild attached to it.
        /// </summary>
        Snowflake? GuildId { get; }
    }
}

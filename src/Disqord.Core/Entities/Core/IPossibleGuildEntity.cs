namespace Disqord
{
    /// <summary>
    ///     Represents an entity that might have a guild ID attached to it.
    /// </summary>
    public interface IPossibleGuildEntity : IEntity
    {
        /// <summary>
        ///     Gets the optional ID of the guild of this entity.
        ///     Returns <see langword="null"/> if this entity has guild attached to it.
        /// </summary>
        Snowflake? GuildId { get; }
    }
}

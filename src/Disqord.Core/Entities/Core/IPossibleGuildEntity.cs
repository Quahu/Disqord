namespace Disqord
{
    /// <summary>
    ///     Represents an entity that can exist within a guild.
    /// </summary>
    public interface IPossibleGuildEntity : IEntity
    {
        /// <summary>
        ///     Gets the optional guild ID this entity is tied to.
        ///     Returns <see langword="null"/> if this entity came from outside of a guild.
        /// </summary>
        Snowflake? GuildId { get; }
    }
}

namespace Disqord
{
    /// <summary>
    ///     Represents a Discord entity that exists within a guild.
    /// </summary>
    public interface IGuildEntity : IEntity
    {
        /// <summary>
        ///     Gets the ID of the guild this entity is tied to.
        /// </summary>
        Snowflake GuildId { get; }
    }
}

namespace Disqord
{
    /// <summary>
    ///     Represents an entity existing within a guild.
    /// </summary>
    public interface IGuildEntity : IEntity
    {
        /// <summary>
        ///     Gets the guild ID this entity is tied to.
        /// </summary>
        Snowflake GuildId { get; }
    }
}

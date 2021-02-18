namespace Disqord
{
    /// <summary>
    ///     Represents an entity existing within guild.
    /// </summary>
    public interface IGuildEntity : IEntity
    {
        /// <summary>
        ///     Gets the guild ID this entity is tied to.
        /// </summary>
        Snowflake GuildId { get; }
    }
}

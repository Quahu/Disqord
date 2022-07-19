namespace Disqord;

/// <summary>
///     Represents a partial channel provided in interactions.
/// </summary>
public interface IInteractionChannel : IChannel
{
    /// <summary>
    ///     Gets the permissions of the interaction's author in this channel.
    /// </summary>
    Permissions AuthorPermissions { get; }

    /// <summary>
    ///     Gets the ID of the parent channel of this channel.
    /// </summary>
    /// <remarks>
    ///     This is only valid for thread channels.
    /// </remarks>
    Snowflake? ParentId { get; }

    /// <summary>
    ///     Gets the thread metadata of this channel.
    /// </summary>
    /// <remarks>
    ///     This is only valid for thread channels.
    /// </remarks>
    IThreadMetadata? ThreadMetadata { get; }
}

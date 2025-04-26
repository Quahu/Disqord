using System;
using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents a guild media channel.
/// </summary>
public interface IMediaChannel : IThreadParentChannel, ITopicChannel, ISlowmodeChannel
{
    /// <summary>
    ///     Gets the ID of the last thread created in this channel.
    /// </summary>
    Snowflake? LastThreadId { get; }

    /// <summary>
    ///     Gets the available tags that can be applied to threads in this channel.
    /// </summary>
    IReadOnlyList<IForumTag> Tags { get; }

    /// <summary>
    ///     Gets the emoji that can be reacted with by default to threads in this channel.
    /// </summary>
    IEmoji? DefaultReactionEmoji { get; }

    /// <summary>
    ///     Gets the default slowmode applied to threads upon their creation in this channel.
    /// </summary>
    TimeSpan DefaultThreadSlowmode { get; }

    /// <summary>
    ///     Gets the default sort order of posts in this channel.
    /// </summary>
    ForumSortOrder? DefaultSortOrder { get; }
}

using System;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a member of a thread channel.
/// </summary>
public interface IThreadMember : IIdentifiableEntity, IJsonUpdatable<ThreadMemberJsonModel>
{
    /// <summary>
    ///     Gets the ID of the thread of this member.
    /// </summary>
    Snowflake ThreadId { get; }

    /// <summary>
    ///     Gets when this member joined the thread.
    /// </summary>
    DateTimeOffset JoinedAt { get; }

    /// <summary>
    ///     Gets the notifications flags of this member.
    /// </summary>
    int Flags { get; }
}
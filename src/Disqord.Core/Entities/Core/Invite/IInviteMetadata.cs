using System;

namespace Disqord;

/// <summary>
///     Represents additional information about an <see cref="IInvite"/>.
/// </summary>
public interface IInviteMetadata : IEntity
{
    /// <summary>
    ///     Gets the current uses of this invite.
    /// </summary>
    int Uses { get; }

    /// <summary>
    ///     Gets the maximum uses of this invite.
    /// </summary>
    int MaxUses { get; }

    /// <summary>
    ///     Gets the maximum age of this invite.
    /// </summary>
    TimeSpan MaxAge { get; }

    /// <summary>
    ///     Gets whether this invite is temporary membership only.
    /// </summary>
    bool IsTemporaryMembership { get; }

    /// <summary>
    ///     Gets when this invite was created.
    /// </summary>
    DateTimeOffset CreatedAt { get; }
}
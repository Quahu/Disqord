using System;

namespace Disqord.Rest;

/// <summary>
///     Represents a pinned non-system message.
/// </summary>
/// <remarks>
///     Same as <see cref="IUserMessage"/> with the addition of <see cref="PinnedAt"/>.
/// </remarks>
public interface IRestPinnedUserMessage : IUserMessage
{
    /// <summary>
    ///     Gets the pin date of this message.
    /// </summary>
    DateTimeOffset PinnedAt { get; }
}

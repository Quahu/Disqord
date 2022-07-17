using System;

namespace Disqord.Rest;

/// <summary>
///     Represents an exception thrown when an interaction has expired.
/// </summary>
public class InteractionExpiredException : InvalidOperationException
{
    /// <summary>
    ///     Gets whether the interaction is response expired,
    ///     i.e. was not responded to in time.
    /// </summary>
    public bool IsResponseExpired { get; }

    /// <inheritdoc />
    public InteractionExpiredException(bool isResponseExpired)
        : base(isResponseExpired
            ? "This interaction has not been responded to in time and is now expired. "
            + "To allow for followups at a later time the interaction should be responded to with a deferral."
            : "This interaction has expired."
        )
    {
        IsResponseExpired = isResponseExpired;
    }
}
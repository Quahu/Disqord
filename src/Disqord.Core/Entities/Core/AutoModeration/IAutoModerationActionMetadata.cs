using System;
using Disqord.Models;

namespace Disqord;

// TODO: Get rid of IPossibleChannelEntity in favour of IEntity & add the property manually for better docs?
/// <summary>
///     Represents an auto-moderation action's metadata.
/// </summary>
public interface IAutoModerationActionMetadata : IPossiblyChannelEntity, IJsonUpdatable<AutoModerationActionMetadataJsonModel>
{
    /// <summary>
    ///     Gets the duration for which the user who triggered the rule should be timed-out.
    /// </summary>
    /// <remarks>
    ///     Used by the <see cref="AutoModerationActionType.Timeout"/> action type.
    /// </remarks>
    /// <returns>
    ///     The timeout duration or <see langword="null"/> if not set.
    /// </returns>
    TimeSpan? TimeoutDuration { get; }

    /// <summary>
    ///     Gets the custom message that will be shown to members when a message is blocked.
    /// </summary>
    /// <remarks>
    ///     Used by the <see cref="AutoModerationActionType.BlockMessage"/> action type.
    /// </remarks>
    /// <returns>
    ///     The custom message or <see langword="null"/> if not set.
    /// </returns>
    string? CustomMessage { get; }
}

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
    TimeSpan? TimeoutDuration { get; }
}
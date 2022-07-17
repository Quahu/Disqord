using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents an auto-moderation action.
/// </summary>
public interface IAutoModerationAction : IEntity, IJsonUpdatable<AutoModerationActionJsonModel>
{
    /// <summary>
    ///     Gets the type of this action.
    /// </summary>
    AutoModerationActionType Type { get; }

    /// <summary>
    ///     Gets the metadata of this action.
    /// </summary>
    /// <returns>
    ///     The metadata of this action or <see langword="null"/> if this action does not have any metadata.
    /// </returns>
    IAutoModerationActionMetadata? Metadata { get; }
}

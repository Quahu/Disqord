using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents an auto-moderation action.
    /// </summary>
    public interface IAutoModerationAction : IEntity, IJsonUpdatable<AutoModerationActionJsonModel>
    {
        /// <summary>
        ///     Gets the type of this action.
        /// </summary>
        AutoModerationActionType Type { get; }

        // TODO: Might return null, would need to be documented
        /// <summary>
        ///     Gets the metadata of this action.
        /// </summary>
        IAutoModerationActionMetadata Metadata { get; }
    }
}

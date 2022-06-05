using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents an auto-moderation trigger's metadata.
    /// </summary>
    public interface IAutoModerationTriggerMetadata : IEntity, IJsonUpdatable<AutoModerationTriggerMetadataJsonModel>
    {
        /// <summary>
        ///     Gets the list of keywords to match in message content.
        /// </summary>
        /// <remarks>
        ///     Used by the <see cref="AutoModerationRuleTriggerType.Keyword"/> trigger type.
        /// </remarks>
        IReadOnlyList<string> Keywords { get; }

        // TODO: Change to AutoModerationWordsetType when migrated in the api
        /// <summary>
        ///     Gets which pre-defined lists of keywords should matched in message content.
        /// </summary>
        /// <remarks>
        ///     Used by the <see cref="AutoModerationRuleTriggerType.KeywordPreset"/> trigger type.
        /// </remarks>
        IReadOnlyList<string> Presets { get; }
    }
}

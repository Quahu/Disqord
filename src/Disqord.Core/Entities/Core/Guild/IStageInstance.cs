using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a stage instance which holds information about a live stage.
    /// </summary>
    public interface IStageInstance : ISnowflakeEntity, IGuildEntity, IChannelEntity, IJsonUpdatable<StageInstanceJsonModel>
    {
        /// <summary>
        ///     Gets the topic of this stage instance.
        /// </summary>
        string Topic { get; }

        /// <summary>
        ///     Gets the privacy level of this stage instance.
        /// </summary>
        StagePrivacyLevel PrivacyLevel { get; }

        /// <summary>
        ///     Gets whether discovery is disabled for this stage instance.
        /// </summary>
        bool IsDiscoveryDisabled { get; }
    }
}

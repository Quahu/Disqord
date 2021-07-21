﻿using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a stage instance which holds information about a live stage.
    /// </summary>
    public interface IStage : ISnowflakeEntity, IGuildEntity, IChannelEntity, IJsonUpdatable<StageInstanceJsonModel>
    {
        /// <summary>
        ///     Gets the topic of this stage.
        /// </summary>
        string Topic { get; }

        /// <summary>
        ///     Gets the privacy level of this stage.
        /// </summary>
        StagePrivacyLevel PrivacyLevel { get; }

        /// <summary>
        ///     Gets whether discovery is disabled for this stage.
        /// </summary>
        bool IsDiscoveryDisabled { get; }
    }
}

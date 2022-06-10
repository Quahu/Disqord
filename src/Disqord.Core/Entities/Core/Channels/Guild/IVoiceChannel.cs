using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a guild voice channel.
    /// </summary>
    public interface IVoiceChannel : IVocalGuildChannel, IMessageGuildChannel
    {
        /// <summary>
        ///     Gets the member limit of this channel.
        /// </summary>
        int MemberLimit { get; }

        /// <summary>
        ///     Gets the camera video quality mode of this channel.
        /// </summary>
        VideoQualityMode VideoQualityMode { get; }

        /// <summary>
        ///     Gets whether this channel is age restricted.
        /// </summary>
        bool IsAgeRestricted { get; }
    }
}

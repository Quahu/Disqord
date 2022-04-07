namespace Disqord
{
    /// <summary>
    ///     Represents a guild voice channel.
    /// </summary>
    public interface IVoiceChannel : IVocalGuildChannel
    {
        /// <summary>
        ///     Gets the member limit of this channel.
        /// </summary>
        int MemberLimit { get; }

        /// <summary>
        ///     Gets the camera video quality mode of this channel.
        /// </summary>
        VideoQualityMode VideoQualityMode { get; }
    }
}

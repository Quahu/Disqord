namespace Disqord
{
    /// <summary>
    ///     Represents a voice guild channel.
    /// </summary>
    public interface IVoiceChannel : INestableChannel
    {
        /// <summary>
        ///     Gets the bitrate of this channel.
        /// </summary>
        int Bitrate { get; }
        
        /// <summary>
        ///     Gets the member limit of this channel.
        /// </summary>
        int MemberLimit { get; }
        
        /// <summary>
        ///     Gets the RTC region of this channel.
        ///     Returns <see langword="null"/> for automatic regions.
        /// </summary>
        string Region { get; }
    }
}

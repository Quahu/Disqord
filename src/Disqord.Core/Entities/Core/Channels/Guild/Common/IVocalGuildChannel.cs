namespace Disqord
{
    public interface IVocalGuildChannel : ICategorizableGuildChannel
    {
        /// <summary>
        ///     Gets the bitrate of this channel.
        /// </summary>
        int Bitrate { get; }

        /// <summary> 
        ///     Gets the RTC region of this channel. 
        ///     Returns <see langword="null"/> for automatic regions. 
        /// </summary> 
        string Region { get; }
    }
}

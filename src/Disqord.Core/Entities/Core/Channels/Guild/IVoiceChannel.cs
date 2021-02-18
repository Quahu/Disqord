namespace Disqord
{
    /// <summary>
    ///     Represents a voice guild channel.
    /// </summary>
    public interface IVoiceChannel : INestableChannel
    {
        /// <summary>
        ///     Gets the member limit of this channel.
        /// </summary>
        int MemberLimit { get; }

        /// <summary>
        ///     Gets the bitrate of this channel.
        /// </summary>
        int Bitrate { get; }
    }
}

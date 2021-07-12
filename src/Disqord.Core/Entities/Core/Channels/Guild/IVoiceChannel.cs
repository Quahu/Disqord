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
    }
}

namespace Disqord;

public interface IVocalGuildChannel : ICategorizableGuildChannel
{
    /// <summary>
    ///     Gets the bitrate of this channel.
    /// </summary>
    int Bitrate { get; }

    /// <summary>
    ///     Gets the RTC region of this channel.
    /// </summary>
    /// <returns>
    ///     The region or <see langword="null"/> for automatic regions.
    /// </returns>
    string? Region { get; }
}

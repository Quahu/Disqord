namespace Disqord;

public interface IGuildVoiceRegion : IVoiceRegion, IGuildEntity
{
    /// <summary>
    ///     Gets whether this voice region is VIP-only.
    /// </summary>
    bool IsVip { get; }

    /// <summary>
    ///     Gets whether this voice region is custom, used for events/etc.
    /// </summary>
    bool IsCustom { get; }
}
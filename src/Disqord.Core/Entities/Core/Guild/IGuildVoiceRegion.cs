namespace Disqord
{
    public interface IGuildVoiceRegion : IVoiceRegion, IGuildEntity
    {
        /// <summary>
        /// Gets whether this voice region is vip-only.
        /// </summary>
        bool Vip { get; }
        
        /// <summary>
        /// Gets whether this voice region is custom, used for events/etc.
        /// </summary>
        bool Custom { get; }
    }
}

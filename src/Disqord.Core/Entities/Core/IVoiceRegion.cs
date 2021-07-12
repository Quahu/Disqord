namespace Disqord
{
    public interface IVoiceRegion
    {
        /// <summary>
        /// Gets the unique id for this voice region.
        /// </summary>
        string Id { get; }
        
        /// <summary>
        /// Gets the name of this voice region.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Gets whether this is the closest voice region to the client.
        /// </summary>
        bool Optimal { get; }
        
        /// <summary>
        /// Gets whether this voice region is deprecated.
        /// </summary>
        bool Deprecated { get; }
    }
}

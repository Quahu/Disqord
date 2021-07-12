namespace Disqord
{
    public interface IVoiceRegion : IEntity, INamable
    {
        /// <summary>
        ///     Gets the unique ID for this voice region.
        /// </summary>
        string Id { get; }

        /// <summary>
        ///     Gets whether this is the closest voice region to the client.
        /// </summary>
        bool IsOptimal { get; }
        
        /// <summary>
        ///     Gets whether this voice region is deprecated.
        /// </summary>
        bool IsDepreciated { get; }
    }
}

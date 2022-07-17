using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a Discord voice region.
/// </summary>
public interface IVoiceRegion : IClientEntity, INamableEntity, IJsonUpdatable<VoiceRegionJsonModel>
{
    /// <summary>
    ///     Gets the ID of this voice region.
    /// </summary>
    string Id { get; }

    /// <summary>
    ///     Gets whether this voice region is the closest to the client.
    /// </summary>
    bool IsOptimal { get; }

    /// <summary>
    ///     Gets whether this voice region is deprecated.
    /// </summary>
    bool IsDeprecated { get; }
}
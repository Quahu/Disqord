using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a guild ban.
/// </summary>
public interface IBan : IClientEntity, IGuildEntity, IJsonUpdatable<BanJsonModel>
{
    /// <summary>
    ///     Gets the user of this ban.
    /// </summary>
    IUser User { get; }

    /// <summary>
    ///     Gets the reason of this ban.
    /// </summary>
    string? Reason { get; }
}

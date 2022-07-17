namespace Disqord.Gateway;

/// <summary>
///     Represents a party of a rich activity.
/// </summary>
public interface IRichActivityParty : IGatewayEntity
{
    /// <summary>
    ///     Gets the ID of this party.
    /// </summary>
    string? Id { get; }

    /// <summary>
    ///     Gets the size of this party.
    /// </summary>
    int? Size { get; }

    /// <summary>
    ///     Gets the maximum size of this party.
    /// </summary>
    int? MaximumSize { get; }
}

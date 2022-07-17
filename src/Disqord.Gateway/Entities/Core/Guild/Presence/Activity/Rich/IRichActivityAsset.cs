namespace Disqord.Gateway;

/// <summary>
///     Represents an asset of a rich activity.
/// </summary>
public interface IRichActivityAsset : IGatewayEntity
{
    /// <summary>
    ///     Gets the ID of the application of this asset's activity.
    /// </summary>
    Snowflake? ApplicationId { get; }

    /// <summary>
    ///     Gets the ID of this asset.
    /// </summary>
    string? Id { get; }

    /// <summary>
    ///     Gets the text of this asset.
    /// </summary>
    string? Text { get; }
}

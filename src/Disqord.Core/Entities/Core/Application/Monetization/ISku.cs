using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a Discord SKU (stock-keeping unit) for premium offerings.
/// </summary>
public interface ISku : ISnowflakeEntity, INamableEntity, IJsonUpdatable<SkuJsonModel>
{
    /// <summary>
    ///     Gets the type of this SKU.
    /// </summary>
    SkuType Type { get; }

    /// <summary>
    ///     Gets the ID of the application this SKU belongs to.
    /// </summary>
    Snowflake ApplicationId { get; }

    /// <summary>
    ///     Gets the system-generated URL slug of this SKU.
    /// </summary>
    string Slug { get; }

    /// <summary>
    ///     Gets the flags of this SKU.
    /// </summary>
    SkuFlags Flags { get; }
}

using System;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents an entitlement of a user or a guild to a premium offering.
/// </summary>
public interface IEntitlement: ISnowflakeEntity, IPossiblyGuildEntity, IJsonUpdatable<EntitlementJsonModel>
{
    /// <summary>
    ///     Gets the ID of the SKU this entitlement offers.
    /// </summary>
    Snowflake SkuId { get; }

    /// <summary>
    ///     Gets the ID of the application this entitlement belongs to.
    /// </summary>
    Snowflake ApplicationId { get; }

    /// <summary>
    ///     Gets the ID of the user this entitlement is for.
    /// </summary>
    Snowflake? UserId { get; }

    /// <summary>
    ///     Gets the type of this entitlement.
    /// </summary>
    EntitlementType Type { get; }

    /// <summary>
    ///     Gets whether this entitlement is deleted.
    /// </summary>
    bool Deleted { get; }

    /// <summary>
    ///     Gets the date from which the entitlement is valid.
    /// </summary>
    DateTimeOffset? StartsAt { get; }

    /// <summary>
    ///     Gets the date from which the entitlement is no longer valid.
    /// </summary>
    DateTimeOffset? EndsAt { get; }

    /// <summary>
    ///     Gets whether this entitlement has been consumed.
    /// </summary>
    bool HasBeenConsumed { get; }
}

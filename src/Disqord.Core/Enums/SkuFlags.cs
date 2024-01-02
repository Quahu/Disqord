using System;

namespace Disqord;

/// <summary>
///     Represents flags for SKUs.
/// </summary>
[Flags]
public enum SkuFlags
{
    /// <summary>
    ///     The SKU is available for purchase.
    /// </summary>
    Available = 1 << 2,

    /// <summary>
    ///     A recurring SKU that can be purchased for a single guild.
    ///     Grants access to every user in the guild.
    /// </summary>
    GuildSubscription = 1 << 7,

    /// <summary>
    ///     A recurring SKU that can be purchased for a user.
    ///     Grants access to the purchasing user in every guild.
    /// </summary>
    UserSubscription = 1 << 8
}

namespace Disqord;

/// <summary>
///     Represents the type of a entitlement.
/// </summary>
public enum EntitlementType
{
    /// <summary>
    ///     The entitlement was purchased by the user.
    /// </summary>
    Purchase = 1,

    /// <summary>
    ///     The entitlement was a part of a Discord Nitro subscription.
    /// </summary>
    PremiumSubscription = 2,

    /// <summary>
    ///     The entitlement was gifted by a developer.
    /// </summary>
    DeveloperGift = 3,

    /// <summary>
    ///     The entitlement was purchased by a developer in application test mode.
    /// </summary>
    TestModePurchase = 4,

    /// <summary>
    ///     The entitlement was granted when the SKU was free.
    /// </summary>
    FreePurchase = 5,

    /// <summary>
    ///     The entitlement was gifted by another user.
    /// </summary>
    UserGift = 6,

    /// <summary>
    ///     The entitlement was claimed by the user for free as a Nitro Subscriber.
    /// </summary>
    PremiumPurchase = 7,

    /// <summary>
    ///     The entitlement was purchased as an app subscription.
    /// </summary>
    ApplicationSubscription = 8
}

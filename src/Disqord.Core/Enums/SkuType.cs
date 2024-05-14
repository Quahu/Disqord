namespace Disqord;

/// <summary>
///     Represents the type of a SKU.
/// </summary>
public enum SkuType
{
    /// <summary>
    ///     Represents a durable one-time purchase.
    /// </summary>
    Durable = 2,
    
    /// <summary>
    ///     Represents a consumable one-time purchase.
    /// </summary>
    Consumable = 3,
    
    /// <summary>
    ///     Represents a recurring subscription.
    /// </summary>
    Subscription = 5,

    /// <summary>
    ///     Represents a system-generated group for subscriptions.
    /// </summary>
    SubscriptionGroup = 6
}

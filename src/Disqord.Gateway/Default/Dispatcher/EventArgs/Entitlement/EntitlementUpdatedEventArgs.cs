using System;

namespace Disqord.Gateway;

public class EntitlementUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the updated entitlement.
    /// </summary>
    public Snowflake EntitlementId => Entitlement.Id;

    /// <summary>
    ///     Gets the updated entitlement.
    /// </summary>
    public IEntitlement Entitlement { get; }

    public EntitlementUpdatedEventArgs(
        IEntitlement entitlement)
    {
        Entitlement = entitlement;
    }
}

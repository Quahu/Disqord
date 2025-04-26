using System;

namespace Disqord.Gateway;

public class EntitlementDeletedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the deleted entitlement.
    /// </summary>
    public Snowflake EntitlementId => Entitlement.Id;

    /// <summary>
    ///     Gets the deleted entitlement.
    /// </summary>
    public IEntitlement Entitlement { get; }

    public EntitlementDeletedEventArgs(
        IEntitlement entitlement)
    {
        Entitlement = entitlement;
    }
}

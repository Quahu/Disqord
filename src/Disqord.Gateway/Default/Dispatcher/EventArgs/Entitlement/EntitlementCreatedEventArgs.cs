using System;

namespace Disqord.Gateway;

public class EntitlementCreatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the created entitlement.
    /// </summary>
    public Snowflake EntitlementId => Entitlement.Id;

    /// <summary>
    ///     Gets the created entitlement.
    /// </summary>
    public IEntitlement Entitlement { get; }

    public EntitlementCreatedEventArgs(
        IEntitlement entitlement)
    {
        Entitlement = entitlement;
    }
}

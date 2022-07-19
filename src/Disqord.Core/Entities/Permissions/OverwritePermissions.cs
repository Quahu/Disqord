using System;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents the allowed and denied permission set of a channel permission overwrite.
/// </summary>
public struct OverwritePermissions : IEquatable<OverwritePermissions>
{
    /// <summary>
    ///     Gets an <see cref="OverwritePermissions"/> with no permissions set.
    /// </summary>
    public static OverwritePermissions None => new(0, 0);

    /// <summary>
    ///     Gets or sets the allowed permissions of this set.
    /// </summary>
    public Permissions Allowed { get; set; }

    /// <summary>
    ///     Gets or sets the denied permissions of this set.
    /// </summary>
    public Permissions Denied { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="OverwritePermissions"/> with the specified allowed and denied permissions.
    /// </summary>
    /// <param name="allowed"> The allowed permissions. </param>
    /// <param name="denied"> The denied permissions. </param>
    public OverwritePermissions(Permissions allowed, Permissions denied)
    {
        Allowed = allowed;
        Denied = denied;
    }

    public OverwritePermissions Allow(Permissions permissions)
    {
        return new(Allowed | permissions, Denied & ~permissions);
    }

    public OverwritePermissions Deny(Permissions permissions)
    {
        return new(Allowed & ~permissions, Denied | permissions);
    }

    public OverwritePermissions Unset(Permissions permissions)
    {
        return new(Allowed & ~permissions, Denied & ~permissions);
    }

    /// <inheritdoc/>
    public bool Equals(OverwritePermissions other)
    {
        return Allowed == other.Allowed && Denied == other.Denied;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is OverwritePermissions overwritePermissions)
            return Equals(overwritePermissions);

        return false;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Allowed, Denied);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Allowed} | {Denied}";
    }

    public static implicit operator OverwritePermissions((Permissions Allowed, Permissions Denied) value)
    {
        return new(value.Allowed, value.Denied);
    }

    public static implicit operator (Permissions, Permissions)(OverwritePermissions value)
    {
        return (value.Allowed, value.Denied);
    }

    public static bool operator ==(OverwritePermissions left, OverwritePermissions right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(OverwritePermissions left, OverwritePermissions right)
    {
        return !left.Equals(right);
    }

    [Obsolete("The '+', '-', and '/' operators have been removed, use '|' and '& ~' (for '/' use it for both allowed and denied permissions) respectively.", true)]
    public static OverwritePermissions operator +(OverwritePermissions left, OverwritePermissions right)
        => Throw.InvalidOperationException<OverwritePermissions>();

    [Obsolete("The '+', '-', and '/' operators have been removed, use '|' and '& ~' (for '/' use it for both allowed and denied permissions) respectively.", true)]
    public static OverwritePermissions operator -(OverwritePermissions left, OverwritePermissions right)
        => Throw.InvalidOperationException<OverwritePermissions>();

    [Obsolete("The '+', '-', and '/' operators have been removed, use '|' and '& ~' (for '/' use it for both allowed and denied permissions) respectively.", true)]
    public static OverwritePermissions operator /(OverwritePermissions left, OverwritePermissions right)
        => Throw.InvalidOperationException<OverwritePermissions>();
}

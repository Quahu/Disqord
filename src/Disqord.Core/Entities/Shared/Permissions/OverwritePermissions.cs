using System;
using Qommon;

namespace Disqord
{
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
        public ChannelPermissions Allowed { get; set; }

        /// <summary>
        ///     Gets or sets the denied permissions of this set.
        /// </summary>
        public ChannelPermissions Denied { get; set; }

        /// <summary>
        ///     Instantiates a new <see cref="OverwritePermissions"/> with the specified allowed and denied permissions.
        /// </summary>
        /// <param name="allowed"> The allowed permissions. </param>
        /// <param name="denied"> The denied permissions. </param>
        public OverwritePermissions(ChannelPermissions allowed, ChannelPermissions denied)
        {
            Allowed = allowed;
            Denied = denied;
        }

        public OverwritePermissions Allow(Permission permission)
            => new(Allowed | permission, Denied & ~permission);

        public OverwritePermissions Deny(Permission permission)
            => new(Allowed & ~permission, Denied | permission);

        public OverwritePermissions Unset(Permission permission)
            => new(Allowed & ~permission, Denied & ~permission);

        public bool Equals(OverwritePermissions other)
            => Allowed.Equals(other.Allowed) && Denied.Equals(other.Denied);

        public override bool Equals(object obj)
        {
            if (obj is OverwritePermissions overwritePermissions)
                return Equals(overwritePermissions);

            return false;
        }

        public override int GetHashCode()
            => HashCode.Combine(Allowed, Denied);

        public override string ToString()
            => $"{Allowed} | {Denied}";

        public static implicit operator OverwritePermissions((Permission Allowed, Permission Denied) value)
            => new(value.Allowed, value.Denied);

        public static implicit operator (Permission, Permission)(OverwritePermissions value)
            => (value.Allowed, value.Denied);

        public static bool operator ==(OverwritePermissions left, OverwritePermissions right)
            => left.Equals(right);

        public static bool operator !=(OverwritePermissions left, OverwritePermissions right)
            => !left.Equals(right);

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
}

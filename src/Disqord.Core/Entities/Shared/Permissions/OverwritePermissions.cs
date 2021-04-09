namespace Disqord
{
    /// <summary>
    ///     Represents an overwritten permissions set of a channel overwrite.
    /// </summary>
    public readonly struct OverwritePermissions
    {
        /// <summary>
        ///     Gets an <see cref="OverwritePermissions"/> with no permissions set.
        /// </summary>
        public static OverwritePermissions None => new OverwritePermissions(0, 0);

        /// <summary>
        ///     Gets the allowed permissions of this set.
        /// </summary>
        public ChannelPermissions Allowed { get; }

        /// <summary>
        ///     Gets the denied permissions of this set.
        /// </summary>
        public ChannelPermissions Denied { get; }

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

        public static implicit operator OverwritePermissions((Permission Allowed, Permission Denied) value)
            => new OverwritePermissions(value.Allowed, value.Denied);

        public static implicit operator OverwritePermissions((ulong Allowed, ulong Denied) value)
            => new OverwritePermissions(value.Allowed, value.Denied);

        public static implicit operator (Permission, Permission)(OverwritePermissions value)
            => (value.Allowed, value.Denied);

        public static implicit operator (ulong, ulong)(OverwritePermissions value)
            => (value.Allowed, value.Denied);

        public OverwritePermissions Allow(Permission permission)
            => new OverwritePermissions(Allowed + permission, Denied - permission);

        public OverwritePermissions Deny(Permission permission)
            => new OverwritePermissions(Allowed - permission, Denied + permission);

        public OverwritePermissions Unset(Permission permission)
            => new OverwritePermissions(Allowed - permission, Denied - permission);

        public static OverwritePermissions operator +(OverwritePermissions left, Permission right)
            => left.Allow(right);

        public static OverwritePermissions operator -(OverwritePermissions left, Permission right)
            => left.Deny(right);

        public static OverwritePermissions operator /(OverwritePermissions left, Permission right)
            => left.Unset(right);

        public override string ToString()
            => $"{Allowed} | {Denied}";
    }
}

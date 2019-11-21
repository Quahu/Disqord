namespace Disqord
{
    public readonly struct OverwritePermissions
    {
        public static OverwritePermissions None => new OverwritePermissions(0, 0);

        public ChannelPermissions Allowed { get; }

        public ChannelPermissions Denied { get; }

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

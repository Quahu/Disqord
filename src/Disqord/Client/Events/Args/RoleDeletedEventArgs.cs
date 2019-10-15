namespace Disqord.Events
{
    public sealed class RoleDeletedEventArgs : DiscordEventArgs
    {
        public CachedRole Role { get; }

        internal RoleDeletedEventArgs(CachedRole role) : base(role.Client)
        {
            Role = role;
        }
    }
}
